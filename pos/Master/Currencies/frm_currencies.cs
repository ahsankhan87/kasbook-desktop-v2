using POS.BLL;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_currencies : Form
    {
        public frm_currencies()
        {
            InitializeComponent();
        }

        private void frm_currencies_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            load_currencies_grid();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel2, lbl_title, panel1, grid_currencies, id);
        }

        public void load_currencies_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading currencies...", "جاري تحميل العملات...")))
                {
                    grid_currencies.DataSource = null;
                    CurrencyBLL objBLL = new CurrencyBLL();
                    grid_currencies.AutoGenerateColumns = false;
                    grid_currencies.DataSource = objBLL.GetAll();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addCurrency frm = new frm_addCurrency(this);
            frm_addCurrency.instance.tb_lbl_is_edit.Text = "false";
            frm.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_currencies.CurrentRow == null)
            {
                UiMessages.ShowInfo("Please select a currency to update.", "يرجى اختيار عملة للتحديث.", "Currencies", "العملات");
                return;
            }

            frm_addCurrency frm = new frm_addCurrency(this);
            frm_addCurrency.instance.tb_lbl_is_edit.Text = "true";
            frm_addCurrency.instance.tb_id.Text = Convert.ToString(grid_currencies.CurrentRow.Cells["id"].Value);
            frm_addCurrency.instance.tb_code.Text = Convert.ToString(grid_currencies.CurrentRow.Cells["code"].Value);
            frm_addCurrency.instance.tb_name.Text = Convert.ToString(grid_currencies.CurrentRow.Cells["name"].Value);
            frm_addCurrency.instance.tb_symbol.Text = Convert.ToString(grid_currencies.CurrentRow.Cells["symbol"].Value);
            frm_addCurrency.instance.tb_exchange_rate.Text = Convert.ToString(grid_currencies.CurrentRow.Cells["exchange_rate"].Value);
            bool isActive;
            bool.TryParse(Convert.ToString(grid_currencies.CurrentRow.Cells["is_active"].Value), out isActive);
            frm_addCurrency.instance.tb_is_active.Checked = isActive;
            frm_addCurrency.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_currencies.CurrentRow == null)
            {
                UiMessages.ShowInfo("Please select a currency to delete.", "يرجى اختيار عملة للحذف.", "Currencies", "العملات");
                return;
            }

            int currencyId;
            if (!int.TryParse(Convert.ToString(grid_currencies.CurrentRow.Cells["id"].Value), out currencyId) || currencyId <= 0)
            {
                UiMessages.ShowInfo("The selected record is not valid.", "السجل المحدد غير صالح.", "Currencies", "العملات");
                return;
            }

            if (UiMessages.ConfirmYesNo("Delete this currency?", "هل تريد حذف هذه العملة؟", "Confirm Delete", "تأكيد الحذف") != DialogResult.Yes) return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    CurrencyBLL objBLL = new CurrencyBLL();
                    objBLL.Delete(currencyId);
                }

                UiMessages.ShowInfo("Record deleted successfully.", "تم حذف السجل بنجاح.", "Deleted", "تم الحذف");
                load_currencies_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_currencies_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    CurrencyBLL objBLL = new CurrencyBLL();
                    grid_currencies.DataSource = objBLL.SearchRecord((txt_search.Text ?? string.Empty).Trim());
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) btn_search.PerformClick();
        }

        private void frm_currencies_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N) btn_new.PerformClick();
            if (e.Control && e.KeyCode == Keys.U) btn_update.PerformClick();
            if (e.Control && e.KeyCode == Keys.D) btn_delete.PerformClick();
        }
    }
}
