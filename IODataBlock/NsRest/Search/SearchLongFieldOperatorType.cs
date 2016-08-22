namespace NsRest.Search
{
    public enum SearchLongFieldOperatorType
    {
        Between
        , Empty
        , EqualTo
        , GreaterThan
        , GreaterThanOrEqualTo
        , LessThan
        , LessThanOrEqualTo
        , NotBetween
        , NotEmpty
        , NotEqualTo
        , NotGreaterThan
        , NotGreaterThanOrEqualTo
        , NotLessThan
        , NotLessThanOrEqualTo
    }

    public static class SearchLongFieldOperatorTypeExtensions
    {
        public static string GetNsValue(this SearchLongFieldOperatorType type)
        {
            switch (type)
            {
                case SearchLongFieldOperatorType.Between:
                    return "between";

                case SearchLongFieldOperatorType.Empty:
                    return "empty";

                case SearchLongFieldOperatorType.EqualTo:
                    return "equalTo";

                case SearchLongFieldOperatorType.GreaterThan:
                    return "greaterThan";

                case SearchLongFieldOperatorType.GreaterThanOrEqualTo:
                    return "greaterThanOrEqualTo";

                case SearchLongFieldOperatorType.LessThan:
                    return "lessThan";

                case SearchLongFieldOperatorType.LessThanOrEqualTo:
                    return "lessThanOrEqualTo";

                case SearchLongFieldOperatorType.NotBetween:
                    return "notBetween";

                case SearchLongFieldOperatorType.NotEmpty:
                    return "notEmpty";

                case SearchLongFieldOperatorType.NotEqualTo:
                    return "notEqualTo";

                case SearchLongFieldOperatorType.NotGreaterThan:
                    return "notGreaterThan";

                case SearchLongFieldOperatorType.NotGreaterThanOrEqualTo:
                    return "notGreaterThanOrEqualTo";

                case SearchLongFieldOperatorType.NotLessThan:
                    return "notLessThan";

                case SearchLongFieldOperatorType.NotLessThanOrEqualTo:
                    return "notLessThanOrEqualTo";

                default:
                    return null;
            }
        }
    }
}