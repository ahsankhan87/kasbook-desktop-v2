using POS.Core;
using POS.Core.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL.Inventory
{
    public class InventoryValuationDLL
    {
        // -----------------------------------------------------------------------
        // Settings
        // -----------------------------------------------------------------------

        public InventoryValuationSettings GetSettings(int branchId = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetInventoryValuationSettings", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                try
                {
                    cn.Open();
                    using (SqlDataReader rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            return new InventoryValuationSettings
                            {
                                Id                   = SafeInt(rd, "id"),
                                BranchId             = SafeInt(rd, "branch_id"),
                                ValuationMethod      = SafeStr(rd, "valuation_method"),
                                CostComponents       = SafeStr(rd, "cost_components"),
                                IncludeFilter        = SafeStr(rd, "include_filter"),
                                CogsAccountId        = SafeNullInt(rd, "cogs_account_id"),
                                CogsAccountName      = SafeStr(rd, "cogs_account_name"),
                                CogsAccountCode      = SafeStr(rd, "cogs_account_code"),
                                InventoryAccountId   = SafeNullInt(rd, "inventory_account_id"),
                                InventoryAccountName = SafeStr(rd, "inventory_account_name"),
                                InventoryAccountCode = SafeStr(rd, "inventory_account_code"),
                                PostPerProduct       = SafeBool(rd, "post_per_product"),
                                UpdatedAt            = SafeNullDate(rd, "updated_at")
                            };
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error loading valuation settings: " + ex.Message, ex);
                }
            }

            // Return safe defaults if no row found yet
            return new InventoryValuationSettings { BranchId = branchId };
        }

        public void SaveSettings(InventoryValuationSettings s)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_SaveInventoryValuationSettings", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId",           s.BranchId);
                cmd.Parameters.AddWithValue("@ValuationMethod",    s.ValuationMethod ?? "WAC");
                cmd.Parameters.AddWithValue("@CostComponents",     s.CostComponents ?? "PURCHASE_ONLY");
                cmd.Parameters.AddWithValue("@IncludeFilter",      s.IncludeFilter ?? "ACTIVE_ONLY");
                cmd.Parameters.AddWithValue("@CogsAccountId",      (object)s.CogsAccountId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@InventoryAccountId", (object)s.InventoryAccountId ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@PostPerProduct",     s.PostPerProduct ? 1 : 0);
                cmd.Parameters.AddWithValue("@UserId",             UsersModal.logged_in_userid);
                try
                {
                    cn.Open();
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error saving valuation settings: " + ex.Message, ex);
                }
            }
        }

        // -----------------------------------------------------------------------
        // Accounts lookup (for dropdowns in settings form)
        // -----------------------------------------------------------------------

        public DataTable GetAccountsByType(int accountTypeId = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetAccountsByType", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@AccountTypeId", accountTypeId);
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error loading accounts: " + ex.Message, ex);
                }
                return dt;
            }
        }

        // -----------------------------------------------------------------------
        // Bulk inventory valuation (calls sp_CalculateInventoryValueBulk)
        // Returns raw DataTable — BLL enriches it
        // -----------------------------------------------------------------------

        public DataTable CalculateValuationRaw(DateTime asOfDate, string method, int branchId = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_CalculateInventoryValueBulk", cn))
            {
                cmd.CommandType  = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@AsOfDate",  asOfDate.Date);
                cmd.Parameters.AddWithValue("@Method",    method ?? "WAC");
                cmd.Parameters.AddWithValue("@BranchId",  branchId);
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error calculating inventory value: " + ex.Message, ex);
                }
                return dt;
            }
        }

        // -----------------------------------------------------------------------
        // Enrich valuation lines with last-purchase info and unit names
        // -----------------------------------------------------------------------

        public DataTable GetLastPurchaseInfo(int branchId)
        {
            const string sql = @"
                SELECT
                    p.id AS product_id,
                    MAX(ph.date_created) AS last_purchase_date,
                    MAX(CASE WHEN ranked.rn = 1 THEN ranked.unit_cost ELSE NULL END) AS last_purchase_cost
                FROM pos_products p
                INNER JOIN pos_purchase_detail pd ON pd.item_number = p.item_number
                INNER JOIN pos_purchase ph ON ph.id = pd.purchase_id AND ph.deleted = 0
                CROSS APPLY (
                    SELECT TOP 1 pd2.unit_cost, ROW_NUMBER() OVER (ORDER BY ph2.date_created DESC, pd2.id DESC) AS rn
                    FROM pos_purchase_detail pd2
                    INNER JOIN pos_purchase ph2 ON ph2.id = pd2.purchase_id AND ph2.deleted = 0
                    WHERE pd2.item_number = p.item_number
                ) ranked
                WHERE p.deleted = 0
                GROUP BY p.id";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch
                {
                    // Non-fatal — return empty table; caller handles missing data
                    return dt;
                }
                return dt;
            }
        }

        public DataTable GetReorderLevels(int branchId)
        {
            const string sql = @"
                SELECT p.id AS product_id,
                       COALESCE((SELECT TOP 1 ps.reorder_level FROM pos_product_stocks ps
                                 WHERE ps.item_number = p.item_number AND ps.branch_id = @branch_id
                                 ORDER BY ps.id DESC), p.re_stock_level, 0) AS reorder_level
                FROM pos_products p WHERE p.deleted = 0";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", branchId);
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch
                {
                    return dt;
                }
                return dt;
            }
        }

        // -----------------------------------------------------------------------
        // Category average cost (used for High Cost status badge)
        // -----------------------------------------------------------------------

        public DataTable GetCategoryAvgCosts()
        {
            const string sql = @"
                SELECT p.category_code,
                       AVG(p.avg_cost) AS category_avg_cost
                FROM pos_products p
                WHERE p.deleted = 0 AND p.avg_cost > 0
                GROUP BY p.category_code";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch
                {
                    return dt;
                }
                return dt;
            }
        }

        // -----------------------------------------------------------------------
        // Top categories / suppliers by inventory value (for summary panel)
        // -----------------------------------------------------------------------

        public DataTable GetTopCategoriesByValue(int branchId, int topN = 5)
        {
            string sql = $@"
                SELECT TOP {topN}
                    p.category_code,
                    ISNULL(c.name, p.category_code) AS category_name,
                    SUM(ISNULL(ps.branch_qty, 0) * p.avg_cost) AS total_value
                FROM pos_products p
                LEFT JOIN pos_categories c ON c.code = p.category_code
                LEFT JOIN (
                    SELECT item_number, SUM(qty) AS branch_qty
                    FROM pos_product_stocks WITH (NOLOCK)
                    WHERE branch_id = @branch_id
                    GROUP BY item_number
                ) ps ON ps.item_number = p.item_number
                WHERE p.deleted = 0
                GROUP BY p.category_code, c.name
                ORDER BY total_value DESC";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", branchId);
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch
                {
                    return dt;
                }
                return dt;
            }
        }

        public DataTable GetTopSuppliersByValue(int branchId, int topN = 5)
        {
            string sql = $@"
                SELECT TOP {topN}
                    ISNULL(s.name, 'Unassigned') AS supplier_name,
                    SUM(ISNULL(ps.branch_qty, 0) * p.avg_cost) AS total_value
                FROM pos_products p
                LEFT JOIN pos_suppliers s ON s.id = p.supplier_id
                LEFT JOIN (
                    SELECT item_number, SUM(qty) AS branch_qty
                    FROM pos_product_stocks WITH (NOLOCK)
                    WHERE branch_id = @branch_id
                    GROUP BY item_number
                ) ps ON ps.item_number = p.item_number
                WHERE p.deleted = 0
                GROUP BY s.name
                ORDER BY total_value DESC";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", branchId);
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch
                {
                    return dt;
                }
                return dt;
            }
        }

        // -----------------------------------------------------------------------
        // COGS report
        // -----------------------------------------------------------------------

        public DataTable GetCOGSReport(DateTime fromDate, DateTime toDate, int branchId, string method)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_InventoryCOGSReport", cn))
            {
                cmd.CommandType  = CommandType.StoredProcedure;
                cmd.CommandTimeout = 120;
                cmd.Parameters.AddWithValue("@FromDate",  fromDate.Date);
                cmd.Parameters.AddWithValue("@ToDate",    toDate.Date);
                cmd.Parameters.AddWithValue("@BranchId",  branchId);
                cmd.Parameters.AddWithValue("@Method",    method ?? "WAC");
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error loading COGS report: " + ex.Message, ex);
                }
                return dt;
            }
        }

        // -----------------------------------------------------------------------
        // Sales value per product for gross-margin calculation
        // -----------------------------------------------------------------------

        public DataTable GetSalesValueByProduct(DateTime fromDate, DateTime toDate, int branchId)
        {
            const string sql = @"
                SELECT
                    p.id AS product_id,
                    SUM(sd.qty * sd.unit_price) AS sales_value
                FROM pos_sales_detail sd
                INNER JOIN pos_sales sh ON sh.id = sd.sales_id
                INNER JOIN pos_products p ON p.item_number = sd.item_number
                WHERE sh.deleted = 0
                  AND CAST(sh.date_created AS DATE) BETWEEN @from_date AND @to_date
                  AND (@branch_id = 0 OR sh.branch_id = @branch_id)
                GROUP BY p.id";

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@from_date",  fromDate.Date);
                cmd.Parameters.AddWithValue("@to_date",    toDate.Date);
                cmd.Parameters.AddWithValue("@branch_id",  branchId);
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch
                {
                    return dt;
                }
                return dt;
            }
        }

        // -----------------------------------------------------------------------
        // COGS JV posting (inserts acc_entries_header + acc_entries lines)
        // -----------------------------------------------------------------------

        public string GetNextCOGSVoucherNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_GetNextCOGSVoucherNo", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                try
                {
                    cn.Open();
                    var result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : "COGS-000001";
                }
                catch (Exception ex)
                {
                    throw new Exception("Error getting COGS voucher number: " + ex.Message, ex);
                }
            }
        }

        public int PostCOGSJournalEntry(COGSPostingRequest request)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        string voucherNo = GetNextCOGSVoucherNoOnConnection(cn, tx);

                        // Insert header
                        int headerId;
                        using (SqlCommand hCmd = new SqlCommand(@"
                            INSERT INTO acc_entries_header
                                (InvoiceNo, EntryDate, VoucherType, ReferenceNo, Narration,
                                 total_debit, total_credit, status, is_auto_posted,
                                 date_created, date_updated, user_id, branch_id)
                            VALUES
                                (@InvoiceNo, @EntryDate, 'JV', @ReferenceNo, @Narration,
                                 @TotalDebit, @TotalCredit, 'Posted', 1,
                                 GETDATE(), GETDATE(), @UserId, @BranchId);
                            SELECT CAST(SCOPE_IDENTITY() AS INT);", cn, tx))
                        {
                            hCmd.Parameters.AddWithValue("@InvoiceNo",    voucherNo);
                            hCmd.Parameters.AddWithValue("@EntryDate",    request.ToDate.Date);
                            hCmd.Parameters.AddWithValue("@ReferenceNo",  $"COGS {request.FromDate:dd-MMM-yyyy} to {request.ToDate:dd-MMM-yyyy}");
                            hCmd.Parameters.AddWithValue("@Narration",    request.Narration ?? $"COGS posting period {request.FromDate:dd-MMM-yy} to {request.ToDate:dd-MMM-yy}");
                            hCmd.Parameters.AddWithValue("@TotalDebit",   request.TotalCogs);
                            hCmd.Parameters.AddWithValue("@TotalCredit",  request.TotalCogs);
                            hCmd.Parameters.AddWithValue("@UserId",       UsersModal.logged_in_userid);
                            hCmd.Parameters.AddWithValue("@BranchId",     UsersModal.logged_in_branch_id);
                            headerId = Convert.ToInt32(hCmd.ExecuteScalar());
                        }

                        if (request.PostPerProduct && request.Lines != null && request.Lines.Count > 0)
                        {
                            // Per-product lines: each product gets its own DR/CR pair
                            foreach (var line in request.Lines)
                            {
                                if (line.CogsValue <= 0) continue;

                                // DR COGS
                                InsertJournalLine(cn, tx, voucherNo, request.CogsAccountId,
                                    request.ToDate, (double)line.CogsValue, 0,
                                    $"COGS - {line.Code} {line.Name}", headerId);

                                // CR Inventory
                                InsertJournalLine(cn, tx, voucherNo, request.InventoryAccountId,
                                    request.ToDate, 0, (double)line.CogsValue,
                                    $"Inv - {line.Code} {line.Name}", headerId);
                            }
                        }
                        else
                        {
                            // Summary: single DR/CR pair
                            string narr = $"COGS {request.FromDate:dd-MMM-yy} to {request.ToDate:dd-MMM-yy}";

                            InsertJournalLine(cn, tx, voucherNo, request.CogsAccountId,
                                request.ToDate, (double)request.TotalCogs, 0, narr, headerId);

                            InsertJournalLine(cn, tx, voucherNo, request.InventoryAccountId,
                                request.ToDate, 0, (double)request.TotalCogs, narr, headerId);
                        }

                        tx.Commit();

                        Log.LogAction("Post COGS JV",
                            $"VoucherNo={voucherNo}, Period={request.FromDate:dd-MMM-yy} to {request.ToDate:dd-MMM-yy}, Amount={request.TotalCogs:N2}",
                            UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

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

        // -----------------------------------------------------------------------
        // Snapshot
        // -----------------------------------------------------------------------

        public DataTable TakeSnapshot(DateTime snapshotDate, int branchId, string method)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_TakeInventorySnapshot", cn))
            {
                cmd.CommandType  = CommandType.StoredProcedure;
                cmd.CommandTimeout = 180;
                cmd.Parameters.AddWithValue("@SnapshotDate", snapshotDate.Date);
                cmd.Parameters.AddWithValue("@BranchId",     branchId);
                cmd.Parameters.AddWithValue("@Method",       method ?? "WAC");
                cmd.Parameters.AddWithValue("@UserId",       UsersModal.logged_in_userid);
                cmd.Parameters.AddWithValue("@Notes",        $"Manual snapshot by user {UsersModal.logged_in_userid}");
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error taking snapshot: " + ex.Message, ex);
                }
                return dt;
            }
        }

        // -----------------------------------------------------------------------
        // Negative stock alert
        // -----------------------------------------------------------------------

        public DataTable GetNegativeStockAlert(int branchId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("sp_NegativeStockAlert", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@BranchId", branchId);
                DataTable dt = new DataTable();
                try
                {
                    cn.Open();
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        da.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error loading negative stock: " + ex.Message, ex);
                }
                return dt;
            }
        }

        // -----------------------------------------------------------------------
        // Private helpers
        // -----------------------------------------------------------------------

        private void InsertJournalLine(SqlConnection cn, SqlTransaction tx,
            string invoiceNo, int accountId, DateTime entryDate,
            double debit, double credit, string description, int entryId)
        {
            using (SqlCommand cmd = new SqlCommand("sp_JournalsCrud", cn, tx))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@invoice_no",              invoiceNo);
                cmd.Parameters.AddWithValue("@account_id",              accountId);
                cmd.Parameters.AddWithValue("@entry_date",              entryDate);
                cmd.Parameters.AddWithValue("@debit",                   debit);
                cmd.Parameters.AddWithValue("@credit",                  credit);
                cmd.Parameters.AddWithValue("@description",             description ?? "");
                cmd.Parameters.AddWithValue("@user_id",                 UsersModal.logged_in_userid);
                cmd.Parameters.AddWithValue("@branch_id",               UsersModal.logged_in_branch_id);
                cmd.Parameters.AddWithValue("@date_created",            DateTime.Now);
                cmd.Parameters.AddWithValue("@customer_id",             0);
                cmd.Parameters.AddWithValue("@supplier_id",             0);
                cmd.Parameters.AddWithValue("@bank_id",                 0);
                cmd.Parameters.AddWithValue("@entry_id",                entryId);
                cmd.Parameters.AddWithValue("@payment_ref_invoice_no",  DBNull.Value);
                cmd.Parameters.AddWithValue("@OperationType",           "1");
                cmd.ExecuteScalar();
            }
        }

        private string GetNextCOGSVoucherNoOnConnection(SqlConnection cn, SqlTransaction tx)
        {
            using (SqlCommand cmd = new SqlCommand(@"
                DECLARE @maxNo NVARCHAR(20);
                SELECT @maxNo = MAX(InvoiceNo) FROM acc_entries_header WHERE InvoiceNo LIKE 'COGS-%';
                IF @maxNo IS NULL SELECT 'COGS-000001';
                ELSE SELECT 'COGS-' + RIGHT('000000' + CAST(TRY_CAST(SUBSTRING(@maxNo,6,10) AS INT)+1 AS VARCHAR),6);",
                cn, tx))
            {
                var r = cmd.ExecuteScalar();
                return r != null ? r.ToString() : "COGS-000001";
            }
        }

        // -- Null-safe reader helpers ------------------------------------------

        private static string SafeStr(SqlDataReader rd, string col)
        {
            int o = rd.GetOrdinal(col);
            return rd.IsDBNull(o) ? string.Empty : rd.GetString(o);
        }

        private static int SafeInt(SqlDataReader rd, string col)
        {
            int o = rd.GetOrdinal(col);
            return rd.IsDBNull(o) ? 0 : Convert.ToInt32(rd[o]);
        }

        private static int? SafeNullInt(SqlDataReader rd, string col)
        {
            int o = rd.GetOrdinal(col);
            return rd.IsDBNull(o) ? (int?)null : Convert.ToInt32(rd[o]);
        }

        private static bool SafeBool(SqlDataReader rd, string col)
        {
            int o = rd.GetOrdinal(col);
            return !rd.IsDBNull(o) && Convert.ToBoolean(rd[o]);
        }

        private static DateTime? SafeNullDate(SqlDataReader rd, string col)
        {
            int o = rd.GetOrdinal(col);
            return rd.IsDBNull(o) ? (DateTime?)null : Convert.ToDateTime(rd[o]);
        }
    }
}
