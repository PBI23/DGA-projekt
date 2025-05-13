-- Drop Database if exist and not in use
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'DGA_ProductDB')
BEGIN
    ALTER DATABASE DGA_ProductDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DGA_ProductDB;
END
GO


-- Create new Database
CREATE DATABASE DGA_ProductDB;
GO

-- Use Database
USE DGA_ProductDB;
GO


------------ TABLES ------------


----- LOOK UP TABLES -----
CREATE TABLE Country (
    CountryID INT PRIMARY KEY,
    CountryCode CHAR(2) NOT NULL,
    CountryName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE ColorGroup (
    ColorGroupID INT PRIMARY KEY,
    ColorGroupName VARCHAR(100)
);
GO 

CREATE TABLE Pantone (
    PantoneID INT PRIMARY KEY,
    ColorCode VARCHAR(50) NOT NULL,
    ColorName VARCHAR(50) 
);
GO

CREATE TABLE Certification (
    CertificationID INT PRIMARY KEY,
    CertificationType NVARCHAR(50) NOT NULL, 
    CertificationDescription NVARCHAR(255)
);
GO

----- ENTITY TABLES -----
CREATE TABLE Designer (
    DesignerID INT PRIMARY KEY,
    DesignerName NVARCHAR(100) NOT NULL
);
GO

___________________

CREATE TABLE dbo.Country (
    CountryID int NOT NULL,
    CountryCode char(2) NOT NULL,
    CountryName nvarchar(100) NOT NULL
);

CREATE TABLE dbo.ColorGroup (
    ColorGroupID int NOT NULL,
    ColorGroupName varchar(100) NULL
);

CREATE TABLE dbo.Pantone (
    PantoneID int NOT NULL,
    ColorCode varchar(50) NOT NULL,
    ColorName varchar(50) NULL
);

CREATE TABLE dbo.Certification (
    CertificationID int NOT NULL,
    CertificationType nvarchar(50) NOT NULL,
    CertificationDescription nvarchar(255) NULL
);

CREATE TABLE dbo.Designer (
    DesignerID int NOT NULL,
    DesignerName nvarchar(100) NOT NULL
);

CREATE TABLE dbo.Supplier (
    SupplierID int NOT NULL,
    ProductID int NULL,
    GroupID int NULL,
    SupplierNo int NULL,
    Name varchar(200) NULL
);

CREATE TABLE dbo.Group (
    GroupID int NOT NULL,
    GroupDescription varchar(250) NULL,
    SupplierID int NULL
);

CREATE TABLE dbo.ProductCategory (
    ProductID int NOT NULL,
    MainGroup varchar(100) NULL,
    MainCategory varchar(100) NULL,
    SubCategory varchar(100) NULL
);

CREATE TABLE dbo.Picture (
    PictureID int NOT NULL,
    ProductID int NULL,
    Name varchar(100) NULL,
    DataType varchar(100) NULL
);

CREATE TABLE dbo.ProductDimensions (
    ProductDID int NOT NULL,
    ProductID int NULL,
    HeightCM decimal(18,0) NULL,
    WidthCM decimal(18,0) NULL,
    DepthCM decimal(18,0) NULL,
    DiameterCM decimal(18,0) NULL,
    NetWeightKG decimal(18,0) NULL,
    GsmWeight decimal(18,0) NULL
);

CREATE TABLE dbo.ProductPackageDimensions (
    ProductPDID int NOT NULL,
    ProductID int NULL,
    GrossWeightKG decimal(18,0) NULL,
    CBM decimal(18,0) NULL,
    PackagingHeightCM decimal(18,0) NULL,
    PackagingWidthCM decimal(18,0) NULL,
    PackagingDepthCM decimal(18,0) NULL,
    KIHeightCM decimal(18,0) NULL,
    KIWidthCM decimal(18,0) NULL,
    KIDepthCM decimal(18,0) NULL,
    KYHeightCM decimal(18,0) NULL,
    KYWidthCM decimal(18,0) NULL,
    KYDepthCM decimal(18,0) NULL,
    PackingDepthCM decimal(18,0) NULL,
    InnerCarton int NULL,
    OuterCarton int NULL
);

CREATE TABLE dbo.ProductCertification (
    ProductID int NOT NULL,
    CertificateID int NOT NULL,
    ValidUntil date NULL
);

CREATE TABLE dbo.ProductPantone (
    ProductID int NOT NULL,
    PantoneID int NOT NULL
);

CREATE TABLE dbo.Product (
    ProductID int NOT NULL,
    SupplierID int NULL,
    CountryID int NULL,
    DesignerID int NULL,
    ColorGroupID int NULL,
    ModifiedDate date NULL,
    SetupStage date NULL,
    HasBeenApproved bit NULL
);

CREATE TABLE dbo.ProductDetails (
    ProductID int NOT NULL,
    DGAItemNo varchar(50) NULL,
    ProductLogo varchar(50) NULL,
    Series varchar(100) NULL,
    ProductDescription text NULL,
    ProductDescriptionFree text NULL,
    MOQ int NULL,
    CostPrice decimal(10,2) NULL,
    CustomerClearanceNo int NULL,
    CustomerClearencePercent decimal(5,2) NULL,
    Unit varchar(10) NULL,
    UnitPCS int NULL,
    ABC char(1) NULL,
    Assorted varchar(20) NULL,
    EANCodes varchar(13) NULL,
    PictureGroup int NULL,
    HangtagsAndStickers varchar(100) NULL,
    SupplierProductNo varchar(50) NULL,
    BurningTimeHours decimal(5,2) NULL,
    Material varchar(100) NULL,
    AdditionalInfo text NULL
);
