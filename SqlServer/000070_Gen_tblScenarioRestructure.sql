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

/*Modifying Column EvalGroupID_FK*/
Exec #dropconstraints N'tblScenario', N'EvalGroupID_FK'
GO


Alter Table tblScenario ALTER COLUMN EvalGroupID_FK int NULL
GO
ALTER TABLE tblScenario ADD CONSTRAINT DF_tblScenario_EvalGroupID_FK DEFAULT -1 FOR EvalGroupID_FK
GO


/*Modifying Column CreatedBy_User*/
Exec #dropconstraints N'tblScenario', N'CreatedBy_User'
GO
ALTER TABLE tblScenario DROP COLUMN CreatedBy_User
GO

/*Modifying Column UserID_No_DELETE*/
Exec #dropconstraints N'tblScenario', N'UserID_No_DELETE'
GO
ALTER TABLE tblScenario DROP COLUMN UserID_No_DELETE
GO




/*Modifying Column ParentScenario*/
Exec #dropconstraints N'tblScenario', N'ParentScenario'
GO
ALTER TABLE tblScenario DROP COLUMN ParentScenario
GO

/*Modifying Column COST_Capital*/
Exec #dropconstraints N'tblScenario', N'COST_Capital'
GO
ALTER TABLE tblScenario DROP COLUMN COST_Capital
GO

/*Modifying Column COST_OM*/
Exec #dropconstraints N'tblScenario', N'COST_OM'
GO
ALTER TABLE tblScenario DROP COLUMN COST_OM
GO

/*Modifying Column COST_Total*/
Exec #dropconstraints N'tblScenario', N'COST_Total'
GO
ALTER TABLE tblScenario DROP COLUMN COST_Total
GO

/*Modifying Column DateCreated*/
Exec #dropconstraints N'tblScenario', N'DateCreated'
GO


Alter Table tblScenario ALTER COLUMN DateCreated datetime NULL
GO
ALTER TABLE tblScenario ADD CONSTRAINT DF_tblScenario_DateCreated DEFAULT GetDate() FOR DateCreated
GO



/*Modifying Column DNA*/
Exec #dropconstraints N'tblScenario', N'DNA'
GO


Alter Table tblScenario ALTER COLUMN DNA nvarchar(255) NULL
GO
ALTER TABLE tblScenario ADD CONSTRAINT DF_tblScenario_DNA DEFAULT -1 FOR DNA
GO




/*Modifying Column ScenDuration*/
Exec #dropconstraints N'tblScenario', N'ScenDuration'
GO
ALTER TABLE tblScenario DROP COLUMN ScenDuration
GO

/*Modifying Column ScenLC_LastStage*/
Exec #dropconstraints N'tblScenario', N'ScenLC_LastStage'
GO


Alter Table tblScenario ALTER COLUMN ScenLC_LastStage int NULL
GO
ALTER TABLE tblScenario ADD CONSTRAINT DF_tblScenario_ScenLC_LastStage DEFAULT -1 FOR ScenLC_LastStage
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.70', 'tblScenario restructuring');
