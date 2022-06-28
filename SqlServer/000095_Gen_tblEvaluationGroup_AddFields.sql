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
/*  Exec #dropconstraints N'tblEvaluationGroup'  */
/*Add column CohortID*/
ALTER TABLE tblEvaluationGroup  ADD CohortID int
GO
Alter table tblEvaluationGroup Alter column CohortID int
go
Update tblEvaluationGroup set CohortID = -1 where CohortID IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN CohortID int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_CohortID DEFAULT -1 FOR CohortID
GO

/*Add column CohortSQN*/
ALTER TABLE tblEvaluationGroup  ADD CohortSQN int
GO
Update tblEvaluationGroup set CohortSQN = -1 where CohortSQN IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN CohortSQN int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_CohortSQN DEFAULT -1 FOR CohortSQN
GO

/*Add column SimIDBaseline*/
ALTER TABLE tblEvaluationGroup  ADD SimIDBaseline int
GO
Update tblEvaluationGroup set SimIDBaseline = -1 where SimIDBaseline IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN SimIDBaseline int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_SimIDBaseline DEFAULT -1 FOR SimIDBaseline
GO

/*Add column IsXModel*/
ALTER TABLE tblEvaluationGroup  ADD IsXModel int
GO
Update tblEvaluationGroup set IsXModel = -1 where IsXModel IS NULL
GO
Alter Table tblEvaluationGroup ALTER COLUMN IsXModel int NOT NULL
GO
ALTER TABLE tblEvaluationGroup ADD CONSTRAINT DF_tblEvaluationGroup_IsXModel DEFAULT -1 FOR IsXModel
GO

drop procedure #dropconstraints
Go

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.95', 'Add CohortID (int, -1), CohortSQN(int,-1), SimIDBaseline(int,-1), IsXModel(int,-1) to tblEvaluationGroup');
