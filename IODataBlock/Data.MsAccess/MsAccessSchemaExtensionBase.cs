//using ExBaseDataUtil;
//using ExBaseIoUtil;
//using SchemaReaderHelper;

using Business.Common.IO;
using Data.DbClient;
using Data.DbClient.Extensions;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;

namespace Data.MsAccess
{
    public static class MsAccessSchemaExtensionBase
    {
        private const String Provider = "System.Data.OleDb";
        private const String ConnectionTemplateStringTrusted = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Persist Security Info=False;";
        private const String ConnectionTemplateString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Jet OLEDB:Database Password=$(Password);";
        private const String LockExceptionString = @"Can not open locked file! The file is locked by another process.";

        public static DataTable GetTablesAsDt(FileInfo fileInfo,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = "TABLE_NAME ASC")
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
            var conn = String.Format(constr, fileInfo.FullName);
            var schema = new SchemaReader(conn, Provider);
            if (lockWaitMs > 0)
            {
                using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (!fileAccess.IsAccessible) throw new Exception(LockExceptionString);

                    if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Tables();
                    return schema.Tables().ApplyFilterSort(rowFilter, sort);
                }
            }
            if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Tables();
            return schema.Tables().ApplyFilterSort(rowFilter, sort);
        }

        public static List<dynamic> GetTablesAsDynamicList(FileInfo fileInfo,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = "TABLE_NAME ASC")
        {
            return GetTablesAsDt(fileInfo, password, lockWaitMs, rowFilter, sort).ToExpandoList();
        }

        public static DataTable GetUserTablesOnlyAsDt(FileInfo fileInfo,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = "TABLE_NAME ASC")
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
            var conn = String.Format(constr, fileInfo.FullName);
            var schema = new SchemaReader(conn, Provider);
            if (lockWaitMs > 0)
            {
                using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (!fileAccess.IsAccessible) throw new Exception(LockExceptionString);

                    if (String.IsNullOrWhiteSpace(rowFilter)) rowFilter = "TABLE_TYPE = 'TABLE'";
                    else rowFilter += " AND TABLE_TYPE = 'TABLE'";
                    return schema.Tables().ApplyFilterSort(rowFilter, sort);
                }
            }
            if (String.IsNullOrWhiteSpace(rowFilter)) rowFilter = "TABLE_TYPE = 'TABLE'";
            else rowFilter += " AND TABLE_TYPE = 'TABLE'";
            return schema.Tables().ApplyFilterSort(rowFilter, sort);
        }

        public static List<dynamic> GetUserTablesOnlyAsDynamicList(FileInfo fileInfo,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = "TABLE_NAME ASC")
        {
            return GetUserTablesOnlyAsDt(fileInfo, password, lockWaitMs, rowFilter, sort).ToExpandoList();
        }

        public static DataTable GetViewsOnlyAsDt(FileInfo fileInfo,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = "TABLE_NAME ASC")
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (fileInfo.Exists)
            {
                var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
                var conn = String.Format(constr, fileInfo.FullName);
                var schema = new SchemaReader(conn, Provider);
                if (lockWaitMs > 0)
                {
                    using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                    {
                        if (!fileAccess.IsAccessible) throw new Exception(LockExceptionString);

                        if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Views(null);
                        return schema.Tables().ApplyFilterSort(rowFilter, sort);
                    }
                }
                if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Views(null);
                return schema.Tables().ApplyFilterSort(rowFilter, sort);
            }
            throw new FileNotFoundException();
        }

        public static List<dynamic> GetViewsOnlyAsDynamicList(FileInfo fileInfo,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = "TABLE_NAME ASC")
        {
            return GetViewsOnlyAsDt(fileInfo, password, lockWaitMs, rowFilter, sort).ToExpandoList();
        }

        public static DataTable GetStoredProceduresAsDt(FileInfo fileInfo,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = null)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
            var conn = String.Format(constr, fileInfo.FullName);
            var schema = new SchemaReader(conn, Provider);
            if (lockWaitMs > 0)
            {
                using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (!fileAccess.IsAccessible) throw new Exception(LockExceptionString);
                    if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.StoredProcedures(null);
                    return schema.StoredProcedures(null).ApplyFilterSort(rowFilter, sort);
                }
            }
            if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.StoredProcedures(null);
            return schema.StoredProcedures(null).ApplyFilterSort(rowFilter, sort);
        }

        public static List<dynamic> GetStoredProceduresAsDynamicList(FileInfo fileInfo,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = null)
        {
            return GetStoredProceduresAsDt(fileInfo, password, lockWaitMs, rowFilter, sort).ToExpandoList();
        }

        public static DataTable GetTableColumnsAsDt(FileInfo fileInfo,
            String tableName,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = "ORDINAL_POSITION ASC")
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (fileInfo.Exists)
            {
                var constr = password == null ? ConnectionTemplateStringTrusted : ConnectionTemplateString.Replace("$(Password)", password);
                var conn = String.Format(constr, fileInfo.FullName);
                var schema = new SchemaReader(conn, Provider);
                if (lockWaitMs > 0)
                {
                    using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                    {
                        if (!fileAccess.IsAccessible) throw new Exception(LockExceptionString);

                        if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Columns(tableName);
                        return schema.Columns(tableName).ApplyFilterSort(rowFilter, sort);
                    }
                }
                if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Columns(tableName);
                return schema.Columns(tableName).ApplyFilterSort(rowFilter, sort);
            }
            throw new FileNotFoundException();
        }

        public static List<dynamic> GetTableColumnsAsDynamicList(FileInfo fileInfo,
            String tableName,
            String password = null,
            Int32 lockWaitMs = 60000,
            String rowFilter = null,
            String sort = "ORDINAL_POSITION ASC")
        {
            return GetTableColumnsAsDt(fileInfo, tableName, password, lockWaitMs, rowFilter, sort).ToExpandoList();
        }
    }
}