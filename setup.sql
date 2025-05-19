-- ========================
-- 1 DATABASE SETUP
-- ========================

-- Drop Database if exists and not in use
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'DGA_ProductDB')
BEGIN
    ALTER DATABASE DGA_ProductDB SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE DGA_ProductDB;
END
GO

-- Create new Database with specific collation
CREATE DATABASE DGA_ProductDB
COLLATE Latin1_General_BIN2;
GO

-- Use the new database
USE DGA_ProductDB;
GO


-- ===================================
-- 2 LOOKUP TABLES
-- ===================================

-- Stores ISO country codes and country names
CREATE TABLE Country (
    CountryId INT IDENTITY(1,1) PRIMARY KEY,
    CountryCode CHAR(2) NOT NULL,
    CountryName NVARCHAR(100) NOT NULL
);
GO

-- Stores color groups for product classification
CREATE TABLE ColorGroup (
    ColorGroupId INT IDENTITY(1,1) PRIMARY KEY,
    ColorGroupName NVARCHAR(100)
);
GO 

-- Stores Pantone color codes and names
CREATE TABLE Pantone (
    PantoneId INT IDENTITY(1,1) PRIMARY KEY,
    ColorCode NVARCHAR(50) NOT NULL,
    ColorName NVARCHAR(50)
);
GO

-- Stores types of certifications applicable to products
CREATE TABLE Certification (
    CertificationId INT IDENTITY(1,1) PRIMARY KEY,
    CertificationType NVARCHAR(100) NOT NULL,
    Description VARCHAR(MAX)
);
GO



-- ===================================
-- 3 CORE ENTITY TABLES
-- ===================================

-- Stores designer information
CREATE TABLE Designer (
    DesignerId INT IDENTITY(1,1) PRIMARY KEY,
    DesignerName NVARCHAR(100) NOT NULL
);
GO

-- Stores supplier groups (Note: [Group] is a reserved word)
CREATE TABLE [Group] (
    GroupID INT IDENTITY(1,1) PRIMARY KEY,
    SupplierId INT,
    GroupDescription NVARCHAR(250)
);
GO

-- Stores supplier details
CREATE TABLE Supplier (
    SupplierId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT,
    GroupID INT,
    SupplierNo INT,
    Name NVARCHAR(200)
);
GO

-- Main product table
CREATE TABLE Product (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    SupplierId INT,
    CountryId INT,
    DesignerId INT,
    ColorGroupId INT,
    CreatedDate DATE,
    ModifiedDate DATE,
    SetupStage DATE,
    HasBeenApproved BIT,
    ApprovedBy NVARCHAR(100), 
    ApprovedAt DATETIME,
    CurrentStep INT
);
GO

-- Stores all product details
CREATE TABLE ProductDetails (
    ProductId INT PRIMARY KEY,
    DGAItemNo NVARCHAR(50),
    ProductName NVARCHAR(100),
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


-- Stores the Product dimensions
CREATE TABLE ProductDimensions (
    ProductDID INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT,
    HeightCM DECIMAL(10,2),
    WidthCM DECIMAL(10,2),
    DepthCM DECIMAL(10,2),
    DiameterCM DECIMAL(10,2),
    NetWeightKG DECIMAL(10,2),
    GsmWeight DECIMAL(10,2)
);
GO

-- Stores the package dimensions
CREATE TABLE ProductPackageDimensions (
    ProductPDId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT,
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


-- Product category classification
CREATE TABLE ProductCategory (
    ProductId INT PRIMARY KEY,
    MainGroup NVARCHAR(100),
    MainCategory NVARCHAR(100),
    SubCategory NVARCHAR(100)
);
GO

-- Stores product pictures
CREATE TABLE Picture (
    PictureId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT,
    Data VARBINARY(MAX),
    DataType NVARCHAR(100)
);
GO

-- Stores product food contact materials
CREATE TABLE FoodContactMaterial (
    FCMId INT IDENTITY(1,1) PRIMARY KEY,
    ProductId INT NOT NULL,
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


-- ===================================
-- 4 RELATIONSTABELLER
-- ===================================

-- Relationship between products and certifications
CREATE TABLE ProductCertification (
    ProductId INT NOT NULL,
    CertificateId INT NOT NULL,
    ValidUntil DATE,
    PRIMARY KEY (ProductId, CertificateId)
);
GO

-- Relationship between products and Pantone colors
CREATE TABLE ProductPantone (
    ProductId INT NOT NULL,
    PantoneId INT NOT NULL,
    PRIMARY KEY (ProductId, PantoneId)
);
GO

GO



-- ===================================
-- 5 SYSTEM & AUDIT TABLES
-- ===================================


CREATE TABLE AuditLog (
    AuditLogID INT IDENTITY PRIMARY KEY,
    TableName NVARCHAR(100),
    Action NVARCHAR(20), -- e.g. INSERT, UPDATE, DELETE
    RecordId INT,
    FieldName NVARCHAR(100),
    OldValue NVARCHAR(MAX),
    NewValue NVARCHAR(MAX),
    PerformedBy NVARCHAR(100),
    PerformedAt DATETIME DEFAULT GETDATE()
);



-- ===================================
-- 6 FOREIGN KEY CONSTRAINTS
-- ===================================

-- Link Product to its dependencies
ALTER TABLE Product
ADD CONSTRAINT FK_Product_Supplier FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId),
    CONSTRAINT FK_Product_Country FOREIGN KEY (CountryId) REFERENCES Country(CountryId),
    CONSTRAINT FK_Product_Designer FOREIGN KEY (DesignerId) REFERENCES Designer(DesignerId),
    CONSTRAINT FK_Product_ColorGroup FOREIGN KEY (ColorGroupId) REFERENCES ColorGroup(ColorGroupId);
GO

-- Link ProductDetails to Product
ALTER TABLE ProductDetails
ADD CONSTRAINT FK_ProductDetails_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId);
GO

-- Link ProductCategory to Product
ALTER TABLE ProductCategory
ADD CONSTRAINT FK_ProductCategory_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId);
GO

