using Business.Common.System;
using Data.Fluent.Base;
using Data.Fluent.Enums;
using Data.Fluent.Model.Schema;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.Fluent.Model
{
    public class Join : ObjectBase<Join>
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public JoinType Type { get; set; }
        public JoinTable ToTable { get; set; }
        public JoinColumn ToColumn { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ComparisonOperatorType ComparisonOperator { get; set; }
        public JoinTable FromTable { get; set; }
        public JoinColumn FromColumn { get; set; }
    }
}
