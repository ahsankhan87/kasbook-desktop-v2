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
    public class PurchasesReportBLL
    {
        public DataTable PurchaseReport(DateTime from_date, DateTime to_date, int customer_id = 0, int product_id = 0, string Purchase_type = "",int employee_id=0, int branch_id = 0)
        {
            try
            {
                PurchasesReportDLL objDLL = new PurchasesReportDLL();
                return objDLL.PurchaseReport(from_date, to_date, customer_id, product_id, Purchase_type, employee_id, branch_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetPurchaseLineItems(int purchaseId, int branchId)
        {
            try
            {
                PurchasesReportDLL objDLL = new PurchasesReportDLL();
                return objDLL.GetPurchaseLineItems(purchaseId, branchId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable PurchaseInvoiceReportNew(DateTime from_date, DateTime to_date, int supplier_id = 0,
            string purchase_type = "All", int employee_id = 0, int branch_id = 0)
        {
            try
            {
                PurchasesReportDLL objDLL = new PurchasesReportDLL();
                return objDLL.PurchaseInvoiceReportNew(from_date, to_date, supplier_id, purchase_type, employee_id, branch_id);
            }
            catch
            {
                throw;
            }
        }

        public DataTable PurchaseInvoiceReport(string from_date, string to_date, string supplier = "", string supplier_inv_no = "", string invoice_no = "",
            double total_amount = 0, int branch_id = 0)
        {
            try
            {
                PurchasesReportDLL objDLL = new PurchasesReportDLL();
                return objDLL.PurchaseInvoiceReport(from_date, to_date, supplier, supplier_inv_no, invoice_no, total_amount, branch_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable Hold_PurchaseInvoiceReport(string from_date, string to_date, string supplier = "", string supplier_inv_no = "", string invoice_no = "",
            double total_amount = 0, int branch_id = 0)
        {
            try
            {
                PurchasesReportDLL objDLL = new PurchasesReportDLL();
                return objDLL.Hold_PurchaseInvoiceReport(from_date, to_date, supplier, supplier_inv_no, invoice_no, total_amount, branch_id);
            }
            catch
            {

                throw;
            }
        }
    }
}
