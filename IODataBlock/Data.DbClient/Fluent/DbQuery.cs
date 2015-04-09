using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Data.DbClient.Fluent
{
    public class DbQuery
    {
        public DbQuery(DbConnection connection, String commandText, Int32 commandTimeout, params object[] parameters)
        {
            Connection = connection;
            CommandText = commandText;
            CommandTimeout = commandTimeout;
            Parameters = parameters;
        }

        public DbConnection Connection { get; set; }

        public String CommandText { get; set; }

        public Int32 CommandTimeout { get; set; }

        public object[] Parameters { get; set; }

        public IEnumerable<dynamic> ExecuteQuery()
        {
            if (Connection == null) throw new ArgumentNullException(paramName: "Connection");
            if (string.IsNullOrEmpty(CommandText)) throw new ArgumentNullException(paramName: "CommandText");
            using (var db = new Database(() => Connection))
            {
                return db.Query(CommandText, CommandTimeout, Parameters);
            }
        }
    }
}