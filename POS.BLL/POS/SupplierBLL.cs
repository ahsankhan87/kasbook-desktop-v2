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
    }
}
