﻿using Business.Common.System;
using Data.DbClient.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.DbClient.Fluent.Model
{
    public class Where : ObjectBase<Where>
    {
        public string FieldName { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public ComparisonOperatorType ComparisonOperator { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public LogicalOperatorType LogicalOperatorType { get; set; }

        public object Value { get; set; }
    }
}