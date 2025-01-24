using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class ExpenseModal_Header
    {
        public int id { get; set; }

        public int user_id { get; set; }
        public int branch_id { get; set; }

        public string cash_account { get; set; }
        public string invoice_no { get; set; }

        public string vat_account { get; set; }
        
        public DateTime sale_date { get; set; }

       
        public string date_created { get; set; }

        public string date_updated { get; set; }
        public string description { get; set; }

        public double amount { get; set; }

        public double vat { get; set; }

        public double vat_amount { get { return amount * vat / 100; } }

        public double total_amount { get { return amount + vat_amount; } }

        public string expense_account { get; set; }
        public string expense_account_name { get; set; }
    }

    public class ExpenseModal_Detail
    {
       
    }
}
