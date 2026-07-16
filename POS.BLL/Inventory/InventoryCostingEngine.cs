using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using POS.Core;
using POS.Core.Inventory;
using POS.DLL;
using POS.DLL.Inventory;

namespace POS.BLL.Inventory
{
    /// <summary>
    /// Inventory Costing Engine — public BLL facade.
    ///
    /// Supported methods:
    ///   1.  GetWACost(productId, asOfDate)              → decimal unitCost
    ///   2.  GetWACBulkAll(branchId, asOfDate)           → List&lt;WACResult&gt; (50k product scale)
    ///   3.  RecalculateWACOnPurchase(...)               → decimal newWAC  (in-txn)
    ///   4.  InsertFIFOLayerOnPurchase(...)              → int layerId      (in-txn)
    ///   5.  GetFIFOCost(productId, sellQty, branchId)   → FIFOCostResult
    ///   6.  PostCOGSForSale(request)                    → COGSPostingResult
    ///   7.  ReconcileInventoryValue(asOfDate, ...)      → InventoryReconciliationResult
    /// </summary>
    public class InventoryCostingEngine
    {
        private readonly InventoryCostingEngineDLL _dll;

        public InventoryCostingEngine()
        {
            _dll = new InventoryCostingEngineDLL();
        }

        // ──────────────────────────────────────────────────────────────────
        // 1. GetWACost — single product, as-of a given date
        //    Uses the live avg_cost stored in pos_products (maintained by
        //    sp_Purchase_items on every purchase posting) for O(1) lookup.
        //    Falls back to sp_CalculateWACBulk row if avg_cost = 0.
        // ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns the Weighted Average Cost per unit for a single product.
        /// <para>
        /// The fast path reads <c>pos_products.avg_cost</c> directly because that
        /// column is kept current by <c>sp_Purchase_items</c> on every purchase posting.
        /// If avg_cost is zero (no purchases yet) the method falls back to a bulk WAC
        /// calculation restricted to this product's item_number.
        /// </para>
        /// </summary>
        /// <param name="productId">ID from pos_products.id</param>
        /// <param name="asOfDate">Ignored for the fast path; used by the fallback bulk query.</param>
        public decimal GetWACost(int productId, DateTime asOfDate)
        {
            // Fast path: read stored avg_cost
            // We need item_number to use the DLL method; resolve via WAC bulk (single row)
            var bulkRows = _dll.GetWACBulk(null, asOfDate);
            foreach (var row in bulkRows)
            {
                if (row.ProductId == productId)
                {
                    // Prefer the historically computed WAC; fall back to stored avg_cost
                    return row.WACUnitCost > 0 ? row.WACUnitCost : row.StoredAvgCost;
                }
            }

            return 0m;
        }

        // ──────────────────────────────────────────────────────────────────
        // 2. GetWACBulkAll — optimised for 50 000 products in one SQL query
        // ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns WAC for every active product via a single <c>sp_CalculateWACBulk</c>
        /// call.  Designed for bulk reporting and reconciliation screens.
        /// </summary>
        /// <param name="branchId">NULL = all branches.</param>
        /// <param name="asOfDate">NULL = today.</param>
        public List<WACResult> GetWACBulkAll(int? branchId = null, DateTime? asOfDate = null)
        {
            return _dll.GetWACBulk(branchId, asOfDate);
        }

