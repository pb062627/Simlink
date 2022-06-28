/****** Object:  Table [dbo].[tblResultVar]    Script Date: 4/4/2016 9:24:39 AM ******/
DROP TABLE [dbo].[tblResultVar]
GO
/****** Object:  Table [dbo].[tblResultTS_EventSummary]    Script Date: 4/4/2016 9:24:39 AM ******/
DROP TABLE [dbo].[tblResultTS_EventSummary]
GO
/****** Object:  Table [dbo].[tblResultTS]    Script Date: 4/4/2016 9:24:39 AM ******/
DROP TABLE [dbo].[tblResultTS]
GO
/****** Object:  Table [dbo].[tblPerformance_ResultXREF]    Script Date: 4/4/2016 9:24:39 AM ******/
DROP TABLE [dbo].[tblPerformance_ResultXREF]
GO
/****** Object:  Table [dbo].[tblPerformance]    Script Date: 4/4/2016 9:24:39 AM ******/
DROP TABLE [dbo].[tblPerformance]
GO
/****** Object:  Table [dbo].[tblPerformance]    Script Date: 4/4/2016 9:24:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblPerformance](
	[PerformanceID] [int] Identity(1,1) NOT NULL,
	[Performance_Label] [nvarchar](50) NULL,
	[PF_Type] [int] NULL,
	[CategoryID_FK] [int] Default -1,
	[LinkTableCode] [int] NULL,
	[PF_FunctionType] [int] NULL,
	[EvalID_FK] [int] NULL,
	[FunctionID_FK] [int] Default -1,
	[TS_Code] [int] NULL,
	[Description] [nvarchar](255) NULL,
	[LKP_LookupID_FK] [int] NULL,
	[LKP_ScaleBy] [int] NULL,
	[LKP_ScaleFactor] [float] NULL,
	[ScaleFactorInput] [float] NULL,
	[UseDifferenceFromBaseline] [bit] NOT NULL,
	[LKP_Qual] [nvarchar](255) NULL,
	[IfErrorVal] [float] NULL,
	[IsDistrib] [bit] NOT NULL,
	[ApplyThreshold] [bit] NOT NULL,
	[Threshold] [float] NULL,
	[ResultFunctionKey] [int] Default -1,
	[IsObjective] [bit] NOT NULL,
	[SQN] [int] NULL,
	[FunctionArgs] [nvarchar](255) NULL,
	[DV_ID_FK] [int] Default -1,
	[OptionID_FK] [int] Default -1,
	[IsOver_Threshold] [bit] NOT NULL,
 CONSTRAINT [PK_tblPerformance] PRIMARY KEY CLUSTERED 
(
	[PerformanceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblPerformance_ResultXREF]    Script Date: 4/4/2016 9:24:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblPerformance_ResultXREF](
	[ID] [int] Identity(1,1) NOT NULL,
	[PerformanceID_FK] [int] NULL,
	[LinkTableID_FK] [int] NULL,
	[ScalingFactor] [float] NULL,
	[LinkType] [int] NULL,
	[SSMA_TimeStamp] [binary](8) NOT NULL,
 CONSTRAINT [PK_tblPerformance_ResultXREF] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblResultTS]    Script Date: 4/4/2016 9:24:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblResultTS](
	[ResultTS_ID] [int] Identity(1,1) NOT NULL,
	[Result_Label] [nvarchar](50) NULL,
	[EvaluationGroup_FK] [int] NULL,
	[ResultID_FK] [int] NULL,
	[VarResultType_FK] [int] NULL,
	[StationID_FK] [int] NULL,
	[Result_Description] [nvarchar](100) NULL,
	[ElementID_FK] [int] NULL,
	[ElementIndex] [nvarchar](255) NULL,
	[Element_Label] [nvarchar](50) NULL,
	[TS_StartDate] [nvarchar](15) NULL,
	[TS_StartHour] [nvarchar](15) NULL,
	[TS_StartMin] [nvarchar](15) NULL,
	[TS_Interval] [int] NULL,
	[TS_Interval_Unit] [int] NULL,
	[BeginPeriodNo] [smallint] Default 1,
	[IsSecondary] [bit] NOT NULL,
	[SQN] [real] NOT NULL,
	[FunctionID_FK] [int] NOT NULL,
	[FunctionArgs] [nvarchar](255) NULL,
	[IsAux] [bit] NOT NULL,
	[RefTS_ID_FK] [int] NULL,
 CONSTRAINT [PK_tblResultTS] PRIMARY KEY CLUSTERED 
(
	[ResultTS_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblResultTS_EventSummary]    Script Date: 4/4/2016 9:24:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblResultTS_EventSummary](
	[EventSummaryID] [int] Identity(1,1) NOT NULL,
	[ResultTS_or_Event_ID_FK] [int] NULL,
	[EvaluationGroupID_FK] [int] NULL,
	[CategoryID_FK] [int] NULL,
	[EventFunctionID] [int] NULL,
	[EventLevelCode] [int] Default 1,
	[Threshold_Inst] [float] NULL,
	[IsOver_Threshold_Inst] [bit] NOT NULL,
	[Threshold_Cumulative] [float] NULL,
	[IsOver_Threshold_Cumulative] [bit] NOT NULL,
	[Description] [nvarchar](255) NULL,
	[InterEvent_Threshold] [int] NULL,
	[CountMeetsThreshold] [int] NULL,
	[ThresholdCalcEQ] [nvarchar](255) NULL,
	[CalcValueInExcessOfThreshold] [bit] NOT NULL,
 CONSTRAINT [PK_tblResultTS_EventSummary] PRIMARY KEY CLUSTERED 
(
	[EventSummaryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblResultVar]    Script Date: 4/4/2016 9:24:39 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblResultVar](
	[Result_ID] [int] Identity(1,1) NOT NULL,
	[Result_Label] [nvarchar](50) NULL,
	[EvaluationGroup_FK] [int] NULL,
	[VarResultType_FK] [int] NULL,
	[Result_Description] [nvarchar](100) NULL,
	[ElementID_FK] [int] NULL,
	[Element_Label] [nvarchar](50) NULL,
	[IsListVar] [bit] NOT NULL,
	[ImportResultDetail] [bit] NOT NULL,
 CONSTRAINT [PK_tblResultVar] PRIMARY KEY CLUSTERED 
(
	[Result_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.40', 'Creating schema for Output Formulation Tables'); 