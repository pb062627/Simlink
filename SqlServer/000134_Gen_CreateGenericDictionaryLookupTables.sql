/****** Object:  Table [dbo].[tlkpModelAttribute]    Script Date: 3/7/2016 1:21:59 PM ******/
DROP TABLE [dbo].[tlkpModelAttribute]
GO
/****** Object:  Table [dbo].[tlkpModelAttributeSection]    Script Date: 3/7/2016 1:21:59 PM ******/
DROP TABLE [dbo].[tlkpModelAttributeSection]
GO
/****** Object:  Table [dbo].[tlkpModelType]    Script Date: 3/7/2016 1:21:59 PM ******/
DROP TABLE [dbo].[tlkpModelType]
GO
/****** Object:  Table [dbo].[tlkpModelAttribute]    Script Date: 3/7/2016 1:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/*add a triple primary key on this so we can keep existing IDs*/
CREATE TABLE [dbo].[tlkpModelAttribute](
	[ID] [int] NOT NULL,
	[ModelTypeID_FK] [int] NOT NULL,
	[SectionID_FK] [int] default -1,
	[FieldName] [nvarchar](255) NOT NULL,
	[FieldAlias] [nvarchar](255) NULL,
	[FieldINP_ColNo] [int] default -1,
	[FieldINP_RowNo] [int] default -1,
	[FieldAPI_Code] [int] default -1,
	[FieldClass] [int] default -1,
	[IsResult] [bit] NOT NULL,
	
CONSTRAINT [PK_tlkpModelAttribute] PRIMARY KEY CLUSTERED 
(
	[ID], [ModelTypeID_FK], [IsResult] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]) 
ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpModelAttributeSection]    Script Date: 3/7/2016 1:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpModelAttributeSection](
	[SectionID] [int] NOT NULL,
	[SectionName] [nvarchar](255) NOT NULL,
	[SectionTableName] [nvarchar](255) NULL,
	
CONSTRAINT [PK_tlkpModelAttributeSection] PRIMARY KEY CLUSTERED 
(
	[SectionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])
 ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpModelType]    Script Date: 3/7/2016 1:21:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpModelType](
	[ModelTypeID] [int] NOT NULL,
	[ModelTypeName] [nvarchar](255) NOT NULL,
	
CONSTRAINT [PK_tlkpModelType] PRIMARY KEY CLUSTERED 
(
	[ModelTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY])
 ON [PRIMARY]

GO

/*populate the model type table*/
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (1, N'SWMM')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (2, N'IW')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (3, N'EPANET')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (4, N'MODFLOW')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (7, N'ISIS1D')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (8, N'SimClim')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (9, N'ISIS2D')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (10, N'ISIS_FAST')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (11, N'ExtendSim')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (12, N'Excel')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (13, N'Simlink')
GO
INSERT [dbo].[tlkpModelType] ([ModelTypeID], [ModelTypeName]) VALUES (14, N'Vissim')
GO

/*populate the model attribute table with VISSIM entries*/
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (200, 14, 2, N'AssumSpeedOncom', N'AssumSpeedOncom', -1, -1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (400, 14, 4, N'DesSpeedDistr(10)', N'DesSpeedDistr - Car', -1, -1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (401, 14, 4, N'DesSpeedDistr(20)', N'DesSpeedDistr - HGV', -1, -1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (402, 14, 4, N'DesSpeedDistr(30)', N'DesSpeedDistr - Bus', -1, -1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (600, 14, 6, N'MinGapTime', N'MinGapTime', -1, -1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (601, 14, 6, N'MinHdwy', N'MinHeadWay', -1, -1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (100, 14, 1, N'TotRes\VehDelay(Current,Current,All)', N'Average Vehicle Delay', -1, -1, -1, -1, -1)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (101, 14, 1, N'TotRes\Vehs(Current,Total,All)', N'Total Number Vehicles Through Node', -1, -1, -1, -1, -1)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (700, 14, 7, N'MaxRate', N'Maximum Rate of Ramp Speed', 3, 1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (701, 14, 7, N'MinRate', N'Minimum Rate of Ramp Speed', 3, 1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (201, 14, 2, N'NumLanes', N'Number of Lanes associated with Link', -1, -1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (202, 14, 2, N'Length2D', N'2D Length of Link', -1, -1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (203, 14, 2, N'LinkEvalSegLen', N'Length of a segment within the Link', -1, -1, -1, -1, 0)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (800, 14, 8, N'QLen(Avg,Avg)', N'Average Queue Length averaged across all time intervals and averaged across all simulations', -1, -1, -1, -1, -1)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (801, 14, 8, N'QLenMax(Avg,Avg)', N'Max Queue Length averaged across all time intervals and averaged for all simulations', -1, -1, -1, -1, -1)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (900, 14, 9, N'Concatenate:LinkEvalSegs\Volume(Avg, Last, All)', N'Volume all Car Types averaged across all simulations for the Last time interval', -1, -1, -1, -1, -1)
INSERT [dbo].[tlkpModelAttribute] ([ID], [ModelTypeID_FK], [SectionID_FK], [FieldName], [FieldAlias], [FieldINP_ColNo], [FieldINP_RowNo], [FieldAPI_Code], [FieldClass], [IsResult]) 
VALUES (900, 14, 9, N'TravTm(Avg,Avg,All)', N'Travel time averaged across all simulation, averaged across all time intervals and for all vehicle types', -1, -1, -1, -1, -1)
GO

/*populate the model section table with VISSIM entries*/
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (-1, N'Default', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (1, N'Node', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (2, N'Link', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (3, N'Area', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (4, N'DesSpeedDecision', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (5, N'PriorityRule', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (6, N'ConflictMarker', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (7, N'VAP', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (8, N'QueueCounter', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (9, N'LinkEvalSegment', NULL)
INSERT [dbo].[tlkpModelAttributeSection] ([SectionID], [SectionName], [SectionTableName]) 
VALUES (10, N'VehicleTravelTimeMeasurement', NULL)
GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.34', 'Creating generic model attribute lookup tables'); 