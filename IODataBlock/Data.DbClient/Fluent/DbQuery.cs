using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Data.DbClient.Fluent
{
    public class DbQuery
    {
        #region Class Initialization

        public DbQuery(DbConnection connection, string commandText, int commandTimeout, params object[] parameters)
        {
            Connection = connection;
            CommandText = commandText;
            CommandTimeout = commandTimeout;
            Parameters = parameters;
        }

        #endregion Class Initialization

        #region Fields and Properties

        public DbConnection Connection { get; set; }

        public string CommandText { get; set; }

        public int CommandTimeout { get; set; }

        public object[] Parameters { get; set; }

        #endregion

        #region Public Methods

        public IEnumerable<dynamic> ExecuteQuery()
        {
            if (Connection == null) throw new ArgumentException("Connection is NULL!");
            if (string.IsNullOrEmpty(CommandText)) throw new ArgumentException("CommandText is NULL!");
            using (var db = new Database(() => Connection))
            {
                return db.Query(CommandText, CommandTimeout, Parameters);
            }
        } 

        #endregion
    }
}