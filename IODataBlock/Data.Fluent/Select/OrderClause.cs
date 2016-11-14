using Data.Fluent.Enums;
using Data.Fluent.Model;
using Data.Fluent.Model.Schema;

namespace Data.Fluent.Select
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
                Column = new OrderByColumn(value.Column, null, SchemaValueType.Preformatted),
                SortDirection = value.Sorting
            };
        }
    }
}