using System.Data.Common;
using MySql.Data.MySqlClient;

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
                if (_providerName == "MySql.Data.MySqlClient")
                {
                    _providerFactory = new MySqlClientFactory();
                }
                else
                {
                    _providerFactory = DbProviderFactories.GetFactory(_providerName);
                }
            }
            var dbConnection = _providerFactory.CreateConnection();
            // ReSharper disable once PossibleNullReferenceException
            dbConnection.ConnectionString = connectionString;
            return dbConnection;
        }
    }
}