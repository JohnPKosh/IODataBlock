using Newtonsoft.Json;

namespace HubSpot.Models.Companies
{
    public class CompanyContactIdViewModel : ModelBase<CompanyContactIdViewModel>
    {

        #region Public Properties

        [JsonProperty("vidOffset")]
        public int vidOffset { get; set; }

        [JsonProperty("vids")]
        public int[] vids { get; set; }

        [JsonProperty("hasMore")]
        public bool hasMore { get; set; }

        #endregion

    }
}
