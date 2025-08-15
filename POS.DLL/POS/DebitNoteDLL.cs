using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using POS.Core;
using POS.Core.POS;

namespace POS.DLL.POS
{
    public class DebitNoteDLL
    {
        public void AddDebitNote(DebitNoteModal note)
        {
            // Example using ADO.NET; replace with your ORM if needed
            SqlConnection conn = new SqlConnection(dbConnection.ConnectionString);
            conn.Open();
            SqlTransaction tran = conn.BeginTransaction();
            try
            {
                // First insert: pos_debitNotes
                using (SqlCommand cmd = new SqlCommand("INSERT INTO pos_debitNotes (OriginalInvoiceId, DebitNoteNumber, IssueDate, Amount,VATAmount,TotalAmount, Reason, CustomerId,user_id,branch_id) " +
                    "VALUES (@OriginalInvoiceId, @DebitNoteNumber, @IssueDate, @Amount,@VATAmount,@TotalAmount, @Reason, @CustomerId,@userId,@branchId)", conn, tran))
                {
                    _ = cmd.Parameters.AddWithValue("@OriginalInvoiceId", note.OriginalInvoiceId);
                    _ = cmd.Parameters.AddWithValue("@DebitNoteNumber", note.DebitNoteNumber);
                    _ = cmd.Parameters.AddWithValue("@IssueDate", note.IssueDate);
                    _ = cmd.Parameters.AddWithValue("@Amount", note.Amount);
                    _ = cmd.Parameters.AddWithValue("@VATAmount", note.VATAmount);
                    _ = cmd.Parameters.AddWithValue("@TotalAmount", note.TotalAmount);
                    _ = cmd.Parameters.AddWithValue("@Reason", note.Reason);
                    _ = cmd.Parameters.AddWithValue("@CustomerId", note.CustomerId);
                    _ = cmd.Parameters.AddWithValue("@userId", UsersModal.logged_in_userid);
                    _ = cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);
                    cmd.ExecuteNonQuery();
                }

                int newSaleID = 0;
                // Second insert: pos_sales
                using (SqlCommand cmd = new SqlCommand("INSERT INTO pos_sales (invoice_no,sale_time,prevInvoiceNo, sale_date,sale_type,account, total_amount,total_tax,discount_value,customer_id,user_id,branch_id, returnReason,invoice_subtype_code) " +
                    "VALUES (@DebitNoteNumber,@sale_time,@OriginalInvoiceId, @IssueDate,@sale_type,@account, @TotalAmount,@VATAmount,@discount_value,@CustomerId,@userId,@branchId, @Reason,@InvoiceSubTypeCode); SELECT SCOPE_IDENTITY();", conn, tran))
                {
                    _ = cmd.Parameters.AddWithValue("@OriginalInvoiceId", note.OriginalInvoiceId);
                    _ = cmd.Parameters.AddWithValue("@IssueDate", note.IssueDate);
                    _ = cmd.Parameters.AddWithValue("@DebitNoteNumber", note.DebitNoteNumber);
                    _ = cmd.Parameters.AddWithValue("@sale_time", DateTime.Now);
                    _ = cmd.Parameters.AddWithValue("@sale_type", "Cash");
                    _ = cmd.Parameters.AddWithValue("@account", "Debit");
                    _ = cmd.Parameters.AddWithValue("@VATAmount", note.VATAmount);
                    _ = cmd.Parameters.AddWithValue("@TotalAmount", note.Amount);
                    _ = cmd.Parameters.AddWithValue("@discount_value", 0);
                    _ = cmd.Parameters.AddWithValue("@Reason", note.Reason);
                    _ = cmd.Parameters.AddWithValue("@InvoiceSubTypeCode", note.InvoiceSubTypeCode);
                    _ = cmd.Parameters.AddWithValue("@CustomerId", note.CustomerId);
                    _ = cmd.Parameters.AddWithValue("@userId", UsersModal.logged_in_userid);
                    _ = cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);

                    object result = cmd.ExecuteScalar();
                    newSaleID = Convert.ToInt32(result);
                }

