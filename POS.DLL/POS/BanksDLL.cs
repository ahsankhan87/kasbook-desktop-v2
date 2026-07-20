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
    public class BankDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private BankModal info = new BankModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_banksCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "5");
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
        public bool IsBankGlAccount(int glAccountId)
        {
            // Heuristic based on a common schema: `pos_gl_accounts` has `id` and `account_type`.
            // account_type might be "Bank", "BANK", or numeric code.
            // Return false on any error to keep behavior safe.
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();
                        string query = "SELECT TOP 500 id, name FROM acc_accounts";
                        //cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        cmd.CommandText = query;
                        cmd.Connection = cn;
                        da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        
                    }
                }
                foreach (DataRow row in dt.Rows)
                {
                    if (row["id"] == DBNull.Value) continue;
                    if (Convert.ToInt32(row["id"]) != glAccountId) continue;

                    if (!dt.Columns.Contains("name") || row["name"] == DBNull.Value)
                        return false;

                    var t = row["name"].ToString().Trim();
                    var id = row["id"].ToString().Trim();

                    // String-based detection
                    if (id.Equals("19", StringComparison.OrdinalIgnoreCase))
                        return true;

                    // Numeric-based detection (if your DB uses codes)
                    int code;
                    if (int.TryParse(t, out code))
                    {
                        // Adjust this if your system has a known bank code.
                        // Keep conservative: assume code 19 indicates bank (common pattern in some POS schemas).
                        if (code == 19) return true;
                    }

                    return false;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public DataTable SearchRecordByBankID(int Bank_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        //cmd = new SqlCommand("SELECT id,first_name,last_name,email,vat_no FROM pos_Banks WHERE id = @id", cn);
                        cmd = new SqlCommand("sp_banksCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@OperationType", "4");
                        cmd.Parameters.AddWithValue("@id", Bank_id);
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

        public DataTable GetBankAccountBalance(int Bank_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand();
                        string query = "SELECT SUM(debit-credit) AS balance FROM pos_Banks_payments WHERE Bank_id = @id AND branch_id=@branch_id";
                        cmd.Parameters.AddWithValue("@id", Bank_id);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        cmd.CommandText = query;
                        cmd.Connection = cn;
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

        public DataTable GetBankAccountsForReconciliation()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    const string query = @"
SELECT
    B.id AS bank_id,
    B.name AS bank_name,
    B.GLAccountID AS account_id,
    ISNULL(A.code, '') AS account_code,
    ISNULL(A.name, B.name) AS account_name
FROM pos_banks B
INNER JOIN acc_accounts A ON A.id = B.GLAccountID
WHERE B.branch_id = @branch_id
ORDER BY B.name;";

                    using (SqlCommand localCmd = new SqlCommand(query, cn))
                    {
                        localCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        using (SqlDataAdapter adapter = new SqlDataAdapter(localCmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetBankReconciliationTransactions(int bankAccountId, DateTime statementDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand localCmd = new SqlCommand("sp_BankReconciliation", cn))
                    {
                        localCmd.CommandType = CommandType.StoredProcedure;
                        localCmd.Parameters.AddWithValue("@OperationType", 1);
                        localCmd.Parameters.AddWithValue("@BranchId", UsersModal.logged_in_branch_id);
                        localCmd.Parameters.AddWithValue("@BankAccountId", bankAccountId);
                        localCmd.Parameters.AddWithValue("@StatementDate", statementDate.Date);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(localCmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetUnclearedBankTransactions(int bankAccountId, DateTime statementDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    using (SqlCommand localCmd = new SqlCommand("sp_BankReconciliation", cn))
                    {
                        localCmd.CommandType = CommandType.StoredProcedure;
                        localCmd.Parameters.AddWithValue("@OperationType", 4);
                        localCmd.Parameters.AddWithValue("@BranchId", UsersModal.logged_in_branch_id);
                        localCmd.Parameters.AddWithValue("@BankAccountId", bankAccountId);
                        localCmd.Parameters.AddWithValue("@StatementDate", statementDate.Date);

                        using (SqlDataAdapter adapter = new SqlDataAdapter(localCmd))
                        {
                            DataTable table = new DataTable();
                            adapter.Fill(table);
                            return table;
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public int SaveBankReconciliation(int bankAccountId, DateTime statementDate, decimal bankStatementBalance, decimal adjustedBankBalance, decimal bookBalance, decimal difference, int userId, DataTable clearedRows)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction tx = null;
                try
                {
                    cn.Open();
                    tx = cn.BeginTransaction();

                    int reconciliationId;
                    using (SqlCommand headerCmd = new SqlCommand("sp_BankReconciliation", cn, tx))
                    {
                        headerCmd.CommandType = CommandType.StoredProcedure;
                        headerCmd.Parameters.AddWithValue("@OperationType", 2);
                        headerCmd.Parameters.AddWithValue("@BranchId", UsersModal.logged_in_branch_id);
                        headerCmd.Parameters.AddWithValue("@BankAccountId", bankAccountId);
                        headerCmd.Parameters.AddWithValue("@StatementDate", statementDate.Date);
                        headerCmd.Parameters.AddWithValue("@BankStatementBalance", bankStatementBalance);
                        headerCmd.Parameters.AddWithValue("@AdjustedBalance", adjustedBankBalance);
                        headerCmd.Parameters.AddWithValue("@BookBalance", bookBalance);
                        headerCmd.Parameters.AddWithValue("@Difference", difference);
                        headerCmd.Parameters.AddWithValue("@UserId", userId);

                        object headerResult = headerCmd.ExecuteScalar();
                        reconciliationId = headerResult == null || headerResult == DBNull.Value ? 0 : Convert.ToInt32(headerResult);
                    }

                    if (clearedRows != null)
                    {
                        foreach (DataRow row in clearedRows.Rows)
                        {
                            int entryId = row["entry_id"] == DBNull.Value ? 0 : Convert.ToInt32(row["entry_id"]);
                            if (entryId <= 0)
                            {
                                continue;
                            }

                            bool isCleared = row.Table.Columns.Contains("is_cleared") && row["is_cleared"] != DBNull.Value && Convert.ToBoolean(row["is_cleared"]);

                            using (SqlCommand itemCmd = new SqlCommand("sp_BankReconciliation", cn, tx))
                            {
                                itemCmd.CommandType = CommandType.StoredProcedure;
                                itemCmd.Parameters.AddWithValue("@OperationType", 3);
                                itemCmd.Parameters.AddWithValue("@ReconciliationId", reconciliationId);
                                itemCmd.Parameters.AddWithValue("@BranchId", UsersModal.logged_in_branch_id);
                                itemCmd.Parameters.AddWithValue("@BankAccountId", bankAccountId);
                                itemCmd.Parameters.AddWithValue("@StatementDate", statementDate.Date);
                                itemCmd.Parameters.AddWithValue("@EntryId", entryId);
                                itemCmd.Parameters.AddWithValue("@IsCleared", isCleared);
                                itemCmd.Parameters.AddWithValue("@UserId", userId);
                                itemCmd.ExecuteNonQuery();
                            }
                        }
                    }

                    tx.Commit();

                    Log.LogAction("Save Bank Reconciliation", "Bank Account ID: " + bankAccountId + ", Statement Date: " + statementDate.ToString("yyyy-MM-dd"), userId, UsersModal.logged_in_branch_id);
                    return reconciliationId;
                }
                catch
                {
                    if (tx != null)
                    {
                        tx.Rollback();
                    }

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

                        cmd = new SqlCommand("SELECT * FROM pos_banks WHERE branch_id=@branch_id AND name LIKE @name OR id LIKE @id ", cn);
                        //cmd.Parameters.AddWithValue("@id", condition);
                        cmd.Parameters.AddWithValue("@name", string.Format("%{0}%", condition));
                        cmd.Parameters.AddWithValue("@id", string.Format("%{0}%", condition));
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

        public int Insert(BankModal obj)
        {
             
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                Int32 result = 0;
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_banksCrud", cn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);

                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@GLAccountID", obj.GLAccountID);
                        cmd.Parameters.AddWithValue("@accountNo", obj.accountNo);
                        cmd.Parameters.AddWithValue("@bankBranch", obj.bankBranch);
                        cmd.Parameters.AddWithValue("@holderName", obj.holderName);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "1");

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                        //App logging 

                    }

                    result = Convert.ToInt32(cmd.ExecuteScalar());

                    Log.LogAction("Add Bank", $"Bank ID: {result}, Bank Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                }
                catch
                {

                    throw;
                }
                return (int)result;
            }
        }

        public int Update(BankModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_banksCrud", cn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        //cmd.Parameters.AddWithValue("@branch_id", 0);
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);

                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@accountNo", obj.accountNo);
                        cmd.Parameters.AddWithValue("@GLAccountID", obj.GLAccountID);
                        cmd.Parameters.AddWithValue("@bankBranch", obj.bankBranch);
                        cmd.Parameters.AddWithValue("@holderName", obj.holderName);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "2");

                        //--operation types   
                        //-- 1) Insert  
                        //-- 2) Update  
                        //-- 3) Delete  
                        //-- 4) Select Perticular Record  
                        //-- 5) Selec All 
                        Log.LogAction("Update Bank", $"Bank ID: {obj.id}, Bank Name: {obj.name}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

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

        public int Delete(int BankId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_banksCrud", cn)
                        {
                            CommandType = CommandType.StoredProcedure
                        };
                        cmd.Parameters.AddWithValue("@id", BankId); 
                        cmd.Parameters.AddWithValue("@OperationType", "3");
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                    }

                    Log.LogAction("Delete Bank", $"Bank ID: {BankId}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);

                    int result = cmd.ExecuteNonQuery();
                    return result;
                }
                catch
                {

                    throw;
                }
            }
        }

        public int DeletePaymentTransaction(string invoiceNo)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                SqlTransaction transaction = null;
                try
                {
                    int result = 0;

                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                    }

                    transaction = cn.BeginTransaction();

                    result += DeletePaymentTransactionRecords(cn, transaction, "pos_banks_payments", invoiceNo);
                    result += DeletePaymentTransactionRecords(cn, transaction, "pos_customers_payments", invoiceNo);
                    result += DeletePaymentTransactionRecords(cn, transaction, "pos_suppliers_payments", invoiceNo);
                    result += DeleteOptionalPaymentTransactionRecords(cn, transaction, "acc_entries_header", invoiceNo);
                    result += DeletePaymentTransactionRecords(cn, transaction, "acc_entries", invoiceNo);

                    transaction.Commit();

                    if (result > 0)
                    {
                        Log.LogAction("Delete Bank Payment", $"InvoiceNo: {invoiceNo}", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
                    }

                    return result;
                }
                catch
                {
                    if (transaction != null)
                    {
                        transaction.Rollback();
                    }

                    throw;
                }
            }
        }

        private static int DeletePaymentTransactionRecords(SqlConnection cn, SqlTransaction transaction, string tableName, string invoiceNo)
        {
            using (SqlCommand deleteCommand = new SqlCommand($"DELETE FROM {tableName} WHERE invoice_no = @invoice_no AND branch_id = @branch_id", cn, transaction))
            {
                deleteCommand.Parameters.Add("@invoice_no", SqlDbType.NVarChar).Value = invoiceNo;
                deleteCommand.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;
                return deleteCommand.ExecuteNonQuery();
            }
        }
        private static int DeleteOptionalPaymentTransactionRecords(SqlConnection cn, SqlTransaction transaction, string invoiceNo, string tableName)
        {
            using (SqlCommand countCommand = new SqlCommand($"SELECT COUNT(1) FROM {tableName} WHERE InvoiceNo = @invoice_no AND branch_id = @branch_id", cn, transaction))
            {
                countCommand.Parameters.Add("@invoice_no", SqlDbType.NVarChar).Value = invoiceNo;
                countCommand.Parameters.Add("@branch_id", SqlDbType.Int).Value = UsersModal.logged_in_branch_id;

                if (Convert.ToInt32(countCommand.ExecuteScalar()) <= 0)
                    return 0;
            }

            return DeletePaymentTransactionRecords(cn, transaction, tableName, invoiceNo);
        }

    }
}
