using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Business.Common.Exceptions
{
    public interface IExceptionObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String ExceptionGroup { get; set; }

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