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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_ProfitAndLossReport));
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
            resources.ApplyResources(this.pnlFilters, "pnlFilters");
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
            this.pnlFilters.Name = "pnlFilters";
            // 
            // lblPeriod
            // 
            resources.ApplyResources(this.lblPeriod, "lblPeriod");
            this.lblPeriod.Name = "lblPeriod";
            // 
            // cmbPeriod
            // 
            resources.ApplyResources(this.cmbPeriod, "cmbPeriod");
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.Name = "cmbPeriod";
            // 
            // lblFrom
            // 
            resources.ApplyResources(this.lblFrom, "lblFrom");
            this.lblFrom.Name = "lblFrom";
            // 
            // dtpFrom
            // 
            resources.ApplyResources(this.dtpFrom, "dtpFrom");
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Name = "dtpFrom";
            // 
            // lblTo
            // 
            resources.ApplyResources(this.lblTo, "lblTo");
            this.lblTo.Name = "lblTo";
            // 
            // dtpTo
            // 
            resources.ApplyResources(this.dtpTo, "dtpTo");
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Name = "dtpTo";
            // 
            // chkShowComparison
            // 
            resources.ApplyResources(this.chkShowComparison, "chkShowComparison");
            this.chkShowComparison.Name = "chkShowComparison";
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
            this.cmbDetailLevel.Name = "cmbDetailLevel";
            // 
            // lblCostCenter
            // 
            resources.ApplyResources(this.lblCostCenter, "lblCostCenter");
            this.lblCostCenter.Name = "lblCostCenter";
            // 
            // cmbCostCenter
            // 
            resources.ApplyResources(this.cmbCostCenter, "cmbCostCenter");
            this.cmbCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCostCenter.Name = "cmbCostCenter";
            // 
            // chkShowPercentage
            // 
            resources.ApplyResources(this.chkShowPercentage, "chkShowPercentage");
            this.chkShowPercentage.Name = "chkShowPercentage";
            // 
            // btnGenerate
            // 
            resources.ApplyResources(this.btnGenerate, "btnGenerate");
            this.btnGenerate.Name = "btnGenerate";
            // 
            // pnlActions
            // 
            resources.ApplyResources(this.pnlActions, "pnlActions");
            this.pnlActions.Controls.Add(this.btnPrintPreview);
            this.pnlActions.Controls.Add(this.btnExportExcel);
            this.pnlActions.Controls.Add(this.btnExportPdf);
            this.pnlActions.Controls.Add(this.btnEmail);
            this.pnlActions.Name = "pnlActions";
            // 
            // btnPrintPreview
            // 
            resources.ApplyResources(this.btnPrintPreview, "btnPrintPreview");
            this.btnPrintPreview.Name = "btnPrintPreview";
            // 
            // btnExportExcel
            // 
            resources.ApplyResources(this.btnExportExcel, "btnExportExcel");
            this.btnExportExcel.Name = "btnExportExcel";
            // 
            // btnExportPdf
            // 
            resources.ApplyResources(this.btnExportPdf, "btnExportPdf");
            this.btnExportPdf.Name = "btnExportPdf";
            // 
            // btnEmail
            // 
            resources.ApplyResources(this.btnEmail, "btnEmail");
            this.btnEmail.Name = "btnEmail";
            // 
            // pnlHeader
            // 
            resources.ApplyResources(this.pnlHeader, "pnlHeader");
            this.pnlHeader.Controls.Add(this.lblCompanyName);
            this.pnlHeader.Controls.Add(this.lblReportTitle);
            this.pnlHeader.Controls.Add(this.lblReportPeriod);
            this.pnlHeader.Controls.Add(this.lblGeneratedOn);
            this.pnlHeader.Controls.Add(this.lblGeneratedBy);
            this.pnlHeader.Name = "pnlHeader";
            // 
            // lblCompanyName
            // 
            resources.ApplyResources(this.lblCompanyName, "lblCompanyName");
            this.lblCompanyName.Name = "lblCompanyName";
            // 
            // lblReportTitle
            // 
            resources.ApplyResources(this.lblReportTitle, "lblReportTitle");
            this.lblReportTitle.Name = "lblReportTitle";
            // 
            // lblReportPeriod
            // 
            resources.ApplyResources(this.lblReportPeriod, "lblReportPeriod");
            this.lblReportPeriod.Name = "lblReportPeriod";
            // 
            // lblGeneratedOn
            // 
            resources.ApplyResources(this.lblGeneratedOn, "lblGeneratedOn");
            this.lblGeneratedOn.Name = "lblGeneratedOn";
            // 
            // lblGeneratedBy
            // 
            resources.ApplyResources(this.lblGeneratedBy, "lblGeneratedBy");
            this.lblGeneratedBy.Name = "lblGeneratedBy";
            // 
            // pnlTreeColumns
            // 
            resources.ApplyResources(this.pnlTreeColumns, "pnlTreeColumns");
            this.pnlTreeColumns.Controls.Add(this.lblColAccount);
            this.pnlTreeColumns.Controls.Add(this.lblColAmount);
            this.pnlTreeColumns.Controls.Add(this.lblColPercent);
            this.pnlTreeColumns.Controls.Add(this.lblColPrevious);
            this.pnlTreeColumns.Controls.Add(this.lblColVariance);
            this.pnlTreeColumns.Name = "pnlTreeColumns";
            // 
            // lblColAccount
            // 
            resources.ApplyResources(this.lblColAccount, "lblColAccount");
            this.lblColAccount.Name = "lblColAccount";
            this.lblColAccount.Click += new System.EventHandler(this.lblColAccount_Click);
            // 
            // lblColAmount
            // 
            resources.ApplyResources(this.lblColAmount, "lblColAmount");
            this.lblColAmount.Name = "lblColAmount";
            // 
            // lblColPercent
            // 
            resources.ApplyResources(this.lblColPercent, "lblColPercent");
            this.lblColPercent.Name = "lblColPercent";
            // 
            // lblColPrevious
            // 
            resources.ApplyResources(this.lblColPrevious, "lblColPrevious");
            this.lblColPrevious.Name = "lblColPrevious";
            // 
            // lblColVariance
            // 
            resources.ApplyResources(this.lblColVariance, "lblColVariance");
            this.lblColVariance.Name = "lblColVariance";
            // 
            // treeReport
            // 
            resources.ApplyResources(this.treeReport, "treeReport");
            this.treeReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.treeReport.DrawMode = System.Windows.Forms.TreeViewDrawMode.OwnerDrawText;
            this.treeReport.FullRowSelect = true;
            this.treeReport.HideSelection = false;
            this.treeReport.Name = "treeReport";
            // 
            // frm_ProfitAndLossReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.treeReport);
            this.Controls.Add(this.pnlTreeColumns);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlActions);
            this.Controls.Add(this.pnlFilters);
            this.Name = "frm_ProfitAndLossReport";
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
