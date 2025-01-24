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
    public partial class frm_companies : Form
    {

        public frm_companies()
        {
            InitializeComponent();
        }


        public void frm_companies_Load(object sender, EventArgs e)
        {
            load_companies_grid();
        }

        public void load_companies_grid()
        {
            try
            {
                grid_companies.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_companies.AutoGenerateColumns = false;

                String keyword = "id,name,address,vat_no,email,currency_id,contact_no,image,date_created";
                String table = "pos_companies";
                grid_companies.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addCompany frm_addCompany_obj = new frm_addCompany(this);
            frm_addCompany.instance.tb_lbl_is_edit.Text = "false";

            frm_addCompany_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            string id = grid_companies.CurrentRow.Cells["id"].Value.ToString();
            string name = grid_companies.CurrentRow.Cells["name"].Value.ToString();
            string address = grid_companies.CurrentRow.Cells["address"].Value.ToString();
            string email = grid_companies.CurrentRow.Cells["email"].Value.ToString();
            string contact_no = grid_companies.CurrentRow.Cells["contact_no"].Value.ToString();
            string vat_no = grid_companies.CurrentRow.Cells["vat_no"].Value.ToString();
            string currency_id = grid_companies.CurrentRow.Cells["currency_id"].Value.ToString();
            
            frm_addCompany frm_addCompany_obj = new frm_addCompany(this);
            frm_addCompany.instance.tb_lbl_is_edit.Text = "true";

            frm_addCompany.instance.tb_id.Text = id;
            frm_addCompany.instance.tb_name.Text = name;
            frm_addCompany.instance.tb_address.Text = address;
            frm_addCompany.instance.tb_email.Text = email;
            frm_addCompany.instance.tb_contact_no.Text = contact_no;
            frm_addCompany.instance.tb_vat_no.Text = vat_no;
            frm_addCompany.instance.tb_currency_id.Text = currency_id;
            
            frm_addCompany.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = grid_companies.CurrentRow.Cells[0].Value.ToString();

            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {

                CompaniesBLL objBLL = new CompaniesBLL();
                objBLL.Delete(int.Parse(id));

                MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                load_companies_grid();
            }
            else
            {
                MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_companies_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_companies.DataSource = null;

                    //bind data in data grid view  
                    CompaniesBLL objBLL = new CompaniesBLL();
                    //grid_companies.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_companies.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_companies_KeyDown(object sender, KeyEventArgs e)
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
