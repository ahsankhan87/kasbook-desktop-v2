using System;
using System.Data;
using POS.BLL.FixedAssets;
using POS.Core;

namespace pos.Reports.FixedAssets
{
    public class DepreciationProjectionReportGenerator
    {
        private readonly FixedAssetBLL _assetBll;
        private readonly DepreciationEngine _engine;

        public DepreciationProjectionReportGenerator()
        {
            _assetBll = new FixedAssetBLL();
            _engine = new DepreciationEngine();
        }

        public DataTable Build(DateTime futureDate)
        {
            DateTime target = FixedAssetBLL.NormalizePeriodDate(futureDate);
            DataTable dt = new DataTable();
            dt.Columns.Add("Asset Code", typeof(string));
            dt.Columns.Add("Asset Name", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Method", typeof(string));
            dt.Columns.Add("Current WDV", typeof(decimal));
            dt.Columns.Add("Projected Depreciation", typeof(decimal));
            dt.Columns.Add("Projected WDV", typeof(decimal));
            dt.Columns.Add("Projection Date", typeof(DateTime));

            var assets = _assetBll.GetAllAssets();
            foreach (FixedAssetModel asset in assets)
            {
                decimal currentWdv = asset.CurrentWDV > 0m ? asset.CurrentWDV : Math.Max(asset.ResidualValue, asset.Cost - asset.AccumulatedDepreciation);
                decimal projectedDep = 0m;
                decimal projectedWdv = currentWdv;

                if (!string.Equals((asset.DepMethod ?? string.Empty).Trim(), "NO_DEPRECIATION", StringComparison.OrdinalIgnoreCase)
                    && asset.UsefulLifeMonths > 0
                    && target >= asset.PurchaseDate.Date)
                {
                    FixedAssetModel temp = CloneAsset(asset, currentWdv);
                    DateTime startPeriod = asset.LastDepDate.HasValue
                        ? FixedAssetBLL.NormalizePeriodDate(asset.LastDepDate.Value).AddMonths(1)
                        : FixedAssetBLL.NormalizePeriodDate(asset.PurchaseDate);

                    if (startPeriod < FixedAssetBLL.NormalizePeriodDate(asset.PurchaseDate))
                    {
                        startPeriod = FixedAssetBLL.NormalizePeriodDate(asset.PurchaseDate);
                    }

                    int guard = 0;
                    for (DateTime period = startPeriod; period <= target && guard < 2400; period = period.AddMonths(1), guard++)
                    {
                        decimal dep = _engine.CalculateAssetDepreciationForProjection(temp, period);
                        if (dep <= 0m)
                        {
                            continue;
                        }

                        projectedDep += dep;
                        temp.AccumulatedDepreciation += dep;
                        temp.CurrentWDV = Math.Max(temp.ResidualValue, temp.CurrentWDV - dep);
                        projectedWdv = temp.CurrentWDV;

                        if (projectedWdv <= temp.ResidualValue)
                        {
                            break;
                        }
                    }
                }

                DataRow row = dt.NewRow();
                row["Asset Code"] = asset.AssetCode;
                row["Asset Name"] = asset.AssetName;
                row["Category"] = asset.CategoryName;
                row["Method"] = asset.DepMethod;
                row["Current WDV"] = currentWdv;
                row["Projected Depreciation"] = Math.Round(projectedDep, 2, MidpointRounding.AwayFromZero);
                row["Projected WDV"] = Math.Round(projectedWdv, 2, MidpointRounding.AwayFromZero);
                row["Projection Date"] = target;
                dt.Rows.Add(row);
            }

            return dt;
        }

        private static FixedAssetModel CloneAsset(FixedAssetModel source, decimal currentWdv)
        {
            return new FixedAssetModel
            {
                AssetId = source.AssetId,
                AssetCode = source.AssetCode,
                AssetName = source.AssetName,
                PurchaseDate = source.PurchaseDate,
                Cost = source.Cost,
                ResidualValue = source.ResidualValue,
                UsefulLifeMonths = source.UsefulLifeMonths,
                DepMethod = source.DepMethod,
                DepRate = source.DepRate,
                AccumulatedDepreciation = source.AccumulatedDepreciation,
                CurrentWDV = currentWdv,
                LastDepDate = source.LastDepDate,
                Status = source.Status
            };
        }
    }
}
