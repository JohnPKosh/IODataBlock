using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace Business.Templates.deprecated
{
    public class RazorTemplateParser
    {
        public String RenderItemTemplateFromString(Func<dynamic, String> templateModelFunction, dynamic model)
        {
            //return Razor.Parse(templateModelFunction(model), model, String.Format("{0}_ItemTemplate", model.GetType().Name));
            return Engine.Razor.RunCompile(String.Format("{0}_Template", model.GetType().Name), templateModelFunction(model), null, model);
        }

        public String RenderItemTemplateFromFile(FileInfo file, dynamic model, bool rawStringFactory = true, DynamicViewBag viewBag = null, IEnumerable<string> nameSpaceNames = null )
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
                return RazorEngineServiceExtensions.RunCompile(service, template, String.Format("{0}_ItemTemplate", model.GetType().Name), modelType: null, model: model, viewBag: viewBag);
            }

            //return Razor.Parse(template, model, String.Format("{0}_ItemTemplate", model.GetType().Name));
        }

        #region Render IEnumerable Item and IEnumerable Model Template Methods

        public IEnumerable<String> RenderIEnumerableItemTemplateFromString(IEnumerable<String> templateString, IEnumerable<object> models)
        {
            return Razor.ParseMany(templateString, models);
        }

        public IEnumerable<String> RenderIEnumerableItemTemplateFromString<T>(IEnumerable<String> templateString, IEnumerable<T> models)
        {
            return Razor.ParseMany(templateString, models);
        }

        public IEnumerable<String> RenderIEnumerableItemTemplateFromFile<T>(IEnumerable<String> templatePaths, IEnumerable<T> models)
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

        public IEnumerable<String> RenderIEnumerableItemTemplateFromFile(IEnumerable<String> templatePaths, IEnumerable<object> models)
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

        public IEnumerable<String> RenderRawIEnumerableItemTemplateFromString(IEnumerable<String> templateString, IEnumerable<object> models, IEnumerable<DynamicViewBag> viewBags = null, IEnumerable<string> cacheNames = null, bool parallel = false)
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

        public IEnumerable<String> RenderRawIEnumerableItemTemplateFromFile<T>(IEnumerable<String> templatePaths, IEnumerable<T> models)
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

        public IEnumerable<String> RenderRawIEnumerableItemTemplateFromFile(IEnumerable<String> templatePaths, IEnumerable<object> models)
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

        public IEnumerable<String> RenderIEnumerableItemTemplateFromFile(String templatePath, IEnumerable<object> models)
        {
            var template = File.ReadAllText(templatePath);
            return Razor.ParseMany(template, models);
        }

        public IEnumerable<String> RenderIEnumerableItemTemplateFromString(String templateString, IEnumerable<object> models)
        {
            return Razor.ParseMany(templateString, models);
        }

        public IEnumerable<String> RenderIEnumerableItemTemplateFromFile<T>(String templatePath, IEnumerable<T> models)
        {
            var template = File.ReadAllText(templatePath);
            return Razor.ParseMany(template, models);
        }

        public IEnumerable<String> RenderIEnumerableItemTemplateFromString<T>(String templateString, IEnumerable<T> models)
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

        public String RenderItemTemplateFromFile(String templatePath, object model)
        {
            var template = File.ReadAllText(templatePath);
            return Razor.Parse(template, model, String.Format("{0}_ItemTemplate", model.GetType().Name));
        }

        public String RenderItemTemplateFromString(String templateString, object model)
        {
            return Razor.Parse(templateString, model, String.Format("{0}_ItemTemplate", model.GetType().Name));
        }

        public String RenderItemTemplateFromFile<T>(String templatePath, T model)
        {
            var template = File.ReadAllText(templatePath);
            return Razor.Parse(template, model, String.Format("{0}_ItemTemplate", typeof(T).Name));
        }

        public String RenderItemTemplateFromString<T>(String templateString, T model)
        {
            return Razor.Parse(templateString, model, String.Format("{0}_ItemTemplate", typeof(T).Name));
        }

        #endregion Render Item Template Methods

        #region Render Raw Item Template Methods

        public String RenderRawItemTemplateFromFile(String templatePath, object model, DynamicViewBag viewBag = null)
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);

            var template = File.ReadAllText(templatePath);
            return ct.Parse(template, model, viewBag, String.Format("{0}_ItemTemplate", model.GetType().Name));
        }

        public String RenderRawItemTemplateFromString(String templateString, object model, DynamicViewBag viewBag = null)
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);
            return ct.Parse(templateString, model, viewBag, String.Format("{0}_ItemTemplate", model.GetType().Name));
        }

        public String RenderRawItemTemplateFromFile<T>(String templatePath, T model, DynamicViewBag viewBag = null)
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);
            var template = File.ReadAllText(templatePath);
            return ct.Parse<T>(template, model, viewBag, String.Format("{0}_ItemTemplate", typeof(T).Name));
        }

        public String RenderRawItemTemplateFromString<T>(String templateString, T model, DynamicViewBag viewBag = null)
        {
            var c = new TemplateServiceConfiguration { EncodedStringFactory = new RawStringFactory() };
            var ct = new TemplateService(c);
            Razor.SetTemplateService(ct);
            return ct.Parse<T>(templateString, model, viewBag, String.Format("{0}_ItemTemplate", typeof(T).Name));
        }

        #endregion Render Raw Item Template Methods
    }
}