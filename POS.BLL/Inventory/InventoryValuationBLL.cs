using POS.Core;
using POS.Core.Inventory;
using POS.DLL.Inventory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace POS.BLL.Inventory
{
    public class InventoryValuationBLL
    {
        private readonly InventoryValuationDLL _dll = new InventoryValuationDLL();

        // -----------------------------------------------------------------------
        // Settings
        // -----------------------------------------------------------------------

        public InventoryValuationSettings GetSettings(int branchId = 0)
        {
            try { return _dll.GetSettings(branchId); }
            catch { throw; }
        }

        public void SaveSettings(InventoryValuationSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (string.IsNullOrEmpty(settings.ValuationMethod))
                throw new ArgumentException("Valuation method is required.");
            if (settings.CogsAccountId == null || settings.CogsAccountId <= 0)
                throw new ArgumentException("COGS account must be selected.");
            if (settings.InventoryAccountId == null || settings.InventoryAccountId <= 0)
                throw new ArgumentException("Inventory asset account must be selected.");

            try { _dll.SaveSettings(settings); }
            catch { throw; }
        }

        // -----------------------------------------------------------------------
        // Account dropdowns
        // -----------------------------------------------------------------------

        public DataTable GetAssetAccounts()
        {
            // account_type_id = 1 (Current Assets) — adjust if your chart differs
            try { return _dll.GetAccountsByType(1); }
            catch { throw; }
        }

        public DataTable GetExpenseAccounts()
        {
            // account_type_id = 5 (Expenses)
            try { return _dll.GetAccountsByType(5); }
            catch { throw; }
        }

        // -----------------------------------------------------------------------
        // Inventory Valuation Calculation
        // -----------------------------------------------------------------------

        /// <summary>
        /// Returns enriched valuation lines and a computed summary.
        /// Heavy work (DB query) happens here; call from Task.Run on UI side.
        /// </summary>
        public List<InventoryValuationLine> CalculateValuation(
            DateTime asOfDate,
            string method,
            int branchId,
            string categoryCode,
            string brandCode,
            int? supplierId,
            string locationCode,
            bool showZeroStock)
        {
            DataTable raw = _dll.CalculateValuationRaw(asOfDate, method, branchId);

            // Lookup tables (lightweight supplementary queries)
            DataTable lastPurchase   = _dll.GetLastPurchaseInfo(branchId);
            DataTable reorderLevels  = _dll.GetReorderLevels(branchId);
            DataTable categoryAvgs   = _dll.GetCategoryAvgCosts();

            // Build indexed dictionaries for O(1) lookup
            var lpDict = BuildIntDict(lastPurchase, "product_id",
                r => new { Date = SafeNullDate(r, "last_purchase_date"), Cost = SafeDecimal(r, "last_purchase_cost") });

            var rlDict = BuildIntDict(reorderLevels, "product_id",
                r => SafeDecimal(r, "reorder_level"));

            var catAvgDict = new Dictionary<string, decimal>(StringComparer.OrdinalIgnoreCase);
            foreach (DataRow r in categoryAvgs.Rows)
                catAvgDict[r["category_code"].ToString()] = SafeDecimal(r, "category_avg_cost");

            var lines = new List<InventoryValuationLine>(raw.Rows.Count);

            foreach (DataRow row in raw.Rows)
            {
                int productId      = Convert.ToInt32(row["product_id"]);
                decimal qty        = SafeDecimal(row, "current_qty");
                decimal unitCost   = SafeDecimal(row, "unit_cost");
                decimal totalValue = SafeDecimal(row, "total_value");
                string catCode     = row["category_code"].ToString();

                // Apply client-side filters
                if (!string.IsNullOrEmpty(categoryCode) &&
                    !string.Equals(catCode, categoryCode, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!string.IsNullOrEmpty(brandCode) &&
                    !string.Equals(row["brand_code"].ToString(), brandCode, StringComparison.OrdinalIgnoreCase))
                    continue;

                if (!showZeroStock && qty == 0)
                    continue;

                // Supplementary data
                DateTime? lastPurchaseDate = null;
                decimal lastPurchaseCost   = 0;
                if (lpDict.TryGetValue(productId, out var lp))
                {
                    lastPurchaseDate = lp.Date;
                    lastPurchaseCost = lp.Cost;
                }

                decimal reorderLevel = rlDict.TryGetValue(productId, out decimal rl) ? rl : 0;

                // Valuation status
                string status = DetermineStatus(qty, unitCost, catCode, catAvgDict);

                lines.Add(new InventoryValuationLine
                {
                    ProductId        = productId,
                    ItemNumber       = row["item_number"].ToString(),
                    Code             = row["code"].ToString(),
                    Name             = row["name"].ToString(),
                    CategoryCode     = catCode,
                    BrandCode        = row["brand_code"].ToString(),
                    CurrentQty       = qty,
                    UnitCost         = unitCost,
                    TotalValue       = totalValue,
                    AvgCost          = SafeDecimal(row, "avg_cost"),
                    LastCost         = SafeDecimal(row, "last_cost"),
                    FifoCost         = SafeDecimal(row, "fifo_unit_cost"),
                    StandardCost     = SafeDecimal(row, "standard_cost"),
                    LastPurchaseDate = lastPurchaseDate,
                    LastPurchaseCost = lastPurchaseCost,
                    ReorderLevel     = reorderLevel,
                    EffectiveMethod  = row["effective_method"].ToString(),
                    ValuationStatus  = status
                });
            }

            return lines;
        }

        public InventoryValuationSummary BuildSummary(List<InventoryValuationLine> lines, int branchId)
        {
            if (lines == null || lines.Count == 0)
                return new InventoryValuationSummary();

            decimal totalValue = lines.Sum(l => l.TotalValue);
            decimal totalQty   = lines.Sum(l => l.CurrentQty);

            // Top 5 categories
            var topCats = lines
                .GroupBy(l => l.CategoryCode)
                .Select(g => new CategoryValueItem
                {
                    CategoryCode = g.Key,
                    CategoryName = g.Key,
                    TotalValue   = g.Sum(l => l.TotalValue),
                    Percentage   = totalValue > 0 ? (double)(g.Sum(l => l.TotalValue) / totalValue * 100) : 0
                })
                .OrderByDescending(x => x.TotalValue)
                .Take(5)
                .ToList();

            // Top 5 suppliers (from DB — lighter than joining in memory)
            DataTable suppDt = _dll.GetTopSuppliersByValue(branchId);
            var topSupp = new List<SupplierValueItem>();
            decimal suppTotal = suppDt.Rows.Count > 0
                ? suppDt.AsEnumerable().Sum(r => SafeDecimal(r, "total_value"))
                : 0;

            foreach (DataRow r in suppDt.Rows)
            {
                decimal val = SafeDecimal(r, "total_value");
                topSupp.Add(new SupplierValueItem
                {
                    SupplierName = r["supplier_name"].ToString(),
                    TotalValue   = val,
                    Percentage   = suppTotal > 0 ? (double)(val / suppTotal * 100) : 0
                });
            }

            return new InventoryValuationSummary
            {
                TotalInventoryValue  = totalValue,
                TotalSKUs            = lines.Count,
                TotalQty             = totalQty,
                AvgCostPerUnit       = totalQty > 0 ? totalValue / totalQty : 0,
                TopCategoriesByValue = topCats,
                TopSuppliersByValue  = topSupp
            };
        }

        // -----------------------------------------------------------------------
        // COGS
        // -----------------------------------------------------------------------

        public List<COGSLine> GetCOGSLines(
            DateTime fromDate, DateTime toDate, int branchId, string method)
        {
            DataTable raw    = _dll.GetCOGSReport(fromDate, toDate, branchId, method);
            DataTable salesV = _dll.GetSalesValueByProduct(fromDate, toDate, branchId);

            var svDict = BuildIntDict(salesV, "product_id",
                r => SafeDecimal(r, "sales_value"));

            var lines = new List<COGSLine>(raw.Rows.Count);

            foreach (DataRow row in raw.Rows)
            {
                int pid        = Convert.ToInt32(row["product_id"]);
                decimal cogs   = SafeDecimal(row, "cogs_value");
                decimal sv     = svDict.TryGetValue(pid, out decimal s) ? s : 0;

                lines.Add(new COGSLine
                {
                    ProductId                = pid,
                    ItemNumber               = row["item_number"].ToString(),
                    Code                     = row["code"].ToString(),
                    Name                     = row["name"].ToString(),
                    CategoryCode             = row["category_code"].ToString(),
                    OpeningQty               = SafeDecimal(row, "opening_qty"),
                    OpeningStockValue        = SafeDecimal(row, "opening_stock_value"),
                    PurchasedQty             = SafeDecimal(row, "purchased_qty"),
                    PurchasesValue           = SafeDecimal(row, "purchases_value"),
                    SoldQty                  = SafeDecimal(row, "sold_qty"),
                    CogsValue                = cogs,
                    ClosingQty               = SafeDecimal(row, "closing_qty"),
                    ClosingUnitCost          = SafeDecimal(row, "closing_unit_cost"),
                    ClosingValue             = SafeDecimal(row, "closing_value"),
                    ExpectedClosingValue     = SafeDecimal(row, "expected_closing_value"),
                    ReconciliationVariance   = SafeDecimal(row, "reconciliation_variance"),
                    SalesValue               = sv,
                    GrossMargin              = sv - cogs
                });
            }

            return lines;
        }

        public int PostCOGSEntry(COGSPostingRequest request)
        {
            if (request == null) throw new ArgumentNullException("request");
            if (request.TotalCogs <= 0)
                throw new ArgumentException("COGS total must be greater than zero.");
            if (request.CogsAccountId <= 0 || request.InventoryAccountId <= 0)
                throw new ArgumentException("Both COGS and Inventory accounts must be configured in Valuation Settings.");

            try { return _dll.PostCOGSJournalEntry(request); }
            catch { throw; }
        }

        // -----------------------------------------------------------------------
        // Snapshot
        // -----------------------------------------------------------------------

        public DataTable TakeSnapshot(DateTime snapshotDate, int branchId, string method)
        {
            try { return _dll.TakeSnapshot(snapshotDate, branchId, method); }
            catch { throw; }
        }

        // -----------------------------------------------------------------------
        // Negative stock alert
        // -----------------------------------------------------------------------

        public DataTable GetNegativeStockAlert(int branchId)
        {
            try { return _dll.GetNegativeStockAlert(branchId); }
            catch { throw; }
        }

        // -----------------------------------------------------------------------
        // Status badge logic
        // -----------------------------------------------------------------------

        private static string DetermineStatus(
            decimal qty, decimal unitCost,
            string categoryCode,
            Dictionary<string, decimal> catAvgDict)
        {
            if (qty < 0) return "NegativeStock";
            if (qty > 0 && unitCost == 0) return "ZeroCost";

            if (catAvgDict.TryGetValue(categoryCode, out decimal catAvg) && catAvg > 0)
            {
                if (unitCost > catAvg * 1.5m) return "HighCost";
            }

            return "Normal";
        }

        // -----------------------------------------------------------------------
        // Private helpers
        // -----------------------------------------------------------------------

        private static decimal SafeDecimal(DataRow r, string col)
        {
            object v = r[col];
            return (v == null || v == DBNull.Value) ? 0m : Convert.ToDecimal(v);
        }

        private static DateTime? SafeNullDate(DataRow r, string col)
        {
            object v = r[col];
            return (v == null || v == DBNull.Value) ? (DateTime?)null : Convert.ToDateTime(v);
        }

        private static Dictionary<int, T> BuildIntDict<T>(
            DataTable dt, string keyCol, Func<DataRow, T> selector)
        {
            var dict = new Dictionary<int, T>(dt.Rows.Count);
            foreach (DataRow row in dt.Rows)
            {
                object k = row[keyCol];
                if (k == null || k == DBNull.Value) continue;
                int key = Convert.ToInt32(k);
                if (!dict.ContainsKey(key))
                    dict[key] = selector(row);
            }
            return dict;
        }
    }
}
