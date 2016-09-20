using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Business.Web.Models;
using Business.Web.Scrape.HtmlReaders;
using Business.Web.Scrape.Services;
using Data.DbClient;
using Newtonsoft.Json.Linq;
using WebTrakrData.Model;
using WebTrakrData.Model.Dto;
using WebTrakrData.Services;

namespace WebTrackr.Controllers
{
    public class TrakrScrapeController : ApiController
    {

        /// <summary>
        /// Gets the specified location URL.
        /// </summary>
        /// <param name="locationUrl">The location URL.</param>
        /// <param name="apikey">The apikey.</param>
        /// <returns></returns>
        [HttpGet]
        public object Get(string locationUrl, string apikey)
        {
            var dbModel = new WebTrakrModel();

            if (locationUrl.Contains("linkedin.com/compan"))
            {
                var util = new LinkedInCompanyHtml(locationUrl);
                var id = long.Parse(util.GetLinkedInCompanyId());
                return dbModel.UserLinkedInCompanies.FirstOrDefault(x => x.AspNetUser.ApiKey.ToString() == apikey && x.LinkedInCompany.LinkedInCompanyId == id);
            }
            //if (locationUrl.Contains("linkedin.com/in/"))
            //{
            //    return
            //        dbModel.LinkedInProfiles.FirstOrDefault(
            //            x => x.AspNetUser.ApiKey.ToString() == apikey && x.LinkedInPage == locationUrl);
            //}
            else return null;
        }

