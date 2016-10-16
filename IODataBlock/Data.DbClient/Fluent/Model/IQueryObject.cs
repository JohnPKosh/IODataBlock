using System.Collections.Generic;
using Data.DbClient.Fluent.Select;
using Newtonsoft.Json;

namespace Data.DbClient.Fluent.Model
{
    public interface IQueryObject
    {
        List<SchemaObject> SelectColumns { get; set; }
        int? Top { get; set; }
        SchemaObject FromTable { get; set; }
        List<Join> Joins { get; set; }
        List<Where> WhereFilters { get; set; }
        List<SchemaObject> GroupBy { get; set; }
        List<Having> HavingClauses { get; set; }
        List<OrderBy> OrderByClauses { get; set; }
        int? Skip { get; set; }
        int? Take { get; set; }
        string GetQuery(IQueryBuilder builder);
        string ToJson(bool indented = false);
        void PopulateFromJson(string value);
        void PopulateFromJson(string value, JsonSerializerSettings settings);
    }
}