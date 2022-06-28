/*add new entries into tlkEPANETFieldDictionary*/
SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] ON
Go

INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (359, 14, N'RuleIDAndDetail', N'RuleIDAndDetail', 0, 0, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO
INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (360, 13, N'ControlIDAndDetail', N'ControlIDAndDetail', 0, 0, NULL, 0, 2, 0, 0, 0, NULL, 1)
GO


SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] off
Go



Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.03a', 'Add new entries into tlkEPANETFieldDictionary');
