﻿using Business.Common.IO;
using Business.Common.Security;
using System.Configuration;
using System.Web.Configuration;

namespace Business.Common.Configuration
{
    public partial class ConfigMgr
    {
        protected internal class AppSettingsMgr
        {
            #region "Class Initialization"

            //protected internal AppSettingsMgr()
            //{
            //    //ProtectAppSettings();
            //}

            #endregion "Class Initialization"

            #region Methods

            /// <summary>
            /// Protect AppSettings in .config File
            /// </summary>
            protected internal void ProtectAppSettings()
            {
                var config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var appsection = config.GetSection("appSettings") as AppSettingsSection;
                if (appsection == null) return;
                if (appsection.SectionInformation.IsProtected) return;
                appsection.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                appsection.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

            /// <summary>
            /// UnProtect AppSettings in .config File
            /// </summary>
            protected internal void UnProtectAppSettings()
            {
                var config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var appsection = config.GetSection("appSettings") as AppSettingsSection;
                if (appsection == null) return;
                if (!appsection.SectionInformation.IsProtected) return;
                appsection.SectionInformation.UnprotectSection();
                appsection.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

            /// <summary>
            /// Get AppSetting By Name from App.Config
            /// </summary>
            /// <param name="name">name of element</param>
            /// <returns>String</returns>
            protected internal string GetAppSettingByName(string name)
            {
                return ConfigurationManager.AppSettings[name];
            }

            /// <summary>
            /// Set AppSetting By Name from App.Config
            /// </summary>
            /// <param name="name">name of element</param>
            /// <param name="value">string value to save</param>
            protected internal void SetElementByName(string name, string value)
            {
                var config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var appsection = config.GetSection("appSettings") as AppSettingsSection;
                if (appsection == null) return;
                if (appsection.Settings[name] != null)
                {
                    appsection.Settings.Remove(name);
                }
                appsection.Settings.Add(name, value);
                appsection.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

            /// <summary>
            /// Remove AppSetting Element By Name
            /// </summary>
            /// <param name="name">name of element</param>
            protected internal void RemoveElementByName(string name)
            {
                var config = IOUtility.IsWebAssembly() ? ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None) : WebConfigurationManager.OpenWebConfiguration(null);
                var appsection = config.GetSection("appSettings") as AppSettingsSection;
                if (appsection == null) return;
                if (appsection.Settings[name] != null)
                {
                    appsection.Settings.Remove(name);
                }
                appsection.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection("appSettings");
            }

            protected internal void SetCspDefaults(bool deleteOnLoad = true)
            {
                CspDefault d;
                if (!CspDefault.TryLoad(out d, deleteOnLoad)) return;
                ProtectAppSettings();

                SetElementByName("cspsK", d.NaK);
                SetElementByName("csppK", d.NapK);
                SetElementByName("cspsI", d.NaI);
                SetElementByName("csppI", d.NapI);
            }

            protected internal byte[] GetAesKBytes()
            {
                return RngCrypto.GetAesKeyBytes(GetAppSettingByName("csppK"), GetAppSettingByName("cspsK"));
            }

            protected internal byte[] GetAesIvBytes()
            {
                return RngCrypto.GetAesIvBytes(GetAppSettingByName("csppI"), GetAppSettingByName("cspsI"));
            }

            protected internal byte[] GetTripleDesKBytes()
            {
                return RngCrypto.GetTripleDesKeyBytes(GetAppSettingByName("csppK"), GetAppSettingByName("cspsK"));
            }

            protected internal byte[] GetTripleDesIvBytes()
            {
                return RngCrypto.GetTripleDesIvBytes(GetAppSettingByName("csppI"), GetAppSettingByName("cspsI"));
            }

            #endregion Methods
        }
    }
}