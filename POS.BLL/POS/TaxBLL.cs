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
    public class TaxBLL
    {
        public DataTable GetAll()
        {
            try
            {
                TaxDLL objDLL = new TaxDLL();
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
                TaxDLL objDLL = new TaxDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByTaxID(int Tax_id)
        {
            try
            {
                TaxDLL objDLL = new TaxDLL();
                return objDLL.SearchRecordByTaxID(Tax_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(TaxModal obj)
        {
            try
            {
                TaxDLL objDLL = new TaxDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(TaxModal obj)
        {
            try
            {
                TaxDLL objDLL = new TaxDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int TaxId)
        {
            try
            {
                TaxDLL objDLL = new TaxDLL();
                return objDLL.Delete(TaxId);
            }
            catch
            {

                throw;
            }
        }
    }
}
