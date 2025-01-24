using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class JournalsModal
    {
        public int id { get; set; }

        public int user_id { get; set; }

        public int entry_id { get; set; }
        
        public int customer_id { get; set; }
        public int bank_id { get; set; }

        public int supplier_id { get; set; }

        public string invoice_no { get; set; }

        public int account_id { get; set; }
        
        public string account_name { get; set; }
        
        public string code { get; set; }

        public string description { get; set; }

        public double debit { get; set; }

        public double credit { get; set; }
        
        public string date_created { get; set; }

        public string date_updated { get; set; }

        public DateTime entry_date { get; set; }

        public int employee_id { get; set; }

    }
}
