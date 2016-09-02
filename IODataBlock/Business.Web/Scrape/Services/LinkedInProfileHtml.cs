using Flurl;
using System.Dynamic;
using System.Linq;
using System;
using System.Text.RegularExpressions;
using System.Web;

namespace Business.Web.Scrape.Services
{
    public class LinkedInProfileHtml : HtmlUtilityBase
    {
        public LinkedInProfileHtml(string content) : base(content)
        {
        }



        #region LinkedIn Methods



        //<div class="masthead" id="member-3753448">
        public long GetLinkedInProfileId()
        {
            if (!HasDocument) return -1;
            if (HasLoadError) return -1;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return -1;
                }
                var link = Doc.DocumentNode?.SelectSingleNode("//div[@class='masthead']")?.GetAttributeValue("id", string.Empty)?.Trim();
                var profileIdString = link.Replace("member-", string.Empty);
                long profileid;
                if (!string.IsNullOrWhiteSpace(profileIdString) && long.TryParse(profileIdString, out profileid))
                {
                    return profileid;
                }
                else
                {
                    return -1;
                }
            }
            catch { }
            return -1;
        }

        public string GetLinkedInProfileUrl()
        {
            //<a class="view-public-profile" href="https://www.linkedin.com/in/sunil-kumar-9b064716">https://www.linkedin.com/in/sunil-kumar-9b064716</a>
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                return HtmlDecodeString(Doc.DocumentNode?.SelectSingleNode("//a[@class='view-public-profile']")?.GetAttributeValue("href", string.Empty)?.Trim());
            }
            catch { }
            return null;
        }

        //<span class="full-name" dir="auto">David Swetnam</span>
        public string GetLinkedInFullName()
        {
            //<a class="view-public-profile" href="https://www.linkedin.com/in/sunil-kumar-9b064716">https://www.linkedin.com/in/sunil-kumar-9b064716</a>
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                return HtmlDecodeString(Doc.DocumentNode?.SelectSingleNode("//span[@class='full-name']")?.InnerText.Trim());
            }
            catch { }
            return null;
        }

        //<div class="member-connections"><strong>298</strong>connections</div>
        public int GetLinkedInConnections()
        {
            if (!HasDocument) return -1;
            if (HasLoadError) return -1;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return -1;
                }
                var link = Doc.DocumentNode?.SelectSingleNode("//div[@class='member-connections']")?.InnerText?.Trim();
                var valueString = link.Replace("connections", string.Empty).Replace("+", string.Empty).Trim();
                int value;
                if (!string.IsNullOrWhiteSpace(valueString) && int.TryParse(valueString, out value))
                {
                    return value;
                }
                else
                {
                    return -1;
                }
            }
            catch { }
            return -1;
        }

        //<p class="title" dir="ltr">Project Management | Customer Service | Strategic Planning | Software Prototyping</p>
        //<p class="title" dir="ltr">Director of Application Development at CloudRoute</p>
        public string GetLinkedInTitle()
        {
            //<a class="view-public-profile" href="https://www.linkedin.com/in/sunil-kumar-9b064716">https://www.linkedin.com/in/sunil-kumar-9b064716</a>
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                var titleString = Doc.DocumentNode?.SelectSingleNode("//p[@class='title']")?.InnerText.Trim();
                if (string.IsNullOrWhiteSpace(titleString)) return null;
                var lastAt = titleString.LastIndexOf(" at ", StringComparison.Ordinal);
                if (lastAt > 0) titleString = titleString.Substring(0, lastAt);
                return HtmlDecodeString(titleString);
            }
            catch { }
            return null;
        }

        //<a dir="auto" href="/company/273381?trk=prof-0-ovw-curr_pos">Evolve IP</a>
        //<a href="/company/974572?trk=prof-0-ovw-curr_pos" dir="auto">T3 Motion</a>
        public string GetLinkedInCompanyName()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }

                var rv = FirstHrefWhere(x => x.GetAttributeValue("href", string.Empty).Contains("trk=prof-0-ovw-curr_pos"))?.InnerText.Trim();
                if (string.IsNullOrWhiteSpace(rv))
                {
                    //<a href="/company/974572?trk=prof-exp-company-name" dir="auto">T3 Motion</a>
                    var matches = AllHrefNodesWhere(x => x.GetAttributeValue("href", string.Empty).Contains(@"trk=prof-exp-company-name") && x.GetAttributeValue("href", string.Empty).Contains(@"/company/")).ToList();
                    rv = FirstHrefWhere(x => x.GetAttributeValue("href", string.Empty).Contains(@"trk=prof-exp-company-name")  && !string.IsNullOrWhiteSpace(x.InnerText))?.InnerText.Trim();
                }
                if (string.IsNullOrWhiteSpace(rv))
                {
                    var titleString = Doc.DocumentNode?.SelectSingleNode("//p[@class='title']")?.InnerText.Trim();
                    if (string.IsNullOrWhiteSpace(titleString)) return null;
                    var lastAt = titleString.LastIndexOf(" at ", StringComparison.Ordinal);
                    if (lastAt > 0) titleString = titleString.Substring(lastAt + 4);
                    rv = titleString;
                }
                return HtmlDecodeString(rv);
            }
            catch { }
            return null;
        }

        //<a dir="auto" href="/company/273381?trk=prof-0-ovw-curr_pos">Evolve IP</a>
        public long GetCurrentLinkedInCompanyId()
        {
            if (!HasDocument) return -1;
            if (HasLoadError) return -1;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return -1;
                }
                var link = FirstHref("trk=prof-0-ovw-curr_pos");
                if (string.IsNullOrWhiteSpace(link))
                {
                    //<div id="experience-720446400" class="editable-item section-item current-position"><div id="experience-720446400-view"><header><h5 class="experience-logo" aria-hidden="true"><a href="/company/14151?trk=prof-exp-company-name">
                    link = FirstHref("trk=prof-exp-company-name");
                }

                if (!string.IsNullOrWhiteSpace(link))
                {
                    var lastSegmentStart = link.LastIndexOf(@"/", StringComparison.Ordinal);

                    //var companyidstring = link.Substring(lastSegmentStart + 1, link.IndexOf("?", StringComparison.Ordinal) - lastSegmentStart - 1);
                    var companyidstring = link.Substring(lastSegmentStart + 1); /* query params are already stripped */
                    long companyid;
                    if (!string.IsNullOrWhiteSpace(companyidstring) && long.TryParse(companyidstring, out companyid))
                    {
                        return companyid;
                    }
                    else
                    {
                        return -1;
                    }
                }
                else
                {
                    //<div id="ad-right-top" class="ganz-module ad">
                    //< script type = "text/javascript" > fs._server.fire("profile_v2_right_ad",{event:"before",type:"html"});</script>
                    //<iframe width = "300" scrolling="no" height="250" frameborder="0" allowtransparency="true" border="0" data-src="/csp/dtag?sz=300x250&amp;ti=2&amp;p=1&amp;z=profile&amp;pk=nprofile-view&amp;_x=%3Bcompany%3D991548%3Bpcntry%3Dus" src="/csp/dtag?sz=300x250&amp;ti=2&amp;p=1&amp;z=profile&amp;pk=nprofile-view&amp;_x=%3Bcompany%3D991548%3Bpcntry%3Dus"></iframe><script type = "text/javascript" > fs._server.fire("profile_v2_right_ad",{event:"after",type:"html"});</script>
                    //</div>
                    var datasrc = Doc.DocumentNode?.SelectSingleNode("//div[@id='ad-right-top']/iframe[@data-src]")?.Attributes["data-src"].Value;

                    var r = Regex.Matches(datasrc, "company%3D([0-9]+)%3B");
                    if (r.Count > 0)
                    {
                        var firstr = r[0];
                        var id = firstr.Groups[1].Value;
                        return long.Parse(id);
                    }

                    //if (!string.IsNullOrWhiteSpace(datasrc))
                    //{
                    //    var datasrcUnencoded = HttpUtility.HtmlDecode(datasrc);
                    //    var id = new Url(datasrcUnencoded).QueryParams["3Bcompany"] as string;
                    //    return long.Parse(id);
                    //}
                }

                return -1;
            }
            catch { }
            return -1;
        }

        //<dd class="industry"><a name="industry" title="Find other members in this industry" href="/vsearch/p?f_I=96&amp;trk=prof-0-ovw-industry">Information Technology and Services</a></dd>
        //<dd class="industry"><a name="industry" title="Find other members in this industry" href="/vsearch/p?f_I=4&amp;trk=prof-0-ovw-industry">Computer Software</a></dd>
        public string GetLinkedInIndustry()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                return HtmlDecodeString(FirstHrefWhere(x => x.GetAttributeValue("href", string.Empty).Contains("trk=prof-0-ovw-industry"))?.InnerText.Trim());
            }
            catch { }
            return null;
        }

        //<a name="location" title="Find other members in Cleveland, Ohio" href="/vsearch/p?f_G=us%3A28&amp;trk=prof-0-ovw-location">Cleveland, Ohio</a>
        public string GetLinkedInLocation()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                return HtmlDecodeString(FirstHrefWhere(x => x.GetAttributeValue("href", string.Empty).Contains("trk=prof-0-ovw-location"))?.InnerText.Trim());
            }
            catch { }
            return null;
        }

        //<div id="email-view"><ul><li><a href="mailto:shepov@gmail.com">shepov@gmail.com</a></li></ul></div>
        public string GetLinkedInEmail()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                //var emailView = Doc.DocumentNode?.SelectSingleNode("//div[@id='email-view']/ul/li/a[@href]");
                return Doc.DocumentNode?.SelectSingleNode("//div[@id='email-view']/ul/li/a[@href]")?.GetAttributeValue("href", string.Empty)?.Replace("mailto:", string.Empty);
            }
            catch { }
            return null;
        }

        //<div class="editable-item" id="im">
        public string GetLinkedInIm()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                return Doc.DocumentNode?.SelectSingleNode("//div[@id='im']")?.InnerText.Trim();
            }
            catch { }
            return null;
        }

        //<div id="twitter-view"><ul><li><a href="/redir/redirect?url=http%3A%2F%2Ftwitter%2Ecom%2Fjgale540&amp;urlhash=riaR">jgale540</a></li></ul></div>
        public string GetLinkedInTwitter()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                return Doc.DocumentNode?.SelectSingleNode("//div[@id='twitter-view']/ul/li/a[@href]").InnerText;
            }
            catch { }
            return null;
        }

        //<div class="editable-item" id="address">
        public string GetLinkedInAddress()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                return HtmlDecodeString(Doc.DocumentNode?.SelectSingleNode("//div[@id='address']")?.InnerText.Trim());
            }
            catch { }
            return null;
        }

        //<div class="editable-item" id="phone">
        public string GetLinkedInPhone()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                //<div id="phone-view"><ul><li>440-773-4642&nbsp;(Mobile)</li></ul></div>
                var nodes =
                    Doc.DocumentNode?.SelectNodes("//div[@id='phone-view']/ul/*")?
                        .Select(x => x.InnerText.Trim().Replace("&nbsp;", " "))
                        .ToArray();
                if (nodes != null) return string.Join(", ", nodes);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            return null;
        }

        public string GetLinkedInProfilePhotoUrl()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                var node = Doc.DocumentNode.SelectSingleNode("//div[@class='profile-picture']");
                return node.SelectSingleNode("//img")?.GetAttributeValue("src", string.Empty);
            }
            catch { }
            return null;
        }





        #endregion LinkedIn Methods
    }
}