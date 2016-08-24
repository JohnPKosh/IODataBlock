using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Business.Web.Models;
using Business.Web.Scrape.HtmlReaders;
using Data.DbClient;
using Newtonsoft.Json.Linq;

namespace WebTrackr.Controllers
{
    public class TrakrScrapeController : ApiController
    {

        /* Captures: location, document, links elements of Post */

        ///// <summary>
        ///// Posts the specified value.
        ///// </summary>
        ///// <param name="value">The value.</param>
        ///// <returns>result</returns>
        //[HttpPost]
        //public string Post(JObject value)
        //{
        //    var rv = string.Empty;
        //    JToken locationToken;
        //    if (!value.HasValues || !value.TryGetValue("location", out locationToken)) return null;
        //    var locationUrl = locationToken.Value<string>();
        //    if (locationUrl.Contains("linkedin.com/compan"))
        //    {
        //        var r = new LinkedInCompanyPageReader(value);
        //        // Do something here...

        //        var dto = r.Dto;
        //        if (dto != null)
        //        {
        //            var CompanyId = dto.CompanyId;
        //        }

        //        double companyid = double.Parse(r.ScrapeData.companyid);
        //        var location = r.Location;
        //        var twitterCompanyName = r.ScrapeData.twitterCompanyName;
        //        var domainName = r.ScrapeData.twitterCompanyAbout.website;
        //        var specialties = r.ScrapeData.twitterCompanyAbout.specialties;
        //        var streetAddress = r.ScrapeData.twitterCompanyAbout.streetAddress;
        //        var locality = r.ScrapeData.twitterCompanyAbout.locality;
        //        var region = r.ScrapeData.twitterCompanyAbout.region;
        //        var postalCode = r.ScrapeData.twitterCompanyAbout.postalCode;
        //        var countryName = r.ScrapeData.twitterCompanyAbout.countryName;
        //        var website = r.ScrapeData.twitterCompanyAbout.website;
        //        var industry = r.ScrapeData.twitterCompanyAbout.industry;
        //        var type = r.ScrapeData.twitterCompanyAbout.type;
        //        var companySize = r.ScrapeData.twitterCompanyAbout.companySize;
        //        var founded = r.ScrapeData.twitterCompanyAbout.founded;
        //        var twitterFollowersCount = r.ScrapeData.twitterFollowersCount;
        //        var followers = 0;
        //        if (!string.IsNullOrWhiteSpace(twitterFollowersCount))
        //        {
        //            followers = int.Parse(twitterFollowersCount.Replace("followers", string.Empty).Replace(",", string.Empty).Trim());
        //        }
        //        var twitterPhotoUrl = r.ScrapeData.twitterPhotoUrl;
        //        var twitterCompanyDescription = r.ScrapeData.twitterCompanyDescription;

        //        var curls = r.ScrapeData.companyUrls;
        //        foreach (var c in curls)
        //        {
        //            var link = c;
        //        }

        //        SaveLinkedInCompanyResult(companyid, location, twitterCompanyName, domainName, specialties,
        //            streetAddress, locality, region, postalCode, countryName,
        //            website, industry, type, companySize, founded, followers, twitterPhotoUrl,
        //            twitterCompanyDescription, DateTime.Now, Guid.NewGuid().ToString());

        //        return r.ScrapeData.companyid;
        //    }
        //    return null;
        //}


        /// <summary>
        /// Posts the specified value.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>result</returns>
        [HttpPost]
        public LinkedInScrapeDto Post(JObject value)
        {
            var rv = string.Empty;
            JToken locationToken;
            if (!value.HasValues || !value.TryGetValue("location", out locationToken)) return null;
            var locationUrl = locationToken.Value<string>();
            if (locationUrl.Contains("linkedin.com/compan"))
            {
                var r = new LinkedInCompanyPageReader(value);
                return SaveLinkedInCompanyDto(r.Dto);
            }
            if (locationUrl.Contains("linkedin.com/in/"))
            {
                var r = new LinkedInCompanyPageReader(value);
                return SaveLinkedInProfileDto(r.Dto);
            }
            return null;
        }

        private LinkedInScrapeDto SaveLinkedInProfileDto(LinkedInScrapeDto dto)
        {
            throw new NotImplementedException();
        }

        private LinkedInScrapeDto SaveLinkedInCompanyDto(LinkedInScrapeDto dto)
        {
            if (dto != null)
            {
                SaveLinkedInCompany(
                    dto.CompanyId,
                    dto.LocationUrl,
                    dto.CompanyName,
                    dto.DomainName,
                    dto.Specialties,
                    dto.StreetAddress,
                    dto.Locality,
                    dto.Region,
                    dto.PostalCode,
                    dto.CountryName,
                    dto.Website,
                    dto.Industry,
                    dto.Type,
                    dto.CompanySize,
                    dto.Founded,
                    dto.Followers,
                    dto.PhotoUrl,
                    dto.CompanyDescription,
                    DateTime.Now,
                    Guid.NewGuid().ToString());
            }
            return dto;
        }

        private void SaveLinkedInCompany(double linkedinId, string link, string companyName, string domainName, string specialties, string streetAddress, string locality,
            string region, string postalCode, string countryName, string website, string industry, string type, string companySize, string founded, int followersCount,
            string photourl, string description, DateTime createdDate, string batchId)
        {
            var ConnectionString = DataExtensionBase.GetSqlConnectionString(@"(local)\EXP14", "TestData");

            if (link.StartsWith("http://")) link = link.Substring(7);

            #region Sql

            var sql = @"

                        DELETE FROM [dbo].[LinkedInCompany] WHERE [LinkedInId] = @0

                        INSERT INTO [dbo].[LinkedInCompany]
                        ([LinkedInId]
                        ,[LinkedInPage]
                        ,[LinkedInCompanyName]
                        ,[DomainName]
                        ,[specialties]
                        ,[streetAddress]
                        ,[locality]
                        ,[region]
                        ,[postalCode]
                        ,[countryName]
                        ,[website]
                        ,[industry]
                        ,[type]
                        ,[companySize]
                        ,[founded]
                        ,[followersCount]
                        ,[photourl]
                        ,[description]
                        ,[CreatedDate]
                        ,[BatchId])
                     VALUES
                        (@0
                        ,@1
                        ,@2
                        ,@3
                        ,@4
                        ,@5
                        ,@6
                        ,@7
                        ,@8
                        ,@9
                        ,@10
                        ,@11
                        ,@12
                        ,@13
                        ,@14
                        ,@15
                        ,@16
                        ,@17
                        ,@18
                        ,@19
                        )		    
                    ";

            #endregion Sql

            try
            {
                using (var db = Database.OpenConnectionString(ConnectionString, "System.Data.SqlClient"))
                {
                    db.Execute(sql, 300, linkedinId, link, companyName, domainName, specialties, streetAddress, locality, region, postalCode, countryName, website, industry, type, companySize, founded, followersCount, photourl, description, createdDate, batchId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

    }
}
