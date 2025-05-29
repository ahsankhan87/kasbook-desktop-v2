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
    public class Purchases_orderBLL
    {
        Purchases_orderDLL objDLL = new Purchases_orderDLL();

        public DataTable GetAll()
        {
            try
            {
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }
        
        public DataTable GetAllPurchasesOrders()
        {
            try
            {
                return objDLL.GetAllPurchasesOrders();
            }
            catch
            {

                throw;
            }
        }

        public double GetPOrder_qty(string item_number)
        {
            try
            {
                return objDLL.GetPOrder_qty(item_number);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetAllPurchases_orderItems(int sale_id)
        {
            try
            {
                return objDLL.GetAllPurchases_orderItems(sale_id);
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetPOrder_bycategory_code(string category_code)
        {
            try
            {
                return objDLL.GetPOrder_bycategory_code(category_code);
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetAllActivePOrder()
        {
            try
            {
                return objDLL.GetAllActivePOrder();
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetAllPurchaseOrder(string invoice_no)
        {
            try
            {
                return objDLL.GetAllPurchaseOrder(invoice_no);
            }
            catch
            {

                throw;
            }
        }
        public String GetMaxInvoiceNo()
        {
            try
            {
                return objDLL.GetMaxInvoiceNo();
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
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public int Insert_Purchase_order_new(Purchases_orderModal obj, DataTable dt)
        {
            try
            {
                return objDLL.Insert_Purchase_order_new(obj, dt);
            }
            catch
            {

                throw;
            }
        }

        public int InsertPurchases_order(Purchases_orderModal obj)
        {
            try
            {
                return objDLL.InsertPurchases_order(obj);
            }
            catch
            {

                throw;
            }
        }
        public int InsertPurchaseOrderBLL(List<Purchases_orderModal> purchases, List<PurchaseOrderDetailModal> purchase_detail)
        {
            try
            {
                return objDLL.InsertPurchaseOrder(purchases, purchase_detail);
            }
            catch
            {

                throw;
            }
        }
        public int InsertPurchases_orderItems(Purchases_orderModal obj)
        {
            try
            {
                return objDLL.InsertPurchases_orderItems(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(Purchases_orderModal obj)
        {
            try
            {
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int Purchases_orderId)
        {
            try
            {
                return objDLL.Delete(Purchases_orderId);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetReturnPurchase(string invoice_no)
        {
            try
            {
                return objDLL.GetReturnPurchase(invoice_no);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetReturnPurchaseItems(string invoice_no)
        {
            try
            {
                return objDLL.GetReturnPurchaseItems(invoice_no);
            }
            catch
            {

                throw;
            }
        }
        public int InsertReturnPurchase(Purchases_orderModal obj)
        {
            try
            {
                return objDLL.InsertReturnPurchase(obj);
            }
            catch
            {

                throw;
            }
        }

        


        public int DeletePurchasesOrder(string invoice_no)
        {
            try
            {
                return objDLL.DeletePurchasesOrder(invoice_no);
            }
            catch
            {

                throw;
            }
        }

    }
}
