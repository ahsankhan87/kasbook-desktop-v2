using System;
using System.Data;
using System.Data.SqlClient;

namespace POS.DLL
{
    public class FixedAssetDLL : AccountingDalBase
    {
        public DataTable GetEligibleAssetsForDepreciation(DateTime periodDate)
        {
            // Direct query so we have full control over eligibility logic.
            // ISNULL(depreciation_start_date, purchase_date) handles assets whose
            // depreciation_start_date was never set (NULL) by falling back to purchase_date.
            // The NOT EXISTS sub-query uses fa_depreciation_runs.period_date to prevent
            // duplicate runs for the same year/month.
            const string sql = @"
                SELECT fa.*
                FROM   fa_assets fa
                WHERE  fa.is_active  = 1
                  AND  fa.status NOT IN ('Disposed', 'Fully Depreciated')
                  AND  fa.cost           > 0
                  AND  fa.useful_life_months > 0
                  AND  fa.purchase_date <= @PeriodDate
                  AND  NOT EXISTS (
                           SELECT 1
                           FROM   fa_depreciation_runs d
                           WHERE  d.asset_id = fa.asset_id
                             AND  YEAR(d.period_date)  = YEAR(@PeriodDate)
                             AND  MONTH(d.period_date) = MONTH(@PeriodDate)
                       )";

            return ExecuteDataTable(sql, cmd =>
            {
                cmd.CommandType = System.Data.CommandType.Text;
                AddParameter(cmd, "@PeriodDate", periodDate.Date);
            });
        }

        public DataTable GetAllAssets()
        {
            // Use a direct query instead of the stored procedure so all columns,
            // including dep_account_id and accum_dep_account_id, are always returned.
            const string sql = @"
                SELECT  *
                FROM    dbo.fa_assets
                WHERE   is_active = 1
                ORDER BY asset_code";

            return ExecuteDataTable(sql, cmd =>
            {
                cmd.CommandType = CommandType.Text;
            });
        }

