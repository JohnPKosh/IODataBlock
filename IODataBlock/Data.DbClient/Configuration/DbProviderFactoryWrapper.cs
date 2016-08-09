using System.Data.Common;
using System.Data.SQLite;
using MySql.Data.MySqlClient;
using Npgsql;
using Oracle.ManagedDataAccess.Client;

namespace Data.DbClient.Configuration
{
    internal class DbProviderFactoryWrapper : IDbProviderFactory
    {
        private DbProviderFactory _providerFactory;

        private string _providerName;

        public DbProviderFactoryWrapper(string providerName)
        {
            _providerName = providerName;
        }

        public DbConnection CreateConnection(string connectionString)
        {
            if (string.IsNullOrEmpty(_providerName))
            {
                _providerName = Database.GetDefaultProviderName();
            }
            if (_providerFactory == null)
            {
                /* TODO add DbProviderFactories for Oracle, SQLite and PostreSql */
                // ReSharper disable once ConvertIfStatementToConditionalTernaryExpression
                switch (_providerName)
                {
                    case "MySql.Data.MySqlClient":
                        _providerFactory = new MySqlClientFactory();
                        break;
                    case "Oracle.ManagedDataAccess.Client":
                        _providerFactory = new OracleClientFactory();
                        break;
                    case "System.Data.SQLite":
                        _providerFactory = new SQLiteFactory();
                        break;
                    case "Npgsql":
                        _providerFactory = NpgsqlFactory.Instance;
                        break;
                    default:
                        _providerFactory = DbProviderFactories.GetFactory(_providerName);
                        break;
                }
            }
            var dbConnection = _providerFactory.CreateConnection();
            // ReSharper disable once PossibleNullReferenceException
            dbConnection.ConnectionString = connectionString;
            return dbConnection;
        }
    }
}