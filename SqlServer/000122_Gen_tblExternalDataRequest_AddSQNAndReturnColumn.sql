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

/*tblExternalDataRequest - Set column SQN to int*/
Exec #dropconstraints N'tblExternalDataRequest', N'SQN'
GO

ALTER TABLE tblExternalDataRequest  ADD SQN int
GO
Alter table tblExternalDataRequest Alter column SQN int
go
Update tblExternalDataRequest set SQN = 1 where SQN IS NULL
GO
Alter Table tblExternalDataRequest ALTER COLUMN SQN int NOT NULL
GO
ALTER TABLE tblExternalDataRequest ADD CONSTRAINT DF_tblExternalDataRequest_SQN DEFAULT 1 FOR SQN
GO

/*tblExternalDataRequest - Set column ReturnColumn to int*/
Exec #dropconstraints N'tblExternalDataRequest', N'ReturnColumn'
GO

ALTER TABLE tblExternalDataRequest  ADD ReturnColumn int
GO
Alter table tblExternalDataRequest Alter column ReturnColumn int
go
Update tblExternalDataRequest set ReturnColumn = 1 where ReturnColumn IS NULL
GO
Alter Table tblExternalDataRequest ALTER COLUMN ReturnColumn int NOT NULL
GO
ALTER TABLE tblExternalDataRequest ADD CONSTRAINT DF_tblExternalDataRequest_ReturnColumn DEFAULT 1 FOR ReturnColumn
GO


drop procedure #dropconstraints
Go


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.22', 'Add SQN (int, 1) and ReturnColumn (int, 1) to tblExternalDataRequest');
