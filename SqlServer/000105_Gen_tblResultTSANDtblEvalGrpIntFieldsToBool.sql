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

/*tblResultTS - Set column IsAux to boolean*/
Exec #dropconstraints N'tblResultTS', N'IsAux'
GO

ALTER TABLE tblResultTS  ALTER COLUMN IsAux bit
GO
Update tblResultTS set IsAux = 0 where IsAux IS NULL
GO
Alter Table tblResultTS ALTER COLUMN IsAux bit NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_IsAux DEFAULT 0 FOR IsAux
GO

/*tblEvaluationGroup - Set column IsXModel to boolean*/
Exec #dropconstraints N'tblEvaluationGroup', N'IsXModel'
GO

ALTER TABLE tblEvaluationGroup  ALTER COLUMN IsXModel bit
GO
Update tblEvaluationGroup set IsXModel = -1 where IsXModel IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN IsXModel bit NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_IsXModel DEFAULT -1 FOR IsXModel
GO

drop procedure #dropconstraints
Go


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.05', 'Modify IsAux (bit, 0) to tblResultTS, IsXModel (bit, 0) to tblEvaluationGroup');
