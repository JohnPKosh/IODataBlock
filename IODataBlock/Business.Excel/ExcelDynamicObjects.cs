//using ExBaseData;

using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using Data.DbClient;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace Business.Excel
{
    public class ExcelDynamicObjects
    {
        public Stream GetExcelStreamFromDynamicObjects(IEnumerable<dynamic> objectCollection, String workSheetName = "Results", dynamic officeProperties = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var rowcnt = objectCollection.Count() + 1;
            var xlPackage = new ExcelPackage();
            var worksheet = xlPackage.Workbook.Worksheets.Add(workSheetName);

            // build worksheet
            if (worksheet != null)
            {
                if (objectCollection != null && objectCollection.Any())
                {
                    PropertyInfo[] oProps = null;// column names
                    var columnsExist = false;
                    var isExpando = false;
                    var row = 0;
                    var columncount = 0;
                    foreach (var r in objectCollection)
                    {
                        if (isExpando || r.GetType().Equals(typeof(ExpandoObject)))
                        {
                            if (!columnsExist)
                            {
                                row++;
                                isExpando = true;
                                var i = 0;
                                foreach (var c in (IDictionary<String, Object>)r)
                                {
                                    i++;
                                    worksheet.Cells[row, i].Value = c.Key;
                                    worksheet.Cells[row, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[row, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                                    worksheet.Cells[row, i].Style.Font.Bold = true;
                                }
                                columnsExist = true;
                                columncount = i;
                            }
                            row++;
                            var j = 0;
                            foreach (var c in (IDictionary<String, Object>)r)
                            {
                                j++;
                                worksheet.Cells[row, j].Value = c.Value;
                                if (c.Value is DateTime)
                                {
                                    worksheet.Cells[row, j].Style.Numberformat.Format = @"yyyy-MM-dd HH:mm:ss";
                                }
                            }
                        }
                        else
                        {
                            if (oProps == null) oProps = ((Type)r.GetType()).GetProperties();
                            if (!columnsExist)
                            {
                                row++;
                                var i = 0;
                                foreach (var pi in oProps)
                                {
                                    i++;
                                    worksheet.Cells[row, i].Value = pi.Name;
                                    worksheet.Cells[row, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    worksheet.Cells[row, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                                    worksheet.Cells[row, i].Style.Font.Bold = true;
                                }
                                columnsExist = true;
                                columncount = i;
                            }
                            row++;
                            var j = 0;
                            foreach (var pi in oProps)
                            {
                                j++;
                                var val = pi.GetValue(r, null);
                                worksheet.Cells[row, j].Value = val;
                                if (val is DateTime)
                                {
                                    worksheet.Cells[row, j].Style.Numberformat.Format = @"yyyy-MM-dd HH:mm:ss";
                                }
                            }
                        }
                    }

                    // set AutoFilter
                    worksheet.Cells[1, 1, rowcnt, columncount].AutoFilter = true;
                    worksheet.Cells[1, 1, rowcnt, columncount].Style.Font.Size = 9;
                    var worksheetview = worksheet.View;
                    worksheetview.FreezePanes(2, 1);

                    for (var i = 0; i < columncount; i++)
                    {
                        worksheet.Column(i + 1).BestFit = true; // does not seem to work?
                        worksheet.Column(i + 1).Width = 18; // sets width
                    }

                    // set property values
                    if (officeProperties != null)
                    {
                        if (officeProperties.Author != null) xlPackage.Workbook.Properties.Author = officeProperties.Author;
                        if (officeProperties.Category != null) xlPackage.Workbook.Properties.Category = officeProperties.Category;
                        if (officeProperties.Comments != null) xlPackage.Workbook.Properties.Comments = officeProperties.Comments;
                        if (officeProperties.Company != null) xlPackage.Workbook.Properties.Company = officeProperties.Company;
                        if (officeProperties.HyperlinkBase != null) xlPackage.Workbook.Properties.HyperlinkBase = officeProperties.HyperlinkBase;
                        if (officeProperties.Keywords != null) xlPackage.Workbook.Properties.Keywords = officeProperties.Keywords;
                        if (officeProperties.LastModifiedBy != null) xlPackage.Workbook.Properties.LastModifiedBy = officeProperties.LastModifiedBy;
                        if (officeProperties.Manager != null) xlPackage.Workbook.Properties.Manager = officeProperties.Manager;
                        if (officeProperties.Status != null) xlPackage.Workbook.Properties.Status = officeProperties.Status;
                        if (officeProperties.Subject != null) xlPackage.Workbook.Properties.Subject = officeProperties.Subject;

                        xlPackage.Workbook.Properties.Title = officeProperties.Title ?? workSheetName;

                        if (officeProperties.CustomPropertyValues != null)
                        {
                            foreach (var p in officeProperties.CustomPropertyValues as List<Tuple<String, Object>>)
                            {
                                xlPackage.Workbook.Properties.SetCustomPropertyValue(p.Item1, p.Item2);
                            }
                        }
                    }
                    else
                    {
                        xlPackage.Workbook.Properties.Title = workSheetName;
                    }
                }
            }
            xlPackage.Save();

            //xlPackage.Stream.Flush();
            xlPackage.Stream.Position = 0;
            return xlPackage.Stream;
            // ReSharper restore PossibleMultipleEnumeration
        }

        public Byte[] GetExcelFromDynamicObjects(IEnumerable<dynamic> objectCollection, String workSheetName = "Results", dynamic officeProperties = null)
        {
            // ReSharper disable PossibleMultipleEnumeration
            var rowcnt = objectCollection.Count() + 1;
            var xlPackage = new ExcelPackage();
            var worksheet = xlPackage.Workbook.Worksheets.Add(workSheetName);

            // build worksheet
            if (worksheet == null) return xlPackage.GetAsByteArray();
            if (objectCollection == null || !objectCollection.Any()) return xlPackage.GetAsByteArray();
            PropertyInfo[] oProps = null;// column names
            var columnsExist = false;
            var isExpando = false;
            var row = 0;
            var columncount = 0;
            foreach (var r in objectCollection)
            {
                if (isExpando || r.GetType().Equals(typeof(ExpandoObject)))
                {
                    if (!columnsExist)
                    {
                        row++;
                        isExpando = true;
                        var i = 0;
                        foreach (var c in (IDictionary<String, Object>)r)
                        {
                            i++;
                            worksheet.Cells[row, i].Value = c.Key;
                            worksheet.Cells[row, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                            worksheet.Cells[row, i].Style.Font.Bold = true;
                        }
                        columnsExist = true;
                        columncount = i;
                    }
                    row++;
                    var j = 0;
                    foreach (var c in (IDictionary<String, Object>)r)
                    {
                        j++;
                        worksheet.Cells[row, j].Value = c.Value;
                        if (c.Value is DateTime)
                        {
                            worksheet.Cells[row, j].Style.Numberformat.Format = @"yyyy-MM-dd HH:mm:ss";
                        }
                    }
                }
                else
                {
                    if (oProps == null) oProps = ((Type)r.GetType()).GetProperties();
                    if (!columnsExist)
                    {
                        row++;
                        var i = 0;
                        foreach (var pi in oProps)
                        {
                            i++;
                            worksheet.Cells[row, i].Value = pi.Name;
                            worksheet.Cells[row, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                            worksheet.Cells[row, i].Style.Font.Bold = true;
                        }
                        columnsExist = true;
                        columncount = i;
                    }
                    row++;
                    var j = 0;
                    foreach (var pi in oProps)
                    {
                        j++;
                        var val = pi.GetValue(r, null);
                        worksheet.Cells[row, j].Value = val;
                        if (val is DateTime)
                        {
                            worksheet.Cells[row, j].Style.Numberformat.Format = @"yyyy-MM-dd HH:mm:ss";
                        }
                    }
                }
            }

            // set AutoFilter
            worksheet.Cells[1, 1, rowcnt, columncount].AutoFilter = true;
            worksheet.Cells[1, 1, rowcnt, columncount].Style.Font.Size = 9;
            var worksheetview = worksheet.View;
            worksheetview.FreezePanes(2, 1);

            for (var i = 0; i < columncount; i++)
            {
                worksheet.Column(i + 1).BestFit = true; // does not seem to work?
                worksheet.Column(i + 1).Width = 18; // sets width
            }

            // set property values
            if (officeProperties != null)
            {
                if (officeProperties.Author != null) xlPackage.Workbook.Properties.Author = officeProperties.Author;
                if (officeProperties.Category != null) xlPackage.Workbook.Properties.Category = officeProperties.Category;
                if (officeProperties.Comments != null) xlPackage.Workbook.Properties.Comments = officeProperties.Comments;
                if (officeProperties.Company != null) xlPackage.Workbook.Properties.Company = officeProperties.Company;
                if (officeProperties.HyperlinkBase != null) xlPackage.Workbook.Properties.HyperlinkBase = officeProperties.HyperlinkBase;
                if (officeProperties.Keywords != null) xlPackage.Workbook.Properties.Keywords = officeProperties.Keywords;
                if (officeProperties.LastModifiedBy != null) xlPackage.Workbook.Properties.LastModifiedBy = officeProperties.LastModifiedBy;
                if (officeProperties.Manager != null) xlPackage.Workbook.Properties.Manager = officeProperties.Manager;
                if (officeProperties.Status != null) xlPackage.Workbook.Properties.Status = officeProperties.Status;
                if (officeProperties.Subject != null) xlPackage.Workbook.Properties.Subject = officeProperties.Subject;

                xlPackage.Workbook.Properties.Title = officeProperties.Title ?? workSheetName;

                if (officeProperties.CustomPropertyValues != null)
                {
                    foreach (var p in officeProperties.CustomPropertyValues as List<Tuple<String, Object>>)
                    {
                        xlPackage.Workbook.Properties.SetCustomPropertyValue(p.Item1, p.Item2);
                    }
                }
            }
            else
            {
                xlPackage.Workbook.Properties.Title = workSheetName;
            }
            return xlPackage.GetAsByteArray();
            // ReSharper restore PossibleMultipleEnumeration
        }

        public Byte[] GetExcelFromDynamicCollections(IEnumerable<IEnumerable<dynamic>> objectCollection, IEnumerable<String> workSheetNames = null, String workBookTitle = "", dynamic officeProperties = null)
        {
            var xlPackage = new ExcelPackage();
            var names = new List<string>();
            if (workSheetNames != null) names = workSheetNames.ToList();
            var index = 0;
            foreach (var c in objectCollection)
            {
                if (index == names.Count) names.Add(String.Format(@"Sheet{0}", index + 1));
                AddWorksheetFromDynamicObjects(ref xlPackage, c, names[index]);
                index++;
            }
            // set property values
            SetExcelPackageProperties(ref xlPackage, workBookTitle, officeProperties);
            return xlPackage.GetAsByteArray();
        }

        public Byte[] GetExcelFromQuery(String selectQuery, String connectionStringName, String workSheetName = "Results", dynamic officeProperties = null)
        {
            using (var db = Database.Open(connectionStringName))
            {
                var rd = db.Query(selectQuery);
                return GetExcelFromDynamicObjects(rd, workSheetName, officeProperties);
            }
        }

        public FileInfo CreateExcelFileFromDynamicObjects(
            FileInfo fileInfo
            , IEnumerable<dynamic> objectCollection
            , String workSheetName = "Results"
            , dynamic officeProperties = null
            , Boolean overWrite = false
            , String tempFolderPath = null
            )
        {
            fileInfo.Refresh();
            if (fileInfo.Exists && !overWrite) throw new Exception(FileOverwriteRestrictionExStr(fileInfo.Name));
            if (fileInfo.Exists)
            {
                FileInfo temp = null;
                try
                {
                    if (!String.IsNullOrWhiteSpace(tempFolderPath))
                    {
                        var fn = Path.Combine(tempFolderPath, Path.GetRandomFileName());
                        temp = new FileInfo(fn);
                    }
                    else
                    {
                        var fn = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                        temp = new FileInfo(fn);
                    }
                    var xlPackage = new ExcelPackage(temp);
                    AddWorksheetFromDynamicObjects(ref xlPackage, objectCollection, workSheetName);
                    SetExcelPackageProperties(ref xlPackage, workSheetName, officeProperties);
                    xlPackage.Save();

                    var moveto = fileInfo.FullName;
                    if (fileInfo.Exists) fileInfo.Delete();
                    temp.MoveTo(moveto);
                }
                catch (Exception)
                {
                    if (temp != null && temp.Exists) temp.Delete();
                    throw;
                }
            }
            else
            {
                var xlPackage = new ExcelPackage(fileInfo);
                AddWorksheetFromDynamicObjects(ref xlPackage, objectCollection, workSheetName);
                SetExcelPackageProperties(ref xlPackage, workSheetName, officeProperties);
                xlPackage.Save();
            }

            fileInfo.Refresh();
            return fileInfo;
        }

        public FileInfo CreateExcelFileFromDynamicCollections(
            FileInfo fileInfo
            , IEnumerable<IEnumerable<dynamic>> objectCollection
            , IEnumerable<String> workSheetNames = null
            , String workBookTitle = ""
            , dynamic officeProperties = null
            , Boolean overWrite = false
            , String tempFolderPath = null
            )
        {
            fileInfo.Refresh();
            if (fileInfo.Exists && !overWrite) throw new Exception(FileOverwriteRestrictionExStr(fileInfo.Name));
            if (fileInfo.Exists)
            {
                FileInfo temp = null;
                try
                {
                    if (!String.IsNullOrWhiteSpace(tempFolderPath))
                    {
                        var fn = Path.Combine(tempFolderPath, Path.GetRandomFileName());
                        temp = new FileInfo(fn);
                    }
                    else
                    {
                        var fn = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                        temp = new FileInfo(fn);
                    }
                    var xlPackage = new ExcelPackage(temp);

                    var names = new List<string>();
                    if (workSheetNames != null) names = workSheetNames.ToList();

                    var index = 0;
                    foreach (var c in objectCollection)
                    {
                        // FIX names array may not have enough values!!!!!!!
                        if (index == names.Count) names.Add(String.Format(@"Sheet{0}", index + 1));
                        AddWorksheetFromDynamicObjects(ref xlPackage, c, names[index]);
                        index++;
                    }
                    SetExcelPackageProperties(ref xlPackage, workBookTitle, officeProperties);
                    xlPackage.Save();

                    var moveto = fileInfo.FullName;
                    if (fileInfo.Exists) fileInfo.Delete();
                    temp.MoveTo(moveto);
                }
                catch (Exception)
                {
                    if (temp.Exists) temp.Delete();
                    throw;
                }
            }
            else
            {
                var xlPackage = new ExcelPackage(fileInfo);
                var names = new List<string>();
                if (workSheetNames != null) names = workSheetNames.ToList();
                var index = 0;
                foreach (var c in objectCollection)
                {
                    // FIX names array may not have enough values!!!!!!!
                    if (index == names.Count) names.Add(String.Format(@"Sheet{0}", index + 1));
                    AddWorksheetFromDynamicObjects(ref xlPackage, c, names[index]);
                    index++;
                }
                SetExcelPackageProperties(ref xlPackage, workBookTitle, officeProperties);
                xlPackage.Save();
            }
            fileInfo.Refresh();
            return fileInfo;
        }

        public FileInfo CreateExcelFileFromDataTable(
        FileInfo fileInfo
        , DataTable data
        , String workSheetName = "Results"
        , dynamic officeProperties = null
        , Boolean overWrite = false
        , String tempFolderPath = null
        )
        {
            fileInfo.Refresh();
            if (fileInfo.Exists && !overWrite) throw new Exception(FileOverwriteRestrictionExStr(fileInfo.Name));
            if (fileInfo.Exists)
            {
                FileInfo temp = null;
                try
                {
                    if (!String.IsNullOrWhiteSpace(tempFolderPath))
                    {
                        var fn = Path.Combine(tempFolderPath, Path.GetRandomFileName());
                        temp = new FileInfo(fn);
                    }
                    else
                    {
                        var fn = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                        temp = new FileInfo(fn);
                    }
                    var xlPackage = new ExcelPackage(temp);
                    AddWorksheetFromDataTable(ref xlPackage, data, workSheetName);
                    SetExcelPackageProperties(ref xlPackage, workSheetName, officeProperties);
                    xlPackage.Save();

                    var moveto = fileInfo.FullName;
                    if (fileInfo.Exists) fileInfo.Delete();
                    temp.MoveTo(moveto);
                }
                catch (Exception)
                {
                    if (temp != null && temp.Exists) temp.Delete();
                    throw;
                }
            }
            else
            {
                var xlPackage = new ExcelPackage(fileInfo);
                AddWorksheetFromDataTable(ref xlPackage, data, workSheetName);
                SetExcelPackageProperties(ref xlPackage, workSheetName, officeProperties);
                xlPackage.Save();
            }

            fileInfo.Refresh();
            return fileInfo;
        }

        public FileInfo CreateNewExcelFile(
            FileInfo fileInfo
            , String workbookTitle = null
            , dynamic officeProperties = null
            , Boolean overWrite = false
            , String tempFolderPath = null
            )
        {
            fileInfo.Refresh();
            if (fileInfo.Exists && !overWrite) throw new Exception(FileOverwriteRestrictionExStr(fileInfo.Name));
            if (fileInfo.Exists)
            {
                FileInfo temp = null;
                try
                {
                    if (!String.IsNullOrWhiteSpace(tempFolderPath))
                    {
                        var fn = Path.Combine(tempFolderPath, Path.GetRandomFileName());
                        temp = new FileInfo(fn);
                    }
                    else
                    {
                        var fn = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName());
                        temp = new FileInfo(fn);
                    }

                    var xlPackage = new ExcelPackage(temp);
                    SetExcelPackageProperties(ref xlPackage, workbookTitle, officeProperties);
                    xlPackage.Save();

                    var moveto = fileInfo.FullName;
                    if (fileInfo.Exists) fileInfo.Delete();
                    temp.MoveTo(moveto);
                }
                catch (Exception)
                {
                    if (temp != null && temp.Exists) temp.Delete();
                    throw;
                }
            }
            else
            {
                var xlPackage = new ExcelPackage(fileInfo);
                SetExcelPackageProperties(ref xlPackage, workbookTitle, officeProperties);
                xlPackage.Save();
            }

            fileInfo.Refresh();
            return fileInfo;
        }

        public ExcelPackage CreateOrOpenNewExcelFile(
            FileInfo fileInfo
            , String workbookTitle = null
            , dynamic officeProperties = null
            )
        {
            var xlPackage = new ExcelPackage(fileInfo);
            if (String.IsNullOrWhiteSpace(workbookTitle) && !String.IsNullOrWhiteSpace(xlPackage.Workbook.Properties.Title)) workbookTitle = xlPackage.Workbook.Properties.Title;
            else if (String.IsNullOrWhiteSpace(workbookTitle) && String.IsNullOrWhiteSpace(xlPackage.Workbook.Properties.Title)) workbookTitle = fileInfo.Name.Replace(@".xlsx", String.Empty);

            if (officeProperties != null)
            {
                SetExcelPackageProperties(ref xlPackage, workbookTitle, officeProperties);
                xlPackage.Save();
            }
            else
            {
                xlPackage.Workbook.Properties.Title = workbookTitle;
                xlPackage.Save();
            }

            return xlPackage;
        }

        public ExcelPackage CreateOrOpenNewExcelFile(
            String filePath
            , String workbookTitle = null
            , dynamic officeProperties = null
            )
        {
            return CreateOrOpenNewExcelFile(new FileInfo(filePath), workbookTitle, officeProperties);
        }

        public ExcelPackage OpenExcelFile(FileInfo fileInfo)
        {
            if (!fileInfo.Exists) throw new FileNotFoundException(String.Format(@"File Not Found Exception: {0} does not exist!", fileInfo.FullName));
            return new ExcelPackage(fileInfo);
        }

        public ExcelPackage OpenExcelFile(String filePath)
        {
            return OpenExcelFile(new FileInfo(filePath));
        }

        public void AddWorksheetFromDynamicObjects(ref ExcelPackage xlPackage, IEnumerable<dynamic> objectCollection, String workSheetName = "Results")
        {
            // ReSharper disable PossibleMultipleEnumeration
            var rowcnt = objectCollection.Count() + 1;
            var worksheet = xlPackage.Workbook.Worksheets.Add(workSheetName);

            // build worksheet
            if (worksheet == null) return;
            if (objectCollection == null || !objectCollection.Any()) return;
            PropertyInfo[] oProps = null;// column names
            var columnsExist = false;
            var isExpando = false;
            var row = 0;
            var columncount = 0;
            foreach (var r in objectCollection)
            {
                if (isExpando || r.GetType().Equals(typeof(ExpandoObject)))
                {
                    if (!columnsExist)
                    {
                        row++;
                        isExpando = true;
                        var i = 0;
                        foreach (var c in (IDictionary<String, Object>)r)
                        {
                            i++;
                            worksheet.Cells[row, i].Value = c.Key;
                            worksheet.Cells[row, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                            worksheet.Cells[row, i].Style.Font.Bold = true;
                        }
                        columnsExist = true;
                        columncount = i;
                    }
                    row++;
                    var j = 0;
                    foreach (var c in (IDictionary<String, Object>)r)
                    {
                        j++;
                        worksheet.Cells[row, j].Value = c.Value;
                        if (c.Value is DateTime)
                        {
                            worksheet.Cells[row, j].Style.Numberformat.Format = @"yyyy-MM-dd HH:mm:ss";
                        }
                    }
                }
                else
                {
                    if (oProps == null) oProps = ((Type)r.GetType()).GetProperties();
                    if (!columnsExist)
                    {
                        row++;
                        var i = 0;
                        foreach (var pi in oProps)
                        {
                            i++;
                            worksheet.Cells[row, i].Value = pi.Name;
                            worksheet.Cells[row, i].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            worksheet.Cells[row, i].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                            worksheet.Cells[row, i].Style.Font.Bold = true;
                        }
                        columnsExist = true;
                        columncount = i;
                    }
                    row++;
                    var j = 0;
                    foreach (var pi in oProps)
                    {
                        j++;
                        var val = pi.GetValue(r, null);
                        worksheet.Cells[row, j].Value = val;
                        if (val is DateTime)
                        {
                            worksheet.Cells[row, j].Style.Numberformat.Format = @"yyyy-MM-dd HH:mm:ss";
                        }
                    }
                }
            }

            // set AutoFilter
            worksheet.Cells[1, 1, rowcnt, columncount].AutoFilter = true;
            worksheet.Cells[1, 1, rowcnt, columncount].Style.Font.Size = 9;
            var worksheetview = worksheet.View;
            worksheetview.FreezePanes(2, 1);
            for (var i = 0; i < columncount; i++)
            {
                worksheet.Column(i + 1).BestFit = true; // does not seem to work?
                worksheet.Column(i + 1).Width = 18; // sets width
            }
            // ReSharper restore PossibleMultipleEnumeration
        }

        public void AddWorksheetFromDataTable(ref ExcelPackage xlPackage, DataTable data, String workSheetName = "Results")
        {
            var rowcnt = data.Rows.Count + 1;
            var columncount = data.Columns.Count;
            var worksheet = xlPackage.Workbook.Worksheets.Add(workSheetName);

            // build worksheet
            if (worksheet == null) return;
            worksheet.Cells["A1"].LoadFromDataTable(data, true);

            // set AutoFilter
            worksheet.Cells[1, 1, rowcnt, columncount].AutoFilter = true;
            worksheet.Cells[1, 1, rowcnt, columncount].Style.Font.Size = 9;
            var worksheetview = worksheet.View;
            worksheetview.FreezePanes(2, 1);
            for (var i = 0; i < columncount; i++)
            {
                worksheet.Column(i + 1).BestFit = true; // does not seem to work?
                worksheet.Column(i + 1).Width = 18; // sets width
            }
        }

        public void AddWorkSheetToExcel(FileInfo fileInfo, IEnumerable<dynamic> objectCollection, String workSheetName = "Results")
        {
            var xlPackage = new ExcelPackage(fileInfo);
            AddWorksheetFromDynamicObjects(ref xlPackage, objectCollection, workSheetName);
            xlPackage.Save();
        }

        public void SetExcelPackageProperties(ref ExcelPackage xlPackage, String workbookTitle = "Results", dynamic officeProperties = null)
        {
            // set property values
            if (officeProperties != null)
            {
                if (officeProperties.Author != null) xlPackage.Workbook.Properties.Author = officeProperties.Author;
                if (officeProperties.Category != null) xlPackage.Workbook.Properties.Category = officeProperties.Category;
                if (officeProperties.Comments != null) xlPackage.Workbook.Properties.Comments = officeProperties.Comments;
                if (officeProperties.Company != null) xlPackage.Workbook.Properties.Company = officeProperties.Company;
                if (officeProperties.HyperlinkBase != null) xlPackage.Workbook.Properties.HyperlinkBase = officeProperties.HyperlinkBase;
                if (officeProperties.Keywords != null) xlPackage.Workbook.Properties.Keywords = officeProperties.Keywords;
                if (officeProperties.LastModifiedBy != null) xlPackage.Workbook.Properties.LastModifiedBy = officeProperties.LastModifiedBy;
                if (officeProperties.Manager != null) xlPackage.Workbook.Properties.Manager = officeProperties.Manager;
                if (officeProperties.Status != null) xlPackage.Workbook.Properties.Status = officeProperties.Status;
                if (officeProperties.Subject != null) xlPackage.Workbook.Properties.Subject = officeProperties.Subject;

                xlPackage.Workbook.Properties.Title = officeProperties.Title ?? workbookTitle;

                if (officeProperties.CustomPropertyValues != null)
                {
                    foreach (var p in officeProperties.CustomPropertyValues as List<Tuple<String, Object>>)
                    {
                        xlPackage.Workbook.Properties.SetCustomPropertyValue(p.Item1, p.Item2);
                    }
                }
            }
            else
            {
                xlPackage.Workbook.Properties.Title = workbookTitle;
            }
        }

        public dynamic CreateOfficeProperties(string author = null,
            string category = null,
            string comments = null,
            string company = null,
            Uri hyperlinkBase = null,
            string keywords = null,
            string lastModifiedBy = null,
            string manager = null,
            string status = null,
            string subject = null,
            string title = null,
            IEnumerable<Tuple<String, Object>> customPropertyValues = null
            )
        {
            dynamic rv = new ExpandoObject();

            rv.Author = author;
            rv.Category = category;
            rv.Comments = comments;
            rv.Company = company;
            rv.HyperlinkBase = hyperlinkBase;
            rv.Keywords = keywords;
            rv.LastModifiedBy = lastModifiedBy;
            rv.Manager = manager;
            rv.Status = status;
            rv.Subject = subject;
            rv.Title = title;
            rv.CustomPropertyValues = customPropertyValues;

            return rv;
        }

        private String FileOverwriteRestrictionExStr(String argumentName)
        {
            return String.Format(@"Exception: {0} cannot be created! The file already exists and the supplied Overwrite parameter is false.", argumentName);
        }

        #region EPP sample

        /*

        private void Sample4()
        {
            var connectionString = @"Data Source=CLEHBDB01;Initial Catalog=CDR_Billing_Sonus_2010-08-05;Persist Security Info=True;Trusted_Connection=True;MultipleActiveResultSets=True;";

            var cmdstr = @"SELECT TOP 1000 [CustomerID]
      ,[ProductID]
      ,[LATA]
      ,[OCN]
      ,[ZoneDestinationCode]
      ,[CallType]
      ,[TotalCalls]
      ,[BillDuration]
      ,[ClientCost]
      ,[Surcharge]
      ,[VendorRate]
  FROM [CDR_Billing_Sonus_2010-12-01].[dbo].[customer_detail_vendor_summary]";

            ExcelPackage xlPackage = new ExcelPackage();

            //var ws = xlPackage.Workbook.Worksheets.Add("Sample4");

            // get handle to the existing worksheet
            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets.Add("Sales");
            var namedStyle = xlPackage.Workbook.Styles.CreateNamedStyle("HyperLink");   //This one is language dependent
            namedStyle.Style.Font.UnderLine = true;
            namedStyle.Style.Font.Color.SetColor(Color.Blue);
            if (worksheet != null)
            {
                const int startRow = 2;
                int row = startRow;

                //Create Headers and format them

                worksheet.Column(2).BestFit = true;

                worksheet.Cells["A1"].Value = "CustomerID";
                worksheet.Cells["B1"].Value = "ProductID";
                worksheet.Cells["C1"].Value = "LATA";
                worksheet.Cells["D1"].Value = "OCN";
                worksheet.Cells["E1"].Value = "ZoneDestinationCode";
                worksheet.Cells["F1"].Value = "CallType";
                worksheet.Cells["G1"].Value = "TotalCalls";
                worksheet.Cells["H1"].Value = "BillDuration";
                worksheet.Cells["I1"].Value = "ClientCost";
                worksheet.Cells["J1"].Value = "Surcharge";
                worksheet.Cells["K1"].Value = "VendorRate";

                worksheet.Cells["A1:K1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells["A1:K1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(184, 204, 228));
                worksheet.Cells["A1:K1"].Style.Font.Bold = true;

                // lets connect to the AdventureWorks sample database for some data
                using (SqlConnection sqlConn = new SqlConnection(connectionString))
                {
                    sqlConn.Open();
                    using (SqlCommand sqlCmd = new SqlCommand(cmdstr, sqlConn))
                    {
                        using (SqlDataReader sqlReader = sqlCmd.ExecuteReader())
                        {
                            // get the data and fill rows 5 onwards
                            while (sqlReader.Read())
                            {
                                int col = 1;

                                // our query has the columns in the right order, so simply iterate
                                // through the columns
                                for (int i = 0; i < sqlReader.FieldCount; i++)
                                {
                                    // use the email address as a hyperlink for column 1
                                    if (sqlReader.GetName(i) == "EmailAddress")
                                    {
                                        // insert the email address as a hyperlink for the name
                                        string hyperlink = "mailto:" + sqlReader.GetValue(i).ToString();
                                        worksheet.Cells[row, 1].Hyperlink = new Uri(hyperlink, UriKind.Absolute);
                                    }
                                    else
                                    {
                                        // do not bother filling cell with blank data (also useful
                                        // if we have a formula in a cell)
                                        if (sqlReader.GetValue(i) != null)
                                            worksheet.Cells[row, col].Value = sqlReader.GetValue(i);
                                        col++;
                                    }
                                }
                                row++;
                            }
                            sqlReader.Close();

                            //worksheet.Cells[startRow, 1, row - 1, 1].StyleName = "HyperLink";
                            //worksheet.Cells[startRow, 4, row - 1, 6].Style.Numberformat.Format = "[$$-409]#,##0";
                            //worksheet.Cells[startRow, 7, row - 1, 7].Style.Numberformat.Format = "0%";

                            //worksheet.Cells[startRow, 7, row - 1, 7].FormulaR1C1 = "=IF(RC[-2]=0,0,RC[-1]/RC[-2])";

                            //Set column width
                            worksheet.Column(1).Width = 12;

                            //worksheet.Column(2).Width = 28;
                            worksheet.Column(3).Width = 18;
                            worksheet.Column(4).Width = 12;
                            worksheet.Column(5).Width = 18;
                            worksheet.Column(6).Width = 10;
                            worksheet.Column(7).Width = 12;
                            worksheet.Column(8).Width = 18;
                            worksheet.Column(9).Width = 18;
                            worksheet.Column(10).Width = 18;
                            worksheet.Column(11).Width = 18;

                            worksheet.Column(2).BestFit = true;
                        }
                    }
                    sqlConn.Close();
                }

                //worksheet.Cells["A1:K1001"].AutoFilter = true;
                worksheet.Cells[1, 1, 1001, 11].AutoFilter = true;

                //// lets set the header text
                //worksheet.HeaderFooter.oddHeader.CenteredText = "AdventureWorks Inc. Sales Report";
                //// add the page number to the footer plus the total number of pages
                //worksheet.HeaderFooter.oddFooter.RightAlignedText =
                //    string.Format("Page {0} of {1}", ExcelHeaderFooter.PageNumber, ExcelHeaderFooter.NumberOfPages);
                //// add the sheet name to the footer
                //worksheet.HeaderFooter.oddFooter.CenteredText = ExcelHeaderFooter.SheetName;
                //// add the file path to the footer
                //worksheet.HeaderFooter.oddFooter.LeftAlignedText = ExcelHeaderFooter.FilePath + ExcelHeaderFooter.FileName;
            }

            // we had better add some document properties to the spreadsheet

            // set some core property values
            xlPackage.Workbook.Properties.Title = "Sample 4";
            xlPackage.Workbook.Properties.Author = "Broadvox LLC";
            xlPackage.Workbook.Properties.Subject = "ExcelPackage Samples";
            xlPackage.Workbook.Properties.Keywords = "Office Open XML";
            xlPackage.Workbook.Properties.Category = "ExcelPackage Samples";
            xlPackage.Workbook.Properties.Comments = "This sample demonstrates how to create an Excel 2007 file from scratch using the Packaging API and Office Open XML";

            // set some extended property values
            xlPackage.Workbook.Properties.Company = "Broadvox LLC";
            xlPackage.Workbook.Properties.HyperlinkBase = new Uri("http://www.broadvox.com");

            // set some custom property values
            //xlPackage.Workbook.Properties.SetCustomPropertyValue("Checked by", "John Tunnicliffe");
            //xlPackage.Workbook.Properties.SetCustomPropertyValue("EmployeeID", "1147");
            //xlPackage.Workbook.Properties.SetCustomPropertyValue("AssemblyName", "ExcelPackage");

            Response.BinaryWrite(xlPackage.GetAsByteArray());
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.AddHeader("content-disposition", "attachment;  filename=Sample4.xlsx");
        }

        */

        #endregion EPP sample
    }
}