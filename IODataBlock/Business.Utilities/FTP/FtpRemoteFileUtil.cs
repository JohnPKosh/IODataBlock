using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FtpLib;

namespace Business.Utilities.FTP
{
    public class FtpRemoteFileUtil
    {
        public List<String> GetRemoteFileList(String hostName, String userName, String password, String remoteDirectory, String mask = "")
        {
            FtpFileInfo[] files = null;
            using (var ftp = new FtpConnection(hostName, userName, password))
            {
                ftp.Open(); /* Open the FTP connection */
                ftp.Login(); /* Login using previously provided credentials */
                if (ftp.DirectoryExists(remoteDirectory))
                {
                    ftp.SetCurrentDirectory(remoteDirectory); /* change current directory */

                    files = String.IsNullOrWhiteSpace(mask) ? ftp.GetFiles() : ftp.GetFiles(mask);
                }
            }
            return files != null ? files.Select(f => f.Name).ToList() : null;
        }

        public FtpFileInfo[] GetFileInfo(String hostName, String userName, String password, String remoteDirectory, String mask = "")
        {
            FtpFileInfo[] files;
            using (var ftp = new FtpConnection(hostName, userName, password))
            {
                ftp.Open(); /* Open the FTP connection */
                ftp.Login(); /* Login using previously provided credentials */
                if (!ftp.DirectoryExists(remoteDirectory)) return null;
                ftp.SetCurrentDirectory(remoteDirectory); /* change current directory */
                files = String.IsNullOrWhiteSpace(mask) ? ftp.GetFiles() : ftp.GetFiles(mask);
            }
            return files;
        }

        public Boolean GetFile(String hostName, String userName, String password, FileInfo localFile, String remoteFileName = null, String remoteDirectory = @"/", Boolean overwrite = true)
        {
            const bool rv = false;
            using (var ftp = new FtpConnection(hostName, userName, password))
            {
                ftp.Open(); /* Open the FTP connection */
                ftp.Login(); /* Login using previously provided credentials */
                if (!ftp.DirectoryExists(remoteDirectory)) return rv;
                ftp.SetCurrentDirectory(remoteDirectory); /* change current directory */
                if (ftp.FileExists(remoteFileName)) return false;
                if (localFile.Exists)
                {
                    if (!overwrite) return true;
                    localFile.Delete();
                    ftp.GetFile(remoteFileName,localFile.FullName,false);
                }
                else
                {
                    ftp.GetFile(remoteFileName, localFile.FullName, false);
                }
                return true;
            }
        }

        public Boolean PutFile(String hostName, String userName, String password, FileInfo localFile, String remoteDirectory = @"/", Boolean overwrite = true)
        {
            const bool rv = false;
            using (var ftp = new FtpConnection(hostName, userName, password))
            {
                ftp.Open(); /* Open the FTP connection */
                ftp.Login(); /* Login using previously provided credentials */
                if (!ftp.DirectoryExists(remoteDirectory)) return rv;
                ftp.SetCurrentDirectory(remoteDirectory); /* change current directory */
                if (ftp.FileExists(localFile.Name))
                {
                    if (!overwrite) return true;
                    ftp.RemoveFile(localFile.Name);
                    ftp.PutFile(localFile.FullName);
                }
                else
                {
                    ftp.PutFile(localFile.FullName);
                }
                return true;
            }
        }
    }
}