--1. Create Database
CREATE DATABASE Minions

--2. Create Tables
CREATE TABLE [Minions](
	[Id] INT NOT NULL,
	[Name] VARCHAR(20) NOT NULL,
	[Age] INT
)

--Add primary key as constraint
ALTER TABLE [Minions]
ADD CONSTRAINT [PK_MinionsId] PRIMARY KEY (Id)

CREATE TABLE [Towns] (
	[Id] INT NOT NULL,
	[Name] VARCHAR(20) NOT NULL,
)

ALTER TABLE [Minions]
ADD CONSTRAINT [PK_TownId] PRIMARY KEY (Id)

--3. Alter Minions Table
ALTER TABLE [Minions]
ADD [TownId] INT

--adding TownId foreign key and references to Id column in Towns table.
ALTER TABLE [Minions]
ADD FOREIGN KEY (TownId) REFERENCES Towns(Id)

--4. Insert Records in Both Tables
INSERT INTO [Towns] ([Id], [Name])
	VALUES(1, 'Sofia'),
		(2, 'Plovdiv'),
		(3, 'Varna')


INSERT INTO [Minions] ([Id], [Name], [Age], [TownId])
	VALUES(1, 'Kevin', 22, 1),
		(2, 'Bob', 15, 3),
		(3, 'Steward', NULL, 2)

--5. Truncate Table Minions
TRUNCATE TABLE Minions

--6. Drop All Tables
DROP TABLE Minions
DROP TABLE Towns

--7. Create Table People
CREATE TABLE [People](
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Name] NVARCHAR(200) NOT NULL,
	[Picture] VARBINARY(MAX),
	[Height] DECIMAL(2,2),
	[Weight] DECIMAL(5,2),
	[Gender] CHAR(1) NOT NULL,
		CHECK([Gender] in('m', 'f')),
	[Birthdate] DATETIME2 NOT NULL,
	[Biography] NVARCHAR(MAX)
)
--Make the Id a primary key
ALTER TABLE [People]
ADD PRIMARY KEY (Id)

--Populate the table with only 5 records
INSERT INTO [People]([Name], [Gender], [Birthdate], [Biography])
	VALUES('Chris', 'm', '2003-08-07', 'Student At SoftUni.'),
		('Kalina', 'f', '2001-12-01', 'Developer.'),
		('Petur', 'm', '1997-11-29', 'Just a cool guy.'),
		('Ioana', 'f', '2002-04-23', 'Model.'),
		('Boris', 'm', '1990-02-11', 'Singer.')

--Searching all from the table
SELECT * FROM [People]

--8. Create Table Users
CREATE TABLE [Users] (
	[Id] INT IDENTITY(1,1) NOT NULL,
	[Username] VARCHAR(30) NOT NULL,
	[Password] VARCHAR(26) NOT NULL,
	[ProfilePicture] VARBINARY(MAX),
		CHECK(DATALENGTH([ProfilePicture]) <= 900000),
	[LastLoginTime] DATETIME2,
	[IsDeleted] CHAR(1)
		CHECK([IsDeleted] in('false', 'true'))
)

--Make the Id a primary key
ALTER TABLE [Users]
ADD PRIMARY KEY (Id)

--Populate the table with only 5 records
INSERT INTO [Users]([Username], [Password], [LastLoginTime])
	VALUES('iluvmyrebs<3', '123456', '2024-08-07'),
		('peturVasilev23', 'kuchetoMiSeKazvaRi4i', '2024-12-01'),
		('minecraftPlayerN00B23', 'minecraft', '2024-11-29'),
		('kristyantodorov552', 'softuni', '2024-04-23'),
		('softuniTheBeST', 'thebest', '2024-02-11')

--Searching all from the table
SELECT * FROM [Users]

--9. Change Primary Key
ALTER TABLE [Users]
DROP CONSTRAINT PK__Users__3214EC07AF7A73F3

--new primary key that would be a combination of fields Id and Usernam
ALTER TABLE [Users]
ADD CONSTRAINT PK_UsersCompositeIdUsername PRIMARY KEY (Id, Username)

--10. Add Check Constraint	
ALTER TABLE [Users]
ADD CONSTRAINT [CHECK_UsersPasswordLength] CHECK (DATALENGTH([Password]) >= 5)

--TEST CHECK CONSTRAINT
INSERT INTO [Users]([Username], [Password], [LastLoginTime])
	VALUES('doesMyPassLengthIsEnough', '123', '2024-08-07')

--11. Set Default Value of a Field
ALTER TABLE [Users]
ADD CONSTRAINT [df_DateTime]
DEFAULT SYSDATETIME() FOR [LastLoginTime]

--Test default login time
INSERT INTO [Users]([Username], [Password])
	VALUES('test', '12235')


--12. Set Unique Field
--Remove Username field from the primary key so only the field Id would be primary key.
ALTER TABLE [Users]
DROP CONSTRAINT PK_UsersCompositeIdUsername

ALTER TABLE [Users]
ADD PRIMARY KEY (Id)

--add unique constraint
--???????

--13. Movies Database

--Directors (Id, DirectorName, Notes)
CREATE TABLE Directors(
	[Id] INT PRIMARY KEY NOT NULL,
	[Name] VARCHAR(30) NOT NULL,
	[Notes] VARCHAR(200)
)
--· Genres (Id, GenreName, Notes
CREATE TABLE Genres(
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[GenreName] VARCHAR(20) NOT NULL,
	[Notes] VARCHAR(200)
)
--Categories (Id, CategoryName, Notes)
CREATE TABLE Categories(
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[CategoryName] VARCHAR(20) NOT NULL,
	[Notes] VARCHAR(200)
)
--Movies 
CREATE TABLE Movies(
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[Title] VARCHAR(50) NOT NULL,
	[DirectorId] INT,
	[CopyrightYear] DATETIME2,
	[Length] DECIMAL(5,2) NOT NULL,
	[GenreId] INT,
	[CategoryId] INT,
	[Rating] DECIMAL(2,1),
	[Notes] VARCHAR(200)
)

INSERT INTO Directors ([Id], [Name])
	VALUES(1, 'Michael Showalter'),
		(2, 'Armado Iannucci'),
		(3, 'Josh Cooley'),
		(4, 'Lilly Wachowski'),
		(5, 'Lee Tamahori')

INSERT INTO Genres([GenreName])
	VALUES('Comedy'),
		('Action'),
		('Crime'),
		('Drama'),
		('Thriller')

INSERT INTO Categories([CategoryName])
	VALUES('R'),
		('TV-Ma'),
		('G'),
		('PG'),
		('PG-13')

ALTER TABLE Movies
ADD FOREIGN KEY ([DirectorId]) REFERENCES Directors(Id)

ALTER TABLE Movies
ADD FOREIGN KEY ([GenreId]) REFERENCES Genres(Id)

ALTER TABLE Movies
ADD FOREIGN KEY ([CategoryId]) REFERENCES Categories(Id)

INSERT INTO Movies([Title],[DirectorId],[Length], [GenreId], [CategoryId], [Rating])
	VALUES('Toy Story', 3, 1.40, 1, 3, 7.7),
		('The Idea of You', 1, 1.55, 1, 1, 6.4),
		('Veep', 2, 0.45, 4, 2, 8.4),
		('Once Were Warriors', 5, 1.42, 3, 1, 7.9),
		('The Matrix', 4, 1.40, 5, 1, 8.7)

SELECT * FROM Directors
SELECT * FROM Genres
SELECT * FROM Categories
SELECT * FROM Movies

