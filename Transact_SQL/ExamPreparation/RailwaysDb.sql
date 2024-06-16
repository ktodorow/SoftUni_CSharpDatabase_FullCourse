CREATE DATABASE RailwaysDb

GO

Use RailwaysDb


--01	

CREATE TABLE Towns
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(30) NOT NULL
)

CREATE TABLE Passengers
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(80) NOT NULL
)

CREATE TABLE RailwayStations 
(
	Id INT PRIMARY KEY IDENTITY,
	[Name] VARCHAR(50) NOT NULL,
	TownId INT FOREIGN KEY REFERENCES Towns(Id) NOT NULL
)

CREATE TABLE Trains
(
	Id INT PRIMARY KEY IDENTITY,
	HourOfDeparture VARCHAR(5) NOT NULL,
	HourOfArrival VARCHAR(5) NOT NULL,
	DepartureTownId INT FOREIGN KEY REFERENCES Towns(Id),
	ArrivalTownId INT FOREIGN KEY REFERENCES Towns(Id)
)

CREATE TABLE TrainsRailwayStations
(
	TrainId INT FOREIGN KEY REFERENCES Trains(Id) NOT NULL,
	RailwayStationId INT FOREIGN KEY REFERENCES RailwayStations(Id) NOT NULL,
	CONSTRAINT pk_trainsRailwayStations PRIMARY KEY(TrainId,RailwayStationId)
)

CREATE TABLE Tickets
(
	Id INT PRIMARY KEY IDENTITY,
	Price DECIMAL (10, 2),
	DateOfDeparture DATETIME2,
	DateOfArrival DATETIME2,
	TrainId INT FOREIGN KEY REFERENCES Trains(Id),
	PassengerId INT FOREIGN KEY REFERENCES Passengers(Id)
)

CREATE TABLE MaintenanceRecords
(
	Id INT PRIMARY KEY IDENTITY,
	DateOfMaintenance DATETIME2 NOT NULL,
	Details VARCHAR(2000) NOT NULL,
	TrainId INT FOREIGN KEY REFERENCES Trains(Id) NOT NULL
)

--02

INSERT INTO Trains(HourOfDeparture,HourOfArrival, DepartureTownId, ArrivalTownId)
       VALUES('07:00', '19:00', 1, 3),
			 ('08:30', '20:30', 5, 6),
			 ('09:00', '21:00', 4, 8),
			 ('06:45', '03:55', 27, 7),
			 ('10:15', '12:15', 15, 5)

INSERT INTO TrainsRailwayStations(TrainId, RailwayStationId)
       VALUES(36, 1),
	         (36, 4),
	         (36, 31),
	         (36, 57),
	         (36, 7),
	         (37, 13),
	         (37, 54),
	         (37, 60),
	         (37, 16),
	         (38, 10),
	         (38, 50),
	         (38, 52),
	         (38, 22),
			 (39, 68),
			 (39, 3),
			 (39, 31),
			 (39, 19),
			 (40, 41),
			 (40, 7),
			 (40, 52),
			 (40, 13)

INSERT INTO Tickets(Price, DateOfDeparture, DateOfArrival, TrainId, PassengerId)
       VALUES('90.00', '2023-12-01', '2023-12-01', 36, 1),
			 ('115.00', '2023-08-02', '2023-08-02', 37, 2),
			 ('160.00', '2023-08-03', '2023-08-03', 38, 3),
			 ('255.00', '2023-09-01', '2023-09-02', 39, 21),
			 ('95.00', '2023-09-02', '2023-09-03', 40, 22)

--03
UPDATE Tickets
SET DateOfDeparture = DATEADD(DAY, 7, DateOfDeparture),
	DateOfArrival = DATEADD(DAY, 7, DateOfArrival)
WHERE MONTH(DateOfDeparture) > 10

--04 
SELECT * FROM Trains AS t
JOIN Towns AS tw ON t.DepartureTownId = tw.Id

DELETE FROM TrainsRailwayStations
WHERE TrainId = 7

DELETE FROM Tickets
WHERE TrainId = 7

DELETE FROM MaintenanceRecords
WHERE TrainId = 7

DELETE FROM Trains 
WHERE DepartureTownId IN 
(SELECT Id FROM Towns WHERE [Name] = 'Berlin')


--05
SELECT DateOfDeparture,
	   Price AS [TicketPrice]
FROM Tickets
ORDER BY Price ASC, DateOfDeparture DESC 

--06
SELECT 
	p.[Name] AS [PassangerName],
	t.Price AS [TicketPrice],
	t.DateOfDeparture,
	t.TrainId AS [TrainID]
FROM Tickets AS t
JOIN Passengers AS p ON t.PassengerId = p.Id
ORDER BY PRICE DESC, p.[Name]

--07
------------------

--08
SELECT TOP 3
	t.Id,
	t.HourOfDeparture,
	ts.Price AS [TicketPrice],
	tw.[Name] as [Destination] 
FROM
		 Trains AS t
JOIN Tickets AS ts ON t.Id = ts.TrainId
JOIN Towns AS tw ON t.ArrivalTownId = tw.Id
WHERE CAST(HourOfDeparture AS datetime2) BETWEEN '08:00' AND '08:59' AND ts.Price > 50.00
ORDER BY ts.Price ASC

--09
SELECT 
	tw.[Name] AS [TownName],
	COUNT(tw.Id) AS [PassengersCount]
	FROM Passengers AS p
JOIN Tickets AS ts ON p.Id = ts.PassengerId
JOIN Trains AS tr ON ts.TrainId = tr.Id
LEFT JOIN Towns AS tw ON tr.ArrivalTownId = tw.Id
WHERE ts.Price > 76.99
GROUP BY tw.[Name]
ORDER BY tw.[Name]

--10
SELECT 
	m.TrainId AS [TrainID],
	t.[Name] AS [DepartureTown],
	Details
FROM MaintenanceRecords AS m
JOIN Trains AS tr ON m.TrainId = tr.Id
JOIN Towns AS t ON tr.DepartureTownId = T.Id
WHERE m.Details LIKE '%inspection%'
ORDER BY tr.Id

--11
CREATE FUNCTION udf_TownsWithTrains(@name VARCHAR(20))
RETURNS INT 
AS
BEGIN
	DECLARE @count INT

	SET @count =
	(SELECT
		COUNT(t.Id)
	FROM Trains AS t
	JOIN Towns AS ta ON t.ArrivalTownId = ta.Id
	JOIN Towns AS td ON t.DepartureTownId = td.Id
	WHERE td.[Name] = @name OR ta.[Name] = @name)

	RETURN @count
END

--12
CREATE PROC usp_SearchByTown(@townName VARCHAR(30))
AS
BEGIN
	SELECT 
		p.[Name] AS [PassengerName],
		ts.DateOfDeparture,
		tr.HourOfDeparture
	FROM Passengers AS p
	JOIN Tickets AS ts ON ts.PassengerId = p.Id 
	JOIN Trains AS tr ON ts.TrainId = tr.Id
	JOIN Towns AS t ON tr.ArrivalTownId= t.Id 
	WHERE t.[Name] = @townName
	ORDER BY ts.DateOfDeparture DESC,
			 p.[Name] ASC
END

EXEC usp_SearchByTown 'Berlin'