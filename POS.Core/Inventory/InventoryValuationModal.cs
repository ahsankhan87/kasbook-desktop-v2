using System;
using System.Collections.Generic;

namespace POS.Core.Inventory
{
    // -------------------------------------------------------------------------
    // Valuation Settings
    // -------------------------------------------------------------------------

    public class InventoryValuationSettings
    {
        public int Id { get; set; }
        public int BranchId { get; set; }

        /// <summary>WAC | FIFO | STANDARD</summary>
        public string ValuationMethod { get; set; } = "WAC";

        /// <summary>PURCHASE_ONLY | WITH_LANDED</summary>
        public string CostComponents { get; set; } = "PURCHASE_ONLY";

        /// <summary>ALL | ACTIVE_ONLY | EXCLUDE_ZERO</summary>
        public string IncludeFilter { get; set; } = "ACTIVE_ONLY";

        public int? CogsAccountId { get; set; }
        public string CogsAccountName { get; set; }
        public string CogsAccountCode { get; set; }

        public int? InventoryAccountId { get; set; }
        public string InventoryAccountName { get; set; }
        public string InventoryAccountCode { get; set; }

        /// <summary>false = single summary JV; true = one JV line per product</summary>
        public bool PostPerProduct { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }

    // -------------------------------------------------------------------------
    // Valuation Line (one row in the report grid)
    // -------------------------------------------------------------------------

    public class InventoryValuationLine
    {
        public int ProductId { get; set; }
        public string ItemNumber { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CategoryCode { get; set; }
        public string BrandCode { get; set; }
        public string UnitName { get; set; }

        public decimal CurrentQty { get; set; }
        public decimal UnitCost { get; set; }
        public decimal TotalValue { get; set; }

        public decimal AvgCost { get; set; }
        public decimal LastCost { get; set; }
        public decimal FifoCost { get; set; }
        public decimal StandardCost { get; set; }

        public DateTime? LastPurchaseDate { get; set; }
        public decimal LastPurchaseCost { get; set; }
        public decimal ReorderLevel { get; set; }

        public string EffectiveMethod { get; set; }

        /// <summary>Normal | HighCost | ZeroCost | NegativeStock</summary>
        public string ValuationStatus { get; set; }
    }

    // -------------------------------------------------------------------------
    // Valuation Summary
    // -------------------------------------------------------------------------

    public class InventoryValuationSummary
    {
        public decimal TotalInventoryValue { get; set; }
        public int TotalSKUs { get; set; }
        public decimal TotalQty { get; set; }
        public decimal AvgCostPerUnit { get; set; }

        /// <summary>Category code → total value (top 5)</summary>
        public List<CategoryValueItem> TopCategoriesByValue { get; set; } = new List<CategoryValueItem>();

        /// <summary>Supplier name → total value (top 5)</summary>
        public List<SupplierValueItem> TopSuppliersByValue { get; set; } = new List<SupplierValueItem>();
    }

    public class CategoryValueItem
    {
        public string CategoryCode { get; set; }
        public string CategoryName { get; set; }
        public decimal TotalValue { get; set; }
        public double Percentage { get; set; }
    }

    public class SupplierValueItem
    {
        public string SupplierName { get; set; }
        public decimal TotalValue { get; set; }
        public double Percentage { get; set; }
    }

    // -------------------------------------------------------------------------
    // COGS Line (one row in the COGS report grid)
    // -------------------------------------------------------------------------

    public class COGSLine
    {
        public int ProductId { get; set; }
        public string ItemNumber { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string CategoryCode { get; set; }

        public decimal OpeningQty { get; set; }
        public decimal OpeningStockValue { get; set; }

        public decimal PurchasedQty { get; set; }
        public decimal PurchasesValue { get; set; }

        public decimal SoldQty { get; set; }
        public decimal CogsValue { get; set; }

        public decimal ClosingQty { get; set; }
        public decimal ClosingUnitCost { get; set; }
        public decimal ClosingValue { get; set; }

        public decimal ExpectedClosingValue { get; set; }
        public decimal ReconciliationVariance { get; set; }

        /// <summary>Revenue - COGS (requires sales value lookup)</summary>
        public decimal GrossMargin { get; set; }
        public decimal SalesValue { get; set; }
        public double GrossMarginPct => SalesValue > 0 ? (double)(GrossMargin / SalesValue) * 100 : 0;
    }

    // -------------------------------------------------------------------------
    // COGS Posting request
    // -------------------------------------------------------------------------

    public class COGSPostingRequest
    {
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int CogsAccountId { get; set; }
        public int InventoryAccountId { get; set; }
        public decimal TotalCogs { get; set; }
        public bool PostPerProduct { get; set; }
        public List<COGSLine> Lines { get; set; } = new List<COGSLine>();
        public string Narration { get; set; }
    }
}
