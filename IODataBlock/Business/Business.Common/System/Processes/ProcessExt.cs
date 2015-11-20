﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Business.Common.System.Processes
{
    public static class ProcessExt
    {
        public static void OpenFileWithDefaultApplication(this String targeFilePath)
        {
            Process.Start(targeFilePath);
        }

        public static void OpenFileWithDefaultApplication(this String targeFilePath, string arguments)
        {
            Process.Start(targeFilePath, arguments);
        }

        public static void OpenFileWithDefaultApplication(this FileInfo targeFileInfo)
        {
            Process.Start(targeFileInfo.FullName);
        }

        public static void OpenFileWithDefaultApplication(this FileInfo targeFileInfo, string arguments)
        {
            Process.Start(targeFileInfo.FullName, arguments);
        }

        public static IEnumerable<String> GetRunningLocalProcesses()
        {
            var processes = Process.GetProcesses();
            var rv = processes.Select(x => x.ProcessName);
            return rv;
        }

    }
}