using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Data.DbClient.Configuration;
using DbExtensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Bson;
using Newtonsoft.Json.Linq;

namespace Data.DbClient
{
    public partial class Database : IDisposable
    {
        #region Class Initialization

        static Database()
        {
            var data = (string)AppDomain.CurrentDomain.GetData("DataDirectory");
            var currentDirectory = data;
            if (data == null)
            {
                currentDirectory = Directory.GetCurrentDirectory();
            }

            DataDirectory = currentDirectory;
            var strs = new Dictionary<string, IDbFileHandler>(StringComparer.OrdinalIgnoreCase)
            {
                {".sdf", new SqlCeDbFileHandler()},
                {".mdf", new SqlServerDbFileHandler()}
            };
            ConfigurationManager = new ConfigurationManagerWrapper(strs);
        }

        internal Database(Func<DbConnection> connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        #endregion Class Initialization

        #region Fields and Props

        private readonly static IConfigurationManager ConfigurationManager;

        private DbConnection _connection;

        private readonly Func<DbConnection> _connectionFactory;

        internal static string DataDirectory;

        public DbConnection Connection
        {
            get { return _connection ?? (_connection = _connectionFactory()); }
        }

        public static Boolean IsWebAssembly
        {
            get
            {
                var entry = Assembly.GetEntryAssembly();
                return entry == null || Assembly.GetCallingAssembly().FullName.Contains(@"App_");
            }
        }

        public static String AssemblyDirectoryPath
        {
            get
            {
                return Path.GetDirectoryName(IsWebAssembly ? Assembly.GetCallingAssembly().Location : Assembly.GetEntryAssembly().Location);
            }
        }

        #endregion Fields and Props

        #region Helper Methods

        private void EnsureConnectionOpen()
        {
            if (Connection.State == ConnectionState.Open) return;
            Connection.Open();
            OnConnectionOpened();
        }

        private async Task EnsureConnectionOpenAsync()
        {
            if (Connection.State == ConnectionState.Open) return;
            await Connection.OpenAsync();
            OnConnectionOpened();
        }

        private void EnsureConnectionIsClosed()
        {
            if (Connection.State == ConnectionState.Open || Connection.State == ConnectionState.Connecting) Connection.Close();
        }

        internal static string GetDefaultProviderName()
        {
            string str;
            if (ConfigurationManager.AppSettings.TryGetValue("systemData:defaultProvider", out str)) return str;
            str = IsWebAssembly ? "System.Data.SqlServerCe.4.0" : "System.Data.SqlServerCe.3.5";
            return str;
        }

        public String GetConnectionProviderName()
        {
            var fullname = Connection.GetType().FullName;
            var providerName = fullname.Substring(0, fullname.Length - (fullname.Length - fullname.LastIndexOf('.')));

            if (providerName == "System.Data.SqlServerCe")
            {
                providerName = IsWebAssembly ? "System.Data.SqlServerCe.4.0" : "System.Data.SqlServerCe.3.5";
            }
            return providerName;
        }

        internal static IConnectionConfiguration GetConnectionConfiguration(string fileName, IDictionary<string, IDbFileHandler> handlers)
        {
            IDbFileHandler dbFileHandler = null;
            var extension = Path.GetExtension(fileName);
            if (extension != null && !handlers.TryGetValue(extension, out dbFileHandler))
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, UnableToDetermineDatabase, fileName));
            }
            return dbFileHandler != null ? dbFileHandler.GetConnectionConfiguration(fileName) : null;
        }

        public dynamic GetLastInsertId()
        {
            return QueryValue("SELECT @@Identity", args: new object[0]);
        }

        private static void AddParameters(DbCommand command, IEnumerable<object> args)
        {
            if (args == null) return;
            var dbParameters = args.Select((o, index) =>
            {
                var dbParameter = command.CreateParameter();
                dbParameter.ParameterName = index.ToString(CultureInfo.InvariantCulture);
                var value = o;
                if (o == null)
                {
                    value = DBNull.Value;
                }
                dbParameter.Value = value;
                return dbParameter;
            });
            foreach (var dbParameter1 in dbParameters)
            {
                command.Parameters.Add(dbParameter1);
            }
        }

        public static IEnumerable<string> GetColumnNames(DbDataRecord record)
        {
            for (var i = 0; i < record.FieldCount; i++)
            {
                yield return record.GetName(i);
            }
        }

        public static IEnumerable<string> GetColumnNames(DbDataReader reader)
        {
            for (var i = 0; i < reader.FieldCount; i++)
            {
                yield return reader.GetName(i);
            }
        }

        public object DbNullToNull(object input)
        {
            return Convert.DBNull.Equals(input) ? null : input;
        }

        #endregion Helper Methods

        #region Open Methods

        public static Database Open(string name)
        {
            if (!string.IsNullOrEmpty(name))
            {
                return OpenNamedConnection(name, ConfigurationManager);
            }
            throw new ArgumentNullException("name");
        }

        private static Database OpenConnectionInternal(IConnectionConfiguration connectionConfig)
        {
            return OpenConnectionStringInternal(connectionConfig.ProviderFactory, connectionConfig.ConnectionString);
        }

        internal static Database OpenNamedConnection(string name, IConfigurationManager configurationManager)
        {
            var connection = configurationManager.GetConnection(name);
            if (connection == null)
            {
                throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, ConnectionStringNotFound, name));
            }
            return OpenConnectionInternal(connection);
        }

        #endregion Open Methods

        #region OpenConnectionString Methods

        public static Database OpenConnectionString(string connectionString)
        {
            return OpenConnectionString(connectionString, null);
        }

        public static Database OpenConnectionString(string connectionString, string providerName)
        {
            if (!string.IsNullOrEmpty(connectionString))
            {
                return OpenConnectionStringInternal(new DbProviderFactoryWrapper(providerName), connectionString);
            }
            throw new ArgumentNullException(@"connectionString");
        }

        internal static Database OpenConnectionStringInternal(IDbProviderFactory providerFactory, string connectionString)
        {
            return new Database(() => providerFactory.CreateConnection(connectionString));
        }

        #endregion OpenConnectionString Methods

        #region Open DbConnection Methods

        public static Database OpenDbConnection(DbConnection connection)
        {
            //return new Database(() => connection.GetProviderFactory().CreateConnection(connection.ConnectionString));
            return new Database(() => connection);
        }

        #endregion

        #region Query Methods

        #region Public Command Methods

        #region Execute Methods

        public int Execute(string commandText, int commandTimeout = 0, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0) dbCommand.CommandTimeout = commandTimeout;
            AddParameters(dbCommand, parameters);
            int num;
            using (dbCommand)
            {
                num = dbCommand.ExecuteNonQuery();
            }
            return num;
        }

        public static int ExecuteNonQuery(string connectionString, String providerName, String commandText, Int32 commandTimeout = 0, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.Execute(commandText, commandTimeout, parameters);
            }
        }

        public static int ExecuteNonQuery(DbConnection connection, String commandText, Int32 commandTimeout = 0, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.Execute(commandText, commandTimeout, parameters);
            }
        }

        #endregion Execute Methods

        #region Query Methods
        
        public IEnumerable<dynamic> Query(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return QueryInternalAsync(commandText, CancellationToken.None, commandTimeout, parameters).Result;
                //return QueryInternal(commandText, commandTimeout, parameters).ToList<object>().AsReadOnly();
                //return QueryInternal(commandText, commandTimeout, parameters);
            }
            throw new ArgumentNullException("commandText");
        }

        public static IEnumerable<dynamic> Query(string connectionString, String providerName, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.Query(commandText, commandTimeout, parameters);
            }
        }

        public static IEnumerable<dynamic> Query(DbConnection connection, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.Query(commandText, commandTimeout, parameters);
            }
        }

        public IEnumerable<JObject> QueryToJObjects(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return QueryInternalJObjectsAsync(commandText, CancellationToken.None, commandTimeout, parameters).Result;
                //return QueryInternalJObjects(commandText, commandTimeout, parameters);
            }
            throw new ArgumentNullException("commandText");
        }

        public static IEnumerable<JObject> QueryToJObjects(string connectionString, String providerName, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.QueryToJObjects(commandText, commandTimeout, parameters);
            }
        }

        public static IEnumerable<JObject> QueryToJObjects(DbConnection connection, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.QueryToJObjects(commandText, commandTimeout, parameters);
            }
        }

        public IEnumerable<T> QueryTransformEach<T>(string commandText, Func<JObject, T> function, int commandTimeout = 60, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return QueryInternalTransformToAsync(commandText, function, CancellationToken.None, commandTimeout, parameters).Result;
                //return QueryInternalJObjects(commandText, commandTimeout, parameters);
            }
            throw new ArgumentNullException("commandText");
        }

        public static IEnumerable<T> QueryTransformEach<T>(string connectionString, String providerName, string commandText, Func<JObject, T> function, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.QueryTransformEach(commandText, function, commandTimeout, parameters);
            }
        }

        public static IEnumerable<T> QueryTransformEach<T>(DbConnection connection, string commandText, Func<JObject, T> function, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.QueryTransformEach(commandText, function, commandTimeout, parameters);
            }
        }

        public Stream QueryToBson(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return QueryInternalBsonAsync(commandText, CancellationToken.None, commandTimeout, parameters).Result;
                //return QueryInternalJObjects(commandText, commandTimeout, parameters);
            }
            throw new ArgumentNullException("commandText");
        }

        public static Stream QueryToBson(string connectionString, String providerName, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.QueryToBson(commandText, commandTimeout, parameters);
            }
        }

        public static Stream QueryToBson(DbConnection connection, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.QueryToBson(commandText, commandTimeout, parameters);
            }
        }

        public Stream QueryToJsonStream(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return QueryInternalJObjectWriterAsync(commandText, CancellationToken.None, commandTimeout, parameters).Result;
                //return QueryInternalJObjects(commandText, commandTimeout, parameters);
            }
            throw new ArgumentNullException("commandText");
        }

        public static Stream QueryToJsonStream(string connectionString, String providerName, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.QueryToJsonStream(commandText, commandTimeout, parameters);
            }
        }

        public static Stream QueryToJsonStream(DbConnection connection, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.QueryToJsonStream(commandText, commandTimeout, parameters);
            }
        }

        public DataTable QueryToDataTable(string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0)
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            var providerName = GetConnectionProviderName();
            var da = DbProviderFactories.GetFactory(providerName).CreateDataAdapter();
            // ReSharper disable once PossibleNullReferenceException
            da.SelectCommand = dbCommand;
            var dt = tableName == null ? new DataTable() : new DataTable(tableName);
            da.Fill(dt);
            return dt;
        }

        public static DataTable QueryToDataTable(string connectionString, String providerName, string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.QueryToDataTable(commandText, tableName, commandTimeout, parameters);
            }
        }

        public static DataTable QueryToDataTable(DbConnection connection, string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.QueryToDataTable(commandText, tableName, commandTimeout, parameters);
            }
        }

        public DataTable QueryAsDataTable(DbCommand dbCommand, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            var dt = string.IsNullOrWhiteSpace(tableName) ? new DataTable() : new DataTable(tableName);
            dt.Load(QueryToDataReader(dbCommand, commandTimeout, parameters));
            return dt;
        }

        public static DataTable QueryToDataTable(DbCommand dbCommand, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            var dt = string.IsNullOrWhiteSpace(tableName) ? new DataTable():new DataTable(tableName);
            using (var db = OpenDbConnection(dbCommand.Connection))
            {
                return db.QueryAsDataTable(dbCommand, tableName, commandTimeout, parameters);
                //dt.Load(db.QueryToDataReader(dbCommand, tableName, commandTimeout, parameters));
                //return dt;
            }
            //_connection = dbCommand.Connection;
            //EnsureConnectionOpen();
            //dt.Load(dbCommand.ExecuteReader(CommandBehavior.CloseConnection));
            //return dt;
        }

        /*
        public static async Task<DataTable> GetDataAsync(string connectionString, string query)
        {
            DataTable resultTable = new DataTable();
            try
            {
                ConnectionStringSettings connectionStringSettings = System.Configuration.ConfigurationManager.ConnectionStrings[connectionString];
                DbProviderFactory factory = DbProviderFactories.GetFactory(connectionStringSettings.ProviderName);
                using (DbConnection connection = factory.CreateConnection())
                {
                    connection.ConnectionString = connectionStringSettings.ConnectionString;
                    connection.Open();
                    DbCommand command = connection.CreateCommand();
                    command.CommandText = query;
                    DbDataReader readers = command.ExecuteReader();
                    DataTable schemaTable = readers.GetSchemaTable();
                    foreach (DataRow dataRow in schemaTable.Rows)
                    {
                        DataColumn dataColumn = new DataColumn();
                        dataColumn.ColumnName = dataRow[0].ToString();
                        dataColumn.DataType = Type.GetType(dataRow["DataType"].ToString());
                        resultTable.Columns.Add(dataColumn);
                    }
                    readers.Close();
                    command.CommandTimeout = 30000;
                    using (DbDataReader reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            DataRow dataRow = resultTable.NewRow();
                            for (int i = 0; i < resultTable.Columns.Count; i++)
                            {
                                dataRow[i] = reader[i];
                            }
                            Console.WriteLine(string.Format("From thread {0}-and data-{1}", System.Threading.Thread.CurrentThread.ManagedThreadId, dataRow[0]));
                            resultTable.Rows.Add(dataRow);
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message);
                throw;
            }
            return resultTable;
        }
        */

        #region Query To DataReader

        public DbDataReader QueryToDataReader(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0)
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            using (dbCommand)
            {
                return dbCommand.ExecuteReader();
            }
        }

        public async Task<DbDataReader> QueryToDataReaderAsync(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            return await QueryToDataReaderAsync(commandText, CommandBehavior.CloseConnection, CancellationToken.None, commandTimeout, parameters);
        }

        public async Task<DbDataReader> QueryToDataReaderAsync(string commandText, CancellationToken cancellationToken, int commandTimeout = 60, params object[] parameters)
        {
            return await QueryToDataReaderAsync(commandText, CommandBehavior.CloseConnection, cancellationToken, commandTimeout, parameters);
        }

        public async Task<DbDataReader> QueryToDataReaderAsync(string commandText, CommandBehavior commandBehavior, CancellationToken cancellationToken, int commandTimeout = 60, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            await EnsureConnectionOpenAsync();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0)
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            using (dbCommand)
            {
                return await dbCommand.ExecuteReaderAsync(commandBehavior, cancellationToken);
            }
        }


        public DbDataReader QueryToDataReader(DbCommand dbCommand, int commandTimeout = 60, params object[] parameters)
        {
            _connection = dbCommand.Connection;
            EnsureConnectionOpen();
            if (commandTimeout > 0)
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            using (dbCommand)
            {
                return dbCommand.ExecuteReader();
            }
        }

        public async Task<DbDataReader> QueryToDataReaderAsync(DbCommand dbCommand, int commandTimeout = 60, params object[] parameters)
        {
            return await QueryToDataReaderAsync(dbCommand, CommandBehavior.CloseConnection, CancellationToken.None, commandTimeout, parameters);
        }

        public async Task<DbDataReader> QueryToDataReaderAsync(DbCommand dbCommand, CancellationToken cancellationToken, int commandTimeout = 60, params object[] parameters)
        {
            return await QueryToDataReaderAsync(dbCommand, CommandBehavior.CloseConnection, cancellationToken, commandTimeout, parameters);
        }

        public async Task<DbDataReader> QueryToDataReaderAsync(DbCommand dbCommand, CommandBehavior commandBehavior, CancellationToken cancellationToken, int commandTimeout = 60, params object[] parameters)
        {
            _connection = dbCommand.Connection;
            await EnsureConnectionOpenAsync();

            if (commandTimeout > 0)
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            using (dbCommand)
            {
                return await dbCommand.ExecuteReaderAsync(commandBehavior, cancellationToken);
            }
        }

        #endregion

        #endregion Query Methods

        #region Single / Scalar Methods

        public dynamic QuerySingle(string commandText, int commandTimeout = 0, params object[] args)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return QueryInternal(commandText, commandTimeout, args).FirstOrDefault<object>();
            }
            throw new ArgumentNullException("commandText");
        }

        public static dynamic QuerySingle(string connectionString, String providerName, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.QuerySingle(commandText, commandTimeout, parameters);
            }
        }

        public static dynamic QuerySingle(DbConnection connection, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.QuerySingle(commandText, commandTimeout, parameters);
            }
        }

        public dynamic QueryValue(string commandText, int commandTimeout = 0, params object[] args)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0) dbCommand.CommandTimeout = commandTimeout;
            AddParameters(dbCommand, args);
            object obj;
            using (dbCommand)
            {
                obj = dbCommand.ExecuteScalar();
            }
            return obj;
        }

        public static dynamic QueryValue(string connectionString, String providerName, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenConnectionString(connectionString, providerName))
            {
                return db.QueryValue(commandText, commandTimeout, parameters);
            }
        }

        public static dynamic QueryValue(DbConnection connection, string commandText, int commandTimeout = 60, params object[] parameters)
        {
            using (var db = OpenDbConnection(connection))
            {
                return db.QueryValue(commandText, commandTimeout, parameters);
            }
        }

        #endregion Single / Scalar Methods

        #endregion Public Command Methods

        #region Private Query Methods

        private IEnumerable<dynamic> QueryInternal(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0)
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            using (dbCommand)
            {
                List<string> columnNames = null;
                var fcnt = 0;
                var dbDataReaders = dbCommand.ExecuteReader();
                using (dbDataReaders)
                {
                    foreach (DbDataRecord dbDataRecord in dbDataReaders)
                    {
                        if (columnNames == null)
                        {
                            fcnt = dbDataRecord.FieldCount;
                            columnNames = GetColumnNames(dbDataRecord).ToList();
                        }
                        dynamic e = new ExpandoObject();
                        var d = e as IDictionary<string, object>;
                        for (var i = 0; i < fcnt; i++)
                            d.Add(columnNames[i], DbNullToNull(dbDataRecord[i]));
                        yield return e;
                    }
                }
            }
        }

        private async Task<List<dynamic>> QueryInternalAsync(string commandText, CancellationToken cancellationToken, int commandTimeout = 60, params object[] parameters)
        {
            var rv = new List<dynamic>();
            List<string> columnNames = null;
            var fcnt = 0;
            using (var dr = await QueryToDataReaderAsync(commandText, CommandBehavior.CloseConnection, cancellationToken, commandTimeout, parameters))
            {
                while (await dr.ReadAsync(cancellationToken))
                {
                    if (columnNames == null)
                    {
                        fcnt = dr.FieldCount;
                        columnNames = GetColumnNames(dr).ToList();
                    }
                    dynamic e = new ExpandoObject();
                    var d = e as IDictionary<string, object>;
                    for (var i = 0; i < fcnt; i++)
                    {
                        d.Add(columnNames[i], DbNullToNull(await dr.GetFieldValueAsync<object>(i, cancellationToken)));
                    }
                    rv.Add(e);
                }
            }
            return rv;
        }

        private IEnumerable<JObject> QueryInternalJObjects(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0)
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            using (dbCommand)
            {
                List<string> columnNames = null;
                var fcnt = 0;
                var dbDataReaders = dbCommand.ExecuteReader();
                using (dbDataReaders)
                {
                    foreach (DbDataRecord dbDataRecord in dbDataReaders)
                    {
                        if (columnNames == null)
                        {
                            fcnt = dbDataRecord.FieldCount;
                            columnNames = GetColumnNames(dbDataRecord).ToList();
                        }
                        dynamic e = new JObject();
                        var d = e as IDictionary<string, JToken>;
                        for (var i = 0; i < fcnt; i++)
                            d.Add(columnNames[i], JToken.FromObject(dbDataRecord[i]));
                        yield return e;
                    }
                }
            }
        }

        private async Task<List<JObject>> QueryInternalJObjectsAsync(string commandText, CancellationToken cancellationToken, int commandTimeout = 60, params object[] parameters)
        {
            var rv = new List<JObject>();
            List<string> columnNames = null;
            var fcnt = 0;
            using (var dr = await QueryToDataReaderAsync(commandText, CommandBehavior.CloseConnection, cancellationToken, commandTimeout, parameters))
            {
                while (await dr.ReadAsync(cancellationToken))
                {
                    if (columnNames == null)
                    {
                        fcnt = dr.FieldCount;
                        columnNames = GetColumnNames(dr).ToList();
                    }
                    dynamic e = new JObject();
                    var d = e as IDictionary<string, JToken>;
                    for (var i = 0; i < fcnt; i++)
                    {
                        d.Add(columnNames[i], JToken.FromObject(await dr.GetFieldValueAsync<object>(i, cancellationToken)));
                    }
                    rv.Add(e);
                }
            }
            return rv;
        }

        private async Task<List<T>> QueryInternalTransformToAsync<T>(string commandText, Func<JObject, T> function, CancellationToken cancellationToken, int commandTimeout = 60, params object[] parameters)
        {
            var rv = new List<T>();
            List<string> columnNames = null;
            var fcnt = 0;
            using (var dr = await QueryToDataReaderAsync(commandText, CommandBehavior.CloseConnection, cancellationToken, commandTimeout, parameters))
            {
                while (await dr.ReadAsync(cancellationToken))
                {
                    if (columnNames == null)
                    {
                        fcnt = dr.FieldCount;
                        columnNames = GetColumnNames(dr).ToList();
                    }
                    dynamic e = new JObject();
                    var d = e as IDictionary<string, JToken>;
                    for (var i = 0; i < fcnt; i++)
                    {
                        d.Add(columnNames[i], JToken.FromObject(await dr.GetFieldValueAsync<object>(i, cancellationToken)));
                    }
                    rv.Add(function.Invoke(e));
                }
            }
            return rv;
        }

        private async Task<Stream> QueryInternalJObjectWriterAsync(string commandText, CancellationToken cancellationToken, int commandTimeout = 60, params object[] parameters)
        {
            var rv = new MemoryStream();
            List<string> columnNames = null;
            var fcnt = 0;
            using (var dr = await QueryToDataReaderAsync(commandText, CommandBehavior.CloseConnection, cancellationToken, commandTimeout, parameters))
            {
                using (var writer = new JsonTextWriter(new StreamWriter(rv)))
                {
                    var jobjectType = typeof(JObject);
                    var serializer = JsonSerializer.CreateDefault();
                    writer.WriteStartArray();
                    while (await dr.ReadAsync(cancellationToken))
                    {
                        writer.WriteStartObject();
                        if (columnNames == null)
                        {
                            fcnt = dr.FieldCount;
                            columnNames = GetColumnNames(dr).ToList();
                        }
                        dynamic e = new JObject();
                        var d = e as IDictionary<string, JToken>;
                        for (var i = 0; i < fcnt; i++)
                        {
                            //d.Add(columnNames[i], JToken.FromObject(await dr.GetFieldValueAsync<object>(i, cancellationToken)));
                            //var token = JToken.FromObject(await dr.GetFieldValueAsync<object>(i, cancellationToken));
                            writer.WritePropertyName(columnNames[i]);
                            writer.WriteValue(await dr.GetFieldValueAsync<object>(i, cancellationToken));
                        }
                        //serializer.Serialize(writer, d, jobjectType);
                        writer.WriteEnd();
                        //writer.WriteValue(e);
                        //rv.Add(e);
                    }
                    writer.WriteEndArray();
                    //serializer.Serialize(writer, value, type);
                }
                if (rv.CanSeek) rv.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            }
            return rv;
        }

        private async Task<Stream> QueryInternalBsonAsync(string commandText, CancellationToken cancellationToken, int commandTimeout = 60, params object[] parameters)
        {
            var rv = new MemoryStream();
            List<string> columnNames = null;
            var fcnt = 0;
            using (var dr = await QueryToDataReaderAsync(commandText, CommandBehavior.CloseConnection, cancellationToken, commandTimeout, parameters))
            {
                using (var writer = new BsonWriter(rv))
                {
                    var jobjectType = typeof(JObject);
                    var serializer = JsonSerializer.CreateDefault();
                    writer.WriteStartArray();
                    while (await dr.ReadAsync(cancellationToken))
                    {
                        writer.WriteStartObject();
                        if (columnNames == null)
                        {
                            fcnt = dr.FieldCount;
                            columnNames = GetColumnNames(dr).ToList();
                        }
                        dynamic e = new JObject();
                        var d = e as IDictionary<string, JToken>;
                        for (var i = 0; i < fcnt; i++)
                        {
                            //d.Add(columnNames[i], JToken.FromObject(await dr.GetFieldValueAsync<object>(i, cancellationToken)));
                            //var token = JToken.FromObject(await dr.GetFieldValueAsync<object>(i, cancellationToken));
                            writer.WritePropertyName(columnNames[i]);
                            writer.WriteValue(await dr.GetFieldValueAsync<object>(i, cancellationToken));
                        }
                        //serializer.Serialize(writer, d, jobjectType);
                        writer.WriteEnd();
                        //writer.WriteValue(e);
                        //rv.Add(e);
                    }
                    writer.WriteEndArray();
                    writer.Flush();
                    //serializer.Serialize(writer, value, type);
                }
                if (rv.CanSeek) rv.Seek(0, SeekOrigin.Begin);  // reset Stream to beginning.
            }
            return rv;
        }

        #endregion Private Query Methods

        #endregion Query Methods

        #region IDisposable Region

        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _connection != null)
            {
                _connection.Close();
                _connection = null;
            }
        }

        #endregion IDisposable Region
    }
}