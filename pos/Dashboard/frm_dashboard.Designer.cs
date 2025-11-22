using System.Drawing;
using System.Windows.Forms;

namespace pos.Dashboard
{
    partial class frm_dashboard
    {
        private System.ComponentModel.IContainer components = null;

        private Panel headerPanel;
        private Label lblTitle;
        private Label lblSubtitle;

        private FlowLayoutPanel summaryPanel;
        private Panel pnlSalesToday;
        private Label lblSalesTitle;
        private Label lblSalesValue;

        private Panel pnlRevenueToday;
        private Label lblRevenueTitle;
        private Label lblRevenueValue;

        private Panel pnlLowStock;
        private Label lblLowStockTitle;
        private Label lblLowStockValue;

        private Label lblQuickAccess;
        private FlowLayoutPanel quickAccessPanel;
        private Button btnNewSale;
        private Button btnProducts;
        private Button btnCustomers;
        private Button btnSuppliers;
        private Button btnSalesReport;
        private Button btnPurchasesReport;
        private Button btnSettings;

        private Label lblRecent;
        private ListView listRecent;
        private ColumnHeader colArea;
        private ColumnHeader colInfo;
        private ColumnHeader colDateTime;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_dashboard));
            this.headerPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.summaryPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlSalesToday = new System.Windows.Forms.Panel();
            this.panelSalesColor = new System.Windows.Forms.Panel();
            this.lblSalesTitle = new System.Windows.Forms.Label();
            this.lblSalesValue = new System.Windows.Forms.Label();
            this.pnlRevenueToday = new System.Windows.Forms.Panel();
            this.panelRevenueColor = new System.Windows.Forms.Panel();
            this.lblRevenueTitle = new System.Windows.Forms.Label();
            this.lblRevenueValue = new System.Windows.Forms.Label();
            this.pnlLowStock = new System.Windows.Forms.Panel();
            this.panelLowStockColor = new System.Windows.Forms.Panel();
            this.lblLowStockTitle = new System.Windows.Forms.Label();
            this.lblLowStockValue = new System.Windows.Forms.Label();
            this.lblQuickAccess = new System.Windows.Forms.Label();
            this.quickAccessPanel = new System.Windows.Forms.FlowLayoutPanel();
            this.btnNewSale = new System.Windows.Forms.Button();
            this.btnProducts = new System.Windows.Forms.Button();
            this.btnCustomers = new System.Windows.Forms.Button();
            this.btnSuppliers = new System.Windows.Forms.Button();
            this.btnSalesReport = new System.Windows.Forms.Button();
            this.btnPurchasesReport = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.lblRecent = new System.Windows.Forms.Label();
            this.listRecent = new System.Windows.Forms.ListView();
            this.colArea = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colInfo = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.colDateTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnNewPurchase = new System.Windows.Forms.Button();
            this.headerPanel.SuspendLayout();
            this.summaryPanel.SuspendLayout();
            this.pnlSalesToday.SuspendLayout();
            this.pnlRevenueToday.SuspendLayout();
            this.pnlLowStock.SuspendLayout();
            this.quickAccessPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.headerPanel.Controls.Add(this.lblTitle);
            this.headerPanel.Controls.Add(this.lblSubtitle);
            resources.ApplyResources(this.headerPanel, "headerPanel");
            this.headerPanel.Name = "headerPanel";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            // 
            // lblSubtitle
            // 
            resources.ApplyResources(this.lblSubtitle, "lblSubtitle");
            this.lblSubtitle.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSubtitle.Name = "lblSubtitle";
            // 
            // summaryPanel
            // 
            this.summaryPanel.BackColor = System.Drawing.Color.White;
            this.summaryPanel.Controls.Add(this.pnlSalesToday);
            this.summaryPanel.Controls.Add(this.pnlRevenueToday);
            this.summaryPanel.Controls.Add(this.pnlLowStock);
            resources.ApplyResources(this.summaryPanel, "summaryPanel");
            this.summaryPanel.Name = "summaryPanel";
            // 
            // pnlSalesToday
            // 
            this.pnlSalesToday.BackColor = System.Drawing.Color.White;
            this.pnlSalesToday.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlSalesToday.Controls.Add(this.panelSalesColor);
            this.pnlSalesToday.Controls.Add(this.lblSalesTitle);
            this.pnlSalesToday.Controls.Add(this.lblSalesValue);
            resources.ApplyResources(this.pnlSalesToday, "pnlSalesToday");
            this.pnlSalesToday.Name = "pnlSalesToday";
            // 
            // panelSalesColor
            // 
            this.panelSalesColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(204)))), ((int)(((byte)(113)))));
            resources.ApplyResources(this.panelSalesColor, "panelSalesColor");
            this.panelSalesColor.Name = "panelSalesColor";
            // 
            // lblSalesTitle
            // 
            resources.ApplyResources(this.lblSalesTitle, "lblSalesTitle");
            this.lblSalesTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblSalesTitle.Name = "lblSalesTitle";
            // 
            // lblSalesValue
            // 
            resources.ApplyResources(this.lblSalesValue, "lblSalesValue");
            this.lblSalesValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblSalesValue.Name = "lblSalesValue";
            // 
            // pnlRevenueToday
            // 
            this.pnlRevenueToday.BackColor = System.Drawing.Color.White;
            this.pnlRevenueToday.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlRevenueToday.Controls.Add(this.panelRevenueColor);
            this.pnlRevenueToday.Controls.Add(this.lblRevenueTitle);
            this.pnlRevenueToday.Controls.Add(this.lblRevenueValue);
            resources.ApplyResources(this.pnlRevenueToday, "pnlRevenueToday");
            this.pnlRevenueToday.Name = "pnlRevenueToday";
            // 
            // panelRevenueColor
            // 
            this.panelRevenueColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(152)))), ((int)(((byte)(219)))));
            resources.ApplyResources(this.panelRevenueColor, "panelRevenueColor");
            this.panelRevenueColor.Name = "panelRevenueColor";
            // 
            // lblRevenueTitle
            // 
            resources.ApplyResources(this.lblRevenueTitle, "lblRevenueTitle");
            this.lblRevenueTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblRevenueTitle.Name = "lblRevenueTitle";
            // 
            // lblRevenueValue
            // 
            resources.ApplyResources(this.lblRevenueValue, "lblRevenueValue");
            this.lblRevenueValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblRevenueValue.Name = "lblRevenueValue";
            // 
            // pnlLowStock
            // 
            this.pnlLowStock.BackColor = System.Drawing.Color.White;
            this.pnlLowStock.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlLowStock.Controls.Add(this.panelLowStockColor);
            this.pnlLowStock.Controls.Add(this.lblLowStockTitle);
            this.pnlLowStock.Controls.Add(this.lblLowStockValue);
            resources.ApplyResources(this.pnlLowStock, "pnlLowStock");
            this.pnlLowStock.Name = "pnlLowStock";
            // 
            // panelLowStockColor
            // 
            this.panelLowStockColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(76)))), ((int)(((byte)(60)))));
            resources.ApplyResources(this.panelLowStockColor, "panelLowStockColor");
            this.panelLowStockColor.Name = "panelLowStockColor";
            // 
            // lblLowStockTitle
            // 
            resources.ApplyResources(this.lblLowStockTitle, "lblLowStockTitle");
            this.lblLowStockTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(100)))), ((int)(((byte)(100)))), ((int)(((byte)(110)))));
            this.lblLowStockTitle.Name = "lblLowStockTitle";
            // 
            // lblLowStockValue
            // 
            this.lblLowStockValue.Cursor = System.Windows.Forms.Cursors.Hand;
            resources.ApplyResources(this.lblLowStockValue, "lblLowStockValue");
            this.lblLowStockValue.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblLowStockValue.Name = "lblLowStockValue";
            this.lblLowStockValue.Click += new System.EventHandler(this.lblLowStockValue_Click);
            // 
            // lblQuickAccess
            // 
            resources.ApplyResources(this.lblQuickAccess, "lblQuickAccess");
            this.lblQuickAccess.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblQuickAccess.Name = "lblQuickAccess";
            // 
            // quickAccessPanel
            // 
            resources.ApplyResources(this.quickAccessPanel, "quickAccessPanel");
            this.quickAccessPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.quickAccessPanel.Controls.Add(this.btnNewSale);
            this.quickAccessPanel.Controls.Add(this.btnNewPurchase);
            this.quickAccessPanel.Controls.Add(this.btnProducts);
            this.quickAccessPanel.Controls.Add(this.btnCustomers);
            this.quickAccessPanel.Controls.Add(this.btnSuppliers);
            this.quickAccessPanel.Controls.Add(this.btnPurchasesReport);
            this.quickAccessPanel.Controls.Add(this.btnSettings);
            this.quickAccessPanel.Controls.Add(this.btnSalesReport);
            this.quickAccessPanel.Name = "quickAccessPanel";
            // 
            // btnNewSale
            // 
            this.btnNewSale.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnNewSale, "btnNewSale");
            this.btnNewSale.Name = "btnNewSale";
            this.btnNewSale.UseVisualStyleBackColor = false;
            this.btnNewSale.Click += new System.EventHandler(this.btnNewSale_Click);
            // 
            // btnProducts
            // 
            this.btnProducts.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnProducts, "btnProducts");
            this.btnProducts.Name = "btnProducts";
            this.btnProducts.UseVisualStyleBackColor = false;
            this.btnProducts.Click += new System.EventHandler(this.btnProducts_Click);
            // 
            // btnCustomers
            // 
            this.btnCustomers.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnCustomers, "btnCustomers");
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.UseVisualStyleBackColor = false;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            // 
            // btnSuppliers
            // 
            this.btnSuppliers.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnSuppliers, "btnSuppliers");
            this.btnSuppliers.Name = "btnSuppliers";
            this.btnSuppliers.UseVisualStyleBackColor = false;
            this.btnSuppliers.Click += new System.EventHandler(this.btnSuppliers_Click);
            // 
            // btnSalesReport
            // 
            this.btnSalesReport.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnSalesReport, "btnSalesReport");
            this.btnSalesReport.Name = "btnSalesReport";
            this.btnSalesReport.UseVisualStyleBackColor = false;
            this.btnSalesReport.Click += new System.EventHandler(this.btnSalesReport_Click);
            // 
            // btnPurchasesReport
            // 
            this.btnPurchasesReport.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnPurchasesReport, "btnPurchasesReport");
            this.btnPurchasesReport.Name = "btnPurchasesReport";
            this.btnPurchasesReport.UseVisualStyleBackColor = false;
            this.btnPurchasesReport.Click += new System.EventHandler(this.btnPurchasesReport_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnSettings, "btnSettings");
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.UseVisualStyleBackColor = false;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // lblRecent
            // 
            resources.ApplyResources(this.lblRecent, "lblRecent");
            this.lblRecent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblRecent.Name = "lblRecent";
            // 
            // listRecent
            // 
            this.listRecent.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colArea,
            this.colInfo,
            this.colDateTime});
            resources.ApplyResources(this.listRecent, "listRecent");
            this.listRecent.FullRowSelect = true;
            this.listRecent.HideSelection = false;
            this.listRecent.Name = "listRecent";
            this.listRecent.UseCompatibleStateImageBehavior = false;
            this.listRecent.View = System.Windows.Forms.View.Details;
            // 
            // colArea
            // 
            resources.ApplyResources(this.colArea, "colArea");
            // 
            // colInfo
            // 
            resources.ApplyResources(this.colInfo, "colInfo");
            // 
            // colDateTime
            // 
            resources.ApplyResources(this.colDateTime, "colDateTime");
            // 
            // btnNewPurchase
            // 
            this.btnNewPurchase.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this.btnNewPurchase, "btnNewPurchase");
            this.btnNewPurchase.Name = "btnNewPurchase";
            this.btnNewPurchase.UseVisualStyleBackColor = false;
            this.btnNewPurchase.Click += new System.EventHandler(this.btnNewPurchase_Click);
            // 
            // frm_dashboard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listRecent);
            this.Controls.Add(this.lblRecent);
            this.Controls.Add(this.quickAccessPanel);
            this.Controls.Add(this.lblQuickAccess);
            this.Controls.Add(this.summaryPanel);
            this.Controls.Add(this.headerPanel);
            this.Name = "frm_dashboard";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_dashboard_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerPanel.PerformLayout();
            this.summaryPanel.ResumeLayout(false);
            this.pnlSalesToday.ResumeLayout(false);
            this.pnlRevenueToday.ResumeLayout(false);
            this.pnlLowStock.ResumeLayout(false);
            this.quickAccessPanel.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private Panel panelSalesColor;
        private Panel panelRevenueColor;
        private Panel panelLowStockColor;
        private Button btnNewPurchase;
    }
}