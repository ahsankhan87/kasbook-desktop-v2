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
    public class BankBLL
    {
        public DataTable GetAll()
        {
            try
            {
                BankDLL objDLL = new BankDLL();
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
                BankDLL objDLL = new BankDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByBankID(int Bank_id)
        {
            try
            {
                BankDLL objDLL = new BankDLL();
                return objDLL.SearchRecordByBankID(Bank_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetBankAccountBalance(int Bank_id)
        {
            try
            {
                BankDLL objDLL = new BankDLL();
                return objDLL.GetBankAccountBalance(Bank_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(BankModal obj)
        {
            try
            {
                BankDLL objDLL = new BankDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(BankModal obj)
        {
            try
            {
                BankDLL objDLL = new BankDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int BankId)
        {
            try
            {
                BankDLL objDLL = new BankDLL();
                return objDLL.Delete(BankId);
            }
            catch
            {

                throw;
            }
        }
    }
}