                // Third insert: pos_sales_items
                using (SqlCommand cmd = new SqlCommand("INSERT INTO pos_sales_items (invoice_no,sale_id,quantity_sold, unit_price, tax_rate,branch_id) " +
                    "VALUES (@DebitNoteNumber,@newSaleID,@quantity_sold, @unit_price,@tax_rate, @branchId)", conn, tran))
                {
                    _ = cmd.Parameters.AddWithValue("@newSaleID", newSaleID);
                    _ = cmd.Parameters.AddWithValue("@DebitNoteNumber", note.DebitNoteNumber);
                    _ = cmd.Parameters.AddWithValue("@quantity_sold", 1);
                    _ = cmd.Parameters.AddWithValue("@unit_price", note.Amount);
                    _ = cmd.Parameters.AddWithValue("@tax_rate", 15);
                    _ = cmd.Parameters.AddWithValue("@discount_value", 0);
                    _ = cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);
                    cmd.ExecuteNonQuery();
                }

                tran.Commit();
            }
            catch
            {
                try { tran.Rollback(); } catch { /* ignore rollback errors */ }
                throw;
            }
            finally
            {
                conn.Close();
                conn.Dispose();
            }
        }

        //public void AddDebitNote(DebitNoteModal note)
        //{
        //    // Example using ADO.NET; replace with your ORM if needed
        //    using (var conn = new SqlConnection(dbConnection.ConnectionString))
        //    using (var cmd = new SqlCommand("INSERT INTO pos_debitNotes (OriginalInvoiceId, DebitNoteNumber, IssueDate, Amount,VATAmount,TotalAmount, Reason, CustomerId,user_id,branch_id) " +
        //        "VALUES (@OriginalInvoiceId, @DebitNoteNumber, @IssueDate, @Amount,@VATAmount,@TotalAmount, @Reason, @CustomerId,@userId,@branchId)", conn))
        //    {
        //        cmd.Parameters.AddWithValue("@OriginalInvoiceId", note.OriginalInvoiceId);
        //        cmd.Parameters.AddWithValue("@DebitNoteNumber", note.DebitNoteNumber);
        //        cmd.Parameters.AddWithValue("@IssueDate", note.IssueDate);
        //        cmd.Parameters.AddWithValue("@Amount", note.Amount);
        //        cmd.Parameters.AddWithValue("@VATAmount", note.VATAmount);
        //        cmd.Parameters.AddWithValue("@TotalAmount", note.TotalAmount);
        //        cmd.Parameters.AddWithValue("@Reason", note.Reason);
        //        cmd.Parameters.AddWithValue("@CustomerId", note.CustomerId);
        //        cmd.Parameters.AddWithValue("@userId", UsersModal.logged_in_userid);
        //        cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);
        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
            
        //    int newSaleID = 0;
        //    using (var conn = new SqlConnection(dbConnection.ConnectionString))
        //    using (var cmd = new SqlCommand("INSERT INTO pos_sales (invoice_no,sale_time,prevInvoiceNo, sale_date,sale_type,account, total_amount,total_tax,discount_value,customer_id,user_id,branch_id, returnReason,invoice_subtype_code) " +
        //        "VALUES (@DebitNoteNumber,@sale_time,@OriginalInvoiceId, @IssueDate,@sale_type,@account, @TotalAmount,@VATAmount,@discount_value,@CustomerId,@userId,@branchId, @Reason,@InvoiceSubTypeCode); SELECT SCOPE_IDENTITY();", conn))
        //    {
        //        cmd.Parameters.AddWithValue("@OriginalInvoiceId", note.OriginalInvoiceId);
        //        cmd.Parameters.AddWithValue("@IssueDate", note.IssueDate);
        //        cmd.Parameters.AddWithValue("@DebitNoteNumber", note.DebitNoteNumber);
        //        cmd.Parameters.AddWithValue("@sale_time", DateTime.Now);
        //        cmd.Parameters.AddWithValue("@sale_type", "Cash");
        //        cmd.Parameters.AddWithValue("@account", "Debit");
        //        cmd.Parameters.AddWithValue("@VATAmount", note.VATAmount);
        //        cmd.Parameters.AddWithValue("@TotalAmount", note.Amount);
        //        cmd.Parameters.AddWithValue("@discount_value", 0);

        //        cmd.Parameters.AddWithValue("@Reason", note.Reason);
        //        cmd.Parameters.AddWithValue("@InvoiceSubTypeCode", note.InvoiceSubTypeCode);
        //        cmd.Parameters.AddWithValue("@CustomerId", note.CustomerId);
        //        cmd.Parameters.AddWithValue("@userId", UsersModal.logged_in_userid);
        //        cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);

        //        conn.Open();
              
        //        object result = cmd.ExecuteScalar();
        //        newSaleID = Convert.ToInt32(result);
        //    }

        //    using (var conn = new SqlConnection(dbConnection.ConnectionString))
        //    using (var cmd = new SqlCommand("INSERT INTO pos_sales_items (invoice_no,sale_id,quantity_sold, unit_price, tax_rate,branch_id) " +
        //        "VALUES (@DebitNoteNumber,@newSaleID,@quantity_sold, @unit_price,@tax_rate, @branchId)", conn))
        //    {
        //        cmd.Parameters.AddWithValue("@newSaleID", newSaleID);
        //        cmd.Parameters.AddWithValue("@DebitNoteNumber", note.DebitNoteNumber);
        //        cmd.Parameters.AddWithValue("@quantity_sold", 1);
        //        cmd.Parameters.AddWithValue("@unit_price", note.Amount);
        //        cmd.Parameters.AddWithValue("@tax_rate", 15);
                
        //        cmd.Parameters.AddWithValue("@discount_value", 0);

               
        //        cmd.Parameters.AddWithValue("@branchId", UsersModal.logged_in_branch_id);

        //        conn.Open();
        //        cmd.ExecuteNonQuery();
        //    }
        //}

        public DebitNoteModal GetDebitNote(int debitNoteId)
        {
            using (var conn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand("SELECT * FROM pos_debitNotes WHERE DebitNoteId = @DebitNoteId", conn))
            {
                cmd.Parameters.AddWithValue("@DebitNoteId", debitNoteId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new DebitNoteModal
                        {
                            DebitNoteId = (int)reader["DebitNoteId"],
                            OriginalInvoiceId = (string)reader["OriginalInvoiceId"],
                            DebitNoteNumber = (string)reader["DebitNoteNumber"],
                            IssueDate = (DateTime)reader["IssueDate"],
                            Amount = (decimal)reader["Amount"],
                            Reason = (string)reader["Reason"],
                            CustomerName = (string)reader["CustomerName"],
                            ZatcaUuid = (string)reader["ZatcaUuid"]
                        };
                    }
                }
            }
            return null;
        }

        
        public List<DebitNoteModal> GetAllDebitNotes()
        {
            var notes = new List<DebitNoteModal>();
            using (var conn = new SqlConnection(dbConnection.ConnectionString))
            using (var cmd = new SqlCommand("SELECT * FROM pos_debitNotes order by DebitNoteId desc", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        notes.Add(new DebitNoteModal
                        {
                            DebitNoteId = reader.HasColumn("DebitNoteId") && reader["DebitNoteId"] != DBNull.Value ? Convert.ToInt32(reader["DebitNoteId"]) : 0,
                            OriginalInvoiceId = reader.HasColumn("OriginalInvoiceId") && reader["OriginalInvoiceId"] != DBNull.Value ? reader["OriginalInvoiceId"].ToString() : "",
                            DebitNoteNumber = reader.HasColumn("DebitNoteNumber") && reader["DebitNoteNumber"] != DBNull.Value ? reader["DebitNoteNumber"].ToString() : string.Empty,
                            IssueDate = reader.HasColumn("IssueDate") && reader["IssueDate"] != DBNull.Value ? Convert.ToDateTime(reader["IssueDate"]) : DateTime.MinValue,
                            Amount = reader.HasColumn("Amount") && reader["Amount"] != DBNull.Value ? Convert.ToDecimal(reader["Amount"]) : 0m,
                            VATAmount = reader.HasColumn("VATAmount") && reader["VATAmount"] != DBNull.Value ? Convert.ToDecimal(reader["VATAmount"]) : 0m,
                            TotalAmount = reader.HasColumn("TotalAmount") && reader["TotalAmount"] != DBNull.Value ? Convert.ToDecimal(reader["TotalAmount"]) : 0m,
                            Reason = reader.HasColumn("Reason") && reader["Reason"] != DBNull.Value ? reader["Reason"].ToString() : string.Empty,
                            CustomerId = reader.HasColumn("CustomerId") && reader["CustomerId"] != DBNull.Value ? Convert.ToInt32(reader["CustomerId"]) : 0,
                        });
                    }
                }
            }
            return notes;
        }

        // Add Update, Delete, and List methods as needed
    }
    
}

public static class SqlDataReaderExtensions
{
    public static bool HasColumn(this SqlDataReader reader, string columnName)
    {
        for (int i = 0; i < reader.FieldCount; i++)
        {
            if (reader.GetName(i).Equals(columnName, StringComparison.InvariantCultureIgnoreCase))
                return true;
        }
        return false;
    }
}
