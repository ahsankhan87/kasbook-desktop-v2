using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.Core
{
    public class UsersModal
    {
        public static int logged_in_user_id;

        ///
        public static int logged_in_userid { get; set; }

        public static string logged_in_username { get; set; }

        public static int logged_in_branch_id { get; set; }
        public static string logged_in_branch_name { get; set; }

        public static int loggedIncompanyID { get; set; }

        public static string logged_in_lang { get; set; }
        public static string logged_in_user_role { get; set; }
        public static int logged_in_user_level { get; set; }
       
        public static DateTime fy_from_date { get; set; }
        public static DateTime fy_to_date { get; set; }
        public static string fiscal_year { get; set; }

        ///

        public int id { get; set; }

        public string name { get; set; }

        public int branch_id { get; set; }

        public string username { get; set; }
        
        public string password { get; set; }

        public string date_created { get; set; }

        public string date_updated { get; set; }

        public string module_name { get; set; }

        public int module_id { get; set; }

        public int user_id { get; set; }

        public string language { get; set; }
        
        public string user_role { get; set; }
        
        public int user_level { get; set; }

        ///user rights
        public double cash_sales_amount_limit { get; set; }
        public double credit_sales_amount_limit { get; set; }
        public double cash_purchase_amount_limit { get; set; }
        public double credit_purchase_amount_limit { get; set; }

        public bool allow_cash_sales { get; set; }
        public bool allow_credit_sales { get; set; }
        public bool allow_cash_purchase { get; set; }
        public bool allow_credit_purchase { get; set; }

        ///

        public int user_commission_percent { get; set; }

        public int companyID { get; set; }

    }
}
