using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Financial
{
    public partial class frm_BalanceSheetReport
    {
        private Panel pnlFilters;
        private Label lblAsOfDate;
        private DateTimePicker dtpAsOfDate;
        private CheckBox chkShowComparison;
        private Label lblComparisonDate;
        private DateTimePicker dtpComparisonDate;
        private Label lblDetailLevel;
        private ComboBox cmbDetailLevel;
        private Button btnGenerate;
        private Panel pnlStatus;
        private Label lblBalanceStatus;
        private Button btnFindDiscrepancy;
        private SplitContainer splitBody;
        private GroupBox grpAssets;
        private DataGridView dgvAssets;
        private GroupBox grpLiabilitiesEquity;
        private DataGridView dgvLiabilitiesEquity;
        private GroupBox grpNotes;
        private TreeView tvNotes;
        private DataGridViewTextBoxColumn colAssetsItem;
        private DataGridViewTextBoxColumn colAssetsAmount;
        private DataGridViewTextBoxColumn colAssetsComparison;
        private DataGridViewTextBoxColumn colAssetsNotes;
        private DataGridViewTextBoxColumn colLiabItem;
        private DataGridViewTextBoxColumn colLiabAmount;
        private DataGridViewTextBoxColumn colLiabComparison;
        private DataGridViewTextBoxColumn colLiabNotes;

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_BalanceSheetReport));
            this.splitBody = new System.Windows.Forms.SplitContainer();
            this.grpAssets = new System.Windows.Forms.GroupBox();
            this.dgvAssets = new System.Windows.Forms.DataGridView();
            this.colAssetsItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAssetsAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAssetsComparison = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAssetsNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpLiabilitiesEquity = new System.Windows.Forms.GroupBox();
            this.dgvLiabilitiesEquity = new System.Windows.Forms.DataGridView();
            this.colLiabItem = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLiabAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLiabComparison = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLiabNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblAsOfDate = new System.Windows.Forms.Label();
            this.dtpAsOfDate = new System.Windows.Forms.DateTimePicker();
            this.chkShowComparison = new System.Windows.Forms.CheckBox();
            this.lblComparisonDate = new System.Windows.Forms.Label();
            this.dtpComparisonDate = new System.Windows.Forms.DateTimePicker();
            this.lblDetailLevel = new System.Windows.Forms.Label();
            this.cmbDetailLevel = new System.Windows.Forms.ComboBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.pnlStatus = new System.Windows.Forms.Panel();
            this.lblBalanceStatus = new System.Windows.Forms.Label();
            this.btnFindDiscrepancy = new System.Windows.Forms.Button();
            this.grpNotes = new System.Windows.Forms.GroupBox();
            this.tvNotes = new System.Windows.Forms.TreeView();
            ((System.ComponentModel.ISupportInitialize)(this.splitBody)).BeginInit();
            this.splitBody.Panel1.SuspendLayout();
            this.splitBody.Panel2.SuspendLayout();
            this.splitBody.SuspendLayout();
            this.grpAssets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).BeginInit();
            this.grpLiabilitiesEquity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLiabilitiesEquity)).BeginInit();
            this.pnlFilters.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            this.grpNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitBody
            // 
            resources.ApplyResources(this.splitBody, "splitBody");
            this.splitBody.Name = "splitBody";
            // 
            // splitBody.Panel1
            // 
            resources.ApplyResources(this.splitBody.Panel1, "splitBody.Panel1");
            this.splitBody.Panel1.Controls.Add(this.grpAssets);
            // 
            // splitBody.Panel2
            // 
            resources.ApplyResources(this.splitBody.Panel2, "splitBody.Panel2");
            this.splitBody.Panel2.Controls.Add(this.grpLiabilitiesEquity);
            // 
            // grpAssets
            // 
            resources.ApplyResources(this.grpAssets, "grpAssets");
            this.grpAssets.Controls.Add(this.dgvAssets);
            this.grpAssets.Name = "grpAssets";
            this.grpAssets.TabStop = false;
            // 
            // dgvAssets
            // 
            resources.ApplyResources(this.dgvAssets, "dgvAssets");
            this.dgvAssets.AllowUserToAddRows = false;
            this.dgvAssets.AllowUserToDeleteRows = false;
            this.dgvAssets.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAssets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAssetsItem,
            this.colAssetsAmount,
            this.colAssetsComparison,
            this.colAssetsNotes});
            this.dgvAssets.MultiSelect = false;
            this.dgvAssets.Name = "dgvAssets";
            this.dgvAssets.ReadOnly = true;
            this.dgvAssets.RowHeadersVisible = false;
            this.dgvAssets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // colAssetsItem
            // 
            this.colAssetsItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAssetsItem.DataPropertyName = "LineItem";
            resources.ApplyResources(this.colAssetsItem, "colAssetsItem");
            this.colAssetsItem.Name = "colAssetsItem";
            this.colAssetsItem.ReadOnly = true;
            // 
            // colAssetsAmount
            // 
            this.colAssetsAmount.DataPropertyName = "Amount";
            resources.ApplyResources(this.colAssetsAmount, "colAssetsAmount");
            this.colAssetsAmount.Name = "colAssetsAmount";
            this.colAssetsAmount.ReadOnly = true;
            // 
            // colAssetsComparison
            // 
            this.colAssetsComparison.DataPropertyName = "Comparison";
            resources.ApplyResources(this.colAssetsComparison, "colAssetsComparison");
            this.colAssetsComparison.Name = "colAssetsComparison";
            this.colAssetsComparison.ReadOnly = true;
            // 
            // colAssetsNotes
            // 
            this.colAssetsNotes.DataPropertyName = "Notes";
            resources.ApplyResources(this.colAssetsNotes, "colAssetsNotes");
            this.colAssetsNotes.Name = "colAssetsNotes";
            this.colAssetsNotes.ReadOnly = true;
            // 
            // grpLiabilitiesEquity
            // 
            resources.ApplyResources(this.grpLiabilitiesEquity, "grpLiabilitiesEquity");
            this.grpLiabilitiesEquity.Controls.Add(this.dgvLiabilitiesEquity);
            this.grpLiabilitiesEquity.Name = "grpLiabilitiesEquity";
            this.grpLiabilitiesEquity.TabStop = false;
            // 
            // dgvLiabilitiesEquity
            // 
            resources.ApplyResources(this.dgvLiabilitiesEquity, "dgvLiabilitiesEquity");
            this.dgvLiabilitiesEquity.AllowUserToAddRows = false;
            this.dgvLiabilitiesEquity.AllowUserToDeleteRows = false;
            this.dgvLiabilitiesEquity.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvLiabilitiesEquity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLiabilitiesEquity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLiabItem,
            this.colLiabAmount,
            this.colLiabComparison,
            this.colLiabNotes});
            this.dgvLiabilitiesEquity.MultiSelect = false;
            this.dgvLiabilitiesEquity.Name = "dgvLiabilitiesEquity";
            this.dgvLiabilitiesEquity.ReadOnly = true;
            this.dgvLiabilitiesEquity.RowHeadersVisible = false;
            this.dgvLiabilitiesEquity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // colLiabItem
            // 
            this.colLiabItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLiabItem.DataPropertyName = "LineItem";
            resources.ApplyResources(this.colLiabItem, "colLiabItem");
            this.colLiabItem.Name = "colLiabItem";
            this.colLiabItem.ReadOnly = true;
            // 
            // colLiabAmount
            // 
            this.colLiabAmount.DataPropertyName = "Amount";
            resources.ApplyResources(this.colLiabAmount, "colLiabAmount");
            this.colLiabAmount.Name = "colLiabAmount";
            this.colLiabAmount.ReadOnly = true;
            // 
            // colLiabComparison
            // 
            this.colLiabComparison.DataPropertyName = "Comparison";
            resources.ApplyResources(this.colLiabComparison, "colLiabComparison");
            this.colLiabComparison.Name = "colLiabComparison";
            this.colLiabComparison.ReadOnly = true;
            // 
            // colLiabNotes
            // 
            this.colLiabNotes.DataPropertyName = "Notes";
            resources.ApplyResources(this.colLiabNotes, "colLiabNotes");
            this.colLiabNotes.Name = "colLiabNotes";
            this.colLiabNotes.ReadOnly = true;
            // 
            // pnlFilters
            // 
            resources.ApplyResources(this.pnlFilters, "pnlFilters");
            this.pnlFilters.Controls.Add(this.lblAsOfDate);
            this.pnlFilters.Controls.Add(this.dtpAsOfDate);
            this.pnlFilters.Controls.Add(this.chkShowComparison);
            this.pnlFilters.Controls.Add(this.lblComparisonDate);
            this.pnlFilters.Controls.Add(this.dtpComparisonDate);
            this.pnlFilters.Controls.Add(this.lblDetailLevel);
            this.pnlFilters.Controls.Add(this.cmbDetailLevel);
            this.pnlFilters.Controls.Add(this.btnGenerate);
            this.pnlFilters.Name = "pnlFilters";
            // 
            // lblAsOfDate
            // 
            resources.ApplyResources(this.lblAsOfDate, "lblAsOfDate");
            this.lblAsOfDate.Name = "lblAsOfDate";
            // 
            // dtpAsOfDate
            // 
            resources.ApplyResources(this.dtpAsOfDate, "dtpAsOfDate");
            this.dtpAsOfDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAsOfDate.Name = "dtpAsOfDate";
            // 
            // chkShowComparison
            // 
            resources.ApplyResources(this.chkShowComparison, "chkShowComparison");
            this.chkShowComparison.Name = "chkShowComparison";
            this.chkShowComparison.UseVisualStyleBackColor = true;
            // 
            // lblComparisonDate
            // 
            resources.ApplyResources(this.lblComparisonDate, "lblComparisonDate");
            this.lblComparisonDate.Name = "lblComparisonDate";
            // 
            // dtpComparisonDate
            // 
            resources.ApplyResources(this.dtpComparisonDate, "dtpComparisonDate");
            this.dtpComparisonDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpComparisonDate.Name = "dtpComparisonDate";
            // 
            // lblDetailLevel
            // 
            resources.ApplyResources(this.lblDetailLevel, "lblDetailLevel");
            this.lblDetailLevel.Name = "lblDetailLevel";
            // 
            // cmbDetailLevel
            // 
            resources.ApplyResources(this.cmbDetailLevel, "cmbDetailLevel");
            this.cmbDetailLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDetailLevel.FormattingEnabled = true;
            this.cmbDetailLevel.Name = "cmbDetailLevel";
            // 
            // btnGenerate
            // 
            resources.ApplyResources(this.btnGenerate, "btnGenerate");
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            // 
            // pnlStatus
            // 
            resources.ApplyResources(this.pnlStatus, "pnlStatus");
            this.pnlStatus.Controls.Add(this.lblBalanceStatus);
            this.pnlStatus.Controls.Add(this.btnFindDiscrepancy);
            this.pnlStatus.Name = "pnlStatus";
            // 
            // lblBalanceStatus
            // 
            resources.ApplyResources(this.lblBalanceStatus, "lblBalanceStatus");
            this.lblBalanceStatus.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblBalanceStatus.Name = "lblBalanceStatus";
            // 
            // btnFindDiscrepancy
            // 
            resources.ApplyResources(this.btnFindDiscrepancy, "btnFindDiscrepancy");
            this.btnFindDiscrepancy.Name = "btnFindDiscrepancy";
            this.btnFindDiscrepancy.UseVisualStyleBackColor = true;
            // 
            // grpNotes
            // 
            resources.ApplyResources(this.grpNotes, "grpNotes");
            this.grpNotes.Controls.Add(this.tvNotes);
            this.grpNotes.Name = "grpNotes";
            this.grpNotes.TabStop = false;
            // 
            // tvNotes
            // 
            resources.ApplyResources(this.tvNotes, "tvNotes");
            this.tvNotes.Name = "tvNotes";
            // 
            // frm_BalanceSheetReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.splitBody);
            this.Controls.Add(this.grpNotes);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.pnlFilters);
            this.Name = "frm_BalanceSheetReport";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.splitBody.Panel1.ResumeLayout(false);
            this.splitBody.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBody)).EndInit();
            this.splitBody.ResumeLayout(false);
            this.grpAssets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).EndInit();
            this.grpLiabilitiesEquity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLiabilitiesEquity)).EndInit();
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.pnlStatus.ResumeLayout(false);
            this.grpNotes.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
