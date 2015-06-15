//using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using Business.Common.IO;

namespace Business.Wpf.Dialogs
{
    public static class IoDialogUtility
    {
        /* http://msdn.microsoft.com/en-us/library/system.environment.specialfolder.aspx */
        /* var result = IoDialogUtility.ShowOpenFileDialog(out fil, "MyDocuments", "Access 2007 document(.accdb)|*.accdb", ".accdb"); */

        public static bool ShowOpenFileDialog(out String FilePath,
            String InitialDirectoryOrSpecialFolder,
            String Filter,
            String DefaultExt = ".*",
            String Title = "Open File...",
            Boolean AddExtension = true,
            Boolean CheckPathExists = true
            )
        {
            FilePath = String.Empty;

            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                dlg.Title = Title;
                dlg.DefaultExt = DefaultExt;
                dlg.AddExtension = AddExtension;
                dlg.CheckPathExists = CheckPathExists;
                if (!String.IsNullOrWhiteSpace(InitialDirectoryOrSpecialFolder))
                {
                    dlg.InitialDirectory = IOUtility.GetInitialDirectoryOrSpecialFolder(InitialDirectoryOrSpecialFolder);
                }

                //dlg.Filter = "Access 2007 document(.accdb)|*.accdb";
                dlg.Filter = Filter;
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    FilePath = dlg.FileName;
                    return true;
                }
            }
            return false;
        }

        public static bool ShowSaveFileDialog(out String FilePath,
            String InitialDirectoryOrSpecialFolder,
            String Filter,
            String DefaultExt = ".*",
            String DefaultName = "New Document",
            String Title = "Save File...",
            Boolean AddExtension = true,
            Boolean CheckPathExists = true,
            Boolean OverwritePrompt = true
            )
        {
            using (SaveFileDialog dlg = new SaveFileDialog())
            {
                dlg.FileName = DefaultName;
                dlg.Title = Title;
                dlg.DefaultExt = DefaultExt;
                dlg.AddExtension = AddExtension;
                dlg.CheckPathExists = CheckPathExists;
                dlg.OverwritePrompt = OverwritePrompt;
                if (!String.IsNullOrWhiteSpace(InitialDirectoryOrSpecialFolder))
                {
                    dlg.InitialDirectory = IOUtility.GetInitialDirectoryOrSpecialFolder(InitialDirectoryOrSpecialFolder);
                }

                //dlg.Filter = "Access 2007 document(.accdb)|*.accdb";
                dlg.Filter = Filter;
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    FilePath = dlg.FileName;
                    return true;
                }
                else
                {
                    FilePath = String.Empty;
                }
            }
            return false;
        }

        public static bool ShowFolderBrowserDialog(out String selectedFolder,
            String initialFolder = null,
            Environment.SpecialFolder rootFolder = Environment.SpecialFolder.MyDocuments,
            String description = "Browse to Directory...",
            Boolean showNewFolderButton = true
            )
        {
            selectedFolder = String.Empty;
            using (var dlg = new FolderBrowserDialog())
            {
                if (String.IsNullOrWhiteSpace(initialFolder) || !Directory.Exists(initialFolder.Trim()))
                {
                    dlg.SelectedPath = Environment.GetFolderPath(rootFolder);
                }
                else
                {
                    dlg.SelectedPath = initialFolder.Trim();
                }
                dlg.Description = description;
                dlg.ShowNewFolderButton = showNewFolderButton;
                DialogResult result = dlg.ShowDialog();
                if (result == DialogResult.OK)
                {
                    selectedFolder = dlg.SelectedPath;
                    return true;
                }
                
            }
            return false;
        }

        public static bool ShowMyPicturesBrowserDialog(out String selectedFolder,
            String initialFolder = null,
            Environment.SpecialFolder rootFolder = Environment.SpecialFolder.MyPictures,
            String description = "Browse to Directory...",
            Boolean showNewFolderButton = true
            )
        {
            return ShowFolderBrowserDialog(out selectedFolder, initialFolder, rootFolder, description, showNewFolderButton);
        }

        public static void OpenFileWithDefaultApplication(String TargeFilePath)
        {
            new Thread(() => System.Diagnostics.Process.Start(TargeFilePath)) { IsBackground = true }.Start();
        }

        public static void OpenFileWithSpecificApplication(String AppString, String TargeFilePath)
        {
            new Thread(() => System.Diagnostics.Process.Start(AppString, TargeFilePath)) { IsBackground = true }.Start();
        }
    }
}