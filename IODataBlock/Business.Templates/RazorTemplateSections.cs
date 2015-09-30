using System.Collections.Generic;

namespace Business.Templates
{
    public class RazorTemplateSections : List<RazorTemplateSection>
    {
        public void Add(string razorTemplate, object model, string cacheName)
        {
            Add(new RazorTemplateSection(razorTemplate, model, cacheName));
        }
    }
}