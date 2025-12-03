using pos.Security.Authorization;
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

namespace pos
{
    public partial class frm_journal_entries : Form
    {
        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public double _dr_total = 0;
        public double _cr_total = 0;

        public frm_journal_entries()
        {
            InitializeComponent();
           
        }

        private void frm_journal_entries_Load(object sender, EventArgs e)
        {
            // permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Journal_Create)) { 
                MessageBox.Show("You do not have permission to access this module.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            Load_account_autocomplete();
            GetMAXInvoiceNo();
        }

        public void GetMAXInvoiceNo()
        {
            JournalsBLL JournalsBLL_obj = new JournalsBLL();
            txt_invoice_no.Text = JournalsBLL_obj.GetMaxInvoiceNo();
        }

        private void Load_account_autocomplete()
        {
            try
            {
                grid_journal.DataSource = null;
                
                GeneralBLL objBLL = new GeneralBLL();

                String keyword = "id,name";
                String table = "acc_accounts";

                DataTable accounts_dt = new DataTable();
                accounts_dt = objBLL.GetRecord(keyword, table);


                for(int i=0; i <= 20; i++)
                {
                    int n = grid_journal.Rows.Add();
                    var accountComboCell = new DataGridViewComboBoxCell();

                    accountComboCell.DataSource = accounts_dt;

                    accountComboCell.DisplayMember = "name";
                    accountComboCell.ValueMember = "id";
                    
                    grid_journal.Rows[n].Cells["account"] = accountComboCell;
                }
                //bind data in data grid view  
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                // Permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Journal_Create))
                {
                    MessageBox.Show("You do not have permission to perform this action.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // check debit and credit balance
                if (_dr_total == _cr_total)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to sale", "Sale Transaction", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        if (grid_journal.Rows.Count > 1)
                        {
                            JournalsModal JournalsModal_obj = new JournalsModal();
                            JournalsBLL JournalsObj = new JournalsBLL();

                            JournalsModal_obj.invoice_no = txt_invoice_no.Text;

                            //set the date from datetimepicker and set time to te current time
                            DateTime now = DateTime.Now;
                            txt_entry_date.Value = new DateTime(txt_entry_date.Value.Year, txt_entry_date.Value.Month, txt_entry_date.Value.Day, now.Hour, now.Minute, now.Second);
                            /////////////////////

                            JournalsModal_obj.entry_date = txt_entry_date.Value;
                            int journal_id = 0;

                            for (int i = 0; i < grid_journal.Rows.Count; i++)
                            {
                                if (grid_journal.Rows[i].Cells["account"].Value != null)
                                {
                                    if (!String.IsNullOrEmpty(grid_journal.Rows[i].Cells["debit_amount"].Value as String))
                                    {
                                        JournalsModal_obj.debit = Convert.ToDouble(grid_journal.Rows[i].Cells["debit_amount"].Value.ToString());

                                    }
                                    else { JournalsModal_obj.debit = 0; }

                                    if (!String.IsNullOrEmpty(grid_journal.Rows[i].Cells["credit_amount"].Value as String))
                                    {
                                        JournalsModal_obj.credit = Convert.ToDouble(grid_journal.Rows[i].Cells["credit_amount"].Value.ToString());
                                    }
                                    else { JournalsModal_obj.credit = 0; }

                                    JournalsModal_obj.account_id = Convert.ToInt32(grid_journal.Rows[i].Cells["account"].Value.ToString());
                                    //JournalsModal_obj.account_name = grid_journal.Rows[i].Cells["account"].FormattedValue.ToString();

                                    if (!String.IsNullOrEmpty(grid_journal.Rows[i].Cells["description"].Value as String))
                                    {
                                        JournalsModal_obj.description = grid_journal.Rows[i].Cells["description"].Value.ToString();
                                    }
                                    else { JournalsModal_obj.description = ""; }

                                    journal_id = JournalsObj.Insert(JournalsModal_obj);
                                }

                            }

                            if (journal_id > 0)
                            {
                                MessageBox.Show("Record Saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                // CLEAR ALL FORM TEXTBOXES, GRID AND EVERYTING
                                clear_form();
                                GetMAXInvoiceNo();

                            }
                            else
                            {
                                MessageBox.Show("Record not saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Please add products", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Debit and Credit amount must be balance", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void frm_journal_entries_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

                if (e.KeyCode == Keys.F3)
                {
                    btn_save.PerformClick();
                }
                
                if (e.KeyCode == Keys.F1)
                {
                    btn_new.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want new transaction", "New Transaction", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                clear_form();
            }
        }

        private void clear_form()
        {
            //txt_invoice_no .Text = "";
            txt_entry_date.Text = "";
            txt_cr_total.Text = "";
            txt_dr_total.Text = "";
            
            _dr_total = 0;
            _cr_total = 0;
            
            grid_journal.DataSource = null;
            grid_journal.Rows.Clear();
            grid_journal.Refresh();
            Load_account_autocomplete();
        }

        private void grid_journal_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if(e.Control is DataGridViewComboBoxEditingControl)
            {
                ((ComboBox)e.Control).DropDownStyle = ComboBoxStyle.DropDown;
                ((ComboBox)e.Control).AutoCompleteSource = AutoCompleteSource.ListItems;
                ((ComboBox)e.Control).AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            }
        }

        private void grid_journal_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                if (grid_journal.CurrentRow.Cells["debit_amount"].Value != null)
                {
                    grid_journal.CurrentRow.Cells["credit_amount"].ReadOnly = true;
                    grid_journal.CurrentRow.Cells["credit_amount"].Style.BackColor = Color.LightGray;
                     
                }
                else
                {
                    grid_journal.CurrentRow.Cells["credit_amount"].ReadOnly = false;
                    grid_journal.CurrentRow.Cells["credit_amount"].Style.BackColor = Color.White;
                }
                
            }
            if (e.ColumnIndex == 2)
            {
                if (grid_journal.CurrentRow.Cells["credit_amount"].Value != null)
                {
                    grid_journal.CurrentRow.Cells["debit_amount"].ReadOnly = true;
                    grid_journal.CurrentRow.Cells["debit_amount"].Style.BackColor = Color.LightGray;
                }
                else
                {
                    grid_journal.CurrentRow.Cells["debit_amount"].ReadOnly = false;
                    grid_journal.CurrentRow.Cells["debit_amount"].Style.BackColor = Color.White;
                }
            }

            get_total_balances();
        }

        private void get_total_balances()
        {
            _dr_total = 0;
            _cr_total = 0;

            for (int i = 0; i <= grid_journal.RowCount - 1; i++)
            {
                if (grid_journal.Rows[i].Cells["debit_amount"].Value != null)
                {
                    _dr_total += Convert.ToDouble(grid_journal.Rows[i].Cells["debit_amount"].Value);
                }

                if (grid_journal.Rows[i].Cells["credit_amount"].Value != null)
                {
                    _cr_total += Convert.ToDouble(grid_journal.Rows[i].Cells["credit_amount"].Value);
                }
                
            }
            txt_dr_total.Text = _dr_total.ToString();
            txt_cr_total.Text = _cr_total.ToString();
        }
    }
}
