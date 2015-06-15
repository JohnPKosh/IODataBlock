ALTER TABLE [dbo].[QueueMeta]
	ADD CONSTRAINT [UC_QueueMeta_QueueExpiration]
	DEFAULT GETDATE()+15
	FOR [QueueExpiration]
