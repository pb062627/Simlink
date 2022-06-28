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

/*Modifying Column Element_Label*/
Exec #dropconstraints N'tblEvaluationGroup', N'IsModFileUserDefined'
GO
ALTER TABLE tblEvaluationGroup DROP COLUMN IsModFileUserDefined
GO

/*  Exec #dropconstraints N'tblEvaluationGroup'  */
/*Add column IsModFileUserDefined*/
ALTER TABLE tblEvaluationGroup  ADD IsModFileUserDefined int
GO
Alter table tblEvaluationGroup Alter column IsModFileUserDefined int
go
Update tblEvaluationGroup set IsModFileUserDefined = 0 where IsModFileUserDefined IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN IsModFileUserDefined int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_IsModFileUserDefined DEFAULT 0 FOR IsModFileUserDefined
GO

drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.97', 'tblEvaluatoinGroup- change IsModFileUserDefined to int default 0');