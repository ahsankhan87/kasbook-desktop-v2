using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace POS.DLL
{
    public partial class JournalsDLL : AccountingDalBase
    {
        public int SaveVoucherWithLines(JVHeaderModel header, List<JVLineModel> lines, SqlTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (header == null)
            {
                throw new ArgumentNullException("header");
            }

            if (lines == null || lines.Count == 0)
            {
                throw new ArgumentException("Voucher must contain at least one line.", "lines");
            }

            try
            {
                int periodId = new AccountingDAL(ConnectionString).GetCurrentPeriodId(header.VoucherDate);
                if (periodId <= 0)
                {
                    throw new DataException("No accounting period exists for the voucher date.");
                }

                header.VoucherNo = string.IsNullOrWhiteSpace(header.VoucherNo)
                    ? new AccountingDAL(ConnectionString).GenerateVoucherNo(header.VoucherType)
                    : header.VoucherNo.Trim();

                header.TotalDebit = lines.Sum(x => x.Debit);
                header.TotalCredit = lines.Sum(x => x.Credit);
                header.Status = string.IsNullOrWhiteSpace(header.Status) ? "Posted" : header.Status.Trim();
                header.CreatedAt = header.CreatedAt.HasValue ? header.CreatedAt.Value : DateTime.Now;
                header.CreatedBy = header.CreatedBy ?? header.PostedBy ?? 0;

                using (SqlCommand cmd = new SqlCommand(@"
INSERT INTO acc_vouchers
    (voucher_no, voucher_date, voucher_type, period_id, narration, ref_no, attachment_path, total_debit, total_credit, status, reversal_of, created_by, created_at, posted_by, posted_at, is_auto_posted)
VALUES
    (@voucher_no, @voucher_date, @voucher_type, @period_id, @narration, @ref_no, @attachment_path, @total_debit, @total_credit, @status, @reversal_of, @created_by, @created_at, @posted_by, @posted_at, @is_auto_posted);
SELECT CAST(SCOPE_IDENTITY() AS INT);", transaction.Connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@voucher_no", header.VoucherNo);
                    cmd.Parameters.AddWithValue("@voucher_date", header.VoucherDate.Date);
                    cmd.Parameters.AddWithValue("@voucher_type", string.IsNullOrWhiteSpace(header.VoucherType) ? "Journal" : header.VoucherType.Trim());
                    cmd.Parameters.AddWithValue("@period_id", periodId);
                    cmd.Parameters.AddWithValue("@narration", DbValue(header.Narration));
                    cmd.Parameters.AddWithValue("@ref_no", DbValue(header.ReferenceNo));
                    cmd.Parameters.AddWithValue("@attachment_path", DbValue(header.Attachment));
                    cmd.Parameters.AddWithValue("@total_debit", header.TotalDebit);
                    cmd.Parameters.AddWithValue("@total_credit", header.TotalCredit);
                    cmd.Parameters.AddWithValue("@status", header.Status);
                    cmd.Parameters.AddWithValue("@reversal_of", header.ReversalOf.HasValue ? (object)header.ReversalOf.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@created_by", header.CreatedBy);
                    cmd.Parameters.AddWithValue("@created_at", header.CreatedAt.Value);
                    cmd.Parameters.AddWithValue("@posted_by", header.PostedBy.HasValue ? (object)header.PostedBy.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@posted_at", header.PostedAt.HasValue ? (object)header.PostedAt.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_auto_posted", header.IsAutoPosted);

                    object voucherIdObj = cmd.ExecuteScalar();
                    int voucherId = voucherIdObj == null || voucherIdObj == DBNull.Value ? 0 : Convert.ToInt32(voucherIdObj);
                    if (voucherId <= 0)
                    {
                        throw new DataException("Voucher header insert failed.");
                    }

                    foreach (JVLineModel line in lines)
                    {
                        line.VoucherId = voucherId;
                    }

                    BulkInsertEntries(lines, transaction, voucherId, header.VoucherNo, header.VoucherDate, periodId, header.CreatedBy ?? 0);
                    return voucherId;
                }
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to save voucher with lines.", ex);
            }
        }

        public int BulkInsertEntries(List<JVLineModel> lines, SqlTransaction transaction)
        {
            return BulkInsertEntries(lines, transaction, 0, null, DateTime.Today, 0, 0);
        }

        public int BulkInsertEntries(List<JVLineModel> lines, SqlTransaction transaction, int voucherId, string voucherNo, DateTime entryDate, int periodId, int createdBy)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            if (lines == null || lines.Count == 0)
            {
                return 0;
            }

            DataTable table = BuildEntriesDataTable(lines, voucherId, voucherNo, entryDate, periodId, createdBy);
            using (SqlBulkCopy bulk = new SqlBulkCopy(transaction.Connection, SqlBulkCopyOptions.CheckConstraints, transaction))
            {
                bulk.DestinationTableName = "acc_entries";
                bulk.BatchSize = 1000;
                bulk.BulkCopyTimeout = 120;
                bulk.ColumnMappings.Add("voucher_id", "voucher_id");
                bulk.ColumnMappings.Add("voucher_no", "voucher_no");
                bulk.ColumnMappings.Add("entry_date", "entry_date");
                bulk.ColumnMappings.Add("acc_id", "acc_id");
                bulk.ColumnMappings.Add("debit", "debit");
                bulk.ColumnMappings.Add("credit", "credit");
                bulk.ColumnMappings.Add("narration", "narration");
                bulk.ColumnMappings.Add("cost_center_id", "cost_center_id");
                bulk.ColumnMappings.Add("ref_module", "ref_module");
                bulk.ColumnMappings.Add("ref_id", "ref_id");
                bulk.ColumnMappings.Add("period_id", "period_id");
                bulk.ColumnMappings.Add("created_by", "created_by");
                bulk.ColumnMappings.Add("created_at", "created_at");
                bulk.WriteToServer(table);
            }

            return table.Rows.Count;
        }

        public DataTable GetVoucherList(VoucherFilter filter)
        {
            try
            {
                VoucherFilter safe = filter ?? new VoucherFilter();
                int page = safe.Page <= 0 ? 1 : safe.Page;
                int pageSize = safe.PageSize <= 0 ? 50 : safe.PageSize;
                int offset = (page - 1) * pageSize;

                StringBuilder sql = new StringBuilder();
                sql.AppendLine("SELECT voucher_id, voucher_no, voucher_date, voucher_type, period_id, narration, ref_no, attachment_path,");
                sql.AppendLine("       total_debit, total_credit, status, reversal_of, created_by, created_at, posted_by, posted_at, is_auto_posted,");
                sql.AppendLine("       COUNT(1) OVER() AS total_count");
                sql.AppendLine("FROM acc_vouchers");
                sql.AppendLine("WHERE 1 = 1");
                if (safe.FromDate.HasValue)
                {
                    sql.AppendLine("  AND voucher_date >= @from_date");
                }
                if (safe.ToDate.HasValue)
                {
                    sql.AppendLine("  AND voucher_date <= @to_date");
                }
                if (!string.IsNullOrWhiteSpace(safe.VoucherType))
                {
                    sql.AppendLine("  AND voucher_type = @voucher_type");
                }
                if (!string.IsNullOrWhiteSpace(safe.Status))
                {
                    sql.AppendLine("  AND status = @status");
                }
                if (safe.PeriodId.HasValue)
                {
                    sql.AppendLine("  AND period_id = @period_id");
                }
                if (safe.CreatedBy.HasValue)
                {
                    sql.AppendLine("  AND created_by = @created_by");
                }
                if (safe.PostedBy.HasValue)
                {
                    sql.AppendLine("  AND posted_by = @posted_by");
                }
                if (!string.IsNullOrWhiteSpace(safe.Search))
                {
                    sql.AppendLine("  AND (voucher_no LIKE @search OR narration LIKE @search OR ref_no LIKE @search)");
                }
                sql.AppendLine("ORDER BY voucher_date DESC, voucher_id DESC");
                sql.AppendLine("OFFSET @offset ROWS FETCH NEXT @fetch ROWS ONLY;");

                return ExecuteDataTable(sql.ToString(), cmd =>
                {
                    if (safe.FromDate.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@from_date", safe.FromDate.Value.Date);
                    }
                    if (safe.ToDate.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@to_date", safe.ToDate.Value.Date);
                    }
                    if (!string.IsNullOrWhiteSpace(safe.VoucherType))
                    {
                        cmd.Parameters.AddWithValue("@voucher_type", safe.VoucherType.Trim());
                    }
                    if (!string.IsNullOrWhiteSpace(safe.Status))
                    {
                        cmd.Parameters.AddWithValue("@status", safe.Status.Trim());
                    }
                    if (safe.PeriodId.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@period_id", safe.PeriodId.Value);
                    }
                    if (safe.CreatedBy.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@created_by", safe.CreatedBy.Value);
                    }
                    if (safe.PostedBy.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@posted_by", safe.PostedBy.Value);
                    }
                    if (!string.IsNullOrWhiteSpace(safe.Search))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + safe.Search.Trim() + "%");
                    }
                    cmd.Parameters.AddWithValue("@offset", offset);
                    cmd.Parameters.AddWithValue("@fetch", pageSize);
                });
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load voucher list.", ex);
            }
        }

        public int UpdateVoucherStatus(int voucherId, string status, int userId, SqlTransaction transaction)
        {
            if (transaction == null)
            {
                throw new ArgumentNullException("transaction");
            }

            try
            {
                using (SqlCommand cmd = new SqlCommand(@"
UPDATE acc_vouchers
SET status = @status,
    posted_by = CASE WHEN @status = 'Posted' THEN @user_id ELSE posted_by END,
    posted_at = CASE WHEN @status = 'Posted' THEN @posted_at ELSE posted_at END
WHERE voucher_id = @voucher_id;", transaction.Connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@voucher_id", voucherId);
                    cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(status) ? "Draft" : status.Trim());
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@posted_at", DateTime.Now);
                    return cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to update voucher status.", ex);
            }
        }

        public string GetNextVoucherNo(string voucherType)
        {
            try
            {
                return new AccountingDAL(ConnectionString).GenerateVoucherNo(voucherType);
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to generate voucher number.", ex);
            }
        }

        private static DataTable BuildEntriesDataTable(List<JVLineModel> lines, int voucherId, string voucherNo, DateTime entryDate, int periodId, int createdBy)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("voucher_id", typeof(int));
            dt.Columns.Add("voucher_no", typeof(string));
            dt.Columns.Add("entry_date", typeof(DateTime));
            dt.Columns.Add("acc_id", typeof(int));
            dt.Columns.Add("debit", typeof(decimal));
            dt.Columns.Add("credit", typeof(decimal));
            dt.Columns.Add("narration", typeof(string));
            dt.Columns.Add("cost_center_id", typeof(int));
            dt.Columns.Add("ref_module", typeof(string));
            dt.Columns.Add("ref_id", typeof(int));
            dt.Columns.Add("period_id", typeof(int));
            dt.Columns.Add("created_by", typeof(int));
            dt.Columns.Add("created_at", typeof(DateTime));

            for (int i = 0; i < lines.Count; i++)
            {
                JVLineModel line = lines[i];
                int? costCenterId = ResolveCostCenterId(line.CostCenter);
                dt.Rows.Add(
                    voucherId,
                    voucherNo ?? string.Empty,
                    entryDate.Date,
                    line.AccountId,
                    line.Debit,
                    line.Credit,
                    (object)NormalizeText(line.Narration) ?? DBNull.Value,
                    costCenterId.HasValue ? (object)costCenterId.Value : DBNull.Value,
                    (object)NormalizeText(line.ModuleName) ?? DBNull.Value,
                    line.RefId.HasValue ? (object)line.RefId.Value : DBNull.Value,
                    periodId,
                    createdBy,
                    DateTime.Now);
            }

            return dt;
        }

        private static int? ResolveCostCenterId(string costCenter)
        {
            int id;
            if (int.TryParse(costCenter, out id) && id > 0)
            {
                return id;
            }

            return null;
        }
    }
}
