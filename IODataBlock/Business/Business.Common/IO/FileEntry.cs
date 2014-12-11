using System;
using System.IO;
using Business.Common.IO;

namespace Business.Common.Exceptions
{
    public class FileEntry : BaseFileSystemEntry
    {
        #region Class Initialization

        public FileEntry(FileInfo fileInfo)
            : base(fileInfo)
        {
            _fileInfo = fileInfo;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly FileInfo _fileInfo;

        public override long Length
        {
            get
            {
                try
                {
                    return Exists ? _fileInfo.Length : 0;
                }
                catch (Exception ex)
                {
                    EntryReadErrors.Add(new ExceptionBase(ex));
                    return 0;
                }
            }
        }

        public override string ParentFullName
        {
            get
            {
                try
                {
                    return _fileInfo.DirectoryName;
                }
                catch (Exception ex)
                {
                    EntryReadErrors.Add(ExceptionBase.CreateSystemException(ex, "ParentFullName"));
                    return null;
                }
            }
        }

        public override string RootFullName
        {
            get
            {
                try
                {
                    return _fileInfo.Directory != null ? _fileInfo.Directory.Root.FullName : null;
                }
                catch (Exception ex)
                {
                    EntryReadErrors.Add(ExceptionBase.CreateSystemException(ex, "RootFullName"));
                    return null;
                }
            }
        }

        #endregion Fields and Properties

        #region Methods

        public FileInfo GetFileInfo()
        {
            return _fileInfo;
        }

        public DirectoryInfo GetDirectory()
        {
            return _fileInfo.Directory;
        }

        #endregion Methods
    }
}