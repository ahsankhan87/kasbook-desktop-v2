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
}
