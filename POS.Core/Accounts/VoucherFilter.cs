using System;

namespace POS.Core
{
    public class VoucherFilter
    {
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string VoucherType { get; set; }
        public string Status { get; set; }
        public string Search { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 50;
        public int? PeriodId { get; set; }
        public int? CreatedBy { get; set; }
        public int? PostedBy { get; set; }
    }
}