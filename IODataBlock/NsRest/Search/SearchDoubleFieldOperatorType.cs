namespace NetSuite.RESTlet.Integration.Search
{
    public enum SearchDoubleFieldOperatorType
    {
        Between 
        ,Empty 
        ,EqualTo 
        ,GreaterThan 
        ,GreaterThanOrEqualTo 
        ,LessThan 
        ,LessThanOrEqualTo 
        ,NotBetween 
        ,NotEmpty 
        ,NotEqualTo 
        ,NotGreaterThan 
        ,NotGreaterThanOrEqualTo 
        ,NotLessThan 
        ,NotLessThanOrEqualTo 
    }

    public static class SearchDoubleFieldOperatorTypeExtensions
    {
        public static string GetNsValue(this SearchDoubleFieldOperatorType type)
        {
            switch (type)
            {
                case SearchDoubleFieldOperatorType.Between:
                    return "between";
                case SearchDoubleFieldOperatorType.Empty:
                    return "empty";
                case SearchDoubleFieldOperatorType.EqualTo:
                    return "equalTo";
                case SearchDoubleFieldOperatorType.GreaterThan:
                    return "greaterThan";
                case SearchDoubleFieldOperatorType.GreaterThanOrEqualTo:
                    return "greaterThanOrEqualTo";
                case SearchDoubleFieldOperatorType.LessThan:
                    return "lessThan";
                case SearchDoubleFieldOperatorType.LessThanOrEqualTo:
                    return "lessThanOrEqualTo";
                case SearchDoubleFieldOperatorType.NotBetween:
                    return "notBetween";
                case SearchDoubleFieldOperatorType.NotEmpty:
                    return "notEmpty";
                case SearchDoubleFieldOperatorType.NotEqualTo:
                    return "notEqualTo";
                case SearchDoubleFieldOperatorType.NotGreaterThan:
                    return "notGreaterThan";
                case SearchDoubleFieldOperatorType.NotGreaterThanOrEqualTo:
                    return "notGreaterThanOrEqualTo";
                case SearchDoubleFieldOperatorType.NotLessThan:
                    return "notLessThan";
                case SearchDoubleFieldOperatorType.NotLessThanOrEqualTo:
                    return "notLessThanOrEqualTo";
                default:
                    return null;
            }
        }
    }
}