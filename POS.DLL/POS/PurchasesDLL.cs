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
    public class PurchasesDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private PurchasesModal info = new PurchasesModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchases", cn);
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

        public DataTable GetAllPurchases()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT TOP 10000 p.*,(p.total_tax+p.total_amount-p.discount_value) as total, CONCAT(sp.first_name,' ',sp.last_name) as supplier_name "+
                            " FROM pos_purchases p LEFT JOIN pos_suppliers sp ON p.supplier_id=sp.id"+
                            " WHERE p.purchase_date BETWEEN @FY_from_date AND @FY_to_date AND p.branch_id = @branch_id order by p.id desc";

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

        public DataTable GetAllPurchasesItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT PI.invoice_no,PI.id,PI.item_code,PI.item_number,PI.quantity,PI.cost_price,PI.discount_value,(PI.cost_price*PI.quantity-ABS(PI.discount_value)) AS total,PI.loc_code," + 
                            " P.name AS product_name, " +
                            "((PI.cost_price*PI.quantity-ABS(PI.discount_value))*PI.tax_rate/100) AS vat, " +
                            " (((PI.cost_price*PI.quantity-ABS(PI.discount_value))*PI.tax_rate/100) + (PI.cost_price*PI.quantity-ABS(PI.discount_value))) AS net_total " +
                            "FROM pos_purchases_items PI " +
                            "LEFT JOIN pos_products P ON P.item_number=PI.item_number " +
                            "WHERE PI.invoice_no = @invoice_no AND PI.branch_id = @branch_id";

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

                        cmd = new SqlCommand("SELECT p.id,p.invoice_no,purchase_type,purchase_date,total_amount,discount_value,total_tax, supplier_invoice_no,CONCAT(sp.first_name,' ',sp.last_name) as supplier_name "+
                            "FROM pos_purchases  p LEFT JOIN pos_suppliers sp ON p.supplier_id=sp.id WHERE p.invoice_no LIKE @invoice_no AND p.branch_id = @branch_id", cn);
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
        
        public DataTable GetAllPurchaseByInvoice(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.purchase_date,S.purchase_time,S.invoice_no,S.purchase_type,S.account,S.supplier_id,S.supplier_invoice_no,S.employee_id,S.description,S.account,S.shipping_cost," +
                            " SI.id,SI.item_code,SI.item_number,SI.quantity,SI.unit_price,SI.cost_price,SI.serialnumber,SI.item_number," +
                            " SI.quantity AS qty,SI.cost_price AS avg_cost," + // this line is for print of build edit product page
                            " SI.discount_value,(SI.unit_price*SI.quantity) AS total, SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity*SI.tax_rate/100) AS vat," +
                            " P.name AS name,P.id,P.name_ar,P.code,P.location_code,P.item_type,P.barcode,P.description," +
                            " U.name AS unit," +
                            " CT.name AS category" +
                            " FROM pos_purchases S" +
                            " LEFT JOIN pos_purchases_items SI ON S.id=SI.purchase_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                            " LEFT JOIN pos_categories CT ON CT.code=P.category_code" +
                            " WHERE S.invoice_no = @invoice_no AND S.branch_id = @branch_id"+
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

        public String GetMaxInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_purchases WHERE SUBSTRING(invoice_no, 1,1) = 'P' AND account <> 'Return' AND branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());
                    
                        if(maxId == "")
                        {
                            return maxId = "P-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(2, 6));
                            intval++;
                            maxId = String.Format("P-{0:000000}", intval);
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

        public String GetMaxReturnInvoiceNo()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_purchases WHERE SUBSTRING(invoice_no, 1,2) = 'PR'  AND branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());

                        if (maxId == "")
                        {
                            return maxId = "PR-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(3, 6));
                            intval++;
                            maxId = String.Format("PR-{00:000000}", intval);
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

        public String GetMaxInvoiceNo_HOLD()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM pos_hold_purchases WHERE branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());

                        if (maxId == "")
                        {
                            return maxId = "H-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(2, 6));
                            intval++;
                            maxId = String.Format("H-{0:000000}", intval);
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

        public int Insertpurchases(List<PurchaseModalHeader> purchases, List<PurchasesModal> purchase_detail)
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
                        cmd = new SqlCommand("sp_Purchases", cn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;

                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@employee_id", purchase_header.employee_id);
                            cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_type", purchase_header.purchase_type);
                            cmd.Parameters.AddWithValue("@supplier_invoice_no", purchase_header.supplier_invoice_no);
                            cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                            cmd.Parameters.AddWithValue("@total_amount", purchase_header.total_amount);
                            cmd.Parameters.AddWithValue("@total_tax", purchase_header.total_tax);
                            cmd.Parameters.AddWithValue("@discount_value", purchase_header.total_discount);
                            cmd.Parameters.AddWithValue("@discount_percent", purchase_header.total_discount_percent);
                            cmd.Parameters.AddWithValue("@purchase_date", purchase_header.purchase_date);
                            cmd.Parameters.AddWithValue("@description", purchase_header.description);
                            cmd.Parameters.AddWithValue("@account", purchase_header.account);
                            cmd.Parameters.AddWithValue("@PO_invoice_no", purchase_header.po_invoice_no);
                            cmd.Parameters.AddWithValue("@PO_status", 0);
                            cmd.Parameters.AddWithValue("@purchase_time", purchase_header.purchase_time);
                            cmd.Parameters.AddWithValue("@shipping_cost", purchase_header.shipping_cost);

                            cmd.Parameters.AddWithValue("@OperationType", "1");
                        }

                        newProdID = Convert.ToInt32(cmd.ExecuteScalar());

                        foreach (PurchasesModal detail in purchase_detail)
                        {
                            cmd = new SqlCommand("sp_Purchase_items", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@serialNo", detail.serialNo);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@item_id", detail.item_id);
                            cmd.Parameters.AddWithValue("@item_code", detail.code);
                            cmd.Parameters.AddWithValue("@invoice_no", detail.invoice_no);
                            cmd.Parameters.AddWithValue("@purchase_id", newProdID);
                            cmd.Parameters.AddWithValue("@tax_id", detail.tax_id);
                            cmd.Parameters.AddWithValue("@unit_price", detail.unit_price);
                            cmd.Parameters.AddWithValue("@quantity", detail.quantity);
                            cmd.Parameters.AddWithValue("@packet_qty", detail.packet_qty);
                            cmd.Parameters.AddWithValue("@discount_value", detail.discount);
                            cmd.Parameters.AddWithValue("@tax_rate", detail.tax_rate);
                            cmd.Parameters.AddWithValue("@cost_price", detail.cost_price);
                            cmd.Parameters.AddWithValue("@supplier_id", detail.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_date", detail.purchase_date);
                            cmd.Parameters.AddWithValue("@PO_invoice_no", detail.po_invoice_no);
                            cmd.Parameters.AddWithValue("@PO_status", detail.po_status);
                            cmd.Parameters.AddWithValue("@location_code", detail.location_code.ToUpper());
                            cmd.Parameters.AddWithValue("@purchase_type", detail.purchase_type);

                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();
                        }

                        /// FOR JOURNAL ENTRIES
                        ///  
                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            ///INVENTORY JOURNAL ENTRY (DEBIT)
                            //Insert_Journal_entry(invoice_no, inventory_acc_id, net_total, 0, purchase_date, txt_description.Text, 0, 0, 0);
                            
                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                            cmd.Parameters.AddWithValue("@account_id", purchase_header.inventory_acc_id);
                            cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                            cmd.Parameters.AddWithValue("@debit", (purchase_header.total_amount));
                            cmd.Parameters.AddWithValue("@credit", 0);
                            cmd.Parameters.AddWithValue("@description", purchase_header.description);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            cmd.Parameters.AddWithValue("@customer_id", 0);
                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                            cmd.Parameters.AddWithValue("@entry_id", 0);
                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();
                            ////

                            if (purchase_header.purchase_type == "Cash")
                            {
                                ///CASH JOURNAL ENTRY (CREDIT)
                                //Insert_Journal_entry(invoice_no, cash_account_id, 0, net_total, purchase_date, txt_description.Text, 0, 0, 0);
                                
                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", purchase_header.cash_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", (purchase_header.total_amount));
                                cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();
                            }
                            else
                            {
                                ///ACCOUNT PAYABLE JOURNAL ENTRY (CREDIT)
                                //int entry_id = Insert_Journal_entry(invoice_no, payable_account_id, 0, net_total, purchase_date, txt_description.Text, 0, 0, 0);
                                
                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", purchase_header.payable_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", (purchase_header.total_amount));
                                cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());

                                if (purchase_header.supplier_id != 0)
                                {
                                    ///ADD ENTRY INTO supplier PAYMENT(Credit)
                                    //Insert_Journal_entry(invoice_no, inventory_acc_id, 0, net_total, purchase_date, txt_description.Text, 0, supplier_id, entry_id);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.inventory_acc_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", 0);
                                    cmd.Parameters.AddWithValue("@credit", (purchase_header.total_amount));
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                                    cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();

                                }

                            }


                            if (purchase_header.total_discount > 0)
                            {
                                /// CASH JOURNAL ENTRY (DEBIT)
                                //Insert_Journal_entry(invoice_no, cash_account_id, net_total_discount, 0, purchase_date, txt_description.Text, 0, 0, 0);
                                if (purchase_header.purchase_type == "Cash")
                                {
                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.cash_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", purchase_header.total_discount);
                                    cmd.Parameters.AddWithValue("@credit", 0);
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();
                                }
                                else
                                {
                                    /// Account Payable JOURNAL ENTRY (DEBIT)
                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.payable_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", purchase_header.total_discount);
                                    cmd.Parameters.AddWithValue("@credit", 0);
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());

                                    if (purchase_header.supplier_id != 0)
                                    {
                                        ///ADD ENTRY INTO SUPPLIER PAYMENT(DEBIT)
                                        //Insert_Journal_entry(invoice_no, purchases_discount_acc_id, 0, net_total_discount, purchase_date, txt_description.Text, 0, supplier_id, entry_id);
                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", purchase_header.purchases_discount_acc_id);
                                        cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                        cmd.Parameters.AddWithValue("@debit", purchase_header.total_discount);
                                        cmd.Parameters.AddWithValue("@credit", 0);
                                        cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", 0);
                                        cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                                        cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();
                                    }
                                }
                                ///PURCHASE DISCOUNT JOURNAL ENTRY (CREDIT)
                                //int entry_id = Insert_Journal_entry(invoice_no, purchases_discount_acc_id, 0, net_total_discount, purchase_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", purchase_header.purchases_discount_acc_id);
                                cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", purchase_header.total_discount);
                                cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();


                            }
                            //discount end here

                            if (purchase_header.total_tax > 0)
                            {
                                ///SALES TAX JOURNAL ENTRY (DEBIT)
                                //Insert_Journal_entry(invoice_no, tax_account_id, net_total_tax, 0, purchase_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", purchase_header.tax_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                cmd.Parameters.AddWithValue("@debit", purchase_header.total_tax);
                                cmd.Parameters.AddWithValue("@credit", 0);
                                cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();

                                if (purchase_header.purchase_type == "Cash")
                                {
                                    ///CASH JOURNAL ENTRY (CREDIT)
                                    //Insert_Journal_entry(invoice_no, cash_account_id, 0, net_total_tax, purchase_date, txt_description.Text, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.cash_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", 0);
                                    cmd.Parameters.AddWithValue("@credit", purchase_header.total_tax);
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();
                                
                                }
                                else
                                {
                                    ///ACCOUNT PAYABLE JOURNAL ENTRY (CREDIT)
                                    //int entry_id = Insert_Journal_entry(invoice_no, payable_account_id, 0, net_total_tax, purchase_date, txt_description.Text, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.payable_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", 0);
                                    cmd.Parameters.AddWithValue("@credit", purchase_header.total_tax);
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());

                                    if (purchase_header.supplier_id != 0)
                                    {
                                        ///ADD ENTRY INTO supplier PAYMENT(Credit)
                                        //Insert_Journal_entry(invoice_no, tax_account_id, 0, net_total_tax, purchase_date, txt_description.Text, 0, supplier_id, entry_id);

                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", purchase_header.tax_account_id);
                                        cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                        cmd.Parameters.AddWithValue("@debit", 0);
                                        cmd.Parameters.AddWithValue("@credit", purchase_header.total_tax);
                                        cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", 0);
                                        cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                                        cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();
                                
                                    }

                                }

                            }


                            
                        }
                        /// END JOURNAL ENTRIES
                        transaction.Commit();

                        //insert log when trans commit
                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            Log.LogAction("Add Purchase", $"InvoiceNo: {purchase_header.invoice_no}, Purchase Date: {purchase_header.purchase_date}, Total Amount: {((purchase_header.total_amount + purchase_header.total_tax) - purchase_header.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
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

        public int InsertpurchasesItems(PurchasesModal obj)
        {

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchase_items", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@item_code", obj.code);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@purchase_id", obj.purchase_id);
                        cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                        cmd.Parameters.AddWithValue("@packet_qty", obj.packet_qty);
                        cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                        cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@PO_invoice_no", obj.po_invoice_no);
                        cmd.Parameters.AddWithValue("@PO_status", obj.po_status);
                        cmd.Parameters.AddWithValue("@location_code", obj.location_code.ToUpper());

                        cmd.Parameters.AddWithValue("@purchase_type", obj.purchase_type);

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

        public int Update(PurchasesModal obj)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchases", cn);
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
                       
                        cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
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

        public int Delete(int purchasesId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchases", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", purchasesId); 
                        cmd.Parameters.AddWithValue("@OperationType", "3");

                    }

                    int result = cmd.ExecuteNonQuery();
                    Log.LogAction("Delete Purchase", $"PurchasesId: {purchasesId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

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
                            " FROM pos_purchases P" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            //" LEFT JOIN pos_products P ON P.id=SI.item_code" +
                            //" LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE P.invoice_no = @invoice_no AND P.branch_id=@branch_id";

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

        public DataTable GetReturnPurchaseItems(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable dt_1 = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT CAST(1 AS BIT) AS chk, SI.invoice_no,SI.loc_code,SI.quantity AS return_qty,SI.packet_qty," +
                            " SI.id,SI.item_code,SI.item_number,SI.quantity,SI.unit_price,SI.cost_price,SI.tax_rate,SI.tax_id,SI.discount_value," +
                            " (((SI.cost_price*SI.quantity-ABS(SI.discount_value))*SI.tax_rate/100) + (SI.cost_price*SI.quantity-ABS(SI.discount_value))) AS total," +
                            " ((SI.cost_price*SI.quantity-discount_value)*SI.tax_rate/100) AS vat," +
                            " P.name AS product_name" +
                            //" C.first_name AS customer_name" +
                            " FROM pos_purchases_items SI" +
                            //" LEFT JOIN pos_sales_items SI ON S.id=SI.sale_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            //" LEFT JOIN pos_customers C ON C.id=S.customer_id" +
                            " WHERE SI.invoice_no = @invoice_no AND SI.branch_id=@branch_id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(dt_1);
                    return dt_1;
                }
                catch
                {

                    throw;
                }
            }

        }

        public int InsertReturnPurchase(List<PurchaseModalHeader> purchases, List<PurchasesModal> purchase_detail)
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
                        cmd = new SqlCommand("sp_Purchases", cn, transaction);
                        cmd.CommandType = CommandType.StoredProcedure;
                        
                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@employee_id", purchase_header.employee_id);
                            cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_type", purchase_header.purchase_type);
                            //cmd.Parameters.AddWithValue("@supplier_invoice_no", purchase_header.supplier_invoice_no);
                            cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                            cmd.Parameters.AddWithValue("@total_amount", purchase_header.total_amount);
                            cmd.Parameters.AddWithValue("@total_tax", purchase_header.total_tax);
                            cmd.Parameters.AddWithValue("@discount_value", purchase_header.total_discount);
                           // cmd.Parameters.AddWithValue("@discount_percent", purchase_header.total_discount_percent);
                            cmd.Parameters.AddWithValue("@purchase_date", purchase_header.purchase_date);
                            cmd.Parameters.AddWithValue("@description", purchase_header.description);
                            cmd.Parameters.AddWithValue("@account", purchase_header.account);
                           // cmd.Parameters.AddWithValue("@PO_invoice_no", purchase_header.po_invoice_no);
                            //cmd.Parameters.AddWithValue("@PO_status", 0);
                            cmd.Parameters.AddWithValue("@purchase_time", purchase_header.purchase_time);
                            //cmd.Parameters.AddWithValue("@shipping_cost", purchase_header.shipping_cost);

                            cmd.Parameters.AddWithValue("@OperationType", "2");
                        }

                        newProdID = Convert.ToInt32(cmd.ExecuteScalar());

                        foreach (PurchasesModal detail in purchase_detail)
                        {
                            cmd = new SqlCommand("sp_Purchase_items", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@item_code", detail.code);
                            cmd.Parameters.AddWithValue("@item_number", detail.item_number);
                            cmd.Parameters.AddWithValue("@invoice_no", detail.invoice_no);
                            cmd.Parameters.AddWithValue("@purchase_id", newProdID);
                            cmd.Parameters.AddWithValue("@tax_id", detail.tax_id);
                            cmd.Parameters.AddWithValue("@unit_price", detail.unit_price);
                            cmd.Parameters.AddWithValue("@quantity", detail.quantity);
                            cmd.Parameters.AddWithValue("@packet_qty", detail.packet_qty);
                            cmd.Parameters.AddWithValue("@discount_value", detail.discount);
                            cmd.Parameters.AddWithValue("@tax_rate", detail.tax_rate);
                            cmd.Parameters.AddWithValue("@cost_price", detail.cost_price);
                            cmd.Parameters.AddWithValue("@supplier_id", detail.supplier_id);
                            cmd.Parameters.AddWithValue("@purchase_date", detail.purchase_date);
                            cmd.Parameters.AddWithValue("@location_code", detail.location_code.ToUpper());
                            cmd.Parameters.AddWithValue("@purchase_type", detail.purchase_type);

                            cmd.Parameters.AddWithValue("@OperationType", "2");

                            cmd.ExecuteScalar();
                        }

                        /// FOR JOURNAL ENTRIES
                        ///  
                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            ///INVENTORY JOURNAL ENTRY (CREDIT)
                            // Insert_Journal_entry(new_invoice_no, inventory_acc_id,  0,total_amount, purchase_date, description, 0, 0, 0);

                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                            cmd.Parameters.AddWithValue("@account_id", purchase_header.inventory_acc_id);
                            cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                            cmd.Parameters.AddWithValue("@debit", 0);
                            cmd.Parameters.AddWithValue("@credit", purchase_header.total_amount);
                            cmd.Parameters.AddWithValue("@description", purchase_header.description);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            cmd.Parameters.AddWithValue("@customer_id", 0);
                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                            cmd.Parameters.AddWithValue("@entry_id", 0);
                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();
                            ////

                            if (purchase_header.purchase_type == "Cash")
                            {
                                ///CASH JOURNAL ENTRY (DEBIT)
                                //Insert_Journal_entry(new_invoice_no, cash_account_id, total_amount, 0, purchase_date, description, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", purchase_header.cash_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                cmd.Parameters.AddWithValue("@debit", purchase_header.total_amount);
                                cmd.Parameters.AddWithValue("@credit", 0);
                                cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();
                            }
                            else
                            {
                                ///ACCOUNT PAYABLE JOURNAL ENTRY (DEBIT)
                                //int entry_id = Insert_Journal_entry(new_invoice_no, payable_account_id, total_amount, 0, purchase_date, description, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", purchase_header.payable_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                cmd.Parameters.AddWithValue("@debit", purchase_header.total_amount);
                                cmd.Parameters.AddWithValue("@credit", 0);
                                cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());

                                if (purchase_header.supplier_id != 0)
                                {
                                    ///ADD ENTRY INTO supplier PAYMENT(debit)
                                    //Insert_Journal_entry(new_invoice_no, inventory_acc_id, total_amount, 0, purchase_date, description, 0, supplier_id, entry_id);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.inventory_acc_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", purchase_header.total_amount);
                                    cmd.Parameters.AddWithValue("@credit", 0);
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                                    cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();

                                }

                            }


                            if (purchase_header.total_discount > 0)
                            {
                                
                                ///PURCHASE DISCOUNT JOURNAL ENTRY (debit) 
                                //int entry_id = Insert_Journal_entry(invoice_no, purchases_discount_acc_id, 0, net_total_discount, purchase_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", purchase_header.purchases_discount_acc_id);
                                cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                cmd.Parameters.AddWithValue("@debit", purchase_header.total_discount);
                                cmd.Parameters.AddWithValue("@credit", 0);
                                cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());

                                if (purchase_header.purchase_type == "Cash")
                                {
                                    /// CASH JOURNAL ENTRY (Credit)
                                    //Insert_Journal_entry(new_invoice_no, cash_account_id, 0, total_discount, purchase_date, description, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.cash_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", 0);
                                    cmd.Parameters.AddWithValue("@credit", purchase_header.total_discount);
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();

                                }
                                else
                                {
                                    /// AC Payable JOURNAL ENTRY (Credit)
                                    //Insert_Journal_entry(new_invoice_no, cash_account_id, 0, total_discount, purchase_date, description, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.payable_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", 0);
                                    cmd.Parameters.AddWithValue("@credit", purchase_header.total_discount);
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();

                                    if (purchase_header.supplier_id != 0)
                                    {
                                        ///ADD ENTRY INTO SUPPLIER PAYMENT(DEBIT)
                                        //Insert_Journal_entry(invoice_no, purchases_discount_acc_id, 0, net_total_discount, purchase_date, txt_description.Text, 0, supplier_id, entry_id);
                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", purchase_header.purchases_discount_acc_id);
                                        cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                        cmd.Parameters.AddWithValue("@debit", 0);
                                        cmd.Parameters.AddWithValue("@credit", purchase_header.total_discount);
                                        cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", 0);
                                        cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                                        cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();
                                    }
                                }

                                
                            }

                            if (purchase_header.total_tax > 0)
                            {
                                ///SALES TAX JOURNAL ENTRY (credit)
                                //Insert_Journal_entry(invoice_no, tax_account_id, net_total_tax, 0, purchase_date, txt_description.Text, 0, 0, 0);

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", purchase_header.tax_account_id);
                                cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", purchase_header.total_tax);
                                cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();

                                if (purchase_header.purchase_type == "Cash")
                                {
                                    ///CASH JOURNAL ENTRY (debit)
                                    //Insert_Journal_entry(invoice_no, cash_account_id, 0, net_total_tax, purchase_date, txt_description.Text, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.cash_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", purchase_header.total_tax);
                                    cmd.Parameters.AddWithValue("@credit", 0);
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    cmd.ExecuteScalar();

                                }
                                else
                                {
                                    ///ACCOUNT PAYABLE JOURNAL ENTRY (debit)
                                    //int entry_id = Insert_Journal_entry(invoice_no, payable_account_id, 0, net_total_tax, purchase_date, txt_description.Text, 0, 0, 0);

                                    cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                    cmd.CommandType = CommandType.StoredProcedure;
                                    cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                    cmd.Parameters.AddWithValue("@account_id", purchase_header.payable_account_id);
                                    cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                    cmd.Parameters.AddWithValue("@debit", purchase_header.total_tax);
                                    cmd.Parameters.AddWithValue("@credit", 0);
                                    cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                    cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                    cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                    cmd.Parameters.AddWithValue("@customer_id", 0);
                                    cmd.Parameters.AddWithValue("@supplier_id", 0);
                                    cmd.Parameters.AddWithValue("@entry_id", 0);
                                    cmd.Parameters.AddWithValue("@OperationType", "1");

                                    Int32 entry_id = Convert.ToInt32(cmd.ExecuteScalar());

                                    if (purchase_header.supplier_id != 0)
                                    {
                                        ///ADD ENTRY INTO supplier PAYMENT(debit)
                                        //Insert_Journal_entry(invoice_no, tax_account_id, 0, net_total_tax, purchase_date, txt_description.Text, 0, supplier_id, entry_id);

                                        cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                        cmd.CommandType = CommandType.StoredProcedure;
                                        cmd.Parameters.AddWithValue("@invoice_no", purchase_header.invoice_no);
                                        cmd.Parameters.AddWithValue("@account_id", purchase_header.tax_account_id);
                                        cmd.Parameters.AddWithValue("@entry_date", purchase_header.purchase_date);
                                        cmd.Parameters.AddWithValue("@debit", purchase_header.total_tax);
                                        cmd.Parameters.AddWithValue("@credit", 0);
                                        cmd.Parameters.AddWithValue("@description", purchase_header.description);
                                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                        cmd.Parameters.AddWithValue("@customer_id", 0);
                                        cmd.Parameters.AddWithValue("@supplier_id", purchase_header.supplier_id);
                                        cmd.Parameters.AddWithValue("@entry_id", entry_id);
                                        cmd.Parameters.AddWithValue("@OperationType", "1");

                                        cmd.ExecuteScalar();

                                    }

                                }
                            }
                        }
                        transaction.Commit();

                        //insert log when trans commit
                        foreach (PurchaseModalHeader purchase_header in purchases)
                        {
                            Log.LogAction("Add Return Purchase", $"InvoiceNo: {purchase_header.invoice_no}, Purchase Date: {purchase_header.purchase_date}, Total Amount: {((purchase_header.total_amount + purchase_header.total_tax) - purchase_header.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                        }
                        //
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

                return (int)newProdID;
            }
        }

        public int InsertReturnPurchaseItems(PurchasesModal obj)
        {

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Purchase_items", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@item_code", obj.code);
                        cmd.Parameters.AddWithValue("@item_number", obj.item_number);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@purchase_id", obj.purchase_id);
                        cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                        cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                        cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
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

        public int DeletePurchases(string invoice_no)
        {
            string SQL1 = "DELETE FROM pos_purchases WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            string SQL2 = "DELETE FROM pos_purchases_items WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            string SQL3 = "DELETE FROM acc_entries WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            string SQL4 = "DELETE FROM pos_suppliers_payments WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            //string SQL5 = "DELETE FROM pos_inventory WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
            Int32 result = 0;

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
                                        " FROM pos_purchases_items SI" +
                                        " WHERE SI.invoice_no = @invoice_no AND branch_id = @branch_id";

                        // cmd1 = new SqlCommand(query_1, cn, transaction);
                        using (SqlCommand cmd = new SqlCommand(query_1, cn, transaction))
                        {
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            da = new SqlDataAdapter(cmd);
                            da.Fill(sales_dt);


                        }

                        foreach (DataRow dr in sales_dt.Rows)
                        {
                            //String query_2 = "UPDATE pos_products SET qty= (SELECT qty FROM pos_products WHERE code = @item_code)-@quantity WHERE code=@item_code ";
                            String query_2 = "UPDATE pos_product_stocks SET qty= (SELECT TOP 1 qty FROM pos_product_stocks WHERE item_number = @item_number AND branch_id = @branch_id)-@quantity WHERE item_number=@item_number AND branch_id = @branch_id ";
                            //--Insert Location qty

                            //String query_2 = "INSERT INTO pos_product_stocks VALUES (0,0,'" + dr["loc_code"].ToString() + "'," + dr["item_code"].ToString() + "," + (double.Parse(dr["quantity"].ToString()) * -1) + ",0,'" + DateTime.Now.Date + "','" + DateTime.Now.Date + "')";
                            //String query_2 = "UPDATE pos_product_stocks SET qty= (SELECT qty FROM pos_product_stocks WHERE item_code = " + dr["item_code"].ToString() + " AND loc_code = '" + dr["loc_code"].ToString() + "')-" + double.Parse(dr["quantity"].ToString()) + " WHERE item_code=" + dr["item_code"].ToString() + " AND loc_code = '" + dr["loc_code"].ToString() + "'";

                            cmd = new SqlCommand(query_2, cn, transaction);
                            cmd.Parameters.AddWithValue("@item_number", dr["item_number"].ToString());
                            cmd.Parameters.AddWithValue("@quantity", double.Parse(dr["quantity"].ToString()));
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                            cmd.ExecuteNonQuery();
                            //using (SqlCommand cmd1 = new SqlCommand(query_2, cn)) { cmd.ExecuteNonQuery(); }

                            String query_3 = "INSERT INTO pos_inventory VALUES (@item_code,-@quantity,-@cost_price,-@unit_price,@branch_id,@user_id,'Purchase Delete',@invoice_no, @purchase_date,GETDATE(),0,0,0,@purchase_date,@location_code,-@packet_qty,@item_number)";
                            cmd = new SqlCommand(query_3, cn, transaction);
                            cmd.Parameters.AddWithValue("@item_code", dr["item_code"].ToString());
                            cmd.Parameters.AddWithValue("@item_number", dr["item_number"].ToString());
                            cmd.Parameters.AddWithValue("@location_code", dr["loc_code"].ToString());
                            cmd.Parameters.AddWithValue("@cost_price", double.Parse(dr["cost_price"].ToString()));
                            cmd.Parameters.AddWithValue("@unit_price", double.Parse(dr["unit_price"].ToString()));
                            cmd.Parameters.AddWithValue("@quantity", double.Parse(dr["quantity"].ToString()));
                            cmd.Parameters.AddWithValue("@packet_qty", double.Parse(dr["packet_qty"].ToString()));
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@purchase_date", DateTime.Now);

                            cmd.ExecuteNonQuery();
                        }

                        using (SqlCommand cmd = new SqlCommand(SQL1, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(SQL2, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(SQL3, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        using (SqlCommand cmd = new SqlCommand(SQL4, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }
                        //using (SqlCommand cmd = new SqlCommand(SQL5, cn, transaction)) { cmd.Parameters.AddWithValue("@invoice_no", invoice_no); cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); cmd.ExecuteNonQuery(); }

                        //String query = "DELETE FROM pos_purchases WHERE invoice_no = @invoice_no" +
                        //            " DELETE FROM pos_purchases_items WHERE invoice_no = @invoice_no" +
                        //            " DELETE FROM acc_entries WHERE invoice_no = @invoice_no" +
                        //            " DELETE FROM pos_suppliers_payments WHERE invoice_no = @invoice_no" +
                        //            " DELETE FROM pos_inventory WHERE invoice_no = @invoice_no";

                        //cmd = new SqlCommand(query, cn);
                        //cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");

                        transaction.Commit();
                        Log.LogAction("Delete Purchase", $"InvoiceNo: {invoice_no}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

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

        }

        public DataTable PurchaseReceipt(string invoice_no)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable PurchaseReceipt_dt = new DataTable();
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.purchase_date,S.purchase_time,S.invoice_no,S.purchase_type,S.account," +
                            " SI.id,SI.item_code,SI.item_number,SI.quantity,SI.unit_price,SI.cost_price," +
                            " SI.discount_value,(SI.cost_price*SI.quantity) AS total, SI.tax_rate,SI.tax_id," +
                            " ((SI.cost_price*SI.quantity-SI.discount_value)*SI.tax_rate/100) AS vat," +
                            " C.first_name AS supplier_name, C.vat_no AS supplier_vat," +
                            " P.name AS product_name, P.code, SI.loc_code" +
                            " FROM pos_purchases S" +
                            " LEFT JOIN pos_purchases_items SI ON S.invoice_no=SI.invoice_no" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_suppliers C ON C.id=S.supplier_id" +
                            " WHERE S.invoice_no = @invoice_no AND S.branch_id = @branch_id";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@invoice_no", invoice_no);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        //cmd.Parameters.AddWithValue("@OperationType", "5");

                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(PurchaseReceipt_dt);
                    return PurchaseReceipt_dt;
                }
                catch
                {

                    throw;
                }
            }

        }


        public DataTable GetAll_Hold_PurchaseByInvoice(string invoice_no)
        { 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT S.purchase_date,S.purchase_time,S.invoice_no,S.purchase_type,S.account,S.supplier_id,S.supplier_invoice_no,S.employee_id,S.description,S.account,S.shipping_cost," +
                            " SI.id,SI.item_code,SI.quantity,SI.unit_price,SI.cost_price,SI.serialnumber,SI.item_number," +
                            " SI.discount_value,(SI.unit_price*SI.quantity) AS total, SI.tax_rate,SI.tax_id," +
                            " (SI.unit_price*SI.quantity*SI.tax_rate/100) AS vat," +
                            " P.name AS name,P.code,P.location_code,P.item_type,P.barcode," +
                            " U.name AS unit," +
                            " CT.name AS category" +
                            " FROM pos_hold_purchases S" +
                            " LEFT JOIN pos_hold_purchases_items SI ON S.id=SI.purchase_id" +
                            " LEFT JOIN pos_products P ON P.item_number=SI.item_number" +
                            " LEFT JOIN pos_units U ON U.id=P.unit_id" +
                            " LEFT JOIN pos_categories CT ON CT.code=P.category_code" +
                            " WHERE S.invoice_no = @invoice_no AND S.branch_id = @branch_id"+
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

        public int Insert_hold_purchases(PurchasesModal obj)
        {
            Int32 newProdID = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Hold_Purchases", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@employee_id", obj.employee_id);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@purchase_type", obj.purchase_type);
                        cmd.Parameters.AddWithValue("@supplier_invoice_no", obj.supplier_invoice_no);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@total_amount", obj.total_amount);
                        cmd.Parameters.AddWithValue("@total_tax", obj.total_tax);
                        cmd.Parameters.AddWithValue("@discount_value", obj.total_discount);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@account", obj.account);
                        cmd.Parameters.AddWithValue("@PO_invoice_no", obj.po_invoice_no);
                        cmd.Parameters.AddWithValue("@PO_status", 0);
                        cmd.Parameters.AddWithValue("@purchase_time", obj.purchase_time);
                        cmd.Parameters.AddWithValue("@OperationType", "1");

                    }

                    newProdID = Convert.ToInt32(cmd.ExecuteScalar());
                    Log.LogAction("Add Hold Purchase", $"InvoiceNo: {obj.invoice_no}, Purchase Date: {obj.purchase_date}, Total Amount: {((obj.total_amount + obj.total_tax) - obj.total_discount)}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    return (int)newProdID;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int Insert_hold_purchasesItems(PurchasesModal obj)
        {

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_Hold_Purchase_items", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@serialNo", obj.serialNo);
                        cmd.Parameters.AddWithValue("@item_number", obj.item_number);
                        cmd.Parameters.AddWithValue("@item_code", obj.code);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@purchase_id", obj.purchase_id);
                        cmd.Parameters.AddWithValue("@tax_id", obj.tax_id);
                        cmd.Parameters.AddWithValue("@unit_price", obj.unit_price);
                        cmd.Parameters.AddWithValue("@quantity", obj.quantity);
                        cmd.Parameters.AddWithValue("@packet_qty", obj.packet_qty);
                        cmd.Parameters.AddWithValue("@discount_value", obj.discount);
                        cmd.Parameters.AddWithValue("@tax_rate", obj.tax_rate);
                        cmd.Parameters.AddWithValue("@cost_price", obj.cost_price);
                        cmd.Parameters.AddWithValue("@supplier_id", obj.supplier_id);
                        cmd.Parameters.AddWithValue("@purchase_date", obj.purchase_date);
                        cmd.Parameters.AddWithValue("@PO_invoice_no", obj.po_invoice_no);
                        cmd.Parameters.AddWithValue("@PO_status", obj.po_status);
                        cmd.Parameters.AddWithValue("@location_code", obj.location_code.ToUpper());
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
        public int UpdateSupplierInPurchases(string invoice_no, string supplier_id, string supplierInvoiceNo)
        {
            int result = 0;
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    String query1 = "UPDATE pos_purchases SET supplier_id=@supplier_id, supplier_invoice_no=@supplierInvoiceNo WHERE invoice_no = @invoice_no AND branch_id = @branch_id";
                    String query2 = "UPDATE pos_inventory SET supplier_id=@supplier_id  WHERE invoice_no= @invoice_no AND branch_id = @branch_id";

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        transaction = cn.BeginTransaction();

                        using (SqlCommand cmd = new SqlCommand(query1, cn, transaction)) 
                        { 
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no); 
                            cmd.Parameters.AddWithValue("@supplier_id", supplier_id); 
                            cmd.Parameters.AddWithValue("@supplierInvoiceNo", supplierInvoiceNo); 
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); 
                            cmd.ExecuteNonQuery(); 
                        }
                        using (SqlCommand cmd = new SqlCommand(query2, cn, transaction)) 
                        { 
                            cmd.Parameters.AddWithValue("@invoice_no", invoice_no); 
                            cmd.Parameters.AddWithValue("@supplier_id", supplier_id); 
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id); 
                            cmd.ExecuteNonQuery(); 
                        }

                    }
                    
                    transaction.Commit();
                    Log.LogAction("Update Supllier Name in Purchase Inv", $"InvoiceNo: {invoice_no}, SupplierId: {supplier_id}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

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

    }
}
