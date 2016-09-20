using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WebTrakrData.Model.Dto
{
    public class FullContactResponse
    {
        [JsonProperty(PropertyName = "status")]
        public int Status { get; set; }

        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }

        [JsonProperty(PropertyName = "requestId")]
        public string RequestId { get; set; }

        [JsonProperty(PropertyName = "responseData")]
        public JObject ResponseData { get; set; }

        [JsonProperty(PropertyName = "requestData")]
        public JObject RequestData { get; set; }

        [JsonProperty(PropertyName = "requestTypeId")]
        public int RequestTypeId { get; set; }

        [JsonProperty(PropertyName = "userId")]
        public string AspNetUserId { get; set; }
    }
}
