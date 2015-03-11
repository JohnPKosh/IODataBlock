using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Business.Common.Requests
{
    public interface IRequestObject
    {

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        object RequestData { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        String CorrelationId { get; set; }

    }
}
