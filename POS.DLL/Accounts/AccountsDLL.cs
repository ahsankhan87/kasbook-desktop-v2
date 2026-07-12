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
    public class AccountsDLL
    {
        private SqlCommand cmd;
        private SqlDataAdapter da;
        private DataTable dt = new DataTable();
        private AccountsModal info = new AccountsModal();

        public DataTable GetAll()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_AccountsCrud", cn);
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

        public DataTable GetGroupAccountByParent(int parent_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT *" +
                            " FROM acc_groups" +
                            " WHERE parent_id = @parent_id";


                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@parent_id", parent_id);

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

        public DataTable GetAccountByGroup(int group_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT *" +
                            " FROM acc_accounts" +
                            " WHERE group_id = @group_id";


                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@group_id", group_id);

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

        public DataTable AccountReport(DateTime from_date, DateTime to_date, int account_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT id,account_name,invoice_no,debit,credit,(debit-credit) AS balance,description" +
                            " FROM acc_entries" +
                            " WHERE branch_id=@branch_id AND entry_date BETWEEN @from_date AND @to_date";


                        if (account_id != 0)
                        {
                            query += " AND account_id = @account_id";
                        }

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@from_date", from_date);
                        cmd.Parameters.AddWithValue("@to_date", to_date);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        if (account_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@account_id", account_id);
                        }
                       
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

        public DataTable GroupAccountReport(DateTime from_date, DateTime to_date, int group_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT AA.name AS account_name," +
                            " SUM(AE.debit) as debit,SUM(AE.credit) as credit, SUM(AE.debit)-SUM(AE.credit) AS balance" +
                            " FROM acc_entries AS AE" +
                            " LEFT JOIN acc_accounts AS AA  ON AE.account_id = AA.id" +
                            " LEFT JOIN acc_groups AS GP  ON AA.group_id=GP.id" +
                            " WHERE AE.branch_id=@branch_id AND AE.entry_date BETWEEN @from_date AND @to_date";
                            

                            if (group_id != 0)
                            {
                                query += " AND AA.group_id = @group_id";
                            }

                        query += " GROUP BY AA.name";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@from_date", from_date);
                        cmd.Parameters.AddWithValue("@to_date", to_date);
                        cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);

                        if (group_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@group_id", group_id);
                        }

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


        public DataTable GetGroupsByAccountType(int account_type_id = 0)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT *" +
                            " FROM acc_groups";
                            
                            if (account_type_id != 0)
                            {
                            query +=" WHERE account_type_id =  @account_type_id";
                            }

                        cmd = new SqlCommand(query, cn);

                        if (account_type_id != 0)
                        {
                            cmd.Parameters.AddWithValue("@account_type_id", account_type_id);
                        }
                        
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

        public DataTable TrialBalanceReport(DateTime from_date, DateTime to_date)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        String query = "SELECT AA.name AS account_name," +
                            " SUM(AE.debit) as debit,SUM(AE.credit) as credit, SUM(AE.debit)-SUM(AE.credit) AS balance" +
                            " FROM acc_entries AE" +
                            " LEFT JOIN acc_accounts AS AA  ON AE.account_id = AA.id" +
                            " WHERE entry_date BETWEEN @from_date AND @to_date"+
                            " AND AE.branch_id=@branch_id GROUP BY AA.name";

                        cmd = new SqlCommand(query, cn);
                        cmd.Parameters.AddWithValue("@from_date", from_date);
                        cmd.Parameters.AddWithValue("@to_date", to_date);
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

        public DataTable GetProfitLossHierarchy()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    const string query = @";
WITH CandidateGroups AS
(
    SELECT DISTINCT G.id, G.parent_id
    FROM acc_groups G
    INNER JOIN acc_account_type T ON T.id = G.account_type_id
    WHERE LOWER(ISNULL(T.name, '')) LIKE '%income%'
       OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%'
       OR LOWER(ISNULL(T.name, '')) LIKE '%expense%'
       OR LOWER(ISNULL(T.name, '')) LIKE '%cogs%'
       OR LOWER(ISNULL(T.name, '')) LIKE '%cost%'
), RecursiveGroups AS
(
    SELECT id, parent_id FROM CandidateGroups
    UNION ALL
    SELECT P.id, P.parent_id
    FROM acc_groups P
    INNER JOIN RecursiveGroups RG ON RG.parent_id = P.id
), DistinctGroups AS
(
    SELECT DISTINCT id FROM RecursiveGroups
)
SELECT 'Group' AS node_kind,
       G.id AS node_id,
       G.parent_id,
       G.id AS group_id,
       CAST(NULL AS INT) AS account_id,
       ISNULL(G.code, '') AS code,
       ISNULL(G.name, '') AS name,
       ISNULL(T.name, '') AS account_type_name,
       CASE
           WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%' OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%' THEN 'Income'
           WHEN LOWER(ISNULL(T.name, '')) LIKE '%cogs%' OR LOWER(ISNULL(T.name, '')) LIKE '%cost%' OR LOWER(ISNULL(T.name, '')) LIKE '%purchase%' THEN 'COGS'
           WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%' THEN 'Expense'
           ELSE 'Other'
       END AS section
FROM acc_groups G
LEFT JOIN acc_account_type T ON T.id = G.account_type_id
WHERE G.id IN (SELECT id FROM DistinctGroups)
UNION ALL
SELECT 'Account' AS node_kind,
       A.id AS node_id,
       A.group_id AS parent_id,
       A.group_id,
       A.id AS account_id,
       ISNULL(A.code, '') AS code,
       ISNULL(A.name, '') AS name,
       ISNULL(T.name, '') AS account_type_name,
       CASE
           WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%' OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%' THEN 'Income'
           WHEN LOWER(ISNULL(T.name, '')) LIKE '%cogs%' OR LOWER(ISNULL(T.name, '')) LIKE '%cost%' OR LOWER(ISNULL(T.name, '')) LIKE '%purchase%' THEN 'COGS'
           WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%' THEN 'Expense'
           ELSE 'Other'
       END AS section
FROM acc_accounts A
INNER JOIN acc_groups G ON G.id = A.group_id
LEFT JOIN acc_account_type T ON T.id = G.account_type_id
WHERE A.group_id IN (SELECT id FROM DistinctGroups)
  AND ISNULL(A.is_active, 1) = 1
ORDER BY node_kind, code, name;";

                    using (SqlCommand localCmd = new SqlCommand(query, cn))
                    using (SqlDataAdapter adapter = new SqlDataAdapter(localCmd))
                    {
                        DataTable table = new DataTable();
                        adapter.Fill(table);
                        return table;
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetProfitLossBalances(DateTime fromDate, DateTime toDate, DateTime? previousFromDate, DateTime? previousToDate, int? costCenterId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    const string query = @"
SELECT
    E.account_id,
    SUM(CASE WHEN E.entry_date BETWEEN @from_date AND @to_date THEN ISNULL(E.debit, 0) ELSE 0 END) AS current_debit,
    SUM(CASE WHEN E.entry_date BETWEEN @from_date AND @to_date THEN ISNULL(E.credit, 0) ELSE 0 END) AS current_credit,
    SUM(CASE WHEN @prev_from IS NOT NULL AND @prev_to IS NOT NULL AND E.entry_date BETWEEN @prev_from AND @prev_to THEN ISNULL(E.debit, 0) ELSE 0 END) AS previous_debit,
    SUM(CASE WHEN @prev_from IS NOT NULL AND @prev_to IS NOT NULL AND E.entry_date BETWEEN @prev_from AND @prev_to THEN ISNULL(E.credit, 0) ELSE 0 END) AS previous_credit
FROM acc_entries E
WHERE E.branch_id = @branch_id
  AND (
       E.entry_date BETWEEN @from_date AND @to_date
       OR (@prev_from IS NOT NULL AND @prev_to IS NOT NULL AND E.entry_date BETWEEN @prev_from AND @prev_to)
  )
  AND (@cost_center_id IS NULL OR E.cost_center_id = @cost_center_id)
GROUP BY E.account_id;";

                    using (SqlCommand localCmd = new SqlCommand(query, cn))
                    {
                        localCmd.Parameters.AddWithValue("@from_date", fromDate.Date);
                        localCmd.Parameters.AddWithValue("@to_date", toDate.Date);
                        localCmd.Parameters.AddWithValue("@prev_from", (object)previousFromDate ?? DBNull.Value);
                        localCmd.Parameters.AddWithValue("@prev_to", (object)previousToDate ?? DBNull.Value);
                        localCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        localCmd.Parameters.AddWithValue("@cost_center_id", (object)costCenterId ?? DBNull.Value);

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

        public DataTable GetProfitLossCostCenters()
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    const string query = @"
                        SELECT DISTINCT
                            E.cost_center_id AS id,
                            CASE
                                WHEN E.cost_center_id IS NULL OR LTRIM(RTRIM(CONVERT(VARCHAR(50), E.cost_center_id))) = '' THEN 'Unassigned'
                                ELSE 'Department ' + CONVERT(VARCHAR(50), E.cost_center_id)
                            END AS name
                        FROM acc_entries E
                        WHERE E.branch_id = @branch_id
                        ORDER BY name;";

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

        public decimal GetInventoryValueAsOf(DateTime asOfDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    const string query = @"
SELECT ISNULL(SUM(ISNULL(I.qty, 0) * ISNULL(P.avg_cost, 0)), 0)
FROM pos_inventory I
INNER JOIN pos_products P ON P.item_number = I.item_number
WHERE I.branch_id = @branch_id
  AND I.trans_date <= @as_of_date;";

                    using (SqlCommand localCmd = new SqlCommand(query, cn))
                    {
                        localCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        localCmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date);
                        object result = localCmd.ExecuteScalar();
                        return result == DBNull.Value || result == null ? 0m : Convert.ToDecimal(result);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public decimal GetNetProfitBetween(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    const string query = @"
SELECT ISNULL(SUM(
    CASE
        WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%' OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%' THEN ISNULL(E.credit, 0) - ISNULL(E.debit, 0)
        WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%' OR LOWER(ISNULL(T.name, '')) LIKE '%cogs%' OR LOWER(ISNULL(T.name, '')) LIKE '%cost%' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0)
        ELSE 0
    END), 0)
FROM acc_entries E
INNER JOIN acc_accounts A ON A.id = E.account_id
INNER JOIN acc_groups G ON G.id = A.group_id
INNER JOIN acc_account_type T ON T.id = G.account_type_id
WHERE E.branch_id = @branch_id
  AND E.entry_date BETWEEN @from_date AND @to_date;";

                    using (SqlCommand localCmd = new SqlCommand(query, cn))
                    {
                        localCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                        localCmd.Parameters.AddWithValue("@from_date", fromDate.Date);
                        localCmd.Parameters.AddWithValue("@to_date", toDate.Date);
                        object result = localCmd.ExecuteScalar();
                        return result == DBNull.Value || result == null ? 0m : Convert.ToDecimal(result);
                    }
                }
                catch
                {
                    throw;
                }
            }
        }

        public decimal GetBalanceByAccountNamePatternsAsOf(DateTime asOfDate, params string[] namePatterns)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();

                    List<string> predicates = new List<string>();
                    SqlCommand localCmd = new SqlCommand();
                    localCmd.Connection = cn;
                    localCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    localCmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date);

                    if (namePatterns != null)
                    {
                        for (int i = 0; i < namePatterns.Length; i++)
                        {
                            string parameterName = "@p" + i;
                            predicates.Add("(LOWER(ISNULL(A.name, '')) LIKE " + parameterName + " OR LOWER(ISNULL(A.name_2, '')) LIKE " + parameterName + ")");
                            localCmd.Parameters.AddWithValue(parameterName, "%" + (namePatterns[i] ?? string.Empty).Trim().ToLowerInvariant() + "%");
                        }
                    }

                    string predicateSql = predicates.Count == 0 ? "1=1" : string.Join(" OR ", predicates);
                    localCmd.CommandText = @"
SELECT ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0)
FROM acc_entries E
INNER JOIN acc_accounts A ON A.id = E.account_id
WHERE E.branch_id = @branch_id
  AND E.entry_date <= @as_of_date
  AND (" + predicateSql + ")";

                    object result = localCmd.ExecuteScalar();
                    return result == DBNull.Value || result == null ? 0m : Convert.ToDecimal(result);
                }
                catch
                {
                    throw;
                }
            }
        }

        public DataTable GetCashFlowStatement(DateTime fromDate, DateTime toDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();

                    decimal netProfit = QueryProfitAndLossMovement(cn, fromDate, toDate);
                    decimal depreciationAndAmortization = QueryPeriodMovement(cn, fromDate, toDate, false, false, false, "depreciat", "amort");

                    decimal receivablesChange = QueryBalanceChange(cn, fromDate, toDate, true, "receivable", "debtor", "customer");
                    decimal payablesChange = QueryBalanceChange(cn, fromDate, toDate, false, "payable", "creditor", "supplier");
                    decimal inventoryChange = QueryBalanceChange(cn, fromDate, toDate, true, "inventory", "stock");

                    decimal purchaseFixedAssets = QueryPeriodMovement(cn, fromDate, toDate, true, false, true, "fixed asset", "property", "plant", "equipment", "vehicle", "machinery", "furniture");
                    decimal saleFixedAssets = QueryPeriodMovement(cn, fromDate, toDate, false, true, true, "fixed asset", "property", "plant", "equipment", "vehicle", "machinery", "furniture");

                    decimal loanProceeds = QueryPeriodMovement(cn, fromDate, toDate, false, true, false, "loan", "borrowing", "finance");
                    decimal loanRepayments = QueryPeriodMovement(cn, fromDate, toDate, true, false, false, "loan", "borrowing", "finance");
                    decimal ownerDrawingsDividends = QueryPeriodMovement(cn, fromDate, toDate, true, false, false, "drawing", "dividend", "owner draw", "partner draw");

                    decimal openingCashBalance = QueryCashBalanceAsOf(cn, fromDate.AddDays(-1));
                    decimal closingCashBalance = QueryCashBalanceAsOf(cn, toDate);

                    DataTable table = new DataTable();
                    table.Columns.Add("line_key", typeof(string));
                    table.Columns.Add("line_label", typeof(string));
                    table.Columns.Add("amount", typeof(decimal));

                    table.Rows.Add("NET_PROFIT", "Net Profit for period", netProfit);
                    table.Rows.Add("DEP_AMORT", "Depreciation & Amortization", depreciationAndAmortization);
                    table.Rows.Add("CHG_RECEIVABLES", "Increase/Decrease in Receivables", receivablesChange);
                    table.Rows.Add("CHG_PAYABLES", "Increase/Decrease in Payables", payablesChange);
                    table.Rows.Add("CHG_INVENTORY", "Increase/Decrease in Inventory", inventoryChange);
                    table.Rows.Add("PURCHASE_FIXED_ASSETS", "Purchase of Fixed Assets", purchaseFixedAssets);
                    table.Rows.Add("SALE_FIXED_ASSETS", "Sale of Fixed Assets", saleFixedAssets);
                    table.Rows.Add("LOAN_PROCEEDS", "Loan Proceeds", loanProceeds);
                    table.Rows.Add("LOAN_REPAYMENTS", "Loan Repayments", loanRepayments);
                    table.Rows.Add("OWNER_DRAWINGS_DIVIDENDS", "Owner Drawings / Dividends", ownerDrawingsDividends);
                    table.Rows.Add("OPENING_CASH", "Opening Cash Balance", openingCashBalance);
                    table.Rows.Add("CLOSING_CASH", "Closing Cash Balance", closingCashBalance);

                    return table;
                }
                catch
                {
                    throw;
                }
            }
        }

        public decimal GetCashAndBankBalanceAsOf(DateTime asOfDate)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    cn.Open();
                    return QueryCashBalanceAsOf(cn, asOfDate);
                }
                catch
                {
                    throw;
                }
            }
        }

        private static decimal QueryProfitAndLossMovement(SqlConnection cn, DateTime fromDate, DateTime toDate)
        {
            const string query = @"
SELECT ISNULL(SUM(
    CASE
        WHEN LOWER(ISNULL(T.name, '')) LIKE '%income%' OR LOWER(ISNULL(T.name, '')) LIKE '%revenue%' THEN ISNULL(E.credit, 0) - ISNULL(E.debit, 0)
        WHEN LOWER(ISNULL(T.name, '')) LIKE '%expense%' OR LOWER(ISNULL(T.name, '')) LIKE '%cogs%' OR LOWER(ISNULL(T.name, '')) LIKE '%cost%' THEN ISNULL(E.debit, 0) - ISNULL(E.credit, 0)
        ELSE 0
    END), 0)
FROM acc_entries E
INNER JOIN acc_accounts A ON A.id = E.account_id
INNER JOIN acc_groups G ON G.id = A.group_id
INNER JOIN acc_account_type T ON T.id = G.account_type_id
WHERE E.branch_id = @branch_id
  AND E.entry_date BETWEEN @from_date AND @to_date;";

            using (SqlCommand localCmd = new SqlCommand(query, cn))
            {
                localCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                localCmd.Parameters.AddWithValue("@from_date", fromDate.Date);
                localCmd.Parameters.AddWithValue("@to_date", toDate.Date);
                object result = localCmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0m : Convert.ToDecimal(result);
            }
        }

        private static decimal QueryCashBalanceAsOf(SqlConnection cn, DateTime asOfDate)
        {
            const string query = @"
SELECT ISNULL(SUM(ISNULL(E.debit, 0) - ISNULL(E.credit, 0)), 0)
FROM acc_entries E
INNER JOIN acc_accounts A ON A.id = E.account_id
WHERE E.branch_id = @branch_id
  AND E.entry_date <= @as_of_date
  AND (
        LOWER(ISNULL(A.name, '')) LIKE '%cash%'
        OR LOWER(ISNULL(A.name_2, '')) LIKE '%cash%'
        OR LOWER(ISNULL(A.name, '')) LIKE '%bank%'
        OR LOWER(ISNULL(A.name_2, '')) LIKE '%bank%'
      );";

            using (SqlCommand localCmd = new SqlCommand(query, cn))
            {
                localCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                localCmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date);
                object result = localCmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0m : Convert.ToDecimal(result);
            }
        }

        private static decimal QueryBalanceChange(SqlConnection cn, DateTime fromDate, DateTime toDate, bool debitNature, params string[] patterns)
        {
            decimal opening = QueryBalanceByPatternsAsOf(cn, fromDate.AddDays(-1), debitNature, patterns);
            decimal closing = QueryBalanceByPatternsAsOf(cn, toDate, debitNature, patterns);
            return closing - opening;
        }

        private static decimal QueryBalanceByPatternsAsOf(SqlConnection cn, DateTime asOfDate, bool debitNature, params string[] patterns)
        {
            List<string> conditions = new List<string>();
            using (SqlCommand localCmd = new SqlCommand())
            {
                localCmd.Connection = cn;
                localCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                localCmd.Parameters.AddWithValue("@as_of_date", asOfDate.Date);

                for (int i = 0; i < patterns.Length; i++)
                {
                    string parameterName = "@pattern" + i;
                    conditions.Add("(LOWER(ISNULL(A.name, '')) LIKE " + parameterName + " OR LOWER(ISNULL(A.name_2, '')) LIKE " + parameterName + ")");
                    localCmd.Parameters.AddWithValue(parameterName, "%" + patterns[i].Trim().ToLowerInvariant() + "%");
                }

                string conditionSql = conditions.Count == 0 ? "1=1" : string.Join(" OR ", conditions);
                localCmd.CommandText = @"
SELECT ISNULL(SUM(CASE WHEN @debit_nature = 1 THEN ISNULL(E.debit,0) - ISNULL(E.credit,0) ELSE ISNULL(E.credit,0) - ISNULL(E.debit,0) END), 0)
FROM acc_entries E
INNER JOIN acc_accounts A ON A.id = E.account_id
WHERE E.branch_id = @branch_id
  AND E.entry_date <= @as_of_date
  AND (" + conditionSql + ");";
                localCmd.Parameters.AddWithValue("@debit_nature", debitNature ? 1 : 0);

                object result = localCmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0m : Convert.ToDecimal(result);
            }
        }

        private static decimal QueryPeriodMovement(SqlConnection cn, DateTime fromDate, DateTime toDate, bool takeDebit, bool takeCredit, bool debitNature, params string[] patterns)
        {
            List<string> conditions = new List<string>();
            using (SqlCommand localCmd = new SqlCommand())
            {
                localCmd.Connection = cn;
                localCmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                localCmd.Parameters.AddWithValue("@from_date", fromDate.Date);
                localCmd.Parameters.AddWithValue("@to_date", toDate.Date);
                localCmd.Parameters.AddWithValue("@take_debit", takeDebit ? 1 : 0);
                localCmd.Parameters.AddWithValue("@take_credit", takeCredit ? 1 : 0);
                localCmd.Parameters.AddWithValue("@debit_nature", debitNature ? 1 : 0);

                for (int i = 0; i < patterns.Length; i++)
                {
                    string parameterName = "@period_pattern" + i;
                    conditions.Add("(LOWER(ISNULL(A.name, '')) LIKE " + parameterName + " OR LOWER(ISNULL(A.name_2, '')) LIKE " + parameterName + ")");
                    localCmd.Parameters.AddWithValue(parameterName, "%" + patterns[i].Trim().ToLowerInvariant() + "%");
                }

                string conditionSql = conditions.Count == 0 ? "1=1" : string.Join(" OR ", conditions);
                localCmd.CommandText = @"
SELECT ISNULL(SUM(
    CASE
        WHEN @take_debit = 1 AND @take_credit = 1 THEN
            CASE WHEN @debit_nature = 1 THEN ISNULL(E.debit,0) - ISNULL(E.credit,0) ELSE ISNULL(E.credit,0) - ISNULL(E.debit,0) END
        WHEN @take_debit = 1 THEN ISNULL(E.debit,0)
        WHEN @take_credit = 1 THEN ISNULL(E.credit,0)
        ELSE 0
    END), 0)
FROM acc_entries E
INNER JOIN acc_accounts A ON A.id = E.account_id
WHERE E.branch_id = @branch_id
  AND E.entry_date BETWEEN @from_date AND @to_date
  AND (" + conditionSql + ");";

                object result = localCmd.ExecuteScalar();
                return result == null || result == DBNull.Value ? 0m : Convert.ToDecimal(result);
            }
        }

        public DataTable SearchRecordByAccountsID(int Accounts_id)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_AccountsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", Accounts_id);
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

                        cmd = new SqlCommand("SELECT id,code,name,date_created FROM acc_accounts WHERE name LIKE @name", cn);
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

        public int Insert(AccountsModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_AccountsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@branch_id",UsersModal.logged_in_branch_id);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@name_2", obj.name_2);
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@group_id", obj.group_id);
                        cmd.Parameters.AddWithValue("@op_dr_balance", obj.op_dr_balance);
                        cmd.Parameters.AddWithValue("@op_cr_balance", obj.op_cr_balance);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@user_id", UsersModal.logged_in_userid);
                        cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
                        cmd.Parameters.AddWithValue("@OperationType", "1");
                        
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

        public int Update(AccountsModal obj)
        {
            Int32 result = 0; 
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_AccountsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", obj.id);
                        cmd.Parameters.AddWithValue("@code", obj.code);
                        cmd.Parameters.AddWithValue("@group_id", obj.group_id);
                        cmd.Parameters.AddWithValue("@op_dr_balance", obj.op_dr_balance);
                        cmd.Parameters.AddWithValue("@op_cr_balance", obj.op_cr_balance);
                        cmd.Parameters.AddWithValue("@description", obj.description);
                        cmd.Parameters.AddWithValue("@name", obj.name);
                        cmd.Parameters.AddWithValue("@name_2", obj.name_2);
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

        public int Delete(int AccountsId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();

                        cmd = new SqlCommand("sp_AccountsCrud", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@id", AccountsId); 
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

        /// <summary>
        /// Get customer sub-ledger entries with running balance and aging analysis
        /// </summary>
        public DataTable GetCustomerSubLedger(int customerId, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_CustomerSubLedger", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@BranchId", (object)branchId ?? DBNull.Value);
                    }

                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
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
        /// Get customer sub-ledger aging summary
        /// </summary>
        public DataTable GetCustomerSubLedgerAging(int customerId, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_CustomerSubLedger", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CustomerId", customerId);
                        cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@BranchId", (object)branchId ?? DBNull.Value);
                    }

                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    // Return the second result set (aging summary)
                    return dt.Rows.Count > 0 ? dt : new DataTable();
                }
                catch
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Get supplier sub-ledger entries with running balance and aging analysis
        /// </summary>
        public DataTable GetSupplierSubLedger(int supplierId, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_SupplierSubLedger", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                        cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@BranchId", (object)branchId ?? DBNull.Value);
                    }

                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
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
        /// Get supplier sub-ledger aging summary
        /// </summary>
        public DataTable GetSupplierSubLedgerAging(int supplierId, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_SupplierSubLedger", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@SupplierId", supplierId);
                        cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@BranchId", (object)branchId ?? DBNull.Value);
                    }

                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
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
        /// Get cash book entries with running balance
        /// </summary>
        public DataTable GetCashBook(int? cashAccountId = null, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_CashBook", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CashAccountId", (object)cashAccountId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@BranchId", (object)branchId ?? DBNull.Value);
                    }

                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
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
        /// Get cash book daily totals
        /// </summary>
        public DataTable GetCashBookDailyTotals(int? cashAccountId = null, DateTime? fromDate = null, DateTime? toDate = null, int? branchId = null)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                try
                {
                    if (cn.State == ConnectionState.Closed)
                    {
                        cn.Open();
                        cmd = new SqlCommand("sp_CashBook", cn);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@CashAccountId", (object)cashAccountId ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@FromDate", (object)fromDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@ToDate", (object)toDate ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@BranchId", (object)branchId ?? DBNull.Value);
                    }

                    da = new SqlDataAdapter(cmd);
                    dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
                catch
                {
                    throw;
                }
            }
        }

    }
}
