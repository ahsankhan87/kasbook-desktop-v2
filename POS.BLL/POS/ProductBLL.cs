using POS.Core;
using POS.DAL;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.BLL
{
    public class ProductBLL
    {
        private ProductDLL productDLL;
        
        public ProductBLL()
        {
            productDLL = new ProductDLL();
        }
        
        
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
                return objDLL.SearchProductByBrandAndCategory_2(condition, category_code, brand_code, group_code);
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

        public DataTable SearchProductsPaged(string condition, string category_code, string brand_code_csv, string group_code, int pageIndex, int pageSize)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchProductsPaged(condition, category_code, brand_code_csv, group_code, pageIndex, pageSize);
            }
            catch { throw; }
        }

        public DataTable SearchProductsPagedWithCount(string condition, string category_code, string brand_code_csv, string group_code, int pageIndex, int pageSize, out int totalCount)
        {
            try
            {
                ProductDLL objDLL = new ProductDLL();
                return objDLL.SearchProductsPagedWithCount(condition, category_code, brand_code_csv, group_code, pageIndex, pageSize, out totalCount);
            }
            catch { throw; }
        }

        /// <summary>
        /// New method to search products by name or code for a specific branch
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="branchId"></param>
        /// <returns></returns>
        
        /*
        public DataTable SearchProducts(string searchTerm, int branchId)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new DataTable();

            return productDLL.SearchProducts(searchTerm.Trim(), branchId);
        }

        public DataTable GetProductByBarcode(string barcode, int branchId)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return new DataTable();

            return productDLL.GetProductByBarcode(barcode.Trim(), branchId);
        }

        public bool ValidateProductQuantity(string productCode, decimal quantity, int branchId)
        {
            DataTable product = productDLL.SearchProducts(productCode, branchId);
            if (product.Rows.Count == 0)
                return false;

            decimal availableQty = Convert.ToDecimal(product.Rows[0]["qty"]);
            return availableQty >= quantity;
        }
        */
        

        public DataTable SearchProductsWithStock(string searchTerm, int branchId, string locationCode = null)
        {
            string query = @"
                SELECT 
                    p.code, p.name, p.name_ar, p.barcode, p.unit_price, 
                    p.cost_price, p.brand_code, p.category_code, p.item_type,
                    p.tax_id, p.unit_id, p.item_number,
                    ISNULL(ps.qty, 0) as current_stock,
                    ps.loc_code, ps.reorder_level
                FROM pos_products p
                LEFT JOIN pos_product_stocks ps ON p.code = ps.item_code 
                    AND ps.branch_id = @BranchId
                    AND (@LocationCode IS NULL OR ps.loc_code = @LocationCode)
                WHERE (p.branch_id = @BranchId OR p.branch_id IS NULL) 
                AND (p.deleted = 0 OR p.deleted IS NULL)
                AND (p.name LIKE @SearchTerm OR p.code LIKE @SearchTerm 
                    OR p.barcode LIKE @SearchTerm OR p.name_ar LIKE @SearchTerm)";

            // This would need to be executed through DatabaseHelper
            // For now, let's modify the ProductBLL to include stock information
            return new DataTable();
        }

        public decimal GetProductStock(string itemNumber, int branchId, string locationCode = null)
        {
            return productDLL.GetCurrentStock(itemNumber, branchId, locationCode);
        }

        public DataTable SearchProducts(string searchTerm, int branchId)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return new DataTable();

            DataTable products = productDLL.SearchProducts(searchTerm.Trim(), branchId);

            // Add stock information to the results
            if (products.Columns.Contains("qty"))
                products.Columns.Remove("qty");

            products.Columns.Add("current_stock", typeof(decimal));
            products.Columns.Add("location", typeof(string));

            foreach (DataRow row in products.Rows)
            {
                string itemNumber = row["item_number"].ToString();
                decimal currentStock = productDLL.GetCurrentStock(itemNumber, branchId);
                row["current_stock"] = currentStock;
                row["location"] = "DEF"; // Default location
            }

            return products;
        }

        public DataTable GetProductByBarcode(string barcode, int branchId)
        {
            if (string.IsNullOrWhiteSpace(barcode))
                return new DataTable();

            DataTable product = productDLL.GetProductByBarcode(barcode.Trim(), branchId);

            if (product.Rows.Count > 0)
            {
                // Add stock information
                string productCode = product.Rows[0]["code"].ToString();
                decimal currentStock = productDLL.GetCurrentStock(productCode, branchId);

                if (product.Columns.Contains("qty"))
                    product.Columns.Remove("qty");

                product.Columns.Add("current_stock", typeof(decimal));
                product.Rows[0]["current_stock"] = currentStock;
            }

            return product;
        }

        public bool ValidateProductQuantity(string itemNumber, decimal quantity, int branchId)
        {
            decimal availableStock = productDLL.GetCurrentStock(itemNumber, branchId);
            return availableStock >= quantity;
        }

        public decimal GetProductStock(string itemNumber, int branchId)
        {
            return productDLL.GetCurrentStock(itemNumber, branchId);
        }
    }
}
