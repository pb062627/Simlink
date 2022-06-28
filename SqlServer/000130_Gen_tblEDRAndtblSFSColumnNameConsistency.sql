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

/*tblSupportingFileSpec - rename column*/
EXEC sp_RENAME 'tblSupportingFileSpec.SimlinkData_Code', 'source_SimlinkDataTypeCode', 'COLUMN'
GO
EXEC sp_RENAME 'tblSupportingFileSpec.DataType_Code', 'source_DataFormat', 'COLUMN'
GO
EXEC sp_RENAME 'tblSupportingFileSpec.Filename', 'conn_string', 'COLUMN'
GO
EXEC sp_RENAME 'tblSupportingFileSpec.Destination_Code', 'destination_ExternalDataCode', 'COLUMN'
GO
EXEC sp_RENAME 'tblSupportingFileSpec.Params', 'params', 'COLUMN'
GO
/*tblExternalDataRequest - rename column*/
EXEC sp_RENAME 'tblExternalDataRequest.return_format_code', 'destination_SimlinkDataTypeCode', 'COLUMN'
GO
EXEC sp_RENAME 'tblExternalDataRequest.source_code', 'source_ExternalDataCode', 'COLUMN'
GO


/*tblExternalDataRequest - drop column db_type - handled in source_code*/
Exec #dropconstraints N'tblExternalDataRequest', N'db_type'
GO
ALTER TABLE tblExternalDataRequest DROP COLUMN db_type
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.30', 'Change to ensure consistency in naming convention between tblSupportingFileSpec and tblExternalDataRequest');
