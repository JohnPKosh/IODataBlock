using System;

//using ExBaseStringUtil;
//using ExBaseIoUtil;
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
    }
}
