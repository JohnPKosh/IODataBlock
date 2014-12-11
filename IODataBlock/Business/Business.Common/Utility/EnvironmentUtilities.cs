using System;
using System.Collections;
using System.Linq;
using System.Reflection;

namespace Business.Common.Utility
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
    }
}