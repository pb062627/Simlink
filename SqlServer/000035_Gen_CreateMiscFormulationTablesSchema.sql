/****** Object:  Table [dbo].[tblSplint]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblSplint]
GO
/****** Object:  Table [dbo].[tblNewFeature]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblNewFeature]
GO
/****** Object:  Table [dbo].[tblEXCEL_Range]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblEXCEL_Range]
GO
/****** Object:  Table [dbo].[tblEnsembleXREF]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblEnsembleXREF]
GO
/****** Object:  Table [dbo].[tblEnsemble]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblEnsemble]
GO
/****** Object:  Table [dbo].[tblDistrib_ElementList_XREF]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblDistrib_ElementList_XREF]
GO
/****** Object:  Table [dbo].[tblDistrib_ElementLibStrategy_XREF]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblDistrib_ElementLibStrategy_XREF]
GO
/****** Object:  Table [dbo].[tblDistrib_ElementLibary_XREF]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblDistrib_ElementLibary_XREF]
GO
/****** Object:  Table [dbo].[tblCategoryRecord_XREF]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblCategoryRecord_XREF]
GO
/****** Object:  Table [dbo].[tblCategory]    Script Date: 4/4/2016 9:15:27 AM ******/
DROP TABLE [dbo].[tblCategory]
GO
/****** Object:  Table [dbo].[tblCategory]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCategory](
	[CategoryID] [int] Identity(1,1) NOT NULL,
	[TableKey] [int] NULL,
	[CategoryID_FK] [int] NULL,
	[Label] [nvarchar](50) NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblCategory] PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblCategoryRecord_XREF]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblCategoryRecord_XREF](
	[ID] [int] Identity(1,1) NOT NULL,
	[CategoryID_FK] [int] NULL,
	[RecordID] [int] NULL,
 CONSTRAINT [PK_tblCategoryRecord_XREF] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblDistrib_ElementLibary_XREF]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDistrib_ElementLibary_XREF](
	[ID] [int] Identity(1,1) NOT NULL,
	[Label] [nvarchar](25) NULL,
	[ElementLibID_FK] [int] NULL,
	[DistribDefID_FK] [int] NULL,
	[Val] [float] NULL,
	[Descrip] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblDistrib_ElementLibary_XREF] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblDistrib_ElementLibStrategy_XREF]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDistrib_ElementLibStrategy_XREF](
	[ID] [int] Identity(1,1) NOT NULL,
	[ElementLibraryID_FK] [int] NULL,
	[StrategyID_FK] [int] NULL,
	[Descrip] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblDistrib_ElementLibStrategy_XREF] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblDistrib_ElementList_XREF]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDistrib_ElementList_XREF](
	[ID] [int] Identity(1,1) NOT NULL,
	[ProjID_FK] [int] NULL,
	[ElementListID_FK] [int] NULL,
	[Qualifier] [nvarchar](25) NULL,
 CONSTRAINT [PK_tblDistrib_ElementList_XREF] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEnsemble]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEnsemble](
	[EnsembleID] [int] Identity(1,1) NOT NULL,
	[ProjID_FK] [int] NULL,
	[Label] [nvarchar](25) NULL,
	[Description] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblEnsemble] PRIMARY KEY CLUSTERED 
(
	[EnsembleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEnsembleXREF]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEnsembleXREF](
	[EDetailID] [int] Identity(1,1) NOT NULL,
	[EnsembleID_FK] [int] NULL,
	[GCM_ID_FK] [int] NULL,
 CONSTRAINT [PK_tblEnsembleXREF] PRIMARY KEY CLUSTERED 
(
	[EDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEXCEL_Range]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEXCEL_Range](
	[ID] [int] Identity(1,1) NOT NULL,
	[Label] [nvarchar](50) NULL,
	[Worksheet] [nvarchar](255) NULL,
	[Row] [int] NULL,
	[Col] [int] NULL,
	[IsFormula] [bit] NOT NULL,
 CONSTRAINT [PK_tblEXCEL_Range] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblNewFeature]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblNewFeature](
	[NewFeatureID] [int] Identity(1,1) NOT NULL,
	[Text_WriteLine] [nvarchar](max) NULL,
	[sqn] [nvarchar](255) NULL,
	[OffsetRow] [int] NULL,
	[SeekText] [nvarchar](255) NULL,
	[InsertPos] [int] NULL,
	[OverwriteExiting] [bit] NOT NULL,
	[QualINT] [int] NULL,
	[SSMA_TimeStamp] [binary](8) NOT NULL,
 CONSTRAINT [PK_tblNewFeature] PRIMARY KEY CLUSTERED 
(
	[NewFeatureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblSplint]    Script Date: 4/4/2016 9:15:27 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblSplint](
	[ID] [int] Identity(1,1) NOT NULL,
	[EvalID_FK] [int] NULL,
	[RecordID] [int] NULL,
	[VarType_FK] [int] NULL,
	[val] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[ActionCode] [int] NULL,
	[ApplyToDependent] [int] NULL,
 CONSTRAINT [PK_tblSplint] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.35', 'Creating schema for Miscellaneous Formulation Tables'); 