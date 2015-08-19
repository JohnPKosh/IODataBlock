using System;
using System.Collections.Generic;
using System.Linq;

namespace Sandbox4.Models.Content
{
    public class ContentManager
    {
        /* TODO: Review how content should be managed.  Using this singleton will store all content in memory.  So caching may be needed for certain content */

        private static ContentManager _data = new ContentManager();

        private ContentManager()
        {
            UnauthenticatedContent = new HashSet<BaseContent>();
            UnauthenticatedContent.Add(new BaseContent()
            {
                Content = "Home Jumbotron Title",
                Targets = new HashSet<TargetPath>(new[]
                {
                    new TargetPath()
                    {
                        AreaName = "",
                        Controller = "Home",
                        Action = "Index",
                        Section = "Jumbotron",
                        ContentId = "Title"
                    }
                })
            });
            UnauthenticatedContent.Add(new BaseContent()
            {
                Content = "Home Jumbotron Subtitle Section! " + DateTime.Now.ToString("O"),
                Targets = new HashSet<TargetPath>(new[]
                {
                    new TargetPath()
                    {
                        AreaName = "",
                        Controller = "Home",
                        Action = "Index",
                        Section = "Jumbotron",
                        ContentId = "Subtitle"
                    }
                })
            });

            // About
            UnauthenticatedContent.Add(new BaseContent()
            {
                Content = "About Jumbotron Title",
                Targets = new HashSet<TargetPath>(new[]
                {
                    new TargetPath()
                    {
                        AreaName = "",
                        Controller = "Home",
                        Action = "About",
                        Section = "Jumbotron",
                        ContentId = "Title"
                    }
                })
            });
            UnauthenticatedContent.Add(new BaseContent()
            {
                Content = "About Jumbotron Subtitle Section!",
                Targets = new HashSet<TargetPath>(new[]
                {
                    new TargetPath()
                    {
                        AreaName = "",
                        Controller = "Home",
                        Action = "About",
                        Section = "Jumbotron",
                        ContentId = "Subtitle"
                    }
                })
            });
        }

        public static ContentManager Data
        {
            get { return _data ?? (_data = new ContentManager()); }
        }

        public static void Reload()
        {
            /* TODO: Review to verify if _data should be set to null or not? */
            /* TODO: Alternately we could just reload the Value property too? */
            //_data = null;
            _data = new ContentManager();
        }

        public HashSet<BaseContent> UnauthenticatedContent { get; set; }

        public HashSet<BaseContent> AuthenticatedContent { get; set; }

        public string GetUnauthenticatedContent(string controller, string action, string section, string contentId, string areaName = "", string defaultContent = "Content Goes Here!")
        {
            var firstOrDefault = Data.UnauthenticatedContent.FirstOrDefault(x => x.IsTarget(controller, action, section, contentId, areaName));
            if (firstOrDefault != null && !string.IsNullOrWhiteSpace(firstOrDefault.Content))
            {
                return firstOrDefault.Content;
            }
            return defaultContent;
        }

    }
}