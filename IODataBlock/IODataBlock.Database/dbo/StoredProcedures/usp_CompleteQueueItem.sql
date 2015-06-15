CREATE PROCEDURE [dbo].[usp_CompleteQueueItemById]
	@Id int
AS
UPDATE [dbo].[QueueMeta] WITH (UPDLOCK)
SET [Status] = 2
WHERE [Id] = @Id

