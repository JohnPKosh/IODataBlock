
--USE [TempData]
--GO


IF (EXISTS(SELECT 1 FROM [dbo].[QueueMeta]))
BEGIN
	SELECT CAST(1 as bit)
END
ELSE
BEGIN
	SELECT CAST(0 as bit)
END



IF (EXISTS(SELECT 1 FROM [dbo].[QueueMeta] WHERE [Status] = 0))
BEGIN
	SELECT CAST(1 as bit) AS [Queued]
END
ELSE
BEGIN
	SELECT CAST(0 as bit) AS [Queued]
END

DECLARE @now datetime
SELECT @now = GETDATE()
IF (EXISTS(SELECT 1 FROM [dbo].[QueueMeta] WHERE [Status] = 1 AND [DequeueItemExpiration] < @now))
BEGIN
	SELECT CAST(1 as bit) AS [ExpiredQueueData]
END
ELSE
BEGIN
	SELECT CAST(0 as bit) AS [ExpiredQueueData]
END

IF (EXISTS(SELECT 1 FROM [dbo].[QueueMeta] WHERE [Status] = 1 AND [DequeueItemExpiration] > @now))
BEGIN
	SELECT CAST(1 as bit)  AS [UnexpiredQueueData]
END
ELSE
BEGIN
	SELECT CAST(0 as bit) AS [UnexpiredQueueData]
END

