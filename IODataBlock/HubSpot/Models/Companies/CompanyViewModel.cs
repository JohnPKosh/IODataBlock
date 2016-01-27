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
    public class CompanyViewModel : ModelBase<CompanyViewModel>
    {
        #region Class Initialization

        public CompanyViewModel()
        {
            // TODO: determine if string hapikey needs added to class signature.
            var configMgr = new ConfigMgr();
            _hapiKey = configMgr.GetAppSetting("hapikey");
            var jsonFilePath = Path.Combine(IOUtility.AppDataFolderPath, @"CompanyPropertyList.json");
            var propertyManager = new PropertyManager(new CompanyPropertyService(_hapiKey), new JsonFileLoader(new FileInfo(jsonFilePath)));
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
        public HashSet<PropertyValue> Properties { get; set; }


        #endregion


        #region Conversion Operators

        static public implicit operator CompanyUpdateModel(CompanyViewModel value)
        {
            var rv = new CompanyUpdateModel { Properties = new HashSet<PropertyUpdateValue>() };
            foreach (var p in value.Properties)
            {
                var prop = value.ManagedProperties.FirstOrDefault(x => x.name == p.Key);
                if (prop == null || prop.readOnlyValue || prop.readOnlyDefinition || prop.mutableDefinitionNotDeletable || prop.calculated) continue;
                rv.Properties.Add(new PropertyUpdateValue(p.Key, p.Value, prop));
            }
            rv.ManagedProperties = value.ManagedProperties;
            return rv;
        }

        static public implicit operator Dictionary<string, object>(CompanyViewModel value)
        {
            string email = null;
            string leadGuid = null;

            var rv = new Dictionary<string, object>
            {
                {"companyId", value.companyId},
                {"portalId", value.portalId},
                {"isDeleted", value.isDeleted}
            };

            foreach (var p in value.Properties)
            {
                var prop = value.ManagedProperties.FirstOrDefault(x => x.name == p.Key);
                if (prop == null) continue;
                
                rv.Add(p.Key, GetPropertyValueByType(p.Value, prop.type));
            }
            return rv;
        }

        public static object GetPropertyValueByType(string value, string type)
        {
            switch (type)
            {
                case "datetime":
                    DateTime? ts = new UnixMsTimestamp(value);
                    return ts?.ToLocalTime();
                case "bool":
                    return string.IsNullOrWhiteSpace(value) ? (bool?) null : bool.Parse(value);
                default:
                    return value;
            }
        }

        #endregion Conversion Operators

    }
}
