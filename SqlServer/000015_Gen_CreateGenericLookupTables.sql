/****** Object:  Table [dbo].[tlkpUnitTypes]    Script Date: 4/4/2016 8:34:49 AM ******/
DROP TABLE [dbo].[tlkpUnitTypes]
GO
/****** Object:  Table [dbo].[tlkpUnitSettings]    Script Date: 4/4/2016 8:34:49 AM ******/
DROP TABLE [dbo].[tlkpUnitSettings]
GO
/****** Object:  Table [dbo].[tlkpUnitConversions]    Script Date: 4/4/2016 8:34:49 AM ******/
DROP TABLE [dbo].[tlkpUnitConversions]
GO
/****** Object:  Table [dbo].[tlkpUI_Dictionary]    Script Date: 4/4/2016 8:34:49 AM ******/
DROP TABLE [dbo].[tlkpUI_Dictionary]
GO
/****** Object:  Table [dbo].[tlkpDistributionDefinition]    Script Date: 4/4/2016 8:34:49 AM ******/
DROP TABLE [dbo].[tlkpDistributionDefinition]
GO
/****** Object:  Table [dbo].[tlkpDistribStrategy]    Script Date: 4/4/2016 8:34:49 AM ******/
DROP TABLE [dbo].[tlkpDistribStrategy]
GO
/****** Object:  Table [dbo].[tlkpCostData]    Script Date: 4/4/2016 8:34:49 AM ******/
DROP TABLE [dbo].[tlkpCostData]
GO
/****** Object:  Table [dbo].[tlkpCostData]    Script Date: 4/4/2016 8:34:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpCostData](
	[CostItemID] [int] Identity(1,1) NOT NULL,
	[CostCategory] [nvarchar](255) NULL,
	[CostItem] [nvarchar](255) NULL,
	[Unit] [nvarchar](255) NULL,
	[costYear] [int] NULL,
	[Source] [nvarchar](255) NULL,
	[CapitalCost] [int] NULL,
	[MaintCostPerc] [float] NULL,
	[DesignLife] [int] NULL,
	[AdditionalNote] [nvarchar](255) NULL,
	[Qual1] [nvarchar](50) NULL,
	[LinkVal1] [nvarchar](50) NULL,
	[ProjID_Fk] [int] NULL,
	[InvalidVal] [bit] NOT NULL,
 CONSTRAINT [PK_tlkpCostData] PRIMARY KEY CLUSTERED 
(
	[CostItemID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpDistribStrategy]    Script Date: 4/4/2016 8:34:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpDistribStrategy](
	[DistribStrategyID] [int] Identity(1,1) NOT NULL,
	[DistribLabel] [nvarchar](25) NULL,
	[DistribDescrip] [nvarchar](255) NULL,
	[DistribQual_FK] [int] NULL,
	[ProjID_FK] [int] NULL,
	[DistribIntensity] [int] NULL,
 CONSTRAINT [PK_tlkpDistribStrategy] PRIMARY KEY CLUSTERED 
(
	[DistribStrategyID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpDistributionDefinition]    Script Date: 4/4/2016 8:34:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpDistributionDefinition](
	[DistribID] [int] Identity(1,1) NOT NULL,
	[Label] [nvarchar](255) NOT NULL,
	[PubPriv] [int] NULL,
	[Descrip] [nvarchar](255) NULL,
	[CostItem_FK] [int] NOT NULL,
	[DistribTotal_FK] [int] NULL,
	[ProjID_FK] [int] NULL,
	[Qualifier] [nvarchar](25) NULL,
	[val] [float] NULL,
 CONSTRAINT [PK_tlkpDistributionDefinition] PRIMARY KEY CLUSTERED 
(
	[DistribID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpUI_Dictionary]    Script Date: 4/4/2016 8:34:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpUI_Dictionary](
	[DictID] [int] Identity(1,1) NOT NULL,
	[Category] [nvarchar](255) NULL,
	[Subcategory] [nvarchar](255) NULL,
	[val] [nvarchar](255) NULL,
	[sequence] [int] NULL,
	[ValInt] [int] NULL,
 CONSTRAINT [PK_tlkpUI_Dictionary] PRIMARY KEY CLUSTERED 
(
	[DictID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpUnitConversions]    Script Date: 4/4/2016 8:34:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpUnitConversions](
	[UnitID] [int] Identity(1,1) NOT NULL,
	[UnitSettingID_FK] [int] NULL,
	[UnitTypeID_FK] [int] NULL,
	[UnitConversion] [float] NULL,
 CONSTRAINT [PK_tlkpUnitConversions] PRIMARY KEY CLUSTERED 
(
	[UnitID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpUnitSettings]    Script Date: 4/4/2016 8:34:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpUnitSettings](
	[UnitSettingID] [int] Identity(1,1) NOT NULL,
	[ModelTypeID_FK] [int] NULL,
	[UnitSetting] [nvarchar](255) NULL,
 CONSTRAINT [PK_tlkpUnitSettings] PRIMARY KEY CLUSTERED 
(
	[UnitSettingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tlkpUnitTypes]    Script Date: 4/4/2016 8:34:49 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpUnitTypes](
	[UnitTypeID] [int] Identity(1,1) NOT NULL,
	[UnitType] [nvarchar](50) NULL,
 CONSTRAINT [PK_tlkpUnitTypes] PRIMARY KEY CLUSTERED 
(
	[UnitTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [tlkpCostData] ON
GO

INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (158, N'Manhole Repair and Rehabilitation', N'Replace Cover/Frame/Seal', N'each', 2011, N'See Newport Memo', 1056, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (159, N'Manhole Repair and Rehabilitation', N'Replace frame Seal Only', N'each', 2011, N'See Newport Memo', 720, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (160, N'Manhole Repair and Rehabilitation', N'Replace Cover/Frame/Seal/Chimney', N'each', 2011, N'See Newport Memo', 2520, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (161, N'Manhole Repair and Rehabilitation', N'Chimney Rehabilitation', N'each', 2011, N'See Newport Memo', 780, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (162, N'Manhole Repair and Rehabilitation', N'Replace Frame Seal/Chimney', N'each', 2011, N'See Newport Memo', 1800, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (163, N'Manhole Repair and Rehabilitation', N'Corbel Rehabilitation', N'each', 2011, N'See Newport Memo', 600, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (164, N'Manhole Repair and Rehabilitation', N'Corbel Replacement', N'each', 2011, N'See Newport Memo', 2112, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (165, N'Manhole Repair and Rehabilitation', N'Wall Rehabilitation', N'each', 2011, N'See Newport Memo', 1020, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (166, N'Manhole Repair and Rehabilitation', N'Bench/Trough Rehabilitation', N'each', 2011, N'See Newport Memo', 656, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (167, N'Manhole Repair and Rehabilitation', N'Replace Bench/Trough', N'each', 2011, N'See Newport Memo', 1440, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (168, N'Manhole Repair and Rehabilitation', N'Replace Steps', N'each', 2011, N'See Newport Memo', 900, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (169, N'Manhole Repair and Rehabilitation', N'Grade Adjustment', N'each', 2011, N'See Newport Memo', 1452, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (170, N'Manhole Repair and Rehabilitation', N'Plug Lift Holes/Patch Holes', N'each', 2011, N'See Newport Memo', 360, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (171, N'Manhole Repair and Rehabilitation', N'Seal Precast Joints', N'each', 2011, N'See Newport Memo', 660, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (172, N'Pipe Repair', N'Grout Pipe Seal', N'each', 2011, N'See Newport Memo', 540, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (173, N'Pipe Repair', N'Coat Pipe Seal', N'each', 2011, N'See Newport Memo', 198, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (174, N'Pipe Lining', N'Sewer Lining 6" Pipe', N'each', 2011, N'See Newport Memo', 138, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (175, N'Pipe Lining', N'Sewer Lining 12" Pipe', N'each', 2011, N'See Newport Memo', 138, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (176, N'Pipe Lining', N'Sewer Lining 18” Pipe', N'each', 2011, N'See Newport Memo', 264, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (177, N'Pipe Lining', N'Sewer Lining 24” Pipe', N'each', 2011, N'See Newport Memo', 264, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (178, N'Pipe Lining', N'Sewer Lining 30” Pipe', N'each', 2011, N'See Newport Memo', 264, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (179, N'Pipe Lining', N'Sewer Lining 36” Pipe', N'each', 2011, N'See Newport Memo', 264, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (180, N'Pipe Replacement', N'Replace Pipe 6”', N'ft', 2011, N'See Newport Memo', 315, NULL, NULL, NULL, N'PipeUpsize', N'0.1524', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (181, N'Pipe Replacement', N'Replace Pipe 10”', N'ft', 2011, N'See Newport Memo', 315, NULL, NULL, NULL, N'PipeUpsize', N'0.254', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (182, N'Pipe Replacement', N'Replace Pipe 12”', N'ft', 2011, N'See Newport Memo', 586, NULL, NULL, NULL, N'PipeUpsize', N'0.3048', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (183, N'Pipe Replacement', N'Replace Pipe 18”', N'ft', 2011, N'See Newport Memo', 586, NULL, NULL, NULL, N'PipeUpsize', N'0.4572', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (184, N'Pipe Replacement', N'Replace Pipe 24”', N'ft', 2011, N'See Newport Memo', 723, NULL, NULL, NULL, N'PipeUpsize', N'0.6096', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (185, N'Pipe Replacement', N'Replace Pipe 30”', N'ft', 2011, N'See Newport Memo', 723, NULL, NULL, NULL, N'PipeUpsize', N'0.762', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (186, N'Pipe Replacement', N'Replace Pipe 36”', N'ft', 2012, N'See Newport Memo', 723, NULL, NULL, NULL, N'PipeUpsize', N'0.9144', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (187, N'Private I/I Removal', N'Foundation Drain Disconnect', N'each', 2011, N'See Newport Memo', 1980, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (188, N'Private I/I Removal', N'Sump Pump Disconnect', N'each', 2011, N'See Newport Memo', 300, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (189, N'Public Inflow Reduction', N'Disconnect Catch Basin', N'each', 2011, N'See Newport Memo', 7824, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (190, N'Public Inflow Reduction', N'Disconnect Area Drain', N'each', 2011, N'See Newport Memo', 6600, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (191, N'Public Inflow Reduction', N'Point Repair Pipe Defect', N'each', 2011, N'See Newport Memo', 3960, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (192, N'Public Inflow Reduction', N'Point Repair Water Valve', N'each', 2011, N'See Newport Memo', 5280, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (193, N'Public Inflow Reduction', N'Repair Indirect Storm Connect', N'each', 2011, N'See Newport Memo', 6600, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (194, N'Public Inflow Reduction', N'Repair Drainage Crossing', N'each', 2011, N'See Newport Memo', 5280, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (195, N'Public Inflow Reduction', N'Repair Direct Storm Connection', N'each', 2011, N'See Newport Memo', 10560, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (196, N'Public Inflow Reduction', N'Replace Gate Seal', N'each', 2011, N'See Newport Memo', 6600, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (197, N'Public Inflow Reduction', N'Replace Gate', N'each', 2011, N'See Newport Memo', 71413, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (198, N'Public Inflow Reduction', N'Downspout Disconnect', N'each', 2011, N'See Newport Memo', 300, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (199, N'Public Inflow Reduction', N'Driveway Drain Disconnect', N'each', 2011, N'See Newport Memo', 10020, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (200, N'Public Inflow Reduction', N'Repair Defective Service Lateral', N'each', 2011, N'See Newport Memo', 1980, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (201, N'Public Inflow Reduction', N'Stairwell Drain Disconnect', N'each', 2011, N'See Newport Memo', 1980, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (202, N'Public Inflow Reduction', N'Replace Cleanout Cap', N'each', 2011, N'See Newport Memo', 300, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (203, N'Public Inflow Reduction', N'Replace Cleanout Cap', N'each', 2011, N'See Newport Memo', 3300, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (204, N'Public Inflow Reduction', N'Window Well Disconnect', N'each', 2011, N'See Newport Memo', 990, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (205, N'Pipe Replacement', N'Replace Pipe 40”', N'ft', 2011, N'See Newport Memo', 840, NULL, NULL, NULL, N'PipeUpsize', N'1.016', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (206, N'Pipe Replacement', N'Replace Pipe 42”', N'ft', 2011, N'See Newport Memo', 840, NULL, NULL, NULL, N'PipeUpsize', N'1.0668', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (207, N'Pipe Replacement', N'Replace Pipe 48”', N'ft', 2011, N'See Newport Memo', 840, NULL, NULL, NULL, N'PipeUpsize', N'1.2192', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (208, N'Pipe Replacement', N'Replace Pipe 34”', N'ft', 2011, N'See Newport Memo', 723, NULL, NULL, NULL, N'PipeUpsize', N'0.8636', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (209, N'Pipe Replacement', N'Replace Pipe 38”', N'ft', 2011, N'See Newport Memo', 723, NULL, NULL, NULL, N'PipeUpsize', N'0.9652', 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (210, N'Public Inflow Reduction', N'Generic I/I For Distrib (2* Downspout)', N'each', 2011, N'See Newport Memo', 600, NULL, NULL, NULL, NULL, NULL, 60, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (211, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 12"', N'ft', 2012, N'See Evansville Memo', 555, NULL, NULL, NULL, N'Pipe_UndST_10', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (212, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 15"', N'ft', 2012, N'See Evansville Memo', 559, NULL, NULL, NULL, N'Pipe_UndST_10', N'1.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (213, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 18"', N'ft', 2012, N'See Evansville Memo', 589, NULL, NULL, NULL, N'Pipe_UndST_10', N'1.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (214, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 24"', N'ft', 2012, N'See Evansville Memo', 624, NULL, NULL, NULL, N'Pipe_UndST_10', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (215, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 27"', N'ft', 2012, N'See Evansville Memo', 673, NULL, NULL, NULL, N'Pipe_UndST_10', N'2.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (216, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 30"', N'ft', 2012, N'See Evansville Memo', 703, NULL, NULL, NULL, N'Pipe_UndST_10', N'2.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (217, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 36"', N'ft', 2012, N'See Evansville Memo', 794, NULL, NULL, NULL, N'Pipe_UndST_10', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (218, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 42"', N'ft', 2012, N'See Evansville Memo', 884, NULL, NULL, NULL, N'Pipe_UndST_10', N'3.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (219, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 48"', N'ft', 2012, N'See Evansville Memo', 945, NULL, NULL, NULL, N'Pipe_UndST_10', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (220, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 54"', N'ft', 2012, N'See Evansville Memo', 1037, NULL, NULL, NULL, N'Pipe_UndST_10', N'4.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (221, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 60"', N'ft', 2012, N'See Evansville Memo', 1102, NULL, NULL, NULL, N'Pipe_UndST_10', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (222, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 66"', N'ft', 2012, N'See Evansville Memo', 1185, NULL, NULL, NULL, N'Pipe_UndST_10', N'5.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (223, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 72"', N'ft', 2012, N'See Evansville Memo', 1290, NULL, NULL, NULL, N'Pipe_UndST_10', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (224, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 78"', N'ft', 2012, N'See Evansville Memo', 1435, NULL, NULL, NULL, N'Pipe_UndST_10', N'6.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (225, N'Pipe Replacement Under Street. Max Depth : 10', N'Replace 84"', N'ft', 2012, N'See Evansville Memo', 1571, NULL, NULL, NULL, N'Pipe_UndST_10', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (226, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 12"', N'ft', 2012, N'See Evansville Memo', 824, NULL, NULL, NULL, N'Pipe_UndST_15', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (227, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 15"', N'ft', 2012, N'See Evansville Memo', 869, NULL, NULL, NULL, N'Pipe_UndST_15', N'1.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (228, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 18"', N'ft', 2012, N'See Evansville Memo', 901, NULL, NULL, NULL, N'Pipe_UndST_15', N'1.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (229, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 24"', N'ft', 2012, N'See Evansville Memo', 956, NULL, NULL, NULL, N'Pipe_UndST_15', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (230, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 27"', N'ft', 2012, N'See Evansville Memo', 1013, NULL, NULL, NULL, N'Pipe_UndST_15', N'2.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (231, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 30"', N'ft', 2012, N'See Evansville Memo', 1030, NULL, NULL, NULL, N'Pipe_UndST_15', N'2.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (232, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 36"', N'ft', 2012, N'See Evansville Memo', 1157, NULL, NULL, NULL, N'Pipe_UndST_15', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (233, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 42"', N'ft', 2012, N'See Evansville Memo', 1279, NULL, NULL, NULL, N'Pipe_UndST_15', N'3.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (234, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 48"', N'ft', 2012, N'See Evansville Memo', 1358, NULL, NULL, NULL, N'Pipe_UndST_15', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (235, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 54"', N'ft', 2012, N'See Evansville Memo', 1474, NULL, NULL, NULL, N'Pipe_UndST_15', N'4.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (236, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 60"', N'ft', 2012, N'See Evansville Memo', 1598, NULL, NULL, NULL, N'Pipe_UndST_15', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (237, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 66"', N'ft', 2012, N'See Evansville Memo', 1719, NULL, NULL, NULL, N'Pipe_UndST_15', N'5.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (238, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 72"', N'ft', 2012, N'See Evansville Memo', 1824, NULL, NULL, NULL, N'Pipe_UndST_15', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (239, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 78"', N'ft', 2012, N'See Evansville Memo', 2041, NULL, NULL, NULL, N'Pipe_UndST_15', N'6.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (240, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 84"', N'ft', 2012, N'See Evansville Memo', 2202, NULL, NULL, NULL, N'Pipe_UndST_15', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (241, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 90"', N'ft', 2012, N'See Evansville Memo', 2377, NULL, NULL, NULL, N'Pipe_UndST_15', N'7.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (242, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 96"', N'ft', 2012, N'See Evansville Memo', 2519, NULL, NULL, NULL, N'Pipe_UndST_15', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (243, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 102"', N'ft', 2012, N'See Evansville Memo', 2695, NULL, NULL, NULL, N'Pipe_UndST_15', N'8.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (244, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 108"', N'ft', 2012, N'See Evansville Memo', 2831, NULL, NULL, NULL, N'Pipe_UndST_15', N'9', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (245, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 120"', N'ft', 2012, N'See Evansville Memo', 3002, NULL, NULL, NULL, N'Pipe_UndST_15', N'10', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (246, N'Pipe Replacement Under Street. Max Depth : 15', N'Replace 132"', N'ft', 2012, N'See Evansville Memo', 3179, NULL, NULL, NULL, N'Pipe_UndST_15', N'11', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (247, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 12"', N'ft', 2012, N'See Evansville Memo', 1135, NULL, NULL, NULL, N'Pipe_UndST_20', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (248, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 15"', N'ft', 2012, N'See Evansville Memo', 1175, NULL, NULL, NULL, N'Pipe_UndST_20', N'1.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (249, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 18"', N'ft', 2012, N'See Evansville Memo', 1185, NULL, NULL, NULL, N'Pipe_UndST_20', N'1.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (250, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 24"', N'ft', 2012, N'See Evansville Memo', 1258, NULL, NULL, NULL, N'Pipe_UndST_20', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (251, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 27"', N'ft', 2012, N'See Evansville Memo', 1361, NULL, NULL, NULL, N'Pipe_UndST_20', N'2.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (252, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 30"', N'ft', 2012, N'See Evansville Memo', 1418, NULL, NULL, NULL, N'Pipe_UndST_20', N'2.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (253, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 36"', N'ft', 2012, N'See Evansville Memo', 1573, NULL, NULL, NULL, N'Pipe_UndST_20', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (254, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 42"', N'ft', 2012, N'See Evansville Memo', 1710, NULL, NULL, NULL, N'Pipe_UndST_20', N'3.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (255, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 48"', N'ft', 2012, N'See Evansville Memo', 1815, NULL, NULL, NULL, N'Pipe_UndST_20', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (256, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 54"', N'ft', 2012, N'See Evansville Memo', 1949, NULL, NULL, NULL, N'Pipe_UndST_20', N'4.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (257, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 60"', N'ft', 2012, N'See Evansville Memo', 2067, NULL, NULL, NULL, N'Pipe_UndST_20', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (258, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 66"', N'ft', 2012, N'See Evansville Memo', 2264, NULL, NULL, NULL, N'Pipe_UndST_20', N'5.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (259, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 72"', N'ft', 2012, N'See Evansville Memo', 2389, NULL, NULL, NULL, N'Pipe_UndST_20', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (260, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 78"', N'ft', 2012, N'See Evansville Memo', 2606, NULL, NULL, NULL, N'Pipe_UndST_20', N'6.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (261, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 84"', N'ft', 2012, N'See Evansville Memo', 2811, NULL, NULL, NULL, N'Pipe_UndST_20', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (262, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 90"', N'ft', 2012, N'See Evansville Memo', 3046, NULL, NULL, NULL, N'Pipe_UndST_20', N'7.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (263, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 96"', N'ft', 2012, N'See Evansville Memo', 3224, NULL, NULL, NULL, N'Pipe_UndST_20', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (264, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 102"', N'ft', 2012, N'See Evansville Memo', 3406, NULL, NULL, NULL, N'Pipe_UndST_20', N'8.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (265, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 108"', N'ft', 2012, N'See Evansville Memo', 3614, NULL, NULL, NULL, N'Pipe_UndST_20', N'9', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (266, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 120"', N'ft', 2012, N'See Evansville Memo', 3810, NULL, NULL, NULL, N'Pipe_UndST_20', N'10', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (267, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 132"', N'ft', 2012, N'See Evansville Memo', 4003, NULL, NULL, NULL, N'Pipe_UndST_20', N'11', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (268, N'Pipe Replacement Under Street. Max Depth : 20', N'Replace 144"', N'ft', 2012, N'See Evansville Memo', 4177, NULL, NULL, NULL, N'Pipe_UndST_20', N'12', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (269, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 12"', N'ft', 2012, N'See Evansville Memo', 1682, NULL, NULL, NULL, N'Pipe_UndST_30', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (270, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 15"', N'ft', 2012, N'See Evansville Memo', 1737, NULL, NULL, NULL, N'Pipe_UndST_30', N'1.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (271, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 18"', N'ft', 2012, N'See Evansville Memo', 1748, NULL, NULL, NULL, N'Pipe_UndST_30', N'1.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (272, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 24"', N'ft', 2012, N'See Evansville Memo', 1860, NULL, NULL, NULL, N'Pipe_UndST_30', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (273, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 27"', N'ft', 2012, N'See Evansville Memo', 1975, NULL, NULL, NULL, N'Pipe_UndST_30', N'2.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (274, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 30"', N'ft', 2012, N'See Evansville Memo', 2084, NULL, NULL, NULL, N'Pipe_UndST_30', N'2.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (275, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 36"', N'ft', 2012, N'See Evansville Memo', 2306, NULL, NULL, NULL, N'Pipe_UndST_30', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (276, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 42"', N'ft', 2012, N'See Evansville Memo', 2473, NULL, NULL, NULL, N'Pipe_UndST_30', N'3.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (277, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 48"', N'ft', 2012, N'See Evansville Memo', 2612, NULL, NULL, NULL, N'Pipe_UndST_30', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (278, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 54"', N'ft', 2012, N'See Evansville Memo', 2781, NULL, NULL, NULL, N'Pipe_UndST_30', N'4.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (279, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 60"', N'ft', 2012, N'See Evansville Memo', 2946, NULL, NULL, NULL, N'Pipe_UndST_30', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (280, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 66"', N'ft', 2012, N'See Evansville Memo', 3214, NULL, NULL, NULL, N'Pipe_UndST_30', N'5.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (281, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 72"', N'ft', 2012, N'See Evansville Memo', 3403, NULL, NULL, NULL, N'Pipe_UndST_30', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (282, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 78"', N'ft', 2012, N'See Evansville Memo', 3681, NULL, NULL, NULL, N'Pipe_UndST_30', N'6.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (283, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 84"', N'ft', 2012, N'See Evansville Memo', 3940, NULL, NULL, NULL, N'Pipe_UndST_30', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (284, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 90"', N'ft', 2012, N'See Evansville Memo', 4248, NULL, NULL, NULL, N'Pipe_UndST_30', N'7.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (285, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 96"', N'ft', 2012, N'See Evansville Memo', 4499, NULL, NULL, NULL, N'Pipe_UndST_30', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (286, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 102"', N'ft', 2012, N'See Evansville Memo', 4753, NULL, NULL, NULL, N'Pipe_UndST_30', N'8.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (287, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 108"', N'ft', 2012, N'See Evansville Memo', 5030, NULL, NULL, NULL, N'Pipe_UndST_30', N'9', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (288, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 120"', N'ft', 2012, N'See Evansville Memo', 5275, NULL, NULL, NULL, N'Pipe_UndST_30', N'10', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (289, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 132"', N'ft', 2012, N'See Evansville Memo', 5507, NULL, NULL, NULL, N'Pipe_UndST_30', N'11', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (290, N'Pipe Replacement Under Street. Max Depth : 30', N'Replace 144"', N'ft', 2012, N'See Evansville Memo', 5679, NULL, NULL, NULL, N'Pipe_UndST_30', N'12', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (291, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 12"', N'ft', 2012, N'See Evansville Memo', 311, NULL, NULL, NULL, N'Pipe_OutSt_10', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (292, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 15"', N'ft', 2012, N'See Evansville Memo', 317, NULL, NULL, NULL, N'Pipe_OutSt_10', N'1.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (293, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 18"', N'ft', 2012, N'See Evansville Memo', 341, NULL, NULL, NULL, N'Pipe_OutSt_10', N'1.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (294, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 24"', N'ft', 2012, N'See Evansville Memo', 373, NULL, NULL, NULL, N'Pipe_OutSt_10', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (295, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 27"', N'ft', 2012, N'See Evansville Memo', 417, NULL, NULL, NULL, N'Pipe_OutSt_10', N'2.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (296, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 30"', N'ft', 2012, N'See Evansville Memo', 443, NULL, NULL, NULL, N'Pipe_OutSt_10', N'2.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (297, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 36"', N'ft', 2012, N'See Evansville Memo', 527, NULL, NULL, NULL, N'Pipe_OutSt_10', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (298, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 42"', N'ft', 2012, N'See Evansville Memo', 623, NULL, NULL, NULL, N'Pipe_OutSt_10', N'3.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (299, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 48"', N'ft', 2012, N'See Evansville Memo', 685, NULL, NULL, NULL, N'Pipe_OutSt_10', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (300, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 54"', N'ft', 2012, N'See Evansville Memo', 782, NULL, NULL, NULL, N'Pipe_OutSt_10', N'4.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (301, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 60"', N'ft', 2012, N'See Evansville Memo', 851, NULL, NULL, NULL, N'Pipe_OutSt_10', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (302, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 66"', N'ft', 2012, N'See Evansville Memo', 937, NULL, NULL, NULL, N'Pipe_OutSt_10', N'5.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (303, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 72"', N'ft', 2012, N'See Evansville Memo', 1045, NULL, NULL, NULL, N'Pipe_OutSt_10', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (304, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 78"', N'ft', 2012, N'See Evansville Memo', 1202, NULL, NULL, NULL, N'Pipe_OutSt_10', N'6.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (305, N'Pipe Replacement Out of Street. Max Depth : 10', N'Replace 84"', N'ft', 2012, N'See Evansville Memo', 1350, NULL, NULL, NULL, N'Pipe_OutSt_10', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (306, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 12"', N'ft', 2012, N'See Evansville Memo', 458, NULL, NULL, NULL, N'Pipe_OutSt_15', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (307, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 15"', N'ft', 2012, N'See Evansville Memo', 487, NULL, NULL, NULL, N'Pipe_OutSt_15', N'1.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (308, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 18"', N'ft', 2012, N'See Evansville Memo', 502, NULL, NULL, NULL, N'Pipe_OutSt_15', N'1.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (309, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 24"', N'ft', 2012, N'See Evansville Memo', 541, NULL, NULL, NULL, N'Pipe_OutSt_15', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (310, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 27"', N'ft', 2012, N'See Evansville Memo', 583, NULL, NULL, NULL, N'Pipe_OutSt_15', N'2.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (311, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 30"', N'ft', 2012, N'See Evansville Memo', 602, NULL, NULL, NULL, N'Pipe_OutSt_15', N'2.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (312, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 36"', N'ft', 2012, N'See Evansville Memo', 698, NULL, NULL, NULL, N'Pipe_OutSt_15', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (313, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 42"', N'ft', 2012, N'See Evansville Memo', 809, NULL, NULL, NULL, N'Pipe_OutSt_15', N'3.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (314, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 48"', N'ft', 2012, N'See Evansville Memo', 874, NULL, NULL, NULL, N'Pipe_OutSt_15', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (315, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 54"', N'ft', 2012, N'See Evansville Memo', 978, NULL, NULL, NULL, N'Pipe_OutSt_15', N'4.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (316, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 60"', N'ft', 2012, N'See Evansville Memo', 1080, NULL, NULL, NULL, N'Pipe_OutSt_15', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (317, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 66"', N'ft', 2012, N'See Evansville Memo', 1188, NULL, NULL, NULL, N'Pipe_OutSt_15', N'5.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (318, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 72"', N'ft', 2012, N'See Evansville Memo', 1282, NULL, NULL, NULL, N'Pipe_OutSt_15', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (319, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 78"', N'ft', 2012, N'See Evansville Memo', 1477, NULL, NULL, NULL, N'Pipe_OutSt_15', N'6.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (320, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 84"', N'ft', 2012, N'See Evansville Memo', 1630, NULL, NULL, NULL, N'Pipe_OutSt_15', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (321, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 90"', N'ft', 2012, N'See Evansville Memo', 1787, NULL, NULL, NULL, N'Pipe_OutSt_15', N'7.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (322, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 96"', N'ft', 2012, N'See Evansville Memo', 1913, NULL, NULL, NULL, N'Pipe_OutSt_15', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (323, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 102"', N'ft', 2012, N'See Evansville Memo', 2064, NULL, NULL, NULL, N'Pipe_OutSt_15', N'8.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (324, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 108"', N'ft', 2012, N'See Evansville Memo', 2196, NULL, NULL, NULL, N'Pipe_OutSt_15', N'9', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (325, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 120"', N'ft', 2012, N'See Evansville Memo', 2367, NULL, NULL, NULL, N'Pipe_OutSt_15', N'10', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (326, N'Pipe Replacement Out of Street. Max Depth : 15', N'Replace 132"', N'ft', 2012, N'See Evansville Memo', 2539, NULL, NULL, NULL, N'Pipe_OutSt_15', N'11', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (327, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 12"', N'ft', 2012, N'See Evansville Memo', 670, NULL, NULL, NULL, N'Pipe_OutSt_20', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (328, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 15"', N'ft', 2012, N'See Evansville Memo', 685, NULL, NULL, NULL, N'Pipe_OutSt_20', N'1.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (329, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 18"', N'ft', 2012, N'See Evansville Memo', 698, NULL, NULL, NULL, N'Pipe_OutSt_20', N'1.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (330, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 24"', N'ft', 2012, N'See Evansville Memo', 750, NULL, NULL, NULL, N'Pipe_OutSt_20', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (331, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 27"', N'ft', 2012, N'See Evansville Memo', 812, NULL, NULL, NULL, N'Pipe_OutSt_20', N'2.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (332, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 30"', N'ft', 2012, N'See Evansville Memo', 862, NULL, NULL, NULL, N'Pipe_OutSt_20', N'2.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (333, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 36"', N'ft', 2012, N'See Evansville Memo', 972, NULL, NULL, NULL, N'Pipe_OutSt_20', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (334, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 42"', N'ft', 2012, N'See Evansville Memo', 1089, NULL, NULL, NULL, N'Pipe_OutSt_20', N'3.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (335, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 48"', N'ft', 2012, N'See Evansville Memo', 1173, NULL, NULL, NULL, N'Pipe_OutSt_20', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (336, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 54"', N'ft', 2012, N'See Evansville Memo', 1288, NULL, NULL, NULL, N'Pipe_OutSt_20', N'4.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (337, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 60"', N'ft', 2012, N'See Evansville Memo', 1388, NULL, NULL, NULL, N'Pipe_OutSt_20', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (338, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 66"', N'ft', 2012, N'See Evansville Memo', 1545, NULL, NULL, NULL, N'Pipe_OutSt_20', N'5.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (339, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 72"', N'ft', 2012, N'See Evansville Memo', 1653, NULL, NULL, NULL, N'Pipe_OutSt_20', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (340, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 78"', N'ft', 2012, N'See Evansville Memo', 1851, NULL, NULL, NULL, N'Pipe_OutSt_20', N'6.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (341, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 84"', N'ft', 2012, N'See Evansville Memo', 2023, NULL, NULL, NULL, N'Pipe_OutSt_20', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (342, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 90"', N'ft', 2012, N'See Evansville Memo', 2223, NULL, NULL, NULL, N'Pipe_OutSt_20', N'7.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (343, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 96"', N'ft', 2012, N'See Evansville Memo', 2371, NULL, NULL, NULL, N'Pipe_OutSt_20', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (344, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 102"', N'ft', 2012, N'See Evansville Memo', 2522, NULL, NULL, NULL, N'Pipe_OutSt_20', N'8.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (345, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 108"', N'ft', 2012, N'See Evansville Memo', 2704, NULL, NULL, NULL, N'Pipe_OutSt_20', N'9', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (346, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 120"', N'ft', 2012, N'See Evansville Memo', 2890, NULL, NULL, NULL, N'Pipe_OutSt_20', N'10', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (347, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 132"', N'ft', 2012, N'See Evansville Memo', 3075, NULL, NULL, NULL, N'Pipe_OutSt_20', N'11', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (348, N'Pipe Replacement Out of Street. Max Depth : 20', N'Replace 144"', N'ft', 2012, N'See Evansville Memo', 3245, NULL, NULL, NULL, N'Pipe_OutSt_20', N'12', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (349, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 12"', N'ft', 2012, N'See Evansville Memo', 1058, NULL, NULL, NULL, N'Pipe_OutSt_30', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (350, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 15"', N'ft', 2012, N'See Evansville Memo', 1072, NULL, NULL, NULL, N'Pipe_OutSt_30', N'1.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (351, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 18"', N'ft', 2012, N'See Evansville Memo', 1086, NULL, NULL, NULL, N'Pipe_OutSt_30', N'1.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (352, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 24"', N'ft', 2012, N'See Evansville Memo', 1160, NULL, NULL, NULL, N'Pipe_OutSt_30', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (353, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 27"', N'ft', 2012, N'See Evansville Memo', 1234, NULL, NULL, NULL, N'Pipe_OutSt_30', N'2.25', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (354, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 30"', N'ft', 2012, N'See Evansville Memo', 1305, NULL, NULL, NULL, N'Pipe_OutSt_30', N'2.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (355, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 36"', N'ft', 2012, N'See Evansville Memo', 1452, NULL, NULL, NULL, N'Pipe_OutSt_30', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (356, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 42"', N'ft', 2012, N'See Evansville Memo', 1580, NULL, NULL, NULL, N'Pipe_OutSt_30', N'3.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (357, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 48"', N'ft', 2012, N'See Evansville Memo', 1685, NULL, NULL, NULL, N'Pipe_OutSt_30', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (358, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 54"', N'ft', 2012, N'See Evansville Memo', 1819, NULL, NULL, NULL, N'Pipe_OutSt_30', N'4.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (359, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 60"', N'ft', 2012, N'See Evansville Memo', 1949, NULL, NULL, NULL, N'Pipe_OutSt_30', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (360, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 66"', N'ft', 2012, N'See Evansville Memo', 2147, NULL, NULL, NULL, N'Pipe_OutSt_30', N'5.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (361, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 72"', N'ft', 2012, N'See Evansville Memo', 2302, NULL, NULL, NULL, N'Pipe_OutSt_30', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (362, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 78"', N'ft', 2012, N'See Evansville Memo', 2547, NULL, NULL, NULL, N'Pipe_OutSt_30', N'6.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (363, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 84"', N'ft', 2012, N'See Evansville Memo', 2739, NULL, NULL, NULL, N'Pipe_OutSt_30', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (364, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 90"', N'ft', 2012, N'See Evansville Memo', 2981, NULL, NULL, NULL, N'Pipe_OutSt_30', N'7.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (365, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 96"', N'ft', 2012, N'See Evansville Memo', 3170, NULL, NULL, NULL, N'Pipe_OutSt_30', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (366, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 102"', N'ft', 2012, N'See Evansville Memo', 3363, NULL, NULL, NULL, N'Pipe_OutSt_30', N'8.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (367, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 108"', N'ft', 2012, N'See Evansville Memo', 3581, NULL, NULL, NULL, N'Pipe_OutSt_30', N'9', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (368, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 120"', N'ft', 2012, N'See Evansville Memo', 3802, NULL, NULL, NULL, N'Pipe_OutSt_30', N'10', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (369, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 132"', N'ft', 2012, N'See Evansville Memo', 4006, NULL, NULL, NULL, N'Pipe_OutSt_30', N'11', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (370, N'Pipe Replacement Out of Street. Max Depth : 30', N'Replace 144"', N'ft', 2012, N'See Evansville Memo', 4177, NULL, NULL, NULL, N'Pipe_OutSt_30', N'12', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (371, N'Forcemain', N'Replace 6"', N'ft', 2012, N'See Evansville Memo', 527, NULL, NULL, NULL, N'Forcemain', N'0.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (372, N'Forcemain', N'Replace 8"', N'ft', 2012, N'See Evansville Memo', 556, NULL, NULL, NULL, N'Forcemain', N'0.6667', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (373, N'Forcemain', N'Replace 12"', N'ft', 2012, N'See Evansville Memo', 580, NULL, NULL, NULL, N'Forcemain', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (374, N'Forcemain', N'Replace 16"', N'ft', 2012, N'See Evansville Memo', 602, NULL, NULL, NULL, N'Forcemain', N'1.3333', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (375, N'Forcemain', N'Replace 20"', N'ft', 2012, N'See Evansville Memo', 657, NULL, NULL, NULL, N'Forcemain', N'1.6667', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (376, N'Forcemain', N'Replace 24"', N'ft', 2012, N'See Evansville Memo', 703, NULL, NULL, NULL, N'Forcemain', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (377, N'Forcemain', N'Replace 30"', N'ft', 2012, N'See Evansville Memo', 833, NULL, NULL, NULL, N'Forcemain', N'2.5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (378, N'Increased Pumping', N'Add 0 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 0, NULL, NULL, NULL, N'AddtlPumping', N'zoe0MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (379, N'Increased Pumping', N'Add 5 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 3419294, NULL, NULL, NULL, N'AddtlPumping', N'zoe5MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (380, N'Increased Pumping', N'Add 10 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 5478855, NULL, NULL, NULL, N'AddtlPumping', N'zoe10MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (381, N'Increased Pumping', N'Add 15 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 7491949, NULL, NULL, NULL, N'AddtlPumping', N'zoe15MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (382, N'Increased Pumping', N'Add 20 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 9460342, NULL, NULL, NULL, N'AddtlPumping', N'zoe20MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (383, N'Increased Pumping', N'Add 25 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 11385800, NULL, NULL, NULL, N'AddtlPumping', N'zoe25MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (384, N'Increased Pumping', N'Add 30 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 13270090, NULL, NULL, NULL, N'AddtlPumping', N'zoe30MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (385, N'Increased Pumping', N'Add 35 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 15114979, NULL, NULL, NULL, N'AddtlPumping', N'zoe35MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (386, N'Increased Pumping', N'Add 40 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 16922233, NULL, NULL, NULL, N'AddtlPumping', N'zoe40MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (387, N'Increased Pumping', N'Add 45 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 18693618, NULL, NULL, NULL, N'AddtlPumping', N'zoe45MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (388, N'Increased Pumping', N'Add 50 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 20430900, NULL, NULL, NULL, N'AddtlPumping', N'zoe50MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (389, N'Increased Pumping', N'Add 55 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 22135846, NULL, NULL, NULL, N'AddtlPumping', N'zoe55MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (390, N'Increased Pumping', N'Add 60 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 23810223, NULL, NULL, NULL, N'AddtlPumping', N'zoe60MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (391, N'Increased Pumping', N'Add 65 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 25455797, NULL, NULL, NULL, N'AddtlPumping', N'zoe65MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (392, N'Increased Pumping', N'Add 70 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 27074334, NULL, NULL, NULL, N'AddtlPumping', N'zoe70MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (393, N'Increased Pumping', N'Add 75 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 28667600, NULL, NULL, NULL, N'AddtlPumping', N'zoe75MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (394, N'Increased Pumping', N'Add 80 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 30237362, NULL, NULL, NULL, N'AddtlPumping', N'zoe80MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (395, N'Increased Pumping', N'Add 85 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 31785387, NULL, NULL, NULL, N'AddtlPumping', N'zoe85MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (396, N'Increased Pumping', N'Add 90 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 33313441, NULL, NULL, NULL, N'AddtlPumping', N'zoe90MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (397, N'Increased Pumping', N'Add 95 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 34823290, NULL, NULL, NULL, N'AddtlPumping', N'zoe95MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (398, N'Increased Pumping', N'Add 100 MGD Capacity', N'MGD', 2012, N'See Evansville Memo', 36316700, NULL, NULL, NULL, N'AddtlPumping', N'zoe100MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (399, N'Satellite Treatment', N'0 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 0, NULL, NULL, NULL, N'HRT', N'zoe0MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (400, N'Satellite Treatment', N'5 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 19664000, NULL, NULL, NULL, N'HRT', N'zoe5MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (401, N'Satellite Treatment', N'10 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 27748000, NULL, NULL, NULL, N'HRT', N'zoe10MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (402, N'Satellite Treatment', N'15 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 35832000, NULL, NULL, NULL, N'HRT', N'zoe15MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (403, N'Satellite Treatment', N'20 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 43916000, NULL, NULL, NULL, N'HRT', N'zoe20MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (404, N'Satellite Treatment', N'25 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 52000000, NULL, NULL, NULL, N'HRT', N'zoe25MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (405, N'Satellite Treatment', N'30 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 60084000, NULL, NULL, NULL, N'HRT', N'zoe30MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (406, N'Satellite Treatment', N'35 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 68168000, NULL, NULL, NULL, N'HRT', N'zoe35MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (407, N'Satellite Treatment', N'40 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 76252000, NULL, NULL, NULL, N'HRT', N'zoe40MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (408, N'Satellite Treatment', N'45 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 84336000, NULL, NULL, NULL, N'HRT', N'zoe45MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (409, N'Satellite Treatment', N'50 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 92420000, NULL, NULL, NULL, N'HRT', N'zoe50MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (410, N'Satellite Treatment', N'55 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 100504000, NULL, NULL, NULL, N'HRT', N'zoe55MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (411, N'Satellite Treatment', N'60 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 108588000, NULL, NULL, NULL, N'HRT', N'zoe60MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (412, N'Satellite Treatment', N'65 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 116672000, NULL, NULL, NULL, N'HRT', N'zoe65MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (413, N'Satellite Treatment', N'70 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 124756000, NULL, NULL, NULL, N'HRT', N'zoe70MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (414, N'Satellite Treatment', N'75 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 132840000, NULL, NULL, NULL, N'HRT', N'zoe75MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (415, N'Satellite Treatment', N'80 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 140924000, NULL, NULL, NULL, N'HRT', N'zoe80MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (416, N'Satellite Treatment', N'85 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 149008000, NULL, NULL, NULL, N'HRT', N'zoe85MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (417, N'Satellite Treatment', N'90 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 157092000, NULL, NULL, NULL, N'HRT', N'zoe90MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (418, N'Satellite Treatment', N'95 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 165176000, NULL, NULL, NULL, N'HRT', N'zoe95MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (419, N'Satellite Treatment', N'100 MGD HRT Capacity', N'MGD', 2012, N'See Evansville Memo', 173260000, NULL, NULL, NULL, N'HRT', N'zoe100MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (420, N'Increase Treatment Plant Capacity', N'Increase WRP Treatment to 40 MGD', N'MGD', 2012, N'V Varghese Email 4/2- needs confirmation', 27845725, NULL, NULL, NULL, N'WRP_Increase', N'zoeWWTP_45MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (421, N'Increase Treatment Plant Capacity', N'Increase WRP Treatment to 60 MGD', N'MGD', 2012, N'Kisch email 4/17  needs confirmation', 63080820, NULL, NULL, NULL, N'WRP_Increase', N'zoeWWTP_60MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (422, N'Increase Treatment Plant Capacity', N'Increase WRP Treatment to 80 MGD', N'MGD', 2012, N'Kisch email 4/17  - needs confirmation', 142246373, NULL, NULL, NULL, N'WRP_Increase', N'zoeWWTP_80MGD', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (423, N'Addtl conveyance- Ohio', N'Additional conveyance to the storage location', N'unit', 2012, N'MET- manual calc- see performance def', 1701500, NULL, NULL, NULL, N'OhioTo7th', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (424, N'Addtl conveyance- Ohio', N'Additional conveyance to the storage location', N'unit', 2012, N'MET- manual calc- see performance def', 1701500, NULL, NULL, NULL, N'OhioTo7th', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (425, N'Addtl conveyance- Ohio', N'Additional conveyance to the storage location', N'unit', 2012, N'MET- manual calc- see performance def', 1701500, NULL, NULL, NULL, N'OhioTo7th', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (426, N'Addtl conveyance- Ohio', N'Additional conveyance to the storage location', N'unit', 2012, N'MET- manual calc- see performance def', 1701500, NULL, NULL, NULL, N'OhioTo7th', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (427, N'Addtl conveyance- Ohio', N'Additional conveyance to the storage location', N'unit', 2012, N'MET- manual calc- see performance def', 1701500, NULL, NULL, NULL, N'OhioTo7th', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (428, N'Addtl conveyance- Ohio', N'Additional conveyance to the storage location', N'unit', 2012, N'MET- manual calc- see performance def', 1701500, NULL, NULL, NULL, N'OhioTo7th', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (429, N'Addtl conveyance- Ohio', N'Additional conveyance to the storage location', N'unit', 2012, N'MET- manual calc- see performance def', 1701500, NULL, NULL, NULL, N'OhioTo7th', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (430, N'Addtl conveyance- Ohio', N'Additional conveyance to the storage location', N'unit', 2012, N'MET- manual calc- see performance def', 1701500, NULL, NULL, NULL, N'OhioTo7th', N'9', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (431, N'Storage', N'Do nothing', N'total', 2012, N'MET: EV_SimLink_Helper', 0, NULL, NULL, NULL, N'7thStorage', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (432, N'Storage', N'2 mgal Vertical Starge at 7th HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 12395928, NULL, NULL, NULL, N'7thStorage', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (433, N'Storage', N'4 mgal Vertical Starge at 7th HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 17745812, NULL, NULL, NULL, N'7thStorage', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (434, N'Storage', N'6 mgal Vertical Starge at 7th HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 23283352, NULL, NULL, NULL, N'7thStorage', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (435, N'Storage', N'8 mgal Vertical Starge at 7th HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 29008548, NULL, NULL, NULL, N'7thStorage', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (436, N'Storage', N'10 mgal Vertical Starge at 7th HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 34921400, NULL, NULL, NULL, N'7thStorage', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (437, N'Storage', N'12 mgal Vertical Starge at 7th HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 41021908, NULL, NULL, NULL, N'7thStorage', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (438, N'Storage', N'14 mgal Vertical Starge at 7th HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 47310072, NULL, NULL, NULL, N'7thStorage', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (439, N'Storage', N'Diamond HRT (Base Case)', N'total', 2012, N'MET: EV_SimLink_Helper', 0, NULL, NULL, NULL, N'DiamondStorage', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (440, N'Storage', N'5 mgal Vertical Storage at Diamond HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 20491125, NULL, NULL, NULL, N'DiamondStorage', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (441, N'Storage', N'10 mgal Vertical Storage at Diamond HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 34921400, NULL, NULL, NULL, N'DiamondStorage', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (442, N'Storage', N'15 mgal Vertical Storage at Diamond HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 50524525, NULL, NULL, NULL, N'DiamondStorage', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (443, N'Storage', N'20 mgal Vertical Storage at Diamond HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 67300500, NULL, NULL, NULL, N'DiamondStorage', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (444, N'Storage', N'25 mgal Vertical Storage at Diamond HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 85249325, NULL, NULL, NULL, N'DiamondStorage', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (445, N'Storage', N'30 mgal Vertical Storage at Diamond HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 104371000, NULL, NULL, NULL, N'DiamondStorage', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (446, N'Storage', N'35 mgal Vertical Storage at Diamond HRT', N'total', 2012, N'MET: EV_SimLink_Helper', 124665525, NULL, NULL, NULL, N'DiamondStorage', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (447, N'Adjustable Gate', N'120" Adjustable Gate', N'each', 2012, N'Evansville', 705480, NULL, NULL, NULL, N'DiamondGATE', N'270', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (448, N'Storm Sewer Separation', N'2007 SMP:  144.9 acres', N'each', 2012, N'EV: see Kisch email', 16249459, NULL, NULL, NULL, N'SEP_CSO025', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (449, N'Storm Sewer Separation', N'2007 SMP:  2.3 acres', N'each', 2012, N'EV: see Kisch email', 765374, NULL, NULL, NULL, N'SEP_CSO017', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (450, N'Storm Sewer Separation', N'2007 SMP:  41.8 acres', N'each', 2012, N'EV: see Kisch email', 3584026, NULL, NULL, NULL, N'SEP_CSO012', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (451, N'Storm Sewer Separation', N'2007 SMP:  55.4 acres', N'each', 2012, N'EV: see Kisch email', 7042658, NULL, NULL, NULL, N'SEP_CSO013_016', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (452, N'Storage', N'not used', N'total', 2012, N'MET: EV_SimLink_Helper', 0, NULL, NULL, NULL, N'WRP_Storage', N'1', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (453, N'Storage', N'4 mgal vertical storage (80 foot depth)', N'total', 2012, N'MET: EV_SimLink_Helper', 17745812, NULL, NULL, NULL, N'WRP_Storage', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (454, N'Storage', N'8 mgal vertical storage (80 foot depth)', N'total', 2012, N'MET: EV_SimLink_Helper', 29008548, NULL, NULL, NULL, N'WRP_Storage', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (455, N'Storage', N'12 mgal vertical storage (80 foot depth)', N'total', 2012, N'MET: EV_SimLink_Helper', 41021908, NULL, NULL, NULL, N'WRP_Storage', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (456, N'Storage', N'16 mgal vertical storage (80 foot depth)', N'total', 2012, N'MET: EV_SimLink_Helper', 53785892, NULL, NULL, NULL, N'WRP_Storage', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (457, N'Storage', N'20 mgal vertical storage (80 foot depth)', N'total', 2012, N'MET: EV_SimLink_Helper', 67300500, NULL, NULL, NULL, N'WRP_Storage', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (458, N'Storage', N'24 mgal vertical storage (80 foot depth)', N'total', 2012, N'MET: EV_SimLink_Helper', 81565732, NULL, NULL, NULL, N'WRP_Storage', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (459, N'Storage', N'28 mgal vertical storage (80 foot depth)', N'total', 2012, N'MET: EV_SimLink_Helper', 96581588, NULL, NULL, NULL, N'WRP_Storage', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (465, N'Conveyance', N'you should never see this', N'total', 2012, N'EV Cost Documentation- Processed in EV_CostHelper', 0, NULL, NULL, NULL, N'FM_to_WRP', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (466, N'Conveyance', N'2 20" Parallel FM (20 MGD total) from 7th St LS to WRP', N'total', 2012, N'EV Cost Documentation- Processed in EV_CostHelper', 19613990, NULL, NULL, NULL, N'FM_to_WRP', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (467, N'Conveyance', N'3 20" Parallel FM (20 MGD total) from 7th St LS to WRP', N'total', 2012, N'EV Cost Documentation- Processed in EV_CostHelper', 29420984, NULL, NULL, NULL, N'FM_to_WRP', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (468, N'Conveyance', N'4 20" Parallel FM (20 MGD total) from 7th St LS to WRP', N'total', 2012, N'EV Cost Documentation- Processed in EV_CostHelper', 39227979, NULL, NULL, NULL, N'FM_to_WRP', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (469, N'Green Infrastructure', N'Manage 10% of Area in "CSO012 basin"', N'total', 2012, N'Prelim cost estimate by MET', 9056150, NULL, NULL, NULL, N'GI_CSO012', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (470, N'Green Infrastructure', N'Manage 25% of Area in "CSO012 basin"', N'total', 2012, N'Prelim cost estimate by MET', 22754340, NULL, NULL, NULL, N'GI_CSO012', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (471, N'Green Infrastructure', N'Manage 10% of Area in "CSO020022 basin"', N'total', 2012, N'Prelim cost estimate by MET', 16187868, NULL, NULL, NULL, N'GI_CSO020_022', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (472, N'Green Infrastructure', N'Manage 25% of Area in"CSO020022 basin"', N'total', 2012, N'Prelim cost estimate by MET', 40673383, NULL, NULL, NULL, N'GI_CSO020_022', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (473, N'Storage', N'3 mgal Underground Storage', N'total', 2012, N'EV Cost basis for underground storage', 12570700, NULL, NULL, NULL, N'School_Storage', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (474, N'Storage', N'6 mgal Underground Storage', N'total', 2012, N'EV Cost basis for underground storage', 21377500, NULL, NULL, NULL, N'School_Storage', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (475, N'Storage', N'9 mgal Underground Storage', N'total', 2012, N'EV Cost basis for underground storage', 30184300, NULL, NULL, NULL, N'School_Storage', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (476, N'Storage', N'12 mgal Underground Storage', N'total', 2012, N'EV Cost basis for underground storage', 38991100, NULL, NULL, NULL, N'School_Storage', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (477, N'Storage', N'15 mgal Underground Storage', N'total', 2012, N'EV Cost basis for underground storage', 47797900, NULL, NULL, NULL, N'School_Storage', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (478, N'Storage', N'17 mgal Underground Storage', N'total', 2012, N'EV Cost basis for underground storage', 53669100, NULL, NULL, NULL, N'School_Storage', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (479, N'Storage', N'1 mgal  Vertical Storage', N'total', 2012, N'EV Cost basis for underground storage', 9791357, NULL, NULL, NULL, N'CSO_012_013', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (480, N'Storage', N'2 mgal  Vertical Storage', N'total', 2012, N'EV Cost basis for underground storage', 12395928, NULL, NULL, NULL, N'CSO_012_013', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (481, N'Storage', N'3 mgal  Vertical Storage', N'total', 2012, N'EV Cost basis for underground storage', 15047413, NULL, NULL, NULL, N'CSO_012_013', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (482, N'Storage', N'4 mgal  Vertical Storage', N'total', 2012, N'EV Cost basis for underground storage', 17745812, NULL, NULL, NULL, N'CSO_012_013', N'5', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (483, N'Storage', N'5 mgal  Vertical Storage', N'total', 2012, N'EV Cost basis for underground storage', 20491125, NULL, NULL, NULL, N'CSO_012_013', N'6', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (484, N'Storage', N'6 mgal  Vertical Storage', N'total', 2012, N'EV Cost basis for underground storage', 23283352, NULL, NULL, NULL, N'CSO_012_013', N'7', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (485, N'Storage', N'7 mgal  Vertical Storage', N'total', 2012, N'EV Cost basis for underground storage', 26122493, NULL, NULL, NULL, N'CSO_012_013', N'8', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (486, N'Conveyance ', N'5'' Sewer Under Pigeon Creek', N'total', 2012, N'EV Cost basis- calculated in spreadhseet to include crossing', 2176733, NULL, NULL, NULL, N'CSO_012_UnderPC', N'2', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (487, N'Conveyance ', N'8'' Sewer Under Pigeon Creek', N'total', 2012, N'EV Cost basis- calculated in spreadhseet to include crossing', 3177802, NULL, NULL, NULL, N'CSO_012_UnderPC', N'3', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (488, N'Conveyance ', N'12'' Sewer Under Pigeon Creek', N'total', 2012, N'EV Cost basis- calculated in spreadhseet to include crossing', 3938517, NULL, NULL, NULL, N'CSO_012_UnderPC', N'4', 64, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (489, N'Underground Storage', N'1 ac-ft', N'each', 2012, N'Assumed', 87120, NULL, NULL, NULL, N'WQ_Storage', N'2', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (490, N'Underground Storage', N'2 ac-ft', N'each', 2012, N'Assumed', 174240, NULL, NULL, NULL, N'WQ_Storage', N'3', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (491, N'Underground Storage', N'3 ac-ft', N'each', 2012, N'Assumed', 261360, NULL, NULL, NULL, N'WQ_Storage', N'4', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (492, N'Underground Storage', N'4 ac-ft', N'each', 2012, N'Assumed', 348480, NULL, NULL, NULL, N'WQ_Storage', N'5', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (493, N'Underground Storage', N'5 ac-ft', N'each', 2012, N'Assumed', 435600, NULL, NULL, NULL, N'WQ_Storage', N'6', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (494, N'Underground Storage', N'6 ac-ft', N'each', 2012, N'Assumed', 522720, NULL, NULL, NULL, N'WQ_Storage', N'7', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (495, N'Underground Storage', N'9 ac-ft', N'each', 2012, N'Assumed', 784080, NULL, NULL, NULL, N'WQ_Storage', N'8', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (496, N'Green Infrastructure', N'0.475 ac-ft', N'each', 2012, N'Assumed', 154769, NULL, NULL, NULL, N'WQ_GI', N'2', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (497, N'Green Infrastructure', N'0.95 ac-ft', N'each', 2012, N'Assumed', 309537, NULL, NULL, NULL, N'WQ_GI', N'3', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (498, N'Green Infrastructure', N'1.9 ac-ft', N'each', 2012, N'Assumed', 619075, NULL, NULL, NULL, N'WQ_GI', N'4', 66, 0)
GO
INSERT [dbo].[tlkpCostData] ([CostItemID], [CostCategory], [CostItem], [Unit], [costYear], [Source], [CapitalCost], [MaintCostPerc], [DesignLife], [AdditionalNote], [Qual1], [LinkVal1], [ProjID_Fk], [InvalidVal]) VALUES (499, N'Green Infrastructure', N'3.8 ac-ft', N'each', 2012, N'Assumed', 1238149, NULL, NULL, NULL, N'WQ_GI', N'5', 66, 0)
GO

SET IDENTITY_INSERT [tlkpCostData] OFF
GO


SET IDENTITY_INSERT [tlkpDistribStrategy] ON
GO

INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (1, N'Comprehensive', N'Low GI mplementation on all potential targets areas', NULL, -1, 1)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (2, N'Comprehensive', N'Medium GI mplementation on all potential targets areas', NULL, -1, 2)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (3, N'Comprehensive', N'High GI mplementation on all potential targets areas', NULL, -1, 3)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (4, N'ROW Only', N'Low GI mplementation on all ROW targets areas', NULL, -1, 1)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (5, N'ROW Only', N'Med GI mplementation on all ROW targets areas', NULL, -1, 2)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (6, N'ROW Only', N'High GI mplementation on all ROW targets areas', NULL, -1, 3)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (7, N'Public Only', N'Low GI mplementation on all public targets areas', NULL, -1, 1)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (8, N'Public Only', N'Med GI mplementation on all public targets areas', NULL, -1, 2)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (9, N'Public Only', N'High GI mplementation on all public targets areas', NULL, -1, 3)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (10, N'Private Only', N'Low GI mplementation on all public targets areas', NULL, -1, 1)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (11, N'Public Only', N'Med GI mplementation on all public targets areas', NULL, -1, 2)
GO
INSERT [dbo].[tlkpDistribStrategy] ([DistribStrategyID], [DistribLabel], [DistribDescrip], [DistribQual_FK], [ProjID_FK], [DistribIntensity]) VALUES (12, N'Public Only', N'High GI mplementation on all public targets areas', NULL, -1, 3)
GO

SET IDENTITY_INSERT [tlkpDistribStrategy] OFF
GO


SET IDENTITY_INSERT [tlkpDistributionDefinition] ON
GO

INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (404, N'ROW Impervious (Non Freeway)', 1, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (405, N'Freeways', 1, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (406, N'Parking Lots', 1, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (407, N'Large Flat Roofs', 1, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (408, N'Other Roofs', 1, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (409, N'Park Impervious Areas', 1, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (410, N'Turf Grass Areas', 1, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (411, N'Parking Lots', 2, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (412, N'Large Flat Roofs', 2, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (413, N'Other Commercial Impervious', 2, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (414, N'Other Industrial Roofs', 2, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (415, N'Residential Impervious', 2, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (416, N'Residential Yards', 2, NULL, -1, NULL, 65, NULL, NULL)
GO
INSERT [dbo].[tlkpDistributionDefinition] ([DistribID], [Label], [PubPriv], [Descrip], [CostItem_FK], [DistribTotal_FK], [ProjID_FK], [Qualifier], [val]) VALUES (417, N'Other Turf Grass Areas', 2, NULL, -1, NULL, 65, NULL, NULL)
GO

SET IDENTITY_INSERT [tlkpDistributionDefinition] OFF
GO


SET IDENTITY_INSERT [tlkpUI_Dictionary] ON
GO

INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (1, N'ModelType', N'Hydrology & Hydraulics', N'EPA SWMM v5.0', 1, 1)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (2, N'ModelType', N'Hydrology & Hydraulics', N'Infoworks CS', 2, 2)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (3, N'FieldDictionaryTable', NULL, N'tlkpSWMMFieldDictionary', NULL, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (4, N'FieldDictionaryTable', NULL, N'tlkpInfoworksFieldDictionary', NULL, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (5, N'ModelType', N'Water Distribution', N'EPA Net', 3, 3)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (6, N'UnitSettings', N'2', N'Metric', NULL, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (7, N'UnitSettings', N'2', N'MGD', NULL, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (8, N'UnitSettings', N'2', N'CFS', NULL, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (10, N'SWMM_Out', NULL, N'Subcatchment', 1, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (11, N'SWMM_Out', NULL, N'Node', 2, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (12, N'SWMM_Out', NULL, N'Link', 3, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (13, N'SWMM_Out', NULL, N'Sys', 4, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (14, N'ModelType', N'Groundwater', N'ModFlow', 4, 4)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (15, N'ModelType', N'Hydrology & Hydraulics', N'Mike Urban', 5, 5)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (16, N'ModelType', N'Hydrology & Hydraulics', N'XP SWMM', 6, 6)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (18, N'ElementLibrary', NULL, N'Green Infrastructure', 1, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (19, N'DistribStrategyCat', NULL, N'Green Infrastructure (Percent Defined)', 1, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (23, N'DistribStrategyCat', NULL, N'Green Infrastructure (Geospatial)', NULL, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (24, N'SystemType', NULL, N'Combined', NULL, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (25, N'SystemType', NULL, N'Separate', NULL, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (26, N'DistribStrategyCat', NULL, N'WQ_Definition', NULL, NULL)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (27, N'ModelType', N'Hydrology & Hydraulics', N'ISIS 1D', 7, 7)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (28, N'ModelType', N'Climate', N'SimClim', 8, 8)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (29, N'ModelType', N'Hydrology & Hydraulics', N'ISIS 2D', 9, 9)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (30, N'LinkedDataType', N'NotSet', NULL, 1, -1)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (31, N'LinkedDataType', N'ModelElements', NULL, 2, 1)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (32, N'LinkedDataType', N'ResultSummary', NULL, 3, 2)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (33, N'LinkedDataType', N'ResultTS', NULL, 4, 3)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (34, N'LinkedDataType', N'DVOptions', NULL, 5, 4)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (35, N'LinkedDataType', N'Event', NULL, 6, 5)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (36, N'LinkedDataType', N'Performance', NULL, 7, 6)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (37, N'Perf_FunctionOnLinkedData', N'Sum', NULL, 1, 1)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (38, N'Perf_FunctionOnLinkedData', N'Min', NULL, 2, 2)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (39, N'Perf_FunctionOnLinkedData', N'Max', NULL, 3, 3)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (40, N'Perf_FunctionOnLinkedData', N'Count', NULL, 4, 4)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (41, N'Event_FunctionOnTimeSeries', N'ThresholdAndDurationCalculations', NULL, 1, -1)
GO
INSERT [dbo].[tlkpUI_Dictionary] ([DictID], [Category], [Subcategory], [val], [sequence], [ValInt]) VALUES (46, N'Perf_FunctionOnLinkedData', N'NotSet', NULL, 0, -1)
GO

SET IDENTITY_INSERT [tlkpUI_Dictionary] OFF
GO


SET IDENTITY_INSERT [tlkpUnitConversions] ON
GO


INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (1, 1, 10, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (2, 1, 2, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (3, 1, 3, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (4, 1, 4, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (5, 1, 5, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (6, 1, 6, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (7, 2, 10, 3.28083)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (8, 2, 2, 10.76385)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (9, 2, 3, 35.31467)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (10, 2, 4, 3.28083)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (11, 2, 5, 3.28083)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (12, 2, 6, 35.31467)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (13, 1, 7, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (14, 2, 7, 0.039370079)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (15, 1, 8, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (16, 2, 8, 2.471044)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (17, 3, 10, 3.28083)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (18, 3, 2, 10.76385)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (19, 3, 3, 22.82447)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (20, 3, 4, 3.28083)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (21, 3, 5, 3.28083)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (22, 3, 6, 0.0002641721)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (23, 3, 7, 0.039370079)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (24, 3, 8, 2.471044)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (25, 1, 1, -1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (26, 2, 1, -1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (27, 3, 1, -1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (28, 1, 9, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (29, 2, 9, 1)
GO
INSERT [dbo].[tlkpUnitConversions] ([UnitID], [UnitSettingID_FK], [UnitTypeID_FK], [UnitConversion]) VALUES (30, 3, 9, 1)
GO

SET IDENTITY_INSERT [tlkpUnitConversions] OFF
GO

SET IDENTITY_INSERT [tlkpUnitSettings] ON
GO

INSERT [dbo].[tlkpUnitSettings] ([UnitSettingID], [ModelTypeID_FK], [UnitSetting]) VALUES (1, 2, N'Metric')
GO
INSERT [dbo].[tlkpUnitSettings] ([UnitSettingID], [ModelTypeID_FK], [UnitSetting]) VALUES (2, 2, N'CFS')
GO
INSERT [dbo].[tlkpUnitSettings] ([UnitSettingID], [ModelTypeID_FK], [UnitSetting]) VALUES (3, 2, N'MGD')
GO

SET IDENTITY_INSERT [tlkpUnitSettings] off
GO


SET IDENTITY_INSERT [tlkpUnitTypes] ON
GO

INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (1, N'No Unit Conversion (or Not set)')
GO
INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (2, N'Area')
GO
INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (3, N'Flow')
GO
INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (4, N'Height AD')
GO
INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (5, N'Velocity')
GO
INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (6, N'Volume')
GO
INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (7, N'Width')
GO
INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (8, N'Area (Big)')
GO
INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (9, N'Unity')
GO
INSERT [dbo].[tlkpUnitTypes] ([UnitTypeID], [UnitType]) VALUES (10, N'Distance')
GO


SET IDENTITY_INSERT [tlkpUnitTypes] OFF
GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.15', 'Creating schema and data for Cost, Distribution Strategy, Distribution Definition, UI Dictionary, Unit Conversions, Unit Settings and Unit Type lookup tables'); 