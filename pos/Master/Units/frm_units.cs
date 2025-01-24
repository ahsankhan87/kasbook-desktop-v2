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
    public partial class frm_units : Form
    {

        public frm_units()
        {
            InitializeComponent();
        }


        public void frm_units_Load(object sender, EventArgs e)
        {
            load_units_grid();
        }

        public void load_units_grid()
        {
            try
            {
                grid_units.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_units.AutoGenerateColumns = false;

                String keyword = "id,name,date_created";
                String table = "pos_units";
                grid_units.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addUnit frm_addUnit_obj = new frm_addUnit(this);
            frm_addUnit.instance.tb_lbl_is_edit.Text = "false";

            frm_addUnit_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if(grid_units.Rows.Count > 0)
            {
                string id = grid_units.CurrentRow.Cells[0].Value.ToString();
                string title = grid_units.CurrentRow.Cells[1].Value.ToString();
                string rate = grid_units.CurrentRow.Cells[2].Value.ToString();

                frm_addUnit frm_addUnit_obj = new frm_addUnit(this);
                frm_addUnit.instance.tb_lbl_is_edit.Text = "true";

                frm_addUnit.instance.tb_id.Text = id;
                frm_addUnit.instance.tb_name.Text = title;

                frm_addUnit.instance.Show();
            }
            
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_units.Rows.Count > 0)
            {
                string id = grid_units.CurrentRow.Cells[0].Value.ToString();

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    UnitsBLL objBLL = new UnitsBLL();
                    objBLL.Delete(int.Parse(id));

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    load_units_grid();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_units_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_units.DataSource = null;

                    //bind data in data grid view  
                    UnitsBLL objBLL = new UnitsBLL();
                    //grid_units.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_units.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_units_KeyDown(object sender, KeyEventArgs e)
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
