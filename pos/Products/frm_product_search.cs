using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Windows.Forms;

namespace pos.Products
{
    public partial class frm_product_search : Form
    {
        public ProductModal SelectedProduct { get; private set; }

        public frm_product_search()
        {
            InitializeComponent();
        }

        private void frm_product_search_Load(object sender, EventArgs e)
        {
            // TODO: Replace with actual product loading logic
            // Example: Load products from database
            
            DataTable productData = ProductBLL.GetAll();
            dataGridViewProducts.DataSource = productData;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            // TODO: Replace with actual search logic
            string filter = txtSearch.Text.Trim();
            ProductBLL products = new ProductBLL();
            DataTable productData = products.SearchRecord(filter);
            dataGridViewProducts.DataSource = productData;
        }

        private void dataGridViewProducts_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectProduct();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            SelectProduct();
        }

        private void SelectProduct()
        {
            if (dataGridViewProducts.CurrentRow != null)
            {
                // TODO: Map DataGridView row to Product object
                SelectedProduct = new ProductModal
                {
                    code = dataGridViewProducts.CurrentRow.Cells["Code"].Value.ToString(),
                    name = dataGridViewProducts.CurrentRow.Cells["Name"].Value.ToString(),
                    unit_price = Convert.ToDouble(dataGridViewProducts.CurrentRow.Cells["UnitPrice"].Value),
                    tax_id = Convert.ToInt16(dataGridViewProducts.CurrentRow.Cells["Tax"].Value),
                    location_code = dataGridViewProducts.CurrentRow.Cells["LocationCode"].Value.ToString(),
                    unit_id = Convert.ToInt16(dataGridViewProducts.CurrentRow.Cells["Unit"].Value.ToString()),
                    category = dataGridViewProducts.CurrentRow.Cells["Category"].Value.ToString()
                    // Add other fields as needed
                };
                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }

    

   
}