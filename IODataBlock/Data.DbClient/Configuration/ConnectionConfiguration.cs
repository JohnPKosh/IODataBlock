namespace Data.DbClient.Configuration
{
    internal class ConnectionConfiguration : IConnectionConfiguration
    {
        public string ConnectionString
        {
            get;
        }

        public IDbProviderFactory ProviderFactory
        {
            get;
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