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
            this._panelMain = new System.Windows.Forms.Panel();
            this._panelLedgerArea = new System.Windows.Forms.Panel();
            this._panelCenter = new System.Windows.Forms.Panel();
            this._gridLedger = new System.Windows.Forms.DataGridView();
            this._btnLoadMore = new System.Windows.Forms.Button();
            this._panelClosingWrap = new System.Windows.Forms.Panel();
            this._gridClosing = new System.Windows.Forms.DataGridView();
            this._panelOpeningWrap = new System.Windows.Forms.Panel();
            this._gridOpening = new System.Windows.Forms.DataGridView();
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
            this._lblFrom = new System.Windows.Forms.Label();
            this._dtFrom = new System.Windows.Forms.DateTimePicker();
            this._lblTo = new System.Windows.Forms.Label();
            this._dtTo = new System.Windows.Forms.DateTimePicker();
            this._btnLoad = new System.Windows.Forms.Button();
            this._lblShow = new System.Windows.Forms.Label();
            this._cmbShow = new System.Windows.Forms.ComboBox();
            this._lblGroupBy = new System.Windows.Forms.Label();
            this._cmbGroupBy = new System.Windows.Forms.ComboBox();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn12 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn13 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn14 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn15 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn16 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn17 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewLinkColumn1 = new System.Windows.Forms.DataGridViewLinkColumn();
            this.dataGridViewTextBoxColumn18 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn20 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn21 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn22 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn23 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn24 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn25 = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this._panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelMain.Location = new System.Drawing.Point(0, 0);
            this._panelMain.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._panelMain.Name = "_panelMain";
            this._panelMain.Padding = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this._panelMain.Size = new System.Drawing.Size(1493, 911);
            this._panelMain.TabIndex = 0;
            // 
            // _panelLedgerArea
            // 
            this._panelLedgerArea.Controls.Add(this._panelCenter);
            this._panelLedgerArea.Controls.Add(this._panelClosingWrap);
            this._panelLedgerArea.Controls.Add(this._panelOpeningWrap);
            this._panelLedgerArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelLedgerArea.Location = new System.Drawing.Point(9, 106);
            this._panelLedgerArea.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._panelLedgerArea.Name = "_panelLedgerArea";
            this._panelLedgerArea.Padding = new System.Windows.Forms.Padding(0, 0, 9, 0);
            this._panelLedgerArea.Size = new System.Drawing.Size(1242, 795);
            this._panelLedgerArea.TabIndex = 0;
            // 
            // _panelCenter
            // 
            this._panelCenter.Controls.Add(this._gridLedger);
            this._panelCenter.Controls.Add(this._btnLoadMore);
            this._panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this._panelCenter.Location = new System.Drawing.Point(0, 54);
            this._panelCenter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._panelCenter.Name = "_panelCenter";
            this._panelCenter.Size = new System.Drawing.Size(1233, 687);
            this._panelCenter.TabIndex = 0;
            // 
            // _gridLedger
            // 
            this._gridLedger.AllowUserToAddRows = false;
            this._gridLedger.AllowUserToDeleteRows = false;
            this._gridLedger.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridLedger.ColumnHeadersHeight = 29;
            this._gridLedger.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn17,
            this.dataGridViewLinkColumn1,
            this.dataGridViewTextBoxColumn18,
            this.dataGridViewTextBoxColumn19,
            this.dataGridViewTextBoxColumn20,
            this.dataGridViewTextBoxColumn21,
            this.dataGridViewTextBoxColumn22,
            this.dataGridViewTextBoxColumn23,
            this.dataGridViewTextBoxColumn24,
            this.dataGridViewTextBoxColumn25});
            this._gridLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridLedger.EnableHeadersVisualStyles = false;
            this._gridLedger.Location = new System.Drawing.Point(0, 0);
            this._gridLedger.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._gridLedger.MultiSelect = false;
            this._gridLedger.Name = "_gridLedger";
            this._gridLedger.ReadOnly = true;
            this._gridLedger.RowHeadersVisible = false;
            this._gridLedger.RowHeadersWidth = 51;
            this._gridLedger.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridLedger.Size = new System.Drawing.Size(1233, 648);
            this._gridLedger.TabIndex = 0;
            // 
            // _btnLoadMore
            // 
            this._btnLoadMore.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._btnLoadMore.Location = new System.Drawing.Point(0, 648);
            this._btnLoadMore.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnLoadMore.Name = "_btnLoadMore";
            this._btnLoadMore.Size = new System.Drawing.Size(1233, 39);
            this._btnLoadMore.TabIndex = 1;
            this._btnLoadMore.Text = "Load More";
            this._btnLoadMore.UseVisualStyleBackColor = true;
            this._btnLoadMore.Visible = false;
            // 
            // _panelClosingWrap
            // 
            this._panelClosingWrap.Controls.Add(this._gridClosing);
            this._panelClosingWrap.Dock = System.Windows.Forms.DockStyle.Bottom;
            this._panelClosingWrap.Location = new System.Drawing.Point(0, 741);
            this._panelClosingWrap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._panelClosingWrap.Name = "_panelClosingWrap";
            this._panelClosingWrap.Size = new System.Drawing.Size(1233, 54);
            this._panelClosingWrap.TabIndex = 1;
            // 
            // _gridClosing
            // 
            this._gridClosing.AllowUserToAddRows = false;
            this._gridClosing.AllowUserToDeleteRows = false;
            this._gridClosing.AllowUserToResizeColumns = false;
            this._gridClosing.AllowUserToResizeRows = false;
            this._gridClosing.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridClosing.ColumnHeadersHeight = 29;
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
            this._gridClosing.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridClosing.Location = new System.Drawing.Point(0, 0);
            this._gridClosing.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._gridClosing.MultiSelect = false;
            this._gridClosing.Name = "_gridClosing";
            this._gridClosing.ReadOnly = true;
            this._gridClosing.RowHeadersVisible = false;
            this._gridClosing.RowHeadersWidth = 51;
            this._gridClosing.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._gridClosing.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridClosing.Size = new System.Drawing.Size(1233, 54);
            this._gridClosing.TabIndex = 0;
            // 
            // _panelOpeningWrap
            // 
            this._panelOpeningWrap.Controls.Add(this._gridOpening);
            this._panelOpeningWrap.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelOpeningWrap.Location = new System.Drawing.Point(0, 0);
            this._panelOpeningWrap.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._panelOpeningWrap.Name = "_panelOpeningWrap";
            this._panelOpeningWrap.Size = new System.Drawing.Size(1233, 54);
            this._panelOpeningWrap.TabIndex = 2;
            // 
            // _gridOpening
            // 
            this._gridOpening.AllowUserToAddRows = false;
            this._gridOpening.AllowUserToDeleteRows = false;
            this._gridOpening.AllowUserToResizeColumns = false;
            this._gridOpening.AllowUserToResizeRows = false;
            this._gridOpening.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this._gridOpening.ColumnHeadersHeight = 29;
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
            this._gridOpening.Dock = System.Windows.Forms.DockStyle.Fill;
            this._gridOpening.Location = new System.Drawing.Point(0, 0);
            this._gridOpening.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._gridOpening.MultiSelect = false;
            this._gridOpening.Name = "_gridOpening";
            this._gridOpening.ReadOnly = true;
            this._gridOpening.RowHeadersVisible = false;
            this._gridOpening.RowHeadersWidth = 51;
            this._gridOpening.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this._gridOpening.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this._gridOpening.Size = new System.Drawing.Size(1233, 54);
            this._gridOpening.TabIndex = 0;
            // 
            // _panelSummary
            // 
            this._panelSummary.BackColor = System.Drawing.Color.White;
            this._panelSummary.Controls.Add(this._flowSummary);
            this._panelSummary.Dock = System.Windows.Forms.DockStyle.Right;
            this._panelSummary.Location = new System.Drawing.Point(1251, 106);
            this._panelSummary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._panelSummary.Name = "_panelSummary";
            this._panelSummary.Padding = new System.Windows.Forms.Padding(9, 10, 9, 10);
            this._panelSummary.Size = new System.Drawing.Size(233, 795);
            this._panelSummary.TabIndex = 1;
            // 
            // _flowSummary
            // 
            this._flowSummary.AutoScroll = true;
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
            this._flowSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this._flowSummary.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this._flowSummary.Location = new System.Drawing.Point(9, 10);
            this._flowSummary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._flowSummary.Name = "_flowSummary";
            this._flowSummary.Size = new System.Drawing.Size(215, 775);
            this._flowSummary.TabIndex = 0;
            this._flowSummary.WrapContents = false;
            // 
            // _lblSummaryAccount
            // 
            this._lblSummaryAccount.AutoSize = true;
            this._lblSummaryAccount.Font = new System.Drawing.Font("Segoe UI Semibold", 11F);
            this._lblSummaryAccount.Location = new System.Drawing.Point(4, 0);
            this._lblSummaryAccount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblSummaryAccount.MaximumSize = new System.Drawing.Size(204, 0);
            this._lblSummaryAccount.Name = "_lblSummaryAccount";
            this._lblSummaryAccount.Size = new System.Drawing.Size(20, 25);
            this._lblSummaryAccount.TabIndex = 0;
            this._lblSummaryAccount.Text = "-";
            // 
            // _lblSummaryType
            // 
            this._lblSummaryType.AutoSize = true;
            this._lblSummaryType.BackColor = System.Drawing.Color.Gainsboro;
            this._lblSummaryType.Location = new System.Drawing.Point(4, 25);
            this._lblSummaryType.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblSummaryType.Name = "_lblSummaryType";
            this._lblSummaryType.Padding = new System.Windows.Forms.Padding(7, 2, 7, 2);
            this._lblSummaryType.Size = new System.Drawing.Size(67, 21);
            this._lblSummaryType.TabIndex = 1;
            this._lblSummaryType.Text = "Type: -";
            // 
            // _lblSummaryNormal
            // 
            this._lblSummaryNormal.AutoSize = true;
            this._lblSummaryNormal.Location = new System.Drawing.Point(4, 46);
            this._lblSummaryNormal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblSummaryNormal.Name = "_lblSummaryNormal";
            this._lblSummaryNormal.Size = new System.Drawing.Size(115, 17);
            this._lblSummaryNormal.TabIndex = 2;
            this._lblSummaryNormal.Text = "Normal Balance: -";
            // 
            // _lblSummaryCurrent
            // 
            this._lblSummaryCurrent.AutoSize = true;
            this._lblSummaryCurrent.Font = new System.Drawing.Font("Segoe UI Semibold", 16F);
            this._lblSummaryCurrent.Location = new System.Drawing.Point(0, 70);
            this._lblSummaryCurrent.Margin = new System.Windows.Forms.Padding(0, 7, 0, 7);
            this._lblSummaryCurrent.Name = "_lblSummaryCurrent";
            this._lblSummaryCurrent.Size = new System.Drawing.Size(105, 37);
            this._lblSummaryCurrent.TabIndex = 3;
            this._lblSummaryCurrent.Text = "0.00 Dr";
            // 
            // _lblSummaryTurnover
            // 
            this._lblSummaryTurnover.AutoSize = true;
            this._lblSummaryTurnover.Location = new System.Drawing.Point(4, 114);
            this._lblSummaryTurnover.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblSummaryTurnover.MaximumSize = new System.Drawing.Size(204, 0);
            this._lblSummaryTurnover.Name = "_lblSummaryTurnover";
            this._lblSummaryTurnover.Size = new System.Drawing.Size(107, 17);
            this._lblSummaryTurnover.TabIndex = 4;
            this._lblSummaryTurnover.Text = "Period Turnover";
            // 
            // _lblSummaryCount
            // 
            this._lblSummaryCount.AutoSize = true;
            this._lblSummaryCount.Location = new System.Drawing.Point(4, 131);
            this._lblSummaryCount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblSummaryCount.Name = "_lblSummaryCount";
            this._lblSummaryCount.Size = new System.Drawing.Size(66, 17);
            this._lblSummaryCount.TabIndex = 5;
            this._lblSummaryCount.Text = "Entries: 0";
            // 
            // _lblSummaryLastTxn
            // 
            this._lblSummaryLastTxn.AutoSize = true;
            this._lblSummaryLastTxn.Location = new System.Drawing.Point(4, 148);
            this._lblSummaryLastTxn.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblSummaryLastTxn.MaximumSize = new System.Drawing.Size(204, 0);
            this._lblSummaryLastTxn.Name = "_lblSummaryLastTxn";
            this._lblSummaryLastTxn.Size = new System.Drawing.Size(75, 17);
            this._lblSummaryLastTxn.TabIndex = 6;
            this._lblSummaryLastTxn.Text = "Last Txn: -";
            // 
            // _btnViewFullHistory
            // 
            this._btnViewFullHistory.Location = new System.Drawing.Point(4, 197);
            this._btnViewFullHistory.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnViewFullHistory.Name = "_btnViewFullHistory";
            this._btnViewFullHistory.Size = new System.Drawing.Size(198, 37);
            this._btnViewFullHistory.TabIndex = 8;
            this._btnViewFullHistory.Text = "View Full History";
            this._btnViewFullHistory.UseVisualStyleBackColor = true;
            // 
            // _btnPrint
            // 
            this._btnPrint.Location = new System.Drawing.Point(4, 242);
            this._btnPrint.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnPrint.Name = "_btnPrint";
            this._btnPrint.Size = new System.Drawing.Size(198, 37);
            this._btnPrint.TabIndex = 9;
            this._btnPrint.Text = "Print Ledger";
            this._btnPrint.UseVisualStyleBackColor = true;
            // 
            // _btnExport
            // 
            this._btnExport.Location = new System.Drawing.Point(4, 287);
            this._btnExport.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnExport.Name = "_btnExport";
            this._btnExport.Size = new System.Drawing.Size(198, 37);
            this._btnExport.TabIndex = 10;
            this._btnExport.Text = "Export Excel";
            this._btnExport.UseVisualStyleBackColor = true;
            // 
            // _panelTop
            // 
            this._panelTop.Controls.Add(this._tblFilters);
            this._panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this._panelTop.Location = new System.Drawing.Point(9, 10);
            this._panelTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._panelTop.Name = "_panelTop";
            this._panelTop.Padding = new System.Windows.Forms.Padding(0, 0, 0, 7);
            this._panelTop.Size = new System.Drawing.Size(1475, 96);
            this._panelTop.TabIndex = 2;
            // 
            // _tblFilters
            // 
            this._tblFilters.ColumnCount = 12;
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 35F));
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this._tblFilters.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this._tblFilters.Controls.Add(this._lblAccount, 0, 0);
            this._tblFilters.Controls.Add(this._cmbAccount, 1, 0);
            this._tblFilters.Controls.Add(this._lblFrom, 2, 0);
            this._tblFilters.Controls.Add(this._dtFrom, 3, 0);
            this._tblFilters.Controls.Add(this._lblTo, 4, 0);
            this._tblFilters.Controls.Add(this._dtTo, 5, 0);
            this._tblFilters.Controls.Add(this._btnLoad, 6, 0);
            this._tblFilters.Controls.Add(this._lblShow, 0, 1);
            this._tblFilters.Controls.Add(this._cmbShow, 1, 1);
            this._tblFilters.Controls.Add(this._lblGroupBy, 2, 1);
            this._tblFilters.Controls.Add(this._cmbGroupBy, 3, 1);
            this._tblFilters.Dock = System.Windows.Forms.DockStyle.Fill;
            this._tblFilters.Location = new System.Drawing.Point(0, 0);
            this._tblFilters.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._tblFilters.Name = "_tblFilters";
            this._tblFilters.RowCount = 2;
            this._tblFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tblFilters.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this._tblFilters.Size = new System.Drawing.Size(1475, 89);
            this._tblFilters.TabIndex = 0;
            // 
            // _lblAccount
            // 
            this._lblAccount.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._lblAccount.AutoSize = true;
            this._lblAccount.Location = new System.Drawing.Point(4, 13);
            this._lblAccount.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblAccount.Name = "_lblAccount";
            this._lblAccount.Size = new System.Drawing.Size(64, 17);
            this._lblAccount.TabIndex = 0;
            this._lblAccount.Text = "Account:";
            // 
            // _cmbAccount
            // 
            this._cmbAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this._cmbAccount.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this._cmbAccount.DropDownHeight = 280;
            this._cmbAccount.IntegralHeight = false;
            this._cmbAccount.Location = new System.Drawing.Point(76, 4);
            this._cmbAccount.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._cmbAccount.Name = "_cmbAccount";
            this._cmbAccount.Size = new System.Drawing.Size(224, 25);
            this._cmbAccount.TabIndex = 1;
            // 
            // _lblFrom
            // 
            this._lblFrom.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._lblFrom.AutoSize = true;
            this._lblFrom.Location = new System.Drawing.Point(308, 13);
            this._lblFrom.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblFrom.Name = "_lblFrom";
            this._lblFrom.Size = new System.Drawing.Size(45, 17);
            this._lblFrom.TabIndex = 2;
            this._lblFrom.Text = "From:";
            // 
            // _dtFrom
            // 
            this._dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtFrom.Location = new System.Drawing.Point(387, 4);
            this._dtFrom.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._dtFrom.Name = "_dtFrom";
            this._dtFrom.Size = new System.Drawing.Size(233, 24);
            this._dtFrom.TabIndex = 3;
            this._dtFrom.Value = new System.DateTime(2026, 6, 10, 0, 0, 0, 0);
            // 
            // _lblTo
            // 
            this._lblTo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._lblTo.AutoSize = true;
            this._lblTo.Location = new System.Drawing.Point(628, 13);
            this._lblTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblTo.Name = "_lblTo";
            this._lblTo.Size = new System.Drawing.Size(29, 17);
            this._lblTo.TabIndex = 4;
            this._lblTo.Text = "To:";
            // 
            // _dtTo
            // 
            this._dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtTo.Location = new System.Drawing.Point(665, 4);
            this._dtTo.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._dtTo.Name = "_dtTo";
            this._dtTo.Size = new System.Drawing.Size(233, 24);
            this._dtTo.TabIndex = 5;
            this._dtTo.Value = new System.DateTime(2026, 7, 10, 0, 0, 0, 0);
            // 
            // _btnLoad
            // 
            this._btnLoad.AutoSize = true;
            this._btnLoad.Location = new System.Drawing.Point(906, 4);
            this._btnLoad.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._btnLoad.Name = "_btnLoad";
            this._btnLoad.Padding = new System.Windows.Forms.Padding(12, 0, 12, 0);
            this._btnLoad.Size = new System.Drawing.Size(133, 33);
            this._btnLoad.TabIndex = 6;
            this._btnLoad.Text = "Load Ledger";
            this._btnLoad.UseVisualStyleBackColor = true;
            // 
            // _lblShow
            // 
            this._lblShow.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._lblShow.AutoSize = true;
            this._lblShow.Location = new System.Drawing.Point(4, 58);
            this._lblShow.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblShow.Name = "_lblShow";
            this._lblShow.Size = new System.Drawing.Size(47, 17);
            this._lblShow.TabIndex = 7;
            this._lblShow.Text = "Show:";
            // 
            // _cmbShow
            // 
            this._cmbShow.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbShow.FormattingEnabled = true;
            this._cmbShow.Items.AddRange(new object[] {
            "All Entries",
            "Debits Only",
            "Credits Only",
            "Unposted/Draft Only"});
            this._cmbShow.Location = new System.Drawing.Point(76, 48);
            this._cmbShow.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._cmbShow.Name = "_cmbShow";
            this._cmbShow.Size = new System.Drawing.Size(135, 24);
            this._cmbShow.TabIndex = 8;
            // 
            // _lblGroupBy
            // 
            this._lblGroupBy.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this._lblGroupBy.AutoSize = true;
            this._lblGroupBy.Location = new System.Drawing.Point(308, 58);
            this._lblGroupBy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this._lblGroupBy.Name = "_lblGroupBy";
            this._lblGroupBy.Size = new System.Drawing.Size(71, 17);
            this._lblGroupBy.TabIndex = 9;
            this._lblGroupBy.Text = "Group By:";
            // 
            // _cmbGroupBy
            // 
            this._cmbGroupBy.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbGroupBy.FormattingEnabled = true;
            this._cmbGroupBy.Items.AddRange(new object[] {
            "None",
            "By Voucher",
            "By Month"});
            this._cmbGroupBy.Location = new System.Drawing.Point(387, 48);
            this._cmbGroupBy.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this._cmbGroupBy.Name = "_cmbGroupBy";
            this._cmbGroupBy.Size = new System.Drawing.Size(140, 24);
            this._cmbGroupBy.TabIndex = 10;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Date";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Voucher No";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Voucher Type";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Ref Module";
            this.dataGridViewTextBoxColumn4.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Narration";
            this.dataGridViewTextBoxColumn5.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Debit";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Credit";
            this.dataGridViewTextBoxColumn7.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Balance";
            this.dataGridViewTextBoxColumn8.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.HeaderText = "Date";
            this.dataGridViewTextBoxColumn9.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn10
            // 
            this.dataGridViewTextBoxColumn10.HeaderText = "Voucher No";
            this.dataGridViewTextBoxColumn10.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            this.dataGridViewTextBoxColumn10.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Voucher Type";
            this.dataGridViewTextBoxColumn11.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn12
            // 
            this.dataGridViewTextBoxColumn12.HeaderText = "Ref Module";
            this.dataGridViewTextBoxColumn12.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn12.Name = "dataGridViewTextBoxColumn12";
            this.dataGridViewTextBoxColumn12.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn13
            // 
            this.dataGridViewTextBoxColumn13.HeaderText = "Narration";
            this.dataGridViewTextBoxColumn13.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn13.Name = "dataGridViewTextBoxColumn13";
            this.dataGridViewTextBoxColumn13.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn14
            // 
            this.dataGridViewTextBoxColumn14.HeaderText = "Debit";
            this.dataGridViewTextBoxColumn14.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn14.Name = "dataGridViewTextBoxColumn14";
            this.dataGridViewTextBoxColumn14.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn15
            // 
            this.dataGridViewTextBoxColumn15.HeaderText = "Credit";
            this.dataGridViewTextBoxColumn15.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn15.Name = "dataGridViewTextBoxColumn15";
            this.dataGridViewTextBoxColumn15.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn16
            // 
            this.dataGridViewTextBoxColumn16.HeaderText = "Balance";
            this.dataGridViewTextBoxColumn16.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn16.Name = "dataGridViewTextBoxColumn16";
            this.dataGridViewTextBoxColumn16.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn17
            // 
            this.dataGridViewTextBoxColumn17.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn17.Name = "dataGridViewTextBoxColumn17";
            this.dataGridViewTextBoxColumn17.ReadOnly = true;
            // 
            // dataGridViewLinkColumn1
            // 
            this.dataGridViewLinkColumn1.MinimumWidth = 6;
            this.dataGridViewLinkColumn1.Name = "dataGridViewLinkColumn1";
            this.dataGridViewLinkColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn18
            // 
            this.dataGridViewTextBoxColumn18.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn18.Name = "dataGridViewTextBoxColumn18";
            this.dataGridViewTextBoxColumn18.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn20
            // 
            this.dataGridViewTextBoxColumn20.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn20.Name = "dataGridViewTextBoxColumn20";
            this.dataGridViewTextBoxColumn20.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn21
            // 
            this.dataGridViewTextBoxColumn21.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn21.Name = "dataGridViewTextBoxColumn21";
            this.dataGridViewTextBoxColumn21.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn22
            // 
            this.dataGridViewTextBoxColumn22.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn22.Name = "dataGridViewTextBoxColumn22";
            this.dataGridViewTextBoxColumn22.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn23
            // 
            this.dataGridViewTextBoxColumn23.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn23.Name = "dataGridViewTextBoxColumn23";
            this.dataGridViewTextBoxColumn23.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn24
            // 
            this.dataGridViewTextBoxColumn24.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn24.Name = "dataGridViewTextBoxColumn24";
            this.dataGridViewTextBoxColumn24.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn25
            // 
            this.dataGridViewTextBoxColumn25.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn25.Name = "dataGridViewTextBoxColumn25";
            this.dataGridViewTextBoxColumn25.ReadOnly = true;
            // 
            // frm_GeneralLedgerReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1493, 911);
            this.Controls.Add(this._panelMain);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frm_GeneralLedgerReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "General Ledger";
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

        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn17;
        private DataGridViewLinkColumn dataGridViewLinkColumn1;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn18;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn20;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn21;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn22;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn23;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn24;
        private DataGridViewTextBoxColumn dataGridViewTextBoxColumn25;
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
    }
}