        /// <summary>
        /// Gets the linked in company.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="apikey">The apikey.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TrakrScrape/LinkedInCompany/{id:long}/{apikey}")]
        public UserLinkedInCompanyDto GetLinkedInCompany(long id, string apikey)
        {
            try
            {
                var svc = new UserLinkedInCompanyService();
                return svc.GetByLinkedInCompanyId(id, apikey);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Gets the linked in company employee profiles.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="apikey"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/TrakrScrape/LinkedInCompanyProfiles/{id:long}/{apikey}")]
        public List<UserLinkedeInProfileDto> GetLinkedInCompanyProfiles(long id, string apikey)
        {
            try
            {
                var svc = new UserLinkedInProfileService();
                var rv = svc.GetByLinkedInCompanyId(id, apikey).ToList();
                return rv;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        /// <summary>
        /// Posts the specified scraped page value.
        /// </summary>
        /// <param name="value">The location, document, links elements, and APIKEY of the page being scraped.</param>
        /// <returns>result</returns>
        //[HttpPost]
        //public object Post(JObject value)
        //{
        //    var rv = string.Empty;
        //    JToken locationToken;
        //    if (!value.HasValues || !value.TryGetValue("location", out locationToken)) return null;
        //    var locationUrl = locationToken.Value<string>();
            
        //    if (locationUrl.Contains("linkedin.com/compan"))
        //    {
        //        var r = new LinkedInCompanyPageReader(value);
        //        return SaveLinkedInCompany(r.Dto, r.ApiKey);
        //    }
        //    if (locationUrl.Contains("linkedin.com/in/"))
        //    {
        //        var r = new LinkedInProfilePageReader(value);
        //        return SaveLinkedInProfile(r.Dto, r.ApiKey);
        //    }
        //    return null;
        //}

        [HttpPost]
        //[Route("api/TrakrScrape/LinkedInCompanyProfiles")]
        public object Post(JObject value)
        {
            var rv = string.Empty;
            JToken locationToken;
            if (!value.HasValues || !value.TryGetValue("location", out locationToken)) return null;
            var locationUrl = locationToken.Value<string>();

            if (locationUrl.Contains("linkedin.com/compan"))
            {
                var r = new LinkedInCompanyPageReader(value);
                return SaveLinkedInCompany(r.Dto, r.ApiKey);
            }
            if (locationUrl.Contains("linkedin.com/in/"))
            {
                var r = new LinkedInProfilePageReader(value);
                return SaveLinkedInProfile(r.Dto, r.ApiKey);
            }
            return null;
        }

        #region LinkedIn Profile Methods

        private LinkedInProfile SaveLinkedInProfile(LinkedInProfileScrapeDto dto, string apikey)
        {
            var dbModel = new WebTrakrModel();
            var user = dbModel.AspNetUsers.FirstOrDefault(x => x.ApiKey == new Guid(apikey));
            var existing = dbModel.LinkedInProfiles.FirstOrDefault(x => x.LinkedInProfileId == dto.LinkedInProfileId);
            if (existing == null)
            {
                try
                {
                    existing = new LinkedInProfile()
                    {
                        //AspNetUser_Id = user.Id,
                        LinkedInProfileId = dto.LinkedInProfileId,
                        LinkedInPage = dto.LinkedInPage,
                        LinkedInFullName = dto.LinkedInFullName,
                        LinkedInConnections = dto.LinkedInConnections,
                        LinkedInTitle = dto.LinkedInTitle,
                        LinkedInCompanyName = dto.LinkedInCompanyName,
                        LinkedInCompanyId = dto.LinkedInCompanyId,
                        Industry = dto.Industry,
                        Location = dto.Location,
                        Email = dto.Email,
                        IM = dto.Im,
                        Twitter = dto.Twitter,
                        Address = dto.Address,
                        Phone = dto.Phone,
                        LinkedInPhotoUrl = dto.LinkedInPhotoUrl,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now
                    };

                    dbModel.LinkedInProfiles.Add(existing);
                    dbModel.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }
            }
            else
            {
                if (existing.UpdatedDate.AddDays(1) < DateTime.Now)
                {
                    existing.LinkedInProfileId = dto.LinkedInProfileId;
                    existing.LinkedInPage = dto.LinkedInPage;
                    existing.LinkedInFullName = dto.LinkedInFullName;
                    existing.LinkedInConnections = dto.LinkedInConnections;
                    existing.LinkedInTitle = dto.LinkedInTitle;
                    existing.LinkedInCompanyName = dto.LinkedInCompanyName;
                    existing.LinkedInCompanyId = dto.LinkedInCompanyId;
                    existing.Industry = dto.Industry;
                    existing.Location = dto.Location;
                    existing.Email = dto.Email;
                    existing.IM = dto.Im;
                    existing.Twitter = dto.Twitter;
                    existing.Address = dto.Address;
                    existing.Phone = dto.Phone;
                    existing.LinkedInPhotoUrl = dto.LinkedInPhotoUrl;
                    existing.UpdatedDate = DateTime.Now;
                    dbModel.SaveChanges();
                }
                if (existing.UserLinkedInProfiles.Any(x => x.AspNetUserId == user.Id)) return existing;
            }
            existing.UserLinkedInProfiles.Add(new UserLinkedInProfile() { AspNetUser = user, LinkedInProfileId = existing.Id, CreatedDate = DateTime.Now });
            dbModel.SaveChanges();
            return existing;
        }

        #endregion

        #region LinkedIn Company Methods

        private LinkedInCompany SaveLinkedInCompany(LinkedInCompanyScrapeDto dto, string apikey)
        {
            var dbModel = new WebTrakrModel();
            var user = dbModel.AspNetUsers.FirstOrDefault(x => x.ApiKey == new Guid(apikey));
            var existing = dbModel.LinkedInCompanies.FirstOrDefault(x => x.LinkedInCompanyId == dto.LinkedInCompanyId);
            if (existing == null)
            {
                try
                {
                    existing = new LinkedInCompany()
                    {
                        //AspNetUser_Id = user.Id,
                        CompanySize = dto.CompanySize,
                        CountryName = dto.CountryName,
                        CreatedDate = DateTime.Now,
                        UpdatedDate = DateTime.Now,
                        CompanyDescription = dto.CompanyDescription,
                        DomainName = dto.DomainName,
                        FollowersCount = dto.FollowersCount,
                        FollowUrl = dto.FollowUrl,
                        Founded = dto.Founded,
                        Industry = dto.Industry,
                        CompanyType = dto.CompanyType,
                        PhotoUrl = dto.PhotoUrl,
                        PostalCode = dto.PostalCode,
                        Locality = dto.Locality,
                        Website = dto.Website,
                        LinkedInCompanyUrl = dto.LinkedInCompanyUrl,
                        Region = dto.Region,
                        StreetAddress = dto.StreetAddress,
                        Specialties = dto.Specialties,
                        LinkedInCompanyName = dto.LinkedInCompanyName,
                        LinkedInCompanyId = dto.LinkedInCompanyId
                    };

                    dbModel.LinkedInCompanies.Add(existing);
                    dbModel.SaveChanges();
                }
                catch (Exception ex)
                {

                    throw;
                }

            }
            else
            {
                if (existing.UpdatedDate.AddDays(1) < DateTime.Now)
                {
                    existing.CompanySize = dto.CompanySize;
                    existing.CountryName = dto.CountryName;
                    existing.UpdatedDate = DateTime.Now;
                    existing.CompanyDescription = dto.CompanyDescription;
                    existing.DomainName = dto.DomainName;
                    existing.FollowersCount = dto.FollowersCount;
                    existing.FollowUrl = dto.FollowUrl;
                    existing.Founded = dto.Founded;
                    existing.Industry = dto.Industry;
                    existing.CompanyType = dto.CompanyType;
                    existing.PhotoUrl = dto.PhotoUrl;
                    existing.PostalCode = dto.PostalCode;
                    existing.Locality = dto.Locality;
                    existing.Website = dto.Website;
                    existing.LinkedInCompanyUrl = dto.LinkedInCompanyUrl;
                    existing.Region = dto.Region;
                    existing.StreetAddress = dto.StreetAddress;
                    existing.Specialties = dto.Specialties;
                    existing.LinkedInCompanyName = dto.LinkedInCompanyName;
                    existing.LinkedInCompanyId = dto.LinkedInCompanyId;
                    dbModel.SaveChanges();
                }
                if (existing.UserLinkedInCompanies.Any(x => x.AspNetUserId == user.Id)) return existing;
            }
            existing.UserLinkedInCompanies.Add(new UserLinkedInCompany() { AspNetUser = user, LinkedInCompanyId = existing.Id, CreatedDate = DateTime.Now });
            dbModel.SaveChanges();
            return existing;
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
