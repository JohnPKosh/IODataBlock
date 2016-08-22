using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Data.DbClient.Fluent
{
    public class DbQueryBuilder
    {
        #region Class Initialization

        public DbQueryBuilder()
        {
        }

        public DbQueryBuilder(DbQuery query)
        {
            _connection = query.Connection;
            _commandText = query.CommandText;
            _commandTimeout = query.CommandTimeout;
            _parameters = query.Parameters;
        }

        public DbQueryBuilder(DbConnection connection, string commandText, int commandTimeout, object[] parameters)
        {
            _connection = connection;
            _commandText = commandText;
            _commandTimeout = commandTimeout;
            _parameters = parameters;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private DbConnection _connection;
        private string _commandText;
        private int _commandTimeout = 120;
        private object[] _parameters;

        #endregion Fields and Properties

        #region Static Factory Methods

        public static DbQueryBuilder CreateFrom(DbConnection connection)
        {
            var rv = new DbQueryBuilder { _connection = connection };
            return rv;
        }

        public static DbQueryBuilder CreateFrom(DbQuery query)
        {
            return new DbQueryBuilder(query);
        }

        #endregion Static Factory Methods

        public DbQueryBuilder FromConnection(DbConnection connection)
        {
            _connection = connection;
            return this;
        }

        public DbQueryBuilder WithCommand(string commandText)
        {
            _commandText = commandText;
            return this;
        }

        public DbQueryBuilder TimeoutAfter(int commandTimeout)
        {
            _commandTimeout = commandTimeout;
            return this;
        }

        public DbQueryBuilder SetParameters(params object[] parameters)
        {
            _parameters = parameters;
            return this;
        }

        public IEnumerable<dynamic> Query()
        {
            if (_connection == null) throw new ArgumentException("Connection is NULL!");
            if (string.IsNullOrEmpty(_commandText)) throw new ArgumentException("CommandText is NULL!");
            using (var db = new Database(() => _connection))
            {
                return db.Query(_commandText, _commandTimeout, _parameters);
            }
        }

        // CONVERSION OPERATOR
        public static implicit operator DbQuery(DbQueryBuilder queryBuilder)
        {
            return new DbQuery(
                queryBuilder._connection,
                queryBuilder._commandText,
                queryBuilder._commandTimeout,
                queryBuilder._parameters
                );
        }
    }
}