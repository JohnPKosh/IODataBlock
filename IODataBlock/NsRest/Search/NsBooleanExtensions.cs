using System;

namespace NsRest.Search
{
    public static class NsBooleanExtensions
    {
        public static string GetNsValue(this Boolean value)
        {
            return value ? "T" : "F";
        }
    }
}