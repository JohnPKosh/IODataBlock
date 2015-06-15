CREATE TABLE [dbo].[QueueMeta](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[QueueName] [nvarchar](36) NOT NULL,
	[QueueDateTime] [datetime] NOT NULL,
	[QueueExpiration] [datetime] NOT NULL,
	[DequeueItemExpiration] [datetime] NOT NULL,
	[Status] [int] NOT NULL,
 CONSTRAINT [PK_QueueMeta] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
