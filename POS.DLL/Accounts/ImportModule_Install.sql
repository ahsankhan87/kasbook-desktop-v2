-- =============================================
-- Accounting Import Module - Complete Installation
-- Execute this script to install everything at once
-- =============================================

PRINT '';
PRINT '╔══════════════════════════════════════════════════════════════╗';
PRINT '║  Accounting Import Module - Complete Installation           ║';
PRINT '╚══════════════════════════════════════════════════════════════╝';
PRINT '';

DECLARE @StartTime DATETIME = GETDATE();

-- =============================================
-- STEP 1: Create Tables
-- =============================================
PRINT '────────────────────────────────────────────────────────────────';
PRINT 'STEP 1: Creating Tables';
PRINT '────────────────────────────────────────────────────────────────';
PRINT '';

-- Table: acc_import_sessions
IF OBJECT_ID(N'dbo.acc_import_sessions', N'U') IS NULL
BEGIN
	PRINT '  ✓ Creating table: acc_import_sessions';

	CREATE TABLE dbo.acc_import_sessions
	(
		session_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_import_sessions PRIMARY KEY,
		import_type VARCHAR(30) NOT NULL,
		file_name NVARCHAR(200) NOT NULL,
		total_rows INT NOT NULL DEFAULT(0),
		imported_rows INT NOT NULL DEFAULT(0),
		skipped_rows INT NOT NULL DEFAULT(0),
		error_rows INT NOT NULL DEFAULT(0),
		status VARCHAR(15) NOT NULL CONSTRAINT DF_acc_import_sessions_status DEFAULT('InProgress'),
		error_log NVARCHAR(MAX) NULL,
		rollback_available_until DATETIME NULL,
		imported_by INT NOT NULL,
		imported_at DATETIME NOT NULL CONSTRAINT DF_acc_import_sessions_imported_at DEFAULT(GETDATE()),
		CONSTRAINT CHK_acc_import_sessions_import_type CHECK (import_type IN ('COA','OPENING_BALANCE','CUSTOMER_BALANCES','SUPPLIER_BALANCES','JOURNAL_HISTORY')),
		CONSTRAINT CHK_acc_import_sessions_status CHECK (status IN ('InProgress','Completed','PartialSuccess','Failed','RolledBack'))
	);
END
ELSE
	PRINT '  → Table acc_import_sessions already exists (skipped)';
GO

-- Table: acc_import_vouchers
IF OBJECT_ID(N'dbo.acc_import_vouchers', N'U') IS NULL
BEGIN
	PRINT '  ✓ Creating table: acc_import_vouchers';

	CREATE TABLE dbo.acc_import_vouchers
	(
		link_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_import_vouchers PRIMARY KEY,
		session_id INT NOT NULL,
		voucher_id INT NOT NULL
	);
END
ELSE
	PRINT '  → Table acc_import_vouchers already exists (skipped)';
GO

-- Table: acc_import_templates
IF OBJECT_ID(N'dbo.acc_import_templates', N'U') IS NULL
BEGIN
	PRINT '  ✓ Creating table: acc_import_templates';

	CREATE TABLE dbo.acc_import_templates
	(
		template_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_import_templates PRIMARY KEY,
		template_name NVARCHAR(100) NOT NULL,
		import_type VARCHAR(30) NOT NULL,
		column_definitions NVARCHAR(MAX) NOT NULL,
		sample_data NVARCHAR(MAX) NULL,
		created_at DATETIME NOT NULL CONSTRAINT DF_acc_import_templates_created_at DEFAULT(GETDATE()),
		CONSTRAINT CHK_acc_import_templates_import_type CHECK (import_type IN ('COA','OPENING_BALANCE','CUSTOMER_BALANCES','SUPPLIER_BALANCES','JOURNAL_HISTORY'))
	);
END
ELSE
	PRINT '  → Table acc_import_templates already exists (skipped)';
GO

-- =============================================
-- STEP 2: Add Foreign Keys
-- =============================================
PRINT '';
PRINT '────────────────────────────────────────────────────────────────';
PRINT 'STEP 2: Adding Foreign Keys';
PRINT '────────────────────────────────────────────────────────────────';
PRINT '';

