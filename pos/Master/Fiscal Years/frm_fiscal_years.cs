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
    public partial class frm_fiscal_years : Form
    {

        public frm_fiscal_years()
        {
            InitializeComponent();
        }


        public void frm_fiscal_years_Load(object sender, EventArgs e)
        {
            load_fiscal_years_grid();
        }

        public void load_fiscal_years_grid()
        {
            try
            {
                grid_fiscal_years.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_fiscal_years.AutoGenerateColumns = false;

                String keyword = "*";
                String table = "acc_fiscal_years";
                grid_fiscal_years.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addFiscalYear frm_addFiscalYear_obj = new frm_addFiscalYear(this);
            frm_addFiscalYear.instance.tb_lbl_is_edit.Text = "false";

            frm_addFiscalYear_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if(grid_fiscal_years.Rows.Count > 0)
            {
                string id = grid_fiscal_years.CurrentRow.Cells["id"].Value.ToString();
                string name = grid_fiscal_years.CurrentRow.Cells["name"].Value.ToString();
                string code = grid_fiscal_years.CurrentRow.Cells["code"].Value.ToString();
                string from_date = grid_fiscal_years.CurrentRow.Cells["from_date"].Value.ToString();
                string to_date = grid_fiscal_years.CurrentRow.Cells["to_date"].Value.ToString();

                frm_addFiscalYear frm_addFiscalYear_obj = new frm_addFiscalYear(this);
                frm_addFiscalYear.instance.tb_lbl_is_edit.Text = "true";

                frm_addFiscalYear.instance.tb_id.Text = id;
                frm_addFiscalYear.instance.tb_name.Text = name;
                frm_addFiscalYear.instance.tb_code.Text = code;
                frm_addFiscalYear.instance.tb_from_date.Text = from_date;
                frm_addFiscalYear.instance.tb_to_date.Text = to_date;

                frm_addFiscalYear.instance.Show();
            }
            
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_fiscal_years.Rows.Count > 0)
            {
                string id = grid_fiscal_years.CurrentRow.Cells["id"].Value.ToString();

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    FiscalYearBLL objBLL = new FiscalYearBLL();
                    objBLL.Delete(int.Parse(id));

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    load_fiscal_years_grid();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

            } 
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_fiscal_years_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                //bind data in data grid view  
                FiscalYearBLL objBLL = new FiscalYearBLL();
                //grid_fiscal_years.AutoGenerateColumns = false;

                String condition = txt_search.Text;
                grid_fiscal_years.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_fiscal_years_KeyDown(object sender, KeyEventArgs e)
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

        private void grid_fiscal_years_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string name = grid_fiscal_years.Columns[e.ColumnIndex].Name;
            if (name == "activate")
            {
                string id = grid_fiscal_years.CurrentRow.Cells["id"].Value.ToString();
                string from_date = grid_fiscal_years.CurrentRow.Cells["from_date"].Value.ToString();
                string to_date = grid_fiscal_years.CurrentRow.Cells["to_date"].Value.ToString();
                string desc = grid_fiscal_years.CurrentRow.Cells["name"].Value.ToString();

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to activate", "Activate Fiscal Year", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    FiscalYearBLL objBLL = new FiscalYearBLL();
                    objBLL.SetAllStatusZero(int.Parse(id));
                    objBLL.UpdateStatus(int.Parse(id),true);

                    ////
                    ////// Update static variables
                    UsersModal userModal_obj = new UsersModal();
                    UsersModal.fiscal_year = desc;
                    UsersModal.fy_from_date = Convert.ToDateTime(from_date);
                    UsersModal.fy_to_date = Convert.ToDateTime(to_date);
                    ////

                    MessageBox.Show("Record updated successfully.", "Update Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    load_fiscal_years_grid();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }

                load_fiscal_years_grid();
            }
        }
    }
}
