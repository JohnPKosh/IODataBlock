//using ExBaseIoUtil;
//using ExBaseDataUtil;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Business.Common.IO;
using Data.DbClient.Extensions;
using ExBaseDataUtil;

namespace Business.Excel
{
    public static class MsExcelSchemaExtensionBase
    {
        private const String Provider = "System.Data.OleDb";
        private const String ConnectionTemplateString = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;{1}IMEX=1;""";
        private const String LockExceptionString = @"Can not open locked file! The file is locked by another process.";

        public static DataTable GetTablesAsDt(FileInfo fileInfo, 
            Int32 lockWaitMs = 60000, 
            String rowFilter = null,
            String sort = "TABLE_NAME ASC",
            Boolean hasHeaderRow = true)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            string conn;
            SchemaReader schema;
            if (lockWaitMs > 0)
            {
                using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (!fileAccess.IsAccessible) throw new Exception(LockExceptionString);
                    conn = String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;");
                    schema = new SchemaReader(conn, Provider);
                    if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Tables();
                    return schema.Tables().ApplyFilterSort(rowFilter, sort);
                }
            }
            conn = String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;");
            schema = new SchemaReader(conn, Provider);
            if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Tables();
            return schema.Tables().ApplyFilterSort(rowFilter, sort);
        }


        public static List<dynamic> GetTablesAsDynamicList(FileInfo fileInfo, 
            Int32 lockWaitMs = 60000, 
            String rowFilter = null,
            String sort = "TABLE_NAME ASC",
            Boolean hasHeaderRow = true)
        {
            return GetTablesAsDt(fileInfo, lockWaitMs, rowFilter, sort, hasHeaderRow).ToExpandoList();
        }


        public static DataTable GetTableColumnsAsDt(FileInfo fileInfo, 
            String tableName, 
            Int32 lockWaitMs = 60000, 
            String rowFilter = null,
            String sort = "ORDINAL_POSITION ASC",
            Boolean hasHeaderRow = true)
        {
            fileInfo.Refresh();
            if (fileInfo.Directory != null && !fileInfo.Directory.Exists) throw new DirectoryNotFoundException();
            if (!fileInfo.Exists) throw new FileNotFoundException();
            string conn;
            SchemaReader schema;
            if (lockWaitMs > 0)
            {
                using (var fileAccess = new ReadFileAccess(fileInfo, lockWaitMs, TimeSpan.FromSeconds(30)))
                {
                    if (!fileAccess.IsAccessible) throw new Exception(LockExceptionString);
                    conn = String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;");
                    schema = new SchemaReader(conn, Provider);
                    if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Columns(tableName);
                    return schema.Columns(tableName).ApplyFilterSort(rowFilter, sort);
                }
            }
            conn = String.Format(ConnectionTemplateString, fileInfo.FullName, hasHeaderRow ? "HDR=YES;" : "HDR=NO;");
            schema = new SchemaReader(conn, Provider);
            if (String.IsNullOrWhiteSpace(rowFilter) && String.IsNullOrWhiteSpace(sort)) return schema.Columns(tableName);
            return schema.Columns(tableName).ApplyFilterSort(rowFilter, sort);
        }


        public static List<dynamic> GetTableColumnsAsDynamicList(FileInfo fileInfo, 
            String tableName, 
            Int32 lockWaitMs = 60000, 
            String rowFilter = null,
            String sort = "ORDINAL_POSITION ASC",
            Boolean hasHeaderRow = true)
        {
            return GetTableColumnsAsDt(fileInfo, tableName, lockWaitMs, rowFilter, sort, hasHeaderRow).ToExpandoList();
        }


    }
}
