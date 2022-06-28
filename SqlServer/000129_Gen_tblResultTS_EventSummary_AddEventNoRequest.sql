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


Exec #dropconstraints N'tblResultTS_EventSummary', N'AssignEventNoCode'
GO

ALTER TABLE tblResultTS_EventSummary  ADD AssignEventNoCode int
GO
Alter table tblResultTS_EventSummary Alter column AssignEventNoCode int
go
Update tblResultTS_EventSummary set AssignEventNoCode = -1 where AssignEventNoCode IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN AssignEventNoCode int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_AssignEventNoCode DEFAULT -1 FOR AssignEventNoCode
GO

Exec #dropconstraints N'tblResultTS_EventSummary', N'RefPrimaryEvent_StartOffset'
GO

ALTER TABLE tblResultTS_EventSummary  ADD RefPrimaryEvent_StartOffset int
GO
Alter table tblResultTS_EventSummary Alter column RefPrimaryEvent_StartOffset int
go
Update tblResultTS_EventSummary set RefPrimaryEvent_StartOffset = -1 where RefPrimaryEvent_StartOffset IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN RefPrimaryEvent_StartOffset int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_RefPrimaryEvent_StartOffset DEFAULT -1 FOR RefPrimaryEvent_StartOffset
GO

Exec #dropconstraints N'tblResultTS_EventSummary', N'RefPrimaryEvent_EndOffset'
GO

ALTER TABLE tblResultTS_EventSummary  ADD RefPrimaryEvent_EndOffset int
GO
Alter table tblResultTS_EventSummary Alter column RefPrimaryEvent_EndOffset int
go
Update tblResultTS_EventSummary set RefPrimaryEvent_EndOffset = -1 where RefPrimaryEvent_EndOffset IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN RefPrimaryEvent_EndOffset int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_RefPrimaryEvent_EndOffset DEFAULT -1 FOR RefPrimaryEvent_EndOffset
GO


Exec #dropconstraints N'tblResultTS_EventSummary_Detail', N'EventNo'
GO

ALTER TABLE tblResultTS_EventSummary_Detail  ADD EventNo int
GO
Alter table tblResultTS_EventSummary_Detail Alter column EventNo int
go
Update tblResultTS_EventSummary_Detail set EventNo = -1 where EventNo IS NULL
GO
Alter Table tblResultTS_EventSummary_Detail ALTER COLUMN EventNo int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary_Detail ADD CONSTRAINT DF_tblResultTS_EventSummary_Detail_EventNo DEFAULT -1 FOR EventNo
GO

drop procedure #dropconstraints
Go

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.29', 'Add variables needed to support event');
