using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class TaxModal
    {
        public int id { get; set; }

        public string title { get; set; }
        
        public double rate { get; set; }
        
        public string status { get; set; }

        public int branch_id { get; set; }

        public int tax_acc_id { get; set; }

        public string date_created { get; set; }

        public string date_updated { get; set; }
    }
}
