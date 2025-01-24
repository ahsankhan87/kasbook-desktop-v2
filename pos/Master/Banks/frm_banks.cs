using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Master.Banks
{
    public partial class frm_banks : Form
    {
        public frm_banks()
        {
            InitializeComponent();
        }

        private void frm_banks_Load(object sender, EventArgs e)
        {
            get_accounts_dropdownlist();
        }

        public void get_accounts_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts";

            DataTable accounts = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = accounts.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            accounts.Rows.InsertAt(emptyRow, 0);

            cmb_GL_account_code.DisplayMember = "name";
            cmb_GL_account_code.ValueMember = "id";
            cmb_GL_account_code.DataSource = accounts;

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (txt_name.Text != string.Empty && txt_name.Text != string.Empty)
                {
                    BankModal info = new BankModal
                    {
                        name = txt_name.Text,
                        holderName = txt_holderName.Text,
                        accountNo = txt_accountNo.Text,
                        bankBranch = txt_bankBranch.Text,
                        GLAccountID = (cmb_GL_account_code.SelectedValue == null ? "" : cmb_GL_account_code.SelectedValue.ToString())
                };

                    BankBLL objBLL = new BankBLL();
                    int result = objBLL.Insert(info);
                    if (result > 0)
                    {
                        MessageBox.Show("Record created successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear_all();
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                }
                else
                {
                    MessageBox.Show("Please enter value in field", "Invalid Data", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void frm_banks_KeyDown(object sender, KeyEventArgs e)
        {
            //when you enter in textbox it will goto next textbox, work like TAB key
            if (e.KeyData == Keys.Enter)
            {
                SendKeys.Send("{TAB}");
            }
            if (e.KeyData == Keys.F3)
            {
                btn_save.PerformClick();
            }
            if (e.KeyData == Keys.F4)
            {
                btn_update.PerformClick();
            }
            if (e.KeyData == Keys.F5)
            {
                btn_refresh.PerformClick();
            }
            
        }

        private void clear_all()
        {
            txt_id.Text = "";
            txt_name.Text = "";
            txt_accountNo.Text = "";
            txt_bankBranch.Text = "";
            txt_holderName.Text = "";
            lbl_bank_name.Text = "";
            grid_banks_transactions.DataSource = null;
            //cmb_GL_account_code.SelectedValue = "";
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            try
            {
                if (String.IsNullOrEmpty(txt_id.Text))
                {
                    MessageBox.Show("Record not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (txt_name.Text != string.Empty && txt_name.Text != string.Empty)
                {
                    BankModal info = new BankModal
                    {
                        name = txt_name.Text,
                        holderName = txt_holderName.Text,
                        accountNo = txt_accountNo.Text,
                        bankBranch = txt_bankBranch.Text,
                        GLAccountID = cmb_GL_account_code.SelectedValue.ToString()
                    };

                    BankBLL objBLL = new BankBLL();

                    info.id = int.Parse(txt_id.Text);

                    int result = objBLL.Update(info);
                    if (result > 0)
                    {
                        MessageBox.Show("Record updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        clear_all();
                    }
                    else
                    {
                        MessageBox.Show("Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = txt_id.Text;

            if (id != "")
            {
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show("Are you sure you want to delete", "Delete Record", buttons, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    BankBLL objBLL = new BankBLL();
                    objBLL.Delete(int.Parse(id));

                    MessageBox.Show("Record deleted successfully.", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    clear_all();
                }
                else
                {
                    MessageBox.Show("Please select record", "Delete Record", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btn_blank_Click(object sender, EventArgs e)
        {
            clear_all();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            frm_banks_search search_obj = new frm_banks_search(this, txt_search.Text);
            search_obj.ShowDialog();
        }

        public void load_banks_transactions_grid(int bank_id)
        {
            try
            {
                grid_banks_transactions.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_banks_transactions.AutoGenerateColumns = false;

                String keyword = "id,invoice_no,debit,credit,(debit-credit) AS balance,description,entry_date,account_id,account_name";
                String table = "pos_banks_payments WHERE bank_id = " + bank_id + "";

                DataTable dt = new DataTable();
                dt = objBLL.GetRecord(keyword, table);

                double _dr_total = 0;
                double _cr_total = 0;

                foreach (DataRow dr in dt.Rows)
                {
                    _dr_total += Convert.ToDouble(dr["debit"].ToString());
                    _cr_total += Convert.ToDouble(dr["credit"].ToString());

                }

                DataRow newRow = dt.NewRow();
                newRow[8] = "Total";
                newRow[2] = _dr_total;
                newRow[3] = _cr_total;
                newRow[4] = (_dr_total - _cr_total);
                dt.Rows.InsertAt(newRow, dt.Rows.Count);

                grid_banks_transactions.DataSource = dt;
                CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_trans_refresh_Click(object sender, EventArgs e)
        {
            string bank_id = txt_id.Text;
            if (bank_id != "")
            {
                load_banks_transactions_grid(int.Parse(bank_id));
                
            }
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            string bank_id = txt_id.Text;
            int bank_account_code = (cmb_GL_account_code.SelectedValue == null ? 0 : int.Parse(cmb_GL_account_code.SelectedValue.ToString()));
            if (bank_id != "")
            {
                frm_bank_payment obj = new frm_bank_payment(this, int.Parse(bank_id), bank_account_code);
                obj.ShowDialog();
                
            }
        }
        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_banks_transactions.Rows[grid_banks_transactions.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_banks_transactions.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }

        public void load_bank_detail(int id)
        {
            BankBLL objBLL = new BankBLL();
            DataTable dt = objBLL.SearchRecordByBankID(id);
            foreach (DataRow myProductView in dt.Rows)
            {
                txt_id.Text = myProductView["id"].ToString();
                txt_name.Text = myProductView["name"].ToString();
                txt_accountNo.Text = myProductView["accountNo"].ToString();
                txt_holderName.Text = myProductView["holderName"].ToString();
                txt_bankBranch.Text = myProductView["bankBranch"].ToString();
                cmb_GL_account_code.SelectedValue = myProductView["GLAccountID"].ToString();
            }
            lbl_bank_name.Visible = true;
            lbl_bank_name.Text = txt_name.Text;
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            string bank_id = txt_id.Text;

            if (bank_id != "")
            {
                load_bank_detail(int.Parse(bank_id));

            }
        }
    }
}
