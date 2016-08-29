namespace WebTrakrData.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LinkedInCompany")]
    public partial class LinkedInCompany1
    {
        [Key]
        [Column(Order = 0)]
        public long Id { get; set; }

        public long? LinkedInId { get; set; }

        [Key]
        [Column(Order = 1)]
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

        [Key]
        [Column(Order = 2)]
        public DateTime CreatedDate { get; set; }

        [Key]
        [Column(Order = 3)]
        [StringLength(36)]
        public string AspNetUsers_Id { get; set; }
    }
}
