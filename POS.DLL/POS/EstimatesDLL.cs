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
    public class EstimatesDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private EstimatesModal info = new EstimatesModal();
        public UsersModal user_obj = new UsersModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Estimates", cn);
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
        public DataTable GetAllSales()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT TOP 10000 SI.invoice_no,SI.id,SI.discount_value,SI.total_amount,SI.total_tax,(SI.total_tax+SI.total_amount-SI.discount_value) as total, SI.sale_date,SI.account,SI.sale_type,CONCAT(C.first_name,' ',C.last_name) AS customer_name " +
                            "FROM pos_sales SI " +
                            "LEFT JOIN pos_customers C ON C.id=SI.customer_id" +
                            " WHERE SI.sale_date BETWEEN @FY_from_date AND @FY_to_date AND SI.branch_id = @branch_id Order by id desc ";


                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@FY_from_date", UsersModal.fy_from_date);
                        cmd.Parameters.AddWithValue("@FY_to_date", UsersModal.fy_to_date);

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

        public DataTable GetAllActiveEstimates()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT SI.*,C.first_name AS customer_name " +
                            "FROM pos_estimates SI " +
                            "LEFT JOIN pos_customers C ON C.id=SI.customer_id" +
                            " WHERE SI.status = 0 ORDER BY SI.id desc";

                        cmd = new SqlCommand(query, cn);

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

        public DataTable GetAllEstimates()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT SI.*,(SI.total_tax+SI.total_amount-SI.discount_value) as total,C.first_name AS customer_name " +
                            "FROM pos_estimates SI " +
                            "LEFT JOIN pos_customers C ON C.id=SI.customer_id "+
                            "WHERE SI.sale_date BETWEEN @FY_from_date AND @FY_to_date AND SI.branch_id = @branch_id Order by id desc ";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); 
                        cmd.Parameters.AddWithValue("@FY_from_date", UsersModal.fy_from_date);
                        cmd.Parameters.AddWithValue("@FY_to_date", UsersModal.fy_to_date);
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

        public DataTable GetAllEstimatesItems(int sale_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT SI.invoice_no,SI.id,SI.item_number,SI.item_code,SI.quantity_sold,SI.unit_price,"+
                            " SI.discount_value,(SI.unit_price*SI.quantity_sold-SI.discount_value) AS total,"+
                            " P.name AS product_name, P.code AS product_code " +
                            "FROM pos_estimates_items SI " +
                            "LEFT JOIN pos_products P ON P.item_number=SI.item_number " +
                            "WHERE sale_id LIKE @sale_id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@sale_id", sale_id);
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

        public DataTable SaleReceipt(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                { 
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.sale_date,S.sale_time,S.invoice_no,S.sale_type,S.account,"+
                            " SI.id,SI.item_code,SI.quantity_sold,SI.unit_price,SI.item_number," +
                             " S.discount_value AS total_discount, S.total_tax, S.total_amount, " + 
                             " SI.discount_value,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate,SI.tax_id," +
                            " S.total_tax as vat,"+
                            //" ((SI.unit_price*SI.quantity_sold-SI.discount_value)*SI.tax_rate/100) AS vat," +
                            " P.name AS product_name,P.code,S.description," +
                            " C.first_name AS customer_name, C.vat_no AS customer_vat" +
                            " FROM pos_estimates S" +
                            " LEFT JOIN pos_estimates_items SI ON S.id=SI.sale_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE S.invoice_no LIKE @invoice_no";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
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

        public DataTable EstimateReceipt(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.sale_date,S.sale_time,S.invoice_no,S.sale_type,S.account," +
                            " SI.id,SI.item_code,SI.quantity_sold,SI.unit_price,SI.item_number," +
                            " SI.discount_value,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat," +
                            " P.name AS product_name," +
                            " C.first_name AS customer_name, C.vat_no AS customer_vat" +
                            " FROM pos_estimates S" +
                            " LEFT JOIN pos_estimates_items SI ON S.id=SI.sale_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE S.invoice_no LIKE @invoice_no";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
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

        public DataTable SearchRecord(String condition)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT id,invoice_no,sale_type,sale_date,total_amount,discount_value,total_tax FROM pos_estimates WHERE invoice_no LIKE @invoice_no", cn);
                        //cmd.Parameters.AddWithValue("@invoice_no", condition);
                        cmd.Parameters.AddWithValue("@invoice_no", string.Format("%{0}%", condition));
                        
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

        

        public String GetMaxEstimateInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_estimates", cn);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());

                        if (maxId == "")
                        {
                            return maxId = "E-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(2, 6));
                            intval++;
                            maxId = String.Format("E-{0:000000}", intval);
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

        
    }
}
