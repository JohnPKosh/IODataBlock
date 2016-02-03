using System;

namespace NsRest
{
    public class NetSuiteScriptSetting : INetSuiteScriptSetting
    {
        public NetSuiteScriptSetting() { }

        public NetSuiteScriptSetting(String scriptName, String deploymentName)
        {
            ScriptName = scriptName;
            DeplomentName = deploymentName;
        }

        public static NetSuiteScriptSetting Create(String scriptName, String deploymentName)
        {
            return new NetSuiteScriptSetting(scriptName,deploymentName);
        }

        public string ScriptName { get; set; }
        public string DeplomentName { get; set; }
    }
}
