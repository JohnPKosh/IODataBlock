using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSuite.RESTlet.Integration
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
