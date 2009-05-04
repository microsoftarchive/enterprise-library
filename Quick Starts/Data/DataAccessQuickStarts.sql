-- ===============================================
-- 
-- Configure Data Access Quick Starts EntLibQuickStarts Database
-- 
-- ===============================================

use master
go

--IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'EntLibQuickStarts')
--	DROP DATABASE [EntLibQuickStarts]
--GO

IF NOT (EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'EntLibQuickStarts'))
	CREATE DATABASE EntLibQuickStarts COLLATE SQL_Latin1_General_CP1_CI_AS
GO

exec sp_dboption N'EntLibQuickStarts', N'autoclose', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'bulkcopy', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'trunc. log', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'torn page detection', N'true'
GO

exec sp_dboption N'EntLibQuickStarts', N'read only', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'dbo use', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'single', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'autoshrink', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'ANSI null default', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'recursive triggers', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'ANSI nulls', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'concat null yields null', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'cursor close on commit', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'default to local cursor', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'quoted identifier', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'ANSI warnings', N'false'
GO

exec sp_dboption N'EntLibQuickStarts', N'auto create statistics', N'true'
GO

exec sp_dboption N'EntLibQuickStarts', N'auto update statistics', N'true'
GO

-- =======================================================
-- Delete existing tables
-- =======================================================

use [EntLibQuickStarts]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Customers]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Customers]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Products]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Products]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Credits]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Credits]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[Debits]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
drop table [dbo].[Debits]
GO

-- =======================================================
-- Create new tables
-- =======================================================

CREATE TABLE [dbo].[Customers] (
	[CustomerID] [int] IDENTITY (1, 1) NOT NULL 
		CONSTRAINT PKCustomerID PRIMARY KEY,
	[Name] [nvarchar] (30)  NULL, 
	[Address] [nvarchar] (60)  NULL,	
	[City] [nvarchar] (15)  NULL,
	[Country] [nvarchar] (15)  NULL,
	[PostalCode] [nvarchar] (10)  NULL
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Products] (
	[ProductID] [int] IDENTITY (1, 1) NOT NULL
		CONSTRAINT PKProductID PRIMARY KEY ,
	[ProductName] [nvarchar] (50)  NOT NULL ,
	[CategoryID] [int] , 
	[UnitPrice] [money] NOT NULL ,
	[LastUpdate] [datetime]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Credits] (
	[CreditID] [int] IDENTITY (1, 1) NOT NULL,
	[AccountID] [nchar] (20)  NOT NULL ,
	[Amount] [money] NOT NULL 
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[Debits] (
	[DebitID] [int] IDENTITY (1, 1) NOT NULL,
	[AccountID] [nchar] (20)  NOT NULL ,
	[Amount] [money] NOT NULL 
) ON [PRIMARY]
GO

-- =======================================================
-- Delete existing stored procedures
-- =======================================================

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetProductDetails]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetProductDetails]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetProductsByCategory]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetProductsByCategory]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[GetProductName]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[GetProductName]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DebitAccount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DebitAccount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[CreditAccount]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[CreditAccount]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[AddProduct]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[AddProduct]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[DeleteProduct]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[DeleteProduct]
GO

if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[UpdateProduct]') and OBJECTPROPERTY(id, N'IsProcedure') = 1)
drop procedure [dbo].[UpdateProduct]
GO


-- =======================================================
-- Create triggers
-- =======================================================

CREATE TRIGGER trInsert ON dbo.Products
FOR INSERT
AS

Update 
  Products
Set
  LastUpdate = GetDate()
From
  Products p Inner Join Inserted i On p.ProductID = i.ProductID
Where
  p.ProductID = i.ProductID

GO

CREATE TRIGGER trUpdate ON dbo.Products
FOR UPDATE
AS

Update 
  Products
Set
  LastUpdate = GetDate()
From
  Products p Inner Join Inserted i On p.ProductID = i.ProductID
Where
  p.ProductID = i.ProductID

GO

-- =======================================================
-- Create stored procedures
-- =======================================================

--/////////////////////////////////////////////////////////////////////

CREATE Procedure GetProductDetails
(
    @ProductID		int ,
    @ProductName	nvarchar(50) OUTPUT,
    @UnitPrice     	money OUTPUT 
)
AS

SELECT 
    @ProductName = ProductName,
    @UnitPrice = UnitPrice
FROM 
    Products
WHERE 
    ProductID = @ProductID
    
RETURN 0    
GO

--/////////////////////////////////////////////////////////////////////
CREATE Procedure GetProductsByCategory
(
    @CategoryID		int 
)
AS

