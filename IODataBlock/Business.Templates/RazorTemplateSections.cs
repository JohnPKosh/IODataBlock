using System;
using System.Collections.Generic;

namespace Business.Templates
{
    public class RazorTemplateSections : List<RazorTemplateSection>
    {
        public void Add(string razorTemplate, string cacheName, Type modelType = null)
        {
            Add(new RazorTemplateSection(razorTemplate, cacheName, modelType));
        }
    }
}