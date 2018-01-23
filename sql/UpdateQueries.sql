SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](200) NOT NULL,
	[Email] [nvarchar](500) NOT NULL,
	[Password] [nvarchar] (200) NOT NULL,
	[Groups] [nvarchar] (500) NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Users_UserID] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
))

GO

INSERT INTO [dbo].[Users] (Name, Email, Password, Groups, CreateDate, UpdateDate)
Values( 'Tianyun Zhang', 'tzhang@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Yoyo Fang', 'yfang@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Caroline Chen', 'cchen@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'May Zhang', 'mzhang@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Test1 Bidding', 'tBidding1@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Test2 Bidding', 'tBidding2@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Test3 Bidding', 'tBidding3@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Test4 Bidding', 'tBidding4@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Test5 Bidding', 'tBidding5@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Test6 Bidding', 'tBidding6@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Test7 Bidding', 'tBidding7@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Test8 Bidding', 'tBidding8@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933'),
( 'Test9 Bidding', 'tBidding9@gmail.com', 'password1', 'ASI;WESP;STOR', '2018-01-20 18:48:35.933', '2018-01-20 18:48:35.933')

CREATE TABLE [dbo].[Watchers](
	[WatcherId] [int] IDENTITY(1,1) NOT NULL,
	[BiddingItemId] [bigint]  NOT NULL,
	[UserId] [int]  NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[UpdateDate] [datetime] NOT NULL,
 CONSTRAINT [PK_dbo.Watchers_WathcerId] PRIMARY KEY CLUSTERED 
(
	[WatcherId] ASC
))
GO
ALTER TABLE [dbo].[Watchers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Watchers_dbo.BiddingItems_BiddingItemId] FOREIGN KEY([BiddingItemId])
REFERENCES [dbo].[BiddingItems] ([BiddingItemId])
GO

ALTER TABLE [dbo].[Watchers] CHECK CONSTRAINT [FK_dbo.Watchers_dbo.BiddingItems_BiddingItemId]
GO

ALTER TABLE [dbo].[Watchers]  WITH CHECK ADD  CONSTRAINT [FK_dbo.Watchers_dbo.Users_UserId] FOREIGN KEY([UserId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[Watchers] CHECK CONSTRAINT [FK_dbo.Watchers_dbo.Users_UserId]
GO

-- Add foreign Key
ALTER TABLE [dbo].[BiddingItems]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.BiddingItems$UserId] FOREIGN KEY([OwnerId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[BiddingItems] CHECK CONSTRAINT [FK_dbo.BiddingItems$UserId]
GO

ALTER TABLE [dbo].[BiddingActions]  WITH NOCHECK ADD  CONSTRAINT [FK_dbo.BiddingActions$UserId] FOREIGN KEY([BidderId])
REFERENCES [dbo].[Users] ([UserId])
GO

ALTER TABLE [dbo].[BiddingActions] CHECK CONSTRAINT [FK_dbo.BiddingActions$UserId]
GO
