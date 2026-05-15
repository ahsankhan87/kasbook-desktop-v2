using System;

namespace POS.Core
{
    /// <summary>
    /// Represents a product-level or brand-level discount scheme stored in pos_discount_schemes.
    /// </summary>
    public class DiscountSchemeModal
    {
        public int id { get; set; }

        public string name { get; set; }
        public string name_ar { get; set; }

        // Link target (FK-based)
        public int? product_id { get; set; }
        public int? brand_id { get; set; }
        public int? category_id { get; set; }

        /// <summary>PERCENT or AMOUNT</summary>
        public string calc_type { get; set; }

        public double value { get; set; }

        public bool is_active { get; set; }

        public DateTime? start_date { get; set; }
        public DateTime? end_date { get; set; }

        public int branch_id { get; set; }
        public int company_id { get; set; }
        public int created_by { get; set; }

        public DateTime created_at { get; set; }
        public DateTime updated_at { get; set; }
    }

    /// <summary>
    /// Carries the result of discount resolution for a single line item.
    /// </summary>
    public class DiscountResultModal
    {
        public int? SchemeId { get; set; }

        /// <summary>PRODUCT | BRAND | CATEGORY | NONE</summary>
        public string DiscountType { get; set; }

        public string CalcType { get; set; }

        public double DiscountValue { get; set; }

        public double DiscountPercent { get; set; }

        public static DiscountResultModal None() => new DiscountResultModal
        {
            DiscountType = "NONE",
            DiscountValue = 0,
            DiscountPercent = 0
        };
    }
}
