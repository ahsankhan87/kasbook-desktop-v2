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
    }
}
