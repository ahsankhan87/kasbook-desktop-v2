using System;
using System.Collections.Generic;

namespace POS.Core.Inventory
{
    // ─────────────────────────────────────────────────────────────────────────
    // WAC result for a single product
    // ─────────────────────────────────────────────────────────────────────────
    public class WACResult
    {
        public int      ProductId           { get; set; }
        public string   ItemNumber          { get; set; }
        public string   ProductName         { get; set; }
        public string   ProductCode         { get; set; }
        public int      BranchId            { get; set; }
        public decimal  TotalPurchaseQty    { get; set; }
        public decimal  TotalPurchaseValue  { get; set; }
        public decimal  TotalSalesQty       { get; set; }
        public decimal  CurrentQty          { get; set; }
        public decimal  WACUnitCost         { get; set; }   // computed WAC
        public decimal  TotalInventoryValue { get; set; }
        public decimal  StoredAvgCost       { get; set; }   // pos_products.avg_cost
        public decimal  StoredQty           { get; set; }
        public DateTime AsOfDate            { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // A single FIFO cost layer (one purchase lot)
    // ─────────────────────────────────────────────────────────────────────────
    public class FIFOCostLayer
    {
        public int      LayerId         { get; set; }
        public int      ProductId       { get; set; }
        public string   ItemNumber      { get; set; }
        public int      BranchId        { get; set; }
        public int?     PurchaseRefId   { get; set; }
        public DateTime PurchaseDate    { get; set; }
        public decimal  OriginalQty     { get; set; }
        public decimal  RemainingQty    { get; set; }
        public decimal  UnitCost        { get; set; }
        public int      CurrencyId      { get; set; }
        public decimal  ExchangeRate    { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // FIFO cost result returned to callers (e.g. SalesBLL before posting)
    // ─────────────────────────────────────────────────────────────────────────
    public class FIFOCostResult
    {
        /// <summary>Total cost for the quantity sold (sum of layer consumptions).</summary>
        public decimal TotalCost        { get; set; }

        /// <summary>Weighted average unit cost derived from the consumed layers.</summary>
        public decimal UnitCostWeighted { get; set; }

        /// <summary>Quantity requested by the caller.</summary>
        public decimal RequestedQty     { get; set; }

        /// <summary>Quantity successfully matched from available layers.</summary>
        public decimal FulfilledQty     { get; set; }

        /// <summary>
        /// True when available stock is less than requested qty
        /// (negative-stock or data-integrity issue).
        /// </summary>
        public bool    HasShortfall     { get; set; }

        /// <summary>Layer-level breakdown of the consumption.</summary>
        public List<FIFOConsumptionLine> UnitCostLayers { get; set; }
            = new List<FIFOConsumptionLine>();
    }

    /// <summary>One row in the FIFO consumption breakdown.</summary>
    public class FIFOConsumptionLine
    {
        public int      LayerId       { get; set; }
        public DateTime PurchaseDate  { get; set; }
        public decimal  ConsumedQty   { get; set; }
        public decimal  UnitCost      { get; set; }
        public decimal  LineCost      { get; set; }   // ConsumedQty × UnitCost
    }

    // ─────────────────────────────────────────────────────────────────────────
    // COGS posting request for a single sale invoice
    // (separate from COGSPostingRequest in InventoryValuationModal which is
    //  period-based for the valuation report)
    // ─────────────────────────────────────────────────────────────────────────
    public class SaleCOGSRequest
    {
        public string   SaleInvoiceNo       { get; set; }
        public int      SaleId              { get; set; }
        public int      BranchId            { get; set; }
        public int      COGSAccountId       { get; set; }
        public int      InventoryAccountId  { get; set; }
        public int      UserId              { get; set; }
        public string   CostingMethod       { get; set; }   // "WAC" | "FIFO"
        public string   JournalRef          { get; set; }   // optional override
        public List<SaleCOGSLine> Lines     { get; set; }
            = new List<SaleCOGSLine>();
    }

    /// <summary>One sale line included in a COGS posting batch.</summary>
    public class SaleCOGSLine
    {
        public int      ProductId   { get; set; }
        public string   ItemNumber  { get; set; }
        public decimal  QtySold     { get; set; }
        public decimal  UnitCost    { get; set; }
        public decimal  COGSAmount  { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Result returned after PostCOGSForSale
    // ─────────────────────────────────────────────────────────────────────────
    public class COGSPostingResult
    {
        public bool     Success      { get; set; }
        public string   JournalRef   { get; set; }
        public decimal  TotalCOGS    { get; set; }
        public string   ErrorMessage { get; set; }
    }

    // ─────────────────────────────────────────────────────────────────────────
    // Reconciliation result
    // ─────────────────────────────────────────────────────────────────────────
    public class InventoryReconciliationResult
    {
        public DateTime AsOfDate              { get; set; }
        public int      BranchId              { get; set; }
        public decimal  TotalStockValue       { get; set; }  // sum(qty × avg_cost)
        public decimal  GLInventoryBalance    { get; set; }  // acc_entries debit-credit
        public decimal  Variance              { get; set; }  // GL – StockValue
        public bool     IsBalanced            { get; set; }  // |variance| < tolerance
        public decimal  ToleranceUsed         { get; set; }
        public List<ReconciliationLine> Lines { get; set; }
            = new List<ReconciliationLine>();
    }

    public class ReconciliationLine
    {
        public int      ProductId       { get; set; }
        public string   ItemNumber      { get; set; }
        public string   ProductName     { get; set; }
        public string   ProductCode     { get; set; }
        public string   CategoryCode    { get; set; }
        public decimal  QtyOnHand       { get; set; }
        public decimal  UnitCost        { get; set; }
        public decimal  StockValue      { get; set; }
    }
}
