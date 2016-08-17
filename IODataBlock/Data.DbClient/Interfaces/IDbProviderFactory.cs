using System.Data.Common;

// ReSharper disable once CheckNamespace
namespace Data.DbClient
{
    public interface IDbProviderFactory
    {
        DbConnection CreateConnection(string connectionString);
    }
}