/****** Object:  Table [dbo].[tblResultVar_Details]    Script Date: 4/4/2016 9:21:33 AM ******/
DROP TABLE [dbo].[tblResultVar_Details]
GO
/****** Object:  Table [dbo].[tblResultTS_EventSummary_Detail]    Script Date: 4/4/2016 9:21:33 AM ******/
DROP TABLE [dbo].[tblResultTS_EventSummary_Detail]
GO
/****** Object:  Table [dbo].[tblResultTS_Detail]    Script Date: 4/4/2016 9:21:33 AM ******/
DROP TABLE [dbo].[tblResultTS_Detail]
GO
/****** Object:  Table [dbo].[tblPerformance_Detail]    Script Date: 4/4/2016 9:21:33 AM ******/
DROP TABLE [dbo].[tblPerformance_Detail]
GO
/****** Object:  Table [dbo].[tblModElementVals]    Script Date: 4/4/2016 9:21:33 AM ******/
DROP TABLE [dbo].[tblModElementVals]
GO
/****** Object:  Table [dbo].[tblModElementVals]    Script Date: 4/4/2016 9:21:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblModElementVals](
	[Model_ID] [int] Identity(1,1) NOT NULL,
	[DV_ID_FK] [int] NULL,
	[TableFieldKey_FK] [int] NULL,
	[DV_Option] [int] NULL,
	[ScenarioID_FK] [int] NULL,
	[val] [nvarchar](100) NULL,
	[element_note] [nvarchar](150) NULL,
	[ElementName] [nvarchar](50) NULL,
	[ElementID] [int] NULL,
	[IsInsert] [bit]  DEFAULT 1,
 CONSTRAINT [PK_tblModElementVals] PRIMARY KEY CLUSTERED 
(
	[Model_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblPerformance_Detail]    Script Date: 4/4/2016 9:21:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPerformance_Detail](
	[PF_DetailID] [int] Identity(1,1) NOT NULL,
	[PerformanceID_FK] [int] NULL,
	[DVID_FK] [int] NULL,
	[VAL] [float] NULL,
	[ScenarioID_FK] [int] NULL,
	[IsLinkToGroup] [bit] NULL,
	[ScenarioElementVal_ID] [int] NULL,
	[PerformanceLKP_FK] [int] NULL,
	[IsInvalid] [bit] NULL,
	[Quantity] [float] NULL,
 CONSTRAINT [PK_tblPerformance_Detail] PRIMARY KEY CLUSTERED 
(
	[PF_DetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblResultTS_Detail]    Script Date: 4/4/2016 9:21:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblResultTS_Detail](
	[TS_ID] [int] Identity(1,1) NOT NULL,
	[ScenarioID_FK] [int] NULL,
	[ResultTSID_FK] [int] NULL,
	[PeriodNo] [int] NULL,
	[valTS] [float] NULL,
 CONSTRAINT [PK_tblResultTS_Detail] PRIMARY KEY CLUSTERED 
(
	[TS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblResultTS_EventSummary_Detail]    Script Date: 4/4/2016 9:21:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblResultTS_EventSummary_Detail](
	[TS_EventSummaryDetailID] [int] IDENTITY(1,1) NOT NULL,
	[ScenarioID_FK] [int] NULL,
	[EventSummary_ID] [int] NULL,
	[ResultTS_ID_FK] [int] NULL,
	[EventDuration] [int] NULL,
	[EventBeginPeriod] [int] NULL,
	[MaxVal] [float] NULL,
	[TotalVal] [float] NULL,
	[SubEventThresholdPeriods] [int] NULL,
	[Rank_TOTAL] [int] NULL,
	[Rank_Peak] [int] NULL,
 CONSTRAINT [PK_tblResultTS_EventSummary_Detail] PRIMARY KEY CLUSTERED 
(
	[TS_EventSummaryDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblResultVar_Details]    Script Date: 4/4/2016 9:21:33 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblResultVar_Details](
	[ResultDetail_ID] [int] Identity(1,1) NOT NULL,
	[Result_ID_FK] [int] NULL,
	[TableFieldKey_FK] [int] NULL,
	[ScenarioID_FK] [int] NULL,
	[val] [float] NULL,
	[ElementName] [nvarchar](50) NULL,
	[ElementID] [int] NULL,
 CONSTRAINT [PK_tblResultVar_Details] PRIMARY KEY CLUSTERED 
(
	[ResultDetail_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.45', 'Creating schema for Simlink Output tables'); 