using System.Data.Common;

// ReSharper disable once CheckNamespace
namespace ExBaseData
{
    internal interface IDbProviderFactory
    {
        DbConnection CreateConnection(string connectionString);
    }
}