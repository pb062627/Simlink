/****** Object:  Table [dbo].[tlkpSimLinkTableDictionary]    Script Date: 4/4/2016 8:16:09 AM ******/
DROP TABLE [dbo].[tlkpSimLinkTableDictionary]
GO
/****** Object:  Table [dbo].[tlkpSimLinkFieldDictionary]    Script Date: 4/4/2016 8:16:09 AM ******/
DROP TABLE [dbo].[tlkpSimLinkFieldDictionary]
GO
/****** Object:  Table [dbo].[tblSimLink_TableMaster]    Script Date: 4/4/2016 8:16:09 AM ******/
DROP TABLE [dbo].[tblSimLink_TableMaster]
GO
/****** Object:  Table [dbo].[tblSimLink_TableMaster]    Script Date: 4/4/2016 8:16:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[tblSimLink_TableMaster](
	[ID] [int] Identity(1,1) NOT NULL,
	[TableName] [nvarchar](255) NULL,
	[ALL] [real] NULL,
	[Modflow] [real] NULL,
	[SSMA_TimeStamp] [binary](8) NOT NULL,
 CONSTRAINT [PK_tblSimLink_TableMaster] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[tlkpSimLinkFieldDictionary]    Script Date: 4/4/2016 8:16:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSimLinkFieldDictionary](
	[FieldID] [int] Identity(1,1) NOT NULL,
	[TableID_FK] [int] NULL,
	[FieldClass] [nvarchar](50) NULL,
	[FieldName] [nvarchar](100) NULL,
	[FieldAlias] [nvarchar](100) NULL,
 CONSTRAINT [PK_tlkpSimLinkFieldDictionary] PRIMARY KEY CLUSTERED 
(
	[FieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpSimLinkTableDictionary]    Script Date: 4/4/2016 8:16:09 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpSimLinkTableDictionary](
	[ID] [int] Identity(1,1) NOT NULL,
	[TableName] [nvarchar](255) NULL,
	[KeyColumn] [nvarchar](255) NULL,
 CONSTRAINT [PK_tlkpSimLinkTableDictionary] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [tblSimLink_TableMaster] ON
GO

INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (1, N'tblDistrib_ElementList_XREF', 1, 0, 0x000000000000523F)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (2, N'tblDV', 1, 0, 0x0000000000005240)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (3, N'tblDV_Group', 1, 0, 0x0000000000005241)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (4, N'tblDV_GroupXREF', 1, 0, 0x0000000000005242)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (5, N'tblElement_XREF', 1, 0, 0x0000000000005243)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (6, N'tblElementListDetails', 1, 0, 0x0000000000005244)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (7, N'tblElementLists', 1, 0, 0x0000000000005245)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (8, N'tblEvaluationGroup', 1, 1, 0x0000000000005246)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (9, N'tblEvaluationGroup_DistribDetail', 1, 0, 0x0000000000005247)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (10, N'tblEvaluationGroup_DistribDetail_OrderingList', 1, 0, 0x0000000000005248)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (11, N'tblModElementVals', 1, 0, 0x0000000000005249)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (12, N'tblMouserResults', 1, 0, 0x000000000000524A)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (13, N'tblNewFeature', 1, 0, 0x000000000000524B)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (14, N'tblOptionDetails', 1, 0, 0x000000000000524C)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (15, N'tblOptionLists', 1, 0, 0x000000000000524D)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (16, N'tblPerformance', 1, 0, 0x000000000000524E)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (17, N'tblPerformance_Detail', 1, 0, 0x000000000000524F)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (18, N'tblPerformance_Grouping', 1, 0, 0x0000000000005250)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (19, N'tblPerformance_GroupXREF', 1, 0, 0x0000000000005251)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (20, N'tblPerformance_ResultXREF', 1, 0, 0x0000000000005252)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (21, N'tblProj', 1, 0, 0x0000000000005253)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (22, N'tblResultTS', 1, 1, 0x0000000000005254)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (23, N'tblResultTS_Detail', 1, 1, 0x0000000000005255)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (24, N'tblResultTS_EventSummary', 1, 0, 0x0000000000005256)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (25, N'tblResultTS_EventSummary_Detail', 1, 0, 0x0000000000005257)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (26, N'tblResultVar', 1, 0, 0x0000000000005258)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (27, N'tblResultVar_Details', 1, 0, 0x0000000000005259)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (28, N'tblScenario', 1, 1, 0x000000000000525A)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (29, N'tblScenario_DistribDetail', 1, 0, 0x000000000000525B)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (30, N'tblScenario_SpecialOps', 1, 0, 0x000000000000525C)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (31, N'tlkpCostData', 1, 0, 0x000000000000525D)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (32, N'tlkpDistributionDefinition', 1, 0, 0x000000000000525E)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (33, N'tlkpIWFieldDictionary', 1, 0, 0x000000000000525F)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (34, N'tlkpIWResults_FieldDictionary', 1, 0, 0x0000000000005260)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (35, N'tlkpIWTableDictionary', 1, 0, 0x0000000000005261)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (36, N'tlkpIWTableDictionary_Key', 1, 0, 0x0000000000005262)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (37, N'tlkpModFlow_FieldDictionary', 1, 1, 0x0000000000005263)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (38, N'tlkpMouseFieldDictionary', 1, 0, 0x0000000000005264)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (39, N'tlkpMouseResults_FieldDictionary', 1, 0, 0x0000000000005265)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (40, N'tlkpMOUSEResults_TableDictionary', 1, 0, 0x0000000000005266)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (41, N'tlkpMouseTableDictionary', 1, 0, 0x0000000000005267)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (42, N'tlkpProj_EV_OutfallClassify', 1, 0, 0x0000000000005268)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (43, N'tlkpSWMMFieldDictionary', 1, 0, 0x0000000000005269)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (44, N'tlkpSWMMFieldDictionary_Key', 1, 0, 0x000000000000526A)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (45, N'tlkpSWMMResults_FieldDictionary', 1, 0, 0x000000000000526B)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (46, N'tlkpSWMMResults_TableDictionary', 1, 0, 0x000000000000526C)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (47, N'tlkpSWMMTableDictionary', 1, 0, 0x000000000000526D)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (48, N'tlkpSWMMTableDictionary_Key', 1, 0, 0x000000000000526E)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (49, N'tlkpUI_Dictionary', 1, 0, 0x000000000000526F)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (50, N'tlkpUnitConversions', 1, 0, 0x0000000000005270)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (51, N'tlkpUnitSettings', 1, 0, 0x0000000000005271)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (52, N'tlkpUnitTypes', 1, 0, 0x0000000000005272)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (53, N'qryDELETE_Examples', 1, 0, 0x0000000000005273)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (54, N'qryDELETE_Link1', 1, 0, 0x0000000000005274)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (55, N'qryDELETE002_sdv', 1, 0, 0x0000000000005275)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (56, N'qryDELETE002_wdv', 1, 0, 0x0000000000005276)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (57, N'qryElementList001_DistribXREF', 1, 0, 0x0000000000005277)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (58, N'qryElementList001_IW', 1, 0, 0x0000000000005278)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (59, N'qryElementList001_ListVars', 1, 0, 0x0000000000005279)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (60, N'qryElementList001_SWMM', 1, 0, 0x000000000000527A)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (61, N'qryElementList002_DistribXREF_Model', 1, 0, 0x000000000000527B)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (62, N'qryElementList002_IWDetails', 1, 0, 0x000000000000527C)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (63, N'qryElementList002_SWMMDetails', 1, 0, 0x000000000000527D)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (64, N'qryEVDELETE_MEV', 1, 0, 0x000000000000527E)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (65, N'qryHYG_BadScenario_DELETE_ModelElementVals', 1, 0, 0x000000000000527F)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (66, N'qryHYG_BadScenario_DELETE_PerformanceDetail', 1, 0, 0x0000000000005280)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (67, N'qryHYG_BadScenario_DELETE_ResultDetail', 1, 0, 0x0000000000005281)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (68, N'qryHYG_Scenario_UPDATE_SetStartEnd_HasBeenRun', 1, 0, 0x0000000000005282)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (69, N'qryOption001_Link', 1, 0, 0x0000000000005283)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (70, N'qryOption001_MinMax', 1, 0, 0x0000000000005284)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (71, N'qryOption002_Link', 1, 0, 0x0000000000005285)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (72, N'qryPerformance001_Cost_ElementLists', 1, 0, 0x0000000000005286)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (73, N'qryPerformance001_Cost_LookupTable', 1, 0, 0x0000000000005287)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (74, N'qryPerformance001_Dist', 1, 0, 0x0000000000005288)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (75, N'qryPerformance001_DVTable', 1, 0, 0x0000000000005289)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (76, N'qryPerformance001_LinkPerformanceXREF', 1, 0, 0x000000000000528A)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (77, N'qryPerformance001_ResultsLinkXREF', 1, 0, 0x000000000000528B)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (78, N'qryPerformance001_ResultTable', 1, 0, 0x000000000000528C)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (79, N'qryPerformance002_Cost_Lookup_GROUPBY', 1, 0, 0x000000000000528D)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (80, N'qryPerformance002_Cost_Lookup_Type1', 1, 0, 0x000000000000528E)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (81, N'qryPerformance002_Cost_Lookup_Type5', 1, 0, 0x000000000000528F)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (82, N'qryPerformance002_ResultXREF_Total', 1, 0, 0x0000000000005290)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (83, N'qryPerformance002_Union', 1, 0, 0x0000000000005291)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (84, N'qryPerformance003_Cost_Lookup_UNION', 1, 0, 0x0000000000005292)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (85, N'qryPerformance004_Cost_Lookup_LinkToVal', 1, 0, 0x0000000000005293)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (86, N'qryPerformance100_PerfGroup', 1, 0, 0x0000000000005294)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (87, N'qryPerformance100_PerfGroupToGroup', 1, 0, 0x0000000000005295)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (88, N'qryPerformance100_PerformanceVals', 1, 0, 0x0000000000005296)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (89, N'qryPerformance101_GetObjective', 1, 0, 0x0000000000005297)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (90, N'qryPerformanceDIST001_LinkCost', 1, 0, 0x0000000000005298)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (91, N'qryProj_EV_Cost001', 1, 0, 0x0000000000005299)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (92, N'qryProj_EV_Performance002Link', 1, 0, 0x000000000000529A)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (93, N'qryProj_EV_Performance003_CostCategory', 1, 0, 0x000000000000529B)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (94, N'qryProj_EV_Performance003_SelectType', 1, 0, 0x000000000000529C)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (95, N'qryProj_EV_PerformanceCSO001', 1, 0, 0x000000000000529D)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (96, N'qryProj_EV_Result001_TimeSeries', 1, 0, 0x000000000000529E)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (97, N'qryProj_EV_Result002_Aggregate', 1, 0, 0x000000000000529F)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (98, N'qryProj_EV_Result002_DWF_Sum', 1, 0, 0x00000000000052A0)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (99, N'qryProj_EV_ResultLink', 1, 0, 0x00000000000052A1)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (100, N'qryProj_EV_ResultLink002_Crosstab', 1, 0, 0x00000000000052A2)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (101, N'qryProj_EV_Results001_EventSummary', 1, 0, 0x00000000000052A3)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (102, N'qryProj_EV_Results002_ActivationCount', 1, 0, 0x00000000000052A4)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (103, N'qryProj_EV_Results003_ActivationsCrossTab', 1, 0, 0x00000000000052A5)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (104, N'qryPROJ_EV_RMG_Results002_TimesSeries_Crosstab', 1, 0, 0x00000000000052A6)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (105, N'qryProj_EV_Scenario001_EG99_Link', 1, 0, 0x00000000000052A7)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (106, N'qryProj_EV001_LevelData', 1, 0, 0x00000000000052A8)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (107, N'qryProj_EV002_LevelData_Crosstab', 1, 0, 0x00000000000052A9)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (108, N'qryRMG_DV_CONS_001_IW_LinkInfo', 1, 0, 0x00000000000052AA)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (109, N'qryRMG_DV_CONS_001_Mouse_ListVar', 1, 0, 0x00000000000052AB)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (110, N'qryRMG_DV_CONS_001_Mouse_NoListVar', 1, 0, 0x00000000000052AC)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (111, N'qryRMG_DV_CONS_001_SWMM_ListVar', 1, 0, 0x00000000000052AD)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (112, N'qryRMG_DV_CONS_001_SWMM_NoListVar', 1, 0, 0x00000000000052AE)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (113, N'qryRMG_DV_CONS_002_Mouse_UNION_List', 1, 0, 0x00000000000052AF)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (114, N'qryRMG_DV_CONS_002_SWMM_UNION_List', 1, 0, 0x00000000000052B0)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (115, N'qryRMG_DV001_GroupLink', 1, 0, 0x00000000000052B1)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (116, N'qryRMG_DV001_IW_LinkInfo', 1, 0, 0x00000000000052B2)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (117, N'qryRMG_DV001_LinkScenario', 1, 0, 0x00000000000052B3)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (118, N'qryRMG_DV001_Mouse_LinkInfo', 1, 0, 0x00000000000052B4)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (119, N'qryRMG_DV001_SpecialOps_EG_Distinct', 1, 0, 0x00000000000052B5)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (120, N'qryRMG_DV001_SWMM_LinkInfo', 1, 0, 0x00000000000052B6)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (121, N'qryRMG_DV002_Crosstab', 1, 0, 0x00000000000052B7)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (122, N'qryRMG_DV003_SelectScenarioHaving', 1, 0, 0x00000000000052B8)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (123, N'qryRMG_Result0005_MOUSE_LinkInfo', 1, 0, 0x00000000000052B9)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (124, N'qryRMG_Result001_IW_LinkInfo', 1, 0, 0x00000000000052BA)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (125, N'qryRMG_Result001_SWMM_LinkInfo', 1, 0, 0x00000000000052BB)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (126, N'qryRMG_Result001TS_MODFLOW_LinkInfo', 1, 1, 0x00000000000052BC)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (127, N'qryRMG_Result002_IW_IsListVar', 1, 0, 0x00000000000052BD)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (128, N'qryRMG_Result002_IW_NoListVar', 1, 0, 0x00000000000052BE)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (129, N'qryRMG_Result003_IW_UNION', 1, 0, 0x00000000000052BF)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (130, N'qryRMG_Result100_IW_Link', 1, 0, 0x00000000000052C0)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (131, N'qryRMG_Results001_MOUSE_Link_Info', 1, 0, 0x00000000000052C1)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (132, N'qryRMG_Results001_TimeSeries', 1, 0, 0x00000000000052C2)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (133, N'qryRMG_Results001_TS_LinkXREF', 1, 0, 0x00000000000052C3)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (134, N'qryRMG_Results001_TS_ToDSS', 1, 0, 0x00000000000052C4)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (135, N'qryRMG_Results002_TimesSeries_Crosstab', 1, 0, 0x00000000000052C5)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (136, N'qryRMG_Results002_TSToDSS_CUSTOM', 1, 0, 0x00000000000052C6)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (137, N'qryRMG001_EvalReference_Crosstab', 1, 0, 0x00000000000052C7)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (138, N'qryRMG001_IW_ModelChanges', 1, 0, 0x00000000000052C8)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (139, N'qryRMG001_IW_Results', 1, 0, 0x00000000000052C9)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (140, N'qryRMG001_Link_ScenarioEvalProj_DELETE_SEE_RMG', 1, 0, 0x00000000000052CA)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (141, N'qryRMG001_MouseModelChanges', 1, 0, 0x00000000000052CB)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (142, N'qryRMG001_MouseModelInserts', 1, 0, 0x00000000000052CC)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (143, N'qryRMG001_RMG_ModValScenarioEvalProj_Link', 1, 0, 0x00000000000052CD)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (144, N'qryRMG001_RMG_ScenarioEvalProj_Link', 1, 0, 0x00000000000052CE)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (145, N'qryRMG001_SWMM_ModelChanges', 1, 0, 0x00000000000052CF)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (146, N'qryRMG001_SWMM_Results_OUT', 1, 0, 0x00000000000052D0)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (147, N'qryRMG002_MouseInsertValues', 1, 0, 0x00000000000052D1)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (148, N'qryRPT_Cost001', 1, 0, 0x00000000000052D2)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (149, N'qryRPT_Cost002_Aggregate', 1, 0, 0x00000000000052D3)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (150, N'qryScenario001_Link', 1, 0, 0x00000000000052D4)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (151, N'qryTEMP001_ElementList_IW', 1, 0, 0x00000000000052D5)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (152, N'qryUnit001_LinkType', 1, 0, 0x00000000000052D6)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (153, N'qryUnit002_IW', 1, 0, 0x00000000000052D7)
GO
INSERT [dbo].[tblSimLink_TableMaster] ([ID], [TableName], [ALL], [Modflow], [SSMA_TimeStamp]) VALUES (154, N'tblSimLink_TableMaster', 1, 0, 0x00000000000052D8)
GO


SET IDENTITY_INSERT [tblSimLink_TableMaster] OFF
GO


SET IDENTITY_INSERT [tlkpSimLinkFieldDictionary] ON
GO

INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (1, 1, N'-1', N'DVD_ID', N'DVD_ID')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (2, 1, N'-2', N'DV_Label', N'DV_Label')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (3, 1, N'-3', N'sqn', N'sqn')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (4, 1, N'-4', N'EvaluationGroup_FK', N'EvaluationGroup_FK')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (5, 1, N'-5', N'VarType_FK', N'VarType_FK')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (6, 1, N'-6', N'DV_Description', N'DV_Description')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (7, 1, N'-7', N'DV_Type', N'DV_Type')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (8, 1, N'-8', N'Option_FK', N'OptionID')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (9, 1, N'-9', N'Option_MIN', N'Option_MIN')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (10, 1, N'-10', N'Option_MAX', N'Option_MAX')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (11, 1, N'-11', N'GetNewValMethod', N'GetNewValMethod')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (12, 1, N'-12', N'FunctionID_FK', N'FunctionID_FK')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (13, 1, N'-13', N'FunctionArgs', N'FunctionArgs')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (14, 1, N'-14', N'ElementID_FK', N'ElementID_FK')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (15, 1, N'-15', N'Element_Label', N'Element_Label')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (16, 1, N'-16', N'IncludeInScenarioLabel', N'IncludeInScenarioLabel')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (17, 1, N'-17', N'IsListVar', N'IsListVar')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (18, 1, N'-18', N'SkipMinVal', N'SkipMinVal')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (19, 1, N'-19', N'CostID_FK', N'CostID_FK')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (20, 1, N'-20', N'Operation_DV', N'Operation_DV')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (21, 1, N'-21', N'SecondaryDV_Key', N'SecondaryDV_Key')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (22, 1, N'-22', N'PrimaryDV_ID_FK', N'PrimaryDV_ID_FK')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (23, 1, N'-23', N'COST_ResultID_FK', N'COST_ResultID_FK')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (24, 1, N'-24', N'Qualifier1', N'Qualifier1')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (25, 1, N'-25', N'Qual1Pos', N'Qual1Pos')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (26, 1, N'-26', N'IsSpecialCase', N'IsSpecialCase')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (27, 1, N'-27', N'IsDistrib', N'IsDistrib')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (28, NULL, N'-28', N'Distrib_VarType_FK', N'Distrib_VarType_FK')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (29, NULL, N'-29', N'IsTS', N'IsTS')
GO
INSERT [dbo].[tlkpSimLinkFieldDictionary] ([FieldID], [TableID_FK], [FieldClass], [FieldName], [FieldAlias]) VALUES (30, NULL, N'-30', N'XModelID_FK', N'XModelID_FK')
GO

SET IDENTITY_INSERT [tlkpSimLinkFieldDictionary] OFF
GO


SET IDENTITY_INSERT [tlkpSimLinkTableDictionary] ON
GO

INSERT [dbo].[tlkpSimLinkTableDictionary] ([ID], [TableName], [KeyColumn]) VALUES (1, N'tblDV', N'DVD_ID')
GO

SET IDENTITY_INSERT [tlkpSimLinkTableDictionary] OFF
GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.10', 'Creating schema and data for SimLink Master dictionary lookup tables'); 