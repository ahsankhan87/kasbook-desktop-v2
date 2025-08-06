using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class CompaniesModal
    {
        public int id { get; set; }

        public string name { get; set; }

        public string address { get; set; }
        
        public string contact_no{ get; set; }
        
        public string vat_no { get; set; }
        
        public string email { get; set; }
        
        public int currency_id { get; set; }
        
        public string image{ get; set; }

        public int branch_id { get; set; }

        public int user_id { get; set; }
        
        public int locked { get; set; }

        public bool useZatcaEInvoice { get; set; }

        public DateTime expiry_date { get; set; }

        public string date_created { get; set; }

        public string date_updated { get; set; }

        public int sales_acc_id { get; set; }
        public int inventory_acc_id { get; set; }
        public int purchases_acc_id { get; set; }
        public int sales_return_acc_id { get; set; }
        public int sales_discount_acc_id { get; set; }
        public int cash_acc_id { get; set; }
        public int receivable_acc_id { get; set; }
        public int tax_acc_id { get; set; }
        public int purchases_return_acc_id { get; set; }
        public int purchases_discount_acc_id { get; set; }

        public int payable_acc_id { get; set; }
        public int item_variance_acc_id { get; set; }
        public int commission_acc_id { get; set; }
        public string systemID { get; set; }
        public string subscriptionKey { get; set; }

    }
}
