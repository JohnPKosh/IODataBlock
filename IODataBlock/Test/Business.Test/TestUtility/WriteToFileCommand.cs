using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Exceptions;
using Business.Common.Requests;
using Business.Common.Responses;
using Business.Common.System;

namespace Business.Test.TestUtility
{
    public class WriteToFileCommand : ICommand, ICommandFactory
    {
        public IRequestObject RequestObject { get; set; }

        public string CommandName
        {
            get { return "WriteToFile"; }
        }

        public string Description
        {
            get { return "WriteToFile"; }
        }

        public IResponseObject Execute()
        {
            // if we needed to short circuit here because of some condition we could do it here!
            //var rv = RequestObject.ToUncompletedResponse(null, RequestObject.CorrelationId);
            try
            {
                // do some work

                var rv = "Did some fake work!";
                var responseCode = "200";

                return RequestObject.ToSuccessfullResponse(rv, responseCode, RequestObject.CorrelationId);
            }
            catch (Exception ex)
            {
                return RequestObject.ToFailedResponse(ExceptionObjectListBase.Create(
                    ex
                    , "WriteToFileCommand failed!"
                    , ex.Message
                    , "Command Exceptions"
                    , typeName: "WriteToFileCommand"
                    , memberName: "Execute"
                    , parentName: "Business.Test.TestUtility")
                    , "500"
                    , RequestObject.CorrelationId
                    );
            }
        }

        public ICommand Create(IRequestObject requestObject)
        {
            return new WriteToFileCommand() { RequestObject = requestObject };
        }
    }
}
