﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebTrakrData.EF
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class WebTrakrEntities : DbContext
    {
        public WebTrakrEntities()
            : base("name=WebTrakrEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<LinkedInCompany> LinkedInCompanies { get; set; }
        public virtual DbSet<LinkedInProfile> LinkedInProfiles { get; set; }
        public virtual DbSet<SalesForceAccount> SalesForceAccounts { get; set; }
        public virtual DbSet<UserLinkedInCompany> UserLinkedInCompanies { get; set; }
        public virtual DbSet<UserLinkedInProfile> UserLinkedInProfiles { get; set; }
    }
}
