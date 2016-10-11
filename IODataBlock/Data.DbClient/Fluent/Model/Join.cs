using Business.Common.System;
using Data.DbClient.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.DbClient.Fluent.Model
{
    public class Join : ObjectBase<Join>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public JoinType Type { get; set; }
        public string ToTable { get; set; }
        public string ToColumn { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ComparisonOperatorType ComparisonOperator { get; set; }
        public string FromTable { get; set; }
        public string FromColumn { get; set; }
    }
}