SELECT 
    ProductID, ProductName, CategoryID, UnitPrice, LastUpdate
FROM 
    Products
WHERE 
    CategoryID = @CategoryID
    
RETURN 0    
GO

--/////////////////////////////////////////////////////////////////////

CREATE Procedure GetProductName
(
    @ProductID		int 
)
AS

SELECT 
    ProductName
FROM 
    Products
WHERE 
    ProductID = @ProductID

GO

--/////////////////////////////////////////////////////////////////////

CREATE Procedure CreditAccount
(
    @AccountID	int,
    @Amount		money
)
AS

INSERT INTO 
    Credits ([AccountID],[Amount])
VALUES
    (@AccountID,@Amount)

GO

--/////////////////////////////////////////////////////////////////////

CREATE Procedure DebitAccount
(
    @AccountID	int,
    @Amount	money
)
AS

INSERT INTO 
    Debits ([AccountID],[Amount])
VALUES
    (@AccountID,@Amount)

GO

--/////////////////////////////////////////////////////////////////////

CREATE PROCEDURE AddProduct
(
    @ProductName nvarchar(50),
    @CategoryID int,
    @UnitPrice money
)
AS

INSERT INTO  
  Products (ProductName, CategoryID, UnitPrice)
VALUES 
  (@ProductName, @CategoryID, @UnitPrice)

SELECT 
    ProductID, ProductName, CategoryID, UnitPrice
FROM 
    Products
WHERE 
    ProductID = SCOPE_IDENTITY()
GO

--/////////////////////////////////////////////////////////////////////

CREATE PROCEDURE UpdateProduct 
(
    @ProductID int,
    @ProductName nvarchar(50),
    @LastUpdate datetime
)
AS

UPDATE
  Products 
SET
  ProductName = @ProductName
WHERE
  ProductID = @ProductID AND
  LastUpdate = @LastUpdate
 
IF @@ROWCOUNT > 0
  -- This statement is used to update the DataSet if changes are done on the updated record (identities, timestamps or triggers )
  SELECT 
	ProductID, ProductName, CategoryID, UnitPrice
  FROM 
	Products
  WHERE 
	ProductID = @ProductID
GO

--/////////////////////////////////////////////////////////////////////

CREATE PROCEDURE DeleteProduct 
(
	@ProductID int
)
AS

DELETE 
	Products 
WHERE 
	ProductID = @ProductID
GO 


-- =======================================================
-- Populate database with intial data
-- =======================================================

INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Maria Anders','Obere Str. 57','Berlin','12209','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Ana Trujillo','Avda. de la Constitución 2222','México D.F.','05021','Mexico')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Antonio Moreno','Mataderos  2312','México D.F.','05023','Mexico')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Thomas Hardy','120 Hanover Sq.','London','WA1 1DP','UK')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Christina Berglund','Berguvsvägen  8','Luleå','S-958 22','Sweden')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Hanna Moos','Forsterstr. 57','Mannheim','68306','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Frédérique Citeaux','24, place Kléber','Strasbourg','67000','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Martín Sommer','C/ Araquil, 67','Madrid','28023','Spain')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Laurence Lebihan','12, rue des Bouchers','Marseille','13008','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Elizabeth Lincoln','23 Tsawassen Blvd.','Tsawassen','T2F 8M4','Canada')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Victoria Ashworth','Fauntleroy Circus','London','EC2 5NT','UK')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Patricio Simpson','Cerrito 333','Buenos Aires','1010','Argentina')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Francisco Chang','Sierras de Granada 9993','México D.F.','05022','Mexico')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Yang Wang','Hauptstr. 29','Bern','3012','Switzerland')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Pedro Afonso','Av. dos Lusíadas, 23','Sao Paulo','05432-043','Brazil')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Elizabeth Brown','Berkeley Gardens 12  Brewery','London','WX1 6LT','UK')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Sven Ottlieb','Walserweg 21','Aachen','52066','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Janine Labrune','67, rue des Cinquante Otages','Nantes','44000','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Ann Devon','35 King George','London','WX3 6FW','UK')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Roland Mendel','Kirchgasse 6','Graz','8010','Austria')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Aria Cruz','Rua Orós, 92','Sao Paulo','05442-030','Brazil')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Diego Roel','C/ Moralzarzal, 86','Madrid','28034','Spain')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Martine Rancé','184, chaussée de Tournai','Lille','59000','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Maria Larsson','Åkergatan 24','Bräcke','S-844 67','Sweden')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Peter Franken','Berliner Platz 43','München','80805','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Carine Schmitt','54, rue Royale','Nantes','44000','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Paolo Accorti','Via Monte Bianco 34','Torino','10100','Italy')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Lino Rodriguez','Jardim das rosas n. 32','Lisboa','1675','Portugal')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Eduardo Saavedra','Rambla de Cataluña, 23','Barcelona','08022','Spain')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('José Pedro Freyre','C/ Romero, 33','Sevilla','41101','Spain')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('André Fonseca','Av. Brasil, 442','Campinas','04876-786','Brazil')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Howard Snyder','2732 Baker Blvd.','Eugene','97403','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Manuel Pereira','5ª Ave. Los Palos Grandes','Caracas','1081','Venezuela')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Mario Pontes','Rua do Paço, 67','Rio de Janeiro','05454-876','Brazil')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Carlos Hernández','Carrera 22 con Ave. Carlos Soublette #8-35','San Cristóbal','5022','Venezuela')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Yoshi Latimer','City Center Plaza 516 Main St.','Elgin','97827','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Patricia McKenna','8 Johnstown Road','Cork',NULL,'Ireland')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Helen Bennett','Garden House Crowther Way','Cowes','PO31 7PJ','UK')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Philip Cramer','Maubelstr. 90','Brandenburg','14776','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Daniel Tonini','67, avenue de l''Europe','Versailles','78000','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Annette Roulet','1 rue Alsace-Lorraine','Toulouse','31000','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Yoshi Tannamuri','1900 Oak St.','Vancouver','V3F 2K1','Canada')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('John Steel','12 Orchestra Terrace','Walla Walla','99362','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Renate Messner','Magazinweg 7','Frankfurt a.M.','60528','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Jaime Yorres','87 Polk St. Suite 5','San Francisco','94117','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Carlos González','Carrera 52 con Ave. Bolívar #65-98 Llano Largo','Barquisimeto','3508','Venezuela')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Felipe Izquierdo','Ave. 5 de Mayo Porlamar','I. de Margarita','4980','Venezuela')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Fran Wilson','89 Chiaroscuro Rd.','Portland','97219','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Giovanni Rovelli','Via Ludovico il Moro 22','Bergamo','24100','Italy')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Catherine Dewey','Rue Joseph-Bens 532','Bruxelles','B-1180','Belgium')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Jean Fresnière','43 rue St. Laurent','Montréal','H1J 1C3','Canada')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Alexander Feuer','Heerstr. 22','Leipzig','04179','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Simon Crowther','South House 300 Queensbridge','London','SW7 1RZ','UK')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Yvonne Moncada','Ing. Gustavo Moncada 8585 Piso 20-A','Buenos Aires','1010','Argentina')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Rene Phillips','2743 Bering St.','Anchorage','99508','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Henriette Pfalzheim','Mehrheimerstr. 369','Köln','50739','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Marie Bertrand','265, boulevard Charonne','Paris','75012','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Guillermo Fernández','Calle Dr. Jorge Cash 321','México D.F.','05033','Mexico')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Georg Pipps','Geislweg 14','Salzburg','5020','Austria')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Isabel de Castro','Estrada da saúde n. 58','Lisboa','1756','Portugal')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Bernardo Batista','Rua da Panificadora, 12','Rio de Janeiro','02389-673','Brazil')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Lúcia Carvalho','Alameda dos Canàrios, 891','Sao Paulo','05487-020','Brazil')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Horst Kloss','Taucherstraße 10','Cunewalde','01307','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Sergio Gutiérrez','Av. del Libertador 900','Buenos Aires','1010','Argentina')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Paula Wilson','2817 Milton Dr.','Albuquerque','87110','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Maurizio Moroni','Strada Provinciale 124','Reggio Emilia','42100','Italy')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Janete Limeira','Av. Copacabana, 267','Rio de Janeiro','02389-890','Brazil')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Michael Holz','Grenzacherweg 237','Genève','1203','Switzerland')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Alejandra Camino','Gran Vía, 1','Madrid','28001','Spain')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Jonas Bergulfsen','Erling Skakkes gate 78','Stavern','4110','Norway')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Jose Pavarotti','187 Suffolk Ln.','Boise','83720','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Hari Kumar','90 Wadhurst Rd.','London','OX15 4NB','UK')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Jytte Petersen','Vinbæltet 34','Kobenhavn','1734','Denmark')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Dominique Perrier','25, rue Lauriston','Paris','75016','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Art Braunschweiger','P.O. Box 555','Lander','82520','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Pascale Cartrain','Boulevard Tirou, 255','Charleroi','B-6000','Belgium')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Liz Nixon','89 Jefferson Way Suite 2','Portland','97201','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Liu Wong','55 Grizzly Peak Rd.','Butte','59801','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Karin Josephs','Luisenstr. 48','Münster','44087','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Miguel Angel Paolino','Avda. Azteca 123','México D.F.','05033','Mexico')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Anabela Domingues','Av. Inês de Castro, 414','Sao Paulo','05634-030','Brazil')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Helvetius Nagy','722 DaVinci Blvd.','Kirkland','98034','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Palle Ibsen','Smagsloget 45','Århus','8200','Denmark')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Mary Saveley','2, rue du Commerce','Lyon','69004','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Paul Henriot','59 rue de l''Abbaye','Reims','51100','France')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Rita Müller','Adenauerallee 900','Stuttgart','70563','Germany')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Pirkko Koskitalo','Torikatu 38','Oulu','90110','Finland')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Paula Parente','Rua do Mercado, 12','Resende','08737-363','Brazil')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Karl Jablonski','305 - 14th Ave. S. Suite 3B','Seattle','98128','USA')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Matti Karttunen','Keskuskatu 45','Helsinki','21240','Finland')
INSERT INTO [Customers] ([Name],[Address],[City],[PostalCode],[Country])VALUES('Zbyszek Piestrzeniewicz','ul. Filtrowa 68','Warszawa','01-012','Poland')

