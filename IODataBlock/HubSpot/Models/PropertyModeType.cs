using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HubSpot.Models
{
    public enum PropertyModeType
    {
        value_only,
        value_and_history
    }

    public static class PropertyModeTypeExtensions
    {
        public static string AsStringLower(this PropertyModeType value)
        {
            return value.ToString().ToLowerInvariant();
        }
    }
}