        // ──────────────────────────────────────────────────────────────────
        // 3. RecalculateWACOnPurchase — utility / manual recalculation ONLY
        //    ⚠ Do NOT call this from PurchasesDLL.Insertpurchases.
        //      sp_Purchase_items is the authoritative WAC updater during live
        //      purchase posting. Use this only for admin corrections or batch jobs.
        // ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// Forces a WAC recalculation for a product using sp_UpdateProductWAC.
        ///
        /// ⚠ Do NOT call from live purchase posting. sp_Purchase_items already
        ///   updates avg_cost atomically; calling this in the same transaction
        ///   would read stale stock and produce incorrect results.
        ///
        /// Use only for: manual corrections, stock adjustments, batch utilities.
        /// </summary>
        /// <param name="itemNumber">pos_products.item_number</param>
        /// <param name="purchaseQty">Quantity in the new purchase line.</param>
        /// <param name="purchaseCost">Per-unit base-currency cost in this purchase.</param>
        /// <param name="cn">Open SqlConnection belonging to the purchase transaction.</param>
        /// <param name="txn">Active SqlTransaction.</param>
        /// <returns>The newly computed WAC value stored in pos_products.avg_cost.</returns>
        public decimal RecalculateWACOnPurchase(
            string itemNumber,
            decimal purchaseQty,
            decimal purchaseCost,
            SqlConnection cn,
            SqlTransaction txn)
        {
            if (string.IsNullOrWhiteSpace(itemNumber))
                throw new ArgumentNullException("itemNumber");
            if (purchaseQty <= 0)
                throw new ArgumentException("purchaseQty must be > 0", "purchaseQty");
            if (purchaseCost < 0)
                throw new ArgumentException("purchaseCost cannot be negative", "purchaseCost");

            return _dll.RecalculateWACOnPurchase(itemNumber, purchaseQty, purchaseCost, cn, txn);
        }

        // ──────────────────────────────────────────────────────────────────
        // 4. InsertFIFOLayerOnPurchase — called from PurchasesDLL inside txn
        // ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// Creates a new FIFO cost layer when a purchase is posted.
        /// Must be called inside the purchase transaction.
        /// </summary>
        /// <returns>New layer_id from inv_cost_layers.</returns>
        public int InsertFIFOLayerOnPurchase(
            int productId, string itemNumber, int branchId, int purchaseRefId,
            DateTime purchaseDate, decimal qty, decimal unitCost,
            int currencyId, decimal exchangeRate,
            SqlConnection cn, SqlTransaction txn)
        {
            if (qty <= 0)
                throw new ArgumentException("qty must be > 0", "qty");
            if (unitCost < 0)
                throw new ArgumentException("unitCost cannot be negative", "unitCost");

            return _dll.InsertFIFOLayer(
                productId, itemNumber, branchId, purchaseRefId,
                purchaseDate, qty, unitCost,
                currencyId, exchangeRate,
                cn, txn);
        }

        // ──────────────────────────────────────────────────────────────────
        // 5. GetFIFOCost — cost consumed from FIFO layers for a given qty
        // ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// Calculates the cost of selling <paramref name="sellQty"/> units using FIFO
        /// (consumes from the oldest layers first).
        ///
        /// <para>This method is <b>read-only</b>: it does NOT reduce layer quantities.
        /// To actually deduct the layers, call <see cref="PostCOGSForSale"/> which
        /// performs the consumption inside the sale transaction.</para>
        /// </summary>
        /// <param name="productId">pos_products.id</param>
        /// <param name="sellQty">Units being sold.</param>
        /// <param name="branchId">Branch to scope layers.</param>
        public FIFOCostResult GetFIFOCost(int productId, decimal sellQty, int branchId)
        {
            var result = new FIFOCostResult { RequestedQty = sellQty };

            if (sellQty <= 0)
                return result;

            List<FIFOCostLayer> layers = _dll.GetFIFOLayers(productId, branchId);

            decimal remaining = sellQty;

            foreach (var layer in layers)
            {
                if (remaining <= 0m)
                    break;

                decimal consume = Math.Min(remaining, layer.RemainingQty);
                decimal lineCost = Math.Round(consume * layer.UnitCost, 6);

                result.TotalCost      += lineCost;
                result.FulfilledQty   += consume;
                result.UnitCostLayers.Add(new FIFOConsumptionLine
                {
                    LayerId      = layer.LayerId,
                    PurchaseDate = layer.PurchaseDate,
                    ConsumedQty  = consume,
                    UnitCost     = layer.UnitCost,
                    LineCost     = lineCost
                });

                remaining -= consume;
            }

            result.HasShortfall = remaining > 0m;

            if (result.FulfilledQty > 0m)
                result.UnitCostWeighted = Math.Round(result.TotalCost / result.FulfilledQty, 6);

            return result;
        }

