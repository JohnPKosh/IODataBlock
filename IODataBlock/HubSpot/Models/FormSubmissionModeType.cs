using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace HubSpot.Models
{
    public enum FormSubmissionModeType
    {
        Newest, All, None, Oldest
    }

    public static class FormSubmissionModeTypeExtensions
    {
        public static string AsStringLower(this FormSubmissionModeType value)
        {
            return value.ToString().ToLowerInvariant();
        }
    }
}