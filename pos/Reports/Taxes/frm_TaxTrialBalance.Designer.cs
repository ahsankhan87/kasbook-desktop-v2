using System.Windows.Forms;

namespace pos.Reports.Taxes
{
    partial class frm_TaxTrialBalance
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;

            // Main form setup
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Text = "Trial Balance for Tax Reporting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Icon = null;

            // Panel for filters
            Panel filterPanel = new Panel();
            filterPanel.Dock = DockStyle.Top;
            filterPanel.Height = 70;
            filterPanel.Padding = new Padding(10);
            this.Controls.Add(filterPanel);

            // Financial Year
            Label lblFinancialYear = new Label { Text = "Financial Year:", Left = 10, Top = 10, Width = 100 };
            this.cmbFinancialYear = new ComboBox { Left = 110, Top = 10, Width = 200 };
            this.cmbFinancialYear.SelectedIndexChanged += cmbFinancialYear_SelectedIndexChanged;
            filterPanel.Controls.Add(lblFinancialYear);
            filterPanel.Controls.Add(this.cmbFinancialYear);

            // Account Type Filter
            Label lblAccountType = new Label { Text = "Account Type:", Left = 320, Top = 10, Width = 100 };
            this.cmbAccountType = new ComboBox { Left = 420, Top = 10, Width = 150 };
            this.cmbAccountType.Items.AddRange(new object[] { "All", "Income", "Expense" });
            this.cmbAccountType.SelectedIndexChanged += cmbAccountType_SelectedIndexChanged;
            filterPanel.Controls.Add(lblAccountType);
            filterPanel.Controls.Add(this.cmbAccountType);

            // Buttons
            this.btnRefresh = new Button { Text = "Refresh", Left = 10, Top = 40, Width = 100, Height = 30 };
            this.btnRefresh.Click += btnRefresh_Click;
            filterPanel.Controls.Add(this.btnRefresh);

            this.btnExportExcel = new Button { Text = "Export Excel", Left = 120, Top = 40, Width = 100, Height = 30 };
            this.btnExportExcel.Click += btnExportExcel_Click;
            filterPanel.Controls.Add(this.btnExportExcel);

            this.btnPrint = new Button { Text = "Print", Left = 230, Top = 40, Width = 100, Height = 30 };
            this.btnPrint.Click += btnPrint_Click;
            filterPanel.Controls.Add(this.btnPrint);

            // Summary Labels
            Panel summaryPanel = new Panel();
            summaryPanel.Dock = DockStyle.Top;
            summaryPanel.Height = 50;
            summaryPanel.Padding = new Padding(10);
            this.Controls.Add(summaryPanel);

            this.lblTotalIncome = new Label { Text = "Total Income: SAR 0.00", Left = 10, Top = 10, Width = 300 };
            this.lblTotalExpense = new Label { Text = "Total Expense: SAR 0.00", Left = 320, Top = 10, Width = 300 };
            this.lblNetIncome = new Label { Text = "Net Income: SAR 0.00", Left = 630, Top = 10, Width = 300 };

            summaryPanel.Controls.AddRange(new[] { this.lblTotalIncome, this.lblTotalExpense, this.lblNetIncome });

            // Grid
            this.dgvTrialBalance = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false };
            this.Controls.Add(this.dgvTrialBalance);
        }

        // Controls
        private ComboBox cmbFinancialYear;
        private ComboBox cmbAccountType;
        private Button btnRefresh;
        private Button btnExportExcel;
        private Button btnPrint;
        private Label lblTotalIncome;
        private Label lblTotalExpense;
        private Label lblNetIncome;
        private DataGridView dgvTrialBalance;
    }
}
