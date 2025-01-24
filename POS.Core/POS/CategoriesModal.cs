using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class CategoriesModal
    {
        public int id { get; set; }

        public string name { get; set; }

        public string code{ get; set; }

        public int branch_id { get; set; }

        public int user_id{ get; set; }

        public string date_created { get; set; }

        public string date_updated { get; set; }
    }
}
