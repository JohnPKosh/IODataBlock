using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Business.Common.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
//using CsvHelper;
using LINQtoCSV;

namespace BasicTests.Extensions
{
    public class row
    {
        [CsvColumn(Name = "COMPANY_NAME", FieldIndex = 1)]
        public string COMPANY_NAME { get; set; }

        [CsvColumn(Name = "EMAIL", FieldIndex = 2)]
        public string EMAIL { get; set; }

        [CsvColumn(Name = "ADDRESS", FieldIndex = 3)]
        public string ADDRESS { get; set; }

        [CsvColumn(Name = "CITY", FieldIndex = 4)]
        public string CITY { get; set; }

        [CsvColumn(Name = "STATE", FieldIndex = 5)]
        public string STATE { get; set; }

        [CsvColumn(Name = "ZIPCODE", FieldIndex = 6)]
        public string ZIPCODE { get; set; }

        [CsvColumn(Name = "PHONE_NUMBER", FieldIndex = 7)]
        public string PHONE_NUMBER { get; set; }

        [CsvColumn(Name = "FAX_NUMBER", FieldIndex = 8)]
        public string FAX_NUMBER { get; set; }

        [CsvColumn(Name = "SIC_CODE", FieldIndex = 9)]
        public string SIC_CODE { get; set; }

        [CsvColumn(Name = "SIC_DESCRIPTION", FieldIndex = 10)]
        public string SIC_DESCRIPTION { get; set; }

        [CsvColumn(Name = "WEB_ADDRESS", FieldIndex = 11)]
        public string WEB_ADDRESS { get; set; }
    }


