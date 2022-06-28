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

/*Modifying Column ScenarioID_FK*/
Exec #dropconstraints N'tblResultTS_EventSummary_Detail', N'ScenarioID_FK'
GO


Alter Table tblResultTS_EventSummary_Detail ALTER COLUMN ScenarioID_FK int NULL
GO
ALTER TABLE tblResultTS_EventSummary_Detail ADD CONSTRAINT DF_tblResultTS_EventSummary_Detail_ScenarioID_FK DEFAULT -1 FOR ScenarioID_FK
GO


/*Modifying Column EventSummary_ID*/
Exec #dropconstraints N'tblResultTS_EventSummary_Detail', N'EventSummary_ID'
GO


Alter Table tblResultTS_EventSummary_Detail ALTER COLUMN EventSummary_ID int NULL
GO
ALTER TABLE tblResultTS_EventSummary_Detail ADD CONSTRAINT DF_tblResultTS_EventSummary_Detail_EventSummary_ID DEFAULT -1 FOR EventSummary_ID
GO


/*Modifying Column ResultTS_ID_FK*/
Exec #dropconstraints N'tblResultTS_EventSummary_Detail', N'ResultTS_ID_FK'
GO
ALTER TABLE tblResultTS_EventSummary_Detail DROP COLUMN ResultTS_ID_FK
GO

/*Modifying Column EventDuration*/
Exec #dropconstraints N'tblResultTS_EventSummary_Detail', N'EventDuration'
GO


Alter Table tblResultTS_EventSummary_Detail ALTER COLUMN EventDuration int NULL
GO
ALTER TABLE tblResultTS_EventSummary_Detail ADD CONSTRAINT DF_tblResultTS_EventSummary_Detail_EventDuration DEFAULT -1 FOR EventDuration
GO


/*Modifying Column EventBeginPeriod*/
Exec #dropconstraints N'tblResultTS_EventSummary_Detail', N'EventBeginPeriod'
GO


Alter Table tblResultTS_EventSummary_Detail ALTER COLUMN EventBeginPeriod int NULL
GO
ALTER TABLE tblResultTS_EventSummary_Detail ADD CONSTRAINT DF_tblResultTS_EventSummary_Detail_EventBeginPeriod DEFAULT 1 FOR EventBeginPeriod
GO





/*Modifying Column Rank_TOTAL*/
Exec #dropconstraints N'tblResultTS_EventSummary_Detail', N'Rank_TOTAL'
GO
ALTER TABLE tblResultTS_EventSummary_Detail DROP COLUMN Rank_TOTAL
GO

/*Modifying Column Rank_Peak*/
Exec #dropconstraints N'tblResultTS_EventSummary_Detail', N'Rank_Peak'
GO
ALTER TABLE tblResultTS_EventSummary_Detail DROP COLUMN Rank_Peak
GO

drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.64', 'tblResultTS_EventSummary_Detail restructuring');
