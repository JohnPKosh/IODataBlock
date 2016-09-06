using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebTrakrData.Model.Dto
{
    public class UserLinkedeInProfileDto
    {
        #region Fields and Properties

        public Guid ApiKey { get; set; }

        public long Id { get; set; }

        public long? LinkedInProfileId { get; set; }

        [Required]
        [StringLength(255)]
        public string LinkedInPage { get; set; }

        [StringLength(100)]
        public string LinkedInFullName { get; set; }

        public int? LinkedInConnections { get; set; }

        [StringLength(100)]
        public string LinkedInTitle { get; set; }

        [StringLength(100)]
        public string LinkedInCompanyName { get; set; }

        public long? LinkedInCompanyId { get; set; }

        [StringLength(100)]
        public string Industry { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(255)]
        public string IM { get; set; }

        [StringLength(255)]
        public string Twitter { get; set; }

        [StringLength(255)]
        public string Address { get; set; }

        public string Phone { get; set; }

        [Required]
        [StringLength(255)]
        public string LinkedInPhotoUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }
        #endregion


        #region Conversion Operators

        public static implicit operator UserLinkedeInProfileDto(UserLinkedInProfile i)
        {
            try
            {
                return new UserLinkedeInProfileDto()
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
                };
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion Conversion Operators
    }
}
