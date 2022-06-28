/****** Object:  Table [dbo].[tlkpModFlow_FieldDictionary]    Script Date: 4/4/2016 8:29:42 AM ******/
DROP TABLE [dbo].[tlkpModFlow_FieldDictionary]
GO
/****** Object:  Table [dbo].[tlkpModFlow_FieldDictionary]    Script Date: 4/4/2016 8:29:42 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpModFlow_FieldDictionary](
	[ResultsFieldID] [int] Identity(1,1) NOT NULL,
	[FeatureType] [nvarchar](255) NULL,
	[FieldName] [nvarchar](255) NULL,
	[FieldAlias] [nvarchar](255) NULL,
	[Qualifier] [nvarchar](10) NULL,
	[UnitType] [int] NULL,
 CONSTRAINT [PK_tlkpModFlow_FieldDictionary] PRIMARY KEY CLUSTERED 
(
	[ResultsFieldID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET IDENTITY_INSERT [tlkpModFlow_FieldDictionary] ON
GO

INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (12, N'ZONEBUDGET', N'CONSTANT HEAD', N'CONSTANT HEAD: IN', N'IN', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (13, N'ZONEBUDGET', N'CONSTANT HEAD', N'CONSTANT HEAD: OUT', N'OUT', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (14, N'ZONEBUDGET', N'Total IN', N'Total IN', N'IN', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (15, N'ZONEBUDGET', N'Total Out', N'Total OUT', N'OUT', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (16, N'ZONEBUDGET', N'IN-OUT', N'IN-OUT', N'TOTAL', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (17, N'ZONEBUDGET', N'Percent Error', N'Percent Error', N'TOTAL', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (18, N'ZONEBUDGET', N'STORAGE', N'STORAGE: IN', N'IN', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (19, N'ZONEBUDGET', N'STORAGE', N'STORAGE: OUT', N'OUT', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (20, N'ZONEBUDGET', N'RIVER LEAKAGE', N'RIVER LEAKAGE: IN', N'IN', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (21, N'ZONEBUDGET', N'HEAD DEP BOUNDS', N'HEAD DEP BOUNDS: IN', N'IN', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (22, N'ZONEBUDGET', N'RECHARGE', N'RECHARGE: IN', N'IN', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (23, N'ZONEBUDGET', N'RIVER LEAKAGE', N'RIVER LEAKAGE: IN', N'OUT', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (24, N'ZONEBUDGET', N'HEAD DEP BOUNDS', N'HEAD DEP BOUNDS: IN', N'OUT', NULL)
GO
INSERT [dbo].[tlkpModFlow_FieldDictionary] ([ResultsFieldID], [FeatureType], [FieldName], [FieldAlias], [Qualifier], [UnitType]) VALUES (25, N'ZONEBUDGET', N'RECHARGE', N'RECHARGE: IN', N'OUT', NULL)
GO

SET IDENTITY_INSERT [tlkpModFlow_FieldDictionary] OFF
GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.05h', 'Creating schema and data for Modflow dictionary lookup tables'); 