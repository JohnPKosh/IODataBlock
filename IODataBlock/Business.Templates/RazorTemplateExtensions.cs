using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace Business.Templates
{
    public static class RazorTemplateExtensions
    {
        #region Parse From Files

        public static String ParseWithRazorFromFile<T>(this T obj, FileInfo templateFile, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(templateFile.FullName);
            return obj.ParseWithRazor(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazorFromFile<T>(this T obj, String templatePath, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(templatePath);
            return obj.ParseWithRazor(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazorFromDirectory<T>(this T obj, String templateFileName, String templateFileExtension = ".cshtml", String templateDirectoryName = "", String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(Path.Combine(templateDirectoryName, templateFileName) + templateFileExtension);
            return obj.ParseWithRazor(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazorFromFileRaw<T>(this T obj, FileInfo templateFile, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(templateFile.FullName);
            return obj.ParseWithRazorRaw(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazorFromFileRaw<T>(this T obj, String templatePath, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(templatePath);
            return obj.ParseWithRazorRaw(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazorFromDirectoryRaw<T>(this T obj, String templateFileName, String templateFileExtension = ".cshtml", String templateDirectoryName = "", String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(Path.Combine(templateDirectoryName, templateFileName) + templateFileExtension);
            return obj.ParseWithRazorRaw(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        #endregion Parse From Files

        #region ParseWithRazor Methods

        public static String ParseWithRazor<T>(this T obj, String templateString, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var config = templateType == null ? new TemplateServiceConfiguration() : new TemplateServiceConfiguration { BaseTemplateType = templateType };
            config.EncodedStringFactory = new HtmlEncodedStringFactory();
            var service = new TemplateService(config);
            if (sectionTemplates != null)
            {
                foreach (var s in sectionTemplates)
                {
                    service.GetTemplate(s.RazorTemplate, s.Model, s.CacheName);
                }
            }
            Razor.SetTemplateService(service);
            templatePrefix = String.IsNullOrWhiteSpace(templatePrefix) ? String.Empty : templatePrefix;
            templateSuffix = String.IsNullOrWhiteSpace(templateSuffix) ? String.Empty : templateSuffix;
            return templatePrefix + service.Parse<T>(templateString, obj, viewBag, String.Format("{0}_ItemTemplate", typeof(T).Name)) + templateSuffix;
        }

        public static String ParseWithRazor<T>(this T obj, Func<T, String> templateModelFunction, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateModelFunction(obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazor<T>(this T obj, String keySelector, ref IDictionary<String, String> templateDictionary, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateDictionary[keySelector], templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazor<T>(this T obj, Int32 keySelector, ref List<String> templateList, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateList[keySelector], templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazor<T>(this T obj, String keySelector, ref IDictionary<String, Func<T, String>> templateDictionary, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateDictionary[keySelector](obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazor<T>(this T obj, Int32 keySelector, ref List<Func<T, String>> templateList, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateList[keySelector](obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        #endregion ParseWithRazor Methods

        #region ParseWithRazorRaw Methods

        public static String ParseWithRazorRaw<T>(this T obj, String templateString, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            TemplateServiceConfiguration config = templateType == null ? new TemplateServiceConfiguration() : new TemplateServiceConfiguration { BaseTemplateType = templateType };

            config.EncodedStringFactory = new RawStringFactory();
            var service = new TemplateService(config);

            if (sectionTemplates != null)
            {
                foreach (var s in sectionTemplates)
                {
                    service.GetTemplate(s.RazorTemplate, s.Model, s.CacheName);
                }
            }

            Razor.SetTemplateService(service);
            templatePrefix = String.IsNullOrWhiteSpace(templatePrefix) ? String.Empty : templatePrefix;
            templateSuffix = String.IsNullOrWhiteSpace(templateSuffix) ? String.Empty : templateSuffix;
            return templatePrefix + service.Parse<T>(templateString, obj, viewBag, String.Format("{0}_ItemTemplate", typeof(T).Name)) + templateSuffix;
        }

        public static String ParseWithRazorRaw<T>(this T obj, Func<T, String> templateModelFunction, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateModelFunction(obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazorRaw<T>(this T obj, String keySelector, ref IDictionary<String, String> templateDictionary, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateDictionary[keySelector], templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazorRaw<T>(this T obj, Int32 keySelector, ref List<String> templateList, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateList[keySelector], templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazorRaw<T>(this T obj, String keySelector, ref IDictionary<String, Func<T, String>> templateDictionary, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateDictionary[keySelector](obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static String ParseWithRazorRaw<T>(this T obj, Int32 keySelector, ref List<Func<T, String>> templateList, String templatePrefix = null, String templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateList[keySelector](obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        #endregion ParseWithRazorRaw Methods

        #region ParseIEnumerableWithRazor Methods

        public static IEnumerable<String> ParseIEnumerableWithRazor<T>(
            this IEnumerable<T> obj,
            Func<T, String> templateModelFunction,
            String itemTemplatePrefix = null,
            String itemTemplateSuffix = null
            )
        {
            if (String.IsNullOrWhiteSpace(itemTemplatePrefix) && String.IsNullOrWhiteSpace(itemTemplateSuffix))
            {
                foreach (var o in obj)
                {
                    yield return o.ParseWithRazor(templateModelFunction);
                }
            }
            else
            {
                itemTemplatePrefix = String.IsNullOrWhiteSpace(itemTemplatePrefix) ? String.Empty : itemTemplatePrefix;
                itemTemplateSuffix = String.IsNullOrWhiteSpace(itemTemplateSuffix) ? String.Empty : itemTemplateSuffix;
                foreach (var o in obj)
                {
                    yield return itemTemplatePrefix + o.ParseWithRazor(templateModelFunction) + itemTemplateSuffix;
                }
            }
        }

        public static IEnumerable<String> ParseIEnumerableWithRazorRaw<T>(
            this IEnumerable<T> obj,
            Func<T, String> templateModelFunction,
            String itemTemplatePrefix = null,
            String itemTemplateSuffix = null,
            DynamicViewBag viewBag = null
            )
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);
            if (String.IsNullOrWhiteSpace(itemTemplatePrefix) && String.IsNullOrWhiteSpace(itemTemplateSuffix))
            {
                foreach (var o in obj)
                {
                    yield return ct.Parse<T>(templateModelFunction(o), o, viewBag, String.Format("{0}_ItemTemplate", o.GetType().Name));
                }
            }
            else
            {
                itemTemplatePrefix = String.IsNullOrWhiteSpace(itemTemplatePrefix) ? String.Empty : itemTemplatePrefix;
                itemTemplateSuffix = String.IsNullOrWhiteSpace(itemTemplateSuffix) ? String.Empty : itemTemplateSuffix;
                foreach (var o in obj)
                {
                    yield return itemTemplatePrefix + ct.Parse<T>(templateModelFunction(o), o, viewBag, String.Format("{0}_ItemTemplate", o.GetType().Name)) + itemTemplateSuffix;
                }
            }
        }

        #endregion ParseIEnumerableWithRazor Methods

        #region ParseIEnumerableWithRazorToLines Methods

        public static String ParseIEnumerableWithRazorToLines<T>(
            this IEnumerable<T> obj, Func<T, String> templateModelFunction,
            String newLineChar = "\r\n",
            String templatePrefix = null,
            String templateSuffix = null,
            String itemTemplatePrefix = null,
            String itemTemplateSuffix = null
            )
        {
            using (var sw = new StringWriter())
            {
                if (!String.IsNullOrWhiteSpace(templatePrefix)) sw.Write(templatePrefix);
                if (String.IsNullOrWhiteSpace(itemTemplatePrefix) && String.IsNullOrWhiteSpace(itemTemplateSuffix))
                {
                    foreach (var o in obj.ParseIEnumerableWithRazor(templateModelFunction))
                    {
                        sw.Write(o);
                        sw.Write(newLineChar);
                    }
                }
                else
                {
                    itemTemplatePrefix = String.IsNullOrWhiteSpace(itemTemplatePrefix) ? String.Empty : itemTemplatePrefix;
                    itemTemplateSuffix = String.IsNullOrWhiteSpace(itemTemplateSuffix) ? String.Empty : itemTemplateSuffix;
                    foreach (var o in obj.ParseIEnumerableWithRazor(templateModelFunction))
                    {
                        sw.Write(itemTemplatePrefix + o + itemTemplateSuffix);
                        sw.Write(newLineChar);
                    }
                }
                if (!String.IsNullOrWhiteSpace(templateSuffix)) sw.Write(templateSuffix);
                sw.Flush();
                return sw.ToString();
            }
        }

        public static String ParseIEnumerableWithRazorRawToLines<T>(
            this IEnumerable<T> obj,
            Func<T, String> templateModelFunction,
            String newLineChar = "\r\n",
            String templatePrefix = null,
            String templateSuffix = null,
            String itemTemplatePrefix = null,
            String itemTemplateSuffix = null
            )
        {
            using (var sw = new StringWriter())
            {
                if (!String.IsNullOrWhiteSpace(templatePrefix)) sw.Write(templatePrefix);
                if (String.IsNullOrWhiteSpace(itemTemplatePrefix) && String.IsNullOrWhiteSpace(itemTemplateSuffix))
                {
                    foreach (var o in obj.ParseIEnumerableWithRazorRaw(templateModelFunction))
                    {
                        sw.Write(o);
                        sw.Write(newLineChar);
                    }
                }
                else
                {
                    itemTemplatePrefix = String.IsNullOrWhiteSpace(itemTemplatePrefix) ? String.Empty : itemTemplatePrefix;
                    itemTemplateSuffix = String.IsNullOrWhiteSpace(itemTemplateSuffix) ? String.Empty : itemTemplateSuffix;
                    foreach (var o in obj.ParseIEnumerableWithRazorRaw(templateModelFunction))
                    {
                        sw.Write(itemTemplatePrefix + o + itemTemplateSuffix);
                        sw.Write(newLineChar);
                    }
                }
                if (!String.IsNullOrWhiteSpace(templateSuffix)) sw.Write(templateSuffix);
                sw.Flush();
                return sw.ToString();
            }
        }

        #endregion ParseIEnumerableWithRazorToLines Methods

        #region Utiltiy Methods

        public static List<dynamic> ToExpandoList(this DataTable rdr)
        {
            var result = new List<dynamic>();
            foreach (DataRow r in rdr.Rows)
            {
                dynamic e = new ExpandoObject();
                var d = e as IDictionary<string, object>;
                for (int i = 0; i < rdr.Columns.Count; i++)
                    d.Add(rdr.Columns[i].ColumnName, r[i]);
                result.Add(e);
            }
            return result;
        }

        public static List<object[]> ToObjectList(this DataTable rdr)
        {
            return (from DataRow r in rdr.Rows select r.ItemArray).ToList();
        }

        public static List<dynamic> ToExpandoList(this IDataReader rdr)
        {
            var result = new List<dynamic>();
            while (rdr.Read())
            {
                dynamic e = new ExpandoObject();
                var d = e as IDictionary<string, object>;
                for (int i = 0; i < rdr.FieldCount; i++)
                    d.Add(rdr.GetName(i), rdr[i]);
                result.Add(e);
            }
            return result;
        }

        public static IEnumerable<dynamic> ToIEnumerableExpando(this IDataReader rdr)
        {
            var result = new List<dynamic>();
            while (rdr.Read())
            {
                dynamic e = new ExpandoObject();
                var d = e as IDictionary<string, object>;
                for (int i = 0; i < rdr.FieldCount; i++)
                    d.Add(rdr.GetName(i), rdr[i]);
                result.Add(e);
                yield return e;
            }
        }

        #endregion Utiltiy Methods
    }
}