-- Link Picture to Product
ALTER TABLE Picture
ADD CONSTRAINT FK_Picture_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId);
GO

-- Link ProductDimensions to Product
ALTER TABLE ProductDimensions
ADD CONSTRAINT FK_ProductDimensions_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId);
GO

-- Link ProductPackageDimensions to Product
ALTER TABLE ProductPackageDimensions
ADD CONSTRAINT FK_ProductPackageDimensions_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId);
GO

-- Link ProductCertification to Product and Certification
ALTER TABLE ProductCertification
ADD CONSTRAINT FK_ProductCertification_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    CONSTRAINT FK_ProductCertification_Certification FOREIGN KEY (CertificateId) REFERENCES Certification(CertificationId);
GO

-- Link ProductPantone to Product and Pantone
ALTER TABLE ProductPantone
ADD CONSTRAINT FK_ProductPantone_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId),
    CONSTRAINT FK_ProductPantone_Pantone FOREIGN KEY (PantoneId) REFERENCES Pantone(PantoneId);
GO

-- Link FoodContactMaterial to Product
ALTER TABLE FoodContactMaterial
ADD CONSTRAINT FK_FoodContactMaterial_Product FOREIGN KEY (ProductId) REFERENCES Product(ProductId);
GO

-- Link Supplier to Group (bidirectional)
ALTER TABLE Supplier
ADD CONSTRAINT FK_Supplier_Group FOREIGN KEY (GroupID) REFERENCES [Group](GroupID);
GO

ALTER TABLE [Group]
ADD CONSTRAINT FK_Group_Supplier FOREIGN KEY (SupplierId) REFERENCES Supplier(SupplierId);
GO


-- ===================================
-- 7 STORED PROCEDURES
-- ===================================


-- ===================================
-- 7.1 STORED PROCEDURES – PRODUCT CRUD
-- ===================================

-- Create a new product with details and category
CREATE PROCEDURE spCreateProduct
    @SupplierId INT,
    @CountryId INT,
    @DesignerId INT,
    @ColorGroupId INT,
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

    DECLARE @ProductId INT;

    INSERT INTO Product (SupplierId, CountryId, DesignerId, ColorGroupId, CreatedDate, ModifiedDate, SetupStage, HasBeenApproved)
    VALUES (@SupplierId, @CountryId, @DesignerId, @ColorGroupId, GETDATE(), GETDATE(), GETDATE(), 0);

    SET @ProductId = SCOPE_IDENTITY();

    INSERT INTO ProductDetails (ProductId, DGAItemNo, ProductLogo, Series, ProductDescription, MOQ, CostPrice, Unit, UnitPCS)
    VALUES (@ProductId, @DGAItemNo, @ProductLogo, @Series, @ProductDescription, @MOQ, @CostPrice, @Unit, @UnitPCS);

    INSERT INTO ProductCategory (ProductId, MainGroup, MainCategory, SubCategory)
    VALUES (@ProductId, @MainGroup, @MainCategory, @SubCategory);

    SELECT @ProductId AS NewProductId;
END
GO

-- Update product details
CREATE PROCEDURE spUpdateProductDetails
    @ProductId INT,
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
    WHERE ProductId = @ProductId;
END
GO

-- Approve product
CREATE OR ALTER PROCEDURE spApproveProduct
    @ProductId INT,
    @IsApproved BIT,
    @ApprovedBy NVARCHAR(100),
    @ApprovedAt DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Product
    SET HasBeenApproved = @IsApproved,
        ApprovedBy = @ApprovedBy,
        ApprovedAt = @ApprovedAt,
        ModifiedDate = @ApprovedAt
    WHERE ProductId = @ProductId;
END
GO

-- Delete a product and related records
CREATE PROCEDURE spDeleteProduct
    @ProductId INT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        DELETE FROM ProductPantone WHERE ProductId = @ProductId;
        DELETE FROM ProductCertification WHERE ProductId = @ProductId;
        DELETE FROM FoodContactMaterial WHERE ProductId = @ProductId;
        DELETE FROM ProductPackageDimensions WHERE ProductId = @ProductId;
        DELETE FROM ProductDimensions WHERE ProductId = @ProductId;
        DELETE FROM Picture WHERE ProductId = @ProductId;
        DELETE FROM ProductCategory WHERE ProductId = @ProductId;
        DELETE FROM ProductDetails WHERE ProductId = @ProductId;
        DELETE FROM Product WHERE ProductId = @ProductId;

        COMMIT;
    END TRY
    BEGIN CATCH
        ROLLBACK;
        THROW;
    END CATCH
END
GO



