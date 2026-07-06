-- =====================================================
-- SQL Script: Generate Unique Hierarchical Codes
-- Purpose: Assign unique codes to existing groups and accounts
--          following the hierarchical numbering scheme:
--          Assets: 1000-1999
--          Liabilities: 2000-2999
--          Equity: 3000-3999
--          Income: 4000-4999
--          Expenses: 5000-5999
-- =====================================================

-- Step 1: Create temporary table for code assignment
IF OBJECT_ID('tempdb..#CodeAssignment') IS NOT NULL
	DROP TABLE #CodeAssignment;

CREATE TABLE #CodeAssignment (
	Id INT,
	TableName NVARCHAR(20),
	ParentId INT,
	CurrentCode NVARCHAR(50),
	NewCode NVARCHAR(50),
	AccountTypeId INT
);

-- =====================================================
-- GROUPS CODE ASSIGNMENT
-- =====================================================

-- Step 2: Assign codes to Level-1 groups (root groups by account type)
-- Assets: 1000, Liabilities: 2000, Equity: 3000, Income: 4000, Expenses: 5000

DECLARE @AssetTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%asset%');
DECLARE @LiabilityTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%liabil%');
DECLARE @EquityTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%equity%');
DECLARE @IncomeTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%income%' OR LOWER(name) LIKE '%revenue%');
DECLARE @ExpenseTypeId INT = (SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE '%expense%');

-- Update Level-1 Groups (parent_id = 0 or NULL)
UPDATE acc_groups
SET code = CASE 
	WHEN account_type_id = @AssetTypeId THEN '1000'
	WHEN account_type_id = @LiabilityTypeId THEN '2000'
	WHEN account_type_id = @EquityTypeId THEN '3000'
	WHEN account_type_id = @IncomeTypeId THEN '4000'
	WHEN account_type_id = @ExpenseTypeId THEN '5000'
	ELSE '9000'
END
WHERE (parent_id = 0 OR parent_id IS NULL)
  AND (code IS NULL OR code = '' OR code = '0');

-- Step 3: Assign codes to Level-2 groups (sub-groups under Level-1)
-- Generate codes incrementally within the parent's range
WITH L2Groups AS (
	SELECT 
		g.id,
		g.parent_id,
		g.account_type_id,
		ROW_NUMBER() OVER (PARTITION BY g.parent_id ORDER BY g.id) AS RowNum,
		p.code AS ParentCode
	FROM acc_groups g
	INNER JOIN acc_groups p ON g.parent_id = p.id
	WHERE g.parent_id > 0
	  AND (g.code IS NULL OR g.code = '' OR g.code = '0')
)
UPDATE acc_groups
SET code = ParentCode + SUBSTRING(RIGHT('00' + CAST(RowNum AS VARCHAR), 2), 1, 2)
FROM L2Groups l2
WHERE acc_groups.id = l2.id;

-- =====================================================
-- ACCOUNTS CODE ASSIGNMENT
-- =====================================================

-- Step 4: Assign codes to accounts (Level-3)
-- Generate codes incrementally within the parent group's range
WITH AccountsWithCodes AS (
	SELECT 
		a.id,
		a.group_id,
		a.branch_id,
		ROW_NUMBER() OVER (PARTITION BY a.group_id, a.branch_id ORDER BY a.id) AS RowNum,
		g.code AS GroupCode
	FROM acc_accounts a
	INNER JOIN acc_groups g ON a.group_id = g.id
	WHERE (a.code IS NULL OR a.code = '' OR a.code = '0')
)
UPDATE acc_accounts
SET code = GroupCode + '-' + RIGHT('000' + CAST(RowNum AS VARCHAR), 3)
FROM AccountsWithCodes awc
WHERE acc_accounts.id = awc.id;

-- =====================================================
-- VERIFICATION QUERIES
-- =====================================================

-- Verify Level-1 Groups assignment
SELECT 'Level-1 Groups' AS Category, id, parent_id, code, name, account_type_id
FROM acc_groups
WHERE parent_id = 0 OR parent_id IS NULL
ORDER BY code;

-- Verify Level-2 Groups assignment
SELECT 'Level-2 Groups' AS Category, g.id, g.parent_id, g.code, g.name, p.code AS ParentCode, g.account_type_id
FROM acc_groups g
LEFT JOIN acc_groups p ON g.parent_id = p.id
WHERE g.parent_id > 0
ORDER BY g.code;

-- Verify Accounts assignment (sample: first 50)
SELECT TOP 50 'Accounts' AS Category, a.id, a.group_id, a.code, a.name, g.code AS GroupCode, a.branch_id
FROM acc_accounts a
INNER JOIN acc_groups g ON a.group_id = g.id
ORDER BY a.code;

-- Summary Statistics
SELECT 
	'Level-1 Groups' AS Category, COUNT(*) AS TotalRecords, 
	SUM(CASE WHEN code IS NOT NULL AND code != '' AND code != '0' THEN 1 ELSE 0 END) AS AssignedCodes
FROM acc_groups
WHERE parent_id = 0 OR parent_id IS NULL
UNION ALL
SELECT 
	'Level-2 Groups', COUNT(*), 
	SUM(CASE WHEN code IS NOT NULL AND code != '' AND code != '0' THEN 1 ELSE 0 END)
FROM acc_groups
WHERE parent_id > 0
UNION ALL
SELECT 
	'Accounts', COUNT(*), 
	SUM(CASE WHEN code IS NOT NULL AND code != '' AND code != '0' THEN 1 ELSE 0 END)
FROM acc_accounts;

-- =====================================================
-- CLEANUP
-- =====================================================
DROP TABLE IF EXISTS #CodeAssignment;

-- =====================================================
-- NOTE: Run this script in parts if there are conflicts:
-- 1. First run Level-1 Groups update
-- 2. Then run Level-2 Groups update
-- 3. Finally run Accounts update
-- This ensures parent codes are set before child codes reference them.
-- =====================================================
