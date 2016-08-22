using System;

namespace Business.Templates
{
    public class RazorTemplateSection
    {
        public RazorTemplateSection()
        {
        }

        public RazorTemplateSection(string razorTemplate, string cacheName, Type modelType = null)
        {
            RazorTemplate = razorTemplate;
            CacheName = cacheName;
            ModelType = modelType;
        }

        public string RazorTemplate { get; set; }

        public string CacheName { get; set; }

        public Type ModelType { get; set; }
    }
}