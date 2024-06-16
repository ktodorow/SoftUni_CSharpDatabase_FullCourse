--One-To-One Relationship

CREATE TABLE [Passports] (
	PassportID INT PRIMARY KEY IDENTITY(101,1),
	PassportNumber VARCHAR(8)
)

CREATE TABLE [Persons] (
	PersonID INT PRIMARY KEY IDENTITY,
	FirstName VARCHAR(60),
	Salary DECIMAL(7, 2),
	PassportID INT FOREIGN KEY REFERENCES Passports(PassportID)
)

INSERT INTO Passports(PassportNumber)
VALUES('N34FG21B'),
	('K65LO4R7'),
	('ZE657QP2')

INSERT INTO Persons(FirstName, Salary, PassportID)
VALUES('Roberto', 43300.00, 102),
	  ('Tom', 56100.00, 103),
	  ('Roberto', 60200.00, 101)

--One-To-Many Relationship
CREATE TABLE [Manufacturers] (
	ManufacturerID INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(20),
	EstablishedOn DATETIME2
)

CREATE TABLE [Models] (
	ModelID INT PRIMARY KEY IDENTITY (101, 1),
	[Name] VARCHAR(20),
	ManufacturerID INT FOREIGN KEY REFERENCES [Manufacturers](ManufacturerID)
)

INSERT INTO Manufacturers([Name], EstablishedOn)
VALUES('BMW', '07/03/1916'),
	('Tesla', '01/01/2003'),
	('Lada', '01/05/1966')

INSERT INTO [Models]([Name], [ManufacturerID])
VALUES('X1', 1),
	('i6', 1),
	('Model S', 2),
	('Model X', 2),
	('Model 3', 2),
	('Nova', 3)

SELECT * FROM Models
SELECT * FROM Manufacturers

CREATE TABLE Exams(
	ExamID INT PRIMARY KEY IDENTITY(101,1),
	[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Students(
	StudentID INT PRIMARY KEY IDENTITY(1,1),
	[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE StudentsExams(
	StudentID INT FOREIGN KEY REFERENCES Students(StudentID),
	ExamID INT FOREIGN KEY REFERENCES Exams(ExamID),
	PRIMARY KEY(StudentID, ExamID)
)

INSERT INTO Students([Name])
	 VALUES('Mila'),
		('Toni'),
		('Ron')

INSERT INTO Exams([Name])
	 VALUES('SpringMVC'),
		('Neo4j'),
		('Oracle 11g')

		
INSERT INTO StudentsExams(StudentID, ExamID)
	 VALUES(1, 101),
		(1, 102),
		(2, 101),
		(3, 103),
		(2, 102),
		(2, 103)

CREATE TABLE Teachers(
	TeacherID INT PRIMARY KEY IDENTITY(101, 1),
	[Name] VARCHAR(60) NOT NULL,
	ManagerID INT FOREIGN KEY REFERENCES Teachers(TeacherID)
)

INSERT INTO Teachers([Name], [ManagerID])
VALUES('John', NULL),
	('Maya', 106),
	('Silvia', 106),
	('Ted', 105),
	('Mark', 101),
	('Greta', 101)