        // ──────────────────────────────────────────────────────────────────
        // 6. PostCOGSForSale — called from SalesBLL when sale is posted
        // ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// Posts Cost of Goods Sold for a completed sale invoice.
        ///
        /// <b>WAC flow</b>:
        ///   – Reads avg_cost from pos_products per line.
        ///   – Calls <c>sp_PostCOGSBatch</c> which posts DR COGS / CR Inventory
        ///     and writes inv_cogs_log.
        ///
        /// <b>FIFO flow</b>:
        ///   – Caller must supply <see cref="SaleCOGSRequest.Lines"/> pre-populated
        ///     with unit_cost from <see cref="GetFIFOCost"/>.
        ///   – This method writes the cogs-log lines (inside the sale transaction),
        ///     then consumes the FIFO layers.
        ///   – After the sale transaction commits, calls <c>sp_PostCOGSBatch</c>
        ///     to post the journal.
        ///
        /// All journal entries use the same <c>sp_JournalsCrud</c> + acc_entries_header
        /// pattern used throughout the application.
        /// </summary>
        public COGSPostingResult PostCOGSForSale(SaleCOGSRequest request)
        {
            if (request == null)
                throw new ArgumentNullException("request");

            Validate(request);

            string method = (request.CostingMethod ?? "WAC").ToUpper();

            if (method == "FIFO")
                return PostCOGSFIFO(request);

            // WAC: delegate entirely to the SP (no C# layer consumption needed)
            return _dll.PostCOGSJournal(request);
        }

        // ──────────────────────────────────────────────────────────────────
        // 7. ReconcileInventoryValue
        // ──────────────────────────────────────────────────────────────────

        /// <summary>
        /// Compares the sum of (qty × avg_cost) for all active products against the
        /// Inventory GL account balance in <c>acc_entries</c> as of
        /// <paramref name="asOfDate"/>.
        ///
        /// Returns a <see cref="InventoryReconciliationResult"/> with per-product
        /// rows, the aggregate stock value, the GL balance, and the variance.
        /// </summary>
        /// <param name="asOfDate">Reporting date for the reconciliation.</param>
        /// <param name="branchId">0 = all branches.</param>
        /// <param name="inventoryAccountId">GL account ID for the Inventory asset.</param>
        public InventoryReconciliationResult ReconcileInventoryValue(
            DateTime asOfDate, int branchId, int inventoryAccountId)
        {
            if (inventoryAccountId <= 0)
                throw new ArgumentException(
                    "inventoryAccountId must be a valid GL account ID.",
                    "inventoryAccountId");

            var result = _dll.GetReconciliationData(asOfDate, branchId, inventoryAccountId);

            // Derive IsBalanced with a 1-cent tolerance
            result.IsBalanced = Math.Abs(result.Variance) <= result.ToleranceUsed;

            return result;
        }

        // ──────────────────────────────────────────────────────────────────
        // Private helpers
        // ──────────────────────────────────────────────────────────────────

