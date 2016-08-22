using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;

namespace Business.Excel
{
    public static class MsExcelClipboardUtility
    {
        /* From thread http://social.msdn.microsoft.com/Forums/en-US/csharpgeneral/thread/0db522eb-1d84-4804-b311-e0cd723349d4 */

        /*
        dataGridView1.Columns.Clear();
        DataTable DtTbl = GetTableFromRtfString(Clipboard.GetData(DataFormats.Rtf).ToString());
        dataGridView1.DataSource = DtTbl;
        */

        public static DataTable GetTableFromRtfString(string strRtfData)
        {
            var idxRowEnd = 0;
            var idxRowStart = 0;
            var dtTbl = new DataTable();
            var firstRow = true;

            do
            {
                idxRowEnd = strRtfData.IndexOf(@"\row", idxRowEnd, StringComparison.OrdinalIgnoreCase);
                if (idxRowEnd < 0) break;
                if (strRtfData[idxRowEnd - 1] == '\\') { idxRowEnd++; continue; }
                idxRowStart = strRtfData.LastIndexOf(@"\trowd", idxRowEnd, StringComparison.OrdinalIgnoreCase);
                if (idxRowStart < 0) break;
                if (strRtfData[idxRowStart - 1] == '\\') { idxRowEnd++; continue; }

                var rowStr = strRtfData.Substring(idxRowStart, idxRowEnd - idxRowStart);
                idxRowEnd++;

                var idxCell = 0;
                var idxCellMem = 0;
                var myDataRow = new List<string>();
                do
                {
                    idxCell = rowStr.IndexOf(@"\Cell ", idxCell, StringComparison.OrdinalIgnoreCase);
                    if (idxCell < 0) break;
                    if (rowStr[idxCell - 1] == '\\') { idxCell++; continue; }

                    myDataRow.Add(PurgeRtfCmds(rowStr.Substring(idxCellMem, idxCell - idxCellMem)));
                    idxCellMem = idxCell;
                    idxCell++;
                }
                while (idxCellMem > 0);

                if (firstRow)
                {
                    firstRow = false;
                    foreach (var colName in myDataRow)
                    {
                        if (dtTbl.Columns.Contains(colName))
                        {
                            var pos = 2;
                            while (dtTbl.Columns.Contains(colName + "_" + pos))
                            {
                                pos++;
                            }
                            dtTbl.Columns.Add(colName + "_" + pos);
                        }
                        else
                        {
                            dtTbl.Columns.Add(colName);
                        }
                    }
                }
                else
                    // ReSharper disable once CoVariantArrayConversion
                    dtTbl.Rows.Add(myDataRow.ToArray());
            }
            while ((idxRowStart > 0) && (idxRowEnd > 0));

            return dtTbl;
        }

        private static string PurgeRtfCmds(string strRtf)
        {
            var idxRtfStart = 0;
            while (idxRtfStart < strRtf.Length)
            {
                idxRtfStart = strRtf.IndexOf('\\', idxRtfStart);
                if (idxRtfStart < 0) break;
                if (strRtf[idxRtfStart + 1] == '\\')
                {
                    strRtf = strRtf.Remove(idxRtfStart, 1);   //1 offset to erase space
                    idxRtfStart++; //sckip "\"
                }
                else
                {
                    var idxRtfEnd = strRtf.IndexOf(' ', idxRtfStart);
                    if (idxRtfEnd < 0)
                        if (strRtf.Length > 0)
                            idxRtfEnd = strRtf.Length - 1;
                        else
                            break;
                    strRtf = strRtf.Remove(idxRtfStart, idxRtfEnd - idxRtfStart + 1);   //1 offset to erase space
                }
            }

            //Erase spaces at the end of the cell info.
            if (strRtf.Length > 0)
                while (strRtf[strRtf.Length - 1] == ' ')
                    strRtf = strRtf.Remove(strRtf.Length - 1);

            //Erase spaces at the beginning of the cell info.
            if (strRtf.Length > 0)
                while (strRtf[0] == ' ')
                    strRtf = strRtf.Substring(1, strRtf.Length - 1);

            return strRtf;
        }

        public static string FixHex(string str)
        {
            return Regex.Replace(str, @"\p{C}+", "");
        }
    }
}