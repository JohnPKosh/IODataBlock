using System;
using Business.Common.Requests;
using Business.Common.System.App;

namespace Business.Common.System.Commands
{
    public class SetDynamicAppStateCommand : CommandObjectBase
    {
        public override string Description => "SetDynamicAppState - Sets the current app state.";

        public override Func<IRequestObject, object> CommandFunction { get; set; }

        public override ICommandObject Create(IRequestObject requestObject)
        {
            return new SetDynamicAppStateCommand
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