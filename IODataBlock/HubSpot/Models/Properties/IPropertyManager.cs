using System;
using System.Collections.Generic;

namespace HubSpot.Models.Properties
{
    public interface IPropertyManager
    {
        DateTime? LastUpdated { get; }
        List<PropertyTypeModel> Properties { get; }
    }
}