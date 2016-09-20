using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JavaScriptEngineSwitcher.Core;
using JavaScriptEngineSwitcher.Msie;

namespace WebTrackr.App_Start
{
    public class JsEngineSwitcherConfig
    {
        public static void Configure(JsEngineSwitcher engineSwitcher)
        {
            engineSwitcher.EngineFactories
                .AddMsie(new MsieSettings
                {
                    UseEcmaScript5Polyfill = true,
                    UseJson2Library = true
                });

            engineSwitcher.DefaultEngineName = MsieJsEngine.EngineName;
        }

    }
}