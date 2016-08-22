using Business.Common.Extensions;
using Business.Common.System.Args;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Core.Objects;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml.Linq;

// ReSharper disable once CheckNamespace
namespace Data.DbClient
{
    public static class DataExtensionBase
    {
        #region Conversion Methods

        public static T GetScalarResult<T>(this ObjectResult<T> result)
        {
            return result.First();
        }

        public static List<dynamic> ToExpandoList(this IDataReader rdr)
        {
            var result = new List<dynamic>();
            while (rdr.Read())
            {
                dynamic e = new ExpandoObject();
                var d = e as IDictionary<string, object>;
                for (var i = 0; i < rdr.FieldCount; i++)
                    d.Add(rdr.GetName(i), rdr[i]);
                result.Add(e);
            }
            return result;
        }

        public static List<dynamic> ToExpandoList(this DataTable rdr)
        {
            var result = new List<dynamic>();
            foreach (DataRow r in rdr.Rows)
            {
                dynamic e = new ExpandoObject();
                var d = e as IDictionary<string, object>;
                for (var i = 0; i < rdr.Columns.Count; i++)
                    d.Add(rdr.Columns[i].ColumnName, r[i]);
                result.Add(e);
            }
            return result;
        }

        public static IEnumerable<dynamic> ToIEnumerableExpando(this IDataReader rdr)
        {
            var result = new List<dynamic>();
            while (rdr.Read())
            {
                dynamic e = new ExpandoObject();
                var d = e as IDictionary<string, object>;
                for (var i = 0; i < rdr.FieldCount; i++)
                    d.Add(rdr.GetName(i), rdr[i]);
                result.Add(e);
                yield return e;
            }
        }

        public static List<Dictionary<string, object>> ToDictionaryList(this IDataReader rdr)
        {
            var result = new List<Dictionary<string, object>>();
            while (rdr.Read())
            {
                dynamic e = new ExpandoObject();
                var d = e as IDictionary<string, object>;
                for (var i = 0; i < rdr.FieldCount; i++)
                    d.Add(rdr.GetName(i), rdr[i]);
                result.Add(e);
            }
            return result;
        }

        public static IEnumerable<Dictionary<string, object>> ToIEnumerableDictionaryObjects(this IDataReader rdr)
        {
            while (rdr.Read())
            {
                dynamic e = new ExpandoObject();
                var d = e as IDictionary<string, object>;
                for (var i = 0; i < rdr.FieldCount; i++)
                    d.Add(rdr.GetName(i), rdr[i]);
                yield return d.ToDictionary(x => x.Key, x => x.Value);
            }
        }

        public static XElement ExpandoToXml(dynamic node, string nodeName)
        {
            var xmlNode = new XElement(nodeName);

            foreach (var property in node)
            {
                if (property is ExpandoObject)
                {
                    xmlNode.Add(ExpandoToXml(property, nodeName));
                }
                else
                {
                    if (property.Value.GetType() == typeof(List<dynamic>))
                    {
                        foreach (var element in (List<dynamic>)property.Value)
                        {
                            xmlNode.Add(ExpandoToXml(element, property.Key));
                        }
                    }
                    else
                    {
                        var t = property.Value.GetType();
                        if (!(t.IsGenericType && !t.UnderlyingSystemType.IsValueType))
                        {
                            xmlNode.Add(new XElement(property.Key, property.Value));
                        }
                    }
                }
            }
            return xmlNode;
        }

        #endregion Conversion Methods

        #region DataTable Methods

