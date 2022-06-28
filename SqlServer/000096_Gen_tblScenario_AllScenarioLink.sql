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
/*Add column AltScenarioID*/
ALTER TABLE tblScenario  ADD AltScenarioID int
GO
Alter table tblScenario Alter column AltScenarioID int
go
Update tblScenario set AltScenarioID = -1 where IS NULL
GO
Alter Table tblScenario ALTER COLUMN AltScenarioID int NOT NULL
GO
ALTER TABLE tblScenario ADD CONSTRAINT DF_tblScenario_AltScenarioID DEFAULT -1 FOR AltScenarioID
GO

drop procedure #dropconstraints
Go

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.96', 'Add  AltScenarioID (int, -1), CohortSQN(int,-1), SimIDBaseline(int,-1), IsXModel(int,-1) to tblScenario');
