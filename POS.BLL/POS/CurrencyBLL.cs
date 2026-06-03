using POS.Core;
using POS.DLL;
using System;
using System.Data;

namespace POS.BLL
{
    public class CurrencyBLL
    {
        public DataTable GetAll()
        {
            try
            {
                CurrencyDLL objDLL = new CurrencyDLL();
                return objDLL.GetAll();
            }
            catch
            {
                throw;
            }
        }

        public DataTable SearchRecord(string condition)
        {
            try
            {
                CurrencyDLL objDLL = new CurrencyDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {
                throw;
            }
        }

        public DataTable SearchRecordByCurrencyID(int currencyId)
        {
            try
            {
                CurrencyDLL objDLL = new CurrencyDLL();
                return objDLL.SearchRecordByCurrencyID(currencyId);
            }
            catch
            {
                throw;
            }
        }

        public int Insert(CurrencyModal obj)
        {
            try
            {
                CurrencyDLL objDLL = new CurrencyDLL();
                return objDLL.Insert(obj);
            }
            catch
            {
                throw;
            }
        }

        public int Update(CurrencyModal obj)
        {
            try
            {
                CurrencyDLL objDLL = new CurrencyDLL();
                return objDLL.Update(obj);
            }
            catch
            {
                throw;
            }
        }

        public int Delete(int currencyId)
        {
            try
            {
                CurrencyDLL objDLL = new CurrencyDLL();
                return objDLL.Delete(currencyId);
            }
            catch
            {
                throw;
            }
        }
    }
}