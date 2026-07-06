using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace POS.BLL
{
    public class ChartOfAccountsBLL
    {
        private const string NormalBalanceDebit = "Dr";
        private const string NormalBalanceCredit = "Cr";

        public string GenerateAccountCode(int parentGroupId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                GroupMeta parent = GetGroupMeta(cn, null, parentGroupId);
                if (parent == null)
                {
                    throw new InvalidOperationException("Parent group not found.");
                }

                CodeRange range = ResolveCodeRange(cn, null, parent);
                int maxInGroup = GetMaxAccountCodeInGroup(cn, null, parentGroupId);
                int candidate = Math.Max(range.Start + 1, maxInGroup + 1);

                while (candidate <= range.End)
                {
                    string code = candidate.ToString();
                    if (IsCodeUnique(cn, null, code, null, null))
                    {
                        return code;
                    }
                    candidate++;
                }

                throw new InvalidOperationException("No available account code in the selected range.");
            }
        }

        public ChartOperationResult SaveAccountGroup(AccGroupModel model, int userId)
        {
            if (model == null)
            {
                return ChartOperationResult.Fail("Invalid group payload.");
            }

            if (string.IsNullOrWhiteSpace(model.GroupName))
            {
                return ChartOperationResult.Fail("Group name is required.");
            }

            if (string.IsNullOrWhiteSpace(model.GroupCode))
            {
                return ChartOperationResult.Fail("Group code is required.");
            }

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                if (!IsCodeUnique(cn, null, model.GroupCode.Trim(), model.Id == 0 ? (int?)null : model.Id, null))
                {
                    return ChartOperationResult.Fail("Group code already exists.");
                }

                if (model.ParentGroupId > 0)
                {
                    GroupMeta parent = GetGroupMeta(cn, null, model.ParentGroupId);
                    if (parent == null)
                    {
                        return ChartOperationResult.Fail("Parent group does not exist.");
                    }

                    if (parent.Level != 1)
                    {
                        return ChartOperationResult.Fail("Level 2 groups must have a Level 1 parent.");
                    }
                }

                if (model.Id == 0)
                {
                    int newId = InsertGroup(cn, null, model, userId);
                    return ChartOperationResult.Ok("Group saved successfully.", newId);
                }

                UpdateGroup(cn, null, model);
                return ChartOperationResult.Ok("Group updated successfully.", model.Id);
            }
        }

        public ChartOperationResult SaveAccount(AccAccountModel model, int userId)
        {
            if (model == null)
            {
                return ChartOperationResult.Fail("Invalid account payload.");
            }

            if (string.IsNullOrWhiteSpace(model.AccountName))
            {
                return ChartOperationResult.Fail("Account name is required.");
            }

            if (string.IsNullOrWhiteSpace(model.AccountCode))
            {
                return ChartOperationResult.Fail("Account code is required.");
            }

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        GroupMeta parent = GetGroupMeta(cn, tx, model.ParentGroupId);
                        if (parent == null)
                        {
                            tx.Rollback();
                            return ChartOperationResult.Fail("Parent group not found.");
                        }

                        if (parent.Level != 2)
                        {
                            tx.Rollback();
                            return ChartOperationResult.Fail("Account parent must be a Level 2 group.");
                        }

                        if (!IsCodeUnique(cn, tx, model.AccountCode.Trim(), null, model.Id == 0 ? (int?)null : model.Id))
                        {
                            tx.Rollback();
                            return ChartOperationResult.Fail("Account code already exists.");
                        }

                        string accountType = string.IsNullOrWhiteSpace(model.AccountType)
                            ? ResolveAccountTypeName(cn, tx, parent.AccountTypeId)
                            : model.AccountType;

                        string normalBalance = ResolveNormalBalance(accountType);
                        model.NormalBalance = normalBalance;

                        int accountId;
                        if (model.Id == 0)
                        {
                            accountId = InsertAccount(cn, tx, model, normalBalance, userId);
                        }
                        else
                        {
                            UpdateAccount(cn, tx, model, normalBalance);
                            accountId = model.Id;
                        }

                        if (model.OpeningBalance > 0)
                        {
                            EnsureOpeningBalancePosted(cn, tx, model, accountId, normalBalance, userId);
                        }

                        tx.Commit();
                        return ChartOperationResult.Ok("Account saved successfully.", accountId);
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        return ChartOperationResult.Fail(ex.Message);
                    }
                }
            }
        }

        public ChartOperationResult DeleteAccount(int accId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                int txCount = GetEntryCountForAccount(cn, null, accId);
                if (txCount > 0)
                {
                    return ChartOperationResult.Fail(string.Format("Cannot delete — {0} transactions exist. Deactivate instead.", txCount));
                }

                AccountsBLL accountsBLL = new AccountsBLL();
                int result = accountsBLL.Delete(accId);
                if (result > 0)
                {
                    return ChartOperationResult.Ok("Account deleted successfully.", accId);
                }

                return ChartOperationResult.Fail("Account delete failed.");
            }
        }

        public ChartOperationResult MergeAccounts(int sourceAccId, int targetAccId, int userId)
        {
            if (sourceAccId == targetAccId)
            {
                return ChartOperationResult.Fail("Source and target accounts must be different.");
            }

            if (!IsAdminUser())
            {
                return ChartOperationResult.Fail("Admin permission is required to merge accounts.");
            }

            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        string sourceName = GetAccountName(cn, tx, sourceAccId);
                        string targetName = GetAccountName(cn, tx, targetAccId);

                        if (string.IsNullOrWhiteSpace(sourceName) || string.IsNullOrWhiteSpace(targetName))
                        {
                            tx.Rollback();
                            return ChartOperationResult.Fail("Source or target account does not exist.");
                        }

                        SqlCommand moveCmd = new SqlCommand(
                            "UPDATE acc_entries SET account_id=@target, account_name=@target_name WHERE account_id=@source", cn, tx);
                        moveCmd.Parameters.AddWithValue("@target", targetAccId);
                        moveCmd.Parameters.AddWithValue("@target_name", targetName);
                        moveCmd.Parameters.AddWithValue("@source", sourceAccId);
                        int movedCount = moveCmd.ExecuteNonQuery();

                        SoftDeleteAccount(cn, tx, sourceAccId, sourceName);
                        InsertMergeAudit(cn, tx, sourceAccId, targetAccId, userId, movedCount);

                        tx.Commit();
                        return ChartOperationResult.Ok("Accounts merged successfully.", targetAccId);
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        return ChartOperationResult.Fail(ex.Message);
                    }
                }
            }
        }

        public DataTable GetAccountsByType(string accountType)
        {
            string normalized = (accountType ?? string.Empty).Trim().ToLowerInvariant();
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();

                string query;
                SqlCommand cmd;

                if (normalized == "bank" || normalized == "bank accounts")
                {
                    query = @"SELECT A.id,A.code,A.name,A.name_2,A.group_id
                              FROM acc_accounts A
                              WHERE A.branch_id=@branch_id";
                    if (ColumnExists(cn, null, "acc_accounts", "is_bank_account"))
                    {
                        query += " AND A.is_bank_account = 1";
                    }
                    cmd = new SqlCommand(query + " ORDER BY A.code,A.name", cn);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                }
                else
                {
                    query = @"SELECT A.id,A.code,A.name,A.name_2,A.group_id,T.name AS account_type
                              FROM acc_accounts A
                              INNER JOIN acc_groups G ON A.group_id = G.id
                              INNER JOIN acc_account_type T ON G.account_type_id = T.id
                              WHERE A.branch_id=@branch_id AND LOWER(T.name) LIKE @type
                              ORDER BY A.code,A.name";
                    cmd = new SqlCommand(query, cn);
                    cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
                    cmd.Parameters.AddWithValue("@type", "%" + normalized + "%");
                }

                DataTable dt = new DataTable();
                using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                {
                    da.Fill(dt);
                }
                return dt;
            }
        }

        public ChartOperationResult SetupStandardCOA(int companyId)
        {
            using (SqlConnection cn = new SqlConnection(dbConnection.ConnectionString))
            {
                cn.Open();
                using (SqlTransaction tx = cn.BeginTransaction())
                {
                    try
                    {
                        if (HasAnyCoaEntries(cn, tx))
                        {
                            tx.Rollback();
                            return ChartOperationResult.Fail("COA already has entries. Setup skipped.");
                        }

                        int assetsType = FindAccountTypeId(cn, tx, "asset");
                        int liabilitiesType = FindAccountTypeId(cn, tx, "liabil");
                        int equityType = FindAccountTypeId(cn, tx, "equity");
                        int incomeType = FindAccountTypeId(cn, tx, "income");
                        int expenseType = FindAccountTypeId(cn, tx, "expense");

                        int assets = EnsureGroup(cn, tx, 0, assetsType, "1000", "Assets", "اثاثے", "Assets - Pakistan standard", UsersModal.logged_in_userid);
                        int liabilities = EnsureGroup(cn, tx, 0, liabilitiesType, "2000", "Liabilities", "واجبات", "Liabilities - Pakistan standard", UsersModal.logged_in_userid);
                        int equity = EnsureGroup(cn, tx, 0, equityType, "3000", "Equity", "ایکویٹی", "Equity - Pakistan standard", UsersModal.logged_in_userid);
                        int income = EnsureGroup(cn, tx, 0, incomeType, "4000", "Income", "آمدنی", "Income - Pakistan standard", UsersModal.logged_in_userid);
                        int expenses = EnsureGroup(cn, tx, 0, expenseType, "5000", "Expenses", "اخراجات", "Expenses - Pakistan standard", UsersModal.logged_in_userid);

                        int currentAssets = EnsureGroup(cn, tx, assets, assetsType, "1100", "Current Assets", "موجودہ اثاثے", "", UsersModal.logged_in_userid);
                        int fixedAssets = EnsureGroup(cn, tx, assets, assetsType, "1200", "Fixed Assets", "مستقل اثاثے", "", UsersModal.logged_in_userid);
                        int currentLiabilities = EnsureGroup(cn, tx, liabilities, liabilitiesType, "2100", "Current Liabilities", "موجودہ واجبات", "", UsersModal.logged_in_userid);
                        int longTermLiabilities = EnsureGroup(cn, tx, liabilities, liabilitiesType, "2200", "Long Term Liabilities", "طویل مدتی واجبات", "", UsersModal.logged_in_userid);
                        int capital = EnsureGroup(cn, tx, equity, equityType, "3100", "Capital & Reserves", "سرمایہ", "", UsersModal.logged_in_userid);
                        int revenue = EnsureGroup(cn, tx, income, incomeType, "4100", "Sales Revenue", "فروخت", "", UsersModal.logged_in_userid);
                        int directExpenses = EnsureGroup(cn, tx, expenses, expenseType, "5100", "Direct Expenses", "براہ راست اخراجات", "", UsersModal.logged_in_userid);
                        int operatingExpenses = EnsureGroup(cn, tx, expenses, expenseType, "5200", "Operating Expenses", "آپریٹنگ اخراجات", "", UsersModal.logged_in_userid);

                        EnsureAccount(cn, tx, currentAssets, "1110", "Cash in Hand", "کیش", "", NormalBalanceDebit);
                        EnsureAccount(cn, tx, currentAssets, "1120", "Bank Account", "بینک اکاؤنٹ", "", NormalBalanceDebit);
                        EnsureAccount(cn, tx, currentAssets, "1130", "Accounts Receivable", "قابل وصول", "", NormalBalanceDebit);
                        EnsureAccount(cn, tx, fixedAssets, "1210", "Furniture & Fixtures", "فرنیچر", "", NormalBalanceDebit);
                        EnsureAccount(cn, tx, fixedAssets, "1220", "Plant & Machinery", "پلانٹ", "", NormalBalanceDebit);

                        EnsureAccount(cn, tx, currentLiabilities, "2110", "Accounts Payable", "قابل ادا", "", NormalBalanceCredit);
                        EnsureAccount(cn, tx, currentLiabilities, "2120", "Sales Tax Payable", "سیلز ٹیکس قابل ادا", "", NormalBalanceCredit);
                        EnsureAccount(cn, tx, longTermLiabilities, "2210", "Bank Loan", "بینک قرض", "", NormalBalanceCredit);

                        EnsureAccount(cn, tx, capital, "3110", "Owner Capital", "مالک کا سرمایہ", "", NormalBalanceCredit);
                        EnsureAccount(cn, tx, capital, "3120", "Retained Earnings", "جمع شدہ منافع", "", NormalBalanceCredit);
                        EnsureAccount(cn, tx, capital, "3130", "Opening Balance Equity", "ابتدائی بیلنس ایکویٹی", "", NormalBalanceCredit);

                        EnsureAccount(cn, tx, revenue, "4110", "Sales", "فروخت", "", NormalBalanceCredit);
                        EnsureAccount(cn, tx, revenue, "4120", "Service Income", "سروس آمدنی", "", NormalBalanceCredit);

                        EnsureAccount(cn, tx, directExpenses, "5110", "Cost of Goods Sold", "سی او جی ایس", "", NormalBalanceDebit);
                        EnsureAccount(cn, tx, operatingExpenses, "5210", "Salaries Expense", "تنخواہیں", "", NormalBalanceDebit);
                        EnsureAccount(cn, tx, operatingExpenses, "5220", "Rent Expense", "کرایہ", "", NormalBalanceDebit);
                        EnsureAccount(cn, tx, operatingExpenses, "5230", "Utilities Expense", "یوٹیلیٹیز", "", NormalBalanceDebit);

                        tx.Commit();
                        return ChartOperationResult.Ok("Standard COA setup completed.");
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        return ChartOperationResult.Fail(ex.Message);
                    }
                }
            }
        }

        private int InsertGroup(SqlConnection cn, SqlTransaction tx, AccGroupModel model, int userId)
        {
            bool hasIsActive = ColumnExists(cn, tx, "acc_groups", "is_active");
            bool hasCompanyId = ColumnExists(cn, tx, "acc_groups", "company_id");
            bool hasBranchId = ColumnExists(cn, tx, "acc_groups", "branch_id");

            string columns = "parent_id,account_type_id,code,name,name_2,description,user_id,date_created";
            string values = "@parent_id,@account_type_id,@code,@name,@name_2,@description,@user_id,@date_created";

            if (hasIsActive)
            {
                columns += ",is_active";
                values += ",@is_active";
            }
            if (hasCompanyId)
            {
                columns += ",company_id";
                values += ",@company_id";
            }
            if (hasBranchId)
            {
                columns += ",branch_id";
                values += ",@branch_id";
            }

            string sql = "INSERT INTO acc_groups (" + columns + ") VALUES (" + values + "); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            SqlCommand cmd = new SqlCommand(sql, cn, tx);
            cmd.Parameters.AddWithValue("@parent_id", model.ParentGroupId);
            cmd.Parameters.AddWithValue("@account_type_id", model.AccountTypeId);
            cmd.Parameters.AddWithValue("@code", model.GroupCode.Trim());
            cmd.Parameters.AddWithValue("@name", model.GroupName.Trim());
            cmd.Parameters.AddWithValue("@name_2", string.IsNullOrWhiteSpace(model.GroupNameAr) ? model.GroupName.Trim() : model.GroupNameAr.Trim());
            cmd.Parameters.AddWithValue("@description", (object)(model.Description ?? string.Empty));
            cmd.Parameters.AddWithValue("@user_id", userId);
            cmd.Parameters.AddWithValue("@date_created", DateTime.Now);

            if (hasIsActive) cmd.Parameters.AddWithValue("@is_active", model.IsActive);
            if (hasCompanyId) cmd.Parameters.AddWithValue("@company_id", model.CompanyId <= 0 ? UsersModal.loggedIncompanyID : model.CompanyId);
            if (hasBranchId) cmd.Parameters.AddWithValue("@branch_id", model.BranchId <= 0 ? UsersModal.logged_in_branch_id : model.BranchId);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void UpdateGroup(SqlConnection cn, SqlTransaction tx, AccGroupModel model)
        {
            bool hasIsActive = ColumnExists(cn, tx, "acc_groups", "is_active");
            string setClause = "parent_id=@parent_id,account_type_id=@account_type_id,code=@code,name=@name,name_2=@name_2,description=@description,date_updated=@date_updated";
            if (hasIsActive)
            {
                setClause += ",is_active=@is_active";
            }

            string sql = "UPDATE acc_groups SET " + setClause + " WHERE id=@id";
            SqlCommand cmd = new SqlCommand(sql, cn, tx);
            cmd.Parameters.AddWithValue("@id", model.Id);
            cmd.Parameters.AddWithValue("@parent_id", model.ParentGroupId);
            cmd.Parameters.AddWithValue("@account_type_id", model.AccountTypeId);
            cmd.Parameters.AddWithValue("@code", model.GroupCode.Trim());
            cmd.Parameters.AddWithValue("@name", model.GroupName.Trim());
            cmd.Parameters.AddWithValue("@name_2", string.IsNullOrWhiteSpace(model.GroupNameAr) ? model.GroupName.Trim() : model.GroupNameAr.Trim());
            cmd.Parameters.AddWithValue("@description", (object)(model.Description ?? string.Empty));
            cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
            if (hasIsActive) cmd.Parameters.AddWithValue("@is_active", model.IsActive);
            cmd.ExecuteNonQuery();
        }

        private int InsertAccount(SqlConnection cn, SqlTransaction tx, AccAccountModel model, string normalBalance, int userId)
        {
            bool hasNormalBalance = ColumnExists(cn, tx, "acc_accounts", "normal_balance");
            bool hasOpeningDate = ColumnExists(cn, tx, "acc_accounts", "opening_balance_date");
            bool hasIsBank = ColumnExists(cn, tx, "acc_accounts", "is_bank_account");
            bool hasIsCash = ColumnExists(cn, tx, "acc_accounts", "is_cash_account");
            bool hasBankName = ColumnExists(cn, tx, "acc_accounts", "bank_name");
            bool hasBranch = ColumnExists(cn, tx, "acc_accounts", "branch");
            bool hasAccountNo = ColumnExists(cn, tx, "acc_accounts", "account_no");
            bool hasIban = ColumnExists(cn, tx, "acc_accounts", "iban");
            bool hasIsActive = ColumnExists(cn, tx, "acc_accounts", "is_active");
            bool hasCompanyId = ColumnExists(cn, tx, "acc_accounts", "company_id");
            bool hasDateCreated = ColumnExists(cn, tx, "acc_accounts", "date_created");

            decimal dr = normalBalance == NormalBalanceDebit ? model.OpeningBalance : 0M;
            decimal cr = normalBalance == NormalBalanceCredit ? model.OpeningBalance : 0M;

            string columns = "group_id,code,name,name_2,description,op_dr_balance,op_cr_balance,branch_id,user_id";
            string values = "@group_id,@code,@name,@name_2,@description,@op_dr_balance,@op_cr_balance,@branch_id,@user_id";

            if (hasNormalBalance) { columns += ",normal_balance"; values += ",@normal_balance"; }
            if (hasOpeningDate) { columns += ",opening_balance_date"; values += ",@opening_balance_date"; }
            if (hasIsBank) { columns += ",is_bank_account"; values += ",@is_bank_account"; }
            if (hasIsCash) { columns += ",is_cash_account"; values += ",@is_cash_account"; }
            if (hasBankName) { columns += ",bank_name"; values += ",@bank_name"; }
            if (hasBranch) { columns += ",branch"; values += ",@branch"; }
            if (hasAccountNo) { columns += ",account_no"; values += ",@account_no"; }
            if (hasIban) { columns += ",iban"; values += ",@iban"; }
            if (hasIsActive) { columns += ",is_active"; values += ",@is_active"; }
            if (hasCompanyId) { columns += ",company_id"; values += ",@company_id"; }
            if (hasDateCreated) { columns += ",date_created"; values += ",@date_created"; }

            string sql = "INSERT INTO acc_accounts (" + columns + ") VALUES (" + values + "); SELECT CAST(SCOPE_IDENTITY() AS INT);";
            SqlCommand cmd = new SqlCommand(sql, cn, tx);
            cmd.Parameters.AddWithValue("@group_id", model.ParentGroupId);
            cmd.Parameters.AddWithValue("@code", model.AccountCode.Trim());
            cmd.Parameters.AddWithValue("@name", model.AccountName.Trim());
            cmd.Parameters.AddWithValue("@name_2", string.IsNullOrWhiteSpace(model.AccountNameAr) ? model.AccountName.Trim() : model.AccountNameAr.Trim());
            cmd.Parameters.AddWithValue("@description", (object)(model.Description ?? string.Empty));
            cmd.Parameters.AddWithValue("@op_dr_balance", dr);
            cmd.Parameters.AddWithValue("@op_cr_balance", cr);
            cmd.Parameters.AddWithValue("@branch_id", model.BranchId <= 0 ? UsersModal.logged_in_branch_id : model.BranchId);
            cmd.Parameters.AddWithValue("@user_id", userId);

            if (hasNormalBalance) cmd.Parameters.AddWithValue("@normal_balance", normalBalance);
            if (hasOpeningDate) cmd.Parameters.AddWithValue("@opening_balance_date", model.OpeningBalanceDate);
            if (hasIsBank) cmd.Parameters.AddWithValue("@is_bank_account", model.IsBankAccount);
            if (hasIsCash) cmd.Parameters.AddWithValue("@is_cash_account", model.IsCashAccount);
            if (hasBankName) cmd.Parameters.AddWithValue("@bank_name", (object)(model.BankName ?? string.Empty));
            if (hasBranch) cmd.Parameters.AddWithValue("@branch", (object)(model.BankBranch ?? string.Empty));
            if (hasAccountNo) cmd.Parameters.AddWithValue("@account_no", (object)(model.AccountNo ?? string.Empty));
            if (hasIban) cmd.Parameters.AddWithValue("@iban", (object)(model.Iban ?? string.Empty));
            if (hasIsActive) cmd.Parameters.AddWithValue("@is_active", model.IsActive);
            if (hasCompanyId) cmd.Parameters.AddWithValue("@company_id", model.CompanyId <= 0 ? UsersModal.loggedIncompanyID : model.CompanyId);
            if (hasDateCreated) cmd.Parameters.AddWithValue("@date_created", DateTime.Now);

            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private void UpdateAccount(SqlConnection cn, SqlTransaction tx, AccAccountModel model, string normalBalance)
        {
            bool hasNormalBalance = ColumnExists(cn, tx, "acc_accounts", "normal_balance");
            bool hasOpeningDate = ColumnExists(cn, tx, "acc_accounts", "opening_balance_date");
            bool hasIsBank = ColumnExists(cn, tx, "acc_accounts", "is_bank_account");
            bool hasIsCash = ColumnExists(cn, tx, "acc_accounts", "is_cash_account");
            bool hasBankName = ColumnExists(cn, tx, "acc_accounts", "bank_name");
            bool hasBranch = ColumnExists(cn, tx, "acc_accounts", "branch");
            bool hasAccountNo = ColumnExists(cn, tx, "acc_accounts", "account_no");
            bool hasIban = ColumnExists(cn, tx, "acc_accounts", "iban");
            bool hasIsActive = ColumnExists(cn, tx, "acc_accounts", "is_active");
            bool hasDateUpdated = ColumnExists(cn, tx, "acc_accounts", "date_updated");

            decimal dr = normalBalance == NormalBalanceDebit ? model.OpeningBalance : 0M;
            decimal cr = normalBalance == NormalBalanceCredit ? model.OpeningBalance : 0M;

            string setClause = "group_id=@group_id,code=@code,name=@name,name_2=@name_2,description=@description,op_dr_balance=@op_dr_balance,op_cr_balance=@op_cr_balance";
            if (hasNormalBalance) setClause += ",normal_balance=@normal_balance";
            if (hasOpeningDate) setClause += ",opening_balance_date=@opening_balance_date";
            if (hasIsBank) setClause += ",is_bank_account=@is_bank_account";
            if (hasIsCash) setClause += ",is_cash_account=@is_cash_account";
            if (hasBankName) setClause += ",bank_name=@bank_name";
            if (hasBranch) setClause += ",branch=@branch";
            if (hasAccountNo) setClause += ",account_no=@account_no";
            if (hasIban) setClause += ",iban=@iban";
            if (hasIsActive) setClause += ",is_active=@is_active";
            if (hasDateUpdated) setClause += ",date_updated=@date_updated";

            SqlCommand cmd = new SqlCommand("UPDATE acc_accounts SET " + setClause + " WHERE id=@id", cn, tx);
            cmd.Parameters.AddWithValue("@id", model.Id);
            cmd.Parameters.AddWithValue("@group_id", model.ParentGroupId);
            cmd.Parameters.AddWithValue("@code", model.AccountCode.Trim());
            cmd.Parameters.AddWithValue("@name", model.AccountName.Trim());
            cmd.Parameters.AddWithValue("@name_2", string.IsNullOrWhiteSpace(model.AccountNameAr) ? model.AccountName.Trim() : model.AccountNameAr.Trim());
            cmd.Parameters.AddWithValue("@description", (object)(model.Description ?? string.Empty));
            cmd.Parameters.AddWithValue("@op_dr_balance", dr);
            cmd.Parameters.AddWithValue("@op_cr_balance", cr);

            if (hasNormalBalance) cmd.Parameters.AddWithValue("@normal_balance", normalBalance);
            if (hasOpeningDate) cmd.Parameters.AddWithValue("@opening_balance_date", model.OpeningBalanceDate);
            if (hasIsBank) cmd.Parameters.AddWithValue("@is_bank_account", model.IsBankAccount);
            if (hasIsCash) cmd.Parameters.AddWithValue("@is_cash_account", model.IsCashAccount);
            if (hasBankName) cmd.Parameters.AddWithValue("@bank_name", (object)(model.BankName ?? string.Empty));
            if (hasBranch) cmd.Parameters.AddWithValue("@branch", (object)(model.BankBranch ?? string.Empty));
            if (hasAccountNo) cmd.Parameters.AddWithValue("@account_no", (object)(model.AccountNo ?? string.Empty));
            if (hasIban) cmd.Parameters.AddWithValue("@iban", (object)(model.Iban ?? string.Empty));
            if (hasIsActive) cmd.Parameters.AddWithValue("@is_active", model.IsActive);
            if (hasDateUpdated) cmd.Parameters.AddWithValue("@date_updated", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        private void EnsureOpeningBalancePosted(SqlConnection cn, SqlTransaction tx, AccAccountModel model, int accountId, string normalBalance, int userId)
        {
            SqlCommand existing = new SqlCommand(
                "SELECT COUNT(1) FROM acc_entries WHERE account_id=@account_id AND description LIKE 'Opening Balance%';", cn, tx);
            existing.Parameters.AddWithValue("@account_id", accountId);
            int exists = Convert.ToInt32(existing.ExecuteScalar());
            if (exists > 0)
            {
                return;
            }

            int offsetAccountId = EnsureOpeningBalanceEquityAccount(cn, tx, userId);
            string accountName = GetAccountName(cn, tx, accountId);
            string offsetName = GetAccountName(cn, tx, offsetAccountId);

            string invoiceNo = "OB-" + DateTime.Now.ToString("yyyyMMddHHmmss");
            decimal amount = model.OpeningBalance;

            decimal debitMain = normalBalance == NormalBalanceDebit ? amount : 0M;
            decimal creditMain = normalBalance == NormalBalanceCredit ? amount : 0M;
            decimal debitOffset = creditMain;
            decimal creditOffset = debitMain;

            InsertJournalEntry(cn, tx, accountId, accountName, invoiceNo, model.OpeningBalanceDate, debitMain, creditMain, "Opening Balance", userId);
            InsertJournalEntry(cn, tx, offsetAccountId, offsetName, invoiceNo, model.OpeningBalanceDate, debitOffset, creditOffset, "Opening Balance Offset", userId);
        }

        private void InsertJournalEntry(SqlConnection cn, SqlTransaction tx, int accountId, string accountName, string invoiceNo, DateTime entryDate, decimal debit, decimal credit, string description, int userId)
        {
            bool hasUserId = ColumnExists(cn, tx, "acc_entries", "user_id");
            bool hasBranchId = ColumnExists(cn, tx, "acc_entries", "branch_id");
            bool hasDateCreated = ColumnExists(cn, tx, "acc_entries", "date_created");

            string columns = "account_id,account_name,invoice_no,entry_date,debit,credit,description";
            string values = "@account_id,@account_name,@invoice_no,@entry_date,@debit,@credit,@description";

            if (hasUserId) { columns += ",user_id"; values += ",@user_id"; }
            if (hasBranchId) { columns += ",branch_id"; values += ",@branch_id"; }
            if (hasDateCreated) { columns += ",date_created"; values += ",@date_created"; }

            SqlCommand cmd = new SqlCommand("INSERT INTO acc_entries (" + columns + ") VALUES (" + values + ")", cn, tx);
            cmd.Parameters.AddWithValue("@account_id", accountId);
            cmd.Parameters.AddWithValue("@account_name", accountName ?? string.Empty);
            cmd.Parameters.AddWithValue("@invoice_no", invoiceNo);
            cmd.Parameters.AddWithValue("@entry_date", entryDate);
            cmd.Parameters.AddWithValue("@debit", debit);
            cmd.Parameters.AddWithValue("@credit", credit);
            cmd.Parameters.AddWithValue("@description", description);
            if (hasUserId) cmd.Parameters.AddWithValue("@user_id", userId);
            if (hasBranchId) cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
            if (hasDateCreated) cmd.Parameters.AddWithValue("@date_created", DateTime.Now);
            cmd.ExecuteNonQuery();
        }

        private int EnsureOpeningBalanceEquityAccount(SqlConnection cn, SqlTransaction tx, int userId)
        {
            SqlCommand find = new SqlCommand(
                "SELECT TOP 1 id FROM acc_accounts WHERE branch_id=@branch_id AND (LOWER(name) LIKE '%opening balance equity%' OR LOWER(name) LIKE '%retained earnings%') ORDER BY id", cn, tx);
            find.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
            object found = find.ExecuteScalar();
            if (found != null && found != DBNull.Value)
            {
                return Convert.ToInt32(found);
            }

            int equityType = FindAccountTypeId(cn, tx, "equity");
            int equityGroup = EnsureGroup(cn, tx, 0, equityType, "3000", "Equity", "Equity", "", userId);
            int reservesGroup = EnsureGroup(cn, tx, equityGroup, equityType, "3100", "Capital & Reserves", "Capital & Reserves", "", userId);

            return EnsureAccount(cn, tx, reservesGroup, "3130", "Opening Balance Equity", "Opening Balance Equity", "", NormalBalanceCredit);
        }

        private void SoftDeleteAccount(SqlConnection cn, SqlTransaction tx, int accountId, string sourceName)
        {
            if (ColumnExists(cn, tx, "acc_accounts", "is_active"))
            {
                SqlCommand cmd = new SqlCommand("UPDATE acc_accounts SET is_active=0 WHERE id=@id", cn, tx);
                cmd.Parameters.AddWithValue("@id", accountId);
                cmd.ExecuteNonQuery();
                return;
            }

            SqlCommand fallback = new SqlCommand("UPDATE acc_accounts SET name=@name WHERE id=@id", cn, tx);
            fallback.Parameters.AddWithValue("@id", accountId);
            fallback.Parameters.AddWithValue("@name", (sourceName ?? string.Empty) + " (Merged)");
            fallback.ExecuteNonQuery();
        }

        private void InsertMergeAudit(SqlConnection cn, SqlTransaction tx, int sourceAccId, int targetAccId, int userId, int movedCount)
        {
            if (!TableExists(cn, tx, "acc_audit"))
            {
                return;
            }

            List<string> columns = new List<string>();
            List<string> values = new List<string>();
            SqlCommand cmd = new SqlCommand();
            cmd.Connection = cn;
            cmd.Transaction = tx;

            AddAuditField(cn, tx, cmd, columns, values, "source_account_id", "@source_account_id", sourceAccId);
            AddAuditField(cn, tx, cmd, columns, values, "target_account_id", "@target_account_id", targetAccId);
            AddAuditField(cn, tx, cmd, columns, values, "action", "@action", "MERGE_ACCOUNTS");
            AddAuditField(cn, tx, cmd, columns, values, "description", "@description", string.Format("Merged account {0} into {1}. Entries moved: {2}", sourceAccId, targetAccId, movedCount));
            AddAuditField(cn, tx, cmd, columns, values, "created_by", "@created_by", userId);
            AddAuditField(cn, tx, cmd, columns, values, "user_id", "@user_id", userId);
            AddAuditField(cn, tx, cmd, columns, values, "created_at", "@created_at", DateTime.Now);
            AddAuditField(cn, tx, cmd, columns, values, "date_created", "@date_created", DateTime.Now);

            if (columns.Count == 0)
            {
                return;
            }

            cmd.CommandText = "INSERT INTO acc_audit (" + string.Join(",", columns) + ") VALUES (" + string.Join(",", values) + ")";
            cmd.ExecuteNonQuery();
        }

        private void AddAuditField(SqlConnection cn, SqlTransaction tx, SqlCommand cmd, List<string> columns, List<string> values, string column, string parameter, object value)
        {
            if (!ColumnExists(cn, tx, "acc_audit", column))
            {
                return;
            }

            columns.Add(column);
            values.Add(parameter);
            cmd.Parameters.AddWithValue(parameter, value ?? DBNull.Value);
        }

        private bool HasAnyCoaEntries(SqlConnection cn, SqlTransaction tx)
        {
            SqlCommand cmd = new SqlCommand("SELECT (SELECT COUNT(1) FROM acc_groups) + (SELECT COUNT(1) FROM acc_accounts WHERE branch_id=@branch_id)", cn, tx);
            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private int EnsureGroup(SqlConnection cn, SqlTransaction tx, int parentId, int accountTypeId, string code, string name, string nameAr, string description, int userId)
        {
            SqlCommand find = new SqlCommand("SELECT TOP 1 id FROM acc_groups WHERE code=@code", cn, tx);
            find.Parameters.AddWithValue("@code", code);
            object id = find.ExecuteScalar();
            if (id != null && id != DBNull.Value)
            {
                return Convert.ToInt32(id);
            }

            AccGroupModel model = new AccGroupModel
            {
                ParentGroupId = parentId,
                AccountTypeId = accountTypeId,
                GroupCode = code,
                GroupName = name,
                GroupNameAr = nameAr,
                Description = description,
                IsActive = true,
                CompanyId = UsersModal.loggedIncompanyID,
                BranchId = UsersModal.logged_in_branch_id
            };

            return InsertGroup(cn, tx, model, userId);
        }

        private int EnsureAccount(SqlConnection cn, SqlTransaction tx, int groupId, string code, string name, string nameAr, string description, string normalBalance)
        {
            SqlCommand find = new SqlCommand("SELECT TOP 1 id FROM acc_accounts WHERE code=@code AND branch_id=@branch_id", cn, tx);
            find.Parameters.AddWithValue("@code", code);
            find.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
            object id = find.ExecuteScalar();
            if (id != null && id != DBNull.Value)
            {
                return Convert.ToInt32(id);
            }

            AccAccountModel model = new AccAccountModel
            {
                ParentGroupId = groupId,
                AccountCode = code,
                AccountName = name,
                AccountNameAr = nameAr,
                Description = description,
                NormalBalance = normalBalance,
                OpeningBalance = 0M,
                OpeningBalanceDate = DateTime.Today,
                IsActive = true,
                BranchId = UsersModal.logged_in_branch_id,
                CompanyId = UsersModal.loggedIncompanyID
            };

            return InsertAccount(cn, tx, model, normalBalance, UsersModal.logged_in_userid);
        }

        private GroupMeta GetGroupMeta(SqlConnection cn, SqlTransaction tx, int groupId)
        {
            SqlCommand cmd = new SqlCommand("SELECT id,parent_id,account_type_id,code,name FROM acc_groups WHERE id=@id", cn, tx);
            cmd.Parameters.AddWithValue("@id", groupId);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (!reader.Read())
                {
                    return null;
                }

                GroupMeta meta = new GroupMeta();
                meta.Id = Convert.ToInt32(reader["id"]);
                meta.ParentId = reader["parent_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["parent_id"]);
                meta.AccountTypeId = reader["account_type_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["account_type_id"]);
                meta.Code = Convert.ToString(reader["code"]);
                meta.Name = Convert.ToString(reader["name"]);
                meta.Level = 1;
                return meta;
            }
        }

        private CodeRange ResolveCodeRange(SqlConnection cn, SqlTransaction tx, GroupMeta parent)
        {
            int level = GetGroupLevel(cn, tx, parent.Id);
            parent.Level = level;

            int categoryBase = ResolveCategoryBase(cn, tx, parent);

            int parsed;
            if (int.TryParse(parent.Code, out parsed))
            {
                int baseHundred = (parsed / 100) * 100;
                if (level >= 2)
                {
                    return new CodeRange(baseHundred, baseHundred + 99);
                }

                int level2Start = categoryBase + 100;
                return new CodeRange(level2Start, level2Start + 99);
            }

            return new CodeRange(categoryBase + 100, categoryBase + 199);
        }

        private int GetGroupLevel(SqlConnection cn, SqlTransaction tx, int groupId)
        {
            int level = 1;
            int current = groupId;

            while (current > 0)
            {
                SqlCommand cmd = new SqlCommand("SELECT parent_id FROM acc_groups WHERE id=@id", cn, tx);
                cmd.Parameters.AddWithValue("@id", current);
                object parent = cmd.ExecuteScalar();
                if (parent == null || parent == DBNull.Value)
                {
                    break;
                }

                int parentId = Convert.ToInt32(parent);
                if (parentId <= 0)
                {
                    break;
                }

                level++;
                current = parentId;
                if (level > 10)
                {
                    break;
                }
            }

            return level;
        }

        private int ResolveCategoryBase(SqlConnection cn, SqlTransaction tx, GroupMeta group)
        {
            int rootId = group.Id;
            int parent = group.ParentId;

            while (parent > 0)
            {
                rootId = parent;
                SqlCommand cmd = new SqlCommand("SELECT parent_id FROM acc_groups WHERE id=@id", cn, tx);
                cmd.Parameters.AddWithValue("@id", parent);
                object p = cmd.ExecuteScalar();
                parent = (p == null || p == DBNull.Value) ? 0 : Convert.ToInt32(p);
            }

            int accountTypeId = 0;
            string rootName = string.Empty;

            SqlCommand rootCmd = new SqlCommand("SELECT account_type_id,name FROM acc_groups WHERE id=@id", cn, tx);
            rootCmd.Parameters.AddWithValue("@id", rootId);
            using (SqlDataReader reader = rootCmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    accountTypeId = reader["account_type_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["account_type_id"]);
                    rootName = Convert.ToString(reader["name"]);
                }
            }

            string typeName = ResolveAccountTypeName(cn, tx, accountTypeId);
            string normalized = (typeName + " " + rootName).ToLowerInvariant();
            if (normalized.Contains("asset")) return 1000;
            if (normalized.Contains("liabil")) return 2000;
            if (normalized.Contains("equity")) return 3000;
            if (normalized.Contains("income") || normalized.Contains("revenue")) return 4000;
            if (normalized.Contains("expense")) return 5000;

            return 1000;
        }

        private int GetMaxAccountCodeInGroup(SqlConnection cn, SqlTransaction tx, int groupId)
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT MAX(TRY_CONVERT(int, code)) FROM acc_accounts WHERE group_id=@group_id AND branch_id=@branch_id", cn, tx);
            cmd.Parameters.AddWithValue("@group_id", groupId);
            cmd.Parameters.AddWithValue("@branch_id", UsersModal.logged_in_branch_id);
            object value = cmd.ExecuteScalar();
            if (value == null || value == DBNull.Value)
            {
                return 0;
            }
            return Convert.ToInt32(value);
        }

        private bool IsCodeUnique(SqlConnection cn, SqlTransaction tx, string code, int? excludeGroupId, int? excludeAccountId)
        {
            SqlCommand cmdGroup = new SqlCommand("SELECT COUNT(1) FROM acc_groups WHERE code=@code" + (excludeGroupId.HasValue ? " AND id<>@id" : string.Empty), cn, tx);
            cmdGroup.Parameters.AddWithValue("@code", code);
            if (excludeGroupId.HasValue) cmdGroup.Parameters.AddWithValue("@id", excludeGroupId.Value);
            int groupCount = Convert.ToInt32(cmdGroup.ExecuteScalar());
            if (groupCount > 0) return false;

            SqlCommand cmdAcc = new SqlCommand("SELECT COUNT(1) FROM acc_accounts WHERE code=@code" + (excludeAccountId.HasValue ? " AND id<>@id" : string.Empty), cn, tx);
            cmdAcc.Parameters.AddWithValue("@code", code);
            if (excludeAccountId.HasValue) cmdAcc.Parameters.AddWithValue("@id", excludeAccountId.Value);
            int accCount = Convert.ToInt32(cmdAcc.ExecuteScalar());
            return accCount == 0;
        }

        private int GetEntryCountForAccount(SqlConnection cn, SqlTransaction tx, int accountId)
        {
            SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM acc_entries WHERE account_id=@account_id", cn, tx);
            cmd.Parameters.AddWithValue("@account_id", accountId);
            return Convert.ToInt32(cmd.ExecuteScalar());
        }

        private string ResolveAccountTypeName(SqlConnection cn, SqlTransaction tx, int accountTypeId)
        {
            if (accountTypeId <= 0) return string.Empty;
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 name FROM acc_account_type WHERE id=@id", cn, tx);
            cmd.Parameters.AddWithValue("@id", accountTypeId);
            object value = cmd.ExecuteScalar();
            return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value);
        }

        private string ResolveNormalBalance(string accountType)
        {
            string normalized = (accountType ?? string.Empty).ToLowerInvariant();
            if (normalized.Contains("asset") || normalized.Contains("expense"))
            {
                return NormalBalanceDebit;
            }

            if (normalized.Contains("liabil") || normalized.Contains("equity") || normalized.Contains("income") || normalized.Contains("revenue"))
            {
                return NormalBalanceCredit;
            }

            return NormalBalanceDebit;
        }

        private int FindAccountTypeId(SqlConnection cn, SqlTransaction tx, string keyword)
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 id FROM acc_account_type WHERE LOWER(name) LIKE @keyword ORDER BY id", cn, tx);
            cmd.Parameters.AddWithValue("@keyword", "%" + (keyword ?? string.Empty).ToLowerInvariant() + "%");
            object value = cmd.ExecuteScalar();
            return value == null || value == DBNull.Value ? 0 : Convert.ToInt32(value);
        }

        private string GetAccountName(SqlConnection cn, SqlTransaction tx, int accountId)
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 name FROM acc_accounts WHERE id=@id", cn, tx);
            cmd.Parameters.AddWithValue("@id", accountId);
            object value = cmd.ExecuteScalar();
            return value == null || value == DBNull.Value ? string.Empty : Convert.ToString(value);
        }

        private bool IsAdminUser()
        {
            string role = (UsersModal.logged_in_user_role ?? string.Empty).Trim();
            if (string.IsNullOrWhiteSpace(role))
            {
                return false;
            }

            return role.Equals("Administrator", StringComparison.OrdinalIgnoreCase)
                   || role.Equals("Admin", StringComparison.OrdinalIgnoreCase);
        }

        private bool ColumnExists(SqlConnection cn, SqlTransaction tx, string tableName, string columnName)
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME=@table AND COLUMN_NAME=@column", cn, tx);
            cmd.Parameters.AddWithValue("@table", tableName);
            cmd.Parameters.AddWithValue("@column", columnName);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private bool TableExists(SqlConnection cn, SqlTransaction tx, string tableName)
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT COUNT(1) FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME=@table", cn, tx);
            cmd.Parameters.AddWithValue("@table", tableName);
            return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
        }

        private sealed class GroupMeta
        {
            public int Id;
            public int ParentId;
            public int AccountTypeId;
            public string Code;
            public string Name;
            public int Level;
        }

        private struct CodeRange
        {
            public int Start;
            public int End;

            public CodeRange(int start, int end)
            {
                Start = start;
                End = end;
            }
        }
    }

    public sealed class ChartOperationResult
    {
        public bool Success { get; private set; }
        public string Message { get; private set; }
        public int? EntityId { get; private set; }

        public static ChartOperationResult Ok(string message, int? entityId = null)
        {
            return new ChartOperationResult { Success = true, Message = message, EntityId = entityId };
        }

        public static ChartOperationResult Fail(string message)
        {
            return new ChartOperationResult { Success = false, Message = message };
        }
    }
}
