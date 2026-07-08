using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    public partial class JournalsDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private JournalsModal info = new JournalsModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "5");

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }
            
        }

        public DataTable SearchRecordByJournalsID(int Journals_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Journals_id);
                        cmd.Parameters.AddWithValue("@OperationType", "4"); 
                        
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        return dt;

                    }

                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable SearchRecord(String condition)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT id,code,name,date_created FROM acc_Journals WHERE name LIKE @name", cn);
                        //cmd.Parameters.AddWithValue("@id", condition);
                        cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", condition));
                        
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        return dt;

                    }

                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        public String GetMaxInvoiceNo(string prefix = "J")
        {
            // Generates Invoice like J1-20230708-0001
            return GenerateDailyInvoiceNo("acc_entries", "invoice_no", prefix);
        }

        public int InsertHeader(JournalsModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand(@"INSERT INTO acc_entries_header
                            (InvoiceNo, EntryDate, VoucherType, ReferenceNo, Narration, Attachment, total_debit, total_credit, status, reversal_of, posted_by, posted_at, is_auto_posted, date_created, date_updated, user_id, branch_id)
                            VALUES
                            (@InvoiceNo, @EntryDate, @VoucherType, @ReferenceNo, @Narration, @Attachment, @total_debit, @total_credit, @status, @reversal_of, @posted_by, @posted_at, @is_auto_posted, @date_created, @date_updated, @user_id, @branch_id);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);
                        cmd.Parameters.AddWithValue("@InvoiceNo", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@EntryDate", obj.entry_date.Date);
                        cmd.Parameters.AddWithValue("@VoucherType", (object)obj.voucher_type ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ReferenceNo", (object)obj.reference_no ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Narration", (object)obj.narration ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Attachment", (object)obj.attachment ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@total_debit", obj.total_debit);
                        cmd.Parameters.AddWithValue("@total_credit", obj.total_credit);
                        cmd.Parameters.AddWithValue("@status", (object)obj.status ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@reversal_of", (object)obj.reversal_of ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@posted_by", (object)obj.posted_by ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@posted_at", (object)obj.posted_at ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@is_auto_posted", obj.is_auto_posted ? 1 : 0);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                    }

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch
                {
                    throw;
                }
            }
        }

        public int InsertVoucher(JournalsModal header, List<JournalsModal> lines)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        int headerId;
                        using (SqlCommand headerCmd = new SqlCommand(@"INSERT INTO acc_entries_header
                            (InvoiceNo, EntryDate, VoucherType, ReferenceNo, Narration, Attachment, total_debit, total_credit, status, reversal_of, posted_by, posted_at, is_auto_posted, date_created, date_updated, user_id, branch_id)
                            VALUES
                            (@InvoiceNo, @EntryDate, @VoucherType, @ReferenceNo, @Narration, @Attachment, @total_debit, @total_credit, @status, @reversal_of, @posted_by, @posted_at, @is_auto_posted, @date_created, @date_updated, @user_id, @branch_id);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            headerCmd.Parameters.AddWithValue("@InvoiceNo", header.invoice_no);
                            headerCmd.Parameters.AddWithValue("@EntryDate", header.entry_date.Date);
                            headerCmd.Parameters.AddWithValue("@VoucherType", (object)header.voucher_type ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@ReferenceNo", (object)header.reference_no ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@Narration", (object)header.narration ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@Attachment", (object)header.attachment ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@total_debit", header.total_debit);
                            headerCmd.Parameters.AddWithValue("@total_credit", header.total_credit);
                            headerCmd.Parameters.AddWithValue("@status", (object)header.status ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@reversal_of", (object)header.reversal_of ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@posted_by", (object)header.posted_by ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@posted_at", (object)header.posted_at ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@is_auto_posted", header.is_auto_posted ? 1 : 0);
                            headerCmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            headerCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            headerCmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            headerCmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                            headerId = Convert.ToInt32(headerCmd.ExecuteScalar());
                        }

                        foreach (JournalsModal line in lines)
                        {
                            line.entry_id = headerId;
                            using (SqlCommand lineCmd = new SqlCommand("sp_JournalsCrud", cn, tx))
                            {
                                lineCmd.CommandType = CommandType.StoredProcedure;
                                lineCmd.Parameters.AddWithValue("@invoice_no", line.invoice_no);
                                lineCmd.Parameters.AddWithValue("@account_id", line.account_id);
                                lineCmd.Parameters.AddWithValue("@entry_date", line.entry_date);
                                lineCmd.Parameters.AddWithValue("@debit", line.debit);
                                lineCmd.Parameters.AddWithValue("@credit", line.credit);
                                lineCmd.Parameters.AddWithValue("@description", line.description);
                                lineCmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                lineCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                lineCmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                lineCmd.Parameters.AddWithValue("@customer_id", line.customer_id);
                                lineCmd.Parameters.AddWithValue("@supplier_id", line.supplier_id);
                                lineCmd.Parameters.AddWithValue("@bank_id", line.bank_id);
                                lineCmd.Parameters.AddWithValue("@entry_id", line.entry_id);
                                lineCmd.Parameters.AddWithValue("@payment_ref_invoice_no", line.payment_ref_invoice_no);
                                lineCmd.Parameters.AddWithValue("@OperationType", "1");
                                lineCmd.ExecuteScalar();
                            }
                        }

                        tx.Commit();
                        return headerId;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public DataTable GetVoucherHeaders(DateTime fromDate, DateTime toDate, string voucherType, string status, string search)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                string sql = @"
SELECT H.id,
       H.InvoiceNo AS VoucherNo,
       H.EntryDate AS VoucherDate,
       H.VoucherType,
       LEFT(ISNULL(H.Narration,''), 160) AS Narration,
       ISNULL(H.total_debit,0) AS TotalDebit,
       ISNULL(H.total_credit,0) AS TotalCredit,
       ISNULL(H.status,'Draft') AS Status,
       ISNULL((SELECT COUNT(1) FROM acc_entries E WHERE E.invoice_no = H.InvoiceNo AND E.branch_id = H.branch_id), 0) AS LinesCount,
       ISNULL(uc.name, uc.username) AS CreatedBy,
       ISNULL(up.name, up.username) AS PostedBy,
       H.posted_at,
       H.reversal_of,
       H.is_auto_posted
FROM acc_entries_header H
LEFT JOIN pos_users uc ON uc.id = H.user_id
LEFT JOIN pos_users up ON up.id = H.posted_by
WHERE H.branch_id = @branch_id
  AND H.EntryDate BETWEEN @from_date AND @to_date
  AND (@voucher_type = 'All' OR H.VoucherType = @voucher_type)
  AND (@status = 'All' OR ISNULL(H.status,'Draft') = @status)
  AND (
        @search = ''
        OR H.InvoiceNo LIKE @search_like
        OR H.Narration LIKE @search_like
        OR EXISTS (
            SELECT 1
            FROM acc_entries E2
            LEFT JOIN acc_accounts A2 ON A2.id = E2.account_id
            WHERE E2.invoice_no = H.InvoiceNo
              AND E2.branch_id = H.branch_id
              AND (
                    A2.code LIKE @search_like
                    OR A2.name LIKE @search_like
                    OR E2.description LIKE @search_like
                  )
        )
      )
ORDER BY H.EntryDate DESC, H.id DESC;";

                using (SqlCommand command = new SqlCommand(sql, cn))
                {
                    command.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    command.Parameters.AddWithValue("@from_date", fromDate.Date);
                    command.Parameters.AddWithValue("@to_date", toDate.Date);
                    command.Parameters.AddWithValue("@voucher_type", string.IsNullOrWhiteSpace(voucherType) ? "All" : voucherType);
                    command.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(status) ? "All" : status);
                    command.Parameters.AddWithValue("@search", string.IsNullOrWhiteSpace(search) ? string.Empty : search.Trim());
                    command.Parameters.AddWithValue("@search_like", string.IsNullOrWhiteSpace(search) ? string.Empty : "%" + search.Trim() + "%");

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        public DataTable GetVoucherLines(string invoiceNo)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlCommand command = new SqlCommand(@"
SELECT E.id,
       E.account_id,
       ISNULL(A.code,'') AS AccountCode,
       ISNULL(A.name,'') AS AccountName,
       ISNULL(A.code,'') + CASE WHEN ISNULL(A.name,'') <> '' THEN ' - ' + ISNULL(A.name,'') ELSE '' END AS AccountDisplay,
       ISNULL(E.description,'') AS Description,
       ISNULL(E.debit,0) AS Debit,
       ISNULL(E.credit,0) AS Credit
FROM acc_entries E
LEFT JOIN acc_accounts A ON A.id = E.account_id
WHERE E.invoice_no = @invoice_no
  AND E.branch_id = @branch_id
ORDER BY E.id;", cn))
                {
                    command.Parameters.AddWithValue("@invoice_no", invoiceNo);
                    command.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        public DataTable GetVoucherHeaderById(int headerId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlCommand command = new SqlCommand(@"
SELECT TOP 1 H.*
FROM acc_entries_header H
WHERE H.id = @id AND H.branch_id = @branch_id;", cn))
                {
                    command.Parameters.AddWithValue("@id", headerId);
                    command.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        public DataTable GetVoucherHeaderByInvoiceNo(string invoiceNo)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlCommand command = new SqlCommand(@"
SELECT TOP 1 H.*
FROM acc_entries_header H
WHERE H.InvoiceNo = @invoice_no AND H.branch_id = @branch_id;", cn))
                {
                    command.Parameters.AddWithValue("@invoice_no", invoiceNo);
                    command.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    DataTable dt = new DataTable();
                    using (SqlDataAdapter da = new SqlDataAdapter(command))
                    {
                        da.Fill(dt);
                    }
                    return dt;
                }
            }
        }

        public int PostDraftVouchers(List<int> headerIds)
        {
            return UpdateHeaderStatus(headerIds, "Posted", UsersModal.logged_in_userid, DateTime.Now);
        }

        public int DeleteDraftVouchers(List<int> headerIds)
        {
            if (headerIds == null || headerIds.Count == 0)
            {
                return 0;
            }

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        string inClause = BuildIdInClause(headerIds, "d");

                        using (SqlCommand selectCmd = new SqlCommand("SELECT InvoiceNo FROM acc_entries_header WHERE id IN (" + inClause + ") AND branch_id = @branch_id AND ISNULL(status,'Draft') = 'Draft'", cn, tx))
                        {
                            selectCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            AddIdParameters(selectCmd, headerIds, "d");

                            List<string> invoiceNos = new List<string>();
                            using (SqlDataReader reader = selectCmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    invoiceNos.Add(Convert.ToString(reader[0]));
                                }
                            }

                            if (invoiceNos.Count == 0)
                            {
                                tx.Rollback();
                                return 0;
                            }

                            foreach (string invoiceNo in invoiceNos)
                            {
                                using (SqlCommand lineDelete = new SqlCommand("DELETE FROM acc_entries WHERE invoice_no = @invoice_no AND branch_id = @branch_id", cn, tx))
                                {
                                    lineDelete.Parameters.AddWithValue("@invoice_no", invoiceNo);
                                    lineDelete.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    lineDelete.ExecuteNonQuery();
                                }
                            }

                            using (SqlCommand headerDelete = new SqlCommand("DELETE FROM acc_entries_header WHERE id IN (" + inClause + ") AND branch_id = @branch_id AND ISNULL(status,'Draft') = 'Draft'", cn, tx))
                            {
                                headerDelete.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                AddIdParameters(headerDelete, headerIds, "d");
                                int result = headerDelete.ExecuteNonQuery();
                                tx.Commit();
                                return result;
                            }
                        }
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public int DeleteDraftVoucherByInvoiceNo(string invoiceNo)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        int affected = 0;
                        using (SqlCommand headerSelect = new SqlCommand("SELECT id FROM acc_entries_header WHERE InvoiceNo = @invoice_no AND branch_id = @branch_id AND ISNULL(status,'Draft') = 'Draft'", cn, tx))
                        {
                            headerSelect.Parameters.AddWithValue("@invoice_no", invoiceNo);
                            headerSelect.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            object headerIdObj = headerSelect.ExecuteScalar();
                            if (headerIdObj == null || headerIdObj == DBNull.Value)
                            {
                                tx.Rollback();
                                return 0;
                            }

                            int headerId = Convert.ToInt32(headerIdObj);
                            using (SqlCommand lineDelete = new SqlCommand("DELETE FROM acc_entries WHERE invoice_no = @invoice_no AND branch_id = @branch_id", cn, tx))
                            {
                                lineDelete.Parameters.AddWithValue("@invoice_no", invoiceNo);
                                lineDelete.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                lineDelete.ExecuteNonQuery();
                            }

                            using (SqlCommand headerDelete = new SqlCommand("DELETE FROM acc_entries_header WHERE id = @id AND branch_id = @branch_id AND ISNULL(status,'Draft') = 'Draft'", cn, tx))
                            {
                                headerDelete.Parameters.AddWithValue("@id", headerId);
                                headerDelete.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                affected = headerDelete.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        return affected;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public int CreateReversalVoucher(int originalHeaderId, DateTime reversalDate, string reason)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        DataTable originalHeader = GetVoucherHeaderById(originalHeaderId);
                        if (originalHeader.Rows.Count == 0)
                        {
                            tx.Rollback();
                            return 0;
                        }

                        DataRow headerRow = originalHeader.Rows[0];
                        string originalInvoiceNo = Convert.ToString(headerRow["InvoiceNo"]);
                        DataTable originalLines = GetVoucherLines(originalInvoiceNo);

                        string reversalInvoiceNo = GenerateDailyInvoiceNo("acc_entries", "invoice_no", "J");
                        JournalsModal reversalHeader = new JournalsModal
                        {
                            invoice_no = reversalInvoiceNo,
                            entry_date = reversalDate.Date,
                            voucher_type = Convert.ToString(headerRow["VoucherType"]),
                            reference_no = originalInvoiceNo,
                            narration = "Reversal of " + originalInvoiceNo + (string.IsNullOrWhiteSpace(reason) ? string.Empty : " - " + reason.Trim()),
                            attachment = null,
                            total_debit = Convert.ToDecimal(headerRow["TotalCredit"] == DBNull.Value ? 0 : headerRow["TotalCredit"]),
                            total_credit = Convert.ToDecimal(headerRow["TotalDebit"] == DBNull.Value ? 0 : headerRow["TotalDebit"]),
                            status = "Posted",
                            reversal_of = originalHeaderId,
                            posted_by = UsersModal.logged_in_userid,
                            posted_at = DateTime.Now,
                            is_auto_posted = false
                        };

                        int reversalHeaderId;
                        using (SqlCommand headerCmd = new SqlCommand(@"INSERT INTO acc_entries_header
                            (InvoiceNo, EntryDate, VoucherType, ReferenceNo, Narration, Attachment, total_debit, total_credit, status, reversal_of, posted_by, posted_at, is_auto_posted, date_created, date_updated, user_id, branch_id)
                            VALUES
                            (@InvoiceNo, @EntryDate, @VoucherType, @ReferenceNo, @Narration, @Attachment, @total_debit, @total_credit, @status, @reversal_of, @posted_by, @posted_at, @is_auto_posted, @date_created, @date_updated, @user_id, @branch_id);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            headerCmd.Parameters.AddWithValue("@InvoiceNo", reversalHeader.invoice_no);
                            headerCmd.Parameters.AddWithValue("@EntryDate", reversalHeader.entry_date.Date);
                            headerCmd.Parameters.AddWithValue("@VoucherType", (object)reversalHeader.voucher_type ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@ReferenceNo", (object)reversalHeader.reference_no ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@Narration", (object)reversalHeader.narration ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@Attachment", (object)reversalHeader.attachment ?? DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@total_debit", reversalHeader.total_debit);
                            headerCmd.Parameters.AddWithValue("@total_credit", reversalHeader.total_credit);
                            headerCmd.Parameters.AddWithValue("@status", reversalHeader.status);
                            headerCmd.Parameters.AddWithValue("@reversal_of", reversalHeader.reversal_of.HasValue ? (object)reversalHeader.reversal_of.Value : DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@posted_by", reversalHeader.posted_by.HasValue ? (object)reversalHeader.posted_by.Value : DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@posted_at", reversalHeader.posted_at.HasValue ? (object)reversalHeader.posted_at.Value : DBNull.Value);
                            headerCmd.Parameters.AddWithValue("@is_auto_posted", reversalHeader.is_auto_posted ? 1 : 0);
                            headerCmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            headerCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            headerCmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            headerCmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                            reversalHeaderId = Convert.ToInt32(headerCmd.ExecuteScalar());
                        }

                        foreach (DataRow row in originalLines.Rows)
                        {
                            decimal debit = Convert.ToDecimal(row["Debit"]);
                            decimal credit = Convert.ToDecimal(row["Credit"]);
                            using (SqlCommand lineCmd = new SqlCommand("sp_JournalsCrud", cn, tx))
                            {
                                lineCmd.CommandType = CommandType.StoredProcedure;
                                lineCmd.Parameters.AddWithValue("@invoice_no", reversalHeader.invoice_no);
                                lineCmd.Parameters.AddWithValue("@account_id", Convert.ToInt32(row["account_id"]));
                                lineCmd.Parameters.AddWithValue("@entry_date", reversalDate.Date);
                                lineCmd.Parameters.AddWithValue("@debit", credit);
                                lineCmd.Parameters.AddWithValue("@credit", debit);
                                lineCmd.Parameters.AddWithValue("@description", "Reversal of " + originalInvoiceNo);
                                lineCmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                lineCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                lineCmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                lineCmd.Parameters.AddWithValue("@customer_id", 0);
                                lineCmd.Parameters.AddWithValue("@supplier_id", 0);
                                lineCmd.Parameters.AddWithValue("@bank_id", 0);
                                lineCmd.Parameters.AddWithValue("@entry_id", reversalHeaderId);
                                lineCmd.Parameters.AddWithValue("@payment_ref_invoice_no", originalInvoiceNo);
                                lineCmd.Parameters.AddWithValue("@OperationType", "1");
                                lineCmd.ExecuteScalar();
                            }
                        }

                        using (SqlCommand updateCmd = new SqlCommand("UPDATE acc_entries_header SET status='Reversed', date_updated = @date_updated WHERE id = @id AND branch_id = @branch_id", cn, tx))
                        {
                            updateCmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@id", originalHeaderId);
                            updateCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            updateCmd.ExecuteNonQuery();
                        }

                        tx.Commit();
                        return reversalHeaderId;
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        public int UpdateHeaderStatus(List<int> headerIds, string status, int? userId = null, DateTime? postedAt = null)
        {
            if (headerIds == null || headerIds.Count == 0)
            {
                return 0;
            }

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        string inClause = BuildIdInClause(headerIds, "p");
                        string sql = @"UPDATE acc_entries_header
SET status = @status,
    posted_by = CASE WHEN @status = 'Posted' THEN @posted_by ELSE posted_by END,
    posted_at = CASE WHEN @status = 'Posted' THEN @posted_at ELSE posted_at END,
    date_updated = @date_updated
WHERE id IN (" + inClause + @")
  AND branch_id = @branch_id;";
                        using (SqlCommand cmd = new SqlCommand(sql, cn, tx))
                        {
                            cmd.Parameters.AddWithValue("@status", status);
                            cmd.Parameters.AddWithValue("@posted_by", (object)(userId ?? (int?)null) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@posted_at", (object)(postedAt ?? (DateTime?)null) ?? DBNull.Value);
                            cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            AddIdParameters(cmd, headerIds, "p");
                            int result = cmd.ExecuteNonQuery();
                            tx.Commit();
                            return result;
                        }
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        private static string BuildIdInClause(List<int> ids, string prefix)
        {
            List<string> tokens = new List<string>();
            for (int i = 0; i < ids.Count; i++)
            {
                tokens.Add("@" + prefix + i);
            }
            return string.Join(",", tokens);
        }

        private static void AddIdParameters(SqlCommand cmd, List<int> ids, string prefix)
        {
            for (int i = 0; i < ids.Count; i++)
            {
                cmd.Parameters.AddWithValue("@" + prefix + i, ids[i]);
            }
        }

        public string GenerateDailyInvoiceNo(string tableName, string invoiceColumn, string prefix, int? branchId = null, DateTime? invoiceDate = null)
        {
            // Generates Invoice like J1-20230708-0001

            int bId = branchId ?? UsersModal.logged_in_branch_id;
            DateTime d = (invoiceDate ?? DateTime.Now).Date;

            string datePart = d.ToString("yyyyMMdd");
            string start = prefix + bId + "-" + datePart + "-";
            string like = start + "%";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand($@"
            SELECT MAX({invoiceColumn})
            FROM {tableName}
            WHERE branch_id = @branch_id
              AND {invoiceColumn} LIKE @like;", cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", bId);
                cmd.Parameters.AddWithValue("@like", like);

                cn.Open();
                string lastRef = Convert.ToString(cmd.ExecuteScalar());

                int newNum = 1;
                if (!string.IsNullOrWhiteSpace(lastRef) && lastRef.StartsWith(start, StringComparison.OrdinalIgnoreCase))
                {
                    string tail = lastRef.Substring(start.Length);
                    int lastNum;
                    if (int.TryParse(tail, out lastNum))
                        newNum = lastNum + 1;
                }

                return start + newNum.ToString("0000");
            }
        }

        public int Insert(JournalsModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@account_id", obj.account_id);
                        cmd.Parameters.AddWithValue("@entry_date", obj.entry_date);
                        cmd.Parameters.AddWithValue("@debit", obj.debit);
                        cmd.Parameters.AddWithValue("@credit", obj.credit);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@customer_id", obj.customer_id);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@bank_id", obj.bank_id);
                        cmd.Parameters.AddWithValue("@entry_id", obj.entry_id);
                        cmd.Parameters.AddWithValue("@payment_ref_invoice_no", obj.payment_ref_invoice_no);
                        cmd.Parameters.AddWithValue("@OperationType", "1");
                        
                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    return (int)result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Update(JournalsModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@account_name", obj.account_name);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@account_id", obj.account_id);
                        cmd.Parameters.AddWithValue("@entry_date", obj.entry_date);
                        cmd.Parameters.AddWithValue("@debit", obj.debit);
                        cmd.Parameters.AddWithValue("@credit", obj.credit);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "2");
                        
                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    return (int)result;
                }
                catch
                {

                    throw;
                }
                
            }
        }

        public int Delete(int JournalsId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", JournalsId); 
                        cmd.Parameters.AddWithValue("@OperationType", "3");

                    }

                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }


    }
}
