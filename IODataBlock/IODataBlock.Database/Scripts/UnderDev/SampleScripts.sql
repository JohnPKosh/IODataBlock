USE [IODataBlock]
GO

EXEC [dbo].[usp_Enqueue] @QueueName = N'demo', @Data = N'test3'
EXEC [dbo].[usp_Enqueue] @QueueName = N'demo', @Data = N'test4'
EXEC [dbo].[usp_Enqueue] @QueueName = N'demo', @Data = N'test5'

EXEC [dbo].[usp_Dequeue] @QueueName = N'demo', @BatchSize = 20, @Ttl = 30
EXEC [dbo].[usp_DequeueByDateTime] @QueueName = N'demo', @BatchSize = 20, @Ttl = 30, @RangeStart = '2015-06-08 13:59:36.677'--, @RangeEnd = '2015-06-08 14:04:13.270'

EXEC [dbo].[usp_Enqueue] @QueueName = N'demo2', @Data = N'demo2'
EXEC [dbo].[usp_Enqueue] @QueueName = N'demo2', @Data = N'demo2'
EXEC [dbo].[usp_Enqueue] @QueueName = N'demo2', @Data = N'demo2'

EXEC [dbo].[usp_Dequeue] @QueueName = N'demo2', @BatchSize = 20, @Ttl = 30


EXEC [dbo].[usp_CompleteQueueItemById] @Id = 23



EXEC [dbo].[usp_DeleteExpiredQueueData]
EXEC [dbo].[usp_DeleteExpiredQueueMeta]