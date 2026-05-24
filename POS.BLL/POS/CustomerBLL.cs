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
    public class CustomerBLL
    {
        public string NormalizeCustomerCodeInput(string input)
        {
            CustomerDLL objDLL = new CustomerDLL();
            return objDLL.NormalizeCustomerCodeInput(input);
        }

        public string GetNextCustomerCode()
        {
            CustomerDLL objDLL = new CustomerDLL();
            return objDLL.GetNextCustomerCode();
        }

        public DataTable GetAll()
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetAll();
            }
            catch
            {

                throw;
            }
        }

        /// <summary>
        /// Generic Search from database
        /// </summary>
        /// <param name="condition">i.e. first_name="Ahsan"</param>
        /// <returns>DataTable</returns>
        public DataTable SearchRecord(String condition)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByCustomerID(int Customer_id)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.SearchRecordByCustomerID(Customer_id);
            }
            catch
            {

                throw;
            }
        }

        public Decimal GetCustomerAccountBalance(int Customer_id)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetCustomerAccountBalance(Customer_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetPendingCustomerInvoices(int customerId)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetPendingCustomerInvoices(customerId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCustomerSummaryDashboard()
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetCustomerSummaryDashboard();
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCustomerRecentTransactions(int customerId, int top = 5)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetCustomerRecentTransactions(customerId, top);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCustomerProfileOverview(int customerId)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetCustomerProfileOverview(customerId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCustomerMonthlyPurchaseHistory(int customerId, int months = 12)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetCustomerMonthlyPurchaseHistory(customerId, months);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCustomerLedger(int customerId, DateTime fromDate, DateTime toDate)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetCustomerLedger(customerId, fromDate, toDate);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCustomerOutstandingInvoices(int customerId)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetCustomerOutstandingInvoices(customerId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCustomerAgingSummary(int customerId)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetCustomerAgingSummary(customerId);
            }
            catch
            {
                throw;
            }
        }

        public DataTable GetCustomerNotes(int customerId)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.GetCustomerNotes(customerId);
            }
            catch
            {
                throw;
            }
        }

        public int AddCustomerNote(int customerId, string noteText)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.AddCustomerNote(customerId, noteText);
            }
            catch
            {
                throw;
            }
        }

        public int UpdateCustomerStatus(int customerId, bool isActive)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.UpdateCustomerStatus(customerId, isActive);
            }
            catch
            {
                throw;
            }
        }

        public int Insert(CustomerModal obj)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(CustomerModal obj)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int CustomerId)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.Delete(CustomerId);
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
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.DeletePaymentTransaction(invoiceNo);
            }
            catch
            {
                throw;
            }
        }

        public bool IsCustomerCodeExists(string CustomerCode, int? excludeCustomerId = null)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.IsCustomerCodeExists(CustomerCode, excludeCustomerId);
            }
            catch
            {
                throw;
            }
        }

        public bool IsCustomerDuplicate(string firstName, string vatNo, string registrationName, int? excludeCustomerId = null)
        {
            try
            {
                CustomerDLL objDLL = new CustomerDLL();
                return objDLL.IsCustomerDuplicate(firstName, vatNo, registrationName, excludeCustomerId);
            }
            catch
            {
                throw;
            }
        }
    }
}
