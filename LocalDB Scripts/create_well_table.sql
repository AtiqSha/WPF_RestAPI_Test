USE [aem_test]
GO

/****** Object:  Table [dbo].[Well]    Script Date: 25/1/2021 10:10:46 AM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Well](
	[id] [int] NOT NULL,
	[platformId] [int] NULL,
	[uniqueName] [varchar](50) NULL,
	[latitude] [float] NULL,
	[longitude] [float] NULL,
	[createdAt] [datetime] NULL,
	[updatedAt] [datetime] NULL,
 CONSTRAINT [PK_Well] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


