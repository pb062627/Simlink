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

/*tblResultTS - rename AUXRetrieveCode to RetrieveCode*/
EXEC sp_RENAME 'tblResultTS.AuxRetrieveCode', 'RetrieveCode', 'COLUMN'
GO

/*drop IsAUX and IsSecondary columns and only use RetrieveCode - update all records in tblResultTS*/
update tblResultTS set RetrieveCode = 1;
update tblResultTS set RetrieveCode = 2 where IsSecondary <> 0;
update tblResultTS set RetrieveCode = 3 where IsAux <> 0;


/*Dropping Column IsSecondary*/
Exec #dropconstraints N'tblResultTS', N'IsSecondary'
GO
ALTER TABLE tblResultTS DROP COLUMN IsSecondary
GO

/*Dropping Column IsAux*/
Exec #dropconstraints N'tblResultTS', N'IsAux'
GO
ALTER TABLE tblResultTS DROP COLUMN IsAux
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.29', 'Restructure tblResultTS RetrieveCode and dropping IsSecondary and IsAux');
