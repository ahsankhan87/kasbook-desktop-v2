using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_period_close_wizard : Form
    {
        private readonly int _periodId;
        private readonly string _periodName;
        private readonly FinancialPeriodBLL _bll = new FinancialPeriodBLL();

        public FinancialPeriodCloseOptionsModal CloseOptions { get; private set; }

        public frm_period_close_wizard(int periodId, string periodName)
        {
            _periodId = periodId;
            _periodName = periodName;
            InitializeComponent();
        }

        private void frm_period_close_wizard_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            lblTitle.Text = string.Format("Month-End Closing Wizard - {0}", _periodName);
            lblConfirmText.Text = string.Format("I confirm all transactions for {0} have been reviewed", _periodName);
            tabSteps.SelectedIndex = 0;
            UpdateStepButtons();
            LoadStep1Checklist();
            LoadStep2Summary();
        }

        private void LoadStep1Checklist()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Checking period readiness...", "جاري التحقق من جاهزية الفترة...")))
                {
                    gridChecklist.DataSource = _bll.GetCloseChecklist(_periodId);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void LoadStep2Summary()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading period summary...", "جاري تحميل ملخص الفترة...")))
                {
                    DataTable dt = _bll.GetPeriodSummary(_periodId);
                    if (dt.Rows.Count == 0)
                    {
                        return;
                    }

                    DataRow row = dt.Rows[0];
                    lblTotalTransactionsValue.Text = Convert.ToString(row["total_transactions"]);
                    lblTotalJournalsValue.Text = Convert.ToString(row["total_journals"]);
                    lblDebitsValue.Text = Convert.ToDecimal(row["total_debits"]).ToString("N2");
                    lblCreditsValue.Text = Convert.ToDecimal(row["total_credits"]).ToString("N2");
                    lblNetProfitValue.Text = Convert.ToDecimal(row["net_profit"]).ToString("N2");
                    lblOutOfBalanceValue.Text = Convert.ToString(row["out_of_balance_entries"]);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (tabSteps.SelectedIndex == 0 && HasChecklistFailures())
            {
                UiMessages.ShowInfo(
                    "Checklist still has failed items. Fix issues before continuing.",
                    "لا تزال هناك عناصر فاشلة في قائمة التحقق. الرجاء إصلاحها قبل المتابعة.",
                    "Checklist",
                    "قائمة التحقق");
                return;
            }

            if (tabSteps.SelectedIndex < tabSteps.TabCount - 1)
            {
                tabSteps.SelectedIndex++;
            }

            UpdateStepButtons();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (tabSteps.SelectedIndex > 0)
            {
                tabSteps.SelectedIndex--;
            }

            UpdateStepButtons();
        }

        private void UpdateStepButtons()
        {
            btnBack.Enabled = tabSteps.SelectedIndex > 0;
            btnNext.Enabled = tabSteps.SelectedIndex < tabSteps.TabCount - 1;
            btnCompleteClosing.Enabled = tabSteps.SelectedIndex == tabSteps.TabCount - 1;
        }

        private bool HasChecklistFailures()
        {
            foreach (DataGridViewRow row in gridChecklist.Rows)
            {
                bool isPassed = row.Cells["colChecklistPassed"].Value != DBNull.Value && Convert.ToBoolean(row.Cells["colChecklistPassed"].Value);
                if (!isPassed)
                {
                    return true;
                }
            }

            return false;
        }

        private void gridChecklist_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (gridChecklist.Columns[e.ColumnIndex].Name == "colChecklistStatus")
            {
                bool isPassed = gridChecklist.Rows[e.RowIndex].Cells["colChecklistPassed"].Value != DBNull.Value &&
                                Convert.ToBoolean(gridChecklist.Rows[e.RowIndex].Cells["colChecklistPassed"].Value);
                e.Value = isPassed ? "✓" : "✗";
                e.CellStyle.ForeColor = isPassed ? Color.ForestGreen : Color.Firebrick;
                e.CellStyle.Font = new Font(gridChecklist.Font, FontStyle.Bold);
                e.FormattingApplied = true;
            }
        }

        private void gridChecklist_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || gridChecklist.Columns[e.ColumnIndex].Name != "colFixIssues")
            {
                return;
            }

            string module = Convert.ToString(gridChecklist.Rows[e.RowIndex].Cells["colFixModule"].Value);
            UiMessages.ShowInfo(
                string.Format("Open module: {0}", module),
                string.Format("افتح الوحدة: {0}", module),
                "Fix Issues",
                "إصلاح المشاكل");
        }

        private void btnCompleteClosing_Click(object sender, EventArgs e)
        {
            if (!chkConfirmReviewed.Checked)
            {
                UiMessages.ShowInfo(
                    "Please confirm the period review before completing close.",
                    "يرجى تأكيد مراجعة الفترة قبل الإغلاق.",
                    "Confirmation Required",
                    "التأكيد مطلوب");
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPinPassword.Text))
            {
                UiMessages.ShowInfo(
                    "Password/PIN confirmation is required.",
                    "تأكيد كلمة المرور/الرقم السري مطلوب.",
                    "Security Check",
                    "تحقق أمني");
                txtPinPassword.Focus();
                return;
            }

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Closing period...", "جاري إغلاق الفترة...")))
                {
                    CloseOptions = new FinancialPeriodCloseOptionsModal
                    {
                        period_id = _periodId,
                        user_id = UsersModal.logged_in_userid,
                        close_type = "Soft",
                        auto_post_depreciation = chkAutoDepreciation.Checked,
                        reverse_prior_accruals = chkReverseAccruals.Checked,
                        confirmation_text = lblConfirmText.Text,
                        pin_or_password = txtPinPassword.Text.Trim()
                    };

                    FinancialPeriodCloseResultModal result = _bll.SoftClosePeriod(CloseOptions);
                    if (!result.success)
                    {
                        UiMessages.ShowInfo(result.message, result.message, "Close Period", "إغلاق الفترة");
                        return;
                    }
                }

                UiMessages.ShowInfo(
                    "Period closed successfully.",
                    "تم إغلاق الفترة بنجاح.",
                    "Success",
                    "نجاح");

                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void tabSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateStepButtons();
        }
    }
}