        private COGSPostingResult PostCOGSFIFO(SaleCOGSRequest request)
        {
            // For FIFO the caller (SalesBLL) must supply Lines with UnitCost already
            // computed via GetFIFOCost().  We:
            //   a) Open a connection and transaction
            //   b) Write cogs-log rows and consume layers atomically
            //   c) Commit
            //   d) Call sp_PostCOGSBatch to post the GL journal

            var result = new COGSPostingResult();

            try
            {
                using (var cn = new SqlConnection(dbConnection.ConnectionString))
                {
                    cn.Open();
                    using (var txn = cn.BeginTransaction())
                    {
                        try
                        {
                            // Write pending COGS log lines (no journal ref yet)
                            _dll.InsertCOGSLogLines(request, cn, txn);

                            // Consume FIFO layers
                            foreach (var line in request.Lines)
                            {
                                // Retrieve layers to consume for this product
                                // We need product_id; look it up via a quick query
                                int productId = line.ProductId;
                                if (productId <= 0)
                                    continue;

                                List<FIFOCostLayer> layers = GetFIFOLayersInTxn(productId, request.BranchId, cn, txn);

                                decimal remaining = line.QtySold;
                                foreach (var layer in layers)
                                {
                                    if (remaining <= 0m)
                                        break;

                                    decimal consume = Math.Min(remaining, layer.RemainingQty);
                                    _dll.ConsumeFIFOLayer(layer.LayerId, consume, cn, txn);
                                    remaining -= consume;
                                }
                            }

                            txn.Commit();
                        }
                        catch
                        {
                            txn.Rollback();
                            throw;
                        }
                    }
                }

                // Post GL journal (its own transaction inside sp_PostCOGSBatch)
                result = _dll.PostCOGSJournal(request);
            }
            catch (Exception ex)
            {
                result.Success      = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// Reads FIFO layers within an existing transaction (so the consumption
        /// reads are consistent with the locks held by the surrounding transaction).
        /// </summary>
        private List<FIFOCostLayer> GetFIFOLayersInTxn(
            int productId, int branchId,
            SqlConnection cn, SqlTransaction txn)
        {
            var layers = new List<FIFOCostLayer>();

            using (var cmd = new SqlCommand(
                @"SELECT layer_id, product_id, item_number, branch_id,
                         purchase_ref_id, purchase_date,
                         original_qty, remaining_qty, unit_cost,
                         currency_id, exchange_rate, created_at
                  FROM dbo.inv_cost_layers
                  WHERE product_id  = @pid
                    AND branch_id   = @bid
                    AND remaining_qty > 0
                  ORDER BY purchase_date ASC, layer_id ASC", cn, txn))
            {
                cmd.Parameters.AddWithValue("@pid", productId);
                cmd.Parameters.AddWithValue("@bid", branchId);

                using (var rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        layers.Add(new FIFOCostLayer
                        {
                            LayerId       = rdr.GetInt32(rdr.GetOrdinal("layer_id")),
                            ProductId     = rdr.GetInt32(rdr.GetOrdinal("product_id")),
                            ItemNumber    = rdr.GetString(rdr.GetOrdinal("item_number")),
                            BranchId      = rdr.GetInt32(rdr.GetOrdinal("branch_id")),
                            PurchaseRefId = rdr.IsDBNull(rdr.GetOrdinal("purchase_ref_id"))
                                               ? (int?)null
                                               : rdr.GetInt32(rdr.GetOrdinal("purchase_ref_id")),
                            PurchaseDate  = rdr.GetDateTime(rdr.GetOrdinal("purchase_date")),
                            OriginalQty   = rdr.GetDecimal(rdr.GetOrdinal("original_qty")),
                            RemainingQty  = rdr.GetDecimal(rdr.GetOrdinal("remaining_qty")),
                            UnitCost      = rdr.GetDecimal(rdr.GetOrdinal("unit_cost")),
                            CurrencyId    = rdr.IsDBNull(rdr.GetOrdinal("currency_id"))
                                               ? 0
                                               : rdr.GetInt32(rdr.GetOrdinal("currency_id")),
                            ExchangeRate  = rdr.GetDecimal(rdr.GetOrdinal("exchange_rate"))
                        });
                    }
                }
            }

            return layers;
        }

        private static void Validate(SaleCOGSRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SaleInvoiceNo))
                throw new ArgumentException("SaleInvoiceNo is required.", "request");

            if (request.COGSAccountId <= 0)
                throw new ArgumentException(
                    "COGSAccountId must be set in valuation settings before posting COGS.",
                    "request");

            if (request.InventoryAccountId <= 0)
                throw new ArgumentException(
                    "InventoryAccountId must be set in valuation settings before posting COGS.",
                    "request");
        }
    }
}