-- ======================================================
-- 7.2 STORED PROCEDURES – Supplementary Product Functions
-- ======================================================

-- Add Pantone color to a product
CREATE PROCEDURE spAddPantoneToProduct
    @ProductId INT,
    @PantoneId INT
AS
BEGIN
    INSERT INTO ProductPantone (ProductId, PantoneId)
    VALUES (@ProductId, @PantoneId);
END
GO

-- Add colorGroup
CREATE PROCEDURE spAddColorGroup
    @ColorGroupName NVARCHAR(100)
AS
BEGIN
    INSERT INTO ColorGroup (ColorGroupName)
    VALUES (@ColorGroupName);
END
GO

-- Add certification to a product
CREATE PROCEDURE spAddCertificationToProduct
    @ProductId INT,
    @CertificateId INT,
    @ValidUntil DATE
AS
BEGIN
    INSERT INTO ProductCertification (ProductId, CertificateId, ValidUntil)
    VALUES (@ProductId, @CertificateId, @ValidUntil);
END
GO

-- Add new certification type
CREATE PROCEDURE spAddCertification
    @CertificationType NVARCHAR(100),
    @Description NVARCHAR(MAX)
AS
BEGIN
    INSERT INTO Certification (CertificationType, Description)
    VALUES (@CertificationType, @Description);
END
GO

-- Check if product exists
CREATE PROCEDURE spProductExists
    @ProductId INT
AS
BEGIN
    IF EXISTS (SELECT 1 FROM Product WHERE ProductId = @ProductId)
        SELECT 1 AS [Exists];

    ELSE
        SELECT 0 AS [Exists];
END
GO

-- Update product status directly
CREATE PROCEDURE spUpdateProductStatus
    @ProductId INT,
    @Status NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Product
    SET 
        HasBeenApproved = CASE WHEN @Status = 'Approved' THEN 1 ELSE 0 END,
        ModifiedDate = GETDATE()
    WHERE ProductId = @ProductId;
END
GO


-- ===================================
-- 7.3 STORED PROCEDURES – Get Product Data
-- ===================================

-- Get all products with detailed information
CREATE PROCEDURE spGetAllProducts
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.ProductId,
        p.CreatedDate,
        p.ModifiedDate,
        p.SetupStage,
        p.HasBeenApproved,
        p.CurrentStep,

        -- Supplier
        s.SupplierId,
        s.Name AS SupplierName,

        -- Designer
        d.DesignerId,
        d.DesignerName,

        -- Country
        c.CountryId,
        c.CountryName AS CountryOfOrigin,

        -- Colorgroup
        cg.ColorGroupId,
        cg.ColorGroupName,

        -- Details
        pd.DGAItemNo,
        pd.ProductLogo,
        pd.Series,
        pd.ProductDescription,
        pd.MOQ,
        pd.CostPrice,
        pd.Unit,
        CAST(pd.UnitPCS AS NVARCHAR) AS ColiSize,
        pd.ABC,
        pd.HangtagsAndStickers,

        -- Category
        pc.MainGroup,
        pc.MainCategory,
        pc.SubCategory

    FROM Product p
    LEFT JOIN Supplier s ON p.SupplierId = s.SupplierId
    LEFT JOIN Designer d ON p.DesignerId = d.DesignerId
    LEFT JOIN Country c ON p.CountryId = c.CountryId
    LEFT JOIN ColorGroup cg ON p.ColorGroupId = cg.ColorGroupId
    LEFT JOIN ProductDetails pd ON p.ProductId = pd.ProductId
    LEFT JOIN ProductCategory pc ON p.ProductId = pc.ProductId
END
GO


-- Get full information for a single product
CREATE PROCEDURE spGetProductFullInfo
    @ProductId INT
AS
BEGIN
    SELECT 
        p.ProductId,
        pd.ProductName AS Name,
        pd.Series AS Season,
        pd.DGAItemNo,
        pd.ProductDescription AS Description,
        pd.UnitPCS AS ColiSize,
        pd.CostPrice,
        pd.Unit,
        pc.MainGroup AS ProductGroup,
        pc.MainCategory,
        pc.SubCategory,
        s.Name AS Supplier,
        d.DesignerName AS Designer,
        c.CountryName AS CountryOfOrigin, 
        p.HasBeenApproved,
        p.CreatedDate,
        CASE 
            WHEN p.HasBeenApproved = 1 THEN 'Approved'
            ELSE 'Draft'
        END AS Status
    FROM Product p
    JOIN ProductDetails pd ON p.ProductId = pd.ProductId
    JOIN ProductCategory pc ON p.ProductId = pc.ProductId
    JOIN Supplier s ON p.SupplierId = s.SupplierId
    JOIN Designer d ON p.DesignerId = d.DesignerId
    JOIN Country c ON p.CountryId = c.CountryId 
    WHERE p.ProductId = @ProductId;
END
GO

-- Get all drafts (unapproved products)
CREATE PROCEDURE spGetAllDrafts
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        p.ProductId,
        pd.DGAItemNo,
        pd.ProductDescription AS Description,
        pd.UnitPCS AS ColiSize,
        pd.Series AS Season,
        pc.MainGroup AS ProductGroup,

        s.Name AS Supplier,
        d.DesignerName AS Designer,
        c.CountryName AS CountryOfOrigin,

        'Draft' AS Status,
        pd.ProductLogo AS Name

    FROM Product p
    LEFT JOIN Supplier s ON p.SupplierId = s.SupplierId
    LEFT JOIN Designer d ON p.DesignerId = d.DesignerId
    LEFT JOIN Country c ON p.CountryId = c.CountryId
    LEFT JOIN ProductDetails pd ON p.ProductId = pd.ProductId
    LEFT JOIN ProductCategory pc ON p.ProductId = pc.ProductId
    WHERE ISNULL(p.HasBeenApproved, 0) = 0;
