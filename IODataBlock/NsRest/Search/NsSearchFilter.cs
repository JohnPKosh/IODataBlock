using System;

namespace NsRest.Search
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
