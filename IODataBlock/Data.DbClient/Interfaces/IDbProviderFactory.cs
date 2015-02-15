using System.Data.Common;

// ReSharper disable once CheckNamespace
namespace Data.DbClient
{
    internal interface IDbProviderFactory
    {
        DbConnection CreateConnection(string connectionString);
    }
}