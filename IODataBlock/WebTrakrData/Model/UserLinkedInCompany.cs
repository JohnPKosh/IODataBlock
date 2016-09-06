namespace WebTrakrData.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserLinkedInCompany
    {
        public long Id { get; set; }

        public long LinkedInCompanyId { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual LinkedInCompany LinkedInCompany { get; set; }
    }
}
