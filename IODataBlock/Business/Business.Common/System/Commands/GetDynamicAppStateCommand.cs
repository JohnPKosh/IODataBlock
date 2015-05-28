using System;
using Business.Common.Requests;
using Business.Common.System.App;

namespace Business.Common.System.Commands
{
    public class GetDynamicAppStateCommand : CommandObjectBase
    {
        //public override string CommandName
        //{
        //    get { return "Get"; }
        //}

        public override string Description
        {
            get { return "DynamicAppStateGet - Get the current app state."; }
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
            return new GetDynamicAppStateCommand
            {
                RequestObject = requestObject
                ,
                CommandFunction = o => DynamicAppState.Instance.Value
            };
        }
    }
}