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
        //private readonly UserLinkedInCompanyService _userLinkedInCompanyService = new UserLinkedInCompanyService();

        #region Fields and Properties

        public Guid ApiKey { get; set; }

        public long Id { get; set; }

        public long? LinkedInCompanyId { get; set; }

        [Required]
        [StringLength(255)]
        public string LinkedInCompanyUrl { get; set; }

        [StringLength(255)]
        public string LinkedInCompanyName { get; set; }

        [StringLength(255)]
        public string DomainName { get; set; }

        public string Specialties { get; set; }

        [StringLength(255)]
        public string StreetAddress { get; set; }

        [StringLength(50)]
        public string Locality { get; set; }

        [StringLength(50)]
        public string Region { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }

        [StringLength(50)]
        public string CountryName { get; set; }

        [StringLength(255)]
        public string Website { get; set; }

        [StringLength(100)]
        public string Industry { get; set; }

        [StringLength(50)]
        public string CompanyType { get; set; }

        [StringLength(25)]
        public string CompanySize { get; set; }

        [StringLength(25)]
        public string Founded { get; set; }

        public int? FollowersCount { get; set; }

        [StringLength(255)]
        public string FollowUrl { get; set; }

        [StringLength(255)]
        public string PhotoUrl { get; set; }

        public string CompanyDescription { get; set; }

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
                    FollowUrl = i.LinkedInCompany.FollowUrl,
                    PhotoUrl = i.LinkedInCompany.PhotoUrl,
                    CompanyDescription = i.LinkedInCompany.CompanyDescription,
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
