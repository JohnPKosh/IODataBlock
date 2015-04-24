using System;
using Business.Common.Requests;
using Business.Common.System.App;

namespace Business.Common.System.Commands
{
    public class DynamicAppStateSetCommand : CommandObjectBase
    {
        public override string CommandName
        {
            get { return "Set"; }
        }

        public override string Description
        {
            get { return "DynamicAppStateSet - Set the current app state."; }
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
            return new DynamicAppStateSetCommand
            {
                RequestObject = requestObject
                ,
                CommandFunction = o =>
                {
                    // add a command here!
                    DynamicAppState.Instance.Value = o.RequestData;
                    return true;
                }
            };
        }
    }
}