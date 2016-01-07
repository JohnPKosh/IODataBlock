using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetSuite.RESTlet.Integration.Search
{
    public class NsSearchFilter
    {
        public String field { get; set; }

        public String join { get; set; }

        public String op { get; set; }

        public String value1 { get; set; }

        public String value2 { get; set; }
    }
}
