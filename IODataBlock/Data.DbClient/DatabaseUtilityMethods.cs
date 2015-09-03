using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Data.DbClient.BulkCopy;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace Data.DbClient
{
    public partial class Database
    {
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



        #region Connection String Utilities

        public static String CreateSqlConnectionString(String sqlServer
            , String databaseName
            , String sqlUserName = null
            , String sqlPassword = null
            , String applicationName = null
            , Int32 connectTimeout = -1
            )
        {
            var cb = new SqlConnectionStringBuilder { DataSource = sqlServer, InitialCatalog = databaseName };
            if (!String.IsNullOrWhiteSpace(sqlUserName) && !String.IsNullOrWhiteSpace(sqlPassword))
            {
                cb.UserID = sqlUserName;
                cb.Password = sqlPassword;
            }
            else
            {
                cb.IntegratedSecurity = true;
            }
            if (!String.IsNullOrWhiteSpace(applicationName)) cb.ApplicationName = applicationName;
            if (connectTimeout > -1) cb.ConnectTimeout = connectTimeout;
            return cb.ConnectionString;
        }

        public static String CreatePostgreSqlConnectionString(String host
            , String database
            , String userName = null
            , String password = null
            , bool? disableSsl = null
            , Int32 connectTimeout = -1
            , Int32 port = 5432
            , bool integratedSecurity = false
            )
        {
            var cb = new NpgsqlConnectionStringBuilder { Host = host, Database = database };
            if (!String.IsNullOrWhiteSpace(userName) && !String.IsNullOrWhiteSpace(password))
            {
                cb.UserName = userName;
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
                    cb.UserName = userName;
                    cb.Password = password;
                }
            }
            if (!disableSsl.HasValue) cb.SslMode = SslMode.Disable;
            if (connectTimeout > -1) cb.Timeout = connectTimeout;
            return cb.ConnectionString;
        }

        public static String CreateSqlLiteConnectionString(String databasePath
            , String password = null
            , Int32 defaultTimeout = 60
            , Boolean failIfMissing = true
            , Boolean enablePooling = false
            , Int32 maxPoolSize = 100
            , Int32 cacheSize = 2000
            , Int32 pageSize = 1024
            , Boolean disableJournal = false
            , Boolean readOnly = false
            )
        {
            // Sqlite connection string examples can be found here: http://www.connectionstrings.com/sqlite-net-provider/
            var d = new List<String> { String.Format(@"Data Source={0};Version=3", databasePath) };
            if (!String.IsNullOrWhiteSpace(password)) d.Add(String.Format(@"Password={0}", password));
            if (defaultTimeout != 30) d.Add(String.Format(@"Default Timeout={0}", defaultTimeout));
            if (failIfMissing) d.Add(@"FailIfMissing=True");
            if (enablePooling) d.Add(String.Format(@"Pooling=True;Max Pool Size={0}", maxPoolSize));
            if (cacheSize != 2000) d.Add(String.Format(@"Cache Size={0}", cacheSize));
            if (pageSize != 1024) d.Add(String.Format(@"Page Size={0}", pageSize));
            if (disableJournal) d.Add(@"Journal Mode=Off");
            if (readOnly) d.Add(@"Read Only=True");
            return String.Join(";", d);
        }

        public static String CreateSqlLiteMemoryConnectionString()
        {
            return "Data Source=:memory:;Version=3;New=True;";
        }

        public static String CreateOracleConnectionString(String dataSource
            , String userId = null
            , String password = null
            , bool persistSecurityInfo = true
            , Int32 connectTimeout = -1
            , Int32 connectionLifetime = -1
            , bool pooling = false
            , Int32 minPoolSize = 10
            , Int32 maxPoolSize = 100
            , Int32 incrPoolSize = 5
            , Int32 decrPoolSize = 2
            )
        {
            var cb = new OracleConnectionStringBuilder { DataSource = dataSource, PersistSecurityInfo = persistSecurityInfo, UserID = userId, Password = password };
            if (!String.IsNullOrWhiteSpace(userId) && !String.IsNullOrWhiteSpace(password))
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

        public Boolean ImportSeperatedTxtToSql(
            String tableName,
            Int32 timeOutSeconds,
            String filePathStr,
            String schemaFilePath,
            Int32 batchRowSize,
            Boolean colHeaders,
            String fieldSeperator,
            String textQualifier,
            String nullValue,
            //String filterExpression,
            Boolean enableIndentityInsert = false
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
            String destConnStr,
            String destTableName,
            int commandTimeout = 60,
            Int32 batchSize = 0,
            Int32 bulkCopyTimeout = 0,
            Boolean enableIndentityInsert = false, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
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



        public DataTable FillSchemaDataTable(string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
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

    }
}