using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    /// <summary>
    /// Data access layer for Tax Trial Balance queries
    /// </summary>
    public class TaxTrialBalanceDLL
    {
        private dbConnection _db = new dbConnection();

        /// <summary>
        /// Retrieves trial balance filtered for income and expense accounts (tax purposes)
        /// </summary>
        public DataTable GetIncomeTaxTrialBalance(int financialYearId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[]
                {
                    new SqlParameter("@FinancialYearId", financialYearId)
                };

                using (SqlConnection conn = new SqlConnection(dbConnection.ConnectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("sp_IncomeTaxTrialBalance", conn))
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
        /// Retrieves trial balance as modal list
        /// </summary>
        public List<TaxTrialBalanceModal> GetIncomeTaxTrialBalanceList(int financialYearId)
        {
            List<TaxTrialBalanceModal> result = new List<TaxTrialBalanceModal>();
            DataTable dt = GetIncomeTaxTrialBalance(financialYearId);

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    result.Add(new TaxTrialBalanceModal
                    {
                        AccountId = Convert.ToInt32(row["AccountId"]),
                        AccountCode = Convert.ToString(row["AccountCode"]),
                        AccountName = Convert.ToString(row["AccountName"]),
                        AccountGroupName = Convert.ToString(row["AccountGroupName"] ?? string.Empty),
                        AccountType = Convert.ToString(row["AccountType"]),
                        DebitAmount = Convert.ToDecimal(row["DebitAmount"]),
                        CreditAmount = Convert.ToDecimal(row["CreditAmount"]),
                        Balance = Convert.ToDecimal(row["Balance"])
                    });
                }
            }

            return result;
        }

        /// <summary>
        /// Gets trial balance filtered by account type
        /// </summary>
        public List<TaxTrialBalanceModal> GetTrialBalanceByType(int financialYearId, string accountType)
        {
            List<TaxTrialBalanceModal> result = new List<TaxTrialBalanceModal>();
            List<TaxTrialBalanceModal> allAccounts = GetIncomeTaxTrialBalanceList(financialYearId);

            foreach (var account in allAccounts)
            {
                if (account.AccountType.Equals(accountType, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(account);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets income accounts only
        /// </summary>
        public List<TaxTrialBalanceModal> GetIncomeAccounts(int financialYearId)
        {
            return GetTrialBalanceByType(financialYearId, "Income");
        }

        /// <summary>
        /// Gets expense accounts only
        /// </summary>
        public List<TaxTrialBalanceModal> GetExpenseAccounts(int financialYearId)
        {
            return GetTrialBalanceByType(financialYearId, "Expense");
        }

        /// <summary>
        /// Gets total income and total expenses
        /// </summary>
        public (decimal TotalIncome, decimal TotalExpense) GetIncomeTaxTotals(int financialYearId)
        {
            decimal totalIncome = 0;
            decimal totalExpense = 0;

            List<TaxTrialBalanceModal> allAccounts = GetIncomeTaxTrialBalanceList(financialYearId);

            foreach (var account in allAccounts)
            {
                if (account.AccountType.Equals("Income", StringComparison.OrdinalIgnoreCase))
                {
                    totalIncome += account.Balance;
                }
                else if (account.AccountType.Equals("Expense", StringComparison.OrdinalIgnoreCase))
                {
                    totalExpense += account.Balance;
                }
            }

            return (totalIncome, totalExpense);
        }
    }
}
