-- ============================================================
-- Company Setup Module — Schema, Seed Data & COA Procedure
-- Run order:
--   1) Alter pos_settings       (add missing columns)
--   2) acc_wht_rates            (withholding tax rate table)
--   3) mst_company              (single-row company master)
--   4) Seed pos_settings        (accounting defaults)
--   5) sp_SetupStandardCOA      (80+ standard accounts)
-- ============================================================

SET NOCOUNT ON;
GO

-- ============================================================
-- 1. Alter pos_settings — add columns that don't yet exist
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_settings' AND COLUMN_NAME = 'setting_type')
	ALTER TABLE dbo.pos_settings ADD setting_type VARCHAR(20) NOT NULL CONSTRAINT DF_pos_settings_setting_type DEFAULT 'STRING';
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_settings' AND COLUMN_NAME = 'description')
	ALTER TABLE dbo.pos_settings ADD description NVARCHAR(255) NULL;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_settings' AND COLUMN_NAME = 'category')
	ALTER TABLE dbo.pos_settings ADD category VARCHAR(30) NOT NULL CONSTRAINT DF_pos_settings_category DEFAULT 'GENERAL';
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_settings' AND COLUMN_NAME = 'is_required')
	ALTER TABLE dbo.pos_settings ADD is_required BIT NOT NULL CONSTRAINT DF_pos_settings_is_required DEFAULT 0;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_settings' AND COLUMN_NAME = 'is_encrypted')
	ALTER TABLE dbo.pos_settings ADD is_encrypted BIT NOT NULL CONSTRAINT DF_pos_settings_is_encrypted DEFAULT 0;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_settings' AND COLUMN_NAME = 'modified_by')
	ALTER TABLE dbo.pos_settings ADD modified_by INT NULL;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_settings' AND COLUMN_NAME = 'modified_at')
	ALTER TABLE dbo.pos_settings ADD modified_at DATETIME NULL;
GO

-- ============================================================
-- 2. acc_wht_rates — Withholding tax rates (Pakistan Income Tax)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'acc_wht_rates')
BEGIN
	CREATE TABLE dbo.acc_wht_rates (
		wht_id              INT             NOT NULL IDENTITY(1,1),
		wht_type            VARCHAR(50)     NOT NULL,           -- e.g. 'PURCHASE_GOODS', 'SERVICES'
		tax_section         VARCHAR(20)     NOT NULL,           -- e.g. '153', '155'
		description         NVARCHAR(200)   NOT NULL,
		rate                DECIMAL(5,2)    NOT NULL,
		effective_from      DATE            NOT NULL,
		effective_to        DATE            NULL,               -- NULL = still active
		is_active           BIT             NOT NULL CONSTRAINT DF_acc_wht_rates_is_active DEFAULT 1,

		CONSTRAINT PK_acc_wht_rates PRIMARY KEY (wht_id),
		CONSTRAINT CK_acc_wht_rates_rate CHECK (rate >= 0 AND rate <= 100)
	);
END
GO

-- ============================================================
-- 3. Alter pos_companies — add 3 setup columns that don't yet exist
--    (existing table is kept as-is; only missing columns are added)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_companies' AND COLUMN_NAME = 'financial_year_start_month')
	ALTER TABLE dbo.pos_companies
		ADD financial_year_start_month INT NOT NULL
			CONSTRAINT DF_pos_companies_fy_month   DEFAULT 7   -- July (Pakistan fiscal year)
			CONSTRAINT CK_pos_companies_fy_month   CHECK (financial_year_start_month BETWEEN 1 AND 12);
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_companies' AND COLUMN_NAME = 'is_setup_complete')
	ALTER TABLE dbo.pos_companies
		ADD is_setup_complete BIT NOT NULL
			CONSTRAINT DF_pos_companies_is_setup   DEFAULT 0;
GO

IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS
			   WHERE TABLE_NAME = 'pos_companies' AND COLUMN_NAME = 'setup_date')
	ALTER TABLE dbo.pos_companies
		ADD setup_date DATE NULL;
GO

-- ============================================================
-- 4. Seed pos_settings — default keys (UPSERT — safe to re-run)
-- ============================================================
-- Helper: insert only when key doesn't exist yet
DECLARE @seeds TABLE (
	setting_key   VARCHAR(100),
	setting_value VARCHAR(500),
	setting_type  VARCHAR(20),
	description   NVARCHAR(255),
	category      VARCHAR(30),
	is_required   BIT
);

