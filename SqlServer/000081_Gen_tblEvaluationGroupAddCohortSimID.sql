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

/*Modifying Column ModFileKey*Exec #dropconstraints N'tblEvaluationGroup', N'ModFileKey'
GO
ALTER TABLE tblEvaluationGroup ADD SimIDBaseline int
Update tblEvaluationGroup set SimIDBaseline = -1 where SimIDBaseline IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN SimIDBaseline nvarchar(255) NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_ModFileKey DEFAULT 'a[replace]b' FOR ModFileKey
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.71', 'tblEvaluationGroup restructuring');