END
GO

-- ===================================
-- 7.4 – STEP-BASED WORKFLOW & FIELD LOGIC
-- ===================================
-- This section contains all procedures for saving and retrieving step-specific product data,
-- including support for dynamic fields and conditional logic.


-- Save a single field value for a given step
CREATE PROCEDURE spSaveFieldValue
    @ProductId INT,
    @FieldName NVARCHAR(100),
    @FieldValue NVARCHAR(MAX),
    @Step INT
AS
BEGIN
    IF EXISTS (
        SELECT 1 
        FROM ProductFieldValue 
        WHERE ProductId = @ProductId 
          AND FieldName = @FieldName
    )
    BEGIN
        UPDATE ProductFieldValue
        SET FieldValue = @FieldValue
        WHERE ProductId = @ProductId 
          AND FieldName = @FieldName;
    END
    ELSE
    BEGIN
        INSERT INTO ProductFieldValue (ProductId, FieldName, FieldValue, Step)
        VALUES (@ProductId, @FieldName, @FieldValue, @Step);
    END
END
GO

-- Save step 1: Basic product info
CREATE PROCEDURE spSaveStep1
    @Name NVARCHAR(100),
    @Season NVARCHAR(50),
    @DgaItemNo NVARCHAR(50),
    @CountryOfOrigin NVARCHAR(100),
    @Supplier NVARCHAR(200),
    @Designer NVARCHAR(100),
    @Description NVARCHAR(MAX),
    @ColiSize NVARCHAR(50),
    @ProductGroup NVARCHAR(100),
    @ProductId INT OUTPUT  
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @CountryId INT = (SELECT CountryId FROM Country WHERE CountryName = @CountryOfOrigin);
    DECLARE @SupplierId INT = (SELECT SupplierId FROM Supplier WHERE Name = @Supplier);
    DECLARE @DesignerId INT = (SELECT DesignerId FROM Designer WHERE DesignerName = @Designer);

    INSERT INTO Product (CountryId, SupplierId, DesignerId, CreatedDate, ModifiedDate, SetupStage, HasBeenApproved)
    VALUES (@CountryId, @SupplierId, @DesignerId, GETDATE(), GETDATE(), GETDATE(), 0);

    SET @ProductId = SCOPE_IDENTITY();

    INSERT INTO ProductDetails (
        ProductId,
        DGAItemNo,
        ProductName,
        ProductDescription,
        UnitPCS
    )
    VALUES (
        @ProductId,
        @DgaItemNo,
        @Name, 
        @Description,
        TRY_CAST(@ColiSize AS INT)
    );

    INSERT INTO ProductCategory (ProductId, MainGroup)
    VALUES (@ProductId, @ProductGroup);
END
GO

-- Save step 2: JSON-formatted fields
CREATE PROCEDURE spSaveStep2
    @ProductId INT,
    @JsonData NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Step INT = 2;

    -- Parse JSON til tabel (kræver SQL Server 2016+)
    SELECT 
        [key] AS FieldName,
        CAST([value] AS NVARCHAR(MAX)) AS FieldValue
    INTO #TempAnswers
    FROM OPENJSON(@JsonData);

    -- Gem hver feltværdi
    MERGE ProductFieldValue AS target
    USING #TempAnswers AS source
    ON target.ProductId = @ProductId AND target.FieldName = source.FieldName
    WHEN MATCHED THEN 
        UPDATE SET FieldValue = source.FieldValue
    WHEN NOT MATCHED THEN
        INSERT (ProductId, FieldName, FieldValue, Step)
        VALUES (@ProductId, source.FieldName, source.FieldValue, @Step);

    DROP TABLE #TempAnswers;
END
GO

