using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using POS.Core;

namespace POS.DLL
{
    public class JournalsDLL : AccountingDalBase
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private JournalsModal info = new JournalsModal();
        private const int BulkCopyThreshold = 100;

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
            // Use accounting settings voucher format for JV (configured in frm_accounting_settings)
            // Keys:
            //   ACC_VOUCHER_JV_PREFIX (default JV)
            //   ACC_VOUCHER_JV_FORMAT (default YYYY-NNNN)
            //   ACC_VOUCHER_JV_START  (default 1)

            string configuredPrefix = string.Empty;
            string configuredFormat = "YYYY-NNNN";
            int startNo = 1;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT setting_key, setting_value
                    FROM pos_settings
                    WHERE setting_key IN ('ACC_VOUCHER_JV_PREFIX', 'ACC_VOUCHER_JV_FORMAT', 'ACC_VOUCHER_JV_START');", cn))
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        string key = Convert.ToString(rdr[0]);
                        string val = Convert.ToString(rdr[1]);

                        if (string.Equals(key, "ACC_VOUCHER_JV_PREFIX", StringComparison.OrdinalIgnoreCase))
                            configuredPrefix = val;
                        else if (string.Equals(key, "ACC_VOUCHER_JV_FORMAT", StringComparison.OrdinalIgnoreCase))
                            configuredFormat = string.IsNullOrWhiteSpace(val) ? "YYYY-NNNN" : val;
                        else if (string.Equals(key, "ACC_VOUCHER_JV_START", StringComparison.OrdinalIgnoreCase))
                        {
                            int parsed;
                            if (int.TryParse(val, out parsed) && parsed > 0)
                                startNo = parsed;
                        }
                    }
                }

                string effectivePrefix = string.IsNullOrWhiteSpace(configuredPrefix) ? "JV" : configuredPrefix.Trim();
                string format = string.IsNullOrWhiteSpace(configuredFormat) ? "YYYY-NNNN" : configuredFormat.Trim();

                DateTime today = DateTime.Today;
                string yyyy = today.ToString("yyyy", CultureInfo.InvariantCulture);
                string yy = today.ToString("yy", CultureInfo.InvariantCulture);
                string mm = today.ToString("MM", CultureInfo.InvariantCulture);
                string dd = today.ToString("dd", CultureInfo.InvariantCulture);

                string formatForLike = format.ToUpperInvariant()
                    .Replace("YYYY", yyyy)
                    .Replace("YY", yy)
                    .Replace("MM", mm)
                    .Replace("DD", dd);

                // Replace all N groups with '%' to narrow candidates
                string formatLikePart = Regex.Replace(formatForLike, "N+", "%");

                string invoicePrefix = string.Format("{0}{1}-", effectivePrefix, UsersModal.logged_in_branch_id);
                string likePattern = invoicePrefix + formatLikePart;

                int maxSequence = 0;
                using (SqlCommand cmd = new SqlCommand(@"
                    SELECT invoice_no
                    FROM acc_entries
                    WHERE branch_id = @branch_id
                      AND invoice_no LIKE @likePattern;", cn))
                {
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@likePattern", likePattern);

                    using (SqlDataReader rdr = cmd.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            string inv = Convert.ToString(rdr[0]);
                            if (string.IsNullOrWhiteSpace(inv))
                                continue;

                            int lastDash = inv.LastIndexOf('-');
                            if (lastDash < 0 || lastDash == inv.Length - 1)
                                continue;

                            int seq;
                            if (int.TryParse(inv.Substring(lastDash + 1), out seq) && seq > maxSequence)
                                maxSequence = seq;
                        }
                    }
                }

                int next = maxSequence > 0 ? (maxSequence + 1) : startNo;

                string finalFormat = formatForLike;
                int nIndex = finalFormat.IndexOf('N');
                if (nIndex >= 0)
                {
                    int nLen = 0;
                    while (nIndex + nLen < finalFormat.Length && finalFormat[nIndex + nLen] == 'N')
                        nLen++;

                    string padded = next.ToString().PadLeft(nLen, '0');
                    finalFormat = finalFormat.Substring(0, nIndex) + padded + finalFormat.Substring(nIndex + nLen);
                }
                else
                {
                    finalFormat = finalFormat + "-" + next.ToString("D4", CultureInfo.InvariantCulture);
                }

                return invoicePrefix + finalFormat;
            }
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
                        OR H.ReferenceNo LIKE @search_like
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
       ISNULL(E.credit,0) AS Credit,
       ISNULL(E.cost_center_id, 0) AS cost_center_id
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

                        string reversalInvoiceNo = GetMaxInvoiceNo();
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
INSERT INTO acc_entries_header
    (InvoiceNo, EntryDate, VoucherType, ReferenceNo, Narration, Attachment, total_debit, total_credit, status, reversal_of, posted_by, posted_at, is_auto_posted, date_created, date_updated, user_id, branch_id)
VALUES
    (@InvoiceNo, @EntryDate, @VoucherType, @ReferenceNo, @Narration, @Attachment, @total_debit, @total_credit, @status, @reversal_of, @posted_by, @posted_at, @is_auto_posted, @date_created, @date_updated, @user_id, @branch_id);
