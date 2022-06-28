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


/*Modifying Column EvaluationGroupID_FK*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'EvaluationGroupID_FK'
GO

Update tblResultTS_EventSummary set EvaluationGroupID_FK = -1 where EvaluationGroupID_FK IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN EvaluationGroupID_FK int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_EvaluationGroupID_FK DEFAULT -1 FOR EvaluationGroupID_FK
GO


/*Modifying Column CategoryID_FK*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'CategoryID_FK'
GO

Update tblResultTS_EventSummary set CategoryID_FK = -1 where CategoryID_FK IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN CategoryID_FK int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_CategoryID_FK DEFAULT -1 FOR CategoryID_FK
GO


/*Modifying Column EventFunctionID*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'EventFunctionID'
GO

Update tblResultTS_EventSummary set EventFunctionID = -1 where EventFunctionID IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN EventFunctionID int NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_EventFunctionID DEFAULT -1 FOR EventFunctionID
GO


/*Modifying Column EventLevelCode*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'EventLevelCode'
GO
ALTER TABLE tblResultTS_EventSummary DROP COLUMN EventLevelCode
GO

/*Modifying Column Threshold_Inst*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'Threshold_Inst'
GO

Update tblResultTS_EventSummary set Threshold_Inst = 0 where Threshold_Inst IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN Threshold_Inst float NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_Threshold_Inst DEFAULT 0 FOR Threshold_Inst
GO



/*Modifying Column Threshold_Cumulative*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'Threshold_Cumulative'
GO

Update tblResultTS_EventSummary set Threshold_Cumulative = 0 where Threshold_Cumulative IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN Threshold_Cumulative float NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_Threshold_Cumulative DEFAULT 0 FOR Threshold_Cumulative
GO





/*Modifying Column CountMeetsThreshold*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'CountMeetsThreshold'
GO
ALTER TABLE tblResultTS_EventSummary DROP COLUMN CountMeetsThreshold
GO

/*Modifying Column ThresholdCalcEQ*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'ThresholdCalcEQ'
GO
ALTER TABLE tblResultTS_EventSummary DROP COLUMN ThresholdCalcEQ
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.63', 'tblResultTS_EventSummary restructuring');
