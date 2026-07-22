USE [pos_db_v3.0.0]
GO
/****** Object:  StoredProcedure [dbo].[sp_PostJournalVoucher]    Script Date: 09/07/2026 1:06:34 am ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[sp_PostJournalVoucher]
	@VoucherXml XML
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	DECLARE @Header TABLE
	(
		VoucherNo       NVARCHAR(100),
		VoucherDate     DATE,
		VoucherType     NVARCHAR(50),
		ReferenceNo     NVARCHAR(100),
		Narration       NVARCHAR(MAX),
		Attachment      NVARCHAR(255),
		TotalDebit      DECIMAL(18,2),
		TotalCredit     DECIMAL(18,2),
		Status          NVARCHAR(30),
		ReversalOf      INT NULL,
		PostedBy        INT NULL,
		PostedAt        DATETIME NULL,
		IsAutoPosted    BIT,
		RefModule       NVARCHAR(50),
		RefId           INT NULL,
		BranchId        INT NULL,
		CompanyId       INT NULL,
		PeriodId        INT NULL,
		CreatedBy       INT NULL,
		CreatedAt       DATETIME NULL
	);

	INSERT INTO @Header
	SELECT
		Hdr.value('(VoucherNo/text())[1]', 'nvarchar(100)'),
		Hdr.value('(VoucherDate/text())[1]', 'date'),
		NULLIF(Hdr.value('(VoucherType/text())[1]', 'nvarchar(50)'), ''),
		NULLIF(Hdr.value('(ReferenceNo/text())[1]', 'nvarchar(100)'), ''),
		NULLIF(Hdr.value('(Narration/text())[1]', 'nvarchar(max)'), ''),
		NULLIF(Hdr.value('(Attachment/text())[1]', 'nvarchar(255)'), ''),
		Hdr.value('(TotalDebit/text())[1]', 'decimal(18,2)'),
		Hdr.value('(TotalCredit/text())[1]', 'decimal(18,2)'),
		NULLIF(Hdr.value('(Status/text())[1]', 'nvarchar(30)'), ''),
		NULLIF(Hdr.value('(ReversalOf/text())[1]', 'int'), 0),
		NULLIF(Hdr.value('(PostedBy/text())[1]', 'int'), 0),
		NULLIF(Hdr.value('(PostedAt/text())[1]', 'datetime'), '1900-01-01'),
		Hdr.value('(IsAutoPosted/text())[1]', 'bit'),
		NULLIF(Hdr.value('(RefModule/text())[1]', 'nvarchar(50)'), ''),
		NULLIF(Hdr.value('(RefId/text())[1]', 'int'), 0),
		NULLIF(Hdr.value('(BranchId/text())[1]', 'int'), 0),
		NULLIF(Hdr.value('(CompanyId/text())[1]', 'int'), 0),
		NULLIF(Hdr.value('(PeriodId/text())[1]', 'int'), 0),
		NULLIF(Hdr.value('(CreatedBy/text())[1]', 'int'), 0),
		NULLIF(Hdr.value('(CreatedAt/text())[1]', 'datetime'), '1900-01-01')
	FROM @VoucherXml.nodes('/Voucher/Header') AS X(Hdr);

	DECLARE @VoucherNo NVARCHAR(100);
	DECLARE @VoucherDate DATE;
	DECLARE @VoucherType NVARCHAR(50);
	DECLARE @ReferenceNo NVARCHAR(100);
	DECLARE @Narration NVARCHAR(MAX);
	DECLARE @Attachment NVARCHAR(255);
	DECLARE @TotalDebit DECIMAL(18,2);
	DECLARE @TotalCredit DECIMAL(18,2);
	DECLARE @Status NVARCHAR(30);
	DECLARE @ReversalOf INT;
	DECLARE @PostedBy INT;
	DECLARE @PostedAt DATETIME;
	DECLARE @IsAutoPosted BIT;
	DECLARE @RefModule NVARCHAR(50);
	DECLARE @RefId INT;
	DECLARE @BranchId INT;
	DECLARE @CompanyId INT;
	DECLARE @PeriodId INT;
	DECLARE @CreatedBy INT;
	DECLARE @CreatedAt DATETIME;

	SELECT TOP 1
		@VoucherNo = VoucherNo,
		@VoucherDate = VoucherDate,
		@VoucherType = VoucherType,
		@ReferenceNo = ReferenceNo,
		@Narration = Narration,
		@Attachment = Attachment,
		@TotalDebit = TotalDebit,
		@TotalCredit = TotalCredit,
		@Status = ISNULL(Status, 'Posted'),
		@ReversalOf = ReversalOf,
		@PostedBy = PostedBy,
		@PostedAt = ISNULL(PostedAt, GETDATE()),
		@IsAutoPosted = IsAutoPosted,
		@RefModule = RefModule,
		@RefId = RefId,
		@BranchId = BranchId,
		@CompanyId = CompanyId,
		@PeriodId = PeriodId,
		@CreatedBy = CreatedBy,
		@CreatedAt = ISNULL(CreatedAt, GETDATE())
	FROM @Header;

	IF @VoucherNo IS NULL OR LTRIM(RTRIM(@VoucherNo)) = ''
	BEGIN
		RAISERROR('Voucher number is required.', 16, 1);
		RETURN;
	END

	IF EXISTS (SELECT 1 FROM acc_entries_header WHERE InvoiceNo = @VoucherNo AND branch_id = @BranchId)
	BEGIN
		RAISERROR('Voucher number already exists.', 16, 1);
		RETURN;
	END

	IF ISNULL(@TotalDebit, 0) <> ISNULL(@TotalCredit, 0)
	BEGIN
		RAISERROR('Voucher is not balanced.', 16, 1);
		RETURN;
	END

	IF ISNULL(@PeriodId, 0) <= 0
	BEGIN
		SELECT TOP 1 @PeriodId = period_id
		FROM acc_financial_periods
		WHERE @VoucherDate BETWEEN start_date AND end_date
		ORDER BY start_date DESC;
	END

	IF ISNULL(@PeriodId, 0) <= 0
	BEGIN
		RAISERROR('No accounting period exists for the voucher date.', 16, 1);
		RETURN;
	END

	DECLARE @Lines TABLE
	(
		[LineNo]            INT,
		AccountId           INT,
		Debit               DECIMAL(18,2),
		Credit              DECIMAL(18,2),
		Narration           NVARCHAR(MAX),
		CostCenter          NVARCHAR(100),
		ModuleName          NVARCHAR(50),
		RefId               INT NULL,
		PeriodId            INT NULL
	);

	INSERT INTO @Lines
		([LineNo], AccountId, Debit, Credit, Narration, CostCenter, ModuleName, RefId, PeriodId)
	SELECT
		Ln.value('(LineNo/text())[1]', 'int'),
		Ln.value('(AccountId/text())[1]', 'int'),
		Ln.value('(Debit/text())[1]', 'decimal(18,2)'),
		Ln.value('(Credit/text())[1]', 'decimal(18,2)'),
		NULLIF(Ln.value('(Narration/text())[1]', 'nvarchar(max)'), ''),
		NULLIF(Ln.value('(CostCenter/text())[1]', 'nvarchar(100)'), ''),
		NULLIF(Ln.value('(ModuleName/text())[1]', 'nvarchar(50)'), ''),
		NULLIF(Ln.value('(RefId/text())[1]', 'int'), 0),
		NULLIF(Ln.value('(PeriodId/text())[1]', 'int'), 0)
	FROM @VoucherXml.nodes('/Voucher/Lines/Line') AS X(Ln);

	IF NOT EXISTS (SELECT 1 FROM @Lines)
	BEGIN
		RAISERROR('At least one line is required.', 16, 1);
		RETURN;
	END

	IF EXISTS (
		SELECT 1
		FROM @Lines L
		LEFT JOIN acc_accounts A ON A.id = L.AccountId
		WHERE A.id IS NULL OR ISNULL(A.is_active, 1) = 0
	)
	BEGIN
		RAISERROR('One or more account lines are invalid or inactive.', 16, 1);
		RETURN;
	END

	BEGIN TRANSACTION;
	BEGIN TRY
		DECLARE @HeaderId INT;
		DECLARE @Inserted TABLE (EntryId INT);

		INSERT INTO acc_entries_header
		(
			InvoiceNo, EntryDate, VoucherType, ReferenceNo, Narration, Attachment,
			total_debit, total_credit, status, reversal_of, posted_by, posted_at,
			is_auto_posted, period_id, date_created, date_updated, user_id, branch_id
		)
		VALUES
		(
			@VoucherNo, @VoucherDate, @VoucherType, @ReferenceNo, @Narration, @Attachment,
			@TotalDebit, @TotalCredit, @Status, @ReversalOf, @PostedBy, @PostedAt,
			@IsAutoPosted, @PeriodId, @CreatedAt, GETDATE(), @CreatedBy, @BranchId
		);

		SET @HeaderId = SCOPE_IDENTITY();

		INSERT INTO acc_entries
		(
			invoice_no, account_id, entry_date, debit, credit, description,
			user_id, branch_id, period_id, date_created --customer_id, supplier_id, bank_id, entry_id, payment_ref_invoice_no

		)
		OUTPUT INSERTED.id INTO @Inserted(EntryId)
		SELECT
			@VoucherNo,
			L.AccountId,
			@VoucherDate,
			L.Debit,
			L.Credit,
			COALESCE(L.Narration, @Narration),
			@CreatedBy,
			@BranchId,
			COALESCE(L.PeriodId, @PeriodId),
			GETDATE()

		FROM @Lines L;
		
		UPDATE acc_entries_header
		SET status = 'Posted',
			posted_by = @PostedBy,
			posted_at = @PostedAt,
			date_updated = GETDATE()
		WHERE id = @HeaderId;

		SELECT @HeaderId AS VoucherId, @VoucherNo AS VoucherNo;
		SELECT EntryId FROM @Inserted ORDER BY EntryId;

		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
		IF @@TRANCOUNT > 0
			ROLLBACK TRANSACTION;
		THROW;
	END CATCH
END
