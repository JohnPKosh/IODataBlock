using System;
using Business.Common.Exceptions;
using Newtonsoft.Json;

namespace Business.Common.Responses
{
    public interface IResponseObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        object RequestData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String CorrelationId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        object ResponseData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        object ResponseCode { get; set; }

        bool HasExceptions { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        int ExceptionCount { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IExceptionObjectList ExceptionList { get; set; }

        string ToJson(bool indented = false);
    }
}