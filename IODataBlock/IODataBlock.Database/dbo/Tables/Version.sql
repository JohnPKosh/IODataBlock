CREATE TABLE [dbo].[version]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
	[majorVersion] INT NULL, 
	[minorVersion] INT NULL,
	[buildNumber] INT NULL,
	[revision] INT NULL,
	[lastUpdated] DATETIME NULL DEFAULT GETDATE()
)
