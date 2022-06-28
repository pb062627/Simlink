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

/*tblResultTS - Set column IsAux to boolean*/

Exec #dropconstraints N'tblResultTS', N'AuxRetrieveCode'
GO

Update tblResultTS set AuxRetrieveCode = 1 where AuxRetrieveCode = 0 
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_AuxRetrieveCode DEFAULT 1 FOR AuxRetrieveCode
GO


drop procedure #dropconstraints
Go


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.24', 'Modify AuxRetrieveCode default value to 1 in tblResultTS');
