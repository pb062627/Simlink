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

/*Modifying Column PerformanceID_FK*/
Exec #dropconstraints N'tblPerformance_ResultXREF', N'PerformanceID_FK'
GO

Alter table tblPerformance_ResultXREF Alter column PerformanceID_FK int
GO
Update tblPerformance_ResultXREF set PerformanceID_FK = -1 where PerformanceID_FK IS NULL
GO
Alter Table tblPerformance_ResultXREF ALTER COLUMN PerformanceID_FK int NOT NULL
GO



/*Modifying Column LinkTableID_FK*/
Exec #dropconstraints N'tblPerformance_ResultXREF', N'LinkTableID_FK'
GO

Alter table tblPerformance_ResultXREF Alter column LinkTableID_FK int
GO
Update tblPerformance_ResultXREF set LinkTableID_FK = -1 where LinkTableID_FK IS NULL
GO
Alter Table tblPerformance_ResultXREF ALTER COLUMN LinkTableID_FK int NOT NULL
GO



/*Modifying Column ScalingFactor*/
Exec #dropconstraints N'tblPerformance_ResultXREF', N'ScalingFactor'
GO

Alter table tblPerformance_ResultXREF Alter column ScalingFactor float
GO
Update tblPerformance_ResultXREF set ScalingFactor = 1 where ScalingFactor IS NULL
GO
Alter Table tblPerformance_ResultXREF ALTER COLUMN ScalingFactor float NOT NULL
GO
ALTER TABLE tblPerformance_ResultXREF ADD CONSTRAINT DF_tblPerformance_ResultXREF_ScalingFactor DEFAULT 1 FOR ScalingFactor
GO


/*Modifying Column LinkType*/
Exec #dropconstraints N'tblPerformance_ResultXREF', N'LinkType'
GO

Alter table tblPerformance_ResultXREF Alter column LinkType int
GO
Update tblPerformance_ResultXREF set LinkType = -1 where LinkType IS NULL
GO
Alter Table tblPerformance_ResultXREF ALTER COLUMN LinkType int NOT NULL
GO
ALTER TABLE tblPerformance_ResultXREF ADD CONSTRAINT DF_tblPerformance_ResultXREF_LinkType DEFAULT -1 FOR LinkType
GO


/*Modifying Column SSMA_TimeStamp*/
Exec #dropconstraints N'tblPerformance_ResultXREF', N'SSMA_TimeStamp'
GO
ALTER TABLE tblPerformance_ResultXREF DROP COLUMN SSMA_TimeStamp
GO

/*Modifying Column ApplyThreshold*/
Exec #dropconstraints N'tblPerformance_ResultXREF', N'ApplyThreshold'
GO

Alter table tblPerformance_ResultXREF Alter column ApplyThreshold int
GO
Update tblPerformance_ResultXREF set ApplyThreshold = -2 where ApplyThreshold IS NULL
GO
Alter Table tblPerformance_ResultXREF ALTER COLUMN ApplyThreshold int NOT NULL
GO
ALTER TABLE tblPerformance_ResultXREF ADD CONSTRAINT DF_tblPerformance_ResultXREF_ApplyThreshold DEFAULT -2 FOR ApplyThreshold
GO


/*Modifying Column Threshold*/
Exec #dropconstraints N'tblPerformance_ResultXREF', N'Threshold'
GO

Alter table tblPerformance_ResultXREF Alter column Threshold float
GO
Update tblPerformance_ResultXREF set Threshold = -1.234 where Threshold IS NULL
GO
Alter Table tblPerformance_ResultXREF ALTER COLUMN Threshold float NOT NULL
GO
ALTER TABLE tblPerformance_ResultXREF ADD CONSTRAINT DF_tblPerformance_ResultXREF_Threshold DEFAULT -1.234 FOR Threshold
GO


/*Modifying Column IsOver_Threshold*/
Exec #dropconstraints N'tblPerformance_ResultXREF', N'IsOver_Threshold'
GO

Alter table tblPerformance_ResultXREF Alter column IsOver_Threshold int
GO
Update tblPerformance_ResultXREF set IsOver_Threshold = -2 where IsOver_Threshold IS NULL
GO
Alter Table tblPerformance_ResultXREF ALTER COLUMN IsOver_Threshold int NOT NULL
GO
ALTER TABLE tblPerformance_ResultXREF ADD CONSTRAINT DF_tblPerformance_ResultXREF_IsOver_Threshold DEFAULT -2 FOR IsOver_Threshold
GO


drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.69', 'tblPerformance_ResultXREF restructuring');
