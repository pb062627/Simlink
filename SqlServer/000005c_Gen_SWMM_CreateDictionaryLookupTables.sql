/****** Object:  Table [dbo].[tlkpSWMMTableDictionary]    Script Date: 3/7/2016 2:14:34 PM ******/
DROP TABLE [dbo].[tlkpSWMMTableDictionary]
GO
/****** Object:  Table [dbo].[tlkpSWMMResults_TableDictionary]    Script Date: 3/7/2016 2:14:34 PM ******/
DROP TABLE [dbo].[tlkpSWMMResults_TableDictionary]
GO
/****** Object:  Table [dbo].[tlkpSWMMResults_FieldDictionary]    Script Date: 3/7/2016 2:14:34 PM ******/
DROP TABLE [dbo].[tlkpSWMMResults_FieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpSWMMFieldDictionary_Key]    Script Date: 3/7/2016 2:14:34 PM ******/
DROP TABLE [dbo].[tlkpSWMMFieldDictionary_Key]
GO
/****** Object:  Table [dbo].[tlkpSWMMFieldDictionary]    Script Date: 3/7/2016 2:14:34 PM ******/
DROP TABLE [dbo].[tlkpSWMMFieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpSWMMFieldDictionary]    Script Date: 3/7/2016 2:14:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSWMMFieldDictionary](
	[ID] [int] Identity (1,1) NOT NULL,
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
 CONSTRAINT [PK_tlkpSWMMFieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpSWMMFieldDictionary_Key]    Script Date: 3/7/2016 2:14:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSWMMFieldDictionary_Key](
	[ID_FieldClass] [int] Identity (1,1) NOT NULL,
	[FieldClass_Definition] [nvarchar](255) NULL,
 CONSTRAINT [PK_tlkpSWMMFieldDictionary_Key] PRIMARY KEY CLUSTERED 
(
	[ID_FieldClass] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpSWMMResults_FieldDictionary]    Script Date: 3/7/2016 2:14:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSWMMResults_FieldDictionary](
	[ResultsFieldID] [int] Identity (1,1)  NOT NULL,
	[TableID_FK] [int] NULL,
	[FeatureType] [nvarchar](255) NULL,
	[FieldName] [nvarchar](255) NULL,
	[SWMM_Alias] [nvarchar](255) NULL,
	[ColumnNo] [float] NULL,
	[IsOutFileVar] [bit] NOT NULL,
	[IsLID_Detail] [bit] NOT NULL,
 CONSTRAINT [PK_tlkpSWMMResults_FieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ResultsFieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpSWMMResults_TableDictionary]    Script Date: 3/7/2016 2:14:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSWMMResults_TableDictionary](
	[ResultTableID] [int] Identity (1,1) NOT NULL,
	[TableName] [nvarchar](255) NULL,
	[TableAlias] [nvarchar](255) NULL,
	[SectionNumber] [int] NULL,
 CONSTRAINT [PK_tlkpSWMMResults_TableDictionary] PRIMARY KEY CLUSTERED 
(
	[ResultTableID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpSWMMTableDictionary]    Script Date: 3/7/2016 2:14:34 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSWMMTableDictionary](
	[ID] [int] Identity (1,1) NOT NULL,
	[SectionNumber] [float] NULL,
	[TableName] [nvarchar](255) NULL,
	[IsOwnTable] [float] NULL,
	[SectionName] [nvarchar](255) NULL,
	[TableClass] [int] NULL,
	[SectionName_Alias] [nvarchar](255) NULL,
	[KeyColumn] [nvarchar](255) NULL,
	[IsScenarioSpecific] [float] NULL,
 CONSTRAINT [PK_tlkpSWMMTableDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]


SET IDENTITY_INSERT [tlkpSWMMFieldDictionary] ON
Go



GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (1, 11, N'ConduitID', N'ConduitID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (2, 11, N'ConduitName', N'Name', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (3, 11, N'InletNode', N'Inlet Node', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (4, 11, N'OutletNode', N'Outlet Node', 1, 3, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (5, 11, N'Length', N'Length', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (6, 11, N'Manning_N', N'Manning N', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (7, 11, N'InletOffset', N'Inlet Offset', 1, 6, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (8, 11, N'OutletOffset', N'Outlet Offset', 1, 7, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (9, 11, N'InitFlow', N'Init. Flow', 1, 8, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (10, 11, N'MaxFlow', N'Max. Flow', 1, 9, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (11, 11, N'ModelVersion', N'ModelVersion', 0, 111, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (12, 11, N'Description', N'Description', 0, 118, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (13, 24, N'CoordinateID', N'CoordinateID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (14, 24, N'Node', N'Node', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (15, 24, N'XCoord', N'X-Coord', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (16, 24, N'YCoord', N'Y-Coord', 1, 3, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (17, 24, N'ModelVersion', N'ModelVersion', 0, 105, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (18, 24, N'Description', N'Description', 0, 128, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (19, 19, N'CurveID', N'CurveID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (20, 19, N'CurveName', N'Name', 1, 1, NULL, 2, 99, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (21, 19, N'CurveType', N'Type', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (22, 19, N'XValue', N'X-Value', 1, 3, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (23, 19, N'YValue', N'Y-Value', 1, 4, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (24, 19, N'ModelVersion', N'ModelVersion', 0, 106, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (25, 19, N'Description', N'Description', 0, 126, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (26, 16, N'DWFID', N'DWFID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (27, 16, N'Node', N'Node', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (28, 16, N'Parameter', N'Parameter', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (29, 16, N'AverageValue', N'AverageValue', 1, 3, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (30, 16, N'TimePatterns', N'TimePatterns', 1, 4, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (31, 16, N'ModelVersion', N'ModelVersion', 0, 106, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (32, 16, N'Description', N'Description', 0, 123, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (33, 3, N'EvapID', N'EvapID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (34, 3, N'Type', N'Type', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (35, 3, N'Parameters_', N'Parameters', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (36, 3, N'ModelVersion', N'ModelVersion', 0, 104, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (37, 3, N'Description', N'Description', 0, 111, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (38, 17, N'HydrographsID', N'HydrographsID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (39, 17, N'HydrographName', N'HydrographName', 1, 1, NULL, 2, 116, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (40, 17, N'RainGage', N'Rain Gage / Month', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (41, 17, N'R1', N'R1', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (42, 17, N'T1', N'T1', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (43, 17, N'K1', N'K1', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (44, 17, N'R2', N'R2', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (45, 17, N'T2', N'T2', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (46, 17, N'K2', N'K2', 1, 8, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (47, 17, N'R3', N'R3', 1, 9, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (48, 17, N'T3', N'T3', 1, 10, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (49, 17, N'K3', N'K3', 1, 11, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (50, 17, N'IA_max', N'IA_max', 1, 12, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (51, 17, N'IA_rec', N'IA_rec', 1, 13, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (52, 17, N'IA_init', N'IA_init', 1, 14, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (53, 17, N'ModelVersion', N'ModelVersion', 0, 117, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (54, 17, N'Description', N'Description', 0, 124, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (55, 7, N'InfiltrationID', N'InfiltrationID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (56, 7, N'Subcatchment', N'Subcatchment', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (57, 7, N'MaxRate', N'MaxRate', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (58, 7, N'MinRate', N'MinRate', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (59, 7, N'Decay', N'Decay', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (60, 7, N'DryTime', N'DryTime', 1, 5, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (61, 7, N'MaxInfil', N'MaxInfil', 1, 6, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (62, 7, N'ModelVersion', N'ModelVersion', 0, 108, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (63, 7, N'Description', N'Description', 0, 114, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (64, 8, N'JunctionID', N'JunctionID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (65, 8, N'JunctionName', N'Name', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (66, 8, N'InvertElev', N'Invert Elev.', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (67, 8, N'MaxDepth', N'Max. Depth', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (68, 8, N'InitDepth', N'Init. Depth', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (69, 8, N'SurchargeDepth', N'Surcharge Depth', 1, 5, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (70, 8, N'PondedArea', N'Ponded Area', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (71, 8, N'ModelVersion', N'ModelVersion', 0, 108, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (72, 8, N'Description', N'Description', 0, 115, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (73, 15, N'LossesID', N'LossesID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (74, 15, N'Link', N'Link', 1, 1, NULL, 22, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (75, 15, N'Inlet', N'Inlet', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (76, 15, N'Outlet', N'Outlet', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (77, 15, N'Average', N'Average', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (78, 15, N'FlapGate', N'Flap Gate', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (79, 15, N'ModelVersion', N'ModelVersion', 0, 107, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (80, 15, N'Description', N'Description', 0, 122, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (81, 9, N'OutfallID', N'OutfallID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (82, 9, N'OutfallName', N'Name', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (83, 9, N'InvertElev', N'Invert Elev.', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (84, 9, N'OutfallType', N'Outfall Type', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (85, 9, N'StageTableTS', N'Stage/Table Time Series', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (86, 9, N'TideGate', N'Tide Gate', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (87, 9, N'ModelVersion', N'ModelVersion', 0, 107, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (88, 9, N'Description', N'Description', 0, 116, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (89, 26, N'PolygonsID', N'PolygonsID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (90, 26, N'Subcatchment', N'Subcatchment', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (91, 26, N'XCoord', N'X-Coord', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (92, 26, N'YCoord', N'Y-Coord', 1, 3, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (93, 26, N'ModelVersion', N'ModelVersion', 0, 105, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (94, 26, N'Description', N'Description', 0, 130, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (95, 12, N'PumpID', N'PumpID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (96, 12, N'PumpName', N'Name', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (97, 12, N'InletNode', N'Inlet Node', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (98, 12, N'OutletNode', N'Outlet Node', 1, 3, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (99, 12, N'PumpCurve', N'Pump Curve', 1, 4, NULL, 5, 0, 1, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (100, 12, N'InitStatus', N'Init. Status', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (101, 12, N'StartupDepth', N'Startup Depth', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (102, 12, N'ShutoffDepth', N'Shutoff Depth', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (103, 12, N'ModelVersion', N'ModelVersion', 0, 109, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (104, 12, N'Description', N'Description', 0, 119, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (105, 4, N'RainID', N'RainID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (106, 4, N'RainName', N'Name', 1, 1, NULL, 2, 185, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (107, 4, N'RainType', N'Rain Type', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (108, 4, N'TimeInterval', N'Time Interval', 1, 3, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (109, 4, N'SnowCatch', N'Snow Catch', 1, 4, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (110, 4, N'DataSource', N'Data Source', 1, 5, NULL, 6, 0, 1, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (111, 4, N'TimeSeries', N'TimeSeries', 1, 6, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (112, 4, N'ModelVersion', N'ModelVersion', 0, 108, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (113, 4, N'Description', N'Description', 0, 112, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (114, 18, N'RDIIID', N'RDIIID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (115, 18, N'Node', N'Node', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (116, 18, N'UnitHydrograph', N'Unit Hydrograph', 1, 2, NULL, 5, 0, 1, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (117, 18, N'SewerArea', N'Sewer Area', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (118, 18, N'ModelVersion', N'ModelVersion', 0, 105, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (119, 18, N'Description', N'Description', 0, 125, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (120, 1, N'SWMM_ID', N'SWMM_ID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (121, 1, N'Title', N'Title', 1, 1, N'Title', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (122, 1, N'FLOW_UNITS', N'FLOW_UNITS', 1, 2, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (123, 1, N'INFILTRATION', N'INFILTRATION', 1, 3, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (124, 1, N'FLOW_ROUTING', N'FLOW_ROUTING', 1, 4, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (125, 1, N'START_DATE', N'START_DATE', 1, 5, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (126, 1, N'START_TIME', N'START_TIME', 1, 6, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (127, 1, N'REPORT_START_DATE', N'REPORT_START_DATE', 1, 7, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (128, 1, N'REPORT_START_TIME', N'REPORT_START_TIME', 1, 8, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (129, 1, N'END_DATE', N'END_DATE', 1, 9, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (130, 1, N'END_TIME', N'END_TIME', 1, 10, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (131, 1, N'SWEEP_START', N'SWEEP_START', 1, 11, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (132, 1, N'SWEEP_END', N'SWEEP_END', 1, 12, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (133, 1, N'DRY_DAYS', N'DRY_DAYS', 1, 13, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (134, 1, N'REPORT_STEP', N'REPORT_STEP', 1, 14, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (135, 1, N'WET_STEP', N'WET_STEP', 1, 15, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (136, 1, N'DRY_STEP', N'DRY_STEP', 1, 16, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (137, 1, N'ROUTING_STEP', N'ROUTING_STEP', 1, 17, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (138, 1, N'ALLOW_PONDING', N'ALLOW_PONDING', 1, 18, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (139, 1, N'INERTIAL_DAMPING', N'INERTIAL_DAMPING', 1, 19, N'Options', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (140, 1, N'VARIABLE_STEP', N'VARIABLE_STEP', 1, 20, N'Options', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (141, 1, N'LENGTHENING_STEP', N'LENGTHENING_STEP', 1, 21, N'Options', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (142, 1, N'MIN_SURFAREA', N'MIN_SURFAREA', 1, 22, N'Options', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (143, 1, N'NORMAL_FLOW_LIMITED', N'NORMAL_FLOW_LIMITED', 1, 23, N'Options', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (144, 1, N'SKIP_STEADY_STATE', N'SKIP_STEADY_STATE', 1, 24, N'Options', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (145, 1, N'FORCE_MAIN_EQUATION', N'FORCE_MAIN_EQUATION', 1, 25, N'Options', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (146, 1, N'LINK_OFFSETS', N'LINK_OFFSETS', 1, 26, N'Options', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (147, 1, N'MIN_SLOPE', N'MIN_SLOPE', 1, 27, N'Options', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (148, 1, N'INPUT_', N'INPUT', 1, 28, N'Report', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (149, 1, N'CONTROLS', N'CONTROLS', 1, 29, N'Report', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (150, 1, N'SUBCATCHMENTS', N'SUBCATCHMENTS', 1, 30, N'Report', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (151, 1, N'NODES', N'NODES', 1, 31, N'Report', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (152, 1, N'LINKS', N'LINKS', 1, 32, N'Report', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (153, 1, N'MAP_X1', N'MAP_X1', 1, 33, N'Map', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (154, 1, N'MAP_X2', N'MAP_X2', 1, 34, N'Map', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (155, 1, N'MAP_Y1', N'MAP_Y1', 1, 35, N'Map', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (156, 1, N'MAP_Y2', N'MAP_Y2', 1, 36, N'Map', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (157, 1, N'Units', N'Units', 1, 37, N'Map', 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (158, 1, N'Description', N'Description', 0, 132, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (159, 1, N'ModelVersion', N'ModelVersion', 0, 140, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (160, 10, N'StorageID', N'StorageID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (161, 10, N'StorageName', N'Name', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (162, 10, N'InvertElev', N'Invert Elev.', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (163, 10, N'MaxDepth', N'Max. Depth', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (164, 10, N'InitDepth', N'Init. Depth', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (165, 10, N'StorageCurve', N'StorageCurve', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (166, 10, N'CurveCoefficient', N'CurveCoefficient', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (167, 10, N'CurveExponent', N'CurveExponent', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (168, 10, N'PondedArea', N'PondedArea', 1, 9, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (169, 10, N'EvapFrac', N'EvapFrac', 1, 10, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (170, 10, N'ModelVersion', N'ModelVersion', 0, 111, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (171, 10, N'Description', N'Description', 0, 117, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (172, 6, N'SubareasID', N'SubareasID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (173, 6, N'Subarea', N'Subarea', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (174, 6, N'N_Imperv', N'N_Imperv', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (175, 6, N'N_Perv', N'N-Perv', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (176, 6, N'S_Imperv', N'S_Imperv', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (177, 6, N'S_Perv', N'S-Perv', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (178, 6, N'PctZero', N'PctZero', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (179, 6, N'RouteTo', N'RouteTo', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (180, 6, N'PctRouted', N'PctRouted', 1, 8, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (181, 6, N'ModelVersion', N'ModelVersion', 0, 110, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (182, 6, N'Description', N'Description', 0, 133, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (183, 5, N'SubcatchmentID', N'SubcatchmentID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (184, 5, N'SubName', N'SubName', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (185, 5, N'RainGage', N'RainGage', 1, 2, NULL, 5, 0, 1, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (186, 5, N'Outlet', N'Outlet', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (187, 5, N'Total_Pcnt_Area', N'Total Pcnt. Area', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (188, 5, N'Imperv', N'Imperv', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (189, 5, N'Pcnt_Width', N'Pcnt. Width', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (190, 5, N'Curb_Slope', N'Curb Slope', 1, 7, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (191, 5, N'Snow_Length', N'Snow Length', 1, 8, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (192, 5, N'Pack', N'Pack', 1, 9, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (193, 5, N'ModelVersion', N'ModelVersion', 0, 111, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (194, 5, N'Description', N'Description', 0, 113, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (195, 27, N'GageID', N'GageID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (196, 27, N'Gage', N'Gage', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (197, 27, N'XCoord', N'XCoord', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (198, 27, N'YCoord', N'YCoord', 1, 3, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (199, 27, N'ModelVersion', N'ModelVersion', 0, 105, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (200, 27, N'Description', N'Description', 0, 131, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (201, 20, N'TimeSeriesID', N'TimeSeriesID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (202, 20, N'TimeSeriesName', N'Name', 1, 1, NULL, 2, 110, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (203, 20, N'DateVal', N'Date', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (204, 20, N'TimeVal', N'Time', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (205, 20, N'ValueVal', N'Value', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (206, 20, N'ModelVersion', N'ModelVersion', 0, 106, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (207, 20, N'Description', N'Description', 0, 127, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (208, 25, N'VerticesID', N'VerticesID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (209, 25, N'Link', N'Link', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (210, 25, N'XCoord', N'X-Coord', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (211, 25, N'YCoord', N'Y-Coord', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (212, 25, N'ModelVersion', N'ModelVersion', 0, 105, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (213, 25, N'Description', N'Description', 0, 129, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (214, 13, N'WeirID', N'WeirID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (215, 13, N'WeirName', N'Name', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (216, 13, N'InletNode', N'Inlet Node', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (217, 13, N'OutletNode', N'Outlet Node', 1, 3, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (218, 13, N'WeirType', N'Weir Type', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (219, 13, N'CrestHeight', N'Crest Height', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (220, 13, N'DischargeCoeff', N'Discharge Coeff.', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (221, 13, N'FlapGate', N'Flap Gate', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (222, 13, N'EndCon', N'End Con.', 1, 8, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (223, 13, N'EndCoeff', N'End Coeff.', 1, 9, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (224, 13, N'ModelVersion', N'ModelVersion', 0, 111, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (225, 13, N'Description', N'Description', 0, 120, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (228, 14, N'Shape', N'Shape', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (229, 14, N'Geom1', N'Width', 1, 3, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (230, 14, N'Geom2', N'Height', 1, 4, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (231, 14, N'Geom3', N'Geom3', 1, 5, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (232, 14, N'Geom4', N'Geom4', 1, 6, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (233, 14, N'Barrels', N'Barrels', 1, 7, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (236, 14, N'Link', N'Link', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (238, 29, N'LID_BioretentionID', N'LID_BioretentionID', 0, 0, NULL, 1, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (239, 29, N'Label', N'Label', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (240, 29, N'LID_Type', N'LID_Type', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (241, 29, N'Surface', N'Surface', 1, 2, NULL, 6, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (242, 29, N'SUR_StorageDepth', N'SUR_StorageDepth', 1, 3, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (243, 29, N'SUR_VegVolFrac', N'SUR_VegVolFrac', 1, 4, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (244, 29, N'SUR_Roughness', N'SUR_Roughness', 1, 5, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (245, 29, N'SUR_Slope', N'SUR_Slope', 1, 6, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (246, 29, N'SUR_SideSlope', N'SUR_SideSlope', 1, 7, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (247, 29, N'Soil', N'Soil', 1, 2, NULL, 6, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (248, 29, N'SOIL_Thickness', N'SOIL_Thickness', 1, 3, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (249, 29, N'SOIL_Porosity', N'SOIL_Porosity', 1, 4, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (250, 29, N'SOIL_FieldCap', N'SOIL_FieldCap', 1, 5, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (251, 29, N'SOIL_WiltPoint', N'SOIL_WiltPoint', 1, 6, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (252, 29, N'SOIL_Conductivity', N'SOIL_Conductivity', 1, 7, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (253, 29, N'SOIL_ConductivitySlope', N'SOIL_ConductivitySlope', 1, 8, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (254, 29, N'SOIL_SuctionHead', N'SOIL_SuctionHead', 1, 9, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (255, 29, N'STORAGE', N'STORAGE', 1, 2, NULL, 6, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (256, 29, N'STOR_Height', N'STOR_Height', 1, 3, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (257, 29, N'STOR_VoidRatio', N'STOR_VoidRatio', 1, 4, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (258, 29, N'STOR_Conductivity', N'STOR_Conductivity', 1, 5, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (259, 29, N'STOR_Clogging', N'STOR_Clogging', 1, 6, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (260, 29, N'UNDERDRAIN', N'UNDERDRAIN', 1, 2, NULL, 6, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (261, 29, N'UND_DrainCoeff', N'UND_DrainCoeff', 1, 3, NULL, 4, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (262, 29, N'UND_DrainExp', N'UND_DrainExp', 1, 4, NULL, 4, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (263, 29, N'UND_DrainOffset', N'UND_DrainOffset', 1, 5, NULL, 4, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (264, 29, N'Description', N'Description', 0, 0, NULL, 7, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (265, 29, N'ModelVersion', N'ModelVersion', 0, 0, NULL, 3, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (266, 29, N'ParentID', N'ParentID', 0, 0, NULL, 3, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (267, 30, N'LID_InfiltrationTrenchID', N'LID_InfiltrationTrenchID', 0, 0, NULL, 1, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (268, 30, N'Label', N'Label', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (269, 30, N'LID_Type', N'LID_Type', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (270, 30, N'Surface', N'Surface', 1, 2, NULL, 6, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (271, 30, N'SUR_StorageDepth', N'SUR_StorageDepth', 1, 3, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (272, 30, N'SUR_VegVolFrac', N'SUR_VegVolFrac', 1, 4, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (273, 30, N'SUR_Roughness', N'SUR_Roughness', 1, 5, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (274, 30, N'SUR_Slope', N'SUR_Slope', 1, 6, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (275, 30, N'SUR_SideSlope', N'SUR_SideSlope', 1, 7, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (276, 30, N'STORAGE', N'STORAGE', 1, 2, NULL, 6, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (277, 30, N'STOR_Height', N'STOR_Height', 1, 3, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (278, 30, N'STOR_VoidRatio', N'STOR_VoidRatio', 1, 4, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (279, 30, N'STOR_Conductivity', N'STOR_Conductivity', 1, 5, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (280, 30, N'STOR_Clogging', N'STOR_Clogging', 1, 6, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (281, 30, N'UNDERDRAIN', N'UNDERDRAIN', 1, 2, NULL, 6, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (282, 30, N'UND_DrainCoeff', N'UND_DrainCoeff', 1, 3, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (283, 30, N'UND_DrainExp', N'UND_DrainExp', 1, 4, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (284, 30, N'UND_DrainOffset', N'UND_DrainOffset', 1, 5, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (285, 30, N'Description', N'Description', 0, 0, NULL, 7, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (286, 30, N'ModelVersion', N'ModelVersion', 0, 0, NULL, 3, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (287, 30, N'ParentID', N'ParentID', 0, 0, NULL, 3, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (288, 31, N'LID_PorousPavementID', N'LID_BioretentionID', 0, 0, NULL, 1, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (289, 31, N'Label', N'Label', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (290, 31, N'LID_Type', N'LID_Type', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (291, 31, N'Surface', N'Surface', 1, 2, NULL, 6, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (292, 31, N'SUR_StorageDepth', N'SUR_StorageDepth', 1, 3, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (293, 31, N'SUR_VegVolFrac', N'SUR_VegVolFrac', 1, 4, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (294, 31, N'SUR_Roughness', N'SUR_Roughness', 1, 5, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (295, 31, N'SUR_Slope', N'SUR_Slope', 1, 6, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (296, 31, N'SUR_SideSlope', N'SUR_SideSlope', 1, 7, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (297, 31, N'Pavement', N'Pavement', 1, 2, NULL, 6, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (298, 31, N'PAVE_Thickness', N'PAVE_Thickness', 1, 3, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (299, 31, N'PAVE_VoidRatio', N'PAVE_VoidRatio', 1, 4, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (300, 31, N'PAVE_ImpSurFrac', N'PAVE_ImpSurFrac', 1, 5, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (301, 31, N'PAVE_Permeability', N'PAVE_Permeability', 1, 6, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (302, 31, N'PAVE_Clogging', N'PAVE_Clogging', 1, 7, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (303, 31, N'STORAGE', N'STORAGE', 1, 2, NULL, 6, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (304, 31, N'STOR_Height', N'STOR_Height', 1, 3, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (305, 31, N'STOR_VoidRatio', N'STOR_VoidRatio', 1, 4, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (306, 31, N'STOR_Conductivity', N'STOR_Conductivity', 1, 5, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (307, 31, N'STOR_Clogging', N'STOR_Clogging', 1, 6, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (308, 31, N'UNDERDRAIN', N'UNDERDRAIN', 1, 2, NULL, 6, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (309, 31, N'UND_DrainCoeff', N'UND_DrainCoeff', 1, 3, NULL, 4, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (310, 31, N'UND_DrainExp', N'UND_DrainExp', 1, 4, NULL, 4, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (311, 31, N'UND_DrainOffset', N'UND_DrainOffset', 1, 5, NULL, 4, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (312, 31, N'Description', N'Description', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (313, 31, N'ModelVersion', N'ModelVersion', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (314, 31, N'ParentID', N'ParentID', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (315, 32, N'LID_RainBarrelID', N'LID_RainBarrelID', 0, 0, NULL, 1, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (316, 32, N'Label', N'Label', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (317, 32, N'LID_Type', N'LID_Type', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (318, 32, N'STORAGE', N'STORAGE', 1, 2, NULL, 6, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (319, 32, N'STOR_Height', N'STOR_Height', 1, 3, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (320, 32, N'STOR_VoidRatio', N'STOR_VoidRatio', 1, 4, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (321, 32, N'STOR_Conductivity', N'STOR_Conductivity', 1, 5, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (322, 32, N'STOR_Clogging', N'STOR_Clogging', 1, 6, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (323, 32, N'UNDERDRAIN', N'UNDERDRAIN', 1, 2, NULL, 6, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (324, 32, N'UND_DrainCoeff', N'UND_DrainCoeff', 1, 3, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (325, 32, N'UND_DrainExp', N'UND_DrainExp', 1, 4, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (326, 32, N'UND_DrainOffset', N'UND_DrainOffset', 1, 5, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (327, 32, N'Description', N'Description', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (328, 32, N'ModelVersion', N'ModelVersion', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (329, 32, N'ParentID', N'ParentID', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (330, 33, N'LID_VegetativeSwaleID', N'LID_VegetativeSwaleID', 0, 0, NULL, 1, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (331, 33, N'Label', N'Label', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (332, 33, N'LID_Type', N'LID_Type', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (333, 33, N'Surface', N'Surface', 1, 2, NULL, 6, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (334, 33, N'SUR_StorageDepth', N'SUR_StorageDepth', 1, 3, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (335, 33, N'SUR_VegVolFrac', N'SUR_VegVolFrac', 1, 4, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (336, 33, N'SUR_Roughness', N'SUR_Roughness', 1, 5, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (337, 33, N'SUR_Slope', N'SUR_Slope', 1, 6, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (338, 33, N'SUR_SideSlope', N'SUR_SideSlope', 1, 7, NULL, 4, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (339, 33, N'Description', N'Description', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (340, 33, N'ModelVersion', N'ModelVersion', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (341, 33, N'ParentID', N'ParentID', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (342, 36, N'LIDUsageID', N'LIDUsageID', 0, 0, NULL, 1, 0, 0, 2)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (343, 36, N'Subcatchment', N'Subcatchment', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (344, 36, N'LID_Process', N'LID_Process', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (345, 36, N'NumberVal', N'NumberVal', 1, 3, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (346, 36, N'Area', N'Area', 1, 4, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (347, 36, N'Width', N'Width', 1, 5, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (348, 36, N'InitSatur', N'InitSatur', 1, 6, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (349, 36, N'FromImprv', N'FromImprv', 1, 7, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (350, 36, N'ToPerv', N'ToPerv', 1, 8, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (351, 36, N'Report_File', N'Report_File', 1, 9, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (352, 36, N'Description', N'Description', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (353, 36, N'ModelVersion', N'ModelVersion', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (354, 36, N'ParentID', N'ParentID', 0, 0, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (355, 29, N'UND_DrainField', N'UND_DrainField', 1, 6, NULL, 4, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (356, 30, N'UND_DrainField', N'UND_DrainField', 1, 6, NULL, 4, 0, 0, 4)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (357, 31, N'UND_DrainField', N'UND_DrainField', 1, 6, NULL, 4, 0, 0, 5)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (358, 32, N'UND_DrainField', N'UND_DrainField', 1, 6, NULL, 4, 0, 0, 3)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (359, 1, N'IGNORE_SNOWMELT', N'IGNORE_SNOWMELT', 1, 28, N'Options', 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (360, 37, N'InflowID', N'InflowID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (361, 37, N'InflowNode', N'Node', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (362, 37, N'InflowParameter', N'Parameter', 1, 2, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (363, 37, N'InflowTimeSeries', N'Time Series', 1, 3, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (364, 37, N'ParamType', N'Param Type', 1, 4, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (365, 37, N'UnitsFactor', N'Units', 1, 5, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (366, 37, N'ScaleFactor', N'Scale', 1, 6, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (367, 37, N'BaselineValue', N'Baseline Value', 1, 7, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (368, 37, N'BaselinePattern', N'Baseline Pattern', 1, 8, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (369, 37, N'ModelVersion', N'ModelVersion', 0, 111, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (370, 37, N'Description', N'Description', 0, 118, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (371, 10, N'CurveConstant', N'CurveConstant', 1, 8, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (372, 14, N'XsectionID', N'XsectionID', 1, 0, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (373, 2, N'OptionID', N'OptionID', 0, 0, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (374, 2, N'FLOW_UNITS', N'FLOW_UNITS', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (375, 2, N'INFILTRATION', N'INFILTRATION', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (376, 2, N'FLOW_ROUTING', N'FLOW_ROUTING', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (377, 2, N'START_DATE', N'START_DATE', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (378, 2, N'START_TIME', N'START_TIME', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (379, 2, N'REPORT_START_DATE', N'REPORT_START_DATE', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (380, 2, N'REPORT_START_TIME', N'REPORT_START_TIME', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (381, 2, N'END_DATE', N'END_DATE', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (382, 2, N'END_TIME', N'END_TIME', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (383, 2, N'SWEEP_START', N'SWEEP_START', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (384, 2, N'SWEEP_END', N'SWEEP_END', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (385, 2, N'DRY_DAYS', N'DRY_DAYS', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (386, 2, N'REPORT_STEP', N'REPORT_STEP', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (387, 2, N'WET_STEP', N'WET_STEP', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (388, 2, N'DRY_STEP', N'DRY_STEP', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (389, 2, N'ROUTING_STEP', N'ROUTING_STEP', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (390, 2, N'ALLOW_PONDING', N'ALLOW_PONDING', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (391, 2, N'INERTIAL_DAMPING', N'INERTIAL_DAMPING', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (392, 2, N'VARIABLE_STEP', N'VARIABLE_STEP', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (393, 2, N'LENGTHENING_STEP', N'LENGTHENING_STEP', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (394, 2, N'MIN_SURFAREA', N'MIN_SURFAREA', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (395, 2, N'NORMAL_FLOW_LIMITED', N'NORMAL_FLOW_LIMITED', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (396, 2, N'SKIP_STEADY_STATE', N'SKIP_STEADY_STATE', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (397, 2, N'FORCE_MAIN_EQUATION', N'FORCE_MAIN_EQUATION', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (398, 2, N'LINK_OFFSETS', N'LINK_OFFSETS', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (399, 2, N'MIN_SLOPE', N'MIN_SLOPE', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (400, 2, N'IGNORE_SNOWMELT', N'IGNORE_SNOWMELT', 1, 2, NULL, -1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (401, 5, N'UN1', N'UN1 (user added)', 0, -1, NULL, 7, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (402, 67, N'LID_USAGE', N'LID_USAGE', 0, -1, NULL, -1, 0, 0, 0)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (403, 68, N'AquiferID', N'AquiferID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (404, 68, N'Name', N'Name', 1, 0, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (405, 68, N'Porosity', N'Porosity', 1, 1, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (406, 68, N'WiltPoint', N'WiltPoint', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (407, 68, N'FieldCapacity', N'FieldCapacity', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (408, 68, N'HydCond', N'HydCond', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (409, 68, N'CondSlope', N'CondSlope', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (410, 68, N'TensSlope', N'TensSlope', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (411, 68, N'UpperEvap', N'UpperEvap', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (412, 68, N'LowerEvap', N'LowerEvap', 1, 8, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (413, 68, N'LowerLoss', N'LowerLoss', 1, 9, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (414, 68, N'BottomElev', N'BottomElev', 1, 10, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (415, 68, N'WaterTable', N'WaterTable', 1, 11, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (416, 68, N'UpperMoist', N'UpperMoist', 1, 12, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (417, 68, N'ModelVersion', N'ModelVersion', 0, 13, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (418, 68, N'Description', N'Description', 0, 14, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (419, 69, N'GROUNDWATERID', N'GROUNDWATERID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (420, 69, N'Subcatchment', N'Subcatchment', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (421, 69, N'Aquifer', N'Aquifer', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (422, 69, N'Node', N'Node', 1, 3, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (423, 69, N'SurfElev', N'SurfElev', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (424, 69, N'A1', N'A1', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (425, 69, N'B1', N'B1', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (426, 69, N'A2', N'A2', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (427, 69, N'B2', N'B2', 1, 8, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (428, 69, N'A3', N'A3', 1, 9, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (429, 69, N'Depth', N'Depth', 1, 10, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (430, 69, N'Elev', N'Elev', 2, 11, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (431, 69, N'ModelVersion', N'ModelVersion', 0, 12, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (432, 69, N'Description', N'Description', 0, 13, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (433, 72, N'POLLUTANTSID', N'POLLUTANTSID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (434, 72, N'Name', N'Name', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (435, 72, N'MassUnits', N'MassUnits', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (436, 72, N'RainConcentration', N'RainConcentration', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (437, 72, N'GWConcentration', N'GWConcentration', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (438, 72, N'IIConcentration', N'IIConcentration', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (439, 72, N'DecayCoeff', N'DecayCoeff', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (440, 72, N'SnowOnly', N'SnowOnly', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (441, 72, N'CoPollutName', N'CoPollutName', 1, 8, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (442, 72, N'CoPollutFraction', N'CoPollutFraction', 1, 9, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (443, 72, N'DWFConcen', N'DWFConcen', 1, 10, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (444, 72, N'ModelVersion', N'ModelVersion', 0, 11, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (445, 72, N'Description', N'Description', 0, 12, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (446, 73, N'LANDUSESID', N'LANDUSESID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (447, 73, N'Name', N'Name', 1, 1, NULL, 2, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (448, 73, N'CleaningInterval', N'CleaningInterval', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (449, 73, N'FractionAvailable', N'FractionAvailable', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (450, 73, N'LastCleaned', N'LastCleaned', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (451, 73, N'ModelVersion', N'ModelVersion', 0, 100, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (452, 73, N'Description', N'Description', 0, 101, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (453, 74, N'COVERAGESID', N'COVERAGESID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (454, 74, N'Subcatchment', N'Subcatchment', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (455, 74, N'LandUse', N'LandUse', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (456, 74, N'PercentVal', N'PercentVal', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (457, 74, N'ModelVersion', N'ModelVersion', 0, 100, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (458, 74, N'Description', N'Description', 0, 101, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (459, 76, N'BUILDUPID', N'BUILDUPID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (460, 76, N'LandUse', N'LandUse', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (461, 76, N'Pollutant', N'Pollutant', 1, 2, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (462, 76, N'FunctionType', N'FunctionType', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (463, 76, N'Coeff1', N'Coeff1', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (464, 76, N'Coeff2', N'Coeff2', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (465, 76, N'Coeff3', N'Coeff3', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (466, 76, N'Normalizer', N'Normalizer', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (467, 76, N'ModelVersion', N'ModelVersion', 0, 100, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (468, 76, N'Description', N'Description', 0, 101, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (469, 77, N'WASHOFFID', N'WASHOFFID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (470, 77, N'LandUse', N'LandUse', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (471, 77, N'Pollutant', N'Pollutant', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (472, 77, N'FunctionType', N'FunctionType', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (473, 77, N'Coeff1', N'Coeff1', 1, 4, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (474, 77, N'Coeff2', N'Coeff2', 1, 5, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (475, 77, N'BMP_Effic', N'BMP_Effic', 1, 6, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (476, 77, N'Effic', N'Effic', 1, 7, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (477, 77, N'ModelVersion', N'ModelVersion', 0, 100, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (478, 77, N'Description', N'Description', 0, 101, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (479, 75, N'LOADINGSID', N'LOADINGSID', 0, 0, NULL, 1, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (480, 75, N'Subcatchment', N'Subcatchment', 1, 1, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (481, 75, N'Pollutant', N'Pollutant', 1, 2, NULL, 4, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (482, 75, N'Loading', N'Loading', 1, 3, NULL, 5, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (483, 75, N'ModelVersion', N'ModelVersion', 0, 100, NULL, 3, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (484, 75, N'Description', N'Description', 0, 101, NULL, 7, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (485, 4, N'Units', N'Units', 1, 8, NULL, 6, 0, 0, 1)
GO
INSERT [dbo].[tlkpSWMMFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (486, 4, N'StationID', N'StationID', 1, 7, NULL, 6, 0, 0, 1)
GO

SET IDENTITY_INSERT [tlkpSWMMFieldDictionary] OFF
Go


SET IDENTITY_INSERT [tlkpSWMMFieldDictionary_Key] ON
Go


INSERT [dbo].[tlkpSWMMFieldDictionary_Key] ([ID_FieldClass], [FieldClass_Definition]) VALUES (1, N'Primary Key')
GO
INSERT [dbo].[tlkpSWMMFieldDictionary_Key] ([ID_FieldClass], [FieldClass_Definition]) VALUES (2, N'Non-Primary Key')
GO
INSERT [dbo].[tlkpSWMMFieldDictionary_Key] ([ID_FieldClass], [FieldClass_Definition]) VALUES (3, N'Name / ID Field')
GO
INSERT [dbo].[tlkpSWMMFieldDictionary_Key] ([ID_FieldClass], [FieldClass_Definition]) VALUES (4, N'DV- Primary')
GO
INSERT [dbo].[tlkpSWMMFieldDictionary_Key] ([ID_FieldClass], [FieldClass_Definition]) VALUES (5, N'DV- Secondary')
GO
INSERT [dbo].[tlkpSWMMFieldDictionary_Key] ([ID_FieldClass], [FieldClass_Definition]) VALUES (6, N'Non-Changeable')
GO
INSERT [dbo].[tlkpSWMMFieldDictionary_Key] ([ID_FieldClass], [FieldClass_Definition]) VALUES (7, N'Metadata')
GO



SET IDENTITY_INSERT [tlkpSWMMFieldDictionary_Key] OFF
Go

SET IDENTITY_INSERT [tlkpSWMMResults_FieldDictionary] ON
Go


INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (3, 1, N'Subcatchment', N'TotalPrecip', N'Total Precip', 2, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (4, 1, N'Subcatchment', N'TotalRunon', N'TotalRunon', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (5, 1, N'Subcatchment', N'TotalEvap', N'TotalEvap', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (6, 1, N'Subcatchment', N'TotalInfil', N'TotalInfil', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (7, 1, N'Subcatchment', N'TotalRunoffDepth', N'TotalRunoffDepth', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (8, 1, N'Subcatchment', N'TotalRunoffVol', N'TotalRunoffVol', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (9, 1, N'Subcatchment', N'PeakRunoff', N'PeakRunoff', 8, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (10, 1, N'Subcatchment', N'RunoffCoeff', N'RunoffCoeff', 9, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (11, 2, N'Node', N'AverageDepth', N'AverageDepth', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (12, 2, N'Node', N'MaximumDepth', N'MaximumDepth', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (13, 2, N'Node', N'MaximumHGL', N'MaximumHGL', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (14, 2, N'Node', N'TimeMaxOccurrence', N'TimeMaxOccurrence', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (15, 3, N'Node', N'MaximumLateralInflow', N'MaximumLateralInflow', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (16, 3, N'Node', N'MaximumTotalInflow', N'MaximumTotalInflow', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (17, 3, N'Node', N'TimeMaxOccurrence', N'TimeMaxOccurrence', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (18, 3, N'Node', N'LateralInflowVolume', N'LateralInflowVolume', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (19, 3, N'Node', N'TotalInflowVolume', N'TotalInflowVolume', 8, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (20, 4, N'Node', N'HoursSurcharged', N'HoursSurcharged', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (21, 4, N'Node', N'MaxHeightAboveCrown', N'MaxHeightAboveCrown', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (22, 4, N'Node', N'MinDepthBelowRim', N'MinDepthBelowRim', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (23, 5, N'Node', N'HoursFlooded', N'HoursFlooded', 2, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (24, 5, N'Node', N'MaximumRate', N'MaximumRate', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (25, 5, N'Node', N'TimeMaxOccurrence', N'TimeMaxOccurrence', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (26, 5, N'Node', N'TotalMaxFloodVol', N'TotalMaxFloodVol', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (27, 5, N'Node', N'PondedDepthFeet', N'PondedDepthFeet', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (28, 6, N'Node', N'AverageVolume', N'AverageVolume', 2, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (29, 6, N'Node', N'AvgPctFull', N'AvgPctFull', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (30, 6, N'Node', N'E_I_PcntLoss', N'E_I_PcntLoss', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (31, 6, N'Node', N'MaximumVolume', N'MaximumVolume', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (32, 6, N'Node', N'MaxPcntFull', N'MaxPcntFull', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (33, 6, N'Node', N'TimeMaxOccurrence', N'TimeMaxOccurrence', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (34, 6, N'Node', N'MaximumOutflow', N'MaximumOutflow', 9, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (35, 7, N'Node', N'FlowFreqPcnt', N'FlowFreqPcnt', 2, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (36, 7, N'Node', N'AvgFlow', N'AvgFlow', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (37, 7, N'Node', N'MaxFlow', N'MaxFlow', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (38, 7, N'Node', N'TotalVolume', N'TotalVolume', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (39, 8, N'Link', N'MaximumFlow', N'MaximumFlow', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (40, 8, N'Link', N'TimeMaxOccurrence', N'TimeMaxOccurrence', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (41, 8, N'Link', N'MaximumVeloc', N'MaximumVeloc', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (42, 8, N'Link', N'MaxFullFlow', N'MaxFullFlow', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (43, 8, N'Link', N'MaxFullDepth', N'MaxFullDepth', 8, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (44, 9, N'Link', N'AdjustedActualLength', N'AdjustedActualLength', 2, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (45, 9, N'Link', N'Dry', N'Dry', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (46, 9, N'Link', N'Frac_UpDry', N'Frac_UpDry', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (47, 9, N'Link', N'Frac_DownDry', N'Frac_DownDry', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (48, 9, N'Link', N'Frac_SubCrit', N'Frac_SubCrit', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (49, 9, N'Link', N'Frac_SupCrit', N'Frac_SupCrit', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (50, 9, N'Link', N'Frac_UpCrit', N'Frac_UpCrit', 8, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (51, 9, N'Link', N'Frac_DownCrit', N'Frac_DownCrit', 9, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (52, 9, N'Link', N'AvgFroudeNum', N'AvgFroudeNum', 10, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (53, 9, N'Link', N'AvgFlowChange', N'AvgFlowChange', 11, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (54, 10, N'Link', N'HrsFull_BothEnds', N'HrsFull_BothEnds', 2, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (55, 10, N'Link', N'HrsFull_UpstreamEnd', N'HrsFull_UpstreamEnd', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (56, 10, N'Link', N'HrsFull_DownstreamEnd', N'HrsFull_DownstreamEnd', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (57, 10, N'Link', N'HrsAboveNormal', N'HrsAboveNormal', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (58, 10, N'Link', N'CapacityLimited', N'CapacityLimited', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (59, 11, N'Pump', N'PercentUtilized', N'PercentUtilized', 2, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (60, 11, N'Pump', N'MaxFlowCFS', N'MaxFlowCFS', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (61, 11, N'Pump', N'AvgFlowCFS', N'AvgFlowCFS', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (62, 11, N'Pump', N'TotalVolume', N'TotalVolume', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (63, 11, N'Pump', N'PowerUsage', N'PowerUsage', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (64, 11, N'Pump', N'TimeOffCurve', N'TimeOffCurve', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (65, 13, N'Subcatchment', N'LID_Control', N'LID_Control', 2, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (66, 13, N'Subcatchment', N'EvapLoss', N'EvapLoss', 3, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (67, 13, N'Subcatchment', N'InfilLoss', N'InfilLoss', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (68, 13, N'Subcatchment', N'SurfaceOutflow', N'SurfaceOutflow', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (69, 13, N'Subcatchment', N'DrainOutflow', N'DrainOutflow', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (70, 13, N'Subcatchment', N'InitStorage', N'InitStorage', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (71, 13, N'Subcatchment', N'FinalStorage', N'FinalStorage', 8, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (72, 13, N'Subcatchment', N'PcntError', N'PcntError', 9, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (73, -1, N'Link', N'FlowRate', N'FlowRate', 0, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (74, -1, N'Node', N'TotalInflow', N'Total Inflow', 4, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (75, -1, N'Subcatchment', N'Runoff', N'Runoff', 3, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (76, -1, N'Node', N'Depth', N'Depth', 0, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (77, -1, N'Node', N'Hydraulic Head', N'Hydraulic Head', 1, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (78, -1, N'Node', N'Volume of Stored and Ponded Water', N'Volume of Stored and Ponded Water', 2, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (79, -1, N'Node', N'Lateral Inflow', N'Lateral Inflow', 3, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (80, -1, N'Node', N'Total Inflow', N'Total Inflow', 4, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (81, -1, N'Node', N'Flow lost to Flooding', N'Flow lost to Flooding', 5, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (82, -1, N'Node', N'Concentration of First Pollutant', N'Concentration of First Pollutant', 6, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (83, 14, N'System', N'Sewershed Rainfall', N'Sewershed Rainfall', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (84, 14, N'System', N'RDII Produced', N'RDII Produced', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (85, 14, N'System', N'RDII Ratio', N'RDII Ratio', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (86, 15, N'System', N'Initial Snow Cover', N'Initial Snow Cover', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (87, 15, N'System', N'Total Precipitation', N'Total Precipitation', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (88, 15, N'System', N'Evaporation Loss', N'Evaporation Loss', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (89, 15, N'System', N'Infiltration Loss', N'Infiltration Loss', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (90, 15, N'System', N'Surface Runoff', N'Surface Runoff', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (91, 15, N'System', N'Snow Removed', N'Snow Removed', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (92, 15, N'System', N'Final Snow Cover', N'Final Snow Cover', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (93, 15, N'System', N'Final Surface Storage', N'Final Surface Storage', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (94, 15, N'System', N'Continuity Error (%)', N'Continuity Error (%)', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (95, 16, N'System', N'Dry Weather Inflow', N'Dry Weather Inflow', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (96, 16, N'System', N'Wet Weather Inflow', N'Wet Weather Inflow', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (97, 16, N'System', N'Groundwater Inflow', N'Groundwater Inflow', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (98, 16, N'System', N'RDII Inflow', N'RDII Inflow', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (99, 16, N'System', N'External Inflow', N'External Inflow', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (100, 16, N'System', N'External Outflow', N'External Outflow', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (101, 16, N'System', N'Internal Outflow', N'Internal Outflow', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (102, 16, N'System', N'Storage Losses', N'Storage Losses', 5, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (103, 16, N'System', N'Initial Stored Volume', N'Initial Stored Volume', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (106, 16, N'System', N'Final Stored Volume', N'Final Stored Volume', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (107, 16, N'System', N'Continuity Error (%)', N'Continuity Error (%)', 4, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (108, 7, N'Node', N'TSS', N'TSS', 6, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (109, 7, N'Node', N'TN', N'TN', 7, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (110, 7, N'Node', N'TP', N'TP', 8, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (111, NULL, N'Node', N'BOD', N'BOD', 9, 0, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (112, 13, N'LID_DETAIL', N'ElapsedTime', N'ElapsedTime', 1, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (113, 13, N'LID_DETAIL', N'TotalInflow', N'TotalInflow', 2, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (114, 13, N'LID_DETAIL', N'TotalEvap', N'TotalEvap', 3, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (115, 13, N'LID_DETAIL', N'SurfaceInfil', N'SurfaceInfil', 4, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (116, 13, N'LID_DETAIL', N'SoilPerc', N'SoilPerc', 5, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (117, 13, N'LID_DETAIL', N'BottomInfil', N'BottomInfil', 6, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (118, 13, N'LID_DETAIL', N'SurfaceRunoff', N'SurfaceRunoff', 7, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (119, 13, N'LID_DETAIL', N'DrainOutflow', N'DrainOutflow', 8, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (120, 13, N'LID_DETAIL', N'SurfaceDepth', N'SurfaceDepth', 9, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (121, 13, N'LID_DETAIL', N'SoilPave', N'SoilPave', 10, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (122, 13, N'LID_DETAIL', N'StorageDepth', N'StorageDepth', 11, 0, 1)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (123, -1, N'Subcatchment', N'BOD', N'BOD', 7, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (124, -1, N'Subcatchment', N'COD', N'COD', 8, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (125, -1, N'Subcatchment', N'EColi', N'EColi', 9, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (126, -1, N'Subcatchment', N'Enterococcus', N'Enterococcus', 10, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (127, -1, N'Subcatchment', N'FecalColiform', N'FecalColiform', 11, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (128, -1, N'Subcatchment', N'NitrateNitriteN', N'NitrateNitriteN', 12, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (129, -1, N'Subcatchment', N'OrthophosphateP', N'OrthophosphateP', 13, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (130, -1, N'Subcatchment', N'TKN', N'TKN', 14, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (131, -1, N'Subcatchment', N'TotalCopper', N'TotalCopper', 15, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (132, -1, N'Subcatchment', N'TotalPhosphorus', N'TotalPhosphorus', 16, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (133, -1, N'Subcatchment', N'TotalZinc', N'TotalZinc', 17, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (134, -1, N'Subcatchment', N'TSS', N'TSS', 18, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (135, -1, N'Node', N'BOD', N'BOD', 7, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (136, -1, N'Node', N'COD', N'COD', 8, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (137, -1, N'Node', N'EColi', N'EColi', 9, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (138, -1, N'Node', N'Enterococcus', N'Enterococcus', 10, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (139, -1, N'Node', N'FecalColiform', N'FecalColiform', 11, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (140, -1, N'Node', N'NitrateNitriteN', N'NitrateNitriteN', 12, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (141, -1, N'Node', N'OrthophosphateP', N'OrthophosphateP', 13, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (142, -1, N'Node', N'TKN', N'TKN', 14, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (143, -1, N'Node', N'TotalCopper', N'TotalCopper', 15, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (144, -1, N'Node', N'TotalPhosphorus', N'TotalPhosphorus', 16, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (145, -1, N'Node', N'TotalZinc', N'TotalZinc', 17, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (146, -1, N'Node', N'TSS', N'TSS', 18, 1, 0)
GO
INSERT [dbo].[tlkpSWMMResults_FieldDictionary] ([ResultsFieldID], [TableID_FK], [FeatureType], [FieldName], [SWMM_Alias], [ColumnNo], [IsOutFileVar], [IsLID_Detail]) VALUES (154, 17, N'Node', N'TotalPhosphorus', N'TotalPhosphorus', 12, 1, 0)
GO



SET IDENTITY_INSERT [tlkpSWMMResults_FieldDictionary] OFF
Go

SET IDENTITY_INSERT [tlkpSWMMResults_TableDictionary] ON
Go


INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (1, N'Subcatchment Runoff Summary', N'Subcatchment Runoff Summary', 4)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (2, N'Node Depth Summary', N'Node Depth Summary', 5)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (3, N'Node Inflow Summary', N'Node Inflow Summary', 6)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (4, N'Node Surcharge Summary', N'Node Surcharge Summary', 7)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (5, N'Node Flooding Summary', N'Node Flooding Summary', 8)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (6, N'Storage Volume Summary', N'Storage Volume Summary', 9)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (7, N'Outfall Loading Summary', N'Outfall Loading Summary', 10)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (8, N'Link Flow Summary', N'Link Flow Summary', 11)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (9, N'Flow Classification Summary', N'Flow Classification Summary', 12)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (10, N'Conduit Surcharge Summary', N'Conduit Surcharge Summary', 13)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (11, N'Pumping Summary', N'Pumping Summary', 14)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (12, N'Flow Classification Summary', N'Flow Classification Summary', 15)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (13, N'LID Performance Summary', N'LID Performance Summary', 16)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (14, N'Rainfall Dependent I/I', N'Rainfall Dependent I/I', 1)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (15, N'Runoff Quantity Continuity', N'Runoff Quantity Continuity', 2)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (16, N'Flow Routing Continuity', N'Flow Routing Continuity', 3)
GO
INSERT [dbo].[tlkpSWMMResults_TableDictionary] ([ResultTableID], [TableName], [TableAlias], [SectionNumber]) VALUES (17, N'Subcatchment Washoff Summary', N'Subcatchment Washoff Summary', 17)
GO


SET IDENTITY_INSERT [tlkpSWMMResults_TableDictionary] OFF
Go

SET IDENTITY_INSERT [tlkpSWMMTableDictionary] ON
Go

INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (1, 1, N'tblSWMM_RunSettings', 0, N'TITLE', 1, N'TITLE', N'TITLEID', 1)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (2, 2, N'tblSWMM_RunSettings', 0, N'OPTIONS', 1, N'OPTIONS', N'OPTIONSID', 1)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (3, 3, N'tblSWMM_EVAPORATION', -1, N'EVAPORATION', 1, N'EVAPORATION', N'EVAPID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (4, 4, N'tblSWMM_RAINGAGES', -1, N'RAINGAGES', 5, N'RAINGAGES', N'RainID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (5, 5, N'tblSWMM_SUBCATCHMENTS', -1, N'SUBCATCHMENTS', 3, N'SUBCATCHMENTS', N'SUBCATCHMENTID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (6, 6, N'tblSWMM_SUBAREAS', -1, N'SUBAREAS', 3, N'SUBAREAS', N'SUBAREAID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (7, 7, N'tblSWMM_INFILTRATION', -1, N'INFILTRATION', 3, N'INFILTRATION', N'INFILTRATIONID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (8, 8, N'tblSWMM_JUNCTIONS', -1, N'JUNCTIONS', 3, N'JUNCTIONS', N'JUNCTIONID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (9, 9, N'tblSWMM_OUTFALLS', -1, N'OUTFALLS', 3, N'OUTFALLS', N'OUTFALLID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (10, 10, N'tblSWMM_STORAGE', -1, N'STORAGE', 3, N'STORAGE', N'STORAGEID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (11, 11, N'tblSWMM_CONDUITS', -1, N'CONDUITS', 3, N'CONDUITS', N'ConduitID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (12, 12, N'tblSWMM_PUMPS', -1, N'PUMPS', 3, N'PUMPS', N'PUMPID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (13, 13, N'tblSWMM_WEIRS', -1, N'WEIRS', 3, N'WEIRS', N'WEIRID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (14, 14, N'tblSWMM_Xsections', -1, N'XSECTIONS', 3, N'XSECTIONS', N'XSectionsID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (15, 15, N'tblSWMM_LOSSES', -1, N'LOSSES', 3, N'LOSSES', N'LOSSESID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (16, 16, N'tblSWMM_DWF', -1, N'DWF', 3, N'DWF', N'DWFID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (17, 17, N'tblSWMM_HYDROGRAPHS', -1, N'HYDROGRAPHS', 4, N'HYDROGRAPHS', N'HYDROGRAPHSID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (18, 18, N'tblSWMM_RDII', -1, N'RDII', 3, N'RDII', N'RDIIID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (19, 19, N'tblSWMM_CURVES', -1, N'CURVES', 4, N'CURVES', N'CURVEID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (20, 20, N'tblSWMM_TIMESERIES', -1, N'TIMESERIES', 4, N'TIMESERIES', N'TIMESERIESID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (21, 21, N'tblSWMM_RunSettings', 0, N'REPORT', 1, N'REPORT', N'REPORTID', 1)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (22, 22, N'tblSWMM_RunSettings', 0, N'TAGS', 1, N'TAGS', N'TAGSID', 1)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (23, 23, N'tblSWMM_RunSettings', 0, N'MAP', 6, N'MAP', N'MAPID', 1)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (24, 24, N'tblSWMM_COORDINATES', -1, N'COORDINATES', 6, N'COORDINATES', N'COORDINATEID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (25, 25, N'tblSWMM_VERTICES', -1, N'VERTICES', 6, N'VERTICES', N'VERTICESID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (26, 26, N'tblSWMM_Polygons', -1, N'Polygons', 6, N'POLYGONS', N'POLYGONSID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (27, 27, N'tblSWMM_SYMBOLS', -1, N'SYMBOLS', 6, N'SYMBOLS', N'GAGEID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (29, 28, N'tblSWMM_LID_Bioretention', -1, N'LID_CONTROLS', 3, N'Bioretention Cell', N'LID_BioretentionID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (30, 29, N'tblSWMM_LID_InfiltrationTrench', -1, N'LID_CONTROLS', 3, N'Infiltration Trench', N'LID_InfiltrationTrenchID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (31, 30, N'tblSWMM_LID_PorousPavement', -1, N'LID_CONTROLS', 3, N'Porous Pavement', N'LID_PorousPavementID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (32, 31, N'tblSWMM_LID_RainBarrel', -1, N'LID_CONTROLS', 3, N'Rain Barrel', N'LID_RainBarrelID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (33, 32, N'tblSWMM_LID_VegetativeSwale', -1, N'LID_CONTROLS', 3, N'Vegetative Swale', N'LID_VegetativeSwaleID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (36, 33, N'tblSWMM_LID_Usage', -1, N'LID_USAGE', 3, N'LID_USAGE', N'LIDUsageID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (37, 45, N'tblSWMM_INFLOWS', -1, N'INFLOWS', NULL, N'INFLOWS', N'INFLOWSID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (67, 1000, N'SpecialCases', 0, N'n/a', -1, N'Special Cases', N'?', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (68, 35, N'tblSWMM_Aquifers', -1, N'AQUIFERS', 3, N'AQUIFERS', N'AquiferID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (69, 36, N'tblSWMM_Groundwater', -1, N'GROUNDWATER', NULL, N'GROUNDWATER', N'GROUNDWATERID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (70, 37, N'tblSWMM_SNOWPACKS', -1, N'SNOWPACKS', NULL, N'SNOWPACKS', N'SNOWPACKSID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (71, 38, N'tblSWMM_TRANSECTS', -1, N'TRANSECTS', NULL, N'TRANSECTS', N'TRANSECTSID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (72, 39, N'tblSWMM_POLLUTANTS', -1, N'POLLUTANTS', NULL, N'POLLUTANTS', N'POLLUTANTSID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (73, 40, N'tblSWMM_LANDUSES', -1, N'LANDUSES', NULL, N'LANDUSES', N'LANDUSESID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (74, 41, N'tblSWMM_COVERAGES', -1, N'COVERAGES', NULL, N'COVERAGES', N'COVERAGESID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (75, 42, N'tblSWMM_LOADINGS', -1, N'LOADINGS', NULL, N'LOADINGS', N'LOADINGSID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (76, 43, N'tblSWMM_BUILDUP', -1, N'BUILDUP', NULL, N'BUILDUP', N'BUILDUPID', 0)
GO
INSERT [dbo].[tlkpSWMMTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [IsScenarioSpecific]) VALUES (77, 44, N'tblSWMM_WASHOFF', -1, N'WASHOFF', NULL, N'WASHOFF', N'WASHOFFID', 0)
GO


SET IDENTITY_INSERT [tlkpSWMMTableDictionary] OFF
Go

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.05c', 'Creating schema and data for SWMM dictionary lookup tables'); 