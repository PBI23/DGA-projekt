-- Drop Database if exists and not in use
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

-- ========================
-- LOOKUP-TABELLER
-- ========================
CREATE TABLE Country (
    CountryID INT IDENTITY(1,1) PRIMARY KEY,
    CountryCode CHAR(2) NOT NULL,
    CountryName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE ColorGroup (
    ColorGroupID INT IDENTITY(1,1) PRIMARY KEY,
    ColorGroupName NVARCHAR(100)
);
GO 

CREATE TABLE Pantone (
    PantoneID INT IDENTITY(1,1) PRIMARY KEY,
    ColorCode NVARCHAR(50) NOT NULL,
    ColorName NVARCHAR(50)
);
GO

CREATE TABLE Certification (
    CertificationID INT IDENTITY(1,1) PRIMARY KEY,
    CertificationType NVARCHAR(100) NOT NULL,
    Description VARCHAR(MAX)
);
GO

-- ========================
-- ENTITETSTABELLER
-- ========================
CREATE TABLE Designer (
    DesignerID INT IDENTITY(1,1) PRIMARY KEY,
    DesignerName NVARCHAR(100) NOT NULL
);
GO

CREATE TABLE [Group] (
    GroupID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierID INT,
    GroupDescription NVARCHAR(250)
);
GO

CREATE TABLE Supplier (
    SupplierID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT,
    GroupID INT,
    SupplierNo INT,
    Name NVARCHAR(200)
);
GO

CREATE TABLE Product (
    ProductID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierID INT,
    CountryID INT,
    DesignerID INT,
    ColorGroupID INT,
    CreatedDate DATE,
    ModifiedDate DATE,
    SetupStage DATE,
    HasBeenApproved BIT
    CurrentStep INT,

);
GO

CREATE TABLE ProductCategory (
    ProductID INT PRIMARY KEY,
    MainGroup NVARCHAR(100),
    MainCategory NVARCHAR(100),
    SubCategory NVARCHAR(100)
);
GO

CREATE TABLE ProductDetails (
    ProductID INT PRIMARY KEY,
    DGAItemNo NVARCHAR(50),
    ProductLogo NVARCHAR(50),
    Series NVARCHAR(100),
    ProductDescription VARCHAR(MAX),
    ProductDescriptionFree VARCHAR(MAX),
    MOQ INT,
    CostPrice DECIMAL(10,2),
    CustomerClearanceNo INT,
    CustomerClearencePercent DECIMAL(5,2),
    Unit NVARCHAR(10),
    UnitPCS INT,
    ABC CHAR(1),
    Assorted NVARCHAR(20),
    EANCodes NVARCHAR(13),
    PictureGroup INT,
    HangtagsAndStickers NVARCHAR(100),
    SupplierProductNo NVARCHAR(50),
    BurningTimeHours DECIMAL(5,2),
    Material NVARCHAR(100),
    AdditionalInfo VARCHAR(MAX)
);
GO

CREATE TABLE Picture (
    PictureID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT,
    Data VARBINARY(MAX),
    DataType NVARCHAR(100)
);
GO

CREATE TABLE ProductDimensions (
    ProductDID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT,
    HeightCM DECIMAL(10,2),
    WidthCM DECIMAL(10,2),
    DepthCM DECIMAL(10,2),
    DiameterCM DECIMAL(10,2),
    NetWeightKG DECIMAL(10,2),
    GsmWeight DECIMAL(10,2)
);
GO

CREATE TABLE ProductPackageDimensions (
    ProductPDID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT,
    GrossWeightKG DECIMAL(10,2),
    CBM DECIMAL(10,2),
    PackagingHeightCM DECIMAL(10,2),
    PackagingWidthCM DECIMAL(10,2),
    PackagingDepthCM DECIMAL(10,2),
    KIHeightCM DECIMAL(10,2),
    KIWidthCM DECIMAL(10,2),
    KIDepthCM DECIMAL(10,2),
    KYHeightCM DECIMAL(10,2),
    KYWidthCM DECIMAL(10,2),
    KYDepthCM DECIMAL(10,2),
    PackingDepthCM DECIMAL(10,2),
    InnerCarton INT,
    OuterCarton INT
);
GO

CREATE TABLE FoodContactMaterial (
    FCMID INT IDENTITY(1,1) PRIMARY KEY,
    ProductID INT NOT NULL,
    PlasticMaterial BIT,
    IonisingRadiation BIT,
    RecycledPlastic BIT,
    ActiveIntelligentMaterial BIT,
    FunctionalBarrier BIT,
    DualUse BIT,
    MultipleApplied BIT,
    FCMDeclarationProvided BIT,
    TestReportAvailable BIT,
    GMP_Certified BIT,
    EUFoodContactCompliant BIT
);
GO

-- ========================
-- RELATIONSTABELLER
-- ========================
CREATE TABLE ProductCertification (
    ProductID INT NOT NULL,
    CertificateID INT NOT NULL,
    ValidUntil DATE,
    PRIMARY KEY (ProductID, CertificateID)
);
GO

CREATE TABLE ProductPantone (
    ProductID INT NOT NULL,
    PantoneID INT NOT NULL,
    PRIMARY KEY (ProductID, PantoneID)
);
GO

-- ========================
-- FOREIGN KEYS
-- ========================

-- Product
ALTER TABLE Product
ADD CONSTRAINT FK_Product_Supplier FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
    CONSTRAINT FK_Product_Country FOREIGN KEY (CountryID) REFERENCES Country(CountryID),
    CONSTRAINT FK_Product_Designer FOREIGN KEY (DesignerID) REFERENCES Designer(DesignerID),
    CONSTRAINT FK_Product_ColorGroup FOREIGN KEY (ColorGroupID) REFERENCES ColorGroup(ColorGroupID);
GO

-- ProductDetails
ALTER TABLE ProductDetails
ADD CONSTRAINT FK_ProductDetails_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID);
GO

