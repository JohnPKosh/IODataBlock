CREATE TABLE [dbo].[SalesForceAccounts] (
    [Id]             INT            IDENTITY (1, 1) NOT NULL,
    [SfClientId]     NVARCHAR (128) NULL,
    [SfClientSecret] NVARCHAR (50)  NULL,
    [SfUsername]     NVARCHAR (50)  NULL,
    [SfPassword]     NVARCHAR (50)  NULL,
    [AspNetUserId]   NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_SalesForceAccounts] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_AspNetUserSalesForceAccount] FOREIGN KEY ([AspNetUserId]) REFERENCES [dbo].[AspNetUsers] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserSalesForceAccount]
    ON [dbo].[SalesForceAccounts]([AspNetUserId] ASC);

