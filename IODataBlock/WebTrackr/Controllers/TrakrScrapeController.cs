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

        [HttpGet]
        public object Get(string locationUrl, string apikey)
        {
            if (locationUrl.Contains("linkedin.com/compan"))
            {
                var r = new LinkedInCompanyPageReader(value);
                return SaveLinkedInCompanyDto(r.Dto);
            }
            if (locationUrl.Contains("linkedin.com/in/"))
            {
                var r = new LinkedInProfilePageReader(value);
                return SaveLinkedInProfileDto(r.Dto);
            }
        }

        /// <summary>
        /// Posts the specified scraped page value.
        /// </summary>
        /// <param name="value">The location, document, links elements of the page being scraped.</param>
        /// <returns>result</returns>
        [HttpPost]
        public object Post(JObject value)
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
                var r = new LinkedInProfilePageReader(value);
                return SaveLinkedInProfileDto(r.Dto);
            }
            return null;
        }

        #region LinkedIn Profile Methods

        private LinkedInProfileScrapeDto SaveLinkedInProfileDto(LinkedInProfileScrapeDto dto)
        {
            if (dto != null)
            {
                SaveLinkedInProfile(
                    dto.ProfileId,
                    dto.ProfileUrl,
                    dto.FullName,
                    dto.Connections,
                    dto.Title,
                    dto.CompanyName,
                    dto.CompanyId,
                    dto.Industry,
                    dto.Location,
                    dto.Email,
                    dto.Im,
                    dto.Twitter,
                    dto.Address,
                    dto.Phone,
                    dto.PhotoUrl,
                    DateTime.Now,
                    Guid.NewGuid().ToString());
            }
            return dto;
        }


        private void SaveLinkedInProfile(double linkedinId, string link, string fullName, int connections, string title, string companyName, double companyId, string industry,
            string location, string email, string im, string twitter, string address, string phone, string photoUrl, DateTime createdDate, string batchId)
        {
            var ConnectionString = DataExtensionBase.GetSqlConnectionString(@"(local)\EXP14", "TestData");

            if (link.StartsWith("http://")) link = link.Substring(7);

            #region Sql

            var sql = @"

                    DELETE FROM [dbo].[LinkedInProfile] WHERE [LinkedInProfileId] = @0

                    INSERT INTO [dbo].[LinkedInProfile]
                    ([LinkedInProfileId]
                    ,[LinkedInPage]
                    ,[LinkedInFullName]
                    ,[LinkedInConnections]
                    ,[LinkedInTitle]
                    ,[LinkedInCompanyName]
                    ,[LinkedInCompanyId]
                    ,[Industry]
                    ,[Location]
                    ,[Email]
                    ,[IM]
                    ,[Twitter]
                    ,[Address]
                    ,[Phone]
                    ,[LinkedInPhotoUrl]
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
                        )		    
                    ";

            #endregion Sql

            try
            {
                using (var db = Database.OpenConnectionString(ConnectionString, "System.Data.SqlClient"))
                {
                    db.Execute(sql, 300, linkedinId, link, fullName, connections, title, companyName, companyId, industry, location, email, im, twitter, address, phone, photoUrl, createdDate, batchId);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        #endregion

        #region LinkedIn Company Methods

        private LinkedInCompanyScrapeDto SaveLinkedInCompanyDto(LinkedInCompanyScrapeDto dto)
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


        //public LinkedInCompanyScrapeDto FindLinkedInCompanyByLocation(string LocationUrl, string apikey)
        //{
        //    var ConnectionString = DataExtensionBase.GetSqlConnectionString(@"(local)\EXP14", "TestData");

        //    if (LocationUrl.StartsWith("http://")) LocationUrl = LocationUrl.Substring(7);

        //    #region Sql

        //    var sql = @"

        //                DELETE FROM [dbo].[LinkedInCompany] WHERE [LinkedInId] = @0

        //                INSERT INTO [dbo].[LinkedInCompany]
        //                ([LinkedInId]
        //                ,[LinkedInPage]
        //                ,[LinkedInCompanyName]
        //                ,[DomainName]
        //                ,[specialties]
        //                ,[streetAddress]
        //                ,[locality]
        //                ,[region]
        //                ,[postalCode]
        //                ,[countryName]
        //                ,[website]
        //                ,[industry]
        //                ,[type]
        //                ,[companySize]
        //                ,[founded]
        //                ,[followersCount]
        //                ,[photourl]
        //                ,[description]
        //                ,[CreatedDate]
        //                ,[BatchId])
        //             VALUES
        //                (@0
        //                ,@1
        //                ,@2
        //                ,@3
        //                ,@4
        //                ,@5
        //                ,@6
        //                ,@7
        //                ,@8
        //                ,@9
        //                ,@10
        //                ,@11
        //                ,@12
        //                ,@13
        //                ,@14
        //                ,@15
        //                ,@16
        //                ,@17
        //                ,@18
        //                ,@19
        //                )		    
        //            ";

        //    #endregion Sql

        //    try
        //    {
        //        using (var db = Database.OpenConnectionString(ConnectionString, "System.Data.SqlClient"))
        //        {
        //            var rv = db.QuerySingle()
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //    }
        //}

        #endregion

    }
}
