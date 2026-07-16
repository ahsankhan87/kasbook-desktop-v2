using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using POS.Core;

namespace POS.DLL
{
    /// <summary>
    /// Data-access layer for pos_settings.
    /// All accounting-module setting reads and writes go through this class.
    /// </summary>
    public class AccountingSettingsDLL : AccountingDalBase
    {
        public AccountingSettingsDLL() { }

        public AccountingSettingsDLL(string connectionString)
            : base(connectionString) { }

        // ── Read ─────────────────────────────────────────────────────────

        /// <summary>
        /// Loads every row from pos_settings into memory.
        /// Called once at application startup by <see cref="POS.BLL.AccountingSettingsService"/>.
        /// </summary>
        public List<AccountingSettingModel> LoadAll()
        {
            const string sql = @"
                SELECT setting_key,
                       setting_value,
                       ISNULL(setting_type,  'STRING')  AS setting_type,
                       ISNULL(description,   '')        AS description,
                       ISNULL(category,      'GENERAL') AS category,
                       ISNULL(is_required,   0)         AS is_required,
                       ISNULL(is_encrypted,  0)         AS is_encrypted,
                       modified_by,
                       modified_at
                FROM   dbo.pos_settings
                ORDER  BY setting_key;";

            var list = new List<AccountingSettingModel>();

            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        list.Add(new AccountingSettingModel
                        {
                            Key         = Convert.ToString(rdr.GetValue(0)),
                            Value       = rdr.IsDBNull(1) ? null : Convert.ToString(rdr.GetValue(1)),
                            SettingType = Convert.ToString(rdr.GetValue(2)),
                            Description = Convert.ToString(rdr.GetValue(3)),
                            Category    = Convert.ToString(rdr.GetValue(4)),
                            IsRequired  = !rdr.IsDBNull(5) && Convert.ToBoolean(rdr.GetValue(5)),
                            IsEncrypted = !rdr.IsDBNull(6) && Convert.ToBoolean(rdr.GetValue(6)),
                            ModifiedBy  = rdr.IsDBNull(7) ? (int?)null : Convert.ToInt32(rdr.GetValue(7)),
                            ModifiedAt  = rdr.IsDBNull(8) ? (DateTime?)null : Convert.ToDateTime(rdr.GetValue(8)),
                        });
                    }
                }
            }

            return list;
        }

        /// <summary>
        /// Returns a single <see cref="AccountingSettingModel"/> by key, or <c>null</c>.
        /// </summary>
        public AccountingSettingModel GetByKey(string key)
        {
            const string sql = @"
SELECT setting_key, setting_value,
       ISNULL(setting_type, 'STRING')  AS setting_type,
       ISNULL(description, '')         AS description,
       ISNULL(category, 'GENERAL')     AS category,
       ISNULL(is_required, 0)          AS is_required,
       ISNULL(is_encrypted, 0)         AS is_encrypted,
       modified_by, modified_at
FROM   dbo.pos_settings
WHERE  setting_key = @key;";

            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@key", key);
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!rdr.Read()) return null;
                    return new AccountingSettingModel
                    {
                        Key         = Convert.ToString(rdr.GetValue(0)),
                        Value       = rdr.IsDBNull(1) ? null : Convert.ToString(rdr.GetValue(1)),
                        SettingType = Convert.ToString(rdr.GetValue(2)),
                        Description = Convert.ToString(rdr.GetValue(3)),
                        Category    = Convert.ToString(rdr.GetValue(4)),
                        IsRequired  = !rdr.IsDBNull(5) && Convert.ToBoolean(rdr.GetValue(5)),
                        IsEncrypted = !rdr.IsDBNull(6) && Convert.ToBoolean(rdr.GetValue(6)),
                        ModifiedBy  = rdr.IsDBNull(7) ? (int?)null : Convert.ToInt32(rdr.GetValue(7)),
                        ModifiedAt  = rdr.IsDBNull(8) ? (DateTime?)null : Convert.ToDateTime(rdr.GetValue(8)),
                    };
                }
            }
        }

        // ── Write ────────────────────────────────────────────────────────

        /// <summary>
        /// Upserts a setting value. If the row does not exist it is inserted with
        /// sensible defaults; if it does exist only <c>setting_value</c>,
        /// <c>modified_by</c> and <c>modified_at</c> are updated.
        /// </summary>
        public void Upsert(string key, string value, int modifiedBy)
        {
            const string sql = @"
IF EXISTS (SELECT 1 FROM dbo.pos_settings WHERE setting_key = @key)
BEGIN
    UPDATE dbo.pos_settings
    SET    setting_value = @value,
           modified_by   = @modified_by,
           modified_at   = GETDATE()
    WHERE  setting_key = @key;
END
ELSE
BEGIN
    INSERT INTO dbo.pos_settings
        (setting_key, setting_value, setting_type, category, is_required, is_encrypted,
         modified_by, modified_at)
    VALUES
        (@key, @value, 'STRING', 'GENERAL', 0, 0, @modified_by, GETDATE());
END";

            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@key",         key);
                cmd.Parameters.AddWithValue("@value",       (object)value ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@modified_by", modifiedBy);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // ── Account lookup ───────────────────────────────────────────────

        /// <summary>
        /// Looks up an <see cref="AccountsModal"/> from <c>acc_accounts</c> by its
        /// account code.  Returns <c>null</c> if not found.
        /// </summary>
        public AccountsModal GetAccountByCode(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) return null;

            const string sql = @"
SELECT TOP 1
    id, branch_id, code, name, name_2, group_id,
    ISNULL(op_dr_balance, 0) AS op_dr_balance,
    ISNULL(op_cr_balance, 0) AS op_cr_balance,
    description, user_id, date_created, is_active
FROM  dbo.acc_accounts
WHERE code = @code
  AND ISNULL(is_active, 1) = 1;";

            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@code", code.Trim());
                cn.Open();
                using (SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                {
                    if (!rdr.Read()) return null;

                    return new AccountsModal
                    {
                        id             = rdr.GetInt32(0),
                        code           = rdr.IsDBNull(2) ? null : rdr.GetString(2),
                        name           = rdr.IsDBNull(3) ? null : rdr.GetString(3),
                        name_2         = rdr.IsDBNull(4) ? null : rdr.GetString(4),
                        group_id       = rdr.IsDBNull(5) ? 0    : rdr.GetInt32(5),
                        op_dr_balance  = rdr.IsDBNull(6) ? 0    : Convert.ToDouble(rdr.GetValue(6)),
                        op_cr_balance  = rdr.IsDBNull(7) ? 0    : Convert.ToDouble(rdr.GetValue(7)),
                        description    = rdr.IsDBNull(8) ? null : rdr.GetString(8),
                        user_id        = rdr.IsDBNull(9) ? 0    : rdr.GetInt32(9),
                        date_created   = rdr.IsDBNull(10)? null : rdr.GetValue(10).ToString(),
                    };
                }
            }
        }

        // ── Voucher sequence (thread-safe via SQL UPDLOCK) ────────────────

        /// <summary>
        /// Atomically increments and returns the next sequence number for the given
        /// voucher type.  If no counter row exists it is seeded at 1.
        /// Uses <c>UPDLOCK + SERIALIZABLE</c> to prevent concurrent duplicate numbers.
        /// </summary>
        public int GetNextVoucherSequence(string voucherType)
        {
            string seqKey = SettingKeys.VoucherSeqPrefix + voucherType.ToUpperInvariant();

            const string sql = @"
BEGIN TRAN;
DECLARE @current INT;

SELECT @current = CAST(setting_value AS INT)
FROM   dbo.pos_settings WITH (UPDLOCK, ROWLOCK)
WHERE  setting_key = @key;

IF @current IS NULL
BEGIN
    -- First time: seed the counter
    INSERT INTO dbo.pos_settings
        (setting_key, setting_value, setting_type, category, is_required, is_encrypted)
    VALUES (@key, '1', 'INT', 'JOURNALS', 0, 0);
    SET @current = 1;
END
ELSE
BEGIN
    SET @current = @current + 1;
    UPDATE dbo.pos_settings
    SET    setting_value = CAST(@current AS VARCHAR(20))
    WHERE  setting_key   = @key;
END

COMMIT TRAN;
SELECT @current;";

            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.Parameters.AddWithValue("@key", seqKey);
                cn.Open();
                object result = cmd.ExecuteScalar();
                return Convert.ToInt32(result);
            }
        }
    }
}
