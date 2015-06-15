CREATE PROCEDURE [dbo].[usp_DeleteExpiredQueueMeta]
AS


BEGIN TRY
	/* BEGIN WORK */ 
	BEGIN TRANSACTION;    

	DECLARE @temp TABLE([Id] int)

	DELETE FROM [dbo].[QueueMeta] 
	OUTPUT DELETED.[Id]
	INTO @temp([Id])
	WHERE [QueueExpiration] < GETDATE()

	DELETE FROM [dbo].[QueueData]
	WHERE [QueueID] IN(SELECT [Id] FROM @temp)

	COMMIT TRANSACTION;
	/* END WORK */ 
END TRY
BEGIN CATCH 		
IF XACT_STATE() <> 0
BEGIN
	ROLLBACK TRANSACTION;
END	
DECLARE @ErrorMessage NVARCHAR(4000);
    DECLARE @ErrorSeverity INT, @ErrorState INT;
    SELECT @ErrorMessage = ERROR_MESSAGE(), @ErrorSeverity = ERROR_SEVERITY(), @ErrorState = ERROR_STATE();
    -- Use RAISERROR inside the CATCH block to return error
    RAISERROR (@ErrorMessage, -- Message text.
               @ErrorSeverity, -- Severity.
               @ErrorState -- State.
               );
END CATCH;