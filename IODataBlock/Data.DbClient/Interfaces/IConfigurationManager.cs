using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace Data.DbClient
{
    internal interface IConfigurationManager
    {
        IDictionary<string, string> AppSettings
        {
            get;
        }

        IConnectionConfiguration GetConnection(string name);
    }
}