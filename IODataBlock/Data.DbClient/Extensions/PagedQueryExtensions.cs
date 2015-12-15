using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Data.DbClient.Extensions
{
    public static class PagedQueryExtensions
    {
        public static DbCommand SetPagingOptions(this DbCommand command, Int32 batchNumber, Int32 batchSize, String rowOrderBy)
        {
            switch (GetDbCommandProviderName(command))
            {
                case "System.Data.SqlClient":
                    command.CommandText = Database.CreateSqlServer2008BatchSelect(command.CommandText, batchNumber, batchSize, rowOrderBy);
                    break;
                case "Npgsql":
                    command.CommandText = Database.CreatePostgreSqlBatchSelect(command.CommandText, batchNumber, batchSize, rowOrderBy);
                    break;
                case "System.Data.SQLite":
                    command.CommandText = Database.CreateSqliteBatchSelect(command.CommandText, batchNumber, batchSize, rowOrderBy);
                    break;
                case "MySql.Data.MySqlClient":
                    command.CommandText = Database.CreateMySqlBatchSelect(command.CommandText, batchNumber, batchSize, rowOrderBy);
                    break;
                case "Oracle.ManagedDataAccess.Client":
                    command.CommandText = Database.CreateMySqlBatchSelect(command.CommandText, batchNumber, batchSize, rowOrderBy);
                    break;
                default:
                    throw new NotImplementedException("This data provider does not support paging!");
            }
            //command.CommandText = Database.CreateSqlServer2008BatchSelect(command.CommandText, batchNumber, batchSize, rowOrderBy);
            return command;
        }

        private static string GetDbCommandProviderName(DbCommand command)
        {
            var fullname = command.Connection.GetType().FullName;
            var providerName = fullname.Substring(0, fullname.Length - (fullname.Length - fullname.LastIndexOf('.')));

            if (providerName == "System.Data.SqlServerCe")
            {
                providerName = IsWebAssembly ? "System.Data.SqlServerCe.4.0" : "System.Data.SqlServerCe.3.5";
            }
            return providerName;
        }
        
        private static bool IsWebAssembly
        {
            get
            {
                var entry = Assembly.GetEntryAssembly();
                return entry == null || Assembly.GetCallingAssembly().FullName.Contains(@"App_");
            }
        }
    }
}
