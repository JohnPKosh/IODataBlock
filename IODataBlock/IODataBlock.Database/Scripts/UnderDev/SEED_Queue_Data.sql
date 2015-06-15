
--USE [TempData]

set nocount on
declare @counter int, @BatchSize int
set @counter = 1
set @BatchSize = 5000
while @counter < @BatchSize
begin
	INSERT INTO [dbo].[QueueMeta]
           ([QueueDateTime]
           ,[Title]
           ,[Status])     
	OUTPUT INSERTED.[QueueID], REPLICATE('X', RAND() * 16000)
	INTO [dbo].[QueueData]
	VALUES(GETDATE(), 'Item no: ' + CONVERT(varchar(10), @counter), 0)
    set @counter = @counter + 1
end

--USE [TempData]
--GO

--TRUNCATE TABLE [dbo].[QueueMeta]
--TRUNCATE TABLE [dbo].[QueueData]