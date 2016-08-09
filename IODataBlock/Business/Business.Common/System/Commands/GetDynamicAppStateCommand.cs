using System;
using Business.Common.Requests;
using Business.Common.System.App;

namespace Business.Common.System.Commands
{
    public class GetDynamicAppStateCommand : CommandObjectBase
    {
        public override string Description
        {
            get { return "GetDynamicAppState - Gets the current app state."; }
        }

        public override Func<IRequestObject, object> CommandFunction { get; set; }

        public override ICommandObject Create(IRequestObject requestObject)
        {
            return new GetDynamicAppStateCommand
            {
                RequestObject = requestObject
                ,
                CommandFunction = o => DynamicAppState.Instance.Value
            };
        }
    }
}