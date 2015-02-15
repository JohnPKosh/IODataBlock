using System;
using System.Collections.Generic;
using System.Data.SqlClient;

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

        public int ExecuteNonQuery(string connectionString, String providerName, String commandText, Int32 commandTimeout = 0, params object[] args)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.Execute(commandText, commandTimeout, args);
            }
        }

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
    }
}