using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class Purchases_orderModal
    {
        
        public int id { get; set; }

        

        public string item_type { get; set; }

        public string description { get; set; }



        public int supplier_id { get; set; }

        public string invoice_no { get; set; }

        public string purchase_type { get; set; }

        public double total_amount { get; set; }

        public double total_tax { get; set; }
        
        public double total_discount { get; set; }

        public string purchase_date { get; set; }
        
        public DateTime purchase_time { get; set; }

        public string account { get; set; }

        public int employee_id { get; set; }

        public string supplier_invoice_no { get; set; }

        public string category_code { get; set; }

        public string delivery_date { get; set; }
    }
    public class PurchaseOrderDetailModal
    {
        public string invoice_no { get; set; }
        public int item_id { get; set; }

        public int purchase_id { get; set; }

        public double quantity { get; set; }
        public int supplier_id { get; set; }
        public string purchase_date { get; set; }

        public double discount { get; set; }

        public int tax_id { get; set; }

        public double tax_rate { get; set; }

        public string status { get; set; }

        public double cost_price { get; set; }
        public int serialNo { get; set; }

        public double avg_cost { get; set; }

        public double unit_price { get; set; }

        public string code { get; set; }

        public string name { get; set; }
    }
}
