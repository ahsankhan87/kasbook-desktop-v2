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
            PopulateCustomersDropdown(cmb_customers, includeAddNewRow: true);
        }

        public static void PopulateCustomersDropdown(ComboBox cmb_customers, bool includeAddNewRow)
        {
            CustomerBLL customerBLL_obj = new CustomerBLL();
            DataTable dt = customerBLL_obj.GetAll();

            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;
            emptyRow[2] = string.Empty;
            dt.Rows.InsertAt(emptyRow, 0);

            if (includeAddNewRow)
            {
                DataRow addNewRow = dt.NewRow();
                addNewRow[0] = "-1";
                addNewRow[2] = "ADD NEW";
                dt.Rows.InsertAt(addNewRow, 1);
            }

            cmb_customers.DisplayMember = "first_name";
            cmb_customers.ValueMember = "id";
            cmb_customers.DataSource = dt;
        }

        /// <summary>
        /// Populates the employees dropdown list.
        /// </summary>
        public static void PopulateEmployeesDropdown(ComboBox cmb_employees)
        {
            EmployeeBLL emp_Obj = new EmployeeBLL();
            DataTable dt = emp_Obj.GetAll();

            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;
            emptyRow[2] = "Select Employee";
            dt.Rows.InsertAt(emptyRow, 0);

            cmb_employees.DisplayMember = "first_name";
            cmb_employees.ValueMember = "id";
            cmb_employees.DataSource = dt;
        }

        /// <summary>
        /// Populates the payment terms dropdown list.
        /// </summary>
        public static void PopulatePaymentTermsDropdown(ComboBox cmb_payment_terms)
        {
            PaymentTermsBLL paymentTermsBLL_obj = new PaymentTermsBLL();
            DataTable dt = paymentTermsBLL_obj.GetAll();

            DataRow emptyRow = dt.NewRow();
            emptyRow[0] = 0;
            emptyRow[4] = string.Empty;
            dt.Rows.InsertAt(emptyRow, 0);

            cmb_payment_terms.DisplayMember = "description";
            cmb_payment_terms.ValueMember = "id";
            cmb_payment_terms.DataSource = dt;
        }

        /// <summary>
        /// Populates the payment method dropdown list.
        /// </summary>
        public static void PopulatePaymentMethodsDropdown(ComboBox cmb_payment_method)
        {
            PaymentMethodBLL paymentMethodBLL_obj = new PaymentMethodBLL();
            DataTable dt = paymentMethodBLL_obj.GetAll();

            cmb_payment_method.DisplayMember = "description";
            cmb_payment_method.ValueMember = "id";
            cmb_payment_method.DataSource = dt;
        }

        /// <summary>
        /// Populates the sale type dropdown list.
        /// </summary>
        public static void PopulateSaleTypeDropdown(ComboBox cmb_sale_type)
        {
            PopulateSaleTypeDropdown(cmb_sale_type, "en-US", false);
        }

        public static void PopulateSaleTypeDropdown(ComboBox cmb_sale_type, string lang, bool allowCreditSales)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id");
            dt.Columns.Add("name");

            dt.Rows.Add("Cash", lang == "ar-SA" ? "نقدي" : "Cash");

            if (allowCreditSales)
                dt.Rows.Add("Credit", lang == "ar-SA" ? "اجل" : "Credit");

            dt.Rows.Add("Quotation", lang == "ar-SA" ? "عرض سعر" : "Quotation");
            dt.Rows.Add("Gift", lang == "ar-SA" ? "هدية" : "Gift");
            dt.Rows.Add("ICT", lang == "ar_SA" ? "نقل قطع الغيار بين الشركات" : "ICT");

            cmb_sale_type.DisplayMember = "name";
            cmb_sale_type.ValueMember = "id";
            cmb_sale_type.DataSource = dt;
            cmb_sale_type.SelectedIndex = 0;
        }

        /// <summary>
        /// Populates the invoice subtype dropdown list for ZATCA compliance.
        /// </summary>
        public static void PopulateInvoiceSubtypeDropdown(ComboBox cmb_invoice_subtype_code, bool useZatcaEInvoice)
        {
            PopulateInvoiceSubtypeDropdown(cmb_invoice_subtype_code, "en-US", useZatcaEInvoice);
        }

        public static void PopulateInvoiceSubtypeDropdown(ComboBox cmb_invoice_subtype_code, string lang, bool useZatcaEInvoice)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(string));
            dt.Columns.Add("name", typeof(string));

            dt.Rows.Add("02", lang == "ar-SA" ? "فاتورة مبسطة" : "Simplified Invoice");
            dt.Rows.Add("01", lang == "ar-SA" ? "فاتورة ضريبية" : "Standard Invoice");

            cmb_invoice_subtype_code.DisplayMember = "name";
            cmb_invoice_subtype_code.ValueMember = "id";
            cmb_invoice_subtype_code.DataSource = dt;
            cmb_invoice_subtype_code.SelectedIndex = 0;
        }

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

                
            }

            return accounts;
        }

        public static void FillLocationsGridCombo(DataGridView grid_sales, int rowIndex, string product_id, string selectedValue = "DEF")
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            DataTable dt = generalBLL_obj.GetRecord("loc_code as location_code", "pos_product_stocks WHERE item_id=" + product_id + " AND  qty > 0 GROUP BY loc_code");

            if (dt.Rows.Count <= 0)
                dt = generalBLL_obj.GetRecord("L.code as location_code,L.name", "pos_locations L");

            DataGridViewComboBoxCell cell = new DataGridViewComboBoxCell();
            cell.DataSource = dt;
            cell.DisplayMember = "location_code";
            cell.ValueMember = "location_code";
            grid_sales.Rows[rowIndex].Cells["location_code"] = cell;
            grid_sales.Rows[rowIndex].Cells["location_code"].Value = dt.Rows[0]["location_code"].ToString();
        }
    }

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