-- Save step 3: Packaging & safety
CREATE PROCEDURE spSaveStep3
    @ProductId INT,
    @SupplierProductNo INT,
    @CustomerClearanceNo INT,
    @CustomerClearancePercent DECIMAL(5,2),
    @CostPrice DECIMAL(10,2),
    @InnerCarton INT,
    @OuterCarton NVARCHAR(50),
    @GrossWeight DECIMAL(10,2),
    @PackingHeight DECIMAL(10,2),
    @PackingWidthLength DECIMAL(10,2),
    @PackingDepth DECIMAL(10,2),
    @DishwasherSafe BIT,
    @MicrowaveSafe BIT,
    @Svanemaerket BIT,
    @GrunerPunkt BIT,
    @FSC100 BIT,
    @FSCMix70 BIT,
    @ABC CHAR(1),
    @ProductLogo NVARCHAR(50),
    @HangtagsAndStickers NVARCHAR(100),
    @Series NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Opdater værdier i ProductDetails
    UPDATE ProductDetails
    SET 
        SupplierProductNo = TRY_CAST(@SupplierProductNo AS NVARCHAR(50)),
        CustomerClearanceNo = @CustomerClearanceNo,
        CustomerClearencePercent = @CustomerClearancePercent,
        CostPrice = @CostPrice,
        ABC = @ABC,
        ProductLogo = @ProductLogo,
        HangtagsAndStickers = @HangtagsAndStickers,
        Series = @Series
    WHERE ProductId = @ProductId;

    -- Opdater emballage
    UPDATE ProductPackageDimensions
    SET
        GrossWeightKG = @GrossWeight,
        PackagingHeightCM = @PackingHeight,
        PackagingWidthCM = @PackingWidthLength,
        PackagingDepthCM = @PackingDepth,
        InnerCarton = @InnerCarton,
        OuterCarton = TRY_CAST(@OuterCarton AS INT)
    WHERE ProductId = @ProductId;

    -- Gem boolean felter som tekst
    DECLARE @Step INT = 3;
    DECLARE @BoolText NVARCHAR(5);

    SET @BoolText = IIF(@DishwasherSafe = 1, N'true', N'false');
    EXEC spSaveFieldValue @ProductId, N'DishwasherSafe', @BoolText, @Step;

    SET @BoolText = IIF(@MicrowaveSafe = 1, N'true', N'false');
    EXEC spSaveFieldValue @ProductId, N'MicrowaveSafe', @BoolText, @Step;

    SET @BoolText = IIF(@Svanemaerket = 1, N'true', N'false');
    EXEC spSaveFieldValue @ProductId, N'Svanemaerket', @BoolText, @Step;

    SET @BoolText = IIF(@GrunerPunkt = 1, N'true', N'false');
    EXEC spSaveFieldValue @ProductId, N'GrunerPunkt', @BoolText, @Step;

    SET @BoolText = IIF(@FSC100 = 1, N'true', N'false');
    EXEC spSaveFieldValue @ProductId, N'FSC100', @BoolText, @Step;

    SET @BoolText = IIF(@FSCMix70 = 1, N'true', N'false');
    EXEC spSaveFieldValue @ProductId, N'FSCMix70', @BoolText, @Step;
END
GO

-- Save step 4: Color, pantone, extra info
CREATE PROCEDURE spSaveStep4
    @ProductId INT,
    @DgaColorGroupName NVARCHAR(100),
    @DgaSalCatGroup NVARCHAR(100),
    @PantonePantone NVARCHAR(100),
    @DgaVendItemCodeCode NVARCHAR(100),
    @Assorted BIT = NULL,
    @AdditionalInformation NVARCHAR(MAX) = NULL,
    @GsmWeight INT = NULL,
    @BurningTimeHours INT = NULL,
    @AntidopingRegulation BIT = NULL,
    @Subcategory NVARCHAR(100) = NULL,
    @OtherInformation2 NVARCHAR(255) = NULL,
    @GsmWeight2 INT = NULL
AS
BEGIN
    SET NOCOUNT ON;
    DECLARE @Step INT = 4;

    DECLARE @ColorGroupId INT = (SELECT TOP 1 ColorGroupId FROM ColorGroup WHERE ColorGroupName = @DgaColorGroupName);
    DECLARE @PantoneId INT = (SELECT TOP 1 PantoneId FROM Pantone WHERE ColorName = @PantonePantone);

    UPDATE Product
    SET ColorGroupId = @ColorGroupId
    WHERE ProductId = @ProductId;

    -- Opdater ProductDetails
    UPDATE ProductDetails
    SET 
        SupplierProductNo = @DgaVendItemCodeCode,
        Assorted = IIF(@Assorted IS NULL, Assorted, IIF(@Assorted = 1, 'true', 'false')),
        AdditionalInfo = @AdditionalInformation,
        BurningTimeHours = @BurningTimeHours
    WHERE ProductId = @ProductId;

    -- Opdater ProductDimensions med GsmWeight
    UPDATE ProductDimensions
    SET GsmWeight = @GsmWeight
    WHERE ProductId = @ProductId;


    UPDATE ProductCategory
    SET SubCategory = @Subcategory
    WHERE ProductId = @ProductId;

    IF @PantoneId IS NOT NULL
    BEGIN
        IF NOT EXISTS (
            SELECT 1 FROM ProductPantone WHERE ProductId = @ProductId AND PantoneId = @PantoneId
        )
        BEGIN
            INSERT INTO ProductPantone (ProductId, PantoneId)
            VALUES (@ProductId, @PantoneId);
        END
    END

    -- Gem ekstra felter via variable
    DECLARE @BoolText NVARCHAR(5) = IIF(@AntidopingRegulation = 1, N'true', N'false');
    DECLARE @GsmWeightText NVARCHAR(20) = COALESCE(CAST(@GsmWeight2 AS NVARCHAR), N'');

    EXEC spSaveFieldValue @ProductId, N'DgaSalCatGroup', @DgaSalCatGroup, @Step;
    EXEC spSaveFieldValue @ProductId, N'AntidopingRegulation', @BoolText, @Step;
    EXEC spSaveFieldValue @ProductId, N'OtherInformation2', @OtherInformation2, @Step;
    EXEC spSaveFieldValue @ProductId, N'GsmWeight2', @GsmWeightText, @Step;
END
GO

-- Validate required fields for step
CREATE PROCEDURE spValidateStep
    @ProductId INT,
    @Step INT
