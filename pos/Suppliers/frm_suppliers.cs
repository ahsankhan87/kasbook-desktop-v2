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

namespace pos
{
    public partial class frm_suppliers : Form
    {
        
        public frm_suppliers()
        {
            InitializeComponent();
        }
        
        public void frm_suppliers_Load(object sender, EventArgs e)
        {
            load_Suppliers_grid();
        }
        
        public void load_Suppliers_grid()
        {
            try
            {
                grid_suppliers.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_suppliers.AutoGenerateColumns = false;

                String keyword = "id,first_name,last_name,email,vat_no,address,contact_no,date_created,vat_status";
                String table = "pos_suppliers";
                grid_suppliers.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addSupplier frm_addSupplier_obj = new frm_addSupplier(this);
            frm_addSupplier.instance.tb_lbl_is_edit.Text = "false";

            frm_addSupplier_obj.ShowDialog();
        }
        
        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_suppliers.CurrentRow.Cells["id"].Value.ToString();
            string first_name = grid_suppliers.CurrentRow.Cells[1].Value.ToString();
            string last_name = grid_suppliers.CurrentRow.Cells[2].Value.ToString();
            string email = grid_suppliers.CurrentRow.Cells[3].Value.ToString();
            string vat_no = grid_suppliers.CurrentRow.Cells[4].Value.ToString();
            string contact_no = grid_suppliers.CurrentRow.Cells[5].Value.ToString();
            string address = grid_suppliers.CurrentRow.Cells[6].Value.ToString();
            bool vat_status = Convert.ToBoolean(grid_suppliers.CurrentRow.Cells["vat_status"].Value.ToString());

            frm_addSupplier frm_addSupplier_obj = new frm_addSupplier(this);
            frm_addSupplier.instance.tb_lbl_is_edit.Text = "true";

            frm_addSupplier.instance.tb_id.Text = id;
            frm_addSupplier.instance.tb_first_name.Text = first_name;
            frm_addSupplier.instance.tb_last_name.Text = last_name;
            frm_addSupplier.instance.tb_email.Text = email;
            frm_addSupplier.instance.tb_vat_no.Text = vat_no;
            frm_addSupplier.instance.tb_address.Text = address;
            frm_addSupplier.instance.tb_contact_no.Text = contact_no;
            frm_addSupplier.instance.vat_with_status.Checked = vat_status;

            frm_addSupplier.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_suppliers.CurrentRow.Cells[0].Value.ToString();
            
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);  

            if (result == DialogResult.Yes)
            {
             
                SupplierBLL objBLL = new SupplierBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_Suppliers_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
            
        }
        private void grid_suppliers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var supplier_id = grid_suppliers.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                var first_name = grid_suppliers.CurrentRow.Cells["first_name"].Value.ToString(); // retreive the current row
                var last_name = grid_suppliers.CurrentRow.Cells["last_name"].Value.ToString(); // retreive the current row
                var full_name = first_name + last_name;
                load_supplier_detail(Convert.ToInt32(supplier_id), full_name);
            }
        }

        private void grid_suppliers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var supplier_id = grid_suppliers.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
            var first_name = grid_suppliers.CurrentRow.Cells["first_name"].Value.ToString(); // retreive the current row
            var last_name = grid_suppliers.CurrentRow.Cells["last_name"].Value.ToString(); // retreive the current row
            var full_name = first_name + " " + last_name;

            load_supplier_detail(Convert.ToInt32(supplier_id), full_name);
        }

        private void load_supplier_detail(int supplier_id, string full_name)
        {
            frm_supplier_detail frm_obj = new frm_supplier_detail(supplier_id, full_name);
            frm_obj.Show();

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_Suppliers_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_suppliers.DataSource = null;

                    //bind data in data grid view  
                    SupplierBLL objBLL = new SupplierBLL();
                    //grid_suppliers.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_suppliers.DataSource = objBLL.SearchRecord(condition);
                    
                    //txt_search.Text = "";
                
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

        private void frm_suppliers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.N)
            {
                btn_new.PerformClick();
                
            }

            if (e.Control == true && e.KeyCode == Keys.U)
            {
                btn_update.PerformClick();
            }
            
            if (e.Control == true && e.KeyCode == Keys.D)
            {
                btn_delete.PerformClick();

            }
        }

        private void grid_suppliers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try {
                if (e.ColumnIndex == 8)
                {
                    var supplier_id = grid_suppliers.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                    var first_name = grid_suppliers.CurrentRow.Cells["first_name"].Value.ToString(); // retreive the current row
                    var last_name = grid_suppliers.CurrentRow.Cells["last_name"].Value.ToString(); // retreive the current row
                    var full_name = first_name + " " + last_name;

                    load_supplier_detail(Convert.ToInt32(supplier_id), full_name);
            

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

    }
}
