namespace WebTrakrData.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WebTrakrModel : DbContext
    {
        public WebTrakrModel()
            : base("name=WebTrakr")
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<LinkedInCompany> LinkedInCompanies { get; set; }
        public virtual DbSet<LinkedInProfile> LinkedInProfiles { get; set; }
        public virtual DbSet<LinkedInCompany1> LinkedInCompanies1 { get; set; }
        public virtual DbSet<LinkedInProfile1> LinkedInProfiles1 { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("AspNetRoles_Id").MapRightKey("AspNetUsers_Id"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.LinkedInCompanies)
                .WithRequired(e => e.AspNetUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.LinkedInProfiles)
                .WithRequired(e => e.AspNetUser)
                .WillCascadeOnDelete(false);
        }
    }
}