AS
BEGIN
    SELECT 
        fd.FieldName
    FROM FieldDefinition fd
    LEFT JOIN ProductFieldValue pfv
        ON fd.FieldName = pfv.FieldName AND pfv.ProductId = @ProductId
    WHERE fd.Step = @Step AND fd.IsRequired = 1 AND (pfv.FieldValue IS NULL OR pfv.FieldValue = '');
END
GO

-- Get previously saved field values for step
CREATE PROCEDURE spGetSavedFieldsForStep
    @ProductId INT,
    @Step INT
AS
BEGIN
    SELECT 
        FieldName,
        FieldValue
    FROM ProductFieldValue
    WHERE ProductId = @ProductId AND Step = @Step;
END
GO



-- ===================================
-- 9. STEP-BASED FIELD VALUE STORAGE
-- ===================================

-- Stores user-entered field values per product and step
CREATE TABLE ProductFieldValue (
    ProductId INT NOT NULL,
    FieldName NVARCHAR(100) NOT NULL,
    FieldValue NVARCHAR(MAX),
    Step INT NOT NULL,
    PRIMARY KEY (ProductId, FieldName),
    FOREIGN KEY (ProductId) REFERENCES Product(ProductId)
);
GO


-- ===================================
-- 10. STEP CONFIGURATION & FIELD DEFINITION
-- ===================================

-- TABLES: Define fields and dependencies for steps

-- -- Defines what fields are available per step, including validation rules
CREATE TABLE FieldDefinition (
    FieldDefinitionId INT IDENTITY(1,1) PRIMARY KEY,
    FieldName NVARCHAR(100) NOT NULL,
    Step INT NOT NULL,
    IsRequired BIT NOT NULL,
    IsOptional BIT NOT NULL,
    DependsOn NVARCHAR(100) NULL,
    Datatype NVARCHAR(50) NOT NULL,
    GroupTag NVARCHAR(50) NULL
);
GO

-- Describes conditional field relationships (i.e. show/hide logic)
CREATE TABLE FieldDependency (
    FieldDependencyID INT IDENTITY(1,1) PRIMARY KEY,
    ParentField NVARCHAR(100) NOT NULL,
    ChildField NVARCHAR(100) NOT NULL,
    TriggerValue NVARCHAR(100) NOT NULL,
    Step INT NOT NULL
);
GO


-- PROCEDURES: Get fields and dependent fields for a step
CREATE PROCEDURE spGetFieldsForStep
    @Step INT
AS
BEGIN
    SELECT 
        FieldDefinitionId,
        FieldName,
        Step,
        IsRequired,
        Datatype,
        GroupTag,
        DependsOn
    FROM FieldDefinition
    WHERE Step = @Step;
END
GO

CREATE PROCEDURE spGetDependentFields
    @ParentField NVARCHAR(100),
    @TriggerValue NVARCHAR(50),
    @Step INT
AS
BEGIN
    SELECT 
        fd.FieldDefinitionId,
        fd.FieldName,
        fd.Step,
        fd.IsRequired,
        fd.Datatype,
        fd.GroupTag,
        fd.DependsOn
    FROM FieldDependency d
    JOIN FieldDefinition fd 
        ON d.ChildField = fd.FieldName
        AND d.Step = fd.Step
    WHERE d.ParentField = @ParentField
        AND d.TriggerValue = @TriggerValue
        AND d.Step = @Step;
END
GO



-- ===================================
-- 11. TRIGGERS – AUDIT LOGGING
-- ===================================

