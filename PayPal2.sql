USE [PayPal]
GO
/****** Object:  User [PayPal]    Script Date: 2018-04-06 09:56:20 ******/
CREATE USER [PayPal] FOR LOGIN [PayPal] WITH DEFAULT_SCHEMA=[dbo]
GO
ALTER ROLE [db_owner] ADD MEMBER [PayPal]
GO
/****** Object:  Table [dbo].[Project]    Script Date: 2018-04-06 09:56:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Project](
	[ProjectId] [bigint] NOT NULL,
	[ProjectName] [nvarchar](150) NOT NULL,
	[SourceLanguage] [varchar](10) NOT NULL,
	[TargetLanguage] [varchar](10) NOT NULL,
	[WordCountIce] [float] NULL,
	[WordCountExact] [float] NULL,
	[WordCount99_80] [float] NULL,
	[WordCount79_70] [float] NULL,
	[WordCount69] [float] NULL,
	[Repetition] [float] NULL,
	[CreationDate] [datetime] NULL,
	[DueDate] [datetime] NULL,
	[Url] [varchar](255) NOT NULL,
	[ProjectTypeId] [int] NOT NULL,
	[StatusId] [int] NOT NULL,
 CONSTRAINT [PK_tblProject] PRIMARY KEY CLUSTERED 
(
	[ProjectId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProjectType]    Script Date: 2018-04-06 09:56:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProjectType](
	[ProjectTypeId] [int] IDENTITY(1,1) NOT NULL,
	[ProjectTypeName] [varchar](150) NOT NULL,
 CONSTRAINT [PK_tblProjectType_1] PRIMARY KEY CLUSTERED 
(
	[ProjectTypeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Status]    Script Date: 2018-04-06 09:56:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Status](
	[StatusId] [int] IDENTITY(1,1) NOT NULL,
	[StatusName] [varchar](50) NOT NULL,
 CONSTRAINT [PK_tblStatus] PRIMARY KEY CLUSTERED 
(
	[StatusId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Token]    Script Date: 2018-04-06 09:56:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Token](
	[TokenId] [int] IDENTITY(1,1) NOT NULL,
	[Token] [nvarchar](250) NOT NULL,
	[RefreshToken] [nvarchar](250) NOT NULL,
	[UserId] [int] NOT NULL,
	[CreatedOn] [datetime] NOT NULL,
	[ExpireOn] [datetime] NOT NULL,
 CONSTRAINT [PK_tblToken] PRIMARY KEY CLUSTERED 
(
	[TokenId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[User]    Script Date: 2018-04-06 09:56:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[UserId] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](100) NULL,
	[Login] [nvarchar](50) NULL,
	[Password] [nvarchar](100) NULL,
 CONSTRAINT [PK_tblUser] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Vendor]    Script Date: 2018-04-06 09:56:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Vendor](
	[VendorId] [int] IDENTITY(1,1) NOT NULL,
	[Email] [varchar](150) NOT NULL,
 CONSTRAINT [PK_tblVendor] PRIMARY KEY CLUSTERED 
(
	[VendorId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vwProject]    Script Date: 2018-04-06 09:56:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO





CREATE VIEW [dbo].[vwProject]
AS
SELECT
	p.ProjectId
   ,p.ProjectName
   ,p.SourceLanguage
   ,p.TargetLanguage
   ,p.WordCountIce
   ,p.WordCountExact
   ,p.WordCount99_80
   ,p.WordCount79_70
   ,p.WordCount69
   ,p.CreationDate
   ,p.DueDate
   ,p.Url
   ,pt.ProjectTypeName
   ,s.StatusName
   ,p.Repetition
FROM dbo.Project AS p
INNER JOIN dbo.ProjectType AS pt
	ON p.ProjectTypeId = pt.ProjectTypeId
INNER JOIN dbo.Status AS s
	ON p.StatusId = s.StatusId
GO
/****** Object:  View [dbo].[vwToken]    Script Date: 2018-04-06 09:56:20 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE VIEW [dbo].[vwToken]
AS
SELECT
	CASE
		WHEN DATEDIFF(mi, [ExpireOn], getdate()) <= 0 THEN 0
		WHEN DATEDIFF(mi, [ExpireOn], getdate()) >= 0 THEN 1
	END AS IsExpired
   ,TokenId
   ,Token
   ,RefreshToken
   ,UserId
   ,CreatedOn
   ,ExpireOn
FROM dbo.Token
GO
ALTER TABLE [dbo].[Project] ADD  CONSTRAINT [DF_tblProject_WordCount]  DEFAULT ((0)) FOR [WordCountIce]
GO
ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_tblToken_CreatedOn]  DEFAULT (getdate()) FOR [CreatedOn]
GO
ALTER TABLE [dbo].[Token] ADD  CONSTRAINT [DF_tblToken_ExpireOn]  DEFAULT (getdate()+(1.0)/(24.0)) FOR [ExpireOn]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_tblProject_tblProjectType] FOREIGN KEY([ProjectTypeId])
REFERENCES [dbo].[ProjectType] ([ProjectTypeId])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_tblProject_tblProjectType]
GO
ALTER TABLE [dbo].[Project]  WITH CHECK ADD  CONSTRAINT [FK_tblProject_tblStatus] FOREIGN KEY([StatusId])
REFERENCES [dbo].[Status] ([StatusId])
GO
ALTER TABLE [dbo].[Project] CHECK CONSTRAINT [FK_tblProject_tblStatus]
GO
ALTER TABLE [dbo].[Token]  WITH CHECK ADD  CONSTRAINT [FK_tblToken_tblUser] FOREIGN KEY([UserId])
REFERENCES [dbo].[User] ([UserId])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Token] CHECK CONSTRAINT [FK_tblToken_tblUser]
GO

