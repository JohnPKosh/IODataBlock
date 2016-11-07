using Business.Common.System;
using Data.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model
{
    public class Join : ObjectBase<Join>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public JoinType Type { get; set; }
        public SchemaObject ToTable { get; set; }
        public SchemaObject ToColumn { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ComparisonOperatorType ComparisonOperator { get; set; }
        public SchemaObject FromTable { get; set; }
        public SchemaObject FromColumn { get; set; }
    }
}
