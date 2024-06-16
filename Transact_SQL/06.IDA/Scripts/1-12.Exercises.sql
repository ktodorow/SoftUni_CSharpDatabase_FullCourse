--01. Records’ Count
SELECT COUNT(*) 
	AS [Count] 
  FROM [WizzardDeposits]

--02. Longest Magic Wand
SELECT 
   MAX([MagicWandSize]) 
    AS [LongestMagicWand] 
  FROM [WizzardDeposits]

--03. Longest Magic Wand per Deposit Groups
  SELECT 
	     [DepositGroup],
     MAX([MagicWandSize]) 
      AS [LongestMagicWand] 
    FROM [WizzardDeposits] 
GROUP BY DepositGroup

--04. Smallest Deposit Group Per Magic Wand Size
  SELECT 
     TOP 2
	     [DepositGroup]
    FROM [WizzardDeposits] 
GROUP BY DepositGroup
ORDER BY AVG([MagicWandSize]) 

--5. Deposits Sum
  SELECT 
	     [DepositGroup],
     SUM([DepositAmount]) 
      AS [LongestMagicWand] 
    FROM [WizzardDeposits] 
GROUP BY DepositGroup

--6. Deposits Sum for Ollivander Family
  SELECT 
	     [DepositGroup],
     SUM([DepositAmount]) 
      AS [TotalSum] 
    FROM [WizzardDeposits] 
WHERE [MagicWandCreator] = 'Ollivander family'
GROUP BY DepositGroup

--7. Deposits Filter
  SELECT 
	     [DepositGroup],
     SUM([DepositAmount]) 
      AS [TotalSum] 
    FROM [WizzardDeposits] 
   WHERE [MagicWandCreator] = 'Ollivander family'
GROUP BY DepositGroup
  HAVING SUM([DepositAmount]) < 150000
ORDER BY [TotalSum] DESC

--8. Deposit Charge
  SELECT 
	     [DepositGroup],
		 [MagicWandCreator],
     MIN([DepositCharge])
      AS [MinDepositCharge] 
    FROM [WizzardDeposits] 
GROUP BY DepositGroup, MagicWandCreator
ORDER BY MagicWandCreator, DepositGroup ASC

--9. Age groups
SELECT
	  AgeGroup,
	  COUNT(Id) AS [WizardCount]
  FROM
(
		  SELECT *,
			CASE
				WHEN [Age] BETWEEN  0 AND 10 THEN '[0-10]'
				WHEN [Age] BETWEEN 11 AND 20 THEN '[11-20]'
				WHEN [Age] BETWEEN 21 AND 30 THEN '[21-30]'
				WHEN [Age] BETWEEN 31 AND 40 THEN '[31-40]'
				WHEN [Age] BETWEEN 41 AND 50 THEN '[41-50]'
				WHEN [Age] BETWEEN 51 AND 60 THEN '[51-60]'
				WHEN [Age] > 60 THEN '[61+]'
			END AS [AgeGroup]
		FROM [WizzardDeposits] 

) AS [AgeGroupSubquery]
GROUP BY [AgeGroup]

--10. FirstLetter
  SELECT FirstLetter FROM
(   
    SELECT 
		SUBSTRING(FirstName, 1, 1) AS [FirstLetter]
	  FROM WizzardDeposits
	 WHERE DepositGroup = 'Troll Chest'
) AS [Subquery]
GROUP BY FirstLetter

--11. AverageInterest
  SELECT 
		 DepositGroup,
		 IsDepositExpired,
		 AVG(DepositInterest) AS [AverageInterest]
    FROM WizzardDeposits
   WHERE DepositStartDate > '01-01-1985'
GROUP BY DepositGroup, IsDepositExpired
ORDER BY DepositGroup DESC, IsDepositExpired ASC 


--12.Rich Wizard, Poor Wizard
SELECT 
	SUM([Difference]) AS [SumDifference]
 FROM 
(
	SELECT 
		FirstName	  AS [Host Wizard],
		DepositAmount AS [Host Wizard Deposit],
		LEAD(FirstName)     OVER (ORDER BY Id) AS [Guest Wizard],
		LEAD(DepositAmount) OVER (ORDER BY Id) AS [Guest Wizard Deposit],
		(DepositAmount - LEAD(DepositAmount)   OVER (ORDER BY Id)) AS [Difference]
	  FROM WizzardDeposits
) AS Subquery