namespace WebTrakrData.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class WebTrakrModel : DbContext
    {
        public WebTrakrModel()
            : base("name=WebTrakrModel")
        {
        }

        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<LinkedInCompany> LinkedInCompanies { get; set; }
        public virtual DbSet<LinkedInProfile> LinkedInProfiles { get; set; }
        public virtual DbSet<SalesForceAccount> SalesForceAccounts { get; set; }
        public virtual DbSet<UserLinkedInCompany> UserLinkedInCompanies { get; set; }
        public virtual DbSet<UserLinkedInProfile> UserLinkedInProfiles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRole>()
                .HasMany(e => e.AspNetUsers)
                .WithMany(e => e.AspNetRoles)
                .Map(m => m.ToTable("AspNetUserRoles").MapLeftKey("RoleId").MapRightKey("UserId"));

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserClaims)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.AspNetUserLogins)
                .WithRequired(e => e.AspNetUser)
                .HasForeignKey(e => e.UserId);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.SalesForceAccounts)
                .WithRequired(e => e.AspNetUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.UserLinkedInCompanies)
                .WithRequired(e => e.AspNetUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<AspNetUser>()
                .HasMany(e => e.UserLinkedInProfiles)
                .WithRequired(e => e.AspNetUser)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LinkedInCompany>()
                .HasMany(e => e.UserLinkedInCompanies)
                .WithRequired(e => e.LinkedInCompany)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LinkedInProfile>()
                .HasMany(e => e.UserLinkedInProfiles)
                .WithRequired(e => e.LinkedInProfile)
                .WillCascadeOnDelete(false);
        }
    }
}
