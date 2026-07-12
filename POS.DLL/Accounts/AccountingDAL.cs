using System;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class AccountingDAL : AccountingDalBase
    {
        private static readonly object VoucherNoLock = new object();

        public AccountingDAL()
        {
        }

        public AccountingDAL(string connectionString)
            : base(connectionString)
        {
        }

        public int GetCurrentPeriodId(DateTime date)
        {
            return new PeriodDAL(ConnectionString).GetCurrentPeriodId(date);
        }

        public string GenerateVoucherNo(string prefix)
        {
            string safePrefix = string.IsNullOrWhiteSpace(prefix) ? "JV" : prefix.Trim();

            lock (VoucherNoLock)
            {
                for (int attempt = 0; attempt < 10; attempt++)
                {
                    string candidate = TryGenerateFromStoredProcedure(safePrefix);
                    if (string.IsNullOrWhiteSpace(candidate))
                    {
                        candidate = BuildFallbackVoucherNo(safePrefix, attempt);
                    }

                    if (!VoucherNoExists(candidate))
                    {
                        return candidate;
                    }
                }

                return BuildFallbackVoucherNo(safePrefix, 10);
            }
        }

        private string TryGenerateFromStoredProcedure(string prefix)
        {
            try
            {
                using (SqlConnection cn = CreateConnection())
                using (SqlCommand cmd = new SqlCommand("sp_GenerateVoucherNo", cn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Prefix", prefix);
                    cn.Open();
                    object value = cmd.ExecuteScalar();
                    return value == null || value == DBNull.Value ? null : Convert.ToString(value);
                }
            }
            catch
            {
                return null;
            }
        }

        private bool VoucherNoExists(string voucherNo)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("SELECT COUNT(1) FROM acc_entries_header WHERE InvoiceNo = @voucher_no;", cn))
            {
                cmd.Parameters.AddWithValue("@voucher_no", voucherNo);
                cn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }

        private static string BuildFallbackVoucherNo(string prefix, int attempt)
        {
            string stamp = DateTime.Now.ToString("yyyyMMdd");
            return string.Format("{0}-{1}-{2:0000}", prefix, stamp, attempt + 1);
        }
    }
}
