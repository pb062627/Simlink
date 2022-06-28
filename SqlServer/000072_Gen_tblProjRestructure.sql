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

/*Modifying Column ProjLabel*/
Exec #dropconstraints N'tblProj', N'ProjLabel'
GO

Alter table tblProj Alter column ProjLabel nvarchar(50)
GO
Update tblProj set ProjLabel = '' where ProjLabel IS NULL
GO
Alter Table tblProj ALTER COLUMN ProjLabel nvarchar(50) NOT NULL
GO
ALTER TABLE tblProj ADD CONSTRAINT DF_tblProj_ProjLabel DEFAULT '' FOR ProjLabel
GO


/*Modifying Column ModelFile_Location*/
Exec #dropconstraints N'tblProj', N'ModelFile_Location'
GO

Alter table tblProj Alter column ModelFile_Location nvarchar(255)
GO
Update tblProj set ModelFile_Location = '' where ModelFile_Location IS NULL
GO
Alter Table tblProj ALTER COLUMN ModelFile_Location nvarchar(255) NOT NULL
GO
ALTER TABLE tblProj ADD CONSTRAINT DF_tblProj_ModelFile_Location DEFAULT '' FOR ModelFile_Location
GO


/*Modifying Column ModelType_ID*/
Exec #dropconstraints N'tblProj', N'ModelType_ID'
GO

Alter table tblProj Alter column ModelType_ID int
GO
Update tblProj set ModelType_ID = -1 where ModelType_ID IS NULL
GO
Alter Table tblProj ALTER COLUMN ModelType_ID int NOT NULL
GO
ALTER TABLE tblProj ADD CONSTRAINT DF_tblProj_ModelType_ID DEFAULT -1 FOR ModelType_ID
GO



/*Modifying Column ModelTargetArea*/
Exec #dropconstraints N'tblProj', N'ModelTargetArea'
GO
ALTER TABLE tblProj DROP COLUMN ModelTargetArea
GO

/*Modifying Column DateCreated*/
Exec #dropconstraints N'tblProj', N'DateCreated'
GO

Alter table tblProj Alter column DateCreated datetime
GO
Update tblProj set DateCreated = GetDate() where DateCreated IS NULL
GO
Alter Table tblProj ALTER COLUMN DateCreated datetime NOT NULL
GO
ALTER TABLE tblProj ADD CONSTRAINT DF_tblProj_DateCreated DEFAULT GetDate() FOR DateCreated
GO


/*Modifying Column UserID_FK*/
Exec #dropconstraints N'tblProj', N'UserID_FK'
GO
ALTER TABLE tblProj DROP COLUMN UserID_FK
GO

/*Modifying Column LastModified*/
Exec #dropconstraints N'tblProj', N'LastModified'
GO

Alter table tblProj Alter column LastModified datetime
GO
Update tblProj set LastModified = GetDate() where LastModified IS NULL
GO
Alter Table tblProj ALTER COLUMN LastModified datetime NOT NULL
GO
ALTER TABLE tblProj ADD CONSTRAINT DF_tblProj_LastModified DEFAULT GetDate() FOR LastModified
GO


/*Modifying Column RESULT_ImportAll*/
Exec #dropconstraints N'tblProj', N'RESULT_ImportAll'
GO
ALTER TABLE tblProj DROP COLUMN RESULT_ImportAll
GO

/*Modifying Column UnitSettings_FK*/
Exec #dropconstraints N'tblProj', N'UnitSettings_FK'
GO
ALTER TABLE tblProj DROP COLUMN UnitSettings_FK
GO

/*Modifying Column DB_Model*/
Exec #dropconstraints N'tblProj', N'DB_Model'
GO
ALTER TABLE tblProj DROP COLUMN DB_Model
GO

drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.72', 'tblProj restructuring');
