using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Data.DbClient.Extensions
{
    public static class SqlBuilderExtensions
    {
        //public static Stream JsonDbQueryToStream(this SqlBuilder sqlBuilder, string connectionString,
        //    string providerName = null, int commandTimeout = 60, JsonSerializerSettings settings = null,
        //    params object[] parameters)
        //{
        //}


        public static IEnumerable<SqlBulkCopyColumnMapping> GetSqlBulkCopyColumnMappings(this IDictionary<string, string> mappings)
        {
            return mappings?.Select(mapId => new SqlBulkCopyColumnMapping(mapId.Key, mapId.Value));
        }
    }
}