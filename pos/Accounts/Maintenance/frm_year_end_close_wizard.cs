using POS.BLL;
using POS.Core;
using pos.UI;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_year_end_close_wizard : Form
    {
        private readonly int _yearId;
        private readonly string _yearName;
        private readonly FinancialPeriodBLL _bll = new FinancialPeriodBLL();
        private bool _isRunning;

        public frm_year_end_close_wizard(int yearId, string yearName)
        {
            _yearId = yearId;
            _yearName = string.IsNullOrWhiteSpace(yearName) ? yearId.ToString() : yearName;
            InitializeComponent();
        }

        private void frm_year_end_close_wizard_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            lblYearValue.Text = _yearName;
            lblResultValue.Text = "Not executed yet.";

            _bll.YearEndCloseProgressChanged += OnYearEndCloseProgressChanged;
            LoadValidationReport();
        }

        private void frm_year_end_close_wizard_FormClosed(object sender, FormClosedEventArgs e)
        {
            _bll.YearEndCloseProgressChanged -= OnYearEndCloseProgressChanged;
        }

        private void OnYearEndCloseProgressChanged(object sender, YearEndCloseProgressEventArgs e)
        {
            AppendProgress(e.StepMessage);
        }

        private void AppendProgress(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return;
            }

            if (lstProgress.InvokeRequired)
            {
                lstProgress.BeginInvoke(new Action<string>(AppendProgress), message);
                return;
            }

            lstProgress.Items.Add(string.Format("[{0}] {1}", DateTime.Now.ToString("HH:mm:ss"), message.Trim()));
            lstProgress.TopIndex = Math.Max(0, lstProgress.Items.Count - 1);
        }

        private void LoadValidationReport()
        {
            try
            {
                DataTable dt = _bll.GetYearEndPreCloseValidationReport(_yearId);
                gridValidation.AutoGenerateColumns = false;
                gridValidation.DataSource = dt;
                ColorValidationRows();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void ColorValidationRows()
        {
            foreach (DataGridViewRow row in gridValidation.Rows)
            {
                bool passed = row.Cells["colCheckPassed"].Value != DBNull.Value && Convert.ToBoolean(row.Cells["colCheckPassed"].Value);
                row.Cells["colCheckStatus"].Value = passed ? "Passed" : "Failed";
                row.DefaultCellStyle.BackColor = passed ? Color.FromArgb(239, 250, 242) : Color.FromArgb(253, 239, 239);
                row.DefaultCellStyle.SelectionBackColor = passed ? Color.FromArgb(205, 239, 214) : Color.FromArgb(245, 206, 206);
            }
        }

        private bool HasValidationFailures()
        {
            foreach (DataGridViewRow row in gridValidation.Rows)
            {
                bool passed = row.Cells["colCheckPassed"].Value != DBNull.Value && Convert.ToBoolean(row.Cells["colCheckPassed"].Value);
                if (!passed)
                {
                    return true;
                }
            }

            return false;
        }

        private void btnRefreshValidation_Click(object sender, EventArgs e)
        {
            LoadValidationReport();
            AppendProgress("Validation report refreshed.");
        }

        private async void btnExecuteClose_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                return;
            }

            if (HasValidationFailures())
            {
                UiMessages.ShowInfo(
                    "Validation has failed checks. Resolve them before closing year.",
                    "فشلت بعض عناصر التحقق. قم بإصلاحها قبل إقفال السنة.",
                    "Year-End Close",
                    "إقفال السنة");
                return;
            }

            if (!string.Equals((txtConfirmClose.Text ?? string.Empty).Trim(), "CLOSE YEAR", StringComparison.OrdinalIgnoreCase))
            {
                UiMessages.ShowInfo(
                    "Type CLOSE YEAR to confirm this critical action.",
                    "اكتب CLOSE YEAR لتأكيد هذه العملية الحساسة.",
                    "Confirmation Required",
                    "التأكيد مطلوب");
                txtConfirmClose.Focus();
                return;
            }

            DialogResult confirm = UiMessages.ConfirmYesNo(
                "This will perform final year-end closing entries and lock the old year. Continue?",
                "سيتم تنفيذ قيود إقفال السنة النهائية وقفل السنة القديمة. هل تريد المتابعة؟",
                "Year-End Close",
                "إقفال السنة");
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                ToggleRunningState(true);
                AppendProgress("Starting year-end close...");

                YearEndCloseResultModal result = await Task.Run(() => _bll.ExecuteYearEndClose(_yearId, UsersModal.logged_in_userid));
                DisplayCloseResult(result);

                if (result.success)
                {
                    txtConfirmClose.Clear();
                    LoadValidationReport();
                }
            }
            catch (Exception ex)
            {
                AppendProgress("Execution failed: " + ex.Message);
                UiMessages.ShowError(ex.Message, ex.Message);
            }
            finally
            {
                ToggleRunningState(false);
            }
        }

        private async void btnRollback_Click(object sender, EventArgs e)
        {
            if (_isRunning)
            {
                return;
            }

            string reason = PromptText("Rollback Reason", "Enter rollback reason:", true);
            if (string.IsNullOrWhiteSpace(reason))
            {
                return;
            }

            DialogResult confirm = UiMessages.ConfirmYesNo(
                "Rollback is admin-only and allowed within 7 days. Continue rollback?",
                "الاسترجاع متاح للمسؤول فقط وخلال 7 أيام. هل تريد المتابعة؟",
                "Rollback Year-End Close",
                "استرجاع إقفال السنة");
            if (confirm != DialogResult.Yes)
            {
                return;
            }

            try
            {
                ToggleRunningState(true);
                AppendProgress("Starting rollback...");

                YearEndRollbackResultModal result = await Task.Run(() => _bll.RollbackYearEndClose(_yearId, reason));
                AppendProgress(result.message);
                UiMessages.ShowInfo(result.message, result.message, "Rollback", "استرجاع");

                if (result.success)
                {
                    lblResultValue.Text = "Rolled back successfully.";
                    lblResultValue.ForeColor = Color.DarkOrange;
                    LoadValidationReport();
                }
            }
            catch (Exception ex)
            {
                AppendProgress("Rollback failed: " + ex.Message);
                UiMessages.ShowError(ex.Message, ex.Message);
            }
            finally
            {
                ToggleRunningState(false);
            }
        }

        private void DisplayCloseResult(YearEndCloseResultModal result)
        {
            if (result == null)
            {
                return;
            }

            string resultText = string.Format(
                "Status: {0}\r\nMessage: {1}\r\nRun ID: {2}\r\nNet P&L: {3:N2}\r\nClosing Voucher: {4}\r\nOpening Voucher: {5}",
                result.success ? "Completed" : "Failed",
                result.message,
                result.run_id,
                result.net_profit_loss,
                string.IsNullOrWhiteSpace(result.closing_voucher_no) ? "N/A" : result.closing_voucher_no,
                string.IsNullOrWhiteSpace(result.opening_voucher_no) ? "N/A" : result.opening_voucher_no);

            lblResultValue.Text = resultText;
            lblResultValue.ForeColor = result.success ? Color.ForestGreen : Color.Firebrick;

            if (result.pre_close_validation_report != null)
            {
                gridValidation.DataSource = result.pre_close_validation_report;
                ColorValidationRows();
            }

            AppendProgress(result.message);
        }

        private void ToggleRunningState(bool running)
        {
            _isRunning = running;
            btnExecuteClose.Enabled = !running;
            btnRollback.Enabled = !running;
            btnRefreshValidation.Enabled = !running;
            btnClose.Enabled = !running;
        }

        private static string PromptText(string title, string labelText, bool multiline)
        {
            Form prompt = new Form();
            prompt.Text = title;
            prompt.Size = new Size(520, multiline ? 240 : 190);
            prompt.StartPosition = FormStartPosition.CenterParent;
            prompt.FormBorderStyle = FormBorderStyle.FixedDialog;
            prompt.MinimizeBox = false;
            prompt.MaximizeBox = false;

            Label textLabel = new Label();
            textLabel.Text = labelText;
            textLabel.SetBounds(12, 12, 480, 24);

            TextBox textBox = new TextBox();
            textBox.Multiline = multiline;
            textBox.SetBounds(12, 42, 480, multiline ? 100 : 30);

            Button confirmation = new Button();
            confirmation.Text = "OK";
            confirmation.SetBounds(332, multiline ? 150 : 84, 75, 30);
            confirmation.DialogResult = DialogResult.OK;

            Button cancel = new Button();
            cancel.Text = "Cancel";
            cancel.SetBounds(417, multiline ? 150 : 84, 75, 30);
            cancel.DialogResult = DialogResult.Cancel;

            prompt.Controls.Add(textLabel);
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(cancel);
            prompt.AcceptButton = confirmation;
            prompt.CancelButton = cancel;

            AppTheme.Apply(prompt);

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text.Trim() : string.Empty;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
