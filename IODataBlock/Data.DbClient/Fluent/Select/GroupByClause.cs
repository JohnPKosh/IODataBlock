namespace Data.DbClient.Fluent.Select
{
    public class GroupByClause
    {
        public string Column { get; set; }

        public GroupByClause(string column)
        {
            Column = column;
        }

        // TODO: consider overriding ToString instead
        public static implicit operator string(GroupByClause value)
        {
            return value.Column;
        }

    }
}