namespace Data.DbClient.Fluent.Select
{
    public enum LogicOperator
    {
        And,
        Or
    }

    public enum Comparison
    {
        Equals,
        NotEquals,
        Like,
        NotLike,
        GreaterThan,
        GreaterOrEquals,
        LessThan,
        LessOrEquals,
        In
    }

    public enum JoinType
    {
        InnerJoin,
        OuterJoin,
        LeftJoin,
        RightJoin
    }

    public enum Order
    {
        Ascending,
        Descending
    }
}