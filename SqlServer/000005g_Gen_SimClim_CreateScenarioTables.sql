/****** Object:  Table [dbo].[tblSimClimScenario]    Script Date: 4/4/2016 8:06:16 AM ******/
DROP TABLE [dbo].[tblSimClimScenario]
GO
/****** Object:  Table [dbo].[tblSimClimScenario]    Script Date: 4/4/2016 8:06:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSimClimScenario](
	[SimClimScenarioID] [int] Identity(1,1) NOT NULL,
	[ScenarioID_FK] [int] NULL,
	[ProjectionYearDONOTUSE] [int] NULL,
	[GCM_Link] [int] NULL,
	[GCM_IsEnsemble] [bit] NOT NULL,
	[SRES_Projection] [int] NULL,
	[ClimateSensitivity] [int] NULL,
	[DateCreated] [datetime] NULL,
	[SimClimVersion] [int] NULL,
	[Percentile] [smallint] NULL,
	[IsprimaryScen] [bit] NOT NULL,
	[StoreIntermediateResultsCode] [int] NULL,
	[PercentileLow_Scen] [int] NULL,
	[PercentileHigh_Scen] [int] NULL,
	[SSMA_TimeStamp] [binary](8) NOT NULL,
 CONSTRAINT [PK_tblSimClimScenario] PRIMARY KEY CLUSTERED 
(
	[SimClimScenarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.05g', 'Creating schema for SimClim scenarios'); 