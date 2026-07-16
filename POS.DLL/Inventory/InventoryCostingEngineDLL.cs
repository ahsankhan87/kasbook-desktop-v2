using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using POS.Core;
using POS.Core.Inventory;

namespace POS.DLL.Inventory
{
    /// <summary>
    /// Data-access layer for the Inventory Costing Engine.
    /// All methods that write data accept an open SqlConnection + SqlTransaction
    /// so they can participate in the caller's existing purchase / sale transaction.
    /// Read-only methods open their own connection.
    /// </summary>
    public class InventoryCostingEngineDLL
    {
        // ─────────────────────────────────────────────────────────────────
        // WAC
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns WAC for every active product using sp_CalculateWACBulk.
        /// </summary>
        public List<WACResult> GetWACBulk(int? branchId = null, DateTime? asOfDate = null)
        {
            var list = new List<WACResult>();

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand("sp_CalculateWACBulk", cn))
                {
                    cmd.CommandType    = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120;
                    cmd.Parameters.AddWithValue("@BranchId", (object)branchId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@AsOfDate", asOfDate.HasValue
                        ? (object)asOfDate.Value.Date
                        : DBNull.Value);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        foreach (DataRow r in dt.Rows)
                            list.Add(MapWACRow(r));
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Returns WAC for a single product by item_number from the live avg_cost.
        /// Fast path — no aggregation needed when avg_cost is maintained on purchase.
        /// </summary>
        public decimal GetStoredWAC(string itemNumber)
        {
            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand(
                    "SELECT ISNULL(avg_cost, 0) FROM dbo.pos_products WHERE item_number = @n", cn))
                {
                    cmd.Parameters.AddWithValue("@n", itemNumber);
                    var val = cmd.ExecuteScalar();
                    return val == null || val == DBNull.Value ? 0m : Convert.ToDecimal(val);
                }
            }
        }

