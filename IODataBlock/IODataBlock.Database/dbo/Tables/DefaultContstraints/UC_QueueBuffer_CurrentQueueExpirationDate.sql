
ALTER TABLE [dbo].[QueueBuffer] 
ADD  CONSTRAINT [UC_QueueBuffer_CurrentQueueExpirationDate]  
DEFAULT (CONVERT([nvarchar](10),getdate()+(1),(120))) 
FOR [CurrentQueueExpiration]
GO

