using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Business.Exceptions.Interfaces
{
    public interface IExceptionObject
    {
        ExceptionLogLevelType LogLevel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IDictionary<String, Object> Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String HelpLink { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        int HResult { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IExceptionObject InnerExceptionDetail { get; }

        String Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String Source { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String StackTrace { get; set; }
    }
}