using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Data.DbClient.Fluent
{
    public class DbQuery
    {
        #region Class Initialization

        public DbQuery(DbConnection connection, String commandText, Int32 commandTimeout, params object[] parameters)
        {
            Connection = connection;
            CommandText = commandText;
            CommandTimeout = commandTimeout;
            Parameters = parameters;
        }

        #endregion Class Initialization

        #region Fields and Properties

        public DbConnection Connection { get; set; }

        public String CommandText { get; set; }

        public Int32 CommandTimeout { get; set; }

        public object[] Parameters { get; set; }

        #endregion

        #region Public Methods

        public IEnumerable<dynamic> ExecuteQuery()
        {
            if (Connection == null) throw new ArgumentNullException(paramName: "Connection");
            if (string.IsNullOrEmpty(CommandText)) throw new ArgumentNullException(paramName: "CommandText");
            using (var db = new Database(() => Connection))
            {
                return db.Query(CommandText, CommandTimeout, Parameters);
            }
        } 

        #endregion
    }
}