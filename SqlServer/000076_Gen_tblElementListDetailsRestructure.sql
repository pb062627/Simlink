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

/*Modifying Column ElementListID_FK*/
Exec #dropconstraints N'tblElementListDetails', N'ElementListID_FK'
GO

Alter table tblElementListDetails Alter column ElementListID_FK int
GO
Update tblElementListDetails set ElementListID_FK = -1 where ElementListID_FK IS NULL
GO
Alter Table tblElementListDetails ALTER COLUMN ElementListID_FK int NOT NULL
GO



/*Modifying Column val*/
Exec #dropconstraints N'tblElementListDetails', N'val'
GO
ALTER TABLE tblElementListDetails DROP COLUMN val
GO


/*Modifying Column VarLabel*/
Exec #dropconstraints N'tblElementListDetails', N'VarLabel'
GO

Alter table tblElementListDetails Alter column VarLabel nvarchar(50)
GO
Update tblElementListDetails set VarLabel = -1 where VarLabel IS NULL
GO
Alter Table tblElementListDetails ALTER COLUMN VarLabel nvarchar(50) NOT NULL
GO



drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.76', 'tblElementListDetails restructuring');
