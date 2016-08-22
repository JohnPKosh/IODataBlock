using Business.Common.Exceptions;

namespace Business.Common.Responses
{
    public static class ResponseExtensions
    {
        public static IResponseObject ToUncompletedResponse(this object requestData, int? id, string code, string correlationId = null)
        {
            ResponseCode rc;
            if (!id.HasValue && string.IsNullOrWhiteSpace(code)) rc = null;
            else rc = new ResponseCode(id, code);
            return new ResponseObject { RequestData = requestData, ResponseCode = rc, CorrelationId = correlationId };
        }

        public static IResponseObject ToUncompletedResponse(this object requestData, IResponseCode responseCode = null, string correlationId = null)
        {
            return new ResponseObject { RequestData = requestData, ResponseCode = responseCode ?? new ResponseCode(400, @"400 Bad Request"), CorrelationId = correlationId };
        }

        public static IResponseObject ToSuccessfullResponse(this object requestData, object responseData, IResponseCode responseCode = null, string correlationId = null)
        {
            return new ResponseObject { RequestData = requestData, ResponseData = responseData, ResponseCode = responseCode, CorrelationId = correlationId };
        }

        public static IResponseObject ToFailedResponse(this object requestData, IExceptionObjectList exceptionObjectList, IResponseCode responseCode = null, string correlationId = null, object responseData = null)
        {
            return new ResponseObject { RequestData = requestData, ResponseCode = responseCode, CorrelationId = correlationId, ExceptionList = exceptionObjectList, ResponseData = responseData };
        }
    }
}