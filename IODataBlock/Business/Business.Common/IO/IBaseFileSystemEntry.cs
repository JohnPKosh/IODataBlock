using Business.Common.Exceptions;
using System;

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

        IExceptionObjectList EntryReadErrors { get; set; }
    }
}