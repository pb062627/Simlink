/****** Object:  Table [dbo].[[tblPerformance]]    Script Date: 6/15/2016 5:25:16 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER TABLE [dbo].[tblPerformance]
ADD 
[ComponentApplyThreshold] [bit] Not NULL DEFAULT 0,
[ComponentThreshold] [float]  Not NULL DEFAULT 0,
[ComponentIsOver_Threshold] [bit] Not NULL DEFAULT -1
GO

ALTER TABLE [dbo].[tblPerformance_ResultXREF]
ADD 
[ApplyThreshold] [bit] NULL,
[Threshold] [float] NULL,
[IsOver_Threshold] [bit] NULL
GO


Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.55', 'Adding threshold structure for performance variables'); 