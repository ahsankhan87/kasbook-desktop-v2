using POS.Core;
using POS.DLL;
using System;
using System.Data;

namespace POS.BLL
{
    public class DiscountEngineBLL
    {
        private readonly DiscountSchemesDLL _dll = new DiscountSchemesDLL();

        public DiscountResultModal ResolveItemDiscount(
            int productId,
            int? brandId,
            int? categoryId,
            double qty,
            double unitPrice,
            int branchId)
        {
            double lineTotal = qty * unitPrice;
            if (lineTotal <= 0)
                return DiscountResultModal.None();

            DataTable schemes = _dll.GetActiveForItem(productId, brandId, categoryId, branchId);
            if (schemes == null || schemes.Rows.Count == 0)
                return DiscountResultModal.None();

            DataRow row = schemes.Rows[0];

            string discountType = row["product_id"] != DBNull.Value ? "PRODUCT"
                                 : row["brand_id"] != DBNull.Value ? "BRAND"
                                 : row["category_id"] != DBNull.Value ? "CATEGORY"
                                 : "NONE";

            string calcType = row["calc_type"].ToString();
            double schemeValue = Convert.ToDouble(row["value"]);
            int schemeId = Convert.ToInt32(row["id"]);

            double discountAmount;
            double discountPercent;

            if (calcType == "PERCENT")
            {
                discountPercent = schemeValue;
                discountAmount = Math.Round(lineTotal * schemeValue / 100.0, 4);
            }
            else
            {
                discountAmount = Math.Round(schemeValue * qty, 4);
                discountPercent = lineTotal == 0 ? 0 : Math.Round(discountAmount / lineTotal * 100.0, 4);
            }

            if (discountAmount > lineTotal)
            {
                discountAmount = lineTotal;
                discountPercent = 100;
            }

            return new DiscountResultModal
            {
                SchemeId = schemeId,
                DiscountType = discountType,
                CalcType = calcType,
                DiscountValue = discountAmount,
                DiscountPercent = discountPercent
            };
        }
    }
}
