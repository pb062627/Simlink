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
Exec #dropconstraints N'tblResultVar', N'Result_Label'
GO

Update tblResultVar set Result_Label = '' where Result_Label IS NULL
GO
Alter Table tblResultVar ALTER COLUMN Result_Label nvarchar(50) NOT NULL
GO
ALTER TABLE tblResultVar ADD CONSTRAINT DF_tblResultVar_Result_Label DEFAULT '' FOR Result_Label
GO


/*Modifying Column EvaluationGroup_FK*/
Exec #dropconstraints N'tblResultVar', N'EvaluationGroup_FK'
GO

Update tblResultVar set EvaluationGroup_FK = -1 where EvaluationGroup_FK IS NULL
GO
Alter Table tblResultVar ALTER COLUMN EvaluationGroup_FK int NOT NULL
GO
ALTER TABLE tblResultVar ADD CONSTRAINT DF_tblResultVar_EvaluationGroup_FK DEFAULT -1 FOR EvaluationGroup_FK
GO


/*Modifying Column VarResultType_FK*/
Exec #dropconstraints N'tblResultVar', N'VarResultType_FK'
GO

Update tblResultVar set VarResultType_FK = -1 where VarResultType_FK IS NULL
GO
Alter Table tblResultVar ALTER COLUMN VarResultType_FK int NOT NULL
GO
ALTER TABLE tblResultVar ADD CONSTRAINT DF_tblResultVar_VarResultType_FK DEFAULT -1 FOR VarResultType_FK
GO



/*Modifying Column ElementID_FK*/
Exec #dropconstraints N'tblResultVar', N'ElementID_FK'
GO

Update tblResultVar set ElementID_FK = -1 where ElementID_FK IS NULL
GO
Alter Table tblResultVar ALTER COLUMN ElementID_FK int NOT NULL
GO
ALTER TABLE tblResultVar ADD CONSTRAINT DF_tblResultVar_ElementID_FK DEFAULT -1 FOR ElementID_FK
GO


/*Modifying Column Element_Label*/
Exec #dropconstraints N'tblResultVar', N'Element_Label'
GO

Update tblResultVar set Element_Label = '' where Element_Label IS NULL
GO
Alter Table tblResultVar ALTER COLUMN Element_Label nvarchar(50) NOT NULL
GO
ALTER TABLE tblResultVar ADD CONSTRAINT DF_tblResultVar_Element_Label DEFAULT '' FOR Element_Label
GO


/*Modifying Column IsListVar*/
Exec #dropconstraints N'tblResultVar', N'IsListVar'
GO

Update tblResultVar set IsListVar = -1 where IsListVar IS NULL
GO
Alter Table tblResultVar ALTER COLUMN IsListVar bit NOT NULL
GO
ALTER TABLE tblResultVar ADD CONSTRAINT DF_tblResultVar_IsListVar DEFAULT -1 FOR IsListVar
GO


/*Modifying Column ImportResultDetail*/
Exec #dropconstraints N'tblResultVar', N'ImportResultDetail'
GO

Update tblResultVar set ImportResultDetail = -1 where ImportResultDetail IS NULL
GO
Alter Table tblResultVar ALTER COLUMN ImportResultDetail bit NOT NULL
GO
ALTER TABLE tblResultVar ADD CONSTRAINT DF_tblResultVar_ImportResultDetail DEFAULT -1 FOR ImportResultDetail
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.65', 'tblResultVar restructuring');
