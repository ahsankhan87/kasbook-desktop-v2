using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Financial
{
    public partial class frm_ProfitAndLossReport
    {
        private Panel pnlFilters;
        private Label lblPeriod;
        private ComboBox cmbPeriod;
        private Label lblFrom;
        private Label lblTo;
        private DateTimePicker dtpFrom;
        private DateTimePicker dtpTo;
        private CheckBox chkShowComparison;
        private Label lblDetailLevel;
        private ComboBox cmbDetailLevel;
        private Label lblCostCenter;
        private ComboBox cmbCostCenter;
        private Button btnGenerate;
        private CheckBox chkShowPercentage;
        private Panel pnlActions;
        private Button btnPrintPreview;
        private Button btnExportExcel;
        private Button btnExportPdf;
        private Button btnEmail;
        private Panel pnlHeader;
        private Label lblCompanyName;
        private Label lblReportTitle;
        private Label lblReportPeriod;
        private Label lblGeneratedOn;
        private Label lblGeneratedBy;
        private Panel pnlTreeColumns;
        private Label lblColAccount;
        private Label lblColAmount;
        private Label lblColPercent;
        private Label lblColPrevious;
        private Label lblColVariance;
        private TreeView treeReport;

        private void InitializeComponent()
        {
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.chkShowComparison = new System.Windows.Forms.CheckBox();
            this.lblDetailLevel = new System.Windows.Forms.Label();
            this.cmbDetailLevel = new System.Windows.Forms.ComboBox();
            this.lblCostCenter = new System.Windows.Forms.Label();
            this.cmbCostCenter = new System.Windows.Forms.ComboBox();
            this.chkShowPercentage = new System.Windows.Forms.CheckBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnPrintPreview = new System.Windows.Forms.Button();
            this.btnExportExcel = new System.Windows.Forms.Button();
            this.btnExportPdf = new System.Windows.Forms.Button();
            this.btnEmail = new System.Windows.Forms.Button();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblCompanyName = new System.Windows.Forms.Label();
            this.lblReportTitle = new System.Windows.Forms.Label();
            this.lblReportPeriod = new System.Windows.Forms.Label();
            this.lblGeneratedOn = new System.Windows.Forms.Label();
            this.lblGeneratedBy = new System.Windows.Forms.Label();
            this.pnlTreeColumns = new System.Windows.Forms.Panel();
            this.lblColAccount = new System.Windows.Forms.Label();
            this.lblColAmount = new System.Windows.Forms.Label();
            this.lblColPercent = new System.Windows.Forms.Label();
            this.lblColPrevious = new System.Windows.Forms.Label();
            this.lblColVariance = new System.Windows.Forms.Label();
            this.treeReport = new System.Windows.Forms.TreeView();
            this.pnlFilters.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlTreeColumns.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.lblPeriod);
            this.pnlFilters.Controls.Add(this.cmbPeriod);
            this.pnlFilters.Controls.Add(this.lblFrom);
            this.pnlFilters.Controls.Add(this.dtpFrom);
            this.pnlFilters.Controls.Add(this.lblTo);
            this.pnlFilters.Controls.Add(this.dtpTo);
            this.pnlFilters.Controls.Add(this.chkShowComparison);
            this.pnlFilters.Controls.Add(this.lblDetailLevel);
            this.pnlFilters.Controls.Add(this.cmbDetailLevel);
            this.pnlFilters.Controls.Add(this.lblCostCenter);
            this.pnlFilters.Controls.Add(this.cmbCostCenter);
            this.pnlFilters.Controls.Add(this.chkShowPercentage);
            this.pnlFilters.Controls.Add(this.btnGenerate);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(1330, 67);
            this.pnlFilters.TabIndex = 4;
            // 
            // lblPeriod
            // 
            this.lblPeriod.Location = new System.Drawing.Point(3, 17);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(53, 23);
            this.lblPeriod.TabIndex = 0;
            this.lblPeriod.Text = "Period:";
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.Location = new System.Drawing.Point(62, 14);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(140, 24);
            this.cmbPeriod.TabIndex = 1;
            // 
            // lblFrom
            // 
            this.lblFrom.Location = new System.Drawing.Point(233, 17);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(53, 23);
            this.lblFrom.TabIndex = 2;
            this.lblFrom.Text = "From";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(295, 14);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(92, 24);
            this.dtpFrom.TabIndex = 3;
            // 
            // lblTo
            // 
            this.lblTo.Location = new System.Drawing.Point(416, 17);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(26, 23);
            this.lblTo.TabIndex = 4;
            this.lblTo.Text = "To";
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(448, 14);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(92, 24);
            this.dtpTo.TabIndex = 5;
            // 
            // chkShowComparison
            // 
            this.chkShowComparison.AutoSize = true;
            this.chkShowComparison.Location = new System.Drawing.Point(62, 42);
            this.chkShowComparison.Name = "chkShowComparison";
            this.chkShowComparison.Size = new System.Drawing.Size(213, 21);
            this.chkShowComparison.TabIndex = 6;
            this.chkShowComparison.Text = "Compare with previous period";
            // 
            // lblDetailLevel
            // 
            this.lblDetailLevel.Location = new System.Drawing.Point(604, 17);
            this.lblDetailLevel.Name = "lblDetailLevel";
            this.lblDetailLevel.Size = new System.Drawing.Size(100, 23);
            this.lblDetailLevel.TabIndex = 7;
            this.lblDetailLevel.Text = "Show Detail:";
            // 
            // cmbDetailLevel
            // 
            this.cmbDetailLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDetailLevel.Location = new System.Drawing.Point(710, 14);
            this.cmbDetailLevel.Name = "cmbDetailLevel";
            this.cmbDetailLevel.Size = new System.Drawing.Size(175, 24);
            this.cmbDetailLevel.TabIndex = 8;
            // 
            // lblCostCenter
            // 
            this.lblCostCenter.Location = new System.Drawing.Point(891, 17);
            this.lblCostCenter.Name = "lblCostCenter";
            this.lblCostCenter.Size = new System.Drawing.Size(100, 23);
            this.lblCostCenter.TabIndex = 9;
            this.lblCostCenter.Text = "Cost Center:";
            // 
            // cmbCostCenter
            // 
            this.cmbCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCostCenter.Location = new System.Drawing.Point(997, 14);
            this.cmbCostCenter.Name = "cmbCostCenter";
            this.cmbCostCenter.Size = new System.Drawing.Size(130, 24);
            this.cmbCostCenter.TabIndex = 10;
            // 
            // chkShowPercentage
            // 
            this.chkShowPercentage.AutoSize = true;
            this.chkShowPercentage.Location = new System.Drawing.Point(1133, 16);
            this.chkShowPercentage.Name = "chkShowPercentage";
            this.chkShowPercentage.Size = new System.Drawing.Size(148, 21);
            this.chkShowPercentage.TabIndex = 11;
            this.chkShowPercentage.Text = "Show % of Income";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(2360, 13);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(90, 27);
            this.btnGenerate.TabIndex = 12;
            this.btnGenerate.Text = "Generate";
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnPrintPreview);
            this.pnlActions.Controls.Add(this.btnExportExcel);
            this.pnlActions.Controls.Add(this.btnExportPdf);
            this.pnlActions.Controls.Add(this.btnEmail);
            this.pnlActions.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlActions.Location = new System.Drawing.Point(0, 67);
            this.pnlActions.Name = "pnlActions";
            this.pnlActions.Size = new System.Drawing.Size(1330, 42);
            this.pnlActions.TabIndex = 3;
            // 
            // btnPrintPreview
            // 
            this.btnPrintPreview.Location = new System.Drawing.Point(15, 8);
            this.btnPrintPreview.Name = "btnPrintPreview";
            this.btnPrintPreview.Size = new System.Drawing.Size(120, 27);
            this.btnPrintPreview.TabIndex = 0;
            this.btnPrintPreview.Text = "Print Preview";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Location = new System.Drawing.Point(141, 8);
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(120, 27);
            this.btnExportExcel.TabIndex = 1;
            this.btnExportExcel.Text = "Export Excel";
            // 
            // btnExportPdf
            // 
            this.btnExportPdf.Location = new System.Drawing.Point(267, 8);
            this.btnExportPdf.Name = "btnExportPdf";
            this.btnExportPdf.Size = new System.Drawing.Size(120, 27);
            this.btnExportPdf.TabIndex = 2;
            this.btnExportPdf.Text = "Export PDF";
            // 
            // btnEmail
            // 
            this.btnEmail.Location = new System.Drawing.Point(393, 8);
            this.btnEmail.Name = "btnEmail";
            this.btnEmail.Size = new System.Drawing.Size(120, 27);
            this.btnEmail.TabIndex = 3;
            this.btnEmail.Text = "Email";
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.lblCompanyName);
            this.pnlHeader.Controls.Add(this.lblReportTitle);
            this.pnlHeader.Controls.Add(this.lblReportPeriod);
            this.pnlHeader.Controls.Add(this.lblGeneratedOn);
            this.pnlHeader.Controls.Add(this.lblGeneratedBy);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 109);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1330, 92);
            this.pnlHeader.TabIndex = 2;
            // 
            // lblCompanyName
            // 
            this.lblCompanyName.AutoSize = true;
            this.lblCompanyName.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lblCompanyName.Location = new System.Drawing.Point(12, 8);
            this.lblCompanyName.Name = "lblCompanyName";
            this.lblCompanyName.Size = new System.Drawing.Size(0, 25);
            this.lblCompanyName.TabIndex = 0;
            // 
            // lblReportTitle
            // 
            this.lblReportTitle.AutoSize = true;
            this.lblReportTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblReportTitle.Location = new System.Drawing.Point(12, 30);
            this.lblReportTitle.Name = "lblReportTitle";
            this.lblReportTitle.Size = new System.Drawing.Size(0, 28);
            this.lblReportTitle.TabIndex = 1;
            // 
            // lblReportPeriod
            // 
            this.lblReportPeriod.AutoSize = true;
            this.lblReportPeriod.Location = new System.Drawing.Point(12, 62);
            this.lblReportPeriod.Name = "lblReportPeriod";
            this.lblReportPeriod.Size = new System.Drawing.Size(0, 17);
            this.lblReportPeriod.TabIndex = 2;
            // 
            // lblGeneratedOn
            // 
            this.lblGeneratedOn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneratedOn.AutoSize = true;
            this.lblGeneratedOn.Location = new System.Drawing.Point(2150, 46);
            this.lblGeneratedOn.Name = "lblGeneratedOn";
            this.lblGeneratedOn.Size = new System.Drawing.Size(0, 17);
            this.lblGeneratedOn.TabIndex = 3;
            // 
            // lblGeneratedBy
            // 
            this.lblGeneratedBy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblGeneratedBy.AutoSize = true;
            this.lblGeneratedBy.Location = new System.Drawing.Point(2150, 64);
            this.lblGeneratedBy.Name = "lblGeneratedBy";
            this.lblGeneratedBy.Size = new System.Drawing.Size(0, 17);
            this.lblGeneratedBy.TabIndex = 4;
            // 
            // pnlTreeColumns
            // 
            this.pnlTreeColumns.Controls.Add(this.lblColAccount);
            this.pnlTreeColumns.Controls.Add(this.lblColAmount);
            this.pnlTreeColumns.Controls.Add(this.lblColPercent);
            this.pnlTreeColumns.Controls.Add(this.lblColPrevious);
            this.pnlTreeColumns.Controls.Add(this.lblColVariance);
            this.pnlTreeColumns.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTreeColumns.Location = new System.Drawing.Point(0, 201);
            this.pnlTreeColumns.Name = "pnlTreeColumns";
            this.pnlTreeColumns.Size = new System.Drawing.Size(1330, 28);
            this.pnlTreeColumns.TabIndex = 1;
            // 
            // lblColAccount
            // 
            this.lblColAccount.Location = new System.Drawing.Point(14, 2);
            this.lblColAccount.Name = "lblColAccount";
            this.lblColAccount.Size = new System.Drawing.Size(100, 23);
            this.lblColAccount.TabIndex = 0;
            this.lblColAccount.Text = "Account / Group";
            // 
            // lblColAmount
            // 
            this.lblColAmount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColAmount.Location = new System.Drawing.Point(2035, 8);
            this.lblColAmount.Name = "lblColAmount";
            this.lblColAmount.Size = new System.Drawing.Size(100, 23);
            this.lblColAmount.TabIndex = 1;
            this.lblColAmount.Text = "Amount";
            // 
            // lblColPercent
            // 
            this.lblColPercent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColPercent.Location = new System.Drawing.Point(2117, 8);
            this.lblColPercent.Name = "lblColPercent";
            this.lblColPercent.Size = new System.Drawing.Size(100, 23);
            this.lblColPercent.TabIndex = 2;
            this.lblColPercent.Text = "% Income";
            // 
            // lblColPrevious
            // 
            this.lblColPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColPrevious.Location = new System.Drawing.Point(2208, 8);
            this.lblColPrevious.Name = "lblColPrevious";
            this.lblColPrevious.Size = new System.Drawing.Size(100, 23);
            this.lblColPrevious.TabIndex = 3;
            this.lblColPrevious.Text = "Previous Period";
            // 
            // lblColVariance
            // 
            this.lblColVariance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblColVariance.Location = new System.Drawing.Point(2332, 8);
            this.lblColVariance.Name = "lblColVariance";
            this.lblColVariance.Size = new System.Drawing.Size(100, 23);
            this.lblColVariance.TabIndex = 4;
            this.lblColVariance.Text = "Variance";
            // 
            // treeReport
            // 
            this.treeReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeReport.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeReport.FullRowSelect = true;
            this.treeReport.HideSelection = false;
            this.treeReport.Indent = 20;
            this.treeReport.Location = new System.Drawing.Point(0, 229);
            this.treeReport.Name = "treeReport";
            this.treeReport.Size = new System.Drawing.Size(1330, 531);
            this.treeReport.TabIndex = 0;
            // 
            // frm_ProfitAndLossReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1330, 760);
            this.Controls.Add(this.treeReport);
            this.Controls.Add(this.pnlTreeColumns);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlFilters);
            this.Name = "frm_ProfitAndLossReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Income Statement (Profit & Loss)";
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.pnlActions.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlTreeColumns.ResumeLayout(false);
            this.ResumeLayout(false);

        }
    }
}
