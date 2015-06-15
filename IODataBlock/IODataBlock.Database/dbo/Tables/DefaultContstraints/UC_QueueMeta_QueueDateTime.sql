ALTER TABLE [dbo].[QueueMeta]
	ADD CONSTRAINT [UC_QueueMeta_QueueDateTime]
	DEFAULT getdate()
	FOR [QueueDateTime]
