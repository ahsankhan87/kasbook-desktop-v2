using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using POS.Core;
using POS.DLL;
using POS.DLL.Inventory;

namespace POS.BLL.Inventory
{
    public static class InventoryValuationHelper
    {
        public static decimal GetEffectiveProductCost(DataRow row, int branchId = 0, string itemNumber = null)
        {
            if (row == null)
                return 0m;

            decimal avgCost = 0m;
            decimal standardCost = 0m;
            var rowItemNumber = itemNumber ?? Convert.ToString(row["item_number"]);

            if (row.Table.Columns.Contains("avg_cost") && row["avg_cost"] != DBNull.Value)
                avgCost = Convert.ToDecimal(row["avg_cost"]);

            if (row.Table.Columns.Contains("standard_cost") && row["standard_cost"] != DBNull.Value)
                standardCost = Convert.ToDecimal(row["standard_cost"]);

            if (avgCost <= 0m && standardCost <= 0m)
                return 0m;

            try
            {
                var valuationSettings = new InventoryValuationDLL().GetSettings(branchId > 0 ? branchId : UsersModal.logged_in_branch_id);
                string valuationMethod = string.IsNullOrWhiteSpace(valuationSettings?.ValuationMethod)
                    ? "WAC"
                    : valuationSettings.ValuationMethod.Trim();

                if (string.Equals(valuationMethod, "STANDARD", StringComparison.OrdinalIgnoreCase))
                    return standardCost > 0m ? standardCost : avgCost;

                if (string.Equals(valuationMethod, "FIFO", StringComparison.OrdinalIgnoreCase))
                {
                    int productId = 0;
                    if (row.Table.Columns.Contains("id") && row["id"] != DBNull.Value)
                        productId = Convert.ToInt32(row["id"]);

                    if (productId <= 0 && !string.IsNullOrWhiteSpace(rowItemNumber))
                    {
                        using (var cn = new SqlConnection(dbConnection.ConnectionString))
                        {
                            cn.Open();
                            using (var cmd = new SqlCommand(@"SELECT TOP 1 id FROM dbo.pos_products WHERE item_number = @item_number AND deleted = 0", cn))
                            {
                                cmd.Parameters.AddWithValue("@item_number", rowItemNumber);
                                var value = cmd.ExecuteScalar();
                                if (value != null && value != DBNull.Value)
                                    productId = Convert.ToInt32(value);
                            }
                        }
                    }

                    if (productId > 0)
                    {
                        var layers = new InventoryCostingEngineDLL().GetFIFOLayers(productId, branchId > 0 ? branchId : UsersModal.logged_in_branch_id);
                        if (layers != null && layers.Count > 0)
                        {
                            decimal totalRemainingQty = layers.Sum(l => l.RemainingQty);
                            if (totalRemainingQty > 0m)
                            {
                                decimal totalRemainingValue = layers.Sum(l => l.RemainingQty * l.UnitCost);
                                return Math.Round(totalRemainingValue / totalRemainingQty, 4, MidpointRounding.AwayFromZero);
                            }
                        }
                    }
                }
            }
            catch
            {
                // fallback to avg cost below
            }

            return avgCost;
        }
    }
}
