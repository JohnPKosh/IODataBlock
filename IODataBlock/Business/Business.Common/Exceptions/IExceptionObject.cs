using System.Collections.Generic;
using Newtonsoft.Json;

namespace Business.Common.Exceptions
{
    public interface IExceptionObject
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string ExceptionGroup { get; set; }

        ExceptionLogLevelType LogLevel { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IDictionary<string, object> Data { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string HelpLink { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        int HResult { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        IExceptionObject InnerExceptionDetail { get; }

        string Message { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Source { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string StackTrace { get; set; }

        //Exception AsException();
    }
}