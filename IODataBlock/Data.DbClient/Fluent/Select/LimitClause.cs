namespace Data.DbClient.Fluent.Select
{
    public class LimitClause
    {
        public int Take { get; set; }

        public LimitClause(int take)
        {
            Take = take;
        }
    }
}