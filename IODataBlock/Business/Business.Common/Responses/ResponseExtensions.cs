using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Exceptions;

namespace Business.Common.Responses
{
    public static class ResponseExtensions
    {

        public static IResponseObject ToUncompletedResponse(this object value, object responseCode = null, string correlationId = null)
        {
            return new ResponseObject() { RequestData = value, ResponseCode = responseCode, CorrelationId = correlationId };
        }

        public static IResponseObject ToSuccessfulResponse(this object value, object responseCode = null, string correlationId = null)
        {
            return new ResponseObject() { RequestData = value, ResponseCode = responseCode, CorrelationId = correlationId };
        }

        public static IResponseObject ToFailedResponse(this object value, IExceptionObjectList exceptionObjectList, object responseCode = null, string correlationId = null, object responseData = null)
        {
            return new ResponseObject() { RequestData = value, ResponseCode = responseCode, CorrelationId = correlationId, ExceptionList = exceptionObjectList, ResponseData = responseData };
        }

    }
}
