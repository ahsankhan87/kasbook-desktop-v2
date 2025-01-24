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
    public class PaymentMethodBLL
    {
        public DataTable GetAll()
        {
            try
            {
                PaymentMethodDLL objDLL = new PaymentMethodDLL();
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
                PaymentMethodDLL objDLL = new PaymentMethodDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByPaymentMethodID(int PaymentMethod_id)
        {
            try
            {
                PaymentMethodDLL objDLL = new PaymentMethodDLL();
                return objDLL.SearchRecordByPaymentMethodID(PaymentMethod_id);
            }
            catch
            {

                throw;
            }
        }

        public int Insert(PaymentMethodModal obj)
        {
            try
            {
                PaymentMethodDLL objDLL = new PaymentMethodDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(PaymentMethodModal obj)
        {
            try
            {
                PaymentMethodDLL objDLL = new PaymentMethodDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int PaymentMethodId)
        {
            try
            {
                PaymentMethodDLL objDLL = new PaymentMethodDLL();
                return objDLL.Delete(PaymentMethodId);
            }
            catch
            {

                throw;
            }
        }
    }
}
