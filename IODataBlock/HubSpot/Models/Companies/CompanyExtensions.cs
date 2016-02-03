using System.Collections.Generic;
using System.IO;
using System.Linq;
using Business.Common.Extensions;
using Business.Excel;

namespace HubSpot.Models.Companies
{
    public static class CompanyExtensions
    {

        public static IEnumerable<dynamic> ConvertToIEnumerableDynamic(this IEnumerable<CompanyViewModel> values)
        {
            return values.Select(c => ((Dictionary<string, object>)c).ToExpando());
        }

        public static FileInfo WriteToExcelFile(this IEnumerable<CompanyViewModel> values, FileInfo fileInfo)
        {
            var data = values.ConvertToIEnumerableDynamic().ToList();
            if (data != null && data.Count > 0)
            {
                var eo = new ExcelDynamicObjects();
                return eo.CreateExcelFileFromDynamicObjects(fileInfo, data, overWrite:true);
            }
               return null; 
        }
    }
}
