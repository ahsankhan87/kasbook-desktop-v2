using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using POS.Core;

namespace POS.DLL
{
    public class BudgetDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;

        /// <summary>
        /// Gets all budget headers
        /// </summary>
        public DataTable GetAllBudgetHeaders()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        SELECT 
                            bh.*,
                            fy.name AS fiscal_year,
                            fy.from_date AS start_date,
                            fy.to_date AS end_date,
                            cc.cc_name,
                            u.name AS created_by_name,
                            approver.name AS approved_by_name
                        FROM acc_budget_headers bh
                        INNER JOIN acc_fiscal_years fy ON bh.financial_year_id = fy.id
                        LEFT JOIN acc_cost_centers cc ON bh.cc_id = cc.cc_id
                        LEFT JOIN pos_users u ON bh.created_by = u.id
                        LEFT JOIN pos_users approver ON bh.approved_by = approver.id
                        ORDER BY bh.created_at DESC";

                    cmd = new SqlCommand(query, cn);
                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets a specific budget header by ID
        /// </summary>
        public DataTable GetBudgetHeaderById(int budgetId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        SELECT 
                            bh.*,
                            fy.name AS fiscal_year,
                            fy.from_date AS start_date,
                            fy.to_date AS end_date,
                            cc.cc_name
                        FROM acc_budget_headers bh
                        INNER JOIN acc_fiscal_years fy ON bh.financial_year_id = fy.id
                        LEFT JOIN acc_cost_centers cc ON bh.cc_id = cc.cc_id
                        WHERE bh.budget_id = @budget_id";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@budget_id", budgetId);
                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets budget lines for a specific budget
        /// </summary>
        public DataTable GetBudgetLines(int budgetId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        SELECT 
                            bl.*,
                            bl.account_id AS acc_id,
                            a.code AS acc_code,
                            a.name AS acc_name,
                            ISNULL(t.name, '') AS account_type
                        FROM acc_budget_lines bl
                        INNER JOIN acc_accounts a ON bl.account_id = a.id
                        INNER JOIN acc_groups g ON a.group_id = g.id
                        LEFT JOIN acc_account_type t ON g.account_type_id = t.id
                        WHERE bl.budget_id = @budget_id
                        ORDER BY a.code";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@budget_id", budgetId);
                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Inserts a new budget header
        /// </summary>
        public int InsertBudgetHeader(BudgetHeaderModal modal)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        INSERT INTO acc_budget_headers 
                        (financial_year_id, budget_version, cc_id, budget_name, status, notes, created_by, created_at)
                        VALUES 
                        (@financial_year_id, @budget_version, @cc_id, @budget_name, @status, @notes, @created_by, @created_at);
                        SELECT SCOPE_IDENTITY();";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@financial_year_id", modal.financial_year_id);
                    cmd.Parameters.AddWithValue("@budget_version", modal.budget_version ?? "V1");
                    cmd.Parameters.AddWithValue("@cc_id", (object)modal.cc_id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@budget_name", modal.budget_name);
                    cmd.Parameters.AddWithValue("@status", modal.status ?? "Draft");
                    cmd.Parameters.AddWithValue("@notes", (object)modal.notes ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@created_by", modal.created_by);
                    cmd.Parameters.AddWithValue("@created_at", modal.created_at);

                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates an existing budget header
        /// </summary>
        public void UpdateBudgetHeader(BudgetHeaderModal modal)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        UPDATE acc_budget_headers 
                        SET 
                            financial_year_id = @financial_year_id,
                            budget_version = @budget_version,
                            cc_id = @cc_id,
                            budget_name = @budget_name,
                            status = @status,
                            notes = @notes
                        WHERE budget_id = @budget_id";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@budget_id", modal.budget_id);
                    cmd.Parameters.AddWithValue("@financial_year_id", modal.financial_year_id);
                    cmd.Parameters.AddWithValue("@budget_version", modal.budget_version ?? "V1");
                    cmd.Parameters.AddWithValue("@cc_id", (object)modal.cc_id ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@budget_name", modal.budget_name);
                    cmd.Parameters.AddWithValue("@status", modal.status ?? "Draft");
                    cmd.Parameters.AddWithValue("@notes", (object)modal.notes ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes a budget header and all associated lines
        /// </summary>
        public void DeleteBudgetHeader(int budgetId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = "DELETE FROM acc_budget_headers WHERE budget_id = @budget_id";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@budget_id", budgetId);

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Inserts a budget line
        /// </summary>
        public void InsertBudgetLine(BudgetLineModal modal)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        INSERT INTO acc_budget_lines 
                        (budget_id, account_id, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec)
                        VALUES 
                        (@budget_id, @account_id, @jan, @feb, @mar, @apr, @may, @jun, @jul, @aug, @sep, @oct, @nov, @dec)";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@budget_id", modal.budget_id);
                    cmd.Parameters.AddWithValue("@account_id", modal.account_id);
                    cmd.Parameters.AddWithValue("@jan", modal.jan);
                    cmd.Parameters.AddWithValue("@feb", modal.feb);
                    cmd.Parameters.AddWithValue("@mar", modal.mar);
                    cmd.Parameters.AddWithValue("@apr", modal.apr);
                    cmd.Parameters.AddWithValue("@may", modal.may);
                    cmd.Parameters.AddWithValue("@jun", modal.jun);
                    cmd.Parameters.AddWithValue("@jul", modal.jul);
                    cmd.Parameters.AddWithValue("@aug", modal.aug);
                    cmd.Parameters.AddWithValue("@sep", modal.sep);
                    cmd.Parameters.AddWithValue("@oct", modal.oct);
                    cmd.Parameters.AddWithValue("@nov", modal.nov);
                    cmd.Parameters.AddWithValue("@dec", modal.dec);

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Updates a budget line
        /// </summary>
        public void UpdateBudgetLine(BudgetLineModal modal)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        UPDATE acc_budget_lines 
                        SET 
                            jan = @jan, feb = @feb, mar = @mar, 
                            apr = @apr, may = @may, jun = @jun, 
                            jul = @jul, aug = @aug, sep = @sep, 
                            oct = @oct, nov = @nov, dec = @dec
                        WHERE line_id = @line_id";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@line_id", modal.line_id);
                    cmd.Parameters.AddWithValue("@jan", modal.jan);
                    cmd.Parameters.AddWithValue("@feb", modal.feb);
                    cmd.Parameters.AddWithValue("@mar", modal.mar);
                    cmd.Parameters.AddWithValue("@apr", modal.apr);
                    cmd.Parameters.AddWithValue("@may", modal.may);
                    cmd.Parameters.AddWithValue("@jun", modal.jun);
                    cmd.Parameters.AddWithValue("@jul", modal.jul);
                    cmd.Parameters.AddWithValue("@aug", modal.aug);
                    cmd.Parameters.AddWithValue("@sep", modal.sep);
                    cmd.Parameters.AddWithValue("@oct", modal.oct);
                    cmd.Parameters.AddWithValue("@nov", modal.nov);
                    cmd.Parameters.AddWithValue("@dec", modal.dec);

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Deletes all budget lines for a budget and re-inserts them (for bulk update)
        /// </summary>
        public void SaveBudgetLinesBulk(int budgetId, DataTable budgetLinesTable)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    using (SqlTransaction trans = cn.BeginTransaction())
                    {
                        try
                        {
                            // Delete existing lines
                            cmd = new SqlCommand("DELETE FROM acc_budget_lines WHERE budget_id = @budget_id", cn, trans);
                            cmd.Parameters.AddWithValue("@budget_id", budgetId);
                            cmd.ExecuteNonQuery();

                            // Bulk insert using SqlBulkCopy
                            using (SqlBulkCopy bulkCopy = new SqlBulkCopy(cn, SqlBulkCopyOptions.Default, trans))
                            {
                                bulkCopy.DestinationTableName = "acc_budget_lines";
                                bulkCopy.ColumnMappings.Add("budget_id", "budget_id");
                                bulkCopy.ColumnMappings.Add("account_id", "account_id");
                                bulkCopy.ColumnMappings.Add("jan", "jan");
                                bulkCopy.ColumnMappings.Add("feb", "feb");
                                bulkCopy.ColumnMappings.Add("mar", "mar");
                                bulkCopy.ColumnMappings.Add("apr", "apr");
                                bulkCopy.ColumnMappings.Add("may", "may");
                                bulkCopy.ColumnMappings.Add("jun", "jun");
                                bulkCopy.ColumnMappings.Add("jul", "jul");
                                bulkCopy.ColumnMappings.Add("aug", "aug");
                                bulkCopy.ColumnMappings.Add("sep", "sep");
                                bulkCopy.ColumnMappings.Add("oct", "oct");
                                bulkCopy.ColumnMappings.Add("nov", "nov");
                                bulkCopy.ColumnMappings.Add("dec", "dec");

                                bulkCopy.WriteToServer(budgetLinesTable);
                            }

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Saves or replaces monthly budgets for a cost center and fiscal year using acc_budget_headers/acc_budget_lines.
        /// </summary>
        public void SaveCostCenterBudgets(int ccId, int yearId, List<AccountBudget> budgets, int userId)
        {
            if (ccId <= 0)
                throw new ArgumentException("Invalid cost center ID.", nameof(ccId));

            if (yearId <= 0)
                throw new ArgumentException("Invalid financial year ID.", nameof(yearId));

            if (budgets == null || budgets.Count == 0)
                throw new ArgumentException("At least one budget entry is required.", nameof(budgets));

            foreach (var budget in budgets)
            {
                if (budget.JanBudget < 0 || budget.FebBudget < 0 || budget.MarBudget < 0 ||
                    budget.AprBudget < 0 || budget.MayBudget < 0 || budget.JunBudget < 0 ||
                    budget.JulBudget < 0 || budget.AugBudget < 0 || budget.SepBudget < 0 ||
                    budget.OctBudget < 0 || budget.NovBudget < 0 || budget.DecBudget < 0)
                {
                    throw new ArgumentException("Budget amounts cannot be negative.");
                }
            }

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        int budgetId;

                        const string findBudgetSql = @"
SELECT TOP 1 budget_id
FROM acc_budget_headers
WHERE financial_year_id = @yearId AND cc_id = @ccId
ORDER BY CASE WHEN status = 'Active' THEN 0 WHEN status = 'Approved' THEN 1 ELSE 2 END,
         created_at DESC;";

                        using (SqlCommand findCmd = new SqlCommand(findBudgetSql, cn, tx))
                        {
                            findCmd.Parameters.AddWithValue("@yearId", yearId);
                            findCmd.Parameters.AddWithValue("@ccId", ccId);
                            object existingId = findCmd.ExecuteScalar();
                            budgetId = existingId == null || existingId == DBNull.Value ? 0 : Convert.ToInt32(existingId);
                        }

                        if (budgetId <= 0)
                        {
                            const string insertHeaderSql = @"
INSERT INTO acc_budget_headers
(financial_year_id, budget_version, cc_id, budget_name, status, notes, created_by, created_at)
VALUES
(@yearId, @version, @ccId, @name, 'Active', @notes, @userId, GETDATE());
SELECT CAST(SCOPE_IDENTITY() AS INT);";

                            using (SqlCommand insertHeaderCmd = new SqlCommand(insertHeaderSql, cn, tx))
                            {
                                insertHeaderCmd.Parameters.AddWithValue("@yearId", yearId);
                                insertHeaderCmd.Parameters.AddWithValue("@version", "Original");
                                insertHeaderCmd.Parameters.AddWithValue("@ccId", ccId);
                                insertHeaderCmd.Parameters.AddWithValue("@name", "Cost Center Budget");
                                insertHeaderCmd.Parameters.AddWithValue("@notes", "Auto-created from cost center budgeting flow.");
                                insertHeaderCmd.Parameters.AddWithValue("@userId", userId);
                                budgetId = (int)insertHeaderCmd.ExecuteScalar();
                            }
                        }
                        else
                        {
                            const string activateHeaderSql = @"
UPDATE acc_budget_headers
SET status = 'Active'
WHERE budget_id = @budgetId;";

                            using (SqlCommand activateCmd = new SqlCommand(activateHeaderSql, cn, tx))
                            {
                                activateCmd.Parameters.AddWithValue("@budgetId", budgetId);
                                activateCmd.ExecuteNonQuery();
                            }
                        }

                        const string deactivateOtherSql = @"
UPDATE acc_budget_headers
SET status = 'Approved'
WHERE financial_year_id = @yearId
  AND cc_id = @ccId
  AND budget_id <> @budgetId
  AND status = 'Active';";

                        using (SqlCommand deactivateCmd = new SqlCommand(deactivateOtherSql, cn, tx))
                        {
                            deactivateCmd.Parameters.AddWithValue("@yearId", yearId);
                            deactivateCmd.Parameters.AddWithValue("@ccId", ccId);
                            deactivateCmd.Parameters.AddWithValue("@budgetId", budgetId);
                            deactivateCmd.ExecuteNonQuery();
                        }

                        const string deleteLinesSql = "DELETE FROM acc_budget_lines WHERE budget_id = @budgetId";
                        using (SqlCommand deleteCmd = new SqlCommand(deleteLinesSql, cn, tx))
                        {
                            deleteCmd.Parameters.AddWithValue("@budgetId", budgetId);
                            deleteCmd.ExecuteNonQuery();
                        }

                        const string insertLineSql = @"
INSERT INTO acc_budget_lines
(budget_id, account_id, jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, dec)
VALUES
(@budgetId, @accountId, @jan, @feb, @mar, @apr, @may, @jun, @jul, @aug, @sep, @oct, @nov, @dec);";

                        foreach (var budget in budgets)
                        {
                            using (SqlCommand insertCmd = new SqlCommand(insertLineSql, cn, tx))
                            {
                                insertCmd.Parameters.AddWithValue("@budgetId", budgetId);
                                insertCmd.Parameters.AddWithValue("@accountId", budget.AccountId);
                                insertCmd.Parameters.AddWithValue("@jan", budget.JanBudget);
                                insertCmd.Parameters.AddWithValue("@feb", budget.FebBudget);
                                insertCmd.Parameters.AddWithValue("@mar", budget.MarBudget);
                                insertCmd.Parameters.AddWithValue("@apr", budget.AprBudget);
                                insertCmd.Parameters.AddWithValue("@may", budget.MayBudget);
                                insertCmd.Parameters.AddWithValue("@jun", budget.JunBudget);
                                insertCmd.Parameters.AddWithValue("@jul", budget.JulBudget);
                                insertCmd.Parameters.AddWithValue("@aug", budget.AugBudget);
                                insertCmd.Parameters.AddWithValue("@sep", budget.SepBudget);
                                insertCmd.Parameters.AddWithValue("@oct", budget.OctBudget);
                                insertCmd.Parameters.AddWithValue("@nov", budget.NovBudget);
                                insertCmd.Parameters.AddWithValue("@dec", budget.DecBudget);
                                insertCmd.ExecuteNonQuery();
                            }
                        }

                        tx.Commit();
                        Log.LogAction("Cost Center Budgets Set", "CC: " + ccId + ", Year: " + yearId + ", Accounts: " + budgets.Count, userId, UsersModal.logged_in_branch_id);
                    }
                    catch
                    {
                        tx.Rollback();
                        throw;
                    }
                }
            }
        }

        /// <summary>
        /// Gets budget alerts for a cost center in the current month using active budget header/lines.
        /// </summary>
        public List<BudgetAlertModel> GetCostCenterBudgetAlerts(int ccId, DateTime currentDate)
        {
            if (ccId <= 0)
                return new List<BudgetAlertModel>();

            var alerts = new List<BudgetAlertModel>();
            int currentMonth = currentDate.Month;
            DateTime monthStart = new DateTime(currentDate.Year, currentMonth, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                const string sql = @"
WITH SelectedBudget AS
(
    SELECT TOP 1 bh.budget_id
    FROM acc_budget_headers bh
    INNER JOIN acc_fiscal_years fy ON bh.financial_year_id = fy.id
    WHERE @AsOfDate BETWEEN fy.from_date AND fy.to_date
      AND (bh.branch_id = @CCId OR bh.branch_id IS NULL OR bh.branch_id = 0)
      AND bh.status IN ('Active', 'Approved')
    ORDER BY CASE WHEN bh.branch_id = @CCId THEN 0 ELSE 1 END,
             CASE bh.status WHEN 'Active' THEN 0 WHEN 'Approved' THEN 1 ELSE 2 END,
             bh.created_at DESC
)
SELECT
    @CCId AS branch_id,
    c.branch_code,
    bl.account_id,
    a.code AS account_code,
    a.name AS account_name,
    @Month AS current_month,
    ISNULL(
        CASE @Month
            WHEN 1 THEN bl.jan
            WHEN 2 THEN bl.feb
            WHEN 3 THEN bl.mar
            WHEN 4 THEN bl.apr
            WHEN 5 THEN bl.may
            WHEN 6 THEN bl.jun
            WHEN 7 THEN bl.jul
            WHEN 8 THEN bl.aug
            WHEN 9 THEN bl.sep
            WHEN 10 THEN bl.oct
            WHEN 11 THEN bl.nov
            WHEN 12 THEN bl.[dec]
        END,
        0
    ) AS budget_amount,
    ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0) AS actual_amount
FROM SelectedBudget ab
INNER JOIN acc_budget_lines bl ON bl.budget_id = ab.budget_id
INNER JOIN acc_accounts a ON a.id = bl.account_id
INNER JOIN acc_branches c ON c.branch_id = @CCId
LEFT JOIN acc_entries E
    ON E.account_id = bl.account_id
    AND E.branch_id = @CCId
    AND E.entry_date >= @FromDate
    AND E.entry_date <= @ToDate
GROUP BY c.branch_code, bl.account_id, a.code, a.name,
         bl.jan, bl.feb, bl.mar, bl.apr, bl.may, bl.jun,
         bl.jul, bl.aug, bl.sep, bl.oct, bl.nov, bl.[dec];";

                cn.Open();
                using (SqlCommand localCmd = new SqlCommand(sql, cn))
                {
                    localCmd.Parameters.AddWithValue("@CCId", ccId);
                    localCmd.Parameters.AddWithValue("@Month", currentMonth);
                    localCmd.Parameters.AddWithValue("@FromDate", monthStart.Date);
                    localCmd.Parameters.AddWithValue("@ToDate", monthEnd.Date);
                    localCmd.Parameters.AddWithValue("@AsOfDate", currentDate.Date);

                    using (SqlDataReader r = localCmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            decimal budgetAmount = Convert.ToDecimal(r["budget_amount"]);
                            decimal actualAmount = Convert.ToDecimal(r["actual_amount"]);
                            decimal overspendAmount = Math.Max(0, actualAmount - budgetAmount);

                            if (overspendAmount <= 0)
                                continue;

                            decimal overspendPercent = budgetAmount > 0 ? (actualAmount / budgetAmount) * 100 : 0;
                            string severity = overspendPercent > 120 ? "Critical" : overspendPercent > 105 ? "Warning" : "Info";

                            alerts.Add(new BudgetAlertModel
                            {
                                BranchId = ccId,
                                BranchCode = Convert.ToString(r["branch_code"]),
                                AccountId = Convert.ToInt32(r["account_id"]),
                                AccountCode = Convert.ToString(r["account_code"]),
                                AccountName = Convert.ToString(r["account_name"]),
                                CurrentMonth = currentMonth,
                                BudgetAmount = budgetAmount,
                                ActualAmount = actualAmount,
                                OverspendAmount = overspendAmount,
                                OverspendPercent = overspendPercent,
                                SeverityLevel = severity
                            });
                        }
                    }
                }
            }

            return alerts;
        }

        /// <summary>
        /// Checks if posting amount to an account in a branch would exceed active monthly budget.
        /// </summary>
        public BudgetCheckResult CheckBranchBudgetBeforePosting(int branchId, int accountId, decimal amount, DateTime date)
        {
            if (branchId <= 0 || accountId <= 0)
                return new BudgetCheckResult { Message = "Invalid branch or account." };

            int month = date.Month;
            DateTime monthStart = new DateTime(date.Year, month, 1);
            DateTime monthEnd = monthStart.AddMonths(1).AddDays(-1);

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                const string sql = @"
WITH SelectedBudget AS
(
    SELECT TOP 1 bh.budget_id
    FROM acc_budget_headers bh
    INNER JOIN acc_fiscal_years fy ON bh.financial_year_id = fy.id
    WHERE @AsOfDate BETWEEN fy.from_date AND fy.to_date
      AND (bh.branch_id = @BranchId OR bh.branch_id IS NULL OR bh.branch_id = 0)
      AND bh.status IN ('Active', 'Approved')
    ORDER BY CASE WHEN bh.branch_id = @BranchId THEN 0 ELSE 1 END,
             CASE bh.status WHEN 'Active' THEN 0 WHEN 'Approved' THEN 1 ELSE 2 END,
             bh.created_at DESC
)
SELECT TOP 1
    ISNULL(
        CASE @Month
            WHEN 1 THEN bl.jan
            WHEN 2 THEN bl.feb
            WHEN 3 THEN bl.mar
            WHEN 4 THEN bl.apr
            WHEN 5 THEN bl.may
            WHEN 6 THEN bl.jun
            WHEN 7 THEN bl.jul
            WHEN 8 THEN bl.aug
            WHEN 9 THEN bl.sep
            WHEN 10 THEN bl.oct
            WHEN 11 THEN bl.nov
            WHEN 12 THEN bl.[dec]
        END,
        0
    ) AS monthly_budget,
    ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0) AS current_actual
