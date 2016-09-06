namespace WebTrakrData.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class UserLinkedInProfile
    {
        public long Id { get; set; }

        public long LinkedInProfileId { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUserId { get; set; }

        public DateTime CreatedDate { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }

        public virtual LinkedInProfile LinkedInProfile { get; set; }
    }
}
