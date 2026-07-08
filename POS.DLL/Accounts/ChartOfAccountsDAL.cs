using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class ChartOfAccountsDAL : AccountingDalBase
    {
        public ChartOfAccountsDAL()
        {
        }

        public ChartOfAccountsDAL(string connectionString)
            : base(connectionString)
        {
        }

        public DataTable GetAllGroups()
        {
            try
            {
                return ExecuteDataTable(@"
SELECT group_id, group_code, group_name, parent_group_id, group_type, [level], sort_order, is_active, created_at
FROM acc_groups
ORDER BY sort_order, group_name;");
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load accounting groups.", ex);
            }
        }

        public List<AccAccountModel> GetAllAccounts()
        {
            try
            {
                List<AccAccountModel> list = new List<AccAccountModel>();
                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand(@"
SELECT acc_id, acc_code, acc_name, parent_group_id, account_type, normal_balance,
       is_bank, is_cash, bank_name, branch, account_no, opening_balance, opening_date,
       is_active, created_at
FROM acc_accounts
ORDER BY acc_code, acc_name;", cn))
                {
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            list.Add(MapAccount(reader));
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load accounting accounts.", ex);
            }
        }

        public List<AccountLookupItem> GetAccountsForDropdown(string accountType = null)
        {
            try
            {
                List<AccountLookupItem> list = new List<AccountLookupItem>();
                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand(@"
SELECT acc_id, acc_code, acc_name, account_type, normal_balance, is_bank, is_cash, is_active
FROM acc_accounts
WHERE (@account_type IS NULL OR @account_type = '' OR account_type = @account_type)
ORDER BY acc_code, acc_name;", cn))
                {
                    cmd.Parameters.AddWithValue("@account_type", NormalizeText(accountType) ?? (object)DBNull.Value);
                    cn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader["acc_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["acc_id"]);
                            string code = Convert.ToString(reader["acc_code"]);
                            string name = Convert.ToString(reader["acc_name"]);
                            string type = Convert.ToString(reader["account_type"]);
                            string normalBalance = Convert.ToString(reader["normal_balance"]);
                            bool isBank = reader["is_bank"] != DBNull.Value && Convert.ToBoolean(reader["is_bank"]);
                            bool isCash = reader["is_cash"] != DBNull.Value && Convert.ToBoolean(reader["is_cash"]);
                            bool isActive = reader["is_active"] == DBNull.Value || Convert.ToBoolean(reader["is_active"]);

                            list.Add(new AccountLookupItem
                            {
                                Id = id,
                                Code = code,
                                Name = name,
                                DisplayText = string.IsNullOrWhiteSpace(code) ? name : code + " - " + name,
                                AccountType = type,
                                NormalBalance = normalBalance,
                                IsBank = isBank,
                                IsCash = isCash,
                                IsActive = isActive
                            });
                        }
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to load account dropdown items.", ex);
            }
        }

        public int SaveGroup(AccGroupModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            try
            {
                string groupType = ResolveGroupType(model.AccountTypeId);
                using (SqlConnection cn = CreateConnection())
                {
                    cn.Open();
                    using (SqlCommand cmd = model.Id <= 0 ? BuildInsertGroupCommand(cn, model, groupType) : BuildUpdateGroupCommand(cn, model, groupType))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            return model.Id;
                        }

                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to save accounting group.", ex);
            }
        }

        public int SaveAccount(AccAccountModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("model");
            }

            try
            {
                using (SqlConnection cn = CreateConnection())
                {
                    cn.Open();
                    using (SqlCommand cmd = model.Id <= 0 ? BuildInsertAccountCommand(cn, model) : BuildUpdateAccountCommand(cn, model))
                    {
                        object result = cmd.ExecuteScalar();
                        if (result == null || result == DBNull.Value)
                        {
                            return model.Id;
                        }

                        return Convert.ToInt32(result);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to save accounting account.", ex);
            }
        }

        public bool DeleteAccount(int accId)
        {
            try
            {
                if (CheckAccountHasEntries(accId) > 0)
                {
                    return false;
                }

                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand(@"
UPDATE acc_accounts
SET is_active = 0
WHERE acc_id = @acc_id;", cn))
                {
                    cmd.Parameters.AddWithValue("@acc_id", accId);
                    cn.Open();
                    return cmd.ExecuteNonQuery() > 0;
                }
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to delete accounting account.", ex);
            }
        }

        public int CheckAccountHasEntries(int accId)
        {
            try
            {
                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM acc_entries WHERE acc_id = @acc_id;", cn))
                {
                    cmd.Parameters.AddWithValue("@acc_id", accId);
                    cn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                throw new DataException("Failed to check account entry count.", ex);
            }
        }

        private static string ResolveGroupType(int accountTypeId)
        {
            switch (accountTypeId)
            {
                case 2:
                    return "Liabilities";
                case 3:
                    return "Equity";
                case 4:
                    return "Income";
                case 5:
                    return "Expenses";
                default:
                    return "Assets";
            }
        }

        private static SqlCommand BuildInsertGroupCommand(SqlConnection cn, AccGroupModel model, string groupType)
        {
            SqlCommand cmd = new SqlCommand(@"
INSERT INTO acc_groups
    (group_code, group_name, parent_group_id, group_type, [level], sort_order, is_active, created_at)
VALUES
    (@group_code, @group_name, @parent_group_id, @group_type, @level, @sort_order, @is_active, @created_at);
SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);

            cmd.Parameters.AddWithValue("@group_code", NormalizeText(model.GroupCode));
            cmd.Parameters.AddWithValue("@group_name", NormalizeText(model.GroupName));
            cmd.Parameters.AddWithValue("@parent_group_id", model.ParentGroupId <= 0 ? (object)DBNull.Value : model.ParentGroupId);
            cmd.Parameters.AddWithValue("@group_type", groupType);
            cmd.Parameters.AddWithValue("@level", model.Level <= 0 ? 1 : model.Level);
            cmd.Parameters.AddWithValue("@sort_order", model.AccountTypeId);
            cmd.Parameters.AddWithValue("@is_active", model.IsActive);
            cmd.Parameters.AddWithValue("@created_at", model.CreatedAt == default(DateTime) ? DateTime.Now : model.CreatedAt);
            return cmd;
        }

        private static SqlCommand BuildUpdateGroupCommand(SqlConnection cn, AccGroupModel model, string groupType)
        {
            SqlCommand cmd = new SqlCommand(@"
UPDATE acc_groups
SET group_code = @group_code,
    group_name = @group_name,
    parent_group_id = @parent_group_id,
    group_type = @group_type,
    [level] = @level,
    sort_order = @sort_order,
    is_active = @is_active
WHERE group_id = @group_id;
SELECT @group_id;", cn);

            cmd.Parameters.AddWithValue("@group_id", model.Id);
            cmd.Parameters.AddWithValue("@group_code", NormalizeText(model.GroupCode));
            cmd.Parameters.AddWithValue("@group_name", NormalizeText(model.GroupName));
            cmd.Parameters.AddWithValue("@parent_group_id", model.ParentGroupId <= 0 ? (object)DBNull.Value : model.ParentGroupId);
            cmd.Parameters.AddWithValue("@group_type", groupType);
            cmd.Parameters.AddWithValue("@level", model.Level <= 0 ? 1 : model.Level);
            cmd.Parameters.AddWithValue("@sort_order", model.AccountTypeId);
            cmd.Parameters.AddWithValue("@is_active", model.IsActive);
            return cmd;
        }

        private static SqlCommand BuildInsertAccountCommand(SqlConnection cn, AccAccountModel model)
        {
            SqlCommand cmd = new SqlCommand(@"
INSERT INTO acc_accounts
    (acc_code, acc_name, parent_group_id, account_type, normal_balance, is_bank, is_cash, bank_name, branch, account_no, opening_balance, opening_date, is_active, created_at)
VALUES
    (@acc_code, @acc_name, @parent_group_id, @account_type, @normal_balance, @is_bank, @is_cash, @bank_name, @branch, @account_no, @opening_balance, @opening_date, @is_active, @created_at);
SELECT CAST(SCOPE_IDENTITY() AS INT);", cn);

            AddAccountParameters(cmd, model);
            return cmd;
        }

        private static SqlCommand BuildUpdateAccountCommand(SqlConnection cn, AccAccountModel model)
        {
            SqlCommand cmd = new SqlCommand(@"
UPDATE acc_accounts
SET acc_code = @acc_code,
    acc_name = @acc_name,
    parent_group_id = @parent_group_id,
    account_type = @account_type,
    normal_balance = @normal_balance,
    is_bank = @is_bank,
    is_cash = @is_cash,
    bank_name = @bank_name,
    branch = @branch,
    account_no = @account_no,
    opening_balance = @opening_balance,
    opening_date = @opening_date,
    is_active = @is_active
WHERE acc_id = @acc_id;
SELECT @acc_id;", cn);

            cmd.Parameters.AddWithValue("@acc_id", model.Id);
            AddAccountParameters(cmd, model);
            return cmd;
        }

        private static void AddAccountParameters(SqlCommand cmd, AccAccountModel model)
        {
            cmd.Parameters.AddWithValue("@acc_code", NormalizeText(model.AccountCode));
            cmd.Parameters.AddWithValue("@acc_name", NormalizeText(model.AccountName));
            cmd.Parameters.AddWithValue("@parent_group_id", model.ParentGroupId <= 0 ? (object)DBNull.Value : model.ParentGroupId);
            cmd.Parameters.AddWithValue("@account_type", NormalizeText(model.AccountType) ?? "General");
            cmd.Parameters.AddWithValue("@normal_balance", string.IsNullOrWhiteSpace(model.NormalBalance) ? "Dr" : model.NormalBalance.Trim());
            cmd.Parameters.AddWithValue("@is_bank", model.IsBankAccount);
            cmd.Parameters.AddWithValue("@is_cash", model.IsCashAccount);
            cmd.Parameters.AddWithValue("@bank_name", (object)NormalizeText(model.BankName) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@branch", (object)NormalizeText(model.BankBranch) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@account_no", (object)NormalizeText(model.AccountNo) ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@opening_balance", model.OpeningBalance);
            cmd.Parameters.AddWithValue("@opening_date", model.OpeningBalanceDate == default(DateTime) ? (object)DBNull.Value : model.OpeningBalanceDate.Date);
            cmd.Parameters.AddWithValue("@is_active", model.IsActive);
            cmd.Parameters.AddWithValue("@created_at", model.CreatedAt == default(DateTime) ? DateTime.Now : model.CreatedAt);
        }

        private static AccAccountModel MapAccount(SqlDataReader reader)
        {
            AccAccountModel model = new AccAccountModel();
            model.Id = reader["acc_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["acc_id"]);
            model.AccountCode = Convert.ToString(reader["acc_code"]);
            model.AccountName = Convert.ToString(reader["acc_name"]);
            model.ParentGroupId = reader["parent_group_id"] == DBNull.Value ? 0 : Convert.ToInt32(reader["parent_group_id"]);
            model.AccountType = Convert.ToString(reader["account_type"]);
            model.NormalBalance = Convert.ToString(reader["normal_balance"]);
            model.IsBankAccount = reader["is_bank"] != DBNull.Value && Convert.ToBoolean(reader["is_bank"]);
            model.IsCashAccount = reader["is_cash"] != DBNull.Value && Convert.ToBoolean(reader["is_cash"]);
            model.BankName = reader["bank_name"] == DBNull.Value ? null : Convert.ToString(reader["bank_name"]);
            model.BankBranch = reader["branch"] == DBNull.Value ? null : Convert.ToString(reader["branch"]);
            model.AccountNo = reader["account_no"] == DBNull.Value ? null : Convert.ToString(reader["account_no"]);
            model.OpeningBalance = reader["opening_balance"] == DBNull.Value ? 0m : Convert.ToDecimal(reader["opening_balance"]);
            model.OpeningBalanceDate = reader["opening_date"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(reader["opening_date"]);
            model.IsActive = reader["is_active"] == DBNull.Value || Convert.ToBoolean(reader["is_active"]);
            model.CreatedAt = reader["created_at"] == DBNull.Value ? DateTime.Now : Convert.ToDateTime(reader["created_at"]);
            return model;
        }
    }
}
