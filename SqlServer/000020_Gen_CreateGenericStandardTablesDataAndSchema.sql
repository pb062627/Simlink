/****** Object:  Table [dbo].[tblFunctions]    Script Date: 4/4/2016 8:53:14 AM ******/
DROP TABLE [dbo].[tblFunctions]
GO
/****** Object:  Table [dbo].[tblConstants]    Script Date: 4/4/2016 8:53:14 AM ******/
DROP TABLE [dbo].[tblConstants]
GO
/****** Object:  Table [dbo].[tblConstants]    Script Date: 4/4/2016 8:53:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblConstants](
	[ConstantID] [int] Identity(1,1) NOT NULL,
	[ProjID_FK] [int] NULL,
	[Category] [nvarchar](25) NULL,
	[Label] [nvarchar](35) NULL,
	[val] [float] NULL,
 CONSTRAINT [PK_tblConstants] PRIMARY KEY CLUSTERED 
(
	[ConstantID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblFunctions]    Script Date: 4/4/2016 8:53:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblFunctions](
	[FunctionID] [int] Identity(1,1) NOT NULL,
	[ProjID_FK] [int] NULL,
	[Label] [nvarchar](25) NULL,
	[Category] [nvarchar](25) NULL,
	[CustomFunction] [nvarchar](255) NOT NULL,
	[TS_Only] [bit] NOT NULL,
	[UseQuickParse] [bit] NOT NULL,
	[Notes] [nvarchar](255) NULL,
 CONSTRAINT [PK_tblFunctions] PRIMARY KEY CLUSTERED 
(
	[FunctionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


SET IDENTITY_INSERT [tblConstants] ON
GO

INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (2, -1, N'Unit Conversion', N'CFS to MGD', 0.646316889697)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (3, -1, N'Unit Conversion', N'MGD to CFS', 1.54722863651112)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (4, -1, N'Unit Conversion', N'inches to feet', 0.083333333333333329)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (5, -1, N'Unit Conversion', N'feet to inches', 12)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (6, -1, N'Unit Conversion', N'MGAL to gallons', 1000000)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (7, -1, N'Unit Conversion', N'Gallons to MGAL', 1E-06)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (8, -1, N'Unit Conversion', N'ft3 to gallons', 7.48051948)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (9, -1, N'Unit Conversion', N'days to hours', 24)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (10, -1, N'Unit Conversion', N'hours to days', 0.041666666666666664)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (22, 107, N'Phosphorous Removal', N'R_TP_1inch_Bio_ret_surface', 0.76)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (23, 107, N'Phosphorous Removal', N'R_TP_1inch_Bio_ret_drain', 0.76)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (24, 107, N'Phosphorous Removal', N'R_TP_1inch_Inf_trench_surface', 0)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (25, 107, N'Phosphorous Removal', N'R_TP_1inch_Inf_trench_drain', 0.94)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (26, 107, N'Phosphorous Removal', N'R_TP_1inch_PP_surface', 0)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (27, 107, N'Phosphorous Removal', N'R_TP_1inch_PP_drain', 0.74)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (28, 107, N'Phosphorous Removal', N'R_TP_1inch_RB_surface', 0)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (29, 107, N'Phosphorous Removal', N'R_TP_1inch_RB_drain', 0)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (30, 107, N'Phosphorous Removal', N'R_TP_1inch_Swale_surface', 0.21)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (31, 107, N'Phosphorous Removal', N'R_TP_1inch_Swale_drain', 0)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (32, 107, N'Cost', N'Bioretention (mean)', 18)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (33, 107, N'Cost', N'Infiltration Trench (mean)', 33)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (34, 107, N'Cost', N'Pourous pavement (mean)', 11.5)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (35, 107, N'Cost', N'Grass Swale (mean)', 1)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (36, 107, N'Cost', N'Rain Barrel (mean)', 80)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (37, -1, N'Unit Conversion', N'Liters to Gallons', 0.264172)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (38, -1, N'Unit Conversion', N'Gallons to Liters', 3.7854125342579832)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (39, -1, N'Unit Conversion', N'Grams to Pounds', 0.00220462)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (40, -1, N'Unit Conversion', N'Pounds to Grams', 453.59290943563968)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (41, 107, N'LID_Ratio', N'Bio-ret_1inch', 27.942857142857143)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (42, 107, N'LID_Ratio', N'Infil_trench_1inch', 31.114285714285714)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (43, 107, N'LID_Ratio', N'PP_1inch', 20.223926073926076)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (44, 107, N'LID_Ratio', N'Bioswale_1inch', 24)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (45, 110, N'Storage', N'Conlin_TargetVol (ac-ft)', 5)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (46, 110, N'Storage', N'Conlin_MaxHeight (ft)', 6)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (47, -1, N'Unit Conversion', N'Square feet per acre', 43560)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (48, 110, N'Storage', N'Engleside (F)_TargetVol (ac-ft)', 7.952)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (49, 110, N'Storage', N'Engleside (F)_MaxDepth(ft)', 11.35)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (50, 110, N'Storage', N'Engleside_C_TargetVol (ac-ft)', 8.52)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (51, 110, N'Storage', N'Engleside_C_MaxDepth(ft)', 5)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (52, 110, N'Cost', N'Underground Storage ($/ac-ft)', 9438408)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (53, 110, N'Cost', N'Underground Storage (fixed cost  $)', 5067300)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (54, 110, N'Storage', N'Engleside (I)_TargetVol (ac-ft)', 13.3868)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (55, 110, N'Storage', N'Engleside (I)_MaxDepth(ft)', 6.5)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (56, 110, N'Storage', N'Susquehanna (D)_TargetVol (ac-ft)', 0.33)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (57, 110, N'Storage', N'Susquehanna (D)_MaxDepth(ft)', 4)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (58, 110, N'Storage', N'Stevens (B) TargetVol (ac-ft)', 1.48)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (59, 110, N'Storage', N'Stevens (B) MaxDepth(ft)', 7)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (60, 110, N'HRT_Cost', N'HRT Cost - Fixed Cost', 15800000.666)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (61, 110, N'HRT_Cost', N'HRT Cost - Variable Cost', 1616800.0001)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (62, 110, N'Nutrient Loading', N'TN - CSO Areas (mg/l)', 3.6)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (63, 110, N'Nutrient Loading', N'TN - SSO Areas (mg/l)', 2)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (64, 110, N'Nutrient Loading', N'TP - CSO Areas (mg/l)', 0.7)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (65, 110, N'Nutrient Loading', N'TP - SSO Areas (mg/l)', 0.27)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (66, 110, N'Nutrient Loading', N'TN - WWTP- Secondary- Base (mg/l)', 0.18)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (67, 110, N'Nutrient Loading', N'TN - WWTP- Primary- Base (mg/l)', 0.1)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (68, 110, N'Nutrient Loading', N'TP - WWTP- Secondary- Base (mg/l)', 0.034999999999999996)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (69, 110, N'Nutrient Loading', N'TP - WWTP- Primary- Base (mg/l)', 0.013500000000000002)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (70, 110, N'Nutrient Loading', N'TN - WWTP- Secondary- Alt (mg/l)', 0.135)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (71, 110, N'Nutrient Loading', N'TN - WWTP- Primary- Alt (mg/l)', 0.075000000000000011)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (72, 110, N'Nutrient Loading', N'TP - WWTP- Secondary- Alt (mg/l)', 0.026249999999999996)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (73, 110, N'Nutrient Loading', N'TP - WWTP- Primary- Alt (mg/l)', 0.010125000000000002)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (75, -1, N'Unit Conversion', N'MGD*mg/l to Pounds', 8.345413)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (76, 110, N'Storage', N'North_Old (DET) Max Height', 8)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (77, -1, N'Unit Conversion', N'gallons to ft3', 0.133680556)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (78, 110, N'Storage', N'ENG_I_AvailFootprint_ft2', 89000)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (79, 110, N'Storage', N'ENG_I_PeakAllowableLevel', 239.07)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (81, 110, N'Storage', N'Stevens (B) MaxDepth (No weir adjus', 5)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (82, 110, N'Cost', N'Underground Storage ($/mgal)', 9438408)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (83, -1, N'Unit Conversion', N'ft3 to gallons', 7.480519456)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (84, -1, N'Unit Conversion', N'ft3 to mgal', 7.48052E-06)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (85, 110, N'Percent Capture', N'PercCapt_b', 0.7636)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (86, 110, N'Percent Capture', N'PercCapt_m', 0.0008)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (87, -1, N'Unit Conversion', N'acft to mgal', 0.325851428)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (88, -1, N'Unit Conversion', N'mgal to acft', 3.068883287)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (89, 110, N'Pollutant Loading', N'UNITS?? Placeholder', 0.123456)
GO
INSERT [dbo].[tblConstants] ([ConstantID], [ProjID_FK], [Category], [Label], [val]) VALUES (90, 110, N'Unit Conversion', N'UNITS?? Placeholder', 1.2345)
GO

SET IDENTITY_INSERT [tblConstants] off
GO


SET IDENTITY_INSERT [tblFunctions] ON
GO

INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (1, 107, N'LID in/hr to CFS', N'Conversion', N'TSinchPerHour * NumberUnits*UnitArea*(1/12*7.48052/1000000*24*1.547)', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (2, -1, N'Total', N'Standard Aggregation', N'TOTAL', 1, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (3, -1, N'Total (Time Scaled)', N'Standard Aggregation', N'TOTAL_TIMESCALED', 1, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (4, -1, N'Average', N'Standard Aggregation', N'AVERAGE', 1, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (5, -1, N'Maximum', N'Standard Aggregation', N'MAXIMUM', 1, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (6, -1, N'Minimum', N'Standard Aggregation', N'MINIMUM', 1, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (7, 107, N'CFS * mg/l', N'Waste Load', N'Outflow * PollutantConcentration*(1-RemovalEfficiency)', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (8, 107, N'CFS * mg/l', N'Waste Load', N'(Qscm-Qd)*TP_Conc', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (9, 107, N'GI Cost', N'Cost', N'UnitCostFor1InchGI*NumberUnits*UnitArea', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (10, 107, N'Imp Area Managed', N'LID Usage', N'100*NumberUnits * 500 *TributaryArea /ImperviousAreaFT', 0, 0, N'500- this is the LID size; assuming this is constant')
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (12, 107, N'RB Imp Area Managed', N'LID Usage', N'100*NumberUnits * 500 /ImperviousAreaFT', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (13, 107, N'Rain Barrel Cost', N'Cost', N'UnitCostFor1InchGI*NumberUnits', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (14, 107, N'Explicit Bioretentio Cost', N'Cost', N'UnitCostFor1InchGI*LidAreaFT2', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (15, 107, N'CFS * mg/l', N'Waste Load', N'Q*PollutantConcentration', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (16, 110, N'CSO Reduction', N'KPI', N'CSO_Total_Baseline -  CSO_Total', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (17, 110, N'Inflow Reduction', N'KPI', N'Inflow_Total_Baseline - Inflow_Total', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (18, 110, N'CSO Reduction Efficiency', N'KPI', N'CSO_Reduction / Inflow_Reduction', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (19, 110, N'Storage Fooptrint', N'MEV', N'TargetVol * PercentOfTarget * ft2_per_acre / MaxHeight', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (20, 110, N'Underground Storage Cost', N'Cost', N'((Storage_ft2 * MaxHeight) / TargetVol ) * CostPerAc-ft + FixedCost', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (21, 110, N'Dollars per Gallon Reduc', N'KPI', N'TotalCost / CSO_Reduction', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (22, 110, N'HRT Cost', N'Cost', N'HRT_MGD*CostPerMGD + HRT_FixedCost', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (23, 110, N'Pollutant Loading', N'KPI', N'PollutantConc_mgl*TotalFlow_MGD*UnitConversion', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (24, 110, N'TN Reduction', N'KPI', N'TN_Baseline-  TN_Alt_Total', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (25, 110, N'Storage Fooptrint by MGAL', N'MEV', N'TargetVol  * ft3_per_gal *1000000/ MaxHeight', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (26, 110, N'Storage Height by MGAL', N'MEV', N'TargetVol  * ft3_per_gal *1000000/ Footprint', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (27, 110, N'Invert from offset', N'MEV', N'PeakLevel - MaxHeight', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (28, 110, N'Underground Storage Cost', N'Cost', N'(Footprint_ft2 * MaxHeight * mgal_per_ft3 ) * CostPerMGAL+ FixedCost', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (29, 110, N'Underground Storage Cost', N'Cost', N'#IF(DV_Val >0,(Footprint_ft2 * MaxHeight * mgal_per_ft3 ) * CostPerMGAL+ FixedCost, 0)', 0, 0, NULL)
GO
INSERT [dbo].[tblFunctions] ([FunctionID], [ProjID_FK], [Label], [Category], [CustomFunction], [TS_Only], [UseQuickParse], [Notes]) VALUES (30, 110, N'Percent Capture', N'KPI', N'm_val*CSO_Reduc_mgal  + b_val', 0, 0, NULL)
GO

SET IDENTITY_INSERT [tblFunctions] off
GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.20', 'Creating schema and data for Constants and Functions tables');