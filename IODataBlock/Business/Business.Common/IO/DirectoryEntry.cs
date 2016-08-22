using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Business.Common.IO
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

        private readonly DirectoryInfo _directoryInfo;

        public override long Length => 0;

        public override string ParentFullName
        {
            get
            {
                try
                {
                    return _directoryInfo.Parent?.FullName;
                }
                catch (Exception ex)
                {
                    EntryReadErrors.Add(ex);
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
                    EntryReadErrors.Add(ex);
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

        public IEnumerable<FileInfo> GetFiles()
        {
            return _directoryInfo.GetFiles().AsEnumerable();
        }

        #endregion Methods
    }
}