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

/*Rename fields in tblSupportingFileSpec*/
EXEC sp_RENAME 'tblSupportingFileSpec.DestColumn', 'DestColumnNo', 'COLUMN'
GO

/*Create new fields in tblSupportingFileSpec*/
EXEC sp_RENAME 'tblSupportingFileSpec.GroupID', 'GroupID_FK', 'COLUMN'
GO

/*Drop Column EvalID_FK*/
Exec #dropconstraints N'tblSupportingFileSpec', N'EvalID_FK'
GO
ALTER TABLE tblSupportingFileSpec DROP COLUMN EvalID_FK
GO

/*Drop Column description*/
Exec #dropconstraints N'tblSupportingFileSpec', N'description'
GO
ALTER TABLE tblSupportingFileSpec DROP COLUMN description
GO

/*Drop Column destination_ExternalDataCode*/
Exec #dropconstraints N'tblSupportingFileSpec', N'destination_ExternalDataCode'
GO
ALTER TABLE tblSupportingFileSpec DROP COLUMN destination_ExternalDataCode
GO

/*Drop Column IsInput*/
Exec #dropconstraints N'tblSupportingFileSpec', N'IsInput'
GO
ALTER TABLE tblSupportingFileSpec DROP COLUMN IsInput
GO

/*Drop Column params*/
Exec #dropconstraints N'tblSupportingFileSpec', N'params'
GO
ALTER TABLE tblSupportingFileSpec DROP COLUMN params
GO

/*Drop Column conn_string*/
Exec #dropconstraints N'tblSupportingFileSpec', N'conn_string'
GO
ALTER TABLE tblSupportingFileSpec DROP COLUMN conn_string
GO


/* --------------- drop and rename columns from tblEDR and use new table tblExternalGroup ----------------*/

/*Rename fields in tblExternalDataRequest*/
EXEC sp_RENAME 'tblExternalDataRequest.ReturnColumn', 'ReturnColumnNo', 'COLUMN'
GO

/*Create new fields in tblExternalDataRequest*/
EXEC sp_RENAME 'tblExternalDataRequest.GroupID', 'GroupID_FK', 'COLUMN'
GO

/*Drop Column EvalID_FK*/
Exec #dropconstraints N'tblExternalDataRequest', N'EvalID_FK'
GO
ALTER TABLE tblExternalDataRequest DROP COLUMN EvalID_FK
GO

/*Drop Column description*/
Exec #dropconstraints N'tblExternalDataRequest', N'description'
GO
ALTER TABLE tblExternalDataRequest DROP COLUMN description
GO

/*Drop Column source_ExternalDataCode*/
Exec #dropconstraints N'tblExternalDataRequest', N'source_ExternalDataCode'
GO
ALTER TABLE tblExternalDataRequest DROP COLUMN source_ExternalDataCode
GO

/*Drop Column params*/
Exec #dropconstraints N'tblExternalDataRequest', N'params'
GO
ALTER TABLE tblExternalDataRequest DROP COLUMN params
GO

/*Drop Column conn_string*/
Exec #dropconstraints N'tblExternalDataRequest', N'conn_string'
GO
ALTER TABLE tblExternalDataRequest DROP COLUMN conn_string
GO



drop procedure #dropconstraints
Go



Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.32', 'Rename and drop fields in tblSFS and tblEDR');