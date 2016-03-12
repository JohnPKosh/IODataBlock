using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;

namespace Business.EWS.HtmlUtil
{
    public static class MailCleaner
    {

        public static string CleanBody(string html)
        {
            return RemoveAllAttributes(RemoveOfficeOPTag(GetBody(html)), new List<string>() {"src", "href", "height", "width"});
        }

        public static string GetBody(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.Descendants().First(x => x.Name == "body").InnerHtml;
        }

        public static string GetStyle(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            return doc.DocumentNode.Descendants().First(x => x.Name == "style").InnerHtml;
        }

        public static string RemoveOfficeOPTag(string html)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.Descendants("o:p").ToList();
            foreach (var o in nodes)
            {
                o.ParentNode.ChildNodes.Remove(o);
            }
            return doc.DocumentNode.OuterHtml;
        }

        public static string RemoveAllAttributes(string html, IEnumerable<string> except)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);
            var elements = doc.DocumentNode.SelectNodes("//*").ToList();
            foreach (var element in elements)
            {
                var remove = new List<string>(element.Attributes.Where(x => !except.Contains(x.Name)).Select(x => x.Name));
                foreach (var r in remove)
                {
                    element.Attributes[r].Remove();
                }
            }
            return doc.DocumentNode.OuterHtml;
        }


    }
}
