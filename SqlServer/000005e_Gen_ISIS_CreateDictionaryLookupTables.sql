/****** Object:  Table [dbo].[tlkpISISTableDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
DROP TABLE [dbo].[tlkpISISTableDictionary]
GO
/****** Object:  Table [dbo].[tlkpISISFieldDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
DROP TABLE [dbo].[tlkpISISFieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpISISDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
DROP TABLE [dbo].[tlkpISISDictionary]
GO
/****** Object:  Table [dbo].[tlkpISIS_2D_TableDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
DROP TABLE [dbo].[tlkpISIS_2D_TableDictionary]
GO
/****** Object:  Table [dbo].[tlkpISIS_2D_FieldDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
DROP TABLE [dbo].[tlkpISIS_2D_FieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpISIS_2D_FieldDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpISIS_2D_FieldDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[TableID_FK] [int] NULL,
	[FieldName] [nvarchar](50) NULL,
	[FieldTypeID] [nvarchar](50) NULL,
	[IsIdentifier] [bit] NOT NULL,
	[IsAttributeNode] [bit] NOT NULL,
 CONSTRAINT [PK_tlkpISIS_2D_FieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpISIS_2D_TableDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpISIS_2D_TableDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[ParentNode] [nvarchar](50) NULL,
	[Node] [nvarchar](50) NULL,
	[NodeLevel] [int] NULL,
	[ElementIdentifier] [nvarchar](255) NULL,
	[DELETE] [nvarchar](255) NULL,
	[Hierarchy] [nvarchar](255) NULL,
 CONSTRAINT [PK_tlkpISIS_2D_TableDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpISISDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpISISDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[DOMAIN] [nvarchar](255) NULL,
	[VAL] [int] NULL,
	[ValLabel] [nvarchar](100) NULL,
 CONSTRAINT [PK_tlkpISISDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpISISFieldDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpISISFieldDictionary](
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
 CONSTRAINT [PK_tlkpISISFieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpISISTableDictionary]    Script Date: 4/1/2016 11:37:16 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpISISTableDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[SectionNumber] [float] NULL,
	[TableName] [nvarchar](255) NULL,
	[IsOwnTable] [float] NULL,
	[SectionName] [nvarchar](255) NULL,
	[TableClass] [int] NULL,
	[SectionName_Alias] [nvarchar](255) NULL,
	[KeyColumn] [nvarchar](255) NULL,
	[ContentClass] [int] NULL,
 CONSTRAINT [PK_tlkpISISTableDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


SET IDENTITY_INSERT [tlkpISIS_2D_FieldDictionary] ON
GO

INSERT [dbo].[tlkpISIS_2D_FieldDictionary] ([ID], [TableID_FK], [FieldName], [FieldTypeID], [IsIdentifier], [IsAttributeNode]) VALUES (18, 15, N'file', N'1', 1, 0)
GO
INSERT [dbo].[tlkpISIS_2D_FieldDictionary] ([ID], [TableID_FK], [FieldName], [FieldTypeID], [IsIdentifier], [IsAttributeNode]) VALUES (19, 15, N'value', N'4', 0, 0)
GO
INSERT [dbo].[tlkpISIS_2D_FieldDictionary] ([ID], [TableID_FK], [FieldName], [FieldTypeID], [IsIdentifier], [IsAttributeNode]) VALUES (20, 15, N'time_units', N'3', 0, 1)
GO
INSERT [dbo].[tlkpISIS_2D_FieldDictionary] ([ID], [TableID_FK], [FieldName], [FieldTypeID], [IsIdentifier], [IsAttributeNode]) VALUES (21, 15, N'units', N'3', 0, 1)
GO
INSERT [dbo].[tlkpISIS_2D_FieldDictionary] ([ID], [TableID_FK], [FieldName], [FieldTypeID], [IsIdentifier], [IsAttributeNode]) VALUES (22, 10, N'start_time', N'2', 0, 0)
GO
INSERT [dbo].[tlkpISIS_2D_FieldDictionary] ([ID], [TableID_FK], [FieldName], [FieldTypeID], [IsIdentifier], [IsAttributeNode]) VALUES (23, 10, N'start_date', N'2', 0, 0)
GO
INSERT [dbo].[tlkpISIS_2D_FieldDictionary] ([ID], [TableID_FK], [FieldName], [FieldTypeID], [IsIdentifier], [IsAttributeNode]) VALUES (24, 10, N'unit', N'3', 0, 1)
GO
INSERT [dbo].[tlkpISIS_2D_FieldDictionary] ([ID], [TableID_FK], [FieldName], [FieldTypeID], [IsIdentifier], [IsAttributeNode]) VALUES (25, 10, N'total', N'2', 0, 0)
GO
INSERT [dbo].[tlkpISIS_2D_FieldDictionary] ([ID], [TableID_FK], [FieldName], [FieldTypeID], [IsIdentifier], [IsAttributeNode]) VALUES (26, 4, N'topography', N'2', 0, 0)
GO

SET IDENTITY_INSERT [tlkpISIS_2D_FieldDictionary] OFF
GO


SET IDENTITY_INSERT [tlkpISIS_2D_TableDictionary] ON
GO

INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (1, N'-1', N'ISIS2Dproject', 1, N'-1', NULL, NULL)
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (2, N'ISIS2Dproject', N'link1d', 2, N'-1', NULL, N'ISIS2Dproject\link1d\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (3, N'ISIS2Dproject', N'logfile', 2, N'-1', NULL, N'ISIS2Dproject\logfile\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (4, N'ISIS2Dproject', N'domain', 2, N'-1', NULL, N'ISIS2Dproject\domain\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (5, N'link1d', N'link', 3, N'-1', N'maybe should be field?', NULL)
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (6, N'link1d', N'ief', 3, N'-1', N'maybe should be field?', NULL)
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (7, N'link1d', N'mb', 3, N'-1', N'maybe should be field?', NULL)
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (8, N'domain', N'computational_area', 3, N'-1', NULL, N'ISIS2Dproject\domain\time\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (9, N'domain', N'topography', 3, N'-1', N'Yes- this is on field', NULL)
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (10, N'domain', N'time', 3, N'-1', NULL, N'ISIS2Dproject\domain\time\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (11, N'domain', N'run_data', 3, N'-1', NULL, N'ISIS2Dproject\domain\run_data\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (12, N'domain', N'initial_conditions', 3, N'-1', NULL, N'ISIS2Dproject\domain\initial_conditions\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (13, N'domain', N'roughness', 3, N'-1', NULL, N'ISIS2Dproject\domain\roughness\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (14, N'domain', N'hydrology', 3, N'-1', NULL, N'ISIS2Dproject\domain\hydrology\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (15, N'hydrology', N'source', 4, N'file', NULL, N'ISIS2Dproject\domain\hydrology\source\')
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (16, N'source', N'file', 5, N'-1', N'Yes- this is on field', NULL)
GO
INSERT [dbo].[tlkpISIS_2D_TableDictionary] ([ID], [ParentNode], [Node], [NodeLevel], [ElementIdentifier], [DELETE], [Hierarchy]) VALUES (17, N'source', N'value', 5, N'-1', N'Yes- this is on field', NULL)
GO

SET IDENTITY_INSERT [tlkpISIS_2D_TableDictionary] OFF
GO


SET IDENTITY_INSERT [tlkpISISDictionary] ON
GO

INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (1, N'RTypeCode', 1, N'RAIN')
GO
INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (2, N'RTypeCode', 2, N'EVAP')
GO
INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (3, N'RTypeCode', 3, N'INFI')
GO
INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (4, N'RepeatCode', 1, N'REPEAT')
GO
INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (5, N'RepeatCode', 2, N'EXTEND')
GO
INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (6, N'RepeatCode', 3, N'NOEXTEND')
GO
INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (7, N'SmoothCode', 1, N'SPLINE')
GO
INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (8, N'SmoothCode', 2, N'LINEAR')
GO
INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (9, N'SmoothCode', 3, N'BAR')
GO
INSERT [dbo].[tlkpISISDictionary] ([ID], [DOMAIN], [VAL], [ValLabel]) VALUES (10, N'RTypeCode', 4, N'HTBDY')

GO

SET IDENTITY_INSERT [tlkpISISDictionary] OFF
GO

SET IDENTITY_INSERT [tlkpISISFieldDictionary] ON
GO

INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (415, 38, N'RainETID', N'RainETID', 0, 0, NULL, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (416, 38, N'BoundaryLabel', N'BoundaryLabel', 1, -1, NULL, NULL, NULL, NULL, -1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (417, 38, N'NodeLabel', N'NodeLabel', 1, -2, NULL, NULL, NULL, NULL, -1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (418, 38, N'NumberRecords', N'NumberRecords', 1, 1, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (419, 38, N'RET_Type', N'RET_Type', 1, -3, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (420, 38, N'TimeLag', N'TimeLag', 1, 2, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (421, 38, N'ElevationDatum', N'ElevationDatum', 1, 3, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (422, 38, N'TimeStep', N'TimeStep', 1, 4, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (423, 38, N'RepeatCode', N'RepeatCode', 1, 5, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (424, 38, N'SmoothCode', N'SmoothCode', 1, 6, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (425, 38, N'FlowMultiplier', N'FlowMultiplier', 1, 7, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (426, 38, N'DIFlag', N'DIFlag', 1, 8, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (427, 38, N'ModelVersion', N'ModelVersion', 0, -4, NULL, 3, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (428, 38, N'ParentID', N'ParentID', 1, -5, NULL, 3, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (429, 38, N'ModelDescription', N'ModelDescription', 1, -6, NULL, 7, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (430, 39, N'RainETID_FK', N'RainETID_FK', NULL, 0, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (431, 39, N'PeriodNo', N'PeriodNo', NULL, -1, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (432, 39, N'val', N'val', NULL, 1, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (433, 40, N'HTID', N'HTID', 0, 0, NULL, 1, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (434, 40, N'BoundaryLabel', N'BoundaryLabel', 1, -1, NULL, NULL, NULL, NULL, -1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (435, 40, N'NodeLabel', N'NodeLabel', 1, -2, NULL, NULL, NULL, NULL, -1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (436, 40, N'NumberRecords', N'NumberRecords', 1, 1, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (438, 40, N'ElevationDatum', N'ElevationDatum', 1, 2, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (439, 40, N'TimeStep', N'TimeStep', 1, 3, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (440, 40, N'RepeatCode', N'RepeatCode', 1, 4, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (441, 40, N'SmoothCode', N'SmoothCode', 1, 5, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (442, 40, N'ModelVersion', N'ModelVersion', 0, -3, NULL, 3, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (443, 40, N'ParentID', N'ParentID', 0, -4, NULL, 3, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (444, 40, N'ModelDescription', N'ModelDescription', 0, -5, NULL, 7, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (445, 41, N'HTID_FK', N'HTID_FK', NULL, 0, NULL, NULL, NULL, NULL, NULL)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (446, 41, N'PeriodNo', N'PeriodNo', NULL, -1, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (447, 41, N'val', N'val', NULL, 1, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (448, 39, N'TimeVal', N'TimeVal', 1, 2, NULL, NULL, NULL, NULL, 1)
GO
INSERT [dbo].[tlkpISISFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [FieldClass], [VarOptionList_FK], [IsLookupList], [RowNo]) VALUES (449, 41, N'TimeVal', N'TimeVal', 1, 2, NULL, NULL, NULL, NULL, 1)
GO

SET IDENTITY_INSERT [tlkpISISFieldDictionary] OFF
GO


SET IDENTITY_INSERT [tlkpISISTableDictionary] ON
GO

INSERT [dbo].[tlkpISISTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [ContentClass]) VALUES (38, NULL, N'tblRainETBoundary', -1, N'REBDY', 2, N'Rainfall / Evaporation / Infiltration', N'RainETID', 1)
GO
INSERT [dbo].[tlkpISISTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [ContentClass]) VALUES (39, NULL, N'tblRainETBoundaryDetail', -1, N'REBDY_DETAIL', 3, N'Rain ET Details', N'RainETDetailID', NULL)
GO
INSERT [dbo].[tlkpISISTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [ContentClass]) VALUES (40, NULL, N'tblHTBoundary', -1, N'HTBDY', 2, N'Level', N'HTID', 1)
GO
INSERT [dbo].[tlkpISISTableDictionary] ([ID], [SectionNumber], [TableName], [IsOwnTable], [SectionName], [TableClass], [SectionName_Alias], [KeyColumn], [ContentClass]) VALUES (41, NULL, N'tblHTBoundaryDetail', -1, N'HTBDY_DETAIL', 3, N'Rain ET Details', N'HTIDDetailID', NULL)
GO

SET IDENTITY_INSERT [tlkpISISTableDictionary] ON
GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.05e', 'Creating schema and data for ISIS dictionary lookup tables'); 