using System;
using Newtonsoft.Json;

namespace Business.Common.GenericRequests
{
    public interface IRequestObject<T>
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string CommandName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        T RequestData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String CorrelationId { get; set; }

        DateTime DateCreatedUtc { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String Title { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String Description { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String ExceptionGroup { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String HostComputerName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String HostUserName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String HostUserDomain { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String ExecutingAssemblyFullName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String CallingAssemblyFullName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String EntryAssemblyFullName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String TypeName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String MemberName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String ParentName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String AppId { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String ClientName { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String ClientIp { get; set; }
    }
}