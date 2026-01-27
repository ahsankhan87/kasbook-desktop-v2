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
using pos.UI;
using pos.UI.Busy;

namespace pos.Expenses
{
    public partial class frm_expenses : Form
    {
        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        private readonly Timer _calcDebounce = new Timer();
        private const int CalcDebounceMs = 120;
        private DataGridViewCellEventArgs _pendingCellEndEdit;

        public frm_expenses()
        {
            InitializeComponent();
        }

        private void frm_expenses_Load(object sender, EventArgs e)
        {
            // debounce for totals calculation
            _calcDebounce.Interval = CalcDebounceMs;
            _calcDebounce.Tick += CalcDebounce_Tick;

            // permission check
            if (!_auth.HasPermission(_currentUser, Permissions.Expenses_Create))
            {
                UiMessages.ShowWarning(
                    "You do not have permission to access Expenses.",
                    "ليس لديك صلاحية للوصول إلى المصروفات.",
                    "Access Denied",
                    "تم رفض الوصول"
                );
                // Keep form open but limit actions via save permission check.
            }

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
                {
                    get_cash_accounts_dropdownlist();
                    cmb_cash_account.SelectedValue = "3";
                    get_vat_accounts_dropdownlist();
                    cmb_vat_account.SelectedValue = "7";
                    get_expense_accounts_dropdownlist();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
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

                    string[] row0 = { account_code, account, amount, vat, desc, total };

                    grid_expenses.Rows.Add(row0);

                    fill_vat_grid_combo(grid_expenses.RowCount - 1);
                }
                else
                {
                    UiMessages.ShowInfo(
                        "Please select an expense account.",
                        "يرجى اختيار حساب المصروف.",
                        "Expenses",
                        "المصروفات"
                    );
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
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
                    var confirm = UiMessages.ConfirmYesNo(
                        "Delete the selected row?",
                        "هل تريد حذف الصف المحدد؟",
                        captionEn: "Confirm Delete",
                        captionAr: "تأكيد الحذف"
                    );

                    if (confirm == DialogResult.Yes)
                    {
                        if (grid_expenses.CurrentRow != null)
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
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void grid_expenses_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_expenses.Columns[e.ColumnIndex].Name;
                if (name == "btn_delete")
                {
                    var confirm = UiMessages.ConfirmYesNo(
                        "Delete the selected row?",
                        "هل تريد حذف الصف المحدد؟",
                        captionEn: "Confirm Delete",
                        captionAr: "تأكيد الحذف"
                    );

                    if (confirm == DialogResult.Yes)
                        grid_expenses.Rows.RemoveAt(e.RowIndex);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            var confirm = UiMessages.ConfirmYesNo(
                "Close this window?",
                "هل تريد إغلاق النافذة؟",
                captionEn: "Close",
                captionAr: "إغلاق"
            );

            if (confirm == DialogResult.Yes)
                this.Close();
        }

        private void grid_expenses_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            // debounce calculation to avoid repeated parsing while edits are in progress
            _pendingCellEndEdit = e;
            _calcDebounce.Stop();
            _calcDebounce.Start();
        }

        private void CalcDebounce_Tick(object sender, EventArgs e)
        {
            _calcDebounce.Stop();

            if (_pendingCellEndEdit == null)
                return;

            try
            {
                int rowIndex = _pendingCellEndEdit.RowIndex;
                if (rowIndex < 0 || rowIndex >= grid_expenses.Rows.Count)
                    return;

                double tax_rate = (grid_expenses.Rows[rowIndex].Cells["vat"].Value == null || grid_expenses.Rows[rowIndex].Cells["vat"].Value.ToString() == "" ? 0 : double.Parse(grid_expenses.Rows[rowIndex].Cells["vat"].Value.ToString()));

                double amount = 0;
                var amountObj = grid_expenses.Rows[rowIndex].Cells["amount"].Value;
                if (amountObj != null)
                {
                    double.TryParse(amountObj.ToString(), out amount);
                }

                double tax = (amount * tax_rate / 100);
                grid_expenses.Rows[rowIndex].Cells["total"].Value = amount + tax;
            }
            catch
            {
                // keep silent; user may still be editing values
            }
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
                // Permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Expenses_Create))
                {
                    UiMessages.ShowWarning(
                        "You do not have permission to perform this action.",
                        "ليس لديك صلاحية لتنفيذ هذا الإجراء.",
                        "Access Denied",
                        "تم رفض الوصول"
                    );
                    return;
                }

                if (grid_expenses.Rows.Count <= 0)
                {
                    UiMessages.ShowInfo(
                        "Please add at least one expense line.",
                        "يرجى إضافة بند مصروف واحد على الأقل.",
                        "Expenses",
                        "المصروفات"
                    );
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Save this expense transaction?",
                    "هل تريد حفظ حركة المصروفات؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T("Saving...", "جاري الحفظ...")))
                {
                    List<ExpenseModal_Header> model_header = new List<ExpenseModal_Header> { };

                    ExpenseBLL salesObj = new ExpenseBLL();
                    string invoice_no = salesObj.GetMaxInvoiceNo();

                    for (int i = 0; i < grid_expenses.Rows.Count; i++)
                    {
                        if (grid_expenses.Rows[i].IsNewRow) continue;
                        if (grid_expenses.Rows[i].Cells["account_code"].Value == null) continue;

                        model_header.Add(new ExpenseModal_Header
                        {
                            cash_account = (cmb_cash_account.SelectedValue == null ? "" : cmb_cash_account.SelectedValue.ToString()),
                            vat_account = (cmb_vat_account.SelectedValue == null ? "" : cmb_vat_account.SelectedValue.ToString()),
                            invoice_no = invoice_no,
                            sale_date = txt_sale_date.Value.Date,
                            expense_account = Convert.ToString(grid_expenses.Rows[i].Cells["account_code"].Value),
                            amount = (grid_expenses.Rows[i].Cells["amount"].Value == null ? 0 : Convert.ToDouble(grid_expenses.Rows[i].Cells["amount"].Value.ToString())),
                            vat = (grid_expenses.Rows[i].Cells["vat"].Value == null ? 0 : Convert.ToDouble(grid_expenses.Rows[i].Cells["vat"].Value.ToString())),
                            description = (grid_expenses.Rows[i].Cells["description"].Value == null ? "" : grid_expenses.Rows[i].Cells["description"].Value.ToString()),
                            expense_account_name = (grid_expenses.Rows[i].Cells["account"].Value == null ? "" : grid_expenses.Rows[i].Cells["account"].Value.ToString()),
                        });
                    }

                    var sale_id = salesObj.Insert(model_header);
                    if (sale_id.ToString().Length > 0)
                    {
                        UiMessages.ShowInfo(
                            "Expense transaction has been saved successfully.",
                            "تم حفظ حركة المصروفات بنجاح.",
                            "Success",
                            "نجاح"
                        );
                        clear_form();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Expense transaction could not be saved. Please try again.",
                            "تعذر حفظ حركة المصروفات. يرجى المحاولة مرة أخرى.",
                            "Error",
                            "خطأ"
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
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
            if (e.KeyData == Keys.Escape)
            {
                btn_close.PerformClick();
            }
        }
    }

}
