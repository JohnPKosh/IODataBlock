using Business.Common.Extensions;
using Business.Common.IO;
using Business.Excel;
using Data.DbClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;

namespace Business.Wpf.Data
{
    public class ReportDataTable : INotifyPropertyChanged
    {
        #region Class Initialization

        public ReportDataTable()
        {
            var idval = DateTime.Now.Ticks.ToString();
            _Id = idval;
            _Dt = new DataTable(idval);
            _Dv = new DataView(_Dt);
            CreatedDate = DateTime.Now;
            DvIsEmpty = true;
        }

        #endregion Class Initialization

        #region Fields and Properties

        private DataTable _Dt;

        public DataTable Dt
        {
            get
            {
                return _Dt;
            }
            set
            {
                if (_Dt == value) return;
                _Dt = value;
                _Dv = new DataView(_Dt)
                {
                    Sort = Sort,
                    RowFilter = RowFilter
                };
                DvIsEmpty = Dv.Count <= 0;
                Id = DateTime.Now.Ticks.ToString();
                CreatedDate = DateTime.Now;
                if (PropertyChanged == null) return;
                PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                PropertyChanged(this, new PropertyChangedEventArgs("Dv"));

                //this.PropertyChanged(this, new PropertyChangedEventArgs("DvIsEmpty"));
            }
        }

        private DataView _Dv;

        public DataView Dv
        {
            get
            {
                return _Dv;
            }
            set
            {
                if (_Dv == value) return;
                _Dv = value;
                DvIsEmpty = Dv.Count <= 0;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Dv"));
            }
        }

        private string _GroupId;

        public string GroupId
        {
            get
            {
                return _GroupId;
            }
            set
            {
                if (_GroupId == value) return;
                _GroupId = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("GroupId"));
            }
        }

        private string _Id;

        public string Id
        {
            get
            {
                return _Id;
            }
            set
            {
                if (_Id == value && !string.IsNullOrWhiteSpace(value)) return;
                _Id = string.IsNullOrWhiteSpace(value) ? DateTime.Now.Ticks.ToString() : value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Id"));
            }
        }

        private string _ReportName = "Report Results";

        public string ReportName
        {
            get
            {
                return _ReportName;
            }
            set
            {
                if (_ReportName == value) return;
                _ReportName = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ReportName"));
            }
        }

        private string _RowFilter;

        public string RowFilter
        {
            get
            {
                return _RowFilter;
            }
            set
            {
                if (_RowFilter == value) return;
                _RowFilter = value;
                _Dv.RowFilter = value;
                DvIsEmpty = Dv.Count <= 0;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("RowFilter"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
                }
            }
        }

        private string _Sort;

