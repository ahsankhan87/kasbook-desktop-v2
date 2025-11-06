using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Dashboard
{
    partial class frm_branchWiseSummary
    {
        private System.ComponentModel.IContainer components = null;
        private Panel headerPanel;
        private Label lblTitle;
        private Button btnRefresh;

        private TableLayoutPanel kpiTable;
        private Panel cardTotalSales;
        private Label lblTotalSalesTitle;
        public Label lbl_total_sales;

        private Panel cardSalesTax;
        private Label lblSalesTaxTitle;
        public Label lbl_sales_tax;

        private Panel cardTotalPurchases;
        private Label lblTotalPurchasesTitle;
        public Label lbl_total_purchases;

        private Panel cardPurchaseTax;
        private Label lblPurchaseTaxTitle;
        public Label lbl_purchase_tax;

        private Panel cardNetIncome;
        private Label lblNetIncomeTitle;
        public Label lbl_net_income;

        public DataGridView dataGridViewBranchSummary;
        private StatusStrip statusStrip;
        public ToolStripStatusLabel lblStatus;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            headerPanel = new Panel();
            lblTitle = new Label();
            btnRefresh = new Button();

            kpiTable = new TableLayoutPanel();
            cardTotalSales = new Panel();
            lblTotalSalesTitle = new Label();
            lbl_total_sales = new Label();

            cardSalesTax = new Panel();
            lblSalesTaxTitle = new Label();
            lbl_sales_tax = new Label();

            cardTotalPurchases = new Panel();
            lblTotalPurchasesTitle = new Label();
            lbl_total_purchases = new Label();

            cardPurchaseTax = new Panel();
            lblPurchaseTaxTitle = new Label();
            lbl_purchase_tax = new Label();

            cardNetIncome = new Panel();
            lblNetIncomeTitle = new Label();
            lbl_net_income = new Label();

            dataGridViewBranchSummary = new DataGridView();
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();

            // Form
            AutoScaleMode = AutoScaleMode.Font;
            Text = "Branch-wise Income Summary";
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9F);
            StartPosition = FormStartPosition.CenterScreen;
            Width = 1000;
            Height = 650;
            Load += frm_branchWiseSummary_Load;

            // Header panel
            headerPanel.Dock = DockStyle.Top;
            headerPanel.Height = 60;
            headerPanel.BackColor = Color.FromArgb(0, 120, 215);
            headerPanel.Padding = new Padding(16, 10, 16, 10);

            // Title
            lblTitle.AutoSize = true;
            lblTitle.ForeColor = Color.White;
            lblTitle.Font = new Font("Segoe UI Semibold", 14.25F, FontStyle.Bold);
            lblTitle.Text = "Branch-wise Income Summary";
            lblTitle.Dock = DockStyle.Left;
            lblTitle.Margin = new Padding(0, 4, 0, 0);

            // Refresh button
            btnRefresh.Text = "Refresh";
            btnRefresh.BackColor = Color.White;
            btnRefresh.ForeColor = Color.FromArgb(0, 120, 215);
            btnRefresh.FlatStyle = FlatStyle.Flat;
            btnRefresh.FlatAppearance.BorderSize = 0;
            btnRefresh.Width = 100;
            btnRefresh.Height = 32;
            btnRefresh.Dock = DockStyle.Right; // designer-safe alignment
            btnRefresh.Margin = new Padding(0);
            btnRefresh.Click += btnRefresh_Click;

            headerPanel.Controls.Add(btnRefresh);
            headerPanel.Controls.Add(lblTitle);
            Controls.Add(headerPanel);

            // KPI Table
            kpiTable.Dock = DockStyle.Top;
            kpiTable.Height = 110;
            kpiTable.Padding = new Padding(16, 12, 16, 12);
            kpiTable.ColumnCount = 5;
            kpiTable.RowCount = 1;
            kpiTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            // Replace the loop with explicit adds:
            kpiTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            kpiTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            kpiTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            kpiTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            kpiTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
            Controls.Add(kpiTable);

            // Helper to create a KPI card
            Panel CreateCard(Panel card, string titleText, Label title, Label value, Color color)
            {
                card.Dock = DockStyle.Fill;
                card.Margin = new Padding(0, 0, 12, 0);
                card.Padding = new Padding(12);
                card.BackColor = color;

                title.AutoSize = true;
                title.ForeColor = Color.White;
                title.Font = new Font("Segoe UI", 9F);
                title.Text = titleText;
                title.Dock = DockStyle.Top;

                value.AutoSize = false;
                value.ForeColor = Color.White;
                value.Font = new Font("Segoe UI Semibold", 14F, FontStyle.Bold);
                value.Text = "0.00";
                value.TextAlign = ContentAlignment.MiddleLeft;
                value.Dock = DockStyle.Bottom;
                value.Height = 36;

                card.Controls.Add(title);
                card.Controls.Add(value);
                return card;
            }

            // Cards
            CreateCard(cardTotalSales, "Total Sales", lblTotalSalesTitle, lbl_total_sales, Color.FromArgb(0, 153, 188));
            CreateCard(cardSalesTax, "Sales Tax", lblSalesTaxTitle, lbl_sales_tax, Color.FromArgb(0, 120, 215));
            CreateCard(cardTotalPurchases, "Total Purchases", lblTotalPurchasesTitle, lbl_total_purchases, Color.FromArgb(0, 99, 177));
            CreateCard(cardPurchaseTax, "Purchase Tax", lblPurchaseTaxTitle, lbl_purchase_tax, Color.FromArgb(45, 125, 154));
            CreateCard(cardNetIncome, "Net Income", lblNetIncomeTitle, lbl_net_income, Color.FromArgb(0, 153, 116));

            // Add cards to table
            kpiTable.Controls.Add(cardTotalSales, 0, 0);
            kpiTable.Controls.Add(cardSalesTax, 1, 0);
            kpiTable.Controls.Add(cardTotalPurchases, 2, 0);
            kpiTable.Controls.Add(cardPurchaseTax, 3, 0);
            kpiTable.Controls.Add(cardNetIncome, 4, 0);

            // Grid
            dataGridViewBranchSummary.Dock = DockStyle.Fill;
            dataGridViewBranchSummary.ReadOnly = true;
            dataGridViewBranchSummary.AllowUserToAddRows = false;
            dataGridViewBranchSummary.AllowUserToDeleteRows = false;
            dataGridViewBranchSummary.RowHeadersVisible = false;
            Controls.Add(dataGridViewBranchSummary);

            // Status strip
            statusStrip.Items.Add(lblStatus);
            statusStrip.SizingGrip = false;
            lblStatus.Text = "Ready";
            Controls.Add(statusStrip);

            // Layout order
            Controls.SetChildIndex(headerPanel, 0);
            Controls.SetChildIndex(kpiTable, 1);
            Controls.SetChildIndex(dataGridViewBranchSummary, 2);
            Controls.SetChildIndex(statusStrip, 3);
        }
    }
}