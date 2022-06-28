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


Exec #dropconstraints N'tblResultTS_EventSummary', N'IsHardOrigin'
GO

ALTER TABLE tblResultTS_EventSummary  ADD IsHardOrigin bit
GO
Alter table tblResultTS_EventSummary Alter column IsHardOrigin bit
go
Update tblResultTS_EventSummary set IsHardOrigin = 0 where IsHardOrigin IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN IsHardOrigin bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_IsHardOrigin DEFAULT 0 FOR IsHardOrigin
GO

Exec #dropconstraints N'tblResultTS_EventSummary', N'IsHardTerminus'
GO

ALTER TABLE tblResultTS_EventSummary  ADD IsHardTerminus bit
GO
Alter table tblResultTS_EventSummary Alter column IsHardTerminus bit
go
Update tblResultTS_EventSummary set IsHardTerminus = 0 where IsHardTerminus IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN IsHardTerminus bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_IsHardTerminus DEFAULT 0 FOR IsHardTerminus
GO


Exec #dropconstraints N'tblResultTS_EventSummary', N'IsPointVal'
GO

ALTER TABLE tblResultTS_EventSummary  ADD IsPointVal bit
GO
Alter table tblResultTS_EventSummary Alter column IsPointVal bit
go
Update tblResultTS_EventSummary set IsPointVal = 0 where IsPointVal IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN IsPointVal bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_IsPointVal DEFAULT 0 FOR IsPointVal
GO

Exec #dropconstraints N'tblResultTS_EventSummary', N'SearchOriginForward'
GO

ALTER TABLE tblResultTS_EventSummary  ADD SearchOriginForward bit
GO
Alter table tblResultTS_EventSummary Alter column SearchOriginForward bit
go
Update tblResultTS_EventSummary set SearchOriginForward = -1 where SearchOriginForward IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN SearchOriginForward bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_SearchOriginForward DEFAULT -1 FOR SearchOriginForward
GO

Exec #dropconstraints N'tblResultTS_EventSummary', N'SearchTerminusForward'
GO

ALTER TABLE tblResultTS_EventSummary  ADD SearchTerminusForward bit
GO
Alter table tblResultTS_EventSummary Alter column SearchTerminusForward bit
go
Update tblResultTS_EventSummary set SearchTerminusForward = -1 where SearchTerminusForward IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN SearchTerminusForward bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_SearchTerminusForward DEFAULT -1 FOR SearchTerminusForward
GO

Exec #dropconstraints N'tblResultTS_EventSummary', N'OriginOffset'
GO

ALTER TABLE tblResultTS_EventSummary  ADD OriginOffset int
GO
Alter table tblResultTS_EventSummary Alter column OriginOffset int
go
Update tblResultTS_EventSummary set OriginOffset = -1 where OriginOffset IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN OriginOffset int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_OriginOffset DEFAULT -1 FOR OriginOffset
GO

Exec #dropconstraints N'tblResultTS_EventSummary', N'TerminusOffset'
GO

ALTER TABLE tblResultTS_EventSummary  ADD TerminusOffset int
GO
Alter table tblResultTS_EventSummary Alter column TerminusOffset int
go
Update tblResultTS_EventSummary set TerminusOffset = -1 where TerminusOffset IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN TerminusOffset int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_TerminusOffset DEFAULT -1 FOR TerminusOffset
GO

/*PeriodsBefore and PeriodsAfter don't get added as part of script 00.01.21*/
/*Exec #dropconstraints N'tblResultTS_EventSummary', N'PeriodsBefore'
GO
ALTER TABLE tblElementLists DROP COLUMN PeriodsBefore
GO

Exec #dropconstraints N'tblResultTS_EventSummary', N'PeriodsAfter '
GO
ALTER TABLE tblElementLists DROP COLUMN PeriodsAfter
GO*/

/*Modifying Column DV_Label*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'Label'
GO

ALTER TABLE tblResultTS_EventSummary  ADD Label nvarchar(50)
GO
Update tblResultTS_EventSummary set Label = 'Label' where Label IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN Label nvarchar(50) NOT NULL
go
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_Label DEFAULT 'Label' FOR Label
GO


drop procedure #dropconstraints
Go


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.23', 'Add secondary event  for tblResultTS_EventSummary');
