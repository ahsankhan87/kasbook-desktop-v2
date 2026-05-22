using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    public class ExpenseDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private JournalsModal info = new JournalsModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
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

        public DataTable SearchRecordByJournalsID(int Journals_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Journals_id);
                        cmd.Parameters.AddWithValue("@OperationType", "4"); 
                        
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

        public DataTable SearchRecord(String condition)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("SELECT id,code,name,date_created FROM acc_payments WHERE name LIKE @name", cn);
                        //cmd.Parameters.AddWithValue("@id", condition);
                        cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", condition));
                        
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

                        cmd = new SqlCommand("SELECT MAX(invoice_no) FROM acc_payments WHERE invoice_no LIKE 'N-%' AND branch_id = @branch_id", cn);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        string maxId = Convert.ToString(cmd.ExecuteScalar());

                        if (maxId == "")
                        {
                            return maxId = "N-000001";
                        }
                        else
                        {
                            int intval = int.Parse(maxId.Substring(2, 6));
                            intval++;
                            maxId = String.Format("N-{0:000000}", intval);
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

        public int Insert(List<ExpenseModal_Header> sales)
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

                        ///
                        foreach (ExpenseModal_Header sale_header in sales)
                        {
                            
                            ///CASH JOURNAL ENTRY (Credit)
                            
                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                            cmd.Parameters.AddWithValue("@account_id", sale_header.cash_account);
                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                            cmd.Parameters.AddWithValue("@debit", 0);
                            cmd.Parameters.AddWithValue("@credit", sale_header.amount);
                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                            cmd.Parameters.AddWithValue("@customer_id", 0);
                            cmd.Parameters.AddWithValue("@supplier_id", 0);
                            cmd.Parameters.AddWithValue("@entry_id", 0);
                            cmd.Parameters.AddWithValue("@OperationType", "1");

                            cmd.ExecuteScalar();

                            //expense JOURNAL ENTRY (debit)

                            cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                            cmd.Parameters.AddWithValue("@account_id", sale_header.expense_account);
                            cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                            cmd.Parameters.AddWithValue("@debit", sale_header.amount);
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
                            ///

                            if (sale_header.vat_amount > 0)
                            {
                                ///VAT JOURNAL ENTRY (Credit side from payment account)
                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", sale_header.cash_account);
                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                cmd.Parameters.AddWithValue("@debit", 0);
                                cmd.Parameters.AddWithValue("@credit", sale_header.vat_amount);
                                cmd.Parameters.AddWithValue("@description", sale_header.description);
                                cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                                cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                                cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                                cmd.Parameters.AddWithValue("@customer_id", 0);
                                cmd.Parameters.AddWithValue("@supplier_id", 0);
                                cmd.Parameters.AddWithValue("@entry_id", 0);
                                cmd.Parameters.AddWithValue("@OperationType", "1");

                                cmd.ExecuteScalar();

                                var vatDebitAccount = string.IsNullOrWhiteSpace(sale_header.vat_account)
                                    ? sale_header.expense_account
                                    : sale_header.vat_account;

                                ///VAT JOURNAL ENTRY (Debit side to VAT account)
                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", vatDebitAccount);
                                cmd.Parameters.AddWithValue("@entry_date", sale_header.sale_date);
                                cmd.Parameters.AddWithValue("@debit", sale_header.vat_amount);
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
                            }

                            String query = "INSERT INTO acc_payments (invoice_no,account_code,name,amount,description,tax_rate,tax_amount,payment_date,branch_id,user_id,entry_id)" +
                                "VALUES (@invoice_no,@account_code,@account_name,@amount,@description,@tax_rate,@tax_amount,@payment_date,@branch_id,@user_id,@entry_id)";

                            cmd = new SqlCommand(query, cn, transaction);
                            cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                            cmd.Parameters.AddWithValue("@account_code", sale_header.expense_account);
                            cmd.Parameters.AddWithValue("@account_name", sale_header.expense_account_name);
                            cmd.Parameters.AddWithValue("@amount", sale_header.amount);
                            cmd.Parameters.AddWithValue("@description", sale_header.description);
                            cmd.Parameters.AddWithValue("@tax_rate", sale_header.vat);
                            cmd.Parameters.AddWithValue("@tax_amount", sale_header.vat_amount);
                            cmd.Parameters.AddWithValue("@payment_date", sale_header.sale_date);
                            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                            cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                            cmd.Parameters.AddWithValue("@entry_id", entry_id);

                            newSaleID = Convert.ToInt32(cmd.ExecuteScalar());


                           
                            ///

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

        public int Update(JournalsModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@account_name", obj.account_name);
                        cmd.Parameters.AddWithValue("@invoice_no", obj.invoice_no);
                        cmd.Parameters.AddWithValue("@account_id", obj.account_id);
                        cmd.Parameters.AddWithValue("@entry_date", obj.entry_date);
                        cmd.Parameters.AddWithValue("@debit", obj.debit);
                        cmd.Parameters.AddWithValue("@credit", obj.credit);
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

                    result = Convert.ToInt32(cmd.ExecuteScalar());
                    return (int)result;
                }
                catch
                {

                    throw;
                }
                
            }
        }

        public int Delete(int JournalsId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_JournalsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", JournalsId); 
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

        public DataTable GetExpenseTrackerList(DateTime fromDate, DateTime toDate, int expenseAccountId = 0, string paymentMode = "", string searchText = "")
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable list = new DataTable();
                    cn.Open();

                    string query = @"SELECT
P.invoice_no AS VoucherNo,
P.payment_date AS [Date],
P.name AS AccountName,
P.description AS [Description],
CASE
WHEN LOWER(P.description) LIKE '%mode: cash%' THEN 'Cash'
WHEN LOWER(P.description) LIKE '%mode: bank%' THEN 'Bank'
WHEN LOWER(P.description) LIKE '%mode: credit%' THEN 'Credit'
ELSE 'Cash'
END AS PaymentMode,
P.amount AS Amount,
P.tax_amount AS Tax,
(P.amount + P.tax_amount) AS NetAmount,
CASE WHEN LOWER(P.description) LIKE '%attachment:%' THEN 1 ELSE 0 END AS HasAttachment,
CASE WHEN EXISTS (SELECT 1 FROM acc_entries E WHERE E.invoice_no = P.invoice_no AND E.branch_id = P.branch_id) THEN 1 ELSE 0 END AS Posted
FROM acc_payments P
WHERE P.branch_id = @branch_id
AND P.payment_date BETWEEN @fromDate AND @toDate";

                    if (expenseAccountId > 0)
                    {
                        query += " AND P.account_code = @account_code";
                    }

                    if (!string.IsNullOrWhiteSpace(paymentMode) && paymentMode != "All")
                    {
                        query += " AND LOWER(P.description) LIKE @modeLike";
                    }

                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        query += " AND (P.invoice_no LIKE @search OR P.name LIKE @search OR P.description LIKE @search)";
                    }

                    query += " ORDER BY P.payment_date DESC, P.id DESC";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1).AddSeconds(-1));

                    if (expenseAccountId > 0)
                    {
                        cmd.Parameters.AddWithValue("@account_code", expenseAccountId.ToString());
                    }

                    if (!string.IsNullOrWhiteSpace(paymentMode) && paymentMode != "All")
                    {
                        cmd.Parameters.AddWithValue("@modeLike", "%mode: " + paymentMode.ToLower() + "%");
                    }

                    if (!string.IsNullOrWhiteSpace(searchText))
                    {
                        cmd.Parameters.AddWithValue("@search", "%" + searchText + "%");
                    }

                    da = new SqlDataAdapter(cmd);
                    da.Fill(list);
                    return list;
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetExpenseByVoucher(string voucherNo)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable detail = new DataTable();
                    cn.Open();

                    string query = @"SELECT TOP 1
