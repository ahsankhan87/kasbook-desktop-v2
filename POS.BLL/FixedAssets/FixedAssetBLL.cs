using System;
using System.Collections.Generic;
using System.Data;
using POS.Core;
using POS.DLL;

namespace POS.BLL.FixedAssets
{
    public class FixedAssetBLL
    {
        private readonly FixedAssetDLL _dll;

        public FixedAssetBLL()
        {
            _dll = new FixedAssetDLL();
        }

        public List<FixedAssetModel> GetAllAssets()
        {
            DataTable table = _dll.GetAllAssets();
            List<FixedAssetModel> assets = MapAssets(table);

            // Enrich with category/location names via in-memory lookup
            // (the SP returns only category_id / location_id)
            if (assets.Count > 0)
            {
                var categories = _dll.GetCategories(activeOnly: false);
                var locations  = _dll.GetLocations(activeOnly: false);

                // Build lookup dictionaries
                var catMap = new System.Collections.Generic.Dictionary<int, string>();
                foreach (System.Data.DataRow r in categories.Rows)
                {
                    int id = GetInt(r, "category_id");
                    if (id > 0) catMap[id] = GetString(r, "category_name", "");
                }

                var locMap = new System.Collections.Generic.Dictionary<int, string>();
                foreach (System.Data.DataRow r in locations.Rows)
                {
                    int id = GetInt(r, "location_id");
                    if (id > 0) locMap[id] = GetString(r, "location_name", "");
                }

                foreach (FixedAssetModel a in assets)
                {
                    if (a.CategoryId > 0 && string.IsNullOrEmpty(a.CategoryName))
                    {
                        string name;
                        if (catMap.TryGetValue(a.CategoryId, out name)) a.CategoryName = name;
                    }
                    if (a.LocationId > 0 && string.IsNullOrEmpty(a.LocationName))
                    {
                        string name;
                        if (locMap.TryGetValue(a.LocationId, out name)) a.LocationName = name;
                    }
                }
            }

            return assets;
        }

        public List<FixedAssetModel> GetEligibleAssetsForDepreciation(DateTime periodDate)
        {
            DataTable table = _dll.GetEligibleAssetsForDepreciation(periodDate);
            return MapAssets(table);
        }

        public bool DepreciationRunExists(int assetId, DateTime periodDate)
        {
            return _dll.DepreciationRunExists(assetId, NormalizePeriodDate(periodDate));
        }

        public DataTable GetMonthlyDepreciationPreview(DateTime periodDate)
        {
            return _dll.GetMonthlyDepreciationPreview(NormalizePeriodDate(periodDate));
        }

        public int InsertDepreciationRun(int assetId, DateTime periodDate, decimal openingWdv, decimal depAmount, decimal closingWdv, int? voucherId, int runBy, DateTime runAt)
        {
            return _dll.InsertDepreciationRun(assetId, NormalizePeriodDate(periodDate), openingWdv, depAmount, closingWdv, voucherId, runBy, runAt);
        }

        public int UpdateAssetDepreciationState(int assetId, decimal accumulatedDepreciation, decimal currentWdv, DateTime lastDepDate, string status)
        {
            return _dll.UpdateAssetDepreciationState(assetId, accumulatedDepreciation, currentWdv, NormalizePeriodDate(lastDepDate), status);
        }

        public List<DepScheduleLine> GetDepreciationHistory(int assetId)
        {
            DataTable table = _dll.GetDepreciationHistory(assetId);
            List<DepScheduleLine> history = new List<DepScheduleLine>();

            if (table == null || table.Rows.Count == 0)
                return history;

            foreach (DataRow row in table.Rows)
            {
                history.Add(new DepScheduleLine
                {
                    AssetId = GetInt(row, "asset_id"),
                    AssetCode = GetString(row, "asset_code"),
                    AssetName = GetString(row, "asset_name"),
                    DepMethod = GetString(row, "dep_method", "STRAIGHT_LINE"),
                    PeriodDate = GetDateTime(row, "period_date"),
                    OpeningWDV = GetDecimal(row, "opening_wdv"),
                    DepreciationAmount = GetDecimal(row, "depreciation_amount"),
                    AccumulatedDepreciation = GetDecimal(row, "accumulated_depreciation"),
                    ClosingWDV = GetDecimal(row, "closing_wdv")
                });
            }

            return history;
        }

        public static DateTime NormalizePeriodDate(DateTime periodDate)
        {
            return new DateTime(periodDate.Year, periodDate.Month, DateTime.DaysInMonth(periodDate.Year, periodDate.Month));
        }

        private static List<FixedAssetModel> MapAssets(DataTable table)
        {
            List<FixedAssetModel> assets = new List<FixedAssetModel>();
            if (table == null)
            {
                return assets;
            }

            foreach (DataRow row in table.Rows)
            {
                assets.Add(MapAsset(row));
            }

            return assets;
        }

