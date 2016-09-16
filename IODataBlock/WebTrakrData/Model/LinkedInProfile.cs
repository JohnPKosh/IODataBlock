namespace WebTrakrData.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class LinkedInProfile
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LinkedInProfile()
        {
            UserLinkedInProfiles = new HashSet<UserLinkedInProfile>();
        }

        public long Id { get; set; }

        public long? LinkedInProfileId { get; set; }

        [Required]
        [StringLength(255)]
        public string LinkedInPage { get; set; }

        [StringLength(100)]
        public string LinkedInFullName { get; set; }

        public int? LinkedInConnections { get; set; }

        [StringLength(255)]
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

        [StringLength(255)]
        public string LinkedInPhotoUrl { get; set; }

        public DateTime CreatedDate { get; set; }

        public DateTime UpdatedDate { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserLinkedInProfile> UserLinkedInProfiles { get; set; }
    }
}
