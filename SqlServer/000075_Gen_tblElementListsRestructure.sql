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
Exec #dropconstraints N'tblElementLists', N'ProjID_FK'
GO

Alter table tblElementLists Alter column ProjID_FK int
GO
Update tblElementLists set ProjID_FK = -1 where ProjID_FK IS NULL
GO
Alter Table tblElementLists ALTER COLUMN ProjID_FK int NOT NULL
GO



/*Modifying Column ElementListLabel*/
Exec #dropconstraints N'tblElementLists', N'ElementListLabel'
GO

Alter table tblElementLists Alter column ElementListLabel nvarchar(25)
GO
Update tblElementLists set ElementListLabel = '' where ElementListLabel IS NULL
GO
Alter Table tblElementLists ALTER COLUMN ElementListLabel nvarchar(25) NOT NULL
GO
ALTER TABLE tblElementLists ADD CONSTRAINT DF_tblElementLists_ElementListLabel DEFAULT '' FOR ElementListLabel
GO


/*Modifying Column TableID_FK*/
Exec #dropconstraints N'tblElementLists', N'TableID_FK'
GO
ALTER TABLE tblElementLists DROP COLUMN TableID_FK
GO

/*Modifying Column ElementID_FK*/
Exec #dropconstraints N'tblElementLists', N'ElementID_FK'
GO
ALTER TABLE tblElementLists DROP COLUMN ElementID_FK
GO


/*Modifying Column CostID_FK*/
Exec #dropconstraints N'tblElementLists', N'CostID_FK'
GO
ALTER TABLE tblElementLists DROP COLUMN CostID_FK
GO

drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.75', 'tblElementLists restructuring');