        public string Sort
        {
            get
            {
                return _Sort;
            }
            set
            {
                if (_Sort == value) return;
                _Sort = value;
                _Dv.Sort = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("Sort"));
                    PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
                }
            }
        }

        private bool _DvIsEmpty;

        public bool DvIsEmpty
        {
            get
            {
                return _DvIsEmpty;
            }
            set
            {
                if (value == _DvIsEmpty) return;
                _DvIsEmpty = value;
                if (PropertyChanged == null) return;
                PropertyChanged(this, new PropertyChangedEventArgs("DvIsEmpty"));
                PropertyChanged(this, new PropertyChangedEventArgs("DvIsNotEmpty"));
            }
        }

        public bool DvIsNotEmpty
        {
            get
            {
                return !_DvIsEmpty;
            }
            set
            {
                if (value != _DvIsEmpty) return;
                _DvIsEmpty = !value;
                if (PropertyChanged == null) return;
                PropertyChanged(this, new PropertyChangedEventArgs("DvIsEmpty"));
                PropertyChanged(this, new PropertyChangedEventArgs("DvIsNotEmpty"));
            }
        }

        private DateTime _CreatedDate;

        public DateTime CreatedDate
        {
            get
            {
                return _CreatedDate;
            }
            set
            {
                if (_CreatedDate == value) return;
                _CreatedDate = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CreatedDate"));
            }
        }

        private int _CurrentBatchNumber;

        public int CurrentBatchNumber
        {
            get
            {
                return _CurrentBatchNumber;
            }
            set
            {
                if (_CurrentBatchNumber == value) return;
                _CurrentBatchNumber = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentBatchNumber"));
            }
        }

        private int _CurrentBatchSize = 100000;

        public int CurrentBatchSize
        {
            get
            {
                return _CurrentBatchSize;
            }
            set
            {
                if (_CurrentBatchSize == value) return;
                _CurrentBatchSize = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentBatchSize"));
            }
        }

        private string _CurrentRowOrderBy;

        public string CurrentRowOrderBy
        {
            get
            {
                return _CurrentRowOrderBy;
            }
            set
            {
                if (_CurrentRowOrderBy == value) return;
                _CurrentRowOrderBy = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("CurrentRowOrderBy"));
            }
        }

        #endregion Fields and Properties

        #region Public Methods

        #region DataView Utility Methods

        public void ApplyFilterSort(string rowFilter, string sort)
        {
            RowFilter = rowFilter;
            Sort = sort;
            DvIsEmpty = Dv.Count <= 0;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Dv"));
        }

        #endregion DataView Utility Methods

        #region DataTable Utility Methods

        public void ClearData()
        {
            Dt.Clear();
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
            PropertyChanged(this, new PropertyChangedEventArgs("DvIsEmpty"));
        }

        #endregion DataTable Utility Methods

        #region Utility Methods

        public dynamic QueryValue(string commandText, string connectionString, string providerString = "System.Data.SqlClient", int commandTimeout = 0, params object[] args)
        {
            using (var db = Database.OpenConnectionString(connectionString, providerString))
            {
                return (int)db.QueryValue(commandText, commandTimeout, args);
            }
        }

        #endregion Utility Methods

        #region Paging Utility Methods

        public int GetPageCountFromSql(string selectCommandText, string connectionString, int batchSize)
        {
            selectCommandText = Database.CreateSqlServer2008CountSelect(selectCommandText, batchSize);
            using (var db = Database.OpenConnectionString(connectionString, "System.Data.SqlClient"))
            {
                return (int)db.QueryValue(selectCommandText, 360);
            }
        }

        public int GetCountFromSql(string selectCommandText, string connectionString)
        {
            selectCommandText = Database.CreateSqlServer2008CountSelect(selectCommandText, 0);
            using (var db = Database.OpenConnectionString(connectionString, "System.Data.SqlClient"))
            {
                return (int)db.QueryValue(selectCommandText, 360);
            }
        }

        #endregion Paging Utility Methods

        #region Load Data Methods

        public void LoadFromDataTable(DataTable dt)
        {
            Dt = dt;
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        }

        public void LoadFromQuery(string connectionString, string providerName, string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            Dt = Database.QueryToDataTable(connectionString, providerName, commandText, tableName, commandTimeout, commandTimeout, parameters);
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
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

        public void LoadFromSqlServer2008PagedQuery(string connectionString, string providerName, string commandText, int batchNumber, int batchSize, string rowOrderBy, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            commandText = Database.CreateSqlServer2008BatchSelect(commandText, batchNumber, batchSize, rowOrderBy);
            Dt = Database.QueryToDataTable(connectionString, providerName, commandText, tableName, commandTimeout, commandTimeout, parameters);
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
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
            , int firstrow = 1
            , string quotedNewLineReplacement = null
            , string quotedDelimiterReplacement = null
            , string trimChars = "\"' "
            , DataTable template = null
            , bool useFirstRowAsColumnNames = false
            , IEnumerable<string> columnNames = null
            , IEnumerable<Type> columnDataTypes = null
            )
        {
            Dt = stringdata.Lines(delimiter, firstrow, quotedNewLineReplacement, quotedDelimiterReplacement, trimChars)
                .LinesToDataTable(template, useFirstRowAsColumnNames, columnNames, columnDataTypes);
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        }

        #endregion Load Data Methods

        #region Load Schema Methods

        public void LoadSchemaOnly(string connectionString, string providerName, string commandText, string tableName = null, int commandTimeout = 60, params object[] parameters)
        {
            Dt = Database.FillSchemaDataTable(connectionString, providerName, commandText, tableName, commandTimeout, parameters);
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
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

        public void AddColumn(string NewColumnName, Type ColumnType = null, object FieldValue = null)
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

            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        }

        public void DeleteColumn(string ColumnName)
        {
            if (Dt.Columns.Cast<DataColumn>().All(c => c.ColumnName != ColumnName)) return;
            Dt.Columns.Remove(ColumnName);
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        }

        public void DeleteColumnAt(int Position)
        {
            Dt.Columns.RemoveAt(Position);
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        }

        public void RenameColumn(string CurrentColumnName, string NewColumnName)
        {
            if (Dt.Columns[CurrentColumnName] != null) Dt.Columns[CurrentColumnName].ColumnName = NewColumnName;
            if (PropertyChanged == null) return;
            PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
            PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
        }

        public void MergeColumns(string FirstColumn, string SecondColumn)
        {
            if (Dt.Columns[FirstColumn].DataType.UnderlyingSystemType == typeof(string)
                && Dt.Columns[FirstColumn].DataType.UnderlyingSystemType == typeof(string))
            {
                /* both are string continue to process */
                foreach (DataRow r in Dt.Rows)
                {
                    var v1 = r.Field<string>(Dt.Columns[FirstColumn]);
                    var v2 = r.Field<string>(Dt.Columns[SecondColumn]);
                    var mergedstring = v1.Trim() + v2.Trim();
                    r.SetField<string>(Dt.Columns[FirstColumn], mergedstring);
                }
                Dt.AcceptChanges();
                DeleteColumn(SecondColumn);
            }
            else if (Dt.Columns[FirstColumn].DataType.UnderlyingSystemType == Dt.Columns[FirstColumn].DataType.UnderlyingSystemType)
            {
                /* both are same type continue to process */
            }
        }

        public void StripPrefixFromColumnName(string ColumnName, string Prefix)
        {
            if (Dt.Columns[ColumnName].DataType.UnderlyingSystemType == typeof(string))
            {
                /* both are string continue to process */
                foreach (DataRow r in Dt.Rows)
                {
                    var col = r.Field<string>(Dt.Columns[ColumnName]);
                    if (col.StartsWith(Prefix))
                    {
                        r.SetField<string>(Dt.Columns[ColumnName], col.Substring(Prefix.Length));
                    }
                }
                Dt.AcceptChanges();
                if (PropertyChanged == null) return;
                PropertyChanged(this, new PropertyChangedEventArgs("Dt"));
                PropertyChanged(this, new PropertyChangedEventArgs("Dv"));
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
            var cols = new DataColumn[Dt.Columns.Count];
            Dt.Columns.CopyTo(cols, 0);
            return cols.ToList();
        }

        public List<string> GetColumnNameList()
        {
            return GetColumnList().Select(x => x.ColumnName).ToList();
        }

        #endregion Column List Methods

        #region Write Data Methods

        public void WriteXmlFromDt(string FilePath)
        {
            if (string.IsNullOrWhiteSpace(Dt.TableName)) Dt.TableName = @"Data";
            Dt.WriteXml(FilePath);
        }

        public void WriteXmlSchemaFromDt(string FilePath)
        {
            if (string.IsNullOrWhiteSpace(Dt.TableName)) Dt.TableName = @"Data";
            Dt.WriteXmlSchema(FilePath);
        }

        public void WriteExcelFromDt(string FilePath)
        {
            var eo = new ExcelDynamicObjects();
            eo.CreateExcelFileFromDataTable(new FileInfo(FilePath), Dt, IOUtility.DefaultFolderPath, overWrite: true);
        }

        public void WriteSeperatedTxtFileFromDataTable(FileInfo file,
            DataTable Data,
            bool ColHeaders = false,
            string FieldSeperator = "\t",
            string TextQualifier = null,
            string NewLineChar = "\r\n",
            string NullValue = "")
        {
            StringExtensionBase.CreateSeperatedTxtFileFromDataTable(file, Data, ColHeaders, FieldSeperator, TextQualifier, NewLineChar, NullValue);
        }

        public void WriteSeperatedTxtFileFromDataTable(string FilePath,
            DataTable Data,
            bool ColHeaders = false,
            string FieldSeperator = "\t",
            string TextQualifier = null,
            string NewLineChar = "\r\n",
            string NullValue = "")
        {
            StringExtensionBase.CreateSeperatedTxtFileFromDataTable(new FileInfo(FilePath), Data, ColHeaders, FieldSeperator, TextQualifier, NewLineChar, NullValue);
        }

        #endregion Write Data Methods

        #endregion Public Methods

        #region INotifyPropertyChanged Section

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion INotifyPropertyChanged Section
    }
}