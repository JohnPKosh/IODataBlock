using System;
using System.Collections.Generic;
using System.Data.Common;

namespace Data.DbClient.Fluent
{
    public class DbQueryBuilder
    {
        private DbConnection _connection;
        private String _commandText;
        private Int32 _commandTimeout = 120;
        private object[] _parameters = null;

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

        public DbQueryBuilder(DbConnection connection, String commandText, Int32 commandTimeout, object[] parameters)
        {
            _connection = connection;
            _commandText = commandText;
            _commandTimeout = commandTimeout;
            _parameters = parameters;
        }

        public static DbQueryBuilder CreateFrom(DbConnection connection)
        {
            var rv = new DbQueryBuilder { _connection = connection };
            return rv;
        }

        public static DbQueryBuilder CreateFrom(DbQuery query)
        {
            return new DbQueryBuilder(query);
        }

        public DbQueryBuilder From(DbConnection connection)
        {
            _connection = connection;
            return this;
        }

        public DbQueryBuilder WithCommand(String commandText)
        {
            _commandText = commandText;
            return this;
        }

        public DbQueryBuilder TimeoutAfter(Int32 commandTimeout)
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
            if (_connection == null) throw new ArgumentNullException("Connection");
            if (string.IsNullOrEmpty(_commandText)) throw new ArgumentNullException("CommandText");
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