﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.DLL;
using System.Data;
using POS.Core;

namespace POS.BLL
{
    public class SalesBLL
    {
        public DataTable GetAll()
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }
        public DataTable GetAllSales()
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.GetAllSales();
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetAllSalesItems(string invoice_no)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.GetAllSalesItems(invoice_no);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetSaleAndSalesItems(string invoice_no)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.GetSaleAndSalesItems(invoice_no);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetEstimatesAndEstimatesItems(string invoice_no)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.GetEstimatesAndEstimatesItems(invoice_no);
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
                SalesDLL objDLL = new SalesDLL();
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
                SalesDLL objDLL = new SalesDLL();
                return objDLL.EstimateReceipt(invoice_no);
            }
            catch
            {

                throw;
            }
        }

        public String GetMaxSaleInvoiceNo()
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.GetMaxInvoiceNo();
            }
            catch
            {

                throw;
            }
        }
        public String GetMaxSalesReturnInvoiceNo()
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.GetMaxSalesReturnInvoiceNo();
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
                SalesDLL objDLL = new SalesDLL();
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
                SalesDLL objDLL = new SalesDLL();
                return objDLL.SearchRecord(condition);
            }
            catch
            {

                throw;
            }
        }

        public int InsertSales(List<SalesModalHeader> sales, List<SalesModal> sales_detail)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.InsertSales(sales, sales_detail);
            }
            catch
            {
                throw;
            }
        }
        public int ict_qty_request(List<SalesModalHeader> sales, List<SalesModal> sales_detail)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.ict_qty_request(sales, sales_detail);
            }
            catch
            {
                throw;
            }
        }
       
        public int InsertSales_1(SalesModal obj)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.InsertSales_1(obj);
            }
            catch
            {

                throw;
            }
        }

        public int InsertSalesItems(SalesModal obj)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.InsertSalesItems(obj);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetReturnSales(string invoice_no)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.GetReturnSales(invoice_no);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetReturnSaleItems(string invoice_no)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.GetReturnSaleItems(invoice_no);
            }
            catch
            {

                throw;
            }
        }
        public int InsertReturnSales(List<SalesModalHeader> sales, List<SalesModal> sales_detail)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.InsertReturnSales(sales, sales_detail);
            }
            catch
            {

                throw;
            }
        }
        public int InsertReturnSalesItems(SalesModal obj)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.InsertReturnSalesItems(obj);
            }
            catch
            {

                throw;
            }
        }
        public int Update(SalesModal obj)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int DeleteSales(string invoice_no)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.DeleteSales(invoice_no);
            }
            catch
            {

                throw;
            }
        }


        public int InsertEstimates(List<SalesModalHeader> sales, List<SalesModal> sales_detail)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.InsertEstimates(sales,sales_detail);
            }
            catch
            {

                throw;
            }
        }
        public int UpdateCustomerInSales(string invoiceNo,string customerId)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.UpdateCustomerInSales(invoiceNo, customerId);
            }
            catch
            {

                throw;
            }
        }
        public int UpdateZetcaQrcodeInSales(string invoiceNo, byte[] Zetca_qrcode)
        {
            try
            {
                SalesDLL objDLL = new SalesDLL();
                return objDLL.UpdateZetcaQrcodeInSales(invoiceNo, Zetca_qrcode);
            }
            catch
            {

                throw;
            }
        }
    }
}
