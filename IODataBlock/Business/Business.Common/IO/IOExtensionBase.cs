﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading;

namespace Business.Common.IO
{
    // ReSharper disable once InconsistentNaming
    public static class IOExtensionBase
    {
        // move to helper class

        #region DriveInfo Extensions and Properties

        public static IEnumerable<DriveInfo> MyDrives => DriveInfo.GetDrives().AsEnumerable();

        public static IEnumerable<DriveInfo> MyCdDrives
        {
            get
            {
                return DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.CDRom).AsEnumerable();
            }
        }

        public static IEnumerable<DriveInfo> MyFixedDrives
        {
            get
            {
                return DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Fixed).AsEnumerable();
            }
        }

        public static IEnumerable<DriveInfo> MyNetworkDrives
        {
            get
            {
                return DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Network).AsEnumerable();
            }
        }

        public static IEnumerable<DriveInfo> MyRemovableDrives
        {
            get
            {
                return DriveInfo.GetDrives().Where(d => d.DriveType == DriveType.Removable).AsEnumerable();
            }
        }

        public static DriveInfo MySystemDrive => new DriveInfo(Environment.GetFolderPath(Environment.SpecialFolder.System).Substring(0, 1));

        public static string RootDirectoryString(this DriveInfo drive)
        {
            return drive.RootDirectory.ToString();
        }

        public static double TotalSizeInMb(this DriveInfo drive)
        {
            return drive.TotalSize / 1048576.0;
        }

        public static double TotalSizeInGb(this DriveInfo drive)
        {
            return drive.TotalSize / 1073741824.0;
        }

        public static double AvailableFreeSpaceInMb(this DriveInfo drive)
        {
            return drive.AvailableFreeSpace / 1048576.0;
        }

        public static double AvailableFreeSpaceInGb(this DriveInfo drive)
        {
            return drive.AvailableFreeSpace / 1073741824.0;
        }

        public static double TotalFreeSpaceInMb(this DriveInfo drive)
        {
            return drive.TotalFreeSpace / 1048576.0;
        }

        public static double TotalFreeSpaceInGb(this DriveInfo drive)
        {
            return drive.TotalFreeSpace / 1073741824.0;
        }

        public static string TotalSizeInMbAsString(this DriveInfo drive)
        {
            return (drive.TotalSize / 1048576.0).ToString("#,###.# MB");
        }

        public static string TotalSizeInGbAsString(this DriveInfo drive)
        {
            return (drive.TotalSize / 1073741824.0).ToString("#,###.## GB");
        }

        public static string AvailableFreeSpaceInMbAsString(this DriveInfo drive)
        {
            return (drive.AvailableFreeSpace / 1048576.0).ToString("#,###.# MB");
        }

        public static string AvailableFreeSpaceInGbAsString(this DriveInfo drive)
        {
            return (drive.AvailableFreeSpace / 1073741824.0).ToString("#,###.## GB");
        }

        public static string TotalFreeSpaceInMbAsString(this DriveInfo drive)
        {
            return (drive.TotalFreeSpace / 1048576.0).ToString("#,###.# MB");
        }

        public static string TotalFreeSpaceInGbAsString(this DriveInfo drive)
        {
            return (drive.TotalFreeSpace / 1073741824.0).ToString("#,###.## GB");
        }

        public static double PercentAvailableFreeSpace(this DriveInfo drive)
        {
            return drive.AvailableFreeSpace / (double)drive.TotalSize;
        }

        public static IEnumerable<DirectoryInfo> Directories(this DriveInfo drive)
        {
            return drive.RootDirectory.GetDirectories().AsEnumerable();
        }

        public static IEnumerable<DirectoryInfo> Directories(this DriveInfo drive, Func<DirectoryInfo, bool> predicate)
        {
            return drive.RootDirectory.GetDirectories().Where(predicate);
        }

        public static IEnumerable<DirectoryInfo> Directories(this DriveInfo drive, string searchPattern)
        {
            return drive.RootDirectory.GetDirectories(searchPattern).AsEnumerable();
        }

        public static IEnumerable<DirectoryInfo> Directories(this DriveInfo drive, string searchPattern, Func<DirectoryInfo, bool> predicate)
        {
            return drive.RootDirectory.GetDirectories(searchPattern).Where(predicate);
        }

        public static IEnumerable<DirectoryInfo> Directories(this DriveInfo drive, string searchPattern, SearchOption option)
        {
            return drive.RootDirectory.GetDirectories(searchPattern, option).AsEnumerable();
        }

        public static IEnumerable<DirectoryInfo> Directories(this DriveInfo drive, string searchPattern, SearchOption option, Func<DirectoryInfo, bool> predicate)
        {
            return drive.RootDirectory.GetDirectories(searchPattern, option).Where(predicate);
        }

        #endregion DriveInfo Extensions and Properties

        #region DirectoryInfo Extensions

        #region Conversion Extensions

        public static string ToDirectoryPath(this string path)
        {
            if (path == null) throw new NullReferenceException();
            path = path.Trim();
            if (!Path.HasExtension(path) && !(path.EndsWith(Path.DirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)) || path.EndsWith(Path.AltDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture)))) path += Path.DirectorySeparatorChar;
            var s = Path.GetDirectoryName(path);
            return s;
        }

        public static string ToFilePath(this string path)
        {
            if (path == null) throw new NullReferenceException();
            return Path.GetFullPath(path.Trim());
        }

        public static bool DirectoryExists(this string path)
        {
            return new DirectoryInfo(path.ToDirectoryPath()).Exists;
        }

        public static bool FileExists(this string path)
        {
            return new FileInfo(path.ToFilePath()).Exists;
        }

        public static DirectoryInfo ToDirectoryInfo(this string path)
        {
            return new DirectoryInfo(path.ToDirectoryPath());
        }

        public static DirectoryInfo ToDirectoryInfo(this string path, bool createDirectory)
        {
            return path.ToDirectoryInfo(createDirectory, false);
        }

        public static DirectoryInfo ToDirectoryInfo(this string path, bool createDirectory, bool overwrite)
        {
            var d = new DirectoryInfo(path.ToDirectoryPath());
            if (!createDirectory) return d;
            var exists = d.Exists;
            if (overwrite && exists)
            {
                d.Delete(true);
                d.Create();
                d.Refresh();
                return d;
            }
            if (exists) return d;
            d.Create();
            d.Refresh();
            return d;
        }

        public static IEnumerable<DirectoryInfo> ToDirectoryInfo(this ICollection<string> paths)
        {
            return paths.Select(ToDirectoryInfo);
        }

        public static void Create(this DirectoryInfo directory, bool overWrite)
        {
            var exists = directory.Exists;
            if (overWrite && exists)
            {
                directory.Delete(true);
                directory.Create();
                directory.Refresh();
            }
            else if (!exists)
            {
                directory.Create();
                directory.Refresh();
            }
        }

        public static FileInfo ToFileInfo(this string path)
        {
            return new FileInfo(path.ToFilePath());
        }

        public static void AppendTextFile(this FileInfo fileinfo, string textToWrite, int waitDelay = 60, Encoding fileEncoding = null)
        {
            FileStream fs = null;
            try
            {
                var endwait = DateTime.Now.AddSeconds(waitDelay);
                fs = fileinfo.Open(FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
                fileinfo.Refresh();
                while (!fs.CanWrite && DateTime.Now < endwait)
                {
                    Thread.Sleep(100);
                    fileinfo.Refresh();
                }
                if (fileEncoding != null)
                {
                    using (var sw = new StreamWriter(fs, fileEncoding))
                    {
                        sw.Write(textToWrite);
                        sw.Flush();
                    }
                }
                else
                {
                    using (var sw = new StreamWriter(fs))
                    {
                        sw.Write(textToWrite);
                        sw.Flush();
                    }
                }
            }
            finally
            {
                fs?.Close();
            }
        }

        #endregion Conversion Extensions

        #region Create Directory Extensions

        public static DirectoryInfo CreateSubDirectory(this DirectoryInfo directory, string subDirectory, bool overWrite)
        {
            if (overWrite && directory.Exists) directory.Delete(true);
            return directory.CreateSubdirectory(subDirectory);
        }

        public static IEnumerable<DirectoryInfo> CreateSubDirectory(this IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, bool> predicate, string subDirectory)
        {
            return directories.Where(predicate).Select(directory => directory.CreateSubdirectory(subDirectory));
        }

        public static IEnumerable<DirectoryInfo> CreateSubDirectory(this IEnumerable<DirectoryInfo> directories, IEnumerable<string> subDirectoryList)
        {
            return directories.SelectMany(directory => subDirectoryList.Select(directory.CreateSubdirectory));
        }

        public static IEnumerable<DirectoryInfo> CreateSubDirectory(this IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, bool> predicate, IEnumerable<string> subDirectoryList)
        {
            return directories.Where(predicate).SelectMany(directory => subDirectoryList.Select(directory.CreateSubdirectory));
        }

        public static IEnumerable<DirectoryInfo> CreateSubDirectory(this IEnumerable<DirectoryInfo> directories, string subDirectory, bool overWrite)
        {
            foreach (var directory in directories)
            {
                DirectoryInfo d;
                if (overWrite)
                {
                    d = new DirectoryInfo(Path.Combine(directory.FullName, subDirectory));
                    if (d.Exists) d.Delete(true);
                }
                d = directory.CreateSubdirectory(subDirectory);
                yield return d;
            }
        }

        public static IEnumerable<DirectoryInfo> CreateSubDirectory(this IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, bool> predicate, string subDirectory, bool overWrite)
        {
            foreach (var directory in directories.Where(predicate))
            {
                DirectoryInfo d;
                if (overWrite)
                {
                    d = new DirectoryInfo(Path.Combine(directory.FullName, subDirectory));
                    if (d.Exists) d.Delete(true);
                }
                d = directory.CreateSubdirectory(subDirectory);
                yield return d;
            }
        }

        public static IEnumerable<DirectoryInfo> CreateSubDirectories(this DirectoryInfo directory, IEnumerable<string> subDirectoryList)
        {
            if (!directory.Exists) yield break;
            foreach (var d in subDirectoryList.Select(directory.CreateSubdirectory))
            {
                yield return d;
            }
        }

        public static IEnumerable<DirectoryInfo> CreateSubDirectories(this DirectoryInfo directory, IEnumerable<string> subDirectoryList, bool overwrite)
        {
            if (!directory.Exists) yield break;
            foreach (var sd in subDirectoryList)
            {
                DirectoryInfo d;
                if (overwrite)
                {
                    d = new DirectoryInfo(Path.Combine(directory.FullName, sd));
                    if (d.Exists) d.Delete(true);
                }
                d = directory.CreateSubdirectory(sd);
                yield return d;
            }
        }

        #endregion Create Directory Extensions

        #region Delete Directory Extensions

        public static void DeleteDirectory(this DirectoryInfo directory, bool recursive, bool continueOnError)
        {
            try
            {
                if (directory.Exists) Directory.Delete(directory.FullName);
            }
            catch (Exception)
            {
                if (continueOnError) return;
                throw;
            }
        }

        public static IEnumerable<string> DeleteSubDirectoriesEnumerable(this DirectoryInfo directory, bool recursive, bool continueOnError)
        {
            foreach (var d in directory.GetDirectories())
            {
                try
                {
                    if (directory.Exists) d.Delete(recursive);
                    else continue;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return d.FullName;
            }
        }

        public static IEnumerable<string> DeleteSubDirectoriesEnumerable(this DirectoryInfo directory, bool recursive, string searchPattern, bool continueOnError)
        {
            foreach (var d in directory.GetDirectories(searchPattern))
            {
                try
                {
                    if (d.Exists) d.Delete(recursive);
                    else continue;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return d.FullName;
            }
        }

        public static IEnumerable<string> DeleteSubDirectoriesEnumerable(this DirectoryInfo directory, bool recursive, string searchPattern, SearchOption searchOption, bool continueOnError)
        {
            foreach (var d in directory.GetDirectories(searchPattern, searchOption))
            {
                try
                {
                    if (d.Exists) d.Delete(recursive);
                    else continue;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return d.FullName;
            }
        }

        public static IEnumerable<string> DeleteSubDirectoriesEnumerable(this DirectoryInfo directory, bool recursive, SearchOption searchOption, bool continueOnError)
        {
            foreach (var d in directory.GetDirectories("*", searchOption))
            {
                try
                {
                    if (d.Exists) d.Delete(recursive);
                    else continue;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return d.FullName;
            }
        }

        public static IEnumerable<string> DeleteSubDirectoriesEnumerable(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate, bool recursive, bool continueOnError)
        {
            foreach (var d in directory.GetDirectories().Where(predicate))
            {
                try
                {
                    if (directory.Exists) d.Delete(recursive);
                    else continue;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return d.FullName;
            }
        }

        public static IEnumerable<string> DeleteSubDirectoriesEnumerable(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate, bool recursive, string searchPattern, bool continueOnError)
        {
            foreach (var d in directory.GetDirectories(searchPattern).Where(predicate))
            {
                try
                {
                    if (d.Exists) d.Delete(recursive);
                    else continue;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return d.FullName;
            }
        }

        public static IEnumerable<string> DeleteSubDirectoriesEnumerable(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate, bool recursive, string searchPattern, SearchOption searchOption, bool continueOnError)
        {
            foreach (var d in directory.GetDirectories(searchPattern, searchOption).Where(predicate))
            {
                try
                {
                    if (d.Exists) d.Delete(recursive);
                    else continue;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return d.FullName;
            }
        }

        public static IEnumerable<string> DeleteSubDirectoriesEnumerable(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate, bool recursive, SearchOption searchOption, bool continueOnError)
        {
            foreach (var d in directory.GetDirectories("*", searchOption).Where(predicate))
            {
                try
                {
                    if (d.Exists) d.Delete(recursive);
                    else continue;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return d.FullName;
            }
        }

        #endregion Delete Directory Extensions

        #region Get Directory Extensions

        public static IEnumerable<DirectoryInfo> GetDirectoriesEnumerable(this DirectoryInfo directory)
        {
            return directory.GetDirectories().AsEnumerable();
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesEnumerable(this DirectoryInfo directory, string searchPattern)
        {
            return directory.GetDirectories(searchPattern).AsEnumerable();
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesEnumerable(this DirectoryInfo directory, string searchPattern, SearchOption searchOption)
        {
            return directory.GetDirectories(searchPattern, searchOption).AsEnumerable();
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesEnumerable(this DirectoryInfo directory, SearchOption searchOption)
        {
            return directory.GetDirectories("*", searchOption).AsEnumerable();
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesEnumerable(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate)
        {
            return directory.GetDirectories().Where(predicate);
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesEnumerable(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate, string searchPattern)
        {
            return directory.GetDirectories(searchPattern).Where(predicate);
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesEnumerable(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate, string searchPattern, SearchOption searchOption)
        {
            return directory.GetDirectories(searchPattern, searchOption).Where(predicate);
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesEnumerable(this DirectoryInfo directory, Func<DirectoryInfo, bool> predicate, SearchOption searchOption)
        {
            return directory.GetDirectories("*", searchOption).Where(predicate);
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesFromEnumerable(this IEnumerable<DirectoryInfo> directories)
        {
            return directories.SelectMany(directory => directory.GetDirectories());
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesFromEnumerable(this IEnumerable<DirectoryInfo> directories, string searchPattern)
        {
            return directories.SelectMany(directory => directory.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly));
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesFromEnumerable(this IEnumerable<DirectoryInfo> directories, string searchPattern, SearchOption searchOption)
        {
            return directories.SelectMany(directory => directory.GetDirectories(searchPattern, searchOption));
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesFromEnumerable(this IEnumerable<DirectoryInfo> directories, SearchOption searchOption)
        {
            return directories.SelectMany(directory => directory.GetDirectories("*", searchOption));
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, bool> predicate)
        {
            return directories.SelectMany(directory => directory.GetDirectories().Where(predicate));
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, bool> predicate, string searchPattern)
        {
            return directories.SelectMany(directory => directory.GetDirectories(searchPattern, SearchOption.TopDirectoryOnly).Where(predicate));
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, bool> predicate, string searchPattern, SearchOption searchOption)
        {
            return directories.SelectMany(directory => directory.GetDirectories(searchPattern, searchOption).Where(predicate));
        }

        public static IEnumerable<DirectoryInfo> GetDirectoriesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<DirectoryInfo, bool> predicate, SearchOption searchOption)
        {
            return directories.SelectMany(directory => directory.GetDirectories("*", searchOption).Where(predicate));
        }

        public static IEnumerable<string> GetDirectoryFullNames(this IEnumerable<DirectoryInfo> directories)
        {
            return directories.Select(directory => directory.FullName);
        }

        #endregion Get Directory Extensions

        #region Move Directory Extensions

        public static IEnumerable<string> MoveDirectoriesFromEnumerable(this IEnumerable<DirectoryInfo> directories, string destinationPathRoot, bool continueOnError)
        {
            foreach (var subdirectory in directories.SelectMany(directory => directory.GetDirectories()))
            {
                string p;
                try
                {
                    p = Path.Combine(destinationPathRoot, subdirectory.Name);  // Add is NullOrEmpty condition for DestinationPathRoot
                    if (!new DirectoryInfo(p).Exists) subdirectory.MoveTo(p);
                    else continue;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return p;
            }
        }

        #endregion Move Directory Extensions

        #endregion DirectoryInfo Extensions

        #region FileStream Extensions

        public static void AcquireFileLock(this FileStream stream, DateTime ioWaitForLockWaitUntil)
        {
            while (true)
            {
                try
                {
                    stream.Lock(0, stream.Length);
                    break;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= ioWaitForLockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static void ReleaseFileLock(this FileStream stream)
        {
            try
            {
                stream.Unlock(0, stream.Length);
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception) { }
        }

        #endregion FileStream Extensions

        #region FileInfo Extensions

        public static StreamWriter AppendText(this FileInfo file, int lockWaitMs)
        {
            return file.AppendText(lockWaitMs, false);
        }

        public static StreamWriter AppendText(this FileInfo file, int lockWaitMs, bool lockStream)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    var sw = file.AppendText();
                    if (lockStream) ((FileStream)sw.BaseStream).Lock(0, sw.BaseStream.Length);
                    return sw;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileInfo CopyTo(this FileInfo file, string destFileName, int lockWaitMs)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    return file.CopyTo(destFileName);
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileInfo CopyTo(this FileInfo file, string destFileName, bool overwrite, int lockWaitMs)
        {
            File.Copy(file.FullName, destFileName, true);

            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    return file.CopyTo(destFileName, overwrite);
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileInfo Decrypt(this FileInfo file, int lockWaitMs)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    file.Decrypt();
                    file.Refresh();
                    return file;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileInfo DeleteFile(this FileInfo file)
        {
            return DeleteFile(file, 120000);
        }

        public static FileInfo DeleteFile(this FileInfo file, int lockWaitMs)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    file.Delete();
                    file.Refresh();
                    return file;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        //public static FileInfo DeleteFile(this FileInfo file, Int32 lockWaitMs, Boolean deleteToRecycleBin)
        //{
        //    var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
        //    while (true)
        //    {
        //        try
        //        {
        //            if (deleteToRecycleBin) FileSystem.DeleteFile(file.FullName, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
        //            else file.Delete();
        //            file.Refresh();
        //            return file;
        //        }
        //        catch (IOException ex)
        //        {
        //            if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
        //            Thread.Sleep(500);
        //        }
        //    }
        //}

        public static FileInfo Encrypt(this FileInfo file, int lockWaitMs)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    file.Encrypt();
                    file.Refresh();
                    return file;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileStream OpenRead(this FileInfo file, int lockWaitMs, bool lockStream)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    var stream = file.OpenRead();
                    if (lockStream) stream.Lock(0, stream.Length);
                    return stream;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static StreamReader OpenText(this FileInfo file, int lockWaitMs, bool lockStream)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    var sr = file.OpenText();
                    if (lockStream) ((FileStream)sr.BaseStream).Lock(0, sr.BaseStream.Length);
                    return sr;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileStream OpenWrite(this FileInfo file, int lockWaitMs, bool lockStream)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    var stream = file.OpenWrite();
                    if (lockStream) stream.Lock(0, stream.Length);
                    return stream;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileStream OpenFileStream(this FileInfo file, FileMode mode, int lockWaitMs, bool lockStream)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    var stream = new FileStream(file.FullName, mode);
                    if (lockStream) stream.Lock(0, stream.Length);
                    return stream;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileStream OpenFileStream(this FileInfo file, FileMode mode, FileAccess access, int lockWaitMs, bool lockStream)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    var stream = new FileStream(file.FullName, mode, access);
                    if (lockStream) stream.Lock(0, stream.Length);
                    return stream;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileStream OpenFileStream(this FileInfo file, FileMode mode, FileAccess access, FileShare share, int lockWaitMs, bool lockStream)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    var stream = new FileStream(file.FullName, mode, access, share);
                    if (lockStream) stream.Lock(0, stream.Length);
                    return stream;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static FileStream OpenFileStream(this FileInfo file, FileMode mode, FileAccess access, FileShare share, int bufferSize, int lockWaitMs, bool lockStream)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    var stream = new FileStream(file.FullName, mode, access, share, bufferSize);
                    if (lockStream) stream.Lock(0, stream.Length);
                    return stream;
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        //public static FileStream OpenFileStream(this FileInfo file, FileMode mode, FileAccess access, FileShare share, Int32 bufferSize, Int32 LockWaitMs, Boolean LockStream)
        //{
        //    var LockWaitUntil = DateTime.Now.AddMilliseconds(LockWaitMs);
        //    while (true)
        //    {
        //        try
        //        {
        //            var stream = new FileStream(file.FullName, mode, access, share, bufferSize);
        //            if (LockStream) stream.Lock(0, stream.Length);
        //            return stream;
        //        }
        //        catch (IOException ex)
        //        {
        //            if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= LockWaitUntil) throw;
        //            else System.Threading.Thread.Sleep(500);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public static FileInfo MoveTo(this FileInfo file, string destFileName, int lockWaitMs)
        {
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                try
                {
                    file.MoveTo(destFileName);
                    file.Refresh();
                    return new FileInfo(destFileName);
                }
                catch (IOException ex)
                {
                    if (!ex.Message.Contains(@"The process cannot access the file") || DateTime.Now >= lockWaitUntil) throw;
                    Thread.Sleep(500);
                }
            }
        }

        public static void WaitForTempLock(this FileInfo file, int lockWaitMs)
        {
            if (file.Directory == null || !file.Directory.Exists) return;
            var templock = new FileInfo(file.FullName + ".tlock");
            var lockWaitUntil = DateTime.Now.AddMilliseconds(lockWaitMs);
            while (true)
            {
                if (!templock.Exists) return;
                if (DateTime.Now <= lockWaitUntil)
                {
                    Thread.Sleep(500);
                }
                else
                {
                    throw new IOException($"A Temporary File Lock Exists for {file.FullName}. ");
                }
            }
        }

        public static string GetKbString(this FileInfo file)
        {
            return file.Length.DisplayKb();
        }

        public static string GetMbString(this FileInfo file)
        {
            return file.Length.DisplayMb();
        }

        public static string GetGbString(this FileInfo file)
        {
            return file.Length.DisplayGb();
        }

        public static string GetGbMbKbString(this FileInfo file)
        {
            return file.Length.DisplayGbMbKb();
        }

        public static string GetMimeType(this FileInfo fileInfo)
        {
            var mime = "application/octetstream";
            var ext = Path.GetExtension(fileInfo.Name).ToLower();
            var rk = Registry.ClassesRoot.OpenSubKey(ext);
            if (rk?.GetValue("Content Type") != null)
            {
                mime = rk.GetValue("Content Type").ToString();
            }
            return mime;
        }

        #region Get Files

        public static IEnumerable<FileInfo> GetFilesEnumerable(this DirectoryInfo directory)
        {
            return directory.GetFiles().AsEnumerable();
        }

        public static IEnumerable<FileInfo> GetFilesEnumerable(this DirectoryInfo directory, string searchPattern)
        {
            return directory.GetFiles(searchPattern).AsEnumerable();
        }

        public static IEnumerable<FileInfo> GetFilesEnumerable(this DirectoryInfo directory, string searchPattern, SearchOption searchOption)
        {
            return directory.GetFiles(searchPattern, searchOption).AsEnumerable();
        }

        public static IEnumerable<FileInfo> GetFilesEnumerable(this DirectoryInfo directory, SearchOption searchOption)
        {
            return directory.GetFiles("*", searchOption).AsEnumerable();
        }

        public static IEnumerable<FileInfo> GetFilesEnumerable(this DirectoryInfo directory, Func<FileInfo, bool> predicate)
        {
            return directory.GetFiles().Where(predicate);
        }

        public static IEnumerable<FileInfo> GetFilesEnumerable(this DirectoryInfo directory, Func<FileInfo, bool> predicate, string searchPattern)
        {
            return directory.GetFiles(searchPattern).Where(predicate);
        }

        public static IEnumerable<FileInfo> GetFilesEnumerable(this DirectoryInfo directory, Func<FileInfo, bool> predicate, string searchPattern, SearchOption searchOption)
        {
            return directory.GetFiles(searchPattern, searchOption).Where(predicate);
        }

        public static IEnumerable<FileInfo> GetFilesEnumerable(this DirectoryInfo directory, Func<FileInfo, bool> predicate, SearchOption searchOption)
        {
            return directory.GetFiles("*", searchOption).Where(predicate);
        }

        public static IEnumerable<FileInfo> GetFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories)
        {
            return directories.SelectMany(directory => directory.GetFiles());
        }

        public static IEnumerable<FileInfo> GetFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, string searchPattern)
        {
            return directories.SelectMany(directory => directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly));
        }

        public static IEnumerable<FileInfo> GetFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, string searchPattern, SearchOption searchOption)
        {
            return directories.SelectMany(directory => directory.GetFiles(searchPattern, searchOption));
        }

        public static IEnumerable<FileInfo> GetFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, SearchOption searchOption)
        {
            return directories.SelectMany(directory => directory.GetFiles("*", searchOption));
        }

        public static IEnumerable<FileInfo> GetFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<FileInfo, bool> predicate)
        {
            return directories.SelectMany(directory => directory.GetFiles().Where(predicate));
        }

        public static IEnumerable<FileInfo> GetFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<FileInfo, bool> predicate, string searchPattern)
        {
            return directories.SelectMany(directory => directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly).Where(predicate));
        }

        public static IEnumerable<FileInfo> GetFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<FileInfo, bool> predicate, string searchPattern, SearchOption searchOption)
        {
            return directories.SelectMany(directory => directory.GetFiles(searchPattern, searchOption).Where(predicate));
        }

        public static IEnumerable<FileInfo> GetFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<FileInfo, bool> predicate, SearchOption searchOption)
        {
            return directories.SelectMany(directory => directory.GetFiles("*", searchOption).Where(predicate));
        }

        #endregion Get Files

        #region Delete Files

        // remove df strings

        public static IEnumerable<string> DeleteFiles(this DirectoryInfo directory, bool continueOnError)
        {
            foreach (var f in directory.GetFiles())
            {
                string df;
                try
                {
                    if (f.Exists) f.Delete();
                    else continue;
                    df = f.FullName;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return df;
            }
        }

        public static IEnumerable<string> DeleteFiles(this DirectoryInfo directory, string searchPattern, bool continueOnError)
        {
            foreach (var f in directory.GetFiles(searchPattern))
            {
                string df;
                try
                {
                    if (f.Exists) f.Delete();
                    else continue;
                    df = f.FullName;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return df;
            }
        }

        public static IEnumerable<string> DeleteFiles(this DirectoryInfo directory, string searchPattern, SearchOption searchOption, bool continueOnError)
        {
            foreach (var f in directory.GetFiles(searchPattern, searchOption))
            {
                string df;
                try
                {
                    if (f.Exists) f.Delete();
                    else continue;
                    df = f.FullName;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return df;
            }
        }

        public static IEnumerable<string> DeleteFiles(this DirectoryInfo directory, SearchOption searchOption, bool continueOnError)
        {
            foreach (var f in directory.GetFiles("*", searchOption))
            {
                string df;
                try
                {
                    if (f.Exists) f.Delete();
                    else continue;
                    df = f.FullName;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return df;
            }
        }

        public static IEnumerable<string> DeleteFiles(this DirectoryInfo directory, Func<FileInfo, bool> predicate, bool continueOnError)
        {
            foreach (var f in directory.GetFiles().Where(predicate))
            {
                string df;
                try
                {
                    if (f.Exists) f.Delete();
                    else continue;
                    df = f.FullName;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return df;
            }
        }

        public static IEnumerable<string> DeleteFiles(this DirectoryInfo directory, Func<FileInfo, bool> predicate, string searchPattern, bool continueOnError)
        {
            foreach (var f in directory.GetFiles(searchPattern).Where(predicate))
            {
                string df;
                try
                {
                    if (f.Exists) f.Delete();
                    else continue;
                    df = f.FullName;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return df;
            }
        }

        public static IEnumerable<string> DeleteFiles(this DirectoryInfo directory, Func<FileInfo, bool> predicate, string searchPattern, SearchOption searchOption, bool continueOnError)
        {
            foreach (var f in directory.GetFiles(searchPattern, searchOption).Where(predicate))
            {
                string df;
                try
                {
                    if (f.Exists) f.Delete();
                    else continue;
                    df = f.FullName;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return df;
            }
        }

        public static IEnumerable<string> DeleteFiles(this DirectoryInfo directory, Func<FileInfo, bool> predicate, SearchOption searchOption, bool continueOnError)
        {
            foreach (var f in directory.GetFiles("*", searchOption).Where(predicate))
            {
                string df;
                try
                {
                    if (f.Exists) f.Delete();
                    else continue;
                    df = f.FullName;
                }
                catch (Exception)
                {
                    if (continueOnError) continue;
                    throw;
                }
                yield return df;
            }
        }

        public static IEnumerable<string> DeleteFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, bool continueOnError)
        {
            foreach (var directory in directories)
            {
                foreach (var file in directory.GetFiles())
                {
                    string df;
                    try
                    {
                        if (file.Exists) file.Delete();
                        else continue;
                        df = file.FullName;
                    }
                    catch (Exception)
                    {
                        if (continueOnError) continue;
                        throw;
                    }
                    yield return df;
                }
            }
        }

        public static IEnumerable<string> DeleteFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, string searchPattern, bool continueOnError)
        {
            foreach (var directory in directories)
            {
                foreach (var file in directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly))
                {
                    string df;
                    try
                    {
                        if (file.Exists) file.Delete();
                        else continue;
                        df = file.FullName;
                    }
                    catch (Exception)
                    {
                        if (continueOnError) continue;
                        throw;
                    }
                    yield return df;
                }
            }
        }

        public static IEnumerable<string> DeleteFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, string searchPattern, SearchOption searchOption, bool continueOnError)
        {
            foreach (var directory in directories)
            {
                foreach (var file in directory.GetFiles(searchPattern, searchOption))
                {
                    string df;
                    try
                    {
                        if (file.Exists) file.Delete();
                        else continue;
                        df = file.FullName;
                    }
                    catch (Exception)
                    {
                        if (continueOnError) continue;
                        throw;
                    }
                    yield return df;
                }
            }
        }

        public static IEnumerable<string> DeleteFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, SearchOption searchOption, bool continueOnError)
        {
            foreach (var directory in directories)
            {
                foreach (var file in directory.GetFiles("*", searchOption))
                {
                    string df;
                    try
                    {
                        if (file.Exists) file.Delete();
                        else continue;
                        df = file.FullName;
                    }
                    catch (Exception)
                    {
                        if (continueOnError) continue;
                        throw;
                    }
                    yield return df;
                }
            }
        }

        public static IEnumerable<string> DeleteFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<FileInfo, bool> predicate, bool continueOnError)
        {
            foreach (var directory in directories)
            {
                foreach (var file in directory.GetFiles().Where(predicate))
                {
                    string df;
                    try
                    {
                        if (file.Exists) file.Delete();
                        else continue;
                        df = file.FullName;
                    }
                    catch (Exception)
                    {
                        if (continueOnError) continue;
                        throw;
                    }
                    yield return df;
                }
            }
        }

        public static IEnumerable<string> DeleteFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<FileInfo, bool> predicate, string searchPattern, bool continueOnError)
        {
            foreach (var directory in directories)
            {
                foreach (var file in directory.GetFiles(searchPattern, SearchOption.TopDirectoryOnly).Where(predicate))
                {
                    string df;
                    try
                    {
                        if (file.Exists) file.Delete();
                        else continue;
                        df = file.FullName;
                    }
                    catch (Exception)
                    {
                        if (continueOnError) continue;
                        throw;
                    }
                    yield return df;
                }
            }
        }

        public static IEnumerable<string> DeleteFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<FileInfo, bool> predicate,
            string searchPattern, SearchOption searchOption, bool continueOnError)
        {
            foreach (var directory in directories)
            {
                foreach (var file in directory.GetFiles(searchPattern, searchOption).Where(predicate))
                {
                    string df;
                    try
                    {
                        if (file.Exists) file.Delete();
                        else continue;
                        df = file.FullName;
                    }
                    catch (Exception)
                    {
                        if (continueOnError) continue;
                        throw;
                    }
                    yield return df;
                }
            }
        }

        public static IEnumerable<string> DeleteFilesFromEnumerable(this IEnumerable<DirectoryInfo> directories, Func<FileInfo, bool> predicate, SearchOption searchOption, bool continueOnError)
        {
            foreach (var directory in directories)
            {
                foreach (var file in directory.GetFiles("*", searchOption).Where(predicate))
                {
                    string df;
                    try
                    {
                        if (file.Exists) file.Delete();
                        else continue;
                        df = file.FullName;
                    }
                    catch (Exception)
                    {
                        if (continueOnError) continue;
                        throw;
                    }
                    yield return df;
                }
            }
        }

        #endregion Delete Files

        public static byte[] GetAllBytes(this FileInfo fileInfo, int lockWaitMs)
        {
            using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                if (!fileAccess.IsAccessible) throw new Exception(@"Can not open locked file! The file is locked by another process.");
                using (var fs = fileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, 60000, true))
                {
                    var buffersize = fs.Length > 4096 ? 4096 : (int)fs.Length;

                    var bytes = new byte[buffersize];
                    using (var ms = new MemoryStream())
                    {
                        while (true) // Loops Rule!!!!!!!!
                        {
                            var bytecount = fs.Read(bytes, 0, bytes.Length);
                            if (bytecount > 0)
                            {
                                ms.Write(bytes, 0, bytecount);
                            }
                            else
                            {
                                break;
                            }
                        }
                        return ms.ToArray();
                    }
                }
            }
        }

        public static byte[] GetBytesByRange(this FileInfo fileInfo, int lockWaitMs, long origin, int length)
        {
            using (var fs = fileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, length, lockWaitMs, true))
            {
                fs.Seek(origin, SeekOrigin.Begin);
                var rv = new byte[length];
                fs.Read(rv, 0, length);
                return rv;
            }
        }

        #endregion FileInfo Extensions

        #region Display Size as String Extensions

        public static string DisplayKb(this Int64 value)
        {
            try
            {
                return (value / 1024.0).ToString("#,##0.#") + "KB";
            }
            catch
            {
                return "0KB";
            }
        }

        public static string DisplayMb(this Int64 value)
        {
            try
            {
                return (value / 1048576.0).ToString("#,##0.#") + "MB";
            }
            catch
            {
                return "0MB";
            }
        }

        public static Double GetMbFromBytes(this Int64 value)
        {
            try
            {
                return value / 1048576.0;
            }
            catch
            {
                return 0D;
            }
        }

        public static string DisplayGb(this Int64 value)
        {
            try
            {
                return (value / 1073741824.0).ToString("#,##0.##") + "GB";
            }
            catch
            {
                return "0GB";
            }
        }

        public static string DisplayGbMbKb(this Int64 value)
        {
            try
            {
                return (value / 1073741824.0).ToString("#,##0.##") + "GB, "
                     + (value / 1048576.0).ToString("#,##0.#") + "MB, "
                    + (value / 1024.0).ToString("#,##0.#") + "KB";
            }
            catch
            {
                return "0KB";
            }
        }

        #endregion Display Size as String Extensions

        public static IEnumerable<FileInfo> GZipDirectory(this DirectoryInfo inputDirectory, string searchPattern = @"*.*", bool topDirectoryOnly = true, bool deleteOriginal = false)
        {
            return GZipDirectory(inputDirectory, null, searchPattern, topDirectoryOnly, deleteOriginal);
        }

        public static IEnumerable<FileInfo> GZipDirectory(this DirectoryInfo inputDirectory, IEnumerable<string> fileNameRestrictions = null, string searchPattern = @"*.*", bool topDirectoryOnly = true, bool deleteOriginal = false)
        {
            var rv = new List<FileInfo>();
            var option = SearchOption.TopDirectoryOnly;
            if (!topDirectoryOnly)
            {
                option = SearchOption.AllDirectories;
            }

            foreach (var gzfi in inputDirectory.GetFiles(searchPattern, option)
                .Where(x =>
                    x.Extension.ToLower() != ".gz" &&
                    !File.Exists(Path.Combine(inputDirectory.FullName, x.Name.Replace(x.Extension, ".gz")))
                    ))
            {
                if (fileNameRestrictions != null)
                {
                    // ReSharper disable once PossibleMultipleEnumeration
                    if (fileNameRestrictions.Contains(gzfi.Extension.ToLower())) continue;
                    rv.Add(gzfi.GZip(360000));
                    if (deleteOriginal) gzfi.Delete();
                }
                else
                {
                    rv.Add(gzfi.GZip(360000));
                    if (deleteOriginal) gzfi.Delete();
                }
            }
            return rv;
        }

        public static FileInfo GZip(this FileInfo fileInfo, int lockWaitMs, bool overWrite = false, int bufferSize = 4096)
        {
            fileInfo.Refresh();
            var outputfi = fileInfo.DirectoryName != null ? new FileInfo(Path.Combine(fileInfo.DirectoryName, fileInfo.Name + ".gz")) : new FileInfo(fileInfo.Name + ".gz");
            if (outputfi.Exists)
            {
                if (overWrite)
                {
                    outputfi.Delete();
                }
                else
                {
                    return fileInfo;
                }
            }

            using (new WriteFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
            {
                using (var inputfs = fileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, bufferSize, lockWaitMs, false))
                {
                    using (var outputfs = outputfi.OpenFileStream(FileMode.CreateNew, FileAccess.Write, FileShare.None, 60000, true))
                    {
                        using (var stream = new GZipStream(outputfs, CompressionMode.Compress, true))
                        {
                            if (inputfs.CanSeek) inputfs.Seek(0, SeekOrigin.Begin); // Important!

                            const long maxint = int.MaxValue / 2; // Reduce 2GB limit to around 1GB for Int32.MaxValue********
                            var length = inputfs.Length < maxint ? (int)inputfs.Length : (int)maxint;
                            int buffersize;
                            if (bufferSize > 0)
                            {
                                buffersize = length > bufferSize ? bufferSize : length;
                            }
                            else
                            {
                                buffersize = length > 4096 ? 4096 : length;
                            }
                            var bytes = new byte[buffersize];

                            while (true) // Loops Rule!!!!!!!!
                            {
                                var bytecount = inputfs.Read(bytes, 0, bytes.Length);
                                if (bytecount > 0)
                                {
                                    stream.Write(bytes, 0, bytecount);
                                    stream.Flush();
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        public static string ReadToEnd(this Stream stream)
        {
            var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }

        public static Stream GZipToStream(this byte[] inputBytes)
        {
            return GZipToStream(new MemoryStream(inputBytes));
        }

        public static Stream GZipToStream(this Stream stream)
        {
            return stream.GZipToStream(4096);
        }

        public static Stream GZipToStream(this Stream stream, int bufferSize)
        {
            var ms = new MemoryStream();
            try
            {
                using (var gzstream = new GZipStream(ms, CompressionMode.Compress, true))
                {
                    if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin); // Important!

                    const long maxint = int.MaxValue / 2; // Reduce 2GB limit to around 1GB for Int32.MaxValue********
                    var length = stream.Length < maxint ? (int)stream.Length : (int)maxint;
                    int buffersize;
                    if (bufferSize > 0)
                    {
                        buffersize = length > bufferSize ? bufferSize : length;
                    }
                    else
                    {
                        buffersize = length > 4096 ? 4096 : length;
                    }
                    var bytes = new byte[buffersize];

                    while (true) // Loops Rule!!!!!!!!
                    {
                        var bytecount = stream.Read(bytes, 0, bytes.Length);
                        if (bytecount > 0)
                        {
                            gzstream.Write(bytes, 0, bytecount);
                            gzstream.Flush();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin); // Important!
            }
            return ms;
        }

        // ReSharper disable once InconsistentNaming
        public static Stream GUnZipToStream(this Stream stream, int bufferSize = 0)
        {
            var ms = new MemoryStream();
            try
            {
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin); // Important!

                using (var gzstream = new GZipStream(stream, CompressionMode.Decompress, true))
                {
                    const long maxint = int.MaxValue / 2; // Reduce 2GB limit to around 1GB for Int32.MaxValue********
                    var length = stream.Length < maxint ? (int)stream.Length : (int)maxint;
                    int buffersize;
                    if (bufferSize > 0)
                    {
                        buffersize = length > bufferSize ? bufferSize : length;
                    }
                    else
                    {
                        buffersize = length > 4096 ? 4096 : length;
                    }
                    var bytes = new byte[buffersize];

                    while (true) // Loops Rule!!!!!!!!
                    {
                        var bytecount = gzstream.Read(bytes, 0, bytes.Length);
                        if (bytecount > 0)
                        {
                            ms.Write(bytes, 0, bytecount);
                            ms.Flush();
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            finally
            {
                if (stream.CanSeek) stream.Seek(0, SeekOrigin.Begin); // Important!
            }
            return ms;
        }

        public static void GUnZipToFileFast(this FileInfo inputFile, FileInfo outputFile, bool overwrite)
        {
            if (overwrite && outputFile.Exists) outputFile.Delete();
            using (var instream = new FileStream(inputFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                using (var outstream = new FileStream(outputFile.FullName, FileMode.Create, FileAccess.Write, FileShare.None, 4096))
                {
                    using (var gz = new GZipStream(instream, CompressionMode.Decompress))
                    {
                        gz.CopyTo(outstream);
                    }
                }
            }
            inputFile.Delete();
        }

        public static void GUnZipToFileFast(this FileInfo inputFile, string outputFilePath, bool overwrite)
        {
            var outputFile = new FileInfo(outputFilePath);
            if (overwrite && outputFile.Exists) outputFile.Delete();
            using (var instream = new FileStream(inputFile.FullName, FileMode.Open, FileAccess.Read, FileShare.Delete | FileShare.Read, 4096, FileOptions.SequentialScan))
            {
                using (var outstream = new FileStream(outputFile.FullName, FileMode.Create, FileAccess.Write, FileShare.None, 4096))
                {
                    using (var gz = new GZipStream(instream, CompressionMode.Decompress))
                    {
                        gz.CopyTo(outstream);
                    }
                }
            }
            inputFile.Delete();
        }

        public static string GetDateTimeUpdatedFileName(this FileInfo fileInfo, DateTime? dateValue = null, bool useUtc = true, string formatString = null)
        {
            if (string.IsNullOrWhiteSpace(formatString)) formatString = @"yyyy-MM-ddTHHmmss";
            if (useUtc)
            {
                var dt = dateValue.HasValue ? @"_UTC" + dateValue.Value.ToUniversalTime().ToString(formatString) : @"_UTC" + DateTime.UtcNow.ToString(formatString);
                return Path.HasExtension(fileInfo.Name) ? fileInfo.FullName.Replace(fileInfo.Extension, dt + fileInfo.Extension) : fileInfo.FullName + dt;
            }
            else
            {
                var dt = dateValue.HasValue ? @"_" + dateValue.Value.ToString(formatString) : @"_" + DateTime.Now.ToString(formatString);
                return Path.HasExtension(fileInfo.Name) ? fileInfo.FullName.Replace(fileInfo.Extension, dt + fileInfo.Extension) : fileInfo.FullName + dt;
            }
        }

        //public static String GetDateTimeUpdatedFileNameString(String fileName, DateTime? dateValue = null, Boolean useUtc = true, String formatString = null)
        //{
        //    if (String.IsNullOrWhiteSpace(formatString)) formatString = @"yyyy-MM-ddTHHmmss";
        //    if (useUtc)
        //    {
        //        var dt = dateValue.HasValue ? @"_UTC" + dateValue.Value.ToUniversalTime().ToString(formatString) : @"_UTC" + DateTime.UtcNow.ToString(formatString);
        //        return fileName.Contains('.') ? fileName.LeftOfLastIndexOf('.') + dt + "." + fileName.RightOfLastIndexOf('.') : fileName + dt;
        //    }
        //    else
        //    {
        //        var dt = dateValue.HasValue ? @"_" + dateValue.Value.ToString(formatString) : @"_" + DateTime.Now.ToString(formatString);
        //        return Path.HasExtension(fileName) ? fileName.LeftOfLastIndexOf('.') + dt + "." + fileName.RightOfLastIndexOf('.') : fileName + dt;
        //    }
        //}
    }
}