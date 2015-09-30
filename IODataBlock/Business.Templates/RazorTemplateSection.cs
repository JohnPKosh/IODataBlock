namespace Business.Templates
{
    public class RazorTemplateSection
    {
        public RazorTemplateSection()
        {
        }

        public RazorTemplateSection(string razorTemplate, object model, string cacheName)
        {
            RazorTemplate = razorTemplate;
            Model = model;
            CacheName = cacheName;
        }

        public string RazorTemplate { get; set; }

        public object Model { get; set; }

        public string CacheName { get; set; }
    }
}