SET ANSI_NULLS ON
SET QUOTED_IDENTIFIER ON

CREATE TABLE [dbo].[Expense](
    	[Id] [uniqueidentifier] NOT NULL,
    	[UserName] [nvarchar](1024) NOT NULL,
    	[Title] [nvarchar](30) NOT NULL,
    	[Description] [nvarchar](100) NOT NULL,
    	[Amount] [money] NOT NULL,
    	[Date] [datetime] NOT NULL,
    	[Approved] [bit] NOT NULL,
    	[CostCenter] [nvarchar](50) NOT NULL,
    	[ReimbursementMethod] [nvarchar](50) NOT NULL,
    	[Approver] [nvarchar](1024) NOT NULL
     CONSTRAINT [PK_Expense] PRIMARY KEY CLUSTERED 
    (
    	[Id] ASC
    )WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)

ALTER TABLE [Expense] ADD  CONSTRAINT [DF_Expense_Id]  DEFAULT (newid()) FOR [Id]
ALTER TABLE [Expense] ADD  CONSTRAINT [DF_Expense_Approved]  DEFAULT ((0)) FOR [Approved]

	CREATE TABLE [dbo].[ExpenseDetail](
	[Id] [uniqueidentifier] NOT NULL,
	[Description] [nvarchar](1024) NOT NULL,
	[Amount] [money] NOT NULL,
	[ExpenseId] [uniqueidentifier] NOT NULL,
	[ReceiptThumbnailUrl] [nvarchar](max) NULL,
	[ReceiptUrl] [nvarchar](max) NULL,
 CONSTRAINT [PK_ExpenseDetail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF)
)

ALTER TABLE [dbo].[ExpenseDetail] ADD  CONSTRAINT [DF_ExpenseDetail_Id]  DEFAULT (newid()) FOR [Id]

ALTER TABLE [dbo].[ExpenseDetail]  WITH CHECK ADD  CONSTRAINT [FK_ExpenseDetail_Expense] FOREIGN KEY([ExpenseId])
REFERENCES [dbo].[Expense] ([Id])

ALTER TABLE [dbo].[ExpenseDetail] CHECK CONSTRAINT [FK_ExpenseDetail_Expense]


    INSERT INTO [dbo].[Expense] ([Id], [UserName], [Title], [Description], [Amount], [Date], [Approved], [CostCenter], [ReimbursementMethod], [Approver]) 
	VALUES 
		(N'abafc874-d0cc-4245-9319-1e5a75108a41', N'ADATUM\mary', N'Dinner', N'Dinner with new employee', 125.2500, '2009-08-25', 0, N'CC-12345-DEV', N'Check', N'ADATUM\bob')	

    INSERT INTO [dbo].[Expense] 
		([Id], [UserName], [Title], [Description], [Amount], [Date], [Approved], [CostCenter], [ReimbursementMethod], [Approver]) 
	VALUES 
		(N'abafc874-d0cc-4245-9319-1e5c77158b42', N'ADATUM\johndoe', N'Dinner', N'Dinner with customer', 200.5000, '2009-08-25', 0, N'CC-12345-DEV', N'Check', N'ADATUM\mary')
		
    INSERT INTO [dbo].[Expense] 
		([Id], [UserName], [Title], [Description], [Amount], [Date], [Approved], [CostCenter], [ReimbursementMethod], [Approver]) 
	VALUES 
		(N'abafc874-a0cc-4145-9319-1e5c78508a41', N'ADATUM\johndoe', N'Breakfast', N'Breakfast with the team', 75.9000, '2009-08-23', 1, N'CC-12345-DEV', N'Check', N'ADATUM\mary')


CREATE TABLE [dbo].[ProfileStore]
(
    [Id] INT NOT NULL PRIMARY KEY, 
    [User] NVARCHAR(1024) NOT NULL, 
    [CostCenter] NVARCHAR(50) NOT NULL, 
    [Manager] NVARCHAR(1024) NOT NULL, 
    [DisplayName] NVARCHAR(1024) NOT NULL
)
GO

INSERT INTO [dbo].[ProfileStore] ([Id], [User], [CostCenter], [Manager], [DisplayName]) 
	VALUES (1, N'ADATUM\johndoe', N'31023', N'ADATUM\mary', N'John Doe')

INSERT INTO [dbo].[ProfileStore] ([Id], [User], [CostCenter], [Manager], [DisplayName]) 
	VALUES (2, N'ADATUM\mary', N'92452', N'ADATUM\bob', N'Mary May')

INSERT INTO [dbo].[ProfileStore] ([Id], [User], [CostCenter], [Manager], [DisplayName]) 
	VALUES (3, N'ADATUM\bob', N'92452', N'ADATUM\bob', N'Robert Tor')
GO

CREATE PROCEDURE [dbo].[aexpense_ProfileStore_GetProfileFromUser] 
	@UserName	nvarchar(256)

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT CostCenter, Manager, DisplayName FROM [dbo].ProfileStore WHERE [User] = @UserName
END

GO