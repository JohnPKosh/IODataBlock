using System;
using System.IO;
using Business.Common.Requests;
using Business.Common.System.App;
using Business.Common.System.States;

namespace Business.Common.System.Commands
{
    public class SaveDynamicAppStateCommand : CommandObjectBase
    {
        public override string Description
        {
            get { return "SaveDynamicAppState - Saves the current app state."; }
        }

        public override Func<IRequestObject, object> CommandFunction { get; set; }

        public override ICommandObject Create(IRequestObject requestObject)
        {
            return new SaveDynamicAppStateCommand
            {
                RequestObject = requestObject
                ,
                CommandFunction = o =>
                {
                    // add a command here!
                    if (o.RequestData is IDynamicLoader)
                    {
                        DynamicAppState.Instance.Save(o.RequestData as IDynamicLoader);
                    }
                    else if (o.RequestData == null)
                    {
                        /* TODO may want to actually check configuration file for specified directory settings and some kind of enum for system IDynamicStateLoader type etc. */
                        /* TODO create DynamicBsonFileLoader */
                        DynamicAppState.Instance.Save(
                            new DynamicJsonFileLoader(
                                new FileInfo(Path.Combine(Environment.CurrentDirectory, @"DynamicAppState.json"))));
                    }
                    else
                    {
                        throw new ArgumentException(
                            "RequestData must be of type IDynamicStateLoader or null (for system default).");
                    }
                    return true;
                }
            };
        }
    }
}