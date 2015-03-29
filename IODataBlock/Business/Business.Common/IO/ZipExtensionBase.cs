using System;
using System.Collections.Generic;
using System.IO;

namespace Business.Common.IO
{
    public static class ZipExtensionBase
    {

        //public static MemoryStream ReadZipEntryToStream(this FileInfo file, Int32 index)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    using (ZipFile zip = ZipFile.Read(file.FullName))
        //    {
        //        ZipEntry e = zip[index];
        //        e.Extract(file.DirectoryName, ExtractExistingFileAction.OverwriteSilently);
        //    }
        //    return ms;
        //}

        //public static MemoryStream ReadZipEntrToStream(this FileInfo file, String EntryName)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    using (ZipFile zip = ZipFile.Read(file.FullName))
        //    {
        //        ZipEntry e = zip[EntryName];
        //        e.Extract(file.DirectoryName, ExtractExistingFileAction.OverwriteSilently);
        //    }
        //    return ms;
        //}

        //public static IEnumerable<FileInfo> ZipFileExtract(this FileInfo file)
        //{
        //    using (ZipFile zip = ZipFile.Read(file.FullName))
        //    {
        //        foreach (ZipEntry e in zip)
        //        {
        //            e.Extract(file.DirectoryName, ExtractExistingFileAction.OverwriteSilently);
        //            yield return new FileInfo(Path.Combine(file.DirectoryName, e.FileName));
        //        }
        //    }
        //}

        //public static IEnumerable<String> ZipFileEntryFileNames(this FileInfo file)
        //{
        //    using (ZipFile zip = ZipFile.Read(file.FullName))
        //    {
        //        foreach (var name in zip.EntryFileNames)
        //        {
        //            yield return name;
        //        }
        //    }
        //}

        //public static IEnumerable<ZipEntry> ZipFileEntries(this FileInfo file)
        //{
        //    using (ZipFile zip = ZipFile.Read(file.FullName))
        //    {
        //        foreach (var entry in zip.Entries)
        //        {
        //            yield return entry;
        //        }
        //    }
        //}

        //public static FileInfo AddFileToZip(this FileInfo file, FileInfo InputFileInfo)
        //{
        //    try
        //    {
        //        file.Refresh();
        //        if (file.Exists)
        //        {
        //            if (file.Length > 0 && !ZipFile.IsZipFile(file.FullName)) throw new ArgumentException(String.Format(@"Argument Exception: {0} is not a Zip file!", file.FullName));

        //            using (var fs = file.OpenFileStream(FileMode.Open, FileAccess.ReadWrite, FileShare.None, 120000, false))
        //            {
        //                using (ZipFile zip = ZipFile.Read(fs))
        //                {
        //                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
        //                    InputFileInfo.Refresh();
        //                    if (InputFileInfo.Exists)
        //                    {
        //                        ZipEntry entry = zip[InputFileInfo.Name];
        //                        if (entry != null)
        //                        {
        //                            using (var inputfs = InputFileInfo.OpenFileStream(FileMode.Open, FileAccess.Read, FileShare.Read, 120000, false))
        //                            {
        //                                zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
        //                                zip.UpdateEntry(InputFileInfo.Name, inputfs);
        //                                zip.Save(fs);
        //                            }                                   

        //                            //zip.AddFile(InputFileInfo.FullName).FileName = InputFileInfo.Name;                                    
        //                        }
        //                        else zip.AddFile(InputFileInfo.FullName).FileName = InputFileInfo.Name;
        //                        //zip.Save(file.FullName);
        //                    }
        //                    else throw new FileNotFoundException(String.Format(@"FileNotFoundException: {0} file does not exist!", InputFileInfo.FullName));
        //                    //zip.Save(file.FullName);
        //                }
        //                fs.Flush();
        //                //fs.Unlock(0, fs.Length);
        //            }
        //            file.Refresh();
        //            return file;
        //        }
        //        else
        //        {
        //            using (var fs = file.OpenFileStream(FileMode.Create, FileAccess.ReadWrite, FileShare.None, 120000, false))
        //            {
        //                using (ZipFile zip = new ZipFile())
        //                {
        //                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.None;
        //                    InputFileInfo.Refresh();
        //                    if (InputFileInfo.Exists)
        //                    {
        //                        zip.AddFile(InputFileInfo.FullName).FileName = InputFileInfo.Name;
        //                        zip.Save(fs);
        //                    }
        //                    else throw new FileNotFoundException(String.Format(@"FileNotFoundException: {0} file does not exist!", InputFileInfo.FullName));
        //                    //zip.Save(file.FullName);
        //                }
        //                fs.Flush();
        //                //fs.Unlock(0, fs.Length);
        //            }
        //            file.Refresh();
        //            return file;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw;
        //    }
        //}

        //public static FileInfo AddFileToZip(this FileInfo file, FileInfo InputFileInfo, String directoryPathInArchive)
        //{
        //    file.Refresh();
        //    if (file.Exists)
        //    {
        //        using (ZipFile zip = ZipFile.Read(file.FullName))
        //        {
        //            InputFileInfo.Refresh();
        //            if (InputFileInfo.Exists)
        //            {
        //                if (zip[InputFileInfo.Name] != null) zip.RemoveEntry(InputFileInfo.Name);
        //                zip.AddFile(InputFileInfo.FullName, directoryPathInArchive);
        //            }
        //            else throw new FileNotFoundException(String.Format(@"FileNotFoundException: {0} file does not exist!", InputFileInfo.FullName));
        //        }
        //        file.Refresh();
        //        return file;
        //    }
        //    else
        //    {
        //        using (ZipFile zip = new ZipFile())
        //        {
        //            InputFileInfo.Refresh();
        //            if (InputFileInfo.Exists)
        //            {
        //                if (zip[InputFileInfo.Name] != null) zip.RemoveEntry(InputFileInfo.Name);
        //                zip.AddFile(InputFileInfo.FullName, directoryPathInArchive);
        //            }
        //            else throw new FileNotFoundException(String.Format(@"FileNotFoundException: {0} file does not exist!", InputFileInfo.FullName));
        //            zip.Save(file.FullName);
        //        }
        //        file.Refresh();
        //        return file;
        //    }
        //}



    }
}
