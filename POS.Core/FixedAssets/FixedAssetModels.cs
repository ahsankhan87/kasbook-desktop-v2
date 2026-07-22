using System;
using System.Collections.Generic;

namespace POS.Core
{
    public class FixedAssetModel
    {
        public int AssetId { get; set; }

        public string AssetCode { get; set; }

        public string AssetName { get; set; }

        public string AssetDescription { get; set; }

        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public int LocationId { get; set; }

        public string LocationName { get; set; }

        public DateTime PurchaseDate { get; set; }

        public string PurchaseInvoiceNo { get; set; }

        public string SerialNumber { get; set; }

        public string ModelNumber { get; set; }

        public decimal Cost { get; set; }

        public decimal ResidualValue { get; set; }

        public int UsefulLifeMonths { get; set; }

        public decimal UsefulLifeYears
        {
            get { return UsefulLifeMonths > 0 ? (decimal)UsefulLifeMonths / 12 : 0; }
        }

        public string DepMethod { get; set; }

        public string DepreciationMethod
        {
            get { return DepMethod; }
            set { DepMethod = value; }
        }

        public decimal DepRate { get; set; }

        public decimal DepreciationRate
        {
            get { return DepRate; }
            set { DepRate = value; }
        }

        public int DepAccountId { get; set; }

        public int AccumDepAccountId { get; set; }

        public decimal AccumulatedDepreciation { get; set; }

        public decimal CurrentWDV { get; set; }

        public string Status { get; set; }

        public DateTime? DisposalDate { get; set; }

        public decimal DisposalProceeds { get; set; }

        public DateTime? LastDepDate { get; set; }

        public int CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? DepreciationStartDate { get; set; }

        public DateTime StartDepreciationFrom
        {
            get { return DepreciationStartDate ?? PurchaseDate; }
            set { DepreciationStartDate = value; }
        }

        public decimal? ReplacementCost { get; set; }

        public bool IsActive { get; set; }

        public bool IsDisposed { get; set; }

        public List<DepScheduleLine> ScheduleLines { get; set; } = new List<DepScheduleLine>();
    }

    public class DepScheduleLine
    {
        public int AssetId { get; set; }

        public string AssetCode { get; set; }

        public string AssetName { get; set; }

        public string DepMethod { get; set; }

        public DateTime PeriodDate { get; set; }

        public decimal OpeningWDV { get; set; }

        public decimal DepreciationAmount { get; set; }

        public decimal AccumulatedDepreciation { get; set; }

        public decimal ClosingWDV { get; set; }

        public int? VoucherId { get; set; }
    }

    public class DepreciationRunSummary
    {
        public DateTime PeriodDate { get; set; }

        public int EvaluatedAssets { get; set; }

        public int PostedCount { get; set; }

        public int SkippedCount { get; set; }

        public int ErrorCount { get; set; }

        public decimal TotalDepreciation { get; set; }

        public List<string> Messages { get; set; } = new List<string>();

        public List<DepScheduleLine> PostedLines { get; set; } = new List<DepScheduleLine>();
    }

    public class CategoryModel
    {
        public int CategoryId { get; set; }

        public string CategoryCode { get; set; }

        public string CategoryName { get; set; }

        public string DepreciationMethod { get; set; }

        public int UsefulLifeMonths { get; set; }

        public decimal AnnualDepreciationRate { get; set; }

        public bool IsActive { get; set; }
    }

    public class LocationModel
    {
        public int LocationId { get; set; }

        public string LocationCode { get; set; }

        public string LocationName { get; set; }

        public string LocationType { get; set; }

        public int? ParentLocationId { get; set; }

        public bool IsActive { get; set; }
    }
}

