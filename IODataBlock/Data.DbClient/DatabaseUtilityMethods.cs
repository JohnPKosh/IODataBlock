using Data.DbClient.BulkCopy;
using Newtonsoft.Json.Linq;
using Npgsql;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Data.DbClient
{
    public partial class Database
    {
        #region Database class IEnumerable<object> paramaterized method overloads

        #region Execute Methods

        public int Execute(string commandText, IEnumerable<object> parameters, int commandTimeout = 0)
        {
            return parameters == null ? Execute(commandText, commandTimeout) : Execute(commandText, commandTimeout, parameters.ToArray());
        }

        public static int ExecuteNonQuery(string connectionString, string providerName, string commandText, IEnumerable<object> parameters, int commandTimeout = 0)
        {
            return parameters == null ? ExecuteNonQuery(connectionString, providerName, commandText, commandTimeout) : ExecuteNonQuery(connectionString, providerName, commandText, commandTimeout, parameters.ToArray());
        }

        public static int ExecuteNonQuery(DbConnection connection, string commandText, IEnumerable<object> parameters, int commandTimeout = 0)
        {
            return parameters == null ? ExecuteNonQuery(connection, commandText, commandTimeout) : ExecuteNonQuery(connection, commandText, commandTimeout, parameters.ToArray());
        }

        #endregion Execute Methods

        #region Query Methods

        public IEnumerable<dynamic> Query(string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? Query(commandText, commandTimeout) : Query(commandText, commandTimeout, parameters.ToArray());
        }

        public static IEnumerable<dynamic> Query(string connectionString, string providerName, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? Query(connectionString, providerName, commandText, commandTimeout) : Query(connectionString, providerName, commandText, commandTimeout, parameters.ToArray());
        }

        public static IEnumerable<dynamic> Query(DbConnection connection, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? Query(connection, commandText, commandTimeout) : Query(connection, commandText, commandTimeout, parameters.ToArray());
        }

        public IEnumerable<JObject> QueryToJObjects(string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryToJObjects(commandText, commandTimeout) : QueryToJObjects(commandText, commandTimeout, parameters.ToArray());
        }

        public static IEnumerable<JObject> QueryToJObjects(string connectionString, string providerName, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryToJObjects(connectionString, providerName, commandText, commandTimeout) : QueryToJObjects(connectionString, providerName, commandText, commandTimeout, parameters.ToArray());
        }

        public static IEnumerable<JObject> QueryToJObjects(DbConnection connection, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryToJObjects(connection, commandText, commandTimeout) : QueryToJObjects(connection, commandText, commandTimeout, parameters.ToArray());
        }

        public IEnumerable<T> QueryTransformEach<T>(string commandText, Func<JObject, T> function, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryTransformEach(commandText, function, commandTimeout) : QueryTransformEach(commandText, function, commandTimeout, parameters.ToArray());
        }

        public static IEnumerable<T> QueryTransformEach<T>(string connectionString, string providerName, string commandText, Func<JObject, T> function, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryTransformEach(connectionString, providerName, commandText, function, commandTimeout) : QueryTransformEach(connectionString, providerName, commandText, function, commandTimeout, parameters.ToArray());
        }

        public static IEnumerable<T> QueryTransformEach<T>(DbConnection connection, string commandText, Func<JObject, T> function, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryTransformEach(connection, commandText, function, commandTimeout) : QueryTransformEach(connection, commandText, function, commandTimeout, parameters.ToArray());
        }

        public Stream QueryToBson(string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryToBson(commandText, commandTimeout) : QueryToBson(commandText, commandTimeout, parameters.ToArray());
        }

        public static Stream QueryToBson(string connectionString, string providerName, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryToBson(connectionString, providerName, commandText, commandTimeout) : QueryToBson(connectionString, providerName, commandText, commandTimeout, parameters.ToArray());
        }

        public static Stream QueryToBson(DbConnection connection, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryToBson(connection, commandText, commandTimeout) : QueryToBson(connection, commandText, commandTimeout, parameters.ToArray());
        }

        public Stream QueryToJsonStream(string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryToJsonStream(commandText, commandTimeout) : QueryToJsonStream(commandText, commandTimeout, parameters.ToArray());
        }

        public static Stream QueryToJsonStream(string connectionString, string providerName, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryToJsonStream(connectionString, providerName, commandText, commandTimeout) : QueryToJsonStream(connectionString, providerName, commandText, commandTimeout, parameters.ToArray());
        }

        public static Stream QueryToJsonStream(DbConnection connection, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryToJsonStream(connection, commandText, commandTimeout) : QueryToJsonStream(connection, commandText, commandTimeout, parameters.ToArray());
        }

        public DataTable QueryToDataTable(string commandText, IEnumerable<object> parameters, string tableName = null, int commandTimeout = 60)
        {
            return parameters == null ? QueryToDataTable(commandText, tableName, commandTimeout) : QueryToDataTable(commandText, tableName, commandTimeout, parameters.ToArray());
        }

        public static DataTable QueryToDataTable(string connectionString, string providerName, string commandText, IEnumerable<object> parameters, string tableName = null, int commandTimeout = 60)
        {
            return parameters == null ? QueryToDataTable(connectionString, providerName, commandText, tableName, commandTimeout) : QueryToDataTable(connectionString, providerName, commandText, tableName, commandTimeout, parameters.ToArray());
        }

        public static DataTable QueryToDataTable(DbConnection connection, string commandText, IEnumerable<object> parameters, string tableName = null, int commandTimeout = 60)
        {
            return parameters == null ? QueryToDataTable(connection, commandText, tableName, commandTimeout) : QueryToDataTable(connection, commandText, tableName, commandTimeout, parameters.ToArray());
        }

        #endregion Query Methods

        #region Single / Scalar Methods

        public dynamic QuerySingle(string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QuerySingle(commandText, commandTimeout) : QuerySingle(commandText, commandTimeout, parameters.ToArray());
        }

        public static dynamic QuerySingle(string connectionString, string providerName, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QuerySingle(connectionString, providerName, commandText, commandTimeout) : QuerySingle(connectionString, providerName, commandText, commandTimeout, parameters.ToArray());
        }

        public dynamic QueryValue(string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryValue(commandText, commandTimeout) : QueryValue(commandText, commandTimeout, parameters.ToArray());
        }

        public static dynamic QueryValue(string connectionString, string providerName, string commandText, IEnumerable<object> parameters, int commandTimeout = 60)
        {
            return parameters == null ? QueryValue(connectionString, providerName, commandText, commandTimeout) : QueryValue(connectionString, providerName, commandText, commandTimeout, parameters.ToArray());
        }

        #endregion Single / Scalar Methods

        #endregion Database class IEnumerable<object> paramaterized method overloads

        #region Connection String Utilities

        public static string CreateSqlConnectionString(string sqlServer
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

        public static string CreatePostgreSqlConnectionString(string host
            , string database
            , string userName = null
            , string password = null
            , bool? disableSsl = null
            , int connectTimeout = -1
            , int port = 5432
            , bool integratedSecurity = false
            )
        {
            var cb = new NpgsqlConnectionStringBuilder { Host = host, Database = database };
            if (!string.IsNullOrWhiteSpace(userName) && !string.IsNullOrWhiteSpace(password))
            {
                cb.Username = userName;
                cb.Password = password;
            }
            else
            {
                if (integratedSecurity)
                {
                    cb.IntegratedSecurity = true;
                }
                else
                {
                    cb.Username = userName;
                    cb.Password = password;
                }
            }
            if (!disableSsl.HasValue) cb.SslMode = SslMode.Disable;
            if (connectTimeout > -1) cb.Timeout = connectTimeout;
            return cb.ConnectionString;
        }

        public static string CreateSqlLiteConnectionString(string databasePath
            , string password = null
            , int defaultTimeout = 60
            , bool failIfMissing = true
            , bool enablePooling = false
            , int maxPoolSize = 100
            , int cacheSize = 2000
            , int pageSize = 1024
            , bool disableJournal = false
            , bool readOnly = false
            )
        {
            // Sqlite connection string examples can be found here: http://www.connectionstrings.com/sqlite-net-provider/
            var d = new List<string> { $@"Data Source={databasePath};Version=3" };
            if (!string.IsNullOrWhiteSpace(password)) d.Add($@"Password={password}");
            if (defaultTimeout != 30) d.Add($@"Default Timeout={defaultTimeout}");
            if (failIfMissing) d.Add(@"FailIfMissing=True");
            if (enablePooling) d.Add($@"Pooling=True;Max Pool Size={maxPoolSize}");
            if (cacheSize != 2000) d.Add($@"Cache Size={cacheSize}");
            if (pageSize != 1024) d.Add($@"Page Size={pageSize}");
            if (disableJournal) d.Add(@"Journal Mode=Off");
            if (readOnly) d.Add(@"Read Only=True");
            return string.Join(";", d);
        }

        public static string CreateSqlLiteMemoryConnectionString()
        {
            return "Data Source=:memory:;Version=3;New=True;";
        }

        public static string CreateOracleConnectionString(string dataSource
            , string userId = null
            , string password = null
            , bool persistSecurityInfo = true
            , int connectTimeout = -1
            , int connectionLifetime = -1
            , bool pooling = false
            , int minPoolSize = 10
            , int maxPoolSize = 100
            , int incrPoolSize = 5
            , int decrPoolSize = 2
            )
        {
            var cb = new OracleConnectionStringBuilder { DataSource = dataSource, PersistSecurityInfo = persistSecurityInfo, UserID = userId, Password = password };
            if (!string.IsNullOrWhiteSpace(userId) && !string.IsNullOrWhiteSpace(password))
            {
                cb.UserID = userId;
                cb.Password = password;
            }
            else
            {
                cb.UserID = "";
            }
            if (connectTimeout > -1) cb.ConnectionTimeout = connectTimeout;
            if (connectionLifetime > -1) cb.ConnectionLifeTime = connectionLifetime;
            if (pooling)
            {
                cb.Pooling = true;
                cb.MinPoolSize = minPoolSize;
                cb.MaxPoolSize = maxPoolSize;
                cb.IncrPoolSize = incrPoolSize;
                cb.DecrPoolSize = decrPoolSize;
            }
            return cb.ConnectionString;
        }

        #endregion Connection String Utilities

        #region Bulk Utility Methods

        public bool ImportSeperatedTxtToSql(
            string tableName,
            int timeOutSeconds,
            string filePathStr,
            string schemaFilePath,
            int batchRowSize,
            bool colHeaders,
            string fieldSeperator,
            string textQualifier,
            string nullValue,
            //String filterExpression,
            bool enableIndentityInsert = false
        )
        {
            var connectionString = Connection.ConnectionString;
            var sqlBulkCopyUtility = new SqlBulkCopyUtility();
            return sqlBulkCopyUtility.ImportSeperatedTxtToSql(
                connectionString,
                tableName,
                timeOutSeconds,
                filePathStr,
                schemaFilePath,
                batchRowSize,
                colHeaders,
                fieldSeperator,
                textQualifier,
                nullValue,
                //filterExpression,
                enableIndentityInsert
                );
        }

        public void QueryToSqlServerBulk(
            string commandText,
            string destConnStr,
            string destTableName,
            int commandTimeout = 60,
            int batchSize = 0,
            int bulkCopyTimeout = 0,
            bool enableIndentityInsert = false, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentException("commandText is NULL!");
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0)
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            using (dbCommand)
            {
                var sqlBulkCopyUtility = new SqlBulkCopyUtility();
                var dataReader = dbCommand.ExecuteReader();
                sqlBulkCopyUtility.SqlServerBulkCopy(ref dataReader, destConnStr, destTableName, batchSize, bulkCopyTimeout, enableIndentityInsert);
            }
        }

        #endregion Bulk Utility Methods

        #region Schema Utilities

        public DataTable FillSchemaDataTable(string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentException("commandText is NULL!");
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0)
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            var providerName = GetConnectionProviderName();
            var da = DbProviderFactories.GetFactory(providerName).CreateDataAdapter();
            // ReSharper disable once PossibleNullReferenceException
            da.SelectCommand = dbCommand;
            var dt = tableName == null ? new DataTable() : new DataTable(tableName);
            da.FillSchema(dt, SchemaType.Source);
            return dt;
        }

        public static DataTable FillSchemaDataTable(string connectionString, string providerName, string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.FillSchemaDataTable(commandText, tableName, commandTimeout, parameters);
            }
        }

        #endregion Schema Utilities

        #region SQL Server Script Utilities

        public static string CreateSqlServer2008BatchSelect(string selectSql, int batchNumber, int batchSize, string rowOrderBy)
        {
            if (selectSql.TrimEnd().EndsWith(";")) selectSql = selectSql.TrimEnd().Substring(0, selectSql.TrimEnd().Length - 1);
            var originalSelect = selectSql.Substring(0, selectSql.IndexOf("from", StringComparison.InvariantCultureIgnoreCase));

            var offset = batchNumber * batchSize;
            var limit = offset + batchSize - 1;

            const string template = @"
WITH [temp_batch_table] AS
(
    $(InputSql)
)
$(originalSelect)
FROM [temp_batch_table]
WHERE [RowNumber] BETWEEN $(Offset) AND $(Limit);
";
            var rowNumberSql = @" ROW_NUMBER() OVER (ORDER BY $(RowOrderBy)) AS RowNumber, ".Replace("$(RowOrderBy)", rowOrderBy);
            var selectIdx = selectSql.IndexOf("select", StringComparison.InvariantCultureIgnoreCase) + "select".Length;
            selectSql = selectSql.Insert(selectIdx, rowNumberSql);
            var outputSql = template.Replace("$(Limit)", limit.ToString(CultureInfo.InvariantCulture))
                .Replace("$(Offset)", offset.ToString(CultureInfo.InvariantCulture))
                .Replace("$(InputSql)", selectSql)
                .Replace("$(originalSelect)", originalSelect);

            return outputSql;
        }

        public static string CreateSqlServer2008CountSelect(string selectSql, int batchSize = 0)
        {
            if (selectSql.TrimEnd().EndsWith(";")) selectSql = selectSql.TrimEnd().Substring(0, selectSql.TrimEnd().Length - 1);

            string template;
            if (batchSize < 1)
            {
                template = @"
SELECT COUNT(*) as [cnt]
FROM
(
    $(InputSql)
) as a
";
            }
            else
            {
                template = @"
SELECT (COUNT(*) / $(BatchSize)) as [cnt]
FROM
(
    $(InputSql)
) as a
".Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture));
            }

            var outputSql = template.Replace("$(InputSql)", selectSql);
            return outputSql;
        }

        #endregion SQL Server Script Utilities

        #region MySql Server Script Utilities

        public static string CreateMySqlBatchSelect(string selectSql, int batchNumber, int batchSize, string rowOrderBy)
        {
            if (selectSql.TrimEnd().EndsWith(";")) selectSql = selectSql.TrimEnd().Substring(0, selectSql.TrimEnd().Length - 1);

            var offset = batchNumber * batchSize;
            const string template = @"
SELECT *
FROM
(
    $(InputSql)
) as a
$(OrderBy)
LIMIT $(BatchSize) OFFSET $(Offset);
";
            //if(!String.IsNullOrWhiteSpace(rowOrderBy))
            var outputSql = template.Replace("$(Offset)", offset.ToString(CultureInfo.InvariantCulture))
                .Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture))
                .Replace("$(InputSql)", selectSql)
                .Replace("$(OrderBy)", !string.IsNullOrWhiteSpace(rowOrderBy) ? $@"ORDER BY {rowOrderBy}" : string.Empty);

            return outputSql;
        }

        public static string CreateMySqlCountSelect(string selectSql, int batchSize = 0)
        {
            string template;
            if (batchSize < 1)
            {
                template = @"
SELECT COUNT(*) as cnt
FROM
(
    $(InputSql)
) as a
";
            }
            else
            {
                template = @"
SELECT (COUNT(*) / $(BatchSize)) as cnt
FROM
(
    $(InputSql)
) as a
".Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture));
            }

            var outputSql = template.Replace("$(InputSql)", selectSql);
            return outputSql;
        }

        #endregion MySql Server Script Utilities

        #region PostgreSql Server Script Utilities

        public static string CreatePostgreSqlBatchSelect(string selectSql, int batchNumber, int batchSize, string rowOrderBy)
        {
            if (selectSql.TrimEnd().EndsWith(";")) selectSql = selectSql.TrimEnd().Substring(0, selectSql.TrimEnd().Length - 1);

            var offset = batchNumber * batchSize;
            const string template = @"
SELECT *
FROM
(
    $(InputSql)
) as a
$(OrderBy)
LIMIT $(BatchSize) OFFSET $(Offset);
";
            //if(!String.IsNullOrWhiteSpace(rowOrderBy))
            var outputSql = template.Replace("$(Offset)", offset.ToString(CultureInfo.InvariantCulture))
                .Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture))
                .Replace("$(InputSql)", selectSql)
                .Replace("$(OrderBy)", !string.IsNullOrWhiteSpace(rowOrderBy) ? $@"ORDER BY {rowOrderBy}" : string.Empty);

            return outputSql;
        }

        public static string CreatePostgreSqlCountSelect(string selectSql, int batchSize = 0)
        {
            string template;
            if (batchSize < 1)
            {
                template = @"
SELECT COUNT(*) as cnt
FROM
(
    $(InputSql)
) as a
";
            }
            else
            {
                template = @"
SELECT (COUNT(*) / $(BatchSize)) as cnt
FROM
(
    $(InputSql)
) as a
".Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture));
            }

            var outputSql = template.Replace("$(InputSql)", selectSql);
            return outputSql;
        }

        #endregion PostgreSql Server Script Utilities

        #region Sqlite Server Script Utilities

        public static string CreateSqliteBatchSelect(string selectSql, int batchNumber, int batchSize, string rowOrderBy)
        {
            if (selectSql.TrimEnd().EndsWith(";")) selectSql = selectSql.TrimEnd().Substring(0, selectSql.TrimEnd().Length - 1);

            var offset = batchNumber * batchSize;
            const string template = @"
SELECT *
FROM
(
    $(InputSql)
) as a
$(OrderBy)
LIMIT $(BatchSize) OFFSET $(Offset);
";
            //if(!String.IsNullOrWhiteSpace(rowOrderBy))
            var outputSql = template.Replace("$(Offset)", offset.ToString(CultureInfo.InvariantCulture))
                .Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture))
                .Replace("$(InputSql)", selectSql)
                .Replace("$(OrderBy)", !string.IsNullOrWhiteSpace(rowOrderBy) ? $@"ORDER BY {rowOrderBy}" : string.Empty);

            return outputSql;
        }

        public static string CreateSqliteCountSelect(string selectSql, int batchSize = 0)
        {
            string template;
            if (batchSize < 1)
            {
                template = @"
SELECT COUNT(*) as cnt
FROM
(
    $(InputSql)
) as a
";
            }
            else
            {
                template = @"
SELECT (COUNT(*) / $(BatchSize)) as cnt
FROM
(
    $(InputSql)
) as a
".Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture));
            }

            var outputSql = template.Replace("$(InputSql)", selectSql);
            return outputSql;
        }

        #endregion Sqlite Server Script Utilities

        #region Oracle Script Utilities

        public static string CreateOracleBatchSelect(string selectSql, int batchNumber, int batchSize, string rowOrderBy)
        {
            if (selectSql.TrimEnd().EndsWith(";")) selectSql = selectSql.TrimEnd().Substring(0, selectSql.TrimEnd().Length - 1);

            var offset = batchNumber * batchSize;
            var limit = offset + batchSize - 1;

            const string template = @"
SELECT *
FROM(
        $(InputSql)
    ) a
WHERE [RowNumber] BETWEEN $(Offset) AND $(Limit);
";

            var rowNumberSql = @" ROW_NUMBER() OVER (ORDER BY $(RowOrderBy)) AS RowNumber, ".Replace("$(RowOrderBy)", rowOrderBy);
            var selectIdx = selectSql.IndexOf("select", StringComparison.InvariantCultureIgnoreCase) + "select".Length;
            selectSql = selectSql.Insert(selectIdx, rowNumberSql);
            var outputSql = template.Replace("$(Limit)", limit.ToString(CultureInfo.InvariantCulture))
                .Replace("$(Offset)", offset.ToString(CultureInfo.InvariantCulture))
                .Replace("$(InputSql)", selectSql);

            return outputSql;
        }

        public static string CreateOracleCountSelect(string selectSql, int batchSize = 0)
        {
            if (selectSql.TrimEnd().EndsWith(";")) selectSql = selectSql.TrimEnd().Substring(0, selectSql.TrimEnd().Length - 1);

            string template;
            if (batchSize < 1)
            {
                template = @"
SELECT COUNT(*) as cnt
FROM
(
    $(InputSql)
) as a
";
            }
            else
            {
                template = @"
SELECT (COUNT(*) / $(BatchSize)) as cnt
FROM
(
    $(InputSql)
) as a
".Replace("$(BatchSize)", batchSize.ToString(CultureInfo.InvariantCulture));
            }

            var outputSql = template.Replace("$(InputSql)", selectSql);
            return outputSql;
        }

        #endregion Oracle Script Utilities

        #region Not Yet Implemented

        //public IEnumerable<dynamic> QueryFromFile(String filePath, IDictionary<String, String> namedArgs = null, String startTag = null, String endTag = null, int commandTimeout = 60, params object[] parameters)
        //{
        //    var commandText = File.ReadAllText(filePath);
        //    if (string.IsNullOrEmpty(commandText))throw new ArgumentException("Command Text from file is null or empty!");
        //    if (namedArgs != null) commandText = commandText.ReplaceNamedParameters(namedArgs);
        //    return QueryInternal(commandText, commandTimeout, parameters).ToList<object>().AsReadOnly();
        //}

        //public IEnumerable<dynamic> QueryFromFileTemplate(String templateName, IDictionary<String, String> namedArgs = null, String startTag = null, String endTag = null, int commandTimeout = 60, params object[] parameters)
        //{
        //    var root = IOUtility.DefaultFolderPath;
        //    var filePath = Path.Combine(root, @"App_Data\Sql", templateName + ".sql");
        //    return QueryFromFile(filePath, namedArgs, startTag, endTag, commandTimeout, parameters);
        //}

        #endregion Not Yet Implemented
    }
}