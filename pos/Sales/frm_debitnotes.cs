using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Sales
{
    public partial class frm_debitnotes : Form
    {
        public frm_debitnotes()
        {
            InitializeComponent();
        }

        private void frm_debitnotes_Load(object sender, EventArgs e)
        {
            // Initialize form components
            GetMaxInvoiceNo();
            Get_customers_dropdownlist();
            autoCompleteInvoice();
            LoadDebitNotes();
            ReasonDDL();
        }
        private void GetMaxInvoiceNo()
        {
            // Logic to get the maximum invoice number
        }
        private void Get_customers_dropdownlist()
        {
            // Logic to populate customers dropdown list
        }
        private void autoCompleteInvoice()
        {
            // Logic to set up autocomplete for invoice numbers
        }
        private void LoadDebitNotes()
        {
            // Logic to load existing debit notes into the DataGridView
        }
        private void ReasonDDL()
        {
            // Logic to populate the reason dropdown list
        }

        private void grid_sales_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (grid_sales.CurrentCell.ColumnIndex == grid_sales.Columns["code"].Index)
            {
                

                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyDown -= ProductCodeCell_KeyDown;
                    tb.KeyDown += ProductCodeCell_KeyDown;
                }
            }
        }
        // Show product search form when Enter is pressed in the first cell
        private void ProductCodeCell_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string product_code = (grid_sales.CurrentRow.Cells["code"].Value != null ? grid_sales.CurrentRow.Cells["code"].Value.ToString() : "");

                bool isGrid = true;
                var brand_code = txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                var category_code = txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                var group_code = txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                using (var productSearchForm = new frm_searchSaleProducts(this, product_code, category_code, brand_code, isGrid, group_code))
                {
                    if (productSearchForm.ShowDialog() == DialogResult.OK)
                    {
                        var product = productSearchForm.SelectedProduct;
                        if (product != null)
                        {
                            AddProductToGrid(product, grid_sales.CurrentCell.RowIndex);
                        }
                    }
                }
                e.Handled = true;
            }
        }
        
        // Fill the current row with product details
        private void AddProductToGrid(Product product, int rowIndex)
        {
            var row = grid_sales.Rows[rowIndex];
            row.Cells["code"].Value = product.Code;
            row.Cells["name"].Value = product.Name;
            row.Cells["Qty"].Value = 1;
            row.Cells["unit_price"].Value = product.UnitPrice;
            row.Cells["discount"].Value = 0;
            row.Cells["discount_percent"].Value = 0;
            row.Cells["total_without_vat"].Value = product.UnitPrice;
            row.Cells["tax"].Value = product.Tax;
            row.Cells["sub_total"].Value = product.UnitPrice + product.Tax;
            row.Cells["location_code"].Value = product.LocationCode;
            row.Cells["unit"].Value = product.Unit;
            row.Cells["category"].Value = product.Category;
            // ... set other columns as needed
        }
    }
}
