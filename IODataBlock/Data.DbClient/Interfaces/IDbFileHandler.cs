// ReSharper disable once CheckNamespace
namespace Data.DbClient
{
    internal interface IDbFileHandler
    {
        IConnectionConfiguration GetConnectionConfiguration(string fileName);
    }
}