SELECT CAST(SCOPE_IDENTITY() AS INT);", transaction.Connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@InvoiceNo", header.VoucherNo);
                    cmd.Parameters.AddWithValue("@EntryDate", header.VoucherDate.Date);
                    cmd.Parameters.AddWithValue("@VoucherType", string.IsNullOrWhiteSpace(header.VoucherType) ? "Journal" : header.VoucherType.Trim());
                    cmd.Parameters.AddWithValue("@ReferenceNo", DbValue(header.ReferenceNo));
                    cmd.Parameters.AddWithValue("@Narration", DbValue(header.Narration));
                    cmd.Parameters.AddWithValue("@Attachment", DbValue(header.Attachment));
                    cmd.Parameters.AddWithValue("@total_debit", header.TotalDebit);
                    cmd.Parameters.AddWithValue("@total_credit", header.TotalCredit);
                    cmd.Parameters.AddWithValue("@status", header.Status);
                    cmd.Parameters.AddWithValue("@reversal_of", header.ReversalOf.HasValue ? (object)header.ReversalOf.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@posted_by", header.PostedBy.HasValue ? (object)header.PostedBy.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@posted_at", header.PostedAt.HasValue ? (object)header.PostedAt.Value : DBNull.Value);
                    cmd.Parameters.AddWithValue("@is_auto_posted", header.IsAutoPosted ? 1 : 0);
                    cmd.Parameters.AddWithValue("@date_created", header.CreatedAt.Value);
                    cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@user_id", header.CreatedBy.HasValue ? (object)header.CreatedBy.Value : UsersModal.logged_in_userid);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

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
                bulk.ColumnMappings.Add("invoice_no", "invoice_no");
                bulk.ColumnMappings.Add("account_id", "account_id");
                bulk.ColumnMappings.Add("entry_date", "entry_date");
                bulk.ColumnMappings.Add("debit", "debit");
                bulk.ColumnMappings.Add("credit", "credit");
                bulk.ColumnMappings.Add("description", "description");
                bulk.ColumnMappings.Add("user_id", "user_id");
                bulk.ColumnMappings.Add("branch_id", "branch_id");
                bulk.ColumnMappings.Add("date_created", "date_created");
                bulk.ColumnMappings.Add("cost_center_id", "cost_center_id");
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
                sql.AppendLine("SELECT id AS voucher_id, InvoiceNo AS voucher_no, EntryDate AS voucher_date, VoucherType AS voucher_type, CAST(NULL AS INT) AS period_id,");
                sql.AppendLine("       Narration AS narration, ReferenceNo AS ref_no, Attachment AS attachment_path,");
                sql.AppendLine("       total_debit, total_credit, ISNULL(status,'Draft') AS status, reversal_of,");
                sql.AppendLine("       user_id AS created_by, date_created AS created_at, posted_by, posted_at, is_auto_posted,");
                sql.AppendLine("       COUNT(1) OVER() AS total_count");
                sql.AppendLine("FROM acc_entries_header");
                sql.AppendLine("WHERE branch_id = @branch_id");
                if (safe.FromDate.HasValue)
                {
                    sql.AppendLine("  AND EntryDate >= @from_date");
                }
                if (safe.ToDate.HasValue)
                {
                    sql.AppendLine("  AND EntryDate <= @to_date");
                }
                if (!string.IsNullOrWhiteSpace(safe.VoucherType))
                {
                    sql.AppendLine("  AND VoucherType = @voucher_type");
                }
                if (!string.IsNullOrWhiteSpace(safe.Status))
                {
                    sql.AppendLine("  AND ISNULL(status,'Draft') = @status");
                }
                if (safe.CreatedBy.HasValue)
                {
                    sql.AppendLine("  AND user_id = @created_by");
                }
                if (safe.PostedBy.HasValue)
                {
                    sql.AppendLine("  AND posted_by = @posted_by");
                }
                if (!string.IsNullOrWhiteSpace(safe.Search))
                {
                    sql.AppendLine("  AND (InvoiceNo LIKE @search OR Narration LIKE @search OR ReferenceNo LIKE @search)");
                }
                sql.AppendLine("ORDER BY EntryDate DESC, id DESC");
                sql.AppendLine("OFFSET @offset ROWS FETCH NEXT @fetch ROWS ONLY;");

                return ExecuteDataTable(sql.ToString(), cmd =>
                {
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

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
UPDATE acc_entries_header
SET status = @status,
    posted_by = CASE WHEN @status = 'Posted' THEN @user_id ELSE posted_by END,
    posted_at = CASE WHEN @status = 'Posted' THEN @posted_at ELSE posted_at END,
    date_updated = @date_updated
WHERE id = @voucher_id
  AND branch_id = @branch_id;", transaction.Connection, transaction))
                {
                    cmd.Parameters.AddWithValue("@voucher_id", voucherId);
                    cmd.Parameters.AddWithValue("@status", string.IsNullOrWhiteSpace(status) ? "Draft" : status.Trim());
                    cmd.Parameters.AddWithValue("@user_id", userId);
                    cmd.Parameters.AddWithValue("@posted_at", DateTime.Now);
                    cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
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
            dt.Columns.Add("invoice_no", typeof(string));
            dt.Columns.Add("account_id", typeof(int));
            dt.Columns.Add("entry_date", typeof(DateTime));
            dt.Columns.Add("debit", typeof(decimal));
            dt.Columns.Add("credit", typeof(decimal));
            dt.Columns.Add("description", typeof(string));
            dt.Columns.Add("user_id", typeof(int));
            dt.Columns.Add("branch_id", typeof(int));
            dt.Columns.Add("date_created", typeof(DateTime));
            dt.Columns.Add("cost_center_id", typeof(int));

            int effectiveUserId = createdBy > 0 ? createdBy : UsersModal.logged_in_userid;

            for (int i = 0; i < lines.Count; i++)
            {
                JVLineModel line = lines[i];
                dt.Rows.Add(
                    voucherNo ?? string.Empty,
                    line.AccountId,
                    entryDate.Date,
                    line.Debit,
                    line.Credit,
                    (object)NormalizeText(line.Narration) ?? DBNull.Value,
                    effectiveUserId,
                    UsersModal.logged_in_branch_id,
                    DateTime.Now,
                    line.CostCenterID > 0 ? (object)line.CostCenterID : DBNull.Value);
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

        public List<ValidationError> ValidateJournalLines(List<JVLineModel> lines)
        {
            List<ValidationError> errors = new List<ValidationError>();

            if (lines == null || lines.Count == 0)
            {
                errors.Add(new ValidationError
                {
                    FieldName = "Lines",
                    Message = "Voucher must contain at least one journal line.",
                    Severity = "Error",
                    IsBlocking = true
                });
                return errors;
            }

            decimal totalDebit = 0m;
            decimal totalCredit = 0m;
            HashSet<int> seenAccounts = new HashSet<int>();
            HashSet<int> duplicateAccounts = new HashSet<int>();

            for (int i = 0; i < lines.Count; i++)
            {
                JVLineModel line = lines[i];
                int lineNo = line.LineNo > 0 ? line.LineNo : i + 1;
                decimal debit = line.Debit;
                decimal credit = line.Credit;

                if (debit < 0m || credit < 0m)
                {
                    errors.Add(new ValidationError
                    {
                        LineNo = lineNo,
                        FieldName = "Amount",
                        Message = "Debit and credit amounts must be positive.",
                        Severity = "Error",
                        IsBlocking = true
                    });
                }

                if (debit > 0m && credit > 0m)
                {
                    errors.Add(new ValidationError
                    {
                        LineNo = lineNo,
                        FieldName = "Debit/Credit",
                        Message = "A journal line cannot have both debit and credit amounts.",
                        Severity = "Error",
                        IsBlocking = true
                    });
                }

                if (debit == 0m && credit == 0m)
                {
                    errors.Add(new ValidationError
                    {
                        LineNo = lineNo,
                        FieldName = "Amount",
                        Message = "A journal line must contain either a debit or a credit amount.",
                        Severity = "Error",
                        IsBlocking = true
                    });
                }

                totalDebit += debit;
                totalCredit += credit;

                if (line.AccountId > 0 && !seenAccounts.Add(line.AccountId))
                {
                    duplicateAccounts.Add(line.AccountId);
                }
            }

            if (Math.Abs(totalDebit - totalCredit) >= 0.005m)
            {
                errors.Add(new ValidationError
                {
                    FieldName = "Balance",
                    Message = string.Format(CultureInfo.InvariantCulture, "Journal is not balanced. Debit={0:N2}, Credit={1:N2}.", totalDebit, totalCredit),
                    Severity = "Error",
                    IsBlocking = true
                });
            }

            foreach (int accountId in duplicateAccounts)
            {
                errors.Add(new ValidationError
                {
                    FieldName = "AccountId",
                    Message = string.Format(CultureInfo.InvariantCulture, "Account {0} appears more than once in the voucher.", accountId),
                    Severity = "Warning",
                    IsBlocking = false
                });
            }

            return errors;
        }

        public PostResult PostJournalVoucher(JVHeaderModel header, List<JVLineModel> lines, int userId)
        {
            PostResult result = new PostResult();
            try
            {
                if (header == null)
                {
                    result.Messages.Add(new ValidationError
                    {
                        FieldName = "Header",
                        Message = "Voucher header is required.",
                        Severity = "Error",
                        IsBlocking = true
                    });
                    return result;
                }

                if (lines == null)
                {
                    lines = new List<JVLineModel>();
                }

                List<ValidationError> validationErrors = ValidateJournalLines(lines);
                result.Messages.AddRange(validationErrors);
                if (validationErrors.Any(x => x.IsBlocking))
                {
                    return result;
                }

                if (IsVoucherDateLocked(header.VoucherDate))
                {
                    result.Messages.Add(new ValidationError
                    {
                        FieldName = "VoucherDate",
                        Message = "Voucher date falls outside the allowed posting period.",
                        Severity = "Error",
                        IsBlocking = true
                    });
                    return result;
                }

                if (!AreAccountsActive(lines, out List<ValidationError> accountErrors))
                {
                    result.Messages.AddRange(accountErrors);
                    return result;
                }

                if (string.IsNullOrWhiteSpace(header.VoucherNo))
                {
                    header.VoucherNo = GetMaxInvoiceNo();
                }

                if (VoucherNoExists(header.VoucherNo, header.VoucherId > 0 ? header.VoucherId : (int?)null))
                {
                    result.Messages.Add(new ValidationError
                    {
                        FieldName = "VoucherNo",
                        Message = string.Format(CultureInfo.InvariantCulture, "Voucher number {0} already exists.", header.VoucherNo),
                        Severity = "Error",
                        IsBlocking = true
                    });
                    return result;
                }

                header.TotalDebit = lines.Sum(x => x.Debit);
                header.TotalCredit = lines.Sum(x => x.Credit);
                header.Status = string.IsNullOrWhiteSpace(header.Status) ? "Posted" : header.Status;
                header.PostedBy = userId;
                header.PostedAt = DateTime.Now;
                header.IsAutoPosted = false;
                header.CreatedBy = userId;
                header.CreatedAt = DateTime.Now;
                header.UpdatedBy = userId;
                header.UpdatedAt = DateTime.Now;

                string voucherXml = BuildVoucherXml(header, lines, userId);

                using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("sp_PostJournalVoucher", cn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@VoucherXml", SqlDbType.Xml).Value = voucherXml;

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                result.VoucherId = reader["VoucherId"] == DBNull.Value ? 0 : Convert.ToInt32(reader["VoucherId"]);
                                result.VoucherNo = Convert.ToString(reader["VoucherNo"]);
                                result.Success = result.VoucherId > 0;
                            }

                            if (reader.NextResult())
                            {
                                while (reader.Read())
                                {
                                    if (reader["EntryId"] != DBNull.Value)
                                    {
                                        result.EntryIds.Add(Convert.ToInt32(reader["EntryId"]));
                                    }
                                }
                            }
                        }
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                result.Messages.Add(new ValidationError
                {
                    FieldName = "PostJournalVoucher",
                    Message = ex.Message,
                    Severity = "Error",
                    IsBlocking = true
                });
                return result;
            }
        }

        public PostResult ReverseJournalVoucher(int voucherId, DateTime reversalDate, string reason, int userId)
        {
            PostResult result = new PostResult();
            try
            {
                var source = GetVoucherWithLines(voucherId);
                if (source.Header == null || string.IsNullOrWhiteSpace(source.Header.VoucherNo))
                {
                    result.Messages.Add(new ValidationError
                    {
                        FieldName = "VoucherId",
                        Message = "Original voucher was not found.",
                        Severity = "Error",
                        IsBlocking = true
                    });
                    return result;
                }

                if (!string.Equals(source.Header.Status, "Posted", StringComparison.OrdinalIgnoreCase))
                {
                    result.Messages.Add(new ValidationError
                    {
                        FieldName = "Status",
                        Message = "Only posted vouchers can be reversed.",
                        Severity = "Error",
                        IsBlocking = true
                    });
                    return result;
                }

                List<JVLineModel> reversalLines = new List<JVLineModel>();
                foreach (JVLineModel line in source.Lines)
                {
                    reversalLines.Add(new JVLineModel
                    {
                        LineNo = line.LineNo,
                        AccountId = line.AccountId,
                        AccountCode = line.AccountCode,
                        AccountName = line.AccountName,
                        Narration = string.IsNullOrWhiteSpace(reason) ? "Reversal of " + source.Header.VoucherNo : reason,
                        Debit = line.Credit,
                        Credit = line.Debit,
                        CostCenterID = line.CostCenterID,
                        ModuleName = "REVERSAL",
                        RefId = voucherId
                    });
                }

                JVHeaderModel reversalHeader = new JVHeaderModel
                {
                    VoucherNo = GetMaxInvoiceNo(),
                    VoucherDate = reversalDate.Date,
                    VoucherType = source.Header.VoucherType,
                    ReferenceNo = source.Header.VoucherNo,
                    Narration = string.IsNullOrWhiteSpace(reason)
                        ? string.Format(CultureInfo.InvariantCulture, "Reversal of {0}", source.Header.VoucherNo)
                        : reason.Trim(),
                    Attachment = null,
                    TotalDebit = source.Header.TotalCredit,
                    TotalCredit = source.Header.TotalDebit,
                    Status = "Posted",
                    ReversalOf = voucherId,
                    PostedBy = userId,
                    PostedAt = DateTime.Now,
                    IsAutoPosted = false,
                    RefModule = "REVERSAL",
                    RefId = voucherId,
                    CreatedBy = userId,
                    CreatedAt = DateTime.Now,
                    UpdatedBy = userId,
                    UpdatedAt = DateTime.Now
                };

                return PostJournalVoucher(reversalHeader, reversalLines, userId);
            }
            catch (Exception ex)
            {
                result.Messages.Add(new ValidationError
                {
                    FieldName = "ReverseJournalVoucher",
                    Message = ex.Message,
                    Severity = "Error",
                    IsBlocking = true
                });
                return result;
            }
        }

        public PostResult PostAutoJournalEntry(AutoJVModel model, int userId)
        {
            PostResult result = new PostResult();
            if (model == null)
            {
                result.Messages.Add(new ValidationError
                {
                    FieldName = "Model",
                    Message = "Auto journal model is required.",
                    Severity = "Error",
                    IsBlocking = true
                });
                return result;
            }

            List<ValidationError> validation = ValidateJournalLines(model.Lines);
            result.Messages.AddRange(validation);
            if (validation.Any(x => x.IsBlocking))
            {
                return result;
            }

            if (IsVoucherDateLocked(model.VoucherDate))
            {
                result.Messages.Add(new ValidationError
                {
                    FieldName = "VoucherDate",
                    Message = "Journal date falls outside the allowed posting period.",
                    Severity = "Error",
                    IsBlocking = true
                });
                return result;
            }

            if (!AreAccountsActive(model.Lines, out List<ValidationError> accountErrors))
            {
                result.Messages.AddRange(accountErrors);
                return result;
            }

            string voucherNo = GetMaxInvoiceNo();
            DataTable stagedLines = BuildAutoLinesDataTable(model, voucherNo, userId);
            List<int> insertedEntryIds = new List<int>();

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        if (stagedLines.Rows.Count >= BulkCopyThreshold)
                        {
                            using (SqlCommand createStage = new SqlCommand(@"
CREATE TABLE #AutoJVLines (
    line_no INT NOT NULL,
    account_id INT NOT NULL,
    debit DECIMAL(18,2) NOT NULL DEFAULT 0,
    credit DECIMAL(18,2) NOT NULL DEFAULT 0,
    narration NVARCHAR(MAX) NULL,
    entry_id INT NULL,
    payment_ref_invoice_no NVARCHAR(100) NULL,
    customer_id INT NULL,
    supplier_id INT NULL,
    bank_id INT NULL
);", cn, tx))
                            {
                                createStage.ExecuteNonQuery();
                            }

                            using (SqlBulkCopy bulk = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, tx))
                            {
                                bulk.DestinationTableName = "#AutoJVLines";
                                bulk.BatchSize = 1000;
                                bulk.BulkCopyTimeout = 120;
                                bulk.ColumnMappings.Add("line_no", "line_no");
                                bulk.ColumnMappings.Add("account_id", "account_id");
                                bulk.ColumnMappings.Add("debit", "debit");
                                bulk.ColumnMappings.Add("credit", "credit");
                                bulk.ColumnMappings.Add("narration", "narration");
                                bulk.ColumnMappings.Add("entry_id", "entry_id");
                                bulk.ColumnMappings.Add("payment_ref_invoice_no", "payment_ref_invoice_no");
                                bulk.ColumnMappings.Add("customer_id", "customer_id");
                                bulk.ColumnMappings.Add("supplier_id", "supplier_id");
                                bulk.ColumnMappings.Add("bank_id", "bank_id");
                                bulk.WriteToServer(stagedLines);
                            }

                            using (SqlCommand insertCmd = new SqlCommand(@"
DECLARE @Inserted TABLE (EntryId INT);
INSERT INTO acc_entries
    (invoice_no, account_id, entry_date, debit, credit, description, user_id, branch_id, date_created, customer_id, supplier_id, bank_id, entry_id, payment_ref_invoice_no)
OUTPUT INSERTED.id INTO @Inserted(EntryId)
SELECT @invoice_no, account_id, @entry_date, debit, credit, narration, @user_id, @branch_id, GETDATE(), customer_id, supplier_id, bank_id, entry_id, payment_ref_invoice_no
FROM #AutoJVLines
ORDER BY line_no;
SELECT EntryId FROM @Inserted ORDER BY EntryId;", cn, tx))
                            {
                                insertCmd.Parameters.AddWithValue("@invoice_no", voucherNo);
                                insertCmd.Parameters.AddWithValue("@entry_date", model.VoucherDate.Date);
                                insertCmd.Parameters.AddWithValue("@user_id", userId);
                                insertCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                using (SqlDataReader reader = insertCmd.ExecuteReader())
                                {
                                    while (reader.Read())
                                    {
                                        if (reader["EntryId"] != DBNull.Value)
                                        {
                                            insertedEntryIds.Add(Convert.ToInt32(reader["EntryId"]));
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            foreach (JVLineModel line in model.Lines)
                            {
                                using (SqlCommand insertCmd = new SqlCommand(@"
INSERT INTO acc_entries
    (invoice_no, account_id, entry_date, debit, credit, description, user_id, branch_id, date_created, customer_id, supplier_id, bank_id, entry_id, payment_ref_invoice_no)
OUTPUT INSERTED.id
VALUES
    (@invoice_no, @account_id, @entry_date, @debit, @credit, @description, @user_id, @branch_id, GETDATE(), @customer_id, @supplier_id, @bank_id, @entry_id, @payment_ref_invoice_no);", cn, tx))
                                {
                                    insertCmd.Parameters.AddWithValue("@invoice_no", voucherNo);
                                    insertCmd.Parameters.AddWithValue("@account_id", line.AccountId);
                                    insertCmd.Parameters.AddWithValue("@entry_date", model.VoucherDate.Date);
                                    insertCmd.Parameters.AddWithValue("@debit", line.Debit);
                                    insertCmd.Parameters.AddWithValue("@credit", line.Credit);
                                    insertCmd.Parameters.AddWithValue("@description", string.IsNullOrWhiteSpace(line.Narration) ? (object)model.Narration ?? DBNull.Value : line.Narration);
                                    insertCmd.Parameters.AddWithValue("@user_id", userId);
                                    insertCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    insertCmd.Parameters.AddWithValue("@customer_id", 0);
                                    insertCmd.Parameters.AddWithValue("@supplier_id", 0);
                                    insertCmd.Parameters.AddWithValue("@bank_id", 0);
                                    insertCmd.Parameters.AddWithValue("@entry_id", model.RefId);
                                    insertCmd.Parameters.AddWithValue("@payment_ref_invoice_no", string.IsNullOrWhiteSpace(model.ReferenceNo) ? (object)DBNull.Value : model.ReferenceNo);
                                    object inserted = insertCmd.ExecuteScalar();
                                    if (inserted != null && inserted != DBNull.Value)
                                    {
                                        insertedEntryIds.Add(Convert.ToInt32(inserted));
                                    }
                                }
                            }
                        }

                        result.Success = insertedEntryIds.Count > 0;
                        result.VoucherNo = voucherNo;
                        result.EntryIds = insertedEntryIds;
                        return result;
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        result.Messages.Add(new ValidationError
                        {
                            FieldName = "PostAutoJournalEntry",
                            Message = ex.Message,
                            Severity = "Error",
                            IsBlocking = true
                        });
                        return result;
                    }
                }
            }
        }

        public (JVHeaderModel Header, List<JVLineModel> Lines) GetVoucherWithLines(int voucherId)
        {
            DataTable headerTable = GetVoucherHeaderById(voucherId);
            if (headerTable.Rows.Count == 0)
            {
                return (null, new List<JVLineModel>());
            }

            JVHeaderModel header = MapHeader(headerTable.Rows[0]);
            DataTable linesTable = GetVoucherLines(header.VoucherNo);
            List<JVLineModel> lines = MapLines(linesTable, header.VoucherId);
            return (header, lines);
        }

        public BatchPostResult BatchPostVouchers(List<int> voucherIds, int userId)
        {
            BatchPostResult result = new BatchPostResult();
            if (voucherIds == null || voucherIds.Count == 0)
            {
                return result;
            }

            foreach (int voucherId in voucherIds.Distinct())
            {
                try
                {
                    var voucher = GetVoucherWithLines(voucherId);
                    if (voucher.Header == null)
                    {
                        result.FailureCount++;
                        result.FailedVouchers.Add(new ValidationError
                        {
                            VoucherNo = voucherId.ToString(CultureInfo.InvariantCulture),
                            Message = "Voucher not found.",
                            Severity = "Error",
                            IsBlocking = true
                        });
                        continue;
                    }

                    if (!string.Equals(voucher.Header.Status, "Draft", StringComparison.OrdinalIgnoreCase))
                    {
                        result.FailureCount++;
                        result.FailedVouchers.Add(new ValidationError
                        {
                            VoucherNo = voucher.Header.VoucherNo,
                            Message = "Only draft vouchers can be batch posted.",
                            Severity = "Error",
                            IsBlocking = true
                        });
                        continue;
                    }

                    List<ValidationError> validation = ValidateJournalLines(voucher.Lines);
                    if (validation.Any(x => x.IsBlocking))
                    {
                        result.FailureCount++;
                        result.FailedVouchers.Add(new ValidationError
                        {
                            VoucherNo = voucher.Header.VoucherNo,
                            Message = string.Join("; ", validation.Where(x => x.IsBlocking).Select(x => x.Message)),
                            Severity = "Error",
                            IsBlocking = true
                        });
                        continue;
                    }

                    if (UpdateHeaderStatus(new List<int> { voucherId }, "Posted", userId, DateTime.Now) <= 0)
                    {
                        result.FailureCount++;
                        result.FailedVouchers.Add(new ValidationError
                        {
                            VoucherNo = voucher.Header.VoucherNo,
                            Message = "Could not update voucher status.",
                            Severity = "Error",
                            IsBlocking = true
                        });
                        continue;
                    }

                    result.SuccessCount++;
                    result.PostedVoucherIds.Add(voucherId);
                }
                catch (Exception ex)
                {
                    result.FailureCount++;
                    result.FailedVouchers.Add(new ValidationError
                    {
                        VoucherNo = voucherId.ToString(CultureInfo.InvariantCulture),
                        Message = ex.Message,
                        Severity = "Error",
                        IsBlocking = true
                    });
                }
            }

            return result;
        }

        private string BuildVoucherXml(JVHeaderModel header, IEnumerable<JVLineModel> lines, int userId)
        {
            XDocument document = new XDocument(
                new XElement("Voucher",
                    new XElement("Header",
                        new XElement("VoucherId", header.VoucherId),
                        new XElement("VoucherNo", header.VoucherNo ?? string.Empty),
                        new XElement("VoucherDate", header.VoucherDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)),
                        new XElement("VoucherType", header.VoucherType ?? string.Empty),
                        new XElement("ReferenceNo", header.ReferenceNo ?? string.Empty),
                        new XElement("Narration", header.Narration ?? string.Empty),
                        new XElement("Attachment", header.Attachment ?? string.Empty),
                        new XElement("TotalDebit", header.TotalDebit.ToString(CultureInfo.InvariantCulture)),
                        new XElement("TotalCredit", header.TotalCredit.ToString(CultureInfo.InvariantCulture)),
                        new XElement("Status", header.Status ?? "Posted"),
                        new XElement("ReversalOf", header.ReversalOf.HasValue ? header.ReversalOf.Value.ToString(CultureInfo.InvariantCulture) : string.Empty),
                        new XElement("PostedBy", header.PostedBy.HasValue ? header.PostedBy.Value.ToString(CultureInfo.InvariantCulture) : userId.ToString(CultureInfo.InvariantCulture)),
                        new XElement("PostedAt", header.PostedAt.HasValue ? header.PostedAt.Value.ToString("o", CultureInfo.InvariantCulture) : DateTime.Now.ToString("o", CultureInfo.InvariantCulture)),
                        new XElement("IsAutoPosted", header.IsAutoPosted ? "1" : "0"),
                        new XElement("RefModule", header.RefModule ?? string.Empty),
                        new XElement("RefId", header.RefId.HasValue ? header.RefId.Value.ToString(CultureInfo.InvariantCulture) : string.Empty),
                        new XElement("BranchId", header.BranchId.HasValue ? header.BranchId.Value.ToString(CultureInfo.InvariantCulture) : UsersModal.logged_in_branch_id.ToString(CultureInfo.InvariantCulture)),
                        new XElement("CompanyId", header.CompanyId.HasValue ? header.CompanyId.Value.ToString(CultureInfo.InvariantCulture) : UsersModal.loggedIncompanyID.ToString(CultureInfo.InvariantCulture)),
                        new XElement("CreatedBy", header.CreatedBy.HasValue ? header.CreatedBy.Value.ToString(CultureInfo.InvariantCulture) : userId.ToString(CultureInfo.InvariantCulture)),
                        new XElement("CreatedAt", header.CreatedAt.HasValue ? header.CreatedAt.Value.ToString("o", CultureInfo.InvariantCulture) : DateTime.Now.ToString("o", CultureInfo.InvariantCulture))
                    ),
                    new XElement("Lines",
                        lines.Select((line, index) =>
                            new XElement("Line",
                                new XElement("LineNo", line.LineNo > 0 ? line.LineNo : index + 1),
                                new XElement("AccountId", line.AccountId),
                                new XElement("Debit", line.Debit.ToString(CultureInfo.InvariantCulture)),
                                new XElement("Credit", line.Credit.ToString(CultureInfo.InvariantCulture)),
                                new XElement("Narration", line.Narration ?? string.Empty),
                                new XElement("CostCenterID", line.CostCenterID > 0 ? line.CostCenterID.ToString(CultureInfo.InvariantCulture) : string.Empty),
                                new XElement("ModuleName", line.ModuleName ?? string.Empty),
                                new XElement("RefId", line.RefId.HasValue ? line.RefId.Value.ToString(CultureInfo.InvariantCulture) : string.Empty)
                            )
                        )
                    )
                )
            );

            return document.ToString(SaveOptions.DisableFormatting);
        }

        private DataTable BuildAutoLinesDataTable(AutoJVModel model, string voucherNo, int userId)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("line_no", typeof(int));
            dt.Columns.Add("account_id", typeof(int));
            dt.Columns.Add("debit", typeof(decimal));
            dt.Columns.Add("credit", typeof(decimal));
            dt.Columns.Add("narration", typeof(string));
            dt.Columns.Add("entry_id", typeof(int));
            dt.Columns.Add("payment_ref_invoice_no", typeof(string));
            dt.Columns.Add("customer_id", typeof(int));
            dt.Columns.Add("supplier_id", typeof(int));
            dt.Columns.Add("bank_id", typeof(int));
            dt.Columns.Add("cost_center_id", typeof(int));

            for (int i = 0; i < model.Lines.Count; i++)
            {
                JVLineModel line = model.Lines[i];
                dt.Rows.Add(
                    line.LineNo > 0 ? line.LineNo : i + 1,
                    line.AccountId,
                    line.Debit,
                    line.Credit,
                    string.IsNullOrWhiteSpace(line.Narration) ? model.Narration : line.Narration,
                    model.RefId,
                    string.IsNullOrWhiteSpace(model.ReferenceNo) ? DBNull.Value : (object)model.ReferenceNo,
                    DBNull.Value,
                    DBNull.Value,
                    DBNull.Value);
            }

            return dt;
        }

        private bool IsVoucherDateLocked(DateTime voucherDate)
        {
            if (UsersModal.fy_from_date == default(DateTime) && UsersModal.fy_to_date == default(DateTime))
            {
                return false;
            }

            if (UsersModal.fy_from_date != default(DateTime) && voucherDate.Date < UsersModal.fy_from_date.Date)
            {
                return true;
            }

            if (UsersModal.fy_to_date != default(DateTime) && voucherDate.Date > UsersModal.fy_to_date.Date)
            {
                return true;
            }

            return false;
        }

        private bool VoucherNoExists(string voucherNo, int? excludeVoucherId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
SELECT COUNT(1)
FROM acc_entries_header
WHERE InvoiceNo = @voucher_no
  AND branch_id = @branch_id
  AND (@exclude_id IS NULL OR id <> @exclude_id);", cn))
                {
                    cmd.Parameters.AddWithValue("@voucher_no", voucherNo);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@exclude_id", (object)excludeVoucherId ?? DBNull.Value);
                    return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
                }
            }
        }

        private bool AreAccountsActive(IEnumerable<JVLineModel> lines, out List<ValidationError> errors)
        {
            errors = new List<ValidationError>();
            List<int> accountIds = lines.Select(x => x.AccountId).Where(x => x > 0).Distinct().ToList();
            if (accountIds.Count == 0)
            {
                errors.Add(new ValidationError
                {
                    FieldName = "AccountId",
                    Message = "At least one valid account is required.",
                    Severity = "Error",
                    IsBlocking = true
                });
                return false;
            }

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlCommand cmd = new SqlCommand(@"
SELECT id, code, name
FROM acc_accounts
WHERE branch_id = @branch_id
  AND id IN (" + string.Join(",", accountIds.Select((x, i) => "@a" + i)) + @")
  AND (is_active = 1 OR is_active IS NULL);", cn))
                {
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    for (int i = 0; i < accountIds.Count; i++)
                    {
                        cmd.Parameters.AddWithValue("@a" + i, accountIds[i]);
                    }

                    HashSet<int> validAccounts = new HashSet<int>();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            validAccounts.Add(Convert.ToInt32(reader["id"]));
                        }
                    }

                    foreach (int accountId in accountIds)
                    {
                        if (!validAccounts.Contains(accountId))
                        {
                            errors.Add(new ValidationError
                            {
                                FieldName = "AccountId",
                                Message = string.Format(CultureInfo.InvariantCulture, "Account {0} is missing or inactive.", accountId),
                                Severity = "Error",
                                IsBlocking = true
                            });
                        }
                    }
                }
            }

            return errors.Count == 0;
        }

        private JVVoucherModel MapVoucher(DataRow headerRow, DataTable linesTable)
        {
            JVVoucherModel voucher = new JVVoucherModel();
            voucher.Header = MapHeader(headerRow);
            voucher.Lines = MapLines(linesTable, voucher.Header.VoucherId);
            return voucher;
        }

        private JVHeaderModel MapHeader(DataRow row)
        {
            JVHeaderModel header = new JVHeaderModel();
            header.VoucherId = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
            header.VoucherNo = row.Table.Columns.Contains("InvoiceNo") ? Convert.ToString(row["InvoiceNo"]) : string.Empty;
            header.VoucherDate = row.Table.Columns.Contains("EntryDate") && row["EntryDate"] != DBNull.Value ? Convert.ToDateTime(row["EntryDate"]) : DateTime.Today;
            header.VoucherType = row.Table.Columns.Contains("VoucherType") ? Convert.ToString(row["VoucherType"]) : string.Empty;
            header.ReferenceNo = row.Table.Columns.Contains("ReferenceNo") ? Convert.ToString(row["ReferenceNo"]) : string.Empty;
            header.Narration = row.Table.Columns.Contains("Narration") ? Convert.ToString(row["Narration"]) : string.Empty;
            header.Attachment = row.Table.Columns.Contains("Attachment") ? Convert.ToString(row["Attachment"]) : string.Empty;
            header.TotalDebit = row.Table.Columns.Contains("total_debit") && row["total_debit"] != DBNull.Value ? Convert.ToDecimal(row["total_debit"]) : 0m;
            header.TotalCredit = row.Table.Columns.Contains("total_credit") && row["total_credit"] != DBNull.Value ? Convert.ToDecimal(row["total_credit"]) : 0m;
            header.Status = row.Table.Columns.Contains("status") ? Convert.ToString(row["status"]) : "Draft";
            header.ReversalOf = row.Table.Columns.Contains("reversal_of") && row["reversal_of"] != DBNull.Value ? Convert.ToInt32(row["reversal_of"]) : (int?)null;
            header.PostedBy = row.Table.Columns.Contains("posted_by") && row["posted_by"] != DBNull.Value ? Convert.ToInt32(row["posted_by"]) : (int?)null;
            header.PostedAt = row.Table.Columns.Contains("posted_at") && row["posted_at"] != DBNull.Value ? Convert.ToDateTime(row["posted_at"]) : (DateTime?)null;
            header.IsAutoPosted = row.Table.Columns.Contains("is_auto_posted") && row["is_auto_posted"] != DBNull.Value && Convert.ToBoolean(row["is_auto_posted"]);
            header.CreatedBy = row.Table.Columns.Contains("user_id") && row["user_id"] != DBNull.Value ? Convert.ToInt32(row["user_id"]) : (int?)null;
            header.CreatedAt = row.Table.Columns.Contains("date_created") && row["date_created"] != DBNull.Value ? Convert.ToDateTime(row["date_created"]) : (DateTime?)null;
            header.UpdatedBy = row.Table.Columns.Contains("updated_by") && row["updated_by"] != DBNull.Value ? Convert.ToInt32(row["updated_by"]) : (int?)null;
            header.UpdatedAt = row.Table.Columns.Contains("date_updated") && row["date_updated"] != DBNull.Value ? Convert.ToDateTime(row["date_updated"]) : (DateTime?)null;
            header.BranchId = row.Table.Columns.Contains("branch_id") && row["branch_id"] != DBNull.Value ? Convert.ToInt32(row["branch_id"]) : (int?)null;
            header.CompanyId = row.Table.Columns.Contains("company_id") && row["company_id"] != DBNull.Value ? Convert.ToInt32(row["company_id"]) : (int?)null;
            header.RefModule = row.Table.Columns.Contains("ref_module") ? Convert.ToString(row["ref_module"]) : string.Empty;
            header.RefId = row.Table.Columns.Contains("ref_id") && row["ref_id"] != DBNull.Value ? Convert.ToInt32(row["ref_id"]) : (int?)null;
            return header;
        }

        private List<JVLineModel> MapLines(DataTable linesTable, int voucherId)
        {
            List<JVLineModel> lines = new List<JVLineModel>();
            if (linesTable == null)
            {
                return lines;
            }

            for (int i = 0; i < linesTable.Rows.Count; i++)
            {
                DataRow row = linesTable.Rows[i];
                JVLineModel line = new JVLineModel();
                line.EntryId = row.Table.Columns.Contains("id") && row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                line.VoucherId = voucherId;
                line.LineNo = i + 1;
                line.AccountId = row.Table.Columns.Contains("account_id") && row["account_id"] != DBNull.Value ? Convert.ToInt32(row["account_id"]) : 0;
                line.AccountCode = row.Table.Columns.Contains("AccountCode") ? Convert.ToString(row["AccountCode"]) : string.Empty;
                line.AccountName = row.Table.Columns.Contains("AccountName") ? Convert.ToString(row["AccountName"]) : string.Empty;
                line.Narration = row.Table.Columns.Contains("Description") ? Convert.ToString(row["Description"]) : string.Empty;
                line.Debit = row.Table.Columns.Contains("Debit") && row["Debit"] != DBNull.Value ? Convert.ToDecimal(row["Debit"]) : 0m;
                line.Credit = row.Table.Columns.Contains("Credit") && row["Credit"] != DBNull.Value ? Convert.ToDecimal(row["Credit"]) : 0m;
                line.CostCenterID = row.Table.Columns.Contains("cost_center_id") && row["cost_center_id"] != DBNull.Value ? Convert.ToInt32(row["cost_center_id"]) : 0;
                lines.Add(line);
            }

            return lines;
        }
    }
}
