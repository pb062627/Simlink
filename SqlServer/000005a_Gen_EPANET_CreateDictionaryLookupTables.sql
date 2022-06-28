/****** Object:  Table [dbo].[tlkpEPANETFieldDictionary]    Script Date: 3/7/2016 1:21:59 PM ******/
DROP TABLE [dbo].[tlkpEPANETFieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpEPANET_TableDictionary]    Script Date: 3/7/2016 1:21:59 PM ******/
DROP TABLE [dbo].[tlkpEPANET_TableDictionary]
GO
/****** Object:  Table [dbo].[tlkpEPANET_ResultsFieldDictionary]    Script Date: 3/7/2016 1:21:59 PM ******/
DROP TABLE [dbo].[tlkpEPANET_ResultsFieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpEPANET_ResultsFieldDictionary]    Script Date: 3/7/2016 1:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpEPANET_ResultsFieldDictionary](
	[ResultsFieldID] [int] Identity(1,1) NOT NULL,
	[FieldName] [nvarchar](255) NULL,
	[EPANET_Label] [nvarchar](255) NULL,
	[EPANET_CODE1] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[IsNode] [float] NULL,
	[IsTankOnly] [float] NULL,
	[IsResult] [float] NULL,
	[ColumnNo] [int] NULL,
	[FeatureType] [nvarchar](10) NULL,
	
CONSTRAINT [PK_tlkpEPANET_ResultsFieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ResultsFieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) 
ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpEPANET_TableDictionary]    Script Date: 3/7/2016 1:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpEPANET_TableDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[SectionNumber] [float] NULL,
	[TableName] [nvarchar](255) NULL,
	[IsOwnTable] [float] NULL,
	[SectionName] [nvarchar](255) NULL,
	[TableClass] [int] NULL,
	[SectionName_Alias] [nvarchar](255) NULL,
	[KeyColumn] [nvarchar](255) NULL,
	[IsScenarioSpecific] [float] NULL,
	[VariableType] [nvarchar](255) NULL,
	[VariableTypeNum] [int] NULL,
	
CONSTRAINT [PK_tlkpEPANET_TableDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])
 ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpEPANETFieldDictionary]    Script Date: 3/7/2016 1:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpEPANETFieldDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[TableName_FK] [int] NULL,
	[FieldName] [nvarchar](255) NULL,
	[FieldAlias] [nvarchar](255) NULL,
	[InINP] [float] NULL,
	[FieldINP_Number] [float] NULL,
	[Subtype] [nvarchar](255) NULL,
	[CanBeDV] [bit] NOT NULL,
	[FieldClass] [int] NULL,
	[VarOptionList_FK] [int] NULL,
	[IsLookupList] [int] NULL,
	[API_Update] [bit] NOT NULL,
	[EPANET_CSharp_LibInt] [smallint] NULL,
	[RowNo] [float] NULL,
	
CONSTRAINT [PK_tlkpEPANETFieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])
 ON [PRIMARY]

GO

SET IDENTITY_INSERT [tlkpEPANET_ResultsFieldDictionary] ON
GO

INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (1, N'Elevation', N'EN_ELEVATION', 0, N'Elevation', -1, 0, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (2, N'Base demand', N'EN_BASEDEMAND', 1, N'Base demand', -1, 0, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (3, N'Pattern', N'EN_PATTERN', 2, N'Demand pattern index', -1, 0, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (4, N'Emitter Coeff', N'EN_EMITTER', 3, N'Emitter coeff.', -1, 0, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (5, N'Initial quality', N'EN_INITQUAL', 4, N'Initial quality', -1, 0, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (6, N'Source quality', N'EN_SOURCEQUAL', 5, N'Source quality', -1, 0, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (7, N'Source Pattern', N'EN_SOURCEPAT', 6, N'Source pattern index', -1, 0, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (8, N'Source Type', N'EN_SOURCETYPE', 7, N'Source type (See note below)', -1, 0, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (9, N'Initial tank level', N'EN_TANKLEVEL', 8, N'Initial water level in tank', -1, 0, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (10, N'Actual demand', N'EN_DEMAND', 9, N'Actual demand', -1, 0, -1, 0, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (11, N'Hydraulic head', N'EN_HEAD', 10, N'Hydraulic head', -1, 0, -1, 1, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (12, N'Pressure', N'EN_PRESSURE', 11, N'Pressure', -1, 0, -1, 2, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (13, N'Actual quality', N'EN_QUALITY', 12, N'Actual quality', -1, 0, -1, 3, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (14, N'Sourcemass', N'EN_SOURCEMASS', 13, N'Mass flow rate per minute of a chemical source', -1, 0, -1, 4, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (15, N'Initial Volume', N'EN_INITVOLUME', 14, N'Initial water volume', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (16, N'Mixmodel', N'EN_MIXMODEL', 15, N'Mixing model code (see below)', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (17, N'Mixzonevol', N'EN_MIXZONEVOL', 16, N'Inlet/Outlet zone volume in a 2-compartment tank', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (18, N'Tank diameter', N'EN_TANKDIAM', 17, N'Tank diameter', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (19, N'Minvolume', N'EN_MINVOLUME', 18, N'Minimum water volume', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (20, N'Volcurve', N'EN_VOLCURVE', 19, N'Index of volume versus depth curve (0 if none assigned)', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (21, N'Minlevel', N'EN_MINLEVEL', 20, N'Minimum water level', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (22, N'Maxlevel', N'EN_MAXLEVEL', 21, N'Maximum water level', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (23, N'Mixfraction', N'EN_MIXFRACTION', 22, N'Fraction of total volume occupied by the inlet/outlet zone in a 2-compartment tank', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (24, N'Kbulk', N'EN_TANK_KBULK', 23, N'Bulk reaction rate coefficient', -1, -1, 0, NULL, N'Node')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (25, N'Diameter', N'EN_DIAMETER', 0, N'Diameter', 0, 0, 0, NULL, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (26, N'Length', N'EN_LENGTH', 1, N'Length', 0, 0, 0, NULL, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (27, N'Roughness coeff.', N'EN_ROUGHNESS', 2, N'Roughness coeff.', 0, 0, 0, NULL, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (28, N'Minor loss coeff.', N'EN_MINORLOSS', 3, N'Minor loss coeff.', 0, 0, 0, NULL, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (29, N'Initial link status', N'EN_INITSTATUS', 4, N'Initial link status', 0, 0, 0, NULL, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (30, N'Roughness for pipes,', N'EN_INITSETTING', 5, N'Roughness for pipes,', 0, 0, 0, NULL, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (31, N'Bulk reaction coeff.', N'EN_KBULK', 6, N'Bulk reaction coeff.', 0, 0, 0, NULL, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (32, N'Wall reaction coeff.', N'EN_KWALL', 7, N'Wall reaction coeff.', 0, 0, 0, NULL, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (33, N'Flow rate', N'EN_FLOW', 8, N'Flow rate', 0, 0, -1, 0, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (34, N'Flow velocity', N'EN_VELOCITY', 9, N'Flow velocity', 0, 0, -1, 1, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (35, N'Head loss', N'EN_HEADLOSS', 10, N'Head loss', 0, 0, -1, 2, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (36, N'Status', N'EN_STATUS', 11, N'Actual link status', 0, 0, -1, 3, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (37, N'Setting', N'EN_SETTING', 12, N'Roughness for pipes,', 0, 0, -1, 4, N'Link')
GO
INSERT [dbo].[tlkpEPANET_ResultsFieldDictionary] ([ResultsFieldID], [FieldName], [EPANET_Label], [EPANET_CODE1], [Description], [IsNode], [IsTankOnly], [IsResult], [ColumnNo], [FeatureType]) VALUES (38, N'Energy expended in kwatts', N'EN_ENERGY', 13, N'Energy expended in kwatts', 0, 0, -1, 5, N'Link')
GO

SET IDENTITY_INSERT [tlkpEPANET_ResultsFieldDictionary] OFF
Go

SET IDENTITY_INSERT [tlkpEPANET_TableDictionary] ON
Go

INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (1, 1, N'tblEPANET_TITLE', 0, N'TITLE', 2, N'TITLE', N'TITLEID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (2, 2, N'tblEPANET_JUNCTIONS', -1, N'JUNCTIONS', 1, N'JUNCTIONS', N'JunctionID', 0, N'node', 1)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (3, 3, N'tblEPANET_RESERVOIRS', -1, N'RESERVOIRS', 2, N'RESERVOIRS', N'RESERVOIRID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (4, 4, N'tblEPANET_TANKS', -1, N'TANKS', 3, N'TANKS', N'TANKID', 0, N'node', 1)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (5, 5, N'tblEPANET_PIPES', -1, N'PIPES', 4, N'PIPES', N'PIPEID', 0, N'link', 2)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (6, 6, N'tblEPANET_PUMPS', -1, N'PUMPS', 5, N'PUMPS', N'PUMPID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (7, 7, N'tblEPANET_VALVES', -1, N'VALVES', 6, N'VALVES', N'VALVEID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (8, 8, N'tblEPANET_TAGS', -1, N'TAGS', 7, N'TAGS', N'TAGID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (9, 9, N'tblEPANET_DEMANDS', -1, N'DEMANDS', 8, N'DEMANDS', N'DEMANDID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (10, 10, N'tblEPANET_STATUS', -1, N'STATUS', 9, N'STATUS', N'STATUSID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (11, 11, N'tblEPANET_PATTERNS', -1, N'PATTERNS', 10, N'PATTERNS', N'PATTERNID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (12, 12, N'tblEPANET_CURVES', -1, N'CURVES', 11, N'CURVES', N'CURVEID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (13, 13, N'tblEPANET_CONTROLS', 0, N'CONTROLS', 12, N'CONTROLS', N'CONTROLID', 1, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (14, 14, N'tblEPANET_RULES', 0, N'RULES', 13, N'RULES', N'RULEID', 1, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (15, 15, N'tblEPANET_ENERGY', 0, N'ENERGY', 14, N'ENERGY', N'ENERGYID', 1, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (16, 16, N'tblEPANET_EMITTERS', -1, N'EMITTERS', 15, N'EMITTERS', N'EMITTERSID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (17, 17, N'tblEPANET_QUALITY', -1, N'QUALITY', 16, N'QUALITY', N'QUALITYID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (18, 18, N'tblEPANET_SOURCES', -1, N'SOURCES', 17, N'SOURCES', N'SOURCEID', 1, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (19, 19, N'tblEPANET_REACTIONS', -1, N'REACTIONS', 18, N'REACTIONS', N'REACTIONID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (20, 20, N'tblEPANET_MIXING', -1, N'MIXING', 19, N'MIXING', N'MIXINGID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (21, 21, N'tblEPANET_TIMES', 0, N'TIMES', 20, N'TIMES', N'TIMEID', 1, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (22, 22, N'tblEPANET_REPORT', 0, N'REPORT', 21, N'REPORT', N'REPORTID', 1, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (23, 23, N'tblEPANET_OPTIONS', 0, N'OPTIONS', 22, N'OPTIONS', N'OPTIONID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (24, 24, N'tblEPANET_COORDINATES', -1, N'COORDINATES', 23, N'COORDINATES', N'COORDINATEID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (25, 25, N'tblEPANET_VERTICES', -1, N'VERTICES', 24, N'VERTICES', N'VERTICEID', 0, NULL, NULL)
GO
INSERT [dbo].[tlkpEPANET_TableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific], [VariableType], [VariableTypeNum]) VALUES (26, 26, N'tblEPANET_LABELS', -1, N'LABELS', 26, N'LABELS', N'LABELID', 0, NULL, NULL)
GO

SET IDENTITY_INSERT [tlkpEPANET_TableDictionary] OFF
Go

SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] ON
Go

INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (237, 2, N'JunctionID', N'JunctionID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (238, 2, N'ID', N'ID', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (239, 2, N'Elev', N'Elev', 1, 2, NULL, 0, 4, 0, 0, 0, 0, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (240, 2, N'Demand', N'Demand', 1, 3, NULL, 0, 4, 0, 0, 1, 9, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (241, 2, N'Pattern', N'Pattern', 1, 4, NULL, 0, 5, 0, 0, 1, 2, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (242, 3, N'ReservoirID', N'ReservoirID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (243, 4, N'ID', N'ID', 0, 0, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (244, 5, N'Head', N'Head', 1, 0, NULL, 0, 4, 0, 0, 1, 10, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (245, 5, N'Pattern', N'Pattern', 1, 0, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (246, 4, N'ID', N'ID', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (247, 4, N'Elevation', N'Elevation', 1, 2, NULL, 0, 4, 0, 0, 1, 0, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (248, 4, N'InitLevel', N'InitLevel', 1, 3, NULL, 0, 4, 0, 0, 1, 8, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (249, 4, N'MinLevel', N'MinLevel', 1, 4, NULL, 0, 4, 0, 0, 1, 20, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (250, 4, N'MaxLevel', N'MaxLevel', 1, 5, NULL, 0, 4, 0, 0, 1, 21, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (251, 4, N'TankDiameter', N'TankDiameter', 1, 6, NULL, 0, 4, 0, 0, 1, 17, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (252, 4, N'MinVol', N'MinVol', 1, 7, NULL, 0, 4, 0, 0, 1, 18, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (253, 4, N'VolCurve', N'VolCurve', 1, 8, NULL, 0, 4, 0, 0, 1, 19, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (254, 5, N'PipeID', N'PipeID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (255, 5, N'ID              ', N'ID              ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (256, 5, N'Node1           ', N'Node1           ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (257, 5, N'Node2           ', N'Node2           ', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (258, 5, N'Length      ', N'Length      ', 1, 4, NULL, 0, 4, 0, 0, 1, 1, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (259, 5, N'Diameter    ', N'Diameter    ', 1, 5, NULL, 0, 4, 0, 0, 1, 0, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (260, 5, N'Roughness   ', N'Roughness   ', 1, 6, NULL, 0, 4, 0, 0, 1, 2, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (261, 5, N'MinorLoss   ', N'MinorLoss   ', 1, 7, NULL, 0, 4, 0, 0, 1, 3, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (262, 5, N'Status', N'Status', 1, 8, NULL, 0, 4, 0, 0, 1, 4, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (263, 6, N'PumpID', N'PumpID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (264, 6, N'ID              ', N'ID              ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (265, 6, N'Node1           ', N'Node1           ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (266, 6, N'Node2           ', N'Node2           ', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (267, 6, N'Parameters_', N'Parameters', 1, 4, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (268, 7, N'ValveID', N'ValveID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (269, 7, N'ID              ', N'ID              ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (270, 7, N'Node1           ', N'Node1           ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (271, 7, N'Node2           ', N'Node2           ', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (272, 7, N'Diameter    ', N'Diameter    ', 1, 4, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (273, 7, N'Type', N'Type', 1, 5, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (274, 7, N'Setting     ', N'Setting     ', 1, 6, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (275, 7, N'MinorLoss ', N'MinorLoss ', 1, 7, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (276, 9, N'DemandID', N'DemandID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (277, 9, N'Junction        ', N'Junction        ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (278, 9, N'Demand      ', N'Demand      ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (279, 9, N'Pattern         ', N'Pattern         ', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (280, 9, N'Category', N'Category', 1, 4, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (281, 10, N'StatusID', N'StatusID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (282, 10, N'ID              ', N'ID              ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (283, 10, N'StatusSetting', N'StatusSetting', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (284, 11, N'PatternsID', N'PatternsID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (285, 11, N'ID', N'ID', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (286, 11, N'Multipliers1', N'Multipliers1', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (287, 11, N'Multipliers2', N'Multipliers2', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (288, 11, N'Multipliers3', N'Multipliers3', 1, 4, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (289, 11, N'Multipliers4', N'Multipliers4', 1, 5, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (290, 11, N'Multipliers5', N'Multipliers5', 1, 6, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (291, 11, N'Multipliers6', N'Multipliers6', 1, 7, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (292, 12, N'CurveID', N'CurveID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (293, 12, N'ID              ', N'ID              ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (294, 12, N'X-Value     ', N'X-Value     ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (295, 12, N'Y-Value', N'Y-Value', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (296, 16, N'EmittersID', N'EmittersID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (297, 16, N'Junction        ', N'Junction        ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (298, 16, N'Coefficient', N'Coefficient', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (299, 17, N'QualityID', N'QualityID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (300, 17, N'Node', N'Node', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (301, 17, N'InitQual', N'InitQual', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (302, 18, N'SourceID', N'SourceID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (303, 18, N'Node            ', N'Node            ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (304, 18, N'Type        ', N'Type        ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (305, 18, N'Quality     ', N'Quality     ', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (306, 18, N'Pattern', N'Pattern', 1, 4, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (307, 19, N'ReactionsID', N'ReactionsID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (308, 19, N'Type     ', N'Type     ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (309, 19, N'PipeTank       ', N'PipeTank       ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (310, 19, N'Coefficient', N'Coefficient', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (311, 20, N'MixingID', N'MixingID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (312, 20, N'Tank            ', N'Tank            ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (313, 20, N'Model', N'Model', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (314, 24, N'CoordinateID', N'CoordinateID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (315, 24, N'Node            ', N'Node            ', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (316, 24, N'XCoord         ', N'X-Coord         ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (317, 24, N'YCoord', N'Y-Coord', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (318, 25, N'VerticesID', N'VerticesID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (319, 25, N'Link', N'Link', 1, 1, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (320, 25, N'XCoord         ', N'X-Coord         ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (321, 25, N'YCoord', N'Y-Coord', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (322, 26, N'LabelID', N'LabelID', 0, 0, NULL, 0, 1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (323, 26, N'XCoord         ', N'X-Coord         ', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (324, 26, N'YCoord', N'Y-Coord', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (325, 26, N'LabelAnchorNode', N'LabelAnchorNode', 1, 4, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (326, 3, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (327, 5, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (328, 6, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (329, 7, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (330, 9, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (331, 10, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (332, 11, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (333, 12, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (334, 16, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (335, 17, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (336, 18, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (337, 19, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (338, 20, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (339, 24, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (340, 25, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (341, 26, N'ModelVersion', N'ModelVersion', 0, -1, NULL, 0, 3, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (342, 2, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (343, 3, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (344, 5, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (345, 6, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (346, 7, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (347, 9, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (348, 10, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (349, 11, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (350, 12, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (351, 16, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (352, 17, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (353, 18, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (354, 19, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (355, 20, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (356, 24, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (357, 25, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (358, 26, N'Description', N'Description', 0, -1, NULL, 0, 7, 0, 0, 0, NULL, 1)
GO

SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] off
Go

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.05a', 'Creating schema and data for EPANET dictionary lookup tables'); 