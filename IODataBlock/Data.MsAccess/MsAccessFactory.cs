//#define DEBUG
//#undef DEBUG

//using ExBaseData;
//using ExBaseDataUtil;
//using ExBaseIoUtil;

using Business.Common.Extensions;
using Business.Common.IO;
using Data.DbClient;
using Data.DbClient.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;

//using ExBaseZipUtil;
//using SchemaReaderHelper;

namespace Data.MsAccess
{
    /// <summary>
    /// Microsoft Access Factory Methods.
    /// </summary>
    public class MsAccessFactory : IDisposable
    {
        #region "Properties"

        /// <summary>
        /// Gets or sets the access file.
        /// </summary>
        /// <value>The access file.</value>
        public FileInfo AccessFile { get; set; }

        public String MsAccessTemplatesDirectory
        {
            get
            {
                // ReSharper disable once AssignNullToNotNullAttribute
                return Path.Combine(Path.GetDirectoryName(IOUtility.AssemblyFilePath), @"MsAccessTemplates");
            }
        }

        #endregion "Properties"



        #region "Class Initialization"

        /// <summary>
        /// Initializes a new instance of the <see cref="MsAccessFactory" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        public MsAccessFactory(FileInfo file)
        {
            AccessFile = file;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MsAccessFactory" /> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public MsAccessFactory(String filePath)
        {
            AccessFile = new FileInfo(filePath);
        }

        /// <summary>
        /// Opens the specified file.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <returns></returns>
        public static MsAccessFactory Open(FileInfo file)
        {
            return new MsAccessFactory(file);
        }

        /// <summary>
        /// Opens the specified file path.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        /// <returns></returns>
        public static MsAccessFactory Open(String filePath)
        {
            return new MsAccessFactory(filePath);
        }

        #endregion "Class Initialization"

        #region "Methods"

        /// <summary>
        /// Queries the specified query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="password"></param>
        /// <param name="namedArgs">The named args.</param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <returns></returns>
        public IEnumerable<dynamic> Query(String queryString,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0)
        {
            return MsAccessExtensionBase.Query(AccessFile, queryString, password, namedArgs, numberedArgs, lockWaitMs);
        }

        /// <summary>
        /// Queries to data table.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="password"></param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="namedArgs">The named args.</param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <returns></returns>
        public DataTable QueryToDataTable(String queryString,
            String password = null,
            String tableName = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0)
        {
            return MsAccessExtensionBase.QueryToDataTable(AccessFile, queryString, password, tableName, namedArgs, numberedArgs, lockWaitMs);
        }

        /// <summary>
        /// Querieses to data set.
        /// </summary>
        /// <param name="queryStrings">The query strings.</param>
        /// <param name="password"></param>
        /// <param name="namedArgs">The named args.</param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <returns></returns>
        public DataSet QueriesToDataSet(Dictionary<String, String> queryStrings = null,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0)
        {
            return MsAccessExtensionBase.QueriesToDataSet(AccessFile, queryStrings, password, namedArgs, numberedArgs, lockWaitMs);
        }

        /// <summary>
        /// Executes the specified query string.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="password"></param>
        /// <param name="namedArgs">The named args.</param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <returns></returns>
        public Int32 Execute(String queryString,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0)
        {
            return MsAccessExtensionBase.Execute(AccessFile, queryString, password, namedArgs, numberedArgs, lockWaitMs);
        }

        /// <summary>
        /// Executes the statements.
        /// </summary>
        /// <param name="queryString">The query string.</param>
        /// <param name="password"></param>
        /// <param name="namedArgs">The named args.</param>
        /// <param name="numberedArgs">The numbered args.</param>
        /// <param name="lockWaitMs">The lock wait ms.</param>
        /// <returns></returns>
        public IEnumerable<Int32> ExecuteStatements(String queryString,
            String password = null,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 60000)
        {
            return MsAccessExtensionBase.ExecuteStatements(AccessFile, queryString, password, namedArgs, numberedArgs, lockWaitMs);
        }

        public void ExportToAccessSimple(String sqlServer,
        String databaseName,
        String sqlTableName = null,
        String accessTableName = null,
        String sqlUserName = null,
        String sqlPassword = null,
        String selectColumns = "*",
        String whereClause = "",
        Boolean create = false,
        Boolean overwriteTable = false,
        Boolean overwriteView = false,
        Boolean zipOutput = false,
        Dictionary<String, String> namedArgs = null,
        IEnumerable<Object> numberedArgs = null,
        Int32 lockWaitMs = 60000,
        Int32 commandTimeout = 60
        )
        {
            var fil = new FileInfo(AccessFile.FullName);
            if (fil.Exists && create) fil.Delete();
            fil.Refresh();
            if (!fil.Exists)
            {
                fil = new FileInfo(Path.Combine(MsAccessTemplatesDirectory, @"blank.accdb")).CopyTo(AccessFile.FullName);
            }
            if (String.IsNullOrWhiteSpace(sqlTableName)) sqlTableName = accessTableName;
            if (String.IsNullOrWhiteSpace(accessTableName)) accessTableName = sqlTableName;
            var insertsql = String.Format(@"select {0} INTO $(AccessTableName) from $(LinkedTableName).$(sqlTableName) {1}", selectColumns, whereClause);
            SelectIntoAccessFromSqlServer(insertsql,
                accessTableName,
                sqlServer,
                databaseName,
                sqlTableName, sqlUserName, sqlPassword, null, overwriteTable, overwriteView, namedArgs, numberedArgs, lockWaitMs, commandTimeout);
            if (!zipOutput) return;
            fil.Refresh();
            var zipfil = new FileInfo(String.Format("{0}.zip", fil.FullName));
            zipfil.CreateZip(fil);
        }

        public Int32 SelectIntoAccessFromSqlServer(String insertIntoTemplate,
            String accessTableName,
            String sqlServer,
            String databaseName,
            String sqlTableName = null,
            String sqlUserName = null,
            String sqlPassword = null,
            String password = null,
            Boolean overwriteTable = false,
            Boolean overwriteView = false,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 60000,
            Int32 commandTimeout = 60)
        {
            if (String.IsNullOrWhiteSpace(sqlTableName)) sqlTableName = accessTableName;
            if (String.IsNullOrWhiteSpace(accessTableName)) accessTableName = sqlTableName;

            var linkedtable = MsAccessExtensionBase.GetAccessOdbcLinkedTableNameForSql(sqlServer, databaseName, sqlUserName, sqlPassword);
            var replacements = new Dictionary<string, string>
            {
                {@"$(AccessTableName)", accessTableName},
                {@"$(LinkedTableName)", linkedtable},
                {@"$(sqlTableName)", sqlTableName}
            };
            var insertsql = insertIntoTemplate.ReplaceNamedParameters(replacements);
            if (overwriteTable)
            {
                var exists = MsAccessSchemaExtensionBase.GetUserTablesOnlyAsDt(AccessFile, rowFilter: String.Format(@"TABLE_NAME = '{0}'", accessTableName)).Rows.Count > 0;
                if (exists)
                {
                    var dropsql = String.Format(@"DROP TABLE {0}", accessTableName);
                    MsAccessExtensionBase.Execute(AccessFile, dropsql, password, lockWaitMs: lockWaitMs, commandTimeout: commandTimeout);
                }
            }
            if (overwriteView)
            {
                var exists = MsAccessSchemaExtensionBase.GetViewsOnlyAsDt(AccessFile, rowFilter: String.Format(@"TABLE_NAME = '{0}'", accessTableName)).Rows.Count > 0;
                if (exists)
                {
                    var dropsql = String.Format(@"DROP VIEW {0}", accessTableName);
                    MsAccessExtensionBase.Execute(AccessFile, dropsql, password, lockWaitMs: lockWaitMs, commandTimeout: commandTimeout);
                }
            }
            return MsAccessExtensionBase.Execute(AccessFile, insertsql, password, namedArgs, numberedArgs, lockWaitMs, commandTimeout);
        }

        public Int32 SelectIntoSqlServerFromAccess(String insertIntoTemplate,
            String openRowsetTemplate,
            String sqlTableName,
            String sqlServer,
            String databaseName,
            String schema = @"dbo",
            String sqlUserName = null,
            String sqlPassword = null,
            String password = null,
            Boolean overwriteTable = false,
            Boolean overwriteView = false,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 60000,
            Int32 commandTimeout = 60)
        {
            if (schema == null) throw new ArgumentNullException("schema");
            var replacements = new Dictionary<string, string>
            {
                {@"$(sqlTableName)", sqlTableName},
                {@"$(Schema)", schema}
            };

            var connstr = DataExtensionBase.GetSqlConnectionString(sqlServer, databaseName, sqlUserName, sqlPassword);

            var insertsql = insertIntoTemplate.ReplaceNamedParameters(replacements);
            if (overwriteTable)
            {
                var schemaReader = new SchemaReader(connstr, "System.Data.SqlClient");
                var tableview = schemaReader.Tables(schema).AsDataView();
                tableview.RowFilter = String.Format(@"TABLE_NAME = '{0}'", sqlTableName);
                var exists = tableview.ToTable().Rows.Count > 0;
                if (exists)
                {
                    var dropsql = @"DROP TABLE [$(Schema)].[$(sqlTableName)]".ReplaceNamedParameters(replacements);
                    using (var db = Database.OpenConnectionString(connstr, "System.Data.SqlClient"))
                    {
                        db.Execute(dropsql, commandTimeout);
                    }
                }
            }
            if (overwriteView)
            {
                var schemaReader = new SchemaReader(connstr, "System.Data.SqlClient");
                var tableview = schemaReader.Views(schema).AsDataView();
                tableview.RowFilter = String.Format(@"TABLE_NAME = '{0}'", sqlTableName);
                var exists = tableview.ToTable().Rows.Count > 0;
                if (exists)
                {
                    var dropsql = @"DROP VIEW [$(Schema)].[$(sqlTableName)]".ReplaceNamedParameters(replacements);
                    using (var db = Database.OpenConnectionString(connstr, "System.Data.SqlClient"))
                    {
                        db.Execute(dropsql, commandTimeout);
                    }
                }
            }
            insertsql = insertsql.ReplaceNamedParameters(namedArgs).ReplaceNumberParameters(numberedArgs: numberedArgs);
            var openrowset = MsAccessExtensionBase.GetMsAccessOpenrowsetString(AccessFile, openRowsetTemplate, password);
            insertsql = insertsql.Replace("$(OpenRowset)", openrowset);
            using (var db = Database.OpenConnectionString(connstr, "System.Data.SqlClient"))
            {
                return db.Execute(insertsql, commandTimeout);
            }
        }

        public void QuerySqlServerToAccess(String sqlServer,
            String databaseName,
            String sqlCommandText,
            CommandType sqlCommandType = CommandType.Text,
            String accessTableName = "Data",
            String sqlUserName = null,
            String sqlPassword = null,
            Boolean create = false,
            Boolean overwriteTable = false,
            Boolean overwriteView = false,
            Boolean zipOutput = false,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 60000,
            Int32 commandTimeout = 60
            )
        {
            var fil = new FileInfo(AccessFile.FullName);
            if (fil.Exists && create) fil.Delete();
            fil.Refresh();
            if (!fil.Exists)
            {
                fil = new FileInfo(Path.Combine(MsAccessTemplatesDirectory, @"blank.accdb")).CopyTo(AccessFile.FullName);
            }

            var connstr = DataExtensionBase.GetSqlConnectionString(sqlServer, databaseName, sqlUserName, sqlPassword);
            var ddl = new CreateAccessDdlUtil();
            var droptable = ddl.GetDropTableCmd(accessTableName);
            var dt = ddl.GetSqlSchemaByCmd(connstr, sqlCommandText, sqlCommandType);
            var createtable = ddl.GetCreateTableCmd(accessTableName, dt);
            var inserttable = ddl.GetInsertTableCmd(accessTableName, dt);

            var accesscon = String.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;", AccessFile.FullName);
            using (var accessdb = AccessDatabase.OpenConnectionString(accesscon, "System.Data.OleDb"))
            {
                try
                {
                    accessdb.Execute(droptable, 60);
                }
                // ReSharper disable once EmptyGeneralCatchClause
                catch (Exception ex)
                {
                    // do nothing
                }
                accessdb.Execute(createtable, 60);

                using (var db = Database.OpenConnectionString(connstr, "System.Data.SqlClient"))
                {
                    using (var dr = db.QueryToDataReader(sqlCommandText, commandTimeout))
                    {
                        if (dr.HasRows)
                        {
                            var objarr = new object[dt.Columns.Count];
                            while (dr.Read())
                            {
                                // ReSharper disable once ReturnValueOfPureMethodIsNotUsed
                                dr.GetValues(objarr);
                                accessdb.Execute(inserttable, 60, objarr);
                            }
                        }
                    }
                }
            }
            if (!zipOutput) return;
            fil.Refresh();
            var zipfil = new FileInfo(String.Format("{0}.zip", fil.FullName));
            zipfil.CreateZip(fil);
        }

        #endregion "Methods"

        #region "Error Handling / Logging"

        /// <summary>
        /// Private LastError backing store field.
        /// </summary>
        private String _lastError;

        /// <summary>
        /// Gets the last error.
        /// </summary>
        /// <value>The last error.</value>
        public String LastError
        {
            get
            {
                return _lastError;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [handle errors].
        /// </summary>
        /// <value><c>true</c> if [handle errors]; otherwise, <c>false</c>.</value>
        public bool HandleErrors { get; set; }

        /// <summary>
        /// Private _LogErrorsToEventLog backing store field.
        /// </summary>
        private Boolean _logErrorsToEventLog = true;

        /// <summary>
        /// Gets or sets a value indicating whether [log errors to event log].
        /// </summary>
        /// <value><c>true</c> if [log errors to event log]; otherwise, <c>false</c>.</value>
        public Boolean LogErrorsToEventLog
        {
            get { return _logErrorsToEventLog; }
            set { _logErrorsToEventLog = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [log errors to file].
        /// </summary>
        /// <value><c>true</c> if [log errors to file]; otherwise, <c>false</c>.</value>
        public bool LogErrorsToFile { get; set; }

        /// <summary>
        /// Private _LogFile backing store field.
        /// </summary>
        private String _logFile = @"\MsAccessFactory_log.txt";

        /// <summary>
        /// Gets or sets the log file.
        /// </summary>
        /// <value>The log file.</value>
        public String LogFile
        {
            get
            {
                return _logFile;
            }
            set
            {
                _logFile = value;
            }
        }

        /// <summary>
        /// Handles the exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        // ReSharper disable once UnusedMember.Local
        private void HandleExceptions(Exception ex)
        {
            if (LogErrorsToFile)
            {
                WriteToLogFile(ex.Message);
            }
            if (LogErrorsToEventLog)
            {
                WriteToEventLog(ex.Message);
            }
            if (HandleErrors)
            {
                _lastError = ex.Message;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Handles the exceptions.
        /// </summary>
        /// <param name="ex">The ex.</param>
        /// <param name="handleError">if set to <c>true</c> [_ handle error].</param>
        // ReSharper disable once UnusedMember.Local
        private void HandleExceptions(Exception ex, Boolean handleError)
        {
            if (LogErrorsToFile)
            {
                WriteToLogFile(ex.Message);
            }
            if (LogErrorsToEventLog)
            {
                WriteToEventLog(ex.Message);
            }
            if (handleError)
            {
                _lastError = ex.Message;
            }
            else
            {
                throw ex;
            }
        }

        /// <summary>
        /// Writes to log file.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private void WriteToLogFile(String msg)
        {
            var writer = File.AppendText(LogFile);
            writer.WriteLine(DateTime.Now + " - " + msg);
            writer.Close();
        }

        /// <summary>
        /// Writes to event log.
        /// </summary>
        /// <param name="msg">The MSG.</param>
        private static void WriteToEventLog(String msg)
        {
            var ac = AppDomain.CurrentDomain.ActivationContext;
            var ai = ac.Identity;
            var eventMsg = ai.FullName + " Error: /n" + msg;

            EventLog.WriteEntry(ai.FullName, eventMsg, EventLogEntryType.Error);
        }

        #endregion "Error Handling / Logging"

        #region "Disposal Section"

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // dispose managed resources
            }
            // free native resources
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion "Disposal Section"
    }
}