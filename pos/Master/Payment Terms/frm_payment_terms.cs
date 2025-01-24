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
    public partial class frm_payment_terms : Form
    {

        public frm_payment_terms()
        {
            InitializeComponent();
        }


        public void frm_payment_terms_Load(object sender, EventArgs e)
        {
            load_payment_terms_grid();
        }

        public void load_payment_terms_grid()
        {
            try
            {
                grid_payment_terms.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_payment_terms.AutoGenerateColumns = false;

                String keyword = "id,code,description,date_created";
                String table = "pos_payment_terms";
                grid_payment_terms.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addPaymentTerm frm_addPaymentTerm_obj = new frm_addPaymentTerm(this);
            frm_addPaymentTerm.instance.tb_lbl_is_edit.Text = "false";

            frm_addPaymentTerm_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if(grid_payment_terms.Rows.Count > 0)
            {
                string id = grid_payment_terms.CurrentRow.Cells[0].Value.ToString();
                string code = grid_payment_terms.CurrentRow.Cells[1].Value.ToString();
                string desc = grid_payment_terms.CurrentRow.Cells[2].Value.ToString();

                frm_addPaymentTerm frm_addPaymentTerm_obj = new frm_addPaymentTerm(this);
                frm_addPaymentTerm.instance.tb_lbl_is_edit.Text = "true";

                frm_addPaymentTerm.instance.tb_id.Text = id;
                frm_addPaymentTerm.instance.tb_code.Text = code;
                frm_addPaymentTerm.instance.tb_desc.Text = desc;

                frm_addPaymentTerm.instance.Show();
            }
            
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_payment_terms.Rows.Count > 0)
            {
                string id = grid_payment_terms.CurrentRow.Cells[0].Value.ToString();

                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {

                    PaymentTermsBLL objBLL = new PaymentTermsBLL();
                    objBLL.Delete(int.Parse(id));

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    load_payment_terms_grid();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_payment_terms_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                    //grid_payment_terms.DataSource = null;

                    //bind data in data grid view  
                    PaymentTermsBLL objBLL = new PaymentTermsBLL();
                    //grid_payment_terms.AutoGenerateColumns = false;

                    String condition = txt_search.Text;
                    grid_payment_terms.DataSource = objBLL.SearchRecord(condition);

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

        private void frm_payment_terms_KeyDown(object sender, KeyEventArgs e)
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
