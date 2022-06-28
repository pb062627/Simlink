/****** Object:  Table [dbo].[tblExtend_Structure]    Script Date: 4/11/2016 5:11:14 PM ******/
DROP TABLE [dbo].[tlkpExtend_StructureDictionary]
GO

/****** Object:  Table [dbo].[hw_node]    Script Date: 4/11/2016 5:11:14 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tlkpExtend_StructureDictionary](
	[StructureID] [int] Identity(1,1) NOT NULL,
	[StructureName] [nvarchar](50) NULL,
		
CONSTRAINT [PK_Extend_Structure] PRIMARY KEY CLUSTERED 
(
	[StructureID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

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
/*Modifying Column StructureID*/
Exec #dropconstraints N'tlkpExtend_StructureDictionary', N'StructureID'
GO

Alter table tlkpExtend_StructureDictionary Alter column StructureID int
GO
Alter Table tlkpExtend_StructureDictionary ALTER COLUMN StructureID int NOT NULL
GO



/*Modifying Column StructureName*/
Exec #dropconstraints N'tlkpExtend_StructureDictionary', N'StructureName'
GO

Alter table tlkpExtend_StructureDictionary Alter column StructureName nvarchar(25)
GO
Update tlkpExtend_StructureDictionary set StructureName = -1 where StructureName IS NULL
GO
Alter Table tlkpExtend_StructureDictionary ALTER COLUMN StructureName nvarchar(25) NOT NULL
GO

/*Insert into new table*/
SET IDENTITY_INSERT [tlkpExtend_StructureDictionary] ON
GO

Insert into tlkpExtend_StructureDictionary(StructureID, StructureName) values (1, 'Database');
Insert into tlkpExtend_StructureDictionary(StructureID, StructureName) values (2, 'Block');

SET IDENTITY_INSERT [tlkpExtend_StructureDictionary] off
GO

drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.5k', 'tlkpExtend_StructureDictionary creation');
