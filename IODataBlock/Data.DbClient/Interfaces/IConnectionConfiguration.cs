// ReSharper disable once CheckNamespace
namespace ExBaseData
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