/****** Object:  Table [dbo].[tblExternalDataRequest]    Script Date: 4/11/2016 5:25:16 PM ******/
DROP TABLE [dbo].[tblExternalDataRequest]
GO
/****** Object:  Table [dbo].[tblExternalDataRequest]    Script Date: 4/11/2016 5:25:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblExternalDataRequest](
	[ID] [int] Identity(1,1) NOT NULL,
	[EvalID_FK] [int] NOT NULL,
	[Label] [nvarchar](50) NULL,
	[description] [nvarchar](255) NULL,
	[source_code] [int] NOT NULL Default 0,
	[return_format_code] [int] NOT NULL Default 0,
	[db_type] [int] NOT NULL Default 0,
	[kwargs] [nvarchar](255) NULL,
	[conn_string] [nvarchar](255) NULL,	
CONSTRAINT [PK_tblExternalDataRequest] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.11', 'Creating schema for retrieving data from external data sources'); 