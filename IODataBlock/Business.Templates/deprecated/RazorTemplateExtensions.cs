using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;

namespace Business.Templates.deprecated
{
    public static class RazorTemplateExtensions
    {
        #region Parse From Files

        public static string ParseWithRazorFromFile<T>(this T obj, FileInfo templateFile, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(templateFile.FullName);
            return obj.ParseWithRazor(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazorFromFile<T>(this T obj, string templatePath, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(templatePath);
            return obj.ParseWithRazor(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazorFromDirectory<T>(this T obj, string templateFileName, string templateFileExtension = ".cshtml", string templateDirectoryName = "", string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(Path.Combine(templateDirectoryName, templateFileName) + templateFileExtension);
            return obj.ParseWithRazor(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazorFromFileRaw<T>(this T obj, FileInfo templateFile, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(templateFile.FullName);
            return obj.ParseWithRazorRaw(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazorFromFileRaw<T>(this T obj, string templatePath, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(templatePath);
            return obj.ParseWithRazorRaw(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazorFromDirectoryRaw<T>(this T obj, string templateFileName, string templateFileExtension = ".cshtml", string templateDirectoryName = "", string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var templateString = File.ReadAllText(Path.Combine(templateDirectoryName, templateFileName) + templateFileExtension);
            return obj.ParseWithRazorRaw(templateString, templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        #endregion Parse From Files

        #region ParseWithRazor Methods

        public static string ParseWithRazor<T>(this T obj, string templateString, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
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
            templatePrefix = string.IsNullOrWhiteSpace(templatePrefix) ? string.Empty : templatePrefix;
            templateSuffix = string.IsNullOrWhiteSpace(templateSuffix) ? string.Empty : templateSuffix;
            return templatePrefix + service.Parse<T>(templateString, obj, viewBag, $"{typeof(T).Name}_ItemTemplate") + templateSuffix;
        }

        public static string ParseWithRazor<T>(this T obj, Func<T, string> templateModelFunction, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateModelFunction(obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazor<T>(this T obj, string keySelector, ref IDictionary<string, string> templateDictionary, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateDictionary[keySelector], templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazor<T>(this T obj, Int32 keySelector, ref List<string> templateList, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateList[keySelector], templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazor<T>(this T obj, string keySelector, ref IDictionary<string, Func<T, string>> templateDictionary, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateDictionary[keySelector](obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazor<T>(this T obj, Int32 keySelector, ref List<Func<T, string>> templateList, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazor(templateList[keySelector](obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        #endregion ParseWithRazor Methods

        #region ParseWithRazorRaw Methods

        public static string ParseWithRazorRaw<T>(this T obj, string templateString, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            var config = templateType == null ? new TemplateServiceConfiguration() : new TemplateServiceConfiguration { BaseTemplateType = templateType };

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
            templatePrefix = string.IsNullOrWhiteSpace(templatePrefix) ? string.Empty : templatePrefix;
            templateSuffix = string.IsNullOrWhiteSpace(templateSuffix) ? string.Empty : templateSuffix;
            return templatePrefix + service.Parse<T>(templateString, obj, viewBag, $"{typeof(T).Name}_ItemTemplate") + templateSuffix;
        }

        public static string ParseWithRazorRaw<T>(this T obj, Func<T, string> templateModelFunction, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateModelFunction(obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazorRaw<T>(this T obj, string keySelector, ref IDictionary<string, string> templateDictionary, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateDictionary[keySelector], templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazorRaw<T>(this T obj, Int32 keySelector, ref List<string> templateList, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateList[keySelector], templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazorRaw<T>(this T obj, string keySelector, ref IDictionary<string, Func<T, string>> templateDictionary, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateDictionary[keySelector](obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        public static string ParseWithRazorRaw<T>(this T obj, Int32 keySelector, ref List<Func<T, string>> templateList, string templatePrefix = null, string templateSuffix = null, DynamicViewBag viewBag = null, Type templateType = null, RazorTemplateSections sectionTemplates = null)
        {
            return obj.ParseWithRazorRaw(templateList[keySelector](obj), templatePrefix, templateSuffix, viewBag, templateType, sectionTemplates);
        }

        #endregion ParseWithRazorRaw Methods

        #region ParseIEnumerableWithRazor Methods

        public static IEnumerable<string> ParseIEnumerableWithRazor<T>(
            this IEnumerable<T> obj,
            Func<T, string> templateModelFunction,
            string itemTemplatePrefix = null,
            string itemTemplateSuffix = null
            )
        {
            if (string.IsNullOrWhiteSpace(itemTemplatePrefix) && string.IsNullOrWhiteSpace(itemTemplateSuffix))
            {
                foreach (var o in obj)
                {
                    yield return o.ParseWithRazor(templateModelFunction);
                }
            }
            else
            {
                itemTemplatePrefix = string.IsNullOrWhiteSpace(itemTemplatePrefix) ? string.Empty : itemTemplatePrefix;
                itemTemplateSuffix = string.IsNullOrWhiteSpace(itemTemplateSuffix) ? string.Empty : itemTemplateSuffix;
                foreach (var o in obj)
                {
                    yield return itemTemplatePrefix + o.ParseWithRazor(templateModelFunction) + itemTemplateSuffix;
                }
            }
        }

        public static IEnumerable<string> ParseIEnumerableWithRazorRaw<T>(
            this IEnumerable<T> obj,
            Func<T, string> templateModelFunction,
            string itemTemplatePrefix = null,
            string itemTemplateSuffix = null,
            DynamicViewBag viewBag = null
            )
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);
            if (string.IsNullOrWhiteSpace(itemTemplatePrefix) && string.IsNullOrWhiteSpace(itemTemplateSuffix))
            {
                foreach (var o in obj)
                {
                    yield return ct.Parse<T>(templateModelFunction(o), o, viewBag, $"{o.GetType().Name}_ItemTemplate");
                }
            }
            else
            {
                itemTemplatePrefix = string.IsNullOrWhiteSpace(itemTemplatePrefix) ? string.Empty : itemTemplatePrefix;
                itemTemplateSuffix = string.IsNullOrWhiteSpace(itemTemplateSuffix) ? string.Empty : itemTemplateSuffix;
                foreach (var o in obj)
                {
                    yield return itemTemplatePrefix + ct.Parse<T>(templateModelFunction(o), o, viewBag,
                        $"{o.GetType().Name}_ItemTemplate") + itemTemplateSuffix;
                }
            }
        }

        #endregion ParseIEnumerableWithRazor Methods

        #region ParseIEnumerableWithRazorToLines Methods

        public static string ParseIEnumerableWithRazorToLines<T>(
            this IEnumerable<T> obj, Func<T, string> templateModelFunction,
            string newLineChar = "\r\n",
            string templatePrefix = null,
            string templateSuffix = null,
            string itemTemplatePrefix = null,
            string itemTemplateSuffix = null
            )
        {
            using (var sw = new StringWriter())
            {
                if (!string.IsNullOrWhiteSpace(templatePrefix)) sw.Write(templatePrefix);
                if (string.IsNullOrWhiteSpace(itemTemplatePrefix) && string.IsNullOrWhiteSpace(itemTemplateSuffix))
                {
                    foreach (var o in obj.ParseIEnumerableWithRazor(templateModelFunction))
                    {
                        sw.Write(o);
                        sw.Write(newLineChar);
                    }
                }
                else
                {
                    itemTemplatePrefix = string.IsNullOrWhiteSpace(itemTemplatePrefix) ? string.Empty : itemTemplatePrefix;
                    itemTemplateSuffix = string.IsNullOrWhiteSpace(itemTemplateSuffix) ? string.Empty : itemTemplateSuffix;
                    foreach (var o in obj.ParseIEnumerableWithRazor(templateModelFunction))
                    {
                        sw.Write(itemTemplatePrefix + o + itemTemplateSuffix);
                        sw.Write(newLineChar);
                    }
                }
                if (!string.IsNullOrWhiteSpace(templateSuffix)) sw.Write(templateSuffix);
                sw.Flush();
                return sw.ToString();
            }
        }

        public static string ParseIEnumerableWithRazorRawToLines<T>(
            this IEnumerable<T> obj,
            Func<T, string> templateModelFunction,
            string newLineChar = "\r\n",
            string templatePrefix = null,
            string templateSuffix = null,
            string itemTemplatePrefix = null,
            string itemTemplateSuffix = null
            )
        {
            using (var sw = new StringWriter())
            {
                if (!string.IsNullOrWhiteSpace(templatePrefix)) sw.Write(templatePrefix);
                if (string.IsNullOrWhiteSpace(itemTemplatePrefix) && string.IsNullOrWhiteSpace(itemTemplateSuffix))
                {
                    foreach (var o in obj.ParseIEnumerableWithRazorRaw(templateModelFunction))
                    {
                        sw.Write(o);
                        sw.Write(newLineChar);
                    }
                }
                else
                {
                    itemTemplatePrefix = string.IsNullOrWhiteSpace(itemTemplatePrefix) ? string.Empty : itemTemplatePrefix;
                    itemTemplateSuffix = string.IsNullOrWhiteSpace(itemTemplateSuffix) ? string.Empty : itemTemplateSuffix;
                    foreach (var o in obj.ParseIEnumerableWithRazorRaw(templateModelFunction))
                    {
                        sw.Write(itemTemplatePrefix + o + itemTemplateSuffix);
                        sw.Write(newLineChar);
                    }
                }
                if (!string.IsNullOrWhiteSpace(templateSuffix)) sw.Write(templateSuffix);
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
                for (var i = 0; i < rdr.Columns.Count; i++)
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
                for (var i = 0; i < rdr.FieldCount; i++)
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
                for (var i = 0; i < rdr.FieldCount; i++)
                    d.Add(rdr.GetName(i), rdr[i]);
                result.Add(e);
                yield return e;
            }
        }

        #endregion Utiltiy Methods
    }
}