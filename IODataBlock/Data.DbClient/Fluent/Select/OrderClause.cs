using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Model;

namespace Data.DbClient.Fluent.Select
{
    public class OrderClause
    {
        public string Column { get; set; }
        public OrderType Sorting { get; set; }

        public OrderClause(string column, OrderType sorting = OrderType.Ascending)
        {
            Column = column;
            Sorting = sorting;
        }

        public static implicit operator OrderBy(OrderClause value)
        {
            return new OrderBy()
            {
                Column = new SchemaObject(value.Column, null, null, SchemaValueType.Preformatted),
                SortDirection = value.Sorting
            };
        }
    }
}