FROM SelectedBudget ab
INNER JOIN acc_budget_lines bl ON bl.budget_id = ab.budget_id
LEFT JOIN acc_entries E
    ON E.account_id = @AccountId
    AND E.branch_id = @BranchId
    AND E.entry_date >= @MonthStart
    AND E.entry_date <= @MonthEnd
WHERE bl.account_id = @AccountId
GROUP BY bl.jan, bl.feb, bl.mar, bl.apr, bl.may, bl.jun,
         bl.jul, bl.aug, bl.sep, bl.oct, bl.nov, bl.[dec];";

                cn.Open();
                using (SqlCommand localCmd = new SqlCommand(sql, cn))
                {
                    localCmd.Parameters.AddWithValue("@BranchId", branchId);
                    localCmd.Parameters.AddWithValue("@AccountId", accountId);
                    localCmd.Parameters.AddWithValue("@Month", month);
                    localCmd.Parameters.AddWithValue("@MonthStart", monthStart.Date);
                    localCmd.Parameters.AddWithValue("@MonthEnd", monthEnd.Date);
                    localCmd.Parameters.AddWithValue("@AsOfDate", date.Date);

                    using (SqlDataReader r = localCmd.ExecuteReader(CommandBehavior.SingleRow))
                    {
                        if (!r.Read())
                        {
                            return new BudgetCheckResult
                            {
                                IsOverBudget = false,
                                RemainingBudget = 0,
                                Message = "No budget defined for this account in this cost center.",
                                SeverityLevel = null
                            };
                        }

                        decimal monthlyBudget = Convert.ToDecimal(r["monthly_budget"]);
                        decimal currentActual = Convert.ToDecimal(r["current_actual"]);
                        decimal projectedActual = currentActual + amount;
                        decimal remainingBudget = monthlyBudget - projectedActual;
                        bool isOver = remainingBudget < 0;

                        string severity = null;
                        string message = "Budget: " + monthlyBudget.ToString("N2") + ", Current: " + currentActual.ToString("N2") + ", Projected: " + projectedActual.ToString("N2");

                        if (isOver)
                        {
                            decimal overspendPercent = monthlyBudget > 0 ? (projectedActual / monthlyBudget) * 100 : 100;
                            severity = overspendPercent > 120 ? "Critical" : "Warning";
                            message = "Over budget by " + Math.Abs(remainingBudget).ToString("N2") + ". " + message;
                        }

                        return new BudgetCheckResult
                        {
                            IsOverBudget = isOver,
                            RemainingBudget = remainingBudget,
                            MonthlyBudget = monthlyBudget,
                            CurrentActual = currentActual,
                            Message = message,
                            SeverityLevel = severity
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Approves a budget (updates status and approval info)
        /// </summary>
        public void ApproveBudget(int budgetId, int approvedBy)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        UPDATE acc_budget_headers 
                        SET 
                            status = 'Approved',
                            approved_by = @approved_by,
                            approved_at = GETDATE()
                        WHERE budget_id = @budget_id";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@budget_id", budgetId);
                    cmd.Parameters.AddWithValue("@approved_by", approvedBy);

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Activates a budget (sets status to Active and deactivates others for same fiscal year)
        /// </summary>
        public void ActivateBudget(int budgetId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    using (SqlTransaction trans = cn.BeginTransaction())
                    {
                        try
                        {
                            // Get fiscal year for this budget
                            cmd = new SqlCommand("SELECT financial_year_id, branch_id FROM acc_budget_headers WHERE budget_id = @budget_id", cn, trans);
                            cmd.Parameters.AddWithValue("@budget_id", budgetId);
                            SqlDataReader reader = cmd.ExecuteReader();

                            int fiscalYearId = 0;
                            object branchIdObj = null;

                            if (reader.Read())
                            {
                                fiscalYearId = Convert.ToInt32(reader["financial_year_id"]);
                                branchIdObj = reader["branch_id"];
                            }
                            reader.Close();

                            if (fiscalYearId == 0)
                                throw new Exception("Budget not found");

                            // Deactivate all other budgets for same fiscal year and cost center
                            string deactivateQuery;
                            if (branchIdObj == DBNull.Value || branchIdObj == null)
                            {
                                deactivateQuery = @"
                                    UPDATE acc_budget_headers 
                                    SET status = 'Approved' 
                                    WHERE financial_year_id = @fiscal_year_id 
                                      AND branch_id IS NULL 
                                      AND budget_id <> @budget_id 
                                      AND status = 'Active'";
                            }
                            else
                            {
                                deactivateQuery = @"
                                    UPDATE acc_budget_headers 
                                    SET status = 'Approved' 
                                    WHERE financial_year_id = @fiscal_year_id 
                                      AND branch_id = @branch_id 
                                      AND budget_id <> @budget_id 
                                      AND status = 'Active'";
                            }

                            cmd = new SqlCommand(deactivateQuery, cn, trans);
                            cmd.Parameters.AddWithValue("@fiscal_year_id", fiscalYearId);
                            cmd.Parameters.AddWithValue("@budget_id", budgetId);
                            if (branchIdObj != DBNull.Value && branchIdObj != null)
                                cmd.Parameters.AddWithValue("@branch_id", branchIdObj);
                            cmd.ExecuteNonQuery();

                            // Activate this budget
                            cmd = new SqlCommand("UPDATE acc_budget_headers SET status = 'Active' WHERE budget_id = @budget_id", cn, trans);
                            cmd.Parameters.AddWithValue("@budget_id", budgetId);
                            cmd.ExecuteNonQuery();

                            trans.Commit();
                        }
                        catch
                        {
                            trans.Rollback();
                            throw;
                        }
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets the active budget for a specific date
        /// </summary>
        public DataTable GetActiveBudgetForPeriod(DateTime date, int? branchId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        SELECT TOP 1
                            bh.*
                        FROM acc_budget_headers bh
                        INNER JOIN acc_fiscal_years fy ON bh.financial_year_id = fy.id
                        WHERE bh.status = 'Active'
                          AND @date BETWEEN fy.from_date AND fy.to_date
                          AND (@branch_id IS NULL OR bh.branch_id = @branch_id OR bh.branch_id IS NULL)
                        ORDER BY 
                            CASE WHEN bh.branch_id = @branch_id THEN 0 ELSE 1 END,
                            bh.created_at DESC";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@branch_id ", (object)branchId ?? DBNull.Value);

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes sp_BudgetVsActual stored procedure (legacy signature)
        /// </summary>
        public DataTable GetBudgetVsActual(int budgetId, DateTime fromDate, DateTime toDate, int? ccId = null)
        {
            return GetBudgetVsActual(budgetId, fromDate, toDate, ccId, "YTD", "All");
        }

        /// <summary>
        /// Executes sp_BudgetVsActual stored procedure with report filters
        /// </summary>
        public DataTable GetBudgetVsActual(int budgetId, DateTime fromDate, DateTime toDate, int? branchId, string periodMode, string accountTypeFilter)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    cmd = new SqlCommand("sp_BudgetVsActual", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BudgetId", budgetId);
                    cmd.Parameters.AddWithValue("@FromDate", fromDate);
                    cmd.Parameters.AddWithValue("@ToDate", toDate);
                    cmd.Parameters.AddWithValue("@CCId", (object)branchId ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@PeriodMode", string.IsNullOrWhiteSpace(periodMode) ? "YTD" : periodMode);
                    cmd.Parameters.AddWithValue("@AccountTypeFilter", string.IsNullOrWhiteSpace(accountTypeFilter) ? "All" : accountTypeFilter);

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets monthly budget vs actual detail for a specific account.
        /// </summary>
        public DataTable GetBudgetMonthlyDetail(int budgetId, int accId, int? branchId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    const string sql = @"
                    DECLARE @FiscalYearStart DATE;
                    DECLARE @FiscalYearEnd DATE;

                    SELECT
                        @FiscalYearStart = fy.from_date,
                        @FiscalYearEnd = fy.to_date
                    FROM acc_budget_headers bh
                    INNER JOIN acc_fiscal_years fy ON bh.financial_year_id = fy.id
                    WHERE bh.budget_id = @BudgetId;

                    IF @FiscalYearStart IS NULL OR @FiscalYearEnd IS NULL
                    BEGIN
                        RAISERROR('Invalid budget/fiscal year mapping.', 16, 1);
                        RETURN;
                    END

                    ;WITH BudgetLine AS
                    (
                        SELECT TOP 1 jan, feb, mar, apr, may, jun, jul, aug, sep, oct, nov, [dec]
                        FROM acc_budget_lines
                        WHERE budget_id = @BudgetId AND account_id = @AccId
                    ),
                    MonthlyBudget AS
                    (
                        SELECT 1 AS MonthNo, ISNULL(jan, 0) AS BudgetAmount FROM BudgetLine UNION ALL
                        SELECT 2, ISNULL(feb, 0) FROM BudgetLine UNION ALL
                        SELECT 3, ISNULL(mar, 0) FROM BudgetLine UNION ALL
                        SELECT 4, ISNULL(apr, 0) FROM BudgetLine UNION ALL
                        SELECT 5, ISNULL(may, 0) FROM BudgetLine UNION ALL
                        SELECT 6, ISNULL(jun, 0) FROM BudgetLine UNION ALL
                        SELECT 7, ISNULL(jul, 0) FROM BudgetLine UNION ALL
                        SELECT 8, ISNULL(aug, 0) FROM BudgetLine UNION ALL
                        SELECT 9, ISNULL(sep, 0) FROM BudgetLine UNION ALL
                        SELECT 10, ISNULL(oct, 0) FROM BudgetLine UNION ALL
                        SELECT 11, ISNULL(nov, 0) FROM BudgetLine UNION ALL
                        SELECT 12, ISNULL([dec], 0) FROM BudgetLine
                    ),
                    MonthlyActual AS
                    (
                        SELECT
                            MONTH(ae.entry_date) AS MonthNo,
                            SUM(ISNULL(ae.debit, 0) - ISNULL(ae.credit, 0)) AS ActualAmount
                        FROM acc_entries ae
                        INNER JOIN acc_entries_header aeh
                            ON ae.invoice_no = aeh.InvoiceNo
                           AND ae.branch_id = aeh.branch_id
                        WHERE ae.account_id = @AccId
                          AND ae.entry_date >= @FiscalYearStart
                          AND ae.entry_date <= @FiscalYearEnd
                          AND UPPER(LTRIM(RTRIM(ISNULL(aeh.status, '')))) = 'POSTED'
                          AND (@CCId IS NULL OR ae.branch_id = @CCId)
                        GROUP BY MONTH(ae.entry_date)
                    )
                    SELECT
                        mb.MonthNo,
                        DATENAME(MONTH, DATEFROMPARTS(2000, mb.MonthNo, 1)) AS MonthName,
                        mb.BudgetAmount,
                        ISNULL(ma.ActualAmount, 0) AS ActualAmount,
                        ISNULL(ma.ActualAmount, 0) - mb.BudgetAmount AS Variance,
                        SUM(mb.BudgetAmount) OVER (ORDER BY mb.MonthNo) AS CumulativeBudget,
                        SUM(ISNULL(ma.ActualAmount, 0)) OVER (ORDER BY mb.MonthNo) AS CumulativeActual
                    FROM MonthlyBudget mb
                    LEFT JOIN MonthlyActual ma ON ma.MonthNo = mb.MonthNo
                    ORDER BY mb.MonthNo;";

                    cmd = new SqlCommand(sql, cn);
                    cmd.Parameters.AddWithValue("@BudgetId", budgetId);
                    cmd.Parameters.AddWithValue("@AccId", accId);
                    cmd.Parameters.AddWithValue("@CCId", (object)branchId ?? DBNull.Value);

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes sp_CopyBudgetFromActuals stored procedure
        /// </summary>
        public DataTable CopyBudgetFromActuals(int sourceYearId, int targetBudgetId, decimal growthPct)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    cmd = new SqlCommand("sp_CopyBudgetFromActuals", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandTimeout = 120; // Extended timeout for data copy
                    cmd.Parameters.AddWithValue("@SourceYearId", sourceYearId);
                    cmd.Parameters.AddWithValue("@TargetBudgetId", targetBudgetId);
                    cmd.Parameters.AddWithValue("@GrowthPct", growthPct);

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes sp_BudgetSeasonalSpread stored procedure
        /// </summary>
        public DataTable ApplySeasonalSpread(int budgetId, int accId, decimal annualAmount, List<MonthlyPercentageModal> percentages)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    // Create table-valued parameter
                    DataTable tvp = new DataTable();
                    tvp.Columns.Add("MonthNo", typeof(int));
                    tvp.Columns.Add("Percentage", typeof(decimal));

                    foreach (var pct in percentages)
                    {
                        tvp.Rows.Add(pct.MonthNo, pct.Percentage);
                    }

                    cmd = new SqlCommand("sp_BudgetSeasonalSpread", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BudgetId", budgetId);
                    cmd.Parameters.AddWithValue("@AccId", accId);
                    cmd.Parameters.AddWithValue("@AnnualAmount", annualAmount);

                    SqlParameter tvpParam = cmd.Parameters.AddWithValue("@Percentages", tvp);
                    tvpParam.SqlDbType = SqlDbType.Structured;
                    tvpParam.TypeName = "dbo.MonthlyPercentagesType";

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Executes sp_BudgetSummaryKPIs stored procedure
        /// </summary>
        public DataTable GetBudgetSummaryKPIs(int budgetId, DateTime asOfDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    cmd = new SqlCommand("sp_BudgetSummaryKPIs", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BudgetId", budgetId);
                    cmd.Parameters.AddWithValue("@AsOfDate", asOfDate);

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Gets variance notes for a budget
        /// </summary>
        public DataTable GetVarianceNotes(int budgetId, int? accId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        SELECT 
                            vn.*,
                            vn.account_id AS acc_id,
                            a.code AS acc_code,
                            a.name AS acc_name,
                            u.name AS added_by_name
                        FROM acc_budget_variance_notes vn
                        INNER JOIN acc_accounts a ON vn.account_id = a.id
                        LEFT JOIN pos_users u ON vn.added_by = u.id
                        WHERE vn.budget_id = @budget_id
                          AND (@acc_id IS NULL OR vn.account_id = @acc_id)
                        ORDER BY vn.period_year DESC, vn.period_month DESC, vn.added_at DESC";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@budget_id", budgetId);
                    cmd.Parameters.AddWithValue("@acc_id", (object)accId ?? DBNull.Value);

                    da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Inserts a variance note
        /// </summary>
        public void InsertVarianceNote(BudgetVarianceNoteModal modal)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    string query = @"
                        INSERT INTO acc_budget_variance_notes 
                        (budget_id, account_id, period_month, period_year, variance_note, added_by, added_at)
                        VALUES 
                        (@budget_id, @account_id, @period_month, @period_year, @variance_note, @added_by, @added_at)";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@budget_id", modal.budget_id);
                    cmd.Parameters.AddWithValue("@account_id", modal.account_id);
                    cmd.Parameters.AddWithValue("@period_month", modal.period_month);
                    cmd.Parameters.AddWithValue("@period_year", modal.period_year);
                    cmd.Parameters.AddWithValue("@variance_note", modal.variance_note ?? string.Empty);
                    cmd.Parameters.AddWithValue("@added_by", modal.added_by);
                    cmd.Parameters.AddWithValue("@added_at", modal.added_at);

                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    throw;
                }
            }
        }
    }
}
