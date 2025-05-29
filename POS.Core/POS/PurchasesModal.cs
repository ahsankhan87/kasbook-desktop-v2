using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class PurchaseModalHeader
    {
        public int supplier_id { get; set; }

        public string invoice_no { get; set; }

        public string purchase_type { get; set; }

        public decimal total_amount { get; set; }

        public decimal total_tax { get; set; }

        public decimal total_discount { get; set; }

        public decimal total_discount_percent { get; set; }
        public string supplier_invoice_no { get; set; }
        public string description { get; set; }

        public bool po_status { get; set; }

        public string po_invoice_no { get; set; }
        public DateTime purchase_date { get; set; }

        public DateTime purchase_time { get; set; }
        public int employee_id { get; set; }
        public string account { get; set; }

        public decimal shipping_cost { get; set; }

        public int cash_account_id { get; set; }
        public int payable_account_id { get; set; }
        public int tax_account_id { get; set; }
        public int purchases_discount_acc_id { get; set; }
        public int inventory_acc_id { get; set; }
        public int purchases_acc_id { get; set; }
    }

    public class PurchasesModal
    {
        /// <summary> header
        public int supplier_id { get; set; }

        public string invoice_no { get; set; }

        public string purchase_type { get; set; }

        public decimal total_amount { get; set; }

        public decimal total_tax { get; set; }

        public decimal total_discount { get; set; }
        public string account { get; set; }

        public decimal total_discount_percent { get; set; }
        public string supplier_invoice_no { get; set; }
        public string description { get; set; }

        public bool po_status { get; set; }

        public string po_invoice_no { get; set; }
        
        public int employee_id { get; set; }

        public decimal shipping_cost { get; set; }
        /// </summary>
        /// 

        public int serialNo { get; set; }

        public int id { get; set; }

        public string code { get; set; }

        public string item_number { get; set; }

        public string name { get; set; }

        public string item_type { get; set; }

        public string status { get; set; }

        public decimal cost_price { get; set; }

        public decimal avg_cost { get; set; }

        public decimal unit_price { get; set; }

        public int item_id { get; set; }

        public int purchase_id { get; set; }

        public decimal quantity{ get; set; }

        public decimal discount { get; set; }

        public int tax_id { get; set; }

        public decimal tax_rate { get; set; }

        public DateTime purchase_date { get; set; }
        
        public DateTime purchase_time { get; set; }

        
        public string location_code { get; set; }

        public decimal packet_qty { get; set; }


    }
}
