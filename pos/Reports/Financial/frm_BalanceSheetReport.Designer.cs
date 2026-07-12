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
            this.grpNotes = new System.Windows.Forms.GroupBox();
            this.tvNotes = new System.Windows.Forms.TreeView();
            this.pnlFilters.SuspendLayout();
            this.pnlStatus.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitBody)).BeginInit();
            this.splitBody.Panel1.SuspendLayout();
            this.splitBody.Panel2.SuspendLayout();
            this.splitBody.SuspendLayout();
            this.grpAssets.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).BeginInit();
            this.grpLiabilitiesEquity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLiabilitiesEquity)).BeginInit();
            this.grpNotes.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.lblAsOfDate);
            this.pnlFilters.Controls.Add(this.dtpAsOfDate);
            this.pnlFilters.Controls.Add(this.chkShowComparison);
            this.pnlFilters.Controls.Add(this.lblComparisonDate);
            this.pnlFilters.Controls.Add(this.dtpComparisonDate);
            this.pnlFilters.Controls.Add(this.lblDetailLevel);
            this.pnlFilters.Controls.Add(this.cmbDetailLevel);
            this.pnlFilters.Controls.Add(this.btnGenerate);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Padding = new System.Windows.Forms.Padding(12, 10, 12, 10);
            this.pnlFilters.Size = new System.Drawing.Size(1384, 68);
            this.pnlFilters.TabIndex = 0;
            // 
            // lblAsOfDate
            // 
            this.lblAsOfDate.AutoSize = true;
            this.lblAsOfDate.Location = new System.Drawing.Point(14, 16);
            this.lblAsOfDate.Name = "lblAsOfDate";
            this.lblAsOfDate.Size = new System.Drawing.Size(69, 15);
            this.lblAsOfDate.TabIndex = 0;
            this.lblAsOfDate.Text = "As of Date:";
            // 
            // dtpAsOfDate
            // 
            this.dtpAsOfDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpAsOfDate.Location = new System.Drawing.Point(89, 12);
            this.dtpAsOfDate.Name = "dtpAsOfDate";
            this.dtpAsOfDate.Size = new System.Drawing.Size(120, 23);
            this.dtpAsOfDate.TabIndex = 1;
            // 
            // chkShowComparison
            // 
            this.chkShowComparison.AutoSize = true;
            this.chkShowComparison.Location = new System.Drawing.Point(231, 14);
            this.chkShowComparison.Name = "chkShowComparison";
            this.chkShowComparison.Size = new System.Drawing.Size(128, 19);
            this.chkShowComparison.TabIndex = 2;
            this.chkShowComparison.Text = "Show comparison";
            this.chkShowComparison.UseVisualStyleBackColor = true;
            // 
            // lblComparisonDate
            // 
            this.lblComparisonDate.AutoSize = true;
            this.lblComparisonDate.Location = new System.Drawing.Point(389, 16);
            this.lblComparisonDate.Name = "lblComparisonDate";
            this.lblComparisonDate.Size = new System.Drawing.Size(99, 15);
            this.lblComparisonDate.TabIndex = 3;
            this.lblComparisonDate.Text = "Comparison date:";
            // 
            // dtpComparisonDate
            // 
            this.dtpComparisonDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpComparisonDate.Location = new System.Drawing.Point(494, 12);
            this.dtpComparisonDate.Name = "dtpComparisonDate";
            this.dtpComparisonDate.Size = new System.Drawing.Size(120, 23);
            this.dtpComparisonDate.TabIndex = 4;
            // 
            // lblDetailLevel
            // 
            this.lblDetailLevel.AutoSize = true;
            this.lblDetailLevel.Location = new System.Drawing.Point(643, 16);
            this.lblDetailLevel.Name = "lblDetailLevel";
            this.lblDetailLevel.Size = new System.Drawing.Size(73, 15);
            this.lblDetailLevel.TabIndex = 5;
            this.lblDetailLevel.Text = "Detail level:";
            // 
            // cmbDetailLevel
            // 
            this.cmbDetailLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDetailLevel.FormattingEnabled = true;
            this.cmbDetailLevel.Location = new System.Drawing.Point(722, 12);
            this.cmbDetailLevel.Name = "cmbDetailLevel";
            this.cmbDetailLevel.Size = new System.Drawing.Size(170, 23);
            this.cmbDetailLevel.TabIndex = 6;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(1267, 11);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(105, 27);
            this.btnGenerate.TabIndex = 7;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            // 
            // pnlStatus
            // 
            this.pnlStatus.Controls.Add(this.lblBalanceStatus);
            this.pnlStatus.Controls.Add(this.btnFindDiscrepancy);
            this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlStatus.Location = new System.Drawing.Point(0, 68);
            this.pnlStatus.Name = "pnlStatus";
            this.pnlStatus.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.pnlStatus.Size = new System.Drawing.Size(1384, 48);
            this.pnlStatus.TabIndex = 1;
            // 
            // lblBalanceStatus
            // 
            this.lblBalanceStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBalanceStatus.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBalanceStatus.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblBalanceStatus.Location = new System.Drawing.Point(12, 8);
            this.lblBalanceStatus.Name = "lblBalanceStatus";
            this.lblBalanceStatus.Size = new System.Drawing.Size(1233, 32);
            this.lblBalanceStatus.TabIndex = 0;
            this.lblBalanceStatus.Text = "Assets = Liabilities + Equity ✓";
            this.lblBalanceStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFindDiscrepancy
            // 
            this.btnFindDiscrepancy.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnFindDiscrepancy.Location = new System.Drawing.Point(1245, 8);
            this.btnFindDiscrepancy.Name = "btnFindDiscrepancy";
            this.btnFindDiscrepancy.Size = new System.Drawing.Size(127, 32);
            this.btnFindDiscrepancy.TabIndex = 1;
            this.btnFindDiscrepancy.Text = "Find Discrepancy";
            this.btnFindDiscrepancy.UseVisualStyleBackColor = true;
            // 
            // splitBody
            // 
            this.splitBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitBody.Location = new System.Drawing.Point(0, 116);
            this.splitBody.Name = "splitBody";
            // 
            // splitBody.Panel1
            // 
            this.splitBody.Panel1.Controls.Add(this.grpAssets);
            // 
            // splitBody.Panel2
            // 
            this.splitBody.Panel2.Controls.Add(this.grpLiabilitiesEquity);
            this.splitBody.Size = new System.Drawing.Size(1384, 525);
            this.splitBody.SplitterDistance = 691;
            this.splitBody.TabIndex = 2;
            // 
            // grpAssets
            // 
            this.grpAssets.Controls.Add(this.dgvAssets);
            this.grpAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpAssets.Location = new System.Drawing.Point(0, 0);
            this.grpAssets.Name = "grpAssets";
            this.grpAssets.Padding = new System.Windows.Forms.Padding(10);
            this.grpAssets.Size = new System.Drawing.Size(691, 525);
            this.grpAssets.TabIndex = 0;
            this.grpAssets.TabStop = false;
            this.grpAssets.Text = "Assets";
            // 
            // dgvAssets
            // 
            this.dgvAssets.AllowUserToAddRows = false;
            this.dgvAssets.AllowUserToDeleteRows = false;
            this.dgvAssets.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvAssets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAssets.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colAssetsItem,
            this.colAssetsAmount,
            this.colAssetsComparison,
            this.colAssetsNotes});
            this.dgvAssets.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAssets.Location = new System.Drawing.Point(10, 26);
            this.dgvAssets.MultiSelect = false;
            this.dgvAssets.Name = "dgvAssets";
            this.dgvAssets.ReadOnly = true;
            this.dgvAssets.RowHeadersVisible = false;
            this.dgvAssets.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAssets.Size = new System.Drawing.Size(671, 489);
            this.dgvAssets.TabIndex = 0;
            // 
            // colAssetsItem
            // 
            this.colAssetsItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAssetsItem.DataPropertyName = "LineItem";
            this.colAssetsItem.HeaderText = "Line Item";
            this.colAssetsItem.Name = "colAssetsItem";
            this.colAssetsItem.ReadOnly = true;
            // 
            // colAssetsAmount
            // 
            this.colAssetsAmount.DataPropertyName = "Amount";
            this.colAssetsAmount.HeaderText = "Amount";
            this.colAssetsAmount.Name = "colAssetsAmount";
            this.colAssetsAmount.ReadOnly = true;
            this.colAssetsAmount.Width = 110;
            // 
            // colAssetsComparison
            // 
            this.colAssetsComparison.DataPropertyName = "Comparison";
            this.colAssetsComparison.HeaderText = "Comparison";
            this.colAssetsComparison.Name = "colAssetsComparison";
            this.colAssetsComparison.ReadOnly = true;
            this.colAssetsComparison.Width = 110;
            // 
            // colAssetsNotes
            // 
            this.colAssetsNotes.DataPropertyName = "Notes";
            this.colAssetsNotes.HeaderText = "Notes";
            this.colAssetsNotes.Name = "colAssetsNotes";
            this.colAssetsNotes.ReadOnly = true;
            this.colAssetsNotes.Width = 180;
            // 
            // grpLiabilitiesEquity
            // 
            this.grpLiabilitiesEquity.Controls.Add(this.dgvLiabilitiesEquity);
            this.grpLiabilitiesEquity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpLiabilitiesEquity.Location = new System.Drawing.Point(0, 0);
            this.grpLiabilitiesEquity.Name = "grpLiabilitiesEquity";
            this.grpLiabilitiesEquity.Padding = new System.Windows.Forms.Padding(10);
            this.grpLiabilitiesEquity.Size = new System.Drawing.Size(689, 525);
            this.grpLiabilitiesEquity.TabIndex = 0;
            this.grpLiabilitiesEquity.TabStop = false;
            this.grpLiabilitiesEquity.Text = "Liabilities & Equity";
            // 
            // dgvLiabilitiesEquity
            // 
            this.dgvLiabilitiesEquity.AllowUserToAddRows = false;
            this.dgvLiabilitiesEquity.AllowUserToDeleteRows = false;
            this.dgvLiabilitiesEquity.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.dgvLiabilitiesEquity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLiabilitiesEquity.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLiabItem,
            this.colLiabAmount,
            this.colLiabComparison,
            this.colLiabNotes});
            this.dgvLiabilitiesEquity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLiabilitiesEquity.Location = new System.Drawing.Point(10, 26);
            this.dgvLiabilitiesEquity.MultiSelect = false;
            this.dgvLiabilitiesEquity.Name = "dgvLiabilitiesEquity";
            this.dgvLiabilitiesEquity.ReadOnly = true;
            this.dgvLiabilitiesEquity.RowHeadersVisible = false;
            this.dgvLiabilitiesEquity.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLiabilitiesEquity.Size = new System.Drawing.Size(669, 489);
            this.dgvLiabilitiesEquity.TabIndex = 0;
            // 
            // colLiabItem
            // 
            this.colLiabItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colLiabItem.DataPropertyName = "LineItem";
            this.colLiabItem.HeaderText = "Line Item";
            this.colLiabItem.Name = "colLiabItem";
            this.colLiabItem.ReadOnly = true;
            // 
            // colLiabAmount
            // 
            this.colLiabAmount.DataPropertyName = "Amount";
            this.colLiabAmount.HeaderText = "Amount";
            this.colLiabAmount.Name = "colLiabAmount";
            this.colLiabAmount.ReadOnly = true;
            this.colLiabAmount.Width = 110;
            // 
            // colLiabComparison
            // 
            this.colLiabComparison.DataPropertyName = "Comparison";
            this.colLiabComparison.HeaderText = "Comparison";
            this.colLiabComparison.Name = "colLiabComparison";
            this.colLiabComparison.ReadOnly = true;
            this.colLiabComparison.Width = 110;
            // 
            // colLiabNotes
            // 
            this.colLiabNotes.DataPropertyName = "Notes";
            this.colLiabNotes.HeaderText = "Notes";
            this.colLiabNotes.Name = "colLiabNotes";
            this.colLiabNotes.ReadOnly = true;
            this.colLiabNotes.Width = 180;
            // 
            // grpNotes
            // 
            this.grpNotes.Controls.Add(this.tvNotes);
            this.grpNotes.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.grpNotes.Location = new System.Drawing.Point(0, 641);
            this.grpNotes.Name = "grpNotes";
            this.grpNotes.Padding = new System.Windows.Forms.Padding(10);
            this.grpNotes.Size = new System.Drawing.Size(1384, 151);
            this.grpNotes.TabIndex = 3;
            this.grpNotes.TabStop = false;
            this.grpNotes.Text = "Notes";
            // 
            // tvNotes
            // 
            this.tvNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvNotes.Location = new System.Drawing.Point(10, 26);
            this.tvNotes.Name = "tvNotes";
            this.tvNotes.Size = new System.Drawing.Size(1364, 115);
            this.tvNotes.TabIndex = 0;
            // 
            // frm_BalanceSheetReport
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1384, 792);
            this.Controls.Add(this.splitBody);
            this.Controls.Add(this.grpNotes);
            this.Controls.Add(this.pnlStatus);
            this.Controls.Add(this.pnlFilters);
            this.Name = "frm_BalanceSheetReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Balance Sheet";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.pnlStatus.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitBody)).EndInit();
            this.splitBody.Panel1.ResumeLayout(false);
            this.splitBody.Panel2.ResumeLayout(false);
            this.splitBody.ResumeLayout(false);
            this.grpAssets.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAssets)).EndInit();
            this.grpLiabilitiesEquity.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLiabilitiesEquity)).EndInit();
            this.grpNotes.ResumeLayout(false);
            this.ResumeLayout(false);
        }
    }
}
