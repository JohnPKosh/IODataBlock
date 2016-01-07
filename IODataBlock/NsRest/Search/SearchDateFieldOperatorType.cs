namespace NetSuite.RESTlet.Integration.Search
{
    public enum SearchDateFieldOperatorType
    {
        After 
        ,Before 
        ,Empty 
        ,NotAfter 
        ,NotBefore 
        ,NotEmpty 
        ,NotOn 
        ,NotOnOrAfter 
        ,NotOnOrBefore 
        ,NotWithin 
        ,On 
        ,OnOrAfter 
        ,OnOrBefore 
        ,Within 
    }

    public static class SearchDateFieldOperatorTypeExtensions
    {
        public static string GetNsValue(this SearchDateFieldOperatorType type)
        {
            switch (type)
            {
                case SearchDateFieldOperatorType.After:
                    return "after";
                case SearchDateFieldOperatorType.Before:
                    return "before";
                case SearchDateFieldOperatorType.Empty:
                    return "empty";
                case SearchDateFieldOperatorType.NotAfter:
                    return "notAfter";
                case SearchDateFieldOperatorType.NotBefore:
                    return "notBefore";
                case SearchDateFieldOperatorType.NotEmpty:
                    return "notEmpty";
                case SearchDateFieldOperatorType.NotOn:
                    return "notOn";
                case SearchDateFieldOperatorType.NotOnOrAfter:
                    return "notOnOrAfter";
                case SearchDateFieldOperatorType.NotOnOrBefore:
                    return "notOnOrBefore";
                case SearchDateFieldOperatorType.NotWithin:
                    return "notWithin";
                case SearchDateFieldOperatorType.On:
                    return "on";
                case SearchDateFieldOperatorType.OnOrAfter:
                    return "onOrAfter";
                case SearchDateFieldOperatorType.OnOrBefore:
                    return "onOrBefore";
                case SearchDateFieldOperatorType.Within:
                    return "within";
                default:
                    return null;
            }
        }
    }
}