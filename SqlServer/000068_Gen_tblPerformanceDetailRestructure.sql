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


/*Modifying Column DVID_FK*/
Exec #dropconstraints N'tblPerformance_Detail', N'DVID_FK'
GO
ALTER TABLE tblPerformance_Detail DROP COLUMN DVID_FK
GO



/*Modifying Column IsLinkToGroup*/
Exec #dropconstraints N'tblPerformance_Detail', N'IsLinkToGroup'
GO
ALTER TABLE tblPerformance_Detail DROP COLUMN IsLinkToGroup
GO

/*Modifying Column ScenarioElementVal_ID*/
Exec #dropconstraints N'tblPerformance_Detail', N'ScenarioElementVal_ID'
GO
ALTER TABLE tblPerformance_Detail DROP COLUMN ScenarioElementVal_ID
GO

/*Modifying Column PerformanceLKP_FK*/
Exec #dropconstraints N'tblPerformance_Detail', N'PerformanceLKP_FK'
GO
ALTER TABLE tblPerformance_Detail DROP COLUMN PerformanceLKP_FK
GO

/*Modifying Column IsInvalid*/
Exec #dropconstraints N'tblPerformance_Detail', N'IsInvalid'
GO
ALTER TABLE tblPerformance_Detail DROP COLUMN IsInvalid
GO

/*Modifying Column Quantity*/
Exec #dropconstraints N'tblPerformance_Detail', N'Quantity'
GO
ALTER TABLE tblPerformance_Detail DROP COLUMN Quantity
GO

drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.68', 'tblPerformance_Detail restructuring');
