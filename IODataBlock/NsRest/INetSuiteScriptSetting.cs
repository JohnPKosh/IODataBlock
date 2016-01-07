using System;

namespace NsRest
{
    public interface INetSuiteScriptSetting
    {
        String ScriptName { get; set; }

        String DeplomentName { get; set; }
    }
}