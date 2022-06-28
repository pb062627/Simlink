/****** Object:  Table [dbo].[tblEPANET_Vertices]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Vertices]
GO
/****** Object:  Table [dbo].[tblEPANET_Valves]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Valves]
GO
/****** Object:  Table [dbo].[tblEPANET_Tanks]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Tanks]
GO
/****** Object:  Table [dbo].[tblEPANET_Status]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Status]
GO
/****** Object:  Table [dbo].[tblEPANET_Sources]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Sources]
GO
/****** Object:  Table [dbo].[tblEPANET_RunSettings]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_RunSettings]
GO
/****** Object:  Table [dbo].[tblEPANET_Reservoirs]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Reservoirs]
GO
/****** Object:  Table [dbo].[tblEPANET_Reactions]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Reactions]
GO
/****** Object:  Table [dbo].[tblEPANET_Quality]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Quality]
GO
/****** Object:  Table [dbo].[tblEPANET_Pumps]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Pumps]
GO
/****** Object:  Table [dbo].[tblEPANET_Pipes]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Pipes]
GO
/****** Object:  Table [dbo].[tblEPANET_Patterns]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Patterns]
GO
/****** Object:  Table [dbo].[tblEPANET_Mixing]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Mixing]
GO
/****** Object:  Table [dbo].[tblEPANET_Junctions]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Junctions]
GO
/****** Object:  Table [dbo].[tblEPANET_Emitters]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Emitters]
GO
/****** Object:  Table [dbo].[tblEPANET_Curves]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Curves]
GO
/****** Object:  Table [dbo].[tblEPANET_Coordinates]    Script Date: 3/7/2016 10:22:02 AM ******/
DROP TABLE [dbo].[tblEPANET_Coordinates]
GO
/****** Object:  Table [dbo].[tblEPANET_Coordinates]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Coordinates](
	[CoordinateID] [int] IDENTITY(1,1) NOT NULL,
	[Node] [nvarchar](255) NULL,
	[XCoord] [float] NULL,
	[YCoord] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Coordinates] PRIMARY KEY CLUSTERED 
(
	[CoordinateID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Curves]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Curves](
	[CurveID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](255) NULL,
	[XValue] [float] NULL,
	[YValue] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Curves] PRIMARY KEY CLUSTERED 
(
	[CurveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Emitters]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Emitters](
	[EmitterID] [int] IDENTITY(1,1) NOT NULL,
	[Junction] [nvarchar](255) NULL,
	[Coefficient] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Emitters] PRIMARY KEY CLUSTERED 
(
	[EmitterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Junctions]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Junctions](
	[JunctionID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](255) NULL,
	[Elev] [float] NULL,
	[Demand] [float] NULL,
	[Pattern] [smallint] NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Junctions] PRIMARY KEY CLUSTERED 
(
	[JunctionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Mixing]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Mixing](
	[MixingID] [int] IDENTITY(1,1) NOT NULL,
	[Tank] [nvarchar](255) NULL,
	[Model] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Mixing] PRIMARY KEY CLUSTERED 
(
	[MixingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Patterns]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Patterns](
	[PatternID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](255) NULL,
	[Multiplier1] [nvarchar](255) NULL,
	[Multiplier2] [nvarchar](255) NULL,
	[Multiplier3] [float] NULL,
	[Multiplier4] [float] NULL,
	[Multiplier5] [float] NULL,
	[Multiplier6] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Patterns] PRIMARY KEY CLUSTERED 
(
	[PatternID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Pipes]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Pipes](
	[PipeID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](255) NULL,
	[Node1] [nvarchar](255) NULL,
	[Node2] [nvarchar](255) NULL,
	[Length] [float] NULL,
	[Diameter] [float] NULL,
	[Roughness] [float] NULL,
	[Status] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Pipes] PRIMARY KEY CLUSTERED 
(
	[PipeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Pumps]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Pumps](
	[PumpID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](255) NULL,
	[Node1] [nvarchar](255) NULL,
	[Node2] [nvarchar](255) NULL,
	[Parameters_] [nvarchar](10) NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Pumps] PRIMARY KEY CLUSTERED 
(
	[PumpID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Quality]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Quality](
	[EmitterID] [int] IDENTITY(1,1) NOT NULL,
	[Node] [nvarchar](255) NULL,
	[InitQual] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Quality] PRIMARY KEY CLUSTERED 
(
	[EmitterID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Reactions]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Reactions](
	[ReactionID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](255) NULL,
	[PipeTank] [nvarchar](255) NULL,
	[Coefficient] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Reactions] PRIMARY KEY CLUSTERED 
(
	[ReactionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Reservoirs]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Reservoirs](
	[ReservoirID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](255) NULL,
	[Head] [nvarchar](255) NULL,
	[Pattern] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Reservoirs] PRIMARY KEY CLUSTERED 
(
	[ReservoirID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_RunSettings]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_RunSettings](
	[EPANET_ID] [int] IDENTITY(1,1) NOT NULL,
	[Scenario] [nvarchar](255) NULL,
	[Date_] [nvarchar](255) NULL,
	[GlobalEfficiency] [nvarchar](255) NULL,
	[GlobalPrice] [nvarchar](255) NULL,
	[DemandChange] [nvarchar](255) NULL,
	[OrderBulk] [int] NULL,
	[OrderTank] [int] NULL,
	[OrderWall] [int] NULL,
	[GlobalBulk] [int] NULL,
	[GlobalWall] [int] NULL,
	[LimitingPotential] [int] NULL,
	[RoughnessCorr] [int] NULL,
	[Duration] [nvarchar](255) NULL,
	[Hydraulic_Timestep] [nvarchar](255) NULL,
	[Quality_Timestep] [nvarchar](255) NULL,
	[Pattern_Timestep] [nvarchar](255) NULL,
	[Pattern_Start] [nvarchar](255) NULL,
	[Report_Timestep] [nvarchar](255) NULL,
	[Report_Start] [nvarchar](255) NULL,
	[Start_ClockTime] [nvarchar](255) NULL,
	[Statistic] [nvarchar](255) NULL,
	[Status] [nvarchar](255) NULL,
	[Summary] [nvarchar](255) NULL,
	[Page] [nvarchar](255) NULL,
	[Units] [nvarchar](255) NULL,
	[Headloss] [nvarchar](255) NULL,
	[SpecificGravity] [nvarchar](255) NULL,
	[Viscosity] [nvarchar](5) NULL,
	[Trials] [nvarchar](5) NULL,
	[Accuracy] [float] NULL,
	[CHECKFREQ] [nvarchar](5) NULL,
	[MAXCHECK] [nvarchar](5) NULL,
	[DAMPLIMIT] [nvarchar](5) NULL,
	[Unbalanced] [nvarchar](20) NULL,
	[Pattern] [nvarchar](5) NULL,
	[DemandMultiplier] [float] NULL,
	[EmitterExponent] [nvarchar](20) NULL,
	[Quality] [nvarchar](25) NULL,
	[Diffusivity] [nvarchar](25) NULL,
	[Tolerance] [float] NULL,
	[MAP_X1] [float] NULL,
	[MAP_X2] [float] NULL,
	[MAP_Y1] [float] NULL,
	[MAP_Y2] [float] NULL,
	[UnitsDELETE] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_RunSettings] PRIMARY KEY CLUSTERED 
(
	[EPANET_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Sources]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Sources](
	[SourceID] [int] IDENTITY(1,1) NOT NULL,
	[Node] [nvarchar](255) NULL,
	[Type] [nvarchar](255) NULL,
	[Quality] [nvarchar](255) NULL,
	[Pattern] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Sources] PRIMARY KEY CLUSTERED 
(
	[SourceID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Status]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Status](
	[StatusID] [int] IDENTITY(1,1) NOT NULL,
	[Id] [nvarchar](255) NULL,
	[StatusSetting] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Status] PRIMARY KEY CLUSTERED 
(
	[StatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Tanks]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Tanks](
	[TankID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](255) NULL,
	[Elevation] [nvarchar](255) NULL,
	[InitLevel] [float] NULL,
	[MinLevel] [float] NULL,
	[MaxLevel] [float] NULL,
	[Diameter] [float] NULL,
	[MinVol] [float] NULL,
	[VolCurve] [nvarchar](255) NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Tanks] PRIMARY KEY CLUSTERED 
(
	[TankID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Valves]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Valves](
	[ValveID] [int] IDENTITY(1,1) NOT NULL,
	[ID] [nvarchar](255) NULL,
	[Node1] [nvarchar](255) NULL,
	[Node2] [nvarchar](255) NULL,
	[Diameter] [float] NULL,
	[Type] [nvarchar](255) NULL,
	[Setting] [float] NULL,
	[MinorLoss] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Valves] PRIMARY KEY CLUSTERED 
(
	[ValveID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[tblEPANET_Vertices]    Script Date: 3/7/2016 10:22:02 AM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[tblEPANET_Vertices](
	[VerticesID] [int] IDENTITY(1,1) NOT NULL,
	[Link] [nvarchar](255) NULL,
	[XCoord] [float] NULL,
	[YCoord] [float] NULL,
	[Description] [nvarchar](255) NULL,
	[ModelVersion] [int] NULL,
	[ParentID] [int] NULL,
 CONSTRAINT [PK_tblEPANET_Vertices] PRIMARY KEY CLUSTERED 
(
	[VerticesID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

Insert Into [dbo].tblVersion (VersionNumber, Description) 
values ('00.00.05b', 'Creating schema for EPANET model mapping tables'); 