-- ProductCategory
ALTER TABLE ProductCategory
ADD CONSTRAINT FK_ProductCategory_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID);
GO

-- Picture
ALTER TABLE Picture
ADD CONSTRAINT FK_Picture_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID);
GO

-- ProductDimensions
ALTER TABLE ProductDimensions
ADD CONSTRAINT FK_ProductDimensions_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID);
GO

-- ProductPackageDimensions
ALTER TABLE ProductPackageDimensions
ADD CONSTRAINT FK_ProductPackageDimensions_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID);
GO

-- ProductCertification
ALTER TABLE ProductCertification
ADD CONSTRAINT FK_ProductCertification_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT FK_ProductCertification_Certification FOREIGN KEY (CertificateID) REFERENCES Certification(CertificationID);
GO

-- ProductPantone
ALTER TABLE ProductPantone
ADD CONSTRAINT FK_ProductPantone_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID),
    CONSTRAINT FK_ProductPantone_Pantone FOREIGN KEY (PantoneID) REFERENCES Pantone(PantoneID);
GO

-- FoodContactMaterial
ALTER TABLE FoodContactMaterial
ADD CONSTRAINT FK_FoodContactMaterial_Product FOREIGN KEY (ProductID) REFERENCES Product(ProductID);
GO

-- Supplier & Group relation
ALTER TABLE Supplier
ADD CONSTRAINT FK_Supplier_Group FOREIGN KEY (GroupID) REFERENCES [Group](GroupID);
GO

ALTER TABLE [Group]
ADD CONSTRAINT FK_Group_Supplier FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID);
GO


-- ========================
-- 🔢 TESTDATA (INSERTS)
-- ========================

-- Lookup: Country
INSERT INTO Country (CountryCode, CountryName) VALUES
('DK', 'Denmark'),
('DE', 'Germany'),
('CN', 'China');
GO

-- Lookup: ColorGroup
INSERT INTO ColorGroup (ColorGroupName) VALUES
('Neutral'),
('Warm'),
('Cool');
GO

-- Lookup: Pantone
INSERT INTO Pantone (ColorCode, ColorName) VALUES
('PMS 186 C', 'Red'),
('PMS 299 C', 'Blue'),
('PMS 375 C', 'Green');
GO

-- Lookup: Certification
INSERT INTO Certification (CertificationType, Description) VALUES
('Organic', 'Certified organic material'),
('FairTrade', 'Produced under fair trade standards');
GO

-- Lookup: Designer
INSERT INTO Designer (DesignerName) VALUES
('Anna Møller'),
('Jonas Lind');
GO

-- Entity: Supplier
INSERT INTO Supplier (SupplierNo, Name) VALUES
(1001, 'Nordic Supplies'),
(1002, 'Asia Imports');
GO

-- Entity: Product
INSERT INTO Product (SupplierID, CountryID, DesignerID, ColorGroupID, CreatedDate, ModifiedDate, SetupStage, HasBeenApproved)
VALUES
(1, 1, 1, 2, GETDATE(), GETDATE(), GETDATE(), 1),
(2, 3, 2, 1, GETDATE(), GETDATE(), GETDATE(), 0);
GO

