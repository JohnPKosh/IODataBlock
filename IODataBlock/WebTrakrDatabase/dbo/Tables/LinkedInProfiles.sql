CREATE TABLE [dbo].[LinkedInProfiles] (
    [Id]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [LinkedInProfileId]   BIGINT         NULL,
    [LinkedInPage]        NVARCHAR (255) NOT NULL,
    [LinkedInFullName]    NVARCHAR (100) NULL,
    [LinkedInConnections] INT            NULL,
    [LinkedInTitle]       NVARCHAR (255) NULL,
    [LinkedInCompanyName] NVARCHAR (100) NULL,
    [LinkedInCompanyId]   BIGINT         NULL,
    [Industry]            NVARCHAR (100) NULL,
    [Location]            NVARCHAR (100) NULL,
    [Email]               NVARCHAR (255) NULL,
    [IM]                  NVARCHAR (255) NULL,
    [Twitter]             NVARCHAR (255) NULL,
    [Address]             NVARCHAR (255) NULL,
    [Phone]               NVARCHAR (MAX) NULL,
    [LinkedInPhotoUrl]    NVARCHAR (255) NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [UpdatedDate]         DATETIME       NOT NULL,
    CONSTRAINT [PK_LinkedInProfiles] PRIMARY KEY CLUSTERED ([Id] ASC)
);