        public static DataTable ToDataTable<T>(this IEnumerable<T> varlist, DataTable template = null)
        {
            var rv = template ?? new DataTable();
            PropertyInfo[] oProps = null; // column names
            var columnsExist = template != null;
            var isExpando = false;
            if (varlist == null) return rv;

            foreach (var rec in varlist)
            {
                if (isExpando || rec is ExpandoObject)
                {
                    if (!columnsExist)
                    {
                        isExpando = true;
                        foreach (var prop in (IDictionary<string, object>)rec)
                        {
                            var colType = prop.Value == null || prop.Value is DBNull ? typeof(object) : prop.Value.GetType();
                            rv.Columns.Add(new DataColumn(prop.Key, colType));
                        }
                        columnsExist = true;
                    }
                    var dr = rv.NewRow();
                    foreach (var prop in ((IDictionary<string, object>)rec).Where(prop => rv.Columns.IndexOf(prop.Key) > -1))
                    {
                        dr[prop.Key] = prop.Value ?? DBNull.Value;
                    }
                    rv.Rows.Add(dr);
                }
                else
                {
                    if (oProps == null) oProps = rec.GetType().GetProperties();
                    // Use reflection to get property names, to create table, Only first time, others will follow
                    if (!columnsExist)
                    {
                        foreach (var pi in oProps)
                        {
                            var colType = pi.PropertyType;
                            if (colType.IsGenericType && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }
                            rv.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                        columnsExist = true;
                    }
                    var dr = rv.NewRow();
                    foreach (var pi in oProps.Where(pi => rv.Columns.IndexOf(pi.Name) > -1))
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                    }
                    rv.Rows.Add(dr);
                }
            }
            return rv;
        }

        public static DataTable TransformToDataTable<TDest, TSource>(this IEnumerable<TSource> varlist, Func<TSource, TDest> function, DataTable template = null)
        {
            var rv = template ?? new DataTable();
            PropertyInfo[] oProps = null;// column names
            var columnsExist = template != null;
            var isExpando = false;
            if (varlist == null) return rv;

            foreach (var rec in varlist.IEnumerableTransformEach(function))
            {
                if (isExpando || rec is ExpandoObject)
                {
                    if (!columnsExist)
                    {
                        isExpando = true;
                        foreach (var prop in (IDictionary<string, object>)rec)
                        {
                            var colType = prop.Value?.GetType() ?? typeof(object);
                            rv.Columns.Add(new DataColumn(prop.Key, colType));
                        }
                        columnsExist = true;
                    }
                    var dr = rv.NewRow();
                    foreach (var prop in ((IDictionary<string, object>)rec).Where(prop => rv.Columns.IndexOf(prop.Key) > -1))
                    {
                        dr[prop.Key] = prop.Value ?? DBNull.Value;
                    }
                    rv.Rows.Add(dr);
                }
                else
                {
                    if (oProps == null) oProps = rec.GetType().GetProperties();
                    // Use reflection to get property names, to create table, Only first time, others will follow
                    if (!columnsExist)
                    {
                        foreach (var pi in oProps)
                        {
                            var colType = pi.PropertyType;
                            if (colType.IsGenericType && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }
                            rv.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                        columnsExist = true;
                    }
                    var dr = rv.NewRow();
                    foreach (var pi in oProps.Where(pi => rv.Columns.IndexOf(pi.Name) > -1))
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                    }
                    rv.Rows.Add(dr);
                }
            }
            return rv;
        }

