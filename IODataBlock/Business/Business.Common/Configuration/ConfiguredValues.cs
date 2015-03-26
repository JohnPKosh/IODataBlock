using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Common.Configuration
{
    public class ConfiguredValues
    {
        private static ConfiguredValues _instance = new ConfiguredValues();

        private ConfiguredValues() { }

        public static ConfiguredValues Instance
        {
            get { return _instance ?? (_instance = new ConfiguredValues()); }
        }

        private dynamic _config = new ExpandoObject();

        public dynamic Config
        {
            get { return _config; }
            set
            {
                _config = value;
                if (_config != null) IsLoaded = true;
            }
        }

        public bool IsLoaded { get; private set; }

        public void Load(IConfigLoader loader)
        {
            Config = loader.LoadConfiguration();
        }

        public bool TryLoad(IConfigLoader loader)
        {
            dynamic newConfig;
            if (!loader.TryLoadConfiguration(out newConfig)) return false;
            Config = newConfig;
            return true;
        }

        public void Save(IConfigLoader loader)
        {
            loader.SaveConfiguration(_config);
        }

        public bool TrySave(IConfigLoader loader)
        {
            return loader.TrySaveConfiguration(_config);
        }
    }
}
