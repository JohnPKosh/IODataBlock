using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Extensions;
using Data.DbClient;
using Data.DbClient.Configuration;
using static Data.DbClient.Database;

namespace Data.MsAccess
{
    public class AccessDatabase : Database
    {
        public AccessDatabase(Func<DbConnection> connectionFactory) : base(connectionFactory){}

        public new int Execute(string commandText, int commandTimeout = 0, params object[] parameters)
        {
            for (int i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] is DateTime)
                {
                    parameters[i] = ((DateTime)parameters[i]).StartOfSecond();
                }
            }
            return base.Execute(commandText, commandTimeout, parameters);
        }

        /* TODO: hide other methods with parameters because MS Access does not support milliseconds in SQL statements */

        public new IEnumerable<dynamic> Query(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return QueryInternal(commandText, commandTimeout, parameters);
            }
            throw new ArgumentException("commandText is NULL!");
        }

        public new static IEnumerable<dynamic> Query(string connectionString, string providerName, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.Query(commandText, commandTimeout, parameters);
            }
        }

        public new static IEnumerable<dynamic> Query(DbConnection connection, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.Query(commandText, commandTimeout, parameters);
            }
        }

        private new IEnumerable<dynamic> QueryInternal(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            for (var i = 0; i < parameters.Length; i++)
            {
                if (parameters[i] is DateTime)
                {
                    parameters[i] = ((DateTime)parameters[i]).StartOfSecond();
                }
            }
            return base.QueryInternal(commandText, commandTimeout, parameters);
        }

        #region OpenConnectionString Methods

        public new static AccessDatabase OpenConnectionString(string connectionString)
        {
            return OpenConnectionString(connectionString, null);
        }

        public new static AccessDatabase OpenConnectionString(string connectionString, string providerName)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                return OpenConnectionStringInternal(new DbProviderFactoryWrapper(providerName), connectionString);
            }
            throw new ArgumentException(@"connectionString is NULL!");
        }

        internal static AccessDatabase OpenConnectionStringInternal(IDbProviderFactory providerFactory, string connectionString)
        {
            return new AccessDatabase(() => providerFactory.CreateConnection(connectionString));
        }

        #endregion OpenConnectionString Methods

        #region Open DbConnection Methods

        public new static AccessDatabase OpenDbConnection(DbConnection connection)
        {
            return new AccessDatabase(() => connection);
        }

        #endregion
    }
}
