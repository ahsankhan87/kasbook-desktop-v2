using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.DLL;
using System.Data;
using POS.Core;

namespace POS.BLL
{
    public class SalesReportBLL
    {
        public DataTable InvoiceReport(string from_date, string to_date, string customer = "", string invoice_no = "",
            double total_amount = 0, int branch_id = 0)
        {
            try
            {
                SalesReportDLL objDLL = new SalesReportDLL();
                return objDLL.InvoiceReport(from_date, to_date, customer, invoice_no, total_amount, branch_id);
            }
            catch
            {

                throw;
            }
        }
        
        public DataTable SaleReport(DateTime from_date, DateTime to_date, 
            int customer_id = 0, string product_code = "", string sale_type = "",int employee_id=0,string sale_account="", int branch_id = 0)
        {
            try
            {
                SalesReportDLL objDLL = new SalesReportDLL();
                return objDLL.SaleReport(from_date, to_date, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);
            }
            catch
            {
                
                throw;
            }
        }

        public DataTable CusomerWiseSaleReport(string from_date, string to_date,
           int customer_id = 0, string product_code = "", string sale_type = "", int employee_id = 0, string sale_account = "", int branch_id = 0)
        {
            try
            {
                SalesReportDLL objDLL = new SalesReportDLL();
                return objDLL.CusomerWiseSaleReport(from_date, to_date, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable ProductWiseSaleReport(string from_date, string to_date,
           int customer_id = 0, string product_code = "", string sale_type = "", int employee_id = 0, string sale_account = "", int branch_id = 0)
        {
            try
            {
                SalesReportDLL objDLL = new SalesReportDLL();
                return objDLL.ProductWiseSaleReport(from_date, to_date, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable categoryWiseSaleReport(string from_date, string to_date,
           int customer_id = 0, string product_code = "", string sale_type = "", int employee_id = 0, string sale_account = "", int branch_id = 0)
        {
            try
            {
                SalesReportDLL objDLL = new SalesReportDLL();
                return objDLL.categoryWiseSaleReport(from_date, to_date, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetBranchSummary()
        {
            try
            {
                SalesReportDLL objDLL = new SalesReportDLL();
                return objDLL.GetBranchSummary();
            }
            catch
            {

                throw;
            }
        }

    }
}