        public static DataTable ToDataTable(this IEnumerable varlist, DataTable template = null)
        {
            var rv = template ?? new DataTable();
            PropertyInfo[] oProps = null; // column names
            var columnsExist = template != null;
            var isExpando = false;
            if (varlist == null) return rv;

            foreach (var rec in varlist)
            {
                if (rec.GetType().FullName == "MS.Internal.NamedObject") continue;
                if (isExpando || rec is ExpandoObject)
                {
                    if (!columnsExist)
                    {
                        isExpando = true;
                        foreach (var prop in (IDictionary<string, object>)rec)
                        {
                            var colType = prop.Value?.GetType() ?? typeof(object);
                            rv.Columns.Add(new DataColumn(prop.Key, colType));
                        }
                        columnsExist = true;
                    }
                    var dr = rv.NewRow();
                    foreach (var prop in ((IDictionary<string, object>)rec).Where(prop => rv.Columns.IndexOf(prop.Key) > -1))
                    {
                        dr[prop.Key] = prop.Value ?? DBNull.Value;
                    }
                    rv.Rows.Add(dr);
                }
                else
                {
                    if (oProps == null) oProps = rec.GetType().GetProperties();
                    // Use reflection to get property names, to create table, Only first time, others will follow
                    if (!columnsExist)
                    {
                        foreach (var pi in oProps)
                        {
                            var colType = pi.PropertyType;
                            if (colType.IsGenericType && (colType.GetGenericTypeDefinition() == typeof(Nullable<>)))
                            {
                                colType = colType.GetGenericArguments()[0];
                            }
                            rv.Columns.Add(new DataColumn(pi.Name, colType));
                        }
                        columnsExist = true;
                    }
                    var dr = rv.NewRow();
                    foreach (var pi in oProps.Where(pi => rv.Columns.IndexOf(pi.Name) > -1))
                    {
                        dr[pi.Name] = pi.GetValue(rec, null) ?? DBNull.Value;
                    }
                    rv.Rows.Add(dr);
                }
            }
            return rv;
        }

        public static DataTable ApplyFilterSort(this DataTable table, string rowFilter = null, string sort = null)
        {
            if (!string.IsNullOrWhiteSpace(rowFilter)) table.DefaultView.RowFilter = rowFilter;
            if (!string.IsNullOrWhiteSpace(sort)) table.DefaultView.Sort = sort;
            return table.DefaultView.ToTable();
        }

        public static void RemoveColumns(this DataTable input, string columnNames, string seperator = ",")
        {
            var columnNameList = columnNames.Split(seperator.ToCharArray(), StringSplitOptions.RemoveEmptyEntries).ToList();
            foreach (var n in columnNameList)
            {
                input.Columns.Remove(n.Trim());
            }
        }

        public static void RemoveByteArrayColumns(this DataTable input)
        {
            var columnNameList = (from DataColumn c in input.Columns where c.DataType.UnderlyingSystemType.Name.ToLower() == "byte[]" select c.ColumnName).ToList();
            foreach (var n in columnNameList)
            {
                input.Columns.Remove(n);
            }
        }

        public static void RemoveNonSystemTypeColumns(this DataTable input, bool removeByteArrayColumns = true)
        {
            var columnNameList = input.Columns.Cast<DataColumn>()
                .Where(
                    c =>
                        c.DataType.Namespace != "System" ||
                        (removeByteArrayColumns && c.DataType.UnderlyingSystemType.Name.ToLower() == "byte[]"))
                .Select(c => c.ColumnName).ToList();
            foreach (var n in columnNameList)
            {
                input.Columns.Remove(n);
            }
        }

        public static DataTable GetDataTableBySqlDataAdapter(string selectCommandText, string sqlServer
            , string databaseName
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            , string tableName = null
            )
        {
            var constr = GetSqlConnectionString(sqlServer, databaseName, sqlUserName, sqlPassword, applicationName, connectTimeout);
            return GetDataTableBySqlDataAdapter(selectCommandText, constr, tableName);
        }

