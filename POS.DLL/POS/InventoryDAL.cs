// DAL/InventoryDAL.cs
using POS.DLL;
using System;
using System.Data;
using System.Data.SqlClient;

namespace POS.DAL
{
    public class InventoryDAL
    {
        private dbConnection dbHelper;

        public InventoryDAL()
        {
            dbHelper = new dbConnection();
        }

        public bool RecordInventoryTransaction(string itemCode, decimal quantity, decimal costPrice,
                                            decimal unitPrice, int branchId, int userId, 
                                            string description, string invoiceNo, string transactionType,
                                            string itemNumber,
                                            int? customerId = null, int? supplierId = null,
                                            string locationCode = "MAIN")
        {
            string query = @"
                INSERT INTO pos_inventory (
                    item_code, qty, cost_price, unit_price, branch_id, user_id,
                    description, invoice_no, date_created, date_updated,
                    customer_id, supplier_id, trans_date, loc_code, packet_qty, item_number
                ) VALUES (
                    @ItemCode, @Quantity, @CostPrice, @UnitPrice, @BranchId, @UserId,
                    @Description, @InvoiceNo, GETDATE(), GETDATE(),
                    @CustomerId, @SupplierId, GETDATE(), @LocationCode, 1, @ItemNumber
                )";

            SqlParameter[] parameters = {
                new SqlParameter("@ItemCode", itemCode),
                new SqlParameter("@Quantity", quantity),
                new SqlParameter("@CostPrice", costPrice),
                new SqlParameter("@UnitPrice", unitPrice),
                new SqlParameter("@BranchId", branchId),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@Description", description),
                new SqlParameter("@InvoiceNo", (object)invoiceNo ?? DBNull.Value),
                new SqlParameter("@CustomerId", (object)customerId ?? DBNull.Value),
                new SqlParameter("@SupplierId", (object)supplierId ?? DBNull.Value),
                new SqlParameter("@LocationCode", locationCode),
                new SqlParameter("@ItemNumber", (object)itemNumber ?? DBNull.Value)
            };

            return dbHelper.ExecuteNonQuery(query, parameters) > 0;
        }

        public DataTable GetInventoryHistory(string itemNumber = null, int? branchId = null, 
                                           DateTime? fromDate = null, DateTime? toDate = null)
        {
            string query = @"
                SELECT 
                    i.item_code, i.qty, i.cost_price, i.unit_price,
                    i.description, i.invoice_no, i.date_created,
                    i.customer_id, i.supplier_id, i.loc_code,
                    p.name as product_name, b.name as branch_name
                FROM pos_inventory i
                LEFT JOIN pos_products p ON i.item_number = p.item_number
                LEFT JOIN pos_branches b ON i.branch_id = b.id
                WHERE 1=1";

            var parameters = new System.Collections.Generic.List<SqlParameter>();

            if (!string.IsNullOrEmpty(itemNumber))
            {
                query += " AND i.item_number = @itemNumber";
                parameters.Add(new SqlParameter("@itemNumber", itemNumber));
            }

            if (branchId.HasValue)
            {
                query += " AND i.branch_id = @BranchId";
                parameters.Add(new SqlParameter("@BranchId", branchId.Value));
            }

            if (fromDate.HasValue)
            {
                query += " AND i.trans_date >= @FromDate";
                parameters.Add(new SqlParameter("@FromDate", fromDate.Value));
            }

            if (toDate.HasValue)
            {
                query += " AND i.trans_date <= @ToDate";
                parameters.Add(new SqlParameter("@ToDate", toDate.Value));
            }

            query += " ORDER BY i.date_created DESC";

            return dbHelper.ExecuteQuery(query, parameters.ToArray());
        }
    }
}