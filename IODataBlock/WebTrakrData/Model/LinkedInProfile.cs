namespace WebTrakrData.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LinkedInProfile
    {
        [Key]
        [Column(Order = 0)]
        public long Id { get; set; }

        public long? LinkedInProfileId { get; set; }

        [Key]
        [Column(Order = 1)]
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

        [Key]
        [Column(Order = 2)]
        [StringLength(255)]
        public string LinkedInPhotoUrl { get; set; }

        [Key]
        [Column(Order = 3)]
        public DateTime CreatedDate { get; set; }

        [Key]
        [Column(Order = 4)]
        [StringLength(36)]
        public string AspNetUsers_Id { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUserId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
