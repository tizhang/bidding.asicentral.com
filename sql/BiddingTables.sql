USE [Bidding]
GO

/****** Object:  Table [dbo].[BiddingSettings]    Script Date: 1/22/2018 9:34:19 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[BiddingSettings](
	[BiddingSettingId] [bigint] IDENTITY(1,1) NOT NULL,
	[GroupNames] [nvarchar](max) NULL,
	[MinIncrement] [float] NOT NULL,
	[ShowOwner] [bit] NOT NULL,
	[ShowCurrentPrice] [bit] NOT NULL,
	[BidTimePerUser] [int] NULL,
	[AcceptMinPrice] [float] NOT NULL,
	[StartPrice] [float] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[Type] [int] NOT NULL,
 CONSTRAINT [PK_dbo.BiddingSettings] PRIMARY KEY CLUSTERED 
(
	[BiddingSettingId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

CREATE TABLE [dbo].[BiddingItems](
	[BiddingItemId] [bigint] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](max) NULL,
	[Description] [nvarchar](max) NULL,
	[ImageUrl] [nvarchar](max) NULL,
	[BidTimes] [int] NOT NULL,
	[Price] [float] NOT NULL,
	[OwnerId] [int] NOT NULL,
	[OwnerEmail] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[CreateDate] [datetime] NOT NULL,
	[Setting_BiddingSettingId] [bigint] NULL,
 CONSTRAINT [PK_dbo.BiddingItems] PRIMARY KEY CLUSTERED 
(
	[BiddingItemId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[BiddingItems]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BiddingItems_dbo.BiddingSettings_Setting_BiddingSettingId] FOREIGN KEY([Setting_BiddingSettingId])
REFERENCES [dbo].[BiddingSettings] ([BiddingSettingId])
GO

ALTER TABLE [dbo].[BiddingItems] CHECK CONSTRAINT [FK_dbo.BiddingItems_dbo.BiddingSettings_Setting_BiddingSettingId]
GO

CREATE TABLE [dbo].[BiddingActions](
	[BiddingActionId] [bigint] IDENTITY(1,1) NOT NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Price] [float] NOT NULL,
	[Comment] [nvarchar](max) NULL,
	[BidderId] [int] NOT NULL,
	[BidderEmail] [nvarchar](max) NULL,
	[Status] [nvarchar](max) NULL,
	[BiddingItem_BiddingItemId] [bigint] NULL,
 CONSTRAINT [PK_dbo.BiddingActions] PRIMARY KEY CLUSTERED 
(
	[BiddingActionId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO

ALTER TABLE [dbo].[BiddingActions]  WITH CHECK ADD  CONSTRAINT [FK_dbo.BiddingActions_dbo.BiddingItems_BiddingItem_BiddingItemId] FOREIGN KEY([BiddingItem_BiddingItemId])
REFERENCES [dbo].[BiddingItems] ([BiddingItemId])
GO

ALTER TABLE [dbo].[BiddingActions] CHECK CONSTRAINT [FK_dbo.BiddingActions_dbo.BiddingItems_BiddingItem_BiddingItemId]
GO
