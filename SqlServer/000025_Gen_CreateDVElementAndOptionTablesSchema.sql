/****** Object:  Table [dbo].[tblOptionLists]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblOptionLists]
GO
/****** Object:  Table [dbo].[tblOptionDetails]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblOptionDetails]
GO
/****** Object:  Table [dbo].[tblElementLists]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblElementLists]
GO
/****** Object:  Table [dbo].[tblElementListDetails]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblElementListDetails]
GO
/****** Object:  Table [dbo].[tblElementLibrary]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblElementLibrary]
GO
/****** Object:  Table [dbo].[tblElement_XREF_DomainLinks]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblElement_XREF_DomainLinks]
GO
/****** Object:  Table [dbo].[tblElement_XREF]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblElement_XREF]
GO
/****** Object:  Table [dbo].[tblDV_GroupXREF]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblDV_GroupXREF]
GO
/****** Object:  Table [dbo].[tblDV_Group]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblDV_Group]
GO
/****** Object:  Table [dbo].[tblDV]    Script Date: 4/4/2016 9:01:15 AM ******/
DROP TABLE [dbo].[tblDV]
GO
/****** Object:  Table [dbo].[tblDV]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDV](
	[DVD_ID] [int] Identity(1,1) NOT NULL,
	[DV_Label] [nvarchar](50) NULL,
	[EvaluationGroup_FK] [int] NULL,
	[VarType_FK] [int] NULL,
	[DV_Description] [nvarchar](100) NULL,
	[DV_Type] [nvarchar](255) NULL,
	[Option_FK] [int] NULL,
	[Option_MIN] [int] NULL,
	[Option_MAX] [int] NULL,
	[GetNewValMethod] [real] NULL,
	[FunctionID_FK] [int] Default -1,
	[FunctionArgs] [nvarchar](255) NULL,
	[ElementID_FK] [int] NULL,
	[Element_Label] [nvarchar](50) NULL,
	[IncludeInScenarioLabel] [bit] NOT NULL,
	[IsListVar] [bit] Default -1,
	[SkipMinVal] [bit] Default -1,
	[sqn] [int] NULL,
	[CostID_FK] [int] NULL,
	[SecondaryDV_Key] [real] Default -1,
	[PrimaryDV_ID_FK] [int] Default -1,
	[COST_ResultID_FK] [int] NULL,
	[Qualifier1] [nvarchar](25) NULL,
	[Qual1Pos] [int] NULL,
	[IsSpecialCase] [bit] NOT NULL,
	[IsDistrib] [bit] NOT NULL,
	[Distrib_VarType_FK] [int] NULL,
	[IsTS] [bit] NOT NULL,
	[XModelID_FK] [int] Default -1,
 CONSTRAINT [PK_tblDV] PRIMARY KEY CLUSTERED 
(
	[DVD_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblDV_Group]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDV_Group](
	[DV_GroupID] [int] Identity(1,1) NOT NULL,
	[DV_GroupLabel] [nvarchar](25) NULL,
	[DV_GroupDescription] [nvarchar](255) NULL,
	[ProjID_FK] [int] NULL,
 CONSTRAINT [PK_tblDV_Group] PRIMARY KEY CLUSTERED 
(
	[DV_GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblDV_GroupXREF]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblDV_GroupXREF](
	[ID] [int] Identity(1,1) NOT NULL,
	[DV_ID] [int] NULL,
	[DV_GroupID] [int] NULL,
 CONSTRAINT [PK_tblDV_GroupXREF] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblElement_XREF]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblElement_XREF](
	[ElementXREF_ID] [int] Identity(1,1) NOT NULL,
	[ProjID_FK] [int] NULL,
	[Descrip] [nvarchar](100) NULL,
	[RefTypeID] [int] NULL,
	[RefTypeCode] [int] NOT NULL,
	[RefID] [int] NULL,
	[RefID_Label] [nvarchar](100) NULL,
	[LinkTypeID] [int] NULL,
	[LinkTypeCode] [int] NOT NULL,
	[LinkID] [int] NULL,
	[LinkID_Label] [nvarchar](100) NULL,
	[LinkOperatorCode] [int] NULL,
	[RefScenarioID] [int] NULL,
	[LinkScenarioID] [int] NULL,
	[RefSimCode] [int] NULL,
	[LinkSimCode] [int] NULL,
	[InterpCode] [int] NULL,
	[IsSpecialCase] [bit] NOT NULL,
	[IsTS] [bit] NOT NULL,
	[LinkByCode] [real] NULL,
	[ShiftTSVal] [int] NULL,
	[ShiftTSCode] [int] NULL,
	[ShiftTSReverseAfterProcess] [bit] NOT NULL,
	[LinkFileIsScen] [bit] NOT NULL,
	[RefFileIsScen] [bit] NOT NULL,
	[IsDV_Link] [bit] NOT NULL,
 CONSTRAINT [PK_tblElement_XREF] PRIMARY KEY CLUSTERED 
(
	[ElementXREF_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblElement_XREF_DomainLinks]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblElement_XREF_DomainLinks](
	[ID] [int] Identity(1,1) NOT NULL,
	[RefFieldID] [int] NULL,
	[RefQualifier] [nvarchar](25) NULL,
	[RefIsResultLink] [bit] NOT NULL,
	[LinkFieldID] [int] NULL,
	[LinkQualifier] [nvarchar](25) NULL,
	[LinkIsResultLink] [bit] NOT NULL,
	[ProjID_FK] [int] NULL,
	[Description] [nvarchar](100) NULL,
	[RefModelTypeID] [int] NULL,
	[LinkModelTypeID] [int] NULL,
 CONSTRAINT [PK_tblElement_XREF_DomainLinks] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblElementLibrary]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblElementLibrary](
	[ElementLibID] [int] Identity(1,1) NOT NULL,
	[ElementLabel] [nvarchar](255) NULL,
	[ProjID_FK] [int] NULL,
	[EvalID_FK] [int] NULL,
	[VarTypeID_FK] [int] NULL,
	[CatID_FK] [int] NULL,
	[RefElement_FK] [int] NULL,
	[ElementLibVal] [nvarchar](max) NULL,
	[SubTuple] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblElementLibrary] PRIMARY KEY CLUSTERED 
(
	[ElementLibID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblElementListDetails]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblElementListDetails](
	[ElementListDetailID] [int] Identity(1,1) NOT NULL,
	[ElementListID_FK] [int] NULL,
	[val] [nvarchar](50) NULL,
	[ElementID_FK] [int] NULL,
	[VarLabel] [nvarchar](50) NULL,
 CONSTRAINT [PK_tblElementListDetails] PRIMARY KEY CLUSTERED 
(
	[ElementListDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblElementLists]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblElementLists](
	[ElementListID] [int] Identity(1,1) NOT NULL,
	[ProjID_FK] [int] NULL,
	[ElementListLabel] [nvarchar](25) NULL,
	[TableID_FK] [int] NULL,
	[ElementID_FK] [int] NULL,
	[Type] [nvarchar](25) NULL,
	[CostID_FK] [int] NULL,
 CONSTRAINT [PK_tblElementLists] PRIMARY KEY CLUSTERED 
(
	[ElementListID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblOptionDetails]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblOptionDetails](
	[OptionID] [int] Identity(1,1) NOT NULL,
	[OptionID_FK] [int] NULL,
	[OptionNo] [int] NULL,
	[val] [nvarchar](50) NULL,
	[valLabelinSCEN] [nvarchar](10) NULL,
	[VarID_FK] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblOptionDetails] PRIMARY KEY CLUSTERED 
(
	[OptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblOptionLists]    Script Date: 4/4/2016 9:01:15 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblOptionLists](
	[OptionID] [int] Identity(1,1) NOT NULL,
	[ProjID_FK] [int] NULL,
	[OptionLabel] [nvarchar](25) NULL,
	[Qual1] [nvarchar](25) NULL,
	[Qual2] [nvarchar](25) NULL,
	[Operation] [nvarchar](10) NULL,
	[VarType_FK] [int] NULL,
	[IsScaleValue] [bit] NOT NULL,
	[VarType_ScaleBy] [int] Default -1,
 CONSTRAINT [PK_tblOptionLists] PRIMARY KEY CLUSTERED 
(
	[OptionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.25', 'Creating schema for DV, Element and Option tables'); 