using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class EstimatesModal
    {
        public int id { get; set; }

        public string code { get; set; }

        public string name { get; set; }

        public string item_type { get; set; }

        public string description { get; set; }

        public string status { get; set; }

        public double cost_price { get; set; }

        public double avg_cost { get; set; }

        public double unit_price { get; set; }

        public double sub_total { get { return unit_price * quantity_sold - discount; } }

        public int customer_id { get; set; }

        public string invoice_no { get; set; }

        public string sale_type { get; set; }

        public double total_amount { get; set; }
        
        public double total_tax { get; set; }
        
        public double total_discount { get; set; }

        public int item_id { get; set; }

        public int sale_id { get; set; }

        public double quantity_sold{ get; set; }

        public double discount { get; set; }

        public int tax_id { get; set; }

        public double tax_rate { get; set; }

        public string sale_date { get; set; }

        public DateTime sale_time { get; set; }

        public string account { get; set; }

        public bool is_return { get; set; }

        public string old_invoice_no { get; set; }

        public int employee_id { get; set; }

    }
}
