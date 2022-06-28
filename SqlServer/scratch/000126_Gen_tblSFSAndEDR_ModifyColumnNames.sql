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

/*tblSupportingFileSpec - rename CohortID to GroupID*/
EXEC sp_RENAME 'tblSupportingFileSpec.CohortID', 'GroupID', 'COLUMN'
GO

/*tblExternalDataRequest - rename kwargs to params*/
EXEC sp_RENAME 'tblExternalDataRequest.kwargs', 'params', 'COLUMN'
GO

/*tblExternalDataRequest - rename SQN to GroupID*/
EXEC sp_RENAME 'tblExternalDataRequest.SQN', 'GroupID', 'COLUMN'
GO



drop procedure #dropconstraints
Go


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.26', 'Modify Column Names Cohort and SQN to GroupID, kwargs to params in tblSupportingFileSpec and tblExternalDataRequest');
