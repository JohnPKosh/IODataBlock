namespace Data.DbClient.Fluent.Select
{
    public class SqlLiteral
    {
        public string Expression { get; set; }

        public SqlLiteral(string expression)
        {
            Expression = expression;
        }
    }
}