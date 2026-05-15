using POS.Core;
using POS.DLL;
using System;
using System.Data;

namespace POS.BLL
{
    public class DiscountEngineBLL
    {
        private readonly DiscountSchemesDLL _dll = new DiscountSchemesDLL();

        /// <summary>
        /// Resolves discount for a sales line using the scheme directly assigned to the product.
        /// </summary>
        public DiscountResultModal ResolveItemDiscount(
            int? discountSchemeId,
            double qty,
            double unitPrice)
        {
            double lineTotal = qty * unitPrice;
            if (lineTotal <= 0 || discountSchemeId == null || discountSchemeId <= 0)
                return DiscountResultModal.None();

            DataTable dt = _dll.GetActiveById(discountSchemeId.Value);
            if (dt == null || dt.Rows.Count == 0)
                return DiscountResultModal.None();

            DataRow row = dt.Rows[0];
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
                CalcType = calcType,
                DiscountValue = discountAmount,
                DiscountPercent = discountPercent
            };
        }
    }
}
