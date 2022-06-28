/****** Object:  Table [dbo].[tlkpIWTableDictionary_Key]    Script Date: 4/11/2016 5:02:47 PM ******/
DROP TABLE [dbo].[tlkpIWTableDictionary_Key]
GO
/****** Object:  Table [dbo].[tlkpIWTableDictionary]    Script Date: 4/11/2016 5:02:47 PM ******/
DROP TABLE [dbo].[tlkpIWTableDictionary]
GO
/****** Object:  Table [dbo].[tlkpIWResults_FieldDictionary]    Script Date: 4/11/2016 5:02:47 PM ******/
DROP TABLE [dbo].[tlkpIWResults_FieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpIWFieldDictionary]    Script Date: 4/11/2016 5:02:47 PM ******/
DROP TABLE [dbo].[tlkpIWFieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpIW_Dictionary]    Script Date: 4/11/2016 5:02:47 PM ******/
DROP TABLE [dbo].[tlkpIW_Dictionary]
GO
/****** Object:  Table [dbo].[tlkpIW_Dictionary]    Script Date: 4/11/2016 5:02:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpIW_Dictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[Qualifier] [nvarchar](255) NULL,
	[val] [nvarchar](255) NULL,
	
CONSTRAINT [PK_tlkpIW_Dictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpIWFieldDictionary]    Script Date: 4/11/2016 5:02:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpIWFieldDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[TableName_FK] [int] NULL,
	[FieldName] [nvarchar](255) NULL,
	[FieldAlias] [nvarchar](255) NULL,
	[Subtype] [nvarchar](255) NULL,
	[FieldClass] [int] NULL,
	[InIWM] [float] NULL,
	
CONSTRAINT [PK_tlkpIWFieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpIWResults_FieldDictionary]    Script Date: 4/11/2016 5:02:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpIWResults_FieldDictionary](
	[ResultsFieldID] [int] Identity(1,1) NOT NULL,
	[FeatureType] [nvarchar](255) NULL,
	[FieldName] [nvarchar](255) NULL,
	[FieldAlias] [nvarchar](255) NULL,
	[ColumnNo] [float] NULL,
	[AltColumnNo] [int] NULL,
	[UnitType] [int] NULL,
	
CONSTRAINT [PK_tlkpIWResults_FieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ResultsFieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpIWTableDictionary]    Script Date: 4/11/2016 5:02:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpIWTableDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[TableName] [nvarchar](255) NULL,
	[IsOwnTable] [float] NULL,
	[ComponentName] [nvarchar](255) NULL,
	[TableClass] [int] NULL,
	[ComponentName_Alias] [nvarchar](255) NULL,
	[KeyColumn] [nvarchar](255) NULL,
	
CONSTRAINT [PK_tlkpIWTableDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpIWTableDictionary_Key]    Script Date: 4/11/2016 5:02:47 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpIWTableDictionary_Key](
	[ID_TableClass] [int] Identity(1,1) NOT NULL,
	[TableClass_Definition] [nvarchar](255) NULL,
	[OneValPerScenario] [bit] NOT NULL,
	
CONSTRAINT [PK_tlkpIWTableDictionary_Key] PRIMARY KEY CLUSTERED 
(
	[ID_TableClass] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


SET IDENTITY_INSERT [tlkpIW_Dictionary] ON
GO

GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (1, N'Run', N'IncludeRTC')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (2, N'Run', N'Gauges')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (3, N'Run', N'Ground Infiltration')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (4, N'Run', N'IncludeBaseFlow')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (5, N'Run', N'IncludeLevel')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (6, N'Run', N'IncludeNode')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (7, N'Run', N'IncludeRainfall')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (8, N'Run', N'EveryNode')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (9, N'Run', N'IncludeRunoff')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (10, N'Run', N'IncludeTradeFlow')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (11, N'Run', N'IncludeWastewater')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (12, N'Run', N'IncludeOutflow')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (13, N'Run', N'GaugeMultiplier')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (14, N'Run', N'EveryOutflow')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (15, N'Run', N'DWFMultiplier')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (16, N'Run', N'DWFModeResults')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (17, N'Run', N'DWFDefinition')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (18, N'Run', N'DurationUnit')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (19, N'Run', N'Duration')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (20, N'Run', N'Disposition')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (21, N'Run', N'DeleteTVDAfterStatistics')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (22, N'Run', N'Comment')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (23, N'Run', N'CheckPumps')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (24, N'Run', N'LevelLag')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (25, N'Run', N'EverySubcatchment')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (26, N'Run', N'SubcatchmentLag')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (27, N'Run', N'Save Final State')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (28, N'Run', N'Sediment Fraction Enabled')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (29, N'Run', N'Start Time')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (30, N'Run', N'StatisticsTemplate')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (31, N'Run', N'StopOnEndOfTimeVaryingData')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (32, N'Run', N'StoreDetails')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (33, N'Run', N'StoreMax')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (34, N'Run', N'RTCLag')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (35, N'Run', N'Level')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (36, N'Run', N'StormDefinition')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (37, N'Run', N'Inflow')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (38, N'Run', N'TimeSeriesSimulation')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (39, N'Run', N'TimeStep')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (40, N'Run', N'TimestepLog')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (41, N'Run', N'Trade Waste')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (42, N'Run', N'UseQM')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (43, N'Run', N'Waste Water')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (44, N'Run', N'StorePRN')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (45, N'Run', N'Pollutant Graph')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (46, N'Run', N'SubcatchmentThreshold')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (47, N'Run', N'LevelThreshold')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (49, N'Run', N'NetworkCheckedOutRevisionID')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (50, N'Run', N'NodeLag')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (51, N'Run', N'NodeThreshold')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (52, N'Run', N'OutflowLag')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (53, N'Run', N'RTC Scenario')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (54, N'Run', N'Pipe Sediment Data')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (55, N'Run', N'QM Dependent Fractions')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (56, N'Run', N'QM Hydraulic Feedback')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (57, N'Run', N'QM Multiplier')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (58, N'Run', N'QM Native Washoff Routing')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (59, N'Run', N'QM Pollutant Enabled')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (60, N'Run', N'RainfallLag')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (61, N'Run', N'RainfallThreshold')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (62, N'Run', N'ResultsMultiplier')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (63, N'Run', N'OutflowThreshold')
GO
INSERT [dbo].[tlkpIW_Dictionary] ([ID], [Qualifier], [val]) VALUES (64, N'Run', N'Experimental')


SET IDENTITY_INSERT [tlkpIW_Dictionary] OFF
GO

SET IDENTITY_INSERT [tlkpIWFieldDictionary] ON
GO

GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (234, 1, N'ConduitID', N'ConduitID', NULL, 1, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (235, 1, N'capacity', N'capacity', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (236, 1, N'conduit_height', N'conduit_height', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (237, 1, N'conduit_height_flag', N'conduit_height_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (238, 1, N'conduit_length', N'conduit_length', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (239, 1, N'conduit_length_flag', N'conduit_length_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (240, 1, N'conduit_material', N'conduit_material', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (241, 1, N'conduit_material_flag', N'conduit_material_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (242, 1, N'conduit_width', N'conduit_width', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (243, 1, N'conduit_width_flag', N'conduit_width_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (244, 1, N'critical_sewer_category', N'critical_sewer_category', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (245, 1, N'critical_sewer_category_flag', N'critical_sewer_category_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (246, 1, N'design_group', N'design_group', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (247, 1, N'design_group_flag', N'design_group_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (248, 1, N'ds_headloss_coeff', N'ds_headloss_coeff', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (249, 1, N'ds_headloss_coeff_flag', N'ds_headloss_coeff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (250, 1, N'ds_headloss_type', N'ds_headloss_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (251, 1, N'ds_headloss_type_flag', N'ds_headloss_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (252, 1, N'ds_invert', N'ds_invert', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (253, 1, N'ds_invert_flag', N'ds_invert_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (254, 1, N'ds_node_id', N'ds_node_id', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (255, 1, N'ds_settlement_eff', N'ds_settlement_eff', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (256, 1, N'ds_settlement_eff_flag', N'ds_settlement_eff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (257, 1, N'excluded', N'excluded', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (258, 1, N'gradient', N'gradient', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (259, 1, N'ground_condition', N'ground_condition', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (260, 1, N'ground_condition_flag', N'ground_condition_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (261, 1, N'inflow', N'inflow', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (262, 1, N'inflow_flag', N'inflow_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (263, 1, N'link_suffix', N'link_suffix', NULL, 2, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (264, 1, N'link_type', N'link_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (265, 1, N'merge_details', N'merge_details', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (266, 1, N'merge_network_GUID', N'merge_network_GUID', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (267, 1, N'min_computational_nodes', N'min_computational_nodes', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (268, 1, N'min_computational_nodes_flag', N'min_computational_nodes_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (269, 1, N'notes', N'notes', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (270, 1, N'point_array', N'point_array', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (271, 1, N'roughness_type', N'roughness_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (272, 1, N'roughness_type_flag', N'roughness_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (273, 1, N'sediment_depth', N'sediment_depth', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (274, 1, N'sediment_depth_flag', N'sediment_depth_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (275, 1, N'sediment_type', N'sediment_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (276, 1, N'sediment_type_flag', N'sediment_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (277, 1, N'sewer_reference', N'sewer_reference', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (278, 1, N'shape', N'shape', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (279, 1, N'shape_flag', N'shape_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (280, 1, N'site_condition', N'site_condition', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (281, 1, N'site_condition_flag', N'site_condition_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (282, 1, N'solution_model_flag', N'solution_model_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (283, 1, N'system_type', N'system_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (284, 1, N'system_type_flag', N'system_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (285, 1, N'taking_off_reference', N'taking_off_reference', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (286, 1, N'taking_off_reference_flag', N'taking_off_reference_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (287, 1, N'us_headloss_coeff', N'us_headloss_coeff', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (288, 1, N'us_headloss_coeff_flag', N'us_headloss_coeff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (289, 1, N'us_headloss_type', N'us_headloss_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (290, 1, N'us_headloss_type_flag', N'us_headloss_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (291, 1, N'us_invert', N'us_invert', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (292, 1, N'us_invert_flag', N'us_invert_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (293, 1, N'us_node_id', N'us_node_id', NULL, 2, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (294, 1, N'us_settlement_eff', N'us_settlement_eff', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (295, 1, N'us_settlement_eff_flag', N'us_settlement_eff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (296, 1, N'user_number_1', N'user_number_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (297, 1, N'user_number_1_flag', N'user_number_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (298, 1, N'user_number_2', N'user_number_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (299, 1, N'user_number_2_flag', N'user_number_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (300, 1, N'user_text_1_flag', N'user_text_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (301, 1, N'user_text_2_flag', N'user_text_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (302, 1, N'hotlinks', N'hotlinks', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (303, 1, N'asset_uid', N'asset_uid', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (304, 1, N'infonet_ds_node_id', N'infonet_ds_node_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (305, 1, N'infonet_link_suffix', N'infonet_link_suffix', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (306, 1, N'infonet_us_node_id', N'infonet_us_node_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (307, 1, N'branch_id', N'branch_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (308, 1, N'branch_id_flag', N'branch_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (309, 1, N'user_number_3', N'user_number_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (310, 1, N'user_number_3_flag', N'user_number_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (311, 1, N'user_number_4', N'user_number_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (312, 1, N'user_number_4_flag', N'user_number_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (313, 1, N'user_number_5', N'user_number_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (314, 1, N'user_number_5_flag', N'user_number_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (315, 1, N'user_text_3_flag', N'user_text_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (316, 1, N'user_text_4_flag', N'user_text_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (317, 1, N'user_text_5_flag', N'user_text_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (318, 1, N'user_text_1', N'user_text_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (319, 1, N'user_text_2', N'user_text_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (320, 1, N'user_text_3', N'user_text_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (321, 1, N'user_text_4', N'user_text_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (322, 1, N'user_text_5', N'user_text_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (323, 1, N'base_height', N'base_height', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (324, 1, N'base_height_flag', N'base_height_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (325, 1, N'fill_material_conductivity', N'fill_material_conductivity', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (326, 1, N'fill_material_conductivity_flag', N'fill_material_conductivity_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (327, 1, N'infiltration_coeff_base', N'infiltration_coeff_base', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (328, 1, N'infiltration_coeff_base_flag', N'infiltration_coeff_base_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (329, 1, N'infiltration_coeff_side', N'infiltration_coeff_side', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (330, 1, N'infiltration_coeff_side_flag', N'infiltration_coeff_side_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (331, 1, N'porosity', N'porosity', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (332, 1, N'porosity_flag', N'porosity_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (333, 1, N'solution_model', N'solution_model', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (334, 1, N'bottom_roughness_CW', N'bottom_roughness_CW', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (335, 1, N'bottom_roughness_CW_flag', N'bottom_roughness_CW_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (336, 1, N'bottom_roughness_Manning', N'bottom_roughness_Manning', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (337, 1, N'bottom_roughness_Manning_flag', N'bottom_roughness_Manning_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (338, 1, N'top_roughness_CW', N'top_roughness_CW', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (339, 1, N'top_roughness_CW_flag', N'top_roughness_CW_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (340, 1, N'top_roughness_Manning', N'top_roughness_Manning', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (341, 1, N'top_roughness_Manning_flag', N'top_roughness_Manning_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (342, 1, N'asset_id_flag', N'asset_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (343, 1, N'ds_node_id_flag', N'ds_node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (344, 1, N'us_node_id_flag', N'us_node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (345, 1, N'Version', N'Version', NULL, 2, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (346, 1, N'asset_id', N'asset_id', NULL, 2, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (347, 1, N'ModelVersion', N'ModelVersion', NULL, 3, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (348, 1, N'Description', N'Description', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (349, 2, N'NodeID', N'NodeID', NULL, 1, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (350, 2, N'chamber_area', N'chamber_area', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (351, 2, N'chamber_area_flag', N'chamber_area_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (352, 2, N'chamber_floor', N'chamber_floor', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (353, 2, N'chamber_floor_flag', N'chamber_floor_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (354, 2, N'chamber_roof', N'chamber_roof', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (355, 2, N'chamber_roof_flag', N'chamber_roof_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (356, 2, N'excluded', N'excluded', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (357, 2, N'flood_area_1', N'flood_area_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (358, 2, N'flood_area_1_flag', N'flood_area_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (359, 2, N'flood_area_2', N'flood_area_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (360, 2, N'flood_area_2_flag', N'flood_area_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (361, 2, N'flood_depth_1', N'flood_depth_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (362, 2, N'flood_depth_1_flag', N'flood_depth_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (363, 2, N'flood_depth_2', N'flood_depth_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (364, 2, N'flood_depth_2_flag', N'flood_depth_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (365, 2, N'flood_level', N'flood_level', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (366, 2, N'flood_level_flag', N'flood_level_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (367, 2, N'flood_type', N'flood_type', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (368, 2, N'flood_type_flag', N'flood_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (369, 2, N'floodable_area', N'floodable_area', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (370, 2, N'floodable_area_flag', N'floodable_area_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (371, 2, N'ground_level', N'ground_level', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (372, 2, N'ground_level_flag', N'ground_level_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (373, 2, N'node_id', N'node_id', NULL, 2, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (374, 2, N'node_type', N'node_type', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (375, 2, N'notes', N'notes', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (376, 2, N'shaft_area', N'shaft_area', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (377, 2, N'shaft_area_flag', N'shaft_area_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (378, 2, N'storage_area_array', N'storage_area_array', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (379, 2, N'storage_level_array', N'storage_level_array', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (380, 2, N'system_type', N'system_type', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (381, 2, N'system_type_flag', N'system_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (382, 2, N'user_number_1', N'user_number_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (383, 2, N'user_number_1_flag', N'user_number_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (384, 2, N'user_number_2', N'user_number_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (385, 2, N'user_number_2_flag', N'user_number_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (386, 2, N'user_text_1_flag', N'user_text_1_flag', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (387, 2, N'user_text_2_flag', N'user_text_2_flag', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (388, 2, N'x', N'x', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (389, 2, N'x_flag', N'x_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (390, 2, N'y', N'y', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (391, 2, N'y_flag', N'y_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (392, 2, N'chamber_area_additional', N'chamber_area_additional', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (393, 2, N'chamber_area_additional_flag', N'chamber_area_additional_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (394, 2, N'shaft_area_additional', N'shaft_area_additional', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (395, 2, N'shaft_area_additional_flag', N'shaft_area_additional_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (396, 2, N'base_area', N'base_area', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (397, 2, N'base_area_flag', N'base_area_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (398, 2, N'infiltration_coeff', N'infiltration_coeff', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (399, 2, N'infiltration_coeff_flag', N'infiltration_coeff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (400, 2, N'perimeter', N'perimeter', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (401, 2, N'perimeter_flag', N'perimeter_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (402, 2, N'porosity', N'porosity', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (403, 2, N'porosity_flag', N'porosity_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (404, 2, N'hotlinks', N'hotlinks', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (405, 2, N'chamber_area_add_comp', N'chamber_area_add_comp', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (406, 2, N'chamber_area_add_comp_flag', N'chamber_area_add_comp_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (407, 2, N'chamber_area_add_ncorrect', N'chamber_area_add_ncorrect', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (408, 2, N'chamber_area_add_simplify', N'chamber_area_add_simplify', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (409, 2, N'chamber_area_add_simplify_flag', N'chamber_area_add_simplify_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (410, 2, N'shaft_area_add_comp', N'shaft_area_add_comp', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (411, 2, N'shaft_area_add_comp_flag', N'shaft_area_add_comp_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (412, 2, N'shaft_area_add_ncorrect', N'shaft_area_add_ncorrect', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (413, 2, N'shaft_area_add_ncorrect_flag', N'shaft_area_add_ncorrect_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (414, 2, N'shaft_area_add_simplify', N'shaft_area_add_simplify', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (415, 2, N'shaft_area_add_simplify_flag', N'shaft_area_add_simplify_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (416, 2, N'chamber_area_add_ncorrect_flag', N'chamber_area_add_ncorrect_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (417, 2, N'asset_uid', N'asset_uid', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (418, 2, N'infonet_id', N'infonet_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (419, 2, N'head_discharge_id', N'head_discharge_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (420, 2, N'n_gullies', N'n_gullies', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (421, 2, N'n_gullies_flag', N'n_gullies_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (422, 2, N'relative_stages', N'relative_stages', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (423, 2, N'user_number_3', N'user_number_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (424, 2, N'user_number_3_flag', N'user_number_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (425, 2, N'user_number_4', N'user_number_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (426, 2, N'user_number_4_flag', N'user_number_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (427, 2, N'user_number_5', N'user_number_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (428, 2, N'user_number_5_flag', N'user_number_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (429, 2, N'user_text_3_flag', N'user_text_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (430, 2, N'user_text_4_flag', N'user_text_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (431, 2, N'user_text_5_flag', N'user_text_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (432, 2, N'user_text_1', N'user_text_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (433, 2, N'user_text_2', N'user_text_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (434, 2, N'user_text_3', N'user_text_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (435, 2, N'user_text_4', N'user_text_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (436, 2, N'user_text_5', N'user_text_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (437, 2, N'infiltratn_coeff_abv_liner_flag', N'infiltratn_coeff_abv_liner_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (438, 2, N'infiltratn_coeff_abv_vegn', N'infiltratn_coeff_abv_vegn', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (439, 2, N'infiltratn_coeff_abv_vegn_flag', N'infiltratn_coeff_abv_vegn_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (440, 2, N'infiltratn_coeff_blw_liner', N'infiltratn_coeff_blw_liner', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (441, 2, N'infiltratn_coeff_blw_liner_flag', N'infiltratn_coeff_blw_liner_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (442, 2, N'liner_level', N'liner_level', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (443, 2, N'liner_level_flag', N'liner_level_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (444, 2, N'perimeter_array', N'perimeter_array', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (445, 2, N'vegetation_level', N'vegetation_level', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (446, 2, N'vegetation_level_flag', N'vegetation_level_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (447, 2, N'infiltratn_coeff_abv_liner', N'infiltratn_coeff_abv_liner', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (448, 2, N'flooding_discharge_coeff', N'flooding_discharge_coeff', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (449, 2, N'flooding_discharge_coeff_flag', N'flooding_discharge_coeff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (450, 2, N'asset_id_flag', N'asset_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (451, 2, N'node_id_flag', N'node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (452, 2, N'node_type_flag', N'node_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (453, 2, N'Version', N'Version', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (454, 2, N'asset_id', N'asset_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (455, 2, N'ModelVersion', N'ModelVersion', NULL, 3, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (456, 2, N'Description', N'Description', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (457, 3, N'WeirID', N'WeirID', NULL, 1, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (458, 3, N'crest', N'crest', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (459, 3, N'crest_flag', N'crest_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (460, 3, N'discharge_coeff', N'discharge_coeff', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (461, 3, N'discharge_coeff_flag', N'discharge_coeff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (462, 3, N'ds_node_id', N'ds_node_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (463, 3, N'excluded', N'excluded', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (464, 3, N'height', N'height', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (465, 3, N'height_flag', N'height_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (466, 3, N'link_suffix', N'link_suffix', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (467, 3, N'link_type', N'link_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (468, 3, N'maximum_value', N'maximum_value', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (469, 3, N'maximum_value_flag', N'maximum_value_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (470, 3, N'minimum_value', N'minimum_value', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (471, 3, N'minimum_value_flag', N'minimum_value_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (472, 3, N'negative_speed', N'negative_speed', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (473, 3, N'negative_speed_flag', N'negative_speed_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (474, 3, N'notes', N'notes', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (475, 3, N'point_array', N'point_array', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (476, 3, N'positive_speed', N'positive_speed', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (477, 3, N'positive_speed_flag', N'positive_speed_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (478, 3, N'secondary_discharge_coeff', N'secondary_discharge_coeff', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (479, 3, N'secondary_discharge_coeff_flag', N'secondary_discharge_coeff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (480, 3, N'sewer_reference', N'sewer_reference', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (481, 3, N'system_type', N'system_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (482, 3, N'system_type_flag', N'system_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (483, 3, N'threshold', N'threshold', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (484, 3, N'threshold_flag', N'threshold_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (485, 3, N'us_node_id', N'us_node_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (486, 3, N'user_number_1', N'user_number_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (487, 3, N'user_number_1_flag', N'user_number_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (488, 3, N'user_number_2', N'user_number_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (489, 3, N'user_number_2_flag', N'user_number_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (490, 3, N'user_text_1_flag', N'user_text_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (491, 3, N'user_text_2_flag', N'user_text_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (492, 3, N'width', N'width', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (493, 3, N'width_flag', N'width_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (494, 3, N'ds_settlement_eff', N'ds_settlement_eff', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (495, 3, N'ds_settlement_eff_flag', N'ds_settlement_eff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (496, 3, N'us_settlement_eff', N'us_settlement_eff', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (497, 3, N'us_settlement_eff_flag', N'us_settlement_eff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (498, 3, N'length', N'length', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (499, 3, N'length_flag', N'length_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (500, 3, N'notch_angle', N'notch_angle', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (501, 3, N'notch_angle_flag', N'notch_angle_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (502, 3, N'notch_height', N'notch_height', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (503, 3, N'notch_height_flag', N'notch_height_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (504, 3, N'notch_width', N'notch_width', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (505, 3, N'notch_width_flag', N'notch_width_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (506, 3, N'number_of_notches', N'number_of_notches', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (507, 3, N'number_of_notches_flag', N'number_of_notches_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (508, 3, N'hotlinks', N'hotlinks', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (509, 3, N'asset_uid', N'asset_uid', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (510, 3, N'infonet_id', N'infonet_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (511, 3, N'branch_id', N'branch_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (512, 3, N'branch_id_flag', N'branch_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (513, 3, N'user_number_3', N'user_number_3', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (514, 3, N'user_number_3_flag', N'user_number_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (515, 3, N'user_number_4', N'user_number_4', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (516, 3, N'user_number_4_flag', N'user_number_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (517, 3, N'user_number_5', N'user_number_5', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (518, 3, N'user_number_5_flag', N'user_number_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (519, 3, N'user_text_3_flag', N'user_text_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (520, 3, N'user_text_4_flag', N'user_text_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (521, 3, N'user_text_5_flag', N'user_text_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (522, 3, N'user_text_1', N'user_text_1', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (523, 3, N'user_text_2', N'user_text_2', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (524, 3, N'user_text_3', N'user_text_3', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (525, 3, N'user_text_4', N'user_text_4', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (526, 3, N'user_text_5', N'user_text_5', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (527, 3, N'asset_id_flag', N'asset_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (528, 3, N'ds_node_id_flag', N'ds_node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (529, 3, N'us_node_id_flag', N'us_node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (530, 3, N'Version', N'Version', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (531, 3, N'asset_id', N'asset_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (532, 3, N'ModelVersion', N'ModelVersion', NULL, 3, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (533, 3, N'Description', N'Description', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (534, 4, N'SubcatchmentID', N'SubcatchmentID', NULL, 1, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (535, 4, N'area_measurement_type_flag', N'area_measurement_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (536, 4, N'base_flow', N'base_flow', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (537, 4, N'base_flow_flag', N'base_flow_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (538, 4, N'boundary_array', N'boundary_array', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (539, 4, N'catchment_dimension', N'catchment_dimension', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (540, 4, N'catchment_dimension_flag', N'catchment_dimension_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (541, 4, N'catchment_slope', N'catchment_slope', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (542, 4, N'catchment_slope_flag', N'catchment_slope_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (543, 4, N'connectivity', N'connectivity', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (544, 4, N'connectivity_flag', N'connectivity_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (545, 4, N'contributing_area', N'contributing_area', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (546, 4, N'contributing_area_flag', N'contributing_area_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (547, 4, N'excluded', N'excluded', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (548, 4, N'land_use_id', N'land_use_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (549, 4, N'node_id', N'node_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (550, 4, N'notes', N'notes', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (551, 4, N'population', N'population', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (552, 4, N'population_flag', N'population_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (553, 4, N'rainfall_profile', N'rainfall_profile', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (554, 4, N'rainfall_profile_flag', N'rainfall_profile_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (555, 4, N'soil_class', N'soil_class', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (556, 4, N'soil_class_flag', N'soil_class_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (557, 4, N'subcatchment_id', N'subcatchment_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (558, 4, N'system_type', N'system_type', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (559, 4, N'system_type_flag', N'system_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (560, 4, N'total_area', N'total_area', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (561, 4, N'total_area_flag', N'total_area_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (562, 4, N'trade_flow', N'trade_flow', NULL, 1, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (563, 4, N'trade_flow_flag', N'trade_flow_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (564, 4, N'trade_profile', N'trade_profile', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (565, 4, N'trade_profile_flag', N'trade_profile_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (566, 4, N'user_number_1', N'user_number_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (567, 4, N'user_number_1_flag', N'user_number_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (568, 4, N'user_text_1_flag', N'user_text_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (569, 4, N'wastewater_profile', N'wastewater_profile', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (570, 4, N'wastewater_profile_flag', N'wastewater_profile_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (571, 4, N'x', N'x', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (572, 4, N'x_flag', N'x_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (573, 4, N'y', N'y', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (574, 4, N'y_flag', N'y_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (575, 4, N'ground_id', N'ground_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (576, 4, N'hotlinks', N'hotlinks', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (577, 4, N'curve_number', N'curve_number', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (578, 4, N'curve_number_flag', N'curve_number_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (579, 4, N'area_absolute_1', N'area_absolute_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (580, 4, N'area_absolute_10', N'area_absolute_10', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (581, 4, N'area_absolute_10_flag', N'area_absolute_10_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (582, 4, N'area_absolute_11', N'area_absolute_11', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (583, 4, N'area_absolute_11_flag', N'area_absolute_11_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (584, 4, N'area_absolute_12', N'area_absolute_12', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (585, 4, N'area_absolute_12_flag', N'area_absolute_12_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (586, 4, N'area_absolute_1_flag', N'area_absolute_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (587, 4, N'area_absolute_2', N'area_absolute_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (588, 4, N'area_absolute_2_flag', N'area_absolute_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (589, 4, N'area_absolute_3', N'area_absolute_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (590, 4, N'area_absolute_3_flag', N'area_absolute_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (591, 4, N'area_absolute_4', N'area_absolute_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (592, 4, N'area_absolute_4_flag', N'area_absolute_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (593, 4, N'area_absolute_5', N'area_absolute_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (594, 4, N'area_absolute_5_flag', N'area_absolute_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (595, 4, N'area_absolute_6', N'area_absolute_6', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (596, 4, N'area_absolute_6_flag', N'area_absolute_6_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (597, 4, N'area_absolute_7', N'area_absolute_7', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (598, 4, N'area_absolute_7_flag', N'area_absolute_7_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (599, 4, N'area_absolute_8', N'area_absolute_8', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (600, 4, N'area_absolute_8_flag', N'area_absolute_8_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (601, 4, N'area_absolute_9', N'area_absolute_9', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (602, 4, N'area_absolute_9_flag', N'area_absolute_9_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (603, 4, N'user_number_2', N'user_number_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (604, 4, N'user_number_2_flag', N'user_number_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (605, 4, N'user_text_2_flag', N'user_text_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (606, 4, N'user_number_3', N'user_number_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (607, 4, N'user_number_3_flag', N'user_number_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (608, 4, N'user_number_4', N'user_number_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (609, 4, N'user_number_4_flag', N'user_number_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (610, 4, N'user_number_5', N'user_number_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (611, 4, N'user_number_5_flag', N'user_number_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (612, 4, N'user_text_3_flag', N'user_text_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (613, 4, N'user_text_4_flag', N'user_text_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (614, 4, N'user_text_5_flag', N'user_text_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (615, 4, N'user_text_1', N'user_text_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (616, 4, N'user_text_2', N'user_text_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (617, 4, N'user_text_3', N'user_text_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (618, 4, N'user_text_4', N'user_text_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (619, 4, N'user_text_5', N'user_text_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (620, 4, N'link_suffix', N'link_suffix', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (621, 4, N'unit_hydrograph_id', N'unit_hydrograph_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (622, 4, N'snow_pack_id', N'snow_pack_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (623, 4, N'area_percent_1', N'area_percent_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (624, 4, N'area_percent_10', N'area_percent_10', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (625, 4, N'area_percent_10_flag', N'area_percent_10_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (626, 4, N'area_percent_11', N'area_percent_11', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (627, 4, N'area_percent_11_flag', N'area_percent_11_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (628, 4, N'area_percent_12', N'area_percent_12', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (629, 4, N'area_percent_12_flag', N'area_percent_12_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (630, 4, N'area_percent_1_flag', N'area_percent_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (631, 4, N'area_percent_2', N'area_percent_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (632, 4, N'area_percent_2_flag', N'area_percent_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (633, 4, N'area_percent_3', N'area_percent_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (634, 4, N'area_percent_3_flag', N'area_percent_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (635, 4, N'area_percent_4', N'area_percent_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (636, 4, N'area_percent_4_flag', N'area_percent_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (637, 4, N'area_percent_5', N'area_percent_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (638, 4, N'area_percent_5_flag', N'area_percent_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (639, 4, N'area_percent_6', N'area_percent_6', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (640, 4, N'area_percent_6_flag', N'area_percent_6_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (641, 4, N'area_percent_7', N'area_percent_7', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (642, 4, N'area_percent_7_flag', N'area_percent_7_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (643, 4, N'area_percent_8', N'area_percent_8', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (644, 4, N'area_percent_8_flag', N'area_percent_8_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (645, 4, N'area_percent_9', N'area_percent_9', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (646, 4, N'area_percent_9_flag', N'area_percent_9_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (647, 4, N'land_use_id_flag', N'land_use_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (648, 4, N'time_of_concentration', N'time_of_concentration', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (649, 4, N'time_of_concentration_flag', N'time_of_concentration_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (650, 4, N'base_time', N'base_time', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (651, 4, N'base_time_flag', N'base_time_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (652, 4, N'time_to_peak', N'time_to_peak', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (653, 4, N'time_to_peak_flag', N'time_to_peak_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (654, 4, N'uh_definition', N'uh_definition', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (655, 4, N'uh_definition_flag', N'uh_definition_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (656, 4, N'tc_time_to_peak_factor', N'tc_time_to_peak_factor', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (657, 4, N'tc_time_to_peak_factor_flag', N'tc_time_to_peak_factor_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (658, 4, N'tc_timestep_factor', N'tc_timestep_factor', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (659, 4, N'tc_timestep_factor_flag', N'tc_timestep_factor_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (660, 4, N'node_id_flag', N'node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (661, 4, N'subcatchment_id_flag', N'subcatchment_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (662, 4, N'Version', N'Version', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (663, 4, N'additional_foul_flow', N'additional_foul_flow', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (664, 4, N'ModelVersion', N'ModelVersion', NULL, 3, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (665, 4, N'Description', N'Description', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (666, 5, N'PumpID', N'PumpID', NULL, 1, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (667, 5, N'base_level', N'base_level', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (668, 5, N'base_level_flag', N'base_level_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (669, 5, N'delay', N'delay', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (670, 5, N'delay_flag', N'delay_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (671, 5, N'discharge', N'discharge', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (672, 5, N'discharge_flag', N'discharge_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (673, 5, N'ds_node_id', N'ds_node_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (674, 5, N'excluded', N'excluded', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (675, 5, N'head_discharge_id', N'head_discharge_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (676, 5, N'link_suffix', N'link_suffix', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (677, 5, N'link_type', N'link_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (678, 5, N'maximum_flow', N'maximum_flow', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (679, 5, N'maximum_flow_flag', N'maximum_flow_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (680, 5, N'minimum_flow', N'minimum_flow', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (681, 5, N'minimum_flow_flag', N'minimum_flow_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (682, 5, N'negative_change_in_flow', N'negative_change_in_flow', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (683, 5, N'negative_change_in_flow_flag', N'negative_change_in_flow_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (684, 5, N'notes', N'notes', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (685, 5, N'point_array', N'point_array', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (686, 5, N'positive_change_in_flow', N'positive_change_in_flow', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (687, 5, N'positive_change_in_flow_flag', N'positive_change_in_flow_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (688, 5, N'sewer_reference', N'sewer_reference', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (689, 5, N'switch_off_level', N'switch_off_level', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (690, 5, N'switch_off_level_flag', N'switch_off_level_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (691, 5, N'switch_on_level', N'switch_on_level', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (692, 5, N'switch_on_level_flag', N'switch_on_level_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (693, 5, N'system_type', N'system_type', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (694, 5, N'system_type_flag', N'system_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (695, 5, N'threshold', N'threshold', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (696, 5, N'threshold_flag', N'threshold_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (697, 5, N'us_node_id', N'us_node_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (698, 5, N'user_number_1', N'user_number_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (699, 5, N'user_number_1_flag', N'user_number_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (700, 5, N'user_number_2', N'user_number_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (701, 5, N'user_number_2_flag', N'user_number_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (702, 5, N'user_text_1_flag', N'user_text_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (703, 5, N'user_text_2_flag', N'user_text_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (704, 5, N'ds_settlement_eff', N'ds_settlement_eff', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (705, 5, N'ds_settlement_eff_flag', N'ds_settlement_eff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (706, 5, N'us_settlement_eff', N'us_settlement_eff', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (707, 5, N'us_settlement_eff_flag', N'us_settlement_eff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (708, 5, N'hotlinks', N'hotlinks', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (709, 5, N'asset_uid', N'asset_uid', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (710, 5, N'infonet_id', N'infonet_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (711, 5, N'branch_id', N'branch_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (712, 5, N'branch_id_flag', N'branch_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (713, 5, N'user_number_3', N'user_number_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (714, 5, N'user_number_3_flag', N'user_number_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (715, 5, N'user_number_4', N'user_number_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (716, 5, N'user_number_4_flag', N'user_number_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (717, 5, N'user_number_5', N'user_number_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (718, 5, N'user_number_5_flag', N'user_number_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (719, 5, N'user_text_3_flag', N'user_text_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (720, 5, N'user_text_4_flag', N'user_text_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (721, 5, N'user_text_5_flag', N'user_text_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (722, 5, N'user_text_1', N'user_text_1', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (723, 5, N'user_text_2', N'user_text_2', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (724, 5, N'user_text_3', N'user_text_3', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (725, 5, N'user_text_4', N'user_text_4', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (726, 5, N'user_text_5', N'user_text_5', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (727, 5, N'electric_hydraulic_ratio', N'electric_hydraulic_ratio', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (728, 5, N'electric_hydraulic_ratio_flag', N'electric_hydraulic_ratio_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (729, 5, N'nominal_flow', N'nominal_flow', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (730, 5, N'nominal_flow_flag', N'nominal_flow_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (731, 5, N'off_delay', N'off_delay', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (732, 5, N'off_delay_flag', N'off_delay_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (733, 5, N'asset_id_flag', N'asset_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (734, 5, N'ds_node_id_flag', N'ds_node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (735, 5, N'us_node_id_flag', N'us_node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (736, 5, N'Version', N'Version', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (737, 5, N'asset_id', N'asset_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (738, 5, N'ModelVersion', N'ModelVersion', NULL, 3, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (739, 5, N'Description', N'Description', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (740, 6, N'SluiceID', N'SluiceID', NULL, 1, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (741, 6, N'discharge_coeff', N'discharge_coeff', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (742, 6, N'discharge_coeff_flag', N'discharge_coeff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (743, 6, N'ds_node_id', N'ds_node_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (744, 6, N'excluded', N'excluded', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (745, 6, N'invert', N'invert', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (746, 6, N'invert_flag', N'invert_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (747, 6, N'link_suffix', N'link_suffix', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (748, 6, N'link_type', N'link_type', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (749, 6, N'maximum_opening', N'maximum_opening', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (750, 6, N'maximum_opening_flag', N'maximum_opening_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (751, 6, N'minimum_opening', N'minimum_opening', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (752, 6, N'minimum_opening_flag', N'minimum_opening_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (753, 6, N'negative_speed', N'negative_speed', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (754, 6, N'negative_speed_flag', N'negative_speed_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (755, 6, N'notes', N'notes', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (756, 6, N'opening', N'opening', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (757, 6, N'opening_flag', N'opening_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (758, 6, N'point_array', N'point_array', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (759, 6, N'positive_speed', N'positive_speed', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (760, 6, N'positive_speed_flag', N'positive_speed_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (761, 6, N'secondary_discharge_coeff', N'secondary_discharge_coeff', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (762, 6, N'secondary_discharge_coeff_flag', N'secondary_discharge_coeff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (763, 6, N'sewer_reference', N'sewer_reference', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (764, 6, N'system_type', N'system_type', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (765, 6, N'system_type_flag', N'system_type_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (766, 6, N'threshold', N'threshold', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (767, 6, N'threshold_flag', N'threshold_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (768, 6, N'us_node_id', N'us_node_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (769, 6, N'user_number_1', N'user_number_1', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (770, 6, N'user_number_1_flag', N'user_number_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (771, 6, N'user_number_2', N'user_number_2', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (772, 6, N'user_number_2_flag', N'user_number_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (773, 6, N'user_text_1_flag', N'user_text_1_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (774, 6, N'user_text_2_flag', N'user_text_2_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (775, 6, N'width', N'width', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (776, 6, N'width_flag', N'width_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (777, 6, N'ds_settlement_eff', N'ds_settlement_eff', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (778, 6, N'ds_settlement_eff_flag', N'ds_settlement_eff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (779, 6, N'us_settlement_eff', N'us_settlement_eff', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (780, 6, N'us_settlement_eff_flag', N'us_settlement_eff_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (781, 6, N'hotlinks', N'hotlinks', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (782, 6, N'asset_uid', N'asset_uid', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (783, 6, N'infonet_id', N'infonet_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (784, 6, N'branch_id', N'branch_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (785, 6, N'branch_id_flag', N'branch_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (786, 6, N'user_number_3', N'user_number_3', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (787, 6, N'user_number_3_flag', N'user_number_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (788, 6, N'user_number_4', N'user_number_4', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (789, 6, N'user_number_4_flag', N'user_number_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (790, 6, N'user_number_5', N'user_number_5', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (791, 6, N'user_number_5_flag', N'user_number_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (792, 6, N'user_text_3_flag', N'user_text_3_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (793, 6, N'user_text_4_flag', N'user_text_4_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (794, 6, N'user_text_5_flag', N'user_text_5_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (795, 6, N'user_text_1', N'user_text_1', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (796, 6, N'user_text_2', N'user_text_2', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (797, 6, N'user_text_3', N'user_text_3', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (798, 6, N'user_text_4', N'user_text_4', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (799, 6, N'user_text_5', N'user_text_5', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (800, 6, N'asset_id_flag', N'asset_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (801, 6, N'ds_node_id_flag', N'ds_node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (802, 6, N'us_node_id_flag', N'us_node_id_flag', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (803, 6, N'Version', N'Version', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (804, 6, N'asset_id', N'asset_id', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (805, 6, N'ModelVersion', N'ModelVersion', NULL, 3, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (806, 6, N'Description', N'Description', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (807, 7, N'RunID', N'RunID', NULL, 1, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (808, 7, N'Duration', N'Duration', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (809, 7, N'DWFMultiplier', N'DWFMultiplier', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (810, 7, N'Inflow', N'Inflow', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (811, 7, N'Level', N'Level', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (812, 7, N'Name', N'Name', NULL, 2, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (813, 7, N'Network', N'Network', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (814, 7, N'ResultsMultiplier', N'ResultsMultiplier', NULL, 5, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (815, 7, N'RTC Scenario', N'RTC Scenario', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (816, 7, N'Start Time', N'Start Time', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (817, 7, N'StatisticsTemplate', N'StatisticsTemplate', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (818, 7, N'TimeStep', N'TimeStep', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (819, 7, N'Trade Waste', N'Trade Waste', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (820, 7, N'Waste Water', N'Waste Water', NULL, 4, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (821, 7, N'Ground Infiltration', N'Ground Infiltration', NULL, 6, 1)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (822, 7, N'ModelVersion', N'ModelVersion', NULL, 3, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (823, 7, N'Description', N'Description', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (824, 8, N'ComponentID', N'ComponentID', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (825, 8, N'ComponentType_FK', N'ComponentType_FK', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (826, 8, N'ComponentLabel', N'ComponentLabel', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (827, 8, N'ComponentDescription', N'ComponentDescription', NULL, 6, 0)
GO
INSERT [dbo].[tlkpIWFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [Subtype], [FieldClass], [InIWM]) VALUES (828, 8, N'ProjID_FK', N'ProjID_FK', NULL, 6, 0)
GO

SET IDENTITY_INSERT [tlkpIWFieldDictionary] OFF
GO


SET IDENTITY_INSERT [tlkpIWResults_FieldDictionary] ON
GO


INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (1, N'Node', N'MAXLEVEL', N'Max Level', 3, NULL, 4)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (2, N'Node', N'FLOODVOLUME', N'Flood Volume', 4, NULL, 6)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (3, N'Node', N'DEPNOD', N'Flood Depth', 5, NULL, 4)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (4, N'Node', N'FLOODAREA', N'Flood Area', 6, NULL, 2)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (5, N'Node', N'MAXSTORED', N'Max Stored', 7, NULL, 6)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (6, N'Node', N'INFLOWVOL', N'Inflow  Vol', 8, NULL, 6)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (7, N'Link', N'MAXDEPTH', N'Max Depth', 13, 8, 4)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (8, N'Link', N'MAXFLOW', N'Max Flow', 14, 9, 3)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (9, N'Link', N'MAXVELOCTIY', N'Max Velocity', 15, -1, 5)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (10, N'Link', N'TOTALFLOW', N'Total Flow', 16, 10, 6)
GO
INSERT [dbo].[tlkpIWResults_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [ColumnNo], [AltColumnNo], [UnitType]) VALUES (11, N'Link', N'SurchargeState', N'SurchargeState', -1, -1, 9)
GO

SET IDENTITY_INSERT [tlkpIWResults_FieldDictionary] OFF
GO


SET IDENTITY_INSERT [tlkpIWTableDictionary] ON
GO

INSERT [dbo].[tlkpIWTableDictionary] ([ID], [TableName], [IsOwnTable], [ComponentName], [TableClass], [ComponentName_Alias], [KeyColumn]) VALUES (1, N'hw_conduit', 0, N'Conduits', 3, N'Conduits', N'ConduitID')
GO
INSERT [dbo].[tlkpIWTableDictionary] ([ID], [TableName], [IsOwnTable], [ComponentName], [TableClass], [ComponentName_Alias], [KeyColumn]) VALUES (2, N'hw_node', 0, N'Nodes', 3, N'Nodes', N'NodeID')
GO
INSERT [dbo].[tlkpIWTableDictionary] ([ID], [TableName], [IsOwnTable], [ComponentName], [TableClass], [ComponentName_Alias], [KeyColumn]) VALUES (3, N'hw_weir', -1, N'Weirs', 3, N'Weirs', N'WeirID')
GO
INSERT [dbo].[tlkpIWTableDictionary] ([ID], [TableName], [IsOwnTable], [ComponentName], [TableClass], [ComponentName_Alias], [KeyColumn]) VALUES (4, N'hw_subcatchment', -1, N'Subcatchments', 3, N'Subcatchments', N'SubcatchmentID')
GO
INSERT [dbo].[tlkpIWTableDictionary] ([ID], [TableName], [IsOwnTable], [ComponentName], [TableClass], [ComponentName_Alias], [KeyColumn]) VALUES (5, N'hw_pump', -1, N'Pumps', 3, N'Pumps', N'PumpID')
GO
INSERT [dbo].[tlkpIWTableDictionary] ([ID], [TableName], [IsOwnTable], [ComponentName], [TableClass], [ComponentName_Alias], [KeyColumn]) VALUES (6, N'hw_sluice', -1, N'Sluices', 3, N'Sluices', N'SluiceID')
GO
INSERT [dbo].[tlkpIWTableDictionary] ([ID], [TableName], [IsOwnTable], [ComponentName], [TableClass], [ComponentName_Alias], [KeyColumn]) VALUES (7, N'Run', -1, N'Run Definition', 2, N'Run Definition', N'RunID')
GO
INSERT [dbo].[tlkpIWTableDictionary] ([ID], [TableName], [IsOwnTable], [ComponentName], [TableClass], [ComponentName_Alias], [KeyColumn]) VALUES (8, N'tblComponent', -1, N'Components', 2, N'Components', N'ComponentID')
GO

SET IDENTITY_INSERT [tlkpIWTableDictionary] OFF
GO


SET IDENTITY_INSERT [tlkpIWTableDictionary_Key] ON
GO


INSERT [dbo].[tlkpIWTableDictionary_Key] ([ID_TableClass], [TableClass_Definition], [OneValPerScenario]) VALUES (1, N'General Settings', 0)
GO
INSERT [dbo].[tlkpIWTableDictionary_Key] ([ID_TableClass], [TableClass_Definition], [OneValPerScenario]) VALUES (2, N'Simulation Settings', 1)
GO
INSERT [dbo].[tlkpIWTableDictionary_Key] ([ID_TableClass], [TableClass_Definition], [OneValPerScenario]) VALUES (3, N'Network Data', 0)
GO
INSERT [dbo].[tlkpIWTableDictionary_Key] ([ID_TableClass], [TableClass_Definition], [OneValPerScenario]) VALUES (4, N'Curve Data', 0)
GO
INSERT [dbo].[tlkpIWTableDictionary_Key] ([ID_TableClass], [TableClass_Definition], [OneValPerScenario]) VALUES (5, N'Model Component', 1)
GO
INSERT [dbo].[tlkpIWTableDictionary_Key] ([ID_TableClass], [TableClass_Definition], [OneValPerScenario]) VALUES (6, N'Mapping Other', 0)
GO

SET IDENTITY_INSERT [tlkpIWTableDictionary_Key] OFF
GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.05i', 'Creating schema and data for InfoWorks dictionary lookup tables'); 