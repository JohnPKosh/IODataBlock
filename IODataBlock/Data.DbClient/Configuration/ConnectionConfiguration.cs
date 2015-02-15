namespace Data.DbClient.Configuration
{
    internal class ConnectionConfiguration : IConnectionConfiguration
    {
        public string ConnectionString
        {
            get;
            private set;
        }

        public IDbProviderFactory ProviderFactory
        {
            get;
            private set;
        }

        internal ConnectionConfiguration(string providerName, string connectionString)
            : this(new DbProviderFactoryWrapper(providerName), connectionString)
        {
        }

        internal ConnectionConfiguration(IDbProviderFactory providerFactory, string connectionString)
        {
            ProviderFactory = providerFactory;
            ConnectionString = connectionString;
        }
    }
}