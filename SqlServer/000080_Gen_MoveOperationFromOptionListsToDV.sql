/*SP 18-Jul-2016 - Note this script does NOT shift existing values across from tblOptionLists to tblDV - it sets all values to be Identity*/

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


/*Modifying Column Operation*/
Exec #dropconstraints N'tblDV', N'Operation'
GO
ALTER TABLE tblDV ADD Operation nvarchar(10)
GO
Alter table tblDV Alter column Operation nvarchar(10)
GO
Update tblDV set Operation = 'Identity' where Operation IS NULL
GO
Alter Table tblDV ALTER COLUMN Operation nvarchar(10) NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_Operation DEFAULT 'Identity' FOR Operation
GO


/*Modifying Column Operation*/
Exec #dropconstraints N'tblOptionLists', N'Operation'
GO
ALTER TABLE tblOptionLists DROP COLUMN Operation
GO



drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.80', 'Move Operation from tblOptionsLists to tblDV');
