using System;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using POS.Core;

namespace pos
{
    public partial class frm_products_labels : Form
    {
        int global_product_id = 0;
        int global_alt_id = 0;

        public frm_products_labels()
        {
            InitializeComponent();
        }

        public void frm_products_labels_Load(object sender, EventArgs e)
        {
            txt_product_code.Focus();
            autoCompleteInvoice();
        }

       
        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                //Convert all grid to datatable and send it to report rpt
                DataTable dt = new DataTable();
                foreach (DataGridViewColumn col in grid_product_groups.Columns)
                {
                    dt.Columns.Add(col.Name);
                }

                foreach (DataGridViewRow row in grid_product_groups.Rows)
                {
                    DataRow dRow = dt.NewRow();
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        dRow[cell.ColumnIndex] = cell.Value;
                    }
                    dt.Rows.Add(dRow);
                }

                using (frm_ProductLabelReport obj = new frm_ProductLabelReport(dt))//send datatable to report for print
                {
                    //obj.load_print();
                    obj.ShowDialog();
                }
                grid_product_groups.Rows.Clear();
                grid_product_groups.Refresh();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_product_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_product_code.Text != "" && e.KeyData == Keys.Enter)
            {
                frm_searchProducts search_product_obj = new frm_searchProducts(null, null, null, txt_product_code.Text, "", "", 0, false,false, this);
                search_product_obj.ShowDialog();

            }
        }

        public void load_products(string product_id = "")
        {

            ProductBLL productsBLL_obj = new ProductBLL();
            DataTable product_dt = new DataTable();

            if (product_id != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductID(product_id);
            }

            if (product_dt.Rows.Count > 0)
            {
                foreach (DataRow myProductView in product_dt.Rows)
                {

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string category = myProductView["category"].ToString();
                    string name = myProductView["name"].ToString();
                    string qty = myProductView["qty"].ToString();
                    string unit_price = myProductView["unit_price"].ToString();
                    string barcode = myProductView["barcode"].ToString();
                    string location_code = myProductView["location_code"].ToString();
                    string label_qty = "0";

                    string[] row0 = { id.ToString(), code,category, name, qty, unit_price, location_code,label_qty, barcode };

                    grid_product_groups.Rows.Add(row0);

                }


            }
            else
            {
                MessageBox.Show("Record not found", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void frm_products_labels_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                btn_save.PerformClick();
            }
        }

        private void grid_product_groups_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int product_id = int.Parse(grid_product_groups.CurrentRow.Cells["id"].Value.ToString());
                            
            string name = grid_product_groups.Columns[e.ColumnIndex].Name;
            if (name == "btn_delete")
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    //ProductGroupsBLL objBLL = new ProductGroupsBLL();
                    //objBLL.DeleteAltNo(product_id);

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

                //load_Products_grid();

            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            if (txt_product_code.Text != "" )
            {
                frm_searchProducts search_product_obj = new frm_searchProducts(null, null, null, txt_product_code.Text, "", "", 0, false, false, this);
                search_product_obj.ShowDialog();

            }

        }
        public void Load_products_to_grid_by_invoiceno(DataTable dt)
        {
            try
            {
                grid_product_groups.Rows.Clear();
                grid_product_groups.Refresh();

                if (dt.Rows.Count > 0)
                {

                    foreach (DataRow myProductView in dt.Rows)
                    {
                        string id =  myProductView["id"].ToString(); //myProductView["code"].ToString(); /
                        string code = myProductView["code"].ToString();
                        string category = myProductView["category"].ToString();
                        string name = myProductView["name"].ToString();
                        string qty = myProductView["quantity"].ToString();
                        string unit_price = myProductView["unit_price"].ToString();
                        string barcode = myProductView["barcode"].ToString();
                        string location_code = myProductView["location_code"].ToString();
                        string label_qty = "0";

                        string[] row0 = { id, code,category, name, qty, unit_price, location_code, label_qty, barcode};

                        grid_product_groups.Rows.Add(row0);

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void txt_purchase_inv_no_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_purchase_inv_no.Text != "" && e.KeyData == Keys.Enter)
            {
                btn_invoice_search.PerformClick();
            }
        }

        private void btn_invoice_search_Click(object sender, EventArgs e)
        {
            string invoice_no = txt_purchase_inv_no.Text;
            DataTable dt; 

            PurchasesBLL purchasesObj = new PurchasesBLL();

            dt = purchasesObj.GetAllPurchaseByInvoice(invoice_no);

            Load_products_to_grid_by_invoiceno(dt);
        }

        public void autoCompleteInvoice()
        {
            try
            {
                txt_purchase_inv_no.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt_purchase_inv_no.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

                GeneralBLL invoicesBLL_obj = new GeneralBLL();
                string keyword = "TOP 500 invoice_no ";
                string table = "pos_purchases WHERE account <> 'return' AND branch_id="+UsersModal.logged_in_branch_id+" ORDER BY id desc";
                DataTable dt = invoicesBLL_obj.GetRecord(keyword, table);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        coll.Add(dr["invoice_no"].ToString());

                    }

                }

                txt_purchase_inv_no.AutoCompleteCustomSource = coll;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        
    }
}
