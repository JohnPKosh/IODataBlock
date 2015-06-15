ALTER TABLE [dbo].[QueueMeta]
	ADD CONSTRAINT [UC_QueueMetaDequeueItemExpiration]
	DEFAULT getdate()
	FOR [DequeueItemExpiration]
