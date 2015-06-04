using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Data.DbClient.BulkCopy
{
    public class SqlBulkCopyUtility
    {
        #region Import Methods

        public void ImportDataTableToSql(
            ref DataTable dt,
            String server,
            String database,
            String destinationTableName,
            Int32 connectTimeout = 360,
            Int32 packetSize = 8192,
            IEnumerable<SqlBulkCopyColumnMapping> sqlBulkCopyColumnMappings = null,
            Int32 batchSize = 0,
            Int32 bulkCopyTimeout = 0, 
            Boolean enableIndentityInsert = false
            )
        {
            var cb = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                IntegratedSecurity = true,
                ConnectTimeout = connectTimeout,
                PacketSize = packetSize
            };
            using (var bulkCopy = new SqlBulkCopy(cb.ConnectionString, enableIndentityInsert ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default))
            {
                bulkCopy.DestinationTableName = destinationTableName;
                if (batchSize > 0) bulkCopy.BatchSize = batchSize;
                if (bulkCopyTimeout > 0) bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
                if (sqlBulkCopyColumnMappings != null)
                {
                    foreach (var mapId in sqlBulkCopyColumnMappings)
                    {
                        bulkCopy.ColumnMappings.Add(mapId);
                    }
                }
                bulkCopy.WriteToServer(dt);
            }
        }

        public void SqlServerBulkCopy(
            String sourceConnStr, 
            String destConnStr, 
            String sourceCmdStr, 
            String destTableName,
            Int32 batchSize = 0,
            Int32 bulkCopyTimeout = 0,
            Boolean enableIndentityInsert = false
            )
        {
            using (var sourceConn = new SqlConnection(sourceConnStr))
            {
                sourceConn.Open();
                var command = new SqlCommand(sourceCmdStr, sourceConn);
                using (var bulkCopy = new SqlBulkCopy(destConnStr, enableIndentityInsert ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default))
                {
                    bulkCopy.DestinationTableName = destTableName;
                    if (batchSize > 0) bulkCopy.BatchSize = batchSize;
                    if (bulkCopyTimeout > 0) bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        bulkCopy.WriteToServer(reader);
                    }
                }
            }
        }

        public void SqlServerBulkCopy(
            ref DbDataReader dataReader,
            String destConnStr,
            String destTableName,
            Int32 batchSize = 0,
            Int32 bulkCopyTimeout = 0,
            Boolean enableIndentityInsert = false
            )
        {
            using (var bulkCopy = new SqlBulkCopy(destConnStr, enableIndentityInsert ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default))
            {
                bulkCopy.DestinationTableName = destTableName;
                if (batchSize > 0) bulkCopy.BatchSize = batchSize;
                if (bulkCopyTimeout > 0) bulkCopy.BulkCopyTimeout = bulkCopyTimeout;
                bulkCopy.WriteToServer(dataReader);
            }
        }

        public Boolean ImportSeperatedTxtToSql(String connectionString, String tableName, Int32 timeOutSeconds, String filePathStr, String schemaFilePath, Int32 batchRowSize, Boolean colHeaders, String fieldSeperator, String textQualifier, String nullValue, Boolean enableIndentityInsert = false)
        {
            var dt = new DataTable(tableName);
            var fi = new FileInfo(filePathStr);
            if (fi.Exists)
            {
                var fi2 = new FileInfo(schemaFilePath);
                if (!fi2.Exists)
                {
                    SqlConnection conn = null;
                    try
                    {
                        conn = new SqlConnection(connectionString);
                        conn.Open();
                        var cmd = new SqlCommand("SELECT * FROM " + tableName, conn) { CommandType = CommandType.Text };
                        var da = new SqlDataAdapter(cmd);
                        da.FillSchema(dt, SchemaType.Source);
                        conn.Close();
                        dt.WriteXmlSchema(schemaFilePath);
                    }
                    finally
                    {
                        if (conn != null && conn.State != ConnectionState.Closed) conn.Close();
                        if (conn != null) conn.Dispose();
                    }
                }
                else
                {
                    dt.ReadXmlSchema(schemaFilePath);
                }
            }
            else
            {
                return false;
            }

            using (var sr = fi.OpenText())
            {
                using (var bulkCopy = new SqlBulkCopy(connectionString, enableIndentityInsert ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default))
                {
                    if (colHeaders)
                    {
                        sr.ReadLine();  // skip first line if contains Column Headers
                    }
                    string tempStr;
                    var csvParser = SeperatedLineParser(fieldSeperator, textQualifier);

                    while ((tempStr = sr.ReadLine()) != null)
                    {
                        if (!tempStr.Contains(fieldSeperator)) continue;
                        var drow = dt.NewRow();
                        var stringArray = csvParser.Split(tempStr).Select(x=> x.Replace(textQualifier, String.Empty)).ToArray();
                        for (var j = 0; j < dt.Columns.Count; j++)
                        {
                            if (stringArray[j] == nullValue || stringArray[j].Trim() == String.Empty)
                            {
                                drow[j] = DBNull.Value;
                            }
                            else
                            {
                                drow[j] = stringArray[j];
                            }
                        }
                        dt.Rows.Add(drow);
                        if (dt.Rows.Count != batchRowSize) continue;
                        bulkCopy.BatchSize = batchRowSize;
                        bulkCopy.BulkCopyTimeout = timeOutSeconds;
                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.WriteToServer(dt);
                        dt.Clear();
                    }
                    bulkCopy.BatchSize = batchRowSize;
                    bulkCopy.BulkCopyTimeout = timeOutSeconds;
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dt);
                }
            }
            return true;
        }

        public Boolean ImportSeperatedTxtToSql(String connectionString, String tableName, Int32 timeOutSeconds, String filePathStr, String schemaFilePath, Int32 batchRowSize, Boolean colHeaders, String fieldSeperator, String textQualifier, String nullValue, String filterExpression, Boolean enableIndentityInsert = false)
        {
            var dt = new DataTable(tableName);
            var fi = new FileInfo(filePathStr);
            if (fi.Exists)
            {
                var fi2 = new FileInfo(schemaFilePath);
                if (!fi2.Exists)
                {
                    SqlConnection conn = null;
                    try
                    {
                        conn = new SqlConnection(connectionString);
                        conn.Open();
                        var cmd = new SqlCommand("SELECT * FROM " + tableName, conn) { CommandType = CommandType.Text };
                        var da = new SqlDataAdapter(cmd);
                        da.FillSchema(dt, SchemaType.Source);
                        conn.Close();
                        dt.WriteXmlSchema(schemaFilePath);
                    }
                    finally
                    {
                        if (conn != null && conn.State != ConnectionState.Closed) conn.Close();
                        if (conn != null) conn.Dispose();
                    }
                }
                else
                {
                    dt.ReadXmlSchema(schemaFilePath);
                }
                dt.DefaultView.RowFilter = filterExpression;
            }
            else
            {
                return false;
            }

            using (var sr = fi.OpenText())
            {
                using (var bulkCopy = new SqlBulkCopy(connectionString, enableIndentityInsert ? SqlBulkCopyOptions.KeepIdentity : SqlBulkCopyOptions.Default))
                {
                    if (colHeaders)
                    {
                        sr.ReadLine();  // skip first line if contains Column Headers
                    }
                    string tempStr;
                    var csvParser = SeperatedLineParser(fieldSeperator, textQualifier);
                    while ((tempStr = sr.ReadLine()) != null)
                    {
                        if (!tempStr.Contains(fieldSeperator)) continue;
                        var drow = dt.NewRow();
                        var stringArray = csvParser.Split(tempStr).Select(x => x.Replace(textQualifier, String.Empty)).ToArray();
                        for (var j = 0; j < dt.Columns.Count; j++)
                        {
                            if (stringArray[j] == nullValue || stringArray[j].Trim() == String.Empty)
                            {
                                drow[j] = DBNull.Value;
                            }
                            else
                            {
                                drow[j] = stringArray[j];
                            }
                        }
                        dt.Rows.Add(drow);
                        if (dt.Rows.Count != batchRowSize) continue;
                        bulkCopy.BatchSize = batchRowSize;
                        bulkCopy.BulkCopyTimeout = timeOutSeconds;
                        bulkCopy.DestinationTableName = tableName;
                        bulkCopy.WriteToServer(dt.DefaultView.ToTable());
                        dt.Clear();
                    }
                    bulkCopy.BatchSize = batchRowSize;
                    bulkCopy.BulkCopyTimeout = timeOutSeconds;
                    bulkCopy.DestinationTableName = tableName;
                    bulkCopy.WriteToServer(dt.DefaultView.ToTable());
                }
            }
            return true;
        }

        #endregion Import Methods

        #region Export Methods

        public Int32 ExportSeperatedTxt(
            String connectionString,
            String commandString,
            String filePathStr,
            Int32 batchRowSize = 1000,
            Boolean colHeaders = false,
            String fieldSeperator = "\t",
            String textQualifier = null,
            String newLineChar = "\r\n",
            String nullValue = "",
            Int32? timeOutSeconds = 600)
        {
            return ExportSeperatedTxt(connectionString, new SqlCommand(commandString), filePathStr, batchRowSize, colHeaders, fieldSeperator, textQualifier, newLineChar, nullValue, timeOutSeconds);
        }

        public Int32 ExportSeperatedTxt(
            String connectionString,
            SqlCommand commandObj,
            String filePathStr,
            Int32 batchRowSize = 1000,
            Boolean colHeaders = false,
            String fieldSeperator = "\t",
            String textQualifier = null,
            String newLineChar = "\r\n",
            String nullValue = "",
            Int32? timeOutSeconds = 600)
        {
            var returnvalue = 0;
            using (var conn = new SqlConnection(connectionString))
            {
                commandObj.Connection = conn;
                if (timeOutSeconds.HasValue) commandObj.CommandTimeout = timeOutSeconds.Value;
                conn.Open();
                var sqlDr = commandObj.ExecuteReader(CommandBehavior.CloseConnection);
                if (!sqlDr.HasRows) return returnvalue;
                if (batchRowSize < 1)
                {
                    batchRowSize = 1;
                }
                var flushcnt = batchRowSize;
                using (var sw = new StreamWriter(filePathStr, true))
                {
                    var schemaDt = sqlDr.GetSchemaTable();
                    if (schemaDt != null)
                    {
                        var colCnt = schemaDt.Rows.Count;
                        var useFormat = new Boolean[colCnt];
                        var fmtString = new String[colCnt];
                        for (var i = 0; i < colCnt; i++)
                        {
                            var fieldtype = sqlDr.GetProviderSpecificFieldType(i);
                            if (fieldtype == typeof(SqlString))
                            {
                                if (string.IsNullOrEmpty(textQualifier)) continue;
                                fmtString[i] = textQualifier + "{0}" + textQualifier;
                                useFormat[i] = true;
                            }
                            else if (fieldtype == typeof(SqlDateTime))
                            {
                                fmtString[i] = "{0:yyyy-MM-dd HH:mm:ss.fff}";
                                useFormat[i] = true;
                            }
                            else if (fieldtype == typeof(SqlBoolean))
                            {
                                useFormat[i] = true;
                            }
                            else if (fieldtype == typeof(SqlBinary))
                            {
                                useFormat[i] = true;
                            }
                            else
                            {
                                useFormat[i] = false;
                            }
                        }
                        if (colHeaders)
                        {
                            if (!string.IsNullOrEmpty(textQualifier))
                            {
                                var fmtHeadString = textQualifier + "{0}" + textQualifier;
                                for (var i = 0; i < colCnt - 1; i++)
                                {
                                    sw.Write(fmtHeadString, sqlDr.GetName(i));
                                    sw.Write(fieldSeperator);
                                }
                                sw.Write(fmtHeadString, sqlDr.GetName(sqlDr.FieldCount - 1));
                            }
                            else
                            {
                                for (var i = 0; i < colCnt - 1; i++)
                                {
                                    sw.Write(sqlDr.GetName(i) + fieldSeperator);
                                }
                                sw.Write(sqlDr.GetName(sqlDr.FieldCount - 1));
                            }
                            sw.Write(newLineChar);
                        }

                        foreach (DbDataRecord rec in sqlDr)
                        {
                            for (var i = 0; i < colCnt - 1; i++)
                            {
                                if (!rec.IsDBNull(i))
                                {
                                    if (useFormat[i])
                                    {
                                        if (!string.IsNullOrEmpty(fmtString[i]))
                                        {
                                            sw.Write(fmtString[i], rec.GetValue(i));
                                        }
                                        else
                                        {
                                            var fieldtype = sqlDr.GetProviderSpecificFieldType(i);
                                            if (fieldtype == typeof(SqlBoolean))
                                            {
                                                if ((Boolean)rec.GetValue(i))
                                                {
                                                    sw.Write(1);
                                                }
                                                else
                                                {
                                                    sw.Write(0);
                                                }
                                            }
                                            else if (fieldtype == typeof(SqlBinary))
                                            {
                                                if (schemaDt.Rows[i][SchemaTableOptionalColumn.IsRowVersion].ToString() == "true")
                                                {
                                                    // used for timestamp rowversion columns to convert to Int64 / bigint
                                                    var arr = (Byte[])rec.GetValue(i);
                                                    if (arr.Length == 8)
                                                    {
                                                        sw.Write(Int64.Parse(BitConverter.ToString((Byte[])rec.GetValue(i), 0).Replace("-", ""), NumberStyles.HexNumber));
                                                    }
                                                    else
                                                    {
                                                        BitConverter.ToString((Byte[])rec.GetValue(i), 0);  // if not write byte[] to string.
                                                    }
                                                }
                                                else
                                                {
                                                    BitConverter.ToString((Byte[])rec.GetValue(i), 0);  // if not write byte[] to string.
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        sw.Write(rec.GetValue(i));
                                    }
                                }
                                else if (!String.IsNullOrWhiteSpace(nullValue))
                                {
                                    sw.Write(nullValue);
                                }
                                sw.Write(fieldSeperator);
                            }
                            // write last column
                            if (!rec.IsDBNull(colCnt - 1))
                            {
                                if (useFormat[colCnt - 1])
                                {
                                    if (!string.IsNullOrEmpty(fmtString[colCnt - 1]))
                                    {
                                        sw.Write(fmtString[colCnt - 1], rec.GetValue(colCnt - 1));
                                    }
                                    else
                                    {
                                        var fieldtype = sqlDr.GetProviderSpecificFieldType(colCnt - 1);
                                        if (fieldtype == typeof(SqlBoolean))
                                        {
                                            if ((Boolean)rec.GetValue(colCnt - 1))
                                            {
                                                sw.Write(1);
                                            }
                                            else
                                            {
                                                sw.Write(0);
                                            }
                                        }
                                        if (fieldtype == typeof(SqlBinary))
                                        {
                                            // used for timestamp rowversion columns to convert to Int64 / bigint
                                            var arr = (Byte[])rec.GetValue(colCnt - 1);
                                            if (arr.Length == 8)
                                            {
                                                sw.Write(Int64.Parse(BitConverter.ToString((Byte[])rec.GetValue(colCnt - 1), 0).Replace("-", ""), NumberStyles.HexNumber));
                                            }
                                            else
                                            {
                                                BitConverter.ToString((Byte[])rec.GetValue(colCnt - 1), 0);  // if not write byte[] to string.
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    sw.Write(rec.GetValue(colCnt - 1));
                                }
                            }
                            else if (!String.IsNullOrWhiteSpace(nullValue))
                            {
                                sw.Write(nullValue);
                            }
                            sw.Write(newLineChar);
                            returnvalue++;

                            flushcnt--;
                            if (flushcnt >= 1) continue;
                            flushcnt = batchRowSize;
                            sw.Flush();
                        }
                    }
                    sw.Flush();
                }
            }
            return returnvalue;
        }

        public Int32 ExportSeperatedGZipTxt(
            String connectionString,
            String commandString,
            String filePathStr,
            Int32 batchRowSize = 1000,
            Boolean colHeaders = false,
            String fieldSeperator = "\t",
            String textQualifier = null,
            String newLineChar = "\r\n",
            String nullValue = "")
        {
            return ExportSeperatedGZipTxt(connectionString, new SqlCommand(commandString), filePathStr, batchRowSize, colHeaders, fieldSeperator, textQualifier, newLineChar, nullValue);
        }

        public Int32 ExportSeperatedGZipTxt(
            String connectionString,
            SqlCommand commandObj,
            String filePathStr,
            Int32 batchRowSize = 1000,
            Boolean colHeaders = false,
            String fieldSeperator = "\t",
            String textQualifier = null,
            String newLineChar = "\r\n",
            String nullValue = "")
        {
            var returnvalue = 0;
            using (var conn = new SqlConnection(connectionString))
            {
                commandObj.Connection = conn;
                conn.Open();
                var sqlDr = commandObj.ExecuteReader(CommandBehavior.CloseConnection);
                if (sqlDr.HasRows)
                {
                    if (batchRowSize < 1)
                    {
                        batchRowSize = 1;
                    }
                    var flushcnt = batchRowSize;
                    var encoding = Encoding.ASCII;
                    using (var fs = new FileStream(filePathStr, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        // start gz
                        using (var gzip = new GZipStream(fs, CompressionMode.Compress, true))
                        {
                            using (var sw = new StreamWriter(gzip, encoding))
                            {
                                var schemaDt = sqlDr.GetSchemaTable();
                                if (schemaDt != null)
                                {
                                    var colCnt = schemaDt.Rows.Count;
                                    var useFormat = new Boolean[colCnt];
                                    var fmtString = new String[colCnt];
                                    for (var i = 0; i < colCnt; i++)
                                    {
                                        var fieldtype = sqlDr.GetProviderSpecificFieldType(i);
                                        if (fieldtype == typeof(SqlString))
                                        {
                                            if (string.IsNullOrEmpty(textQualifier)) continue;
                                            fmtString[i] = textQualifier + "{0}" + textQualifier;
                                            useFormat[i] = true;
                                        }
                                        else if (fieldtype == typeof(SqlDateTime))
                                        {
                                            fmtString[i] = "{0:yyyy-MM-dd HH:mm:ss.fff}";
                                            useFormat[i] = true;
                                        }
                                        else if (fieldtype == typeof(SqlBoolean))
                                        {
                                            useFormat[i] = true;
                                        }
                                        else if (fieldtype == typeof(SqlBinary))
                                        {
                                            useFormat[i] = true;
                                        }
                                        else
                                        {
                                            useFormat[i] = false;
                                        }
                                    }
                                    if (colHeaders)
                                    {
                                        if (!string.IsNullOrEmpty(textQualifier))
                                        {
                                            string fmtHeadString = textQualifier + "{0}" + textQualifier;

                                            for (var i = 0; i < colCnt - 1; i++)
                                            {
                                                sw.Write(fmtHeadString, sqlDr.GetName(i));
                                                sw.Write(fieldSeperator);
                                            }
                                            sw.Write(fmtHeadString, sqlDr.GetName(sqlDr.FieldCount - 1));
                                        }
                                        else
                                        {
                                            for (var i = 0; i < colCnt - 1; i++)
                                            {
                                                sw.Write(sqlDr.GetName(i) + fieldSeperator);
                                            }
                                            sw.Write(sqlDr.GetName(sqlDr.FieldCount - 1));
                                        }
                                        sw.Write(newLineChar);
                                    }
                                    foreach (DbDataRecord rec in sqlDr)
                                    {
                                        for (var i = 0; i < colCnt - 1; i++)
                                        {
                                            if (!rec.IsDBNull(i))
                                            {
                                                if (useFormat[i])
                                                {
                                                    if (!string.IsNullOrEmpty(fmtString[i]))
                                                    {
                                                        sw.Write(fmtString[i], rec.GetValue(i));
                                                    }
                                                    else
                                                    {
                                                        var fieldtype = sqlDr.GetProviderSpecificFieldType(i);
                                                        if (fieldtype == typeof(SqlBoolean))
                                                        {
                                                            if ((Boolean)rec.GetValue(i))
                                                            {
                                                                sw.Write(1);
                                                            }
                                                            else
                                                            {
                                                                sw.Write(0);
                                                            }
                                                        }
                                                        else if (fieldtype == typeof(SqlBinary))
                                                        {
                                                            if (schemaDt.Rows[i][SchemaTableOptionalColumn.IsRowVersion].ToString() == "true")
                                                            {
                                                                // used for timestamp rowversion columns to convert to Int64 / bigint
                                                                var arr = (Byte[])rec.GetValue(i);
                                                                if (arr.Length == 8)
                                                                {
                                                                    sw.Write(Int64.Parse(BitConverter.ToString((Byte[])rec.GetValue(i), 0).Replace("-", ""), NumberStyles.HexNumber));
                                                                }
                                                                else
                                                                {
                                                                    BitConverter.ToString((Byte[])rec.GetValue(i), 0);  // if not write byte[] to string.
                                                                }
                                                            }
                                                            else
                                                            {
                                                                BitConverter.ToString((Byte[])rec.GetValue(i), 0);  // if not write byte[] to string.
                                                            }
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    sw.Write(rec.GetValue(i));
                                                }
                                            }
                                            else if (!String.IsNullOrWhiteSpace(nullValue))
                                            {
                                                sw.Write(nullValue);
                                            }
                                            sw.Write(fieldSeperator);
                                        }
                                        // write last column
                                        if (!rec.IsDBNull(colCnt - 1))
                                        {
                                            if (useFormat[colCnt - 1])
                                            {
                                                if (!string.IsNullOrEmpty(fmtString[colCnt - 1]))
                                                {
                                                    sw.Write(fmtString[colCnt - 1], rec.GetValue(colCnt - 1));
                                                }
                                                else
                                                {
                                                    var fieldtype = sqlDr.GetProviderSpecificFieldType(colCnt - 1);
                                                    if (fieldtype == typeof(SqlBoolean))
                                                    {
                                                        if ((Boolean)rec.GetValue(colCnt - 1))
                                                        {
                                                            sw.Write(1);
                                                        }
                                                        else
                                                        {
                                                            sw.Write(0);
                                                        }
                                                    }
                                                    if (fieldtype == typeof(SqlBinary))
                                                    {
                                                        // used for timestamp rowversion columns to convert to Int64 / bigint
                                                        var arr = (Byte[])rec.GetValue(colCnt - 1);
                                                        if (arr.Length == 8)
                                                        {
                                                            sw.Write(Int64.Parse(BitConverter.ToString((Byte[])rec.GetValue(colCnt - 1), 0).Replace("-", ""), NumberStyles.HexNumber));
                                                        }
                                                        else
                                                        {
                                                            BitConverter.ToString((Byte[])rec.GetValue(colCnt - 1), 0);  // if not write byte[] to string.
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                sw.Write(rec.GetValue(colCnt - 1));
                                            }
                                        }
                                        else if (!String.IsNullOrWhiteSpace(nullValue))
                                        {
                                            sw.Write(nullValue);
                                        }
                                        sw.Write(newLineChar);
                                        returnvalue++;

                                        flushcnt--;
                                        if (flushcnt >= 1) continue;
                                        flushcnt = batchRowSize;
                                        sw.Flush();
                                    }
                                }
                                sw.Flush();
                                gzip.Flush();
                            }
                            // end gz
                        }
                    }
                }
            }
            return returnvalue;
        }

        #endregion Export Methods

        #region Helper Methods

        public DataTable GetDataTableWithSchema(String server, String database, String table)
        {
            var dt = new DataTable();
            var cb = new SqlConnectionStringBuilder
            {
                DataSource = server,
                InitialCatalog = database,
                IntegratedSecurity = true,
                ConnectTimeout = 360
            };
            //cb.PacketSize = ???

            using (var conn = new SqlConnection(cb.ConnectionString))
            {
                conn.Open();
                var cmd = new SqlCommand("SELECT * FROM " + table, conn) { CommandType = CommandType.Text };
                var da = new SqlDataAdapter(cmd);
                da.FillSchema(dt, SchemaType.Source);
                conn.Close();
            }

            return dt;
        }

        private String AddNull(String str)
        {
            if (string.IsNullOrEmpty(str))
            {
                str = "NULL";
            }
            return str;
        }

        private String AddStrNull(String str)
        {
            if (!string.IsNullOrWhiteSpace(str)) return "\"" + str + "\"";
            str = "NULL";
            return str;
        }

        private Regex SeperatedLineParser(String fieldSeperator, String textQualifier)
        {
            var regexPattern = String.Format("{0}(?=(?:[^{1}]*{1}[^{1}]*{1})*(?![^{1}]*{1}))", fieldSeperator, textQualifier);
            return new Regex(regexPattern);
        }

        #endregion Helper Methods
    }
}