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
                                ///VAT JOURNAL ENTRY (Debit)
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
                                ////
                                ///
                                //expense JOURNAL ENTRY (debit)

                                cmd = new SqlCommand("sp_JournalsCrud", cn, transaction);
                                cmd.CommandType = CommandType.StoredProcedure;
                                cmd.Parameters.AddWithValue("@invoice_no", sale_header.invoice_no);
                                cmd.Parameters.AddWithValue("@account_id", sale_header.expense_account);
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


    }
}
