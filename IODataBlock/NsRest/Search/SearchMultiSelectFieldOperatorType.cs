namespace NetSuite.RESTlet.Integration.Search
{
    public enum SearchMultiSelectFieldOperatorType
    {
        AnyOf
        , NoneOf 
    }

    public static class SearchMultiSelectFieldOperatorTypeExtensions
    {
        public static string GetNsValue(this SearchMultiSelectFieldOperatorType type)
        {
            switch (type)
            {
                case SearchMultiSelectFieldOperatorType.AnyOf:
                    return "anyOf";
                case SearchMultiSelectFieldOperatorType.NoneOf:
                    return "noneOf";
                default:
                    return null;
            }
        }
    }
}