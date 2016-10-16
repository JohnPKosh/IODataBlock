using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.DbClient.Fluent.Enums;
using Data.DbClient.Fluent.Model;

namespace Data.DbClient.Fluent.Extensions
{
    public static class SelectColumnExtensions
    {

        
        public static string AsString(this SelectColumn column, string quotedPrefix = "", string quotedSuffix = "")
        {
            switch (column.ColumnType)
            {
                case SelectColumnType.Column:
                    return ConvertSelectColumnToString(column, quotedPrefix, quotedSuffix);
                case SelectColumnType.Function:
                    return ConvertQueryOrFuntionSelectColumnToString(column, quotedPrefix, quotedSuffix);
                case SelectColumnType.SubQuery:
                    return ConvertQueryOrFuntionSelectColumnToString(column, quotedPrefix, quotedSuffix);
                case SelectColumnType.Preformatted:
                    return column.SelectItem;
                default:
                    return column.SelectItem;
            }
        }

        private static string ConvertSelectColumnToString(SelectColumn column, string quotedPrefix = "", string quotedSuffix = "")
        {
            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(column.Prefix))
            {
                sb.Append(ApplyQuotePrefixAndSuffix(column.Prefix, quotedPrefix, quotedSuffix));
                sb.Append(".");
            }
            sb.Append(ApplyQuotePrefixAndSuffix(column.SelectItem, quotedPrefix, quotedSuffix));
            if (string.IsNullOrWhiteSpace(column.Alias)) return sb.ToString();
            sb.Append(" AS ");
            sb.Append(ApplyQuotePrefixAndSuffix(column.Alias, quotedPrefix, quotedSuffix));
            return sb.ToString();
        }

        private static string ConvertQueryOrFuntionSelectColumnToString(SelectColumn column, string quotedPrefix = "", string quotedSuffix = "")
        {
            var sb = new StringBuilder();
            sb.Append("(");
            sb.Append(column.SelectItem);
            sb.Append(")");
            if (!string.IsNullOrWhiteSpace(column.Alias))
            {
                sb.Append(" AS ");
                sb.Append(ApplyQuotePrefixAndSuffix(column.Alias, quotedPrefix, quotedSuffix));
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
