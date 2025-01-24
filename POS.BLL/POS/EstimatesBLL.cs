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
    public class EstimatesBLL
    {
        public DataTable GetAll()
        {
            try
            {
                EstimatesDLL objDLL = new EstimatesDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }
        public DataTable GetAllEstimates()
        {
            try
            {
                EstimatesDLL objDLL = new EstimatesDLL();
                return objDLL.GetAllEstimates();
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetAllActiveEstimates()
        {
            try
            {
                EstimatesDLL objDLL = new EstimatesDLL();
                return objDLL.GetAllActiveEstimates();
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetAllEstimatesItems(int sale_id)
        {
            try
            {
                EstimatesDLL objDLL = new EstimatesDLL();
                return objDLL.GetAllEstimatesItems(sale_id);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SaleReceipt(string invoice_no)
        {
            try
            {
                EstimatesDLL objDLL = new EstimatesDLL();
                return objDLL.SaleReceipt(invoice_no);
            }
            catch
            {

                throw;
            }
        }

        public DataTable EstimateReceipt(string invoice_no)
        {
            try
            {
                EstimatesDLL objDLL = new EstimatesDLL();
                return objDLL.EstimateReceipt(invoice_no);
            }
            catch
            {

                throw;
            }
        }


        public String GetMaxEstimateInvoiceNo()
        {
            try
            {
                EstimatesDLL objDLL = new EstimatesDLL();
                return objDLL.GetMaxEstimateInvoiceNo();
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
                EstimatesDLL objDLL = new EstimatesDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

       

    }
}
