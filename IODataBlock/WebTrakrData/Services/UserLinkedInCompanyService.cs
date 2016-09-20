using System;
using System.Linq;
using WebTrakrData.Model;
using WebTrakrData.Model.Dto;

namespace WebTrakrData.Services
{
    public class UserLinkedInCompanyService
    {
        public IQueryable<UserLinkedInCompanyDto> GetUserLinkedInCompanyDto()
        {
            var dbModel = new WebTrakrModel();
            var items = dbModel.UserLinkedInCompanies.Select(i => new UserLinkedInCompanyDto()
            {
                ApiKey = i.AspNetUser.ApiKey,
                Id = i.Id,
                LinkedInCompanyId = i.LinkedInCompany.LinkedInCompanyId,
                LinkedInCompanyUrl = i.LinkedInCompany.LinkedInCompanyUrl,
                LinkedInCompanyName = i.LinkedInCompany.LinkedInCompanyName,
                DomainName = i.LinkedInCompany.DomainName,
                Specialties = i.LinkedInCompany.Specialties,
                StreetAddress = i.LinkedInCompany.StreetAddress,
                Locality = i.LinkedInCompany.Locality,
                Region = i.LinkedInCompany.Region,
                PostalCode = i.LinkedInCompany.PostalCode,
                CountryName = i.LinkedInCompany.CountryName,
                Website = i.LinkedInCompany.Website,
                Industry = i.LinkedInCompany.Industry,
                CompanyType = i.LinkedInCompany.CompanyType,
                CompanySize = i.LinkedInCompany.CompanySize,
                Founded = i.LinkedInCompany.Founded,
                FollowersCount = i.LinkedInCompany.FollowersCount,
                PhotoUrl = i.LinkedInCompany.PhotoUrl,
                CompanyDescription = i.LinkedInCompany.CompanyDescription,
                CreatedDate = i.LinkedInCompany.CreatedDate,
                UpdatedDate = i.LinkedInCompany.UpdatedDate
            });

            return items;
        }

        public UserLinkedInCompanyDto GetByLinkedInCompanyId(long id, string apikey)
        {
            try
            {
                var dbModel = new WebTrakrModel();
                var rv = dbModel.UserLinkedInCompanies.FirstOrDefault(x => x.AspNetUserId.ToString() == apikey && x.LinkedInCompany.LinkedInCompanyId == id);
                return rv;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}