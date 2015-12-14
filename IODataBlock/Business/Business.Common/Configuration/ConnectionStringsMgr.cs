using System;
using System.Configuration;
using System.Web.Configuration;
using Business.Common.IO;

namespace Business.Common.Configuration
{
    public partial class ConfigMgr
    {
        protected internal class ConnectionStringsMgr
        {
            #region "Class Initialization"

            //protected internal ConnectionStringsMgr()
            //{
            //    //ProtectConnectionStrings();
            //}

            #endregion "Class Initialization"

            #region "Methods"

            /// <summary>
            /// Protect ConnectionStrings in App.config File
            /// </summary>
            protected internal void ProtectConnectionStrings()
            {
                global::System.Configuration.Configuration config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var section = config.GetSection("connectionStrings") as ConnectionStringsSection;
                if (section == null) return;
                if (section.SectionInformation.IsProtected) return;
                section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                section.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }

            /// <summary>
            /// UnProtect AppSettings in .config File
            /// </summary>
            protected internal void UnProtectConnectionStrings()
            {
                global::System.Configuration.Configuration config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var section = config.GetSection("connectionStrings") as ConnectionStringsSection;
                if (section == null) return;
                if (!section.SectionInformation.IsProtected) return;
                section.SectionInformation.UnprotectSection();
                section.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }

            protected internal String GetConnectionStringByName(String name)
            {
                var settings = ConfigurationManager.ConnectionStrings[name];
                return settings != null ? settings.ConnectionString : null;
            }

            protected internal String GetProviderByName(String name)
            {
                var settings = ConfigurationManager.ConnectionStrings[name];
                return settings != null ? settings.ProviderName : null;
            }

            protected internal void SetElementByName(String name, String value)
            {
                global::System.Configuration.Configuration config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var connStrings = config.GetSection("connectionStrings") as ConnectionStringsSection;
                if (connStrings == null) return;
                if (connStrings.ConnectionStrings[name] != null)
                {
                    connStrings.ConnectionStrings.Remove(name);
                }
                var connectionElement = new ConnectionStringSettings(name, value);
                connStrings.ConnectionStrings.Add(connectionElement);
                connStrings.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }

            protected internal void SetElementByName(String name, String value, String provider)
            {
                global::System.Configuration.Configuration config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var connStrings = config.GetSection("connectionStrings") as ConnectionStringsSection;
                if (connStrings == null) return;
                if (connStrings.ConnectionStrings[name] != null)
                {
                    connStrings.ConnectionStrings.Remove(name);
                }
                var connectionElement = new ConnectionStringSettings(name, value, provider);
                connStrings.ConnectionStrings.Add(connectionElement);
                connStrings.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }

            protected internal void RemoveElementByName(String name)
            {
                global::System.Configuration.Configuration config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var connStrings = config.GetSection("connectionStrings") as ConnectionStringsSection;
                if (connStrings == null) return;
                if (connStrings.ConnectionStrings[name] == null) return;
                connStrings.ConnectionStrings.Remove(name);
                connStrings.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }





            #endregion "Methods"
        }
    }
}
