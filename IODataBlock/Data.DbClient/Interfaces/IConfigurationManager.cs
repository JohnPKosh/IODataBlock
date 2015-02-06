using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace ExBaseData
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