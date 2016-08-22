using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;

namespace Business.Common.IO
{
    public static class ZipExtensionBase
    {
        public static MemoryStream ReadZipEntryToStream(this FileInfo file, int index)
        {
            var ms = new MemoryStream();
            using (var zip = ZipFile.OpenRead(file.FullName))
            {
                var e = zip.Entries[index];
                e.Open().CopyToAsync(ms);
            }
            return ms;
        }

        public static MemoryStream ReadZipEntryToStream(this FileInfo file, string EntryName)
        {
            var ms = new MemoryStream();
            using (var zip = ZipFile.OpenRead(file.FullName))
            {
                var e = zip.Entries.First(x => x.Name == EntryName);
                e.Open().CopyToAsync(ms);
            }
            return ms;
        }

        public static IEnumerable<FileInfo> ZipFileExtract(this FileInfo file, string directoryPath = null)
        {
            using (var zip = ZipFile.OpenRead(file.FullName))
            {
                if (string.IsNullOrWhiteSpace(directoryPath)) directoryPath = file.DirectoryName;
                var entries = (from entry in zip.Entries where directoryPath != null select new FileInfo(Path.Combine(directoryPath, entry.FullName))).ToList();
                zip.ExtractToDirectory(@"\");
                return entries;
            }
        }

        public static IEnumerable<string> ZipFileEntryFileNames(this FileInfo file)
        {
            using (var zip = ZipFile.OpenRead(file.FullName))
            {
                return zip.Entries.Select(entry => entry.FullName).ToList();
            }
        }

        public static IEnumerable<ZipArchiveEntry> ZipFileEntries(this FileInfo file)
        {
            using (var zip = ZipFile.OpenRead(file.FullName))
            {
                return zip.Entries.ToList();
            }
        }

        public static FileInfo AddFileToZip(this FileInfo file, FileInfo InputFileInfo, string directoryPathInArchive = null)
        {
            file.Refresh();
            if (file.Exists)
            {
                using (var archive = ZipFile.Open(file.FullName, ZipArchiveMode.Update))
                {
                    archive.CreateEntryFromFile(InputFileInfo.FullName, string.IsNullOrWhiteSpace(directoryPathInArchive) ? InputFileInfo.Name : Path.Combine(directoryPathInArchive, InputFileInfo.Name));
                }
                file.Refresh();
                return file;
            }
            else
            {
                using (var archive = ZipFile.Open(file.FullName, ZipArchiveMode.Create))
                {
                    archive.CreateEntryFromFile(InputFileInfo.FullName, InputFileInfo.Name);
                }
                file.Refresh();
                return file;
            }
        }

        public static void CreateZip(this FileInfo file, FileInfo InputFileInfo, string directoryPathInArchive = null, bool OverWriteFile = true)
        {
            if (file.Exists && OverWriteFile) file.Delete();
            file.AddFileToZip(InputFileInfo, directoryPathInArchive);
            file.Refresh();
        }
    }
}