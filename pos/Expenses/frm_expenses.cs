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

namespace pos.Expenses
{
    public partial class frm_expenses : Form
    {
        public frm_expenses()
        {
            InitializeComponent();
        }

        private void frm_expenses_Load(object sender, EventArgs e)
        {
            get_cash_accounts_dropdownlist();
            cmb_cash_account.SelectedValue = "3";
            get_vat_accounts_dropdownlist();
            cmb_vat_account.SelectedValue = "7";
            get_expense_accounts_dropdownlist();
        }

        public void get_expense_accounts_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts WHERE group_id = 12"; // 12 is operative expense group id

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            cmb_account_code.DataSource = dt;

            cmb_account_code.DisplayMember = "name";
            cmb_account_code.ValueMember = "id";
        }

        private void btn_add_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmb_account_code.Items.Count > 0)
                {
                    
                    string account_code = cmb_account_code.SelectedValue.ToString();
                    string account = cmb_account_code.GetItemText(cmb_account_code.SelectedItem).ToString();
                    string amount = "0";
                    string vat = "";
                    string desc = "";
                    string total = "";

                    string[] row0 = { account_code, account, amount, vat, desc,total };
                    
                    grid_expenses.Rows.Add(row0);

                    fill_vat_grid_combo(grid_expenses.RowCount-1);

                }
                else
                {
                    MessageBox.Show("Please select account", "Expenses", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            }
        }

        public void fill_vat_grid_combo(int RowIndex)
        {
            DataTable dt = new DataTable();
            var vatComboCell = new DataGridViewComboBoxCell();
            GeneralBLL generalBLL_obj = new GeneralBLL();
            
            string keyword1 = "*";
            string table1 = "pos_taxes";

            dt = generalBLL_obj.GetRecord(keyword1, table1);

            ///////////

            vatComboCell.DataSource = dt;
            vatComboCell.DisplayMember = "title";
            vatComboCell.ValueMember = "rate";

            grid_expenses.Rows[RowIndex].Cells["vat"] = vatComboCell;
            //grid_expenses.Rows[RowIndex].Cells["location_code"].Value = SelectedValue;
            //grid_expenses.Rows[RowIndex].Cells["vat"].Value = dt.Rows[0]["title"].ToString(); // GET FIRST COLUMN OF DT TO SHOW FIRST VALUE AS SELECTED


        }

        private void grid_expenses_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Are you sure you want delete", "Delete", buttons, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        grid_expenses.Rows.RemoveAt(grid_expenses.CurrentRow.Index);

                      
                    }
                }

                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    e.SuppressKeyPress = true;
                    int iColumn = grid_expenses.CurrentCell.ColumnIndex;
                    int iRow = grid_expenses.CurrentCell.RowIndex;

                    if (iColumn <= 5)
                    {
                        
                            grid_expenses.CurrentCell = grid_expenses.Rows[iRow].Cells[iColumn + 1];
                            grid_expenses.Focus();
                            grid_expenses.CurrentCell.Selected = true;
                        
                    }
                    //else if (iColumn > 5)
                    //{
                    //    if (grid_expenses.Rows[iRow].Cells["code"].Value != null && grid_expenses.Rows[iRow].Cells["unit_price"].Value != null && grid_expenses.Rows[iRow].Cells["qty"].Value != null)
                    //    {
                    //        grid_expenses.Rows.Add();  //adds new row on last cell of row
                    //        this.ActiveControl = grid_expenses;
                    //        grid_expenses.CurrentCell = grid_expenses.Rows[iRow + 1].Cells["code"];
                    //        //grid_expenses.Rows[iRow + 1].Cells["code"].Value = product_code;
                    //        grid_expenses.CurrentCell.Selected = true;
                    //        grid_expenses.BeginEdit(true);
                    //    }

                    //}
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_expenses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_expenses.Columns[e.ColumnIndex].Name;
                if (name == "btn_delete")
                {
                    grid_expenses.Rows.RemoveAt(e.RowIndex);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want close", "Close Form", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void grid_expenses_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            double tax_rate = (grid_expenses.Rows[e.RowIndex].Cells["vat"].Value == null || grid_expenses.Rows[e.RowIndex].Cells["vat"].Value.ToString() == "" ? 0 : double.Parse(grid_expenses.Rows[e.RowIndex].Cells["vat"].Value.ToString()));
            double amount = Convert.ToDouble(grid_expenses.Rows[e.RowIndex].Cells["amount"].Value);
            double tax = (amount * tax_rate / 100);

            grid_expenses.Rows[e.RowIndex].Cells["total"].Value = amount + tax;

        }

        public void get_cash_accounts_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts"; 

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            cmb_cash_account.DataSource = dt;

            cmb_cash_account.DisplayMember = "name";
            cmb_cash_account.ValueMember = "id";
        }
        public void get_vat_accounts_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "acc_accounts"; 

            DataTable dt = generalBLL_obj.GetRecord(keyword, table);
            cmb_vat_account.DataSource = dt;

            cmb_vat_account.DisplayMember = "name";
            cmb_vat_account.ValueMember = "id";

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are you sure you want to ?", "Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    if (grid_expenses.Rows.Count > 0)
                    {
                        List<ExpenseModal_Header> model_header = new List<ExpenseModal_Header> { };

                        ExpenseBLL salesObj = new ExpenseBLL();
                        string invoice_no = salesObj.GetMaxInvoiceNo();

                        for (int i = 0; i < grid_expenses.Rows.Count; i++)
                        {
                            /////Added sales header into the List
                            model_header.Add(new ExpenseModal_Header
                            {

                                cash_account = (cmb_cash_account.SelectedValue.ToString() == null ? "" : cmb_cash_account.SelectedValue.ToString()),
                                vat_account = (cmb_vat_account.SelectedValue.ToString() == null ? "" : cmb_vat_account.SelectedValue.ToString()),
                                invoice_no = invoice_no,
                                sale_date = txt_sale_date.Value.Date,
                                expense_account = grid_expenses.Rows[i].Cells["account_code"].Value.ToString(),
                                amount = Convert.ToDouble(grid_expenses.Rows[i].Cells["amount"].Value.ToString()),
                                vat = (grid_expenses.Rows[i].Cells["vat"].Value == null ? 0 : Convert.ToDouble(grid_expenses.Rows[i].Cells["vat"].Value.ToString())),
                                description = grid_expenses.Rows[i].Cells["description"].Value.ToString(),
                                expense_account_name = grid_expenses.Rows[i].Cells["account"].Value.ToString(),
                            });
                            //////
                        }

                        var sale_id = salesObj.Insert(model_header);// for sales items
                        if (sale_id.ToString().Length > 0)
                        {
                            MessageBox.Show("Transaction created successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clear_form();
                        }
                        else
                        {
                            MessageBox.Show("Record not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void clear_form()
        {
            grid_expenses.DataSource = null;
            grid_expenses.Rows.Clear();
            grid_expenses.Refresh();
        }

        private void frm_expenses_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.F3)
            {
                btn_save.PerformClick();
            }
        }
    }

}
