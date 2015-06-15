--USE [TempData]

SELECT TOP 1000 [QueueID]
      ,[QueueDateTime]
      ,[Title]
      ,[Status]
  FROM [TempData].[dbo].[QueueMeta]

SELECT TOP 1000 [QueueID]
      ,[TextData]
  FROM [TempData].[dbo].[QueueData]