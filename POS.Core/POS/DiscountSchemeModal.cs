using System;

namespace POS.Core
{
    /// <summary>
    /// Represents a discount scheme stored in pos_discount_schemes.
    /// The FK link to a product is stored on pos_products.discount_scheme_id.
    /// </summary>
    public class DiscountSchemeModal
    {
        public int id { get; set; }

        public string name { get; set; }
        public string name_ar { get; set; }

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

        public string CalcType { get; set; }

        public double DiscountValue { get; set; }

        public double DiscountPercent { get; set; }

        public static DiscountResultModal None() => new DiscountResultModal
        {
            DiscountValue = 0,
            DiscountPercent = 0
        };
    }
}
