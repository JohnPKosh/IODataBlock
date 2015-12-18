using System;
using System.Configuration;
using System.IO;

namespace Business.Common.Configuration
{
    public partial class ConfigMgr
    {
        #region Class Initialization

        //public ConfigMgr()
        //{

        //}

        #endregion Class Initialization

        #region Fields and Properties

        private readonly ConnectionStringsMgr _connectionStrings = new ConnectionStringsMgr();
        private readonly AppSettingsMgr _appSettings = new AppSettingsMgr();

        #endregion Fields and Properties

        #region "Methods"

        public void ProtectSettings()
        {
            _connectionStrings.ProtectConnectionStrings();
            _appSettings.ProtectAppSettings();
        }

        #region ConnectionString Section

        public String GetConnectionString(String name)
        {
            return _connectionStrings.GetConnectionStringByName(name);
        }

        public String GetConnectionProvider(String name)
        {
            return _connectionStrings.GetProviderByName(name);
        }

        public void SetConnectionString(String name, String value)
        {
            _connectionStrings.SetElementByName(name, value);
        }

        public void SetConnectionString(String name, String value, String provider)
        {
            _connectionStrings.SetElementByName(name, value, provider);
        }

        public void RemoveConnectionString(String name)
        {
            _connectionStrings.RemoveElementByName(name);
        }

        public void ProtectConnectionStrings()
        {
            _connectionStrings.ProtectConnectionStrings();
        }

        public void UnProtectConnectionStrings()
        {
            _connectionStrings.UnProtectConnectionStrings();
        }

        #endregion ConnectionString Section

        #region AppSettings Section

        public String GetAppSetting(String name)
        {
            return _appSettings.GetAppSettingByName(name);
        }

        public void SetAppSetting(String name, String value)
        {
            _appSettings.SetElementByName(name, value);
        }

        public void RemoveAppSetting(String name)
        {
            _appSettings.RemoveElementByName(name);
        }

        public void ProtectAppSettings()
        {
            _appSettings.ProtectAppSettings();
        }

        public void UnProtectAppSettings()
        {
            _appSettings.UnProtectAppSettings();
        }

        public void SetCspDefaults(bool deleteOnLoad = true)
        {
            _appSettings.SetCspDefaults(deleteOnLoad);
            // add additional default setting initialization below.
        }

        public bool TryCreateCsp(string napK = null, string naK = null, string napI = null, string naI = null, bool deleteOnLoad = true)
        {
            var rv = CspDefault.TryCreate(napK, naK, napI, naI);
            if (rv) _appSettings.SetCspDefaults(deleteOnLoad);
            return rv;
        }

        public byte[] GetAesKBytes()
        {
            return _appSettings.GetAesKBytes();
        }

        public byte[] GetAesIvBytes()
        {
            return _appSettings.GetAesIvBytes();
        }

        public byte[] GetTripleDesKBytes()
        {
            return _appSettings.GetTripleDesKBytes();
        }

        public byte[] GetTripleDesIvBytes()
        {
            return _appSettings.GetTripleDesIvBytes();
        }

        #endregion AppSettings Section

        #endregion "Methods"
    }
}

/*

// Usage Example

            var config = new ConfigMgr();
            ConnStr = config.GetConnectionString("0302CDRCURRENT");
            BaseFolderPath = config.GetAppSetting("BaseFolderPath");

*/

/*

// Sample App.config:

<?xml version="1.0" encoding="utf-8" ?>
<configuration>
  <appSettings>
    <add key="BaseFolderPath" value="Z:\CarrierCDRBilling\" />
  </appSettings>
  <connectionStrings>
    <add name="0302CDRCURRENT" connectionString="User ID=CDRViewer; Password=*******; Initial Catalog=CDRCURRENT; Data Source=CLEHBSQL0302; Connection Timeout=600;" />
  </connectionStrings>
</configuration>

*/