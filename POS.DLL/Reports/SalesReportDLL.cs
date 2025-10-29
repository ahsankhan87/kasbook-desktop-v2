using POS.Core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DLL
{
    public class SalesReportDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private SalesModal info = new SalesModal();


        public DataTable InvoiceReport(string from_date, string to_date, string customer = "", string invoice_no = "", 
            double total_amount= 0, int branch_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            { 
                try
                {
                    if (cn.State == ConnectionState.Closed) 
                    {
                        cn.Open();
                        cmd = new SqlCommand();

                        String query = "SELECT S.sale_date,S.invoice_no,(S.total_amount+S.total_tax-discount_value) as total,S.description," +
                            //" SI.id,SI.item_code,SI.quantity_sold,SI.unit_price,SI.discount_value," +
                            //" IIF(S.account = 'Return',(-SI.unit_price*SI.quantity_sold),(SI.unit_price*SI.quantity_sold)) AS total," +
                            //" SI.tax_rate," +
                            //" IIF(S.account = 'Return',(-SI.unit_price*SI.quantity_sold*SI.tax_rate/100),(SI.unit_price*SI.quantity_sold*SI.tax_rate/100)) AS vat," +
                            //" P.name AS product_name," +
                            " C.first_name AS customer_name" +
                            " FROM pos_sales S" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.id=SI.item_code" +
                            " LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE S.branch_id=@branch_id";

                        
                        if (from_date != "" && to_date != "")
                        {
                            query += " AND S.sale_date BETWEEN @from_date AND @to_date";
                            cmd.Parameters.AddWithValue("@from_date", from_date);
                            cmd.Parameters.AddWithValue("@to_date", to_date);

                        }
                        if (customer != "")
                        {
                            query += " AND C.first_name LIKE @customer";
                            cmd.Parameters.AddWithValue("@customer", string.Format("%{0}%",customer));
                        }

                        if (invoice_no != "")
                        {
                            query += " AND S.invoice_no = @invoice_no";
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);

                        }
                         
                        if (total_amount != 0)
                        {
                            query += " AND (S.total_amount+S.total_tax-discount_value) LIKE @amount";
                            cmd.Parameters.AddWithValue("@amount", string.Format("{0}%", total_amount));

                        }

                        query += " ORDER BY S.id DESC";
                        cmd.CommandText = query;
                        cmd.Connection = cn;
                    }
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

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

        public DataTable SaleReport(DateTime from_date, DateTime to_date, int customer_id = 0, string product_code = "", string sale_type = "", int employee_id = 0,string sale_account="",int branch_id=0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.id,S.sale_date,S.invoice_no," +
                            " C.first_name AS customer_name," +
                            " SI.item_code,SI.item_number,SI.item_name AS product_name,SI.loc_code,SI.quantity_sold,SI.unit_price," +
                            " SI.discount_value,"+
                            " SI.tax_rate," +
                            " IIF(S.account = 'Return',((-SI.unit_price*SI.quantity_sold-SI.discount_value)*SI.tax_rate/100),((SI.unit_price*SI.quantity_sold-SI.discount_value)*SI.tax_rate/100)) AS vat," +
                            " (IIF(S.account = 'Return',(-SI.unit_price*SI.quantity_sold-SI.discount_value),(SI.unit_price*SI.quantity_sold-SI.discount_value))+IIF(S.account = 'Return',((-SI.unit_price*SI.quantity_sold-SI.discount_value)*SI.tax_rate/100),((SI.unit_price*SI.quantity_sold-SI.discount_value)*SI.tax_rate/100))) AS total_with_vat," +
                            " (IIF(S.account = 'Return',(-SI.unit_price*SI.quantity_sold-SI.discount_value),(SI.unit_price*SI.quantity_sold-SI.discount_value))) AS total" +
                            //" IIF(S.account = 'Return',(-SI.unit_price*SI.quantity_sold),(SI.unit_price*SI.quantity_sold)) AS total," +
                            " FROM pos_sales S" +
                            " LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE S.branch_id = @branch_id AND S.sale_date BETWEEN @from_date AND @to_date";
                            
                        if (sale_type != "All")
                        {
                            query += " AND S.sale_type = @sale_type";
                        }
                        if (sale_account != "All")
                        {
                            query += " AND S.account = @account";
                        }
                        if (customer_id != 0)
                        {
                            query += " AND S.customer_id = @customer_id";
                        }
                        if (product_code != "")
                        {
                            query += " AND SI.item_code = @product_code";
                        }
                        if (employee_id != 0)
                        {
                            query += " AND S.employee_id = @employee_id";
                        }

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@from_date", from_date);
                        cmd.Parameters.AddWithValue("@to_date", to_date);
                        cmd.Parameters.AddWithValue("@branch_id", branch_id);

                        if (sale_type != "All")
                        {
                            cmd.Parameters.AddWithValue("@sale_type", sale_type);

                        }
                        if (sale_account != "All")
                        {
                            cmd.Parameters.AddWithValue("@account", sale_account);
                        }
                        if (customer_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@customer_id", customer_id);
                        }
                        if (product_code != "")
                        {
                            cmd.Parameters.AddWithValue("@product_code", product_code);
                        
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

        public DataTable CusomerWiseSaleReport(string from_date, string to_date, int customer_id = 0, string product_code = "", string sale_type = "", int employee_id = 0, string sale_account = "", int branch_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT SUM(S.total_amount) as total_amount," +
                            //" SI.id,SI.item_code,SI.quantity_sold,SI.unit_price, SI.discount_value,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate," +
                            //" (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat," +
                            //" P.name AS product_name," +
                            " C.first_name AS customer_name" +
                            " FROM pos_sales S" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.id=SI.item_code" +
                            " LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE S.branch_id = @branch_id AND S.sale_date BETWEEN @from_date AND @to_date";

                        if (sale_type != "All")
                        {
                            query += " AND S.sale_type = @sale_type";
                        }
                        if (sale_account != "All")
                        {
                            query += " AND S.account = @account";
                        }
                        if (customer_id != 0)
                        {
                            query += " AND S.customer_id = @customer_id";
                        }
                        if (product_code != "")
                        {
                            query += " AND SI.item_code = @product_code";
                        }
                        if (employee_id != 0)
                        {
                            query += " AND S.employee_id = @employee_id";
                        }

                        query += " GROUP BY C.first_name";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@from_date", from_date);
                        cmd.Parameters.AddWithValue("@to_date", to_date);
                        cmd.Parameters.AddWithValue("@branch_id", branch_id);

                        if (sale_type != "All")
                        {
                            cmd.Parameters.AddWithValue("@sale_type", sale_type);

                        }
                        if (sale_account != "All")
                        {
                            cmd.Parameters.AddWithValue("@account", sale_account);
                        }
                        if (customer_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@customer_id", customer_id);
                        }
                        if (product_code != "")
                        {
                            cmd.Parameters.AddWithValue("@product_code", product_code);

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

        public DataTable ProductWiseSaleReport(string from_date, string to_date, int customer_id = 0, string product_code = "", string sale_type = "", int employee_id = 0, string sale_account = "", int branch_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT" +
                            " SUM(SI.quantity_sold) AS qty," +
                            " SI.item_name AS product_name" +
                            //" (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat," +
                            //" P.name AS product_name" +
                            //" C.first_name AS customer_name" +
                            " FROM pos_sales S" +
                            " LEFT JOIN pos_sales_items SI ON S.invoice_no=SI.invoice_no" +
                            //" LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            //" LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE S.branch_id = @branch_id AND S.sale_date BETWEEN @from_date AND @to_date";

                        if (sale_type != "All")
                        {
                            query += " AND S.sale_type = @sale_type";
                        }
                        if (sale_account != "All")
                        {
                            query += " AND S.account = @account";
                        }
                        if (customer_id != 0)
                        {
                            query += " AND S.customer_id = @customer_id";
                        }
                        if (product_code != "")
                        {
                            query += " AND SI.item_code = @product_code";
                        }
                        if (employee_id != 0)
                        {
                            query += " AND S.employee_id = @employee_id";
                        }

                        query += " GROUP BY SI.item_name";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@from_date", from_date);
                        cmd.Parameters.AddWithValue("@to_date", to_date);
                        cmd.Parameters.AddWithValue("@branch_id", branch_id);

                        if (sale_type != "All")
                        {
                            cmd.Parameters.AddWithValue("@sale_type", sale_type);

                        }
                        if (sale_account != "All")
                        {
                            cmd.Parameters.AddWithValue("@account", sale_account);
                        }
                        if (customer_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@customer_id", customer_id);
                        }
                        if (product_code !="")
                        {
                            cmd.Parameters.AddWithValue("@product_code", product_code);

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


        public DataTable categoryWiseSaleReport(string from_date, string to_date, int customer_id = 0, string product_code = "", string sale_type = "", int employee_id = 0, string sale_account = "", int branch_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT (SUM(SI.unit_price*SI.quantity_sold)-SUM(SI.discount_value)) as total_amount," +
                            " SUM(SI.quantity_sold) AS qty," +
                            //" (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat," +
                            " C.name AS category_name" +
                            //" C.first_name AS customer_name" +
                            " FROM pos_sales S" +
                            " LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_categories C ON C.code=P.category_code" +
                            " WHERE S.branch_id = @branch_id AND S.sale_date BETWEEN @from_date AND @to_date";

                        if (sale_type != "All")
                        {
                            query += " AND S.sale_type = @sale_type";
                        }
                        if (sale_account != "All")
                        {
                            query += " AND S.account = @account";
                        }
                        if (customer_id != 0)
                        {
                            query += " AND S.customer_id = @customer_id";
                        }
                        if (product_code != "")
                        {
                            query += " AND SI.item_code = @product_code";
                        }
                        if (employee_id != 0)
                        {
                            query += " AND S.employee_id = @employee_id";
                        }

                        query += " GROUP BY C.name";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@from_date", from_date);
                        cmd.Parameters.AddWithValue("@to_date", to_date);
                        cmd.Parameters.AddWithValue("@branch_id", branch_id);

                        if (sale_type != "All")
                        {
                            cmd.Parameters.AddWithValue("@sale_type", sale_type);

                        }
                        if (sale_account != "All")
                        {
                            cmd.Parameters.AddWithValue("@account", sale_account);
                        }
                        if (customer_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@customer_id", customer_id);
                        }
                        if (product_code != "")
                        {
                            cmd.Parameters.AddWithValue("@product_code", product_code);

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

        public DataTable GetBranchSummary()
        {
            using (SqlConnection conn = new SqlConnection(dbConnection.ConnectionString))
            {
                conn.Open();
                SqlCommand cmd = new SqlCommand("sp_GetBranchSummary", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                return dt;


            }
        }

        public IList<SalesByPaymentMethodDto> GetSalesByPaymentMethod(DateTime? startDate, DateTime? endDate)
        {

            const string sql = @"
                SELECT
                    ISNULL(pm.description, 'Unknown') AS PaymentMethod,
                    COUNT(1)                           AS TransactionCount,
                    SUM(s.total_amount)                 AS TotalAmount
                FROM dbo.pos_sales s
                LEFT JOIN dbo.pos_payment_method pm ON s.payment_method_id = pm.id
                WHERE (@StartDate IS NULL OR s.sale_date >= @StartDate)
                  AND (@EndDate   IS NULL OR s.sale_date < DATEADD(DAY, 1, @EndDate))
                GROUP BY ISNULL(pm.description, 'Unknown')
                ORDER BY PaymentMethod;";

            var results = new List<SalesByPaymentMethodDto>();

            using (SqlConnection conn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@StartDate", SqlDbType.DateTime).Value = (object)startDate ?? DBNull.Value;
                cmd.Parameters.Add("@EndDate", SqlDbType.DateTime).Value = (object)endDate ?? DBNull.Value;

                conn.Open();
                using (var rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
                {
                    var paymentMethodOrdinal = rdr.GetOrdinal("PaymentMethod");
                    var txCountOrdinal = rdr.GetOrdinal("TransactionCount");
                    var totalAmountOrdinal = rdr.GetOrdinal("TotalAmount");

                    while (rdr.Read())
                    {
                        results.Add(new SalesByPaymentMethodDto
                        {
                            PaymentMethod = rdr.IsDBNull(paymentMethodOrdinal) ? null : rdr.GetString(paymentMethodOrdinal),
                            TransactionCount = rdr.GetInt32(txCountOrdinal),
                            TotalAmount = rdr.GetDecimal(totalAmountOrdinal)
                        });
                    }
                }
            }

            return results;
        }
        
    }

    public sealed class SalesByPaymentMethodDto
    {
        public string PaymentMethod { get; set; }
        public int TransactionCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
}
