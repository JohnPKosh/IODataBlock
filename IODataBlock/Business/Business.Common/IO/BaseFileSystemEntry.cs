using System;
using System.IO;
using System.Linq;
using Business.Common.Exceptions;

namespace Business.Common.IO
{
    public class BaseFileSystemEntry : IBaseFileSystemEntry
    {
        #region Class Initialization

        public BaseFileSystemEntry(FileSystemInfo fileSystemInfo)
        {
            try
            {
                _entryTime = DateTime.Now;
                _entryTimeUtc = DateTime.UtcNow;
                _fileSystemInfo = fileSystemInfo;
            }
            catch (Exception ex)
            {
                AddException(ex);
            }
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly FileSystemInfo _fileSystemInfo;

        public IExceptionObjectList EntryReadErrors { get; set; }

        public virtual long Length
        {
            get
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return 0;
                }
            }
        }

        public string Name
        {
            get
            {
                try
                {
                    return _fileSystemInfo.Name;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return null;
                }
            }
        }

        public string FullName
        {
            get
            {
                try
                {
                    return _fileSystemInfo.FullName;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return null;
                }
            }
        }

        public virtual string ParentFullName
        {
            get
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return null;
                }
            }
        }

        public virtual string RootFullName
        {
            get
            {
                try
                {
                    throw new NotImplementedException();
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return null;
                }
            }
        }

        public string Extension
        {
            get
            {
                try
                {
                    return _fileSystemInfo.Extension;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return null;
                }
            }
        }

        public DateTime CreationTime
        {
            get
            {
                try
                {
                    return _fileSystemInfo.CreationTime;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return DateTime.MinValue;
                }
            }
        }

        public DateTime CreationTimeUtc
        {
            get
            {
                try
                {
                    return _fileSystemInfo.CreationTimeUtc;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return DateTime.MinValue;
                }
            }
        }

        public DateTime LastAccessTime
        {
            get
            {
                try
                {
                    return _fileSystemInfo.LastAccessTime;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return DateTime.MinValue;
                }
            }
        }

        public DateTime LastAccessTimeUtc
        {
            get
            {
                try
                {
                    return _fileSystemInfo.LastAccessTimeUtc;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return DateTime.MinValue;
                }
            }
        }

        public DateTime LastWriteTime
        {
            get
            {
                try
                {
                    return _fileSystemInfo.LastWriteTime;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return DateTime.MinValue;
                }
            }
        }

        public DateTime LastWriteTimeUtc
        {
            get
            {
                try
                {
                    return _fileSystemInfo.LastWriteTimeUtc;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return DateTime.MinValue;
                }
            }
        }

        private readonly DateTime _entryTime;

        public DateTime EntryReadTime
        {
            get { return _entryTime; }
        }

        private readonly DateTime _entryTimeUtc;

        public DateTime EntryReadTimeUtc
        {
            get { return _entryTimeUtc; }
        }

        public bool Exists
        {
            get
            {
                try
                {
                    return _fileSystemInfo.Exists;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return false;
                }
            }
        }

        public bool Directory
        {
            get
            {
                try
                {
                    return (_fileSystemInfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return false;
                }
            }
        }

        public bool Hidden
        {
            get
            {
                try
                {
                    if (!Exists) return false;
                    return (_fileSystemInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return false;
                }
            }
        }

        public bool ReadOnly
        {
            get
            {
                try
                {
                    if (!Exists) return false;
                    return (_fileSystemInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return false;
                }
            }
        }

        public bool System
        {
            get
            {
                try
                {
                    if (!Exists) return false;
                    return (_fileSystemInfo.Attributes & FileAttributes.System) == FileAttributes.System;
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return false;
                }
            }
        }

        public bool HasEntryReadErrors
        {
            get
            {
                try
                {
                    return EntryReadErrors != null && EntryReadErrors.Exceptions.Any();
                }
                catch (Exception ex)
                {
                    AddException(ex);
                    return true;
                }
            }
        }

        #endregion Fields and Properties

        #region Methods

        public void AddException(Exception ex)
        {
            if (EntryReadErrors == null) InitExceptionList();
            if (EntryReadErrors != null) AddException(ex);
        }

        private void InitExceptionList()
        {
            var meta = ExceptionMetaBase.CreateExceptionMeta();
            meta.MemberName = "BaseFileSystemEntry";
            meta.Description = "BaseFileSystemEntry error reading file system entry.";
            EntryReadErrors = new ExceptionObjectListBase(meta);
        }

        #endregion Methods
    }
}