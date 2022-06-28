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

/*Modifying Column EvaluationLabel*/
Exec #dropconstraints N'tblEvaluationGroup', N'EvaluationLabel'
GO

Alter table tblEvaluationGroup Alter column EvaluationLabel nvarchar(50)
GO
Update tblEvaluationGroup set EvaluationLabel = '' where EvaluationLabel IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN EvaluationLabel nvarchar(50) NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_EvaluationLabel DEFAULT '' FOR EvaluationLabel
GO



/*Modifying Column DateCreated*/
Exec #dropconstraints N'tblEvaluationGroup', N'DateCreated'
GO

Alter table tblEvaluationGroup Alter column DateCreated datetime
GO
Update tblEvaluationGroup set DateCreated = GetDate() where DateCreated IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN DateCreated datetime NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_DateCreated DEFAULT GetDate() FOR DateCreated
GO



/*Modifying Column ProjID_FK*/
Exec #dropconstraints N'tblEvaluationGroup', N'ProjID_FK'
GO

Alter table tblEvaluationGroup Alter column ProjID_FK int
GO
Update tblEvaluationGroup set ProjID_FK = -1 where ProjID_FK IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN ProjID_FK int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_ProjID_FK DEFAULT -1 FOR ProjID_FK
GO


/*Modifying Column EvalPrefix*/
Exec #dropconstraints N'tblEvaluationGroup', N'EvalPrefix'
GO
ALTER TABLE tblEvaluationGroup DROP COLUMN EvalPrefix
GO

/*Modifying Column RESULT_ImportAll*/
Exec #dropconstraints N'tblEvaluationGroup', N'RESULT_ImportAll'
GO
ALTER TABLE tblEvaluationGroup DROP COLUMN RESULT_ImportAll
GO

/*Modifying Column ModelFileLocation*/
Exec #dropconstraints N'tblEvaluationGroup', N'ModelFileLocation'
GO

Alter table tblEvaluationGroup Alter column ModelFileLocation nvarchar(255)
GO
Update tblEvaluationGroup set ModelFileLocation = '' where ModelFileLocation IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN ModelFileLocation nvarchar(255) NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_ModelFileLocation DEFAULT '' FOR ModelFileLocation
GO


/*Modifying Column ModelType_ID*/
Exec #dropconstraints N'tblEvaluationGroup', N'ModelType_ID'
GO

Alter table tblEvaluationGroup Alter column ModelType_ID int
GO
Update tblEvaluationGroup set ModelType_ID = -1 where ModelType_ID IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN ModelType_ID int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_ModelType_ID DEFAULT -1 FOR ModelType_ID
GO


/*Modifying Column ScenarioID_Baseline_FK*/
Exec #dropconstraints N'tblEvaluationGroup', N'ScenarioID_Baseline_FK'
GO

Alter table tblEvaluationGroup Alter column ScenarioID_Baseline_FK int
GO
Update tblEvaluationGroup set ScenarioID_Baseline_FK = -1 where ScenarioID_Baseline_FK IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN ScenarioID_Baseline_FK int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_ScenarioID_Baseline_FK DEFAULT -1 FOR ScenarioID_Baseline_FK
GO


/*Modifying Column IsSecondary*/
Exec #dropconstraints N'tblEvaluationGroup', N'IsSecondary'
GO
ALTER TABLE tblEvaluationGroup DROP COLUMN IsSecondary
GO

/*Modifying Column ReferenceEvalID_FK*/
Exec #dropconstraints N'tblEvaluationGroup', N'ReferenceEvalID_FK'
GO

Alter table tblEvaluationGroup Alter column ReferenceEvalID_FK int
GO
Update tblEvaluationGroup set ReferenceEvalID_FK = -1 where ReferenceEvalID_FK IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN ReferenceEvalID_FK int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_ReferenceEvalID_FK DEFAULT -1 FOR ReferenceEvalID_FK
GO


/*Modifying Column TS_StartDate*/
Exec #dropconstraints N'tblEvaluationGroup', N'TS_StartDate'
GO

Alter table tblEvaluationGroup Alter column TS_StartDate nvarchar(15)
GO
Update tblEvaluationGroup set TS_StartDate = 36526 where TS_StartDate IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN TS_StartDate nvarchar(15) NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_TS_StartDate DEFAULT 36526 FOR TS_StartDate
GO


