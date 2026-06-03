using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class SalesModalHeader
    {
        public DateTime sale_date { get; set; }

        public DateTime sale_time { get; set; }

        public string account { get; set; }

        public bool is_return { get; set; }

        public string returnReasonCode { get; set; }
        public string returnReason { get; set; }
        
        public int customer_id { get; set; }
        public string customer_name { get; set; }
        public string customer_vat { get; set; }

        public string invoice_no { get; set; }

        public string sale_type { get; set; }
        public string invoice_subtype { get; set; }

        public double total_amount { get; set; }
        public double total_cost_amount { get; set; }

        public double total_tax { get; set; }

        public double total_discount { get; set; }

        public double total_discount_percent { get; set; }
        public int employee_id { get; set; }
        public int payment_terms_id { get; set; }

        public int payment_method_id { get; set; }
        public string description { get; set; }
        public bool estimate_status { get; set; }

        public string estimate_invoice_no { get; set; }

        public int cash_account_id { get; set; }
        public int receivable_account_id { get; set; }
        public int tax_account_id { get; set; }
        public int sales_discount_acc_id { get; set; }
        public int sales_account_id { get; set; }
        public int purchases_acc_id { get; set; }
        public int inventory_acc_id { get; set; }

        public string old_invoice_no { get; set; }
        public DateTime previousInvoiceDate { get; set; }


        public double flat_discount_value { get; set; }
        public string payment_method_text{get; set;}
        public int bank_id{get; set;}
        public string bankGLAccountID{get; set; }
        public string PONumber { get; set; }
        
    }
    public class SalesModal
    {
        
        /// <summary> header
        public DateTime sale_date { get; set; }

        public DateTime sale_time { get; set; }

        public string account { get; set; }

        public bool is_return { get; set; }
        public int customer_id { get; set; }

        public string invoice_no { get; set; }

        public string sale_type { get; set; }

        public double total_amount { get; set; }

        public double total_tax { get; set; }

        public double total_discount { get; set; }

        public double total_discount_percent { get; set; }
        public int employee_id { get; set; }
        public int payment_terms_id { get; set; }

        public int payment_method_id { get; set; }
        public string description { get; set; }
        public bool estimate_status { get; set; }

        public string estimate_invoice_no { get; set; }

        public string old_invoice_no { get; set; }

        /// </summary>
        /// header

        public int serialNo { get; set; }

        public int id { get; set; }

        public string code { get; set; }
        public string item_number { get; set; }

        public string name { get; set; }

        public string item_type { get; set; }

        
        public string status { get; set; }

        public double cost_price { get; set; }

        public double avg_cost { get; set; }

        public double unit_price { get; set; }

        public double sub_total { get { return unit_price * quantity_sold - discount; } }

        
        public int item_id { get; set; }

        public int sale_id { get; set; }

        public double quantity_sold{ get; set; }

        public double discount { get; set; }
        public double discount_percent { get; set; }

        public int tax_id { get; set; }

        public double tax_rate { get; set; }
        public int destination_branch_id { get; set; }

        public int source_branch_id { get; set; }

        public string location_code { get; set; }

        
        public double packet_qty { get; set; }

    }

    public sealed class FinanceSummaryDto
    {
        public decimal SalesAmount { get; set; }
        public decimal SalesTax { get; set; }
        public decimal SalesNet { get; set; }
        public decimal ExpensesTotal { get; set; }
        public decimal Profit => SalesNet - ExpensesTotal;
    }

    public class AdjSessionModel
    {
        public int AdjId { get; set; }
        public string AdjNo { get; set; }
        public DateTime AdjDate { get; set; }
        public string AdjType { get; set; }
        public int WarehouseId { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public int? PostedBy { get; set; }
        public DateTime? PostedAt { get; set; }
        public DateTime? ModifiedAt { get; set; }
        public List<AdjSessionLineModel> Lines { get; set; }

        public AdjSessionModel()
        {
            Lines = new List<AdjSessionLineModel>();
        }
    }

    public class AdjSessionLineModel
    {
        public int LineId { get; set; }
        public int AdjId { get; set; }
        public int ProductId { get; set; }
        public decimal SystemQty { get; set; }
        public decimal PhysicalQty { get; set; }
        public decimal QtyDifference { get; set; }
        public decimal CurrentSalePrice { get; set; }
        public decimal NewSalePrice { get; set; }
        public string CurrentLocation { get; set; }
        public string NewLocation { get; set; }
        public string Reason { get; set; }
        public string Notes { get; set; }
        public bool IsVerified { get; set; }
    }

    public class AdjSessionCreateResult
    {
        public int AdjId { get; set; }
        public string AdjNo { get; set; }
    }

    public class AdjSessionSummaryModel
    {
        public int TotalLines { get; set; }
        public decimal QtyIncrease { get; set; }
        public decimal QtyDecrease { get; set; }
        public int PriceChangeProducts { get; set; }
        public int RelocatedProducts { get; set; }
        public decimal TotalStockValueImpact { get; set; }
    }

    public class AdjAuditRow
    {
        public int AuditId { get; set; }
        public string AdjNo { get; set; }
        public DateTime ChangedAt { get; set; }
        public string ChangeType { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string ChangedBy { get; set; }
        public string Reason { get; set; }
        public string AdjType { get; set; }
    }

    public class AdjSessionListRow
    {
        public int AdjId { get; set; }
        public string AdjNo { get; set; }
        public string AdjDate { get; set; }
        public string AdjType { get; set; }
        public string Status { get; set; }
        public int ProductCount { get; set; }
        public int QtyIncreases { get; set; }
        public int QtyDecreases { get; set; }
        public int PriceChanges { get; set; }
        public int LocationChanges { get; set; }
        public string CreatedBy { get; set; }
        public string PostedBy { get; set; }
        public string Notes { get; set; }
    }

    public class StockVarianceRow
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public int TotalAdjustments { get; set; }
        public decimal TotalQtyAdjusted { get; set; }
        public decimal TotalValueImpact { get; set; }
        public DateTime LastAdjustmentDate { get; set; }
    }

    public class PriceChangeRow
    {
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string CategoryName { get; set; }
        public string AdjNo { get; set; }
        public string ChangeDate { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public decimal PctChange { get; set; }
        public string ApprovedBy { get; set; }
        public string Reason { get; set; }
    }

    public class AdjPostResult
    {
        public bool Success { get; set; }
        public int AffectedRows { get; set; }
        public string ErrorMessage { get; set; }
    }
}
