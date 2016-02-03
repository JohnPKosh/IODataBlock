using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.IO;
using Business.Common.System;
using Business.Common.System.States;
using HubSpot.Models.Properties;
using HubSpot.Services.Companies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
