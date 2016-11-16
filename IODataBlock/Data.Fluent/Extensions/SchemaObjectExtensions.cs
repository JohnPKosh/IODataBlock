using System;
using System.Text;
using Data.Fluent.Enums;
using Data.Fluent.Interfaces;

namespace Data.Fluent.Extensions
{
    public static class SchemaObjectExtensions
    {
        public static string AsString(this ISchemaObject schemaObject, SqlLanguageType languageType, bool quotedIdentifiers = true)
        {
            switch (languageType)
            {
                case SqlLanguageType.SqlServer:
                    return quotedIdentifiers ? schemaObject.AsString("[", "]") : schemaObject.AsString();
                case SqlLanguageType.Oracle:
                    return quotedIdentifiers ? schemaObject.AsString("\"", "\"") : schemaObject.AsString();
                case SqlLanguageType.PostgreSql:
                    return quotedIdentifiers ? schemaObject.AsString("\"", "\"") : schemaObject.AsString();
                case SqlLanguageType.MySql:
                    return quotedIdentifiers ? schemaObject.AsString("`", "`") : schemaObject.AsString();
                default:
                    return quotedIdentifiers ? schemaObject.AsString("\"", "\"") : schemaObject.AsString();
            }
        }

        public static string AsString(this ISchemaObject schemaObject, string quotedPrefix = "", string quotedSuffix = "")
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

        private static string ConvertToString(ISchemaObject schemaObject, string quotedPrefix = "", string quotedSuffix = "")
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(schemaObject.PrefixOrSchema))
            {
                sb.Append(ApplyQuotePrefixAndSuffix(schemaObject.PrefixOrSchema, quotedPrefix, quotedSuffix));
                sb.Append(".");
            }

            if (schemaObject.Value == "*")
            {
                sb.Append(schemaObject.Value);
                return sb.ToString();
            }

            sb.Append(ApplyQuotePrefixAndSuffix(schemaObject.Value, quotedPrefix, quotedSuffix));
            if (string.IsNullOrWhiteSpace(schemaObject.Alias)) return sb.ToString();
            sb.Append(" AS ");
            sb.Append(ApplyQuotePrefixAndSuffix(schemaObject.Alias, quotedPrefix, quotedSuffix));
            return sb.ToString();
        }

        private static string ConvertQueryOrFuntionToString(ISchemaObject schemaObject, string quotedPrefix = "", string quotedSuffix = "")
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
