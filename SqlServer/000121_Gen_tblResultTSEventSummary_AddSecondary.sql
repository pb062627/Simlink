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

/*Modifying Column ResultTS_or_Event_ID_FK*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'ResultTS_or_Event_ID_FK'
GO

Update tblResultTS_EventSummary set ResultTS_or_Event_ID_FK = -1 where ResultTS_or_Event_ID_FK IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN ResultTS_or_Event_ID_FK int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_ResultTS_or_Event_ID_FK DEFAULT -1 FOR ResultTS_or_Event_ID_FK
GO

ALTER TABLE tblResultTS_EventSummary  ADD IsSecondary bit
GO
Alter table tblResultTS_EventSummary Alter column IsSecondary bit
go
Update tblResultTS_EventSummary set IsSecondary = 0 where IsSecondary IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN IsSecondary bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_IsSecondary DEFAULT 0 FOR IsSecondary
GO

ALTER TABLE tblResultTS_EventSummary  ADD RefFromBeginning bit
GO
Alter table tblResultTS_EventSummary Alter column RefFromBeginning bit
go
Update tblResultTS_EventSummary set RefFromBeginning = -1 where RefFromBeginning IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN RefFromBeginning bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_RefFromBeginning DEFAULT -1 FOR RefFromBeginning
GO

ALTER TABLE tblResultTS_EventSummary  ADD RefEventID int
GO
Alter table tblResultTS_EventSummary Alter column RefEventID int
go
Update tblResultTS_EventSummary set RefEventID = -1 where RefEventID IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN RefEventID int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_RefEventID DEFAULT -1 FOR RefEventID
GO

ALTER TABLE tblResultTS_EventSummary  ADD sqn int
GO
Alter table tblResultTS_EventSummary Alter column sqn int
go
Update tblResultTS_EventSummary set sqn = 0 where sqn IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN sqn int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_sqn DEFAULT 0 FOR sqn
GO

drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.21', 'tblResultTS_EventSummary - Add secondary column for ');
