CREATE TABLE [dbo].[LinkedInProfiles] (
    [Id]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [LinkedInProfileId]   BIGINT         NULL,
    [LinkedInPage]        NVARCHAR (255) NOT NULL,
    [LinkedInFullName]    NVARCHAR (100) NULL,
    [LinkedInConnections] INT            NULL,
    [LinkedInTitle]       NVARCHAR (100) NULL,
    [LinkedInCompanyName] NVARCHAR (100) NULL,
    [LinkedInCompanyId]   BIGINT         NULL,
    [Industry]            NVARCHAR (100) NULL,
    [Location]            NVARCHAR (100) NULL,
    [Email]               NVARCHAR (255) NULL,
    [IM]                  NVARCHAR (255) NULL,
    [Twitter]             NVARCHAR (255) NULL,
    [Address]             NVARCHAR (255) NULL,
    [Phone]               NVARCHAR (MAX) NULL,
    [LinkedInPhotoUrl]    NVARCHAR (255) NOT NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [AspNetUsers_Id]      NVARCHAR (36)  NOT NULL,
    [AspNetUserId]        NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_LinkedInProfiles] PRIMARY KEY CLUSTERED ([Id] ASC, [LinkedInPage] ASC, [LinkedInPhotoUrl] ASC, [CreatedDate] ASC, [AspNetUsers_Id] ASC),
    CONSTRAINT [FK_AspNetUserLinkedInProfile] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserLinkedInProfile]
    ON [dbo].[LinkedInProfiles]([AspNetUserId] ASC);

