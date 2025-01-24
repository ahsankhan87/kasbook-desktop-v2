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
    public partial class frm_taxes : Form
    {

        public frm_taxes()
        {
            InitializeComponent();
        }


        public void frm_taxes_Load(object sender, EventArgs e)
        {
            load_Taxes_grid();
        }

        public void load_Taxes_grid()
        {
            try
            {
                grid_taxes.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_taxes.AutoGenerateColumns = false;

                String keyword = "id,title,rate,date_created";
                String table = "pos_taxes";
                grid_taxes.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addTax frm_addTax_obj = new frm_addTax(this);
            frm_addTax.instance.tb_lbl_is_edit.Text = "false";

            frm_addTax_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_taxes.Rows.Count > 0)
            {
                string id = grid_taxes.CurrentRow.Cells["id"].Value.ToString();
                string title = grid_taxes.CurrentRow.Cells["title"].Value.ToString();
                string rate = grid_taxes.CurrentRow.Cells["rate"].Value.ToString();

                frm_addTax frm_addTax_obj = new frm_addTax(this);
                frm_addTax.instance.tb_lbl_is_edit.Text = "true";

                frm_addTax.instance.tb_id.Text = id;
                frm_addTax.instance.tb_title.Text = title;
                frm_addTax.instance.tb_rate.Text = rate;

                frm_addTax.instance.Show();
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_taxes.Rows.Count > 0)
            {
                string id = grid_taxes.CurrentRow.Cells[0].Value.ToString();

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    TaxBLL objBLL = new TaxBLL();
                    objBLL.Delete(int.Parse(id));

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    load_Taxes_grid();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_Taxes_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_taxes.DataSource = null;

                    //bind data in data grid view  
                    TaxBLL objBLL = new TaxBLL();
                    //grid_taxes.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_taxes.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_taxes_KeyDown(object sender, KeyEventArgs e)
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
