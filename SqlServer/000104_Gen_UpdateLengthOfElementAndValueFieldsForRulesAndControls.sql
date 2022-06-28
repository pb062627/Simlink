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


/*Modifying Column val*/
Exec #dropconstraints N'tblOptionDetails', N'val'
GO

Alter table tblOptionDetails Alter column val nvarchar(1000)
GO
Update tblOptionDetails set val = -1 where val IS NULL
GO
Alter Table tblOptionDetails ALTER COLUMN val nvarchar(1000) NOT NULL
GO


/*Modifying Column val in tblModElementVals*/
Exec #dropconstraints N'tblModElementVals', N'val'
GO

Alter Table tblModElementVals ALTER COLUMN val nvarchar(1000) NULL
GO




/*Modifying Column ElementName in tblModElementVals*/
Exec #dropconstraints N'tblModElementVals', N'ElementName'
GO

Alter Table tblModElementVals ALTER COLUMN ElementName nvarchar(1000) NULL
GO



drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.04', 'tblOptionDetails and tblModElementVals increasing varchar field lengths to account for rules and controls');

