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


/*Modifying Column IsSpecialCase*/
Exec #dropconstraints N'tblDV', N'IsSpecialCase'
GO

Alter table tblDV Alter column IsSpecialCase bit
GO
Update tblDV set IsSpecialCase = 0 where IsSpecialCase IS NULL
GO
Alter Table tblDV ALTER COLUMN IsSpecialCase bit NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_IsSpecialCase DEFAULT 0 FOR IsSpecialCase
GO



/*Modifying Column IsTS*/
Exec #dropconstraints N'tblDV', N'IsTS'
GO

Alter table tblDV Alter column IsTS bit
GO
Update tblDV set IsTS = 0 where IsTS IS NULL
GO
Alter Table tblDV ALTER COLUMN IsTS bit NOT NULL
GO
ALTER TABLE tblDV ADD CONSTRAINT DF_tblDV_IsTS DEFAULT 0 FOR IsTS
GO


/*Modifying Column IsSecondary*/
Exec #dropconstraints N'tblResultTS', N'IsSecondary'
GO

Alter table tblResultTS Alter column IsSecondary bit
GO
Update tblResultTS set IsSecondary = 0 where IsSecondary IS NULL
GO
Alter Table tblResultTS ALTER COLUMN IsSecondary bit NOT NULL
GO
ALTER TABLE tblResultTS ADD CONSTRAINT DF_tblResultTS_IsSecondary DEFAULT 0 FOR IsSecondary
GO



/*Modifying Column IsOver_Threshold_Inst*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'IsOver_Threshold_Inst'
GO

Alter table tblResultTS_EventSummary Alter column IsOver_Threshold_Inst bit
GO
Update tblResultTS_EventSummary set IsOver_Threshold_Inst = 0 where IsOver_Threshold_Inst IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN IsOver_Threshold_Inst bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_IsOver_Threshold_Inst DEFAULT 0 FOR IsOver_Threshold_Inst
GO



/*Modifying Column IsOver_Threshold_Cumulative*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'IsOver_Threshold_Cumulative'
GO

Alter table tblResultTS_EventSummary Alter column IsOver_Threshold_Cumulative bit
GO
Update tblResultTS_EventSummary set IsOver_Threshold_Cumulative = 0 where IsOver_Threshold_Cumulative IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN IsOver_Threshold_Cumulative bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_IsOver_Threshold_Cumulative DEFAULT 0 FOR IsOver_Threshold_Cumulative
GO



/*Modifying Column CalcValueInExcessOfThreshold*/
Exec #dropconstraints N'tblResultTS_EventSummary', N'CalcValueInExcessOfThreshold'
GO

Alter table tblResultTS_EventSummary Alter column CalcValueInExcessOfThreshold bit
GO
Update tblResultTS_EventSummary set CalcValueInExcessOfThreshold = 0 where CalcValueInExcessOfThreshold IS NULL
GO
Alter Table tblResultTS_EventSummary ALTER COLUMN CalcValueInExcessOfThreshold bit NOT NULL
GO
ALTER TABLE tblResultTS_EventSummary ADD CONSTRAINT DF_tblResultTS_EventSummary_CalcValueInExcessOfThreshold DEFAULT 0 FOR CalcValueInExcessOfThreshold
GO




drop procedure #dropconstraints
Go
Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.90', 'Adding default fields of zeros that were missed as part of restructuring');
