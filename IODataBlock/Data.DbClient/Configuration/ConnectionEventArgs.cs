using System;
using System.Data.Common;

namespace Data.DbClient.Configuration
{
    public class ConnectionEventArgs : EventArgs
    {
        public DbConnection Connection
        {
            get;
            private set;
        }

        public ConnectionEventArgs(DbConnection connection)
        {
            Connection = connection;
        }
    }
}