INSERT INTO @seeds VALUES
-- ── Accounting ──────────────────────────────────────────────
('ACC_AUTO_POST_SALES',            'true',       'BOOLEAN', 'Automatically post accounting entries when a sales invoice is saved',                             'ACCOUNTING', 1),
('ACC_AUTO_POST_PURCHASES',        'true',       'BOOLEAN', 'Automatically post accounting entries when a purchase invoice is saved',                         'ACCOUNTING', 1),
('ACC_AUTO_POST_RECEIPTS',         'true',       'BOOLEAN', 'Automatically post accounting entries for customer receipts',                                    'ACCOUNTING', 0),
('ACC_AUTO_POST_PAYMENTS',         'true',       'BOOLEAN', 'Automatically post accounting entries for vendor payments',                                      'ACCOUNTING', 0),
('ACC_REQUIRE_NARRATION',          'false',      'BOOLEAN', 'Require narration/description on every journal entry',                                           'ACCOUNTING', 0),
('ACC_BACKDATING_LIMIT_DAYS',      '90',         'INTEGER', 'Maximum number of days in the past a transaction can be backdated',                              'ACCOUNTING', 0),
('ACC_BUDGET_WARNING_PCT',         '85',         'INTEGER', 'Show budget warning when spending reaches this percentage of budget',                             'ACCOUNTING', 0),
('ACC_VALUATION_METHOD',           'WAC',        'STRING',  'Inventory valuation method: WAC (Weighted Avg Cost), FIFO, or LIFO',                             'ACCOUNTING', 1),
('ACC_AMOUNT_FORMAT',              'PAKISTANI',  'STRING',  'Number formatting: PAKISTANI (1,23,45,678) or INTERNATIONAL (1,234,567)',                        'ACCOUNTING', 1),
('ACC_FINANCIAL_YEAR_START_MONTH', '7',          'INTEGER', 'Month number (1-12) when the financial year starts. 7 = July (Pakistan default)',               'ACCOUNTING', 1),
('ACC_DECIMAL_PLACES',             '2',          'INTEGER', 'Number of decimal places for monetary amounts',                                                  'ACCOUNTING', 1),
('ACC_ROUNDING_METHOD',            'HALF_UP',    'STRING',  'Rounding method: HALF_UP, HALF_EVEN (banker rounding)',                                          'ACCOUNTING', 0),

-- ── Default Accounts ────────────────────────────────────────
('ACC_DEFAULT_SALES_ACCOUNT',      '',           'STRING',  'Chart of accounts code for the default sales/revenue account',                                   'DEFAULT_ACCOUNTS', 1),
('ACC_DEFAULT_PURCHASE_ACCOUNT',   '',           'STRING',  'Chart of accounts code for the default purchases/COGS account',                                  'DEFAULT_ACCOUNTS', 1),
('ACC_DEFAULT_AR_ACCOUNT',         '',           'STRING',  'Chart of accounts code for accounts receivable (trade debtors)',                                 'DEFAULT_ACCOUNTS', 1),
('ACC_DEFAULT_AP_ACCOUNT',         '',           'STRING',  'Chart of accounts code for accounts payable (trade creditors)',                                  'DEFAULT_ACCOUNTS', 1),
('ACC_DEFAULT_CASH_ACCOUNT',       '',           'STRING',  'Chart of accounts code for petty cash account',                                                  'DEFAULT_ACCOUNTS', 1),
('ACC_DEFAULT_BANK_ACCOUNT',       '',           'STRING',  'Chart of accounts code for primary bank account',                                                'DEFAULT_ACCOUNTS', 1),
('ACC_DEFAULT_STOCK_ACCOUNT',      '',           'STRING',  'Chart of accounts code for inventory / stock-in-hand account',                                   'DEFAULT_ACCOUNTS', 1),
('ACC_DEFAULT_TAX_PAYABLE',        '',           'STRING',  'Chart of accounts code for sales tax / GST payable',                                             'DEFAULT_ACCOUNTS', 0),
('ACC_DEFAULT_TAX_RECEIVABLE',     '',           'STRING',  'Chart of accounts code for input tax / GST receivable',                                          'DEFAULT_ACCOUNTS', 0),
('ACC_DEFAULT_RETAINED_EARNINGS',  '',           'STRING',  'Chart of accounts code for retained earnings / accumulated profit',                              'DEFAULT_ACCOUNTS', 1),
('ACC_DEFAULT_OPENING_BALANCE_AC', '',           'STRING',  'Suspense account used to receive opening balance entries',                                       'DEFAULT_ACCOUNTS', 0),
('ACC_DEFAULT_DISCOUNT_ACCOUNT',   '',           'STRING',  'Chart of accounts code for trade discount expense',                                              'DEFAULT_ACCOUNTS', 0),
('ACC_DEFAULT_FREIGHT_ACCOUNT',    '',           'STRING',  'Chart of accounts code for freight / carriage inward expense',                                   'DEFAULT_ACCOUNTS', 0),

-- ── WHT / Tax ────────────────────────────────────────────────
('ACC_WHT_ENABLED',                'false',      'BOOLEAN', 'Enable withholding tax (WHT) deduction on vendor payments',                                      'TAX', 0),
('ACC_GST_RATE',                   '18',         'INTEGER', 'Standard GST/Sales Tax rate (%)',                                                                'TAX', 0),
('ACC_GST_ENABLED',                'true',       'BOOLEAN', 'Enable GST/Sales Tax on sales invoices',                                                         'TAX', 0),

