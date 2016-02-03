using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Business.Common.Configuration;
using Business.Common.IO;
using Business.Common.System;
using Business.Common.System.States;
using HubSpot.Models.Properties;
using HubSpot.Services;
using HubSpot.Services.Companies;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HubSpot.Models.Companies
{
    public class CompanyModel: ModelBase<CompanyModel>
    {
        #region Class Initialization

        public CompanyModel()
        {
            // TODO: determine if string hapikey needs added to class signature.
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
            var jsonFilePath = Path.Combine(IOUtility.AppDataFolderPath, @"CompanyPropertyList.json");
            var propertyManager = new CompanyPropertyManager(new CompanyPropertyService(_hapiKey), new JsonFileLoader(new FileInfo(jsonFilePath)));
            ManagedProperties = propertyManager.Properties;
        }

        #endregion

        #region Private Fields and Properties

        private readonly string _hapiKey;
        internal List<PropertyTypeModel> ManagedProperties;

        #endregion

        #region Public Properties

        [JsonProperty("companyId")]
        public int companyId { get; set; }

        [JsonProperty("portalId")]
        public int portalId { get; set; }

        [JsonProperty("isDeleted")]
        public bool isDeleted { get; set; }

        [JsonProperty("properties")]
        public JObject Properties { get; set; }

        #endregion


        #region Conversion Operators

        static public implicit operator CompanyViewModel(CompanyModel value)
        {
            var rv = new CompanyViewModel
            {
                Properties = new HashSet<PropertyValue>(),
                companyId = value.companyId,
                portalId = value.portalId,
                isDeleted = value.isDeleted,
                ManagedProperties = value.ManagedProperties
            };
            foreach (var p in value.ManagedProperties)
            {
                JToken token;
                if (value.Properties.TryGetValue(p.name, StringComparison.InvariantCulture, out token))
                {
                    var versions = (JArray)token["versions"];
                    if (versions != null)
                    {
                        var ver = versions.ToObject<List<PropertyVersion>>();
                        rv.Properties.Add(new PropertyValue(p.name, token.Value<string>("value"), new HashSet<PropertyVersion>(ver), p));
                    }
                    else
                    {
                        rv.Properties.Add(new PropertyValue(p.name, token.Value<string>("value"), null, p));
                    }
                }
                else
                {
                    rv.Properties.Add(new PropertyValue(p.name, null, null, p));
                }
            }
            return rv;
        }

        #endregion Conversion Operators
    }
}
