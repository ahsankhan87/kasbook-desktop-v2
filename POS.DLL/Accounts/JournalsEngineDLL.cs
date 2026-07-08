using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace POS.DLL
{
    public partial class JournalsDLL
    {
        private const int BulkCopyThreshold = 100;

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

                //if (!AreAccountsActive(lines, out List<ValidationError> accountErrors))
                //{
                //    result.Messages.AddRange(accountErrors);
                //    return result;
                //}

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
                JVVoucherModel source = GetVoucherWithLines(voucherId);
                if (source == null || source.Header == null || string.IsNullOrWhiteSpace(source.Header.VoucherNo))
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
                        CostCenter = line.CostCenter,
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

        public JVVoucherModel GetVoucherWithLines(int voucherId)
        {
            DataTable headerTable = GetVoucherHeaderById(voucherId);
            if (headerTable == null || headerTable.Rows.Count == 0)
            {
                return null;
            }

            DataRow headerRow = headerTable.Rows[0];
            string invoiceNo = Convert.ToString(headerRow["InvoiceNo"]);
            DataTable linesTable = GetVoucherLines(invoiceNo);

            JVVoucherModel voucher = new JVVoucherModel();
            voucher.Header = MapHeader(headerRow);
            voucher.Lines = MapLines(linesTable, voucherId);
            return voucher;
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
                    JVVoucherModel voucher = GetVoucherWithLines(voucherId);
                    if (voucher == null || voucher.Header == null)
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
                                new XElement("CostCenter", line.CostCenter ?? string.Empty),
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
                line.CostCenter = string.Empty;
                lines.Add(line);
            }

            return lines;
        }
    }
}