-- ── Journals ─────────────────────────────────────────────────
('ACC_JOURNAL_AUTO_NUMBER',        'true',       'BOOLEAN', 'Automatically generate sequential journal voucher numbers',                                      'JOURNALS', 0),
('ACC_JOURNAL_NUMBER_PREFIX',      'JV',         'STRING',  'Prefix for auto-generated journal voucher numbers (e.g. JV-2024-001)',                           'JOURNALS', 0),
('ACC_ALLOW_UNBALANCED_DRAFTS',    'false',      'BOOLEAN', 'Allow saving draft journal entries that are not balanced (debit ≠ credit)',                      'JOURNALS', 0),

-- ── General ──────────────────────────────────────────────────
('COMPANY_SETUP_COMPLETE',         'false',      'BOOLEAN', 'Set to true once the company setup wizard has been completed',                                   'GENERAL', 1),
('SYSTEM_DATE_FORMAT',             'dd/MM/yyyy', 'STRING',  'Display date format used across the application',                                                'GENERAL', 0),
('SYSTEM_LANGUAGE',                'EN',         'STRING',  'Default UI language: EN or AR',                                                                  'GENERAL', 0);

-- UPSERT: insert new rows, skip existing keys
INSERT INTO dbo.pos_settings (setting_key, setting_value, setting_type, description, category, is_required)
SELECT s.setting_key, s.setting_value, s.setting_type, s.description, s.category, s.is_required
FROM   @seeds s
WHERE  NOT EXISTS (
		   SELECT 1 FROM dbo.pos_settings p WHERE p.setting_key = s.setting_key
	   );
GO

-- ============================================================
-- 5. Sample WHT seed data (Pakistan FY 2024-25)
-- ============================================================
IF NOT EXISTS (SELECT 1 FROM dbo.acc_wht_rates WHERE tax_section = '153' AND wht_type = 'PURCHASE_GOODS')
BEGIN
	INSERT INTO dbo.acc_wht_rates (wht_type, tax_section, description, rate, effective_from)
	VALUES
	('PURCHASE_GOODS',   '153', 'Sec 153(1)(a) – Purchase of goods – Filer',          4.50, '2024-07-01'),
	('PURCHASE_GOODS',   '153', 'Sec 153(1)(a) – Purchase of goods – Non-filer',       9.00, '2024-07-01'),
	('SERVICES',         '153', 'Sec 153(1)(b) – Services rendered – Filer',           8.00, '2024-07-01'),
	('SERVICES',         '153', 'Sec 153(1)(b) – Services rendered – Non-filer',      16.00, '2024-07-01'),
	('CONTRACT',         '153', 'Sec 153(1)(c) – Execution of contract – Filer',       7.00, '2024-07-01'),
	('CONTRACT',         '153', 'Sec 153(1)(c) – Execution of contract – Non-filer',  14.00, '2024-07-01'),
	('RENT',             '155', 'Sec 155 – Rental income – Filer',                    15.00, '2024-07-01'),
	('RENT',             '155', 'Sec 155 – Rental income – Non-filer',                30.00, '2024-07-01');
END
GO

-- ============================================================
-- 6. sp_SetupStandardCOA — Insert 80+ standard accounts
--    Expects: @BranchId INT, @CreatedBy INT
--    Returns: count of accounts inserted
--
--    Account hierarchy uses numeric code convention:
--      1xxxx  Assets        5xxxx  Cost of Sales
--      2xxxx  Liabilities   6xxxx  Operating Expenses
--      3xxxx  Equity        7xxxx  Other Income
--      4xxxx  Revenue       8xxxx  Other Expenses
-- ============================================================
IF OBJECT_ID(N'dbo.sp_SetupStandardCOA', N'P') IS NOT NULL
	DROP PROCEDURE dbo.sp_SetupStandardCOA;
GO

