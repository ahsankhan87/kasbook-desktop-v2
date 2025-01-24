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
    public class PurchasesReportDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private PurchasesModal info = new PurchasesModal();


        public DataTable PurchaseInvoiceReport(string from_date, string to_date, string supplier = "", string supplier_inv_no = "", string invoice_no = "",
            double total_amount = 0, int branch_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();

                        String query = "SELECT S.purchase_date,S.invoice_no,S.total_amount,S.total_tax, (S.total_amount+S.total_tax) as total, S.description,S.supplier_invoice_no," +
                            //" SI.id,SI.item_code,SI.quantity_sold,SI.unit_price,SI.discount_value," +
                            //" IIF(S.account = 'Return',(-SI.unit_price*SI.quantity_sold),(SI.unit_price*SI.quantity_sold)) AS total," +
                            //" SI.tax_rate," +
                            //" IIF(S.account = 'Return',(-SI.unit_price*SI.quantity_sold*SI.tax_rate/100),(SI.unit_price*SI.quantity_sold*SI.tax_rate/100)) AS vat," +
                            //" P.name AS product_name," +
                            " C.first_name AS supplier_name" +
                            " FROM pos_purchases S" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.code=SI.item_code" +
                            " LEFT JOIN pos_suppliers C ON C.id=S.supplier_id" +
                            " WHERE S.branch_id=@branch_id";

                        
                        if (from_date != "" && to_date != "")
                        {
                            query += " AND S.purchase_date BETWEEN @from_date AND @to_date";
                            cmd.Parameters.AddWithValue("@from_date", from_date);
                            cmd.Parameters.AddWithValue("@to_date", to_date);

                        }
                        if (supplier != "")
                        {
                            query += " AND C.first_name LIKE @supplier";
                            cmd.Parameters.AddWithValue("@supplier", string.Format("%{0}%", supplier));
                        }

                        if (supplier_inv_no != "")
                        {
                            query += " AND S.supplier_invoice_no = @supplier_inv_no";
                            cmd.Parameters.AddWithValue("@supplier_inv_no", supplier_inv_no);
                        }


                        if (invoice_no != "")
                        {
                            query += " AND S.invoice_no = @invoice_no";
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);

                        }

                        if (total_amount != 0)
                        {
                            query += " AND S.total_amount = @amount";
                            cmd.Parameters.AddWithValue("@amount", total_amount);

                        }
                        query += " ORDER BY S.id DESC";
                        cmd.CommandText = query;
                        cmd.Connection = cn;
                    }
                    cmd.Parameters.AddWithValue("@branch_id", branch_id);

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

        public DataTable PurchaseReport(DateTime from_date, DateTime to_date, int supplier_id = 0, int product_id = 0, string purchase_type = "", int employee_id=0, int branch_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.purchase_date,S.invoice_no,SI.id,SI.item_code,SI.quantity,SI.cost_price,SI.discount_value," +
                            " ((SI.cost_price*SI.quantity-SI.discount_value)+((SI.cost_price*SI.quantity-SI.discount_value)*SI.tax_rate/100)) AS total," +
                            " SI.tax_rate," +
                            " ((SI.cost_price*SI.quantity-SI.discount_value)*SI.tax_rate/100) AS vat," +
                            " P.name AS product_name,SI.loc_code," +
                            " C.first_name AS supplier_name" +
                            " FROM pos_purchases S" +
                            " LEFT JOIN pos_purchases_items SI ON S.id=SI.purchase_id" +
                            " LEFT JOIN pos_products P ON P.code=SI.item_code" +
                            " LEFT JOIN pos_suppliers C ON C.id=S.supplier_id" +
                            " WHERE S.branch_id=@branch_id AND S.purchase_date BETWEEN @from_date AND @to_date";

                        if (purchase_type != "All")
                        {
                            query += " AND S.purchase_type = @purchase_type ";
                        }
                        if (supplier_id != 0)
                        {
                            query += " AND S.supplier_id  = @supplier_id ";
                        }
                        if (product_id != 0)
                        {
                            query += " AND SI.item_code = @product_id";
                        }
                        if (employee_id != 0)
                        {
                            query += " AND S.employee_id = @employee_id ";
                        }

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@from_date", from_date);
                        cmd.Parameters.AddWithValue("@to_date", to_date);
                        cmd.Parameters.AddWithValue("@branch_id", branch_id);

                        if (purchase_type != "All")
                        {
                            cmd.Parameters.AddWithValue("@purchase_type", purchase_type);
                        }
                        if (supplier_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@supplier_id", supplier_id);
                        }
                        if (product_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@product_id", product_id);

                        } 
                        if (employee_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@employee_id", employee_id);
                        }
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


        public DataTable Hold_PurchaseInvoiceReport(string from_date, string to_date, string supplier = "", string supplier_inv_no = "", string invoice_no = "",
            double total_amount = 0, int branch_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();

                        String query = "SELECT S.purchase_date,S.invoice_no,S.total_amount,S.total_tax, (S.total_amount+S.total_tax) as total, S.description,S.supplier_invoice_no," +
                            //" SI.id,SI.item_code,SI.quantity_sold,SI.unit_price,SI.discount_value," +
                            //" IIF(S.account = 'Return',(-SI.unit_price*SI.quantity_sold),(SI.unit_price*SI.quantity_sold)) AS total," +
                            //" SI.tax_rate," +
                            //" IIF(S.account = 'Return',(-SI.unit_price*SI.quantity_sold*SI.tax_rate/100),(SI.unit_price*SI.quantity_sold*SI.tax_rate/100)) AS vat," +
                            //" P.name AS product_name," +
                            " C.first_name AS supplier_name" +
                            " FROM pos_hold_purchases S" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.code=SI.item_code" +
                            " LEFT JOIN pos_suppliers C ON C.id=S.supplier_id" +
                            " WHERE S.branch_id=@branch_id";
                        
                        
                        if (from_date != "" && to_date != "")
                        {
                            query += " AND S.purchase_date BETWEEN @from_date AND @to_date";
                            cmd.Parameters.AddWithValue("@from_date", from_date);
                            cmd.Parameters.AddWithValue("@to_date", to_date);

                        }
                        if (supplier != "")
                        {
                            query += " AND C.first_name LIKE @supplier";
                            cmd.Parameters.AddWithValue("@supplier", string.Format("%{0}%", supplier));
                        }

                        if (supplier_inv_no != "")
                        {
                            query += " AND S.supplier_invoice_no = @supplier_inv_no";
                            cmd.Parameters.AddWithValue("@supplier_inv_no", supplier_inv_no);
                        }


                        if (invoice_no != "")
                        {
                            query += " AND S.invoice_no = @invoice_no";
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);

                        }

                        if (total_amount != 0)
                        {
                            query += " AND (S.total_amount+S.total_tax) = @amount";
                            cmd.Parameters.AddWithValue("@amount", total_amount);

                        }
                        query += " ORDER BY S.id DESC";
                        cmd.CommandText = query;
                        cmd.Connection = cn;
                    }
                    cmd.Parameters.AddWithValue("@branch_id", branch_id);

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


    }
}
