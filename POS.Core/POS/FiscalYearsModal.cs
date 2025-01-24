using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class FiscalYearModal
    {
        public int id { get; set; }

        public string name { get; set; }

        public string code { get; set; }

        public DateTime from_date { get; set; }
        
        public DateTime to_date { get; set; }

        public DateTime date_created { get; set; }

        public DateTime date_updated { get; set; }
    }
}
