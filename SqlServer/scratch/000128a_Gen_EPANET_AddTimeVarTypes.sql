/*add additional fields for having a DV represent a time value in EPANET*/

/****** Object:  Table [dbo].[tlkpEPANETFieldDictionary]    Script Date: 3/7/2016 1:21:59 PM ******/

SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] ON
Go

INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (405, 21, N'Duration', N'Duration', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1);
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (406, 21, N'HydraulicTimeStep', N'HydraulicTimeStep', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1);
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (407, 21, N'QualityTimeStep', N'QualityTimeStep', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1);
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (408, 21, N'PatternTimeStep', N'PatternTimeStep', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1);
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (409, 21, N'ReportTimeStep', N'ReportTimeStep', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1);
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (410, 21, N'ReportStart', N'ReportStart', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1);
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (411, 21, N'RuleTimestep', N'RuleTimestep', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1);
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) VALUES (412, 21, N'PatternStart', N'PatternStart', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1);


SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] off
Go



Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.28a', 'Update for more vartypes for time parameters allowing DVs to reference time changes'); 