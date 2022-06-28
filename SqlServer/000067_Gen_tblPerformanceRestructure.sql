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


/*Modifying Column PF_Type*/
Exec #dropconstraints N'tblPerformance', N'PF_Type'
GO

Alter table tblPerformance Alter column PF_Type int
GO
Update tblPerformance set PF_Type = -1 where PF_Type IS NULL
GO
Alter Table tblPerformance ALTER COLUMN PF_Type int NOT NULL
GO




/*Modifying Column LinkTableCode*/
Exec #dropconstraints N'tblPerformance', N'LinkTableCode'
GO

Alter table tblPerformance Alter column LinkTableCode int
GO
Update tblPerformance set LinkTableCode = -1 where LinkTableCode IS NULL
GO
Alter Table tblPerformance ALTER COLUMN LinkTableCode int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_LinkTableCode DEFAULT -1 FOR LinkTableCode
GO


/*Modifying Column PF_FunctionType*/
Exec #dropconstraints N'tblPerformance', N'PF_FunctionType'
GO

Alter table tblPerformance Alter column PF_FunctionType int
GO
Update tblPerformance set PF_FunctionType = -1 where PF_FunctionType IS NULL
GO
Alter Table tblPerformance ALTER COLUMN PF_FunctionType int NOT NULL
GO



/*Modifying Column EvalID_FK*/
Exec #dropconstraints N'tblPerformance', N'EvalID_FK'
GO

Alter table tblPerformance Alter column EvalID_FK int
GO
Update tblPerformance set EvalID_FK = -1 where EvalID_FK IS NULL
GO
Alter Table tblPerformance ALTER COLUMN EvalID_FK int NOT NULL
GO



/*Modifying Column FunctionID_FK*/
Exec #dropconstraints N'tblPerformance', N'FunctionID_FK'
GO

Alter table tblPerformance Alter column FunctionID_FK int
GO
Update tblPerformance set FunctionID_FK = -1 where FunctionID_FK IS NULL
GO
Alter Table tblPerformance ALTER COLUMN FunctionID_FK int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_FunctionID_FK DEFAULT -1 FOR FunctionID_FK
GO


/*Modifying Column TS_Code*/
Exec #dropconstraints N'tblPerformance', N'TS_Code'
GO
ALTER TABLE tblPerformance DROP COLUMN TS_Code
GO


/*Modifying Column LKP_LookupID_FK*/
Exec #dropconstraints N'tblPerformance', N'LKP_LookupID_FK'
GO
ALTER TABLE tblPerformance DROP COLUMN LKP_LookupID_FK
GO

/*Modifying Column LKP_ScaleBy*/
Exec #dropconstraints N'tblPerformance', N'LKP_ScaleBy'
GO
ALTER TABLE tblPerformance DROP COLUMN LKP_ScaleBy
GO

/*Modifying Column LKP_ScaleFactor*/
Exec #dropconstraints N'tblPerformance', N'LKP_ScaleFactor'
GO
ALTER TABLE tblPerformance DROP COLUMN LKP_ScaleFactor
GO

/*Modifying Column ScaleFactorInput*/
Exec #dropconstraints N'tblPerformance', N'ScaleFactorInput'
GO
ALTER TABLE tblPerformance DROP COLUMN ScaleFactorInput
GO

/*Modifying Column UseDifferenceFromBaseline*/
Exec #dropconstraints N'tblPerformance', N'UseDifferenceFromBaseline'
GO
ALTER TABLE tblPerformance DROP COLUMN UseDifferenceFromBaseline
GO

/*Modifying Column LKP_Qual*/
Exec #dropconstraints N'tblPerformance', N'LKP_Qual'
GO
ALTER TABLE tblPerformance DROP COLUMN LKP_Qual
GO

/*Modifying Column IfErrorVal*/
Exec #dropconstraints N'tblPerformance', N'IfErrorVal'
GO
ALTER TABLE tblPerformance DROP COLUMN IfErrorVal
GO

/*Modifying Column IsDistrib*/
Exec #dropconstraints N'tblPerformance', N'IsDistrib'
GO
ALTER TABLE tblPerformance DROP COLUMN IsDistrib
GO

/*Modifying Column ApplyThreshold*/
Exec #dropconstraints N'tblPerformance', N'ApplyThreshold'
GO

