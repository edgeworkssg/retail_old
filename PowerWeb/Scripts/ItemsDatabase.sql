if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[FK_Recurrence_CalendarItems]') and OBJECTPROPERTY(id, N'IsForeignKey') = 1)
ALTER TABLE [dbo].[Recurrence] DROP CONSTRAINT FK_Recurrence_CalendarItems
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CalendarItems_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CalendarItems_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CalendarItems_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CalendarItems_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CalendarItems_GetAll]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CalendarItems_GetAll]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CalendarItems_GetById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CalendarItems_GetById]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CalendarItems_LoadByDates]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CalendarItems_LoadByDates]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CalendarItems_Update]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CalendarItems_Update]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CalendarItems_UpdateWithoutPlace]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CalendarItems_UpdateWithoutPlace]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_Add]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_Add]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_Delete]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_Delete]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_DeleteByItemId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_DeleteByItemId]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_GetById]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_GetById]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_GetByItemId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_GetByItemId]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence_UpdateByItemId]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[Recurrence_UpdateByItemId]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CalendarItems]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[CalendarItems]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Recurrence]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Recurrence]
GO

CREATE TABLE [dbo].[CalendarItems] (
	[Id] [int] IDENTITY (1, 1) NOT NULL ,
	[Title] [nvarchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[StartDate] [datetime] NOT NULL ,
	[EndDate] [datetime] NOT NULL ,
	[Description] [nvarchar] (50) COLLATE Cyrillic_General_CI_AS NOT NULL ,
	[IsAllDay] [bit] NOT NULL ,
	[Place] [nvarchar] (100) COLLATE Cyrillic_General_CI_AS NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Recurrence] (
	[RecId] [int] IDENTITY (1, 1) NOT NULL ,
	[ItemId] [int] NOT NULL ,
	[Pattern] [int] NOT NULL ,
	[SubPattern] [int] NOT NULL ,
	[EndType] [int] NOT NULL ,
	[StartDate] [datetime] NOT NULL ,
	[EndDate] [datetime] NULL ,
	[EndAfter] [int] NULL ,
	[Frequency] [int] NULL ,
	[WeekDays] [int] NULL ,
	[DayOfMonth] [int] NULL ,
	[WeekNum] [int] NULL ,
	[Comment] [nvarchar] (20) COLLATE Cyrillic_General_CI_AS NULL 
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[CalendarItems] WITH NOCHECK ADD 
	CONSTRAINT [DF_CalendarItems_Place] DEFAULT (N'') FOR [Place],
	CONSTRAINT [PK_CalendarItems] PRIMARY KEY  CLUSTERED 
	(
		[Id]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Recurrence] WITH NOCHECK ADD 
	CONSTRAINT [PK_Recurrence] PRIMARY KEY  CLUSTERED 
	(
		[RecId]
	)  ON [PRIMARY] 
GO

ALTER TABLE [dbo].[Recurrence] ADD 
	CONSTRAINT [FK_Recurrence_CalendarItems] FOREIGN KEY 
	(
		[ItemId]
	) REFERENCES [dbo].[CalendarItems] (
		[Id]
	) ON DELETE CASCADE 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE [dbo].CalendarItems_Add
	@Title as nvarchar(50),
	@StartDate as datetime,
	@EndDate as datetime,
	@Description as nvarchar(50),
	@IsAllDay as bit,
	@Place as nvarchar(100),
	@retval int output

AS
INSERT INTO CalendarItems (Title, StartDate, EndDate, [Description], IsAllDay, Place) VALUES (@Title, @StartDate, @EndDate, @Description, @IsAllDay, @Place)
select @retval = @@identity
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE [dbo].CalendarItems_Delete
	@Id as int
AS
DELETE FROM CalendarItems WHERE ([Id] = @Id)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].CalendarItems_GetAll
	
AS
SELECT Id, Title, StartDate, EndDate, Description, IsAllDay, Place FROM CalendarItems
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE [dbo].CalendarItems_GetById
	@Id as int
AS
SELECT Id, Title, StartDate, EndDate, Description, IsAllDay, Place FROM CalendarItems WHERE (Id = @Id)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE [dbo].CalendarItems_LoadByDates
	@StartDate as datetime,
	@EndDate as datetime
AS
SELECT [Id], Title, StartDate, EndDate, [Description], IsAllDay, Place FROM CalendarItems 
WHERE ((StartDate BETWEEN @StartDate AND @EndDate) OR (EndDate BETWEEN @StartDate AND @EndDate) OR (StartDate<@StartDate AND EndDate>@EndDate))
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO


CREATE PROCEDURE [dbo].CalendarItems_Update
	@Id as int,
	@Title as nvarchar(50),
	@StartDate as datetime,
	@EndDate as datetime,
	@Description as nvarchar(50),
	@IsAllDay as bit,
	@Place as nvarchar(100)

AS
UPDATE CalendarItems SET Title = @Title, StartDate = @StartDate, EndDate = @EndDate, [Description] = @Description, IsAllDay = @IsAllDay, Place = @Place WHERE ([Id] = @Id)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].CalendarItems_UpdateWithoutPlace
	@Id as int,
	@Title as nvarchar(50),
	@StartDate as datetime,
	@EndDate as datetime,
	@Description as nvarchar(50),
	@IsAllDay as bit