/*Modifying Column TS_EndDate*/
Exec #dropconstraints N'tblEvaluationGroup', N'TS_EndDate'
GO

Alter table tblEvaluationGroup Alter column TS_EndDate nvarchar(15)
GO
Update tblEvaluationGroup set TS_EndDate = 36527 where TS_EndDate IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN TS_EndDate nvarchar(15) NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_TS_EndDate DEFAULT 36527 FOR TS_EndDate
GO


/*Modifying Column TS_StartHour*/
Exec #dropconstraints N'tblEvaluationGroup', N'TS_StartHour'
GO

Alter table tblEvaluationGroup Alter column TS_StartHour int
GO
Update tblEvaluationGroup set TS_StartHour = 0 where TS_StartHour IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN TS_StartHour int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_TS_StartHour DEFAULT 0 FOR TS_StartHour
GO


/*Modifying Column TS_StartMin*/
Exec #dropconstraints N'tblEvaluationGroup', N'TS_StartMin'
GO

Alter table tblEvaluationGroup Alter column TS_StartMin int
GO
Update tblEvaluationGroup set TS_StartMin = 0 where TS_StartMin IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN TS_StartMin int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_TS_StartMin DEFAULT 0 FOR TS_StartMin
GO


/*Modifying Column TS_Interval*/
Exec #dropconstraints N'tblEvaluationGroup', N'TS_Interval'
GO


Alter Table tblEvaluationGroup ALTER COLUMN TS_Interval int NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_TS_Interval DEFAULT 1 FOR TS_Interval
GO


/*Modifying Column TS_Duration*/
Exec #dropconstraints N'tblEvaluationGroup', N'TS_Duration'
GO
ALTER TABLE tblEvaluationGroup DROP COLUMN TS_Duration
GO

/*Modifying Column TS_ValShift*/
Exec #dropconstraints N'tblEvaluationGroup', N'TS_ValShift'
GO
ALTER TABLE tblEvaluationGroup DROP COLUMN TS_ValShift
GO

/*Modifying Column ModelScenarioFilter_FK*/
Exec #dropconstraints N'tblEvaluationGroup', N'ModelScenarioFilter_FK'
GO
ALTER TABLE tblEvaluationGroup DROP COLUMN ModelScenarioFilter_FK
GO


/*Modifying Column TSFileIsScen*/
Exec #dropconstraints N'tblEvaluationGroup', N'TSFileIsScen'
GO

Alter table tblEvaluationGroup Alter column TSFileIsScen bit
GO
Update tblEvaluationGroup set TSFileIsScen = -1 where TSFileIsScen IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN TSFileIsScen bit NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_TSFileIsScen DEFAULT -1 FOR TSFileIsScen
GO


/*Modifying Column IntermediateResultCode*/
Exec #dropconstraints N'tblEvaluationGroup', N'IntermediateResultCode'
GO

Alter table tblEvaluationGroup Alter column IntermediateResultCode int
GO
Update tblEvaluationGroup set IntermediateResultCode = -1 where IntermediateResultCode IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN IntermediateResultCode int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_IntermediateResultCode DEFAULT -1 FOR IntermediateResultCode
GO


/*Modifying Column IsModFileUserDefined*/
Exec #dropconstraints N'tblEvaluationGroup', N'IsModFileUserDefined'
GO
ALTER TABLE tblEvaluationGroup ADD IsModFileUserDefined bit
GO
Alter table tblEvaluationGroup Alter column IsModFileUserDefined bit
GO
Update tblEvaluationGroup set IsModFileUserDefined = 0 where IsModFileUserDefined IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN IsModFileUserDefined bit NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_IsModFileUserDefined DEFAULT 0 FOR IsModFileUserDefined
GO


/*Modifying Column ModFileKey*/
Exec #dropconstraints N'tblEvaluationGroup', N'ModFileKey'
GO
ALTER TABLE tblEvaluationGroup ADD ModFileKey nvarchar(255)
GO
Alter table tblEvaluationGroup Alter column ModFileKey nvarchar(255)
GO
Update tblEvaluationGroup set ModFileKey = 'a[replace]b' where ModFileKey IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN ModFileKey nvarchar(255) NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_ModFileKey DEFAULT 'a[replace]b' FOR ModFileKey
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.71', 'tblEvaluationGroup restructuring');
