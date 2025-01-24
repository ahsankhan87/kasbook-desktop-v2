using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using POS.BLL;
using POS.Core;

namespace pos
{
    public partial class frm_low_stock_products : Form
    {
        private DataGridView brandsDataGridView = new DataGridView();
        
        public frm_low_stock_products()
        {
            InitializeComponent();
        }

        
        public void frm_low_stock_products_Load(object sender, EventArgs e)
        {
            load_low_stock_products_grid();
        }
        
        public void load_low_stock_products_grid()
        {
            try
            {
                grid_low_stock_products.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_low_stock_products.AutoGenerateColumns = false;

                String keyword = "id,code,name,qty,avg_cost,unit_price,loc_code, (1) AS purchase_order_qty";
                String table = "pos_products_location_view WHERE qty <= re_stock_level ORDER BY id desc";
                grid_low_stock_products.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }


        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_low_stock_products.CurrentRow.Cells[0].Value.ToString();
            
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);  

            if (result == DialogResult.Yes)
            {
                ProductBLL objBLL = new ProductBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_low_stock_products_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_low_stock_products_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                String condition = txt_search.Text;
                var brand_code = txt_brand_code.Text;

                grid_low_stock_products.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_low_stock_products.AutoGenerateColumns = false;

                String keyword = "id,code,name,qty,avg_cost,unit_price,loc_code, (1) AS purchase_order_qty";
                String table = string.Format("pos_products_location_view WHERE qty <= re_stock_level AND brand_code LIKE '%{0}%' ORDER BY id desc", brand_code);
                
                grid_low_stock_products.DataSource = objBLL.GetRecord(keyword, table);
                    
                txt_search.Text = "";
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } 
            
        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

        private void grid_low_stock_products_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string name = grid_low_stock_products.Columns[e.ColumnIndex].Name;
            if(name == "edit")
            {
                string product_id = grid_low_stock_products.CurrentRow.Cells["id"].Value.ToString();
                //frm_addProduct frm_addProduct_obj = new frm_addProduct(this, int.Parse(product_id), "true");
                //frm_addProduct_obj.ShowDialog();
            
            }
            else if (name == "delete")
            {
                string id = grid_low_stock_products.CurrentRow.Cells["id"].Value.ToString();

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    ProductBLL objBLL = new ProductBLL();
                    objBLL.Delete(int.Parse(id));

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    load_low_stock_products_grid();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

                load_low_stock_products_grid();
            }
        }

        private void SetupBrandDataGridView()
        {
            brandsDataGridView.ColumnCount = 2;
            brandsDataGridView.Name = "brandsDataGridView";
            brandsDataGridView.Location = new Point(240, 80);
            brandsDataGridView.Size = new Size(200, 250);
            brandsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            brandsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            brandsDataGridView.Columns[0].Name = "Code";
            brandsDataGridView.Columns[1].Name = "Name";
            brandsDataGridView.Columns[0].ReadOnly = true;
            brandsDataGridView.Columns[1].ReadOnly = true;
            brandsDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            brandsDataGridView.MultiSelect = false;
            brandsDataGridView.AllowUserToAddRows = false;
            brandsDataGridView.AllowUserToDeleteRows = false;

            brandsDataGridView.CellClick += new DataGridViewCellEventHandler(brandsDataGridView_CellClick);
            this.brandsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(brandsDataGridView_KeyDown);

            this.Controls.Add(brandsDataGridView);
            brandsDataGridView.BringToFront();

        }

