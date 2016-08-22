namespace NsRest.Search
{
    public enum SearchEnumMultiSelectFieldOperatorType
    {
        AnyOf
        , NoneOf
    }

    public static class SearchEnumMultiSelectFieldOperatorTypeExtensions
    {
        public static string GetNsValue(this SearchEnumMultiSelectFieldOperatorType type)
        {
            switch (type)
            {
                case SearchEnumMultiSelectFieldOperatorType.AnyOf:
                    return "anyOf";

                case SearchEnumMultiSelectFieldOperatorType.NoneOf:
                    return "noneOf";

                default:
                    return null;
            }
        }
    }
}