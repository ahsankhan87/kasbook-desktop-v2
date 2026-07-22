using POS.Core;
using System;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class PeriodDAL : AccountingDalBase
    {
        public PeriodDAL()
        {
        }

        public PeriodDAL(string connectionString)
            : base(connectionString)
        {
        }

        public DataTable GetAllPeriods(int yearId)
        {
            try
            {
                return ExecuteDataTable(@"
                    SELECT p.period_id,
                            p.year_id,
                            fy.name AS financial_year,
                            p.period_name,
                            p.start_date,
                            p.end_date,
                            p.status,
                            CASE
                                WHEN p.closed_by IS NULL THEN ''
                                ELSE LEFT(CONCAT('', p.closed_by), 50)
                            END AS closed_by,
                            p.closed_at,
                            ISNULL(v.trx_count, 0) AS transactions_count,
                            CASE WHEN p.status = 'SoftClosed' THEN CAST(1 AS bit) ELSE CAST(0 AS bit) END AS can_reopen
                    FROM acc_financial_periods p
                    LEFT JOIN acc_fiscal_years fy ON fy.id = p.year_id
                    OUTER APPLY
                    (
                        SELECT COUNT(1) AS trx_count
                        FROM acc_entries_header hh
                        WHERE hh.EntryDate >= p.start_date
                            AND hh.EntryDate < DATEADD(DAY, 1, p.end_date)
                    ) v
                    WHERE p.year_id = @year_id
                    ORDER BY p.start_date;",
                    cmd => cmd.Parameters.AddWithValue("@year_id", yearId));
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load financial periods.", ex);
            }
        }

        public bool IsPeriodLocked(DateTime date)
        {
            try
            {
                string status = ExecuteScalar<string>(@"
                    SELECT TOP 1 status
                    FROM acc_financial_periods
                    WHERE @dt BETWEEN start_date AND end_date
                    ORDER BY start_date DESC;",
                    cmd => cmd.Parameters.AddWithValue("@dt", date.Date));

                if (string.IsNullOrWhiteSpace(status))
                {
                    return true;
                }

                return !string.Equals(status, "Open", StringComparison.OrdinalIgnoreCase);
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to check period lock state.", ex);
            }
        }

        public int GetCurrentPeriodId(DateTime date)
        {
            try
            {
                return ExecuteScalar<int>(@"
                    SELECT TOP 1 period_id
                    FROM acc_financial_periods
                    WHERE @dt BETWEEN start_date AND end_date
                    ORDER BY start_date DESC;",
                    cmd => cmd.Parameters.AddWithValue("@dt", date.Date));
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to resolve current period.", ex);
            }
        }

        public int CreateNextMonthPeriod(int yearId)
        {
            try
            {
                return ExecuteScalar<int>(@"
DECLARE @nextStart date;
DECLARE @nextEnd date;
DECLARE @periodName nvarchar(60);

SELECT TOP 1 @nextStart = DATEADD(DAY, 1, end_date)
FROM acc_financial_periods
WHERE year_id = @year_id
ORDER BY end_date DESC;

IF @nextStart IS NULL
BEGIN
    SELECT @nextStart = CAST(from_date AS date)
    FROM acc_fiscal_years
    WHERE id = @year_id;
END

IF @nextStart IS NULL
BEGIN
    SELECT 0;
    RETURN;
END

SET @nextEnd = EOMONTH(@nextStart);
SET @periodName = DATENAME(MONTH, @nextStart) + ' ' + CAST(YEAR(@nextStart) AS nvarchar(4));

IF EXISTS (SELECT 1 FROM acc_financial_periods WHERE year_id = @year_id AND start_date = @nextStart)
BEGIN
    SELECT 0;
    RETURN;
END

INSERT INTO acc_financial_periods(year_id, period_name, start_date, end_date, status, closed_by, closed_at)
VALUES(@year_id, @periodName, @nextStart, @nextEnd, 'Open', NULL, NULL);

SELECT CAST(SCOPE_IDENTITY() AS int);",
                    cmd => cmd.Parameters.AddWithValue("@year_id", yearId));
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to create next financial period.", ex);
            }
        }

        public DataTable GetPeriodCloseChecklist(int periodId)
        {
            try
            {
                return ExecuteDataTable("sp_CheckPeriodClose",
                    cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@period_id", periodId);
                    });
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to evaluate period close checklist.", ex);
            }
        }

        public DataTable GetPeriodSummary(int periodId)
        {
            try
            {
                return ExecuteDataTable(@"
SELECT p.period_id,
       p.period_name,
       ISNULL(v.total_transactions, 0) AS total_transactions,
       ISNULL(v.total_journals, 0) AS total_journals,
       ISNULL(t.total_debits, 0) AS total_debits,
       ISNULL(t.total_credits, 0) AS total_credits,
       ISNULL(t.net_profit, 0) AS net_profit,
       ISNULL(ob.out_of_balance_entries, 0) AS out_of_balance_entries
FROM acc_financial_periods p
OUTER APPLY
(
    SELECT COUNT(1) AS total_transactions,
           SUM(CASE WHEN ISNULL(h.VoucherType, '') = 'General Journal' THEN 1 ELSE 0 END) AS total_journals
    FROM acc_entries_header h
    WHERE h.EntryDate >= p.start_date
      AND h.EntryDate < DATEADD(DAY, 1, p.end_date)
) v
OUTER APPLY
(
    SELECT SUM(ISNULL(e.debit, 0)) AS total_debits,
           SUM(ISNULL(e.credit, 0)) AS total_credits,
           SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0)) AS net_profit
    FROM acc_entries e
    INNER JOIN acc_entries_header h ON h.InvoiceNo = e.invoice_no
    WHERE h.EntryDate >= p.start_date
      AND h.EntryDate < DATEADD(DAY, 1, p.end_date)
) t
OUTER APPLY
(
    SELECT COUNT(1) AS out_of_balance_entries
    FROM
    (
        SELECT e.invoice_no,
               SUM(ISNULL(e.debit, 0) - ISNULL(e.credit, 0)) AS balance_check
        FROM acc_entries e
        INNER JOIN acc_entries_header h ON h.InvoiceNo = e.invoice_no
        WHERE h.EntryDate >= p.start_date
          AND h.EntryDate < DATEADD(DAY, 1, p.end_date)
        GROUP BY e.invoice_no
    ) b
    WHERE ABS(ISNULL(b.balance_check, 0)) > 0.009
) ob
WHERE p.period_id = @period_id;",
                    cmd => cmd.Parameters.AddWithValue("@period_id", periodId));
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load period summary.", ex);
            }
        }

        public DataTable GetPeriodTransactions(int periodId)
        {
            try
            {
                return ExecuteDataTable(@"
                    SELECT h.InvoiceNo AS voucher_no,
                           h.EntryDate AS entry_date,
                           h.VoucherType AS voucher_type,
                           ISNULL(h.ReferenceNo, '') AS reference_no,
                           ISNULL(h.Narration, '') AS narration,
                           ISNULL(SUM(ISNULL(e.debit, 0)), 0) AS debit_total,
                           ISNULL(SUM(ISNULL(e.credit, 0)), 0) AS credit_total
                    FROM acc_financial_periods p
                    INNER JOIN acc_entries_header h
                        ON h.EntryDate >= p.start_date
                       AND h.EntryDate < DATEADD(DAY, 1, p.end_date)
                    LEFT JOIN acc_entries e ON h.InvoiceNo = e.invoice_no
                    WHERE p.period_id = @period_id
                    GROUP BY h.InvoiceNo, h.EntryDate, h.VoucherType, h.ReferenceNo, h.Narration
                    ORDER BY h.EntryDate, h.InvoiceNo;",
                    cmd => cmd.Parameters.AddWithValue("@period_id", periodId));
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load period transactions.", ex);
            }
        }

        public FinancialPeriodCloseResultModal ClosePeriod(FinancialPeriodCloseOptionsModal options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            try
            {
                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand("sp_ClosePeriod", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@period_id", options.period_id);
                    cmd.Parameters.AddWithValue("@close_type", string.IsNullOrWhiteSpace(options.close_type) ? "Soft" : options.close_type);
                    cmd.Parameters.AddWithValue("@closed_by", options.user_id);
                    cmd.Parameters.AddWithValue("@auto_post_depreciation", options.auto_post_depreciation);
                    cmd.Parameters.AddWithValue("@reverse_prior_accruals", options.reverse_prior_accruals);
                    cmd.Parameters.AddWithValue("@confirmation_text", options.confirmation_text ?? string.Empty);

                    SqlParameter successOutput = new SqlParameter("@is_success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter messageOutput = new SqlParameter("@result_message", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(successOutput);
                    cmd.Parameters.Add(messageOutput);

                    cn.Open();
                    int affected = cmd.ExecuteNonQuery();

                    return new FinancialPeriodCloseResultModal
                    {
                        success = successOutput.Value != DBNull.Value && Convert.ToBoolean(successOutput.Value),
                        message = Convert.ToString(messageOutput.Value),
                        affected_rows = affected
                    };
                }
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to close financial period.", ex);
            }
        }

        public int ReopenSoftClosedPeriod(int periodId, int userId, string reason)
        {
            try
            {
                return ExecuteNonQuery(@"
                    UPDATE acc_financial_periods
                    SET status = 'Open',
                        closed_by = NULL,
                        closed_at = NULL
                    WHERE period_id = @period_id
                      AND status = 'SoftClosed';

                    IF @@ROWCOUNT > 0 AND OBJECT_ID('acc_financial_period_reopen_logs', 'U') IS NOT NULL
                    BEGIN
                        INSERT INTO acc_financial_period_reopen_logs(period_id, reopened_by, reopened_at, reason)
                        VALUES(@period_id, @user_id, GETDATE(), @reason);
                    END;",
                    cmd =>
                    {
                        cmd.Parameters.AddWithValue("@period_id", periodId);
                        cmd.Parameters.AddWithValue("@user_id", userId);
                        cmd.Parameters.AddWithValue("@reason", string.IsNullOrWhiteSpace(reason) ? string.Empty : reason.Trim());
                    });
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to reopen financial period.", ex);
            }
        }

        public int UpdatePeriodStatus(int periodId, string status, int userId)
        {
            try
            {
                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand(@"
UPDATE acc_financial_periods
SET status = @status,
    closed_by = CASE WHEN @status = 'Open' THEN NULL ELSE LEFT(CONCAT('', @user_id), 50) END,
    closed_at = CASE WHEN @status = 'Open' THEN NULL ELSE @closed_at END
WHERE period_id = @period_id;", cn))
                {
                    cmd.Parameters.AddWithValue("@period_id", periodId);
                    cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(status) ? "Open" : status.Trim());
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@closed_at", DateTime.Now);
                    cn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to update financial period status.", ex);
            }
        }

        public DataTable GetYearEndPreCloseValidationReport(int yearId)
        {
            if (yearId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(yearId));
            }

            try
            {
                return ExecuteDataTable("sp_YearEndPreCloseValidation",
                    cmd =>
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@year_id", yearId);
                    });
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load year-end pre-close validation report.", ex);
            }
        }

        public YearEndCloseResultModal ExecuteYearEndClose(YearEndCloseOptionsModal options, Action<string> progressCallback = null)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            if (options.year_id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.year_id));
            }

            if (options.user_id <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(options.user_id));
            }

            try
            {
                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand("sp_YearEndClose", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 300;

                    cmd.Parameters.AddWithValue("@year_id", options.year_id);
                    cmd.Parameters.AddWithValue("@user_id", options.user_id);
                    cmd.Parameters.AddWithValue("@branch_id", (object)options.branch_id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@income_summary_account_id", (object)options.income_summary_account_id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@retained_earnings_account_id", (object)options.retained_earnings_account_id ?? DBNull.Value);

                    SqlParameter successOutput = new SqlParameter("@is_success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter messageOutput = new SqlParameter("@result_message", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    SqlParameter runIdOutput = new SqlParameter("@run_id", SqlDbType.Int) { Direction = ParameterDirection.Output };
                    SqlParameter netOutput = new SqlParameter("@net_profit_loss", SqlDbType.Decimal)
                    {
                        Direction = ParameterDirection.Output,
                        Precision = 18,
                        Scale = 2
                    };
                    SqlParameter closingVoucherOutput = new SqlParameter("@closing_voucher_no", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };
                    SqlParameter openingVoucherOutput = new SqlParameter("@opening_voucher_no", SqlDbType.NVarChar, 100) { Direction = ParameterDirection.Output };

                    cmd.Parameters.Add(successOutput);
                    cmd.Parameters.Add(messageOutput);
                    cmd.Parameters.Add(runIdOutput);
                    cmd.Parameters.Add(netOutput);
                    cmd.Parameters.Add(closingVoucherOutput);
                    cmd.Parameters.Add(openingVoucherOutput);

                    DataSet dataSet = new DataSet();
                    SqlInfoMessageEventHandler infoHandler = null;
                    infoHandler = (sender, args) =>
                    {
                        if (progressCallback != null && !string.IsNullOrWhiteSpace(args.Message))
                        {
                            progressCallback(args.Message.Trim());
                        }
                    };

                    cn.FireInfoMessageEventOnUserErrors = true;
                    cn.InfoMessage += infoHandler;

                    try
                    {
                        cn.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dataSet);
                        }
                    }
                    finally
                    {
                        cn.InfoMessage -= infoHandler;
                    }

                    return new YearEndCloseResultModal
                    {
                        success = successOutput.Value != DBNull.Value && Convert.ToBoolean(successOutput.Value),
                        message = Convert.ToString(messageOutput.Value),
                        run_id = runIdOutput.Value == DBNull.Value ? 0 : Convert.ToInt32(runIdOutput.Value),
                        net_profit_loss = netOutput.Value == DBNull.Value ? 0m : Convert.ToDecimal(netOutput.Value),
                        closing_voucher_no = Convert.ToString(closingVoucherOutput.Value),
                        opening_voucher_no = Convert.ToString(openingVoucherOutput.Value),
                        pre_close_validation_report = dataSet.Tables.Count > 0 ? dataSet.Tables[0] : new DataTable()
                    };
                }
            }
            catch (SqlException ex)
            {
                throw new DataException("Failed to execute year-end close.", ex);
            }
            catch (Exception ex)
            {
                throw new DataException("Unexpected error during year-end close.", ex);
            }
        }

        public YearEndRollbackResultModal RollbackYearEndClose(int yearId, int userId, string reason)
        {
            if (yearId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(yearId));
            }

            if (userId <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(userId));
            }

            try
            {
                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand("sp_RollbackYearEndClose", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@year_id", yearId);
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@reason", string.IsNullOrWhiteSpace(reason) ? (object)DBNull.Value : reason.Trim());

                    SqlParameter successOutput = new SqlParameter("@is_success", SqlDbType.Bit) { Direction = ParameterDirection.Output };
                    SqlParameter messageOutput = new SqlParameter("@result_message", SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                    cmd.Parameters.Add(successOutput);
                    cmd.Parameters.Add(messageOutput);

                    cn.Open();
                    cmd.ExecuteNonQuery();

                    return new YearEndRollbackResultModal
                    {
                        success = successOutput.Value != DBNull.Value && Convert.ToBoolean(successOutput.Value),
                        message = Convert.ToString(messageOutput.Value)
                    };
                }
            }
            catch (SqlException ex)
            {
                throw new DataException("Failed to roll back year-end close.", ex);
            }
            catch (Exception ex)
            {
                throw new DataException("Unexpected error during year-end close rollback.", ex);
            }
        }
    }
}
