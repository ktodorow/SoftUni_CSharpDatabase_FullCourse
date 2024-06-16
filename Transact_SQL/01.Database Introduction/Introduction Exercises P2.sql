CREATE TABLE [Categories] (
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[CategoryName] VARCHAR(20) NOT NULL,
	[DailyRate] DECIMAL (1,1),
	[MonthlyRate] DECIMAL (1,1),
	[WeekendRate] DECIMAL (1,1)
)

CREATE TABLE [Cars] (
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[PlateNumber] VARCHAR(10) NOT NULL,
	[Manufacturer] VARCHAR(20) NOT NULL,
	[Model] VARCHAR(20) NOT NULL,
	[CarYear] DATETIME2,
	[CategoryId] INT FOREIGN KEY REFERENCES Categories([Id]),
	[Doors] TINYINT NOT NULL,
	[Picture] VARBINARY(MAX),
	[Condition] VARCHAR(10),
		CHECK([Condition] in('Bad', 'Good', 'New')),
	[Available] VARCHAR(10) NOT NULL,
		CHECK([Available] in('Yes', 'No')),
)

CREATE TABLE [Employees]
(
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[FirstName] VARCHAR(20) NOT NULL,
	[LastName] VARCHAR(20) NOT NULL,
	[Title] VARCHAR(30) NOT NULL,
	[Notes] VARCHAR(200)
)

CREATE TABLE [Customers]
(
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[DriverLicenceNumber] TINYINT,
	[FullName] VARCHAR(60) NOT NULL,
	[Address] VARCHAR(100),
	[City] VARCHAR(100) NOT NULL,
	[ZIPCode] INT NOT NULL,
	[Notes] VARCHAR(200)
)

CREATE TABLE RentalOrders
(
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[EmployeeId] INT FOREIGN KEY REFERENCES Employees(Id) NOT NULL,
	[CustomerId] INT FOREIGN KEY REFERENCES Customers(Id) NOT NULL,
	[CarId] INT FOREIGN KEY REFERENCES Cars(Id) NOT NULL,
	[TankLevel] DECIMAL (5,2),
	[KilometrageStart] DECIMAL (5,2),
	[KilometrageEnd] DECIMAL (5,2),
	[TotalKilometrage] DECIMAL (5,2),
	[StartDate] DATETIME2,
	[EndDate] DATETIME2,
	[TotalDays] TINYINT,
	[RateApplied] DECIMAL(1,1),
	[TaxRate] DECIMAL(1,1),
	[OrderStatus] VARCHAR(20) NOT NULL,
	[Notes] VARCHAR(100)
)

INSERT INTO [Categories] (CategoryName)
	VALUES('AnyCategory'),
		('AnyCategory'),
		('AnyCategory')

INSERT INTO [Cars] ([PlateNumber], [Manufacturer], [Model], [Doors], [Condition], [Available])
	VALUES('P5124KP', 'Volkswagen', 'GOLF IV', 4, 'Good', 'Yes'),
		('CB5124P', 'BMW', 'M3', 4, 'Bad', 'Yes'),
		('E2445CC', 'Tesla', 'Model Y', 4, 'New', 'No')

INSERT INTO [Employees] (FirstName, LastName, Title)
	VALUES('Pesho', 'Petrov', 'Manager'),
		('Antonio', 'Todorov', 'Seller'),
		('Dimitar', 'Lyapchev', 'CEO')

INSERT INTO [Customers] (FullName, City, ZIPCode)
	VALUES('Kristiyan Todorov', 'Rousse', 7000),
		('Valentin Todorov', 'Sofia', 1756),
		('Dimitar Zapryanov', 'Varna', 9000)

INSERT INTO RentalOrders (EmployeeId, CustomerId, CarId, OrderStatus)
	VALUES(2, 1, 1, 'Preparing documents'),
		(1, 3, 2, 'Closed'),
		(3, 2, 3, 'Bought')