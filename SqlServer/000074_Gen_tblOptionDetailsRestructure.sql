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

/*Modifying Column OptionID_FK*/
Exec #dropconstraints N'tblOptionDetails', N'OptionID_FK'
GO

Alter table tblOptionDetails Alter column OptionID_FK int
GO
Update tblOptionDetails set OptionID_FK = -1 where OptionID_FK IS NULL
GO
Alter Table tblOptionDetails ALTER COLUMN OptionID_FK int NOT NULL
GO



/*Modifying Column OptionNo*/
Exec #dropconstraints N'tblOptionDetails', N'OptionNo'
GO

Alter table tblOptionDetails Alter column OptionNo int
GO
Update tblOptionDetails set OptionNo = -1 where OptionNo IS NULL
GO
Alter Table tblOptionDetails ALTER COLUMN OptionNo int NOT NULL
GO



/*Modifying Column val*/
Exec #dropconstraints N'tblOptionDetails', N'val'
GO

Alter table tblOptionDetails Alter column val nvarchar(50)
GO
Update tblOptionDetails set val = -1 where val IS NULL
GO
Alter Table tblOptionDetails ALTER COLUMN val nvarchar(50) NOT NULL
GO



/*Modifying Column valLabelinSCEN*/
Exec #dropconstraints N'tblOptionDetails', N'valLabelinSCEN'
GO
ALTER TABLE tblOptionDetails DROP COLUMN valLabelinSCEN
GO

/*Modifying Column VarID_FK*/
Exec #dropconstraints N'tblOptionDetails', N'VarID_FK'
GO
ALTER TABLE tblOptionDetails DROP COLUMN VarID_FK
GO

drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.74', 'tblOptionDetails restructuring');
