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
    public class PurchasesBLL
    {
        PurchasesDLL objDLL = new PurchasesDLL();

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

        public DataTable GetAllPurchases()
        {
            try
            {
                return objDLL.GetAllPurchases();
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetAllPurchasesItems(string invoice_no)
        {
            try
            {
                return objDLL.GetAllPurchasesItems(invoice_no);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetAllPurchaseByInvoice(string invoice_no)
        {
            try
            {
                return objDLL.GetAllPurchaseByInvoice(invoice_no);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetAll_Hold_PurchaseByInvoice(string invoice_no)
        {
            try
            {
                return objDLL.GetAll_Hold_PurchaseByInvoice(invoice_no);
            }
            catch
            {

                throw;
            }
        }
        // PURCHASE
        public string GeneratePurchaseInvoiceNo(int? branchId = null, DateTime? invoiceDate = null)
        {
            return objDLL.GenerateDailyInvoiceNo("pos_purchases", "invoice_no", "P", branchId, invoiceDate);
        }

        // PURCHASE RETURN
        public string GeneratePurchaseReturnInvoiceNo(int? branchId = null, DateTime? invoiceDate = null)
        {
            return objDLL.GenerateDailyInvoiceNo("pos_purchases", "invoice_no", "PR", branchId, invoiceDate);
        }
        // PURCHASE HOLD
        public string GenerateHoldPurchaseInvoiceNo(int? branchId = null, DateTime? invoiceDate = null)
        {
            return objDLL.GenerateDailyInvoiceNo("pos_hold_purchases", "invoice_no", "PH", branchId, invoiceDate);
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
        public String GetMaxReturnInvoiceNo()
        {
            try
            {
                return objDLL.GetMaxReturnInvoiceNo();
            }
            catch
            {

                throw;
            }
        }
        public String GetMaxInvoiceNo_HOLD()
        {
            try
            {
                return objDLL.GetMaxInvoiceNo_HOLD();
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

        public int Insertpurchases(List<PurchaseModalHeader> purchases, List<PurchasesModal> purchase_detail)
        {
            try
            {
                return objDLL.Insertpurchases(purchases, purchase_detail);
            }
            catch
            {

                throw;
            }
        }

        public int InsertpurchasesItems(PurchasesModal obj)
        {
            try
            {
                return objDLL.InsertpurchasesItems(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Insert_hold_purchases(PurchasesModal obj)
        {
            try
            {
                return objDLL.Insert_hold_purchases(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Insert_hold_purchasesItems(PurchasesModal obj)
        {
            try
            {
                return objDLL.Insert_hold_purchasesItems(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(PurchasesModal obj)
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

        public int Delete(int purchasesId)
        {
            try
            {
                return objDLL.Delete(purchasesId);
            }
            catch
            {

                throw;
            }
        }

        public int DeletePurchases(string invoice_no)
        {
            try
            {
                return objDLL.DeletePurchases(invoice_no);
            }
            catch
            {

                throw;
            }
        }

        public DataTable PurchaseReceipt(string invoice_no)
        {
            try
            {
                return objDLL.PurchaseReceipt(invoice_no);
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
        public int InsertReturnPurchase(List<PurchaseModalHeader> purchases, List<PurchasesModal> purchase_detail)
        {
            try
            {
                return objDLL.InsertReturnPurchase(purchases, purchase_detail);
            }
            catch
            {

                throw;
            }
        }

        public int InsertReturnPurchaseItems(PurchasesModal obj)
        {
            try
            {
                return objDLL.InsertReturnPurchaseItems(obj);
            }
            catch
            {

                throw;
            }
        }
        public int UpdateSupplierInPurchases(string invoiceNo, string supplier_id,string supplierInvoiceNo)
        {
            try
            {
                return objDLL.UpdateSupplierInPurchases(invoiceNo, supplier_id, supplierInvoiceNo);
            }
            catch
            {

                throw;
            }
        }

        public bool IsSupplierInvoiceNoExists(int supplierId, string supplierInvoiceNo, string excludeInvoiceNo = null)
        {
            try
            {
                return objDLL.IsSupplierInvoiceNoExists(supplierId, supplierInvoiceNo, excludeInvoiceNo);
            }
            catch
            {
                throw;
            }
        }

        public bool IsHoldSupplierInvoiceNoExists(int supplierId, string supplierInvoiceNo, string excludeInvoiceNo = null)
        {
            try
            {
                return objDLL.IsHoldSupplierInvoiceNoExists(supplierId, supplierInvoiceNo, excludeInvoiceNo);
            }
            catch
            {
                throw;
            }
        }
    }
}
