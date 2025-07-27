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
    public class ProductBLL
    {
        static public DataTable GetAll()
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetAll();
            }
            catch
            {
                
                throw;
            }
        }
        static public List<ProductModal> GetAllProducts(string Product_code)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetAllProducts(Product_code);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetAllByProductId(int Product_id)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetAllByProductId(Product_id);
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetAllByProductCode(string Product_code)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetAllByProductCode(Product_code);
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetAllByProductByItemNumber(string item_number)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetAllByProductByItemNumber(item_number);
            }
            catch
            {

                throw;
            }
        }

        public bool CheckDuplicateBarcode(string barcode)
        { 
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.CheckDuplicateBarcode(barcode);
            }
            catch
            {

                throw;
            }
        }
        public bool IsProductExist(string part_number, string category_code)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.IsProductExist(part_number, category_code);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecord(String condition, bool by_code = false, bool by_name = false)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchRecord(condition, by_code, by_name);
            }
            catch
            {

                throw;
            }
        }
        public DataTable SearchProductByLocation(String condition, bool by_code = false, bool by_name = false)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchProductByLocation(condition, by_code, by_name);
            }
            catch
            {

                throw;
            }
        }

        static public DataTable SearchProductByBrandAndCategory(String condition, string category_code, string brand_code, string group_code="")
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchProductByBrandAndCategory(condition, category_code, brand_code, group_code);
            }
            catch
            {

                throw;
            }
        }
        static public DataTable SearchProductByLocations(String condition, string category_code, string brand_code, string group_code, string fromLocation, string toLocation, bool qty_onhand)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchProductByLocations(condition, category_code, brand_code, group_code,fromLocation,toLocation, qty_onhand);
            }
            catch
            {

                throw;
            }
        }

        public DataTable SearchRecordByProductNumber(String condition)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchRecordByProductNumber(condition);
            }
            catch
            {

                throw;
            }
        }

        public DataTable GetAllProductCodes()
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetAllProductCodes();
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetAllProductNamesOnly()
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetAllProductNamesOnly();
            }
            catch
            {

                throw;
            }
        }
        public DataTable SearchRecordByProductCode_1(string product_code)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchRecordByProductCode_1(product_code);
            }
            catch
            {

                throw;
            }
        }
        public DataTable SearchRecordByProductID(string product_id)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchRecordByProductID(product_id);
            }
            catch
            {

                throw;
            }
        }
        public DataTable SearchRecordByProductCode(string product_code)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchRecordByProductCode(product_code);
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetProductsSummary(DateTime StartDate, DateTime EndDate, bool is_zero, string group_code, string brand_code, string category_code)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetProductsSummary(StartDate,EndDate, is_zero, group_code, brand_code, category_code);
            }
            catch
            {

                throw;
            }
        }
        public DataTable SearchProductBySupplier(string supplierID)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchProductBySupplier(supplierID);
            }
            catch
            {

                throw;
            }
        }
        public DataTable GetProductsByAlternateNo(int alternate_no)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetProductsByAlternateNo(alternate_no);
            }
            catch
            {

                throw;
            }
        }
        public DataTable SearchRecordByProductName(string product_name)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchRecordByProductName(product_name);
            }
            catch
            {

                throw;
            }
        }
        public DataTable SearchRecordByBarcode(string barcode)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchRecordByBarcode(barcode);
            }
            catch
            {

                throw;
            }
        }
        public DataTable Get_otherStock(string productID, string item_number)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.Get_otherStock(productID, item_number);
            }
            catch
            {

                throw;
            }
        }
        public string GetMaxProductNumber()
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetMaxProductNumber();
            }
            catch
            {

                throw;
            }
        }

        public int Insert(ProductModal obj)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.Insert(obj);
            }
            catch
            {

                throw;
            }
        }

        public int Update(ProductModal obj)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.Update(obj);
            }
            catch
            {

                throw;
            }
        }

        public int UpdateReorder_level(ProductModal obj)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.UpdateReorder_level(obj);
            }
            catch
            {

                throw;
            }
        }

        public int BulkUpdate(ProductModal obj)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.BulkUpdate(obj);
            }
            catch
            {

                throw;
            }
        }

        public string UpdateQtyAdjustment(ProductModal obj)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.UpdateQtyAdjustment(obj);
            }
            catch
            {

                throw;
            }
        }

        public string GetMaxAdjustmentInvoiceNo()
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetMaxAdjustmentInvoiceNo();
            }
            catch
            {

                throw;
            }
        }

        public int Delete(int ProductId)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.Delete(ProductId);
            }
            catch
            {

                throw;
            }
        }

        public int UpdateProductLocationTransfer(ProductModal obj)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.UpdateProductLocationTransfer(obj);
            }
            catch
            {

                throw;
            }
        }

        public string GetMaxLocationTransferInvoiceNo()
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.GetMaxLocationTransferInvoiceNo();
            }
            catch
            {

                throw;
            }
        }

    }
}
