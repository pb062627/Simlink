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

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/* --------------- drop and rename columns from tblSFS and use new table tblExternalGroup ----------------*/
/*Drop Column source_SimlinkDataTypeCode*/
Exec #dropconstraints N'tblSupportingFileSpec', N'source_SimlinkDataTypeCode'
GO
ALTER TABLE tblSupportingFileSpec DROP COLUMN source_SimlinkDataTypeCode
GO


drop procedure #dropconstraints
Go



Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.35', 'Drop field source_SimlinkDataTypeCode in tblSFS');