-- Entity: ProductDetails
INSERT INTO ProductDetails (ProductID, DGAItemNo, ProductLogo, Series, ProductDescription, MOQ, CostPrice, Unit, UnitPCS)
VALUES
(1, 'DGA-001', 'Logo1', 'Cozy Line', 'A stylish red candle', 100, 25.50, 'pcs', 12),
(2, 'DGA-002', 'Logo2', 'Nature Touch', 'A green ceramic mug', 200, 15.00, 'pcs', 6);
GO

-- Entity: ProductCategory
INSERT INTO ProductCategory (ProductID, MainGroup, MainCategory, SubCategory)
VALUES
(1, 'Home', 'Candles', 'Scented'),
(2, 'Kitchen', 'Cups', 'Ceramic');
GO

-- Relation: ProductPantone
INSERT INTO ProductPantone (ProductID, PantoneID) VALUES
(1, 1),
(2, 3);
GO

-- Relation: ProductCertification
INSERT INTO ProductCertification (ProductID, CertificateID, ValidUntil) VALUES
(1, 1, '2026-12-31'),
(2, 2, '2025-06-30');
GO



-- ========================
-- STORED PROCEDURES
-- ========================

-- 1. Opret nyt produkt (med detaljer og kategorier)
CREATE PROCEDURE spCreateProduct
    @SupplierID INT,
    @CountryID INT,
    @DesignerID INT,
    @ColorGroupID INT,
    @DGAItemNo NVARCHAR(50),
    @ProductLogo NVARCHAR(50),
    @Series NVARCHAR(100),
    @ProductDescription NVARCHAR(MAX),
    @MOQ INT,
    @CostPrice DECIMAL(10,2),
    @Unit NVARCHAR(10),
    @UnitPCS INT,
    @MainGroup NVARCHAR(100),
    @MainCategory NVARCHAR(100),
    @SubCategory NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ProductID INT;

    INSERT INTO Product (SupplierID, CountryID, DesignerID, ColorGroupID, CreatedDate, ModifiedDate, SetupStage, HasBeenApproved)
    VALUES (@SupplierID, @CountryID, @DesignerID, @ColorGroupID, GETDATE(), GETDATE(), GETDATE(), 0);

    SET @ProductID = SCOPE_IDENTITY();

    INSERT INTO ProductDetails (ProductID, DGAItemNo, ProductLogo, Series, ProductDescription, MOQ, CostPrice, Unit, UnitPCS)
    VALUES (@ProductID, @DGAItemNo, @ProductLogo, @Series, @ProductDescription, @MOQ, @CostPrice, @Unit, @UnitPCS);

    INSERT INTO ProductCategory (ProductID, MainGroup, MainCategory, SubCategory)
    VALUES (@ProductID, @MainGroup, @MainCategory, @SubCategory);

    SELECT @ProductID AS NewProductID;
END
GO

-- 2. Opdater produktdetaljer
CREATE PROCEDURE spUpdateProductDetails
    @ProductID INT,
    @MOQ INT,
    @CostPrice DECIMAL(10,2),
    @ProductDescription NVARCHAR(MAX),
    @Unit NVARCHAR(10),
    @UnitPCS INT
AS
BEGIN
    UPDATE ProductDetails
    SET MOQ = @MOQ,
        CostPrice = @CostPrice,
        ProductDescription = @ProductDescription,
        Unit = @Unit,
        UnitPCS = @UnitPCS
    WHERE ProductID = @ProductID;
END
GO

-- 3. Godkend produkt
CREATE PROCEDURE spApproveProduct
    @ProductID INT
AS
BEGIN
    UPDATE Product
    SET HasBeenApproved = 1,
        ModifiedDate = GETDATE()
    WHERE ProductID = @ProductID;
END
GO

-- 4. Slet produkt og tilhørende poster
CREATE PROCEDURE spDeleteProduct
    @ProductID INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM ProductPantone WHERE ProductID = @ProductID;
        DELETE FROM ProductCertification WHERE ProductID = @ProductID;
        DELETE FROM FoodContactMaterial WHERE ProductID = @ProductID;
        DELETE FROM ProductPackageDimensions WHERE ProductID = @ProductID;
        DELETE FROM ProductDimensions WHERE ProductID = @ProductID;
        DELETE FROM Picture WHERE ProductID = @ProductID;
        DELETE FROM ProductCategory WHERE ProductID = @ProductID;
        DELETE FROM ProductDetails WHERE ProductID = @ProductID;
        DELETE FROM Product WHERE ProductID = @ProductID;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO

-- 5. Tilføj pantonefarve til produkt 
CREATE PROCEDURE spAddPantoneToProduct
    @ProductID INT,
    @PantoneID INT
