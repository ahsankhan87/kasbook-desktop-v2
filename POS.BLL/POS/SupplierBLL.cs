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
    public class SupplierBLL
    {
        public string NormalizeSupplierCodeInput(string input)
        {
            SupplierDLL objDLL = new SupplierDLL();
            return objDLL.NormalizeSupplierCodeInput(input);
        }

        public string getNextSupplierCode()
        {
            SupplierDLL objDLL = new SupplierDLL();
            return objDLL.GetNextSupplierCode();
        }

        public DataTable GetAll()
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetAll();
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecord(String condition)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordBySupplierID(int Supplier_id)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.SearchRecordBySupplierID(Supplier_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetSupplierAccountBalance(int Supplier_id)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierAccountBalance(Supplier_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetPendingSupplierInvoices(int supplierId)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetPendingSupplierInvoices(supplierId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierDashboardKPIs()
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierDashboardKPIs();
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierSummaryDashboard(string searchText = null, string category = null, string statusFilter = "All")
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierSummaryDashboard(searchText, category, statusFilter);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierRecentBills(int supplierId, int top = 5)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierRecentBills(supplierId, top);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierProfileOverview(int supplierId)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierProfileOverview(supplierId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierMonthlyPurchaseHistory(int supplierId, int months = 12)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierMonthlyPurchaseHistory(supplierId, months);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierTopItems(int supplierId, int top = 5)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierTopItems(supplierId, top);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierLedger(int supplierId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierLedger(supplierId, fromDate, toDate);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierOutstandingBills(int supplierId)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierOutstandingBills(supplierId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierPayableAgingSummary(int supplierId)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierPayableAgingSummary(supplierId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierTopProductsByValue(int supplierId, int top = 10)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierTopProductsByValue(supplierId, top);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierMonthlySpendTrend(int supplierId, int months = 24)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierMonthlySpendTrend(supplierId, months);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierProductsForPriceHistory(int supplierId)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierProductsForPriceHistory(supplierId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetSupplierProductPriceHistory(int supplierId, string itemNumber)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierProductPriceHistory(supplierId, itemNumber);
            }
            catch
            {
                throw;
            }
        }

        public int Insert(SupplierModal obj)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(SupplierModal obj)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int SupplierId)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.Delete(SupplierId);
            }
            catch
            {

                throw;
            }
        }

        public int DeletePaymentTransaction(string invoiceNo)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.DeletePaymentTransaction(invoiceNo);
            }
            catch
            {
                throw;
            }
        }

        public decimal GetSupplierOpeningBalance(int supplierId)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.GetSupplierOpeningBalance(supplierId);
            }
            catch
            {
                throw;
            }
        }

        public bool IsSupplierCodeExists(string Supplier_code, int? excludeSupplierId = null)
        {
            try
            {
                SupplierDLL objDLL = new SupplierDLL();
                return objDLL.IsSupplierCodeExists(Supplier_code, excludeSupplierId);
            }
            catch
            {
                throw;
            }
        }
    }
}
