using System.Collections.Generic;
using System.Linq;

namespace Sandbox3.Models.Content
{
    public class BaseContent
    {
        public HashSet<TargetPath> Targets { get; set; }

        public string Content { get; set; }

        public bool IsTarget(string controller, string action, string section, string contentId, string areaName = "")
        {
            return Targets.Any(x=>x.AreaName == areaName && x.Controller == controller && x.Action == action && x.Section == section && x.ContentId == contentId);
        }

    }
}