AS
BEGIN
    INSERT INTO ProductPantone (ProductID, PantoneID)
    VALUES (@ProductID, @PantoneID);
END
GO

-- 6. Tilføj certificering til produkt
CREATE PROCEDURE spAddCertificationToProduct
    @ProductID INT,
    @CertificateID INT,
    @ValidUntil DATE
AS
BEGIN
    INSERT INTO ProductCertification (ProductID, CertificateID, ValidUntil)
    VALUES (@ProductID, @CertificateID, @ValidUntil);
END
GO

-- 7. Hent produktinfo med detaljer og kategori
CREATE PROCEDURE spGetProductFullInfo
    @ProductID INT
AS
BEGIN
    SELECT 
        p.ProductID, p.CreatedDate, p.HasBeenApproved,
        pd.DGAItemNo, pd.ProductDescription, pd.CostPrice, pd.Unit, pd.UnitPCS,
        pc.MainGroup, pc.MainCategory, pc.SubCategory,
        s.Name AS SupplierName,
        d.DesignerName
    FROM Product p
    JOIN ProductDetails pd ON p.ProductID = pd.ProductID
    JOIN ProductCategory pc ON p.ProductID = pc.ProductID
    JOIN Supplier s ON p.SupplierID = s.SupplierID
    JOIN Designer d ON p.DesignerID = d.DesignerID
    WHERE p.ProductID = @ProductID;
END
GO

-- 8. Tilføj ny farvegruppe
CREATE PROCEDURE spAddColorGroup
    @ColorGroupName NVARCHAR(100)
AS
BEGIN
    INSERT INTO ColorGroup (ColorGroupName)
    VALUES (@ColorGroupName);
END
GO

-- 9. Valider om produkt eksisterer
CREATE PROCEDURE spProductExists
    @ProductID INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Product WHERE ProductID = @ProductID)
        SELECT 1 AS Exists;
    ELSE
        SELECT 0 AS Exists;
END
GO

-- 10. Tilføj ny certificeringstype
CREATE PROCEDURE spAddCertification
    @CertificationType NVARCHAR(100),
    @Description NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Certification (CertificationType, Description)
    VALUES (@CertificationType, @Description);
END
GO

-- WIP
-- ========================
-- STEP-FLOW & VALIDATION
-- ========================

--  FieldDefinition – styring af felter og deres regler
CREATE TABLE FieldDefinition (
    FieldDefinitionID INT IDENTITY(1,1) PRIMARY KEY,
    FieldName NVARCHAR(100) NOT NULL,
    Step INT NOT NULL,
    IsRequired BIT NOT NULL,
    IsOptional BIT NOT NULL,
    DependsOn NVARCHAR(100) NULL,
    Datatype NVARCHAR(50) NOT NULL,
    GroupTag NVARCHAR(50) NULL
);
GO

--  FieldDependency – regler for afhængige felter
CREATE TABLE FieldDependency (
    FieldDependencyID INT IDENTITY(1,1) PRIMARY KEY,
    ParentField NVARCHAR(100) NOT NULL,
    ChildField NVARCHAR(100) NOT NULL,
    TriggerValue NVARCHAR(100) NOT NULL,
    Step INT NOT NULL
);
GO
-- Returnerer alle felter, som frontend (FE) skal vise i et givent trin i produktoprettelsen – inklusive deres regler og afhængigheder.
CREATE PROCEDURE spGetFieldsForStep
    @Step INT
AS
BEGIN
    SELECT 
        FieldName,
        IsRequired,
        IsOptional,
        DependsOn,
        Datatype,
        GroupTag
    FROM FieldDefinition
    WHERE Step = @Step;
END
GO



-- ========================
-- TESTDATA TIL FLOW-STYRING
-- ========================
-- 💡 Eksempel: Hvis 'IsFoodApproved = true' → vis 'FKM_MaterialType'

-- FieldDefinition: definér felter til step 1 og 2
INSERT INTO FieldDefinition (FieldName, Step, IsRequired, IsOptional, DependsOn, Datatype, GroupTag) VALUES
('ProductName',        1, 1, 0, NULL,            'Text',   'Basis'),
('IsFoodApproved',     2, 1, 0, NULL,            'Boolean','Certificering'),
('FKM_MaterialType',   2, 1, 0, 'IsFoodApproved','String', 'Certificering');
GO

-- FieldDependency: afhængighed – hvis IsFoodApproved = true → vis FKM_MaterialType
INSERT INTO FieldDependency (ParentField, ChildField, TriggerValue, Step) VALUES
('IsFoodApproved', 'FKM_MaterialType', 'true', 2);
GO
