using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace Data.DbClient.Extensions
{
    /// <summary> Simple access to database schemas in ADO.Net 2.0. </summary> <example> Form Load:
    /// DataTable dt = DbProviderFactories.GetFactoryClasses(); ProviderName.DataSource = dt;
    /// ProviderName.DisplayMember = "InvariantName";
    ///
    /// After picked a provider from above list: SchemaReader schema = new
    /// SchemaReader(ConnectionString.Text); schema.ProviderName = ProviderName.Text;
    /// dataGrid1.DataSource = schema.Tables(); //a list of all tables dataGrid1.DataSource =
    /// schema.Columns("MYTABLENAME"); //a list of columns for a specific table
    ///
    ///</example>
    public class SchemaReader
    {
        private string _connectionString;
        private string _providerName;
        private DataTable _metadata;
        private DataTable _restrictions;

        /// <summary>
        /// Constructor with connectionString <para>Uses provderName of System.Data.OracleClient,
        /// but you can reset with <see cref="ProviderName" /> property</para>
        /// </summary>
        /// <param name="connectionString">
        /// Eg "Data Source=localhost;Integrated Security=SSPI;Initial Catalog=Northwind;"
        /// </param>
        public SchemaReader(string connectionString)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException("connectionString must not be empty");

            _connectionString = connectionString;
            _providerName = "System.Data.OracleClient";
        }

        /// <summary>
        /// Constructor with connectionString and ProviderName
        /// </summary>
        /// <param name="connectionString">
        /// Eg "Data Source=localhost;Integrated Security=SSPI;Initial Catalog=Northwind;"
        /// </param>
        /// <param name="providerName">
        /// ProviderInvariantName for the provider (eg System.Data.SqlClient or System.Data.OracleClient)
        /// </param>
        public SchemaReader(string connectionString, string providerName)
        {
            if (String.IsNullOrEmpty(connectionString))
                throw new ArgumentException("connectionString must not be empty");

            if (String.IsNullOrEmpty(providerName))
                throw new ArgumentException("providerName must not be empty");

            _connectionString = connectionString;
            _providerName = providerName;
        }

        ///<summary>
        ///ConnectionString Eg "Data Source=localhost;Integrated Security=SSPI;Initial Catalog=Northwind;"
        /// or "Server=.\SQLExpress;AttachDbFilename=|DataDirectory|Northwind.mdf;Trusted_Connection=Yes;Database=Northwind;"
        ///</summary>
        public string ConnectionString
        {
            get { return _connectionString; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("connectionString must not be empty");

                _connectionString = value;
            }
        }

        ///<summary>
        ///ProviderName (the ProviderInvariantName- eg System.Data.SqlClient or System.Data.OracleClient).
        /// <para>See <see cref="Providers"/> for available list</para>
        ///</summary>
        public string ProviderName
        {
            get { return _providerName; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    throw new ArgumentException("providerName must not be empty");

                _providerName = value;
            }
        }

        /// <summary>
        /// There are a number of special-cases for Oracle, so we check the provider string
        /// </summary>
        internal bool IsOracle
        {
            get
            {
                return ProviderName.Equals("System.Data.OracleClient", StringComparison.OrdinalIgnoreCase) ||
                    ProviderName.Equals("Oracle.DataAccess.Client", StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Gets or sets the owner (for Oracle) /schema (for SqlServer). Always set it with Oracle;
        /// if you use other than dbo in SqlServer you should also set it. If it is null or empty,
        /// all owners are returned.
        /// </summary>
        public string Owner { get; set; }

        #region Generic Sql

        /// <summary>
        /// Run a Select against the database and return a DataTable (no parameters)
        /// </summary>
        /// <param name="sqlCommand">The SQL statement</param>
        /// <returns>A DataTable containing the rows returned</returns>
        public DataTable Select(string sqlCommand)
        {
            var dt = new DataTable();
            var factory =
                DbProviderFactories.GetFactory(_providerName);
            //can't pass in parameters as we don't have the factory until now.
            //open a connection
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                //create a dataadaptor and fill it
                using (var da = factory.CreateDataAdapter())
                {
                    if (da == null) return dt;
                    da.SelectCommand = conn.CreateCommand();
                    da.SelectCommand.CommandText = sqlCommand;

                    da.Fill(dt);
                    return dt;
                }
            }
        }

        /// <summary>
        /// Execute a Non Query (insert, update, delete) against the database
        /// </summary>
        /// <param name="sqlCommand">The SQL statement</param>
        /// <returns>Returns number of rows affected</returns>
        public int ExecuteNonQuery(string sqlCommand)
        {
            var factory =
                DbProviderFactories.GetFactory(_providerName);
            //open a connection
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return 0;
                conn.ConnectionString = _connectionString;
                //create a command
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sqlCommand;
                    cmd.CommandType = CommandType.Text;
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion Generic Sql

        /// <summary>
        /// List of all the valid Providers. Use the ProviderInvariantName to fill ProviderName property
        /// </summary>
        /// <returns></returns>
        public static DataTable Providers()
        {
            return DbProviderFactories.GetFactoryClasses();
        }

        /// <summary>
        /// DataTable of all users
        /// </summary>
        /// <returns>Datatable with columns NAME, ID, CREATEDDATE</returns>
        public DataTable Users()
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();
                _metadata = MetadataCollections(conn);
                return conn.GetSchema("Users");
            }
        }

        /// <summary>
        /// DataTable of all tables
        /// </summary>
        public DataTable Tables()
        {
            return Tables(null);
        }

        /// <summary>
        /// DataTable of all tables for a specific owner
        /// </summary>
        public DataTable Tables(string owner)
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();
                var restrictions = GetSchemaRestrictionsForOwner(conn, owner);
                return conn.GetSchema("Tables", restrictions);
            }
        }

        /// <summary>
        /// Get all data for a specified table name.
        /// </summary>
        /// <param name="tableName">Name of the table. Oracle names can be case sensitive.</param>
        /// <returns>
        /// A dataset containing the tables: Columns, Primary_Keys, Foreign_Keys, Unique_Keys (only
        /// filled for Oracle), Indexes, IndexColumns
        /// </returns>
        public DataSet Table(string tableName)
        {
            var ds = new DataSet();
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return ds;
                conn.ConnectionString = _connectionString;
                conn.Open();

                var restrictions = GetSchemaRestrictionsForTable(conn, tableName);
                ds.Tables.Add(conn.GetSchema("Columns", restrictions));

                var pks = FindKeys(tableName, GetPrimaryKeyType(), factory, conn);
                pks.TableName = "PRIMARY_KEYS";
                ds.Tables.Add(pks);

                var fks = FindKeys(tableName, GetForeignKeyType(), factory, conn);
                fks.TableName = "FOREIGN_KEYS";
                ds.Tables.Add(fks);

                var uks = FindKeys(tableName, GetUniqueKeyType(), factory, conn);
                uks.TableName = "UNIQUE_KEYS";
                ds.Tables.Add(uks);

                var indexRestrictions = GetSchemaRestrictions(conn, tableName, "Indexes", "Table", "Table_Name", "TableName");
                ds.Tables.Add(conn.GetSchema("Indexes", indexRestrictions));

                var indexColRestrictions = GetSchemaRestrictions(conn, tableName, "IndexColumns", "Table", "Table_Name", "TableName");
                ds.Tables.Add(conn.GetSchema("IndexColumns", indexColRestrictions));

                ds.Tables.Add(IdentityColumns(tableName, factory, conn));
            }
            return ds;
        }

        /// <summary>
        /// DataTable of all tables for a specific owner
        /// </summary>
        /// <returns>Datatable with columns OWNER, TABLE_NAME, TYPE</returns>
        public DataTable Views(string owner)
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();
                var restrictions = GetSchemaRestrictions(conn, owner, "Views", "Owner");
                return conn.GetSchema("Views", restrictions);
            }
        }

        /// <summary>
        /// All the columns for a specific table
        /// </summary>
        /// <param name="tableName">Name of the table. Oracle names can be case sensitive.</param>
        /// <returns>
        /// DataTable columns incl. COLUMN_NAME, DATATYPE, LENGTH, PRECISION, SCALE, NULLABLE
        /// </returns>
        public DataTable Columns(string tableName)
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();

                var restrictions = GetSchemaRestrictionsForTable(conn, tableName);
                return conn.GetSchema("Columns", restrictions);
            }
        }

        public DataTable IndexColumns(string tableName)
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();

                var indexColRestrictions = GetSchemaRestrictions(conn, tableName, "IndexColumns", "Table", "Table_Name", "TableName");
                return conn.GetSchema("IndexColumns", indexColRestrictions);
            }
        }

        public DataTable IdentityColumns(string tableName)
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();

                return IdentityColumns(tableName, factory, conn);
            }
        }

        private DataTable IdentityColumns(string tableName, DbProviderFactory factory, DbConnection conn)
        {
            var dt = new DataTable("IdentityColumns");
            if (IsOracle) return dt; //Oracle has sequences instead

            //create a dataadaptor and fill it
            using (var da = factory.CreateDataAdapter())
            {
                var byTableName = !string.IsNullOrEmpty(tableName);
                var byOwnerSchema = !string.IsNullOrEmpty(Owner);
                var sqlCommand = @"SELECT
SchemaOwner = s.name, TableName = o.name, ColumnName = c.name
FROM sys.identity_columns c
INNER JOIN sys.all_objects o ON c.object_id = o.object_id
INNER JOIN sys.schemas s ON s.schema_id = o.schema_id
WHERE " +
                        (byTableName ? "o.name = @tableName AND " : "") +
                        (byOwnerSchema ? "s.name = @schemaOwner AND " : "") +
"o.type= 'U' ORDER BY o.name, c.name";

                if (da == null) return dt;
                da.SelectCommand = conn.CreateCommand();
                da.SelectCommand.CommandText = sqlCommand;
                if (byTableName)
                {
                    da.SelectCommand.Parameters.Add(
                        AddDbParameter(factory, "tableName", tableName));
                }
                if (byOwnerSchema)
                {
                    da.SelectCommand.Parameters.Add(
                        AddDbParameter(factory, "schemaOwner", Owner));
                }
                da.Fill(dt);
                return dt;
            }
        }

        #region Constraints

        /// <summary>
        /// The PK columns for a specific table
        /// </summary>
        /// <param name="tableName">Name of the table. Oracle names can be case sensitive.</param>
        /// <returns>
        /// DataTable with constraint_name, table_name, column_name, ordinal_position
        /// </returns>
        public DataTable PrimaryKeys(string tableName)
        {
            return FindKeys(tableName, GetPrimaryKeyType());
        }

        /// <summary>
        /// The Foreign Key columns for a specific table
        /// </summary>
        /// <param name="tableName">Name of the table. Oracle names can be case sensitive.</param>
        public DataTable ForeignKeys(string tableName)
        {
            return FindKeys(tableName, GetForeignKeyType());
        }

        /// <summary>
        /// The Unique Key columns for a specific table. This is Oracle only and returns an empty
        /// datatable for SqlServer.
        /// </summary>
        public DataTable UniqueKeys(string tableName)
        {
            return FindKeys(tableName, GetUniqueKeyType());
        }

        #region Constraint private methods

        /// <summary>
        /// Finds the primary/foreign/unique keys constraints
        /// </summary>
        /// <param name="tableName">Name of the table. Oracle names can be case sensitive.</param>
        /// <param name="constraintType">Type of the constraint.</param>
        private DataTable FindKeys(string tableName, string constraintType)
        {
            var factory =
                DbProviderFactories.GetFactory(_providerName);
            //open a connection
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                return FindKeys(tableName, constraintType, factory, conn);
            }
        }

        private DataTable FindKeys(string tableName, string constraintType, DbProviderFactory factory, DbConnection conn)
        {
            if (constraintType == "U" && !IsOracle)
                return new DataTable("UNIQUE_KEYS"); //only Oracle has this concept

            //create a dataadaptor and fill it
            using (var da = factory.CreateDataAdapter())
            {
                var byTableName = !string.IsNullOrEmpty(tableName);
                var byOwnerSchema = !string.IsNullOrEmpty(Owner);
                var sqlCommand = GetKeySql(byTableName, byOwnerSchema);
                var dt = new DataTable(constraintType);

                if (da == null) return dt;
                da.SelectCommand = conn.CreateCommand();
                da.SelectCommand.CommandText = sqlCommand;
                if (byTableName)
                {
                    da.SelectCommand.Parameters.Add(
                        AddDbParameter(factory, "tableName", tableName));
                }
                if (byOwnerSchema)
                {
                    da.SelectCommand.Parameters.Add(
                        AddDbParameter(factory, "schemaOwner", Owner));
                }

                var type = factory.CreateParameter();
                if (type != null)
                {
                    type.ParameterName = "constraint_type";
                    type.Value = constraintType;
                    da.SelectCommand.Parameters.Add(type);
                }

                da.Fill(dt);
                return dt;
            }
        }

        private static DbParameter AddDbParameter(DbProviderFactory factory, string parameterName, object value)
        {
            var parameter = factory.CreateParameter();
            if (parameter == null) return null;
            parameter.ParameterName = parameterName;
            parameter.Value = value;
            return parameter;
        }

        private string GetPrimaryKeyType()
        {
            return IsOracle ? "P" : "PRIMARY KEY";
        }

        private string GetForeignKeyType()
        {
            return IsOracle ? "R" : "FOREIGN KEY";
        }

        private static string GetUniqueKeyType()
        {
            return "U";
        }

        /// <summary>
        /// Gets the key SQL. GetSchema doesn't work :(
        /// </summary>
        /// <returns></returns>
        private string GetKeySql(bool byTableName, bool bySchemaOwner)
        {
            string sqlCommand;
            if (IsOracle)//Oracle doesn't have INFORMATION_SCHEMA
            {
                sqlCommand = @"SELECT cols.constraint_name, cols.table_name, cols.column_name, cols.position AS ordinal_position, cons.r_constraint_name AS unique_constraint_name, cons2.table_name AS fk_table
FROM all_constraints cons
INNER JOIN all_cons_columns cols
  ON cons.constraint_name = cols.constraint_name
  AND cons.owner = cols.owner
LEFT OUTER JOIN all_constraints cons2
  ON cons.r_constraint_name = cons2.constraint_name
WHERE " +
                        (byTableName ? "cols.table_name = :tableName AND " : "") +
                        (bySchemaOwner ? "cols.owner = :schemaOwner AND " : "") +
                        @"cons.constraint_type = :constraint_type
ORDER BY cols.table_name, cols.position";
            }
            else //use SQL92 INFORMATION_SCHEMA
            {
                sqlCommand = @"SELECT cons.constraint_name, cons.table_name,
column_name, ordinal_position, refs.unique_constraint_name, cons2.table_name AS fk_table
FROM information_schema.table_constraints AS cons
INNER JOIN information_schema.key_column_usage AS keys
ON cons.constraint_catalog = keys.constraint_catalog AND
cons.constraint_schema = keys.constraint_schema AND
cons.constraint_name = keys.constraint_name
LEFT OUTER JOIN information_schema.referential_constraints AS refs
ON cons.constraint_catalog = refs.constraint_catalog AND
cons.constraint_schema = refs.constraint_schema AND
cons.constraint_name = refs.constraint_name
LEFT OUTER JOIN information_schema.table_constraints AS cons2
ON cons2.constraint_catalog = refs.constraint_catalog AND
cons2.constraint_schema = refs.constraint_schema AND
cons2.constraint_name = refs.unique_constraint_name
WHERE " +
                        (byTableName ? "cons.table_name = @tableName AND " : "") +
                        (bySchemaOwner ? "cons.constraint_schema = @schemaOwner AND " : "") +
                        @"cons.constraint_type = @constraint_type
                        ";
            }
            return sqlCommand;
        }

        #endregion Constraint private methods

        #endregion Constraints

        #region Sprocs

        /// <summary>
        /// Get all the stored procedures (owner required for Oracle- otherwise null).
        /// NB: in oracle does not get stored procedures in packages
        /// </summary>
        public DataTable StoredProcedures(string owner)
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();
                var restrictions = GetSchemaRestrictions(conn, owner, "Procedures", "Owner");
                return conn.GetSchema("Procedures", restrictions);
            }
        }

        /// <summary>
        /// Get all the arguments for a stored procedures (or all sprocs)
        /// NB: in oracle we get arguments for sprocs in packages. This is slow.
        /// </summary>
        public DataTable StoredProcedureArguments(string storedProcedureName)
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();
                //different collections here- we could just if(IsOracle)
                var argKey = "ProcedureParameters";
                if (IsOracle)
                    argKey = "Arguments"; //Oracle, we assume you mean pacakages

                var restrictions = GetSchemaRestrictions(conn, storedProcedureName, argKey, "SPECIFIC_NAME", "OBJECT_NAME");
                return conn.GetSchema(argKey, restrictions);
            }
        }

        /// <summary>
        /// Get all the packages (Oracle only concept- returns empty DataTable for others)
        /// </summary>
        public DataTable Packages(string owner)
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();
                if (_metadata == null)
                    _metadata = MetadataCollections(conn);
                if (_metadata.Select("[CollectionName] = 'Packages'").Length == 0)
                    return new DataTable(); //packages not supported
                var restrictions = GetSchemaRestrictions(conn, owner, "Packages", "Owner");
                return conn.GetSchema("Packages", restrictions);
            }
        }

        #endregion Sprocs

        #region Restrictions

        /// <summary>
        /// Gets the schema restrictions. There are different restrictions for each dataprovider :(
        /// </summary>
        private string[] GetSchemaRestrictions(DbConnection conn, string value, string restrictionType, params string[] restrictionName)
        {
            //
            var dv = GetRestrictions(conn, restrictionType);

            var restrictions = new string[dv.Count];
            for (var i = 0; i < dv.Count; i++)
            {
                var name = (string)dv[i].Row["RestrictionName"];
                var found = false;
                //if set for owner restriction, apply it here
                if (!string.IsNullOrEmpty(Owner) && name.Equals("OWNER", StringComparison.OrdinalIgnoreCase))
                {
                    restrictions[i] = Owner;
                    continue;
                }
                //other restrictions: different possible names
                if (restrictionName.Any(rname => name.Equals(rname, StringComparison.OrdinalIgnoreCase)))
                {
                    found = true;
                    restrictions[i] = value;
                }
                if (!found) restrictions[i] = null;
            }
            return restrictions;
        }

        /// <summary>
        /// Gets all the restrictions. Caches it.
        /// </summary>
        private DataView GetRestrictions(DbConnection conn, string restrictionType)
        {
            //get table of restrictions
            if (_restrictions == null)
                _restrictions = conn.GetSchema(DbMetaDataCollectionNames.Restrictions);

            //get the dataview (the defaultview from the datatable)
            var dv = _restrictions.DefaultView;
            dv.RowFilter = "CollectionName = '" + restrictionType + "'";
            dv.Sort = "RestrictionNumber";
            return dv;
        }

        private string[] GetSchemaRestrictionsForOwner(DbConnection conn, string owner)
        {
            return GetSchemaRestrictions(conn, owner, "Tables", "Owner");
        }

        private string[] GetSchemaRestrictionsForTable(DbConnection conn, string tableName)
        {
            return GetSchemaRestrictions(conn, tableName, "Columns", "Table", "Table_Name");
        }

        #endregion Restrictions

        #region MetadataCollections

        /// <summary>
        /// All the collections that are available via GetSchema
        /// </summary>
        public DataTable MetadataCollections()
        {
            var factory =
                DbProviderFactories.GetFactory(_providerName);
            //open a connection
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return _metadata;
                conn.ConnectionString = _connectionString;
                //get table of restrictions
                conn.Open();
                _metadata = MetadataCollections(conn);
                return _metadata;
            }
        }

        private static DataTable MetadataCollections(DbConnection conn)
        {
            return conn.GetSchema(DbMetaDataCollectionNames.MetaDataCollections);
        }

        /// <summary>
        /// All the Datatypes in the database and the mappings to .Net types
        /// </summary>
        /// <returns>DataTable with columns incl. TYPENAME, DataType (.net)</returns>
        public DataTable DataTypes()
        {
            var factory = DbProviderFactories.GetFactory(_providerName);
            using (var conn = factory.CreateConnection())
            {
                if (conn == null) return null;
                conn.ConnectionString = _connectionString;
                conn.Open();
                return conn.GetSchema(DbMetaDataCollectionNames.DataTypes);
            }
        }

        #endregion MetadataCollections
    }
}