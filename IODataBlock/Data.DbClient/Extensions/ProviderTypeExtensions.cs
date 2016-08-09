using System;
using Data.DbClient.Commands;

namespace Data.DbClient.Extensions
{
    public static  class ProviderTypeExtensions
    {
        public static string GetProviderInvariant(this ProviderType t)
        {
            switch (t)
            {
                case ProviderType.SqlServer:
                    return @"System.Data.SqlClient";
                case ProviderType.Npgsql:
                    return @"Npgsql";
                case ProviderType.MySql:
                    return @"MySql.Data.MySqlClient";
                case ProviderType.Oracle:
                    return @"Oracle.ManagedDataAccess.Client";
                case ProviderType.SQLite:
                    return @"System.Data.SQLite";
                default:
                    return @"System.Data.SqlClient";
            }
        }

        public static ProviderType GetProviderTypeByString(String providerInvariant)
        {
            if (String.IsNullOrWhiteSpace(providerInvariant)) return ProviderType.SqlServer;
            switch (providerInvariant)
            {
                case @"System.Data.SqlClient":
                    return ProviderType.SqlServer;
                case @"Npgsql":
                    return ProviderType.Npgsql;
                case @"MySql.Data.MySqlClient":
                    return ProviderType.MySql;
                case @"Oracle.ManagedDataAccess.Client":
                    return ProviderType.Oracle;
                case @"System.Data.SQLite":
                    return ProviderType.SQLite;
                default:
                    return ProviderType.SqlServer;
            }
        }

    }
}
