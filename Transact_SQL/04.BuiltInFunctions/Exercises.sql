--01
SELECT FirstName,
	   LastName
  FROM Employees
 WHERE FirstName LIKE 'Sa%'

--02
SELECT FirstName,
	   LastName
  FROM Employees
 WHERE LastName LIKE '%ei%'

--03
SELECT [FirstName] 
  FROM [Employees]
 WHERE [DepartmentID] IN (3,10) AND YEAR([HireDate]) BETWEEN 1992 AND 2005

--04
SELECT FirstName,
	   LastName
  FROM Employees
 WHERE NOT JobTitle LIKE '%engineer%'
 
--05
SELECT [Name]
FROM [Towns]
WHERE LEN([Name]) = 5 OR LEN([Name]) = 6
ORDER BY [Name]

--06
SELECT [TownId], [Name]
FROM [Towns]
WHERE [Name] LIKE '[MKBE]%'
ORDER BY [Name]

--07
SELECT [TownId], [Name]
FROM [Towns]
WHERE NOT [Name] LIKE '[RBD]%'
ORDER BY [Name]

--08
CREATE VIEW V_EmployeesHiredAfter2000
AS
   SELECT [FirstName], 
		   [LastName]
	 FROM [Employees]
     WHERE DATEPART(YEAR, HireDate) > 2000;

--09
SELECT [FirstName],
	   [LastName]
  FROM Employees
 WHERE LEN([LastName]) = 5

--10
  SELECT [EmployeeID], 
	      [FirstName], 
	       [LastName], 
	         [Salary],
		DENSE_RANK() OVER (PARTITION BY [Salary] ORDER BY [EmployeeID]) AS [Rank]
	FROM Employees
   WHERE [Salary] BETWEEN 10000 AND 50000
ORDER BY [Salary] DESC

--11
SELECT * FROM
(
	SELECT [EmployeeID], 
	      [FirstName], 
	       [LastName], 
	         [Salary],
		DENSE_RANK() OVER (PARTITION BY [Salary] ORDER BY [EmployeeID]) AS [Rank]
	FROM Employees
   WHERE [Salary] BETWEEN 10000 AND 50000 
)
      AS [RankingTable]
   WHERE [Rank] = 2
ORDER BY [Salary] DESC

--12
  SELECT  [CountryName]
	  AS [Country Name],
			  [IsoCode] 
	  AS	 [Iso Code]
    FROM    [Countries]
   WHERE  [CountryName] LIKE '%a%a%a%'
ORDER BY     [Iso Code]

--13
 SELECT
	 p.[PeakName],
	r.[RiverName],
	LOWER(CONCAT(LEFT(p.[PeakName], LEN(p.[PeakName]) - 1), r.[RiverName]))
				AS [Mix]
    FROM  [Peaks] AS p,
	     [Rivers] AS r
   WHERE LOWER(RIGHT(p.[PeakName], 1)) = LOWER(LEFT(r.[RiverName], 1))
ORDER BY [Mix]

--14
SELECT TOP 50  
		[Name],
	   [Start] = FORMAT([Start], 'yyyy-MM-dd')
  FROM [Games]
 WHERE YEAR([Start]) BETWEEN 2011 AND 2012
 ORDER BY [Start], [Name]

--15
  SELECT       [Username],
		SUBSTRING([Email], CHARINDEX('@', [Email]) + 1, 100) AS [Email Provider]
    FROM          [Users]
ORDER BY [Email Provider], 
			   [Username]

--16
  SELECT   [Username],
	      [IpAddress] 
	  AS [IP Address]
    FROM      [Users]
   WHERE  [IpAddress] LIKE '___.1_%._%.___'
ORDER BY   [Username]

--17
SELECT [Name] 
	AS [Game],
	   CASE
			WHEN DATEPART(HOUR, [Start]) >= 0 AND DATEPART(HOUR, [Start]) < 12 THEN 'Morning'
			WHEN DATEPART(HOUR, [Start]) >= 12 AND DATEPART(HOUR, [Start]) < 18 THEN 'Afternoon'
			ELSE 'Evening'
	    END
	AS [Part Of the Day],
	   CASE
			WHEN [Duration] <= 3 THEN 'Extra Short'
			WHEN [Duration] BETWEEN 4 AND 6 THEN 'Short'
			WHEN [Duration] > 6 THEN 'Long'
			ELSE 'Extra Long'
	    END
	AS [Duration]
	FROM [Games]
ORDER BY [Name], [Duration]