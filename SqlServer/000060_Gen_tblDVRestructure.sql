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

/*Modifying Column DV_Label*/
Exec #dropconstraints N'tblDV', N'DV_Label'
GO

Update tblDV set DV_Label = 'Label' where DV_Label IS NULL
GO
Alter Table tblDV ALTER COLUMN DV_Label nvarchar(50) NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_DV_Label DEFAULT 'Label' FOR DV_Label
GO


/*Modifying Column EvaluationGroup_FK*/
Exec #dropconstraints N'tblDV', N'EvaluationGroup_FK'
GO

Update tblDV set EvaluationGroup_FK = -1 where EvaluationGroup_FK IS NULL
GO
Alter Table tblDV ALTER COLUMN EvaluationGroup_FK int NOT NULL
GO



/*Modifying Column VarType_FK*/
Exec #dropconstraints N'tblDV', N'VarType_FK'
GO

Update tblDV set VarType_FK = -1 where VarType_FK IS NULL
GO
Alter Table tblDV ALTER COLUMN VarType_FK int NOT NULL
GO





/*Modifying Column Option_FK*/
Exec #dropconstraints N'tblDV', N'Option_FK'
GO

Update tblDV set Option_FK = -1 where Option_FK IS NULL
GO
Alter Table tblDV ALTER COLUMN Option_FK int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_Option_FK DEFAULT -1 FOR Option_FK
GO


/*Modifying Column Option_MIN*/
Exec #dropconstraints N'tblDV', N'Option_MIN'
GO

Update tblDV set Option_MIN = 1 where Option_MIN IS NULL
GO
Alter Table tblDV ALTER COLUMN Option_MIN int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_Option_MIN DEFAULT 1 FOR Option_MIN
GO


/*Modifying Column Option_MAX*/
Exec #dropconstraints N'tblDV', N'Option_MAX'
GO

Update tblDV set Option_MAX = 4 where Option_MAX IS NULL
GO
Alter Table tblDV ALTER COLUMN Option_MAX int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_Option_MAX DEFAULT 4 FOR Option_MAX
GO


/*Modifying Column GetNewValMethod*/
Exec #dropconstraints N'tblDV', N'GetNewValMethod'
GO

Update tblDV set GetNewValMethod = 1 where GetNewValMethod IS NULL
GO
Alter Table tblDV ALTER COLUMN GetNewValMethod int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_GetNewValMethod DEFAULT 1 FOR GetNewValMethod
GO


/*Modifying Column FunctionID_FK*/
Exec #dropconstraints N'tblDV', N'FunctionID_FK'
GO

Update tblDV set FunctionID_FK = -1 where FunctionID_FK IS NULL
GO
Alter Table tblDV ALTER COLUMN FunctionID_FK int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_FunctionID_FK DEFAULT -1 FOR FunctionID_FK
GO


/*Modifying Column FunctionArgs*/
Exec #dropconstraints N'tblDV', N'FunctionArgs'
GO

Update tblDV set FunctionArgs = '' where FunctionArgs IS NULL
GO
Alter Table tblDV ALTER COLUMN FunctionArgs nvarchar(255) NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_FunctionArgs DEFAULT '' FOR FunctionArgs
GO


/*Modifying Column ElementID_FK*/
Exec #dropconstraints N'tblDV', N'ElementID_FK'
GO

Update tblDV set ElementID_FK = -1 where ElementID_FK IS NULL
GO
Alter Table tblDV ALTER COLUMN ElementID_FK int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_ElementID_FK DEFAULT -1 FOR ElementID_FK
GO


/*Modifying Column Element_Label*/
Exec #dropconstraints N'tblDV', N'Element_Label'
GO
ALTER TABLE tblDV DROP COLUMN Element_Label
GO

/*Modifying Column IncludeInScenarioLabel*/
Exec #dropconstraints N'tblDV', N'IncludeInScenarioLabel'
GO
ALTER TABLE tblDV DROP COLUMN IncludeInScenarioLabel
GO

/*Modifying Column IsListVar*/
Exec #dropconstraints N'tblDV', N'IsListVar'
GO

Update tblDV set IsListVar = -1 where IsListVar IS NULL
GO
Alter Table tblDV ALTER COLUMN IsListVar bit NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_IsListVar DEFAULT -1 FOR IsListVar
GO


/*Modifying Column SkipMinVal*/
Exec #dropconstraints N'tblDV', N'SkipMinVal'
GO

Update tblDV set SkipMinVal = -1 where SkipMinVal IS NULL
GO
Alter Table tblDV ALTER COLUMN SkipMinVal bit NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_SkipMinVal DEFAULT -1 FOR SkipMinVal
GO


/*Modifying Column sqn*/
Exec #dropconstraints N'tblDV', N'sqn'
GO

Update tblDV set sqn = 100 where sqn IS NULL
GO
Alter Table tblDV ALTER COLUMN sqn int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_sqn DEFAULT 100 FOR sqn
GO



/*Modifying Column SecondaryDV_Key*/
Exec #dropconstraints N'tblDV', N'SecondaryDV_Key'
GO

Update tblDV set SecondaryDV_Key = 1 where SecondaryDV_Key IS NULL
GO
Alter Table tblDV ALTER COLUMN SecondaryDV_Key int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_SecondaryDV_Key DEFAULT 1 FOR SecondaryDV_Key
GO


/*Modifying Column PrimaryDV_ID_FK*/
Exec #dropconstraints N'tblDV', N'PrimaryDV_ID_FK'
GO

Update tblDV set PrimaryDV_ID_FK = -1 where PrimaryDV_ID_FK IS NULL
GO
Alter Table tblDV ALTER COLUMN PrimaryDV_ID_FK int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_PrimaryDV_ID_FK DEFAULT -1 FOR PrimaryDV_ID_FK
GO






/*Modifying Column IsDistrib*/
Exec #dropconstraints N'tblDV', N'IsDistrib'
GO
ALTER TABLE tblDV DROP COLUMN IsDistrib
GO

/*Modifying Column Distrib_VarType_FK*/
Exec #dropconstraints N'tblDV', N'Distrib_VarType_FK'
GO
ALTER TABLE tblDV DROP COLUMN Distrib_VarType_FK
GO


/*Modifying Column XModelID_FK*/
Exec #dropconstraints N'tblDV', N'XModelID_FK'
GO

Update tblDV set XModelID_FK = -1 where XModelID_FK IS NULL
GO
Alter Table tblDV ALTER COLUMN XModelID_FK int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_XModelID_FK DEFAULT -1 FOR XModelID_FK
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.60', 'tblDV restructuring');
