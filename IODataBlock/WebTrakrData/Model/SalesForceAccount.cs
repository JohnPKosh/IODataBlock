namespace WebTrakrData.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class SalesForceAccount
    {
        public int Id { get; set; }

        [StringLength(128)]
        public string SfClientId { get; set; }

        [StringLength(50)]
        public string SfClientSecret { get; set; }

        [StringLength(50)]
        public string SfUsername { get; set; }

        [StringLength(50)]
        public string SfPassword { get; set; }

        [Required]
        [StringLength(128)]
        public string AspNetUserId { get; set; }

        public virtual AspNetUser AspNetUser { get; set; }
    }
}
