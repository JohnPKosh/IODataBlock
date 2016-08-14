using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Data.MsAccess
{
    public class CreateAccessDdlUtil
    {
        //public DataTable GetSqlSchemaByCmd(String _ConnectionStr, String _CmdStr)
        //{
        //    return GetSqlSchemaByCmd(_ConnectionStr, _CmdStr, CommandType.Text);
        //}

        public DataTable GetSqlSchemaByCmd(String connectionStr, String cmdStr, CommandType cmdType = CommandType.Text)
        {
            DataTable dt;
            using (var connection = new SqlConnection(connectionStr))
            {
                connection.Open();
                var cmd = new SqlCommand(cmdStr, connection);
                var dr = cmd.ExecuteReader(CommandBehavior.SchemaOnly);
                dt = dr.GetSchemaTable();
            }
            return dt;
        }

        public String GetMsAccessCreateBySqlCmd(String tableName, String connectionStr, String cmdStr, CommandType cmdType = CommandType.Text)
        {
            var sb = new StringBuilder();
            var columnDdl = new List<string>();

            var dt = GetSqlSchemaByCmd(connectionStr, cmdStr, cmdType);
            foreach (DataRow r in dt.Rows)
            {
                var datatypename = r.Field<String>("DataTypeName").ToLower();
                if (datatypename.Contains("geography")) datatypename = "geography";
                if (datatypename.Contains("geometry")) datatypename = "geometry";
                if (datatypename.Contains("hierarchyid")) datatypename = "hierarchyid";

                short precision;
                short scale;
                switch (datatypename)
                {
                    case "bigint":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_bigint(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "binary":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_binary(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "bit":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_bit(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "char":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_char(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "date":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_date(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "datetime":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_datetime(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "datetime2":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_datetime2(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "datetimeoffset":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_datetimeoffset(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "decimal":
                        precision = r.Field<Int16>("NumericPrecision");
                        scale = r.Field<Int16>("NumericScale");
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_decimal(r.Field<String>("ColumnName"), precision, scale, r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "float":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_float(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "geography":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_geography(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "geometry":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_geometry(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "hierarchyid":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_hierarchyid(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "image":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_image(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "int":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_int(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "money":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_money(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "nchar":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_nchar(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "ntext":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_ntext(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "numeric":
                        precision = r.Field<Int16>("NumericPrecision");
                        scale = r.Field<Int16>("NumericScale");
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_numeric(r.Field<String>("ColumnName"), precision, scale, r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "nvarchar":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_nvarchar(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "real":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_real(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "smalldatetime":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_smalldatetime(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "smallint":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_smallint(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "smallmoney":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_smallmoney(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "sql_variant":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_sql_variant(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "text":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_text(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "time":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_time(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "timestamp":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_timestamp(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "tinyint":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_tinyint(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "uniqueidentifier":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_uniqueidentifier(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "varbinary":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_varbinary(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "varchar":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_varchar(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "xml":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_xml(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    default:
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_text(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;
                }
            }
            sb.AppendLine(String.Format(@"CREATE TABLE [{0}](", tableName));
            sb.AppendLine(String.Join(String.Format(@",{0}", Environment.NewLine), columnDdl.ToArray()));
            sb.AppendLine(@")");
            return sb.ToString();
        }

        public String GetCreateTableCmd(String tableName, DataTable dt)
        {
            var sb = new StringBuilder();
            var columnDdl = new List<string>();

            foreach (DataRow r in dt.Rows)
            {
                var datatypename = r.Field<String>("DataTypeName").ToLower();
                if (datatypename.Contains("geography")) datatypename = "geography";
                if (datatypename.Contains("geometry")) datatypename = "geometry";
                if (datatypename.Contains("hierarchyid")) datatypename = "hierarchyid";

                short precision;
                short scale;
                switch (datatypename)
                {
                    case "bigint":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_bigint(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "binary":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_binary(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "bit":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_bit(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "char":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_char(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "date":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_date(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "datetime":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_datetime(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "datetime2":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_datetime2(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "datetimeoffset":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_datetimeoffset(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "decimal":
                        precision = r.Field<Int16>("NumericPrecision");
                        scale = r.Field<Int16>("NumericScale");
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_decimal(r.Field<String>("ColumnName"), precision, scale, r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "float":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_float(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "geography":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_geography(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "geometry":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_geometry(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "hierarchyid":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_hierarchyid(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "image":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_image(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "int":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_int(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "money":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_money(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "nchar":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_nchar(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "ntext":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_ntext(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "numeric":
                        precision = r.Field<Int16>("NumericPrecision");
                        scale = r.Field<Int16>("NumericScale");
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_numeric(r.Field<String>("ColumnName"), precision, scale, r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "nvarchar":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_nvarchar(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "real":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_real(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "smalldatetime":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_smalldatetime(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "smallint":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_smallint(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "smallmoney":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_smallmoney(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "sql_variant":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_sql_variant(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "text":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_text(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "time":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_time(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "timestamp":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_timestamp(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "tinyint":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_tinyint(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "uniqueidentifier":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_uniqueidentifier(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "varbinary":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_varbinary(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "varchar":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_varchar(r.Field<String>("ColumnName"), r.Field<Int32>("ColumnSize"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    case "xml":
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_xml(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;

                    default:
                        columnDdl.Add(GetBinaryDDLFromDataTypeName_text(r.Field<String>("ColumnName"), r.Field<Boolean>("AllowDBNull")));
                        break;
                }
            }
            sb.AppendLine(String.Format(@"CREATE TABLE [{0}](", tableName));
            sb.AppendLine(String.Join(String.Format(@",{0}", Environment.NewLine), columnDdl.ToArray()));
            sb.AppendLine(@")");
            return sb.ToString();
        }

        public String GetInsertTableCmd(String tableName, DataTable dt)
        {
            var sb = new StringBuilder();
            var paramDdl = new List<string>();

            var columnDdl = (from DataRow r in dt.Rows select String.Format(@"[{0}]", r.Field<String>("ColumnName"))).ToList();
            sb.AppendLine(String.Format(@"INSERT INTO [{0}](", tableName));
            sb.AppendLine(String.Join(String.Format(@",{0}", Environment.NewLine), columnDdl.ToArray()));
            sb.AppendLine(@") VALUES (");

            var pcnt = 0;
            // ReSharper disable once UnusedVariable
            foreach (var p in columnDdl)
            {
                paramDdl.Add(String.Format(@"@{0}", pcnt));
                pcnt++;
            }
            sb.AppendLine(String.Join(String.Format(@",{0}", Environment.NewLine), paramDdl.ToArray()));
            sb.AppendLine(@")");
            return sb.ToString();
        }

        public String GetDropTableCmd(String tableName)
        {
            return String.Format(@"DROP TABLE [{0}]", tableName);
        }

        #region Get DDL Statements

        private String GetBinaryDDLFromDataTypeName_bigint(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] BINARY(255) {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_binary(String columnName, Int32 length, Boolean isNullable)
        {
            if (length < 256)
            {
                return String.Format("[{0}] BINARY({1}) {2}", columnName, length, isNullable ? "NULL" : "NOT NULL");
            }
            return GetBinaryDDLFromDataTypeName_image(columnName, isNullable);
        }

        private String GetBinaryDDLFromDataTypeName_bit(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] BIT {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_char(String columnName, Int32 length, Boolean isNullable)
        {
            if (length < 256)
            {
                return String.Format("[{0}] TEXT({1}) {2}", columnName, length, isNullable ? "NULL" : "NOT NULL");
            }
            return GetBinaryDDLFromDataTypeName_text(columnName, isNullable);
        }

        private String GetBinaryDDLFromDataTypeName_date(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] DATETIME {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_datetime(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] DATETIME {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_datetime2(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] DATETIME {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_datetimeoffset(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] TEXT(34) {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_decimal(String columnName, Int16 precision, Int16 scale, Boolean isNullable)
        {
            return String.Format("[{0}] DECIMAL({1},{2}) {3}", columnName, precision, scale, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_float(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] FLOAT {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_geography(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] IMAGE {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_geometry(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] IMAGE {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_hierarchyid(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] IMAGE {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_image(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] IMAGE {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_int(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] INTEGER {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_money(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] MONEY {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_nchar(String columnName, Int32 length, Boolean isNullable)
        {
            if (length < 256)
            {
                return String.Format("[{0}] TEXT({1}) {2}", columnName, length, isNullable ? "NULL" : "NOT NULL");
            }
            return GetBinaryDDLFromDataTypeName_text(columnName, isNullable);
        }

        private String GetBinaryDDLFromDataTypeName_ntext(String columnName, Boolean isNullable)
        {
            return GetBinaryDDLFromDataTypeName_text(columnName, isNullable);
        }

        private String GetBinaryDDLFromDataTypeName_numeric(String columnName, Int16 precision, Int16 scale, Boolean isNullable)
        {
            return String.Format("[{0}] DECIMAL({1},{2}) {3}", columnName, precision, scale, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_nvarchar(String columnName, Int32 length, Boolean isNullable)
        {
            if (length < 256)
            {
                return String.Format("[{0}] TEXT({1}) {2}", columnName, length, isNullable ? "NULL" : "NOT NULL");
            }
            return GetBinaryDDLFromDataTypeName_text(columnName, isNullable);
        }

        private String GetBinaryDDLFromDataTypeName_real(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] REAL {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_smalldatetime(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] DATETIME {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_smallint(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] SMALLINT {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_smallmoney(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] MONEY {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_sql_variant(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] TEXT(255) {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_text(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] TEXT {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_time(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] TEXT(16) {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_timestamp(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] BINARY(8) {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_tinyint(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] TINYINT {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_uniqueidentifier(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] UNIQUEIDENTIFIER {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        private String GetBinaryDDLFromDataTypeName_varbinary(String columnName, Int32 length, Boolean isNullable)
        {
            if (length < 256)
            {
                return String.Format("[{0}] BINARY({1}) {2}", columnName, length, isNullable ? "NULL" : "NOT NULL");
            }
            return GetBinaryDDLFromDataTypeName_image(columnName, isNullable);
        }

        private String GetBinaryDDLFromDataTypeName_varchar(String columnName, Int32 length, Boolean isNullable)
        {
            if (length < 256)
            {
                return String.Format("[{0}] TEXT({1}) {2}", columnName, length, isNullable ? "NULL" : "NOT NULL");
            }
            return GetBinaryDDLFromDataTypeName_text(columnName, isNullable);
        }

        private String GetBinaryDDLFromDataTypeName_xml(String columnName, Boolean isNullable)
        {
            return String.Format("[{0}] TEXT {1}", columnName, isNullable ? "NULL" : "NOT NULL");
        }

        #endregion Get DDL Statements
    }
}