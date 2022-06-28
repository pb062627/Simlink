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

/*Modifying Column Result_Label*/
Exec #dropconstraints N'tblResultTS', N'Result_Label'
GO

Update tblResultTS set Result_Label = '' where Result_Label IS NULL
GO
Alter Table tblResultTS ALTER COLUMN Result_Label nvarchar(50) NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_Result_Label DEFAULT '' FOR Result_Label
GO


/*Modifying Column EvaluationGroup_FK*/
Exec #dropconstraints N'tblResultTS', N'EvaluationGroup_FK'
GO

Update tblResultTS set EvaluationGroup_FK = -1 where EvaluationGroup_FK IS NULL
GO
Alter Table tblResultTS ALTER COLUMN EvaluationGroup_FK int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_EvaluationGroup_FK DEFAULT -1 FOR EvaluationGroup_FK
GO


/*Modifying Column ResultID_FK*/
Exec #dropconstraints N'tblResultTS', N'ResultID_FK'
GO
ALTER TABLE tblResultTS DROP COLUMN ResultID_FK
GO

/*Modifying Column VarResultType_FK*/
Exec #dropconstraints N'tblResultTS', N'VarResultType_FK'
GO

Update tblResultTS set VarResultType_FK = -1 where VarResultType_FK IS NULL
GO
Alter Table tblResultTS ALTER COLUMN VarResultType_FK int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_VarResultType_FK DEFAULT -1 FOR VarResultType_FK
GO


/*Modifying Column StationID_FK*/
Exec #dropconstraints N'tblResultTS', N'StationID_FK'
GO

Update tblResultTS set StationID_FK = -1 where StationID_FK IS NULL
GO
Alter Table tblResultTS ALTER COLUMN StationID_FK int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_StationID_FK DEFAULT -1 FOR StationID_FK
GO




/*Modifying Column ElementIndex*/
Exec #dropconstraints N'tblResultTS', N'ElementIndex'
GO

Update tblResultTS set ElementIndex = -1 where ElementIndex IS NULL
GO
Alter Table tblResultTS ALTER COLUMN ElementIndex int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_ElementIndex DEFAULT -1 FOR ElementIndex
GO



/*Modifying Column TS_StartDate*/
Exec #dropconstraints N'tblResultTS', N'TS_StartDate'
GO


Alter Table tblResultTS ALTER COLUMN TS_StartDate datetime NULL
GO



/*Modifying Column TS_StartHour*/
Exec #dropconstraints N'tblResultTS', N'TS_StartHour'
GO


Alter Table tblResultTS ALTER COLUMN TS_StartHour int NULL
GO



/*Modifying Column TS_StartMin*/
Exec #dropconstraints N'tblResultTS', N'TS_StartMin'
GO


Alter Table tblResultTS ALTER COLUMN TS_StartMin int NULL
GO





/*Modifying Column BeginPeriodNo*/
Exec #dropconstraints N'tblResultTS', N'BeginPeriodNo'
GO

Update tblResultTS set BeginPeriodNo = 1 where BeginPeriodNo IS NULL
GO
Alter Table tblResultTS ALTER COLUMN BeginPeriodNo int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_BeginPeriodNo DEFAULT 1 FOR BeginPeriodNo
GO



/*Modifying Column SQN*/
Exec #dropconstraints N'tblResultTS', N'SQN'
GO

Update tblResultTS set SQN = 100 where SQN IS NULL
GO
Alter Table tblResultTS ALTER COLUMN SQN int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_SQN DEFAULT 100 FOR SQN
GO


/*Modifying Column FunctionID_FK*/
Exec #dropconstraints N'tblResultTS', N'FunctionID_FK'
GO

Update tblResultTS set FunctionID_FK = -1 where FunctionID_FK IS NULL
GO
Alter Table tblResultTS ALTER COLUMN FunctionID_FK int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_FunctionID_FK DEFAULT -1 FOR FunctionID_FK
GO


/*Modifying Column FunctionArgs*/
Exec #dropconstraints N'tblResultTS', N'FunctionArgs'
GO

Update tblResultTS set FunctionArgs = '' where FunctionArgs IS NULL
GO
Alter Table tblResultTS ALTER COLUMN FunctionArgs nvarchar(255) NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_FunctionArgs DEFAULT '' FOR FunctionArgs
GO


/*Modifying Column IsAux*/
Exec #dropconstraints N'tblResultTS', N'IsAux'
GO
ALTER TABLE tblResultTS DROP COLUMN IsAux
GO

/*Modifying Column RefTS_ID_FK*/
Exec #dropconstraints N'tblResultTS', N'RefTS_ID_FK'
GO

Update tblResultTS set RefTS_ID_FK = -1 where RefTS_ID_FK IS NULL
GO
Alter Table tblResultTS ALTER COLUMN RefTS_ID_FK int NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_RefTS_ID_FK DEFAULT -1 FOR RefTS_ID_FK
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.61', 'tblResultTS restructuring');
