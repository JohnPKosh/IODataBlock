CREATE TABLE [dbo].[UserLinkedInCompanies] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [LinkedInCompanyId] BIGINT         NOT NULL,
    [AspNetUserId]      NVARCHAR (128) NOT NULL,
	[CreatedDate]         DATETIME       NOT NULL,
    CONSTRAINT [PK_UserLinkedInCompanies] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserUserLinkedInCompany] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_LinkedInCompanyUserLinkedInCompany] FOREIGN KEY ([LinkedInCompanyId]) REFERENCES [dbo].[LinkedInCompanies] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserUserLinkedInCompany]
    ON [dbo].[UserLinkedInCompanies]([AspNetUserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_LinkedInCompanyUserLinkedInCompany]
    ON [dbo].[UserLinkedInCompanies]([LinkedInCompanyId] ASC);

