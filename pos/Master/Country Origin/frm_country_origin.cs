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
using pos.UI;

namespace pos
{
    public partial class frm_country_origin : Form
    {

        public frm_country_origin()
        {
            InitializeComponent();
        }


        public void frm_country_origin_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            load_country_origin_grid();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel2, lbl_taxes_title, panel1, grid_country_origin, id);
        }

        public void load_country_origin_grid()
        {
            try
            {
                grid_country_origin.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_country_origin.AutoGenerateColumns = false;

                String keyword = "id,code,name,date_created";
                String table = "pos_country_origin";
                grid_country_origin.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addOrigin frm_addOrigin_obj = new frm_addOrigin(this);
            frm_addOrigin.instance.tb_lbl_is_edit.Text = "false";

            frm_addOrigin_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_country_origin.CurrentRow.Cells["id"].Value.ToString();
            string code = grid_country_origin.CurrentRow.Cells["code"].Value.ToString();
            string name = grid_country_origin.CurrentRow.Cells["name"].Value.ToString();
            //string brand_code = grid_country_origin.CurrentRow.Cells["brand_code"].Value.ToString();
            
            frm_addOrigin frm_addOrigin_obj = new frm_addOrigin(this);
            frm_addOrigin.instance.tb_lbl_is_edit.Text = "true";

            frm_addOrigin.instance.tb_id.Text = id;
            frm_addOrigin.instance.tb_code.Text = code;
            frm_addOrigin.instance.tb_name.Text = name;
            //frm_addOrigin.instance.tb_origin_code.SelectedValue = brand_code;
            frm_addOrigin.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_country_origin.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                OriginBLL objBLL = new OriginBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_country_origin_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_country_origin_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_country_origin.DataSource = null;

                    //bind data in data grid view  
                    OriginBLL objBLL = new OriginBLL();
                    //grid_country_origin.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_country_origin.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_country_origin_KeyDown(object sender, KeyEventArgs e)
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
