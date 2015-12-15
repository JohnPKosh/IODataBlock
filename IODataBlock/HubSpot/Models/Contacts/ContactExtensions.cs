using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Extensions;
using Business.Excel;

namespace HubSpot.Models.Contacts
{
    public static class ContactExtensions
    {

        public static IEnumerable<dynamic> ConvertToIEnumerableDynamic(this IEnumerable<ContactViewModel> values)
        {
            return values.Select(c => ((Dictionary<string, object>)c).ToExpando());
        }

        public static FileInfo WriteToExcelFile(this IEnumerable<ContactViewModel> values, FileInfo fileInfo)
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
