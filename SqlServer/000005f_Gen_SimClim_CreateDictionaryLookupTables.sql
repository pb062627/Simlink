/****** Object:  Table [dbo].[tlkpSIMClimTableDictionary]    Script Date: 4/4/2016 7:30:22 AM ******/
DROP TABLE [dbo].[tlkpSIMClimTableDictionary]
GO
/****** Object:  Table [dbo].[tlkpSIMClimFieldDictionary]    Script Date: 4/4/2016 7:30:22 AM ******/
DROP TABLE [dbo].[tlkpSIMClimFieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpSimClimDictionary]    Script Date: 4/4/2016 7:30:22 AM ******/
DROP TABLE [dbo].[tlkpSimClimDictionary]
GO
/****** Object:  Table [dbo].[tlkpSimClim_ResultsDictionary]    Script Date: 4/4/2016 7:30:22 AM ******/
DROP TABLE [dbo].[tlkpSimClim_ResultsDictionary]
GO


/****** Object:  Table [dbo].[tblSimClimStation]    Script Date: 4/4/2016 7:43:52 AM ******/
DROP TABLE [dbo].[tblSimClimStation]
GO

/****** Object:  Table [dbo].[tlkpSimClim_ResultsDictionary]    Script Date: 4/4/2016 7:30:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSimClim_ResultsDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[ResultsLabel] [nvarchar](50) NULL,
	[ResultHeader] [nvarchar](20) NULL,
	[SC_FieldName] [nvarchar](25) NULL,
	[ResultCat] [int] NULL,
	[IsPerturbation] [bit] NOT NULL,
	[IsGrid] [bit] NOT NULL,
	[FileType] [nvarchar](255) NULL,
	[IsTS] [bit] NOT NULL,
	[DefaultReadIn] [bit] NOT NULL,
 CONSTRAINT [PK_tlkpSimClim_ResultsDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpSimClimDictionary]    Script Date: 4/4/2016 7:30:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSimClimDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[DOMAIN_CAT] [nvarchar](255) NULL,
	[VAL] [int] NULL,
	[ValLabel] [nvarchar](100) NULL,
 CONSTRAINT [PK_tlkpSimClimDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpSIMClimFieldDictionary]    Script Date: 4/4/2016 7:30:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSIMClimFieldDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[TableName_FK] [int] NULL,
	[FieldName] [nvarchar](255) NULL,
	[FieldAlias] [nvarchar](255) NULL,
	[InINP] [float] NULL,
	[FieldINP_Number] [float] NULL,
	[Subtype] [nvarchar](255) NULL,
	[FieldClass] [int] NULL,
	[VarOptionList_FK] [int] NULL,
	[IsLookupList] [int] NULL,
	[RowNo] [float] NULL,
 CONSTRAINT [PK_tlkpSIMClimFieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpSIMClimTableDictionary]    Script Date: 4/4/2016 7:30:22 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSIMClimTableDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[SectionNumber] [float] NULL,
	[TableName] [nvarchar](255) NULL,
	[IsOwnTable] [float] NULL,
	[SectionName] [nvarchar](255) NULL,
	[TableClass] [int] NULL,
	[SectionName_Alias] [nvarchar](255) NULL,
	[KeyColumn] [nvarchar](255) NULL,
 CONSTRAINT [PK_tlkpSIMClimTableDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


/****** Object:  Table [dbo].[tblSimClimStation]    Script Date: 4/4/2016 7:43:52 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSimClimStation](
	[StationID] [int] Identity(1,1) NOT NULL,
	[EvalID_FK] [int] NULL,
	[RefSystemKey] [int] NULL,
	[SiteLabel] [nvarchar](50) NULL,
	[SiteTypeKey] [int] NULL,
	[TimeStepKey] [int] NULL,
	[Longitude] [float] NULL,
	[Latitude] [float] NULL,
	[LocalX] [float] NULL,
	[LocalY] [float] NULL,
	[PolygonFileLocation] [nvarchar](255) NULL,
	[Elevation] [float] NULL,
	[SiteID_External] [int] NULL,
	[SCData_Path] [nvarchar](255) NULL,
	[StoreIntermediateResultsCode] [int] NULL,
	[SSMA_TimeStamp] [binary](8) NOT NULL,
 CONSTRAINT [PK_tblSimClimStation] PRIMARY KEY CLUSTERED 
(
	[StationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO


SET IDENTITY_INSERT [tlkpSimClim_ResultsDictionary] ON
GO

GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (2, N'Precipitation', N'Precip (mm)', N'Precip', 1, 0, 0, NULL, 1, 1)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (3, N'Max Temperature', N'T Max (°C)', N'MaxTemp', 1, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (4, N'Mean Temperature', N'T Mean (°C)', N'MeanTemp', 1, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (5, N'Min Temperature', NULL, N'MinTemp', 1, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (6, N'Solar Radiation', NULL, NULL, 2, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (7, N'Sunshine Hours', NULL, NULL, 2, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (8, N'Relative humidity', NULL, NULL, 2, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (9, N'Sea level', N'Sea Level Rise (mm)', NULL, 1, 0, 0, NULL, 1, 1)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (10, N'River discharge', NULL, NULL, NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (11, N'WindSpeed', NULL, NULL, NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (12, N'Sea Surface Temperature', NULL, N'SeaSurfaceTemp', 1, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (13, N'Precipitation (Perturbation)', N'Precip (mm)', N'Precip', 1, 1, 0, NULL, 1, 1)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (14, N'Max Temperature (Perturbation)', N'T Max (°C)', N'MaxTemp', 1, 1, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (15, N'Mean Temperature (Perturbation)', N'T Mean (°C)', N'MeanTemp', 1, 1, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (16, N'Min Temperature (Perturbation)', NULL, N'MinTemp', 1, 1, 0, NULL, 1, 0)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (17, N'Sea level (Perturbation)', N'Sea Level Rise (mm)', NULL, 1, 1, 0, NULL, 1, 1)
GO
INSERT [dbo].[tlkpSimClim_ResultsDictionary] ([ID], [ResultsLabel], [ResultHeader], [SC_FieldName], [ResultCat], [IsPerturbation], [IsGrid], [FileType], [IsTS], [DefaultReadIn]) VALUES (18, N'Sea Surface Temperature (Perturbation)', NULL, N'SeaSurfaceTemp', 1, 1, 0, NULL, 1, 0)
GO

SET IDENTITY_INSERT [tlkpSimClim_ResultsDictionary] OFF
GO


SET IDENTITY_INSERT [tlkpSimClimDictionary] ON
GO

INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (11, N'SRES', 1, N'Baseline')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (12, N'SRES', 2, N'SRES A1B')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (13, N'SRES', 3, N'SRES AF1')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (14, N'SRES', 4, N'SRES A1T')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (15, N'SRES', 5, N'SRES A2')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (16, N'SRES', 6, N'SRES B1')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (17, N'SRES', 7, N'SRES B2')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (18, N'SENSITIVITY', 1, N'Low')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (19, N'SENSITIVITY', 2, N'Mid')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (20, N'SENSITIVITY', 3, N'High')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (21, N'Version', 1, N'2.5.9.4')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (22, N'GCM', -1, N'BCCRBCM2')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (23, N'GCM', -2, N'CCCMA-31')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (24, N'GCM', -3, N'CCSM--30')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (25, N'GCM', -4, N'CNRM-CM3')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (26, N'GCM', -5, N'CSIRO-30')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (27, N'GCM', -6, N'CSIRO-35')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (28, N'GCM', -7, N'ECHO---G')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (29, N'GCM', -8, N'FGOALS1G')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (30, N'GCM', -9, N'GFDLCM20')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (31, N'GCM', -10, N'GFDLCM21')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (32, N'GCM', -11, N'GISS--EH')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (33, N'GCM', -12, N'GISS--ER')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (34, N'GCM', -13, N'INMCM-30')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (35, N'GCM', -14, N'IPSL_CM4')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (36, N'GCM', -15, N'MIROC-HI')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (37, N'GCM', -16, N'MIROCMED')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (38, N'GCM', -17, N'MPIECH-5')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (39, N'GCM', -18, N'MRI-232A')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (40, N'GCM', -19, N'NCARPCM1')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (41, N'GCM', -20, N'UKHADCM3')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (42, N'GCM', -21, N'UKHADGEM')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (43, N'TIMESTEP', 1, N'Daily')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (44, N'TIMESTEP', 2, N'Hourly')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (45, N'TIMESTEP', 3, N'Monthly')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (46, N'TIMESTEP', 4, N'TenMinutes')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (47, N'TIMESTEP', 5, N'NonClimate')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (48, N'SITE', 1, N'Site')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (49, N'SITE', 2, N'Polygon')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (50, N'IMPORT1', 1, N'Precip/Level')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (51, N'IMPORT1', 2, N'All')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (52, N'IMPORT1', 3, N'Proj Specific')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (53, N'INTERPOLATE', 1, N'Linear')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (54, N'INTERPOLATE', 2, N'Spline')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (56, N'GCM_TYPE', 1, N'Show all')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (57, N'GCM_TYPE', 2, N'GCM')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (58, N'GCM_TYPE', 3, N'Ensemble')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (59, N'ENSEMB_PERC', 0, N'Low Percentage')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (60, N'ENSEMB_PERC', 1, N'Median')
GO
INSERT [dbo].[tlkpSimClimDictionary] ([ID], [DOMAIN_CAT], [VAL], [ValLabel]) VALUES (61, N'ENSEMB_PERC', 2, N'High Percentage')


SET IDENTITY_INSERT [tlkpSimClimDictionary] OFF
GO

SET IDENTITY_INSERT [tlkpSIMClimFieldDictionary] ON
GO

GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (450, 42, N'ScenarioID_FK', N'ScenarioID_FK', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (451, 42, N'SimClimLabel', N'SimClimLabel', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (452, 42, N'ProjectionYear', N'ProjectionYear', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (453, 42, N'GCM_Link', N'GCM_Link', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (454, 42, N'GCM_IsEnsemble', N'GCM_IsEnsemble', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (455, 42, N'SRES_Projection', N'SRES_Projection', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (456, 42, N'ClimateSensitivity', N'ClimateSensitivity', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (457, 42, N'Description', N'Description', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (458, 42, N'DateCreated', N'DateCreated', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (459, 43, N'ScenarioID_FK', N'ScenarioID_FK', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (460, 43, N'RefSystemKey', N'RefSystemKey', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (461, 43, N'SiteLabel', N'SiteLabel', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (462, 43, N'SiteTypeKey', N'SiteTypeKey', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (463, 43, N'TimeStepKey', N'TimeStepKey', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (464, 43, N'Longitude', N'Longitude', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (465, 43, N'Latitude', N'Latitude', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (466, 43, N'LocalX', N'LocalX', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (467, 43, N'LocalY', N'LocalY', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpSIMClimFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (468, 43, N'PolygonFileLocation', N'PolygonFileLocation', 0, NULL, NULL, NULL, NULL, NULL, NULL)
GO

SET IDENTITY_INSERT [tlkpSIMClimFieldDictionary] OFF
GO

SET IDENTITY_INSERT [tlkpSIMClimTableDictionary] ON
GO

INSERT [dbo].[tlkpSIMClimTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn]) VALUES (42, NULL, N'tblSimClimScenario', -1, NULL, NULL, N'SimClim Parameters', N'SimClimScenarioID')
GO
INSERT [dbo].[tlkpSIMClimTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn]) VALUES (43, NULL, N'tblSimClimStation', -1, NULL, NULL, N'Station', N'StationID')
GO

SET IDENTITY_INSERT [tlkpSIMClimTableDictionary] OFF
GO

SET IDENTITY_INSERT [tblSimClimStation] ON
GO

INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (18, 22, NULL, N'AlexSite1', 2, 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x0000000000019E6D)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (19, 22, NULL, N'AlexSite2', 1, 3, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x0000000000019E6E)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (20, 25, NULL, N'LA_AIRPORT', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x0000000000019E6F)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (21, 26, NULL, N'MySite1', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x0000000000019E70)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (22, 26, NULL, N'LA_scen2_site2', 2, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x0000000000019E71)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (23, 25, NULL, N'newStation', 1, 1, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, NULL, 0x0000000000019E72)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (94, 197, NULL, N'Saint Bernard  AL', 1, 1, -86.8133, 34.1736, NULL, NULL, NULL, 243.8, 17157, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E73)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (95, 197, NULL, N'Safford Agricultural Ctr.  AZ ', 1, 1, -109.6808, 32.815, NULL, NULL, NULL, 900.4, 27390, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E74)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (96, -197, NULL, N'Tucson WFO  AZ', 1, 1, -110.9536, 32.2292, NULL, NULL, NULL, 742.2, 28815, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E75)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (97, -197, NULL, N'William  AZ', 1, 1, -112.1903, 35.2406, NULL, NULL, NULL, 2057.4, 29359, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E76)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (98, -197, NULL, N'Los Angeles Int. AP  CA ', 1, 1, -118.3889, 33.9381, NULL, NULL, NULL, 29.6, 45114, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E77)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (99, -197, NULL, N'Yosemite Park Headquarter  CA ', 1, 1, -119.5897, 37.75, NULL, NULL, NULL, 1224.7, 49855, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E78)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (100, -197, NULL, N'San Luis Obispo Poly  CA ', 1, 1, -120.6639, 35.3056, NULL, NULL, NULL, 96, 47851, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E79)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (101, -197, NULL, N'Mt Shasta  CA ', 1, 1, -122.3081, 41.3206, NULL, NULL, NULL, 1094.2, 45983, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E7A)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (102, -197, NULL, N'Tahoe City  CA ', 1, 1, -120.1428, 39.1678, NULL, NULL, NULL, 1898.9, 48758, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E97)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (103, -197, NULL, N'Ft Collins  CO ', 1, 1, -105.1314, 40.6147, NULL, NULL, NULL, 1525.2, 53005, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E98)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (104, -197, NULL, N'Cheyenne Wells  CO ', 1, 1, -102.3486, 38.8236, NULL, NULL, NULL, 1295.4, 51564, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E99)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (105, -197, NULL, N'Cheesman  CO', 1, 1, -105.2783, 39.2203, NULL, NULL, NULL, 2097, 51528, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E9A)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (106, -197, NULL, N'Montrose No. 2  CO', 1, 1, -107.8792, 38.4858, NULL, NULL, NULL, 1764.5, 55722, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E9B)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (107, -197, NULL, N'Perrine 4W  FL', 1, 1, -80.4361, 25.5819, NULL, NULL, NULL, 3, 87020, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E9C)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (108, -197, NULL, N'Tarpon Spgs Sewage Pl  FL', 1, 1, -82.7644, 28.1586, NULL, NULL, NULL, 2.4, 88824, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E9D)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (109, -197, NULL, N'Albany 3 SE  GA', 1, 1, -84.1489, 31.5339, NULL, NULL, NULL, 54.9, 90140, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E9E)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (110, -197, NULL, N'Rock Rapids  IA', 1, 1, -96.1686, 43.43, NULL, NULL, NULL, 411.5, 137147, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E9F)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (111, -197, NULL, N'Moscow University of Idaho  ID ', 1, 1, -116.9608, 46.7244, NULL, NULL, NULL, 810.8, 106152, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA0)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (112, -197, NULL, N'New Meadows Rs  ID', 1, 1, -116.2864, 44.9647, NULL, NULL, NULL, 1179.6, 106388, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA1)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (113, -197, NULL, N'Urbana  IL ', 1, 1, -88.2403, 40.0839, NULL, NULL, NULL, 219.8, 118740, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA2)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (114, -197, NULL, N'Columbus  IN', 1, 1, -85.9211, 39.1978, NULL, NULL, NULL, 189.3, 121747, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA3)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (115, -197, NULL, N'Louisville WSO  KY ', 1, 1, -85.645, 38.115, NULL, NULL, NULL, 193.5, 154954, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA4)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (116, -197, NULL, N'Bowling Green Rgnl AP.  KY ', 1, 1, -86.4239, 36.9647, NULL, NULL, NULL, 160.9, 150909, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA5)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (117, -197, NULL, N'Alexandria  LA', 1, 1, -92.4611, 31.3206, NULL, NULL, NULL, 26.5, 160098, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA6)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (118, -197, NULL, N'Jennings  LA', 1, 1, -92.6642, 30.2003, NULL, NULL, NULL, 7.6, 164700, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA7)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (119, -197, NULL, N'Baton Rouge Metro Ap  LA', 1, 1, -91.1469, 30.5372, NULL, NULL, NULL, 19.5, 160549, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA8)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (120, -197, NULL, N'Amherst  MA ', 1, 1, -72.5375, 42.3861, NULL, NULL, NULL, 45.7, 190120, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EA9)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (121, -197, NULL, N'Boston Logan AP  MA ', 1, 1, -71.0106, 42.3606, NULL, NULL, NULL, 3.7, 190770, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EAA)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (122, -197, NULL, N'Md Sci Ctr Baltimore  MD', 1, 1, -76.61, 39.2811, NULL, NULL, NULL, 6.1, 185718, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EAB)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (123, -197, NULL, N'Ann Arbor University of Michigan  MI ', 1, 1, -83.7108, 42.2947, NULL, NULL, NULL, 274.3, 200230, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EAC)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (124, -197, NULL, N'Pine River Dam  MN ', 1, 1, -94.1089, 46.6694, NULL, NULL, NULL, 381, 216547, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EAD)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (125, -197, NULL, N'Mtn Grove 2N  MO ', 1, 1, -92.2636, 37.1528, NULL, NULL, NULL, 442, 235834, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EAE)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (126, -197, NULL, N'Jefferson City Wtp  MO', 1, 1, -92.1825, 38.585, NULL, NULL, NULL, 204.2, 234271, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EAF)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (127, -197, NULL, N'Steffenville  MO', 1, 1, -91.8872, 39.9714, NULL, NULL, NULL, 210.3, 238051, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EB0)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (128, -197, NULL, N'Neosho  MO', 1, 1, -94.36, 36.8639, NULL, NULL, NULL, 308.2, 235976, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EB1)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (129, -197, NULL, N'Louisville  MS', 1, 1, -89.0711, 33.1353, NULL, NULL, NULL, 177.1, 225247, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EB2)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (130, -197, NULL, N'Columbus  MS', 1, 1, -88.3847, 33.4678, NULL, NULL, NULL, 44.2, 221880, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EB3)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (131, -197, NULL, N'Moccasin Exp. Stn.  MT ', 1, 1, -109.9514, 47.0575, NULL, NULL, NULL, 1310.6, 245761, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EB4)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (132, -197, NULL, N'Tarboro 1 S  NC ', 1, 1, -77.5386, 35.8847, NULL, NULL, NULL, 10.7, 318500, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EB5)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (133, -197, NULL, N'Jamestown State Hospital  ND', 1, 1, -98.685, 46.8844, NULL, NULL, NULL, 447.1, 324418, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EB6)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (134, -197, NULL, N'Gothenburg  NE', 1, 1, -100.1522, 40.94, NULL, NULL, NULL, 787.9, 253365, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EB7)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (135, -197, NULL, N'Atlantic City  NJ ', 1, 1, -74.4242, 39.3792, NULL, NULL, NULL, 3, 280325, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019EB8)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (136, -197, NULL, N'Mountain Park  NM', 1, 1, -105.8242, 32.9547, NULL, NULL, NULL, 2066.5, 295960, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E7B)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (137, -197, NULL, N'Santa Rosa  NM', 1, 1, -104.6805, 34.9358, NULL, NULL, NULL, 1405.1, 298107, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E7C)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (138, -197, NULL, N'Red River  NM', 1, 1, -105.4036, 36.7058, NULL, NULL, NULL, 2644.4, 297323, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E7D)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (139, -197, NULL, N'Port Jervis  NY ', 1, 1, -74.6847, 41.38, NULL, NULL, NULL, 143.3, 306774, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E7E)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (140, -197, NULL, N'Hemlock  NY ', 1, 1, -77.6083, 42.7747, NULL, NULL, NULL, 274.9, 303773, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E7F)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (141, -197, NULL, N'Buffalo  NY', 1, 1, -78.7358, 42.9408, NULL, NULL, NULL, 214.9, 301012, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E80)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (142, -197, NULL, N'Maryland 9 SW  NY', 1, 1, -75.0106, 42.4694, NULL, NULL, NULL, 373.4, 305113, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E81)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (143, -197, NULL, N'Watertown  NY', 1, 1, -75.8753, 43.9761, NULL, NULL, NULL, 151.5, 309000, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E82)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (144, -197, NULL, N'Ada  OK', 1, 1, -96.685, 34.7864, NULL, NULL, NULL, 309.4, 340017, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E83)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (145, -197, NULL, N'North Bend FCWOS  OR ', 1, 1, -124.2436, 43.4133, NULL, NULL, NULL, 1.8, 356073, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E84)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (146, -197, NULL, N'Hood River Exp. Stn.  OR ', 1, 1, -121.5175, 45.6847, NULL, NULL, NULL, 152.4, 354003, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E85)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (147, -197, NULL, N'Charleston City  SC', 1, 1, -79.9319, 32.78, NULL, NULL, NULL, 3, 381549, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E86)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (148, -197, NULL, N'Hot Springs  SD', 1, 1, -103.4739, 43.4378, NULL, NULL, NULL, 1085.1, 394007, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E87)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (149, -197, NULL, N'Plainview  TX', 1, 1, -101.7022, 34.1892, NULL, NULL, NULL, 1027.2, 417079, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E88)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (150, -197, NULL, N'Danevang 1 W  TX', 1, 1, -96.2319, 29.0567, NULL, NULL, NULL, 21.3, 412266, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E89)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (151, -197, NULL, N'San Antonio Intl Ap  TX', 1, 1, -98.47, 29.5333, NULL, NULL, NULL, 246.6, 417945, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E8A)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (152, -197, NULL, N'Danevang 1 W  TX', 1, 1, -96.2319, 29.0567, NULL, NULL, NULL, 21.3, 412266, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E8B)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (153, -197, NULL, N'Liberty  TX', 1, 1, -94.795, 30.0592, NULL, NULL, NULL, 10.7, 415196, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E8C)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (154, -197, NULL, N'El Paso AP  TX', 1, 1, -106.3758, 31.8111, NULL, NULL, NULL, 1194.2, 412797, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E8D)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (155, -197, NULL, N'Logan Utah St. University  UT', 1, 1, -111.8033, 41.7456, NULL, NULL, NULL, 1460, 425186, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E8E)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (156, -197, NULL, N'Spanish Fork Pwr House  UT ', 1, 1, -111.6044, 40.0797, NULL, NULL, NULL, 1438.7, 428119, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E8F)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (157, -197, NULL, N'Vienna  VA  ', 1, 1, -77.2664, 38.9, NULL, NULL, NULL, 127.4, 448737, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E90)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (158, -197, NULL, N'South Lincoln  VT', 1, 1, -72.9736, 44.0725, NULL, NULL, NULL, 408.7, 437612, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E91)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (159, -197, NULL, N'Spokane Intl. AP  WA ', 1, 1, -117.5281, 47.6217, NULL, NULL, NULL, 717.2, 457938, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E92)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (160, -197, NULL, N'Vancouver 4 NNE  WA ', 1, 1, -122.6519, 45.6778, NULL, NULL, NULL, 64, 458773, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E93)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (161, -197, NULL, N'Spooner Ag Res Stn  WI', 1, 1, -91.8761, 45.8236, NULL, NULL, NULL, 335.3, 478027, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E94)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (162, -197, NULL, N'Green River  WY ', 1, 1, -109.4767, 41.5314, NULL, NULL, NULL, 1852.3, 484065, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E95)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (163, -197, NULL, N'Lake Yellowstone  WY ', 1, 1, -110.3986, 44.5619, NULL, NULL, NULL, 2398.8, 485345, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Global (reduced)', NULL, 0x0000000000019E96)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (164, 203, NULL, N'Naperville', 1, -1, -88.1656, 41.7481, NULL, NULL, NULL, NULL, NULL, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Illinois', 1, 0x0000000000019EB9)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (165, 204, NULL, N'Melbourne', 1, -1, 144.97, -37.8075, NULL, NULL, NULL, NULL, 86071, N'C:\Users\mthroneb\Documents\Optimization\SimClim\ScenarioGeneratorAPI_v0_1\Samples\Data\Areas\Vic', 1, 0x0000000000019EBA)
GO
INSERT [dbo].[tblSimClimStation] ([StationID], [EvalID_FK], [RefSystemKey], [SiteLabel], [SiteTypeKey], [TimeStepKey], [Longitude], [Latitude], [LocalX], [LocalY], [PolygonFileLocation], [Elevation], [SiteID_External], [SCData_Path], [StoreIntermediateResultsCode], [SSMA_TimeStamp]) VALUES (166, 255, NULL, N'Naperville', 1, -1, -88.1656, 41.7481, NULL, NULL, NULL, NULL, NULL, N'Illinois', 1, 0x000000000001EC31)
GO


SET IDENTITY_INSERT [tblSimClimStation] OFF
GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.05f', 'Creating schema and data for SimClim dictionary lookup tables and SimClim Stations'); 