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
    public class PaymentTermsBLL
    {
        public DataTable GetAll()
        {
            try
            {
                PaymentTermsDLL objDLL = new PaymentTermsDLL();
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
                PaymentTermsDLL objDLL = new PaymentTermsDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByPaymentTermsID(int PaymentTerms_id)
        {
            try
            {
                PaymentTermsDLL objDLL = new PaymentTermsDLL();
                return objDLL.SearchRecordByPaymentTermsID(PaymentTerms_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(PaymentTermsModal obj)
        {
            try
            {
                PaymentTermsDLL objDLL = new PaymentTermsDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(PaymentTermsModal obj)
        {
            try
            {
                PaymentTermsDLL objDLL = new PaymentTermsDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int PaymentTermsId)
        {
            try
            {
                PaymentTermsDLL objDLL = new PaymentTermsDLL();
                return objDLL.Delete(PaymentTermsId);
            }
            catch
            {

                throw;
            }
        }
    }
}
