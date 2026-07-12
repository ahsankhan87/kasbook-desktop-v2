using System.Windows.Forms;

namespace pos.Reports.Taxes
{
    partial class frm_WithholdingTaxReport
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
            this.Text = "Withholding Tax Report";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Icon = null;

            // Panel for filters
            Panel filterPanel = new Panel();
            filterPanel.Dock = DockStyle.Top;
            filterPanel.Height = 70;
            filterPanel.Padding = new Padding(10);
            this.Controls.Add(filterPanel);

            // From Date
            Label lblFromDate = new Label { Text = "From Date:", Left = 10, Top = 10, Width = 100 };
            this.dtpFromDate = new DateTimePicker { Left = 110, Top = 10, Width = 150, Format = System.Windows.Forms.DateTimePickerFormat.Short };
            filterPanel.Controls.Add(lblFromDate);
            filterPanel.Controls.Add(this.dtpFromDate);

            // To Date
            Label lblToDate = new Label { Text = "To Date:", Left = 270, Top = 10, Width = 100 };
            this.dtpToDate = new DateTimePicker { Left = 370, Top = 10, Width = 150, Format = System.Windows.Forms.DateTimePickerFormat.Short };
            filterPanel.Controls.Add(lblToDate);
            filterPanel.Controls.Add(this.dtpToDate);

            // Buttons
            this.btnRefresh = new Button { Text = "Refresh", Left = 10, Top = 40, Width = 100, Height = 30 };
            this.btnRefresh.Click += btnRefresh_Click;
            filterPanel.Controls.Add(this.btnRefresh);

            this.btnGeneratePSID = new Button { Text = "Generate PSID", Left = 120, Top = 40, Width = 100, Height = 30 };
            this.btnGeneratePSID.Click += btnGeneratePSID_Click;
            filterPanel.Controls.Add(this.btnGeneratePSID);

            this.btnExportExcel = new Button { Text = "Export Excel", Left = 230, Top = 40, Width = 100, Height = 30 };
            this.btnExportExcel.Click += btnExportExcel_Click;
            filterPanel.Controls.Add(this.btnExportExcel);

            this.btnPrint = new Button { Text = "Print", Left = 340, Top = 40, Width = 100, Height = 30 };
            this.btnPrint.Click += btnPrint_Click;
            filterPanel.Controls.Add(this.btnPrint);

            // Summary Labels
            Panel summaryPanel = new Panel();
            summaryPanel.Dock = DockStyle.Top;
            summaryPanel.Height = 50;
            summaryPanel.Padding = new Padding(10);
            this.Controls.Add(summaryPanel);

            this.lblTotalPayments = new Label { Text = "Total Payments: SAR 0.00", Left = 10, Top = 10, Width = 350 };
            this.lblTotalWHT = new Label { Text = "Total WHT: SAR 0.00 | Avg Rate: 0.00%", Left = 370, Top = 10, Width = 400 };
            this.lblTransactionCount = new Label { Text = "Transactions: 0", Left = 770, Top = 10, Width = 200 };

            summaryPanel.Controls.AddRange(new[] { this.lblTotalPayments, this.lblTotalWHT, this.lblTransactionCount });

            // TabControl for transactions and summary
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl);

            // WHT Transactions Tab
            TabPage tpTransactions = new TabPage { Text = "WHT Transactions" };
            this.dgvWHTTransactions = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false };
            tpTransactions.Controls.Add(this.dgvWHTTransactions);
            tabControl.TabPages.Add(tpTransactions);

            // WHT Summary Tab
            TabPage tpSummary = new TabPage { Text = "Summary by Section" };
            this.dgvWHTSummary = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false };
            tpSummary.Controls.Add(this.dgvWHTSummary);
            tabControl.TabPages.Add(tpSummary);
        }

        // Controls
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private Button btnRefresh;
        private Button btnGeneratePSID;
        private Button btnExportExcel;
        private Button btnPrint;
        private Label lblTotalPayments;
        private Label lblTotalWHT;
        private Label lblTransactionCount;
        private DataGridView dgvWHTTransactions;
        private DataGridView dgvWHTSummary;
    }
}