-- Audit trigger for Product table
CREATE TRIGGER trg_Audit_Product
ON Product
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @User NVARCHAR(100) = SYSTEM_USER;

    -- ========== INSERT ==========
    INSERT INTO AuditLog (TableName, Action, RecordId, FieldName, OldValue, NewValue, PerformedBy)
    SELECT 'Product', 'INSERT', i.ProductId, 'FULL_ROW', NULL,
           CONCAT(
               'SupplierId=', i.SupplierId, '; ',
               'CountryId=', i.CountryId, '; ',
               'DesignerId=', i.DesignerId, '; ',
               'ColorGroupId=', i.ColorGroupId, '; ',
               'HasBeenApproved=', i.HasBeenApproved, '; ',
               'ApprovedBy=', i.ApprovedBy, '; ',
               'ApprovedAt=', CONVERT(NVARCHAR, i.ApprovedAt), '; ',
               'CurrentStep=', i.CurrentStep
           ),
           @User
    FROM inserted i
    LEFT JOIN deleted d ON i.ProductId = d.ProductId
    WHERE d.ProductId IS NULL;

    -- ========== DELETE ==========
    INSERT INTO AuditLog (TableName, Action, RecordId, FieldName, OldValue, NewValue, PerformedBy)
    SELECT 'Product', 'DELETE', d.ProductId, 'FULL_ROW',
           CONCAT(
               'SupplierId=', d.SupplierId, '; ',
               'CountryId=', d.CountryId, '; ',
               'DesignerId=', d.DesignerId, '; ',
               'ColorGroupId=', d.ColorGroupId, '; ',
               'HasBeenApproved=', d.HasBeenApproved, '; ',
               'ApprovedBy=', d.ApprovedBy, '; ',
               'ApprovedAt=', CONVERT(NVARCHAR, d.ApprovedAt), '; ',
               'CurrentStep=', d.CurrentStep
           ),
           NULL,
           @User
    FROM deleted d
    LEFT JOIN inserted i ON d.ProductId = i.ProductId
    WHERE i.ProductId IS NULL;

    -- ========== UPDATE ==========
    INSERT INTO AuditLog (TableName, Action, RecordId, FieldName, OldValue, NewValue, PerformedBy)
    SELECT 'Product', 'UPDATE', i.ProductId, 'SupplierId',
           CAST(d.SupplierId AS NVARCHAR), CAST(i.SupplierId AS NVARCHAR), @User
    FROM inserted i JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.SupplierId, -1) <> ISNULL(d.SupplierId, -1)

    UNION ALL
    SELECT 'Product', 'UPDATE', i.ProductId, 'CountryId',
           CAST(d.CountryId AS NVARCHAR), CAST(i.CountryId AS NVARCHAR), @User
    FROM inserted i JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.CountryId, -1) <> ISNULL(d.CountryId, -1)

    UNION ALL
    SELECT 'Product', 'UPDATE', i.ProductId, 'DesignerId',
           CAST(d.DesignerId AS NVARCHAR), CAST(i.DesignerId AS NVARCHAR), @User
    FROM inserted i JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.DesignerId, -1) <> ISNULL(d.DesignerId, -1)

    UNION ALL
    SELECT 'Product', 'UPDATE', i.ProductId, 'ColorGroupId',
           CAST(d.ColorGroupId AS NVARCHAR), CAST(i.ColorGroupId AS NVARCHAR), @User
    FROM inserted i JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.ColorGroupId, -1) <> ISNULL(d.ColorGroupId, -1)

    UNION ALL
    SELECT 'Product', 'UPDATE', i.ProductId, 'HasBeenApproved',
           CAST(d.HasBeenApproved AS NVARCHAR), CAST(i.HasBeenApproved AS NVARCHAR), @User
    FROM inserted i JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.HasBeenApproved, 0) <> ISNULL(d.HasBeenApproved, 0)

    UNION ALL
    SELECT 'Product', 'UPDATE', i.ProductId, 'ApprovedBy',
           d.ApprovedBy, i.ApprovedBy, @User
    FROM inserted i JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.ApprovedBy, '') <> ISNULL(d.ApprovedBy, '')

    UNION ALL
    SELECT 'Product', 'UPDATE', i.ProductId, 'ApprovedAt',
           CONVERT(NVARCHAR, d.ApprovedAt), CONVERT(NVARCHAR, i.ApprovedAt), @User
    FROM inserted i JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.ApprovedAt, '') <> ISNULL(d.ApprovedAt, '')

    UNION ALL
    SELECT 'Product', 'UPDATE', i.ProductId, 'CurrentStep',
           CAST(d.CurrentStep AS NVARCHAR), CAST(i.CurrentStep AS NVARCHAR), @User
    FROM inserted i JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.CurrentStep, -1) <> ISNULL(d.CurrentStep, -1);
END
GO



-- Audit trigger for ProductDetails
CREATE TRIGGER trg_Audit_ProductDetails
ON ProductDetails
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @User NVARCHAR(100) = SYSTEM_USER;

    -- ========== INSERT ==========
    INSERT INTO AuditLog (TableName, Action, RecordId, FieldName, OldValue, NewValue, PerformedBy)
    SELECT 'ProductDetails', 'INSERT', i.ProductId, 'FULL_ROW', NULL, 
           CONCAT(
               'DGAItemNo=', i.DGAItemNo, '; ',
               'ProductName=', i.ProductName, '; ',
               'MOQ=', i.MOQ, '; ',
               'CostPrice=', i.CostPrice, '; ',
               'UnitPCS=', i.UnitPCS
           ),
           @User
    FROM inserted i
    LEFT JOIN deleted d ON i.ProductId = d.ProductId
    WHERE d.ProductId IS NULL;

    -- ========== DELETE ==========
    INSERT INTO AuditLog (TableName, Action, RecordId, FieldName, OldValue, NewValue, PerformedBy)
    SELECT 'ProductDetails', 'DELETE', d.ProductId, 'FULL_ROW', 
           CONCAT(
               'DGAItemNo=', d.DGAItemNo, '; ',
               'ProductName=', d.ProductName, '; ',
               'MOQ=', d.MOQ, '; ',
               'CostPrice=', d.CostPrice, '; ',
               'UnitPCS=', d.UnitPCS
           ),
           NULL,
           @User
    FROM deleted d
    LEFT JOIN inserted i ON d.ProductId = i.ProductId
    WHERE i.ProductId IS NULL;

    -- ========== UPDATE ==========
    INSERT INTO AuditLog (TableName, Action, RecordId, FieldName, OldValue, NewValue, PerformedBy)
    SELECT 'ProductDetails', 'UPDATE', i.ProductId, 'DGAItemNo',
           d.DGAItemNo, i.DGAItemNo, @User
    FROM inserted i
    JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.DGAItemNo, '') <> ISNULL(d.DGAItemNo, '')

    UNION ALL

    SELECT 'ProductDetails', 'UPDATE', i.ProductId, 'ProductName',
           d.ProductName, i.ProductName, @User
    FROM inserted i
    JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.ProductName, '') <> ISNULL(d.ProductName, '')

    UNION ALL

    SELECT 'ProductDetails', 'UPDATE', i.ProductId, 'MOQ',
           CAST(d.MOQ AS NVARCHAR), CAST(i.MOQ AS NVARCHAR), @User
    FROM inserted i
    JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.MOQ, -1) <> ISNULL(d.MOQ, -1)

    UNION ALL

    SELECT 'ProductDetails', 'UPDATE', i.ProductId, 'CostPrice',
           CAST(d.CostPrice AS NVARCHAR), CAST(i.CostPrice AS NVARCHAR), @User
    FROM inserted i
    JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.CostPrice, -1) <> ISNULL(d.CostPrice, -1)

    UNION ALL

    SELECT 'ProductDetails', 'UPDATE', i.ProductId, 'ProductDescription',
           d.ProductDescription, i.ProductDescription, @User
    FROM inserted i
    JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.ProductDescription, '') <> ISNULL(d.ProductDescription, '')

    UNION ALL

    SELECT 'ProductDetails', 'UPDATE', i.ProductId, 'UnitPCS',
           CAST(d.UnitPCS AS NVARCHAR), CAST(i.UnitPCS AS NVARCHAR), @User
    FROM inserted i
    JOIN deleted d ON i.ProductId = d.ProductId
    WHERE ISNULL(i.UnitPCS, -1) <> ISNULL(d.UnitPCS, -1);
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




