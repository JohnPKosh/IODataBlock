using Newtonsoft.Json;
using System.Collections.Generic;

// ReSharper disable InconsistentNaming

namespace HubSpot.Models.Properties
{
    public class PropertyTypeModel
    {
        public string name { get; set; }
        public string label { get; set; }
        public string description { get; set; }
        public string groupName { get; set; }
        public string type { get; set; }
        public string fieldType { get; set; }
        public bool hidden { get; set; }
        public List<Option> options { get; set; }
        public int displayOrder { get; set; }
        public bool formField { get; set; }
        public bool readOnlyValue { get; set; }
        public bool readOnlyDefinition { get; set; }
        public bool mutableDefinitionNotDeletable { get; set; }
        public bool calculated { get; set; }
        public bool externalOptions { get; set; }
        public string displayMode { get; set; }
        public bool hubspotDefined { get; set; }
        public bool optionsAreMutable { get; set; }
        public long? updatedUserId { get; set; }
        public long? createdUserId { get; set; }

        public class Option
        {
            public string description { get; set; }
            public bool hidden { get; set; }
            public string value { get; set; }

            [JsonProperty(DefaultValueHandling = DefaultValueHandling.Populate, NullValueHandling = NullValueHandling.Ignore)]
            public bool readOnly { get; set; }

            public double? doubleData { get; set; }
            public string label { get; set; }
            public int displayOrder { get; set; }
        }
    }
}