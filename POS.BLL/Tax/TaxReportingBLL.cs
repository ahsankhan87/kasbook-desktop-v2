using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace POS.BLL
{
    /// <summary>
    /// Business logic layer for Tax Trial Balance reporting
    /// </summary>
    public class TaxReportingBLL
    {
        private TaxTrialBalanceDLL _taxTrialBalanceDll = new TaxTrialBalanceDLL();

        /// <summary>
        /// Gets complete tax trial balance
        /// </summary>
        public List<TaxTrialBalanceModal> GetIncomeTaxTrialBalance(int financialYearId)
        {
            return _taxTrialBalanceDll.GetIncomeTaxTrialBalanceList(financialYearId);
        }

        /// <summary>
        /// Gets trial balance as data table for grid binding
        /// </summary>
        public DataTable GetIncomeTaxTrialBalanceDataTable(int financialYearId)
        {
            return _taxTrialBalanceDll.GetIncomeTaxTrialBalance(financialYearId);
        }

        /// <summary>
        /// Gets income accounts only
        /// </summary>
        public List<TaxTrialBalanceModal> GetIncomeAccounts(int financialYearId)
        {
            return _taxTrialBalanceDll.GetIncomeAccounts(financialYearId);
        }

        /// <summary>
        /// Gets expense accounts only
        /// </summary>
        public List<TaxTrialBalanceModal> GetExpenseAccounts(int financialYearId)
        {
            return _taxTrialBalanceDll.GetExpenseAccounts(financialYearId);
        }

        /// <summary>
        /// Gets total income and total expenses
        /// </summary>
        public (decimal TotalIncome, decimal TotalExpense) GetIncomeTaxTotals(int financialYearId)
        {
            return _taxTrialBalanceDll.GetIncomeTaxTotals(financialYearId);
        }

        /// <summary>
        /// Calculates net income (income - expense)
        /// </summary>
        public decimal GetNetIncome(int financialYearId)
        {
            var (totalIncome, totalExpense) = GetIncomeTaxTotals(financialYearId);
            return totalIncome - totalExpense;
        }

        /// <summary>
        /// Prepares trial balance data for export with summary totals
        /// </summary>
        public DataTable PrepareExportData(int financialYearId)
        {
            DataTable dt = GetIncomeTaxTrialBalanceDataTable(financialYearId);

            if (dt != null)
            {
                // Rename columns for export-friendly display
                dt.Columns["AccountCode"].ColumnName = "Code";
                dt.Columns["AccountName"].ColumnName = "Account Name";
                dt.Columns["AccountGroupName"].ColumnName = "Account Group";
                dt.Columns["AccountType"].ColumnName = "Type";
                dt.Columns["DebitAmount"].ColumnName = "Debit";
                dt.Columns["CreditAmount"].ColumnName = "Credit";
                dt.Columns["Balance"].ColumnName = "Balance";

                // Remove unnecessary columns
                if (dt.Columns.Contains("AccountId"))
                    dt.Columns.Remove("AccountId");
            }

            return dt;
        }

        /// <summary>
        /// Gets trial balance summary with group totals
        /// </summary>
        public DataTable GetTrialBalanceSummary(int financialYearId)
        {
            List<TaxTrialBalanceModal> accounts = GetIncomeTaxTrialBalance(financialYearId);

            DataTable summaryDt = new DataTable();
            summaryDt.Columns.Add("Account Type", typeof(string));
            summaryDt.Columns.Add("Count", typeof(int));
            summaryDt.Columns.Add("Total Debit", typeof(decimal));
            summaryDt.Columns.Add("Total Credit", typeof(decimal));
            summaryDt.Columns.Add("Total Balance", typeof(decimal));

            if (accounts != null && accounts.Count > 0)
            {
                var groupedByType = accounts.GroupBy(a => a.AccountType);

                foreach (var group in groupedByType)
                {
                    summaryDt.Rows.Add(
                        group.Key,
                        group.Count(),
                        group.Sum(a => a.DebitAmount),
                        group.Sum(a => a.CreditAmount),
                        group.Sum(a => a.Balance)
                    );
                }

                // Add total row
                DataRow totalRow = summaryDt.NewRow();
                totalRow["Account Type"] = "TOTAL";
                totalRow["Count"] = accounts.Count;
                totalRow["Total Debit"] = accounts.Sum(a => a.DebitAmount);
                totalRow["Total Credit"] = accounts.Sum(a => a.CreditAmount);
                totalRow["Total Balance"] = accounts.Sum(a => a.Balance);
                summaryDt.Rows.Add(totalRow);
            }

            return summaryDt;
        }

        /// <summary>
        /// Gets data grouped by account group for hierarchical display
        /// </summary>
        public DataTable GetTrialBalanceByGroup(int financialYearId)
        {
            List<TaxTrialBalanceModal> accounts = GetIncomeTaxTrialBalance(financialYearId);

            DataTable groupedDt = new DataTable();
            groupedDt.Columns.Add("Account Group", typeof(string));
            groupedDt.Columns.Add("Account Code", typeof(string));
            groupedDt.Columns.Add("Account Name", typeof(string));
            groupedDt.Columns.Add("Type", typeof(string));
            groupedDt.Columns.Add("Debit", typeof(decimal));
            groupedDt.Columns.Add("Credit", typeof(decimal));
            groupedDt.Columns.Add("Balance", typeof(decimal));

            if (accounts != null && accounts.Count > 0)
            {
                var groupedByGroup = accounts.GroupBy(a => a.AccountGroupName);

                foreach (var group in groupedByGroup)
                {
                    // Add group header row (optional, for hierarchical display)
                    // DataRow headerRow = groupedDt.NewRow();
                    // headerRow["Account Group"] = group.Key;
                    // groupedDt.Rows.Add(headerRow);

                    foreach (var account in group)
                    {
                        groupedDt.Rows.Add(
                            account.AccountGroupName,
                            account.AccountCode,
                            account.AccountName,
                            account.AccountType,
                            account.DebitAmount,
                            account.CreditAmount,
                            account.Balance
                        );
                    }
                }
            }

            return groupedDt;
        }
    }
}
