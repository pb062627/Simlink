Create proc #dropconstraints
@table_name nvarchar(256), 
@col_name nvarchar(256)
AS
BEGIN declare @Command  nvarchar(1000)

select @Command = 'ALTER TABLE ' + @table_name + ' drop constraint ' + d.name from sys.tables t   
  join    sys.default_constraints d       
   on d.parent_object_id = t.object_id  
  join    sys.columns c      
  on c.object_id = t.object_id      
    and c.column_id = d.parent_column_id where t.name = @table_name
  and c.name = @col_name

--print @Command

execute (@Command)

END
GO

SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] ON
Go


INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (361, 3, N'ReservoirHead', N'ReservoirHead', 1, 2, NULL, 0, 4, 0, 0, 0, NULL, 1)

INSERT [dbo].[tlkpEPANETFieldDictionary] ([ID], [TableName_FK], [FieldName], [FieldAlias], [InINP], [FieldINP_Number], [Subtype], [CanBeDV], [FieldClass], [VarOptionList_FK], [IsLookupList], [API_Update], [EPANET_CSharp_LibInt], [RowNo]) 
VALUES (362, 3, N'ReservoirPattern', N'ReservoirPattern', 1, 3, NULL, 0, 4, 0, 0, 0, NULL, 1)

GO

SET IDENTITY_INSERT [tlkpEPANETFieldDictionary] off
Go

drop procedure #dropconstraints
Go


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.10a', 'Add Reservoir fields to EPANETFieldDictionary table');
