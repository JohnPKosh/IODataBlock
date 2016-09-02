using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebTrakrData.Services;

namespace WebTrakrData.Model.Dto
{
    public class UserLinkedInCompanyDto
    {
        private readonly UserLinkedInCompanyService _userLinkedInCompanyService = new UserLinkedInCompanyService();

        #region Fields and Properties

        public Guid ApiKey { get; set; }

        public long Id { get; set; }

        public long? LinkedInId { get; set; }

        [Required]
        [StringLength(255)]
        public string LinkedInPage { get; set; }

        [StringLength(255)]
        public string LinkedInCompanyName { get; set; }

        [StringLength(255)]
        public string DomainName { get; set; }

        public string specialties { get; set; }

        [StringLength(255)]
        public string streetAddress { get; set; }

        [StringLength(50)]
        public string locality { get; set; }

        [StringLength(50)]
        public string region { get; set; }

        [StringLength(20)]
        public string postalCode { get; set; }

        [StringLength(50)]
        public string countryName { get; set; }

        [StringLength(255)]
        public string website { get; set; }

        [StringLength(100)]
        public string industry { get; set; }

        [StringLength(50)]
        public string type { get; set; }

        [StringLength(25)]
        public string companySize { get; set; }

        [StringLength(25)]
        public string founded { get; set; }

        public int? followersCount { get; set; }

        [StringLength(255)]
        public string photourl { get; set; }

        public string description { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        #endregion

        #region Conversion Operators

        public static implicit operator UserLinkedInCompanyDto(UserLinkedInCompany i)
        {
            try
            {
                return new UserLinkedInCompanyDto()
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
