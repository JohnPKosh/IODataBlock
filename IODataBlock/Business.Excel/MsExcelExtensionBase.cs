//using ExBaseData;
//using ExBaseIoUtil;
//using ExBaseStringUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using Business.Common.Extensions;
using Business.Common.IO;
using Data.DbClient;

namespace Business.Excel
{
    public static class MsExcelExtensionBase
    {
        private const String Provider = "System.Data.OleDb";
        private const String ConnectionTemplateString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;{1}IMEX=1;""";

        #region Query and Execute Methods

        public static IEnumerable<dynamic> Query(FileInfo fileInfo,
            String queryString,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0,
            Boolean hasHeaderRow = true,
            Int32 commandTimeout = 60)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
            if (lockWaitMs > 0)
            {
                using (new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    using (var db = Database.OpenConnectionString(String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;"), Provider))
                    {
                        return numberedArgs == null ? db.Query(queryString, commandTimeout) : db.Query(queryString, commandTimeout, numberedArgs.ToArray());
                    }
                }
            }
            using (var db = Database.OpenConnectionString(String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;"), Provider))
            {
                return numberedArgs == null ? db.Query(queryString, commandTimeout) : db.Query(queryString, commandTimeout, numberedArgs.ToArray());
            }
        }

        public static DataTable QueryToDataTable(FileInfo fileInfo,
            String queryString,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 0,
            Boolean hasHeaderRow = true,
            Int32 commandTimeout = 60)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
            queryString = queryString.ReplaceNumberParameters(numberedArgs: numberedArgs);
            if (lockWaitMs > 0)
            {
                using (new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    using (var conn = new OleDbConnection(String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;")))
                    {
                        conn.Open();
                        var da = new OleDbDataAdapter(queryString, conn);
                        if (commandTimeout > 0) da.SelectCommand.CommandTimeout = commandTimeout;
                        var dt = new DataTable();
                        da.Fill(dt);
                        conn.Close();
                        return dt;
                    }
                }
            }
            using (var conn = new OleDbConnection(String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;")))
            {
                conn.Open();
                var da = new OleDbDataAdapter(queryString, conn);
                if (commandTimeout > 0) da.SelectCommand.CommandTimeout = commandTimeout;
                var dt = new DataTable();
                da.Fill(dt);
                conn.Close();
                return dt;
            }
        }

        public static Int32 Execute(FileInfo fileInfo,
            String queryString,
            Dictionary<String, String> namedArgs = null,
            IEnumerable<Object> numberedArgs = null,
            Int32 lockWaitMs = 60000,
            Boolean hasHeaderRow = true,
            Int32 commandTimeout = 60)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            if (namedArgs != null) queryString = queryString.ReplaceNamedParameters(namedArgs);
            if (lockWaitMs > 0)
            {
                using (new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    using (var db = Database.OpenConnectionString(String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;"), Provider))
                    {
                        return numberedArgs == null ? db.Execute(queryString, commandTimeout) : db.Execute(queryString, commandTimeout, numberedArgs.ToArray());
                    }
                }
            }
            using (var db = Database.OpenConnectionString(String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;"), Provider))
            {
                return numberedArgs == null ? db.Execute(queryString, commandTimeout) : db.Execute(queryString, commandTimeout, numberedArgs.ToArray());
            }
        }

        #endregion Query and Execute Methods

        public static FileInfo CreateExcelFromQuery(FileInfo fileInfo
            , String sqlScript
            , String connectionString
            , String providerName = "System.Data.SqlClient"
            , String workSheetName = "Results"
            , dynamic officeProperties = null
            , Boolean overWrite = false
            , String tempFolderPath = null
            , Boolean createOnNoResults = false
            , Int32 commandTimeout = 60
            , params object[] sqlParameters
            )
        {
            if (String.IsNullOrWhiteSpace(connectionString)) throw new ArgumentNullException(ArgNullExStr("connectionString"));
            if (String.IsNullOrWhiteSpace(sqlScript)) throw new ArgumentNullException(ArgNullExStr("SqlScript"));
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();

            var edo = new ExcelDynamicObjects();
            var select = sqlScript;

            //// use tuple maybe???
            //var tprops = new List<Tuple<String, Object>>() { new Tuple<String, Object>("ver", "1.1") };
            //var props = edo.CreateOfficeProperties(Title: "Table Data", CustomPropertyValues: tprops);
            using (var db = Database.OpenConnectionString(connectionString, providerName))
            {
                var dr = db.Query(@select, commandTimeout, sqlParameters);
                // ReSharper disable PossibleMultipleEnumeration
                if (dr.Count() != 0 || createOnNoResults) fileInfo = edo.CreateExcelFileFromDynamicObjects(fileInfo, dr, workSheetName, officeProperties, overWrite, tempFolderPath);
                // ReSharper restore PossibleMultipleEnumeration
            }
            return fileInfo;
        }

        private static String ArgNullExStr(String argumentName)
        {
            return String.Format(@"ArgumentNullException: {0} argument is null or empty!", argumentName);
        }
    }
}