using System;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace Data.DbClient
{
    public class SqlTableCreator
    {
        #region Instance Variables

        private SqlConnection _connection;

        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        private SqlTransaction _transaction;

        public SqlTransaction Transaction
        {
            get { return _transaction; }
            set { _transaction = value; }
        }

        private string _tableName;

        public string DestinationTableName
        {
            get { return _tableName; }
            set { _tableName = value; }
        }

        #endregion Instance Variables

        #region Constructor

        public SqlTableCreator()
        {
        }

        public SqlTableCreator(SqlConnection connection)
            : this(connection, null)
        {
        }

        public SqlTableCreator(SqlConnection connection, SqlTransaction transaction)
        {
            _connection = connection;
            _transaction = transaction;
        }

        #endregion Constructor

        #region Instance Methods

        public object Create(DataTable schema)
        {
            return Create(schema, null);
        }

        public object Create(DataTable schema, int numKeys)
        {
            var primaryKeys = new int[numKeys];
            for (var i = 0; i < numKeys; i++)
            {
                primaryKeys[i] = 1;
            }
            return Create(schema, primaryKeys);
        }

        public object Create(DataTable schema, int[] primaryKeys)
        {
            var sql = GetCreateSQL(_tableName, schema, primaryKeys);
            var cmd = _transaction?.Connection != null ? new SqlCommand(sql, _connection, _transaction) : new SqlCommand(sql, _connection);
            return cmd.ExecuteNonQuery();
        }

        public object CreateFromDataTable(DataTable table)
        {
            var sql = GetCreateFromDataTableSQL(_tableName, table);
            var cmd = _transaction?.Connection != null ? new SqlCommand(sql, _connection, _transaction) : new SqlCommand(sql, _connection);
            return cmd.ExecuteNonQuery();
        }

        #endregion Instance Methods

        #region Static Methods

        // ReSharper disable once InconsistentNaming
        public static string GetCreateSQL(string tableName, DataTable schema, int[] primaryKeys)
        {
            var sql = "CREATE TABLE " + tableName + " (\n";

            // columns
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (DataRow column in schema.Rows)
            {
                if (!(schema.Columns.Contains("IsHidden") && (bool)column["IsHidden"]))
                    sql += column["ColumnName"] + " " + SQLGetType(column) + ",\n";
            }
            sql = sql.TrimEnd(',', '\n') + "\n";

            // primary keys
            var pk = "CONSTRAINT PK_" + tableName + " PRIMARY KEY CLUSTERED (";
            var hasKeys = primaryKeys != null && primaryKeys.Length > 0;

            if (hasKeys)
            {
                // user defined keys
                pk = primaryKeys.Aggregate(pk, (current, key) => current + (schema.Rows[key]["ColumnName"] + ", "));
            }
            else
            {
                // check schema for keys
                var keys = string.Join(", ", GetPrimaryKeys(schema));
                pk += keys;
                hasKeys = keys.Length > 0;
            }
            pk = pk.TrimEnd(',', ' ', '\n') + ")\n";
            if (hasKeys) sql += pk;

            sql += ")";

            return sql;
        }

        // ReSharper disable once InconsistentNaming
        public static string GetCreateFromDataTableSQL(string tableName, DataTable table, int defaultStringColumnSize = -1)
        {
            var sql = tableName.Contains("[") ? "CREATE TABLE " + tableName + " (\n" : "CREATE TABLE [" + tableName + "] (\n";
            // columns
            sql = table.Columns.Cast<DataColumn>().Aggregate(sql, (current, column) => current + "\t[" + column.ColumnName + "] " + SQLGetType(column, defaultStringColumnSize) + ",\n");
            sql = sql.TrimEnd(',', '\n') + "\n";
            // primary keys
            if (table.PrimaryKey.Length > 0)
            {
                sql += "CONSTRAINT [PK_" + tableName + "] PRIMARY KEY CLUSTERED (";
                sql = table.PrimaryKey.Aggregate(sql, (current, column) => current + "\t[" + column.ColumnName + "],");
                sql = sql.TrimEnd(',') + "))\n";
            }

            //if not ends with ")"
            if ((table.PrimaryKey.Length == 0) && !sql.EndsWith(")"))
            {
                sql += ")";
            }

            return sql;
        }

        public static string[] GetPrimaryKeys(DataTable schema)
        {
            return (from DataRow column in schema.Rows where schema.Columns.Contains("IsKey") && (bool)column["IsKey"] select column["ColumnName"].ToString()).ToArray();
        }

        // Return T-SQL data type definition, based on schema definition for a column
        // ReSharper disable once InconsistentNaming
        public static string SQLGetType(object type, int columnSize, int numericPrecision, int numericScale)
        {
            switch (type.ToString())
            {
                case "System.String":
                    return GetMaxTypeString("NVARCHAR", columnSize);

                case "System.Decimal":
                    if (numericScale > 0)
                        return "REAL";
                    return numericPrecision > 10 ? "BIGINT" : "INT";

                case "System.Double":
                    return "FLOAT";

                case "System.Single":
                    return "REAL";

                case "System.Int64":
                    return "BIGINT";

                case "System.Int16":
                    return "INT";

                case "System.Int32":
                    return "INT";

                case "System.DateTime":
                    return "DATETIME";

                case "System.Boolean":
                    return "BIT";

                case "System.Byte":
                    return "TINYINT";

                case "System.Byte[]":
                    return GetMaxTypeString("VARBINARY", columnSize);

                case "System.Guid":
                    return "UNIQUEIDENTIFIER";

                default:
                    throw new Exception(type + " not implemented.");
            }
        }

        // ReSharper disable once InconsistentNaming
        public static string SQLGetTypeFromDataColumn(DataColumn column, int defaultStringColumnSize = -1)
        {
            var nullsuffix = column.AllowDBNull ? " NULL" : " NOT NULL";

            var maxlen = column.MaxLength < 1 ? defaultStringColumnSize : column.MaxLength;

            switch (column.DataType.UnderlyingSystemType.ToString())
            {
                case "System.String":
                    return GetMaxTypeString("NVARCHAR", maxlen) + nullsuffix;

                case "System.Decimal": // temporary
                    return "NVARCHAR(39)" + nullsuffix;

                case "System.Double":
                    return "FLOAT" + nullsuffix;

                case "System.Single":
                    return "REAL" + nullsuffix;

                case "System.Int64":
                    return "BIGINT" + nullsuffix;

                case "System.Int16":
                    return "INT" + nullsuffix;

                case "System.Int32":
                    return "INT" + nullsuffix;

                case "System.DateTime":
                    return "DATETIME" + nullsuffix;

                case "System.Boolean":
                    return "BIT" + nullsuffix;

                case "System.Byte":
                    return "TINYINT" + nullsuffix;

                case "System.Byte[]":
                    //return GetMaxTypeString("VARBINARY", column.MaxLength);
                    return "image" + nullsuffix;

                case "System.Guid":
                    return "UNIQUEIDENTIFIER" + nullsuffix;

                default:
                    throw new Exception(column.DataType.UnderlyingSystemType + " not implemented.");
            }
        }

        public static string GetMsSqlColumnStringFromClrType(string valueType, bool nullable, int maxStringLength = -1)
        {
            var nullsuffix = nullable ? " NULL" : " NOT NULL";
            var maxlen = maxStringLength < 1 ? -1 : maxStringLength;

            switch (valueType)
            {
                case "System.String":
                    return GetMaxTypeString("NVARCHAR", maxlen) + nullsuffix;

                case "System.Decimal": // temporary
                    return "NVARCHAR(39)" + nullsuffix;

                case "System.Double":
                    return "FLOAT" + nullsuffix;

                case "System.Single":
                    return "REAL" + nullsuffix;

                case "System.Int64":
                    return "BIGINT" + nullsuffix;

                case "System.Int16":
                    return "INT" + nullsuffix;

                case "System.Int32":
                    return "INT" + nullsuffix;

                case "System.DateTime":
                    return "DATETIME" + nullsuffix;

                case "System.Boolean":
                    return "BIT" + nullsuffix;

                case "System.Byte":
                    return "TINYINT" + nullsuffix;

                case "System.Byte[]":
                    //return GetMaxTypeString("VARBINARY", column.MaxLength);
                    return "image" + nullsuffix;

                case "System.Guid":
                    return "UNIQUEIDENTIFIER" + nullsuffix;

                default:
                    throw new ArgumentException(valueType + " type not implemented.");
            }
        }

        public static string GetNvarcharTypeString(int columnSize = -1)
        {
            var rv = columnSize == -1 ? "MAX" : columnSize.ToString(CultureInfo.InvariantCulture);
            return $"NVARCHAR({rv})";
        }

        public static string GetMaxTypeString(string value, int columnSize = -1)
        {
            var rv = columnSize == -1 ? "MAX" : columnSize.ToString(CultureInfo.InvariantCulture);
            return $"{value}({rv})";
        }

        // Overload based on row from schema table
        // ReSharper disable once InconsistentNaming
        public static string SQLGetType(DataRow schemaRow)
        {
            return SQLGetType(schemaRow["DataType"],
                                int.Parse(schemaRow["ColumnSize"].ToString()),
                                int.Parse(schemaRow["NumericPrecision"].ToString()),
                                int.Parse(schemaRow["NumericScale"].ToString()));
        }

        // Overload based on DataColumn from DataTable type
        // ReSharper disable once InconsistentNaming
        public static string SQLGetType(DataColumn column, int defaultStringColumnSize = -1)
        {
            //return SQLGetType(column.DataType, column.MaxLength, 10, 2);

            return SQLGetTypeFromDataColumn(column, defaultStringColumnSize);
        }

        #endregion Static Methods
    }
}