using System;
using System.IO;
using Business.Utilities.Extensions;

namespace Business.Common.System.States
{
    public class JsonFileLoader : IStateLoader
    {

        public JsonFileLoader(FileInfo file)
        {
            _file = file;
        }

        private readonly FileInfo _file;

        public T LoadState<T>()
        {
            return _file.ReadJsonFile<T>();
        }

        public bool TryLoadState<T>(out T stateValue)
        {
            try
            {
                stateValue = _file.ReadJsonFile<T>();
                return true;
            }
            catch (Exception)
            {
                stateValue = default(T);
                return false;
            }
        }

        public void SaveState<T>(T stateValue)
        {
            stateValue.WriteJsonToFile(_file);
        }


        public bool TrySaveState<T>(T stateValue)
        {
            try
            {
                stateValue.WriteJsonToFile(_file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
