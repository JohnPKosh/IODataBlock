using Data.DbClient.Configuration;
using System;

namespace Data.DbClient
{
    public partial class Database
    {
        #region Connection Opened Event Region

        private void OnConnectionOpened()
        {
            ConnectionOpenedEvent?.Invoke(this, new ConnectionEventArgs(Connection));
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