--/////////////////////////////////////////////////////////////////////

INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Chai',1,18.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Chang',1,19.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Aniseed Syrup',2,10.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Chef Anton''s Cajun Seasoning',2,22.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Chef Anton''s Gumbo Mix',2,21.3500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Grandma''s Boysenberry Spread',2,25.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Uncle Bob''s Organic Dried Pears',7,30.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Northwoods Cranberry Sauce',2,40.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Mishi Kobe Niku',6,97.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Ikura',8,31.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Queso Cabrales',4,21.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Queso Manchego La Pastora',4,38.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Konbu',8,6.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Tofu',7,23.2500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Genen Shouyu',2,15.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Pavlova',3,17.4500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Alice Mutton',6,39.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Carnarvon Tigers',8,62.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Teatime Chocolate Biscuits',3,9.2000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Sir Rodney''s Marmalade',3,81.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Sir Rodney''s Scones',3,10.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Gustaf''s Knäckebröd',5,21.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Tunnbröd',5,9.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Guaraná Fantástica',1,4.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('NuNuCa Nuß-Nougat-Creme',3,14.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Gumbär Gummibärchen',3,31.2300)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Schoggi Schokolade',3,43.9000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Rössle Sauerkraut',7,45.6000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Thüringer Rostbratwurst',6,123.7900)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Nord-Ost Matjeshering',8,25.8900)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Gorgonzola Telino',4,12.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Mascarpone Fabioli',4,32.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Geitost',4,2.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Sasquatch Ale',1,14.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Steeleye Stout',1,18.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Inlagd Sill',8,19.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Gravad lax',8,26.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Côte de Blaye',1,263.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Chartreuse verte',1,18.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Boston Crab Meat',8,18.4000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Jack''s New England Clam Chowder',8,9.6500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Singaporean Hokkien Fried Mee',5,14.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Ipoh Coffee',1,46.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Gula Malacca',2,19.4500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Rogede sild',8,9.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Spegesild',8,12.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Zaanse koeken',3,9.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Chocolade',3,12.7500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Maxilaku',3,20.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Valkoinen suklaa',3,16.2500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Manjimup Dried Apples',7,53.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Filo Mix',5,7.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Perth Pasties',6,32.8000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Tourtière',6,7.4500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Pâté chinois',6,24.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Gnocchi di nonna Alice',5,38.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Ravioli Angelo',5,19.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Escargots de Bourgogne',8,13.2500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Raclette Courdavault',4,55.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Camembert Pierrot',4,34.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Sirop d''érable',2,28.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Tarte au sucre',3,49.3000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Vegie-spread',2,43.9000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Wimmers gute Semmelknödel',5,33.2500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Louisiana Fiery Hot Pepper Sauce',2,21.0500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Louisiana Hot Spiced Okra',2,17.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Laughing Lumberjack Lager',1,14.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Scottish Longbreads',3,12.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Gudbrandsdalsost',4,36.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Outback Lager',1,15.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Flotemysost',4,21.5000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Mozzarella di Giovanni',4,34.8000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Röd Kaviar',8,15.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Longlife Tofu',7,10.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Rhönbräu Klosterbier',1,7.7500)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Lakkalikööri',1,18.0000)
INSERT INTO [Products] ([ProductName],[CategoryID],[UnitPrice])VALUES('Original Frankfurter grüne Soße',2,13.0000)
