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

/*tblSupportingFileSpec - Set column Destination_Code   to int*/
Exec #dropconstraints N'tblSupportingFileSpec', N'Destination_Code'
GO

ALTER TABLE tblSupportingFileSpec  ADD Destination_Code   int
GO
Alter table tblSupportingFileSpec Alter column Destination_Code   int
go
Update tblSupportingFileSpec set Destination_Code   = 3 where Destination_Code IS NULL
GO
Alter Table tblSupportingFileSpec ALTER COLUMN Destination_Code int NOT NULL
GO
ALTER TABLE tblSupportingFileSpec ADD CONSTRAINT DF_tblSupportingFileSpec_Destination_Code DEFAULT 3 FOR Destination_Code  
GO


/*tblSupportingFileSpec - Set column DV_ID_FK  to int*/
Exec #dropconstraints N'tblSupportingFileSpec', N'DV_ID_FK'
GO

ALTER TABLE tblSupportingFileSpec  ADD DV_ID_FK   int
GO
Alter table tblSupportingFileSpec Alter column DV_ID_FK   int
go
Update tblSupportingFileSpec set DV_ID_FK   = -1 where DV_ID_FK IS NULL
GO
Alter Table tblSupportingFileSpec ALTER COLUMN DV_ID_FK int NOT NULL
GO
ALTER TABLE tblSupportingFileSpec ADD CONSTRAINT DF_tblSupportingFileSpec_DV_ID_FK DEFAULT -1 FOR DV_ID_FK  
GO


/*tblSupportingFileSpec - Set column CohortID  to int*/
Exec #dropconstraints N'tblSupportingFileSpec', N'CohortID'
GO

ALTER TABLE tblSupportingFileSpec  ADD CohortID   int
GO
Alter table tblSupportingFileSpec Alter column CohortID   int
go
Update tblSupportingFileSpec set CohortID   =  1 where CohortID IS NULL
GO
Alter Table tblSupportingFileSpec ALTER COLUMN CohortID int NOT NULL
GO
ALTER TABLE tblSupportingFileSpec ADD CONSTRAINT DF_tblSupportingFileSpec_CohortID DEFAULT 1 FOR CohortID  
GO


/*tblSupportingFileSpec - Set column DestColumn  to int*/
Exec #dropconstraints N'tblSupportingFileSpec', N'DestColumn'
GO

ALTER TABLE tblSupportingFileSpec  ADD DestColumn   int
GO
Alter table tblSupportingFileSpec Alter column DestColumn   int
go
Update tblSupportingFileSpec set DestColumn = 1 where DestColumn IS NULL
GO
Alter Table tblSupportingFileSpec ALTER COLUMN DestColumn int NOT NULL
GO
ALTER TABLE tblSupportingFileSpec ADD CONSTRAINT DF_tblSupportingFileSpec_DestColumn DEFAULT 1 FOR DestColumn  
GO


/*enable this when more stable - tblSupportingFileSpec - rename DataType_Code to DataFormat_Code*/
/*EXEC sp_RENAME 'tblSupportingFileSpec.DataType_Code', 'DataFormat_Code', 'COLUMN'
GO

Exec #dropconstraints N'tblSupportingFileSpec', N'DataFormat_Code'
GO

Alter table tblSupportingFileSpec Alter column DataFormat_Code int
go
Update tblSupportingFileSpec set DataFormat_Code = 1 where DataFormat_Code IS NULL
GO
Alter Table tblSupportingFileSpec ALTER COLUMN DataFormat_Code int NOT NULL
GO
ALTER TABLE tblSupportingFileSpec ADD CONSTRAINT DF_tblSupportingFileSpec_DataFormat_Code DEFAULT 1 FOR DataFormat_Code  
GO
*/


drop procedure #dropconstraints
Go


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.25', 'Add Destination_Code (int, 3), DV_ID_FK(int, -1), CohortID (int, 1) and DestColumn (int, 1) to tblSupportingFileSpec. Rename column DataType_Code to DataFormat_Code');
