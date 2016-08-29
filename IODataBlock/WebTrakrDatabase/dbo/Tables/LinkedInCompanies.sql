CREATE TABLE [dbo].[LinkedInCompanies] (
    [Id]                  BIGINT         IDENTITY (1, 1) NOT NULL,
    [LinkedInId]          BIGINT         NULL,
    [LinkedInPage]        NVARCHAR (255) NOT NULL,
    [LinkedInCompanyName] NVARCHAR (255) NULL,
    [DomainName]          NVARCHAR (255) NULL,
    [specialties]         NVARCHAR (MAX) NULL,
    [streetAddress]       NVARCHAR (255) NULL,
    [locality]            NVARCHAR (50)  NULL,
    [region]              NVARCHAR (50)  NULL,
    [postalCode]          NVARCHAR (20)  NULL,
    [countryName]         NVARCHAR (50)  NULL,
    [website]             NVARCHAR (255) NULL,
    [industry]            NVARCHAR (100) NULL,
    [type]                NVARCHAR (50)  NULL,
    [companySize]         NVARCHAR (25)  NULL,
    [founded]             NVARCHAR (25)  NULL,
    [followersCount]      INT            NULL,
    [photourl]            NVARCHAR (255) NULL,
    [description]         NVARCHAR (MAX) NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [AspNetUsers_Id]      NVARCHAR (36)  NOT NULL,
    [AspNetUserId]        NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_LinkedInCompanies] PRIMARY KEY CLUSTERED ([Id] ASC, [LinkedInPage] ASC, [CreatedDate] ASC, [AspNetUsers_Id] ASC),
    CONSTRAINT [FK_AspNetUserLinkedInCompany] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserLinkedInCompany]
    ON [dbo].[LinkedInCompanies]([AspNetUserId] ASC);

