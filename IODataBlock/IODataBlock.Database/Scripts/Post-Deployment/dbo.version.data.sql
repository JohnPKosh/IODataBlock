TRUNCATE TABLE [dbo].[version]
GO

SET IDENTITY_INSERT [dbo].[version] ON
INSERT INTO [dbo].[version] ([Id], [majorVersion], [minorVersion], [buildNumber], [revision]) VALUES (1, 1, 0, 0, 0)
SET IDENTITY_INSERT [dbo].[version] OFF
GO