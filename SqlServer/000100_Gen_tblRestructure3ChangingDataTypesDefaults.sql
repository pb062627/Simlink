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


/*Modifying Column SecondaryDV_Key*/
Exec #dropconstraints N'tblDV', N'SecondaryDV_Key'
GO

Alter table tblDV Alter column SecondaryDV_Key int
GO
Update tblDV set SecondaryDV_Key = -1 where SecondaryDV_Key IS NULL
GO
Alter Table tblDV ALTER COLUMN SecondaryDV_Key int NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_SecondaryDV_Key DEFAULT -1 FOR SecondaryDV_Key
GO


/*Modifying Column ApplyThreshold*/
Exec #dropconstraints N'tblPerformance', N'ApplyThreshold'
GO

Alter table tblPerformance Alter column ApplyThreshold bit
GO
Update tblPerformance set ApplyThreshold = 0 where ApplyThreshold IS NULL
GO
Alter Table tblPerformance ALTER COLUMN ApplyThreshold bit NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_ApplyThreshold DEFAULT 0 FOR ApplyThreshold
GO



/*Modifying Column IsOver_Threshold*/
Exec #dropconstraints N'tblPerformance', N'IsOver_Threshold'
GO

Alter table tblPerformance Alter column IsOver_Threshold bit
GO
Update tblPerformance set IsOver_Threshold = -1 where IsOver_Threshold IS NULL
GO
Alter Table tblPerformance ALTER COLUMN IsOver_Threshold bit NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_IsOver_Threshold DEFAULT -1 FOR IsOver_Threshold
GO




/*Modifying Column ComponentApplyThreshold*/
Exec #dropconstraints N'tblPerformance', N'ComponentApplyThreshold'
GO

Alter table tblPerformance Alter column ComponentApplyThreshold bit
GO
Update tblPerformance set ComponentApplyThreshold = 0 where ComponentApplyThreshold IS NULL
GO
Alter Table tblPerformance ALTER COLUMN ComponentApplyThreshold bit NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_ComponentApplyThreshold DEFAULT 0 FOR ComponentApplyThreshold
GO



/*Modifying Column ComponentIsOver_Threshold*/
Exec #dropconstraints N'tblPerformance', N'ComponentIsOver_Threshold'
GO

Alter table tblPerformance Alter column ComponentIsOver_Threshold bit
GO
Update tblPerformance set ComponentIsOver_Threshold = -1 where ComponentIsOver_Threshold IS NULL
GO
Alter Table tblPerformance ALTER COLUMN ComponentIsOver_Threshold bit NOT NULL
GO
ALTER TABLE tblPerformance ADD CONSTRAINT DF_tblPerformance_ComponentIsOver_Threshold DEFAULT -1 FOR ComponentIsOver_Threshold
GO







drop procedure #dropconstraints
Go

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.00', 'tidy up - changing data types and default values ');
