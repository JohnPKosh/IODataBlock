using System;
using System.IO;
using Business.Common.Extensions;

namespace Business.Common.System.States
{
    public class DynamicJsonFileLoader : IDynamicStateLoader
    {
        public DynamicJsonFileLoader(FileInfo file)
        {
            _file = file;
        }

        private readonly FileInfo _file;

        public dynamic LoadState()
        {
            return _file.ReadJsonFile();
        }

        public bool TryLoadState(out dynamic stateValue)
        {
            try
            {
                stateValue = _file.ReadJsonFile();
                return true;
            }
            catch (Exception)
            {
                stateValue = null;
                return false;
            }
        }

        public void SaveState(dynamic stateValue)
        {
            JsonObjectFileInfoSerialization.WriteJsonToFile(stateValue, _file);
        }

        public bool TrySaveState(dynamic stateValue)
        {
            try
            {
                JsonObjectFileInfoSerialization.WriteJsonToFile(stateValue, _file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}