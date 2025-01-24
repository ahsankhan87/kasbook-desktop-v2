using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class BankModal
    {
        public int id { get; set; }

        public string code { get; set; }
        
        public string name { get; set; }
        
        public string accountNo { get; set; }
        
        public string holderName { get; set; }
        
        public string bankBranch{ get; set; }
        
        public string GLAccountID { get; set; }
        
        public int branch_id { get; set; }

        public DateTime  date_created { get; set; }

        public DateTime date_updated { get; set; }

        public int user_id{ get; set; }

       

    }
}
