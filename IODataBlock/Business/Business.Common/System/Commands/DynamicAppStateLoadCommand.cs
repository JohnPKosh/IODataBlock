using System;
using System.IO;
using Business.Common.Requests;
using Business.Common.System.App;
using Business.Common.System.States;

namespace Business.Common.System.Commands
{
    public class DynamicAppStateLoadCommand : CommandObjectBase
    {
        public override string CommandName
        {
            get { return "Load"; }
        }

        public override string Description
        {
            get { return "DynamicAppStateLoad - Loads the current app state."; }
        }

        public override object SuccessResponseCode
        {
            get { return "200"; }
        }

        public override object ErrorResponseCode
        {
            get { return "500"; }
        }

        public override Func<IRequestObject, object> CommandFunction { get; set; }

        public override ICommandObject Create(IRequestObject requestObject)
        {
            return new DynamicAppStateSaveCommand
            {
                RequestObject = requestObject
                ,
                CommandFunction = o =>
                {
                    // add a command here!
                    if (o.RequestData is IDynamicStateLoader)
                    {
                        DynamicAppState.Instance.Load(o.RequestData as IDynamicStateLoader);
                    }
                    else if (o.RequestData == null)
                    {
                        /* TODO may want to actually check configuration file for specified directory settings and some kind of enum for system IDynamicStateLoader type etc. */
                        /* TODO create DynamicBsonFileLoader */
                        DynamicAppState.Instance.Load(
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