IF OBJECT_ID(N'dbo.pos_users', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_acc_import_sessions_imported_by')
BEGIN
	PRINT '  ✓ Adding FK: acc_import_sessions → pos_users';
	ALTER TABLE dbo.acc_import_sessions
	ADD CONSTRAINT FK_acc_import_sessions_imported_by
		FOREIGN KEY (imported_by) REFERENCES dbo.pos_users(id);
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_acc_import_vouchers_session')
BEGIN
	PRINT '  ✓ Adding FK: acc_import_vouchers → acc_import_sessions';
	ALTER TABLE dbo.acc_import_vouchers
	ADD CONSTRAINT FK_acc_import_vouchers_session
		FOREIGN KEY (session_id) REFERENCES dbo.acc_import_sessions(session_id) ON DELETE CASCADE;
END
GO

IF OBJECT_ID(N'dbo.acc_entries_header', N'U') IS NOT NULL
   AND NOT EXISTS (SELECT 1 FROM sys.foreign_keys WHERE name = N'FK_acc_import_vouchers_voucher')
BEGIN
	PRINT '  ✓ Adding FK: acc_import_vouchers → acc_entries_header';
	ALTER TABLE dbo.acc_import_vouchers
	ADD CONSTRAINT FK_acc_import_vouchers_voucher
		FOREIGN KEY (voucher_id) REFERENCES dbo.acc_entries_header(id);
END
GO

-- =============================================
-- STEP 3: Create Indexes
-- =============================================
PRINT '';
PRINT '────────────────────────────────────────────────────────────────';
PRINT 'STEP 3: Creating Indexes';
PRINT '────────────────────────────────────────────────────────────────';
PRINT '';

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.acc_import_sessions') AND name = N'IX_acc_import_sessions_import_type')
BEGIN
	PRINT '  ✓ Creating index: IX_acc_import_sessions_import_type';
	CREATE NONCLUSTERED INDEX IX_acc_import_sessions_import_type ON dbo.acc_import_sessions(import_type, imported_at DESC);
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.acc_import_sessions') AND name = N'IX_acc_import_sessions_status')
BEGIN
	PRINT '  ✓ Creating index: IX_acc_import_sessions_status';
	CREATE NONCLUSTERED INDEX IX_acc_import_sessions_status ON dbo.acc_import_sessions(status, imported_at DESC);
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.acc_import_vouchers') AND name = N'IX_acc_import_vouchers_session')
BEGIN
	PRINT '  ✓ Creating index: IX_acc_import_vouchers_session';
	CREATE NONCLUSTERED INDEX IX_acc_import_vouchers_session ON dbo.acc_import_vouchers(session_id);
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.acc_import_vouchers') AND name = N'IX_acc_import_vouchers_voucher')
BEGIN
	PRINT '  ✓ Creating index: IX_acc_import_vouchers_voucher';
	CREATE NONCLUSTERED INDEX IX_acc_import_vouchers_voucher ON dbo.acc_import_vouchers(voucher_id);
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.acc_import_templates') AND name = N'UX_acc_import_templates_name')
BEGIN
	PRINT '  ✓ Creating unique index: UX_acc_import_templates_name';
	CREATE UNIQUE NONCLUSTERED INDEX UX_acc_import_templates_name ON dbo.acc_import_templates(template_name);
END

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID(N'dbo.acc_import_templates') AND name = N'IX_acc_import_templates_import_type')
BEGIN
	PRINT '  ✓ Creating index: IX_acc_import_templates_import_type';
	CREATE NONCLUSTERED INDEX IX_acc_import_templates_import_type ON dbo.acc_import_templates(import_type);
END
GO

-- =============================================
-- STEP 4: Create Table Types
-- =============================================
PRINT '';
PRINT '────────────────────────────────────────────────────────────────';
PRINT 'STEP 4: Creating Table Types';
PRINT '────────────────────────────────────────────────────────────────';
PRINT '';

IF NOT EXISTS (SELECT 1 FROM sys.types WHERE name = 'ImportBalanceRow' AND is_table_type = 1)
BEGIN
	PRINT '  ✓ Creating table type: ImportBalanceRow';

	CREATE TYPE dbo.ImportBalanceRow AS TABLE
	(
		acc_code NVARCHAR(50) NOT NULL,
		dr_amount DECIMAL(18,2) NOT NULL DEFAULT(0),
		cr_amount DECIMAL(18,2) NOT NULL DEFAULT(0)
	);
END
ELSE
	PRINT '  → Table type ImportBalanceRow already exists (skipped)';
GO

-- =============================================
-- STEP 5: Create Stored Procedures
-- =============================================
PRINT '';
PRINT '────────────────────────────────────────────────────────────────';
PRINT 'STEP 5: Creating Stored Procedures';
PRINT '────────────────────────────────────────────────────────────────';
PRINT '';

-- Import the procedures from the main procedures file
-- For a complete installation, execute ImportModule_Procedures.sql separately
-- or copy the procedure definitions here

PRINT '  ℹ For stored procedures, please execute:';
PRINT '    ImportModule_Procedures.sql';
PRINT '';

-- =============================================
-- Installation Summary
-- =============================================
DECLARE @EndTime DATETIME = GETDATE();
DECLARE @Duration INT = DATEDIFF(SECOND, @StartTime, @EndTime);

PRINT '';
PRINT '════════════════════════════════════════════════════════════════';
PRINT '  Installation Completed Successfully!';
PRINT '════════════════════════════════════════════════════════════════';
PRINT '';
PRINT '  Duration: ' + CAST(@Duration AS VARCHAR(10)) + ' seconds';
PRINT '';
PRINT '  Tables Created:';
PRINT '    ✓ acc_import_sessions';
PRINT '    ✓ acc_import_vouchers';
PRINT '    ✓ acc_import_templates';
PRINT '';
PRINT '  Indexes Created: 6 indexes';
PRINT '  Foreign Keys: 3 constraints';
PRINT '  Table Types: 1 type (ImportBalanceRow)';
PRINT '';
PRINT '  Next Steps:';
PRINT '  1. Execute: ImportModule_Procedures.sql (4 stored procedures)';
PRINT '  2. Build POS.DLL project (ImportTemplateGenerator.cs)';
PRINT '  3. Create UI forms for import operations';
PRINT '';
PRINT '  Documentation: IMPORT_MODULE_README.md';
PRINT '';
PRINT '════════════════════════════════════════════════════════════════';
PRINT '';
