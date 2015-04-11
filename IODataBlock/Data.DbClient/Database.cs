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

        #region Query Methods

        #region Public Query Methods

        public int Execute(string commandText, int commandTimeout = 0, params object[] args)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0) dbCommand.CommandTimeout = commandTimeout;
            AddParameters(dbCommand, args);
            int num;
            using (dbCommand)
            {
                num = dbCommand.ExecuteNonQuery();
            }
            return num;
        }

        public dynamic GetLastInsertId()
        {
            return QueryValue("SELECT @@Identity", args: new object[0]);
        }

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

        public IEnumerable<JObject> QueryToJObjects(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return QueryInternalJObjectsAsync(commandText, commandTimeout, parameters).Result;
                //return QueryInternalJObjects(commandText, commandTimeout, parameters);
            }
            throw new ArgumentNullException("commandText");
        }

        public DataTable QueryToDataTable(string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0 && Connection.GetType().Name != "SqlCeConnection")
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

        public DbDataReader QueryToDataReader(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            if (string.IsNullOrEmpty(commandText)) throw new ArgumentNullException("commandText");
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0 && Connection.GetType().Name != "SqlCeConnection")
            {
                dbCommand.CommandTimeout = commandTimeout;
            }
            AddParameters(dbCommand, parameters);
            using (dbCommand)
            {
                return dbCommand.ExecuteReader();
            }
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

        public dynamic QuerySingle(string commandText, int commandTimeout = 0, params object[] args)
        {
            if (!string.IsNullOrEmpty(commandText))
            {
                return QueryInternal(commandText, commandTimeout, args).FirstOrDefault<object>();
            }
            throw new ArgumentNullException("commandText");
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

        #endregion Public Query Methods

        #region Private Query Methods

        private IEnumerable<dynamic> QueryInternal(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            EnsureConnectionOpen();
            var dbCommand = Connection.CreateCommand();
            dbCommand.CommandText = commandText;
            if (commandTimeout > 0 && Connection.GetType().Name != "SqlCeConnection")
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
                            d.Add(columnNames[i], dbDataRecord[i]);
                        yield return e;
                    }
                }
            }
        }

        private async Task<List<dynamic>> QueryInternalAsync(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            var rv = new List<dynamic>();
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
                List<string> columnNames = null;
                var fcnt = 0;
                using (var dr = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (await dr.ReadAsync())
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
                            d.Add(columnNames[i], await dr.GetFieldValueAsync<object>(i));
                        }
                        rv.Add(e);
                    }
                }
            }
            return rv;
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
                        d.Add(columnNames[i], await dr.GetFieldValueAsync<object>(i, cancellationToken));
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
            if (commandTimeout > 0 && Connection.GetType().Name != "SqlCeConnection")
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

        private async Task<List<JObject>> QueryInternalJObjectsAsync(string commandText, int commandTimeout = 60, params object[] parameters)
        {
            var rv = new List<JObject>();
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
                List<string> columnNames = null;
                var fcnt = 0;
                using (var dr = await dbCommand.ExecuteReaderAsync(CommandBehavior.CloseConnection))
                {
                    while (await dr.ReadAsync())
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
                            d.Add(columnNames[i], JToken.FromObject(await dr.GetFieldValueAsync<object>(i)));
                        }
                        rv.Add(e);
                    }
                }
            }
            return rv;
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

                //var str = command.CreateParameter();
                //str.ParameterName = index.ToString(CultureInfo.InvariantCulture);
                //var dbParameter = str;
                //var obj = o;
                //var @value = obj;
                //if (obj == null)
                //{
                //    @value = DBNull.Value;
                //}
                //dbParameter.Value = @value;
                //return str;
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