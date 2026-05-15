using System;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using POS.Core;

namespace pos.Sales.Helpers
{
    /// <summary>
    /// Helper class for sales dropdown/fill operations.
    /// Extracts dropdown population logic from frm_sales to improve maintainability.
    /// </summary>
    public static class SalesDropdownHelper
    {
        /// <summary>
        /// Populates the customers dropdown list.
        /// </summary>
        public static void PopulateCustomersDropdown(ComboBox cmb_customers)
        {
            CustomerBLL customerBLL_obj = new CustomerBLL();
            DataTable dt = customerBLL_obj.GetRecord("TOP 100 *", "pos_customers");
            
            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["name"] = "--Select--";
            dt.Rows.InsertAt(dr, 0);

            cmb_customers.DataSource = dt;
            cmb_customers.DisplayMember = "name";
            cmb_customers.ValueMember = "id";
        }

        /// <summary>
        /// Populates the employees dropdown list.
        /// </summary>
        public static void PopulateEmployeesDropdown(ComboBox cmb_employees)
        {
            EmployeeBLL emp_Obj = new EmployeeBLL();
            DataTable dt = emp_Obj.GetRecord("*", "pos_employees");
            
            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["name"] = "--Select--";
            dt.Rows.InsertAt(dr, 0);

            cmb_employees.DataSource = dt;
            cmb_employees.DisplayMember = "name";
            cmb_employees.ValueMember = "id";
        }

        /// <summary>
        /// Populates the payment terms dropdown list.
        /// </summary>
        public static void PopulatePaymentTermsDropdown(ComboBox cmb_payment_terms)
        {
            PaymentTermsBLL paymentTermsBLL_obj = new PaymentTermsBLL();
            DataTable dt = paymentTermsBLL_obj.GetRecord("*", "pos_payment_terms");
            
            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["name"] = "--Select--";
            dt.Rows.InsertAt(dr, 0);

            cmb_payment_terms.DataSource = dt;
            cmb_payment_terms.DisplayMember = "name";
            cmb_payment_terms.ValueMember = "id";
        }

        /// <summary>
        /// Populates the payment method dropdown list.
        /// </summary>
        public static void PopulatePaymentMethodsDropdown(ComboBox cmb_payment_method)
        {
            PaymentMethodBLL paymentMethodBLL_obj = new PaymentMethodBLL();
            DataTable dt = paymentMethodBLL_obj.GetRecord("*", "pos_payment_methods");
            
            DataRow dr = dt.NewRow();
            dr["id"] = 0;
            dr["name"] = "--Select--";
            dt.Rows.InsertAt(dr, 0);

            cmb_payment_method.DataSource = dt;
            cmb_payment_method.DisplayMember = "name";
            cmb_payment_method.ValueMember = "id";
        }

        /// <summary>
        /// Populates the sale type dropdown list.
        /// </summary>
        public static void PopulateSaleTypeDropdown(ComboBox cmb_sale_type)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("name", typeof(string));

            dt.Rows.Add("0", "--Select--");
            dt.Rows.Add("Cash", "Cash");
            dt.Rows.Add("Credit", "Credit");
            dt.Rows.Add("Return", "Return");
            dt.Rows.Add("ICT", "ICT");
            dt.Rows.Add("Quotation", "Quotation");

            cmb_sale_type.DataSource = dt;
            cmb_sale_type.DisplayMember = "name";
            cmb_sale_type.ValueMember = "id";
            cmb_sale_type.SelectedValue = "0";
        }

        /// <summary>
        /// Populates the invoice subtype dropdown list for ZATCA compliance.
        /// </summary>
        public static void PopulateInvoiceSubtypeDropdown(ComboBox cmb_invoice_subtype_code, bool useZatcaEInvoice)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("code", typeof(string));
            dt.Columns.Add("name", typeof(string));

            if (useZatcaEInvoice)
            {
                dt.Rows.Add("01", "Standard Invoice (Tax Invoice)");
                dt.Rows.Add("02", "Simplified Invoice");
            }
            else
            {
                dt.Rows.Add("02", "Simplified Invoice");
            }

            cmb_invoice_subtype_code.DataSource = dt;
            cmb_invoice_subtype_code.DisplayMember = "name";
            cmb_invoice_subtype_code.ValueMember = "code";
            cmb_invoice_subtype_code.SelectedValue = "02";
        }

        /// <summary>
        /// Gets account IDs from company settings.
        /// </summary>
        public static CompanyAccounts GetCompanyAccountIds()
        {
            GeneralBLL objBLL = new GeneralBLL();
            DataTable companies_dt = objBLL.GetRecord("TOP 1 *", "pos_companies");
            
            var accounts = new CompanyAccounts();
            
            foreach (DataRow dr in companies_dt.Rows)
            {
                accounts.CashAccountId = (int)dr["cash_acc_id"];
                accounts.SalesAccountId = (int)dr["sales_acc_id"];
                accounts.ReceivableAccountId = (int)dr["receivable_acc_id"];
                accounts.TaxAccountId = (int)dr["tax_acc_id"];
                accounts.SalesDiscountAccId = (int)dr["sales_discount_acc_id"];
                accounts.InventoryAccId = (int)dr["inventory_acc_id"];
                accounts.PurchasesAccId = (int)dr["purchases_acc_id"];
                
                // Optional: cash sales amount limit and credit sales settings
                if (dr.Table.Columns.Contains("cash_sales_amount_limit"))
                {
                    accounts.CashSalesAmountLimit = Convert.ToDouble(dr["cash_sales_amount_limit"]);
                }
                
                if (dr.Table.Columns.Contains("allow_credit_sales"))
                {
                    accounts.AllowCreditSales = Convert.ToBoolean(dr["allow_credit_sales"]);
                }
            }
            
            return accounts;
        }

        /// <summary>
        /// Fills the locations grid combo for a specific row.
        /// </summary>
        public static void FillLocationsGridCombo(DataGridView grid_sales, int rowIndex, string product_id, string selectedValue = "DEF")
        {
            LocationsBLL locationBLL_obj = new LocationsBLL();
            DataTable dt = locationBLL_obj.GetRecord("*", "pos_locations");
            
            DataGridViewComboBoxCell cell = (DataGridViewComboBoxCell)grid_sales.Rows[rowIndex].Cells["location_id"];
            cell.DataSource = dt;
            cell.DisplayMember = "name";
            cell.ValueMember = "id";
            cell.Value = selectedValue;
        }
    }

    /// <summary>
    /// Data class to hold company account configuration.
    /// </summary>
    public class CompanyAccounts
    {
        public int CashAccountId { get; set; }
        public int SalesAccountId { get; set; }
        public int ReceivableAccountId { get; set; }
        public int TaxAccountId { get; set; }
        public int SalesDiscountAccId { get; set; }
        public int InventoryAccId { get; set; }
        public int PurchasesAccId { get; set; }
        public double CashSalesAmountLimit { get; set; }
        public bool AllowCreditSales { get; set; }
    }
}
