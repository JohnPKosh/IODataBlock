//using ExBaseData;
//using ExBaseDataUtil;
//using ExBaseExcelData;
//using ExBaseIoUtil;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using Business.Common.Extensions;
using Business.Common.IO;
using Business.Excel;
using Data.DbClient;

namespace Business.Wpf.Data
{
    public class ReportDataTable : INotifyPropertyChanged
    {
        #region Class Initialization

        public ReportDataTable()
        {
            var idval = DateTime.Now.Ticks.ToString();
            this._Id = idval;
            this._Dt = new DataTable(idval);
            this._Dv = new DataView(_Dt);
            this.CreatedDate = DateTime.Now;
            this.DvIsEmpty = true;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private DataTable _Dt;

        public DataTable Dt
        {
            get
            {
                return this._Dt;
            }
            set
            {
                if (this._Dt == value) return;
                this._Dt = value;
                this._Dv = new DataView(_Dt);
                this._Dv.Sort = this.Sort;
                this._Dv.RowFilter = this.RowFilter;
                this.DvIsEmpty = this.Dv.Count > 0 ? false : true;
                this.Id = DateTime.Now.Ticks.ToString();
                this.CreatedDate = DateTime.Now;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));

                    //this.PropertyChanged(this, new PropertyChangedEventArgs("DvIsEmpty"));
                }
            }
        }

        private DataView _Dv;

        public DataView Dv
        {
            get
            {
                return this._Dv;
            }
            set
            {
                if (this._Dv == value) return;
                this._Dv = value;
                this.DvIsEmpty = this.Dv.Count > 0 ? false : true;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
            }
        }

        private String _GroupId;

        public String GroupId
        {
            get
            {
                return this._GroupId;
            }
            set
            {
                if (this._GroupId == value) return;
                this._GroupId = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("GroupId"));
            }
        }

        private String _Id;

        public String Id
        {
            get
            {
                return this._Id;
            }
            set
            {
                if (this._Id == value && !String.IsNullOrWhiteSpace(value)) return;
                this._Id = String.IsNullOrWhiteSpace(value) ? DateTime.Now.Ticks.ToString() : value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Id"));
            }
        }

        private String _ReportName = "Report Results";

        public String ReportName
        {
            get
            {
                return this._ReportName;
            }
            set
            {
                if (this._ReportName == value) return;
                this._ReportName = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("ReportName"));
            }
        }

        private String _RowFilter;

        public String RowFilter
        {
            get
            {
                return this._RowFilter;
            }
            set
            {
                if (this._RowFilter == value) return;
                this._RowFilter = value;
                this._Dv.RowFilter = value;
                this.DvIsEmpty = this.Dv.Count > 0 ? false : true;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("RowFilter"));
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
                }
            }
        }

        private String _Sort;

        public String Sort
        {
            get
            {
                return this._Sort;
            }
            set
            {
                if (this._Sort == value) return;
                this._Sort = value;
                this._Dv.Sort = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Sort"));
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
                }
            }
        }

        private Boolean _DvIsEmpty;

        public Boolean DvIsEmpty
        {
            get
            {
                return _DvIsEmpty;
            }
            set
            {
                if (value == _DvIsEmpty) return;
                _DvIsEmpty = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("DvIsEmpty"));
                    this.PropertyChanged(this, new PropertyChangedEventArgs("DvIsNotEmpty"));
                }
            }
        }

        public Boolean DvIsNotEmpty
        {
            get
            {
                return !_DvIsEmpty;
            }
            set
            {
                if (value != _DvIsEmpty) return;
                _DvIsEmpty = !value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("DvIsEmpty"));
                    this.PropertyChanged(this, new PropertyChangedEventArgs("DvIsNotEmpty"));
                }
            }
        }

        private DateTime _CreatedDate;

        public DateTime CreatedDate
        {
            get
            {
                return this._CreatedDate;
            }
            set
            {
                if (this._CreatedDate == value) return;
                this._CreatedDate = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CreatedDate"));
            }
        }

        private Int32 _CurrentBatchNumber = 0;

        public Int32 CurrentBatchNumber
        {
            get
            {
                return this._CurrentBatchNumber;
            }
            set
            {
                if (this._CurrentBatchNumber == value) return;
                this._CurrentBatchNumber = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentBatchNumber"));
            }
        }

        private Int32 _CurrentBatchSize = 100000;

        public Int32 CurrentBatchSize
        {
            get
            {
                return this._CurrentBatchSize;
            }
            set
            {
                if (this._CurrentBatchSize == value) return;
                this._CurrentBatchSize = value;
                if (this.PropertyChanged != null)
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentBatchSize"));
            }
        }

        private String _CurrentRowOrderBy;

        public String CurrentRowOrderBy
        {
            get
            {
                return this._CurrentRowOrderBy;
            }
            set
            {
                if (this._CurrentRowOrderBy == value) return;
                this._CurrentRowOrderBy = value;
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("CurrentRowOrderBy"));
                }
            }
        }

        #endregion Fields and Properties

        #region Public Methods

        #region DataView Utility Methods

        public void ApplyFilterSort(String RowFilter, String Sort)
        {
            this.RowFilter = RowFilter;
            this.Sort = Sort;
            this.DvIsEmpty = this.Dv.Count > 0 ? false : true;
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        }

        #endregion DataView Utility Methods

        #region DataTable Utility Methods

        public void ClearData()
        {
            this.Dt.Clear();
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
                this.PropertyChanged(this, new PropertyChangedEventArgs("DvIsEmpty"));
            }
        }

        #endregion DataTable Utility Methods

        #region Utility Methods

        public dynamic QueryValue(string commandText, String connectionString, String providerString = "System.Data.SqlClient", int commandTimeout = 0, params object[] args)
        {
            using (Database db = Database.OpenConnectionString(connectionString, providerString))
            {
                return (int)db.QueryValue(commandText, commandTimeout, args);
            }
        }

        #endregion Utility Methods

        #region Paging Utility Methods

        public int GetPageCountFromSql(String selectCommandText, String connectionString, int batchSize)
        {
            selectCommandText = Database.CreateSqlServer2008CountSelect(selectCommandText, batchSize);
            using (Database db = Database.OpenConnectionString(connectionString, "System.Data.SqlClient"))
            {
                return (int)db.QueryValue(selectCommandText, 360);
            }
        }

        public int GetCountFromSql(String selectCommandText, String connectionString)
        {
            selectCommandText = Database.CreateSqlServer2008CountSelect(selectCommandText, 0);
            using (Database db = Database.OpenConnectionString(connectionString, "System.Data.SqlClient"))
            {
                return (int)db.QueryValue(selectCommandText, 360);
            }
        }

        #endregion Paging Utility Methods

        #region Load Data Methods

        public void LoadFromDataTable(DataTable dt)
        {
            Dt = dt;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
            }
        }

        public void LoadFromQuery(string connectionString, String providerName, string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            Dt = Database.QueryToDataTable(connectionString, providerName, commandText, tableName, commandTimeout, commandTimeout, parameters);
            if (this.PropertyChanged == null) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        }

        //public void LoadFromSql(String selectCommandText
        //    , String SqlServer
        //    , String DatabaseName
        //    , String SqlUserName = null
        //    , String SqlPassword = null
        //    , String ApplicationName = null
        //    , Int32 ConnectTimeout = 120
        //    , String TableName = null
        //    )
        //{
        //    Dt = Database.QueryToDataTable(selectCommandText, SqlServer, DatabaseName, SqlUserName, SqlPassword, ApplicationName, ConnectTimeout, TableName);
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        //    }
        //}

        //public void LoadFromSql(String selectCommandText, String connectionString, String TableName = null)
        //{
        //    Dt = DataExtensionBase.GetDataTableBySqlDataAdapter(selectCommandText, connectionString, TableName);
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        //    }
        //}


        public void LoadFromSqlServer2008PagedQuery(string connectionString, String providerName, string commandText, Int32 batchNumber, Int32 batchSize, String rowOrderBy, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            commandText = Database.CreateSqlServer2008BatchSelect(commandText, batchNumber, batchSize, rowOrderBy);
            Dt = Database.QueryToDataTable(connectionString, providerName, commandText, tableName, commandTimeout, commandTimeout, parameters);
            if (this.PropertyChanged == null) return;
            this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        }

        //public void LoadBatchFromSql(String selectCommandText
        //    , Int32 BatchNumber
        //    , Int32 BatchSize
        //    , String RowOrderBy
        //    , String SqlServer
        //    , String DatabaseName
        //    , String SqlUserName = null
        //    , String SqlPassword = null
        //    , String ApplicationName = null
        //    , Int32 ConnectTimeout = -1
        //    , String TableName = null
        //    )
        //{
        //    selectCommandText = Database.CreateSqlServer2008BatchSelect(selectCommandText, BatchNumber, BatchSize, RowOrderBy);
        //    Dt = DataExtensionBase.GetDataTableBySqlDataAdapter(selectCommandText, SqlServer, DatabaseName, SqlUserName, SqlPassword, ApplicationName, ConnectTimeout, TableName);
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        //    }
        //}

        //public void LoadBatchFromSql(String selectCommandText
        //    , Int32 BatchNumber
        //    , Int32 BatchSize
        //    , String RowOrderBy
        //    , String connectionString
        //    , String TableName = null)
        //{
        //    selectCommandText = DataExtensionBase.CreateSQLServerBatchSelect(selectCommandText, BatchNumber, BatchSize, RowOrderBy);
        //    Dt = DataExtensionBase.GetDataTableBySqlDataAdapter(selectCommandText, connectionString, TableName);
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        //    }
        //}

        public void LoadFromLineData(string stringdata
            , string delimiter = "\t"
            , Int32 firstrow = 1
            , String quotedNewLineReplacement = null
            , String quotedDelimiterReplacement = null
            , String trimChars = "\"' "
            , DataTable template = null
            , Boolean useFirstRowAsColumnNames = false
            , IEnumerable<String> columnNames = null
            , IEnumerable<Type> columnDataTypes = null
            )
        {
            Dt = (stringdata.Lines(delimiter, firstrow, quotedNewLineReplacement, quotedDelimiterReplacement, trimChars))
                .LinesToDataTable(template, useFirstRowAsColumnNames, columnNames, columnDataTypes);
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
            }
        }

        #endregion Load Data Methods

        #region Load Schema Methods

        public void LoadSchemaOnly(string connectionString, String providerName, string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            Dt = Database.FillSchemaDataTable(connectionString, providerName, commandText, tableName, commandTimeout, parameters);
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
            }
        }

        //public void LoadSchemaOnlyFromSql(String selectCommandText
        //    , String SqlServer
        //    , String DatabaseName
        //    , String SqlUserName = null
        //    , String SqlPassword = null
        //    , String ApplicationName = null
        //    , Int32 ConnectTimeout = -1
        //    , String TableName = null
        //    )
        //{
        //    Dt = DataExtensionBase.GetSchemaDataTableBySqlDataAdapter(selectCommandText, SqlServer, DatabaseName, SqlUserName, SqlPassword, ApplicationName, ConnectTimeout, TableName);
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        //    }
        //}

        //public void LoadSchemaOnlyFromSql(String selectCommandText, String connectionString, String TableName = null)
        //{
        //    Dt = DataExtensionBase.GetSchemaDataTableBySqlDataAdapter(selectCommandText, connectionString, TableName);
        //    if (this.PropertyChanged != null)
        //    {
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
        //        this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        //    }
        //}

        #endregion Load Schema Methods

        #region Column Utility Methods

        public void AddColumn(String NewColumnName, Type ColumnType = null, Object FieldValue = null)
        {
            if (Dt.Columns[NewColumnName] == null)
            {
                if (ColumnType != null)
                {
                    Dt.Columns.Add(NewColumnName, ColumnType);
                }
                else
                {
                    Dt.Columns.Add(NewColumnName);
                }
            }

            if (FieldValue != null)
            {
                foreach (DataRow r in Dt.Rows)
                {
                    r.SetField(Dt.Columns[NewColumnName], FieldValue);
                }
            }
            Dt.AcceptChanges();

            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
            }
        }

        public void DeleteColumn(String ColumnName)
        {
            foreach (DataColumn c in Dt.Columns)
            {
                if (c.ColumnName == ColumnName)
                {
                    Dt.Columns.Remove(ColumnName);
                    if (this.PropertyChanged != null)
                    {
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                        this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
                    }
                    break;
                }
            }
        }

        public void DeleteColumnAt(int Position)
        {
            Dt.Columns.RemoveAt(Position);
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
            }
        }

        public void RenameColumn(String CurrentColumnName, String NewColumnName)
        {
            if (Dt.Columns[CurrentColumnName] != null) Dt.Columns[CurrentColumnName].ColumnName = NewColumnName;
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
            }
        }

        public void MergeColumns(String FirstColumn, String SecondColumn)
        {
            if (Dt.Columns[FirstColumn].DataType.UnderlyingSystemType == typeof(String)
                && Dt.Columns[FirstColumn].DataType.UnderlyingSystemType == typeof(String))
            {
                /* both are string continue to process */
                foreach (DataRow r in Dt.Rows)
                {
                    var v1 = r.Field<String>(Dt.Columns[FirstColumn]);
                    var v2 = r.Field<String>(Dt.Columns[SecondColumn]);
                    var mergedstring = (v1.Trim() + v2.Trim());
                    r.SetField<String>(Dt.Columns[FirstColumn], mergedstring);
                }
                Dt.AcceptChanges();
                DeleteColumn(SecondColumn);
            }
            else if (Dt.Columns[FirstColumn].DataType.UnderlyingSystemType == Dt.Columns[FirstColumn].DataType.UnderlyingSystemType)
            {
                /* both are same type continue to process */
            }
        }

        public void StripPrefixFromColumnName(String ColumnName, String Prefix)
        {
            if (Dt.Columns[ColumnName].DataType.UnderlyingSystemType == typeof(String))
            {
                /* both are string continue to process */
                foreach (DataRow r in Dt.Rows)
                {
                    var col = r.Field<String>(Dt.Columns[ColumnName]);
                    if (col.StartsWith(Prefix))
                    {
                        r.SetField<String>(Dt.Columns[ColumnName], col.Substring(Prefix.Length));
                    }
                }
                Dt.AcceptChanges();
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                    this.PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
                }
            }
            else
            {
                /* convert to string and continue to process */
            }
        }

        #endregion Column Utility Methods

        #region Column List Methods

        public List<DataColumn> GetColumnList()
        {
            if (Dt == null) return new List<DataColumn>();
            DataColumn[] cols = new DataColumn[Dt.Columns.Count];
            Dt.Columns.CopyTo(cols, 0);
            return cols.ToList();
        }

        public List<String> GetColumnNameList()
        {
            return GetColumnList().Select(x => x.ColumnName).ToList();
        }

        #endregion Column List Methods

        #region Write Data Methods

        public void WriteXmlFromDt(String FilePath)
        {
            if (String.IsNullOrWhiteSpace(this.Dt.TableName)) this.Dt.TableName = @"Data";
            this.Dt.WriteXml(FilePath);
        }

        public void WriteXmlSchemaFromDt(String FilePath)
        {
            if (String.IsNullOrWhiteSpace(this.Dt.TableName)) this.Dt.TableName = @"Data";
            this.Dt.WriteXmlSchema(FilePath);
        }

        public void WriteExcelFromDt(String FilePath)
        {
            ExcelDynamicObjects eo = new ExcelDynamicObjects();
            eo.CreateExcelFileFromDataTable(new System.IO.FileInfo(FilePath), this.Dt, IOUtility.DefaultFolderPath, overWrite: true);
        }

        public void WriteSeperatedTxtFileFromDataTable(FileInfo file,
            DataTable Data,
            Boolean ColHeaders = false,
            String FieldSeperator = "\t",
            String TextQualifier = null,
            String NewLineChar = "\r\n",
            String NullValue = "")
        {
            StringExtensionBase.CreateSeperatedTxtFileFromDataTable(file, Data, ColHeaders, FieldSeperator, TextQualifier, NewLineChar, NullValue);
        }

        public void WriteSeperatedTxtFileFromDataTable(String FilePath,
            DataTable Data,
            Boolean ColHeaders = false,
            String FieldSeperator = "\t",
            String TextQualifier = null,
            String NewLineChar = "\r\n",
            String NullValue = "")
        {
            StringExtensionBase.CreateSeperatedTxtFileFromDataTable(new System.IO.FileInfo(FilePath), Data, ColHeaders, FieldSeperator, TextQualifier, NewLineChar, NullValue);
        }

        #endregion Write Data Methods

        #endregion Public Methods

        #region INotifyPropertyChanged Section

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Section
    }
}