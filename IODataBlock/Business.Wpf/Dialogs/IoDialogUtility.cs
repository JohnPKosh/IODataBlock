//using Microsoft.Win32;
using Business.Common.IO;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;

namespace Business.Wpf.Dialogs
{
    public static class IoDialogUtility
    {
        /* http://msdn.microsoft.com/en-us/library/system.environment.specialfolder.aspx */
        /* var result = IoDialogUtility.ShowOpenFileDialog(out fil, "MyDocuments", "Access 2007 document(.accdb)|*.accdb", ".accdb"); */

        public static bool ShowOpenFileDialog(out string FilePath,
            string InitialDirectoryOrSpecialFolder,
            string Filter,
            string DefaultExt = ".*",
            string Title = "Open File...",
            bool AddExtension = true,
            bool CheckPathExists = true
            )
        {
            FilePath = string.Empty;

            using (var dlg = new OpenFileDialog())
            {
                dlg.Title = Title;
                dlg.DefaultExt = DefaultExt;
                dlg.AddExtension = AddExtension;
                dlg.CheckPathExists = CheckPathExists;
                if (!string.IsNullOrWhiteSpace(InitialDirectoryOrSpecialFolder))
                {
                    dlg.InitialDirectory = IOUtility.GetInitialDirectoryOrSpecialFolder(InitialDirectoryOrSpecialFolder);
                }

                //dlg.Filter = "Access 2007 document(.accdb)|*.accdb";
                dlg.Filter = Filter;
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    FilePath = dlg.FileName;
                    return true;
                }
            }
            return false;
        }

        public static bool ShowSaveFileDialog(out string FilePath,
            string InitialDirectoryOrSpecialFolder,
            string Filter,
            string DefaultExt = ".*",
            string DefaultName = "New Document",
            string Title = "Save File...",
            bool AddExtension = true,
            bool CheckPathExists = true,
            bool OverwritePrompt = true
            )
        {
            using (var dlg = new SaveFileDialog())
            {
                dlg.FileName = DefaultName;
                dlg.Title = Title;
                dlg.DefaultExt = DefaultExt;
                dlg.AddExtension = AddExtension;
                dlg.CheckPathExists = CheckPathExists;
                dlg.OverwritePrompt = OverwritePrompt;
                if (!string.IsNullOrWhiteSpace(InitialDirectoryOrSpecialFolder))
                {
                    dlg.InitialDirectory = IOUtility.GetInitialDirectoryOrSpecialFolder(InitialDirectoryOrSpecialFolder);
                }

                //dlg.Filter = "Access 2007 document(.accdb)|*.accdb";
                dlg.Filter = Filter;
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    FilePath = dlg.FileName;
                    return true;
                }
                else
                {
                    FilePath = string.Empty;
                }
            }
            return false;
        }

        public static bool ShowFolderBrowserDialog(out string selectedFolder,
            string initialFolder = null,
            Environment.SpecialFolder rootFolder = Environment.SpecialFolder.MyDocuments,
            string description = "Browse to Directory...",
            bool showNewFolderButton = true
            )
        {
            selectedFolder = string.Empty;
            using (var dlg = new FolderBrowserDialog())
            {
                if (string.IsNullOrWhiteSpace(initialFolder) || !Directory.Exists(initialFolder.Trim()))
                {
                    dlg.SelectedPath = Environment.GetFolderPath(rootFolder);
                }
                else
                {
                    dlg.SelectedPath = initialFolder.Trim();
                }
                dlg.Description = description;
                dlg.ShowNewFolderButton = showNewFolderButton;
                var result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    selectedFolder = dlg.SelectedPath;
                    return true;
                }
            }
            return false;
        }

        public static bool ShowMyPicturesBrowserDialog(out string selectedFolder,
            string initialFolder = null,
            Environment.SpecialFolder rootFolder = Environment.SpecialFolder.MyPictures,
            string description = "Browse to Directory...",
            bool showNewFolderButton = true
            )
        {
            return ShowFolderBrowserDialog(out selectedFolder, initialFolder, rootFolder, description, showNewFolderButton);
        }

        public static void OpenFileWithDefaultApplication(string TargeFilePath)
        {
            new Thread(() => System.Diagnostics.Process.Start(TargeFilePath)) { IsBackground = true }.Start();
        }

        public static void OpenFileWithSpecificApplication(string AppString, string TargeFilePath)
        {
            new Thread(() => System.Diagnostics.Process.Start(AppString, TargeFilePath)) { IsBackground = true }.Start();
        }
    }
}