CREATE DATABASE CigarShop

GO

USE CigarShop

GO

--01
CREATE TABLE Sizes
(
	Id INT PRIMARY KEY IDENTITY,
	[Length] INT NOT NULL,
		CHECK([Length] BETWEEN 10 AND 25),
	RingRange DECIMAL(5,2) NOT NULL
		CHECK(RingRange BETWEEN 1.5 AND 7.5)
)

CREATE TABLE Tastes
(
	Id INT PRIMARY KEY IDENTITY,
	TasteType VARCHAR(20) NOT NULL,
	TasteStrength VARCHAR(15) NOT NULL,
	ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Brands
(
	Id INT PRIMARY KEY IDENTITY,
	BrandName VARCHAR(30) UNIQUE NOT NULL,
	BrandDescription VARCHAR(MAX),
)

CREATE TABLE Cigars
(
	Id INT PRIMARY KEY IDENTITY,
	CigarName VARCHAR(80) NOT NULL,
	BrandId INT FOREIGN KEY REFERENCES Brands(Id) NOT NULL,
	TastId INT FOREIGN KEY REFERENCES Tastes(Id) NOT NULL,
	SizeId INT FOREIGN KEY REFERENCES Sizes(Id) NOT NULL,
	PriceForSingleCigar MONEY NOT NULL,
	ImageURL NVARCHAR(100) NOT NULL
)

CREATE TABLE Addresses
(
	Id INT PRIMARY KEY IDENTITY,
	Town VARCHAR(30) NOT NULL,
	Country NVARCHAR(30) NOT NULL,
	Streat NVARCHAR(100) NOT NULL,
	ZIP VARCHAR(20) NOT NULL,
)

CREATE TABLE Clients
(
	Id INT PRIMARY KEY IDENTITY,
	FirstName NVARCHAR(30) NOT NULL,
	LastName NVARCHAR(30) NOT NULL,
	Email NVARCHAR(50) NOT NULL,
	AddressId INT FOREIGN KEY REFERENCES Addresses(Id) NOT NULL,
)

CREATE TABLE ClientsCigars
(
	ClientId INT FOREIGN KEY REFERENCES Clients(Id) NOT NULL,
	CigarId INT FOREIGN KEY REFERENCES Cigars(Id) NOT NULL
	CONSTRAINT PK_ClientsCigars PRIMARY KEY (ClientId,CigarId)
)

--02
INSERT INTO Cigars(CigarName, BrandId, TastId, SizeId, PriceForSingleCigar, ImageURL)
VALUES('COHIBA ROBUSTO', 9, 1, 5, 15.50, 'cohiba-robusto-stick_18.jpg'),
	  ('COHIBA SIGLO I', 9, 1, 10, 410.00, 'cohiba-siglo-i-stick_12.jpg'),
	  ('HOYO DE MONTERREY LE HOYO DU MAIRE', 14, 5, 11, 7.50, 'hoyo-du-maire-stick_17.jpg'),
	  ('HOYO DE MONTERREY LE HOYO DE SAN JUAN', 14, 4, 15, 32.00, 'hoyo-de-san-juan-stick_20.jpg'),
	  ('TRINIDAD COLONIALES', 2, 3, 8, 85.21, 'trinidad-coloniales-stick_30.jpg')

INSERT INTO Addresses(Town, Country, Streat, ZIP) VALUES
('Sofia', 'Bulgaria', '18 Bul. Vasil levski', 1000),
('Athens', 'Greece', '4342 McDonald Avenue', 10435),
('Zagreb', 'Croatia', '4333 Lauren Drive', 10000)

--03
UPDATE Cigars
SET PriceForSingleCigar = PriceForSingleCigar + (PriceForSingleCigar*20/100)
WHERE TastId = 1

UPDATE Brands
SET BrandDescription = 'New description'
WHERE BrandDescription IS NULL

--04
DELETE FROM Clients
WHERE AddressId IN (SELECT Id FROM Addresses WHERE Country LIKE 'C%')

DELETE FROM Addresses
WHERE Country LIKE 'C%'

--05
SELECT 
	CigarName, 
	PriceForSingleCigar, 
	ImageURL 
FROM Cigars
ORDER BY PriceForSingleCigar ASC, CigarName DESC

--06
SELECT 
		c.Id,
		c.CigarName,
		c.PriceForSingleCigar,
        t.TasteType,
		t.TasteStrength
FROM Cigars AS c
JOIN Tastes AS t ON c.TastId = t.Id
WHERE t.TasteType IN ('Earthy', 'Woody')
ORDER BY c.PriceForSingleCigar DESC

--07
SELECT 
	Id,
	CONCAT_WS(' ', FirstName, LastName) AS ClientName,
	Email
FROM Clients
WHERE Id NOT IN (SELECT ClientId FROM ClientsCigars)
ORDER BY ClientName ASC

--08
SELECT
	c.CigarName,
	c.PriceForSingleCigar,
	c.ImageURL,
	s.RingRange
FROM Cigars AS c
JOIN Sizes AS s ON c.SizeId = s.Id
WHERE s.[Length] > 12 AND
c.CigarName LIKE '%ci%' OR
c.PriceForSingleCigar > 50 AND 
s.RingRange > 2.55
ORDER BY c.CigarName ASC, c.PriceForSingleCigar DESC

--09
SELECT 
	CONCAT_WS(' ', cl.FirstName, cl.LastName) FullName,
	a.Country,
	a.ZIP,
	CONCAT('$',MAX(c.PriceForSingleCigar)) AS CigarPrice
FROM Cigars AS c
JOIN ClientsCigars AS clc ON c.Id = clc.CigarId
JOIN Clients AS cl ON clc.ClientId = cl.Id
JOIN Addresses AS a ON cL.AddressId = a.Id
WHERE a.ZIP NOT LIKE '%[^0-9]%'
GROUP BY cl.FirstName, cl.LastName, a.Country, a.ZIP

--10
SELECT 
	cl.LastName,
	
	CEILING(s.RingRange)
FROM Cigars AS c
JOIN ClientsCigars AS clc ON c.Id = clc.CigarId
JOIN Clients AS cl ON clc.ClientId = cl.Id
JOIN Sizes AS s ON s.Id = c.SizeId
GROUP BY cl.LastName, s.RingRange

