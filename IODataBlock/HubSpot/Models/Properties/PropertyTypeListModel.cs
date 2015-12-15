using System;
using System.Collections.Generic;

namespace HubSpot.Models.Properties
{
    public class PropertyTypeListModel
    {
        public DateTime? LastUpdated { get; set; }

        public List<PropertyTypeModel> Properties { get; set; }
    }
}