-- ==========
-- TEST DATA
-- ==========



-- ========================
-- Lookup værdier
-- ========================
INSERT INTO Country (CountryCode, CountryName) VALUES
('DK', 'Denmark'),
('CN', 'China'),
('DE', 'Germany');

INSERT INTO Designer (DesignerName) VALUES
('Anna Møller'),
('Jonas Lind');

INSERT INTO Supplier (SupplierNo, Name) VALUES
(1001, 'Nordic Supplies'),
(1002, 'Asia Import Co.');

INSERT INTO ColorGroup (ColorGroupName) VALUES
('Neutral'),
('Warm'),
('Cool');

-- ========================
-- Ugodkendt produkt (kladde)
-- ========================
INSERT INTO Product (SupplierId, CountryId, DesignerId, ColorGroupId, CreatedDate, ModifiedDate, SetupStage, HasBeenApproved, CurrentStep)
VALUES (1, 1, 1, 2, GETDATE(), GETDATE(), GETDATE(), 0, 1); -- ID = 1

INSERT INTO ProductDetails (
    ProductId, DGAItemNo, ProductLogo, Series, ProductDescription,
    MOQ, CostPrice, Unit, UnitPCS, ABC, HangtagsAndStickers
) VALUES (
    1, 'DGA-001', 'logo1.png', 'Spring 2024', 'Elegant lysestage i sort keramik',
    50, 22.5, 'stk', 6, 'A', 'Sticker 2024'
);

INSERT INTO ProductCategory (ProductId, MainGroup, MainCategory, SubCategory)
VALUES (1, 'Home', 'Decoration', 'Candles');

-- ========================
-- Godkendt produkt
-- ========================
INSERT INTO Product (SupplierId, CountryId, DesignerId, ColorGroupId, CreatedDate, ModifiedDate, SetupStage, HasBeenApproved, CurrentStep)
VALUES (2, 2, 2, 1, GETDATE(), GETDATE(), GETDATE(), 1, 4); -- ID = 2

INSERT INTO ProductDetails (
    ProductId, DGAItemNo, ProductLogo, Series, ProductDescription,
    MOQ, CostPrice, Unit, UnitPCS, ABC, HangtagsAndStickers
) VALUES (
    2, 'DGA-002', 'logo2.png', 'Autumn 2024', 'Farverig kop i stentøj',
    100, 14.95, 'stk', 12, 'B', 'Eco Tag 2024'
);

INSERT INTO ProductCategory (ProductId, MainGroup, MainCategory, SubCategory)
VALUES (2, 'Kitchen', 'Tableware', 'Cups');
GO

INSERT INTO ProductDimensions (ProductId, HeightCM, WidthCM, DepthCM, DiameterCM, NetWeightKG, GsmWeight)
VALUES 
(1, 10.0, 6.0, 6.0, 0.0, 0.25, 180),
(2, 12.5, 9.0, 9.0, 0.0, 0.4, 250);


INSERT INTO ProductPackageDimensions (
    ProductId, GrossWeightKG, CBM,
    PackagingHeightCM, PackagingWidthCM, PackagingDepthCM,
    KIHeightCM, KIWidthCM, KIDepthCM,
    KYHeightCM, KYWidthCM, KYDepthCM,
    PackingDepthCM, InnerCarton, OuterCarton
)
VALUES
(1, 0.3, 0.005,
 12.0, 8.0, 8.0,
 0.0, 0.0, 0.0,
 0.0, 0.0, 0.0,
 8.0, 6, 24),

(2, 0.5, 0.008,
 15.0, 10.0, 10.0,
 0.0, 0.0, 0.0,
 0.0, 0.0, 0.0,
 10.0, 12, 48);


INSERT INTO ProductFieldValue (ProductId, FieldName, FieldValue, Step)
VALUES 
(1, 'IsFoodApproved', 'true', 2),
(1, 'FKM_MaterialType', 'Plastic', 2),
(1, 'AntidopingRegulation', 'false', 4),
(2, 'IsFoodApproved', 'false', 2),
(2, 'GsmWeight2', '300', 4);


INSERT INTO Picture (ProductId, DataType)
VALUES 
(1, 'image/png'),
(2, 'image/jpeg');
GO

