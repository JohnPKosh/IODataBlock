namespace NsRest.Search
{
    public enum SearchStringFieldOperatorType
    {
        Contains
        , DoesNotContain
        , DoesNotStartWith
        , Empty
        , HasKeywords
        , Is
        , IsNot
        , NotEmpty
        , StartsWith
    }

    public static class SearchStringFieldOperatorTypeExtensions
    {
        public static string GetNsValue(this SearchStringFieldOperatorType type)
        {
            switch (type)
            {
                case SearchStringFieldOperatorType.Contains:
                    return "contains";

                case SearchStringFieldOperatorType.DoesNotContain:
                    return "doesNotContain";

                case SearchStringFieldOperatorType.DoesNotStartWith:
                    return "doesNotStartWith";

                case SearchStringFieldOperatorType.Empty:
                    return "empty";

                case SearchStringFieldOperatorType.HasKeywords:
                    return "hasKeywords";

                case SearchStringFieldOperatorType.Is:
                    return "is";

                case SearchStringFieldOperatorType.IsNot:
                    return "isNot";

                case SearchStringFieldOperatorType.NotEmpty:
                    return "notEmpty";

                case SearchStringFieldOperatorType.StartsWith:
                    return "startsWith";

                default:
                    return null;
            }
        }
    }
}