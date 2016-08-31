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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLinkedInCompany> UserLinkedInCompanies { get; set; }
    }
}
