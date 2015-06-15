CREATE PROCEDURE [dbo].[usp_Enqueue]
	@QueueName [nvarchar](36),
	@Data [nvarchar](max),
	@QueueExpiration [datetime] = NULL
AS

IF(@QueueExpiration IS NULL)
BEGIN

INSERT INTO [dbo].[QueueMeta]
           ([QueueName])     
OUTPUT INSERTED.[Id], @Data
INTO [dbo].[QueueData]([QueueID],[Data])
VALUES(@QueueName)
SELECT SCOPE_IDENTITY()

END
ELSE
BEGIN

INSERT INTO [dbo].[QueueMeta]
           ([QueueName], [QueueExpiration])     
OUTPUT INSERTED.[Id], @Data
INTO [dbo].[QueueData]([QueueID],[Data])
VALUES(@QueueName, @QueueExpiration)
SELECT SCOPE_IDENTITY()

END