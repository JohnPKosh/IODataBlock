using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Business.Common.System.App
{
    public static class EnvironmentUtilities
    {
        public static string GetComputerName()
        {
            var computerName = string.Empty;

            var environmentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            foreach (var de in environmentVariables.Cast<DictionaryEntry>().Where(de => de.Key.ToString() == "COMPUTERNAME"))
            {
                computerName = de.Value.ToString();
            }
            return computerName;
        }

        public static string GetUserDomain()
        {
            var userDomain = string.Empty;
            var environmentVariables = Environment.GetEnvironmentVariables(EnvironmentVariableTarget.Process);
            foreach (var de in environmentVariables.Cast<DictionaryEntry>().Where(de => de.Key.ToString() == "USERDOMAIN"))
            {
                userDomain = de.Value.ToString();
            }
            return userDomain;
        }

        public static string GetUserName()
        {
            return Environment.UserName;
        }

        public static bool IsWebAssembly()
        {
            var entry = Assembly.GetEntryAssembly();
            return entry == null || Assembly.GetCallingAssembly().FullName.Contains(@"App_");
        }

        public static string GetAssemblyName()
        {
            return IsWebAssembly() ? Assembly.GetCallingAssembly().FullName : Assembly.GetEntryAssembly().FullName;
        }

        public static string GetAssemblyLocation()
        {
            return IsWebAssembly() ? Assembly.GetCallingAssembly().Location : Assembly.GetEntryAssembly().Location;
        }

        public static string GetAssemblyDirectory()
        {
            return new FileInfo(GetAssemblyLocation()).DirectoryName;
        }

        public static DirectoryInfo GetAssemblyDirectoryInfo()
        {
            return new FileInfo(GetAssemblyLocation()).Directory;
        }
    }
}