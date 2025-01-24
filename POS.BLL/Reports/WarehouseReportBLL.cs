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
    public class WarehouseReportBLL
    {
        public DataTable WarehouseReport(string[] category_code, string[] brand_code, string[] location_code, int unit_id = 0, string item_type = "", bool qty_onhand = false)
        {
            try
            {
                WarehouseReportDLL objDLL = new WarehouseReportDLL();
                return objDLL.WarehouseReport(category_code, brand_code, location_code, unit_id, item_type, qty_onhand);
            }
            catch
            {
                
                throw;
            }
        }
        public DataSet InventoryReport(int branchId, int userId, string category = null, string brand = null, string location = null, int OperationType=1)
        {
            try
            {
                WarehouseReportDLL objDLL = new WarehouseReportDLL();
                return objDLL.InventoryReport(branchId, userId, category,  brand,  location, OperationType);
            }
            catch
            {

                throw;
            }
        }
        public DataTable WarehouseReport_total_amount(string[] category_code, string[] brand_code, string[] location_code, int unit_id = 0, string item_type = "", bool qty_onhand = false)
        {
            try
            {
                WarehouseReportDLL objDLL = new WarehouseReportDLL();
                return objDLL.WarehouseReport_total_amount(category_code, brand_code, location_code, unit_id, item_type, qty_onhand);
            }
            catch
            {

                throw;
            }
        }
    }
}
