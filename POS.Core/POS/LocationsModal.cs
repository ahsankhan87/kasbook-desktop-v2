using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class LocationsModal
    {
        public int id { get; set; }

        public string name { get; set; }
        public string code { get; set; }

        public int branch_id { get; set; }

        public int user_id{ get; set; }

        public DateTime date_created { get; set; }

        public DateTime date_updated { get; set; }
    }
}
