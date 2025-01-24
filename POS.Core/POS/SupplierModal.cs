using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class SupplierModal
    {
        public int id { get; set; }

        public string first_name { get; set; }
        
        public string last_name { get; set; }
        
        public string email { get; set; }
        
        public string address { get; set; }
        
        public string status { get; set; }
        
        public string contact_no { get; set; }

        public int branch_id { get; set; }
        
        public int cash_acc_id { get; set; }
        
        public int payable_acc_id { get; set; }

        public string vat_no { get; set; }

        public string date_created { get; set; }

        public string date_updated { get; set; }

        public bool vat_with_status { get; set; }

    }
}
