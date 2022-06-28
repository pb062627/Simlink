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
ALTER TABLE tblResultTS  ADD IsAux int
GO
Update tblResultTS set IsAux = 0 where IsAux IS NULL
GO
Alter Table tblResultTS ALTER COLUMN IsAux int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_IsAux DEFAULT 0 FOR IsAux
GO

drop procedure #dropconstraints
Go


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.02', 'Add  IsAux (int, 0) to tblResultTS');
