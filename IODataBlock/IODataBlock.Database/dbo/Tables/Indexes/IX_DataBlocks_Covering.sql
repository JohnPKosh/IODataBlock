CREATE INDEX [IX_DataBlocks_Covering] ON [dbo].[DataBlocks]
(
	[BlockKey] ASC,
	[Key] ASC,
	[Value] ASC,
	[Id] ASC
) 