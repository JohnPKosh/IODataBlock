namespace NetSuite.RESTlet.Integration.Search
{
    public enum SearchTextNumberFieldOperatorType
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


    public static class SearchTextNumberFieldOperatorTypeExtensions
    {
        public static string GetNsValue(this SearchTextNumberFieldOperatorType type)
        {
            switch (type)
            {
                case SearchTextNumberFieldOperatorType.Between:
                    return "between";
                case SearchTextNumberFieldOperatorType.Empty:
                    return "empty";
                case SearchTextNumberFieldOperatorType.EqualTo:
                    return "equalTo";
                case SearchTextNumberFieldOperatorType.GreaterThan:
                    return "greaterThan";
                case SearchTextNumberFieldOperatorType.GreaterThanOrEqualTo:
                    return "greaterThanOrEqualTo";
                case SearchTextNumberFieldOperatorType.LessThan:
                    return "lessThan";
                case SearchTextNumberFieldOperatorType.LessThanOrEqualTo:
                    return "lessThanOrEqualTo";
                case SearchTextNumberFieldOperatorType.NotBetween:
                    return "notBetween";
                case SearchTextNumberFieldOperatorType.NotEmpty:
                    return "notEmpty";
                case SearchTextNumberFieldOperatorType.NotEqualTo:
                    return "notEqualTo";
                case SearchTextNumberFieldOperatorType.NotGreaterThan:
                    return "notGreaterThan";
                case SearchTextNumberFieldOperatorType.NotGreaterThanOrEqualTo:
                    return "notGreaterThanOrEqualTo";
                case SearchTextNumberFieldOperatorType.NotLessThan:
                    return "notLessThan";
                case SearchTextNumberFieldOperatorType.NotLessThanOrEqualTo:
                    return "notLessThanOrEqualTo";
                default:
                    return null;
            }
        }
    }
}