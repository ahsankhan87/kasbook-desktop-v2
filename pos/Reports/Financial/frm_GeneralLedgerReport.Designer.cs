using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Financial
{
    partial class frm_GeneralLedgerReport
    {
        private System.ComponentModel.IContainer components = null;

        private Panel _panelMain;
        private Panel _panelTop;
        private TableLayoutPanel _tblFilters;
        private Label _lblAccount;
        private Label _lblFrom;
        private Label _lblTo;
        private Label _lblShow;
        private Label _lblGroupBy;

        private ComboBox _cmbAccount;
        private DateTimePicker _dtFrom;
        private DateTimePicker _dtTo;
        private ComboBox _cmbShow;
        private ComboBox _cmbGroupBy;
        private Button _btnLoad;

        private Panel _panelSummary;
        private FlowLayoutPanel _flowSummary;
        private Label _lblSummaryAccount;
        private Label _lblSummaryType;
        private Label _lblSummaryNormal;
        private Label _lblSummaryCurrent;
        private Label _lblSummaryTurnover;
        private Label _lblSummaryCount;
        private Label _lblSummaryLastTxn;
        private Button _btnViewFullHistory;
        private Button _btnPrint;
        private Button _btnExport;

        private Panel _panelLedgerArea;
        private Panel _panelOpeningWrap;
        private Panel _panelClosingWrap;
        private Panel _panelCenter;
        private DataGridView _gridOpening;
        private DataGridView _gridLedger;
        private DataGridView _gridClosing;
        private Button _btnLoadMore;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_GeneralLedgerReport));
            this._panelMain = new System.Windows.Forms.Panel();
            this._panelLedgerArea = new System.Windows.Forms.Panel();
            this._panelCenter = new System.Windows.Forms.Panel();
            this._gridLedger = new System.Windows.Forms.DataGridView();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.VoucherNo = new System.Windows.Forms.DataGridViewLinkColumn();
            this.VoucherType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RefModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Narration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Debit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Credit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RunningBalance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ReferenceNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._btnLoadMore = new System.Windows.Forms.Button();
            this._panelClosingWrap = new System.Windows.Forms.Panel();
            this._gridClosing = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._panelOpeningWrap = new System.Windows.Forms.Panel();
            this._gridOpening = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this._panelSummary = new System.Windows.Forms.Panel();
            this._flowSummary = new System.Windows.Forms.FlowLayoutPanel();
            this._lblSummaryAccount = new System.Windows.Forms.Label();
            this._lblSummaryType = new System.Windows.Forms.Label();
            this._lblSummaryNormal = new System.Windows.Forms.Label();
            this._lblSummaryCurrent = new System.Windows.Forms.Label();
            this._lblSummaryTurnover = new System.Windows.Forms.Label();
            this._lblSummaryCount = new System.Windows.Forms.Label();
            this._lblSummaryLastTxn = new System.Windows.Forms.Label();
            this._btnViewFullHistory = new System.Windows.Forms.Button();
            this._btnPrint = new System.Windows.Forms.Button();
            this._btnExport = new System.Windows.Forms.Button();
            this._panelTop = new System.Windows.Forms.Panel();
            this._tblFilters = new System.Windows.Forms.TableLayoutPanel();
            this._lblAccount = new System.Windows.Forms.Label();
            this._cmbAccount = new System.Windows.Forms.ComboBox();
            this._btnSearchAccount = new System.Windows.Forms.Button();
            this._lblFrom = new System.Windows.Forms.Label();
            this._dtFrom = new System.Windows.Forms.DateTimePicker();
            this._lblTo = new System.Windows.Forms.Label();
            this._dtTo = new System.Windows.Forms.DateTimePicker();
            this._btnLoad = new System.Windows.Forms.Button();
            this._lblShow = new System.Windows.Forms.Label();
            this._cmbShow = new System.Windows.Forms.ComboBox();
            this._lblGroupBy = new System.Windows.Forms.Label();
            this._cmbGroupBy = new System.Windows.Forms.ComboBox();
            this._panelMain.SuspendLayout();
            this._panelLedgerArea.SuspendLayout();
            this._panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridLedger)).BeginInit();
            this._panelClosingWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridClosing)).BeginInit();
            this._panelOpeningWrap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this._gridOpening)).BeginInit();
            this._panelSummary.SuspendLayout();
            this._flowSummary.SuspendLayout();
            this._panelTop.SuspendLayout();
            this._tblFilters.SuspendLayout();
            this.SuspendLayout();
            // 
            // _panelMain
            // 
            this._panelMain.Controls.Add(this._panelLedgerArea);
            this._panelMain.Controls.Add(this._panelSummary);
            this._panelMain.Controls.Add(this._panelTop);
            resources.ApplyResources(this._panelMain, "_panelMain");
            this._panelMain.Name = "_panelMain";
            // 
            // _panelLedgerArea
            // 
            this._panelLedgerArea.Controls.Add(this._panelCenter);
            this._panelLedgerArea.Controls.Add(this._panelClosingWrap);
            this._panelLedgerArea.Controls.Add(this._panelOpeningWrap);
            resources.ApplyResources(this._panelLedgerArea, "_panelLedgerArea");
            this._panelLedgerArea.Name = "_panelLedgerArea";
            // 
            // _panelCenter
            // 
            this._panelCenter.Controls.Add(this._gridLedger);
            this._panelCenter.Controls.Add(this._btnLoadMore);
            resources.ApplyResources(this._panelCenter, "_panelCenter");
            this._panelCenter.Name = "_panelCenter";
            // 
            // _gridLedger
            // 
            this._gridLedger.AllowUserToAddRows = false;
            this._gridLedger.AllowUserToDeleteRows = false;
            this._gridLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            resources.ApplyResources(this._gridLedger, "_gridLedger");
            this._gridLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Date,
            this.VoucherNo,
            this.VoucherType,
            this.RefModule,
            this.Narration,
            this.Debit,
            this.Credit,
            this.RunningBalance,
            this.ReferenceNo,
            this.Status});
            this._gridLedger.EnableHeadersVisualStyles = false;
            this._gridLedger.MultiSelect = false;
            this._gridLedger.Name = "_gridLedger";
            this._gridLedger.ReadOnly = true;
            this._gridLedger.RowHeadersVisible = false;
            this._gridLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // Date
            // 
            resources.ApplyResources(this.Date, "Date");
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // VoucherNo
            // 
            resources.ApplyResources(this.VoucherNo, "VoucherNo");
            this.VoucherNo.Name = "VoucherNo";
            this.VoucherNo.ReadOnly = true;
            // 
            // VoucherType
            // 
            resources.ApplyResources(this.VoucherType, "VoucherType");
            this.VoucherType.Name = "VoucherType";
            this.VoucherType.ReadOnly = true;
            // 
            // RefModule
            // 
            resources.ApplyResources(this.RefModule, "RefModule");
            this.RefModule.Name = "RefModule";
            this.RefModule.ReadOnly = true;
            // 
            // Narration
            // 
            resources.ApplyResources(this.Narration, "Narration");
            this.Narration.Name = "Narration";
            this.Narration.ReadOnly = true;
            // 
            // Debit
            // 
            resources.ApplyResources(this.Debit, "Debit");
            this.Debit.Name = "Debit";
            this.Debit.ReadOnly = true;
            // 
            // Credit
            // 
            resources.ApplyResources(this.Credit, "Credit");
            this.Credit.Name = "Credit";
            this.Credit.ReadOnly = true;
            // 
            // RunningBalance
            // 
            resources.ApplyResources(this.RunningBalance, "RunningBalance");
            this.RunningBalance.Name = "RunningBalance";
            this.RunningBalance.ReadOnly = true;
            // 
            // ReferenceNo
            // 
            resources.ApplyResources(this.ReferenceNo, "ReferenceNo");
            this.ReferenceNo.Name = "ReferenceNo";
            this.ReferenceNo.ReadOnly = true;
            // 
            // Status
            // 
            resources.ApplyResources(this.Status, "Status");
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // _btnLoadMore
            // 
            resources.ApplyResources(this._btnLoadMore, "_btnLoadMore");
            this._btnLoadMore.Name = "_btnLoadMore";
            this._btnLoadMore.UseVisualStyleBackColor = true;
            // 
            // _panelClosingWrap
            // 
            this._panelClosingWrap.Controls.Add(this._gridClosing);
            resources.ApplyResources(this._panelClosingWrap, "_panelClosingWrap");
            this._panelClosingWrap.Name = "_panelClosingWrap";
            // 
            // _gridClosing
            // 
            this._gridClosing.AllowUserToAddRows = false;
            this._gridClosing.AllowUserToDeleteRows = false;
            this._gridClosing.AllowUserToResizeColumns = false;
            this._gridClosing.AllowUserToResizeRows = false;
            this._gridClosing.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            resources.ApplyResources(this._gridClosing, "_gridClosing");
            this._gridClosing.ColumnHeadersVisible = false;
            this._gridClosing.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this._gridClosing.MultiSelect = false;
            this._gridClosing.Name = "_gridClosing";
            this._gridClosing.ReadOnly = true;
            this._gridClosing.RowHeadersVisible = false;
            this._gridClosing.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // dataGridViewTextBoxColumn1
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn1, "dataGridViewTextBoxColumn1");
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn2, "dataGridViewTextBoxColumn2");
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn3, "dataGridViewTextBoxColumn3");
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn4, "dataGridViewTextBoxColumn4");
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn5, "dataGridViewTextBoxColumn5");
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn6, "dataGridViewTextBoxColumn6");
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn7, "dataGridViewTextBoxColumn7");
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn8, "dataGridViewTextBoxColumn8");
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // _panelOpeningWrap
            // 
            this._panelOpeningWrap.Controls.Add(this._gridOpening);
            resources.ApplyResources(this._panelOpeningWrap, "_panelOpeningWrap");
            this._panelOpeningWrap.Name = "_panelOpeningWrap";
            // 
            // _gridOpening
            // 
            this._gridOpening.AllowUserToAddRows = false;
            this._gridOpening.AllowUserToDeleteRows = false;
            this._gridOpening.AllowUserToResizeColumns = false;
            this._gridOpening.AllowUserToResizeRows = false;
            this._gridOpening.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            resources.ApplyResources(this._gridOpening, "_gridOpening");
            this._gridOpening.ColumnHeadersVisible = false;
            this._gridOpening.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewTextBoxColumn10,
            this.dataGridViewTextBoxColumn11,
            this.dataGridViewTextBoxColumn12,
            this.dataGridViewTextBoxColumn13,
            this.dataGridViewTextBoxColumn14,
            this.dataGridViewTextBoxColumn15,
            this.dataGridViewTextBoxColumn16});
            this._gridOpening.MultiSelect = false;
            this._gridOpening.Name = "_gridOpening";
            this._gridOpening.ReadOnly = true;
            this._gridOpening.RowHeadersVisible = false;
            this._gridOpening.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // dataGridViewTextBoxColumn9
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn9, "dataGridViewTextBoxColumn9");
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn10, "dataGridViewTextBoxColumn10");
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn11, "dataGridViewTextBoxColumn11");
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn12, "dataGridViewTextBoxColumn12");
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn13, "dataGridViewTextBoxColumn13");
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn14
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn14, "dataGridViewTextBoxColumn14");
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn15
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn15, "dataGridViewTextBoxColumn15");
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn16
            // 
            resources.ApplyResources(this.dataGridViewTextBoxColumn16, "dataGridViewTextBoxColumn16");
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            // 
            // _panelSummary
            // 
            this._panelSummary.BackColor = System.Drawing.Color.White;
            this._panelSummary.Controls.Add(this._flowSummary);
            resources.ApplyResources(this._panelSummary, "_panelSummary");
            this._panelSummary.Name = "_panelSummary";
            // 
            // _flowSummary
            // 
            resources.ApplyResources(this._flowSummary, "_flowSummary");
            this._flowSummary.Controls.Add(this._lblSummaryAccount);
            this._flowSummary.Controls.Add(this._lblSummaryType);
            this._flowSummary.Controls.Add(this._lblSummaryNormal);
            this._flowSummary.Controls.Add(this._lblSummaryCurrent);
            this._flowSummary.Controls.Add(this._lblSummaryTurnover);
            this._flowSummary.Controls.Add(this._lblSummaryCount);
            this._flowSummary.Controls.Add(this._lblSummaryLastTxn);
            this._flowSummary.Controls.Add(this._btnViewFullHistory);
            this._flowSummary.Controls.Add(this._btnPrint);
            this._flowSummary.Controls.Add(this._btnExport);
            this._flowSummary.Name = "_flowSummary";
            // 
            // _lblSummaryAccount
            // 
            resources.ApplyResources(this._lblSummaryAccount, "_lblSummaryAccount");
            this._lblSummaryAccount.Name = "_lblSummaryAccount";
            // 
            // _lblSummaryType
            // 
            resources.ApplyResources(this._lblSummaryType, "_lblSummaryType");
            this._lblSummaryType.BackColor = System.Drawing.Color.Gainsboro;
            this._lblSummaryType.Name = "_lblSummaryType";
            // 
            // _lblSummaryNormal
            // 
            resources.ApplyResources(this._lblSummaryNormal, "_lblSummaryNormal");
            this._lblSummaryNormal.Name = "_lblSummaryNormal";
            // 
            // _lblSummaryCurrent
            // 
            resources.ApplyResources(this._lblSummaryCurrent, "_lblSummaryCurrent");
            this._lblSummaryCurrent.Name = "_lblSummaryCurrent";
            // 
            // _lblSummaryTurnover
            // 
            resources.ApplyResources(this._lblSummaryTurnover, "_lblSummaryTurnover");
            this._lblSummaryTurnover.Name = "_lblSummaryTurnover";
            // 
            // _lblSummaryCount
            // 
            resources.ApplyResources(this._lblSummaryCount, "_lblSummaryCount");
            this._lblSummaryCount.Name = "_lblSummaryCount";
            // 
            // _lblSummaryLastTxn
            // 
            resources.ApplyResources(this._lblSummaryLastTxn, "_lblSummaryLastTxn");
            this._lblSummaryLastTxn.Name = "_lblSummaryLastTxn";
            // 
            // _btnViewFullHistory
            // 
            resources.ApplyResources(this._btnViewFullHistory, "_btnViewFullHistory");
            this._btnViewFullHistory.Name = "_btnViewFullHistory";
            this._btnViewFullHistory.UseVisualStyleBackColor = true;
            // 
            // _btnPrint
            // 
            resources.ApplyResources(this._btnPrint, "_btnPrint");
            this._btnPrint.Name = "_btnPrint";
            this._btnPrint.UseVisualStyleBackColor = true;
            // 
            // _btnExport
            // 
            resources.ApplyResources(this._btnExport, "_btnExport");
            this._btnExport.Name = "_btnExport";
            this._btnExport.UseVisualStyleBackColor = true;
            // 
            // _panelTop
            // 
            this._panelTop.Controls.Add(this._tblFilters);
            resources.ApplyResources(this._panelTop, "_panelTop");
            this._panelTop.Name = "_panelTop";
            // 
            // _tblFilters
            // 
            resources.ApplyResources(this._tblFilters, "_tblFilters");
            this._tblFilters.Controls.Add(this._lblAccount, 0, 0);
            this._tblFilters.Controls.Add(this._cmbAccount, 1, 0);
            this._tblFilters.Controls.Add(this._btnSearchAccount, 2, 0);
            this._tblFilters.Controls.Add(this._lblFrom, 3, 0);
            this._tblFilters.Controls.Add(this._dtFrom, 4, 0);
            this._tblFilters.Controls.Add(this._lblTo, 5, 0);
            this._tblFilters.Controls.Add(this._dtTo, 6, 0);
            this._tblFilters.Controls.Add(this._btnLoad, 7, 0);
            this._tblFilters.Controls.Add(this._lblShow, 0, 1);
            this._tblFilters.Controls.Add(this._cmbShow, 1, 1);
            this._tblFilters.Controls.Add(this._lblGroupBy, 2, 1);
            this._tblFilters.Controls.Add(this._cmbGroupBy, 3, 1);
            this._tblFilters.Name = "_tblFilters";
            // 
            // _lblAccount
            // 
            resources.ApplyResources(this._lblAccount, "_lblAccount");
            this._lblAccount.Name = "_lblAccount";
            // 
            // _cmbAccount
            // 
            resources.ApplyResources(this._cmbAccount, "_cmbAccount");
            this._cmbAccount.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._cmbAccount.DropDownHeight = 280;
            this._cmbAccount.Name = "_cmbAccount";
            // 
            // _btnSearchAccount
            // 
            resources.ApplyResources(this._btnSearchAccount, "_btnSearchAccount");
            this._btnSearchAccount.Name = "_btnSearchAccount";
            this._btnSearchAccount.UseVisualStyleBackColor = true;
            // 
            // _lblFrom
            // 
            resources.ApplyResources(this._lblFrom, "_lblFrom");
            this._lblFrom.Name = "_lblFrom";
            // 
            // _dtFrom
            // 
            this._dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this._dtFrom, "_dtFrom");
            this._dtFrom.Name = "_dtFrom";
            this._dtFrom.Value = new System.DateTime(2026, 6, 10, 0, 0, 0, 0);
            // 
            // _lblTo
            // 
            resources.ApplyResources(this._lblTo, "_lblTo");
            this._lblTo.Name = "_lblTo";
            // 
            // _dtTo
            // 
            this._dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this._dtTo, "_dtTo");
            this._dtTo.Name = "_dtTo";
            this._dtTo.Value = new System.DateTime(2026, 7, 10, 0, 0, 0, 0);
            // 
            // _btnLoad
            // 
            resources.ApplyResources(this._btnLoad, "_btnLoad");
            this._btnLoad.Name = "_btnLoad";
            this._btnLoad.UseVisualStyleBackColor = true;
            // 
            // _lblShow
            // 
            resources.ApplyResources(this._lblShow, "_lblShow");
            this._lblShow.Name = "_lblShow";
            // 
            // _cmbShow
            // 
            this._cmbShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbShow.FormattingEnabled = true;
            this._cmbShow.Items.AddRange(new object[] {
            resources.GetString("_cmbShow.Items"),
            resources.GetString("_cmbShow.Items1"),
            resources.GetString("_cmbShow.Items2"),
            resources.GetString("_cmbShow.Items3")});
            resources.ApplyResources(this._cmbShow, "_cmbShow");
            this._cmbShow.Name = "_cmbShow";
            // 
            // _lblGroupBy
            // 
            resources.ApplyResources(this._lblGroupBy, "_lblGroupBy");
            this._lblGroupBy.Name = "_lblGroupBy";
            // 
            // _cmbGroupBy
            // 
            this._cmbGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbGroupBy.FormattingEnabled = true;
            this._cmbGroupBy.Items.AddRange(new object[] {
            resources.GetString("_cmbGroupBy.Items"),
            resources.GetString("_cmbGroupBy.Items1"),
            resources.GetString("_cmbGroupBy.Items2")});
            resources.ApplyResources(this._cmbGroupBy, "_cmbGroupBy");
            this._cmbGroupBy.Name = "_cmbGroupBy";
            // 
            // frm_GeneralLedgerReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this._panelMain);
            this.Name = "frm_GeneralLedgerReport";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this._panelMain.ResumeLayout(false);
            this._panelLedgerArea.ResumeLayout(false);
            this._panelCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridLedger)).EndInit();
            this._panelClosingWrap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridClosing)).EndInit();
            this._panelOpeningWrap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._gridOpening)).EndInit();
            this._panelSummary.ResumeLayout(false);
            this._flowSummary.ResumeLayout(false);
            this._flowSummary.PerformLayout();
            this._panelTop.ResumeLayout(false);
            this._tblFilters.ResumeLayout(false);
            this._tblFilters.PerformLayout();
            this.ResumeLayout(false);

        }

        private DataGridViewTextBoxColumn colDate;
        private DataGridViewLinkColumn colVoucherNo;
        private DataGridViewTextBoxColumn colVoucherType;
        private DataGridViewTextBoxColumn colRefModule;
        private DataGridViewTextBoxColumn colNarration;
        private DataGridViewTextBoxColumn colDebit;
        private DataGridViewTextBoxColumn colCredit;
        private DataGridViewTextBoxColumn colRunningBalance;
        private DataGridViewTextBoxColumn colRefId;
        private DataGridViewTextBoxColumn colStatus;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn12;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn13;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn14;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn15;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn16;
        private Button _btnSearchAccount;
        private DataGridViewTextBoxColumn Date;
        private DataGridViewLinkColumn VoucherNo;
        private DataGridViewTextBoxColumn VoucherType;
        private DataGridViewTextBoxColumn RefModule;
        private DataGridViewTextBoxColumn Narration;
        private DataGridViewTextBoxColumn Debit;
        private DataGridViewTextBoxColumn Credit;
        private DataGridViewTextBoxColumn RunningBalance;
        private DataGridViewTextBoxColumn ReferenceNo;
        private DataGridViewTextBoxColumn Status;
    }
}
