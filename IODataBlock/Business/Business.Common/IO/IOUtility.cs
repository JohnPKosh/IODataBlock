#region "using Namespaces"

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion "using Namespaces"

namespace Business.Common.IO
{
    // ReSharper disable once InconsistentNaming
    public static class IOUtility
    {
        #region "Properties"

        public static Boolean IsWebAssembly()
        {
            var entry = Assembly.GetEntryAssembly();
            return entry == null || Assembly.GetCallingAssembly().FullName.Contains(@"App_");
        }

        public static String AssemblyNameStr
        {
            get
            {
                if (IsWebAssembly())
                {
                    var pvtAssemblyNameStr = String.Empty;
                    pvtAssemblyNameStr += Assembly.GetCallingAssembly().GetName().Name;
                    pvtAssemblyNameStr += " (";
                    pvtAssemblyNameStr += Assembly.GetExecutingAssembly().GetName().Name;
                    pvtAssemblyNameStr += ") ";
                    return pvtAssemblyNameStr;

                    //return Assembly.GetCallingAssembly().FullName;
                }
                else
                {
                    var pvtAssemblyNameStr = String.Empty;
                    pvtAssemblyNameStr += Assembly.GetEntryAssembly().GetName().Name;
                    pvtAssemblyNameStr += " (";
                    pvtAssemblyNameStr += Assembly.GetExecutingAssembly().GetName().Name;
                    pvtAssemblyNameStr += ") ";
                    return pvtAssemblyNameStr;
                }
            }
        }

        // System.Reflection.Assembly.GetEntryAssembly().Location OR Directory.GetCurrentDirectory()
        public static String AssemblyFilePath
        {
            get
            {
                return IsWebAssembly() ? Assembly.GetCallingAssembly().Location : Assembly.GetEntryAssembly().Location;
                // HostingEnvironment.ApplicationPhysicalPath;
            }
        }

        private static String _pvtDefaultFolderPath = String.Empty;

