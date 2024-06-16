--13. Departments Total Salaries
  SELECT
		DepartmentID, 
		SUM([Salary]) AS [TotalSalary]
    FROM Employees
GROUP BY DepartmentID

--14. Employees Minimum Salaries
  SELECT
		DepartmentID, 
		MIN([Salary]) AS [MinimumSalary]
    FROM Employees 
WHERE DepartmentID IN (2,5,7) AND HireDate > '01-01-2000'
GROUP BY DepartmentID

--15. Employees Average Salary
SELECT * INTO NewEmployeesTable
  FROM Employees
 WHERE Salary > 30000

DELETE FROM NewEmployeesTable WHERE ManagerID = 42

UPDATE NewEmployeesTable
   SET Salary = Salary + 5000
 WHERE DepartmentID = 1

   SELECT
		DepartmentID, 
		AVG([Salary]) AS [AverageSalary]
    FROM NewEmployeesTable
GROUP BY DepartmentID

--16. Employees Maximum Salaries
  SELECT
		DepartmentID, 
		MAX([Salary]) AS [MaxSalary]
    FROM Employees 
GROUP BY DepartmentID
HAVING MAX([Salary]) NOT BETWEEN 30000 AND 70000

--17. Employees Count Salaries
SELECT
		COUNT(Salary) AS [Count]
  FROM Employees
 WHERE ManagerID IS NULL

--18
 SELECT 
	   DepartmentID,
	   ThirdHighestSalary
   FROM
( 
		  SELECT
				DepartmentID,
				MAX(Salary) ThirdHighestSalary,
				DENSE_RANK() OVER (PARTITION BY DepartmentID ORDER BY Salary DESC) AS SalaryRank
			FROM Employees
		GROUP BY DepartmentID, Salary
) AS SubQuery
  WHERE SubQuery.SalaryRank = 3

--19
WITH AverageDepartmentSalary AS
(   
      SELECT
		    DepartmentID,
			AVG(Salary) AS AverageSalary
	    FROM Employees
	GROUP BY DepartmentID
)
SELECT TOP 10
	FirstName,
	LastName,
	e.DepartmentID
FROM Employees AS e
JOIN AverageDepartmentSalary as ads ON ads.DepartmentID = e.DepartmentID
WHERE e.Salary > ads.AverageSalary
ORDER BY DepartmentID