using Newtonsoft.Json;

namespace Business.Common.Responses
{
    public interface IResponseCode
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        int? Id { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        string Code { get; set; }

        string ToString();
    }
}