using System;
using Business.Common.Exceptions;
using Business.Common.Generic;
using Business.Common.GenericRequests;
using Business.Common.Requests;
using Business.Common.Responses;
using Business.Common.System;

namespace Business.Test.TestUtility
{
    public class ThrowExceptionFromGenericCommand : CommandObjectBase<string, string>
    {
        public override string Description
        {
            get { return "ThrowExceptionFromGenericCommand."; }
        }

        public override Func<IRequestObject<string>, string> CommandFunction { get; set; }

        public override ICommandObject<string, string> Create(IRequestObject<string> requestObject)
        {
            return new ThrowExceptionFromGenericCommand
            {
                RequestObject = requestObject
                ,
                CommandFunction = o =>
                {
                    // if we needed to short circuit here because of some condition we could do it here!
                    //var rv = RequestObject.ToUncompletedResponse(null, RequestObject.CorrelationId);
                    //try
                    //{
                        // do some work

                        var zero = 0;
                        var errorHere = 10 / zero;
                        var neverGetHere = errorHere;

                        var rv = String.Format("hello {0} from ReadFromFileGenericCommand!", o.RequestData);
                        return rv;
                    //}
                    //catch (Exception ex)
                    //{
                    //    throw;
                    //}
                }
            };
        }
    }

}