invoice_no,
payment_date,
account_code,
name,
amount,
tax_rate,
tax_amount,
description
FROM acc_payments
WHERE branch_id = @branch_id AND invoice_no = @invoice_no
ORDER BY id DESC";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@invoice_no", voucherNo);

                    da = new SqlDataAdapter(cmd);
                    da.Fill(detail);
                    return detail;
                }
                catch
                {
                    throw;
                }
            }
        }

        public int DeleteByVoucher(string voucherNo)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    cn.Open();
                    transaction = cn.BeginTransaction();

                    int result = 0;

                    cmd = new SqlCommand("DELETE FROM acc_entries WHERE branch_id=@branch_id AND invoice_no=@invoice_no", cn, transaction);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@invoice_no", voucherNo);
                    cmd.ExecuteNonQuery();

                    cmd = new SqlCommand("DELETE FROM acc_payments WHERE branch_id=@branch_id AND invoice_no=@invoice_no", cn, transaction);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@invoice_no", voucherNo);
                    result = cmd.ExecuteNonQuery();

                    transaction.Commit();
                    return result;
                }
                catch
                {
                    if (transaction != null)
                        transaction.Rollback();
                    throw;
                }
            }
        }

        public decimal GetExpenseTotal(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    cmd = new SqlCommand("SELECT ISNULL(SUM(amount + tax_amount), 0) FROM acc_payments WHERE branch_id = @branch_id AND payment_date BETWEEN @fromDate AND @toDate", cn);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1).AddSeconds(-1));
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
                catch
                {
                    throw;
                }
            }
        }

        public decimal GetPendingExpenseTotal()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    cmd = new SqlCommand("SELECT ISNULL(SUM(P.amount + P.tax_amount), 0) FROM acc_payments P WHERE P.branch_id = @branch_id AND NOT EXISTS (SELECT 1 FROM acc_entries E WHERE E.branch_id = P.branch_id AND E.invoice_no = P.invoice_no)", cn);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    return Convert.ToDecimal(cmd.ExecuteScalar());
                }
                catch
                {
                    throw;
                }
            }
        }

        public int GetPendingExpenseCount()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    cmd = new SqlCommand("SELECT COUNT(1) FROM acc_payments P WHERE P.branch_id = @branch_id AND NOT EXISTS (SELECT 1 FROM acc_entries E WHERE E.branch_id = P.branch_id AND E.invoice_no = P.invoice_no)", cn);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetExpenseMonthlyComparison(int year)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable result = new DataTable();
                    cn.Open();
                    string query = @"SELECT MONTH(payment_date) AS MonthNo, ISNULL(SUM(amount + tax_amount), 0) AS TotalAmount
                    FROM acc_payments
                    WHERE branch_id = @branch_id AND YEAR(payment_date) = @year
                    GROUP BY MONTH(payment_date)
                    ORDER BY MONTH(payment_date)";
                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@year", year);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(result);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetExpenseBreakdown(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable result = new DataTable();
                    cn.Open();
                    string query = @"SELECT TOP 8 name AS AccountName, ISNULL(SUM(amount + tax_amount), 0) AS TotalAmount
                        FROM acc_payments
                        WHERE branch_id = @branch_id AND payment_date BETWEEN @fromDate AND @toDate
                        GROUP BY name
                        ORDER BY TotalAmount DESC";
                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1).AddSeconds(-1));
                    da = new SqlDataAdapter(cmd);
                    da.Fill(result);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetRecentExpenses(int top)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable result = new DataTable();
                    cn.Open();
                    string query = @"SELECT TOP (@top)
                        payment_date AS TxnDate,
                        name AS AccountName,
                        (amount + tax_amount) AS NetAmount,
                        CASE WHEN EXISTS (SELECT 1 FROM acc_entries E WHERE E.branch_id = P.branch_id AND E.invoice_no = P.invoice_no) THEN 'Posted' ELSE 'Pending' END AS Status
                        FROM acc_payments P
                        WHERE P.branch_id = @branch_id
                        ORDER BY payment_date DESC, id DESC";
                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@top", top);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    da = new SqlDataAdapter(cmd);
                    da.Fill(result);
                    return result;
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetTopExpenseAccounts(DateTime fromDate, DateTime toDate, int top)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    DataTable result = new DataTable();
                    cn.Open();
                    string query = @"WITH ExpenseTotals AS (
                            SELECT name AS AccountName, ISNULL(SUM(amount + tax_amount), 0) AS TotalAmount
                            FROM acc_payments
                            WHERE branch_id = @branch_id AND payment_date BETWEEN @fromDate AND @toDate
                            GROUP BY name
                        ), GrandTotal AS (
                            SELECT ISNULL(SUM(TotalAmount), 0) AS AllTotal FROM ExpenseTotals
                        )
                        SELECT TOP (@top)
                        E.AccountName,
                        E.TotalAmount,
                        CASE WHEN G.AllTotal = 0 THEN 0 ELSE (E.TotalAmount * 100.0 / G.AllTotal) END AS SharePercent
                        FROM ExpenseTotals E
                        CROSS JOIN GrandTotal G
                        ORDER BY E.TotalAmount DESC";
                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@top", top);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@fromDate", fromDate.Date);
                    cmd.Parameters.AddWithValue("@toDate", toDate.Date.AddDays(1).AddSeconds(-1));
                    da = new SqlDataAdapter(cmd);
                    da.Fill(result);
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
