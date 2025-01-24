using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class AccountsModal
    {
        public int id { get; set; }

        public int user_id { get; set; }

        public string name { get; set; }
        
        public string name_2 { get; set; }
        
        public string code { get; set; }

        public string description { get; set; }

        public double op_dr_balance { get; set; }

        public double op_cr_balance { get; set; }
        
        public int group_id { get; set; }

        public string date_created { get; set; }

        public string date_updated { get; set; }
    }
}
