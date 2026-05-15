using System;
using System.Windows.Forms;

namespace pos.Sales.Helpers
{
    /// <summary>
    /// Helper class for sales calculation operations.
    /// Extracts calculation logic from frm_sales to improve maintainability.
    /// </summary>
    public static class SalesCalculationHelper
    {
        /// <summary>
        /// Calculates the sub-total amount by summing qty * unit_price for all rows.
        /// </summary>
        public static double GetSubTotalAmount(DataGridView grid_sales)
        {
            double total_sub_total = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                if (grid_sales.Rows[i].Cells["qty"].Value != null && 
                    grid_sales.Rows[i].Cells["unit_price"].Value != null)
                {
                    total_sub_total += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * 
                                      Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value);
                }
            }

            return total_sub_total;
        }

        /// <summary>
        /// Calculates the total cost amount and cost amount including VAT.
        /// </summary>
        /// <returns>Tuple containing (total_cost_amount, total_cost_amount_e_vat)</returns>
        public static Tuple<double, double> GetTotalCostAmount(DataGridView grid_sales)
        {
            double total_cost_amount = 0;
            double total_cost_amount_e_vat = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                //if tax is not assigned with product then assign zero
                double tax_rate = 0;
                if (grid_sales.Rows[i].Cells["tax_rate"].Value == null || 
                    grid_sales.Rows[i].Cells["tax_rate"].Value == DBNull.Value || 
                    String.IsNullOrWhiteSpace(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()))
                {
                    tax_rate = 0;
                }
                else
                {
                    tax_rate = double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString());
                }

                double total_cost = Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * 
                                   Convert.ToDouble(grid_sales.Rows[i].Cells["cost_price"].Value);

                total_cost_amount += total_cost;
                total_cost_amount_e_vat += (total_cost + (total_cost * tax_rate / 100));
            }

            return Tuple.Create(total_cost_amount, total_cost_amount_e_vat);
        }

        /// <summary>
        /// Calculates the total amount by summing qty * unit_price for all rows.
        /// </summary>
        public static double GetTotalAmount(DataGridView grid_sales)
        {
            double total_amount = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                if (grid_sales.Rows[i].Cells["qty"].Value != null && 
                    grid_sales.Rows[i].Cells["unit_price"].Value != null)
                {
                    total_amount += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * 
                                   Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value);
                }
            }

            return total_amount;
        }

        /// <summary>
        /// Calculates the net amount after applying tax and discount.
        /// </summary>
        public static double CalculateNetAmount(double total_amount, double total_tax, double total_discount)
        {
            return Math.Round((total_amount + total_tax - total_discount), 2);
        }

        /// <summary>
        /// Calculates the total tax by summing tax values for all rows.
        /// </summary>
        public static double GetTotalTax(DataGridView grid_sales)
        {
            double total_tax = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                if (grid_sales.Rows[i].Cells["tax"].Value != null)
                {
                    total_tax += Convert.ToDouble(grid_sales.Rows[i].Cells["tax"].Value);
                }
            }

            return total_tax;
        }

        /// <summary>
        /// Calculates the total discount by summing discount values for all rows.
        /// </summary>
        public static double GetTotalDiscount(DataGridView grid_sales, double flatDiscountValue = 0, double flatDiscountPercent = 0, double total_amount = 0)
        {
            double total_discount = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                if (grid_sales.Rows[i].Cells["discount"].Value != null)
                {
                    total_discount += Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);
                }
            }
            
            total_discount += flatDiscountValue;
            total_discount += flatDiscountPercent;

            return total_discount;
        }

        /// <summary>
        /// Calculates the total quantity by summing qty values for all rows.
        /// </summary>
        public static double GetTotalQty(DataGridView grid_sales)
        {
            double total_qty = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                if (grid_sales.Rows[i].Cells["qty"].Value != null)
                {
                    total_qty += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);
                }
            }

            return total_qty;
        }

        /// <summary>
        /// Validates if customer credit limit would be exceeded.
        /// </summary>
        /// <returns>Tuple containing (isValid, limitExceededBy)</returns>
        public static Tuple<bool, double> ValidateCreditLimit(string sale_type, double customer_credit_limit, 
            double customerBalance, double netAmount)
        {
            if (string.IsNullOrEmpty(sale_type) || sale_type != "Credit")
            {
                return Tuple.Create(true, 0.0);
            }

            double netCreditLimit = customer_credit_limit - customerBalance;
            double limitExceededBy = netAmount - netCreditLimit;

            if (customer_credit_limit > 0 && netAmount > netCreditLimit)
            {
                return Tuple.Create(false, limitExceededBy);
            }

            return Tuple.Create(true, 0.0);
        }
    }
}
