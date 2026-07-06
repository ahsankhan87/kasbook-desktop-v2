using System;
using System.Windows.Forms;
using POS.DLL.Accounts;

namespace pos.Accounts.Maintenance
{
    public partial class frm_codes_maintenance : Form
    {
        public frm_codes_maintenance()
        {
            InitializeComponent();
        }

        private void frm_codes_maintenance_Load(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                var helper = new CodesUpdateHelper();
                var stats = helper.GetCodeAssignmentStats();

                // Level 1 Groups
                txt_l1_total.Text = stats.Level1GroupsTotal.ToString();
                txt_l1_with_codes.Text = stats.Level1GroupsWithCodes.ToString();
                txt_l1_missing.Text = (stats.Level1GroupsTotal - stats.Level1GroupsWithCodes).ToString();
                txt_l1_coverage.Text = stats.Level1GroupsCoverage.ToString("F2") + "%";

                // Level 2 Groups
                txt_l2_total.Text = stats.Level2GroupsTotal.ToString();
                txt_l2_with_codes.Text = stats.Level2GroupsWithCodes.ToString();
                txt_l2_missing.Text = (stats.Level2GroupsTotal - stats.Level2GroupsWithCodes).ToString();
                txt_l2_coverage.Text = stats.Level2GroupsCoverage.ToString("F2") + "%";

                // Accounts
                txt_acc_total.Text = stats.AccountsTotal.ToString();
                txt_acc_with_codes.Text = stats.AccountsWithCodes.ToString();
                txt_acc_missing.Text = (stats.AccountsTotal - stats.AccountsWithCodes).ToString();
                txt_acc_coverage.Text = stats.AccountsCoverage.ToString("F2") + "%";

                lbl_status.Text = "Status: Ready";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading statistics: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lbl_status.Text = "Status: Error";
            }
        }

        private void btn_update_codes_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "This will generate codes for all groups and accounts without codes.\n\nContinue?",
                "Generate Codes Confirmation",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    lbl_status.Text = "Status: Generating codes...";
                    Application.DoEvents();

                    var helper = new CodesUpdateHelper();
                    var updateResult = helper.UpdateAllCodes();

                    if (updateResult.IsSuccess)
                    {
                        MessageBox.Show(updateResult.Message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lbl_status.Text = "Status: Code generation completed successfully";
                        LoadStatistics();
                    }
                    else
                    {
                        MessageBox.Show("Code generation failed: " + updateResult.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        lbl_status.Text = "Status: Code generation failed";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error generating codes: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lbl_status.Text = "Status: Error";
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
