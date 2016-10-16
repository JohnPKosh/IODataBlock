using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.System;
using Data.DbClient.Fluent.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Data.DbClient.Fluent.Model
{
    public class SelectColumn : ObjectBase<SelectColumn>
    {
        public SelectColumn(string selectItem = null, string prefix = null, string alias = null, SelectColumnType columnType = SelectColumnType.Column)
        {
            SelectItem = selectItem;
            Prefix = prefix;
            Alias = alias;
            ColumnType = columnType;
        }

        public string SelectItem { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Prefix { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Alias { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(StringEnumConverter))]
        public SelectColumnType ColumnType { get; set; }
    }
}
