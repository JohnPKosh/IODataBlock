--USE [TempData]
--GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO


--ALTER PROCEDURE [dbo].[Dequeue]
CREATE PROCEDURE [dbo].[Dequeue]
@BatchSize int = 100
AS
SET NOCOUNT ON;

UPDATE TOP(@BatchSize) [dbo].[QueueMeta] WITH (UPDLOCK, READPAST)
SET Status = 1
OUTPUT [INSERTED].[QueueID], [INSERTED].[QueueDateTime], [INSERTED].[Title], qd.[TextData]
FROM [dbo].[QueueMeta] AS qm
INNER JOIN [dbo].[QueueData]  AS qd
    ON qm.[QueueID] = qd.[QueueID]
WHERE qm.[Status] = 0

/*
EXECUTE [dbo].[Dequeue] 1000
*/

GO