    [TestClass]
    public class StringExtensionTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            MainTest();
        }


        private static List<IEnumerable<string>> errorRows = new List<IEnumerable<string>>();

        static void MainTest()
        {

            // C:\junk\US Database\DE Email.csv

            if (File.Exists(@"C:\junk\US Database\GA Email_ouptut.csv")) File.Delete(@"C:\junk\US Database\GA Email_ouptut.csv");

            using (var fs = File.OpenWrite(@"C:\junk\US Database\GA Email_ouptut.csv"))
            {
                using (var sr = new StreamWriter(fs))
                {
                    sr.AutoFlush = true;
                    //sr.WriteLine("CompanyName,Email1,Email2,Email3,Email4,Email5,Address,City,State,PostalCode,Phone1,Phone2,Phone3,Phone4,Phone5,Fax,SicCode,SicDescription,Url1,Url2,Url3,Url4,Url5");
                    sr.WriteLine("\"CompanyName\",\"Email1\",\"Email2\",\"Email3\",\"Email4\",\"Email5\",\"Address\",\"City\",\"State\",\"PostalCode\",\"Phone1\",\"Phone2\",\"Phone3\",\"Phone4\",\"Phone5\",\"Fax\",\"SicCode\",\"SicDescription\",\"Url1\",\"Url2\",\"Url3\",\"Url4\",\"Url5\",\"Dn1\",\"Dn2\",\"Dn3\",\"Dn4\",\"Dn5\"");
                    var i = 0;
                    foreach (var l in GetProcessedLines(@"C:\junk\US Database\GA Email.csv").Where(x=>x[29] != "E"))
                    {
                        try
                        {
                            i++;
                            sr.WriteLine($"\"{l[0]}\",\"{l[1]}\",\"{l[2]}\",\"{l[3]}\",\"{l[4]}\",\"{l[5]}\",\"{l[6]}\",\"{l[7]}\",\"{l[8]}\",\"{l[9]}\",\"{l[10]}\",\"{l[11]}\",\"{l[12]}\",\"{l[13]}\",\"{l[14]}\",\"{l[15]}\",\"{l[16]}\",\"{l[17]}\",\"{l[18]}\",\"{l[19]}\",\"{l[20]}\",\"{l[21]}\",\"{l[22]}\",\"{l[23]}\",\"{l[24]}\",\"{l[25]}\",\"{l[26]}\",\"{l[27]}\",\"{l[28]}\"");
                        }
                        catch (Exception ex)
                        {
                            throw;
                        }
                    }
                    sr.Flush();
                }
            }
        }


        public static List<string[]> GetRawLines(string filePath)
        {
            CsvContext cc = new CsvContext();
            CsvFileDescription inputFileDescription = new CsvFileDescription
            {
                SeparatorChar = ',',
                FirstLineHasColumnNames = true
            };
            var products = cc.Read<row>(filePath, inputFileDescription).Where(y => !string.IsNullOrWhiteSpace(y.WEB_ADDRESS) && y.WEB_ADDRESS.Trim().ToUpper(CultureInfo.InvariantCulture) != "NULL");
            return
                products.Select(
                    x =>
                        new string[]
                        {
                            FormatField(x.COMPANY_NAME),
                            x.EMAIL,
                            FormatField(x.ADDRESS),
                            FormatField(x.CITY),
                            FormatField(x.STATE),
                            FormatField(x.ZIPCODE),
                            x.PHONE_NUMBER,
                            x.FAX_NUMBER,
                            x.SIC_CODE,
                            FormatField(x.SIC_DESCRIPTION),
                            x.WEB_ADDRESS


                            //x.COMPANY_NAME,
                            //x.EMAIL,
                            //(string.IsNullOrWhiteSpace(x.ADDRESS) || x.ADDRESS.ToUpper(CultureInfo.InvariantCulture) == "NULL") ? null : x.ADDRESS.Replace(",","||"),
                            //x.CITY,
                            //x.STATE,
                            //x.ZIPCODE,
                            //x.PHONE_NUMBER,
                            //x.FAX_NUMBER,
                            //x.SIC_CODE,
                            //string.IsNullOrWhiteSpace(x.SIC_DESCRIPTION)? null : x.SIC_DESCRIPTION.Replace(",","||"),
                            //x.WEB_ADDRESS
                        }).ToList();


            //using (var fs = File.OpenRead(filePath))
            //{
            //    using (var sr = new StreamReader(fs))
            //    {
            //        return sr.Lines(",", 2, "\"").ToList();
            //    }
            //}

            //using (var _csvReader = new CsvReader(new StreamReader(filePath)))
            //{
            //    _csvReader.Configuration.IgnoreQuotes = true;
            //    var rv = new List<string[]>();
            //    while (_csvReader.Read())
            //    {
            //        rv.Add(_csvReader.CurrentRecord);
            //    }
            //    return rv;
            //}
        }

        public static IEnumerable<string[]> GetProcessedLines(string filePath)
        {
            //errorRows.AddRange(GetRawLines(filePath).Where(x => x.Length != 11));
            return GetRawLines(filePath).Select(ProcessLine);
        }

        public static string[] ProcessLine(string[] line)
        {
            var rv = new string[30];
            try
            {
                rv[0] = line[0];

                var emailsarr = GetSplitColumnValues(line[1]);
                for (var i = 0; i < emailsarr.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(emailsarr[i]) || emailsarr[i].ToUpper(CultureInfo.InvariantCulture) == "NULL") continue;
                    rv[i + 1] = emailsarr[i];
                }

                rv[6] = line[2];
                rv[7] = line[3];
                rv[8] = line[4];
                rv[9] = line[5];

                var phonesarr = GetSplitColumnValues(line[6]);
                for (var i = 0; i < phonesarr.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(phonesarr[i]) || phonesarr[i].ToUpper(CultureInfo.InvariantCulture) == "NULL") continue;
                    rv[i + 10] = phonesarr[i];
                }

                rv[15] = GetSplitColumnValues(line[7]).FirstOrDefault();

                rv[16] = line[8];
                rv[17] = line[9];

                var urlsarr = GetSplitWebsiteValues(line[10]);
                bool urlexists = false;
                for (var i = 0; i < urlsarr.Length; i++)
                {
                    if (string.IsNullOrWhiteSpace(urlsarr[i]) || urlsarr[i].ToUpper(CultureInfo.InvariantCulture) == "NULL") continue;
                    urlexists = true;
                    rv[i + 18] = urlsarr[i];
                    try
                    {
                        rv[i + 23] = Regex.Replace(urlsarr[i], "^w{3}2{0,1}[.]{1}", string.Empty, RegexOptions.IgnoreCase);
                    }
                    catch (Exception ex)
                    {

                        throw;
                    }
                }
                if (!urlexists)
                {
                    rv[29] = "E";
                }
                else
                {
                    rv[29] = string.Empty;
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return rv;
        }

        public static string FormatField(string value)
        {
            if (value == null) return null;
            if (string.IsNullOrWhiteSpace(value))
            {
                return value.Trim();
            }
            //return Regex.IsMatch(value.Trim(), "null", RegexOptions.IgnoreCase) ? null : Regex.Replace(value.Trim(), "&Amp;", "@", RegexOptions.IgnoreCase).Replace(";", "||").Replace("''", "'").Replace(",", "||");
            return Regex.IsMatch(value.Trim(), "null", RegexOptions.IgnoreCase) ? null : Regex.Replace(value.Trim(), "&Amp;", "@", RegexOptions.IgnoreCase).Replace("''", "'");
        }

        public static string[] GetSplitColumnValues(string columnText)
        {
            var rv = new List<string>();
            if (string.IsNullOrWhiteSpace(columnText)) return rv.ToArray();
            var valuesarr = columnText.Replace(". ", "^")
                .Replace("^^", "^")
                .Replace(";", "^")
                .Replace(".com ", ".com^")
                .Replace(",", "^")
                .Split(new[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
            var valueslen = valuesarr.Length > 5 ? 5 : valuesarr.Length;
            for (var i = 0; i < valueslen; i++)
            {
                if (valuesarr[i].Trim().ToUpper(CultureInfo.InvariantCulture) == "NULL") continue;
                rv.Add(valuesarr[i].Trim().ToLowerInvariant());
            }
            return rv.ToArray();
        }

        public static string[] GetSplitWebsiteValues(string columnText)
        {
            var rv = new List<string>();
            if (string.IsNullOrWhiteSpace(columnText)) return rv.ToArray();
            var valuesarr = columnText.Replace(". ", "^")
                .Replace("^^", "^")
                .Replace(";", "^")
                .Replace(".com ", ".com^")
                .Replace(",", "^")
                .Split(new[] { '^' }, StringSplitOptions.RemoveEmptyEntries);
            var valueslen = valuesarr.Length > 5 ? 5 : valuesarr.Length;
            for (var i = 0; i < valueslen; i++)
            {
                if (valuesarr[i].ToUpper(CultureInfo.InvariantCulture) == "NULL") continue;
                var url = valuesarr[i].Trim();
                url = url.Replace(@"http://", string.Empty).Replace(@"https://", string.Empty);
                url = url.LeftOfIndexOf('/');
                if (Regex.IsMatch(url.ToLowerInvariant(), @"^([a-zA-Z0-9][-a-zA-Z0-9]*[a-zA-Z0-9]\.)+([a-zA-Z0-9]{2,8})$", RegexOptions.IgnoreCase))
                {
                    rv.Add(url.Trim().ToLowerInvariant());
                }
            }
            return rv.ToArray();
        }

    }




}
