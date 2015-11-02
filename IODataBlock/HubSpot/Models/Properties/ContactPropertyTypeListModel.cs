using System;
using System.Collections.Generic;

namespace HubSpot.Models.Properties
{
    public class ContactPropertyTypeListModel
    {
        public DateTime? LastUpdated { get; set; }

        public List<ContactPropertyTypeModel> Properties { get; set; }
    }
}
