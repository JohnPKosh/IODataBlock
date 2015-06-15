ALTER TABLE [dbo].[QueueMeta]
	ADD CONSTRAINT [UC_QueueMeta_Status]
	DEFAULT 0
	FOR [Status]
