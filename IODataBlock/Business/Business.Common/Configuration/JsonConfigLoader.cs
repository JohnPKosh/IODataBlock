using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Utilities.Extensions;

namespace Business.Common.Configuration
{
    public class JsonConfigLoader: IConfigLoader
    {

        public JsonConfigLoader(FileInfo file)
        {
            _file = file;
        }

        private readonly FileInfo _file;

        public dynamic LoadConfiguration()
        {
            return _file.ReadJsonFile();
        }

        public bool TryLoadConfiguration(out dynamic config)
        {
            try
            {
                config = _file.ReadJsonFile();
                return true;
            }
            catch (Exception)
            {
                config = null;
                return false;
            }
        }

        public void SaveConfiguration(dynamic config)
        {
            ClassExtensions.WriteJsonToFile(config,_file);
        }

        public bool TrySaveConfiguration(dynamic config)
        {
            try
            {
                ClassExtensions.WriteJsonToFile(config, _file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
