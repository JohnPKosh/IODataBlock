using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTrakrData.Model;
using WebTrakrData.Model.Dto;

namespace WebTrakrData.Services
{
    public class UserLinkedInProfileService
    {

        public IQueryable<UserLinkedeInProfileDto> GetUserLinkedInCompanyDto()
        {
            var dbModel = new WebTrakrModel();
            var items = dbModel.UserLinkedInProfiles.Select(i => new UserLinkedeInProfileDto()
            {
                ApiKey = i.AspNetUser.ApiKey,
                Id = i.Id,
                LinkedInProfileId = i.LinkedInProfile.LinkedInProfileId,
                LinkedInPage = i.LinkedInProfile.LinkedInPage,
                LinkedInFullName = i.LinkedInProfile.LinkedInFullName,
                LinkedInConnections = i.LinkedInProfile.LinkedInConnections,
                LinkedInTitle = i.LinkedInProfile.LinkedInTitle,
                LinkedInCompanyName = i.LinkedInProfile.LinkedInCompanyName,
                LinkedInCompanyId = i.LinkedInProfile.LinkedInCompanyId,
                Industry = i.LinkedInProfile.Industry,
                Location = i.LinkedInProfile.Location,
                Email = i.LinkedInProfile.Email,
                IM = i.LinkedInProfile.IM,
                Twitter = i.LinkedInProfile.Twitter,
                Address = i.LinkedInProfile.Address,
                Phone = i.LinkedInProfile.Phone,
                LinkedInPhotoUrl = i.LinkedInProfile.LinkedInPhotoUrl,
                CreatedDate = i.LinkedInProfile.CreatedDate,
                UpdatedDate = i.LinkedInProfile.UpdatedDate
            });

            return items;
        }

        public UserLinkedeInProfileDto GetByLinkedInProfileId(long id, string apikey)
        {
            try
            {
                var dbModel = new WebTrakrModel();
                var rv = dbModel.UserLinkedInProfiles.FirstOrDefault(x => x.AspNetUserId.ToString() == apikey && x.LinkedInProfile.LinkedInProfileId == id);
                return rv;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public IQueryable<UserLinkedeInProfileDto> GetByLinkedInCompanyId(long id, string apikey)
        {
            try
            {
                var dbModel = new WebTrakrModel();
                var rv = dbModel.UserLinkedInProfiles.Where(x => x.AspNetUserId.ToString() == apikey && x.LinkedInProfile.LinkedInCompanyId == id).Select(i => new UserLinkedeInProfileDto()
                {
                    ApiKey = i.AspNetUser.ApiKey,
                    Id = i.Id,
                    LinkedInProfileId = i.LinkedInProfile.LinkedInProfileId,
                    LinkedInPage = i.LinkedInProfile.LinkedInPage,
                    LinkedInFullName = i.LinkedInProfile.LinkedInFullName,
                    LinkedInConnections = i.LinkedInProfile.LinkedInConnections,
                    LinkedInTitle = i.LinkedInProfile.LinkedInTitle,
                    LinkedInCompanyName = i.LinkedInProfile.LinkedInCompanyName,
                    LinkedInCompanyId = i.LinkedInProfile.LinkedInCompanyId,
                    Industry = i.LinkedInProfile.Industry,
                    Location = i.LinkedInProfile.Location,
                    Email = i.LinkedInProfile.Email,
                    IM = i.LinkedInProfile.IM,
                    Twitter = i.LinkedInProfile.Twitter,
                    Address = i.LinkedInProfile.Address,
                    Phone = i.LinkedInProfile.Phone,
                    LinkedInPhotoUrl = i.LinkedInProfile.LinkedInPhotoUrl,
                    CreatedDate = i.LinkedInProfile.CreatedDate,
                    UpdatedDate = i.LinkedInProfile.UpdatedDate
                });
                return rv;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }
}
