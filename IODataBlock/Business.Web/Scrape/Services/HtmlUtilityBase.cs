using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Business.Web.Scrape.Services
{
    public class HtmlUtilityBase
    {
        #region Class Initialization

        public HtmlUtilityBase(string content)
        {
            Content = content;
            Doc = new HtmlDocument();
            InitializeDocument();
        }

        private void InitializeDocument()
        {
            if (!string.IsNullOrWhiteSpace(Content))
            {
                try
                {
                    Doc.LoadHtml(Content);
                    HasDocument = true;
                }
                catch (Exception ex)
                {
                    HasLoadError = true;
                    LoadErrorMessage = $"{ex.Message} - {ex.InnerException.Message}";
                }
            }
        }

        #endregion Class Initialization

        #region Fields and Props

        protected HtmlDocument Doc { get; set; }
        protected string Content { get; set; }
        protected bool HasDocument { get; set; }
        protected bool HasLoadError { get; set; }
        protected string LoadErrorMessage { get; set; }

        #endregion Fields and Props

        public bool IsGoDaddyParked()
        {
            if (!HasDocument) return false;
            return Content.Contains("mcc.godaddy.com/park");
        }

        public string FirstHref(string containing)
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.Descendants("a").Any() || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                foreach (var link in Doc.DocumentNode.SelectNodes("//a[@href]"))
                {
                    string hrefValue;
                    try
                    {
                        hrefValue = link.GetAttributeValue("href", string.Empty);
                    }
                    catch
                    {
                        continue;
                    }
                    if (!hrefValue.Contains(containing)) continue;
                    if (hrefValue.Contains("?"))
                    {
                        hrefValue = hrefValue.Remove(hrefValue.IndexOf("?", StringComparison.Ordinal));
                    }
                    return hrefValue.Trim().Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);
                }
            }
            catch { }
            return null;
        }


        public HtmlNode FirstHrefWhere(Func<HtmlNode, bool> predicate)
        {
            if (!HasDocument) return null;
            if (HasLoadError) return null;
            try
            {
                return Doc.DocumentNode.SelectNodes("//a[@href]").FirstOrDefault(predicate);
            }
            catch { }
            return null;
        }

        public IEnumerable<HtmlNode> AllHrefsWhere(Func<HtmlNode, bool> predicate)
        {
            if (!HasDocument) return null;
            if (HasLoadError) return null;
            try
            {
                return Doc.DocumentNode.SelectNodes("//a[@href]").Where(predicate);
            }
            catch { }
            return null;
        }

        public IEnumerable<string> AllHrefUrlsWhere(Func<HtmlNode, bool> predicate)
        {
            return AllHrefsWhere(predicate).Select(x => x.GetAttributeValue("href", string.Empty)).Where(y => !string.IsNullOrWhiteSpace(y));
        }


        public string GetFirstLinkedInUrl()
        {
            return FirstHref("linkedin.com/");
        }

        public string GetFirstLinkedInCompanyUrl()
        {
            return FirstHref("linkedin.com/company");
        }

        public string GetFirstGooglePlusUrl()
        {
            return FirstHref("plus.google.com");
        }

        public string GetFirstYouTubeUrl()
        {
            return FirstHref("youtube.com/user");
        }

        public string GetFirstFacebookUrl()
        {
            return FirstHref("facebook.com/pages");
        }

        public string GetFirstTwitterUrl()
        {
            return FirstHref("twitter.com/");
        }
    }
}