Alter table tblPerformance Alter column ApplyThreshold int
GO
Update tblPerformance set ApplyThreshold = 0 where ApplyThreshold IS NULL
GO
Alter Table tblPerformance ALTER COLUMN ApplyThreshold int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_ApplyThreshold DEFAULT 0 FOR ApplyThreshold
GO


/*Modifying Column Threshold*/
Exec #dropconstraints N'tblPerformance', N'Threshold'
GO

Alter table tblPerformance Alter column Threshold float
GO
Update tblPerformance set Threshold = 0 where Threshold IS NULL
GO
Alter Table tblPerformance ALTER COLUMN Threshold float NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_Threshold DEFAULT 0 FOR Threshold
GO


/*Modifying Column ResultFunctionKey*/
Exec #dropconstraints N'tblPerformance', N'ResultFunctionKey'
GO

Alter table tblPerformance Alter column ResultFunctionKey int
GO
Update tblPerformance set ResultFunctionKey = -1 where ResultFunctionKey IS NULL
GO
Alter Table tblPerformance ALTER COLUMN ResultFunctionKey int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_ResultFunctionKey DEFAULT -1 FOR ResultFunctionKey
GO



/*Modifying Column SQN*/
Exec #dropconstraints N'tblPerformance', N'SQN'
GO

Alter table tblPerformance Alter column SQN int
GO
Update tblPerformance set SQN = 100 where SQN IS NULL
GO
Alter Table tblPerformance ALTER COLUMN SQN int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_SQN DEFAULT 100 FOR SQN
GO


/*Modifying Column FunctionArgs*/
Exec #dropconstraints N'tblPerformance', N'FunctionArgs'
GO

Alter table tblPerformance Alter column FunctionArgs nvarchar(255)
GO
Update tblPerformance set FunctionArgs = '' where FunctionArgs IS NULL
GO
Alter Table tblPerformance ALTER COLUMN FunctionArgs nvarchar(255) NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_FunctionArgs DEFAULT '' FOR FunctionArgs
GO


/*Modifying Column DV_ID_FK*/
Exec #dropconstraints N'tblPerformance', N'DV_ID_FK'
GO

Alter table tblPerformance Alter column DV_ID_FK int
GO
Update tblPerformance set DV_ID_FK = -1 where DV_ID_FK IS NULL
GO
Alter Table tblPerformance ALTER COLUMN DV_ID_FK int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_DV_ID_FK DEFAULT -1 FOR DV_ID_FK
GO


/*Modifying Column OptionID_FK*/
Exec #dropconstraints N'tblPerformance', N'OptionID_FK'
GO

Alter table tblPerformance Alter column OptionID_FK int
GO
Update tblPerformance set OptionID_FK = -1 where OptionID_FK IS NULL
GO
Alter Table tblPerformance ALTER COLUMN OptionID_FK int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_OptionID_FK DEFAULT -1 FOR OptionID_FK
GO


/*Modifying Column IsOver_Threshold*/
Exec #dropconstraints N'tblPerformance', N'IsOver_Threshold'
GO

Alter table tblPerformance Alter column IsOver_Threshold int
GO
Update tblPerformance set IsOver_Threshold = -1 where IsOver_Threshold IS NULL
GO
Alter Table tblPerformance ALTER COLUMN IsOver_Threshold int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_IsOver_Threshold DEFAULT -1 FOR IsOver_Threshold
GO


/*Modifying Column ComponentApplyThreshold*/
Exec #dropconstraints N'tblPerformance', N'ComponentApplyThreshold'
GO

Alter table tblPerformance Alter column ComponentApplyThreshold int
GO
Update tblPerformance set ComponentApplyThreshold = 0 where ComponentApplyThreshold IS NULL
GO
Alter Table tblPerformance ALTER COLUMN ComponentApplyThreshold int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_ComponentApplyThreshold DEFAULT 0 FOR ComponentApplyThreshold
GO



/*Modifying Column ComponentIsOver_Threshold*/
Exec #dropconstraints N'tblPerformance', N'ComponentIsOver_Threshold'
GO

Alter table tblPerformance Alter column ComponentIsOver_Threshold int
GO
Update tblPerformance set ComponentIsOver_Threshold = -1 where ComponentIsOver_Threshold IS NULL
GO
Alter Table tblPerformance ALTER COLUMN ComponentIsOver_Threshold int NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_ComponentIsOver_Threshold DEFAULT -1 FOR ComponentIsOver_Threshold
GO


drop procedure #dropconstraints
Go

