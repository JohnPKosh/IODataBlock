// ReSharper disable once CheckNamespace
namespace ExBaseData
{
    internal interface IDbFileHandler
    {
        IConnectionConfiguration GetConnectionConfiguration(string fileName);
    }
}