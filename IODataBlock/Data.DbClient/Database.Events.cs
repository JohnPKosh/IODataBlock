using System;
using Data.DbClient.Configuration;

namespace Data.DbClient
{
    public partial class Database
    {
        #region Connection Opened Event Region

        private void OnConnectionOpened()
        {
            if (ConnectionOpenedEvent != null)
            {
                ConnectionOpenedEvent(this, new ConnectionEventArgs(Connection));
            }
        }

        private static event EventHandler<ConnectionEventArgs> ConnectionOpenedEvent;

        public static event EventHandler<ConnectionEventArgs> ConnectionOpened
        {
            add
            {
                ConnectionOpenedEvent += value;
            }
            remove
            {
                ConnectionOpenedEvent -= value;
            }
        }

        #endregion Connection Opened Event Region
    }
}