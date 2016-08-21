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
                var config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
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
                var config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var section = config.GetSection("connectionStrings") as ConnectionStringsSection;
                if (section == null) return;
                if (!section.SectionInformation.IsProtected) return;
                section.SectionInformation.UnprotectSection();
                section.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }

            protected internal string GetConnectionStringByName(string name)
            {
                var settings = ConfigurationManager.ConnectionStrings[name];
                return settings?.ConnectionString;
            }

            protected internal string GetProviderByName(string name)
            {
                var settings = ConfigurationManager.ConnectionStrings[name];
                return settings?.ProviderName;
            }

            protected internal void SetElementByName(string name, string value)
            {
                var config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
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

            protected internal void SetElementByName(string name, string value, string provider)
            {
                var config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
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

            protected internal void RemoveElementByName(string name)
            {
                var config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var connStrings = config.GetSection("connectionStrings") as ConnectionStringsSection;
                if (connStrings?.ConnectionStrings[name] == null) return;
                connStrings.ConnectionStrings.Remove(name);
                connStrings.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("connectionStrings");
            }





            #endregion "Methods"
        }
    }
}
