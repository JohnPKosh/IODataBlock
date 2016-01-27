using System.Collections.Generic;
using System.IO;
using Business.Common.Configuration;
using Business.Common.IO;
using Business.Common.System.States;
using HubSpot.Models.Properties;
using HubSpot.Services.Companies;
using Newtonsoft.Json;

namespace HubSpot.Models.Companies
{
    public class CompanyUpdateModel : ModelBase<CompanyUpdateModel>
    {

        public CompanyUpdateModel()
        {
            // TODO: determine if string hapikey needs added to class signature.
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
            var jsonFilePath = Path.Combine(IOUtility.AppDataFolderPath, @"CompanyPropertyList.json");
            var propertyManager = new PropertyManager(new CompanyPropertyService(_hapiKey), new JsonFileLoader(new FileInfo(jsonFilePath)));
            ManagedProperties = propertyManager.Properties;
        }

        private readonly string _hapiKey;
        internal List<PropertyTypeModel> ManagedProperties;
            
        [JsonProperty("properties")]
        public HashSet<PropertyUpdateValue> Properties { get; set; }
        
    }
}
