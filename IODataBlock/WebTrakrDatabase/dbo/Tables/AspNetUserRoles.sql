CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] NVARCHAR (128) NOT NULL,
    [AspNetUsers_Id] NVARCHAR (128) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY CLUSTERED ([AspNetRoles_Id] ASC, [AspNetUsers_Id] ASC),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles] FOREIGN KEY ([AspNetRoles_Id]) REFERENCES [dbo].[AspNetRoles] ([Id]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers] FOREIGN KEY ([AspNetUsers_Id]) REFERENCES [dbo].[AspNetUsers] ([Id])
);




GO



GO
CREATE NONCLUSTERED INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
    ON [dbo].[AspNetUserRoles]([AspNetUsers_Id] ASC);

