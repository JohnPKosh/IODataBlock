using System.Collections.Generic;
using Newtonsoft.Json;

namespace Data.DbClient.Fluent.Model
{
    public interface IQueryObjectBase
    {
        List<string> SelectColumns { get; set; }
        int? Top { get; set; }
        string FromTable { get; set; }
        List<Join> Joins { get; set; }
        List<Where> WhereFilters { get; set; }
        List<string> GroupBy { get; set; }
        List<Having> HavingClauses { get; set; }
        List<OrderBy> OrderByClauses { get; set; }
        int? Skip { get; set; }
        int? Take { get; set; }
        string ToJson(bool indented = false);
        void PopulateFromJson(string value);
        void PopulateFromJson(string value, JsonSerializerSettings settings);
    }
}