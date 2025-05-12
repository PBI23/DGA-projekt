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