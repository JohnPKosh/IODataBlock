namespace Data.Fluent.Select
{
    public class TopClause
    {
        public int Quantity { get; set; }

        public TopClause(int quantity)
        {
            Quantity = quantity;
        }
    }
}