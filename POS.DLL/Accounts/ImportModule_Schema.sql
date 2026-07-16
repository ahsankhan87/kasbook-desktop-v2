-- =============================================
-- Accounting Import Module - Database Schema
-- =============================================

PRINT '========================================';
PRINT 'Accounting Import Module Schema Installation Starting...';
PRINT '========================================';
PRINT '';

-- =============================================
-- TABLE: acc_import_sessions
-- =============================================
IF OBJECT_ID(N'dbo.acc_import_sessions', N'U') IS NULL
BEGIN
	PRINT 'Creating table: acc_import_sessions';

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

	PRINT 'Table acc_import_sessions created successfully.';
END
ELSE
BEGIN
	PRINT 'Table acc_import_sessions already exists.';
END
GO

-- Add foreign key to pos_users for imported_by
IF OBJECT_ID(N'dbo.pos_users', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_import_sessions_imported_by'
   )
BEGIN
	PRINT 'Adding foreign key: FK_acc_import_sessions_imported_by';
	ALTER TABLE dbo.acc_import_sessions
	ADD CONSTRAINT FK_acc_import_sessions_imported_by
		FOREIGN KEY (imported_by) REFERENCES dbo.pos_users(id);
	PRINT 'Foreign key added successfully.';
END
GO

-- Create index on import_type for filtering
IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_import_sessions')
	  AND name = N'IX_acc_import_sessions_import_type'
)
BEGIN
	PRINT 'Creating index: IX_acc_import_sessions_import_type';
	CREATE NONCLUSTERED INDEX IX_acc_import_sessions_import_type
	ON dbo.acc_import_sessions(import_type, imported_at DESC);
	PRINT 'Index created successfully.';
END
GO

-- Create index on status for filtering
IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_import_sessions')
	  AND name = N'IX_acc_import_sessions_status'
)
BEGIN
	PRINT 'Creating index: IX_acc_import_sessions_status';
	CREATE NONCLUSTERED INDEX IX_acc_import_sessions_status
	ON dbo.acc_import_sessions(status, imported_at DESC);
	PRINT 'Index created successfully.';
END
GO

-- =============================================
-- TABLE: acc_import_vouchers
-- =============================================
IF OBJECT_ID(N'dbo.acc_import_vouchers', N'U') IS NULL
BEGIN
	PRINT 'Creating table: acc_import_vouchers';

	CREATE TABLE dbo.acc_import_vouchers
	(
		link_id INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_acc_import_vouchers PRIMARY KEY,
		session_id INT NOT NULL,
		voucher_id INT NOT NULL
	);

	PRINT 'Table acc_import_vouchers created successfully.';
END
ELSE
BEGIN
	PRINT 'Table acc_import_vouchers already exists.';
END
GO

-- Add foreign key to acc_import_sessions
IF NOT EXISTS
(
	SELECT 1
	FROM sys.foreign_keys
	WHERE name = N'FK_acc_import_vouchers_session'
)
BEGIN
	PRINT 'Adding foreign key: FK_acc_import_vouchers_session';
	ALTER TABLE dbo.acc_import_vouchers
	ADD CONSTRAINT FK_acc_import_vouchers_session
		FOREIGN KEY (session_id) REFERENCES dbo.acc_import_sessions(session_id) ON DELETE CASCADE;
	PRINT 'Foreign key added successfully.';
END
GO

-- Add foreign key to acc_entries_header (vouchers table in this system)
IF OBJECT_ID(N'dbo.acc_entries_header', N'U') IS NOT NULL
   AND NOT EXISTS
   (
		SELECT 1
		FROM sys.foreign_keys
		WHERE name = N'FK_acc_import_vouchers_voucher'
   )
BEGIN
	PRINT 'Adding foreign key: FK_acc_import_vouchers_voucher';
	ALTER TABLE dbo.acc_import_vouchers
	ADD CONSTRAINT FK_acc_import_vouchers_voucher
		FOREIGN KEY (voucher_id) REFERENCES dbo.acc_entries_header(id);
	PRINT 'Foreign key added successfully.';
END
GO

-- Create index on session_id for quick lookup
IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_import_vouchers')
	  AND name = N'IX_acc_import_vouchers_session'
)
BEGIN
	PRINT 'Creating index: IX_acc_import_vouchers_session';
	CREATE NONCLUSTERED INDEX IX_acc_import_vouchers_session
	ON dbo.acc_import_vouchers(session_id);
	PRINT 'Index created successfully.';
END
GO

-- Create index on voucher_id for quick lookup
IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_import_vouchers')
	  AND name = N'IX_acc_import_vouchers_voucher'
)
BEGIN
	PRINT 'Creating index: IX_acc_import_vouchers_voucher';
	CREATE NONCLUSTERED INDEX IX_acc_import_vouchers_voucher
	ON dbo.acc_import_vouchers(voucher_id);
	PRINT 'Index created successfully.';
END
GO

-- =============================================
-- TABLE: acc_import_templates
-- =============================================
IF OBJECT_ID(N'dbo.acc_import_templates', N'U') IS NULL
BEGIN
	PRINT 'Creating table: acc_import_templates';

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

	PRINT 'Table acc_import_templates created successfully.';
END
ELSE
BEGIN
	PRINT 'Table acc_import_templates already exists.';
END
GO

-- Create unique index on template_name
IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_import_templates')
	  AND name = N'UX_acc_import_templates_name'
)
BEGIN
	PRINT 'Creating unique index: UX_acc_import_templates_name';
	CREATE UNIQUE NONCLUSTERED INDEX UX_acc_import_templates_name
	ON dbo.acc_import_templates(template_name);
	PRINT 'Index created successfully.';
END
GO

-- Create index on import_type for filtering
IF NOT EXISTS
(
	SELECT 1
	FROM sys.indexes
	WHERE object_id = OBJECT_ID(N'dbo.acc_import_templates')
	  AND name = N'IX_acc_import_templates_import_type'
)
BEGIN
	PRINT 'Creating index: IX_acc_import_templates_import_type';
	CREATE NONCLUSTERED INDEX IX_acc_import_templates_import_type
	ON dbo.acc_import_templates(import_type);
	PRINT 'Index created successfully.';
END
GO

PRINT '';
PRINT '========================================';
PRINT 'Schema Installation Completed Successfully';
PRINT '========================================';
PRINT '';
PRINT 'Next Steps:';
PRINT '1. Execute: POS.DLL\Accounts\ImportModule_Procedures.sql';
PRINT '   This creates all stored procedures';
PRINT '';
GO
