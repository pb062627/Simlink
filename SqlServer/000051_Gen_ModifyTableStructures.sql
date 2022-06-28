exec sp_rename 'tblperformance.SkipIfBelowThreshold', 'ApplyThreshold', 'column'
Go

alter table tbldv
drop column Operation_DV
Go

alter table tblscenario
drop column projid_FK
Go

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.51', 'dropped Operation_DV from tblDV, dropped projid_FK from tblScenario, renamed SkipIfBelowThreshold to ApplyThreshold in tblPerformance');
Go