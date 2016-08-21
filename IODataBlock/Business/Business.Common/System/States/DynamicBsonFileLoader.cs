using Business.Common.Extensions;
using Business.Common.IO;
using System;
using System.IO;

namespace Business.Common.System.States
{
    public class DynamicBsonFileLoader : IDynamicLoader
    {
        #region Class Initialization

        public DynamicBsonFileLoader(string fileName)
        {
            _file = new FileInfo(fileName);
        }

        public DynamicBsonFileLoader(FileInfo file)
        {
            _file = file;
        }

        public DynamicBsonFileLoader(FileEntry fileEntry)
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
            return _file.ReadBsonFile();
        }

        public bool TryLoad(out dynamic value)
        {
            try
            {
                value = Load();
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
            BsonObjectFileInfoSerialization.WriteBsonToFile(value, _file);
        }

        public bool TrySave(dynamic value)
        {
            try
            {
                Save(value);
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