/****** Object:  Table [dbo].[tblScenario]    Script Date: 4/4/2016 9:05:52 AM ******/
DROP TABLE [dbo].[tblScenario]
GO
/****** Object:  Table [dbo].[tblProj]    Script Date: 4/4/2016 9:05:52 AM ******/
DROP TABLE [dbo].[tblProj]
GO
/****** Object:  Table [dbo].[tblEvaluationGroup_DistribDetail_OrderingList]    Script Date: 4/4/2016 9:05:52 AM ******/
DROP TABLE [dbo].[tblEvaluationGroup_DistribDetail_OrderingList]
GO
/****** Object:  Table [dbo].[tblEvaluationGroup_DistribDetail]    Script Date: 4/4/2016 9:05:52 AM ******/
DROP TABLE [dbo].[tblEvaluationGroup_DistribDetail]
GO
/****** Object:  Table [dbo].[tblEvaluationGroup]    Script Date: 4/4/2016 9:05:52 AM ******/
DROP TABLE [dbo].[tblEvaluationGroup]
GO
/****** Object:  Table [dbo].[tblEvaluationGroup]    Script Date: 4/4/2016 9:05:52 AM ******/

/****** Object:  Table [dbo].[tblScenarioAttributes]    Script Date: 4/4/2016 9:10:25 AM ******/
DROP TABLE [dbo].[tblScenarioAttributes]
GO
/****** Object:  Table [dbo].[tblScenario_SpecialOps]    Script Date: 4/4/2016 9:10:25 AM ******/
DROP TABLE [dbo].[tblScenario_SpecialOps]
GO
/****** Object:  Table [dbo].[tblScenario_DistribDetail]    Script Date: 4/4/2016 9:10:25 AM ******/
DROP TABLE [dbo].[tblScenario_DistribDetail]
GO
/****** Object:  Table [dbo].[tblScenario_DistribDetail]    Script Date: 4/4/2016 9:10:25 AM ******/


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEvaluationGroup](
	[EvaluationID] [int] Identity(1,1) NOT NULL,
	[EvaluationLabel] [nvarchar](50) NULL,
	[EvaluationDescription] [nvarchar](255) NULL,
	[DateCreated] [datetime] NULL,
	[LastModified] [datetime] NULL,
	[ProjID_FK] [int] NULL,
	[EvalPrefix] [nvarchar](20) NULL,
	[RESULT_ImportAll] [bit] NOT NULL,
	[ModelFileLocation] [nvarchar](255) NULL,
	[ModelType_ID] [int] NULL,
	[ScenarioID_Baseline_FK] [int] NULL,
	[IsSecondary] [bit] NOT NULL,
	[ReferenceEvalID_FK] [int] NULL,
	[TS_StartDate] [nvarchar](15) NULL,
	[TS_EndDate] [nvarchar](15) NULL,
	[TS_StartHour] [nvarchar](15) NULL,
	[TS_StartMin] [nvarchar](15) NULL,
	[TS_Interval] [int] NULL,
	[TS_Duration] [int] NULL,
	[TS_ValShift] [float] NULL,
	[ModelScenarioFilter_FK] [int] NULL,
	[TS_Interval_Unit] [int] NULL,
	[TSFileIsScen] [bit] NOT NULL,
	[IntermediateResultCode] [int] NULL,
 CONSTRAINT [PK_tblEvaluationGroup] PRIMARY KEY CLUSTERED 
(
	[EvaluationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEvaluationGroup_DistribDetail]    Script Date: 4/4/2016 9:05:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblEvaluationGroup_DistribDetail](
	[ID] [int] Identity(1,1) NOT NULL,
	[EvalGroup_FK] [int] NULL,
	[ElementListID_FK] [int] NULL,
	[Qualifier_DELETEDELETE] [nvarchar](25) NULL,
	[DistribType] [int] NULL,
	[IsSubScenario] [bit] NOT NULL,
	[DistribTotal_FK] [int] NULL,
	[DistribTypeKey] [int] NULL,
	[IsDiffFromBaseline] [bit] NOT NULL,
	[ScaleBy] [float] NULL,
	[DistribTotal_FieldScalar] [float] NULL,
	[DescripNote] [nvarchar](255) NULL,
	[PerformanceRecord_FK] [int] NULL,
	[SSMA_TimeStamp] [binary](8) NOT NULL,
 CONSTRAINT [PK_tblEvaluationGroup_DistribDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblEvaluationGroup_DistribDetail_OrderingList]    Script Date: 4/4/2016 9:05:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEvaluationGroup_DistribDetail_OrderingList](
	[ID] [int] Identity(1,1) NOT NULL,
	[EvalDetailID_FK] [int] NULL,
	[DistribOrderID_FK] [int] NULL,
	[sqn] [int] NULL,
 CONSTRAINT [PK_tblEvaluationGroup_DistribDetail_OrderingList] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblProj]    Script Date: 4/4/2016 9:05:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblProj](
	[ProjID] [int] Identity(1,1) NOT NULL,
	[ProjLabel] [nvarchar](50) NULL,
	[ModelFile_Location] [nvarchar](255) NULL,
	[ModelType_ID] [int] NULL,
	[ModelDescription] [nvarchar](255) NULL,
	[ModelTargetArea] [nvarchar](255) NULL,
	[DateCreated] [datetime] NULL,
	[UserID_FK] [int] NULL,
	[LastModified] [datetime] NULL,
	[RESULT_ImportAll] [bit] NOT NULL,
	[UnitSettings_FK] [int] NULL,
	[DB_Model] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblProj] PRIMARY KEY CLUSTERED 
