/****** Object:  Table [dbo].[tblSupportingFileSpec]Script Date: 4/11/2016 5:25:16 PM ******/
DROP TABLE [dbo].[tblSupportingFileSpec]
GO
/****** Object:  Table [dbo].[tblSupportingFileSpec]Script Date: 4/11/2016 5:25:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSupportingFileSpec](
	[ID] [int] Identity(1,1) NOT NULL,
	[EvalID_FK] [int] NOT NULL Default -1,
	[description] [nvarchar](255) NULL,
	[SimlinkData_Code] [int] NOT NULL Default 0,
	[RecordID_FK] [int] NOT NULL Default -1,
	[DataType_Code] [int] NOT NULL Default -1,
	[Filename] [nvarchar](255) NULL,
	[Params] [nvarchar](255) NULL,	
	[IsInput] [bit] NOT NULL Default 0,	
CONSTRAINT [PK_tblSupportingFileSpec] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.13', 'Creating schema for requesting input/output file info (tblSupportingFileSpec)'); 