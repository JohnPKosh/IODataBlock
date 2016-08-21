using Newtonsoft.Json;
using System;

namespace Business.Common.GenericRequests
{
    public interface IRequestObject<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string CommandName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        T RequestData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string CorrelationId { get; set; }

        DateTime DateCreatedUtc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string ExceptionGroup { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string HostComputerName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string HostUserName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string HostUserDomain { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string ExecutingAssemblyFullName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string CallingAssemblyFullName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string EntryAssemblyFullName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string TypeName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string MemberName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string ParentName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string AppId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string ClientName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string ClientIp { get; set; }
    }
}