        /// <summary>
        /// Forces a WAC recalculation for a single product using sp_UpdateProductWAC.
        ///
        /// ⚠ DO NOT call this inside Insertpurchases or any live purchase transaction.
        ///    sp_Purchase_items is the authoritative WAC updater during purchase posting;
        ///    calling this in parallel would read stale stock and produce incorrect results.
        ///
        /// Use this only for:
        ///   • Manual/admin WAC correction (stock adjustment forms)
        ///   • Batch recalculation utilities
        ///   • Unit-test scenarios operating on isolated data
        ///
        /// Pass the open connection + transaction when calling from within a larger operation.
        /// </summary>
        public decimal RecalculateWACOnPurchase(
            string itemNumber,
            decimal purchaseQty,
            decimal purchaseCost,
            SqlConnection cn,
            SqlTransaction txn)
        {
            using (var cmd = new SqlCommand("sp_UpdateProductWAC", cn, txn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ItemNumber",   itemNumber);
                cmd.Parameters.AddWithValue("@PurchaseQty",  purchaseQty);
                cmd.Parameters.AddWithValue("@PurchaseCost", purchaseCost);

                using (var da = new SqlDataAdapter(cmd))
                {
                    var dt = new DataTable();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                        return Convert.ToDecimal(dt.Rows[0]["new_wac"]);
                }
            }

            return purchaseCost; // fallback
        }

        // ─────────────────────────────────────────────────────────────────
        // FIFO layers
        // ─────────────────────────────────────────────────────────────────

        /// <summary>Returns open FIFO layers for a product (oldest-first).</summary>
        public List<FIFOCostLayer> GetFIFOLayers(int productId, int? branchId = null)
        {
            var layers = new List<FIFOCostLayer>();

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand("sp_GetFIFOLayers", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ProductId", productId);
                    cmd.Parameters.AddWithValue("@BranchId",
                        branchId.HasValue ? (object)branchId.Value : DBNull.Value);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);
                        foreach (DataRow r in dt.Rows)
                            layers.Add(MapFIFOLayerRow(r));
                    }
                }
            }

            return layers;
        }

        /// <summary>
        /// Inserts a new FIFO cost layer when a purchase is posted.
        /// Must be called inside the purchase transaction.
        /// Returns the new layer_id.
        /// </summary>
        public int InsertFIFOLayer(
            int productId, string itemNumber, int branchId, int purchaseRefId,
            DateTime purchaseDate, decimal qty, decimal unitCost,
            int currencyId, decimal exchangeRate,
            SqlConnection cn, SqlTransaction txn)
        {
            using (var cmd = new SqlCommand("sp_InsertFIFOLayer", cn, txn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@ProductId",     productId);
                cmd.Parameters.AddWithValue("@ItemNumber",    itemNumber);
                cmd.Parameters.AddWithValue("@BranchId",      branchId);
                cmd.Parameters.AddWithValue("@PurchaseRefId", purchaseRefId);
                cmd.Parameters.AddWithValue("@PurchaseDate",  purchaseDate.Date);
                cmd.Parameters.AddWithValue("@Qty",           qty);
                cmd.Parameters.AddWithValue("@UnitCost",      unitCost);
                cmd.Parameters.AddWithValue("@CurrencyId",    currencyId);
                cmd.Parameters.AddWithValue("@ExchangeRate",  exchangeRate);

                var result = cmd.ExecuteScalar();
                return result == null ? 0 : Convert.ToInt32(result);
            }
        }

        /// <summary>
        /// Reduces remaining_qty on one layer during FIFO consumption.
        /// Must be called inside the sale transaction.
        /// </summary>
        public void ConsumeFIFOLayer(
            int layerId, decimal consumeQty,
            SqlConnection cn, SqlTransaction txn)
        {
            using (var cmd = new SqlCommand("sp_ConsumeFIFOLayer", cn, txn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@LayerId",    layerId);
                cmd.Parameters.AddWithValue("@ConsumeQty", consumeQty);
                cmd.ExecuteNonQuery();
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // COGS log (for FIFO pre-insert before SP call)
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Writes FIFO COGS lines into inv_cogs_log (no journal ref yet).
        /// Called inside the sale transaction so the SP can pick them up.
        /// </summary>
        public void InsertCOGSLogLines(
            SaleCOGSRequest request,
            SqlConnection cn,
            SqlTransaction txn)
        {
            foreach (var line in request.Lines)
            {
                using (var cmd = new SqlCommand(@"
                    INSERT INTO dbo.inv_cogs_log
                        (sale_invoice_no, sale_id, product_id, item_number, branch_id,
                         qty_sold, unit_cost, cogs_amount, costing_method,
                         journal_ref, posted_at, posted_by)
                    VALUES
                        (@invoice_no, @sale_id, @product_id, @item_number, @branch_id,
                         @qty_sold, @unit_cost, @cogs_amount, @method,
                         NULL, GETDATE(), @user_id);", cn, txn))
                {
                    cmd.Parameters.AddWithValue("@invoice_no",  request.SaleInvoiceNo);
                    cmd.Parameters.AddWithValue("@sale_id",     request.SaleId);
                    cmd.Parameters.AddWithValue("@product_id",  line.ProductId);
                    cmd.Parameters.AddWithValue("@item_number", line.ItemNumber);
                    cmd.Parameters.AddWithValue("@branch_id",   request.BranchId);
                    cmd.Parameters.AddWithValue("@qty_sold",    line.QtySold);
                    cmd.Parameters.AddWithValue("@unit_cost",   line.UnitCost);
                    cmd.Parameters.AddWithValue("@cogs_amount", line.COGSAmount);
                    cmd.Parameters.AddWithValue("@method",      request.CostingMethod ?? "WAC");
                    cmd.Parameters.AddWithValue("@user_id",     request.UserId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // COGS journal posting
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Posts the COGS journal for a sale invoice by calling sp_PostCOGSBatch.
        /// Opens its own connection (called after the sale transaction commits).
        /// </summary>
        public COGSPostingResult PostCOGSJournal(SaleCOGSRequest request)
        {
            var result = new COGSPostingResult();

            try
            {
                using (var cn = new SqlConnection(dbConnection.ConnectionString))
                {
                    cn.Open();
                    using (var cmd = new SqlCommand("sp_PostCOGSBatch", cn))
                    {
                        cmd.CommandType    = CommandType.StoredProcedure;
                        cmd.CommandTimeout = 60;
                        cmd.Parameters.AddWithValue("@InvoiceNo",          request.SaleInvoiceNo);
                        cmd.Parameters.AddWithValue("@COGSAccountId",      request.COGSAccountId);
                        cmd.Parameters.AddWithValue("@InventoryAccountId", request.InventoryAccountId);
                        cmd.Parameters.AddWithValue("@UserId",             request.UserId);
                        cmd.Parameters.AddWithValue("@BranchId",           request.BranchId);
                        cmd.Parameters.AddWithValue("@CostingMethod",      request.CostingMethod ?? "WAC");
                        cmd.Parameters.AddWithValue("@HeaderRef",
                            string.IsNullOrWhiteSpace(request.JournalRef)
                                ? (object)DBNull.Value
                                : request.JournalRef);

                        using (var da = new SqlDataAdapter(cmd))
                        {
                            var dt = new DataTable();
                            da.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                result.Success    = true;
                                result.JournalRef = Convert.ToString(dt.Rows[0]["journal_ref"]);
                                result.TotalCOGS  = Convert.ToDecimal(dt.Rows[0]["total_cogs"]);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Success      = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        // ─────────────────────────────────────────────────────────────────
        // Reconciliation
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Calls sp_ReconcileInventoryValue and returns per-product rows
        /// plus aggregate variance.
        /// </summary>
        public InventoryReconciliationResult GetReconciliationData(
            DateTime asOfDate, int branchId, int inventoryAccountId)
        {
            var result = new InventoryReconciliationResult
            {
                AsOfDate           = asOfDate,
                BranchId           = branchId,
                ToleranceUsed      = 0.01m
            };

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (var cmd = new SqlCommand("sp_ReconcileInventoryValue", cn))
                {
                    cmd.CommandType    = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120;
                    cmd.Parameters.AddWithValue("@AsOfDate",           asOfDate.Date);
                    cmd.Parameters.AddWithValue("@BranchId",           branchId > 0 ? (object)branchId : DBNull.Value);
                    cmd.Parameters.AddWithValue("@InventoryAccountId", inventoryAccountId);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        var dt = new DataTable();
                        da.Fill(dt);

                        foreach (DataRow r in dt.Rows)
                        {
                            result.Lines.Add(new ReconciliationLine
                            {
                                ProductId    = Convert.ToInt32(r["product_id"]),
                                ItemNumber   = Convert.ToString(r["item_number"]),
                                ProductName  = Convert.ToString(r["product_name"]),
                                ProductCode  = Convert.ToString(r["product_code"]),
                                CategoryCode = Convert.ToString(r["category_code"]),
                                QtyOnHand    = Convert.ToDecimal(r["qty_on_hand"]),
                                UnitCost     = Convert.ToDecimal(r["unit_cost"]),
                                StockValue   = Convert.ToDecimal(r["stock_value"])
                            });
                        }

                        if (dt.Rows.Count > 0)
                        {
                            result.TotalStockValue    = Convert.ToDecimal(dt.Rows[0]["total_stock_value"]);
                            result.GLInventoryBalance = Convert.ToDecimal(dt.Rows[0]["gl_inventory_balance"]);
                            result.Variance           = Convert.ToDecimal(dt.Rows[0]["variance"]);
                            result.IsBalanced         = Math.Abs(result.Variance) <= result.ToleranceUsed;
                        }
                    }
                }
            }

            return result;
        }

        // ─────────────────────────────────────────────────────────────────
        // Private helpers
        // ─────────────────────────────────────────────────────────────────

        private static WACResult MapWACRow(DataRow r)
        {
            return new WACResult
            {
                ProductId           = Convert.ToInt32(r["product_id"]),
                ItemNumber          = Convert.ToString(r["item_number"]),
                ProductName         = Convert.ToString(r["product_name"]),
                ProductCode         = Convert.ToString(r["product_code"]),
                BranchId            = Convert.ToInt32(r["branch_id"]),
                TotalPurchaseQty    = Convert.ToDecimal(r["total_purchase_qty"]),
                TotalPurchaseValue  = Convert.ToDecimal(r["total_purchase_value"]),
                TotalSalesQty       = Convert.ToDecimal(r["total_sales_qty"]),
                CurrentQty          = Convert.ToDecimal(r["current_qty"]),
                WACUnitCost         = Convert.ToDecimal(r["wac_unit_cost"]),
                TotalInventoryValue = Convert.ToDecimal(r["total_inventory_value"]),
                StoredAvgCost       = Convert.ToDecimal(r["stored_avg_cost"]),
                StoredQty           = Convert.ToDecimal(r["stored_qty"]),
                AsOfDate            = Convert.ToDateTime(r["as_of_date"])
            };
        }

        private static FIFOCostLayer MapFIFOLayerRow(DataRow r)
        {
            return new FIFOCostLayer
            {
                LayerId        = Convert.ToInt32(r["layer_id"]),
                ProductId      = Convert.ToInt32(r["product_id"]),
                ItemNumber     = Convert.ToString(r["item_number"]),
                BranchId       = Convert.ToInt32(r["branch_id"]),
                PurchaseRefId  = r["purchase_ref_id"] == DBNull.Value
                                    ? (int?)null
                                    : Convert.ToInt32(r["purchase_ref_id"]),
                PurchaseDate   = Convert.ToDateTime(r["purchase_date"]),
                OriginalQty    = Convert.ToDecimal(r["original_qty"]),
                RemainingQty   = Convert.ToDecimal(r["remaining_qty"]),
                UnitCost       = Convert.ToDecimal(r["unit_cost"]),
                CurrencyId     = r["currency_id"] == DBNull.Value
                                    ? 0
                                    : Convert.ToInt32(r["currency_id"]),
                ExchangeRate   = Convert.ToDecimal(r["exchange_rate"])
            };
        }
    }
}
