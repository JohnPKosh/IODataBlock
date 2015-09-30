using System;
using Business.Common.Exceptions;
using Newtonsoft.Json;

namespace Business.Common.Responses
{
    public interface IResponseObject<TIn, TOut>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        TIn RequestData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String CorrelationId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        TOut ResponseData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IResponseCode ResponseCode { get; set; }

        bool HasExceptions { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        int ExceptionCount { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IExceptionObjectList ExceptionList { get; set; }

        string ToJson(bool indented = false);
    }
}