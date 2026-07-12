using System.Windows.Forms;

namespace pos.Reports.Taxes
{
    partial class frm_SalesTaxSummary
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
            this.Text = "Sales Tax Summary Report (ZATCA)";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Icon = null;

            // Panel for filters
            Panel filterPanel = new Panel();
            filterPanel.Dock = DockStyle.Top;
            filterPanel.Height = 80;
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

            // Tax Reg No
            Label lblTaxRegNo = new Label { Text = "Tax Reg No:", Left = 530, Top = 10, Width = 100 };
            this.txtTaxRegNo = new TextBox { Left = 630, Top = 10, Width = 150, ReadOnly = true };
            filterPanel.Controls.Add(lblTaxRegNo);
            filterPanel.Controls.Add(this.txtTaxRegNo);

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
            summaryPanel.Height = 120;
            summaryPanel.Padding = new Padding(10);
            summaryPanel.AutoScroll = true;
            this.Controls.Add(summaryPanel);

            this.lblStandardRatedTax = new Label { Text = "Standard Rated Tax: SAR 0.00", Left = 10, Top = 10, Width = 300 };
            this.lblZeroRatedSales = new Label { Text = "Zero Rated Sales: SAR 0.00", Left = 320, Top = 10, Width = 300 };
            this.lblExemptSales = new Label { Text = "Exempt Sales: SAR 0.00", Left = 630, Top = 10, Width = 300 };
            this.lblTotalOutputTax = new Label { Text = "Total Output Tax: SAR 0.00", Left = 10, Top = 35, Width = 300 };
            this.lblTotalInputTax = new Label { Text = "Total Input Tax: SAR 0.00", Left = 320, Top = 35, Width = 300 };
            this.lblNetTaxPayable = new Label { Text = "Net Tax Payable: SAR 0.00", Left = 630, Top = 35, Width = 300 };
            this.lblTaxablePurchases = new Label { Text = "Taxable Purchases: SAR 0.00", Left = 10, Top = 60, Width = 300 };
            this.lblImportTax = new Label { Text = "Import Tax: SAR 0.00", Left = 320, Top = 60, Width = 300 };

            summaryPanel.Controls.AddRange(new[] { this.lblStandardRatedTax, this.lblZeroRatedSales, this.lblExemptSales, 
                this.lblTotalOutputTax, this.lblTotalInputTax, this.lblNetTaxPayable, this.lblTaxablePurchases, this.lblImportTax });

            // TabControl for Sales and Purchase registers
            TabControl tabControl = new TabControl();
            tabControl.Dock = DockStyle.Fill;
            this.Controls.Add(tabControl);

            // Sales Tax Register Tab
            TabPage tpSales = new TabPage { Text = "Sales Tax Register" };
            this.dgvSalesTaxRegister = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false };
            tpSales.Controls.Add(this.dgvSalesTaxRegister);
            tabControl.TabPages.Add(tpSales);

            // Purchase Tax Register Tab
            TabPage tpPurchase = new TabPage { Text = "Purchase Tax Register" };
            this.dgvPurchaseTaxRegister = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false };
            tpPurchase.Controls.Add(this.dgvPurchaseTaxRegister);
            tabControl.TabPages.Add(tpPurchase);
        }

        // Controls
        private DateTimePicker dtpFromDate;
        private DateTimePicker dtpToDate;
        private TextBox txtTaxRegNo;
        private Button btnRefresh;
        private Button btnExportExcel;
        private Button btnPrint;
        private Label lblStandardRatedTax;
        private Label lblZeroRatedSales;
        private Label lblExemptSales;
        private Label lblTotalOutputTax;
        private Label lblTotalInputTax;
        private Label lblNetTaxPayable;
        private Label lblTaxablePurchases;
        private Label lblImportTax;
        private DataGridView dgvSalesTaxRegister;
        private DataGridView dgvPurchaseTaxRegister;
    }
}
