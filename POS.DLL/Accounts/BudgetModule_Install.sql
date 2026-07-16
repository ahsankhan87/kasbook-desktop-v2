-- =============================================
-- Budget Module - Quick Installation Script
-- Run this script to install the complete budget module
-- =============================================

PRINT '========================================';
PRINT 'Budget Module Installation Starting...';
PRINT '========================================';
PRINT '';

-- Check prerequisites
IF OBJECT_ID(N'dbo.fiscal_years', N'U') IS NULL
BEGIN
	PRINT 'ERROR: fiscal_years table not found. Please ensure accounting module is installed.';
	RAISERROR('Missing prerequisite table: fiscal_years', 16, 1);
	RETURN;
END

IF OBJECT_ID(N'dbo.acc_accounts', N'U') IS NULL
BEGIN
	PRINT 'ERROR: acc_accounts table not found. Please ensure accounting module is installed.';
	RAISERROR('Missing prerequisite table: acc_accounts', 16, 1);
	RETURN;
END

IF OBJECT_ID(N'dbo.acc_entries', N'U') IS NULL
BEGIN
	PRINT 'ERROR: acc_entries table not found. Please ensure accounting module is installed.';
	RAISERROR('Missing prerequisite table: acc_entries', 16, 1);
	RETURN;
END

PRINT 'Prerequisites check passed.';
PRINT '';

-- Execute the main installation script
PRINT 'Executing BudgetProcedures.sql...';
-- Note: Run BudgetProcedures.sql separately or use sqlcmd to execute it
-- This file just provides installation guidance

PRINT '';
PRINT '========================================';
PRINT 'Installation Instructions:';
PRINT '========================================';
PRINT '1. Execute: POS.DLL\Accounts\BudgetProcedures.sql';
PRINT '   This creates all tables, stored procedures, and types';
PRINT '';
PRINT '2. Rebuild solution to compile C# classes:';
PRINT '   - POS.Core\Accounts\BudgetModal.cs';
PRINT '   - POS.DLL\Accounts\BudgetDLL.cs';
PRINT '   - POS.BLL\Accounts\BudgetBLL.cs';
PRINT '';
PRINT '3. Review documentation:';
PRINT '   - POS.DLL\Accounts\BUDGET_MODULE_README.md';
PRINT '';
PRINT '========================================';
PRINT 'Post-Installation Verification';
PRINT '========================================';

-- Verification queries
IF OBJECT_ID(N'dbo.acc_budget_headers', N'U') IS NOT NULL
	PRINT '[OK] Table: acc_budget_headers';
ELSE
	PRINT '[PENDING] Table: acc_budget_headers';

IF OBJECT_ID(N'dbo.acc_budget_lines', N'U') IS NOT NULL
	PRINT '[OK] Table: acc_budget_lines';
ELSE
	PRINT '[PENDING] Table: acc_budget_lines';

IF OBJECT_ID(N'dbo.acc_budget_variance_notes', N'U') IS NOT NULL
	PRINT '[OK] Table: acc_budget_variance_notes';
ELSE
	PRINT '[PENDING] Table: acc_budget_variance_notes';

IF OBJECT_ID(N'dbo.sp_BudgetVsActual', N'P') IS NOT NULL
	PRINT '[OK] Procedure: sp_BudgetVsActual';
ELSE
	PRINT '[PENDING] Procedure: sp_BudgetVsActual';

IF OBJECT_ID(N'dbo.sp_BudgetMonthlyDetail', N'P') IS NOT NULL
	PRINT '[OK] Procedure: sp_BudgetMonthlyDetail';
ELSE
	PRINT '[PENDING] Procedure: sp_BudgetMonthlyDetail';

IF OBJECT_ID(N'dbo.sp_CopyBudgetFromActuals', N'P') IS NOT NULL
	PRINT '[OK] Procedure: sp_CopyBudgetFromActuals';
ELSE
	PRINT '[PENDING] Procedure: sp_CopyBudgetFromActuals';

IF OBJECT_ID(N'dbo.sp_BudgetSeasonalSpread', N'P') IS NOT NULL
	PRINT '[OK] Procedure: sp_BudgetSeasonalSpread';
ELSE
	PRINT '[PENDING] Procedure: sp_BudgetSeasonalSpread';

IF OBJECT_ID(N'dbo.sp_BudgetSummaryKPIs', N'P') IS NOT NULL
	PRINT '[OK] Procedure: sp_BudgetSummaryKPIs';
ELSE
	PRINT '[PENDING] Procedure: sp_BudgetSummaryKPIs';

IF EXISTS (SELECT 1 FROM sys.types WHERE name = 'MonthlyPercentagesType' AND is_table_type = 1)
	PRINT '[OK] Type: MonthlyPercentagesType';
ELSE
	PRINT '[PENDING] Type: MonthlyPercentagesType';

PRINT '';
PRINT '========================================';
PRINT 'Sample Data Creation (Optional)';
PRINT '========================================';

-- Sample: Create a test budget for the current fiscal year
/*
-- Uncomment to create sample budget

DECLARE @CurrentFiscalYearId INT;
DECLARE @SampleBudgetId INT;

-- Get current active fiscal year
SELECT TOP 1 @CurrentFiscalYearId = id
FROM fiscal_years
WHERE status = 'Active'
ORDER BY start_date DESC;

IF @CurrentFiscalYearId IS NOT NULL
BEGIN
	-- Create sample budget header
	INSERT INTO acc_budget_headers 
	(financial_year_id, budget_version, budget_name, status, created_by, created_at)
	VALUES 
	(@CurrentFiscalYearId, 'V1', 'Sample Operating Budget', 'Draft', 1, GETDATE());

	SET @SampleBudgetId = SCOPE_IDENTITY();

	-- Add sample budget lines for a few accounts
	INSERT INTO acc_budget_lines (budget_id, acc_id, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec)
	SELECT 
		@SampleBudgetId,
		a.id,
		10000, 10000, 10000, 10000, 10000, 10000,
		10000, 10000, 10000, 10000, 10000, 10000
	FROM acc_accounts a
	WHERE a.id IN (
		SELECT TOP 5 id 
		FROM acc_accounts 
		WHERE group_id IN (SELECT id FROM acc_groups WHERE account_type = 'Expense')
		ORDER BY code
	);

	PRINT 'Sample budget created with ID: ' + CAST(@SampleBudgetId AS VARCHAR(10));
END
ELSE
BEGIN
	PRINT 'No active fiscal year found. Sample budget not created.';
END
*/

PRINT '';
PRINT '========================================';
PRINT 'Installation Complete!';
PRINT '========================================';
PRINT 'Next Steps:';
PRINT '- Create budget UI forms (refer to README)';
PRINT '- Integrate CheckBudgetExceeded() into journal entry forms';
PRINT '- Add budget widgets to accounting dashboard';
PRINT '';

GO
