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

/*Modifying Column ProjID_FK*/
Exec #dropconstraints N'tblOptionLists', N'ProjID_FK'
GO

Alter table tblOptionLists Alter column ProjID_FK int
GO
Update tblOptionLists set ProjID_FK = -1 where ProjID_FK IS NULL
GO
Alter Table tblOptionLists ALTER COLUMN ProjID_FK int NOT NULL
GO
ALTER TABLE tblOptionLists ADD CONSTRAINT DF_tblOptionLists_ProjID_FK DEFAULT -1 FOR ProjID_FK
GO




/*Modifying Column Qual2*/
Exec #dropconstraints N'tblOptionLists', N'Qual2'
GO
ALTER TABLE tblOptionLists DROP COLUMN Qual2
GO

/*Modifying Column Operation*/
Exec #dropconstraints N'tblOptionLists', N'Operation'
GO

Alter table tblOptionLists Alter column Operation nvarchar(10)
GO
Update tblOptionLists set Operation = 'Identity' where Operation IS NULL
GO
Alter Table tblOptionLists ALTER COLUMN Operation nvarchar(10) NOT NULL
GO
ALTER TABLE tblOptionLists ADD CONSTRAINT DF_tblOptionLists_Operation DEFAULT 'Identity' FOR Operation
GO


/*Modifying Column VarType_FK*/
Exec #dropconstraints N'tblOptionLists', N'VarType_FK'
GO
ALTER TABLE tblOptionLists DROP COLUMN VarType_FK
GO

/*Modifying Column IsScaleValue*/
Exec #dropconstraints N'tblOptionLists', N'IsScaleValue'
GO
ALTER TABLE tblOptionLists DROP COLUMN IsScaleValue
GO

/*Modifying Column VarType_ScaleBy*/
Exec #dropconstraints N'tblOptionLists', N'VarType_ScaleBy'
GO
ALTER TABLE tblOptionLists DROP COLUMN VarType_ScaleBy
GO

drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.73', 'tblOptionLists restructuring');
