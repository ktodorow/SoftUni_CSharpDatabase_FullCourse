-- employees with no project
--TIP: u can use left and right OUTER joins without the OUTER word
SELECT 
	e.FirstName,
	e.LastName,
	p.[Name] AS ProjectName
FROM [Employees] AS e
LEFT JOIN [EmployeesProjects] AS ep ON e.EmployeeID = ep.EmployeeID
LEFT JOIN Projects as p ON ep.ProjectID = p.ProjectID

-- projects with no employees
SELECT 
	e.FirstName,
	e.LastName,
	p.[Name] AS ProjectName
FROM [Employees] AS e
RIGHT JOIN [EmployeesProjects] AS ep ON e.EmployeeID = ep.EmployeeID
RIGHT JOIN Projects as p ON ep.ProjectID = p.ProjectID

-- employees with no projects AND projects with no employees
SELECT 
	e.FirstName,
	e.LastName,
	p.[Name] AS ProjectName
FROM [Employees] AS e
FULL JOIN [EmployeesProjects] AS ep ON e.EmployeeID = ep.EmployeeID
FULL JOIN Projects as p ON ep.ProjectID = p.ProjectID

-- INNER JOIN, get all employees with projects
--TIP: U can use inner join with or without the INNER word
SELECT 
	e.FirstName,
	e.LastName,
	p.[Name] AS ProjectName
FROM [Employees] AS e
JOIN [EmployeesProjects] AS ep ON e.EmployeeID = ep.EmployeeID
JOIN Projects as p ON ep.ProjectID = p.ProjectID

--Display address information of all employees.
--Select first 50
SELECT 
	 e.[FirstName],
	  e.[LastName],
	      t.[Name] 
	     AS [Town],
   a.[AddressText]
  FROM [Employees]   AS e
    JOIN [Addresses] AS a ON e.AddressID = a.AddressID
    JOIN     [Towns] AS t ON a.TownID = t.TownID
ORDER BY [FirstName], 
         [LastName] ASC
SELECT * FROM [Employees]
--Display all employees that are hired after 1/1/99 and are either in sales or finance
	 SELECT 
			e.[FirstName],
			 e.[LastName],
			 e.[HireDate],
				 d.[Name] AS [DeptName]
	   FROM   [Employees] AS e
	   JOIN [Departments] AS d ON e.DepartmentID = d.DepartmentID
	  WHERE
		e.HireDate > '1999-01-01'
		AND d.[Name] IN ('Sales', 'Finance')
   ORDER BY [HireDate] ASC

--Display information about employees manager and employees department
	SELECT TOP 50
	      e.[EmployeeID],
		  CONCAT_WS(' ', e.[FirstName], e.[LastName]) 
        				 AS    [EmployeeName],
		  CONCAT_WS(' ', m.[FirstName], m.[LastName]) 
        				 AS     [ManagerName],
		        d.[Name] AS  [DepartmentName]
	  FROM   [Employees] AS e
 LEFT JOIN   [Employees] AS m ON e.ManagerID = m.EmployeeID
 LEFT JOIN [Departments] AS d ON e.DepartmentID = d.DepartmentID
  ORDER BY [EmployeeID] ASC