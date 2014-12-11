using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Business.Common.Exceptions;

namespace Business.Common.IO
{
    public interface IBaseFileSystemEntry
    {
        long Length { get; }

        string Name { get; }

        string FullName { get; }

        string ParentFullName { get; }

        string RootFullName { get; }

        string Extension { get; }

        DateTime CreationTime { get; }

        DateTime CreationTimeUtc { get; }

        DateTime LastAccessTime { get; }

        DateTime LastAccessTimeUtc { get; }

        DateTime LastWriteTime { get; }

        DateTime LastWriteTimeUtc { get; }

        DateTime EntryReadTime { get; }

        DateTime EntryReadTimeUtc { get; }

        bool Exists { get; }

        bool Directory { get; }

        bool Hidden { get; }

        bool ReadOnly { get; }

        bool System { get; }

        bool HasEntryReadErrors { get; }

        List<ExceptionBase> EntryReadErrors { get; set; }
    }

    public class BaseFileSystemEntry : IBaseFileSystemEntry
    {
        #region Class Initialization

        public BaseFileSystemEntry(FileSystemInfo fileSystemInfo)
        {
            try
            {
                EntryReadErrors = new List<ExceptionBase>();
                _entryTime = DateTime.Now;
                _entryTimeUtc = DateTime.UtcNow;
                this._fileSystemInfo = fileSystemInfo;
            }
            catch (Exception ex)
            {
                EntryReadErrors.Add(new ExceptionBase(ex));
            }
        }

        #endregion Class Initialization

        #region Fields and Properties

        private readonly FileSystemInfo _fileSystemInfo;

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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(ExceptionBase.CreateSystemException(ex, "ParentFullName"));
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
                    EntryReadErrors.Add(ExceptionBase.CreateSystemException(ex, "RootFullName"));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    return (_fileSystemInfo.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden;
                }
                catch (Exception ex)
                {
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    return (_fileSystemInfo.Attributes & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
                }
                catch (Exception ex)
                {
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    return (_fileSystemInfo.Attributes & FileAttributes.System) == FileAttributes.System;
                }
                catch (Exception ex)
                {
                    EntryReadErrors.Add(new ExceptionBase(ex));
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
                    return EntryReadErrors.Any();
                }
                catch (Exception ex)
                {
                    EntryReadErrors.Add(new ExceptionBase(ex));
                    return true;
                }
            }
        }

        public List<ExceptionBase> EntryReadErrors { get; set; }

        #endregion Fields and Properties
    }
}