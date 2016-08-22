using Business.Common.Extensions;
using Business.Common.IO;
using System;
using System.IO;

namespace Business.Common.System.States
{
    public class DynamicJsonFileLoader : IDynamicLoader
    {
        #region Class Initialization

        public DynamicJsonFileLoader(string fileName)
        {
            _file = new FileInfo(fileName);
        }

        public DynamicJsonFileLoader(FileInfo file)
        {
            _file = file;
        }

        public DynamicJsonFileLoader(FileEntry fileEntry)
        {
            _file = fileEntry.GetFileInfo();
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly FileInfo _file;

        #endregion Fields and Properties

        #region IDynamicStateLoader Members

        public dynamic Load()
        {
            return _file.ReadJsonFile();
        }

        public bool TryLoad(out dynamic value)
        {
            try
            {
                value = _file.ReadJsonFile();
                return true;
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
        }

        public void Save(dynamic value)
        {
            JsonObjectFileInfoSerialization.WriteJsonToFile(value, _file);
        }

        public bool TrySave(dynamic value)
        {
            try
            {
                JsonObjectFileInfoSerialization.WriteJsonToFile(value, _file);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        #endregion IDynamicStateLoader Members
    }
}