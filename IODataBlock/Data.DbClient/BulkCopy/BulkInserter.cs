﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using DbExtensions;
using Fasterflect;

namespace Data.DbClient.BulkCopy
{
    public class BulkInsertEventArgs<T> : EventArgs
    {
        public BulkInsertEventArgs(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");
            Items = items.ToArray();
        }

        public T[] Items { get; private set; }
    }

    /// <summary>
    /// Performs buffered bulk inserts into a sql server table using objects instead of DataRows. :)
    /// </summary>
    public class BulkInserter<T> where T : class
    {
        #region Event Handling

        public event EventHandler<BulkInsertEventArgs<T>> PreBulkInsert;

        public void OnPreBulkInsert(BulkInsertEventArgs<T> e)
        {
            var handler = PreBulkInsert;
            if (handler != null) handler(this, e);
        }

        public event EventHandler<BulkInsertEventArgs<T>> PostBulkInsert;

        public void OnPostBulkInsert(BulkInsertEventArgs<T> e)
        {
            var handler = PostBulkInsert;
            if (handler != null) handler(this, e);
        }

        #endregion Event Handling

        #region Fields and Properties

        private readonly int _bufferSize;

        public int BufferSize { get { return _bufferSize; } }

        private readonly SqlConnection _connection;
        private readonly Lazy<Dictionary<string, MemberGetter>> _props = new Lazy<Dictionary<string, MemberGetter>>(GetPropertyInformation);
        private readonly SqlBulkCopy _sbc;
        private readonly List<T> _queue = new List<T>();

        #endregion Fields and Properties

        #region Class Initialization

        /// <param name="connection">SqlConnection to use for retrieving the schema of sqlBulkCopy.DestinationTableName</param>
        /// <param name="sqlBulkCopy">SqlBulkCopy to use for bulk insert.</param>
        /// <param name="bufferSize">Number of rows to bulk insert at a time. The default is 10000.</param>
        public BulkInserter(SqlConnection connection, SqlBulkCopy sqlBulkCopy, int bufferSize = 10000)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (sqlBulkCopy == null) throw new ArgumentNullException("sqlBulkCopy");

            _bufferSize = bufferSize;
            _connection = connection;
            _sbc = sqlBulkCopy;
        }

        /// <param name="connection">SqlConnection to use for retrieving the schema of sqlBulkCopy.DestinationTableName and for bulk insert.</param>
        /// <param name="tableName">The name of the table that rows will be inserted into.</param>
        /// <param name="bufferSize">Number of rows to bulk insert at a time. The default is 10000.</param>
        /// <param name="copyOptions">Options for SqlBulkCopy.</param>
        /// <param name="sqlTransaction">SqlTransaction for SqlBulkCopy</param>
        public BulkInserter(SqlConnection connection, string tableName, int bufferSize = 10000, SqlBulkCopyOptions copyOptions = SqlBulkCopyOptions.Default, SqlTransaction sqlTransaction = null)
            : this(connection, new SqlBulkCopy(connection, copyOptions, sqlTransaction) { DestinationTableName = tableName }, bufferSize)
        {
        }

        #endregion Class Initialization

        #region Methods

        /// <summary>
        /// Performs buffered bulk insert of enumerable items.
        /// </summary>
        /// <param name="items">The items to be inserted.</param>
        public void Insert(IEnumerable<T> items)
        {
            if (items == null) throw new ArgumentNullException("items");

            var dt = CreateDataTable();

            // get columns that have a matching property
            var cols = dt.Columns.Cast<DataColumn>()
                .Where(x => _props.Value.ContainsKey(x.ColumnName))
                .Select(x => new { Column = x, Getter = _props.Value[x.ColumnName] })
                .Where(x => x.Getter != null)
                .ToArray();

            foreach (var buffer in Buffer(items))
            {
                foreach (var item in buffer)
                {
                    var row = dt.NewRow();
                    foreach (var col in cols)
                    {
                        row[col.Column] = col.Getter(item);
                    }
                    dt.Rows.Add(row);
                }

                var bulkInsertEventArgs = new BulkInsertEventArgs<T>(buffer);
                OnPreBulkInsert(bulkInsertEventArgs);

                _sbc.WriteToServer(dt);

                OnPostBulkInsert(bulkInsertEventArgs);

                dt.Clear();
            }
        }

        /// <summary>
        /// Queues a single item for bulk insert. When the queue count reaches the buffer size, bulk insert will happen.
        /// Call Flush() to manually bulk insert the currently queued items.
        /// </summary>
        /// <param name="item">The item to be inserted.</param>
        public void Insert(T item)
        {
            if (item == null) throw new ArgumentNullException("item");
            _queue.Add(item);
            if (_queue.Count == _bufferSize) Flush();
        }

        /// <summary>
        /// Bulk inserts the currently queued items.
        /// </summary>
        public void Flush()
        {
            Insert(_queue);
            _queue.Clear();
        }

        private static Dictionary<string, MemberGetter> GetPropertyInformation()
        {
            return typeof(T).Properties().ToDictionary(x => x.Name, x => x.DelegateForGetPropertyValue());
        }

        private DataTable CreateDataTable()
        {
            var commandText = string.Format("select top 0 * from {0}", _sbc.DestinationTableName);
            return Database.FillSchemaDataTable(_connection.ConnectionString, "System.Data.SqlClient", commandText, _sbc.DestinationTableName);

            //var dt = new DataTable();
            //using (var cmd = _connection.CreateCommand())
            //{
            //    cmd.CommandText = string.Format("select top 0 * from {0}", _sbc.DestinationTableName);
            //    using (var reader = cmd.ExecuteReader())
            //        dt.Load(reader);
            //}
            //return dt;
        }

        private IEnumerable<T[]> Buffer(IEnumerable<T> enumerable)
        {
            var buffer = new List<T>();
            foreach (var item in enumerable)
            {
                buffer.Add(item);
                if (buffer.Count < BufferSize) continue;
                yield return buffer.ToArray();
                buffer.Clear();
            }
            if (buffer.Count > 0)
                yield return buffer.ToArray();
        }

        #endregion Methods
    }
}