        public static String DefaultFolderPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_pvtDefaultFolderPath)) return _pvtDefaultFolderPath;
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                _pvtDefaultFolderPath = baseDirectory.Substring(0, baseDirectory.LastIndexOf(@"\", StringComparison.Ordinal));
                return _pvtDefaultFolderPath;
            }
            set { _pvtDefaultFolderPath = value; }
        }

        private static String _pvtAppDataFolderPath = String.Empty;

        public static string AppDataFolderPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_pvtAppDataFolderPath)) return _pvtAppDataFolderPath;
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                _pvtAppDataFolderPath = Path.Combine(baseDirectory.Substring(0, baseDirectory.LastIndexOf(@"\", StringComparison.Ordinal)), @"App_Data");
                return _pvtAppDataFolderPath;
            }
            set { _pvtAppDataFolderPath = value; }
        }

        private static String _pvtDefaultLogFolderPath = String.Empty;

        public static String DefaultLogFolderPath
        {
            get
            {
                if (!string.IsNullOrEmpty(_pvtDefaultLogFolderPath)) return _pvtDefaultLogFolderPath;
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
                _pvtDefaultLogFolderPath = baseDirectory.Substring(0, baseDirectory.LastIndexOf(@"\", StringComparison.Ordinal));
                return _pvtDefaultLogFolderPath;
            }
            set { _pvtDefaultLogFolderPath = value; }
        }

        private static String _pvtDefaultConfigPath;// = EntryAssemblyPath;

        public static String DefaultConfigPath
        {
            get
            {
                if (string.IsNullOrEmpty(_pvtDefaultConfigPath))
                {
                    _pvtDefaultConfigPath = IsWebAssembly() ? "~" : DefaultAssemblyPath;
                }
                return _pvtDefaultConfigPath;
            }
            set { _pvtDefaultAssemblyPath = value; }
        }

        private static String _pvtDefaultAssemblyPath;// = EntryAssemblyPath;

        public static String DefaultAssemblyPath
        {
            get
            {
                if (string.IsNullOrEmpty(_pvtDefaultAssemblyPath))
                {
                    _pvtDefaultAssemblyPath = IsWebAssembly() ? Assembly.GetCallingAssembly().Location : Assembly.GetEntryAssembly().Location;
                }
                return _pvtDefaultAssemblyPath;
            }
            set { _pvtDefaultAssemblyPath = value; }
        }

        #endregion "Properties"

        #region "Methods"

        public static Boolean CompareFiles(String file1, String file2)
        {
            if (!File.Exists(file1) || !File.Exists(file2)) return false;
            var fileInfo1 = new FileInfo(file1);
            var fileInfo2 = new FileInfo(file2);

            if (fileInfo1.Length != fileInfo2.Length) return false;
            //Read the bytes from both files.
            var bytes1 = File.ReadAllBytes(file1);
            var bytes2 = File.ReadAllBytes(file2);

            //Make sure the arrays are of equal length.
            if (bytes1.Length != bytes2.Length)
            {
                return false;
            }

            //Compare each byte.
            for (var i = 0; i <= bytes1.Length - 1; i++)
            {
                if (bytes1[i] != bytes2[i])
                {
                    return false;
                }
            }
            return true;
        }

        public static Boolean SetFileAsHidden(String filePathStr)
        {
            if (File.Exists(filePathStr))
            {
                File.SetAttributes(filePathStr, FileAttributes.Hidden);
            }
            const bool result = true;
            return result;
        }

        public static Boolean SetFileAsArchive(String filePathStr)
        {
            if (File.Exists(filePathStr))
            {
                File.SetAttributes(filePathStr, FileAttributes.Archive);
            }
            return true;
        }

        public static Boolean SetFileAsReadOnly(String filePathStr)
        {
            if (File.Exists(filePathStr))
            {
                File.SetAttributes(filePathStr, FileAttributes.ReadOnly);
            }
            return true;
        }

        public static Boolean SetFileAsNormal(String filePathStr)
        {
            if (File.Exists(filePathStr))
            {
                File.SetAttributes(filePathStr, FileAttributes.Normal);
            }
            return true;
        }

        public static Boolean SetFileAsCompressed(String filePathStr)
        {
            if (File.Exists(filePathStr))
            {
                File.SetAttributes(filePathStr, FileAttributes.Compressed);
            }
            return true;
        }

        public static Boolean IsFileArchive(String filePathStr)
        {
            var result = (File.GetAttributes(filePathStr) & FileAttributes.Archive) == FileAttributes.Archive;
            return result;
        }

        public static Boolean IsFileCompressed(String filePathStr)
        {
            var results = (File.GetAttributes(filePathStr) & FileAttributes.Compressed) == FileAttributes.Compressed;
            return results;
        }

        public static Boolean IsFileEncrypted(String filePathStr)
        {
            var result = (File.GetAttributes(filePathStr) & FileAttributes.Encrypted) == FileAttributes.Encrypted;
            return result;
        }

        public static Boolean IsFileHidden(String filePathStr)
        {
            var result = (File.GetAttributes(filePathStr) & FileAttributes.Hidden) == FileAttributes.Hidden;
            return result;
        }

        public static Boolean IsFileReadOnly(String filePathStr)
        {
            var result = (File.GetAttributes(filePathStr) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly;
            return result;
        }

        public static Boolean IsFileSystem(String filePathStr)
        {
            var result = (File.GetAttributes(filePathStr) & FileAttributes.System) == FileAttributes.System;
            return result;
        }

        public static Boolean IsFileTemporary(String filePathStr)
        {
            var result = (File.GetAttributes(filePathStr) & FileAttributes.Temporary) == FileAttributes.Temporary;
            return result;
        }

        public static Boolean CopyDirectory(String src, String dst)
        {
            if (dst[dst.Length - 1] != Path.DirectorySeparatorChar)
                dst += Path.DirectorySeparatorChar;
            if (!Directory.Exists(dst))
                Directory.CreateDirectory(dst);
            var files = Directory.GetFileSystemEntries(src);
            foreach (var element in files)
            {
                if (Directory.Exists(element))
                    CopyDirectory(element, dst + Path.GetFileName(element));
                else
                    File.Copy(element, dst + Path.GetFileName(element), true);
            }
            return true;
        }

        public static Boolean CreateDirectory(String dirPathStr)
        {
            Directory.CreateDirectory(dirPathStr);
            return true;
        }

        public static Boolean DirectoryExists(String dirPathStr)
        {
            return Directory.Exists(dirPathStr);
        }

        public static String GetTempFile()
        {
            return Path.GetTempFileName();
        }

        public static Int64 FileSize(String filePathStr)
        {
            Int64 result = 0;
            var fileinfo = new FileInfo(filePathStr);
            if (fileinfo.Exists)
            {
                result = fileinfo.Length;
            }
            return result;
        }

        public static DateTime GetFileCreationDate(String filePathStr)
        {
            var fi = new FileInfo(filePathStr);
            return fi.CreationTime;
        }

        public static DateTime GetFileLastModifiedDate(String filePathStr)
        {
            var modified = DateTime.Now;
            var fi = new FileInfo(filePathStr);
            if (fi.Exists)
            {
                modified = fi.LastWriteTime;
            }
            return modified;
        }

        public static String GetFileName(String filePathStr)
        {
            return Path.GetFileName(filePathStr);
        }

        public static String GetFileNameWithoutExtension(String filePathStr)
        {
            return Path.GetFileNameWithoutExtension(filePathStr);
        }

        public static String GetExtensionOnly(String filePathStr)
        {
            return Path.GetExtension(filePathStr);
        }

        public static String GetFullPath(String pathStr)
        {
            return Path.GetFullPath(pathStr);
        }

        public static List<String> GetFileListByExt(String directory, String ext)
        {
            var fileList = new List<string>();
            var fileArray = Directory.GetFiles(directory, ext, SearchOption.TopDirectoryOnly);
            fileList.InsertRange(0, fileArray);
            return fileList;
        }

        public static List<String> GetDirectories(String pathStr)
        {
            return Directory.GetDirectories(pathStr).ToList();
        }

        public static List<DriveInfo> GetDrives()
        {
            return DriveInfo.GetDrives().ToList();
        }

        public static List<DriveInfo> GetFixedDrives()
        {
            return DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.Fixed).ToList();
        }

        public static String GetWindowsDrive()
        {
            var value = @"C:\";
            var specialFolder = EnvironmentSpecialFolders.FirstOrDefault(x => x.Key == "Windows");
            if (specialFolder.Key != null)
            {
                value = specialFolder.Value.Substring(0, 3);
            }
            return value;
        }

        public static List<DriveInfo> GetCdDrives()
        {
            return DriveInfo.GetDrives().Where(drive => drive.DriveType == DriveType.CDRom).ToList();
        }

        public static Int64 DriveCapacity(String driveLetter)
        {
            Int64 capacity = 0;
            foreach (var drive in DriveInfo.GetDrives().Where(drive => drive.Name == driveLetter))
            {
                capacity = drive.TotalSize;
            }
            return capacity;
        }

        public static Int64 DriveFreeSpace(String driveLetter)
        {
            Int64 capacity = 0;
            foreach (var drive in DriveInfo.GetDrives().Where(drive => drive.Name == driveLetter))
            {
                capacity = drive.AvailableFreeSpace;
            }
            return capacity;
        }

        /// <summary>
        /// Creates default FolderDS and filenames DataTable
        /// </summary>
        public static DataTable GetFileDtFull(String folderPath, String dataTablename, String ext, SearchOption searchOption)
        {
            var di = new DirectoryInfo(folderPath);
            var fileDt = new DataTable(dataTablename);
            if (di.Exists)
            {
                fileDt.Columns.Add("CreationTime", typeof(DateTime));
                fileDt.Columns.Add("CreationTimeUtc", typeof(DateTime));
                fileDt.Columns.Add("Directory");
                fileDt.Columns.Add("DirectoryName");
                fileDt.Columns.Add("Exists", typeof(Boolean));
                fileDt.Columns.Add("Extension");
                fileDt.Columns.Add("FullName");
                fileDt.Columns.Add("IsReadOnly", typeof(Boolean));
                fileDt.Columns.Add("LastAccessTime", typeof(DateTime));
                fileDt.Columns.Add("LastAccessTimeUtc", typeof(DateTime));
                fileDt.Columns.Add("LastWriteTime", typeof(DateTime));
                fileDt.Columns.Add("LastWriteTimeUtc", typeof(DateTime));
                fileDt.Columns.Add("Length", typeof(Int64));
                fileDt.Columns.Add("Name");

                fileDt.Columns.Add("IsArchive", typeof(Boolean)).DefaultValue = false;
                fileDt.Columns.Add("IsCompressed", typeof(Boolean)).DefaultValue = false;
                fileDt.Columns.Add("IsEncrypted", typeof(Boolean)).DefaultValue = false;
                fileDt.Columns.Add("IsHidden", typeof(Boolean)).DefaultValue = false;

                //fileDT.Columns.Add("IsReadOnly", typeof(Boolean)).DefaultValue = false;
                fileDt.Columns.Add("IsSystem", typeof(Boolean)).DefaultValue = false;
                fileDt.Columns.Add("IsTemporary", typeof(Boolean)).DefaultValue = false;

                var files = di.GetFiles(ext, searchOption);
                foreach (var fil in files)
                {
                    var row = fileDt.NewRow();
                    row["CreationTime"] = fil.CreationTime;
                    row["CreationTimeUtc"] = fil.CreationTimeUtc;
                    row["Directory"] = fil.Directory;
                    row["DirectoryName"] = fil.DirectoryName;
                    row["Exists"] = fil.Exists;
                    row["Extension"] = fil.Extension;
                    row["FullName"] = fil.FullName;
                    row["IsReadOnly"] = fil.IsReadOnly;
                    row["LastAccessTime"] = fil.LastAccessTime;
                    row["LastAccessTimeUtc"] = fil.LastAccessTimeUtc;
                    row["LastWriteTime"] = fil.LastWriteTime;
                    row["LastWriteTimeUtc"] = fil.LastWriteTimeUtc;
                    row["Length"] = fil.Length;
                    row["Name"] = fil.Name;

                    if ((File.GetAttributes(fil.FullName) & FileAttributes.Archive) == FileAttributes.Archive)
                    {
                        row["IsArchive"] = true;
                    }
                    if ((File.GetAttributes(fil.FullName) & FileAttributes.Compressed) == FileAttributes.Compressed)
                    {
                        row["IsCompressed"] = true;
                    }
                    if ((File.GetAttributes(fil.FullName) & FileAttributes.Encrypted) == FileAttributes.Encrypted)
                    {
                        row["IsEncrypted"] = true;
                    }
                    if ((File.GetAttributes(fil.FullName) & FileAttributes.Hidden) == FileAttributes.Hidden)
                    {
                        row["IsHidden"] = true;
                    }

                    //if ((File.GetAttributes(fil.FullName) & FileAttributes.ReadOnly) == FileAttributes.ReadOnly)
                    //{
                    //    row["IsReadOnly"] = true;
                    //}
                    if ((File.GetAttributes(fil.FullName) & FileAttributes.System) == FileAttributes.System)
                    {
                        row["IsSystem"] = true;
                    }
                    if ((File.GetAttributes(fil.FullName) & FileAttributes.Temporary) == FileAttributes.Temporary)
                    {
                        row["IsTemporary"] = true;
                    }

                    fileDt.Rows.Add(row);
                }
            }
            return fileDt;
        }

        /// <summary>
        /// Creates default FolderDS and filenames DataTable to bind to FileGrid.
        /// </summary>
        public static DataTable GetFileDt(String folderPath, String dataTablename, String ext, SearchOption searchOption)
        {
            var di = new DirectoryInfo(folderPath);
            var fileDt = new DataTable(dataTablename);
            if (di.Exists)
            {
                fileDt.Columns.Add("FullName");
                fileDt.Columns.Add("LastWriteTime");

                var files = di.GetFiles(ext, searchOption);

                foreach (var fil in files)
                {
                    var row = fileDt.NewRow();
                    row[0] = fil.FullName;
                    row[1] = fil.LastWriteTime;
                    fileDt.Rows.Add(row);
                }
            }
            return fileDt;
        }

        #region SpecialFolders

        /* http://msdn.microsoft.com/en-us/library/system.environment.specialfolder.aspx */

        private static Dictionary<String, String> _environmentSpecialFolders;

        public static Dictionary<String, String> EnvironmentSpecialFolders
        {
            get
            {
                if (_environmentSpecialFolders != null) return _environmentSpecialFolders;
                _environmentSpecialFolders = new Dictionary<string, string>();
                foreach (var value in Enum.GetNames(typeof(Environment.SpecialFolder)))
                {
                    var path = Environment.GetFolderPath((Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), value));
                    if (!String.IsNullOrWhiteSpace(path)) _environmentSpecialFolders.Add(value, path);
                }
                return _environmentSpecialFolders;
            }
        }

        private static Dictionary<String, DirectoryInfo> _environmentSpecialFolderDirectories;

        public static Dictionary<String, DirectoryInfo> EnvironmentSpecialFolderDirectories
        {
            get
            {
                if (_environmentSpecialFolderDirectories != null) return _environmentSpecialFolderDirectories;
                _environmentSpecialFolderDirectories = new Dictionary<string, DirectoryInfo>();
                foreach (var value in Enum.GetNames(typeof(Environment.SpecialFolder)))
                {
                    var path = Environment.GetFolderPath((Environment.SpecialFolder)Enum.Parse(typeof(Environment.SpecialFolder), value));
                    if (!String.IsNullOrWhiteSpace(path)) _environmentSpecialFolderDirectories.Add(value, new DirectoryInfo(path));
                }
                return _environmentSpecialFolderDirectories;
            }
        }

        public static String GetEnvironmentSpecialFolder(String enumName)
        {
            Environment.SpecialFolder value;
            return Enum.TryParse(enumName, true, out value) ? Environment.GetFolderPath(value) : Environment.GetFolderPath(value);
        }

        public static DirectoryInfo GetEnvironmentSpecialFolderDirectory(String enumName)
        {
            return new DirectoryInfo(GetEnvironmentSpecialFolder(enumName));
        }

        public static Boolean TryGetEnvironmentSpecialFolder(String enumName, out Environment.SpecialFolder folder)
        {
            return Enum.TryParse(enumName, true, out folder);
        }

        public static Boolean TryGetEnvironmentSpecialFolderFullName(String enumName, out String fullName)
        {
            Environment.SpecialFolder specialFolder;
            if (TryGetEnvironmentSpecialFolder(enumName, out specialFolder))
            {
                fullName = Environment.GetFolderPath(specialFolder);
                return true;
            }
            fullName = null;
            return false;
        }

        public static Boolean TryGetEnvironmentSpecialFolderDirectory(String enumName, out DirectoryInfo directory)
        {
            Environment.SpecialFolder specialFolder;
            if (TryGetEnvironmentSpecialFolder(enumName, out specialFolder))
            {
                directory = new DirectoryInfo(Environment.GetFolderPath(specialFolder));
                return true;
            }
            directory = new DirectoryInfo(DefaultFolderPath);
            return false;
        }

        public static String GetInitialDirectoryOrSpecialFolder(String value)
        {
            /* return DefaultFolderPath if no path is supplied */
            if (String.IsNullOrWhiteSpace(value) || value == @"\") return DefaultFolderPath;

            if (value == "MyComputer") return "";
            if (value == "WindowsDrive") return GetWindowsDrive();

            /* if it is a UNC path try to return it */
            if (value.StartsWith(@"\\") || (value.Length > 2 && value.Substring(1, 1) == ":"))
            {
                if (Directory.Exists(value)) return value;
                throw new DirectoryNotFoundException("Directory or Special Folder Not Found!");
            }

            /* remove starting \ if it exists */
            if (value.StartsWith(@"\")) value = value.Substring(1);

            /* detemine if it starts with a SpecialFolder then return the SpecialFolderPath */
            var specialFolder = EnvironmentSpecialFolders.FirstOrDefault(x => value.ToLower().StartsWith(x.Key.ToLower()));
            if (specialFolder.Key != null)
            {
                value = specialFolder.Value + value.Remove(0, specialFolder.Key.Length);
            }
            /* else if it is not a SpecialFolder then combine with the DefaultFolderPath of the assembly */
            else
            {
                value = Path.Combine(DefaultFolderPath, value);
            }

            /* finally check if the Directory exists */
            if (Directory.Exists(value)) return value;
            throw new DirectoryNotFoundException("Directory or Special Folder Not Found!");
        }

        public static Boolean TryGetInitialDirectoryOrSpecialFolder(String value, out DirectoryInfo outDirectory)
        {
            /* return DefaultFolderPath if no path is supplied */
            if (String.IsNullOrWhiteSpace(value) || value == @"\")
            {
                outDirectory = new DirectoryInfo(DefaultFolderPath);
                return true;
            }

            /* if it is a UNC path try to return it */
            if (value.StartsWith(@"\\") || (value.Length > 2 && value.Substring(1, 1) == ":"))
            {
                var di = new DirectoryInfo(value);
                if (di.Exists)
                {
                    outDirectory = di;
                    return true;
                }
                outDirectory = null;
                return false;
            }

            /* remove starting \ if it exists */
            if (value.StartsWith(@"\")) value = value.Substring(1);

            /* detemine if it starts with a SpecialFolder then return the SpecialFolderPath */
            var specialFolder = EnvironmentSpecialFolders.FirstOrDefault(x => value.ToLower().StartsWith(x.Key.ToLower()));
            if (specialFolder.Key != null)
            {
                value = specialFolder.Value + value.Remove(0, specialFolder.Key.Length);
            }
            /* else if it is not a SpecialFolder then combine with the DefaultFolderPath of the assembly */
            else
            {
                value = Path.Combine(DefaultFolderPath, value);
            }

            /* finally check if the Directory exists */
            var rv = new DirectoryInfo(value);
            if (rv.Exists)
            {
                outDirectory = rv;
                return true;
            }
            outDirectory = null;
            return false;
        }

        public static Boolean TryGetInitialDirectoryOrSpecialFolder(String value, out String outDirectory)
        {
            /* return DefaultFolderPath if no path is supplied */
            if (String.IsNullOrWhiteSpace(value) || value == @"\")
            {
                outDirectory = new DirectoryInfo(DefaultFolderPath).FullName;
                return true;
            }

            /* if it is a UNC path try to return it */
            if (value.StartsWith(@"\\") || (value.Length > 2 && value.Substring(1, 1) == ":"))
            {
                var di = new DirectoryInfo(value);
                if (di.Exists)
                {
                    outDirectory = di.FullName;
                    return true;
                }
                outDirectory = null;
                return false;
            }

            /* remove starting \ if it exists */
            if (value.StartsWith(@"\")) value = value.Substring(1);

            /* detemine if it starts with a SpecialFolder then return the SpecialFolderPath */
            var specialFolder = EnvironmentSpecialFolders.FirstOrDefault(x => value.ToLower().StartsWith(x.Key.ToLower()));
            if (specialFolder.Key != null)
            {
                value = specialFolder.Value + value.Remove(0, specialFolder.Key.Length);
            }
            /* else if it is not a SpecialFolder then combine with the DefaultFolderPath of the assembly */
            else
            {
                value = Path.Combine(DefaultFolderPath, value);
            }

            /* finally check if the Directory exists */
            var rv = new DirectoryInfo(value);
            if (rv.Exists)
            {
                outDirectory = rv.FullName;
                return true;
            }
            outDirectory = null;
            return false;
        }

        /*
        private static Dictionary<String, String> _EnvironmentSpecialFolders2 = null;
        public static Dictionary<String, String> EnvironmentSpecialFolders2
        {
            get
            {
                if (_EnvironmentSpecialFolders2 != null) return _EnvironmentSpecialFolders2;
                _EnvironmentSpecialFolders2 = new Dictionary<string, string>();
                var names = Enum.GetNames(typeof(Environment.SpecialFolder));
                var values = Enum.GetValues(typeof(Environment.SpecialFolder));

                for (int i = 0; i < names.Length; i++)
                {
                    var value = Environment.GetFolderPath((Environment.SpecialFolder)values.GetValue(i));
                    if (!String.IsNullOrWhiteSpace(value)) _EnvironmentSpecialFolders2.Add(names[i], value);
                }
                return _EnvironmentSpecialFolders2;
            }
        }
        */

        #endregion SpecialFolders

        #region "Compress and Uncompress Methods"

        // stream helper methods

        public static MemoryStream ReadStringToMemoryStream(String inputString)
        {
            return new MemoryStream(Encoding.Default.GetBytes(inputString));
        }

        public static MemoryStream ReadStringToMemoryStream(String inputString, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(inputString));
        }

        public static MemoryStream ReadFileToMemoryStream(String filePathStr)
        {
            return ReadFileToMemoryStream(filePathStr, FileMode.Open, FileShare.Read, FileAccess.Read, 4096);
        }

        public static MemoryStream ReadFileToMemoryStream(String filePathStr, Int32 bufferSize)
        {
            return ReadFileToMemoryStream(filePathStr, FileMode.Open, FileShare.Read, FileAccess.Read, bufferSize);
        }

        public static MemoryStream ReadFileToMemoryStream(String filePathStr, FileMode mode, FileShare share, FileAccess access, Int32 bufferSize)
        {
            var ms = new MemoryStream();
            if (File.Exists(filePathStr))
            {
                using (var fs = new FileStream(filePathStr, mode, access, share, bufferSize))
                {
                    using (var br = new BinaryReader(fs, Encoding.Default))
                    {
                        const long maxint = Int32.MaxValue / 2; // Reduce 2GB limit to around 1GB for Int32.MaxValue********
                        var length = fs.Length < maxint ? (Int32)fs.Length : (Int32)maxint;
                        Int32 buffersize;
                        ms.Position = 0;
                        if (bufferSize > 0)
                        {
                            buffersize = length > bufferSize ? bufferSize : length;
                        }
                        else
                        {
                            buffersize = length > 4096 ? 4096 : length;
                        }
                        var bytes = new Byte[buffersize];

                        while (true) // Loops Rule!!!!!!!!
                        {
                            var bytecount = br.Read(bytes, 0, bytes.Length);
                            if (bytecount > 0)
                            {
                                ms.Write(bytes, 0, bytecount);
                            }
                            else
                            {
                                fs.Flush();
                                break;
                            }
                        }
                        ms.Position = 0;
                        br.Close();
                    }
                    fs.Close();
                }
            }
            return ms;
        }

        public static Byte[] ReadStreamToBytes(Stream inputStream)
        {
            var ms = (MemoryStream)inputStream;
            return ms.ToArray();
        }

        public static String ReadBytesToBase64(Byte[] inputBytes)
        {
            return Convert.ToBase64String(inputBytes);
        }

        public static String ReadStreamToBase64(MemoryStream inputStream)
        {
            return ReadBytesToBase64(inputStream.ToArray());
        }

        public static Byte[] ReadBase64ToBytes(String inputString)
        {
            return Convert.FromBase64String(inputString);
        }

        public static String ReadFile(String filePathStr)
        {
            var result = String.Empty;
            var fi = new FileInfo(filePathStr);
            if (fi.Exists)
            {
                result = fi.OpenText().ReadToEnd();
            }
            return result;
        }

        public static Boolean WriteFile(String fileInputStr, String filePathStr, Boolean append = false)
        {
            using (var sw = new StreamWriter(filePathStr, append))
            {
                sw.WriteLine(fileInputStr);
            }
            return true;
        }

        public static Boolean AppendFile(Byte[] input, String filePathStr, int bufferSize = 4096)
        {
            var fi = new FileInfo(filePathStr);
            using (var fs = fi.OpenFileStream(FileMode.Append, FileAccess.Write, FileShare.None, bufferSize, 30000, false))
            {
                fs.Seek(0, SeekOrigin.End);
                fs.Write(input, 0, input.Length);
                fs.Flush();
            }
            return true;
        }

        #endregion "Compress and Uncompress Methods"

        #endregion "Methods"
    }
}