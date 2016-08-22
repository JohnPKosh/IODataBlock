using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace Business.Excel
{
    public static class MsExcelReaderUtil
    {
        public static IEnumerable<Object> GetWorkSheetRowAsCollection(ref ExcelWorksheet worksheet, Int32 rowNumber = 1, IEnumerable<Int32> columnCollection = null)
        {
            var firstRow = worksheet.StartRow();
            if (rowNumber < firstRow) rowNumber = firstRow;

            if (columnCollection == null)
            {
                return worksheet.Cells[string.Format(@"{0}:{0}", rowNumber)].Select(x => x.Value);
            }
            var rv = new List<Object>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var i in columnCollection)
            {
                rv.Add(worksheet.Cells[rowNumber, i].Value);
            }
            return rv;
        }

        public static IEnumerable<IEnumerable<Object>> GetWorkSheetRowsAsCollection(ExcelWorksheet worksheet, Int32 startRow = 1, Int32 endRow = -1, IEnumerable<Int32> columnCollection = null)
        {
            var lastRow = worksheet.EndRow();
            var firstRow = worksheet.StartRow();
            if (endRow < 0 || endRow > lastRow)
            {
                endRow = worksheet.EndRow();
            }
            if (startRow < firstRow) startRow = firstRow;
            while (startRow <= endRow)
            {
                IEnumerable<Object> row;
                if (columnCollection == null)
                {
                    row = worksheet.Cells[string.Format(@"{0}:{0}", startRow)].Select(x => x.Value);
                }
                else
                {
                    // ReSharper disable once PossibleMultipleEnumeration
                    var rv = columnCollection.Select(i => worksheet.Cells[startRow, i].Value).ToList();
                    row = rv;
                }
                yield return row;
                startRow++;
            }
        }

        public static IEnumerable<Object> GetWorkSheetRowAsCollectionWithDefaults(ref ExcelWorksheet worksheet, Int32 rowNumber = 1, IEnumerable<Tuple<Int32, Object>> columnCollection = null)
        {
            var firstRow = worksheet.StartRow();
            if (rowNumber < firstRow) rowNumber = firstRow;

            if (columnCollection == null)
            {
                return worksheet.Cells[string.Format(@"{0}:{0}", rowNumber)].Select(x => x.Value);
            }
            var rv = new List<Object>();
            // ReSharper disable once LoopCanBeConvertedToQuery
            foreach (var i in columnCollection)
            {
                rv.Add(worksheet.Cells[rowNumber, i.Item1].Value ?? i.Item2);
            }
            return rv;
        }

        public static IEnumerable<IEnumerable<Object>> GetWorkSheetRowsAsCollectionWithDefaults(ExcelWorksheet worksheet, Int32 startRow = 1, Int32 endRow = -1, IEnumerable<Tuple<Int32, Object>> columnCollection = null)
        {
            var lastRow = worksheet.EndRow();
            var firstRow = worksheet.StartRow();
            if (endRow < 0 || endRow > lastRow)
            {
                endRow = worksheet.EndRow();
            }
            if (startRow < firstRow) startRow = firstRow;
            while (startRow <= endRow)
            {
                IEnumerable<Object> row;
                var collection = columnCollection as IList<Tuple<int, object>>;
                if (collection == null || !collection.Any())
                {
                    row = worksheet.Cells[string.Format(@"{0}:{0}", startRow.ToString(CultureInfo.InvariantCulture))].Select(x => x.Value);
                }
                else
                {
                    var rv = collection.Select(i => worksheet.Cells[startRow, i.Item1].Value ?? i.Item2).ToList();
                    row = rv;
                }
                yield return row;
                startRow++;
            }
        }

        public static Int32 FindFirstRowContainingAnyStrings(ExcelWorksheet worksheet, IEnumerable<string> matchCollection, Int32 startRow = 1, Int32 endRow = -1)
        {
            var rv = -1;
            var cnt = startRow;
            foreach (var found in GetWorkSheetRowsAsCollectionWithDefaults(worksheet, startRow, endRow)
                .Select(o => o.Any(field => matchCollection.Select(x => x.ToLower()).Contains(field.ToString().ToLower()))))
            {
                if (found)
                {
                    rv = cnt;
                    break;
                }
                cnt++;
            }
            return rv;
        }

        public static Int32 FindFirstRowContainingAllStrings(ExcelWorksheet worksheet, IEnumerable<string> matchCollection, Int32 startRow = 1, Int32 endRow = -1)
        {
            var rv = -1;
            var cnt = startRow;
            foreach (var o in GetWorkSheetRowsAsCollectionWithDefaults(worksheet, startRow, endRow))
            {
                var found = false;
                var row = string.Join(", ", o.Select(x => x.ToString()));
                // ReSharper disable once PossibleMultipleEnumeration
                foreach (var field in matchCollection)
                {
                    if (row.ToLower().Contains(field.ToLower()))
                    {
                        found = true;
                    }
                    else
                    {
                        found = false;
                        break;
                    }
                }

                if (found)
                {
                    rv = cnt;
                    break;
                }
                cnt++;
            }
            return rv;
        }

        public static Dictionary<Int32, string> GetColumnPositions(ExcelWorksheet worksheet, IEnumerable<string> matchCollection, Int32 rowNumber = 1)
        {
            var rv = new Dictionary<Int32, string>();
            var fid = 1;
            foreach (var o in GetWorkSheetRowAsCollectionWithDefaults(ref worksheet, rowNumber))
            {
                if (o != null)
                {
                    var obj = o.ToString();
                    // ReSharper disable once PossibleMultipleEnumeration
                    if (matchCollection.Any(x => x.ToLower() == obj.ToLower())) rv.Add(fid, obj);
                }
                fid++;
            }
            return rv;
        }

        public static Int32 StartRow(this ExcelWorksheet worksheet)
        {
            return worksheet.Dimension.Start.Row;
        }

        public static Int32 EndRow(this ExcelWorksheet worksheet)
        {
            return worksheet.Dimension.End.Row;
        }

        public static IEnumerable<ExcelWorksheet> GetWorkSheetsFromFile(FileInfo fileInfo)
        {
            var edo = new ExcelDynamicObjects();
            var xlPackage = edo.OpenExcelFile(fileInfo);
            return xlPackage.Workbook.Worksheets.AsEnumerable();
        }

        public static IEnumerable<ExcelWorksheet> GetWorkSheetsFromFile(string filePath)
        {
            return GetWorkSheetsFromFile(new FileInfo(filePath));
        }

        public static IEnumerable<string> GetWorkSheetNamesFromFile(FileInfo fileInfo)
        {
            var edo = new ExcelDynamicObjects();
            var xlPackage = edo.OpenExcelFile(fileInfo);
            return xlPackage.Workbook.Worksheets.Select(x => x.Name);
        }

        public static IEnumerable<string> GetWorkSheetNamesFromFile(string filePath)
        {
            return GetWorkSheetNamesFromFile(new FileInfo(filePath));
        }
    }
}