        private static FixedAssetModel MapAsset(DataRow row)
        {
            FixedAssetModel asset = new FixedAssetModel();
            asset.AssetId = GetInt(row, "asset_id");
            asset.AssetCode = GetString(row, "asset_code");
            asset.AssetName = GetString(row, "asset_name");
            asset.CategoryId = GetInt(row, "category_id", GetInt(row, "cat_id"));
            asset.CategoryName = GetString(row, "category_name", GetString(row, "cat_name"));
            asset.LocationId = GetInt(row, "location_id");
            asset.PurchaseDate = GetDateTime(row, "purchase_date");
            asset.Cost = GetDecimal(row, "cost");
            asset.ResidualValue = GetDecimal(row, "residual_value", GetDecimal(row, "salvage_value"));
            asset.UsefulLifeMonths = GetInt(row, "useful_life_months", GetInt(row, "default_useful_life", 0));
            asset.DepMethod = GetString(row, "dep_method", GetString(row, "default_dep_method", "STRAIGHT_LINE"));
            asset.DepRate = GetDecimal(row, "dep_rate", GetDecimal(row, "default_dep_rate"));
            asset.DepAccountId = GetInt(row, "dep_account_id");
            asset.AccumDepAccountId = GetInt(row, "accum_dep_account_id");
            asset.AccumulatedDepreciation = GetDecimal(row, "accumulated_depreciation");
            asset.CurrentWDV = GetDecimal(row, "current_wdv", asset.Cost - asset.AccumulatedDepreciation);
            asset.Status = GetString(row, "status", "Active");
            asset.DisposalDate = GetNullableDateTime(row, "disposal_date");
            asset.DisposalProceeds = GetDecimal(row, "disposal_proceeds");
            asset.LastDepDate = GetNullableDateTime(row, "last_dep_date");
            asset.CreatedBy = GetInt(row, "created_by");
            asset.CreatedAt = GetDateTime(row, "created_at", DateTime.Now);
            asset.DepreciationStartDate = GetNullableDateTime(row, "depreciation_start_date", asset.PurchaseDate);
            asset.ReplacementCost = GetNullableDecimal(row, "replacement_cost");
            asset.IsActive = GetBool(row, "is_active", true);
            asset.IsDisposed = GetBool(row, "is_disposed", false);
            return asset;
        }

        private static string GetString(DataRow row, string columnName, string defaultValue = null)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return Convert.ToString(row[columnName]);
            }

