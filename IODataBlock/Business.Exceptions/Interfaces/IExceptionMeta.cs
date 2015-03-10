using System;
using Newtonsoft.Json;

namespace Business.Exceptions.Interfaces
{
    public interface IExceptionMeta
    {
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

    }
}