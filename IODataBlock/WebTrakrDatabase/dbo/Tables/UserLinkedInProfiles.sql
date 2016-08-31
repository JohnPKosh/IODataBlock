CREATE TABLE [dbo].[UserLinkedInProfiles] (
    [Id]                BIGINT         IDENTITY (1, 1) NOT NULL,
    [LinkedInProfileId] BIGINT         NOT NULL,
    [AspNetUserId]      NVARCHAR (128) NOT NULL,
	[CreatedDate]         DATETIME       NOT NULL,
    CONSTRAINT [PK_UserLinkedInProfiles] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserUserLinkedInProfile] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id]),
    CONSTRAINT [FK_LinkedInProfileUserLinkedInProfile] FOREIGN KEY ([LinkedInProfileId]) REFERENCES [dbo].[LinkedInProfiles] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserUserLinkedInProfile]
    ON [dbo].[UserLinkedInProfiles]([AspNetUserId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FK_LinkedInProfileUserLinkedInProfile]
    ON [dbo].[UserLinkedInProfiles]([LinkedInProfileId] ASC);