        void brandsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(brandsDataGridView);
                grid_low_stock_products.Focus();
            }
        }

        private void brandsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(brandsDataGridView);
            grid_low_stock_products.Focus();

        }

        private void txt_brands_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_brands.Text != "")
                {
                    SetupBrandDataGridView();

                    BrandsBLL brandsBLL_obj = new BrandsBLL();
                    string brand_name = txt_brands.Text;

                    DataTable dt = brandsBLL_obj.SearchRecord(brand_name);

                    if (dt.Rows.Count > 0)
                    {
                        brandsDataGridView.Rows.Clear();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string code = dr["code"].ToString();
                            string name = dr["name"].ToString();

                            string[] row0 = { code, name };

                            brandsDataGridView.Rows.Add(row0);
                        }
                        brandsDataGridView.CurrentCell = brandsDataGridView.Rows[0].Cells[0];

                    }

                }
                else
                {
                    txt_brand_code.Text = "";
                    this.Controls.Remove(brandsDataGridView);
                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_brands_Leave(object sender, EventArgs e)
        {
            if (!brandsDataGridView.Focused)
            {
                this.Controls.Remove(brandsDataGridView);
            }
        }

        private void frm_low_stock_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                brandsDataGridView.Focus();
                
            }
        }

        private void btn_p_order_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to order", "Purchase Order Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_low_stock_products.Rows.Count > 0)
                    {
                        Purchases_orderModal Purchases_orderModal_obj = new Purchases_orderModal();
                        Purchases_orderBLL purchases_orderObj = new Purchases_orderBLL();
                        DateTime purchase_date = DateTime.Now;
                        int supplier_id = 0; // (cmb_suppliers.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_suppliers.SelectedValue.ToString()));
                        int employee_id = 0; // (cmb_employees.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_employees.SelectedValue.ToString()));

                        Purchases_orderModal_obj.employee_id = employee_id;
                        Purchases_orderModal_obj.supplier_id = supplier_id;
                        Purchases_orderModal_obj.supplier_invoice_no = ""; // txt_supplier_invoice.Text;
                        Purchases_orderModal_obj.invoice_no = GetMAX_purchaseorder_InvoiceNo();
                        Purchases_orderModal_obj.total_amount = get_total_amount();
                        Purchases_orderModal_obj.purchase_type = ""; // cmb_purchase_type.SelectedValue.ToString();
                        Purchases_orderModal_obj.total_discount = 0; // total_discount;
                        Purchases_orderModal_obj.total_tax = 0; // total_tax;
                        Purchases_orderModal_obj.description = ""; // txt_description.Text;
                        Purchases_orderModal_obj.purchase_date = purchase_date.ToString("yyyy-MM-dd");
                        Purchases_orderModal_obj.account = "Purchase Order";
                        Purchases_orderModal_obj.delivery_date = purchase_date.ToString("yyyy-MM-dd"); // txt_delivery_date.Value.Date;
                        Purchases_orderModal_obj.purchase_time = purchase_date;


                        Int32 purchase_id = purchases_orderObj.Insert_Purchase_order_new(Purchases_orderModal_obj, DGV_to_datatable(grid_low_stock_products));

                        if (purchase_id > 0)
                        {
                            MessageBox.Show("Purchase Order Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btn_refresh.PerformClick();
                        }
                        else
                        {
                            MessageBox.Show("Record not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please add products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private DataTable DGV_to_datatable(DataGridView dgv)
        {
            DataTable dt = new DataTable();
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                dt.Columns.Add(col.Name);
            }

            foreach (DataGridViewRow row in dgv.Rows)
            {
                DataRow dRow = dt.NewRow();
                foreach (DataGridViewCell cell in row.Cells)
                {
                    dRow[cell.ColumnIndex] = cell.Value;
                }
                dt.Rows.Add(dRow);
            }
            return dt;
        }

        public string GetMAX_purchaseorder_InvoiceNo()
        {
            Purchases_orderBLL Purchases_orderBLL_obj = new Purchases_orderBLL();
            return Purchases_orderBLL_obj.GetMaxInvoiceNo();
        }


        private double get_total_amount()
        {
            double total_amount = 0;

            for (int i = 0; i <= grid_low_stock_products.Rows.Count-1; i++)
            {
                total_amount += Convert.ToDouble(grid_low_stock_products.Rows[i].Cells["purchase_order_qty"].Value) * Convert.ToDouble(grid_low_stock_products.Rows[i].Cells["avg_cost"].Value);
            }
            double net = (total_amount);
            return net;
        }

    }
}
