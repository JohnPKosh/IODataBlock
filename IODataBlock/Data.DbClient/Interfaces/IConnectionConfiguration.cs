// ReSharper disable once CheckNamespace
namespace Data.DbClient
{
    internal interface IConnectionConfiguration
    {
        string ConnectionString
        {
            get;
        }

        IDbProviderFactory ProviderFactory
        {
            get;
        }
    }
}