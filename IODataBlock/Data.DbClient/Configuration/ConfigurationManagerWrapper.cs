using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace Data.DbClient.Configuration
{
    internal class ConfigurationManagerWrapper : IConfigurationManager
    {
        private IDictionary<string, string> _appSettings;

        private string _dataDirectory;

        private readonly IDictionary<string, IDbFileHandler> _handlers;

        public IDictionary<string, string> AppSettings
        {
            get
            {
                if (_appSettings == null)
                {
                    var dictionary = this;
                    var strs = ConfigurationManager.AppSettings.Cast<string>();
                    var strs1 = strs.Select(key => key);
                    Func<string, string> func = key => key;
                    dictionary._appSettings = strs1.ToDictionary(func, key => ConfigurationManager.AppSettings[key]);
                }
                return _appSettings;
            }
        }

        public ConfigurationManagerWrapper(IDictionary<string, IDbFileHandler> handlers, string dataDirectory = null)
        {
            var configurationManagerWrapper = this;
            var str = dataDirectory;
            var str1 = str;
            if (str == null)
            {
                str1 = Database.DataDirectory;
            }
            configurationManagerWrapper._dataDirectory = str1;
            _handlers = handlers;
        }

        public IConnectionConfiguration GetConnection(string name)
        {
            return GetConnection(name, GetConnectionConfigurationFromConfig, File.Exists);
        }

        internal IConnectionConfiguration GetConnection(string name, Func<string, IConnectionConfiguration> getConfigConnection, Func<string, bool> fileExists)
        {
            var connectionConfiguration1 = getConfigConnection(name);
            if (connectionConfiguration1 != null) return connectionConfiguration1;
            var strs = _handlers;
            var enumerator = strs.OrderBy(h => h.Key).GetEnumerator();
            using (enumerator)
            {
                while (enumerator.MoveNext())
                {
                    var current = enumerator.Current;
                    var str = Path.Combine(_dataDirectory, string.Concat(name, current.Key));
                    if (!fileExists(str))
                    {
                        continue;
                    }
                    var connectionConfiguration = current.Value.GetConnectionConfiguration(str);
                    return connectionConfiguration;
                }
                return null;
            }
        }

        private static IConnectionConfiguration GetConnectionConfigurationFromConfig(string name)
        {
            var item = ConfigurationManager.ConnectionStrings[name];
            return item == null ? null : new ConnectionConfiguration(item.ProviderName, item.ConnectionString);
        }
    }
}