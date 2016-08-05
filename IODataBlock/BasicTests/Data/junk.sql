SELECT customers.name,customers.firstname,regions.city 
FROM [dbo].[customers] 
INNER JOIN [dbo].[regions] 
ON customers.zip = regions.zip 
WHERE regions.city LIKE 'do%' 
GROUP BY regions.city, customers.name, customers.firstname 
HAVING COUNT(*) > 5 
ORDER BY customers.name ASC


SELECT customers.name,customers.firstname,regions.city 
FROM [dbo].[customers] 
INNER JOIN [dbo].[regions] 
ON customers.zip = regions.zip 
WHERE regions.city LIKE 'do%' 
GROUP BY regions.city, customers.name, customers.firstname 
HAVING COUNT(*) > 5 
ORDER BY customers.name ASC



SELECT * FROM [dbo].[customers] WHERE zip IN ('58965','47841','12569') ORDER BY name DESC
SELECT * FROM [dbo].[customers] WHERE zip IN ('58965','47841','12569') ORDER BY name DESC


SELECT TOP 100 * FROM [dbo].[customers] WHERE age < 55 AND (name LIKE 'jo%' OR name LIKE 'pe%')
SELECT TOP 100 * FROM [dbo].[customers] WHERE age < 55 AND (name LIKE 'jo%' OR name LIKE 'pe%')