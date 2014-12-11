using System;
using System.IO;
using Business.Common.IO;

namespace Business.Common.Exceptions
{
    public class DirectoryEntry : BaseFileSystemEntry
    {
        #region Class Initialization

        public DirectoryEntry(DirectoryInfo directoryInfo)
            : base(directoryInfo)
        {
            _directoryInfo = directoryInfo;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private DirectoryInfo _directoryInfo;

        public override long Length
        {
            get { return 0; }
        }

        public override string ParentFullName
        {
            get
            {
                try
                {
                    return _directoryInfo.Parent.FullName;
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
                    return _directoryInfo.Root.FullName;
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

        public DirectoryInfo GetDirectory()
        {
            return _directoryInfo;
        }

        #endregion Methods
    }
}