using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Model;

namespace Data.DbClient.Fluent.Extensions
{
    public static class SchemaObjectExtensions
    {
        public static string AsString(this SchemaObject schemaObject, string quotedPrefix = "", string quotedSuffix = "")
        {
            switch (schemaObject.ValueType)
            {
                case SchemaValueType.NamedObject:
                    return ConvertToString(schemaObject, quotedPrefix, quotedSuffix);
                case SchemaValueType.Function:
                    return ConvertQueryOrFuntionToString(schemaObject, quotedPrefix, quotedSuffix);
                case SchemaValueType.SubQuery:
                    return ConvertQueryOrFuntionToString(schemaObject, quotedPrefix, quotedSuffix);
                case SchemaValueType.Preformatted:
                    return schemaObject.Value;
                default:
                    return schemaObject.Value;
            }
        }

        private static string ConvertToString(SchemaObject schemaObject, string quotedPrefix = "", string quotedSuffix = "")
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(schemaObject.PrefixOrSchema))
            {
                sb.Append(ApplyQuotePrefixAndSuffix(schemaObject.PrefixOrSchema, quotedPrefix, quotedSuffix));
                sb.Append(".");
            }
            sb.Append(ApplyQuotePrefixAndSuffix(schemaObject.Value, quotedPrefix, quotedSuffix));
            if (string.IsNullOrWhiteSpace(schemaObject.Alias)) return sb.ToString();
            sb.Append(" AS ");
            sb.Append(ApplyQuotePrefixAndSuffix(schemaObject.Alias, quotedPrefix, quotedSuffix));
            return sb.ToString();
        }

        private static string ConvertQueryOrFuntionToString(SchemaObject schemaObject, string quotedPrefix = "", string quotedSuffix = "")
        {
            var sb = new StringBuilder();
            sb.Append("(");
            sb.Append(schemaObject.Value);
            sb.Append(")");
            if (!string.IsNullOrWhiteSpace(schemaObject.Alias))
            {
                sb.Append(" AS ");
                sb.Append(ApplyQuotePrefixAndSuffix(schemaObject.Alias, quotedPrefix, quotedSuffix));
                return sb.ToString();
            }
            else
            {
                throw new ArgumentException("Column Alias is required when ColumnType is Function!");
            }
        }

        private static string ApplyQuotePrefixAndSuffix(string value, string quotedPrefix, string quotedSuffix)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            value = value.Trim();
            if (!value.StartsWith(quotedPrefix)) value = $"{quotedPrefix}{value}";
            if (!value.EndsWith(quotedSuffix)) value = $"{value}{quotedSuffix}";
            return value;
        }


    }
}