AS
UPDATE CalendarItems SET Title = @Title, StartDate = @StartDate, EndDate = @EndDate, [Description] = @Description, IsAllDay = @IsAllDay WHERE ([Id] = @Id)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].Recurrence_Add
	@ItemId as int,
	@Pattern as int,
	@SubPattern as int,
	@EndType as int,
	@StartDate as datetime,
	@EndDate as datetime,
	@EndAfter as int,
	@Frequency as int,
	@WeekDays as int,
	@DayOfMonth as int,
	@WeekNum as int,
	@Comment as nvarchar(20),
	@retval int output
AS
INSERT INTO Recurrence (ItemId, Pattern, SubPattern, EndType, StartDate, EndDate, EndAfter, Frequency, WeekDays, DayOfMonth, WeekNum, Comment) VALUES (@ItemId, @Pattern, @SubPattern, @EndType, @StartDate, @EndDate, @EndAfter, @Frequency, @WeekDays, @DayOfMonth, @WeekNum, @Comment)
select @retval = @@identity
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].Recurrence_Delete
	@RecId as int
AS
DELETE FROM Recurrence WHERE (RecId = @RecId)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].Recurrence_DeleteByItemId
	@ItemId as int
AS
DELETE FROM Recurrence WHERE (ItemId = @ItemId)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].Recurrence_GetById
	@RecId as int
AS
SELECT RecId, ItemId, Pattern, SubPattern, EndType, StartDate, EndDate, EndAfter, Frequency, WeekDays, DayOfMonth, WeekNum, Comment FROM Recurrence WHERE (RecId = @RecId)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].Recurrence_GetByItemId
	@ItemId as int
AS
SELECT RecId, ItemId, Pattern, SubPattern, EndType, StartDate, EndDate, EndAfter, Frequency, WeekDays, DayOfMonth, WeekNum, Comment FROM Recurrence WHERE (ItemId = @ItemId)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE [dbo].Recurrence_UpdateByItemId
	@ItemId as int,
	@Pattern as int,
	@SubPattern as int,
	@EndType as int,
	@StartDate as datetime,
	@EndDate as datetime,
	@EndAfter as int,
	@Frequency as int,
	@WeekDays as int,
	@DayOfMonth as int,
	@WeekNum as int,
	@Comment as nvarchar(20)	
AS
UPDATE Recurrence SET Pattern = @Pattern, SubPattern = @SubPattern, EndType = @EndType, StartDate = @StartDate, EndDate = @EndDate, EndAfter = @EndAfter, Frequency = @Frequency, WeekDays = @WeekDays, DayOfMonth = @DayOfMonth, WeekNum = @WeekNum, Comment = @Comment WHERE (ItemId = @ItemId)
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

