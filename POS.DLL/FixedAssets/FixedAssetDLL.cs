using System;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class FixedAssetDLL : AccountingDalBase
    {
        public DataTable GetEligibleAssetsForDepreciation(DateTime periodDate)
        {
            return ExecuteDataTable("dbo.sp_FixedAsset_GetEligibleAssetsForDepreciation", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@PeriodDate", periodDate.Date);
            });
        }

        public bool DepreciationRunExists(int assetId, DateTime periodDate)
        {
            int exists = ExecuteScalar<int>("dbo.sp_FixedAsset_DepreciationRunExists", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@AssetId", assetId);
                AddParameter(cmd, "@PeriodDate", periodDate.Date);
            });

            return exists == 1;
        }

        public DataTable GetMonthlyDepreciationPreview(DateTime periodDate)
        {
            return ExecuteDataTable("dbo.sp_FixedAsset_GetMonthlyDepreciationPreview", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@PeriodDate", periodDate.Date);
            });
        }

        public int InsertDepreciationRun(int assetId, DateTime periodDate, decimal openingWdv, decimal depAmount, decimal closingWdv, int? voucherId, int runBy, DateTime runAt)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("dbo.sp_FixedAsset_InsertDepreciationRun", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@AssetId", assetId);
                AddParameter(cmd, "@PeriodDate", periodDate.Date);
                AddParameter(cmd, "@OpeningWdv", openingWdv);
                AddParameter(cmd, "@DepAmount", depAmount);
                AddParameter(cmd, "@ClosingWdv", closingWdv);
                AddParameter(cmd, "@VoucherId", voucherId);
                AddParameter(cmd, "@RunBy", runBy);
                AddParameter(cmd, "@RunAt", runAt);

                cn.Open();
                object value = cmd.ExecuteScalar();
                if (value == null || value == DBNull.Value)
                {
                    return 0;
                }

                return Convert.ToInt32(value);
            }
        }

        public int UpdateAssetDepreciationState(int assetId, decimal accumulatedDepreciation, decimal currentWdv, DateTime lastDepDate, string status)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("dbo.sp_FixedAsset_UpdateAssetDepreciationState", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@AssetId", assetId);
                AddParameter(cmd, "@AccumulatedDepreciation", accumulatedDepreciation);
                AddParameter(cmd, "@CurrentWdv", currentWdv);
                AddParameter(cmd, "@LastDepDate", lastDepDate.Date);
                AddParameter(cmd, "@Status", status);

                cn.Open();
                return cmd.ExecuteNonQuery();
            }
        }
    }
}
