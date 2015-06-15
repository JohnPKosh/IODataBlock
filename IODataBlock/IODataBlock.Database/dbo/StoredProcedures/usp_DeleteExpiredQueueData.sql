CREATE PROCEDURE [dbo].[usp_DeleteExpiredQueueData]	
AS
DELETE FROM [dbo].[QueueData]
WHERE [QueueID] IN(SELECT 1 FROM [dbo].[QueueMeta] WHERE [QueueExpiration] < GETDATE())
