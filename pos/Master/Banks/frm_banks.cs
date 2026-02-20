using pos.Reports.Banks;
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
            try
            {
                AppTheme.Apply(this);
                StyleBanksForm();

                using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
                {
                    get_accounts_dropdownlist();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void StyleBanksForm()
        {
            panel1.BackColor = AppTheme.PrimaryDark;
            panel1.ForeColor = AppTheme.TextOnPrimary;
            panel1.Padding = new Padding(8, 4, 8, 4);

            panel2.BackColor = SystemColors.Control;
            panel2.AutoScroll = true;

            panel3.BackColor = SystemColors.Control;

            foreach (Label lbl in panel1.Controls.OfType<Label>())
                lbl.ForeColor = AppTheme.TextOnPrimary;

            tabControl1.Font = AppTheme.FontTab;
            Detail.BackColor = SystemColors.Control;
            Transactions.BackColor = SystemColors.Control;

            groupBox2.Font = AppTheme.FontGroupBox;

            StyleLedgerGrid(grid_banks_transactions);
        }

        private static void StyleLedgerGrid(DataGridView grid)
        {
            if (grid == null) return;

            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.SetProperty,
                null, grid, new object[] { true });

            grid.BackgroundColor = SystemColors.AppWorkspace;
            grid.BorderStyle = BorderStyle.None;
            grid.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            grid.GridColor = SystemColors.ControlLight;
            grid.RowHeadersVisible = false;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid.ColumnHeadersHeight = 34;
            grid.RowTemplate.Height = 30;

            grid.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Control,
                ForeColor = SystemColors.ControlText,
                Font = AppTheme.FontGridHeader,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText,
                Alignment = DataGridViewContentAlignment.MiddleLeft,
                Padding = new Padding(6, 4, 6, 4)
            };

            grid.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Window,
                ForeColor = AppTheme.TextPrimary,
                Font = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                Padding = new Padding(6, 2, 6, 2)
            };

            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = AppTheme.GridAltRow,
                ForeColor = AppTheme.TextPrimary,
                Font = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText
            };
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

            cmb_GL_account_code.SelectedValue = "19"; // 19 is the default Bank Account id in acc_accounts table

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_name.Text))
                {
                    UiMessages.ShowInfo(
                        "Bank name is required.",
                        "اسم البنك مطلوب.",
                        "Validation",
                        "التحقق"
                    );
                    txt_name.Focus();
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Save this bank?",
                    "هل تريد حفظ هذا البنك؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );
                if (confirm != DialogResult.Yes) return;

                using (BusyScope.Show(this, UiMessages.T("Saving...", "جاري الحفظ...")))
                {
                    BankModal info = new BankModal
                    {
                        name = txt_name.Text.Trim(),
                        holderName = txt_holderName.Text,
                        accountNo = txt_accountNo.Text,
                        bankBranch = txt_bankBranch.Text,
                        GLAccountID = (cmb_GL_account_code.SelectedValue == null ? "" : cmb_GL_account_code.SelectedValue.ToString())
                    };

                    BankBLL objBLL = new BankBLL();
                    int result = objBLL.Insert(info);
                    if (result > 0)
                    {
                        UiMessages.ShowInfo(
                            "Bank has been created successfully.",
                            "تم إنشاء البنك بنجاح.",
                            "Success",
                            "نجاح"
                        );
                        clear_all();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Bank could not be saved. Please try again.",
                            "تعذر حفظ البنك. يرجى المحاولة مرة أخرى.",
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
                    UiMessages.ShowInfo(
                        "Please select a bank record to update.",
                        "يرجى اختيار سجل بنك للتحديث.",
                        "Not Found",
                        "غير موجود"
                    );
                    return;
                }

                if (string.IsNullOrWhiteSpace(txt_name.Text))
                {
                    UiMessages.ShowInfo(
                        "Bank name is required.",
                        "اسم البنك مطلوب.",
                        "Validation",
                        "التحقق"
                    );
                    txt_name.Focus();
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Update this bank?",
                    "هل تريد تحديث هذا البنك؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );
                if (confirm != DialogResult.Yes) return;

                using (BusyScope.Show(this, UiMessages.T("Updating...", "جاري التحديث...")))
                {
                    BankModal info = new BankModal
                    {
                        name = txt_name.Text.Trim(),
                        holderName = txt_holderName.Text,
                        accountNo = txt_accountNo.Text,
                        bankBranch = txt_bankBranch.Text,
                        GLAccountID = (cmb_GL_account_code.SelectedValue == null ? "" : cmb_GL_account_code.SelectedValue.ToString()),
                        id = int.Parse(txt_id.Text)
                    };

                    BankBLL objBLL = new BankBLL();
                    int result = objBLL.Update(info);
                    if (result > 0)
                    {
                        UiMessages.ShowInfo(
                            "Bank has been updated successfully.",
                            "تم تحديث البنك بنجاح.",
                            "Success",
                            "نجاح"
                        );
                        clear_all();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Bank could not be updated. Please try again.",
                            "تعذر تحديث البنك. يرجى المحاولة مرة أخرى.",
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

        private void btn_delete_Click(object sender, EventArgs e)
        {
            string id = txt_id.Text;

            if (string.IsNullOrWhiteSpace(id))
            {
                UiMessages.ShowInfo(
                    "Please select a bank record to delete.",
                    "يرجى اختيار سجل بنك للحذف.",
                    "Delete",
                    "حذف"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this bank? This action cannot be undone.",
                "هل تريد حذف هذا البنك؟ لا يمكن التراجع عن هذا الإجراء.",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );
            if (confirm != DialogResult.Yes) return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    BankBLL objBLL = new BankBLL();
                    objBLL.Delete(int.Parse(id));

                    UiMessages.ShowInfo(
                        "Bank has been deleted successfully.",
                        "تم حذف البنك بنجاح.",
                        "Deleted",
                        "تم الحذف"
                    );
                    clear_all();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        public void load_banks_transactions_grid(int bank_id)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading bank transactions...", "جاري تحميل حركات البنك...")))
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
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
                throw;
            }

        }

        private void btn_trans_refresh_Click(object sender, EventArgs e)
        {
            int bankId;
            if (int.TryParse(txt_id.Text, out bankId) && bankId > 0)
            {
                load_banks_transactions_grid(bankId);
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a bank first.",
                    "يرجى اختيار بنك أولاً.",
                    "Bank",
                    "البنك"
                );
            }
        }

        private void btn_payment_Click(object sender, EventArgs e)
        {
            int bankId;
            if (!int.TryParse(txt_id.Text, out bankId) || bankId <= 0)
            {
                UiMessages.ShowInfo(
                    "Please select a bank first.",
                    "يرجى اختيار بنك أولاً.",
                    "Bank",
                    "البنك"
                );
                return;
            }

            string bankName = lbl_bank_name.Text;
            int bank_account_code = (cmb_GL_account_code.SelectedValue == null ? 0 : int.Parse(cmb_GL_account_code.SelectedValue.ToString()));

            var confirm = UiMessages.ConfirmYesNo(
                "Open bank payment window?",
                "هل تريد فتح شاشة دفع البنك؟",
                captionEn: "Confirm",
                captionAr: "تأكيد"
            );
            if (confirm != DialogResult.Yes) return;

            frm_bank_payment obj = new frm_bank_payment(this, bankId, bank_account_code, bankName);
            obj.ShowDialog();
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            int bankId;
            if (int.TryParse(txt_id.Text, out bankId) && bankId > 0)
            {
                load_bank_detail(bankId);
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a bank first.",
                    "يرجى اختيار بنك أولاً.",
                    "Bank",
                    "البنك"
                );
            }
        }

        private void Btn_deposit_Click(object sender, EventArgs e)
        {
            int bankId;
            if (!int.TryParse(txt_id.Text, out bankId) || bankId <= 0)
            {
                UiMessages.ShowInfo(
                    "Please select a bank first.",
                    "يرجى اختيار بنك أولاً.",
                    "Bank",
                    "البنك"
                );
                return;
            }

            string bankName = lbl_bank_name.Text;
            int bank_account_code = (cmb_GL_account_code.SelectedValue == null ? 0 : int.Parse(cmb_GL_account_code.SelectedValue.ToString()));

            var confirm = UiMessages.ConfirmYesNo(
                "Open deposit window?",
                "هل تريد فتح شاشة الإيداع؟",
                captionEn: "Confirm",
                captionAr: "تأكيد"
            );
            if (confirm != DialogResult.Yes) return;

            frm_deposit_to_bank obj = new frm_deposit_to_bank(this, bankId, bank_account_code, bankName);
            obj.ShowDialog();
        }

        private void Btn_bank_report_Click(object sender, EventArgs e)
        {
            try
            {
                int bankId;
                if (!int.TryParse(txt_id.Text, out bankId) || bankId <= 0)
                {
                    UiMessages.ShowInfo(
                        "Please select a bank to view the report.",
                        "يرجى اختيار بنك لعرض التقرير.",
                        "Bank Report",
                        "تقرير البنك"
                    );
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Opening report...", "جاري فتح التقرير...")))
                {
                    frm_bankLedgerReport bankLedger = new frm_bankLedgerReport(bankId);
                    bankLedger.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
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
    }
}
