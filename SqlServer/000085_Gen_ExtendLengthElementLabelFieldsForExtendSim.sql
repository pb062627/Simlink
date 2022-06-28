/*SP 22-Jul-2016 - Extend length of DB fields for Element Label*/

/*requesting values from ExtendSim locations*/
Alter Table tblresultTS ALTER COLUMN Element_Label nvarchar(255)
GO

Alter Table tblResultVar ALTER COLUMN Element_Label nvarchar(255) Not Null
GO

/*poking values to ExtendSim locations*/
Alter Table tblElementListDetails ALTER COLUMN VarLabel nvarchar(255) NOT NULL
GO

Alter Table tblModElementVals ALTER COLUMN ElementName nvarchar(255)
GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.85', 'Extend length of Element Label fields to allow for specifying blocks, database parameters');
