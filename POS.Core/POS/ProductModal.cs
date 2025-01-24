using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class ProductModal
    {
        public int id { get; set; }

        public int tax_id { get; set; }

        public string code { get; set; }

        public string group_code { get; set; }

        public string item_number { get; set; }

        public string alt_item_number { get; set; }
        
        public string origin { get; set; }

        public string name { get; set; }

        public string name_ar { get; set; }
        
        public string item_type { get; set; }
        
        public string description { get; set; }
        
        public bool status { get; set; }
        
        public double cost_price { get; set; }

        public double qty { get; set; }

        public double adjustment_qty { get; set; }

        public double avg_cost { get; set; }

        public double unit_price { get; set; }

        public double unit_price_2 { get; set; }

        public string date_created { get; set; }

        public string date_updated { get; set; }

        public int unit_id { get; set; }

        public string category { get; set; }
        public int category_id { get; set; }
        public string category_code { get; set; }

        public string brand_code { get; set; }

        public string location_code { get; set; }
        public string from_location_code { get; set; }

        public string barcode { get; set; }

        public decimal demand_qty { get; set; }

        public decimal purchase_demand_qty { get; set; }

        public decimal sale_demand_qty { get; set; }

        public decimal re_stock_level { get; set; }

        public int sales_acc_id { get; set; }
        public int inventory_acc_id { get; set; }
        public int cos_acc_id { get; set; }
        public int sales_return_acc_id { get; set; }
        public int sales_discount_acc_id { get; set; }

        public string invoice_no { get; set; }

        public byte[] picture { get; set; }

        public DateTime expiry_date { get; set; }

        public int supplier_id { get; set; }
        public int alt_no { get; set; } // alternate part number
        public decimal packet_qty { get; set; }

    }
}
