using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class ICTModal
    {
        public DateTime transfer_date { get; set; }
        public DateTime release_date { get; set; }

        public int destination_branch_id{ get; set; }
        public int source_branch_id { get; set; }
        public double quantity { get; set; }
        public double release_qty { get; set; }
        public double requested_qty { get; set; }
        public bool status { get; set; }
        public string item_code { get; set; }



    }

}
