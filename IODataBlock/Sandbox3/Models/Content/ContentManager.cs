using System.Collections.Generic;

namespace Sandbox3.Models.Content
{
    public class ContentManager
    {
        private static ContentManager _data = new ContentManager();

        private ContentManager()
        {
            Value = new HashSet<BaseContent>();
            Value.Add(new BaseContent()
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
            Value.Add(new BaseContent()
            {
                Content = "Home Jumbotron Subtitle Section!",
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
            Value.Add(new BaseContent()
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
            Value.Add(new BaseContent()
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

        public HashSet<BaseContent> Value { get; set; }
    }
}