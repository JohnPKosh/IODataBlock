﻿using Flurl;
using System.Dynamic;
using System.Linq;
using System.Web;

namespace Business.Web.Scrape.Services
{
    public class LinkedInCompanyHtml : HtmlUtilityBase
    {
        public LinkedInCompanyHtml(string content) : base(content)
        {
        }



        #region LinkedIn Methods

        public string GetLinkedInCompanyPhotoUrl()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                foreach (var node in Doc.DocumentNode.SelectNodes("//div[@class='image-wrapper']"))
                {
                    var img = node.SelectSingleNode("//img[@class='image']");
                    var hrefValue = string.Empty;
                    try
                    {
                        hrefValue = img.GetAttributeValue("src", string.Empty);
                    }
                    catch
                    {
                        continue;
                    }
                    return hrefValue.Trim().Replace("\r", string.Empty).Replace("\n", string.Empty).Replace("\t", string.Empty);
                }
            }
            catch { }
            return null;
        }

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
                return HtmlDecodeString(Doc.DocumentNode?.SelectSingleNode("//span[@itemprop='name']")?.InnerText.Trim());
            }
            catch { }
            return null;
        }

        public string GetLinkedInFollowersCount()
        {
            //<p class="followers-count">204 followers</p>
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                return Doc.DocumentNode?.SelectSingleNode("//p[@class='followers-count']")?.InnerText.Trim();
            }
            catch { }
            return null;
        }

        public string GetLinkedInCompanyId()
        {
            //<a class="follow-start" href="https://www.linkedin.com/company/follow/submit?id=999585&amp;fl=start&amp;ft=0_Y3kiyf9HWAmYDU6VAEIvRVAObMxlNM_IGCvo1b5lkvTF_k9D5fA1WjmaCTx-TCZL&amp;csrfToken=ajax%3A8554679308426727204&amp;trk=co_followed-start" role="button">Follow</a>
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                var link = Doc.DocumentNode?.SelectSingleNode("//a[@class='follow-start']")?.GetAttributeValue("href", string.Empty)?.Trim();
                return new Url(link).QueryParams["id"] as string;
            }
            catch { }
            return null;
        }

        public string GetLinkedInCompanyDescription()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                return HtmlDecodeString(Doc.DocumentNode?.SelectSingleNode("//div[@class='basic-info-description']")?.InnerText.Trim());
            }
            catch { }
            return null;
        }

        public dynamic GetLinkedInCompanyAboutSection()
        {
            if (!HasDocument) return null;
            if (HasLoadError) return LoadErrorMessage;
            try
            {
                if (Doc.DocumentNode?.FirstChild == null || !Doc.DocumentNode.HasChildNodes)
                {
                    return null;
                }
                var node = Doc.DocumentNode?.SelectSingleNode("//div[@class='basic-info-about']");
                if (node != null)
                {
                    dynamic rv = new ExpandoObject();

                    //<div class="basic-info-about"><div class="specialties"><h3>Specialties</h3><p>Utility Pole &amp; Attachment Inventory - Software &amp; Services, Joint Use, NESC Inspections, Third Party Joint Use Administration, Resistograph, Strength &amp; Rot Testing - Utility Poles, Central Office, OSP - Network Asset MGMT for Telecom</p></div><ul><li class="website"><h4>Website</h4><p><a href="https://www.linkedin.com/redirect?url=http%3A%2F%2Fwww%2Ealdensys%2Ecom&amp;urlhash=Uc5B" target="_blank" rel="nofollow">http://www.aldensys.com</a></p></li><li class="industry"><h4>Industry</h4><p>Computer Software</p></li><li class="type"><h4>Type</h4><p>Privately Held</p></li><li class="vcard hq"><h4>Headquarters</h4><p class="adr" itemprop="address" itemscope="" itemtype="http://schema.org/PostalAddress"><span class="street-address" itemprop="streetAddress">10 Inverness Center Parkway</span> <span class="street-address" itemprop="streetAddress">Suite 500</span> <span class="locality" itemprop="addressLocality">Birmingham,</span> <abbr class="region" title="AL" itemprop="addressRegion">AL</abbr> <span class="postal-code" itemprop="postalCode">35242</span> <span class="country-name" itemprop="addressCountry">United States</span></p></li><li class="company-size"><h4>Company Size</h4><p>51-200 employees</p></li><li class="founded"><h4>Founded</h4><p>1995</p></li></ul></div>
                    rv.specialties =  HtmlDecodeString(node.SelectSingleNode("//div[@class='basic-info-about']/div[@class='specialties']/p")?.InnerText.Trim());

                    //< li class="website"><h4>Website</h4><p><a href = "https://www.linkedin.com/redirect?url=http%3A%2F%2Fwww%2Eriveroak%2Eorg&amp;urlhash=5A9I" target="_blank" rel="nofollow">http://www.riveroak.org</a></p></li>
                    rv.website = node.SelectSingleNode("//li[@class='website']//a[@href]")?.InnerText;

                    //< li class="industry"><h4>Industry</h4><p>Mental Health Care</p></li>
                    rv.industry = HtmlDecodeString(node.SelectSingleNode("//li[@class='industry']//p")?.InnerText.Trim());

                    //< li class="type"><h4>Type</h4><p>Privately Held</p></li>
                    rv.type = HtmlDecodeString(node.SelectSingleNode("//li[@class='type']//p")?.InnerText.Trim());

                    //< p class="adr" itemprop="address" itemscope="" itemtype="http://schema.org/PostalAddress"><span class="street-address" itemprop="streetAddress">8135 E Indian Bend Rd</span> <span class="street-address" itemprop="streetAddress">#101</span> <span class="locality" itemprop="addressLocality">Scottsdale,</span> <abbr class="region" title="Arizona" itemprop="addressRegion">Arizona</abbr> <span class="postal-code" itemprop="postalCode">85250</span> <span class="country-name" itemprop="addressCountry">United States</span></p>
                    var vcard = node.SelectSingleNode("//p[@class='adr']");
                    if (vcard != null)
                    {
                        var streetAddress = string.Empty;
                        var sn = vcard.SelectNodes("//span[@class='street-address']");
                        if (sn != null) streetAddress = string.Join(" ", sn.Select(x => x.InnerText));
                        rv.streetAddress = HtmlDecodeString(streetAddress);
                        var locality = HtmlDecodeString(vcard.SelectSingleNode("//span[@class='locality']")?.InnerText.Trim());
                        if (!string.IsNullOrWhiteSpace(locality))
                        {
                            rv.locality = locality.EndsWith(",") ? locality.Remove(locality.Length - 1) : locality;
                        }
                        else
                        {
                            rv.locality = null;
                        }
                        rv.region = HtmlDecodeString(vcard.SelectSingleNode("//abbr[@class='region']")?.InnerText.Trim());
                        rv.postalCode = vcard.SelectSingleNode("//span[@class='postal-code']")?.InnerText.Trim();
                        rv.countryName = HtmlDecodeString(vcard.SelectSingleNode("//span[@class='country-name']")?.InnerText.Trim());
                    }
                    else
                    {
                        rv.streetAddress = null;
                        rv.locality = null;
                        rv.region = null;
                        rv.postalCode = null;
                        rv.countryName = null;
                    }

                    //<li class="company-size"><h4>Company Size</h4><p>11-50 employees</p></li>
                    rv.companySize = node.SelectSingleNode("//li[@class='company-size']//p")?.InnerText;

                    //<li class="founded"><h4>Founded</h4><p>1996</p></li>
                    rv.founded = node.SelectSingleNode("//li[@class='founded']//p")?.InnerText;

                    return rv;
                }
            }
            catch { }
            return null;
        }

        #endregion LinkedIn Methods
    }
}