/****** Object:  Table [dbo].[tblVersion]    Script Date: 3/7/2016 10:39:15 AM ******/
DROP TABLE [dbo].[tblVersion]
GO
/****** Object:  Table [dbo].[tblVersion]    Script Date: 3/7/2016 10:39:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblVersion](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[VersionNumber] [nvarchar](255) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[LastUpdated] [datetime] DEFAULT CURRENT_TIMESTAMP,
 CONSTRAINT [PK_tblVersion] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) 
ON [PRIMARY]

GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.00', 'Created tblVersion'); 