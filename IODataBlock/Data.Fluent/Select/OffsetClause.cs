namespace Data.Fluent.Select
{
    public class OffsetClause
    {
        public int Skip { get; set; }

        public OffsetClause(int skip)
        {
            Skip = skip;
        }
    }
}