CREATE PROCEDURE dbo.sp_SetupStandardCOA
	@BranchId      INT,
	@CreatedBy     INT,
	@InsertedCount INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;

	-- ----------------------------------------------------------------
	-- Staging table — mirrors final shape of acc_groups / acc_accounts
	-- is_group = 1  → row goes into acc_groups
	-- is_posting = 1 → row goes into acc_accounts (leaf / transactable)
	-- ----------------------------------------------------------------
	DECLARE @coa TABLE (
		acc_code        VARCHAR(20)     NOT NULL,
		acc_name        NVARCHAR(150)   NOT NULL,
		acc_name_2      NVARCHAR(150)   NULL,       -- Arabic / secondary name
		parent_code     VARCHAR(20)     NULL,       -- NULL = root group
		acc_type        VARCHAR(20)     NOT NULL,   -- ASSET / LIABILITY / EQUITY / REVENUE / EXPENSE
		is_group        BIT             NOT NULL,
		is_posting      BIT             NOT NULL    -- 1 = leaf account → acc_accounts
	);

	-- ----------------------------------------------------------------
	-- Mapping tables built during the two-pass insert
	-- code → inserted acc_groups.id
	-- ----------------------------------------------------------------
	DECLARE @groupMap TABLE (
		acc_code  VARCHAR(20) NOT NULL,
		group_id  INT         NOT NULL
	);

	-- ── ASSETS ──────────────────────────────────────────────────────────────
	-- acc_code, acc_name, acc_name_2 (Arabic), parent_code, acc_type, is_group, is_posting
	INSERT INTO @coa VALUES
	('10000', 'Assets',                          N'الأصول',                             NULL,    'ASSET',     1, 0),
	('11000', 'Current Assets',                  N'الأصول المتداولة',                    '10000', 'ASSET',     1, 0),
	('11100', 'Cash & Cash Equivalents',         N'النقد وما يعادله',                    '11000', 'ASSET',     1, 0),
	('11101', 'Petty Cash',                      N'النقد الصغير',                        '11100', 'ASSET',     0, 1),
	('11102', 'Cash in Hand',                    N'نقد في الصندوق',                      '11100', 'ASSET',     0, 1),
	('11200', 'Bank Accounts',                   N'الحسابات البنكية',                    '11000', 'ASSET',     1, 0),
	('11201', 'Current Account - Main Bank',     N'الحساب الجاري - البنك الرئيسي',        '11200', 'ASSET',     0, 1),
	('11202', 'Savings Account',                 N'حساب التوفير',                        '11200', 'ASSET',     0, 1),
	('11300', 'Accounts Receivable',             N'الذمم المدينة',                        '11000', 'ASSET',     1, 0),
	('11301', 'Trade Debtors',                   N'مدينون تجاريون',                      '11300', 'ASSET',     0, 1),
	('11302', 'Advance to Customers',            N'سلف للعملاء',                         '11300', 'ASSET',     0, 1),
	('11303', 'Bills Receivable',                N'أوراق القبض',                          '11300', 'ASSET',     0, 1),
	('11400', 'Inventory',                       N'المخزون',                              '11000', 'ASSET',     1, 0),
	('11401', 'Stock in Hand',                   N'البضاعة في المخزن',                   '11400', 'ASSET',     0, 1),
	('11402', 'Raw Materials',                   N'المواد الخام',                         '11400', 'ASSET',     0, 1),
	('11403', 'Finished Goods',                  N'البضاعة التامة الصنع',                '11400', 'ASSET',     0, 1),
	('11404', 'Goods in Transit',                N'البضاعة في الطريق',                   '11400', 'ASSET',     0, 1),
	('11500', 'Prepaid Expenses & Deposits',     N'المصاريف المدفوعة مقدماً',            '11000', 'ASSET',     1, 0),
	('11501', 'Prepaid Rent',                    N'إيجار مدفوع مقدماً',                  '11500', 'ASSET',     0, 1),
	('11502', 'Prepaid Insurance',               N'تأمين مدفوع مقدماً',                  '11500', 'ASSET',     0, 1),
	('11503', 'Security Deposits',               N'ودائع ضمان',                          '11500', 'ASSET',     0, 1),
	('11600', 'Tax Receivable',                  N'ضرائب مستحقة',                        '11000', 'ASSET',     1, 0),
	('11601', 'Sales Tax Receivable (Input)',     N'ضريبة مبيعات مدخلات',                '11600', 'ASSET',     0, 1),
	('11602', 'Income Tax Advance',              N'ضريبة دخل مقدمة',                     '11600', 'ASSET',     0, 1),
	('11603', 'WHT Recoverable',                 N'ضريبة استقطاع قابلة للاسترداد',       '11600', 'ASSET',     0, 1),
	('12000', 'Non-Current Assets',              N'الأصول غير المتداولة',                '10000', 'ASSET',     1, 0),
	('12100', 'Property, Plant & Equipment',     N'الممتلكات والمصانع والمعدات',          '12000', 'ASSET',     1, 0),
	('12101', 'Land & Buildings',                N'الأراضي والمباني',                    '12100', 'ASSET',     0, 1),
	('12102', 'Machinery & Equipment',           N'الآلات والمعدات',                     '12100', 'ASSET',     0, 1),
	('12103', 'Furniture & Fixtures',            N'الأثاث والتجهيزات',                   '12100', 'ASSET',     0, 1),
	('12104', 'Vehicles',                        N'المركبات',                             '12100', 'ASSET',     0, 1),
	('12105', 'Computer & IT Equipment',         N'معدات الكمبيوتر وتكنولوجيا المعلومات','12100', 'ASSET',     0, 1),
	('12200', 'Accumulated Depreciation',        N'مجمع الاستهلاك',                      '12000', 'ASSET',     1, 0),
	('12201', 'Accum. Depr. - Buildings',        N'مجمع استهلاك المباني',                '12200', 'ASSET',     0, 1),
	('12202', 'Accum. Depr. - Machinery',        N'مجمع استهلاك الآلات',                 '12200', 'ASSET',     0, 1),
	('12203', 'Accum. Depr. - Furniture',        N'مجمع استهلاك الأثاث',                 '12200', 'ASSET',     0, 1),
	('12204', 'Accum. Depr. - Vehicles',         N'مجمع استهلاك المركبات',               '12200', 'ASSET',     0, 1),
	('12205', 'Accum. Depr. - IT Equipment',     N'مجمع استهلاك معدات تقنية المعلومات',  '12200', 'ASSET',     0, 1),
	('12300', 'Intangible Assets',               N'الأصول غير الملموسة',                 '12000', 'ASSET',     1, 0),
	('12301', 'Goodwill',                        N'الشهرة التجارية',                     '12300', 'ASSET',     0, 1),
	('12302', 'Software & Licenses',             N'البرامج والتراخيص',                   '12300', 'ASSET',     0, 1),
	('12400', 'Long-Term Investments',           N'الاستثمارات طويلة الأمد',             '12000', 'ASSET',     1, 0),
	('12401', 'Investment in Subsidiaries',      N'استثمار في الشركات التابعة',           '12400', 'ASSET',     0, 1),
	('12402', 'Long-Term Deposits',              N'ودائع طويلة الأمد',                   '12400', 'ASSET',     0, 1),

	-- ── LIABILITIES ─────────────────────────────────────────────────────────
	('20000', 'Liabilities',                     N'الالتزامات',                           NULL,    'LIABILITY', 1, 0),
	('21000', 'Current Liabilities',             N'الالتزامات المتداولة',                 '20000', 'LIABILITY', 1, 0),
	('21100', 'Accounts Payable',                N'الذمم الدائنة',                        '21000', 'LIABILITY', 1, 0),
	('21101', 'Trade Creditors',                 N'دائنون تجاريون',                       '21100', 'LIABILITY', 0, 1),
	('21102', 'Advance from Customers',          N'سلف من العملاء',                       '21100', 'LIABILITY', 0, 1),
	('21103', 'Bills Payable',                   N'أوراق الدفع',                          '21100', 'LIABILITY', 0, 1),
	('21200', 'Tax Payable',                     N'الضرائب المستحقة الدفع',               '21000', 'LIABILITY', 1, 0),
	('21201', 'Sales Tax Payable (Output)',       N'ضريبة مبيعات مخرجات',                 '21200', 'LIABILITY', 0, 1),
	('21202', 'Income Tax Payable',              N'ضريبة دخل مستحقة',                     '21200', 'LIABILITY', 0, 1),
	('21203', 'WHT Payable',                     N'ضريبة استقطاع مستحقة',                 '21200', 'LIABILITY', 0, 1),
	('21300', 'Accrued Liabilities',             N'الالتزامات المستحقة',                  '21000', 'LIABILITY', 1, 0),
	('21301', 'Accrued Salaries & Wages',        N'رواتب وأجور مستحقة',                  '21300', 'LIABILITY', 0, 1),
	('21302', 'Accrued Utilities',               N'مرافق مستحقة',                         '21300', 'LIABILITY', 0, 1),
	('21303', 'Accrued Rent',                    N'إيجار مستحق',                          '21300', 'LIABILITY', 0, 1),
	('21400', 'Short-Term Borrowings',           N'اقتراضات قصيرة الأمد',                '21000', 'LIABILITY', 1, 0),
	('21401', 'Bank Overdraft',                  N'السحب على المكشوف',                    '21400', 'LIABILITY', 0, 1),
	('21402', 'Short-Term Loans',                N'قروض قصيرة الأمد',                    '21400', 'LIABILITY', 0, 1),
	('22000', 'Non-Current Liabilities',         N'الالتزامات غير المتداولة',             '20000', 'LIABILITY', 1, 0),
	('22101', 'Long-Term Bank Loans',            N'قروض بنكية طويلة الأمد',               '22000', 'LIABILITY', 0, 1),
	('22102', 'Deferred Tax Liability',          N'ضريبة مؤجلة',                          '22000', 'LIABILITY', 0, 1),
	('22103', 'Employee End-of-Service Benefits',N'مكافآت نهاية الخدمة',                  '22000', 'LIABILITY', 0, 1),

	-- ── EQUITY ──────────────────────────────────────────────────────────────
	('30000', 'Equity',                          N'حقوق الملكية',                         NULL,    'EQUITY',    1, 0),
	('31000', 'Paid-up Capital',                 N'رأس المال المدفوع',                    '30000', 'EQUITY',    1, 0),
	('31001', 'Owner Capital',                   N'رأس مال المالك',                       '31000', 'EQUITY',    0, 1),
	('31002', 'Share Capital',                   N'رأس مال الأسهم',                       '31000', 'EQUITY',    0, 1),
	('31100', 'Reserves & Surplus',              N'الاحتياطيات والفائض',                  '30000', 'EQUITY',    1, 0),
	('31101', 'General Reserve',                 N'احتياطي عام',                          '31100', 'EQUITY',    0, 1),
	('31102', 'Retained Earnings',               N'الأرباح المحتجزة',                     '31100', 'EQUITY',    0, 1),
	('31103', 'Current Year Profit / Loss',      N'ربح / خسارة السنة الحالية',            '31100', 'EQUITY',    0, 1),
	('31200', 'Drawings',                        N'المسحوبات',                             '30000', 'EQUITY',    0, 1),
	('31900', 'Opening Balance Suspense',        N'حساب الأرصدة الافتتاحية',             '30000', 'EQUITY',    0, 1),

	-- ── REVENUE ─────────────────────────────────────────────────────────────
	('40000', 'Revenue',                         N'الإيرادات',                            NULL,    'REVENUE',   1, 0),
	('41000', 'Sales Revenue',                   N'إيرادات المبيعات',                     '40000', 'REVENUE',   1, 0),
	('41001', 'Sales - Products',                N'مبيعات المنتجات',                      '41000', 'REVENUE',   0, 1),
	('41002', 'Sales - Services',                N'مبيعات الخدمات',                       '41000', 'REVENUE',   0, 1),
	('41003', 'Sales Returns & Allowances',      N'مردودات ومسموحات المبيعات',            '41000', 'REVENUE',   0, 1),
	('41004', 'Trade Discounts Given',           N'خصومات تجارية ممنوحة',                 '41000', 'REVENUE',   0, 1),
	('42000', 'Other Operating Income',          N'دخل تشغيلي آخر',                       '40000', 'REVENUE',   1, 0),
	('42001', 'Rental Income',                   N'دخل الإيجار',                          '42000', 'REVENUE',   0, 1),
	('42002', 'Commission Income',               N'دخل العمولات',                         '42000', 'REVENUE',   0, 1),
	('70000', 'Other Income',                    N'الدخل الآخر',                          NULL,    'REVENUE',   1, 0),
	('70001', 'Interest / Profit on Deposits',   N'فوائد / أرباح على الودائع',            '70000', 'REVENUE',   0, 1),
	('70002', 'Gain on Disposal of Assets',      N'ربح من بيع الأصول',                   '70000', 'REVENUE',   0, 1),
	('70003', 'Foreign Exchange Gain',           N'مكاسب صرف العملات الأجنبية',           '70000', 'REVENUE',   0, 1),
	('70004', 'Miscellaneous Income',            N'دخل متنوع',                            '70000', 'REVENUE',   0, 1),

	-- ── COST OF SALES ───────────────────────────────────────────────────────
	('50000', 'Cost of Sales',                   N'تكلفة المبيعات',                       NULL,    'EXPENSE',   1, 0),
	('51001', 'Cost of Goods Sold',              N'تكلفة البضاعة المباعة',               '50000', 'EXPENSE',   0, 1),
	('51002', 'Purchase Returns & Allowances',   N'مردودات ومسموحات المشتريات',           '50000', 'EXPENSE',   0, 1),
	('51003', 'Freight / Carriage Inward',       N'شحن / نقل للداخل',                    '50000', 'EXPENSE',   0, 1),
	('51004', 'Purchase Discounts Received',     N'خصومات مشتريات مستلمة',               '50000', 'EXPENSE',   0, 1),
	('51005', 'Customs & Import Duties',         N'جمارك ورسوم استيراد',                  '50000', 'EXPENSE',   0, 1),

	-- ── OPERATING EXPENSES ──────────────────────────────────────────────────
	('60000', 'Operating Expenses',              N'المصاريف التشغيلية',                   NULL,    'EXPENSE',   1, 0),
	('61000', 'Salaries & Employee Costs',       N'الرواتب وتكاليف الموظفين',            '60000', 'EXPENSE',   1, 0),
	('61001', 'Salaries & Wages',                N'الرواتب والأجور',                      '61000', 'EXPENSE',   0, 1),
	('61002', 'EOBI / Social Security',          N'الضمان الاجتماعي',                     '61000', 'EXPENSE',   0, 1),
	('61003', 'Staff Allowances & Benefits',     N'بدلات ومزايا الموظفين',               '61000', 'EXPENSE',   0, 1),
	('62000', 'Occupancy Costs',                 N'تكاليف الإشغال',                       '60000', 'EXPENSE',   1, 0),
	('62001', 'Rent Expense',                    N'مصاريف الإيجار',                       '62000', 'EXPENSE',   0, 1),
	('62002', 'Utilities (Electricity, Water)',  N'المرافق (كهرباء، ماء)',               '62000', 'EXPENSE',   0, 1),
	('62003', 'Repair & Maintenance',            N'إصلاح وصيانة',                         '62000', 'EXPENSE',   0, 1),
	('63000', 'Administrative Expenses',         N'المصاريف الإدارية',                    '60000', 'EXPENSE',   1, 0),
	('63001', 'Office Supplies & Stationery',    N'مستلزمات المكتب والقرطاسية',          '63000', 'EXPENSE',   0, 1),
	('63002', 'Communication (Phone, Internet)', N'الاتصالات (هاتف، إنترنت)',             '63000', 'EXPENSE',   0, 1),
	('63003', 'Postage & Courier',               N'بريد وشحن',                            '63000', 'EXPENSE',   0, 1),
	('63004', 'Printing & Reproduction',         N'طباعة واستنساخ',                       '63000', 'EXPENSE',   0, 1),
	('63005', 'Travel & Transportation',         N'سفر ونقل',                             '63000', 'EXPENSE',   0, 1),
	('63006', 'Entertainment & Hospitality',     N'ترفيه وضيافة',                         '63000', 'EXPENSE',   0, 1),
	('63007', 'Professional Fees',               N'أتعاب مهنية',                          '63000', 'EXPENSE',   0, 1),
	('63008', 'Insurance Expense',               N'مصاريف التأمين',                       '63000', 'EXPENSE',   0, 1),
	('63009', 'Depreciation Expense',            N'مصاريف الاستهلاك',                     '63000', 'EXPENSE',   0, 1),
	('63010', 'Amortisation Expense',            N'مصاريف الإطفاء',                       '63000', 'EXPENSE',   0, 1),
	('64000', 'Selling & Distribution Expenses', N'مصاريف البيع والتوزيع',               '60000', 'EXPENSE',   1, 0),
	('64001', 'Advertising & Marketing',         N'إعلان وتسويق',                         '64000', 'EXPENSE',   0, 1),
	('64002', 'Freight Outward',                 N'شحن للخارج',                           '64000', 'EXPENSE',   0, 1),
	('64003', 'Sales Commission',                N'عمولة مبيعات',                         '64000', 'EXPENSE',   0, 1),
	('64004', 'Packaging Expense',               N'مصاريف التغليف',                       '64000', 'EXPENSE',   0, 1),

	-- ── OTHER EXPENSES ───────────────────────────────────────────────────────
	('80000', 'Other Expenses',                  N'المصاريف الأخرى',                      NULL,    'EXPENSE',   1, 0),
	('80001', 'Interest / Bank Charges',         N'فوائد / رسوم بنكية',                  '80000', 'EXPENSE',   0, 1),
	('80002', 'Loss on Disposal of Assets',      N'خسارة من بيع الأصول',                 '80000', 'EXPENSE',   0, 1),
	('80003', 'Foreign Exchange Loss',           N'خسائر صرف العملات الأجنبية',           '80000', 'EXPENSE',   0, 1),
	('80004', 'Bad Debts Written Off',           N'ديون معدومة',                          '80000', 'EXPENSE',   0, 1),
	('80005', 'Penalties & Fines',               N'غرامات وعقوبات',                       '80000', 'EXPENSE',   0, 1),
	('80006', 'Miscellaneous Expense',           N'مصاريف متنوعة',                        '80000', 'EXPENSE',   0, 1);

	-- ================================================================
	-- PASS 1: Seed acc_account_type (idempotent)
	-- ================================================================
	IF NOT EXISTS (SELECT 1 FROM dbo.acc_account_type WHERE UPPER(name) = 'ASSET')
		INSERT INTO dbo.acc_account_type (name) VALUES ('Asset');
	IF NOT EXISTS (SELECT 1 FROM dbo.acc_account_type WHERE UPPER(name) = 'LIABILITY')
		INSERT INTO dbo.acc_account_type (name) VALUES ('Liability');
	IF NOT EXISTS (SELECT 1 FROM dbo.acc_account_type WHERE UPPER(name) = 'EQUITY')
		INSERT INTO dbo.acc_account_type (name) VALUES ('Equity');
	IF NOT EXISTS (SELECT 1 FROM dbo.acc_account_type WHERE UPPER(name) = 'REVENUE')
		INSERT INTO dbo.acc_account_type (name) VALUES ('Revenue');
	IF NOT EXISTS (SELECT 1 FROM dbo.acc_account_type WHERE UPPER(name) = 'EXPENSE')
		INSERT INTO dbo.acc_account_type (name) VALUES ('Expense');

	-- ================================================================
	-- PASS 2: Insert acc_groups (group rows only, parents before children)
	--   acc_groups columns used: parent_id, code, name, account_type_id
	--   Rows ordered by code length so parents are always inserted first.
	-- ================================================================
	DECLARE
		@g_code     VARCHAR(20),
		@g_name     NVARCHAR(150),
		@g_parent   VARCHAR(20),
		@g_type     VARCHAR(20),
		@g_type_id  INT,
		@g_parent_id INT,
		@g_new_id   INT;

	DECLARE grp_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT acc_code, acc_name, parent_code, acc_type
		FROM   @coa
		WHERE  is_group = 1
		ORDER  BY LEN(acc_code), acc_code;  -- ensures parents inserted before children

	OPEN grp_cur;
	FETCH NEXT FROM grp_cur INTO @g_code, @g_name, @g_parent, @g_type;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		-- Skip if this group already exists (idempotent)
		IF NOT EXISTS (SELECT 1 FROM dbo.acc_groups WHERE code = @g_code)
		BEGIN
			SELECT @g_type_id  = id FROM dbo.acc_account_type WHERE UPPER(name) = UPPER(@g_type);
			SELECT @g_parent_id = group_id FROM @groupMap WHERE acc_code = @g_parent;

			INSERT INTO dbo.acc_groups (parent_id, code, name, account_type_id)
			VALUES (@g_parent_id, @g_code, @g_name, @g_type_id);

			SET @g_new_id = SCOPE_IDENTITY();
		END
		ELSE
		BEGIN
			SELECT @g_new_id = id FROM dbo.acc_groups WHERE code = @g_code;
		END

		INSERT INTO @groupMap (acc_code, group_id) VALUES (@g_code, @g_new_id);

		FETCH NEXT FROM grp_cur INTO @g_code, @g_name, @g_parent, @g_type;
	END

	CLOSE grp_cur;
	DEALLOCATE grp_cur;

	-- ================================================================
	-- PASS 3: Insert acc_accounts (posting / leaf rows only)
	--   acc_accounts columns used:
	--     branch_id, code, name, name_2, group_id,
	--     op_dr_balance, op_cr_balance, description, user_id, date_created, is_active
	-- ================================================================
	SET @InsertedCount = 0;

	DECLARE
		@a_code      VARCHAR(20),
		@a_name      NVARCHAR(150),
		@a_name_2    NVARCHAR(150),
		@a_parent    VARCHAR(20),
		@a_group_id  INT;

	DECLARE acc_cur CURSOR LOCAL FAST_FORWARD FOR
		SELECT acc_code, acc_name, acc_name_2, parent_code
		FROM   @coa
		WHERE  is_posting = 1
		ORDER  BY acc_code;

	OPEN acc_cur;
	FETCH NEXT FROM acc_cur INTO @a_code, @a_name, @a_name_2, @a_parent;

	WHILE @@FETCH_STATUS = 0
	BEGIN
		IF NOT EXISTS (
			SELECT 1 FROM dbo.acc_accounts
			WHERE  code = @a_code AND branch_id = @BranchId
		)
		BEGIN
			-- Resolve group_id: try direct parent first, walk up to first group match
			SELECT @a_group_id = group_id FROM @groupMap WHERE acc_code = @a_parent;

			-- Fallback: use the account type root group if parent not in map
			IF @a_group_id IS NULL
				SELECT TOP 1 @a_group_id = id FROM dbo.acc_groups WHERE code = @a_parent;

			INSERT INTO dbo.acc_accounts
				(branch_id, code, name, name_2, group_id,
				 op_dr_balance, op_cr_balance, description, user_id, date_created, is_active)
			VALUES
				(@BranchId, @a_code, @a_name, @a_name_2, @a_group_id,
				 0, 0, NULL, @CreatedBy, GETDATE(), 1);

			SET @InsertedCount = @InsertedCount + 1;
		END

		FETCH NEXT FROM acc_cur INTO @a_code, @a_name, @a_name_2, @a_parent;
	END

	CLOSE acc_cur;
	DEALLOCATE acc_cur;

	-- ================================================================
	-- PASS 4: Update pos_settings with the account codes we just created
	-- ================================================================
	UPDATE dbo.pos_settings SET setting_value = '11301' WHERE setting_key = 'ACC_DEFAULT_AR_ACCOUNT'        AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '21101' WHERE setting_key = 'ACC_DEFAULT_AP_ACCOUNT'        AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '11101' WHERE setting_key = 'ACC_DEFAULT_CASH_ACCOUNT'      AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '11201' WHERE setting_key = 'ACC_DEFAULT_BANK_ACCOUNT'      AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '11401' WHERE setting_key = 'ACC_DEFAULT_STOCK_ACCOUNT'     AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '41001' WHERE setting_key = 'ACC_DEFAULT_SALES_ACCOUNT'     AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '51001' WHERE setting_key = 'ACC_DEFAULT_PURCHASE_ACCOUNT'  AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '21201' WHERE setting_key = 'ACC_DEFAULT_TAX_PAYABLE'       AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '11601' WHERE setting_key = 'ACC_DEFAULT_TAX_RECEIVABLE'    AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '31102' WHERE setting_key = 'ACC_DEFAULT_RETAINED_EARNINGS' AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '31900' WHERE setting_key = 'ACC_DEFAULT_OPENING_BALANCE_AC' AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '41004' WHERE setting_key = 'ACC_DEFAULT_DISCOUNT_ACCOUNT'  AND setting_value = '';
	UPDATE dbo.pos_settings SET setting_value = '51003' WHERE setting_key = 'ACC_DEFAULT_FREIGHT_ACCOUNT'   AND setting_value = '';

	-- Mark the company setup as complete in pos_companies and pos_settings
	UPDATE dbo.pos_companies
	SET    is_setup_complete = 1,
		   setup_date        = CAST(GETDATE() AS DATE)
	WHERE  is_setup_complete = 0;

	UPDATE dbo.pos_settings
	SET    setting_value = 'true'
	WHERE  setting_key = 'COMPANY_SETUP_COMPLETE';

	RETURN;
END
GO

-- ============================================================
-- Quick verification queries (comment out before production)
-- ============================================================
-- SELECT COUNT(*) AS settings_seeded    FROM dbo.pos_settings;
-- SELECT COUNT(*) AS wht_rates          FROM dbo.acc_wht_rates;
-- SELECT financial_year_start_month, is_setup_complete, setup_date FROM dbo.pos_companies;
-- SELECT COUNT(*) AS groups_created     FROM dbo.acc_groups  WHERE code LIKE '[0-9][0-9][0-9][0-9][0-9]';
-- DECLARE @n INT; EXEC dbo.sp_SetupStandardCOA @BranchId=1, @CreatedBy=1, @InsertedCount=@n OUTPUT; SELECT @n AS coa_accounts_inserted;
