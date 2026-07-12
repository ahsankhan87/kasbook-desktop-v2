using System;
using System.Collections.Generic;
using System.Data;

namespace pos.Reports.Financial
{
    public sealed class BalanceNote
    {
        public string Title { get; set; }
        public string Summary { get; set; }
        public List<string> Details { get; } = new List<string>();
    }

    public sealed class BalanceSheetReportModel
    {
        public DataTable Assets { get; } = CreateLineTable();
        public DataTable LiabilitiesAndEquity { get; } = CreateLineTable();
        public DataTable Diagnostics { get; } = CreateDiagnosticsTable();
        public List<BalanceNote> Notes { get; } = new List<BalanceNote>();
        public decimal AssetsTotal { get; set; }
        public decimal LiabilitiesAndEquityTotal { get; set; }
        public decimal Difference { get; set; }
        public bool IsBalanced => Math.Abs(Difference) < 0.01m;
        public DateTime AsOfDate { get; set; }
        public DateTime? ComparisonDate { get; set; }

        private static DataTable CreateLineTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("LineItem", typeof(string));
            dt.Columns.Add("Amount", typeof(decimal));
            dt.Columns.Add("Comparison", typeof(decimal));
            dt.Columns.Add("Notes", typeof(string));
            dt.Columns.Add("IsTotal", typeof(bool));
            dt.Columns.Add("IsHeading", typeof(bool));
            dt.Columns.Add("IndentLevel", typeof(int));
            return dt;
        }

        private static DataTable CreateDiagnosticsTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("GroupName", typeof(string));
            dt.Columns.Add("CurrentAmount", typeof(decimal));
            dt.Columns.Add("ComparisonAmount", typeof(decimal));
            dt.Columns.Add("Difference", typeof(decimal));
            dt.Columns.Add("Notes", typeof(string));
            return dt;
        }
    }
}