        public bool DepreciationRunExists(int assetId, DateTime periodDate)
        {
            const string sql = @"
                SELECT CASE
                           WHEN EXISTS (
                               SELECT 1
                               FROM fa_depreciation_runs
                               WHERE asset_id = @AssetId
                                 AND YEAR(period_date) = YEAR(@PeriodDate)
                                 AND MONTH(period_date) = MONTH(@PeriodDate)
                           ) THEN 1 ELSE 0 END";

            int exists = ExecuteScalar<int>(sql, cmd =>
            {
                cmd.CommandType = CommandType.Text;
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

        /// <summary>
        /// Inserts a row into fa_asset_disposals and updates fa_assets disposal fields in a single transaction.
        /// </summary>
        public void RecordDisposal(
            int assetId,
            DateTime disposalDate,
            string disposalMethod,
            decimal disposalProceeds,
            decimal disposalCost,
            decimal accumDepAtDisposal,
            decimal wdvAtDisposal,
            string notes)
        {
            const string sql = @"
                INSERT INTO dbo.fa_asset_disposals
                    (asset_id, disposal_date, disposal_method, disposal_proceeds,
                     disposal_cost, accum_dep_at_disposal, wdv_at_disposal, notes, created_at)
                VALUES
                    (@AssetId, @DisposalDate, @DisposalMethod, @DisposalProceeds,
                     @DisposalCost, @AccumDepAtDisposal, @WdvAtDisposal, @Notes, GETDATE());

                UPDATE dbo.fa_assets
                SET
                    is_disposed       = 1,
                    disposed_on       = @DisposalDate,
                    disposal_method   = @DisposalMethod,
                    disposal_proceeds = @DisposalProceeds,
                    status         = 'Disposed',
                    updated_at        = GETDATE()
                WHERE asset_id = @AssetId;";

            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                AddParameter(cmd, "@AssetId",            assetId);
                AddParameter(cmd, "@DisposalDate",       disposalDate.Date);
                AddParameter(cmd, "@DisposalMethod",     disposalMethod ?? "Write-Off");
                AddParameter(cmd, "@DisposalProceeds",   disposalProceeds);
                AddParameter(cmd, "@DisposalCost",       disposalCost);
                AddParameter(cmd, "@AccumDepAtDisposal", accumDepAtDisposal);
                AddParameter(cmd, "@WdvAtDisposal",      wdvAtDisposal);
                AddParameter(cmd, "@Notes",              (object)notes ?? DBNull.Value);
                cn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public bool HasAssetRevaluation(int assetId)
        {
            const string sql = @"
                SELECT CASE
                    WHEN EXISTS (SELECT 1 FROM dbo.fa_asset_revaluations WHERE asset_id = @AssetId)
                    THEN 1 ELSE 0 END";

            int exists = ExecuteScalar<int>(sql, cmd =>
            {
                cmd.CommandType = CommandType.Text;
                AddParameter(cmd, "@AssetId", assetId);
            });

            return exists == 1;
        }

        /// <summary>
        /// Inserts a row into fa_asset_revaluations and updates fa_assets revaluation fields.
        /// Only one revaluation is allowed per asset.
        /// </summary>
        public void RecordRevaluation(
            int assetId,
            DateTime revaluationDate,
            decimal oldCost,
            decimal newCost,
            decimal oldAccumDep,
            decimal newAccumDep,
            decimal oldWdv,
            decimal newWdv,
            string notes)
        {
            const string sql = @"
                IF EXISTS (SELECT 1 FROM dbo.fa_asset_revaluations WHERE asset_id = @AssetId)
                BEGIN
                    RAISERROR('This asset is already revalued.', 16, 1);
                    RETURN;
                END;

                INSERT INTO dbo.fa_asset_revaluations
                    (asset_id, revaluation_date, old_cost, new_cost,
                     old_accum_dep, new_accum_dep, old_wdv, new_wdv, notes, created_at)
                VALUES
                    (@AssetId, @RevaluationDate, @OldCost, @NewCost,
                     @OldAccumDep, @NewAccumDep, @OldWdv, @NewWdv, @Notes, GETDATE());

                UPDATE dbo.fa_assets
                SET
                    cost                      = @NewCost,
                    replacement_cost          = @NewCost,
                    accumulated_depreciation  = @NewAccumDep,
                    current_wdv               = @NewWdv,
                    updated_at                = GETDATE()
                WHERE asset_id = @AssetId;";

            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand(sql, cn))
            {
                cmd.CommandType = CommandType.Text;
                AddParameter(cmd, "@AssetId",         assetId);
                AddParameter(cmd, "@RevaluationDate", revaluationDate.Date);
                AddParameter(cmd, "@OldCost",         oldCost);
                AddParameter(cmd, "@NewCost",         newCost);
                AddParameter(cmd, "@OldAccumDep",     oldAccumDep);
                AddParameter(cmd, "@NewAccumDep",     newAccumDep);
                AddParameter(cmd, "@OldWdv",          oldWdv);
                AddParameter(cmd, "@NewWdv",          newWdv);
                AddParameter(cmd, "@Notes",           (object)notes ?? DBNull.Value);
                cn.Open();
                cmd.ExecuteNonQuery();
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
            const string sql = @"
                SELECT
                    r.asset_id,
                    a.asset_code,
                    a.asset_name,
                    a.dep_method,
                    r.period_date,
                    r.opening_wdv,
                    r.dep_amount AS depreciation_amount,
                    r.closing_wdv,
                    r.voucher_id
                FROM fa_depreciation_runs r
                INNER JOIN fa_assets a ON a.asset_id = r.asset_id
                WHERE r.asset_id = @AssetId
                ORDER BY r.period_date DESC";

            return ExecuteDataTable(sql, cmd =>
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

        public int UpdateAssetInfoTabDetails(
            int assetId,
            string assetName,
            string assetDescription,
            DateTime purchaseDate,
            string supplierName,
            string purchaseInvoiceNo,
            int? locationId,
            string serialNumber,
            string modelNumber,
            string status)
        {
            const string sql = @"
                UPDATE dbo.fa_assets
                SET asset_name   = @AssetName,
                    purchase_date = @PurchaseDate,
                    location_id   = @LocationId,
                    serial_number = @SerialNumber,
                    notes         = CASE WHEN ISNULL(NULLIF(LTRIM(RTRIM(@AssetDescription)), ''), '') = '' THEN notes ELSE @AssetDescription END,
                    updated_at    = GETDATE()
                WHERE asset_id = @AssetId;

                IF COL_LENGTH('dbo.fa_assets', 'purchase_invoice_no') IS NOT NULL
                    UPDATE dbo.fa_assets SET purchase_invoice_no = @PurchaseInvoiceNo WHERE asset_id = @AssetId;

                IF COL_LENGTH('dbo.fa_assets', 'model_number') IS NOT NULL
                    UPDATE dbo.fa_assets SET model_number = @ModelNumber WHERE asset_id = @AssetId;

                IF COL_LENGTH('dbo.fa_assets', 'supplier_name') IS NOT NULL
                    UPDATE dbo.fa_assets SET supplier_name = @SupplierName WHERE asset_id = @AssetId;

                IF COL_LENGTH('dbo.fa_assets', 'supplier_id') IS NOT NULL
                    UPDATE dbo.fa_assets
                    SET supplier_id = CASE
                        WHEN ISNULL(NULLIF(LTRIM(RTRIM(@SupplierName)), ''), '') = '' THEN NULL
                        ELSE (SELECT TOP 1 id FROM dbo.pos_suppliers WHERE first_name = @SupplierName ORDER BY id)
                    END
                    WHERE asset_id = @AssetId;";

            return ExecuteNonQuery(sql, cmd =>
            {
                cmd.CommandType = CommandType.Text;
                AddParameter(cmd, "@AssetId", assetId);
                AddParameter(cmd, "@AssetName", assetName ?? (object)DBNull.Value);
                AddParameter(cmd, "@AssetDescription", assetDescription ?? (object)DBNull.Value);
                AddParameter(cmd, "@PurchaseDate", purchaseDate.Date);
                AddParameter(cmd, "@SupplierName", supplierName ?? (object)DBNull.Value);
                AddParameter(cmd, "@PurchaseInvoiceNo", purchaseInvoiceNo ?? (object)DBNull.Value);
                AddParameter(cmd, "@LocationId", locationId ?? (object)DBNull.Value);
                AddParameter(cmd, "@SerialNumber", serialNumber ?? (object)DBNull.Value);
                AddParameter(cmd, "@ModelNumber", modelNumber ?? (object)DBNull.Value);
                AddParameter(cmd, "@Status", status ?? "Active");
            });
        }

        public int UpdateAssetDepreciationAccounts(int assetId, int depAccountId, int accumDepAccountId)
        {
            const string sql = @"
                UPDATE fa_assets
                SET    dep_account_id       = @DepAccountId,
                       accum_dep_account_id = @AccumDepAccountId
                WHERE  asset_id = @AssetId";

            return ExecuteNonQuery(sql, cmd =>
            {
                cmd.CommandType = CommandType.Text;
                AddParameter(cmd, "@AssetId", assetId);
                AddParameter(cmd, "@DepAccountId", depAccountId);
                AddParameter(cmd, "@AccumDepAccountId", accumDepAccountId);
            });
        }

        public bool HasAssetTransactions(int assetId)
        {
            const string sql = @"
                SELECT CASE
                    WHEN EXISTS (SELECT 1 FROM fa_depreciation_runs WHERE asset_id = @AssetId)
                         OR EXISTS (
                            SELECT 1
                            FROM acc_entries
                            WHERE ref_id = @AssetId
                              AND LOWER(ISNULL(ref_module, '')) = 'fixedassets'
                         )
                    THEN 1 ELSE 0 END";

            int exists = ExecuteScalar<int>(sql, cmd =>
            {
                cmd.CommandType = CommandType.Text;
                AddParameter(cmd, "@AssetId", assetId);
            });

            return exists == 1;
        }

        public int DeleteAssetIfNoTransactions(int assetId)
        {
            if (HasAssetTransactions(assetId))
            {
                return 0;
            }

            return DeleteAsset(assetId);
        }

        public int DeleteAsset(int assetId)
        {
            using (SqlConnection cn = CreateConnection())
            using (SqlCommand cmd = new SqlCommand("dbo.sp_FixedAssetsCrud", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                AddParameter(cmd, "@asset_id", assetId);
                AddParameter(cmd, "@OperationType", 3); // Delete

                SqlParameter returnParam = new SqlParameter("@ReturnValue", SqlDbType.Int)
                {
                    Direction = ParameterDirection.ReturnValue
                };
                cmd.Parameters.Add(returnParam);

                cn.Open();
                cmd.ExecuteNonQuery();

                if (returnParam.Value == null || returnParam.Value == DBNull.Value)
                {
                    return -1;
                }

                return Convert.ToInt32(returnParam.Value);
            }
        }

    }
}
