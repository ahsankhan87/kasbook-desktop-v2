using System;
using System.Data;
using System.Linq;
using POS.BLL.FixedAssets;
using POS.Core;

namespace pos.Reports.FixedAssets
{
    public class FixedAssetScheduleReportGenerator
    {
        private readonly FixedAssetBLL _assetBll;

        public FixedAssetScheduleReportGenerator()
        {
            _assetBll = new FixedAssetBLL();
        }

        public DataTable Build(DateTime asOfDate)
        {
            DateTime asOf = asOfDate.Date;
            DateTime yearStart = new DateTime(asOf.Year, 1, 1);

            DataTable dt = CreateSchema();
            var assets = _assetBll.GetAllAssets();

            var groups = assets
                .OrderBy(a => a.CategoryName)
                .ThenBy(a => a.AssetCode)
                .GroupBy(a => string.IsNullOrWhiteSpace(a.CategoryName) ? "Uncategorized" : a.CategoryName);

            decimal grandOpening = 0m;
            decimal grandAdditions = 0m;
            decimal grandDisposals = 0m;
            decimal grandDep = 0m;
            decimal grandClosing = 0m;

            foreach (var categoryGroup in groups)
            {
                decimal catOpening = 0m;
                decimal catAdditions = 0m;
                decimal catDisposals = 0m;
                decimal catDep = 0m;
                decimal catClosing = 0m;

                foreach (FixedAssetModel asset in categoryGroup)
                {
                    decimal opening = asset.PurchaseDate.Date < yearStart ? asset.Cost : 0m;
                    decimal additions = asset.PurchaseDate.Date >= yearStart && asset.PurchaseDate.Date <= asOf ? asset.Cost : 0m;

                    bool disposedAsOf = (asset.DisposalDate.HasValue && asset.DisposalDate.Value.Date <= asOf) || asset.IsDisposed;
                    decimal disposals = disposedAsOf ? asset.Cost : 0m;

                    decimal depreciation = Math.Max(0m, asset.AccumulatedDepreciation);
                    decimal closing = Math.Max(asset.ResidualValue, asset.CurrentWDV);

                    DataRow row = dt.NewRow();
                    row["RowType"] = "DETAIL";
                    row["Category"] = categoryGroup.Key;
                    row["AssetCode"] = asset.AssetCode;
                    row["AssetName"] = asset.AssetName;
                    row["OpeningCost"] = opening;
                    row["Additions"] = additions;
                    row["Disposals"] = disposals;
                    row["Depreciation"] = depreciation;
                    row["ClosingWDV"] = closing;
                    row["Status"] = asset.Status;
                    dt.Rows.Add(row);

                    catOpening += opening;
                    catAdditions += additions;
                    catDisposals += disposals;
                    catDep += depreciation;
                    catClosing += closing;
                }

                AddTotalRow(dt, "CATEGORY_TOTAL", categoryGroup.Key + " Total", catOpening, catAdditions, catDisposals, catDep, catClosing);

                grandOpening += catOpening;
                grandAdditions += catAdditions;
                grandDisposals += catDisposals;
                grandDep += catDep;
                grandClosing += catClosing;
            }

            AddTotalRow(dt, "GRAND_TOTAL", "Grand Total", grandOpening, grandAdditions, grandDisposals, grandDep, grandClosing);
            return dt;
        }

        private static DataTable CreateSchema()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("RowType", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("AssetCode", typeof(string));
            dt.Columns.Add("AssetName", typeof(string));
            dt.Columns.Add("OpeningCost", typeof(decimal));
            dt.Columns.Add("Additions", typeof(decimal));
            dt.Columns.Add("Disposals", typeof(decimal));
            dt.Columns.Add("Depreciation", typeof(decimal));
            dt.Columns.Add("ClosingWDV", typeof(decimal));
            dt.Columns.Add("Status", typeof(string));
            return dt;
        }

        private static void AddTotalRow(DataTable dt, string rowType, string label, decimal opening, decimal additions, decimal disposals, decimal depreciation, decimal closing)
        {
            DataRow totalRow = dt.NewRow();
            totalRow["RowType"] = rowType;
            totalRow["Category"] = label;
            totalRow["AssetCode"] = string.Empty;
            totalRow["AssetName"] = string.Empty;
            totalRow["OpeningCost"] = opening;
            totalRow["Additions"] = additions;
            totalRow["Disposals"] = disposals;
            totalRow["Depreciation"] = depreciation;
            totalRow["ClosingWDV"] = closing;
            totalRow["Status"] = string.Empty;
            dt.Rows.Add(totalRow);
        }
    }
}
