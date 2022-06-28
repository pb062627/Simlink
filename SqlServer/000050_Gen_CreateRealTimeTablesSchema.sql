/****** Object:  Table [dbo].[tblRT_RealTime]    Script Date: 4/11/2016 5:25:16 PM ******/
DROP TABLE [dbo].[tblRT_RealTime]
GO
/****** Object:  Table [dbo].[tblRT_RealTime]    Script Date: 4/11/2016 5:25:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblRT_RealTime](
	[ID] [int] Identity(1,1) NOT NULL,
	[EvalID_FK] [int] NULL,
	[Label] [nvarchar](25) NULL,
	[Lat] [float] NULL,
	[Longitude] [float] NULL,
	[ResultsCode] [int] NULL,
	
CONSTRAINT [PK_tblRT_RealTime] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.50', 'Creating schema for Real Time tables'); 