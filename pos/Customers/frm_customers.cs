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
    public partial class frm_customers : Form
    {

        public frm_customers()
        {
            InitializeComponent();
        }


        public void frm_customers_Load(object sender, EventArgs e)
        {
            load_customers_grid();
        }

        public void load_customers_grid()
        {
            try
            {
                grid_customers.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_customers.AutoGenerateColumns = false;

                String keyword = "id,first_name,last_name,email,vat_no,address,contact_no,date_created";
                String table = "pos_customers";
                grid_customers.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }
        
        private void btn_new_Click(object sender, EventArgs e)
        {
            //frm_addCustomer frm_addCustomer_obj = new frm_addCustomer(this,0,"false");
            //frm_addCustomer_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            //int customer_id = int.Parse(grid_customers.CurrentRow.Cells["id"].Value.ToString());
            //frm_addCustomer frm_addCustomer_obj = new frm_addCustomer(this,customer_id,"true");
            //frm_addCustomer_obj.ShowDialog();

        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_customers.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                CustomerBLL objBLL = new CustomerBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_customers_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }
        private void grid_customers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var customer_id = grid_customers.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                var first_name = grid_customers.CurrentRow.Cells["first_name"].Value.ToString(); // retreive the current row
                var last_name = grid_customers.CurrentRow.Cells["last_name"].Value.ToString(); // retreive the current row
                var full_name = first_name+last_name;
                load_customer_detail(Convert.ToInt32(customer_id), full_name);
            }
        }

        private void grid_customers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var customer_id = grid_customers.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
            var first_name = grid_customers.CurrentRow.Cells["first_name"].Value.ToString(); // retreive the current row
            var last_name = grid_customers.CurrentRow.Cells["last_name"].Value.ToString(); // retreive the current row
            var full_name = first_name +" "+ last_name;

            load_customer_detail(Convert.ToInt32(customer_id), full_name);
        }

        private void load_customer_detail(int customer_id,string full_name)
        {
            frm_customer_detail frm_obj = new frm_customer_detail(customer_id, full_name);
            frm_obj.Show();
            
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_customers_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_customers.DataSource = null;

                    //bind data in data grid view  
                    CustomerBLL objBLL = new CustomerBLL();
                    //grid_customers.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_customers.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_customers_KeyDown(object sender, KeyEventArgs e)
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

        private void grid_customers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.ColumnIndex == 8)
                {
                    var customer_id = grid_customers.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                    var first_name = grid_customers.CurrentRow.Cells["first_name"].Value.ToString(); // retreive the current row
                    var last_name = grid_customers.CurrentRow.Cells["last_name"].Value.ToString(); // retreive the current row
                    var full_name = first_name + " " + last_name;

                    load_customer_detail(Convert.ToInt32(customer_id), full_name);
            

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            
        }

    }
}