            return defaultValue;
        }

        private static int GetInt(DataRow row, string columnName, int defaultValue = 0)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return Convert.ToInt32(row[columnName]);
            }

            return defaultValue;
        }

        private static int? GetNullableInt(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return Convert.ToInt32(row[columnName]);
            }

            return null;
        }

        private static decimal GetDecimal(DataRow row, string columnName, decimal defaultValue = 0m)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return Convert.ToDecimal(row[columnName]);
            }

            return defaultValue;
        }

        private static decimal? GetNullableDecimal(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return Convert.ToDecimal(row[columnName]);
            }

            return null;
        }

        private static DateTime GetDateTime(DataRow row, string columnName)
        {
            return GetDateTime(row, columnName, DateTime.MinValue);
        }

        private static DateTime GetDateTime(DataRow row, string columnName, DateTime defaultValue)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return Convert.ToDateTime(row[columnName]);
            }

            return defaultValue;
        }

        private static DateTime? GetNullableDateTime(DataRow row, string columnName)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return Convert.ToDateTime(row[columnName]);
            }

            return null;
        }

        private static DateTime? GetNullableDateTime(DataRow row, string columnName, DateTime defaultValue)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return Convert.ToDateTime(row[columnName]);
            }

            return defaultValue;
        }

        private static bool GetBool(DataRow row, string columnName, bool defaultValue)
        {
            if (row.Table.Columns.Contains(columnName) && row[columnName] != DBNull.Value)
            {
                return Convert.ToBoolean(row[columnName]);
            }

            return defaultValue;
        }

        // ============================================================
        // Category Management
        // ============================================================

        public List<CategoryModel> GetCategories(bool activeOnly = true)
        {
            DataTable dt = _dll.GetCategories(activeOnly);
            List<CategoryModel> categories = new List<CategoryModel>();

            foreach (DataRow row in dt.Rows)
            {
                categories.Add(new CategoryModel
                {
                    CategoryId = GetInt(row, "category_id", 0),
                    CategoryCode = GetString(row, "category_code", ""),
                    CategoryName = GetString(row, "category_name", ""),
                    DepreciationMethod = GetString(row, "depreciation_method", "STRAIGHT_LINE"),
                    UsefulLifeMonths = GetInt(row, "useful_life_months", 60),
                    AnnualDepreciationRate = GetDecimal(row, "annual_depreciation_rate", 0),
                    IsActive = GetBool(row, "is_active", true)
                });
            }

            return categories;
        }

        public int InsertCategory(string categoryCode, string categoryName, string deprecationMethod = "STRAIGHT_LINE", int usefulLifeMonths = 60, decimal? annualDepreciationRate = null)
        {
            if (string.IsNullOrWhiteSpace(categoryCode))
                throw new ArgumentException("Category code is required.");
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name is required.");
            if (usefulLifeMonths <= 0)
                throw new ArgumentException("Useful life months must be greater than 0.");

            return _dll.InsertCategory(categoryCode.Trim(), categoryName.Trim(), deprecationMethod, usefulLifeMonths, annualDepreciationRate);
        }

        public int UpdateCategory(int categoryId, string categoryName, string deprecationMethod, int usefulLifeMonths, decimal? annualDepreciationRate = null, bool isActive = true)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Category ID is invalid.");
            if (string.IsNullOrWhiteSpace(categoryName))
                throw new ArgumentException("Category name is required.");
            if (usefulLifeMonths <= 0)
                throw new ArgumentException("Useful life months must be greater than 0.");

            return _dll.UpdateCategory(categoryId, categoryName.Trim(), deprecationMethod, usefulLifeMonths, annualDepreciationRate, isActive);
        }

        public int DeleteCategory(int categoryId)
        {
            if (categoryId <= 0)
                throw new ArgumentException("Category ID is invalid.");

            return _dll.DeleteCategory(categoryId);
        }

        // ============================================================
        // Location Management
        // ============================================================

        public List<LocationModel> GetLocations(bool activeOnly = true)
        {
            DataTable dt = _dll.GetLocations(activeOnly);
            List<LocationModel> locations = new List<LocationModel>();

            foreach (DataRow row in dt.Rows)
            {
                locations.Add(new LocationModel
                {
                    LocationId = GetInt(row, "location_id", 0),
                    LocationCode = GetString(row, "location_code", ""),
                    LocationName = GetString(row, "location_name", ""),
                    LocationType = GetString(row, "location_type", "LOCATION"),
                    ParentLocationId = GetNullableInt(row, "parent_location_id"),
                    IsActive = GetBool(row, "is_active", true)
                });
            }

            return locations;
        }

        public int InsertLocation(string locationCode, string locationName, string locationType = "LOCATION", int? parentLocationId = null)
        {
            if (string.IsNullOrWhiteSpace(locationCode))
                throw new ArgumentException("Location code is required.");
            if (string.IsNullOrWhiteSpace(locationName))
                throw new ArgumentException("Location name is required.");

            return _dll.InsertLocation(locationCode.Trim(), locationName.Trim(), locationType, parentLocationId);
        }

        public int UpdateLocation(int locationId, string locationName, string locationType, int? parentLocationId = null, bool isActive = true)
        {
            if (locationId <= 0)
                throw new ArgumentException("Location ID is invalid.");
            if (string.IsNullOrWhiteSpace(locationName))
                throw new ArgumentException("Location name is required.");

            return _dll.UpdateLocation(locationId, locationName.Trim(), locationType, parentLocationId, isActive);
        }

        public int DeleteLocation(int locationId)
        {
            if (locationId <= 0)
                throw new ArgumentException("Location ID is invalid.");

            return _dll.DeleteLocation(locationId);
        }

        // ============================================================
        // Asset Management
        // ============================================================

        public int InsertAsset(string assetCode, string assetName, int categoryId, int? locationId = null, string serialNumber = null, DateTime? purchaseDate = null, decimal cost = 0, string depMethod = "STRAIGHT_LINE", int usefulLifeMonths = 60, decimal salvageValue = 0, decimal? replacementCost = null, string notes = null, int? createdBy = null)
        {
            if (string.IsNullOrWhiteSpace(assetCode))
                throw new ArgumentException("Asset code is required.");
            if (string.IsNullOrWhiteSpace(assetName))
                throw new ArgumentException("Asset name is required.");
            if (categoryId <= 0)
                throw new ArgumentException("Category is required.");
            if (cost < 0)
                throw new ArgumentException("Cost cannot be negative.");
            if (usefulLifeMonths <= 0)
                throw new ArgumentException("Useful life months must be greater than 0.");

            return _dll.InsertAsset(assetCode.Trim(), assetName.Trim(), categoryId, locationId, serialNumber, purchaseDate, cost, depMethod, usefulLifeMonths, salvageValue, replacementCost, notes, createdBy);
        }

        public int UpdateAssetDetails(int assetId, string assetName, int? locationId = null, string notes = null, bool isActive = true)
        {
            if (assetId <= 0)
                throw new ArgumentException("Asset ID is invalid.");
            if (string.IsNullOrWhiteSpace(assetName))
                throw new ArgumentException("Asset name is required.");

            return _dll.UpdateAssetDetails(assetId, assetName.Trim(), locationId, notes, isActive);
        }
    }
}
