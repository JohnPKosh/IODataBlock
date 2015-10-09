using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Business.Common.Reflection;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;

namespace Business.Templates
{
    public class GenericTemplateParser<T>
    {
        public String RenderTemplate(T model, string templateString, DynamicViewBag viewBag = null, String templatePrefix = null, String templateSuffix = null, string templateName = null, RazorTemplateSections sectionTemplates = null, bool rawStringFactory = true, bool allowMissingPropertiesOnDynamic = false, IEnumerable<string> nameSpaceNames = null, Type templateBase = null)
        {
            var config = GetConfiguration(rawStringFactory, allowMissingPropertiesOnDynamic, nameSpaceNames, templateBase);
            return RunTemplate(model, templateString, viewBag, templatePrefix, templateSuffix, templateName, sectionTemplates, config);
        }

        public String RenderTemplate(T model, Func<T, String> templateModelFunction, DynamicViewBag viewBag = null, String templatePrefix = null, String templateSuffix = null, string templateName = null, RazorTemplateSections sectionTemplates = null, bool rawStringFactory = true, bool allowMissingPropertiesOnDynamic = false, IEnumerable<string> nameSpaceNames = null, Type templateBase = null)
        {
            var config = GetConfiguration(rawStringFactory, allowMissingPropertiesOnDynamic, nameSpaceNames, templateBase);
            return RunTemplate(model, templateModelFunction(model), viewBag, templatePrefix, templateSuffix, templateName, sectionTemplates, config);
        }

        public String RunTemplate(T model, string templateString, DynamicViewBag viewBag = null, String templatePrefix = null, String templateSuffix = null, string templateName = null, RazorTemplateSections sectionTemplates = null, TemplateServiceConfiguration config = null)
        {
            var modelType = typeof(T).IsAnonymousOrDynamicType() ? null : typeof (T);
            templateName = String.IsNullOrWhiteSpace(templateName) ? String.Format("{0}_Template", model.GetType().IsAnonymousOrDynamicType() ? "anonymous" : model.GetType().Name) : templateName;
            if (config == null) config = new TemplateServiceConfiguration();
            using (var service = RazorEngineService.Create(config))
            {
                if (sectionTemplates != null)
                {
                    foreach (var s in sectionTemplates)
                    {
                        service.Compile(s.RazorTemplate, s.CacheName, s.ModelType ?? modelType);
                    }
                }
                service.Compile(templateString, templateName, modelType);
                return _wrapPrefixAndSuffix(templatePrefix, templateSuffix, service.Run(templateName, modelType, model, viewBag));
            }
        }

        public IEnumerable<String> RenderTemplates(IEnumerable<T> model, string templateString, DynamicViewBag viewBag = null, String templatePrefix = null, String templateSuffix = null, string templateName = null, RazorTemplateSections sectionTemplates = null, bool rawStringFactory = true, bool allowMissingPropertiesOnDynamic = false, IEnumerable<string> nameSpaceNames = null, Type templateBase = null)
        {
            var config = GetConfiguration(rawStringFactory, allowMissingPropertiesOnDynamic, nameSpaceNames, templateBase);
            return RunTemplates(model, templateString, viewBag, templatePrefix, templateSuffix, templateName, sectionTemplates, config);
        }

        public IEnumerable<String> RunTemplates(IEnumerable<T> model, string templateString, DynamicViewBag viewBag = null, String templatePrefix = null, String templateSuffix = null, string templateName = null, RazorTemplateSections sectionTemplates = null, TemplateServiceConfiguration config = null)
        {
            var modelType = typeof(T).IsAnonymousOrDynamicType() ? null : typeof(T);
            templateName = String.IsNullOrWhiteSpace(templateName) ? String.Format("{0}_ItemTemplate", model.GetType().IsAnonymousOrDynamicType() ? "anonymous" : model.GetType().Name) : templateName;
            if (config == null) config = new TemplateServiceConfiguration();
            using (var service = RazorEngineService.Create(config))
            {
                if (sectionTemplates != null)
                {
                    foreach (var s in sectionTemplates)
                    {
                        service.Compile(s.RazorTemplate, s.CacheName, s.ModelType ?? modelType);
                    }
                }
                service.Compile(templateString, templateName, modelType);
                foreach (var o in model)
                {
                    yield return _wrapPrefixAndSuffix(templatePrefix, templateSuffix, service.Run(templateName, modelType, o, viewBag));
                }
            }
        }

        public IEnumerable<String> RenderTemplates(IEnumerable<T> model, Func<T, String> templateModelFunction, DynamicViewBag viewBag = null, String templatePrefix = null, String templateSuffix = null, string templateName = null, RazorTemplateSections sectionTemplates = null, bool rawStringFactory = true, bool allowMissingPropertiesOnDynamic = false, IEnumerable<string> nameSpaceNames = null, Type templateBase = null)
        {
            var config = GetConfiguration(rawStringFactory, allowMissingPropertiesOnDynamic, nameSpaceNames, templateBase);
            return RunTemplates(model, templateModelFunction, viewBag, templatePrefix, templateSuffix, templateName, sectionTemplates, config);
        }

        public IEnumerable<String> RunTemplates(IEnumerable<T> model, Func<T, String> templateModelFunction, DynamicViewBag viewBag = null, String templatePrefix = null, String templateSuffix = null, string templateName = null, RazorTemplateSections sectionTemplates = null, TemplateServiceConfiguration config = null)
        {
            var modelType = typeof(T).IsAnonymousOrDynamicType() ? null : typeof(T);
            templateName = String.IsNullOrWhiteSpace(templateName) ? String.Format("{0}_ItemTemplate", model.GetType().IsAnonymousOrDynamicType() ? "anonymous" : model.GetType().Name) : templateName;
            if (config == null) config = new TemplateServiceConfiguration();
            using (var service = RazorEngineService.Create(config))
            {
                if (sectionTemplates != null)
                {
                    foreach (var s in sectionTemplates)
                    {
                        service.Compile(s.RazorTemplate, s.CacheName, s.ModelType ?? modelType);
                    }
                }
                foreach (var o in model)
                {
                    service.Compile(templateModelFunction(o), templateName, modelType);
                    yield return _wrapPrefixAndSuffix(templatePrefix, templateSuffix, service.Run(templateName, modelType, o, viewBag));
                }
            }
        }

        public TemplateServiceConfiguration GetConfiguration(bool rawStringFactory = true, bool allowMissingPropertiesOnDynamic = false, IEnumerable<string> nameSpaceNames = null, Type templateBase = null)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var config = new TemplateServiceConfiguration();
            config.EncodedStringFactory = rawStringFactory ? new RawStringFactory() as IEncodedStringFactory : new HtmlEncodedStringFactory();
            config.AllowMissingPropertiesOnDynamic = allowMissingPropertiesOnDynamic;

            /* config.Namespaces.Add("System.Configuration"); */
            config.Namespaces.Add("System.Dynamic");
            if (nameSpaceNames != null)
            {
                foreach (var ns in nameSpaceNames)
                {
                    config.Namespaces.Add(ns);
                }
            }
            if (templateBase != null) config.BaseTemplateType = templateBase;
            return config;
        }

        private string _wrapPrefixAndSuffix(string prefix, string suffix, string value)
        {
            if (prefix == null && suffix == null) return value;
            return prefix + value + suffix;
        }
    }
}
