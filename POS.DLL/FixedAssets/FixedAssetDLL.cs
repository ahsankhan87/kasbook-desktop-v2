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

        public DataTable GetAllAssets()
        {
            return ExecuteDataTable("dbo.sp_FixedAssetsCrud", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@OperationType", 5); // Select All active
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

        public DataTable GetDepreciationHistory(int assetId)
        {
            return ExecuteDataTable("SELECT * FROM fa_asset_depreciation WHERE asset_id = @AssetId ORDER BY period_date DESC", cmd =>
            {
                cmd.CommandType = CommandType.Text;
                AddParameter(cmd, "@AssetId", assetId);
            });
        }

        // ============================================================
        // Category CRUD (unified sp_FixedAssetCategoriesCrud)
        // OperationTypes: 1=Insert, 2=Update, 3=Delete, 4=Select One, 5=Select All
        // ============================================================

        public DataTable GetCategories(bool activeOnly = true)
        {
            return ExecuteDataTable("dbo.sp_FixedAssetCategoriesCrud", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@is_active", activeOnly ? 1 : 0);
                AddParameter(cmd, "@OperationType", 5); // Select All
            });
        }

        public int InsertCategory(string categoryCode, string categoryName, string deprecationMethod = "STRAIGHT_LINE", int usefulLifeMonths = 60, decimal? annualDepreciationRate = null)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("dbo.sp_FixedAssetCategoriesCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@category_code", categoryCode);
                AddParameter(cmd, "@category_name", categoryName);
                AddParameter(cmd, "@depreciation_method", deprecationMethod);
                AddParameter(cmd, "@useful_life_months", usefulLifeMonths);
                AddParameter(cmd, "@annual_depreciation_rate", annualDepreciationRate ?? (object)DBNull.Value);
                AddParameter(cmd, "@is_active", 1);

                SqlParameter idParam = new SqlParameter("@category_id", SqlDbType.Int) { Direction = ParameterDirection.InputOutput };
                cmd.Parameters.Add(idParam);
                AddParameter(cmd, "@OperationType", 1); // Insert

                cn.Open();
                cmd.ExecuteNonQuery();

                return (int)idParam.Value;
            }
        }

        public int UpdateCategory(int categoryId, string categoryName, string deprecationMethod, int usefulLifeMonths, decimal? annualDepreciationRate = null, bool isActive = true)
        {
            return ExecuteNonQuery("dbo.sp_FixedAssetCategoriesCrud", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@category_id", categoryId);
                AddParameter(cmd, "@category_name", categoryName);
                AddParameter(cmd, "@depreciation_method", deprecationMethod);
                AddParameter(cmd, "@useful_life_months", usefulLifeMonths);
                AddParameter(cmd, "@annual_depreciation_rate", annualDepreciationRate ?? (object)DBNull.Value);
                AddParameter(cmd, "@is_active", isActive ? 1 : 0);
                AddParameter(cmd, "@OperationType", 2); // Update
            });
        }

        public int DeleteCategory(int categoryId)
        {
            return ExecuteNonQuery("dbo.sp_FixedAssetCategoriesCrud", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@category_id", categoryId);
                AddParameter(cmd, "@OperationType", 3); // Delete
            });
        }

        // ============================================================
        // Location CRUD (unified sp_FixedAssetLocationsCrud)
        // OperationTypes: 1=Insert, 2=Update, 3=Delete, 4=Select One, 5=Select All
        // ============================================================

        public DataTable GetLocations(bool activeOnly = true)
        {
            return ExecuteDataTable("dbo.sp_FixedAssetLocationsCrud", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@is_active", activeOnly ? 1 : 0);
                AddParameter(cmd, "@OperationType", 5); // Select All
            });
        }

        public int InsertLocation(string locationCode, string locationName, string locationType = "LOCATION", int? parentLocationId = null)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("dbo.sp_FixedAssetLocationsCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@location_code", locationCode);
                AddParameter(cmd, "@location_name", locationName);
                AddParameter(cmd, "@location_type", locationType);
                AddParameter(cmd, "@parent_location_id", parentLocationId ?? (object)DBNull.Value);
                AddParameter(cmd, "@is_active", 1);

                SqlParameter idParam = new SqlParameter("@location_id", SqlDbType.Int) { Direction = ParameterDirection.InputOutput };
                cmd.Parameters.Add(idParam);
                AddParameter(cmd, "@OperationType", 1); // Insert

                cn.Open();
                cmd.ExecuteNonQuery();

                return (int)idParam.Value;
            }
        }

        public int UpdateLocation(int locationId, string locationName, string locationType, int? parentLocationId = null, bool isActive = true)
        {
            return ExecuteNonQuery("dbo.sp_FixedAssetLocationsCrud", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@location_id", locationId);
                AddParameter(cmd, "@location_name", locationName);
                AddParameter(cmd, "@location_type", locationType);
                AddParameter(cmd, "@parent_location_id", parentLocationId ?? (object)DBNull.Value);
                AddParameter(cmd, "@is_active", isActive ? 1 : 0);
                AddParameter(cmd, "@OperationType", 2); // Update
            });
        }

        public int DeleteLocation(int locationId)
        {
            return ExecuteNonQuery("dbo.sp_FixedAssetLocationsCrud", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@location_id", locationId);
                AddParameter(cmd, "@OperationType", 3); // Delete
            });
        }

        // ============================================================
        // Asset CRUD (unified sp_FixedAssetsCrud)
        // OperationTypes: 1=Insert, 2=Update, 3=Delete, 4=Select One, 5=Select All
        // ============================================================

        public int InsertAsset(string assetCode, string assetName, int categoryId, int? locationId = null, string serialNumber = null, DateTime? purchaseDate = null, decimal cost = 0, string depMethod = "STRAIGHT_LINE", int usefulLifeMonths = 60, decimal salvageValue = 0, decimal? replacementCost = null, string notes = null, int? createdBy = null)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("dbo.sp_FixedAssetsCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@asset_code", assetCode);
                AddParameter(cmd, "@asset_name", assetName);
                AddParameter(cmd, "@category_id", categoryId);
                AddParameter(cmd, "@location_id", locationId ?? (object)DBNull.Value);
                AddParameter(cmd, "@serial_number", serialNumber ?? (object)DBNull.Value);
                AddParameter(cmd, "@purchase_date", purchaseDate ?? (object)DBNull.Value);
                AddParameter(cmd, "@cost", cost);
                AddParameter(cmd, "@dep_method", depMethod);
                AddParameter(cmd, "@useful_life_months", usefulLifeMonths);
                AddParameter(cmd, "@salvage_value", salvageValue);
                AddParameter(cmd, "@replacement_cost", replacementCost ?? (object)DBNull.Value);
                AddParameter(cmd, "@notes", notes ?? (object)DBNull.Value);
                AddParameter(cmd, "@created_by", createdBy ?? (object)DBNull.Value);
                AddParameter(cmd, "@is_active", 1);

                SqlParameter idParam = new SqlParameter("@asset_id", SqlDbType.Int) { Direction = ParameterDirection.InputOutput };
                cmd.Parameters.Add(idParam);
                AddParameter(cmd, "@OperationType", 1); // Insert

                cn.Open();
                cmd.ExecuteNonQuery();

                return (int)idParam.Value;
            }
        }

        public int UpdateAssetDetails(int assetId, string assetName, int? locationId = null, string notes = null, bool isActive = true)
        {
            return ExecuteNonQuery("dbo.sp_FixedAssetsCrud", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@asset_id", assetId);
                AddParameter(cmd, "@asset_name", assetName);
                AddParameter(cmd, "@location_id", locationId ?? (object)DBNull.Value);
                AddParameter(cmd, "@notes", notes ?? (object)DBNull.Value);
                AddParameter(cmd, "@is_active", isActive ? 1 : 0);
                AddParameter(cmd, "@OperationType", 2); // Update
            });
        }

        public int DeleteAsset(int assetId)
        {
            return ExecuteNonQuery("dbo.sp_FixedAssetsCrud", cmd =>
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@asset_id", assetId);
                AddParameter(cmd, "@OperationType", 3); // Delete
            });
        }

    }
}
