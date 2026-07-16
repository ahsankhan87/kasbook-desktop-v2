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
                            cmd = new SqlCommand("SELECT financial_year_id, cc_id FROM acc_budget_headers WHERE budget_id = @budget_id", cn, trans);
                            cmd.Parameters.AddWithValue("@budget_id", budgetId);
                            SqlDataReader reader = cmd.ExecuteReader();

                            int fiscalYearId = 0;
                            object ccIdObj = null;

                            if (reader.Read())
                            {
                                fiscalYearId = Convert.ToInt32(reader["financial_year_id"]);
                                ccIdObj = reader["cc_id"];
                            }
                            reader.Close();

                            if (fiscalYearId == 0)
                                throw new Exception("Budget not found");

                            // Deactivate all other budgets for same fiscal year and cost center
                            string deactivateQuery;
                            if (ccIdObj == DBNull.Value || ccIdObj == null)
                            {
                                deactivateQuery = @"
                                    UPDATE acc_budget_headers 
                                    SET status = 'Approved' 
                                    WHERE financial_year_id = @fiscal_year_id 
                                      AND cc_id IS NULL 
                                      AND budget_id <> @budget_id 
                                      AND status = 'Active'";
                            }
                            else
                            {
                                deactivateQuery = @"
                                    UPDATE acc_budget_headers 
                                    SET status = 'Approved' 
                                    WHERE financial_year_id = @fiscal_year_id 
                                      AND cc_id = @cc_id 
                                      AND budget_id <> @budget_id 
                                      AND status = 'Active'";
                            }

                            cmd = new SqlCommand(deactivateQuery, cn, trans);
                            cmd.Parameters.AddWithValue("@fiscal_year_id", fiscalYearId);
                            cmd.Parameters.AddWithValue("@budget_id", budgetId);
                            if (ccIdObj != DBNull.Value && ccIdObj != null)
                                cmd.Parameters.AddWithValue("@cc_id", ccIdObj);
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
        public DataTable GetActiveBudgetForPeriod(DateTime date, int? ccId = null)
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
                          AND (@cc_id IS NULL OR bh.cc_id = @cc_id OR bh.cc_id IS NULL)
                        ORDER BY 
                            CASE WHEN bh.cc_id = @cc_id THEN 0 ELSE 1 END,
                            bh.created_at DESC";

                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@date", date);
                    cmd.Parameters.AddWithValue("@cc_id", (object)ccId ?? DBNull.Value);

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
        public DataTable GetBudgetVsActual(int budgetId, DateTime fromDate, DateTime toDate, int? ccId, string periodMode, string accountTypeFilter)
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
                    cmd.Parameters.AddWithValue("@CCId", (object)ccId ?? DBNull.Value);
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
        /// Executes sp_BudgetMonthlyDetail stored procedure
        /// </summary>
        public DataTable GetBudgetMonthlyDetail(int budgetId, int accId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                        cn.Open();

                    cmd = new SqlCommand("sp_BudgetMonthlyDetail", cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@BudgetId", budgetId);
                    cmd.Parameters.AddWithValue("@AccId", accId);

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
