using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    public class Purchases_orderDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private Purchases_orderModal info = new Purchases_orderModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchases_order", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "5");

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }
            
        }
        public DataTable GetAllPurchasesOrders()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt1 = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT TOP 2000 p.*,(p.total_tax+p.total_amount-p.discount_value) as total, CONCAT(sp.first_name,' ',sp.last_name) as supplier_name " +
                            " FROM pos_purchases_order p LEFT JOIN pos_suppliers sp ON p.supplier_id=sp.id" +
                            " WHERE p.purchase_date BETWEEN @FY_from_date AND @FY_to_date AND p.branch_id = @branch_id order by p.id desc";


                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@FY_from_date", UsersModal.fy_from_date);
                        cmd.Parameters.AddWithValue("@FY_to_date", UsersModal.fy_to_date);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt1);
                    return dt1;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable GetAllPurchases_orderItems(int purchase_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT PI.invoice_no,PI.id,PI.item_code,PI.item_number,PI.quantity,PI.cost_price,PI.discount_value,"+
                            "(PI.cost_price*PI.quantity-PI.discount_value) AS total, P.name AS product_name," +
                            "(PI.cost_price*PI.tax_rate/100) AS tax " +
                            "FROM pos_purchases_order_items PI " +
                            "LEFT JOIN pos_products P ON P.item_number=PI.item_number " +
                            "WHERE PI.branch_id = @branch_id AND purchase_id = @purchase_id AND PI.status=0";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        cmd.Parameters.AddWithValue("@purchase_id", purchase_id);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }


        public double GetPOrder_qty(string item_number)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    double POrder_qty = 0;

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT COALESCE(SUM(PI.quantity),0) AS porder_qty" +
                            " FROM pos_purchases_order_items PI" +
                            " WHERE PI.branch_id = @branch_id AND PI.item_number = @item_number AND PI.status=0";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@item_number", item_number);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    POrder_qty = Convert.ToDouble(cmd.ExecuteScalar());

                    return (double)POrder_qty;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable SearchRecord(String condition)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT * FROM pos_purchases_order WHERE invoice_no LIKE @invoice_no AND branch_id = @branch_id", cn);
                        //cmd.Parameters.AddWithValue("@invoice_no", condition);
                        cmd.Parameters.AddWithValue("@invoice_no", string.Format("%{0}%", condition));
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        return dt;

                    }

                    return dt;
                }
                catch
                {
                    throw;
                }
            } 
        }

        public DataTable GetAllPurchaseOrder(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.purchase_date,S.purchase_time,S.invoice_no,S.purchase_type,S.account,S.supplier_id,S.supplier_invoice_no,S.employee_id,S.description,S.account,S.delivery_date,'0' AS shipping_cost," +
                            " SI.id,SI.item_code,SI.quantity,SI.unit_price,SI.cost_price,SI.item_number," +
                            " SI.discount_value,(SI.unit_price*SI.quantity) AS total, SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity*SI.tax_rate/100) AS vat," +
                            " P.name AS name,P.code,P.location_code,P.item_type," +
                            " C.first_name AS supplier_name, C.vat_no AS supplier_vat," + 
                            " U.name AS unit," +
                            " CT.name AS category" +
                            " FROM pos_purchases_order S" +
                            " LEFT JOIN pos_purchases_order_items SI ON S.id=SI.purchase_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                            " LEFT JOIN pos_categories CT ON CT.code=P.category_code" +
                            " LEFT JOIN pos_suppliers C ON C.id=S.supplier_id" +
                            " WHERE S.branch_id = @branch_id AND S.invoice_no = @invoice_no AND SI.status = 0"+
                            " ORDER BY SI.serialnumber ASC";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable GetAllActivePOrder()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT SI.*,C.first_name AS supplier_name " +
                            "FROM pos_purchases_order SI " +
                            "LEFT JOIN pos_suppliers C ON C.id=SI.supplier_id" +
                            " WHERE SI.branch_id = @branch_id AND SI.status = 0 ORDER BY SI.id desc";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable GetPOrder_bycategory_code(string category_code)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT * " +
                            "FROM pos_purchases_order " +
                            " WHERE branch_id = @branch_id AND category_code = @category_code";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@category_code", category_code);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public String GetMaxInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                    
                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_purchases_order WHERE branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());
                    
                        if(maxId == "")
                        {
                            return maxId = "PO-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(3, 6));
                            intval++;
                            maxId = String.Format("PO-{0:000000}", intval);
                            return maxId;
                        }
                    
                    }
                    return "";
                }
                catch
                {

                    throw;
                }
            }

        }
        public int InsertPurchaseOrder(List<Purchases_orderModal> purchases, List<PurchaseOrderDetailModal> purchase_detail)
        {
            Int32 newProdID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction;

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    try
                    {
                        cmd = new SqlCommand("sp_Purchases_order", cn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (Purchases_orderModal purchase_header in purchases)
                        {
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@employee_id", purchase_header.employee_id);
                            cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_type", purchase_header.purchase_type);
                            cmd.Parameters.AddWithValue("@delivery_date", purchase_header.delivery_date);
                            cmd.Parameters.AddWithValue("@supplier_invoice_no", purchase_header.supplier_invoice_no);
                            cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                            cmd.Parameters.AddWithValue("@total_amount", purchase_header.total_amount);
                            cmd.Parameters.AddWithValue("@total_tax", purchase_header.total_tax);
                            cmd.Parameters.AddWithValue("@discount_value", purchase_header.total_discount);
                            cmd.Parameters.AddWithValue("@purchase_date", purchase_header.purchase_date);
                            cmd.Parameters.AddWithValue("@description", purchase_header.description);
                            cmd.Parameters.AddWithValue("@account", purchase_header.account);
                            cmd.Parameters.AddWithValue("@category_code", purchase_header.category_code);
                            //cmd.Parameters.AddWithValue("@purchase_time", obj.purchase_time);
                            cmd.Parameters.AddWithValue("@OperationType", "1");

                        }

                        newProdID = Convert.ToInt32(cmd.ExecuteScalar());

                        foreach (PurchaseOrderDetailModal detail in purchase_detail)
                        {
                            cmd = new SqlCommand("sp_Purchases_order_items", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@item_code", detail.code);
                            cmd.Parameters.AddWithValue("@serialNo", detail.serialNo);
                            cmd.Parameters.AddWithValue("@invoice_no", detail.invoice_no);
                            cmd.Parameters.AddWithValue("@purchase_id", newProdID);
                            cmd.Parameters.AddWithValue("@tax_id", detail.tax_id);
                            cmd.Parameters.AddWithValue("@unit_price", detail.unit_price);
                            cmd.Parameters.AddWithValue("@quantity", detail.quantity);
                            cmd.Parameters.AddWithValue("@discount_value", detail.discount);
                            cmd.Parameters.AddWithValue("@tax_rate", detail.tax_rate);
                            cmd.Parameters.AddWithValue("@cost_price", detail.cost_price);
                            cmd.Parameters.AddWithValue("@supplier_id", detail.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_date", detail.purchase_date);

                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();
                        }

                        
                        transaction.Commit();

                        //insert log when trans commit
                        foreach (Purchases_orderModal purchase_header in purchases)
                        {
                            Log.LogAction("Add Purchase Order", $"InvoiceNo: {purchase_header.invoice_no}, Purchase Date: {purchase_header.purchase_date}, Total Amount: {((purchase_header.total_amount + purchase_header.total_tax) - purchase_header.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        }
                        //
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
                return newProdID;
            }
        }

        public int InsertPurchases_order(Purchases_orderModal obj)
        {
            Int32 newProdID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                       
                        cmd = new SqlCommand("sp_Purchases_order", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@employee_id", obj.employee_id);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@purchase_type", obj.purchase_type);
                        cmd.Parameters.AddWithValue("@delivery_date", obj.delivery_date);
                        cmd.Parameters.AddWithValue("@supplier_invoice_no", obj.supplier_invoice_no);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
                        cmd.Parameters.AddWithValue("@total_tax", obj.total_tax);
                        cmd.Parameters.AddWithValue("@discount_value", obj.total_discount);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@account", obj.account);
                        cmd.Parameters.AddWithValue("@category_code", obj.category_code);
                        //cmd.Parameters.AddWithValue("@purchase_time", obj.purchase_time);
                        cmd.Parameters.AddWithValue("@OperationType", "1");

                   
                    }

                    newProdID = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Add Purchase Order", $"InvoiceNo: {obj.invoice_no}, Purchase Date: {obj.purchase_date}, Total Amount: {((obj.total_amount+obj.total_tax) - obj.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)newProdID;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int InsertPurchases_orderItems(Purchases_orderModal obj)
        {

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        //cmd = new SqlCommand("sp_Purchases_order_items", cn);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        //cmd.Parameters.AddWithValue("@item_code", obj.code);
                        //cmd.Parameters.AddWithValue("@serialNo", obj.serialNo);
                        //cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        //cmd.Parameters.AddWithValue("@purchase_id", obj.purchase_id);
                        //cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                        //cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        //cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                        //cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                        //cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                        //cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        //cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        //cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        //cmd.Parameters.AddWithValue("@OperationType", "1");

                        

                    }

                    int result = Convert.ToInt32(cmd.ExecuteScalar());
                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Insert_Purchase_order_new(Purchases_orderModal obj, DataTable dt)
        {
            Int32 newPurchaseID = 0;
            SqlTransaction trans;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    trans = cn.BeginTransaction();
                    
                    try
                    {
                        cmd = new SqlCommand("sp_Purchases_order", cn,trans);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@employee_id", obj.employee_id);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@purchase_type", obj.purchase_type);
                        cmd.Parameters.AddWithValue("@delivery_date", obj.delivery_date);
                        cmd.Parameters.AddWithValue("@supplier_invoice_no", obj.supplier_invoice_no);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
                        cmd.Parameters.AddWithValue("@total_tax", obj.total_tax);
                        cmd.Parameters.AddWithValue("@discount_value", obj.total_discount);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@account", obj.account);
                        cmd.Parameters.AddWithValue("@category_code", obj.category_code);
                        //cmd.Parameters.AddWithValue("@purchase_time", obj.purchase_time);
                        cmd.Parameters.AddWithValue("@OperationType", "1");

                        newPurchaseID = Convert.ToInt32(cmd.ExecuteScalar());
                        Log.LogAction("Add Purchase Order", $"InvoiceNo: {obj.invoice_no}, Purchase Date: {obj.purchase_date}, Total Amount: {((obj.total_amount + obj.total_tax) - obj.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                        if (dt.Rows.Count > 0)
                        {
                            foreach (DataRow dr in dt.Rows)
                            {
                                if (dr["purchase_order_qty"].ToString() != "" && double.Parse(dr["purchase_order_qty"].ToString()) != 0)
                                {
                                    cmd = new SqlCommand("sp_Purchases_order_items", cn, trans);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@item_number", int.Parse(dr["item_number"].ToString()));
                                    cmd.Parameters.AddWithValue("@item_code", int.Parse(dr["id"].ToString()));
                                    cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                                    cmd.Parameters.AddWithValue("@purchase_id", newPurchaseID);
                                    //cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                                    cmd.Parameters.AddWithValue("@unit_price", double.Parse(dr["unit_price"].ToString()));
                                    cmd.Parameters.AddWithValue("@quantity", double.Parse(dr["purchase_order_qty"].ToString()));
                                    //cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                                    //cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                                    cmd.Parameters.AddWithValue("@cost_price", Convert.ToDouble(dr["avg_cost"].ToString()));
                                    //cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                                    cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");
                                    cmd.ExecuteScalar();
                                }
                                
                            }
                        }
                        else
                        {
                            trans.Rollback();
                        }
                        trans.Commit();

                    }
                    catch
                    {
                        trans.Rollback();
                        throw;
                    }
                    
                }

                return (int)newPurchaseID;
            }
        }

        public int Update(Purchases_orderModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        //cmd = new SqlCommand("sp_Purchases_order", cn);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@id", obj.id);
                        ////cmd.Parameters.AddWithValue("@branch_id", 0);
                        //cmd.Parameters.AddWithValue("@code", obj.code);
                        //cmd.Parameters.AddWithValue("@name", obj.name);
                        //cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        //cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        //cmd.Parameters.AddWithValue("@avg_cost", obj.cost_price);
                        //cmd.Parameters.AddWithValue("@item_type", obj.item_type);
                        //cmd.Parameters.AddWithValue("@status", 1);
                        //cmd.Parameters.AddWithValue("@description", obj.description);
                       
                        //cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
                        //cmd.Parameters.AddWithValue("@OperationType", "2");
                        
                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                    }

                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch
                {

                    throw;
                }
                
            }
        }

        public int Delete(int Purchases_orderId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchases_order", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Purchases_orderId); 
                        cmd.Parameters.AddWithValue("@OperationType", "3");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Purchase Order", $"PurchasesOrderId: {Purchases_orderId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }


        public DataTable GetReturnPurchase(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT *" +
                            //" SI.id,SI.item_code,SI.quantity_sold,SI.unit_price," +
                            //" SI.discount_value,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate,SI.tax_id," +
                            //" (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat," +
                            //" P.name AS product_name," +
                            //" C.first_name AS customer_name" +
                            " FROM pos_purchases_order P" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.id=SI.item_code" +
                            //" LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE P.branch_id = @branch_id AND P.invoice_no = @invoice_no";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public DataTable GetReturnPurchaseItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT CAST(1 AS BIT) AS chk, SI.invoice_no," +
                            " SI.id,SI.item_code,SI.quantity,SI.unit_price,SI.cost_price,SI.tax_rate,SI.tax_id,SI.discount_value," +
                            " (SI.cost_price*SI.quantity) AS total," +
                            " (SI.cost_price*SI.quantity*SI.tax_rate/100) AS vat," +
                            " P.name AS product_name" +
                            //" C.first_name AS customer_name" +
                            " FROM pos_purchases_order_items SI" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            //" LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE SI.branch_id = @branch_id AND SI.invoice_no = @invoice_no";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }

        public int InsertReturnPurchase(Purchases_orderModal obj)
        {
            Int32 newProdID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchases_order", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@employee_id", obj.employee_id);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@purchase_type", obj.purchase_type);
                        cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
                        cmd.Parameters.AddWithValue("@total_tax", obj.total_tax);
                        cmd.Parameters.AddWithValue("@discount_value", obj.total_discount);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@purchase_time", obj.purchase_time);
                        cmd.Parameters.AddWithValue("@account", obj.account);
                        //cmd.Parameters.AddWithValue("@is_return", obj.is_return);
                        //cmd.Parameters.AddWithValue("@old_invoice_no", obj.old_invoice_no);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@OperationType", "2");

                    }

                    newProdID = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Return Purchase Order", $"InvoiceNo: {obj.invoice_no}, Purchase Date: {obj.purchase_date}, Total Amount: {((obj.total_amount + obj.total_tax) - obj.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                }
                catch
                {

                    throw;
                }
            }
            return (int)newProdID;
        }

        
        public int DeletePurchasesOrder(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable sales_dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        String query = "DELETE FROM pos_purchases_order WHERE invoice_no = @invoice_no AND branch_id = @branch_id" +
                                " DELETE FROM pos_purchases_order_items WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
                              
                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Purchase Order", $"InvoiceNo: {invoice_no}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return result;
                }
                catch
                {

                    throw;
                }
            }

        }
    }
}
