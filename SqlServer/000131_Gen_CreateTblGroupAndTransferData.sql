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

/****** Object:  Table [dbo].[tblExternalGroup]    Script Date: 4/4/2016 8:53:14 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblExternalGroup](
	[GroupID] [int] Identity(1,1) NOT NULL,
	[EvalID_FK] [int] NOT NULL,
	[description] [nvarchar](255) NULL,
	[params] [nvarchar](255) NULL,
	[ExternalDataCode] [int] NOT NULL Default 0,
	[conn_string] [nvarchar](255) NULL,
	[IsColIDName] bit Default 0,
	[IsInput] bit Default 0,
	[tempHelperGroupID] [int] NULL
 CONSTRAINT [PK_tblExternalGroup] PRIMARY KEY CLUSTERED 
(
	[GroupID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO


/* ----------------- move records from tblSFS to new table -------------------- */
Insert into tblExternalGroup(EvalID_FK, description,params, ExternalDataCode, conn_string, IsInput, tempHelperGroupID) 
 (select distinct EvalID_FK, description,  
  Case when CharIndex('header_col', Params) > 0 then Substring(Params, 1, LEN(Params) - Charindex(',',Reverse(Params))) else Params end, 
 destination_ExternalDataCode, conn_string, IsInput, GroupID from tblSupportingFileSpec)

/*update for new GroupIDs*/
Update A set A.GroupID = B.GroupID from tblSupportingFileSpec A Inner Join tblExternalGroup B on A.EvalID_FK = B.EvalID_FK and isnull(A.[description],'x') = isnull(B.[description],'x') and 
	isnull(Case when CharIndex('header_col', A.Params) > 0 then Substring(A.Params, 1, LEN(A.Params) - Charindex(',',Reverse(A.Params))) else A.Params end, 'x') = isnull(B.params,'x')    
	and A.destination_ExternalDataCode = B.ExternalDataCode and A.conn_string = B.conn_string and A.IsInput = B.IsInput and A.GroupID = B.tempHelperGroupID  
		

/*tblSupportingFileSpec - Set column DestColumnName to int*/
Exec #dropconstraints N'tblSupportingFileSpec', N'DestColumnName'
GO

ALTER TABLE tblSupportingFileSpec  ADD DestColumnName [nvarchar](100)
GO
Alter table tblSupportingFileSpec Alter column DestColumnName [nvarchar](100)
go
Update tblSupportingFileSpec set DestColumnName = '' where DestColumnName IS NULL
GO
Alter Table tblSupportingFileSpec ALTER COLUMN DestColumnName [nvarchar](100) NOT NULL
GO
ALTER TABLE tblSupportingFileSpec ADD CONSTRAINT DF_tblSupportingFileSpec_DestColumnName DEFAULT '' FOR DestColumnName  
GO

/*attempt to populate the DestColumnName with Params fields - MSAccess*/
--Update tblSupportingFileSpec set DestColumnName = iif(Instr(1,Params,'header_col') > 0, Mid(Params, InStrRev(Params, '?') + 1, len(Params)), '')
/*SQL Server*/
Update tblSupportingFileSpec set DestColumnName = (Case when CharIndex('header_col', Params) > 0 then Substring(Params, LEN(Params) - Charindex('?',Reverse(Params))+2, LEN(Params)) else '' end)



/* --------------- move records from tblEDR to new table ----------------*/
Insert into tblExternalGroup(EvalID_FK, description,params, ExternalDataCode, conn_string, tempHelperGroupID)  
 (select distinct EvalID_FK, description,  
 Case when CharIndex('header_col', Params) > 0 then Substring(Params, 1, LEN(Params) - Charindex(',',Reverse(Params))) else Params end, 
 source_ExternalDataCode, conn_string, GroupID from tblExternalDataRequest)

/*update for new GroupIDs*/
Update A set A.GroupID = B.GroupID from tblExternalDataRequest A Inner Join tblExternalGroup B on A.EvalID_FK = B.EvalID_FK and isnull(A.[description],'x') = isnull(B.[description],'x') and 
	isnull(Case when CharIndex('header_col', A.params) > 0 then Substring(A.params, 1, LEN(A.params) - Charindex(',',Reverse(A.params))) else A.params end, 'x') = isnull(B.params,'x')  
	and A.source_ExternalDataCode = B.ExternalDataCode and A.conn_string = B.conn_string and A.GroupID = B.tempHelperGroupID 

/*tblExternalDataRequest - Set column DestColumnName to int*/
Exec #dropconstraints N'tblExternalDataRequest', N'DestColumnName'
GO

ALTER TABLE tblExternalDataRequest  ADD ReturnColumnName [nvarchar](100)
GO
Alter table tblExternalDataRequest Alter column ReturnColumnName [nvarchar](100)
go
Update tblExternalDataRequest set ReturnColumnName = '' where ReturnColumnName IS NULL
GO
Alter Table tblExternalDataRequest ALTER COLUMN ReturnColumnName [nvarchar](100) NOT NULL
GO
ALTER TABLE tblExternalDataRequest ADD CONSTRAINT DF_tblExternalDataRequest_ReturnColumnName DEFAULT '' FOR ReturnColumnName  
GO

/*attempt to populate the ReturnColumnName with Params fields - MSAccess*/
--Update tblExternalDataRequest set ReturnColumnName = iif(Instr(1,Params,'header_col') > 0, Mid(Params, InStrRev(Params, '?') + 1, len(Params)), '')
/*SQL Server*/
Update tblExternalDataRequest set ReturnColumnName = (Case when CharIndex('header_col', Params) > 0 then Substring(Params, LEN(Params) - Charindex('?',Reverse(Params))+2, LEN(Params)) else '' end)


/*update new column IsColIDName - MSAccess*/
--Update tblExternalGroup set IsColIDName = -1 where Instr(1,Params,'header_col') > 0
/*update new column IsColIDName - SQL Server*/
Update tblExternalGroup set IsColIDName = -1 where CharIndex('header_col', Params) > 0

/*drop the temporary helper column*/
Alter table tblExternalGroup drop column tempHelperGroupID

drop procedure #dropconstraints
Go



Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.01.31', 'Create tblExternalGroup and transfer data from tblSFS and tblEDR tables');