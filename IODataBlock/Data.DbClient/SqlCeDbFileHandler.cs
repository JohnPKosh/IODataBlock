using System.Globalization;
using System.IO;
using ExBaseData;

namespace Data.DbClient
{
    internal class SqlCeDbFileHandler : IDbFileHandler
    {
        public IConnectionConfiguration GetConnectionConfiguration(string fileName)
        {
            var defaultProviderName = Database.GetDefaultProviderName();
            var connectionString = GetConnectionString(fileName);
            return new ConnectionConfiguration(defaultProviderName, connectionString);
        }

        public static string GetConnectionString(string fileName)
        {
            if (!Path.IsPathRooted(fileName))
            {
                var str = string.Concat("|DataDirectory|\\", Path.GetFileName(fileName));

                if (Database.IsWebAssembly)
                {
                    return string.Format(CultureInfo.InvariantCulture, "Data Source={0};File Access Retry Timeout=10", str);
                }
                return string.Format(CultureInfo.InvariantCulture, "Data Source={0};", str);
            }
            if (Database.IsWebAssembly)
            {
                return string.Format(CultureInfo.InvariantCulture, "Data Source={0};File Access Retry Timeout=10", fileName);
            }
            return string.Format(CultureInfo.InvariantCulture, "Data Source={0};", fileName);
        }
    }
}