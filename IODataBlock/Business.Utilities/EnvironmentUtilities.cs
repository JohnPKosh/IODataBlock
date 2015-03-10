using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Business.Utilities
{
    public static class EnvironmentUtilities
    {
        public static String GetComputerName()
        {
            var computerName = String.Empty;

            var environmentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            foreach (var de in environmentVariables.Cast<DictionaryEntry>().Where(de => de.Key.ToString() == "COMPUTERNAME"))
            {
                computerName = de.Value.ToString();
            }
            return computerName;
        }

        public static String GetUserDomain()
        {
            var userDomain = String.Empty;
            var environmentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            foreach (var de in environmentVariables.Cast<DictionaryEntry>().Where(de => de.Key.ToString() == "USERDOMAIN"))
            {
                userDomain = de.Value.ToString();
            }
            return userDomain;
        }

        public static String GetUserName()
        {
            return Environment.UserName;
        }

        public static Boolean IsWebAssembly()
        {
            var entry = Assembly.GetEntryAssembly();
            return entry == null || Assembly.GetCallingAssembly().FullName.Contains(@"App_");
        }

        public static String GetAssemblyName()
        {
            return IsWebAssembly() ? Assembly.GetCallingAssembly().FullName : Assembly.GetEntryAssembly().FullName;
        }

        public static String GetAssemblyLocation()
        {
            return IsWebAssembly() ? Assembly.GetCallingAssembly().Location : Assembly.GetEntryAssembly().Location;
        }

        public static String GetAssemblyDirectory()
        {
            return new FileInfo(GetAssemblyLocation()).DirectoryName;
        }

        public static DirectoryInfo GetAssemblyDirectoryInfo()
        {
            return new FileInfo(GetAssemblyLocation()).Directory;
        }
    }
}