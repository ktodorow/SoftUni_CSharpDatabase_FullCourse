CREATE TABLE [Employees] (
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[FirstName] VARCHAR(20) NOT NULL,
	[LastName] VARCHAR(20) NOT NULL,
	[Title] VARCHAR(100),
	[Notes] VARCHAR(200),
)

CREATE TABLE [Customers] (
	[AccountNumber] INT PRIMARY KEY IDENTITY NOT NULL,
	[FirstName] VARCHAR(20) NOT NULL,
	[LastName] VARCHAR(20) NOT NULL,
	[PhoneNumber] INT NOT NULL,
	[EmergencyName] VARCHAR(30),
	[EmergencyNumber] INT NOT NULL,
	[Notes] VARCHAR(200)
)

CREATE TABLE [RoomStatus] (
	[RoomStatus] VARCHAR(10) PRIMARY KEY NOT NULL,
		CHECK([RoomStatus] in ('Open' , 'Busy', 'Closed')),
	[Notes] VARCHAR(200)
)


CREATE TABLE [RoomTypes] (
	[RoomTypes] VARCHAR(10) PRIMARY KEY NOT NULL,
		CHECK([RoomTypes] in ('Small' , 'Medium', 'Vip')),
	[Notes] VARCHAR(200)
)

CREATE TABLE [BedTypes] (
	[BedTypes] VARCHAR(10) PRIMARY KEY NOT NULL,
		CHECK([BedTypes] in ('Small' , 'Medium', 'Vip')),
	[Notes] VARCHAR(200)
)

CREATE TABLE [Rooms] (
	[RoomNumber] INT PRIMARY KEY IDENTITY (100,1) NOT NULL,
	[RoomTypes] VARCHAR(10) FOREIGN KEY REFERENCES RoomTypes([RoomTypes]) NOT NULL,
	[BedTypes] VARCHAR(10) FOREIGN KEY REFERENCES BedTypes([BedTypes]) NOT NULL,
	[Rate] DECIMAL(1,1),
	[RoomStatus] VARCHAR(10) FOREIGN KEY REFERENCES RoomStatus([RoomStatus]) NOT NULL,
	[Notes] VARCHAR(200)
)

CREATE TABLE [Payments] (
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[EmployeeId] INT FOREIGN KEY REFERENCES Employees([Id]) NOT NULL,
	[PaymentDate] DATETIME2 NOT NULL,
	[AccountNumber] INT FOREIGN KEY REFERENCES Customers([AccountNumber]) NOT NULL,
	[FirstDateOccupied] DATETIME2,
	[LastDateOccupied] DATETIME2,	
	[TotalDays] INT,
	[AmountCharged] DECIMAL(5,2),
	[TaxRate] DECIMAL(1,1),
	[TaxAmount] INT,
	[PaymentTotal] INT,
	[Notes] VARCHAR(200)
)

CREATE TABLE [Occupancies] (
	[Id] INT PRIMARY KEY IDENTITY NOT NULL,
	[EmployeeId] INT FOREIGN KEY REFERENCES Employees([Id]) NOT NULL,
	[DateOccupied] DATETIME2,
	[AccountNumber] INT FOREIGN KEY REFERENCES Customers([AccountNumber]) NOT NULL,
	[RoomNumber] INT FOREIGN KEY REFERENCES Rooms([RoomNumber]),
	[RateApplied] DECIMAL(5,2),
	[PhoneCharge] INT,
	[Notes] VARCHAR(200)
)

INSERT INTO Employees([FirstName], [LastName])
	VALUES('Todor', 'Toshkov'),
		('Ivo', 'Bojanov'),
		('Ivan', 'Seherov')

INSERT INTO Customers([FirstName], [LastName], [PhoneNumber], [EmergencyNumber])
	VALUES('Kris', 'Todorov', 0882148235, 00101),
		('Ivailo', 'Tejkerov', 0872245169, 00107),
		('Dimitrichko', 'Dimitrichkov', 0985268175, 00205)

INSERT INTO RoomStatus([RoomStatus])
	VALUES('Open'),
		('Busy'),
		('Closed')

INSERT INTO RoomTypes([RoomTypes])
	VALUES('Small'),
		('Medium'),
		('Vip')

INSERT INTO BedTypes([BedTypes])
	VALUES('Small'),
		('Medium'),
		('Vip')

INSERT INTO Rooms([RoomTypes], [BedTypes], [RoomStatus])
	VALUES('Small', 'Small', 'Closed'),
		('Vip', 'Vip', 'Busy'),
		('Medium', 'Small', 'Open')

INSERT INTO Payments([EmployeeId], [PaymentDate], [AccountNumber])
	VALUES(3, '2024-12-03', 1),
		(1, '2024-11-02', 2),
		(3, '2024-10-01', 3)

INSERT INTO Occupancies([EmployeeId], [AccountNumber], [RoomNumber])
	VALUES(1, 1, 100),
		(2, 3, 102),
		(3, 2, 101)