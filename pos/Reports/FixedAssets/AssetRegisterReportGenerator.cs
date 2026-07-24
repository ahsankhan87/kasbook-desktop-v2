using System;
using System.Data;
using POS.BLL.FixedAssets;

namespace pos.Reports.FixedAssets
{
    public class AssetRegisterReportGenerator
    {
        private readonly FixedAssetBLL _assetBll;

        public AssetRegisterReportGenerator()
        {
            _assetBll = new FixedAssetBLL();
        }

        public DataTable Build()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("Asset Code", typeof(string));
            dt.Columns.Add("Asset Name", typeof(string));
            dt.Columns.Add("Category", typeof(string));
            dt.Columns.Add("Location", typeof(string));
            dt.Columns.Add("Purchase Date", typeof(DateTime));
            dt.Columns.Add("Serial No", typeof(string));
            dt.Columns.Add("Model No", typeof(string));
            dt.Columns.Add("Cost", typeof(decimal));
            dt.Columns.Add("Residual Value", typeof(decimal));
            dt.Columns.Add("Useful Life (Months)", typeof(int));
            dt.Columns.Add("Dep. Method", typeof(string));
            dt.Columns.Add("Dep. Rate %", typeof(decimal));
            dt.Columns.Add("Accum. Depreciation", typeof(decimal));
            dt.Columns.Add("Current WDV", typeof(decimal));
            dt.Columns.Add("Status", typeof(string));
            dt.Columns.Add("Verified By", typeof(string));
            dt.Columns.Add("Verification Notes", typeof(string));

            var assets = _assetBll.GetAllAssets();
            foreach (var asset in assets)
            {
                DataRow row = dt.NewRow();
                row["Asset Code"] = asset.AssetCode;
                row["Asset Name"] = asset.AssetName;
                row["Category"] = asset.CategoryName;
                row["Location"] = asset.LocationName;
                row["Purchase Date"] = asset.PurchaseDate;
                row["Serial No"] = asset.SerialNumber;
                row["Model No"] = asset.ModelNumber;
                row["Cost"] = asset.Cost;
                row["Residual Value"] = asset.ResidualValue;
                row["Useful Life (Months)"] = asset.UsefulLifeMonths;
                row["Dep. Method"] = asset.DepMethod;
                row["Dep. Rate %"] = asset.DepRate;
                row["Accum. Depreciation"] = asset.AccumulatedDepreciation;
                row["Current WDV"] = asset.CurrentWDV;
                row["Status"] = asset.Status;
                row["Verified By"] = string.Empty;
                row["Verification Notes"] = string.Empty;
                dt.Rows.Add(row);
            }

            return dt;
        }
    }
}
