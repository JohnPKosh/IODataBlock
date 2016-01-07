using System;

namespace NetSuite.RESTlet.Integration
{
    public interface INetSuiteScriptSetting
    {
        String ScriptName { get; set; }

        String DeplomentName { get; set; }
    }
}