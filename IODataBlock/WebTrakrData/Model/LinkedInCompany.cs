namespace WebTrakrData.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LinkedInCompany
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LinkedInCompany()
        {
            UserLinkedInCompanies = new HashSet<UserLinkedInCompany>();
        }

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLinkedInCompany> UserLinkedInCompanies { get; set; }
    }
}
