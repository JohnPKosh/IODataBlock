
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 08/30/2016 09:31:00
-- Generated from EDMX file: C:\Users\jkosh\Source\Repos\IODataBlock\IODataBlock\WebTrakrData\EF\WebTrakrModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [WebTrakr];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AspNetUserClaims_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserClaims] DROP CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserLogins_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserLogins] DROP CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserRoles_AspNetRoles_RoleId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId];
GO
IF OBJECT_ID(N'[dbo].[FK_AspNetUserSalesForceAccount]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesForceAccount] DROP CONSTRAINT [FK_AspNetUserSalesForceAccount];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AspNetUserRoles] DROP CONSTRAINT [FK_dbo_AspNetUserRoles_dbo_AspNetUsers_UserId];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AspNetRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserClaims]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserClaims];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserLogins]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserLogins];
GO
IF OBJECT_ID(N'[dbo].[AspNetUserRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUserRoles];
GO
IF OBJECT_ID(N'[dbo].[AspNetUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AspNetUsers];
GO
IF OBJECT_ID(N'[dbo].[LinkedInCompany]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LinkedInCompany];
GO
IF OBJECT_ID(N'[dbo].[LinkedInProfile]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LinkedInProfile];
GO
IF OBJECT_ID(N'[dbo].[SalesForceAccount]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesForceAccount];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [ApiKey] uniqueidentifier  NOT NULL,
    [AccountNumber] int  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'LinkedInCompanies'
CREATE TABLE [dbo].[LinkedInCompanies] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [LinkedInId] bigint  NULL,
    [LinkedInPage] nvarchar(255)  NOT NULL,
    [LinkedInCompanyName] nvarchar(255)  NULL,
    [DomainName] nvarchar(255)  NULL,
    [specialties] nvarchar(max)  NULL,
    [streetAddress] nvarchar(255)  NULL,
    [locality] nvarchar(50)  NULL,
    [region] nvarchar(50)  NULL,
    [postalCode] nvarchar(20)  NULL,
    [countryName] nvarchar(50)  NULL,
    [website] nvarchar(255)  NULL,
    [industry] nvarchar(100)  NULL,
    [type] nvarchar(50)  NULL,
    [companySize] nvarchar(25)  NULL,
    [founded] nvarchar(25)  NULL,
    [followersCount] int  NULL,
    [photourl] nvarchar(255)  NULL,
    [description] nvarchar(max)  NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedDate] datetime  NOT NULL
);
GO

-- Creating table 'LinkedInProfiles'
CREATE TABLE [dbo].[LinkedInProfiles] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [LinkedInProfileId] bigint  NULL,
    [LinkedInPage] nvarchar(255)  NOT NULL,
    [LinkedInFullName] nvarchar(100)  NULL,
    [LinkedInConnections] int  NULL,
    [LinkedInTitle] nvarchar(100)  NULL,
    [LinkedInCompanyName] nvarchar(100)  NULL,
    [LinkedInCompanyId] bigint  NULL,
    [Industry] nvarchar(100)  NULL,
    [Location] nvarchar(100)  NULL,
    [Email] nvarchar(255)  NULL,
    [IM] nvarchar(255)  NULL,
    [Twitter] nvarchar(255)  NULL,
    [Address] nvarchar(255)  NULL,
    [Phone] nvarchar(max)  NULL,
    [LinkedInPhotoUrl] nvarchar(255)  NOT NULL,
    [CreatedDate] datetime  NOT NULL,
    [UpdatedDate] datetime  NOT NULL
);
GO

-- Creating table 'SalesForceAccounts'
CREATE TABLE [dbo].[SalesForceAccounts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SfClientId] nvarchar(128)  NULL,
    [SfClientSecret] nvarchar(50)  NULL,
    [SfUsername] nvarchar(50)  NULL,
    [SfPassword] nvarchar(50)  NULL,
    [AspNetUserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'UserLinkedInCompanies'
CREATE TABLE [dbo].[UserLinkedInCompanies] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [LinkedInCompanyId] bigint  NOT NULL,
    [AspNetUserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'UserLinkedInProfiles'
CREATE TABLE [dbo].[UserLinkedInProfiles] (
    [Id] bigint IDENTITY(1,1) NOT NULL,
    [LinkedInProfileId] bigint  NOT NULL,
    [AspNetUserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LinkedInCompanies'
ALTER TABLE [dbo].[LinkedInCompanies]
ADD CONSTRAINT [PK_LinkedInCompanies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LinkedInProfiles'
ALTER TABLE [dbo].[LinkedInProfiles]
ADD CONSTRAINT [PK_LinkedInProfiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesForceAccounts'
ALTER TABLE [dbo].[SalesForceAccounts]
ADD CONSTRAINT [PK_SalesForceAccounts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserLinkedInCompanies'
ALTER TABLE [dbo].[UserLinkedInCompanies]
ADD CONSTRAINT [PK_UserLinkedInCompanies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserLinkedInProfiles'
ALTER TABLE [dbo].[UserLinkedInProfiles]
ADD CONSTRAINT [PK_UserLinkedInProfiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserClaims_AspNetUsers_UserId'
CREATE INDEX [IX_FK_AspNetUserClaims_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserLogins_AspNetUsers_UserId'
CREATE INDEX [IX_FK_AspNetUserLogins_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [AspNetUserId] in table 'SalesForceAccounts'
ALTER TABLE [dbo].[SalesForceAccounts]
ADD CONSTRAINT [FK_AspNetUserSalesForceAccount]
    FOREIGN KEY ([AspNetUserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserSalesForceAccount'
CREATE INDEX [IX_FK_AspNetUserSalesForceAccount]
ON [dbo].[SalesForceAccounts]
    ([AspNetUserId]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRole]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUser]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUser'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUser]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- Creating foreign key on [LinkedInCompanyId] in table 'UserLinkedInCompanies'
ALTER TABLE [dbo].[UserLinkedInCompanies]
ADD CONSTRAINT [FK_LinkedInCompanyUserLinkedInCompany]
    FOREIGN KEY ([LinkedInCompanyId])
    REFERENCES [dbo].[LinkedInCompanies]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LinkedInCompanyUserLinkedInCompany'
CREATE INDEX [IX_FK_LinkedInCompanyUserLinkedInCompany]
ON [dbo].[UserLinkedInCompanies]
    ([LinkedInCompanyId]);
GO

-- Creating foreign key on [AspNetUserId] in table 'UserLinkedInCompanies'
ALTER TABLE [dbo].[UserLinkedInCompanies]
ADD CONSTRAINT [FK_AspNetUserUserLinkedInCompany]
    FOREIGN KEY ([AspNetUserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserUserLinkedInCompany'
CREATE INDEX [IX_FK_AspNetUserUserLinkedInCompany]
ON [dbo].[UserLinkedInCompanies]
    ([AspNetUserId]);
GO

-- Creating foreign key on [LinkedInProfileId] in table 'UserLinkedInProfiles'
ALTER TABLE [dbo].[UserLinkedInProfiles]
ADD CONSTRAINT [FK_LinkedInProfileUserLinkedInProfile]
    FOREIGN KEY ([LinkedInProfileId])
    REFERENCES [dbo].[LinkedInProfiles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_LinkedInProfileUserLinkedInProfile'
CREATE INDEX [IX_FK_LinkedInProfileUserLinkedInProfile]
ON [dbo].[UserLinkedInProfiles]
    ([LinkedInProfileId]);
GO

-- Creating foreign key on [AspNetUserId] in table 'UserLinkedInProfiles'
ALTER TABLE [dbo].[UserLinkedInProfiles]
ADD CONSTRAINT [FK_AspNetUserUserLinkedInProfile]
    FOREIGN KEY ([AspNetUserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserUserLinkedInProfile'
CREATE INDEX [IX_FK_AspNetUserUserLinkedInProfile]
ON [dbo].[UserLinkedInProfiles]
    ([AspNetUserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------