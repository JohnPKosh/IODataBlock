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

        public static NsSearchFilter NewStringFilter(string field, SearchStringFieldOperatorType op, string value1, string value2 = null)
        {
            return new NsSearchFilter
            {
                field = field,
                op = op.GetNsValue(),
                value1 = value1,
                value2 = value2
            };
        }

        public static NsSearchFilter NewDateFilter(string field, SearchDateFieldOperatorType op, string value1, string value2 = null)
        {
            return new NsSearchFilter
            {
                field = field,
                op = op.GetNsValue(),
                value1 = value1,
                value2 = value2
            };
        }

        public static NsSearchFilter NewDoubleFilter(string field, SearchDoubleFieldOperatorType op, string value1, string value2 = null)
        {
            return new NsSearchFilter
            {
                field = field,
                op = op.GetNsValue(),
                value1 = value1,
                value2 = value2
            };
        }

        public static NsSearchFilter NewLongFilter(string field, SearchLongFieldOperatorType op, string value1, string value2 = null)
        {
            return new NsSearchFilter
            {
                field = field,
                op = op.GetNsValue(),
                value1 = value1,
                value2 = value2
            };
        }

        public static NsSearchFilter NewTextNumberFilter(string field, SearchTextNumberFieldOperatorType op, string value1, string value2 = null)
        {
            return new NsSearchFilter
            {
                field = field,
                op = op.GetNsValue(),
                value1 = value1,
                value2 = value2
            };
        }

        public static NsSearchFilter NewBooleanFilter(string field, bool value)
        {
            return new NsSearchFilter
            {
                field = field,
                op = SearchStringFieldOperatorType.Is.GetNsValue(),
                value1 = value.GetNsValue()
            };
        }

        public static NsSearchFilter NewMultiSelectFilter(string field, SearchMultiSelectFieldOperatorType op, string value1, string value2 = null)
        {
            return new NsSearchFilter
            {
                field = field,
                op = op.GetNsValue(),
                value1 = value1,
                value2 = value2
            };
        }

        public static NsSearchFilter NewEnumMultiSelectFilter(string field, SearchEnumMultiSelectFieldOperatorType op, string value1, string value2 = null)
        {
            return new NsSearchFilter
            {
                field = field,
                op = op.GetNsValue(),
                value1 = value1,
                value2 = value2
            };
        }

        /* TODO: implement other filters */
    }
}