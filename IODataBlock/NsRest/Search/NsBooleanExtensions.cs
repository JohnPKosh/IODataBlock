using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSuite.RESTlet.Integration.Search
{
    public static class NsBooleanExtensions
    {
        public static string GetNsValue(this Boolean value)
        {
            return value ? "T" : "F";
        }
    }
}
