CREATE PROCEDURE [dbo].[usp_DequeueByDateTime]
	@QueueName [nvarchar](36),
	@BatchSize int = 100,
	@Ttl int = 600,
	@RangeStart datetime,
	@RangeEnd datetime = NULL
AS
SET NOCOUNT ON;

UPDATE [dbo].[QueueMeta] WITH (UPDLOCK, READPAST)
SET [Status] = 0
WHERE [Status] = 1
AND [DequeueItemExpiration] < GETDATE()

UPDATE TOP(@BatchSize) [dbo].[QueueMeta] WITH (UPDLOCK, READPAST)
SET [Status] = 1
, [DequeueItemExpiration] = DATEADD(second, @Ttl, GETDATE())
OUTPUT [INSERTED].[Id], [INSERTED].[QueueDateTime], [INSERTED].[DequeueItemExpiration], qd.[Id] As [Uuid], qd.[Data]
FROM [dbo].[QueueMeta] AS qm
INNER JOIN [dbo].[QueueData]  AS qd
    ON qm.[Id] = qd.[QueueID]
WHERE qm.[QueueName] = @QueueName
AND qm.[Status] = 0
AND qm.[QueueDateTime] BETWEEN @RangeStart AND COALESCE(@RangeEnd, GETDATE())

/*
EXECUTE [dbo].[Dequeue] 1000
*/

/* 
0 = new
1 = dequeued
2 = complete
*/