        public static DataTable GetDataTableBySqlDataAdapter(string selectCommandText, string constr, string tableName = null)
        {
            DataTable rv;
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(constr);
                var tname = string.IsNullOrWhiteSpace(tableName) ? DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) : tableName;
                rv = new DataTable(tname);
                var da = new SqlDataAdapter(selectCommandText, con) { SelectCommand = { CommandTimeout = 600 } };
                if (con.State != ConnectionState.Open) con.Open();
                da.Fill(rv);
                if (con.State == ConnectionState.Open) con.Close();
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed) con.Close();
                con?.Dispose();
            }
            return rv;
        }

        public static void ImportDataTableToSql(
            ref DataTable dt
            , string destinationTableName
            , string sqlServer
            , string databaseName
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            , int packetSize = 8192
            , IEnumerable<SqlBulkCopyColumnMapping> sqlBulkCopyColumnMappings = null
            , int batchSize = 0
            , int bulkCopyTimeout = 0
            , bool enableIndentityInsert = false
            )
        {
            var providerConnectionString = GetSqlConnectionString(sqlServer
                , databaseName
                , sqlUserName
                , sqlPassword
                , applicationName
                , connectTimeout
                );
            using (var bulkCopy = new SqlBulkCopy(providerConnectionString, enableIndentityInsert ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default))
            {
                bulkCopy.DestinationTableName = destinationTableName;
                if (batchSize > 0) bulkCopy.BatchSize = batchSize;
                if (bulkCopyTimeout > 0) bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
                if (sqlBulkCopyColumnMappings != null)
                {
                    foreach (var mapId in sqlBulkCopyColumnMappings)
                    {
                        bulkCopy.ColumnMappings.Add(mapId);
                    }
                }
                bulkCopy.WriteToServer(dt);
            }
        }

        #endregion DataTable Methods

        #region Schema Methods

        public static SqlParameter[] DiscoverSqlSpParameterSet(SqlConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentException("connection is NULL!");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentException("spName is NULL!");
            var cmd = new SqlCommand(spName, connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            SqlCommandBuilder.DeriveParameters(cmd);
            connection.Close();
            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }
            var discoveredParameters = new SqlParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            foreach (var discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        public static OleDbParameter[] DiscoverOleDbSpParameterSet(OleDbConnection connection, string spName, bool includeReturnValueParameter)
        {
            if (connection == null) throw new ArgumentException("connection is NULL!");
            if (string.IsNullOrEmpty(spName)) throw new ArgumentException("spName is NULL!");
            var cmd = new OleDbCommand(spName, connection) { CommandType = CommandType.StoredProcedure };
            connection.Open();
            OleDbCommandBuilder.DeriveParameters(cmd);
            connection.Close();
            if (!includeReturnValueParameter)
            {
                cmd.Parameters.RemoveAt(0);
            }
            var discoveredParameters = new OleDbParameter[cmd.Parameters.Count];
            cmd.Parameters.CopyTo(discoveredParameters, 0);

            // Init the parameters with a DBNull value
            foreach (var discoveredParameter in discoveredParameters)
            {
                discoveredParameter.Value = DBNull.Value;
            }
            return discoveredParameters;
        }

        public static DataTable GetSchemaDataTableBySqlDataAdapter(string selectCommandText, string sqlServer
            , string databaseName
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            , string tableName = null
            )
        {
            var constr = GetSqlConnectionString(sqlServer, databaseName, sqlUserName, sqlPassword, applicationName, connectTimeout);
            return GetSchemaDataTableBySqlDataAdapter(selectCommandText, constr, tableName);
        }

        public static DataTable GetSchemaDataTableBySqlDataAdapter(string selectCommandText, string constr, string tableName = null)
        {
            DataTable rv;
            SqlConnection con = null;
            try
            {
                con = new SqlConnection(constr);
                var tname = string.IsNullOrWhiteSpace(tableName) ? DateTime.Now.Ticks.ToString(CultureInfo.InvariantCulture) : tableName;
                rv = new DataTable(tname);
                var da = new SqlDataAdapter(selectCommandText, con);
                if (con.State != ConnectionState.Open) con.Open();
                da.FillSchema(rv, SchemaType.Source);
                if (con.State == ConnectionState.Open) con.Close();
            }
            finally
            {
                if (con != null && con.State != ConnectionState.Closed) con.Close();
                con?.Dispose();
            }
            return rv;
        }

        #endregion Schema Methods

        #region ConnectionString Methods

        public static string GetSqlConnectionString(string sqlServer
            , string databaseName
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            )
        {
            var cb = new SqlConnectionStringBuilder { DataSource = sqlServer, InitialCatalog = databaseName };
            if (!string.IsNullOrWhiteSpace(sqlUserName) && !string.IsNullOrWhiteSpace(sqlPassword))
            {
                cb.UserID = sqlUserName;
                cb.Password = sqlPassword;
            }
            else
            {
                cb.IntegratedSecurity = true;
            }
            if (!string.IsNullOrWhiteSpace(applicationName)) cb.ApplicationName = applicationName;
            if (connectTimeout > -1) cb.ConnectTimeout = connectTimeout;
            return cb.ConnectionString;
        }

        public static string ConnectionStringFromEntityConnectionString(string entityConnectionString)
        {
            var cb = new EntityConnectionStringBuilder(entityConnectionString);
            return cb.ProviderConnectionString;
        }

        #region Create SqlServer EntityConnection Methods

        public static EntityConnection CreateEntityConnection(string sqlServer
            , string databaseName
            , string metaDataPath
            , string metaModelName
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            )
        {
            var providerConnectionString = GetSqlConnectionString(sqlServer
                , databaseName
                , sqlUserName
                , sqlPassword
                , applicationName
                , connectTimeout
                );

            var cb = new EntityConnectionStringBuilder();
            var metaPath = Path.Combine(metaDataPath, metaModelName);
            cb.Metadata = string.Format(@"{0}.csdl|{0}.ssdl|{0}.msl", metaPath);
            cb.Provider = @"System.Data.SqlClient";
            cb.ProviderConnectionString = providerConnectionString;
            return new EntityConnection(cb.ConnectionString);
        }

        public static EntityConnection CreateEntityConnection(string sqlServer
            , string databaseName
            , string csdlPath
            , string ssdlPath
            , string mslPath
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            )
        {
            var providerConnectionString = GetSqlConnectionString(sqlServer
                , databaseName
                , sqlUserName
                , sqlPassword
                , applicationName
                , connectTimeout
                );

            var cb = new EntityConnectionStringBuilder
            {
                Metadata = $"{csdlPath}|{ssdlPath}|{mslPath}",
                Provider = @"System.Data.SqlClient",
                ProviderConnectionString = providerConnectionString
            };
            return new EntityConnection(cb.ConnectionString);
        }

        public static EntityConnection CreateEntityConnection(string sqlServer
            , string databaseName
            , string metaPath
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            )
        {
            var providerConnectionString = GetSqlConnectionString(sqlServer
                , databaseName
                , sqlUserName
                , sqlPassword
                , applicationName
                , connectTimeout
                );

            var cb = new EntityConnectionStringBuilder
            {
                Metadata = metaPath,
                Provider = @"System.Data.SqlClient",
                ProviderConnectionString = providerConnectionString
            };
            return new EntityConnection(cb.ConnectionString);
        }

        #endregion Create SqlServer EntityConnection Methods

        #region Create SqlCe EntityConnection Methods

        public static EntityConnection CreateSqlCeEntityConnection(string sqlServer
            , string databaseName
            , string metaDataPath
            , string metaModelName
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            )
        {
            var providerConnectionString = GetSqlConnectionString(sqlServer
                , databaseName
                , sqlUserName
                , sqlPassword
                , applicationName
                , connectTimeout
                );

            var cb = new EntityConnectionStringBuilder();
            var metaPath = Path.Combine(metaDataPath, metaModelName);
            cb.Metadata = string.Format(@"{0}.csdl|{0}.ssdl|{0}.msl", metaPath);
            cb.Provider = @"System.Data.SqlServerCe.4.0";
            cb.ProviderConnectionString = providerConnectionString;
            return new EntityConnection(cb.ConnectionString);
        }

        public static EntityConnection CreateSqlCeEntityConnection(string sqlServer
            , string databaseName
            , string csdlPath
            , string ssdlPath
            , string mslPath
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            )
        {
            var providerConnectionString = GetSqlConnectionString(sqlServer
                , databaseName
                , sqlUserName
                , sqlPassword
                , applicationName
                , connectTimeout
                );

            var cb = new EntityConnectionStringBuilder
            {
                Metadata = $"{csdlPath}|{ssdlPath}|{mslPath}",
                Provider = @"System.Data.SqlServerCe.4.0",
                ProviderConnectionString = providerConnectionString
            };
            return new EntityConnection(cb.ConnectionString);
        }

        public static EntityConnection CreateSqlCeEntityConnection(string sqlServer
            , string databaseName
            , string metaPath
            , string sqlUserName = null
            , string sqlPassword = null
            , string applicationName = null
            , int connectTimeout = -1
            )
        {
            var providerConnectionString = GetSqlConnectionString(sqlServer
                , databaseName
                , sqlUserName
                , sqlPassword
                , applicationName
                , connectTimeout
                );

            var cb = new EntityConnectionStringBuilder
            {
                Metadata = metaPath,
                Provider = @"System.Data.SqlServerCe.4.0",
                ProviderConnectionString = providerConnectionString
            };
            return new EntityConnection(cb.ConnectionString);
        }

        #endregion Create SqlCe EntityConnection Methods

        #endregion ConnectionString Methods

        #region Sql Script Methods

        public static IEnumerable<string> ReadSqlScriptArgumentListFromFile(string filePath)
        {
            var rawscripts = ArgumentList.ReadXml(filePath).Items.OrderBy(x => int.Parse(x["step"])).Select(x => x["script"]);
            var rv = new List<string>();
            foreach (var s in rawscripts)
            {
                rv.AddRange(SplitToSqlStatements(s));
            }
            return rv.AsEnumerable();
        }

        public static IEnumerable<string> SplitToSqlStatements(this string s)
        {
            return Regex.Split(s, @"^\s*GO;*\r*\n*\b|(^|\s+)GO;*\r*\n*\b|\s*;\b*\s*$", RegexOptions.IgnoreCase | RegexOptions.Multiline).AsEnumerable().Select(x => x.Trim()).Where(x => !string.IsNullOrWhiteSpace(x.Trim()));
        }

        //// ReSharper disable once InconsistentNaming
        //        public static String CreateSQLServerBatchSelect(String selectSql, Int32 batchNumber, Int32 batchSize, String rowOrderBy)
        //        {
        //            var rowNumberSql = @" ROW_NUMBER() OVER (ORDER BY $(RowOrderBy)) AS [RowNumber], ".Replace("$(RowOrderBy)", rowOrderBy);
        //            const string template = @"
        //WITH [temp_batch_table] AS
        //(
        //    $(InputSql)
        //)
        //SELECT *
        //FROM [temp_batch_table]
        //WHERE [RowNumber] BETWEEN ($(BatchNo)*$(BatchSize)) AND (($(BatchNo)*$(BatchSize))+$(BatchSize)-1);
        //";

        //            var selectIdx = selectSql.IndexOf("select", StringComparison.InvariantCultureIgnoreCase) + "select".Length;
        //            selectSql = selectSql.Insert(selectIdx, rowNumberSql);
        //            var outputSql = template.Replace("$(BatchNo)", batchNumber.ToString(CultureInfo.InvariantCulture)).Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture)).Replace("$(InputSql)", selectSql);

        //            return outputSql;
        //        }

        //// ReSharper disable once InconsistentNaming
        //        public static String CreateSQLServerCountSelect(String selectSql, Int32 batchSize = 0)
        //        {
        //            string template;
        //            if (batchSize < 1)
        //            {
        //                template = @"
        //SELECT COUNT(*) as [cnt]
        //FROM
        //(
        //    $(InputSql)
        //) as a
        //";
        //            }
        //            else
        //            {
        //                template = @"
        //SELECT (COUNT(*) / $(BatchSize)) as [cnt]
        //FROM
        //(
        //    $(InputSql)
        //) as a
        //".Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture));
        //            }

        //            var outputSql = template.Replace("$(InputSql)", selectSql);
        //            return outputSql;
        //        }

        #endregion Sql Script Methods
    }
}