(
	[ProjID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblScenario]    Script Date: 4/4/2016 9:05:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblScenario](
	[ScenarioID] [int] Identity(1,1) NOT NULL,
	[EvalGroupID_FK] [int] NULL,
	[CreatedBy_User] [nvarchar](255) NULL,
	[UserID_No_DELETE] [int] NULL,
	[ScenarioLabel] [nvarchar](50) NULL,
	[ScenarioDescription] [nvarchar](255) NULL,
	[DateEvaluated] [datetime] NULL,
	[ParentScenario] [int] NULL,
	[COST_Capital] [float] NULL,
	[COST_OM] [float] NULL,
	[COST_Total] [float] NULL,
	[DateCreated] [datetime] NULL,
	[HasBeenRun] [bit] Default 0,
	[DNA] [nvarchar](255) NULL,
	[ScenStart] [int] Default -1,
	[ScenEnd] [int] Default 100,
	[ScenDuration] [int] NULL,
	[ScenLC_LastStage] [int] NULL,
 CONSTRAINT [PK_tblScenario] PRIMARY KEY CLUSTERED 
(
	[ScenarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblScenario_DistribDetail](
	[ID] [int] Identity(1,1) NOT NULL,
	[ScenarioID_FK] [int] NULL,
	[EvalGroup_DistribDetailID_FK] [int] NULL,
	[ExceedsLimit] [bit] NOT NULL,
	[SSMA_TimeStamp] [binary](8) NOT NULL,
 CONSTRAINT [PK_tblScenario_DistribDetail] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tblScenario_SpecialOps]    Script Date: 4/4/2016 9:10:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblScenario_SpecialOps](
	[ID] [int] Identity(1,1) NOT NULL,
	[ScenarioID] [int] NULL,
 CONSTRAINT [PK_tblScenario_SpecialOps] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblScenarioAttributes]    Script Date: 4/4/2016 9:10:25 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblScenarioAttributes](
	[ScenarioAttributeID] [int] Identity(1,1) NOT NULL,
	[ScenarioID_FK] [int] NULL,
	[CatID] [int] NULL,
	[CatID_LinkLibrary_FK] [int] NULL,
	[ElementListID_FK] [int] NULL,
	[val] [float] NULL,
	[NoteDescrip] [nvarchar](255) NULL,
	[SSMA_TimeStamp] [binary](8) NOT NULL,
 CONSTRAINT [PK_tblScenarioAttributes] PRIMARY KEY CLUSTERED 
(
	[ScenarioAttributeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.30', 'Creating schema for Project, Evaluation Group and Scenario Tables'); 