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
                LinkedInId = i.LinkedInCompany.LinkedInId,
                LinkedInPage = i.LinkedInCompany.LinkedInPage,
                LinkedInCompanyName = i.LinkedInCompany.LinkedInCompanyName,
                DomainName = i.LinkedInCompany.DomainName,
                specialties = i.LinkedInCompany.specialties,
                streetAddress = i.LinkedInCompany.streetAddress,
                locality = i.LinkedInCompany.locality,
                region = i.LinkedInCompany.region,
                postalCode = i.LinkedInCompany.postalCode,
                countryName = i.LinkedInCompany.countryName,
                website = i.LinkedInCompany.website,
                industry = i.LinkedInCompany.industry,
                type = i.LinkedInCompany.type,
                companySize = i.LinkedInCompany.companySize,
                founded = i.LinkedInCompany.founded,
                followersCount = i.LinkedInCompany.followersCount,
                photourl = i.LinkedInCompany.photourl,
                description = i.LinkedInCompany.description,
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
                var rv = dbModel.UserLinkedInCompanies.FirstOrDefault(x => x.AspNetUserId.ToString() == apikey && x.LinkedInCompany.LinkedInId == id);
                return rv;
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}