using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    /// <summary>
    /// Data access layer for Sales Tax Register queries
    /// </summary>
    public class TaxRegistrationDLL
    {
        private dbConnection _db = new dbConnection();

        /// <summary>
        /// Retrieves sales and purchase tax register for a date range
        /// </summary>
        public DataTable GetSalesTaxRegister(DateTime fromDate, DateTime toDate, string taxType = "ALL")
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FromDate", fromDate),
                    new SqlParameter("@ToDate", toDate),
                    new SqlParameter("@TaxType", taxType)
                };

                using (SqlConnection conn = new SqlConnection(dbConnection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_SalesTaxRegister", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddRange(parameters);

                        DataTable dt = new DataTable();
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        return dt;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Retrieves sales tax register as modal list
        /// </summary>
        public List<SalesTaxModal> GetSalesTaxRegisterList(DateTime fromDate, DateTime toDate, string taxType = "ALL")
        {
            List<SalesTaxModal> result = new List<SalesTaxModal>();
            DataTable dt = GetSalesTaxRegister(fromDate, toDate, taxType);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new SalesTaxModal
                    {
                        TransactionId = Convert.ToInt32(row["TransactionId"]),
                        TransactionType = Convert.ToString(row["TransactionType"]),
                        InvoiceNo = Convert.ToString(row["InvoiceNo"]),
                        TransactionDate = Convert.ToDateTime(row["Date"]),
                        PartyName = Convert.ToString(row["PartyName"]),
                        NTN = Convert.ToString(row["NTN"] ?? string.Empty),
                        Amount = Convert.ToDecimal(row["Amount"]),
                        TaxRate = Convert.ToDecimal(row["TaxRate"]),
                        TaxAmount = Convert.ToDecimal(row["TaxAmount"]),
                        Remarks = Convert.ToString(row["Remarks"] ?? string.Empty),
                        BranchId = Convert.ToInt32(row["branch_id"])
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets only sales transactions
        /// </summary>
        public List<SalesTaxModal> GetSalesTaxRegisterSalesOnly(DateTime fromDate, DateTime toDate)
        {
            return GetSalesTaxRegisterList(fromDate, toDate, "SALES");
        }

        /// <summary>
        /// Gets only purchase transactions
        /// </summary>
        public List<SalesTaxModal> GetSalesTaxRegisterPurchasesOnly(DateTime fromDate, DateTime toDate)
        {
            return GetSalesTaxRegisterList(fromDate, toDate, "PURCHASES");
        }
    }
}
