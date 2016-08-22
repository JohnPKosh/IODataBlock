using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Business.Templates
{
    public class RazorTemplateParser
    {
        public string RenderItemTemplateFromString(Func<dynamic, string> templateModelFunction, dynamic model)
        {
            //return Razor.Parse(templateModelFunction(model), model, String.Format("{0}_ItemTemplate", model.GetType().Name));
            return Engine.Razor.RunCompile(string.Format("{0}_Template", model.GetType().Name), templateModelFunction(model), null, model);
        }

        public string RenderItemTemplateFromFile(FileInfo file, dynamic model, bool rawStringFactory = true, DynamicViewBag viewBag = null, IEnumerable<string> nameSpaceNames = null)
        {
            string template;
            using (var fs = file.OpenText())
            {
                template = fs.ReadToEnd();
            }
            // ReSharper disable once UseObjectOrCollectionInitializer
            var config = new TemplateServiceConfiguration();

            config.EncodedStringFactory = rawStringFactory ? new RawStringFactory() as IEncodedStringFactory : new HtmlEncodedStringFactory();
            config.AllowMissingPropertiesOnDynamic = false;

            /* config.Namespaces.Add("System.Configuration"); */
            if (nameSpaceNames != null)
            {
                foreach (var ns in nameSpaceNames)
                {
                    config.Namespaces.Add(ns);
                }
            }

            using (var service = RazorEngineService.Create(config))
            {
                return RazorEngineServiceExtensions.RunCompile(service, template, string.Format("{0}_ItemTemplate", model.GetType().Name), modelType: null, model: model, viewBag: viewBag);
            }

            //return Razor.Parse(template, model, String.Format("{0}_ItemTemplate", model.GetType().Name));
        }

        #region Render IEnumerable Item and IEnumerable Model Template Methods

        public IEnumerable<string> RenderIEnumerableItemTemplateFromString(IEnumerable<string> templateString, IEnumerable<object> models)
        {
            return Razor.ParseMany(templateString, models);
        }

        public IEnumerable<string> RenderIEnumerableItemTemplateFromString<T>(IEnumerable<string> templateString, IEnumerable<T> models)
        {
            return Razor.ParseMany(templateString, models);
        }

        public IEnumerable<string> RenderIEnumerableItemTemplateFromFile<T>(IEnumerable<string> templatePaths, IEnumerable<T> models)
        {
            var modelarr = models.ToArray();
            var cnt = 0;
            foreach (var f in templatePaths)
            {
                var output = RenderItemTemplateFromFile(f, modelarr[cnt]);
                cnt++;
                yield return output;
            }
        }

        public IEnumerable<string> RenderIEnumerableItemTemplateFromFile(IEnumerable<string> templatePaths, IEnumerable<object> models)
        {
            var modelarr = models.ToArray();
            var cnt = 0;
            foreach (var f in templatePaths)
            {
                var output = RenderItemTemplateFromFile(f, modelarr[cnt]);
                cnt++;
                yield return output;
            }
        }

        #endregion Render IEnumerable Item and IEnumerable Model Template Methods

        #region Render Raw IEnumerable Item and IEnumerable Model Template Methods

        public IEnumerable<string> RenderRawIEnumerableItemTemplateFromString(IEnumerable<string> templateString, IEnumerable<object> models, IEnumerable<DynamicViewBag> viewBags = null, IEnumerable<string> cacheNames = null, bool parallel = false)
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            return ct.ParseMany(templateString, models, viewBags, cacheNames, parallel);
        }

        //public IEnumerable<String> RenderRawIEnumerableItemTemplateFromString<T>(IEnumerable<String> TemplateString, IEnumerable<T> models)
        //{
        //    try
        //    {
        //        var c = new TemplateServiceConfiguration();
        //        c.EncodedStringFactory = new RawStringFactory();
        //        var ct = new TemplateService(c);
        //        return ct.ParseMany<T>(TemplateString, models);
        //    }
        //    catch (TemplateParsingException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        public IEnumerable<string> RenderRawIEnumerableItemTemplateFromFile<T>(IEnumerable<string> templatePaths, IEnumerable<T> models)
        {
            var modelarr = models.ToArray();
            var cnt = 0;
            foreach (var f in templatePaths)
            {
                var output = RenderRawItemTemplateFromFile(f, modelarr[cnt]);
                cnt++;
                yield return output;
            }
        }

        public IEnumerable<string> RenderRawIEnumerableItemTemplateFromFile(IEnumerable<string> templatePaths, IEnumerable<object> models)
        {
            var modelarr = models.ToArray();
            var cnt = 0;
            foreach (var f in templatePaths)
            {
                var output = RenderRawItemTemplateFromFile(f, modelarr[cnt]);
                cnt++;
                yield return output;
            }
        }

        #endregion Render Raw IEnumerable Item and IEnumerable Model Template Methods

        #region Render IEnumerable Item Template Methods

        public IEnumerable<string> RenderIEnumerableItemTemplateFromFile(string templatePath, IEnumerable<object> models)
        {
            var template = File.ReadAllText(templatePath);
            return Razor.ParseMany(template, models);
        }

        public IEnumerable<string> RenderIEnumerableItemTemplateFromString(string templateString, IEnumerable<object> models)
        {
            return Razor.ParseMany(templateString, models);
        }

        public IEnumerable<string> RenderIEnumerableItemTemplateFromFile<T>(string templatePath, IEnumerable<T> models)
        {
            var template = File.ReadAllText(templatePath);
            return Razor.ParseMany(template, models);
        }

        public IEnumerable<string> RenderIEnumerableItemTemplateFromString<T>(string templateString, IEnumerable<T> models)
        {
            return Razor.ParseMany(templateString, models);
        }

        #endregion Render IEnumerable Item Template Methods

        #region Render Raw IEnumerable Item Template Methods

        //public IEnumerable<String> RenderRawIEnumerableItemTemplateFromFile(String TemplatePath, IEnumerable<object> models, IEnumerable<DynamicViewBag> viewBags = null, IEnumerable<string> cacheNames = null, bool parallel = false)
        //{
        //    try
        //    {
        //        var c = new TemplateServiceConfiguration();
        //        c.EncodedStringFactory = new RawStringFactory();
        //        var ct = new TemplateService(c);
        //        Razor.SetTemplateService(ct);
        //        var template = File.ReadAllText(TemplatePath);
        //        return ct.ParseMany(template, models, viewBags, cacheNames, parallel);
        //    }
        //    catch (TemplateParsingException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IEnumerable<String> RenderRawIEnumerableItemTemplateFromString(String TemplateString, IEnumerable<object> models, IEnumerable<DynamicViewBag> viewBags = null, IEnumerable<string> cacheNames = null, bool parallel = false)
        //{
        //    try
        //    {
        //        var c = new TemplateServiceConfiguration();
        //        c.EncodedStringFactory = new RawStringFactory();
        //        var ct = new TemplateService(c);
        //        Razor.SetTemplateService(ct);
        //        return ct.ParseMany(TemplateString, models,viewBags, cacheNames, parallel);
        //    }
        //    catch (TemplateParsingException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IEnumerable<String> RenderRawIEnumerableItemTemplateFromFile<T>(String TemplatePath, IEnumerable<T> models)
        //{
        //    try
        //    {
        //        var c = new TemplateServiceConfiguration();
        //        c.EncodedStringFactory = new RawStringFactory();
        //        var ct = new TemplateService(c);
        //        Razor.SetTemplateService(ct);
        //        var template = File.ReadAllText(TemplatePath);
        //        return ct.ParseMany<T>(template, models);
        //    }
        //    catch (TemplateParsingException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        //public IEnumerable<String> RenderRawIEnumerableItemTemplateFromString<T>(String TemplateString, IEnumerable<T> models)
        //{
        //    try
        //    {
        //        var c = new TemplateServiceConfiguration();
        //        c.EncodedStringFactory = new RawStringFactory();
        //        var ct = new TemplateService(c);
        //        Razor.SetTemplateService(ct);
        //        return ct.ParseMany<T>(TemplateString, models);
        //    }
        //    catch (TemplateParsingException)
        //    {
        //        throw;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        #endregion Render Raw IEnumerable Item Template Methods

        #region Render Item Template Methods

        public string RenderItemTemplateFromFile(string templatePath, object model)
        {
            var template = File.ReadAllText(templatePath);
            return Razor.Parse(template, model, $"{model.GetType().Name}_ItemTemplate");
        }

        public string RenderItemTemplateFromString(string templateString, object model)
        {
            return Razor.Parse(templateString, model, $"{model.GetType().Name}_ItemTemplate");
        }

        public string RenderItemTemplateFromFile<T>(string templatePath, T model)
        {
            var template = File.ReadAllText(templatePath);
            return Razor.Parse(template, model, $"{typeof(T).Name}_ItemTemplate");
        }

        public string RenderItemTemplateFromString<T>(string templateString, T model)
        {
            return Razor.Parse(templateString, model, $"{typeof(T).Name}_ItemTemplate");
        }

        #endregion Render Item Template Methods

        #region Render Raw Item Template Methods

        public string RenderRawItemTemplateFromFile(string templatePath, object model, DynamicViewBag viewBag = null)
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);

            var template = File.ReadAllText(templatePath);
            return ct.Parse(template, model, viewBag, $"{model.GetType().Name}_ItemTemplate");
        }

        public string RenderRawItemTemplateFromString(string templateString, object model, DynamicViewBag viewBag = null)
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);
            return ct.Parse(templateString, model, viewBag, $"{model.GetType().Name}_ItemTemplate");
        }

        public string RenderRawItemTemplateFromFile<T>(string templatePath, T model, DynamicViewBag viewBag = null)
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);
            var template = File.ReadAllText(templatePath);
            return ct.Parse<T>(template, model, viewBag, $"{typeof(T).Name}_ItemTemplate");
        }

        public string RenderRawItemTemplateFromString<T>(string templateString, T model, DynamicViewBag viewBag = null)
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);
            return ct.Parse<T>(templateString, model, viewBag, $"{typeof(T).Name}_ItemTemplate");
        }

        #endregion Render Raw Item Template Methods
    }
}