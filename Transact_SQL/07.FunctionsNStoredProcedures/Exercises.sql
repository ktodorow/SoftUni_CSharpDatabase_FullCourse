--INCLUDED EXERCISES IN THIS QUERY
--01,02,03,04,05,07,10,11

--01
CREATE PROC usp_GetEmployeesSalaryAbove35000 
AS
BEGIN
	SELECT 
		   FirstName,
		    LastName
	  FROM Employees
	 WHERE Salary > 35000
END

--02
CREATE PROC usp_GetEmployeesSalaryAboveNumber(@Num DECIMAL(18,4))
AS
BEGIN
	SELECT 
		   FirstName,
		    LastName
	  FROM Employees
	 WHERE Salary >= @Num
END


--03
CREATE PROC usp_GetTownsStartingWith(@string VARCHAR(20))
AS
BEGIN
	SELECT 
		[Name]
	  FROM Towns
	 WHERE [Name] LIKE @string + '%'
END

--04
CREATE PROC usp_GetEmployeesFromTown(@string VARCHAR(20))
AS
BEGIN
	SELECT 
		FirstName,
	     LastName
	  FROM Employees AS e
	  JOIN Addresses AS a ON e.AddressID = a.AddressID
	  JOIN Towns as t ON a.TownID = t.TownID
	  WHERE t.[Name] = @string
END

--05
CREATE FUNCTION ufn_GetSalaryLevel(@Salary DECIMAL(18,4))
RETURNS VARCHAR(10)
AS
BEGIN
	DECLARE @result VARCHAR(10)

	IF(@Salary < 30000)
	BEGIN
		SET @result = 'Low'
	END
	
	IF(@Salary BETWEEN 30000 AND 50000)
	BEGIN
		SET @result = 'Average'
	END

		IF(@Salary BETWEEN 30000 AND 50000)
	BEGIN
		SET @result = 'Average'
	END

	IF(@Salary > 50000)
	BEGIN
		SET @result = 'High'
	END
	RETURN @result
END

--06
CREATE PROC usp_EmployeesBySalaryLevel(@string VARCHAR(10))
AS
BEGIN 
	SELECT 
		FirstName, 
		 LastName
	  FROM Employees
	  WHERE dbo.ufn_GetSalaryLevel(Salary) = @string
END

--07
CREATE FUNCTION ufn_IsWordComprised(@setOfLetters NVARCHAR(20), @word NVARCHAR(20))
RETURNS BIT
AS
BEGIN
		DECLARE @WordLength INT = LEN(@word)
		DECLARE @Iterator INT = 1

		WHILE(@Iterator <= @WordLength)
			BEGIN
				IF(CHARINDEX(SUBSTRING(@word, @Iterator, 1), @setOfLetters) = 0)
					RETURN 0
				SET @Iterator += 1
			  END
		   RETURN 1
END

--10
CREATE PROC usp_GetHoldersWithBalanceHigherThan(@balance MONEY)
AS
BEGIN
	SELECT 
		ah.FirstName AS [First Name],
		 ah.LastName AS  [Last Name]
	FROM AccountHolders AS ah
	JOIN Accounts AS a ON ah.Id = a.AccountHolderId
	GROUP BY FirstName, LastName
	HAVING SUM(a.Balance) > @balance
	ORDER BY FirstName, LastName
END

--11
CREATE FUNCTION ufn_CalculateFutureValue(@sum DECIMAL(10,4), @rate FLOAT, @years INT)
RETURNS DECIMAL(10,4)
AS
BEGIN
	RETURN @sum * (POWER((1 + @rate), @years))
END