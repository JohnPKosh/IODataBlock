using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using Business.Common.Extensions;
using Business.Common.IO;
using Data.DbClient;

namespace Data.MsAccess
{
    /// <summary>
    /// Microsoft Access Extension Base Methods.
    /// </summary>
    public static class MsAccessExtensionBase
    {
        #region Contants

        /// <summary>
        /// Private const _ConnectionTemplateString.
        /// </summary>
        private const String ConnectionTemplateStringTrusted = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;";

        /// <summary>
        /// Private const _ConnectionTemplateString.
        /// </summary>
        private const String ConnectionTemplateString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Jet OLEDB:Database Password=$(Password);";

        #endregion Contants

        #region Query Methods

        #region Query IEnumerable<dynamic> Methods

        public static IEnumerable<dynamic> Query(FileInfo fileInfo,
            String queryString,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0,
            Int32 commandTimeout = 60)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (fileInfo.Exists)
            {
                var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
                if (lockWaitMs > 0)
                {
                    using (new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                    {
                        if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
                        if (numberedArgs == null) return Query(String.Format(constr, fileInfo.FullName), queryString, commandTimeout);
                        return Query(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
                    }
                }
                if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
                return numberedArgs == null ? Query(String.Format(constr, fileInfo.FullName), queryString, commandTimeout) : Query(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
            }
            throw new FileNotFoundException();
        }

        public static IEnumerable<dynamic> Query(String connectionString, String query, Int32 commandTimeout = 60, params object[] parameters)
        {
            using (var connection = new OleDbConnection(connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = commandTimeout;
                if (parameters != null)
                {
                    var dbParameters = parameters.Select((o, index) =>
                    {
                        var str = cmd.CreateParameter();
                        str.ParameterName = index.ToString(CultureInfo.InvariantCulture);
                        var dbParameter = str;
                        var obj = o;
                        var @value = obj;
                        if (obj == null)
                        {
                            @value = DBNull.Value;
                        }
                        dbParameter.Value = @value;
                        return str;
                    }
                    );
                    foreach (var dbParameter1 in dbParameters)
                    {
                        cmd.Parameters.Add(dbParameter1);
                    }
                }
                connection.Open();
                using (cmd)
                {
                    List<string> columnNames = null;
                    var fcnt = 0;
                    var dbDataReaders = cmd.ExecuteReader();
                    using (dbDataReaders)
                    {
                        if (dbDataReaders != null)
                            foreach (DbDataRecord dbDataRecord in dbDataReaders)
                            {
                                if (columnNames == null)
                                {
                                    fcnt = dbDataRecord.FieldCount;
                                    columnNames = GetColumnNames(dbDataRecord).ToList();
                                }
                                dynamic e = new ExpandoObject();
                                var d = e as IDictionary<string, object>;
                                for (var i = 0; i < fcnt; i++)
                                    d.Add(columnNames[i], dbDataRecord[i]);
                                yield return e;
                            }
                    }
                }
                //if (Connection.State != ConnectionState.Closed)
                //{
                connection.Close();
                //}
            }
        }

        #endregion Query IEnumerable<dynamic> Methods

        #region Query Single Methods

        public static dynamic QuerySingle(FileInfo fileInfo,
            String queryString,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0,
            Int32 commandTimeout = 60)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
            if (lockWaitMs > 0)
            {
                using (new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
                    if (numberedArgs == null) return QuerySingle(String.Format(constr, fileInfo.FullName), queryString, commandTimeout);
                    return QuerySingle(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
                }
            }
            if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
            return numberedArgs == null ? QuerySingle(String.Format(constr, fileInfo.FullName), queryString, commandTimeout) : QuerySingle(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
        }

        public static dynamic QuerySingle(String connectionString, String query, Int32 commandTimeout = 60, params object[] parameters)
        {
            dynamic e = new ExpandoObject();
            using (var connection = new OleDbConnection(connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = commandTimeout;
                if (parameters != null)
                {
                    var dbParameters = parameters.Select((o, index) =>
                    {
                        var str = cmd.CreateParameter();
                        str.ParameterName = index.ToString(CultureInfo.InvariantCulture);
                        var dbParameter = str;
                        var obj = o;
                        var @value = obj;
                        if (obj == null)
                        {
                            @value = DBNull.Value;
                        }
                        dbParameter.Value = @value;
                        return str;
                    }
                    );
                    foreach (var dbParameter1 in dbParameters)
                    {
                        cmd.Parameters.Add(dbParameter1);
                    }
                }
                connection.Open();
                using (cmd)
                {
                    var dbDataReaders = cmd.ExecuteReader();
                    using (dbDataReaders)
                    {
                        if (dbDataReaders == null) return e;
                        foreach (DbDataRecord dbDataRecord in dbDataReaders)
                        {
                            var fcnt = dbDataRecord.FieldCount;
                            var columnNames = GetColumnNames(dbDataRecord).ToList();
                            var d = e as IDictionary<string, object>;
                            for (var i = 0; i < fcnt; i++)
                                d.Add(columnNames[i], dbDataRecord[i]);
                            break;
                        }
                    }
                }
            }
            return e;
        }

        #endregion Query Single Methods

        #region Query Scalar Methods

        public static dynamic QueryScalar(FileInfo fileInfo,
            String queryString,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0,
            Int32 commandTimeout = 60)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
            if (lockWaitMs > 0)
            {
                using (new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
                    if (numberedArgs == null) return QueryScalar(String.Format(constr, fileInfo.FullName), queryString, commandTimeout);
                    return QueryScalar(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
                }
            }
            if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
            return numberedArgs == null ? QueryScalar(String.Format(constr, fileInfo.FullName), queryString, commandTimeout) : QueryScalar(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
        }

        public static dynamic QueryScalar(String connectionString, String query, Int32 commandTimeout = 60, params object[] parameters)
        {
            object rv;
            using (var connection = new OleDbConnection(connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = query;
                cmd.CommandTimeout = commandTimeout;
                if (parameters != null)
                {
                    var dbParameters = parameters.Select((o, index) =>
                    {
                        var str = cmd.CreateParameter();
                        str.ParameterName = index.ToString(CultureInfo.InvariantCulture);
                        var dbParameter = str;
                        var obj = o;
                        var @value = obj;
                        if (obj == null)
                        {
                            @value = DBNull.Value;
                        }
                        dbParameter.Value = @value;
                        return str;
                    }
                    );
                    foreach (var dbParameter1 in dbParameters)
                    {
                        cmd.Parameters.Add(dbParameter1);
                    }
                }
                connection.Open();
                using (cmd)
                {
                    rv = cmd.ExecuteScalar();
                }
            }
            return rv;
        }

        #endregion Query Scalar Methods

        #endregion Query Methods

        #region Query To DataTable / Dataset Methods

        /// <summary>
        /// Queries to data table.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="password"></param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="namedArgs">The named args.</param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static DataTable QueryToDataTable(FileInfo fileInfo,
            String queryString,
            String password = null,
            String tableName = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0,
            Int32 commandTimeout = 60)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
            if (lockWaitMs > 0)
            {
                using (new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    using (var conn = new OleDbConnection(String.Format(constr, fileInfo.FullName)))
                    {
                        conn.Open();
                        if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
                        queryString = queryString.ReplaceNumberParameters(numberedArgs: numberedArgs);
                        var da = new OleDbDataAdapter(queryString, conn);
                        var dt = tableName == null ? new DataTable() : new DataTable(tableName);
                        if (commandTimeout > 0) da.SelectCommand.CommandTimeout = commandTimeout;
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            using (var conn = new OleDbConnection(String.Format(constr, fileInfo.FullName)))
            {
                conn.Open();
                if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
                queryString = queryString.ReplaceNumberParameters(numberedArgs: numberedArgs);
                var da = new OleDbDataAdapter(queryString, conn);
                var dt = tableName == null ? new DataTable() : new DataTable(tableName);
                if (commandTimeout > 0) da.SelectCommand.CommandTimeout = commandTimeout;
                da.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Queries to data set.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <param name="queryStrings">The query strings.</param>
        /// <param name="password"></param>
        /// <param name="namedArgs">The named args.</param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static DataSet QueriesToDataSet(FileInfo fileInfo,
            Dictionary<String, String> queryStrings = null,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0,
            Int32 commandTimeout = 60)
        {
            var rv = new DataSet();
            if (queryStrings == null) return rv;
            foreach (var dt in queryStrings.Select(q => QueryToDataTable(fileInfo, q.Value, password, q.Key, namedArgs, numberedArgs, lockWaitMs, commandTimeout)))
            {
                rv.Tables.Add(dt);
            }
            return rv;
        }

        #endregion Query To DataTable / Dataset Methods

        #region Execute Methods

        /// <summary>
        /// Executes the specified file info.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="password"></param>
        /// <param name="namedArgs">The named args.</param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static Int32 Execute(FileInfo fileInfo,
            String queryString,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 60000,
            Int32 commandTimeout = 60)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
            if (lockWaitMs > 0)
            {
                using (new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
                    if (numberedArgs == null) return Execute(String.Format(constr, fileInfo.FullName, commandTimeout), queryString);
                    return Execute(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
                }
            }
            if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
            return numberedArgs == null ? Execute(String.Format(constr, fileInfo.FullName), queryString, commandTimeout) : Execute(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
        }

        public static Int32 Execute(String connectionString, String queryString, Int32 commandTimeout = 60, params object[] parameters)
        {
            using (var connection = new OleDbConnection(connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandText = queryString;
                cmd.CommandTimeout = commandTimeout;
                if (parameters != null)
                {
                    var dbParameters = parameters.Select((o, index) =>
                    {
                        var str = cmd.CreateParameter();
                        str.ParameterName = index.ToString(CultureInfo.InvariantCulture);
                        var dbParameter = str;
                        var obj = o;
                        var @value = obj;
                        if (obj == null)
                        {
                            @value = DBNull.Value;
                        }
                        dbParameter.Value = @value;
                        return str;
                    }
                    );
                    foreach (var dbParameter1 in dbParameters)
                    {
                        cmd.Parameters.Add(dbParameter1);
                    }
                }
                connection.Open();
                using (cmd)
                {
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        #endregion Execute Methods

        #region ExecuteStatements Methods

        /// <summary>
        /// Executes the statements.
        /// </summary>
        /// <param name="fileInfo">The file info.</param>
        /// <param name="queryString">The query string.</param>
        /// <param name="password"></param>
        /// <param name="namedArgs">The named args.</param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        public static IEnumerable<Int32> ExecuteStatements(FileInfo fileInfo,
            String queryString,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 60000,
            Int32 commandTimeout = 60)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
            if (lockWaitMs > 0)
            {
                using (new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
                    return numberedArgs == null ? ExecuteStatements(String.Format(constr, fileInfo.FullName, commandTimeout), queryString) : ExecuteStatements(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
                }
            }

            if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
            return numberedArgs == null ? ExecuteStatements(String.Format(constr, fileInfo.FullName), queryString, commandTimeout) : ExecuteStatements(String.Format(constr, fileInfo.FullName), queryString, commandTimeout, numberedArgs.ToArray());
        }

        public static IEnumerable<Int32> ExecuteStatements(String connectionString, IEnumerable<String> queryStrings, Int32 commandTimeout = 60, params object[] parameters)
        {
            using (var connection = new OleDbConnection(connectionString))
            {
                var cmd = connection.CreateCommand();
                cmd.CommandTimeout = commandTimeout;
                if (parameters != null)
                {
                    var dbParameters = parameters.Select((o, index) =>
                    {
                        var str = cmd.CreateParameter();
                        str.ParameterName = index.ToString(CultureInfo.InvariantCulture);
                        var dbParameter = str;
                        var obj = o;
                        var @value = obj;
                        if (obj == null)
                        {
                            @value = DBNull.Value;
                        }
                        dbParameter.Value = @value;
                        return str;
                    }
                    );
                    foreach (var dbParameter1 in dbParameters)
                    {
                        cmd.Parameters.Add(dbParameter1);
                    }
                }
                connection.Open();
                using (cmd)
                {
                    foreach (var q in queryStrings)
                    {
                        cmd.CommandText = q;
                        yield return cmd.ExecuteNonQuery();
                    }
                }
                connection.Close();
            }
        }

        public static IEnumerable<Int32> ExecuteStatements(String connectionString, String queryString, Int32 commandTimeout = 60, params object[] parameters)
        {
            return ExecuteStatements(connectionString, queryString.SplitToSqlStatements(), commandTimeout, parameters);
        }

        #endregion ExecuteStatements Methods

        #region Helper Methods

        public static IEnumerable<string> GetColumnNames(DbDataRecord record)
        {
            for (var i = 0; i < record.FieldCount; i++)
            {
                yield return record.GetName(i);
            }
        }

        /// <summary>
        /// Gets the access ODBC linked table name for SQL.
        /// </summary>
        /// <param name="sqlServer">The SQL server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="sqlUserName"></param>
        /// <param name="sqlPassword"></param>
        /// <returns></returns>
        public static String GetAccessOdbcLinkedTableNameForSql(String sqlServer, String databaseName, String sqlUserName = null, String sqlPassword = null)
        {
            string connection;
            if (sqlUserName.IsNotNullOrEmpty() && sqlPassword.IsNotNullOrEmpty())
            {
                connection = @"[odbc;driver={SQL Server};server=$(SqlServer);database=$(DatabaseName);UID=$(SqlUserName);PWD=$(SqlPassword);]";
                var paramlist = new Dictionary<String, String>
                {
                    {"$(SqlServer)", sqlServer},
                    {"$(DatabaseName)", databaseName},
                    {"$(SqlUserName)", sqlUserName},
                    {"$(SqlPassword)", sqlPassword}
                };
                connection = connection.ReplaceNamedParameters(paramlist);
            }
            else
            {
                connection = @"[odbc;driver={SQL Server};server=$(SqlServer);database=$(DatabaseName);Integrated Security=True]";
                var paramlist = new Dictionary<String, String>
                {
                    {"$(SqlServer)", sqlServer},
                    {"$(DatabaseName)", databaseName}
                };
                connection = connection.ReplaceNamedParameters(paramlist);
            }
            return connection;
        }

        public static String GetMsAccessOpenrowsetString(FileInfo fileInfo, String accessTableQuery, String password = "")
        {
            const string template = @"OPENROWSET('Microsoft.ACE.OLEDB.12.0','$(AccessFile)';'admin';'$(Password)','$(AccessTableQuery)')";
            var paramlist = new Dictionary<String, String>
            {
                {"$(AccessFile)", fileInfo.FullName},
                {"$(AccessTableQuery)", accessTableQuery},
                {"$(Password)", password}
            };
            return template.ReplaceNamedParameters(paramlist);
        }

        #endregion Helper Methods
    }
}