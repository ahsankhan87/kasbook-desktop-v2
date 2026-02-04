using POS.Core;
using POS.DAL;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POS.DLL
{
    public class SalesDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private SalesModal info = new SalesModal();
        public UsersModal user_obj = new UsersModal();

        private InventoryDAL inventoryDAL;
        private ProductDLL productDLL;
        public SalesDLL()
        {
            inventoryDAL = new InventoryDAL();
            productDLL = new ProductDLL();
        }

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Sales", cn);
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
                        String query = "SELECT TOP 10000 SI.*,IIF(invoice_subtype_code = '02','Simplified','Standard') AS invoice_subtype, " +
                            "(SI.total_tax+SI.total_amount-SI.discount_value) as total, " +
                            "CONCAT(C.first_name,' ',C.last_name) AS customer " +
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

        public DataTable GetAllSalesItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT SI.invoice_no,SI.id,SI.item_code,SI.item_number,SI.item_name,SI.quantity_sold,SI.unit_price,SI.loc_code," +
                            " SI.discount_value,ABS(SI.unit_price*SI.quantity_sold-SI.discount_value) AS total," +
                            " ((SI.unit_price*SI.quantity_sold-ABS(SI.discount_value))*SI.tax_rate/100) AS vat," +
                            " (ABS((SI.unit_price*SI.quantity_sold-ABS(SI.discount_value))*SI.tax_rate/100) + ABS(SI.unit_price*SI.quantity_sold-ABS(SI.discount_value))) AS net_total, " +
                            " P.name AS product_name, P.code AS product_code " +
                            "FROM pos_sales_items SI " +
                            "LEFT JOIN pos_products P ON P.item_number=SI.item_number " +
                            "WHERE invoice_no = @invoice_no AND SI.branch_id = @branch_id";

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

        public DataTable SaleReceipt(string invoice_no)
        { 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable SaleReceipt_dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.sale_date,S.sale_time,S.invoice_no,S.sale_type,S.account,S.customer_id,S.invoice_subtype_code,S.PONumber," +
                            " IIF(invoice_subtype_code = '02','Simplified','Standard') AS invoice_subtype,S.zatca_qrcode_phase2," +
                            " S.employee_id,S.description,S.account, S.total_amount, S.discount_value, S.total_tax, S.discount_value AS total_discount," +
                            " S.discount_percent AS total_disc_percent,S.discount_value AS total_disc_value,S.flatDiscountValue, " +
                            " SI.id,SI.item_code,SI.item_number,SI.quantity_sold,SI.unit_price,SI.item_name AS product_name,SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity_sold) AS total, " +
                            " ((SI.unit_price*SI.quantity_sold-SI.discount_value)*SI.tax_rate/100) AS vat," +
                            " C.first_name AS customer_name, C.vat_no AS customer_vat, C.RegistrationName AS customer_company, " +
                            " C.StreetName, C.BuildingNumber, C.CitySubdivisionName, C.CityName, C.PostalCode, C.CountryName, C.cr_number AS customer_cr_number," +
                            " U.name AS username," +
                            " SI.loc_code,S.description,SI.item_code as code" +
                            " FROM pos_sales S" +
                            " LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " LEFT JOIN pos_users U ON U.id=S.user_id" +
                            " WHERE S.invoice_no = @invoice_no AND S.branch_id = @branch_id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(SaleReceipt_dt);
                    return SaleReceipt_dt;
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
                            " 0 AS invoice_subtype,0 AS invoice_subtype_code," +
                            " SI.id,SI.item_code,SI.item_number,SI.quantity_sold,SI.unit_price," +
                            " S.discount_value AS total_discount, S.total_tax, S.total_amount, " +
                            " SI.discount_value,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat," +
                            " SI.item_name AS product_name," +
                            " C.first_name AS customer_name, C.vat_no AS customer_vat" +
                            " FROM pos_estimates S" +
                            " LEFT JOIN pos_estimates_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE S.invoice_no = @invoice_no AND S.branch_id = @branch_id";

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

        public DataTable SearchRecord(String condition)
        { 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("SELECT s.*,(s.total_amount+total_tax-discount_value) AS total," +
                            "IIF(s.invoice_subtype_code = '02','Simplified','Standard') AS invoice_subtype,CONCAT(C.first_name,' ',C.last_name) AS customer " +
                            "FROM pos_sales s LEFT JOIN pos_customers C ON C.id=S.customer_id WHERE invoice_no LIKE @invoice_no AND s.branch_id = @branch_id", cn);
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
        public DataTable SearchInvoices(String invoiceNo, DateTime? fromdate, String type, String subtype, DateTime? todate, String status, bool SkipSmallInvoices = true)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();
                        cmd.Connection = cn;

                        var query = new StringBuilder("SELECT s.*,(s.total_amount+total_tax-discount_value) AS total," +
                            "IIF(s.invoice_subtype_code = '02','Simplified','Standard') AS invoice_subtype,CONCAT(C.first_name,' ',C.last_name) AS customer " +
                            "FROM pos_sales s LEFT JOIN pos_customers C ON C.id=S.customer_id WHERE 1=1");

                        if (!string.IsNullOrEmpty(invoiceNo))
                        {
                            query.Append(" AND s.invoice_no LIKE @InvoiceNo");
                            cmd.Parameters.AddWithValue("@InvoiceNo", string.Format("%{0}%", invoiceNo));
                        }
                        if (!string.IsNullOrEmpty(type))
                        {
                            query.Append(" AND s.account = @Type");
                            cmd.Parameters.AddWithValue("@Type", type);
                        }
                        if (!string.IsNullOrEmpty(subtype))
                        {
                            query.Append(" AND s.invoice_subtype_code = @Subtype");
                            cmd.Parameters.AddWithValue("@Subtype", subtype);
                        }
                        if (fromdate.HasValue)
                        {
                            query.Append(" AND s.sale_date >= @FromDate");
                            cmd.Parameters.AddWithValue("@FromDate", fromdate.Value.Date);
                        }

                        if (todate.HasValue)
                        {
                            query.Append(" AND s.sale_date <= @ToDate");
                            cmd.Parameters.AddWithValue("@ToDate", todate.Value.Date);
                        }
                        if (!string.IsNullOrEmpty(status))
                        {
                            query.Append(" AND s.zatca_status = @Status");
                            cmd.Parameters.AddWithValue("@Status", status);
                        }

                        if (!SkipSmallInvoices)
                        {
                            //exclude sales with invoice numbers starting with ZS-00000
                            query.Append(" AND s.invoice_no NOT LIKE 'ZS%'");
                        }

                        query.Append(" AND s.branch_id = @branch_id");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        query.Append(" ORDER BY s.id DESC");
                        cmd.CommandText = query.ToString();

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

        public String GetMaxInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_sales WHERE SUBSTRING(invoice_no, 1,1) = 'S' AND account <> 'Return' AND branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());

                        if (maxId == "")
                        {
                            return maxId = "S-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(2, 6));
                            intval++;
                            maxId = String.Format("S-{0:000000}", intval);
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
        
        public string GenerateSaleInvoiceNo(string prefix = "S", int? branchId = null, DateTime? invoiceDate = null)
        {
            int bId = branchId ?? UsersModal.logged_in_branch_id;
            DateTime d = (invoiceDate ?? DateTime.Now).Date;

            string date = d.ToString("yyyyMMdd");
            string start = prefix + bId + "-" + date + "-";          // e.g. "S1-20260128-"
            string like = start + "%";

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(@"
            SELECT MAX(invoice_no)
            FROM pos_sales
            WHERE branch_id = @branch_id
              AND invoice_no LIKE @like;", cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", bId);
                cmd.Parameters.AddWithValue("@like", like);

                cn.Open();
                string lastRef = Convert.ToString(cmd.ExecuteScalar());

                int newNum = 1;
                if (!string.IsNullOrWhiteSpace(lastRef) && lastRef.StartsWith(start, StringComparison.OrdinalIgnoreCase))
                {
                    // Equivalent to PHP: substr(lastRef, strlen(start))
                    string tail = lastRef.Substring(start.Length); // "0001"
                    int lastNum;
                    if (int.TryParse(tail, out lastNum))
                        newNum = lastNum + 1;
                }

                return start + newNum.ToString("0000"); // "S1-20260128-0001"
            }
        }
        // Add inside SalesDLL class (recommended near other invoice helpers)

        public string GenerateDailyInvoiceNo(string tableName, string invoiceColumn, string prefix, int? branchId = null, DateTime? invoiceDate = null)
        {
            int bId = branchId ?? UsersModal.logged_in_branch_id;
            DateTime d = (invoiceDate ?? DateTime.Now).Date;

            string datePart = d.ToString("yyyyMMdd");
            string start = prefix + bId + "-" + datePart + "-"; // e.g. "SR1-20260128-"
            string like = start + "%";

            using (var cn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand($@"
            SELECT MAX({invoiceColumn})
            FROM {tableName}
            WHERE branch_id = @branch_id
              AND {invoiceColumn} LIKE @like;", cn))
            {
                cmd.Parameters.AddWithValue("@branch_id", bId);
                cmd.Parameters.AddWithValue("@like", like);

                cn.Open();
                string lastRef = Convert.ToString(cmd.ExecuteScalar());

                int newNum = 1;
                if (!string.IsNullOrWhiteSpace(lastRef) && lastRef.StartsWith(start, StringComparison.OrdinalIgnoreCase))
                {
                    string tail = lastRef.Substring(start.Length); // "0001"
                    int lastNum;
                    if (int.TryParse(tail, out lastNum))
                        newNum = lastNum + 1;
                }

                return start + newNum.ToString("0000");
            }
        }

        public String GetMaxSalesReturnInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_sales WHERE SUBSTRING(invoice_no, 1,2) = 'SR' AND branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());

                        if (maxId == "")
                        {
                            return maxId = "SR-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(3, 6));
                            intval++;
                            maxId = String.Format("SR-{00:000000}", intval);
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
        public String GetMaxDebitNoteInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_sales WHERE SUBSTRING(invoice_no, 1,2) = 'DN' AND branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());

                        if (maxId == "")
                        {
                            return maxId = "DN-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(3, 6));
                            intval++;
                            maxId = String.Format("DN-{00:000000}", intval);
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
        public String GetMaxEstimateInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_estimates WHERE SUBSTRING(invoice_no, 1,1) = 'E' AND branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

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

        public int InsertSales(List<SalesModalHeader> sales, List<SalesModal> sales_detail)
        {
            Int32 newSaleID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction;

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    try
                    {
                        int CustomerID = 0;
                        //if customer name not selected in sales form then whatever text is typed in textbox, it will be saved a new customer
                        //else the selected customer id will be used
                        if (sales[0].customer_id == 0 && sales[0].customer_name.Length > 0)
                        {
                            cmd = new SqlCommand("sp_CustomersCrud", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            foreach (SalesModalHeader sale_header in sales)
                            {
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@first_name", sale_header.customer_name);
                                cmd.Parameters.AddWithValue("@status", 1);
                                //cmd.Parameters.AddWithValue("@contact_no", obj.contact_no);
                                cmd.Parameters.AddWithValue("@vat_no", sale_header.customer_vat);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                //cmd.Parameters.AddWithValue("@credit_limit", obj.credit_limit);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@OperationType", "1");
                            }
                            CustomerID = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        else
                        {
                            CustomerID = sales[0].customer_id;
                        }

                        cmd = new SqlCommand("sp_Sales", cn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (SalesModalHeader sale_header in sales)
                        {
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@customer_id", CustomerID);
                            cmd.Parameters.AddWithValue("@employee_id", sale_header.employee_id);
                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                            cmd.Parameters.AddWithValue("@sale_type", sale_header.sale_type);
                            cmd.Parameters.AddWithValue("@invoice_subtype_code", sale_header.invoice_subtype);
                            cmd.Parameters.AddWithValue("@total_amount", sale_header.total_amount);
                            cmd.Parameters.AddWithValue("@total_tax", sale_header.total_tax);
                            cmd.Parameters.AddWithValue("@discount_value", sale_header.total_discount);
                            cmd.Parameters.AddWithValue("@discount_percent", sale_header.total_discount_percent);
                            cmd.Parameters.AddWithValue("@flatDiscountValue", sale_header.flat_discount_value);
                            cmd.Parameters.AddWithValue("@sale_date", sale_header.sale_date);
                            cmd.Parameters.AddWithValue("@sale_time", sale_header.sale_time);
                            cmd.Parameters.AddWithValue("@account", sale_header.account);
                            cmd.Parameters.AddWithValue("@is_return", sale_header.is_return);
                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                            cmd.Parameters.AddWithValue("@estimate_status", sale_header.estimate_status);
                            cmd.Parameters.AddWithValue("@estimate_invoice_no", sale_header.estimate_invoice_no);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@payment_terms_id", sale_header.payment_terms_id);
                            cmd.Parameters.AddWithValue("@payment_method_id", sale_header.payment_method_id);
                            cmd.Parameters.AddWithValue("@PONumber", sale_header.PONumber);

                            cmd.Parameters.AddWithValue("@OperationType", "1");
                        }

                        newSaleID = Convert.ToInt32(cmd.ExecuteScalar());

                        foreach (SalesModal detail in sales_detail)
                        {
                            cmd = new SqlCommand("sp_Sales_items", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@serialNo", detail.serialNo);
                            cmd.Parameters.AddWithValue("@item_id", detail.item_id);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@item_code", detail.code);
                            cmd.Parameters.AddWithValue("@item_name", detail.name);
                            cmd.Parameters.AddWithValue("@item_type", detail.item_type);
                            cmd.Parameters.AddWithValue("@invoice_no", detail.invoice_no);
                            cmd.Parameters.AddWithValue("@sale_id", newSaleID);
                            cmd.Parameters.AddWithValue("@tax_id", detail.tax_id);
                            cmd.Parameters.AddWithValue("@unit_price", detail.unit_price);
                            cmd.Parameters.AddWithValue("@cost_price", detail.cost_price);
                            cmd.Parameters.AddWithValue("@quantity_sold", detail.quantity_sold);
                            cmd.Parameters.AddWithValue("@discount_value", detail.discount);
                            cmd.Parameters.AddWithValue("@discount_percent", detail.discount_percent);
                            cmd.Parameters.AddWithValue("@tax_rate", detail.tax_rate);
                            cmd.Parameters.AddWithValue("@sale_date", detail.sale_date);
                            cmd.Parameters.AddWithValue("@customer_id", detail.customer_id);
                            cmd.Parameters.AddWithValue("@location_code", detail.location_code);
                            cmd.Parameters.AddWithValue("@packet_qty", detail.packet_qty);

                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();
                        }

                        /// JOURNAL ENTRIES
                        foreach (SalesModalHeader sale_header in sales)
                        {
                            if (sale_header.sale_type != "Quotation" && sale_header.sale_type != "Gift")//for sales 
                            {
                                if (sale_header.sale_type == "Cash")
                                {
                                    //////////////
                                    //// BANK ENtRY
                                    if (sale_header.payment_method_text.Contains("Bank") || sale_header.payment_method_text.Contains("bank") || sale_header.payment_method_text.Contains("banks") || sale_header.payment_method_text.Contains("Banks"))
                                    {
                                        ///Bank JOURNAL ENTRY (DEBIT)
                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", sale_header.bankGLAccountID);
                                        cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                        cmd.Parameters.AddWithValue("@debit", (sale_header.total_amount));
                                        cmd.Parameters.AddWithValue("@credit", 0);
                                        cmd.Parameters.AddWithValue("@description", sale_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", 0);
                                        cmd.Parameters.AddWithValue("@supplier_id", 0);
                                        cmd.Parameters.AddWithValue("@bank_id", 0);
                                        cmd.Parameters.AddWithValue("@entry_id", 0);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();
                                        ////
                                        ///
                                        if (sale_header.bank_id != 0)
                                        {
                                            ///ADD ENTRY INTO bank PAYMENT(DEBIT)

                                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                            cmd.Parameters.AddWithValue("@account_id", sale_header.bankGLAccountID);
                                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                            cmd.Parameters.AddWithValue("@debit", (sale_header.total_amount));
                                            cmd.Parameters.AddWithValue("@credit", 0);
                                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@customer_id", 0);
                                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                                            cmd.Parameters.AddWithValue("@bank_id", sale_header.bank_id);
                                            cmd.Parameters.AddWithValue("@entry_id", 0);
                                            cmd.Parameters.AddWithValue("@OperationType", "1");

                                            cmd.ExecuteScalar();
                                            ////
                                        }
                                    } // bank entry end
                                    else// if bank is not selected in payment metd then cash entry will happen
                                    {
                                        ///CASH JOURNAL ENTRY (DEBIT)

                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", sale_header.cash_account_id);
                                        cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                        cmd.Parameters.AddWithValue("@debit", (sale_header.total_amount));
                                        cmd.Parameters.AddWithValue("@credit", 0);
                                        cmd.Parameters.AddWithValue("@description", sale_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", 0);
                                        cmd.Parameters.AddWithValue("@supplier_id", 0);
                                        cmd.Parameters.AddWithValue("@entry_id", 0);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();
                                        ////

                                    }

                                }
                                else //saletype is credit
                                {
                                    ///ACCOUNT RECEIVABLE JOURNAL ENTRY (DEBIT)

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", sale_header.receivable_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                    cmd.Parameters.AddWithValue("@debit", (sale_header.total_amount));
                                    cmd.Parameters.AddWithValue("@credit", 0);
                                    cmd.Parameters.AddWithValue("@description", sale_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());
                                    ////

                                    if (CustomerID != 0)
                                    {
                                        ///ADD ENTRY INTO CUSTOMER PAYMENT(DEBIT)
                                        //Insert_Journal_entry(invoice_no, sales_account_id, net_total, 0, sale_date, txt_description.Text, customer_id, 0, entry_id);

                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", sale_header.sales_account_id);
                                        cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                        cmd.Parameters.AddWithValue("@debit", (sale_header.total_amount));
                                        cmd.Parameters.AddWithValue("@credit", 0);
                                        cmd.Parameters.AddWithValue("@description", sale_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", CustomerID);
                                        cmd.Parameters.AddWithValue("@supplier_id", 0);
                                        cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();
                                        ////
                                    }

                                }
                                ///SALES JOURNAL ENTRY (CREDIT)
                                //Insert_Journal_entry(invoice_no, sales_account_id, 0, net_total, sale_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", sale_header.sales_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", (sale_header.total_amount));
                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();
                                ////

                                /// PURCHASES / COST OF SALE JOURNAL ENTRY (DEBIT)
                                //Insert_Journal_entry(invoice_no, purchases_acc_id, total_cost_amount, 0, sale_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", sale_header.purchases_acc_id);
                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                cmd.Parameters.AddWithValue("@debit", sale_header.total_cost_amount);
                                cmd.Parameters.AddWithValue("@credit", 0);
                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();
                                ////

                                ///INVENTORY JOURNAL ENTRY (CREDIT)
                                //Insert_Journal_entry(invoice_no, inventory_acc_id, 0, total_cost_amount, sale_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", sale_header.inventory_acc_id);
                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", sale_header.total_cost_amount);
                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();
                                //// 


                                if (sale_header.total_discount > 0)
                                {
                                    /// SALES DISCOUNT JOURNAL ENTRY (DEBIT)
                                    //Int32 entry_id = Insert_Journal_entry(invoice_no, sales_discount_acc_id, net_total_discount, 0, sale_date, txt_description.Text, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", sale_header.sales_discount_acc_id);
                                    cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                    cmd.Parameters.AddWithValue("@debit", sale_header.total_discount);
                                    cmd.Parameters.AddWithValue("@credit", 0);
                                    cmd.Parameters.AddWithValue("@description", sale_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());
                                    ////

                                    if (sale_header.sale_type == "Cash") //for cash entry
                                    {
                                        //////////////
                                        //// BANK ENtRY
                                        /// insert tax into bank entry if bank not select then normal cash tax entry happen
                                        if (sale_header.payment_method_text.Contains("Bank") || sale_header.payment_method_text.Contains("bank") || sale_header.payment_method_text.Contains("banks") || sale_header.payment_method_text.Contains("Banks"))
                                        {
                                            ///Bank JOURNAL ENTRY (DEBIT)
                                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                            cmd.Parameters.AddWithValue("@account_id", sale_header.bankGLAccountID);
                                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                            cmd.Parameters.AddWithValue("@debit", 0);
                                            cmd.Parameters.AddWithValue("@credit", sale_header.total_discount);
                                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@customer_id", 0);
                                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                                            cmd.Parameters.AddWithValue("@entry_id", 0);
                                            cmd.Parameters.AddWithValue("@OperationType", "1");

                                            cmd.ExecuteScalar();
                                            ////
                                            ///
                                            if (sale_header.bank_id != 0)
                                            {
                                                ///ADD ENTRY INTO bank PAYMENT(DEBIT)

                                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                                cmd.Parameters.AddWithValue("@account_id", sale_header.bankGLAccountID);
                                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                                cmd.Parameters.AddWithValue("@debit", 0);
                                                cmd.Parameters.AddWithValue("@credit", sale_header.total_discount);
                                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                                cmd.Parameters.AddWithValue("@bank_id", sale_header.bank_id);
                                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                                cmd.ExecuteScalar();
                                                ////
                                            }
                                        } // bank entry end
                                        else // if bank is not selected in payment metd then cash entry will happen
                                        {
                                            ///SALES JOURNAL ENTRY (CREDIT)
                                            //Insert_Journal_entry(invoice_no, sales_account_id, 0, net_total_discount, sale_date, txt_description.Text, 0, 0, 0);

                                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                            cmd.Parameters.AddWithValue("@account_id", sale_header.cash_account_id);
                                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                            cmd.Parameters.AddWithValue("@debit", 0);
                                            cmd.Parameters.AddWithValue("@credit", sale_header.total_discount);
                                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@customer_id", 0);
                                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                                            cmd.Parameters.AddWithValue("@entry_id", 0);
                                            cmd.Parameters.AddWithValue("@OperationType", "1");

                                            cmd.ExecuteScalar();
                                            ////
                                            ///
                                        }
                                    }
                                    else //if credit entry
                                    {
                                        ///ACCOUNT RECEIVABLE JOURNAL ENTRY (CREDIT)
                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", sale_header.receivable_account_id);
                                        cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                        cmd.Parameters.AddWithValue("@debit", 0);
                                        cmd.Parameters.AddWithValue("@credit", sale_header.total_discount);
                                        cmd.Parameters.AddWithValue("@description", sale_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", 0);
                                        cmd.Parameters.AddWithValue("@supplier_id", 0);
                                        cmd.Parameters.AddWithValue("@entry_id", 0);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();

                                        if (CustomerID != 0)
                                        {
                                            ///ADD ENTRY INTO CUSTOMER PAYMENT(Credit)
                                            //Insert_Journal_entry(invoice_no, sales_account_id, net_total_discount, 0, sale_date, txt_description.Text, customer_id, 0, entry_id);

                                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                            cmd.Parameters.AddWithValue("@account_id", sale_header.sales_discount_acc_id);
                                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                            cmd.Parameters.AddWithValue("@debit", 0);
                                            cmd.Parameters.AddWithValue("@credit", sale_header.total_discount);
                                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@customer_id", CustomerID);
                                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                                            cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                            cmd.Parameters.AddWithValue("@OperationType", "1");

                                            cmd.ExecuteScalar();
                                            ////
                                        }
                                    }

                                }
                                ///dicount end here

                                if (sale_header.total_tax > 0)
                                {
                                    if (sale_header.sale_type == "Cash")
                                    {
                                        //////////////
                                        //// BANK ENtRY
                                        /// insert tax into bank entry if bank not select then normal cash tax entry happen
                                        if (sale_header.payment_method_text.Contains("Bank") || sale_header.payment_method_text.Contains("bank") || sale_header.payment_method_text.Contains("banks") || sale_header.payment_method_text.Contains("Banks"))
                                        {
                                            ///Bank JOURNAL ENTRY (DEBIT)
                                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                            cmd.Parameters.AddWithValue("@account_id", sale_header.bankGLAccountID);
                                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                            cmd.Parameters.AddWithValue("@debit", sale_header.total_tax);
                                            cmd.Parameters.AddWithValue("@credit", 0);
                                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@customer_id", 0);
                                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                                            cmd.Parameters.AddWithValue("@entry_id", 0);
                                            cmd.Parameters.AddWithValue("@OperationType", "1");

                                            cmd.ExecuteScalar();
                                            ////
                                            ///
                                            if (sale_header.bank_id != 0)
                                            {
                                                ///ADD ENTRY INTO bank PAYMENT(DEBIT)

                                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                                cmd.CommandType = CommandType.StoredProcedure;
                                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                                cmd.Parameters.AddWithValue("@account_id", sale_header.bankGLAccountID);
                                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                                cmd.Parameters.AddWithValue("@debit", sale_header.total_tax);
                                                cmd.Parameters.AddWithValue("@credit", 0);
                                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                                cmd.Parameters.AddWithValue("@bank_id", sale_header.bank_id);
                                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                                cmd.ExecuteScalar();
                                                ////
                                            }
                                        } // bank entry end
                                        else // if bank is not selected in payment metd then cash entry will happen
                                        {
                                            ///CASH JOURNAL ENTRY (DEBIT)
                                            //Insert_Journal_entry(invoice_no, cash_account_id, net_total_tax, 0, sale_date, txt_description.Text, 0, 0, 0);

                                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                            cmd.Parameters.AddWithValue("@account_id", sale_header.cash_account_id);
                                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                            cmd.Parameters.AddWithValue("@debit", sale_header.total_tax);
                                            cmd.Parameters.AddWithValue("@credit", 0);
                                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@customer_id", 0);
                                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                                            cmd.Parameters.AddWithValue("@entry_id", 0);
                                            cmd.Parameters.AddWithValue("@OperationType", "1");

                                            cmd.ExecuteScalar();
                                            ////
                                            ///
                                        }
                                    }
                                    else
                                    {
                                        ///ACCOUNT RECEIVABLE JOURNAL ENTRY (DEBIT)
                                        //Int32 entry_id = Insert_Journal_entry(invoice_no, receivable_account_id, net_total_tax, 0, sale_date, txt_description.Text, 0, 0, 0);

                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", sale_header.receivable_account_id);
                                        cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                        cmd.Parameters.AddWithValue("@debit", sale_header.total_tax);
                                        cmd.Parameters.AddWithValue("@credit", 0);
                                        cmd.Parameters.AddWithValue("@description", sale_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", 0);
                                        cmd.Parameters.AddWithValue("@supplier_id", 0);
                                        cmd.Parameters.AddWithValue("@entry_id", 0);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());
                                        ////

                                        if (CustomerID != 0)
                                        {
                                            ///ADD ENTRY INTO CUSTOMER PAYMENT(DEBIT)
                                            //Insert_Journal_entry(invoice_no, tax_account_id, net_total_tax, 0, sale_date, txt_description.Text, customer_id, 0, entry_id);

                                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                            cmd.CommandType = CommandType.StoredProcedure;
                                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                            cmd.Parameters.AddWithValue("@account_id", sale_header.tax_account_id);
                                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                            cmd.Parameters.AddWithValue("@debit", sale_header.total_tax);
                                            cmd.Parameters.AddWithValue("@credit", 0);
                                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                            cmd.Parameters.AddWithValue("@customer_id", CustomerID);
                                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                                            cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                            cmd.Parameters.AddWithValue("@OperationType", "1");

                                            cmd.ExecuteScalar();
                                            ////
                                        }

                                    }
                                    ///SALES TAX JOURNAL ENTRY (CREDIT)
                                    //Insert_Journal_entry(invoice_no, tax_account_id, 0, net_total_tax, sale_date, txt_description.Text, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", sale_header.tax_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                    cmd.Parameters.AddWithValue("@debit", 0);
                                    cmd.Parameters.AddWithValue("@credit", sale_header.total_tax);
                                    cmd.Parameters.AddWithValue("@description", sale_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();
                                    ////

                                }

                            }
                        }

                        transaction.Commit();

                        //insert log when trans commit
                        foreach (SalesModalHeader sale_header in sales)
                        {
                            Log.LogAction("Add Sales", $"InvoiceNo: {sale_header.invoice_no}, Sale Date: {sale_header.sale_date}, Total Amount: {((sale_header.total_amount + sale_header.total_tax) - sale_header.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        }
                        //
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }


                return newSaleID;

            }
        }
        public int ict_qty_request(List<SalesModalHeader> sales, List<SalesModal> sales_detail)// for request qty from branch
        {
            int newSaleID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction;

                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    try
                    {
                        foreach (SalesModal detail in sales_detail)
                        {
                            cmd = new SqlCommand("sp_ict_transfer", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@source_branch_id", detail.source_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@item_code", detail.code);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@destination_branch_id", detail.destination_branch_id);
                            cmd.Parameters.AddWithValue("@quantity_requested", detail.quantity_sold);
                            cmd.Parameters.AddWithValue("@requested_date", detail.sale_date);

                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            newSaleID = int.Parse(Convert.ToString(cmd.ExecuteScalar()));

                            Log.LogAction("ICT Request", $"Source Branch Id: {detail.source_branch_id}, Destination Branch Id: {detail.destination_branch_id}, Item Code={detail.code}, Quantity Released={detail.quantity_sold}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                        }

                        transaction.Commit();

                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }


                return newSaleID;

            }
        }

        public int InsertSales_1(SalesModal obj)
        {
            Int32 newProdID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Sales", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@customer_id", obj.customer_id);
                        cmd.Parameters.AddWithValue("@employee_id", obj.employee_id);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@sale_type", obj.sale_type);
                        cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
                        cmd.Parameters.AddWithValue("@total_tax", obj.total_tax);
                        cmd.Parameters.AddWithValue("@discount_value", obj.total_discount);
                        cmd.Parameters.AddWithValue("@discount_percent", obj.total_discount_percent);
                        cmd.Parameters.AddWithValue("@sale_date", obj.sale_date);
                        cmd.Parameters.AddWithValue("@sale_time", obj.sale_time);
                        cmd.Parameters.AddWithValue("@account", obj.account);
                        cmd.Parameters.AddWithValue("@is_return", obj.is_return);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@estimate_status", obj.estimate_status);
                        cmd.Parameters.AddWithValue("@estimate_invoice_no", obj.estimate_invoice_no);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@payment_terms_id", obj.payment_terms_id);
                        cmd.Parameters.AddWithValue("@payment_method_id", obj.payment_method_id);

                        cmd.Parameters.AddWithValue("@OperationType", "1");

                    }

                    newProdID = Convert.ToInt32(cmd.ExecuteScalar());

                    return (int)newProdID;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int InsertSalesItems(SalesModal obj)
        {

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Sales_items", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@item_code", obj.code);
                        cmd.Parameters.AddWithValue("@item_name", obj.name);
                        cmd.Parameters.AddWithValue("@item_type", obj.item_type);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@sale_id", obj.sale_id);
                        cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@quantity_sold", obj.quantity_sold);
                        cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                        cmd.Parameters.AddWithValue("@discount_percent", obj.discount_percent);
                        cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                        cmd.Parameters.AddWithValue("@sale_date", obj.sale_date);
                        cmd.Parameters.AddWithValue("@customer_id", obj.customer_id);
                        cmd.Parameters.AddWithValue("@location_code", obj.location_code);
                        cmd.Parameters.AddWithValue("@packet_qty", obj.packet_qty);

                        cmd.Parameters.AddWithValue("@OperationType", "1");


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

        public DataTable GetReturnSales(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT *" +
                            //" SI.id,SI.item_code,SI.quantity_sold,SI.unit_price," +
                            //" SI.discount_value,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate,SI.tax_id," +
                            //" (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat," +
                            //" P.name AS product_name," +
                            //" C.first_name AS customer_name" +
                            " FROM pos_sales S" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.id=SI.item_code" +
                            //" LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE S.invoice_no = @invoice_no AND S.branch_id=@branch_id";

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

        public DataTable GetReturnSaleItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT SI.id,SI.item_code,SI.item_number,CAST(1 AS BIT) AS chk, SI.invoice_no,SI.quantity_sold AS return_qty,SI.packet_qty," +
                            " SI.quantity_sold,SI.unit_price,SI.cost_price,SI.tax_rate,SI.tax_id,SI.discount_value,SI.loc_code," +
                            " (SI.unit_price*SI.quantity_sold-SI.discount_value)+((SI.unit_price*SI.quantity_sold-SI.discount_value)*SI.tax_rate/100) AS total," +
                            " ((SI.unit_price*SI.quantity_sold-SI.discount_value)*SI.tax_rate/100) AS vat," +
                            " SI.item_name AS product_name,(SI.quantity_sold - ISNULL(r.TotalReturnedQty,0)) AS ReturnQty," +
                            " ISNULL(r.TotalReturnedQty,0) AS ReturnedQty,(SI.quantity_sold - ISNULL(r.TotalReturnedQty,0)) AS ReturnableQty" +
                            //" C.first_name AS customer_name" +
                            " FROM pos_sales_items SI" +
                            " LEFT JOIN (SELECT ItemNumber, SUM(QtyReturned) AS TotalReturnedQty FROM pos_salesReturn WHERE OriginalInvoiceNo = @invoice_no GROUP BY ItemNumber) r ON r.ItemNumber = SI.item_number" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.code=SI.item_code" +
                            //" LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE SI.invoice_no = @invoice_no AND SI.branch_id=@branch_id" +
                            " Order by SI.id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt);

                    // Add input column for new return qty (not persisted yet)
                    //if (!dt.Columns.Contains("ReturnQty"))
                    //    dt.Columns.Add("ReturnQty", typeof(decimal));
                    //foreach (DataRow r in dt.Rows)
                    //    r["ReturnQty"] = 0m;

                    return dt;
                }
                catch
                {

                    throw;
                }
            }

        }
        public DataTable GetSalesReturnedItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT *" +
                            " FROM pos_salesReturn" +
                            " WHERE invoice_no = @invoice_no AND branch_id=@branch_id";

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
        public int InsertReturnSales(List<SalesModalHeader> sales, List<SalesModal> sales_detail)
        {
            Int32 newSaleID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction;
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    try
                    {
                        cmd = new SqlCommand("sp_Sales", cn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (SalesModalHeader sale_header in sales)
                        {
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@customer_id", sale_header.customer_id);
                            cmd.Parameters.AddWithValue("@employee_id", sale_header.employee_id);
                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                            cmd.Parameters.AddWithValue("@sale_type", sale_header.sale_type);
                            cmd.Parameters.AddWithValue("@invoice_subtype_code", sale_header.invoice_subtype);
                            cmd.Parameters.AddWithValue("@total_amount", sale_header.total_amount);
                            cmd.Parameters.AddWithValue("@total_tax", sale_header.total_tax);
                            cmd.Parameters.AddWithValue("@discount_value", sale_header.total_discount);
                            //cmd.Parameters.AddWithValue("@discount_percent", sale_header.total_discount_percent);
                            cmd.Parameters.AddWithValue("@sale_date", sale_header.sale_date);
                            cmd.Parameters.AddWithValue("@sale_time", sale_header.sale_time);
                            cmd.Parameters.AddWithValue("@account", sale_header.account);
                            cmd.Parameters.AddWithValue("@is_return", sale_header.is_return);
                            cmd.Parameters.AddWithValue("@description", sale_header.description);

                            cmd.Parameters.AddWithValue("@old_invoice_no", sale_header.old_invoice_no);
                            cmd.Parameters.AddWithValue("@previousInvoiceDate", sale_header.previousInvoiceDate);
                            cmd.Parameters.AddWithValue("@returnReasonCode", sale_header.returnReasonCode);
                            cmd.Parameters.AddWithValue("@returnReason", sale_header.returnReason);
                            //cmd.Parameters.AddWithValue("@estimate_status", sale_header.estimate_status);
                            //cmd.Parameters.AddWithValue("@estimate_invoice_no", sale_header.estimate_invoice_no);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            //cmd.Parameters.AddWithValue("@payment_terms_id", sale_header.payment_terms_id);
                            //cmd.Parameters.AddWithValue("@payment_method_id", sale_header.payment_method_id);

                            cmd.Parameters.AddWithValue("@OperationType", "2");
                        }

                        newSaleID = Convert.ToInt32(cmd.ExecuteScalar());

                        foreach (SalesModal detail in sales_detail)
                        {
                            cmd = new SqlCommand("sp_Sales_items", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@old_invoice_no", sales[0].old_invoice_no);
                            cmd.Parameters.AddWithValue("@previousInvoiceDate", sales[0].previousInvoiceDate);
                            cmd.Parameters.AddWithValue("@returnReasonCode", sales[0].returnReasonCode);
                            cmd.Parameters.AddWithValue("@returnReason", sales[0].returnReason);

                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@item_code", detail.code);
                            cmd.Parameters.AddWithValue("@item_name", detail.name);
                            cmd.Parameters.AddWithValue("@item_type", detail.item_type);
                            cmd.Parameters.AddWithValue("@invoice_no", detail.invoice_no);
                            cmd.Parameters.AddWithValue("@sale_id", newSaleID);
                            cmd.Parameters.AddWithValue("@tax_id", detail.tax_id);
                            cmd.Parameters.AddWithValue("@unit_price", detail.unit_price);
                            cmd.Parameters.AddWithValue("@cost_price", detail.cost_price);
                            cmd.Parameters.AddWithValue("@quantity_sold", detail.quantity_sold);
                            cmd.Parameters.AddWithValue("@discount_value", detail.discount);
                            //cmd.Parameters.AddWithValue("@discount_percent", detail.discount_percent);
                            cmd.Parameters.AddWithValue("@tax_rate", detail.tax_rate);
                            cmd.Parameters.AddWithValue("@sale_date", detail.sale_date);
                            cmd.Parameters.AddWithValue("@customer_id", detail.customer_id);
                            cmd.Parameters.AddWithValue("@location_code", detail.location_code);
                            cmd.Parameters.AddWithValue("@packet_qty", detail.packet_qty);

                            cmd.Parameters.AddWithValue("@OperationType", "2");

                            cmd.ExecuteScalar();
                        }

                        ///
                        /// for journal entries 
                        /// 
                        foreach (SalesModalHeader sale_header in sales)
                        {

                            if (sale_header.sale_type == "Cash")
                            {
                                ///CASH JOURNAL ENTRY (cr)
                                //Insert_Journal_entry(invoice_no, cash_account_id, net_total, 0, sale_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", sale_header.cash_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", (sale_header.total_amount));
                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();
                                ////
                            }
                            else if (sale_header.sale_type == "Credit")//saletype is credit
                            {
                                ///ACCOUNT RECEIVABLE JOURNAL ENTRY (cr)
                                //Int32 entry_id = Insert_Journal_entry(invoice_no, receivable_account_id, net_total, 0, sale_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", sale_header.receivable_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", (sale_header.total_amount));
                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());
                                ////

                                if (sale_header.customer_id != 0)
                                {
                                    ///ADD ENTRY INTO CUSTOMER PAYMENT(cr)
                                    //Insert_Journal_entry(invoice_no, sales_account_id, net_total, 0, sale_date, txt_description.Text, customer_id, 0, entry_id);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", sale_header.sales_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                    cmd.Parameters.AddWithValue("@debit", 0);
                                    cmd.Parameters.AddWithValue("@credit", (sale_header.total_amount));
                                    cmd.Parameters.AddWithValue("@description", sale_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", sale_header.customer_id);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();
                                    ////
                                }

                            }
                            ///SALES JOURNAL ENTRY (dr)
                            //Insert_Journal_entry(invoice_no, sales_account_id, 0, net_total, sale_date, txt_description.Text, 0, 0, 0);

                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                            cmd.Parameters.AddWithValue("@account_id", sale_header.sales_account_id);
                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                            cmd.Parameters.AddWithValue("@debit", (sale_header.total_amount));
                            cmd.Parameters.AddWithValue("@credit", 0);
                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            cmd.Parameters.AddWithValue("@customer_id", 0);
                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                            cmd.Parameters.AddWithValue("@entry_id", 0);
                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();
                            ////

                            /// PURCHASES / COST OF SALE JOURNAL ENTRY (cr)
                            //Insert_Journal_entry(invoice_no, purchases_acc_id, total_cost_amount, 0, sale_date, txt_description.Text, 0, 0, 0);

                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                            cmd.Parameters.AddWithValue("@account_id", sale_header.purchases_acc_id);
                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                            cmd.Parameters.AddWithValue("@debit", 0);
                            cmd.Parameters.AddWithValue("@credit", sale_header.total_cost_amount);
                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            cmd.Parameters.AddWithValue("@customer_id", 0);
                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                            cmd.Parameters.AddWithValue("@entry_id", 0);
                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();
                            ////

                            ///INVENTORY JOURNAL ENTRY (dr)
                            //Insert_Journal_entry(invoice_no, inventory_acc_id, 0, total_cost_amount, sale_date, txt_description.Text, 0, 0, 0);

                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                            cmd.Parameters.AddWithValue("@account_id", sale_header.inventory_acc_id);
                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                            cmd.Parameters.AddWithValue("@debit", sale_header.total_cost_amount);
                            cmd.Parameters.AddWithValue("@credit", 0);
                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            cmd.Parameters.AddWithValue("@customer_id", 0);
                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                            cmd.Parameters.AddWithValue("@entry_id", 0);
                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();
                            ////


                            if (sale_header.total_discount > 0)
                            {
                                /// SALES DISCOUNT JOURNAL ENTRY (cr)
                                //Int32 entry_id = Insert_Journal_entry(invoice_no, sales_discount_acc_id, net_total_discount, 0, sale_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", sale_header.sales_discount_acc_id);
                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", sale_header.total_discount);
                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());
                                ////
                                if (sale_header.sale_type == "Cash") //for cash entry
                                {
                                    ///SALES JOURNAL ENTRY (CREDIT)
                                    //Insert_Journal_entry(invoice_no, sales_account_id, 0, net_total_discount, sale_date, txt_description.Text, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", sale_header.cash_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                    cmd.Parameters.AddWithValue("@debit", sale_header.total_discount);
                                    cmd.Parameters.AddWithValue("@credit", 0);
                                    cmd.Parameters.AddWithValue("@description", sale_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();
                                    ////
                                }
                                else //if credit entry sale_header.sale_type == "Credit" &&
                                {
                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", sale_header.receivable_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                    cmd.Parameters.AddWithValue("@debit", sale_header.total_discount);
                                    cmd.Parameters.AddWithValue("@credit", 0);
                                    cmd.Parameters.AddWithValue("@description", sale_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();


                                    ////

                                    if (sale_header.customer_id != 0)
                                    {
                                        ///ADD ENTRY INTO CUSTOMER PAYMENT(cr)
                                        //Insert_Journal_entry(invoice_no, sales_account_id, net_total_discount, 0, sale_date, txt_description.Text, customer_id, 0, entry_id);

                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", sale_header.sales_discount_acc_id);
                                        cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                        cmd.Parameters.AddWithValue("@debit", sale_header.total_discount);
                                        cmd.Parameters.AddWithValue("@credit", 0);
                                        cmd.Parameters.AddWithValue("@description", sale_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", sale_header.customer_id);
                                        cmd.Parameters.AddWithValue("@supplier_id", 0);
                                        cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();
                                        ////
                                    }
                                }
                            }

                            if (sale_header.total_tax > 0)
                            {
                                if (sale_header.sale_type == "Cash")
                                {
                                    ///CASH JOURNAL ENTRY (cr)
                                    //Insert_Journal_entry(invoice_no, cash_account_id, net_total_tax, 0, sale_date, txt_description.Text, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", sale_header.cash_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                    cmd.Parameters.AddWithValue("@debit", 0);
                                    cmd.Parameters.AddWithValue("@credit", sale_header.total_tax);
                                    cmd.Parameters.AddWithValue("@description", sale_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();
                                    ////
                                }
                                else if (sale_header.sale_type == "Credit")
                                {
                                    ///ACCOUNT RECEIVABLE JOURNAL ENTRY (DEBIT)
                                    //Int32 entry_id = Insert_Journal_entry(invoice_no, receivable_account_id, net_total_tax, 0, sale_date, txt_description.Text, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", sale_header.receivable_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                    cmd.Parameters.AddWithValue("@debit", 0);
                                    cmd.Parameters.AddWithValue("@credit", sale_header.total_tax);
                                    cmd.Parameters.AddWithValue("@description", sale_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());
                                    ////

                                    if (sale_header.customer_id != 0)
                                    {
                                        ///ADD ENTRY INTO CUSTOMER PAYMENT(cr)
                                        //Insert_Journal_entry(invoice_no, tax_account_id, net_total_tax, 0, sale_date, txt_description.Text, customer_id, 0, entry_id);

                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", sale_header.tax_account_id);
                                        cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                        cmd.Parameters.AddWithValue("@debit", 0);
                                        cmd.Parameters.AddWithValue("@credit", sale_header.total_tax);
                                        cmd.Parameters.AddWithValue("@description", sale_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", sale_header.customer_id);
                                        cmd.Parameters.AddWithValue("@supplier_id", 0);
                                        cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();
                                        ////
                                    }

                                }
                                ///SALES TAX JOURNAL ENTRY (dr)
                                //Insert_Journal_entry(invoice_no, tax_account_id, 0, net_total_tax, sale_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", sale_header.tax_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                cmd.Parameters.AddWithValue("@debit", sale_header.total_tax);
                                cmd.Parameters.AddWithValue("@credit", 0);
                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();
                                ////

                            }

                        }
                        ///

                        transaction.Commit();
                        //insert log when trans commit
                        foreach (SalesModalHeader sale_header in sales)
                        {
                            Log.LogAction("Add Return Sales", $"InvoiceNo: {sale_header.invoice_no}, Sale Date: {sale_header.sale_date}, Total Amount: {((sale_header.total_amount + sale_header.total_tax) - sale_header.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        }
                        //
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return (int)newSaleID;

            }
        }

        public int InsertReturnSalesItems(SalesModal obj)
        {

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Sales_items", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@item_code", obj.code);
                        cmd.Parameters.AddWithValue("@item_number", obj.item_number);
                        cmd.Parameters.AddWithValue("@item_name", obj.name);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@sale_id", obj.sale_id);
                        cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@quantity_sold", obj.quantity_sold);
                        cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                        cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@sale_date", obj.sale_date);
                        cmd.Parameters.AddWithValue("@customer_id", obj.customer_id);
                        cmd.Parameters.AddWithValue("@location_code", obj.location_code);
                        cmd.Parameters.AddWithValue("@OperationType", "2");


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

        public int Update(SalesModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Sales", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        //cmd.Parameters.AddWithValue("@branch_id", 0);
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@avg_cost", obj.cost_price);
                        cmd.Parameters.AddWithValue("@item_type", obj.item_type);
                        cmd.Parameters.AddWithValue("@status", 1);
                        cmd.Parameters.AddWithValue("@description", obj.description);

                        cmd.Parameters.AddWithValue("@date_updated", obj.sale_time);
                        cmd.Parameters.AddWithValue("@OperationType", "2");

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

        public int Delete(int SalesId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Sales", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", SalesId);
                        cmd.Parameters.AddWithValue("@OperationType", "3");

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


        public int InsertEstimates(List<SalesModalHeader> sales, List<SalesModal> sales_detail)
        {
            Int32 newSaleID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction;



                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    try
                    {
                        int CustomerID = 0;
                        if (sales[0].customer_id == 0 && sales[0].customer_name.Length > 0)
                        {


                            cmd = new SqlCommand("sp_CustomersCrud", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            foreach (SalesModalHeader sale_header in sales)
                            {
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@first_name", sale_header.customer_name);
                                cmd.Parameters.AddWithValue("@status", 1);
                                //cmd.Parameters.AddWithValue("@contact_no", obj.contact_no);
                                cmd.Parameters.AddWithValue("@vat_no", sale_header.customer_vat);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                //cmd.Parameters.AddWithValue("@credit_limit", obj.credit_limit);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@OperationType", "1");
                            }
                            CustomerID = Convert.ToInt32(cmd.ExecuteScalar());
                        }
                        else
                        {
                            CustomerID = sales[0].customer_id;
                        }

                        cmd = new SqlCommand("sp_estimates", cn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (SalesModalHeader sale_header in sales)
                        {
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@customer_id", CustomerID);
                            cmd.Parameters.AddWithValue("@employee_id", sale_header.employee_id);
                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                            cmd.Parameters.AddWithValue("@sale_type", sale_header.sale_type);
                            cmd.Parameters.AddWithValue("@total_amount", sale_header.total_amount);
                            cmd.Parameters.AddWithValue("@total_tax", sale_header.total_tax);
                            cmd.Parameters.AddWithValue("@discount_value", sale_header.total_discount);
                            cmd.Parameters.AddWithValue("@discount_percent", sale_header.total_discount_percent);
                            cmd.Parameters.AddWithValue("@flatDiscountValue", sale_header.flat_discount_value);
                            cmd.Parameters.AddWithValue("@sale_date", sale_header.sale_date);
                            cmd.Parameters.AddWithValue("@sale_time", sale_header.sale_time);
                            cmd.Parameters.AddWithValue("@account", sale_header.account);
                            //cmd.Parameters.AddWithValue("@is_return", sale_header.is_return);
                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                            //cmd.Parameters.AddWithValue("@estimate_status", sale_header.estimate_status);
                            //cmd.Parameters.AddWithValue("@estimate_invoice_no", sale_header.estimate_invoice_no);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@payment_terms_id", sale_header.payment_terms_id);
                            cmd.Parameters.AddWithValue("@payment_method_id", sale_header.payment_method_id);

                            cmd.Parameters.AddWithValue("@OperationType", "1");
                        }


                        newSaleID = Convert.ToInt32(cmd.ExecuteScalar());


                        foreach (SalesModal detail in sales_detail)
                        {
                            cmd = new SqlCommand("sp_estimates_items", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@serialNo", detail.serialNo);
                            cmd.Parameters.AddWithValue("@item_code", detail.code);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@item_name", detail.name);
                            cmd.Parameters.AddWithValue("@item_type", detail.item_type);
                            cmd.Parameters.AddWithValue("@invoice_no", detail.invoice_no);
                            cmd.Parameters.AddWithValue("@sale_id", newSaleID);
                            cmd.Parameters.AddWithValue("@tax_id", detail.tax_id);
                            cmd.Parameters.AddWithValue("@unit_price", detail.unit_price);
                            cmd.Parameters.AddWithValue("@cost_price", detail.cost_price);
                            cmd.Parameters.AddWithValue("@quantity_sold", detail.quantity_sold);
                            cmd.Parameters.AddWithValue("@discount_value", detail.discount);
                            cmd.Parameters.AddWithValue("@discount_percent", detail.discount_percent);
                            cmd.Parameters.AddWithValue("@tax_rate", detail.tax_rate);
                            cmd.Parameters.AddWithValue("@sale_date", detail.sale_date);
                            cmd.Parameters.AddWithValue("@customer_id", detail.customer_id);
                            cmd.Parameters.AddWithValue("@location_code", detail.location_code);
                            cmd.Parameters.AddWithValue("@packet_qty", detail.packet_qty);

                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();
                        }

                        transaction.Commit();

                        //insert log when trans commit
                        foreach (SalesModalHeader sale_header in sales)
                        {
                            Log.LogAction("Add Estimate", $"InvoiceNo: {sale_header.invoice_no}, Sale Date: {sale_header.sale_date}, Total Amount: {((sale_header.total_amount + sale_header.total_tax) - sale_header.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        }
                        //
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }


                return newSaleID;

            }
        }
        public void UpdateZatcaStatus(string invoiceNo, string status, string ublPath, string errorMessage)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand(@"UPDATE pos_sales
                SET zatca_status = @status,
                    zatca_ubl_path = @path,
                    zatca_error_message = @error
                WHERE invoice_no = @invoice", cn))
            {
                cmd.Parameters.AddWithValue("@status", status);
                cmd.Parameters.AddWithValue("@path", (object)ublPath ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@error", (object)errorMessage ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@invoice", invoiceNo);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }
        public void UpdateZatcaQrCode(string invoiceNo, byte[] qrCode)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("UPDATE pos_sales SET zatca_qrcode_phase2 = @qr WHERE invoice_no = @invoice", cn))
            {
                cmd.Parameters.AddWithValue("@qr", qrCode);
                cmd.Parameters.AddWithValue("@invoice", invoiceNo);

                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public string GetUblPath(string invoiceNo)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT zatca_ubl_path FROM pos_sales WHERE invoice_no = @invoice", cn))
            {
                cmd.Parameters.AddWithValue("@invoice", invoiceNo);
                cn.Open();
                return Convert.ToString(cmd.ExecuteScalar());
            }
        }
        public string GetInvoiceTypeCode(string invoiceNo)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            using (SqlCommand cmd = new SqlCommand("SELECT invoice_subtype_code FROM pos_sales WHERE invoice_no = @invoice", cn))
            {
                cmd.Parameters.AddWithValue("@invoice", invoiceNo);
                cn.Open();
                return Convert.ToString(cmd.ExecuteScalar());
            }
        }
        public DataSet GetSaleAndItemsDataSet(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;

                // Query 1: Sale Header
                string headerSql = "SELECT s.*, C.* FROM pos_sales s" +
                    " LEFT JOIN pos_customers C ON C.id = s.customer_id" +
                    " WHERE invoice_no = @invoice_no AND s.branch_id = @branch_id";
                // Query 2: Items
                string itemsSql = "SELECT SI.id,SI.item_code,SI.quantity_sold,SI.unit_price,SI.cost_price,SI.loc_code,SI.packet_qty,SI.item_number," +
                            " SI.discount_value,SI.discount_percent,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate,SI.tax_id," +
                            " ((SI.unit_price*SI.quantity_sold-SI.discount_value)*SI.tax_rate/100) AS vat, SI.item_number, " +
                                  "P.name, P.code, U.name AS unit_name, CT.name AS category_name " +
                                  "FROM pos_sales_items SI " +
                                  "LEFT JOIN pos_products P ON P.item_number = SI.item_number " +
                                  "LEFT JOIN pos_units U ON U.id = SI.unit_id " +
                                  "LEFT JOIN pos_categories CT ON CT.code = P.category_code " +
                                  "WHERE SI.invoice_no = @invoice_no";

                SqlDataAdapter da = new SqlDataAdapter();

                cmd.CommandText = headerSql;
                cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                da.SelectCommand = cmd;
                DataSet ds = new DataSet();

                da.Fill(ds, "Sale");

                cmd.CommandText = itemsSql;
                da.SelectCommand = cmd;

                da.Fill(ds, "SalesItems");

                return ds;
            }
        }

        public DataTable GetSaleAndSalesItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.sale_date,S.sale_time,S.invoice_no,S.sale_type,S.account,S.customer_id,S.employee_id,S.description,S.account," +
                            " S.discount_percent AS total_disc_percent,S.discount_value AS total_disc_value,S.total_amount,S.flatDiscountValue,S.invoice_subtype_code," +
                            " SI.id,SI.item_code,SI.quantity_sold,SI.unit_price,SI.cost_price,SI.loc_code,SI.packet_qty,SI.item_number," +
                            " SI.discount_value,SI.discount_percent,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat, SI.item_number," +
                            " P.name AS name,P.code,P.item_type,P.qty,P.category_code," +
                            " U.name AS unit," +
                            " CT.name AS category, CT.id AS category_id," +
                            " C.first_name as customer_name, C.vat_no, C.credit_limit" +
                            " FROM pos_sales S" +
                            " LEFT JOIN pos_sales_items SI ON S.invoice_no = SI.invoice_no" +
                            " LEFT JOIN pos_products P ON P.item_number = SI.item_number" +
                            " LEFT JOIN pos_customers C ON C.id = S.customer_id" +
                            " LEFT JOIN pos_units U ON U.id = SI.unit_id" +
                            " LEFT JOIN pos_categories CT ON CT.code = P.category_code" +
                            " WHERE S.invoice_no = @invoice_no AND S.account = 'Sale' AND S.branch_id = @branch_id" +
                            " ORDER BY SI.serialnumber ASC";

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

        public DataTable GetEstimatesAndEstimatesItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.sale_date,S.sale_time,S.invoice_no,S.sale_type,S.account,S.customer_id,S.employee_id,S.description,S.account," +
                            " S.discount_percent AS total_disc_percent,S.discount_value AS total_disc_value,S.total_amount,S.flatDiscountValue," +
                            " SI.id,SI.item_code,SI.quantity_sold,SI.unit_price,SI.cost_price,SI.loc_code,SI.item_number," +
                            " SI.discount_value,SI.discount_percent,(SI.unit_price*SI.quantity_sold) AS total, SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity_sold*SI.tax_rate/100) AS vat, SI.item_number," +
                            " SI.item_name AS name,P.code,P.item_type,P.category_code," +
                            " U.name AS unit," +
                            " CT.name AS category, CT.id AS category_id," +
                            " C.first_name as customer_name, C.vat_no, C.credit_limit" +
                            " FROM pos_estimates S" +
                            " LEFT JOIN pos_estimates_items SI ON S.invoice_no=SI.invoice_no" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_customers C ON C.id = S.customer_id" +
                            " LEFT JOIN pos_units U ON U.id=SI.unit_id" +
                            " LEFT JOIN pos_categories CT ON CT.code=P.category_code" +
                            " WHERE S.invoice_no = @invoice_no AND S.status != 1 AND S.branch_id = @branch_id" +
                            " ORDER BY SI.serialnumber ASC";

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

        public int DeleteSales(string invoice_no)
        {
            String query1 = "DELETE FROM pos_sales WHERE invoice_no  = @invoice_no AND branch_id = @branch_id";
            String query2 = " DELETE FROM pos_sales_items WHERE invoice_no  = @invoice_no AND branch_id = @branch_id";
            String query3 = " DELETE FROM acc_entries WHERE invoice_no =  @invoice_no AND branch_id = @branch_id";
            String query4 = " DELETE FROM pos_customers_payments WHERE invoice_no = @invoice_no";
            //String query5 = " DELETE FROM pos_inventory WHERE invoice_no =  @invoice_no";
            String query6 = " DELETE FROM pos_employees_commission WHERE invoice_no = @invoice_no";
            String query7 = " DELETE FROM pos_user_commission WHERE invoice_no = @invoice_no";
            int result = 0;

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction = null;

                DataTable sales_dt = new DataTable();
                if (cn.State == ConnectionState.Closed)
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();
                    try
                    {
                        String query_1 = "SELECT SI.*" +
                            " FROM pos_sales_items SI" +
                            " WHERE SI.invoice_no = @invoice_no AND branch_id = @branch_id";

                        //cmd = new SqlCommand(query_1, cn, transaction);


                        //da = new SqlDataAdapter(cmd);
                        //da.Fill(sales_dt);
                        using (SqlCommand cmd = new SqlCommand(query_1, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            //cmd.ExecuteNonQuery();
                            da = new SqlDataAdapter(cmd);
                            da.Fill(sales_dt);

                        }

                        //string date_now = DateTime.Now.Date.ToString();
                        foreach (DataRow dr in sales_dt.Rows)
                        {
                            //String query_2 = "UPDATE pos_products SET qty= (SELECT qty FROM pos_products WHERE code = @item_code)+@quantity_sold WHERE code=@item_code ";
                            String query_2 = "UPDATE pos_product_stocks SET qty= (SELECT TOP 1 qty FROM pos_product_stocks WHERE item_number = @item_number AND branch_id = @branch_id)+@quantity WHERE item_number=@item_number AND branch_id = @branch_id ";
                            //String query_2 = "INSERT INTO pos_product_stocks VALUES (0,0,'" + dr["loc_code"].ToString() + "'," + dr["item_code"].ToString() + "," + double.Parse(dr["quantity_sold"].ToString()) + ",0,'" + date_now + "','" + date_now + "')";
                            //String query_2 = "UPDATE pos_product_stocks SET qty= (SELECT qty FROM pos_product_stocks WHERE item_code = " + dr["item_code"].ToString() + " AND loc_code = '" + dr["loc_code"].ToString() + "')+" + double.Parse(dr["quantity_sold"].ToString()) + " WHERE item_code=" + dr["item_code"].ToString() + " AND loc_code = '" + dr["loc_code"].ToString() + "'";
                            cmd = new SqlCommand(query_2, cn, transaction);
                            cmd.Parameters.AddWithValue("@item_number", dr["item_number"].ToString());
                            cmd.Parameters.AddWithValue("@quantity", double.Parse(dr["quantity_sold"].ToString()));
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.ExecuteNonQuery();

                            String query_3 = "INSERT INTO pos_inventory VALUES (@item_code,@quantity,@cost_price,@unit_price,@branch_id,@user_id,'Sales Delete',@invoice_no, @sale_date,GETDATE(),0,0,0,@sale_date,@location_code,-@packet_qty,@item_number)";
                            cmd = new SqlCommand(query_3, cn, transaction);
                            cmd.Parameters.AddWithValue("@item_code", dr["item_code"].ToString());
                            cmd.Parameters.AddWithValue("@item_number", dr["item_number"].ToString());
                            cmd.Parameters.AddWithValue("@location_code", dr["loc_code"].ToString());
                            cmd.Parameters.AddWithValue("@cost_price", double.Parse(dr["cost_price"].ToString()));
                            cmd.Parameters.AddWithValue("@unit_price", double.Parse(dr["unit_price"].ToString()));
                            cmd.Parameters.AddWithValue("@quantity", double.Parse(dr["quantity_sold"].ToString()));
                            cmd.Parameters.AddWithValue("@packet_qty", double.Parse(dr["packet_qty"].ToString()));
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@sale_date", DateTime.Now);

                            cmd.ExecuteNonQuery();

                        }

                        using (SqlCommand cmd = new SqlCommand(query1, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(query2, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(query3, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(query4, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.ExecuteNonQuery(); }
                        //using (SqlCommand cmd = new SqlCommand(query5, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(query7, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(query6, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.ExecuteNonQuery(); }


                        //cmd = new SqlCommand(query, cn);
                        //cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        transaction.Commit();

                        Log.LogAction("Delete Sale", $"InvoiceNo: {invoice_no}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                        result = 1;
                    }
                    catch
                    {
                        transaction.Rollback();

                        throw;

                    }
                }

                //int result = cmd.ExecuteNonQuery();

                return result;

            }

        }

        public int UpdateCustomerInSales(string invoice_no, string customerId)
        {
            int result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    String query1 = "UPDATE pos_sales SET customer_id=@customerId WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
                    String query2 = "UPDATE pos_inventory SET customer_id=@customerId  WHERE invoice_no= @invoice_no AND branch_id = @branch_id";
                    String query3 = "UPDATE pos_customers_payments SET customer_id=@customerId  WHERE invoice_no= @invoice_no AND branch_id = @branch_id";

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        transaction = cn.BeginTransaction();

                        //String query = "UPDATE pos_sales SET customer_id=@customerId" +
                        //    " WHERE invoice_no = @invoice_no AND branch_id = @branch_id";


                        //cmd = new SqlCommand(query, cn);
                        //cmd.Parameters.AddWithValue("@customerId", customerId);
                        //cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        using (SqlCommand cmd = new SqlCommand(query1, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@customerId", customerId); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(query2, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@customerId", customerId); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(query3, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@customerId", customerId); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }

                    }

                    transaction.Commit();
                    Log.LogAction("Update Customer Name In Sales", $"InvoiceNo: {invoice_no}, CustomerId: {customerId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    result = 1;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return result;
        }
        public int UpdateZetcaQrcodeInSales(string invoice_no, byte[] Zetca_qrcode)
        {
            int result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    String query1 = "UPDATE pos_sales SET Zetca_qrcode=@Zetca_qrcode WHERE invoice_no = @invoice_no AND branch_id = @branch_id";

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        transaction = cn.BeginTransaction();

                        using (SqlCommand cmd = new SqlCommand(query1, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@Zetca_qrcode", Zetca_qrcode);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.ExecuteNonQuery();
                        }

                    }

                    transaction.Commit();

                    result = 1;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
            return result;
        }

        // Summary row (Sales, SalesTax, SalesNet, Expenses, Profit computed in app)
        public FinanceSummaryDto GetSummary(DateTime? startDate, DateTime? endDate)
        {
            const string sql = @"
            DECLARE @StartDate DATETIME = @pStart, @EndExclusive DATETIME = @pEnd;
            WITH SalesAgg AS (
                SELECT
                    SalesAmount = SUM(ISNULL(s.total_amount, 0)),
                    SalesTax    = SUM(ISNULL(s.total_tax, 0)),
                    SalesNet    = SUM(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0))
                FROM dbo.pos_sales s
                WHERE (@StartDate IS NULL OR s.sale_date >= @StartDate)
                  AND (@EndExclusive IS NULL OR s.sale_date < @EndExclusive)
            ),
            ExpenseAgg AS (
                SELECT
                    ExpensesTotal = SUM(ISNULL(p.amount, 0))
                FROM dbo.acc_payments p
                WHERE (@StartDate IS NULL OR p.payment_date >= @StartDate)
                  AND (@EndExclusive IS NULL OR p.payment_date < @EndExclusive)
            )
            SELECT
                ISNULL(sa.SalesAmount, 0) AS SalesAmount,
                ISNULL(sa.SalesTax, 0)    AS SalesTax,
                ISNULL(sa.SalesNet, 0)    AS SalesNet,
                ISNULL(ea.ExpensesTotal,0)AS ExpensesTotal
            FROM SalesAgg sa CROSS JOIN ExpenseAgg ea;";

            var dto = new FinanceSummaryDto();

            using (var conn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@pStart", SqlDbType.DateTime).Value = (object)startDate ?? DBNull.Value;
                cmd.Parameters.Add("@pEnd", SqlDbType.DateTime).Value = (object)(endDate?.Date.AddDays(1)) ?? DBNull.Value;

                conn.Open();
                using (var rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (rdr.Read())
                    {
                        dto.SalesAmount = rdr.GetDecimal(rdr.GetOrdinal("SalesAmount"));
                        dto.SalesTax = rdr.GetDecimal(rdr.GetOrdinal("SalesTax"));
                        dto.SalesNet = rdr.GetDecimal(rdr.GetOrdinal("SalesNet"));
                        dto.ExpensesTotal = rdr.GetDecimal(rdr.GetOrdinal("ExpensesTotal"));
                    }
                }
            }

            return dto;
        }

        // Optional: daily breakdown for grid
        public DataTable GetDailyBreakdown(DateTime? startDate, DateTime? endDate)
        {
            const string sql = @"
            DECLARE @StartDate DATE = @pStart, @EndExclusive DATE = @pEnd;
            WITH SalesByDay AS (
                SELECT
                    [Date]   = CONVERT(date, s.sale_date),
                    SalesNet = SUM(ISNULL(s.total_amount,0) + ISNULL(s.total_tax,0) - ISNULL(s.discount_value,0))
                FROM dbo.pos_sales s
                WHERE (@StartDate IS NULL OR s.sale_date >= @StartDate)
                  AND (@EndExclusive IS NULL OR s.sale_date <  @EndExclusive)
                GROUP BY CONVERT(date, s.sale_date)
            ),
            ExpensesByDay AS (
                SELECT
                    [Date]        = CONVERT(date, p.payment_date),
                    ExpensesTotal = SUM(ISNULL(p.amount,0))
                FROM dbo.acc_payments p
                WHERE (@StartDate IS NULL OR p.payment_date >= @StartDate)
                  AND (@EndExclusive IS NULL OR p.payment_date <  @EndExclusive)
                GROUP BY CONVERT(date, p.payment_date)
            )
            SELECT
                d.[Date],
                ISNULL(s.SalesNet, 0)      AS SalesNet,
                ISNULL(e.ExpensesTotal, 0) AS ExpensesTotal,
                ISNULL(s.SalesNet, 0) - ISNULL(e.ExpensesTotal, 0) AS Profit
            FROM (
                SELECT [Date] FROM SalesByDay
                UNION
                SELECT [Date] FROM ExpensesByDay
            ) d
            LEFT JOIN SalesByDay s ON s.[Date] = d.[Date]
            LEFT JOIN ExpensesByDay e ON e.[Date] = d.[Date]
            ORDER BY d.[Date];";

            var dt = new DataTable();

            using (var conn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand(sql, conn))
            using (var da = new SqlDataAdapter(cmd))
            {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@pStart", SqlDbType.Date).Value = (object)startDate?.Date ?? DBNull.Value;
                cmd.Parameters.Add("@pEnd", SqlDbType.Date).Value = (object)endDate?.Date.AddDays(1) ?? DBNull.Value;
                conn.Open();
                da.Fill(dt);
            }

            return dt;
        }
        public DataTable GenerateDetailedReport(DateTime startDate, DateTime endDate)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(dbConnection.ConnectionString))
                {
                    conn.Open();

                    string query = @"
                -- Sales Summary Header
                SELECT 
                    'SALES SUMMARY' as Category,
                    '' as TransactionDate,
                    'Total Sales' as Description,
                    CAST(SUM(total_amount) as decimal(18,2)) as SalesAmount,
                    0 as ExpenseAmount,
                    1 as SortOrder
                FROM pos_sales 
                WHERE sale_date BETWEEN @FromDate AND @ToDate
                
                UNION ALL
                
                -- Individual Sales
                SELECT 
                    'Sale' as Category,
                    CONVERT(VARCHAR, sale_date, 103) as TransactionDate,
                    ISNULL(customer_name, 'Walk-in Customer') as Description,
                    total_amount as SalesAmount,
                    0 as ExpenseAmount,
                    2 as SortOrder
                FROM pos_sales 
                WHERE sale_date BETWEEN @FromDate AND @ToDate
                
                UNION ALL
                
                -- Expenses Summary Header
                SELECT 
                    'EXPENSES SUMMARY' as Category,
                    '' as TransactionDate,
                    'Total Expenses' as Description,
                    0 as SalesAmount,
                    CAST(SUM(amount) as decimal(18,2)) as ExpenseAmount,
                    3 as SortOrder
                FROM acc_payments 
                WHERE payment_date BETWEEN @FromDate AND @ToDate
                
                UNION ALL
                
                -- Individual Expenses
                SELECT 
                    'Expense' as Category,
                    CONVERT(VARCHAR, payment_date, 103) as TransactionDate,
                    ISNULL(name, 'Miscellaneous Expense') as Description,
                    0 as SalesAmount,
                    amount as ExpenseAmount,
                    4 as SortOrder
                FROM acc_payments 
                WHERE payment_date BETWEEN @FromDate AND @ToDate
                
                ORDER BY SortOrder, TransactionDate";


                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@FromDate", startDate);
                        cmd.Parameters.AddWithValue("@ToDate", endDate.AddDays(1).AddSeconds(-1));

                        DataTable dt = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }

                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
                //MessageBox.Show($"Error generating report: {ex.Message}", "Error",
                //   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// New methods for POS product operations
        /// </summary>
        private dbConnection dbHelper = new dbConnection();

        /// <summary>
        /// Get the next invoice number for a given branch
        /// </summary>
        /// <param name="branchId"></param>
        /// <returns></returns>
        public string GetNextInvoiceNumber(int branchId)
        {
            string query = @"
                SELECT 'INV-' + CONVERT(varchar, @BranchId) + '-' + 
                       RIGHT('000000' + CONVERT(varchar, ISNULL(MAX(CAST(SUBSTRING(invoice_no, LEN(invoice_no)-5, 6) AS INT)), 0) + 1), 6)
                FROM pos_sales 
                WHERE branch_id = @BranchId 
                AND invoice_no LIKE 'INV-' + CONVERT(varchar, @BranchId) + '-%'";

            SqlParameter[] parameters = {
                new SqlParameter("@BranchId", branchId)
            };

            object result = dbHelper.ExecuteScalar(query, parameters);
            return result?.ToString() ?? $"INV-{branchId}-000001";
        }

        public int CreateSale_1(DataTable saleItems, decimal totalAmount, decimal totalTax,
                            decimal discount, int customerId, int userId, int branchId,
                            int paymentMethodId, string customerName = "")
        {
            string invoiceNo = GetNextInvoiceNumber(branchId);

            string saleQuery = @"
                INSERT INTO pos_sales (
                    invoice_no, store_id, sale_time, sale_date, sale_type,
                    account, total_amount, total_tax, exchange_rate, paid,
                    discount_value, customer_id, employee_id, user_id,
                    register_mode, amount_due, currency_id, branch_id,
                    is_return, payment_method_id, customer_name, flatDiscountValue
                ) VALUES (
                    @InvoiceNo, @StoreId, GETDATE(), GETDATE(), 'POS',
                    'SALE', @TotalAmount, @TotalTax, 1, @TotalAmount,
                    @DiscountValue, @CustomerId, @EmployeeId, @UserId,
                    'POS', 0, 1, @BranchId, 0, @PaymentMethodId, @CustomerName, @DiscountValue
                ); SELECT SCOPE_IDENTITY();";

            SqlParameter[] saleParams = {
                new SqlParameter("@InvoiceNo", invoiceNo),
                new SqlParameter("@StoreId", branchId),
                new SqlParameter("@TotalAmount", totalAmount),
                new SqlParameter("@TotalTax", totalTax),
                new SqlParameter("@DiscountValue", discount),
                new SqlParameter("@CustomerId", customerId == 0 ? DBNull.Value : (object)customerId),
                new SqlParameter("@EmployeeId", DBNull.Value),
                new SqlParameter("@UserId", userId),
                new SqlParameter("@BranchId", branchId),
                new SqlParameter("@PaymentMethodId", paymentMethodId),
                new SqlParameter("@CustomerName", string.IsNullOrEmpty(customerName) ? DBNull.Value : (object)customerName)
            };

            int saleId = Convert.ToInt32(dbHelper.ExecuteScalar(saleQuery, saleParams));

            // Insert sale items
            foreach (DataRow row in saleItems.Rows)
            {
                string itemQuery = @"
                    INSERT INTO pos_sales_items (
                        invoice_no, sale_id, item_code, item_name, quantity_sold,
                        cost_price, unit_price, discount_percent, discount_value,
                        service, unit_id, currency_id, exchange_rate, branch_id,
                        tax_id, tax_rate, packet_qty, item_number
                    ) VALUES (
                        @InvoiceNo, @SaleId, @ItemCode, @ItemName, @QuantitySold,
                        @CostPrice, @UnitPrice, @DiscountPercent, @DiscountValue,
                        0, @UnitId, 1, 1, @BranchId, @TaxId, @TaxRate, @PacketQty, @ItemNumber
                    )";

                SqlParameter[] itemParams = {
                    new SqlParameter("@InvoiceNo", invoiceNo),
                    new SqlParameter("@SaleId", saleId),
                    new SqlParameter("@ItemCode", row["ProductCode"]),
                    new SqlParameter("@ItemName", row["ProductName"]),
                    new SqlParameter("@QuantitySold", row["Quantity"]),
                    new SqlParameter("@CostPrice", row["CostPrice"]),
                    new SqlParameter("@UnitPrice", row["UnitPrice"]),
                    new SqlParameter("@DiscountPercent", row["DiscountPercent"]),
                    new SqlParameter("@DiscountValue", row["DiscountValue"]),
                    new SqlParameter("@UnitId", row["UnitId"] ?? DBNull.Value),
                    new SqlParameter("@BranchId", branchId),
                    new SqlParameter("@TaxId", row["TaxId"] ?? DBNull.Value),
                    new SqlParameter("@TaxRate", row["TaxRate"] ?? 0),
                    new SqlParameter("@PacketQty", row["PacketQty"] ?? 1),
                    new SqlParameter("@ItemNumber", row["ItemNumber"] ?? DBNull.Value)
                };

                dbHelper.ExecuteNonQuery(itemQuery, itemParams);

                // Update product quantity
                ProductDLL productDal = new ProductDLL();
                productDal.UpdateProductQuantity(row["ItemNumber"].ToString(),
                    Convert.ToDecimal(row["Quantity"]), branchId);
            }

            return saleId;
        }

        public int CreateSale(DataTable saleItems, decimal totalAmount, decimal totalTax,
                            decimal discount, int customerId, int userId, int branchId,
                            int paymentMethodId, string customerName = "", string locationCode = "MAIN")
        {
            string invoiceNo = GetNextInvoiceNumber(branchId);

            using (SqlConnection conn = dbHelper.GetConnection())
            {
                conn.Open();
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        // Insert sale header
                        string saleQuery = @"
                            INSERT INTO pos_sales (
                                invoice_no, store_id, sale_time, sale_date, sale_type,
                                account, total_amount, total_tax, exchange_rate, paid,
                                discount_value, customer_id, employee_id, user_id,
                                register_mode, amount_due, currency_id, branch_id,
                                is_return, payment_method_id, customer_name, flatDiscountValue
                            ) VALUES (
                                @InvoiceNo, @StoreId, GETDATE(), GETDATE(), 'POS',
                                'SALE', @TotalAmount, @TotalTax, 1, @TotalAmount,
                                @DiscountValue, @CustomerId, @EmployeeId, @UserId,
                                'POS', 0, 1, @BranchId, 0, @PaymentMethodId, @CustomerName, @DiscountValue
                            ); SELECT SCOPE_IDENTITY();";

                        SqlParameter[] saleParams = {
                            new SqlParameter("@InvoiceNo", invoiceNo),
                            new SqlParameter("@StoreId", branchId),
                            new SqlParameter("@TotalAmount", totalAmount),
                            new SqlParameter("@TotalTax", totalTax),
                            new SqlParameter("@DiscountValue", discount),
                            new SqlParameter("@CustomerId", customerId == 0 ? DBNull.Value : (object)customerId),
                            new SqlParameter("@EmployeeId", DBNull.Value),
                            new SqlParameter("@UserId", userId),
                            new SqlParameter("@BranchId", branchId),
                            new SqlParameter("@PaymentMethodId", paymentMethodId),
                            new SqlParameter("@CustomerName", string.IsNullOrEmpty(customerName) ? DBNull.Value : (object)customerName)
                        };

                        int saleId = Convert.ToInt32(dbHelper.ExecuteScalar(saleQuery, saleParams, conn, transaction));

                        // Insert sale items and update inventory
                        foreach (DataRow row in saleItems.Rows)
                        {
                            string itemCode = row["ProductCode"].ToString();
                            decimal quantity = Convert.ToDecimal(row["Quantity"]);
                            decimal unitPrice = Convert.ToDecimal(row["UnitPrice"]);
                            decimal costPrice = Convert.ToDecimal(row["CostPrice"]);
                            string itemNumber = row["ItemNumber"]?.ToString();

                            // Validate stock before processing
                            if (!productDLL.ValidateStockAvailability(itemNumber, branchId, quantity, locationCode))
                            {
                                throw new Exception($"Insufficient stock for product: {row["ProductName"]}. Available: {productDLL.GetProductStock(itemNumber, branchId, locationCode)}");
                            }

                            // Insert sale item
                            string itemQuery = @"
                                INSERT INTO pos_sales_items (
                                    invoice_no, sale_id, item_code, item_name, quantity_sold,
                                    cost_price, unit_price, discount_percent, discount_value,
                                    service, unit_id, currency_id, exchange_rate, branch_id,
                                    tax_id, tax_rate, packet_qty, item_number
                                ) VALUES (
                                    @InvoiceNo, @SaleId, @ItemCode, @ItemName, @QuantitySold,
                                    @CostPrice, @UnitPrice, @DiscountPercent, @DiscountValue,
                                    0, @UnitId, 1, 1, @BranchId, @TaxId, @TaxRate, @PacketQty, @ItemNumber
                                )";

                            SqlParameter[] itemParams = {
                                new SqlParameter("@InvoiceNo", invoiceNo),
                                new SqlParameter("@SaleId", saleId),
                                new SqlParameter("@ItemCode", itemCode),
                                new SqlParameter("@ItemName", row["ProductName"]),
                                new SqlParameter("@QuantitySold", quantity),
                                new SqlParameter("@CostPrice", costPrice),
                                new SqlParameter("@UnitPrice", unitPrice),
                                new SqlParameter("@DiscountPercent", row["DiscountPercent"]),
                                new SqlParameter("@DiscountValue", row["DiscountValue"]),
                                new SqlParameter("@UnitId", row["UnitId"] ?? DBNull.Value),
                                new SqlParameter("@BranchId", branchId),
                                new SqlParameter("@TaxId", row["TaxId"] ?? DBNull.Value),
                                new SqlParameter("@TaxRate", row["TaxRate"] ?? 0),
                                new SqlParameter("@PacketQty", row["PacketQty"] ?? 1),
                                new SqlParameter("@ItemNumber", (object)itemNumber ?? DBNull.Value)
                            };

                            dbHelper.ExecuteNonQuery(itemQuery, itemParams, conn, transaction);

                            // Process inventory transaction
                            bool inventoryProcessed = productDLL.ProcessSaleTransaction(
                                itemCode, branchId, quantity, unitPrice, costPrice, userId,
                                invoiceNo, itemNumber, customerId == 0 ? null : (int?)customerId, locationCode);

                            if (!inventoryProcessed)
                            {
                                throw new Exception($"Failed to update inventory for product: {row["ProductName"]}");
                            }
                        }

                        transaction.Commit();
                        return saleId;
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public string GetMaxSmallSaleInvoiceNo()
        {
            using (var cn = new System.Data.SqlClient.SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new System.Data.SqlClient.SqlCommand())
            {
                cmd.Connection = cn;
                cmd.CommandText = @"SELECT MAX(invoice_no) FROM pos_sales WHERE invoice_no LIKE 'ZS-%'";

                if (cn.State == System.Data.ConnectionState.Closed)
                    cn.Open();

                string maxId = Convert.ToString(cmd.ExecuteScalar());

                if (string.IsNullOrWhiteSpace(maxId))
                {
                    return "ZS-000001";
                }

                // ZS-000001
                int intval = int.Parse(maxId.Substring(3, 6));
                intval++;
                return string.Format("ZS-{0:000000}", intval);
            }
        }
    }
}