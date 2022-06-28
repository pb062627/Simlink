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
/*
Exec #dropconstraints N'tblResultTS', N'IsAux'
GO
*/

ALTER TABLE tblResultTS  ADD AuxRetrieveCode int
GO
Alter table tblResultTS Alter column AuxRetrieveCode int
go
Update tblResultTS set AuxRetrieveCode = 0 where AuxRetrieveCode IS NULL
GO
Alter Table tblResultTS ALTER COLUMN AuxRetrieveCode int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_AuxRetrieveCode DEFAULT 0 FOR AuxRetrieveCode
GO

ALTER TABLE tblResultTS  ADD AuxID_FK int
GO
Alter table tblResultTS Alter column AuxID_FK int
go
Update tblResultTS set AuxID_FK = -1 where AuxID_FK IS NULL
GO
Alter Table tblResultTS ALTER COLUMN AuxID_FK int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_AuxID_FK DEFAULT -1 FOR AuxID_FK
GO


/*tblEvaluationGroup - Set column IsXModel to boolean
Exec #dropconstraints N'tblEvaluationGroup', N'IsXModel'
GO
*/

drop procedure #dropconstraints
Go


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.12', 'Add AuxRetrieveCode (int, 0) to tblResultTS');
