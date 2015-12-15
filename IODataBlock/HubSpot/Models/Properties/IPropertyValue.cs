using Newtonsoft.Json;
using System.Collections.Generic;

namespace HubSpot.Models.Properties
{
    internal interface IPropertyValue : IPropertyUpdateValue
    {
        [JsonProperty("versions")]
        HashSet<PropertyVersion> Versions { get; set; }
    }
}