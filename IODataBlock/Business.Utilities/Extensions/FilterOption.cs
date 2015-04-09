namespace Business.Utilities.Extensions
{
    public enum StringFilterOption
    {
        Equals,
        StartsWith,
        Contains,
        EndsWith,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        Between,
        Null,
        NotNull,
        NullOrWhiteSpace,
        NotNullOrWhiteSpace,
        None
    }

    public enum NumericFilterOption
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        Between,
        Null,
        NotNull,
        None
    }

    public enum DateTimeFilterOption
    {
        Equals,
        NotEquals,
        GreaterThan,
        LessThan,
        GreaterThanOrEqualTo,
        LessThanOrEqualTo,
        Between,
        Null,
        NotNull,
        None
    }
}