using System.Globalization;
using System.IO;

namespace Data.DbClient.Configuration
{
    internal class SqlServerDbFileHandler : IDbFileHandler
    {
        //private const string SqlServerConnectionStringFormat = "Data Source=.\\SQLEXPRESS;AttachDbFilename={0};Initial Catalog={1};Integrated Security=True;User Instance=True;MultipleActiveResultSets=True";

        //private const string SqlServerProviderName = "System.Data.SqlClient";

        public IConnectionConfiguration GetConnectionConfiguration(string fileName)
        {
            return new ConnectionConfiguration("System.Data.SqlClient", GetConnectionString(fileName, Database.DataDirectory));
        }

        internal static string GetConnectionString(string fileName, string dataDirectory)
        {
            if (!Path.IsPathRooted(fileName))
            {
                var str = string.Concat("|DataDirectory|\\", Path.GetFileName(fileName));
                // ReSharper disable once AssignNullToNotNullAttribute
                var str1 = Path.Combine(dataDirectory, Path.GetFileName(fileName));
                var objArray = new object[2];
                objArray[0] = str;
                objArray[1] = str1;
                return string.Format(CultureInfo.InvariantCulture, "Data Source=.\\SQLEXPRESS;AttachDbFilename={0};Initial Catalog={1};Integrated Security=True;User Instance=True;MultipleActiveResultSets=True", objArray);
            }
            var objArray1 = new object[2];
            objArray1[0] = fileName;
            objArray1[1] = fileName;
            return string.Format(CultureInfo.InvariantCulture, "Data Source=.\\SQLEXPRESS;AttachDbFilename={0};Initial Catalog={1};Integrated Security=True;User Instance=True;MultipleActiveResultSets=True", objArray1);
        }
    }
}