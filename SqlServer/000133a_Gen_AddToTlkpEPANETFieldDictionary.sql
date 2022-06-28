/*add new entries into tlkEPANETFieldDictionary*/
SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] ON
Go

INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (413, 21, N'START_DATE', N'START_DATE', 0, -1, NULL, 0, -1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (414, 21, N'START_TIME', N'START_TIME', 0, -1, NULL, 0, -1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (415, 21, N'END_DATE', N'END_DATE', 0, -1, NULL, 0, -1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (416, 21, N'END_TIME', N'END_TIME', 0, -1, NULL, 0, -1, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (417, 21, N'START CLOCKTIME TIME', N'START CLOCKTIME', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (418, 21, N'START CLOCKTIME AM / PM', N'START CLOCKTIME', 1, 4, NULL, 0, 4, 0, 0, 0, NULL, 1)
GO

SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] off
Go



Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.33a', 'Add new entries into tlkpEPANETFieldDictionary');