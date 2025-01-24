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
    public partial class frm_locations : Form
    {

        public frm_locations()
        {
            InitializeComponent();
        }


        public void frm_locations_Load(object sender, EventArgs e)
        {
            load_Locations_grid();
        }

        public void load_Locations_grid()
        {
            try
            {
                grid_locations.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_locations.AutoGenerateColumns = false;

                String keyword = "*";
                String table = "pos_Locations";
                grid_locations.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addLocation frm_addLocation_obj = new frm_addLocation(this);
            frm_addLocation.instance.tb_lbl_is_edit.Text = "false";

            frm_addLocation_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_locations.CurrentRow.Cells["id"].Value.ToString();
            string name = grid_locations.CurrentRow.Cells["name"].Value.ToString();
            string code = grid_locations.CurrentRow.Cells["code"].Value.ToString();
            
            frm_addLocation frm_addLocation_obj = new frm_addLocation(this);
            frm_addLocation.instance.tb_lbl_is_edit.Text = "true";

            frm_addLocation.instance.tb_id.Text = id;
            frm_addLocation.instance.tb_name.Text = name;
            frm_addLocation.instance.tb_code.Text = code;
            
            frm_addLocation.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_locations.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                LocationsBLL objBLL = new LocationsBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_Locations_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_Locations_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_locations.DataSource = null;

                    //bind data in data grid view  
                    LocationsBLL objBLL = new LocationsBLL();
                    //grid_locations.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_locations.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_locations_KeyDown(object sender, KeyEventArgs e)
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
    }
}
