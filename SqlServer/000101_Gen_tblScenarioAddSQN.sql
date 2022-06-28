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
/*  Exec #dropconstraints N'tblScenario'  */
/*Add column sqn*/
ALTER TABLE tblScenario  ADD sqn int
GO
Alter table tblScenario Alter column sqn int
go
Update tblScenario set sqn = -1 where sqn  IS NULL
GO
Alter Table tblScenario ALTER COLUMN sqn int NOT NULL
GO
ALTER TABLE tblScenario ADD CONSTRAINT DF_tblScenario_sqn DEFAULT -1 FOR sqn
GO

drop procedure #dropconstraints
Go

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.01', 'Add  sqn (int, -1) to tblScenario');
