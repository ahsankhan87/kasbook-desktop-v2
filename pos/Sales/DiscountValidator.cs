using POS.Core;
using System.Windows.Forms;

namespace pos
{
    /// <summary>
    /// Helper class for discount validation against user limits
    /// </summary>
    public static class DiscountValidator
    {
        /// <summary>
        /// Validates if the discount amount/percent is within the user's limit
        /// </summary>
        /// <param name="discountAmount">The discount amount being applied</param>
        /// <param name="lineItemTotal">The line item total before discount</param>
        /// <param name="maxDiscountPercent">Max discount % allowed for user (0 = no limit)</param>
        /// <param name="maxDiscountAmount">Max discount amount allowed for user (0 = no limit)</param>
        /// <returns>True if valid, False if exceeds limits</returns>
        public static bool IsDiscountValid(double discountAmount, double lineItemTotal, 
            double maxDiscountPercent, double maxDiscountAmount)
        {
            // If no limits set, allow any discount
            if (maxDiscountPercent == 0 && maxDiscountAmount == 0)
                return true;

            // Calculate discount percent
            double discountPercent = lineItemTotal == 0 ? 0 : (discountAmount / lineItemTotal) * 100;

            // Check percent limit
            if (maxDiscountPercent > 0 && discountPercent > maxDiscountPercent)
            {
                MessageBox.Show(
                    $"Discount exceeds maximum allowed percentage.\n\nMax: {maxDiscountPercent}%\nAttempted: {discountPercent:F2}%",
                    "Discount Limit Exceeded",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            // Check amount limit
            if (maxDiscountAmount > 0 && discountAmount > maxDiscountAmount)
            {
                MessageBox.Show(
                    $"Discount exceeds maximum allowed amount.\n\nMax: {maxDiscountAmount:F2}\nAttempted: {discountAmount:F2}",
                    "Discount Limit Exceeded",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the effective discount limit for a user
        /// </summary>
        public static string GetDiscountLimitDescription(double maxDiscountPercent, double maxDiscountAmount)
        {
            if (maxDiscountPercent == 0 && maxDiscountAmount == 0)
                return "No discount limit set";

            if (maxDiscountPercent > 0 && maxDiscountAmount > 0)
                return $"Max {maxDiscountPercent}% or {maxDiscountAmount:F2}";

            if (maxDiscountPercent > 0)
                return $"Max {maxDiscountPercent}%";

            return $"Max {maxDiscountAmount:F2}";
        }
    }
}
