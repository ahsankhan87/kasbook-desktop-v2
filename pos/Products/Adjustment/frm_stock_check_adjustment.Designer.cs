namespace pos
{
    partial class frm_stock_check_adjustment
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.headerPanel = new System.Windows.Forms.Panel();
            this.headerLayout = new System.Windows.Forms.TableLayoutPanel();
            this.pnlHeaderLeft = new System.Windows.Forms.Panel();
            this.picWarehouse = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlHeaderCenter = new System.Windows.Forms.Panel();
            this.sessionLayout = new System.Windows.Forms.TableLayoutPanel();
            this.lblAdjustmentNo = new System.Windows.Forms.Label();
            this.txtAdjustmentNo = new System.Windows.Forms.TextBox();
            this.lblAdjustmentDate = new System.Windows.Forms.Label();
            this.dtpAdjustmentDate = new System.Windows.Forms.DateTimePicker();
            this.lblAdjustmentType = new System.Windows.Forms.Label();
            this.cmbAdjustmentType = new System.Windows.Forms.ComboBox();
            this.pnlHeaderRight = new System.Windows.Forms.Panel();
            this.lblStatusTitle = new System.Windows.Forms.Label();
            this.lblSessionStatus = new System.Windows.Forms.Label();
            this.progressPanel = new System.Windows.Forms.Panel();
            this.lblVerificationProgress = new System.Windows.Forms.Label();
            this.progressVerification = new System.Windows.Forms.ProgressBar();
            this.toolbarPanel = new System.Windows.Forms.Panel();
            this.toolbarStrip = new System.Windows.Forms.ToolStrip();
            this.btnNewSession = new System.Windows.Forms.ToolStripButton();
            this.btnOpenSession = new System.Windows.Forms.ToolStripButton();
            this.btnSaveDraft = new System.Windows.Forms.ToolStripButton();
            this.btnPostAdjustment = new System.Windows.Forms.ToolStripButton();
            this.sep1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnImportExcel = new System.Windows.Forms.ToolStripButton();
            this.btnExportExcel = new System.Windows.Forms.ToolStripButton();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.sep2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnScanBarcode = new System.Windows.Forms.ToolStripButton();
            this.btnSearchProduct = new System.Windows.Forms.ToolStripButton();
            this.sep3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUndoLast = new System.Windows.Forms.ToolStripButton();
            this.btnSessionLog = new System.Windows.Forms.ToolStripButton();
            this.mainSplit = new System.Windows.Forms.SplitContainer();
            this.leftPanelHost = new System.Windows.Forms.Panel();
            this.lblLeftPlaceholder = new System.Windows.Forms.Label();
            this.rightPanelHost = new System.Windows.Forms.Panel();
            this.lblRightPlaceholder = new System.Windows.Forms.Label();
            this.bottomStatus = new System.Windows.Forms.StatusStrip();
            this.tslTotals = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslSpacerLeft = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslValueChange = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslSpacerRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.tslMeta = new System.Windows.Forms.ToolStripStatusLabel();
            this.headerPanel.SuspendLayout();
            this.headerLayout.SuspendLayout();
            this.pnlHeaderLeft.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWarehouse)).BeginInit();
            this.pnlHeaderCenter.SuspendLayout();
            this.sessionLayout.SuspendLayout();
            this.pnlHeaderRight.SuspendLayout();
            this.progressPanel.SuspendLayout();
            this.toolbarPanel.SuspendLayout();
            this.toolbarStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).BeginInit();
            this.mainSplit.Panel1.SuspendLayout();
            this.mainSplit.Panel2.SuspendLayout();
            this.mainSplit.SuspendLayout();
            this.leftPanelHost.SuspendLayout();
            this.rightPanelHost.SuspendLayout();
            this.bottomStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel
            // 
            this.headerPanel.BackColor = System.Drawing.Color.White;
            this.headerPanel.Controls.Add(this.headerLayout);
            this.headerPanel.Controls.Add(this.progressPanel);
            this.headerPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel.Location = new System.Drawing.Point(0, 0);
            this.headerPanel.Name = "headerPanel";
            this.headerPanel.Size = new System.Drawing.Size(1280, 88);
            this.headerPanel.TabIndex = 0;
            // 
            // headerLayout
            // 
            this.headerLayout.ColumnCount = 3;
            this.headerLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 27F));
            this.headerLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 53F));
            this.headerLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.headerLayout.Controls.Add(this.pnlHeaderLeft, 0, 0);
            this.headerLayout.Controls.Add(this.pnlHeaderCenter, 1, 0);
            this.headerLayout.Controls.Add(this.pnlHeaderRight, 2, 0);
            this.headerLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.headerLayout.Location = new System.Drawing.Point(0, 0);
            this.headerLayout.Name = "headerLayout";
            this.headerLayout.RowCount = 1;
            this.headerLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.headerLayout.Size = new System.Drawing.Size(1280, 64);
            this.headerLayout.TabIndex = 0;
            // 
            // pnlHeaderLeft
            // 
            this.pnlHeaderLeft.Controls.Add(this.picWarehouse);
            this.pnlHeaderLeft.Controls.Add(this.lblTitle);
            this.pnlHeaderLeft.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeaderLeft.Location = new System.Drawing.Point(3, 3);
            this.pnlHeaderLeft.Name = "pnlHeaderLeft";
            this.pnlHeaderLeft.Size = new System.Drawing.Size(339, 58);
            this.pnlHeaderLeft.TabIndex = 0;
            // 
            // picWarehouse
            // 
            this.picWarehouse.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.picWarehouse.Location = new System.Drawing.Point(12, 14);
            this.picWarehouse.Name = "picWarehouse";
            this.picWarehouse.Size = new System.Drawing.Size(24, 24);
            this.picWarehouse.TabIndex = 0;
            this.picWarehouse.TabStop = false;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.lblTitle.Location = new System.Drawing.Point(46, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(303, 32);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Stock Check && Adjustment";
            // 
            // pnlHeaderCenter
            // 
            this.pnlHeaderCenter.Controls.Add(this.sessionLayout);
            this.pnlHeaderCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeaderCenter.Location = new System.Drawing.Point(348, 3);
            this.pnlHeaderCenter.Name = "pnlHeaderCenter";
            this.pnlHeaderCenter.Size = new System.Drawing.Size(672, 58);
            this.pnlHeaderCenter.TabIndex = 1;
            // 
            // sessionLayout
            // 
            this.sessionLayout.ColumnCount = 6;
            this.sessionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 96F));
            this.sessionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.sessionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.sessionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.sessionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 106F));
            this.sessionLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 46.66667F));
            this.sessionLayout.Controls.Add(this.lblAdjustmentNo, 0, 0);
            this.sessionLayout.Controls.Add(this.txtAdjustmentNo, 1, 0);
            this.sessionLayout.Controls.Add(this.lblAdjustmentDate, 2, 0);
            this.sessionLayout.Controls.Add(this.dtpAdjustmentDate, 3, 0);
            this.sessionLayout.Controls.Add(this.lblAdjustmentType, 4, 0);
            this.sessionLayout.Controls.Add(this.cmbAdjustmentType, 5, 0);
            this.sessionLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sessionLayout.Location = new System.Drawing.Point(0, 0);
            this.sessionLayout.Name = "sessionLayout";
            this.sessionLayout.RowCount = 1;
            this.sessionLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.sessionLayout.Size = new System.Drawing.Size(672, 58);
            this.sessionLayout.TabIndex = 0;
            // 
            // lblAdjustmentNo
            // 
            this.lblAdjustmentNo.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAdjustmentNo.AutoSize = true;
            this.lblAdjustmentNo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAdjustmentNo.Location = new System.Drawing.Point(3, 9);
            this.lblAdjustmentNo.Name = "lblAdjustmentNo";
            this.lblAdjustmentNo.Size = new System.Drawing.Size(89, 40);
            this.lblAdjustmentNo.TabIndex = 0;
            this.lblAdjustmentNo.Text = "Adjustment No";
            // 
            // txtAdjustmentNo
            // 
            this.txtAdjustmentNo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.txtAdjustmentNo.BackColor = System.Drawing.Color.White;
            this.txtAdjustmentNo.Location = new System.Drawing.Point(99, 17);
            this.txtAdjustmentNo.Name = "txtAdjustmentNo";
            this.txtAdjustmentNo.ReadOnly = true;
            this.txtAdjustmentNo.Size = new System.Drawing.Size(117, 24);
            this.txtAdjustmentNo.TabIndex = 1;
            // 
            // lblAdjustmentDate
            // 
            this.lblAdjustmentDate.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAdjustmentDate.AutoSize = true;
            this.lblAdjustmentDate.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAdjustmentDate.Location = new System.Drawing.Point(222, 9);
            this.lblAdjustmentDate.Name = "lblAdjustmentDate";
            this.lblAdjustmentDate.Size = new System.Drawing.Size(89, 40);
            this.lblAdjustmentDate.TabIndex = 2;
            this.lblAdjustmentDate.Text = "Adjustment Date";
            // 
            // dtpAdjustmentDate
            // 
            this.dtpAdjustmentDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpAdjustmentDate.Location = new System.Drawing.Point(322, 17);
            this.dtpAdjustmentDate.Name = "dtpAdjustmentDate";
            this.dtpAdjustmentDate.Size = new System.Drawing.Size(68, 24);
            this.dtpAdjustmentDate.TabIndex = 3;
            // 
            // lblAdjustmentType
            // 
            this.lblAdjustmentType.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.lblAdjustmentType.AutoSize = true;
            this.lblAdjustmentType.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblAdjustmentType.Location = new System.Drawing.Point(396, 9);
            this.lblAdjustmentType.Name = "lblAdjustmentType";
            this.lblAdjustmentType.Size = new System.Drawing.Size(89, 40);
            this.lblAdjustmentType.TabIndex = 4;
            this.lblAdjustmentType.Text = "Adjustment Type";
            // 
            // cmbAdjustmentType
            // 
            this.cmbAdjustmentType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbAdjustmentType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAdjustmentType.FormattingEnabled = true;
            this.cmbAdjustmentType.Location = new System.Drawing.Point(502, 17);
            this.cmbAdjustmentType.Name = "cmbAdjustmentType";
            this.cmbAdjustmentType.Size = new System.Drawing.Size(167, 24);
            this.cmbAdjustmentType.TabIndex = 5;
            // 
            // pnlHeaderRight
            // 
            this.pnlHeaderRight.Controls.Add(this.lblStatusTitle);
            this.pnlHeaderRight.Controls.Add(this.lblSessionStatus);
            this.pnlHeaderRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlHeaderRight.Location = new System.Drawing.Point(1026, 3);
            this.pnlHeaderRight.Name = "pnlHeaderRight";
            this.pnlHeaderRight.Size = new System.Drawing.Size(251, 58);
            this.pnlHeaderRight.TabIndex = 2;
            // 
            // lblStatusTitle
            // 
            this.lblStatusTitle.AutoSize = true;
            this.lblStatusTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblStatusTitle.Location = new System.Drawing.Point(16, 9);
            this.lblStatusTitle.Name = "lblStatusTitle";
            this.lblStatusTitle.Size = new System.Drawing.Size(102, 20);
            this.lblStatusTitle.TabIndex = 0;
            this.lblStatusTitle.Text = "Session Status";
            // 
            // lblSessionStatus
            // 
            this.lblSessionStatus.AutoSize = true;
            this.lblSessionStatus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(193)))), ((int)(((byte)(7)))));
            this.lblSessionStatus.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.lblSessionStatus.ForeColor = System.Drawing.Color.White;
            this.lblSessionStatus.Location = new System.Drawing.Point(16, 29);
            this.lblSessionStatus.Name = "lblSessionStatus";
            this.lblSessionStatus.Padding = new System.Windows.Forms.Padding(10, 4, 10, 4);
            this.lblSessionStatus.Size = new System.Drawing.Size(64, 28);
            this.lblSessionStatus.TabIndex = 1;
            this.lblSessionStatus.Text = "Draft";
            // 
            // progressPanel
            // 
            this.progressPanel.BackColor = System.Drawing.Color.White;
            this.progressPanel.Controls.Add(this.lblVerificationProgress);
            this.progressPanel.Controls.Add(this.progressVerification);
            this.progressPanel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.progressPanel.Location = new System.Drawing.Point(0, 64);
            this.progressPanel.Name = "progressPanel";
            this.progressPanel.Padding = new System.Windows.Forms.Padding(12, 4, 12, 6);
            this.progressPanel.Size = new System.Drawing.Size(1280, 24);
            this.progressPanel.TabIndex = 1;
            // 
            // lblVerificationProgress
            // 
            this.lblVerificationProgress.AutoSize = true;
            this.lblVerificationProgress.Dock = System.Windows.Forms.DockStyle.Right;
            this.lblVerificationProgress.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblVerificationProgress.Location = new System.Drawing.Point(1105, 4);
            this.lblVerificationProgress.Name = "lblVerificationProgress";
            this.lblVerificationProgress.Size = new System.Drawing.Size(163, 20);
            this.lblVerificationProgress.TabIndex = 1;
            this.lblVerificationProgress.Text = "0 of 0 products verified";
            // 
            // progressVerification
            // 
            this.progressVerification.Dock = System.Windows.Forms.DockStyle.Fill;
            this.progressVerification.Location = new System.Drawing.Point(12, 4);
            this.progressVerification.Name = "progressVerification";
            this.progressVerification.Size = new System.Drawing.Size(1256, 14);
            this.progressVerification.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.progressVerification.TabIndex = 0;
            // 
            // toolbarPanel
            // 
            this.toolbarPanel.BackColor = System.Drawing.Color.White;
            this.toolbarPanel.Controls.Add(this.toolbarStrip);
            this.toolbarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.toolbarPanel.Location = new System.Drawing.Point(0, 88);
            this.toolbarPanel.Name = "toolbarPanel";
            this.toolbarPanel.Size = new System.Drawing.Size(1280, 42);
            this.toolbarPanel.TabIndex = 1;
            // 
            // toolbarStrip
            // 
            this.toolbarStrip.BackColor = System.Drawing.Color.White;
            this.toolbarStrip.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolbarStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolbarStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolbarStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNewSession,
            this.btnOpenSession,
            this.btnSaveDraft,
            this.btnPostAdjustment,
            this.sep1,
            this.btnImportExcel,
            this.btnExportExcel,
            this.btnPrint,
            this.sep2,
            this.btnScanBarcode,
            this.btnSearchProduct,
            this.sep3,
            this.btnUndoLast,
            this.btnSessionLog});
            this.toolbarStrip.Location = new System.Drawing.Point(0, 0);
            this.toolbarStrip.Name = "toolbarStrip";
            this.toolbarStrip.Padding = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.toolbarStrip.Size = new System.Drawing.Size(1280, 42);
            this.toolbarStrip.TabIndex = 0;
            // 
            // btnNewSession
            // 
            this.btnNewSession.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnNewSession.Name = "btnNewSession";
            this.btnNewSession.Size = new System.Drawing.Size(96, 27);
            this.btnNewSession.Text = "New Session";
            this.btnNewSession.ToolTipText = "Start a new stock check session";
            this.btnNewSession.Click += new System.EventHandler(this.btnNewSession_Click);
            // 
            // btnOpenSession
            // 
            this.btnOpenSession.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnOpenSession.Name = "btnOpenSession";
            this.btnOpenSession.Size = new System.Drawing.Size(102, 27);
            this.btnOpenSession.Text = "Open Session";
            this.btnOpenSession.ToolTipText = "Open an existing draft/in-progress session";
            // 
            // btnSaveDraft
            // 
            this.btnSaveDraft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSaveDraft.Name = "btnSaveDraft";
            this.btnSaveDraft.Size = new System.Drawing.Size(82, 27);
            this.btnSaveDraft.Text = "Save Draft";
            this.btnSaveDraft.ToolTipText = "Save current session as draft";
            // 
            // btnPostAdjustment
            // 
            this.btnPostAdjustment.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPostAdjustment.Name = "btnPostAdjustment";
            this.btnPostAdjustment.Size = new System.Drawing.Size(120, 27);
            this.btnPostAdjustment.Text = "Post Adjustment";
            this.btnPostAdjustment.ToolTipText = "Post and finalize current adjustment";
            // 
            // sep1
            // 
            this.sep1.Name = "sep1";
            this.sep1.Size = new System.Drawing.Size(6, 30);
            // 
            // btnImportExcel
            // 
            this.btnImportExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnImportExcel.Name = "btnImportExcel";
            this.btnImportExcel.Size = new System.Drawing.Size(132, 27);
            this.btnImportExcel.Text = "Import from Excel";
            this.btnImportExcel.ToolTipText = "Import stock check lines from Excel";
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(112, 27);
            this.btnExportExcel.Text = "Export to Excel";
            this.btnExportExcel.ToolTipText = "Export current session lines to Excel";
            // 
            // btnPrint
            // 
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(43, 27);
            this.btnPrint.Text = "Print";
            this.btnPrint.ToolTipText = "Print stock check sheet/session";
            // 
            // sep2
            // 
            this.sep2.Name = "sep2";
            this.sep2.Size = new System.Drawing.Size(6, 30);
            // 
            // btnScanBarcode
            // 
            this.btnScanBarcode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnScanBarcode.Name = "btnScanBarcode";
            this.btnScanBarcode.Size = new System.Drawing.Size(103, 27);
            this.btnScanBarcode.Text = "Scan Barcode";
            this.btnScanBarcode.ToolTipText = "Scan and locate product quickly";
            // 
            // btnSearchProduct
            // 
            this.btnSearchProduct.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSearchProduct.Name = "btnSearchProduct";
            this.btnSearchProduct.Size = new System.Drawing.Size(112, 27);
            this.btnSearchProduct.Text = "Search Product";
            this.btnSearchProduct.ToolTipText = "Search products by code/name";
            // 
            // sep3
            // 
            this.sep3.Name = "sep3";
            this.sep3.Size = new System.Drawing.Size(6, 30);
            // 
            // btnUndoLast
            // 
            this.btnUndoLast.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnUndoLast.Name = "btnUndoLast";
            this.btnUndoLast.Size = new System.Drawing.Size(79, 27);
            this.btnUndoLast.Text = "Undo Last";
            this.btnUndoLast.ToolTipText = "Undo last stock count action";
            // 
            // btnSessionLog
            // 
            this.btnSessionLog.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSessionLog.Name = "btnSessionLog";
            this.btnSessionLog.Size = new System.Drawing.Size(91, 27);
            this.btnSessionLog.Text = "Session Log";
            this.btnSessionLog.ToolTipText = "View session change log";
            // 
            // mainSplit
            // 
            this.mainSplit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainSplit.Location = new System.Drawing.Point(0, 130);
            this.mainSplit.Name = "mainSplit";
            // 
            // mainSplit.Panel1
            // 
            this.mainSplit.Panel1.Controls.Add(this.leftPanelHost);
            this.mainSplit.Panel1MinSize = 260;
            // 
            // mainSplit.Panel2
            // 
            this.mainSplit.Panel2.Controls.Add(this.rightPanelHost);
            this.mainSplit.Panel2MinSize = 520;
            this.mainSplit.Size = new System.Drawing.Size(1280, 564);
            this.mainSplit.SplitterDistance = 384;
            this.mainSplit.TabIndex = 2;
            // 
            // leftPanelHost
            // 
            this.leftPanelHost.BackColor = System.Drawing.Color.White;
            this.leftPanelHost.Controls.Add(this.lblLeftPlaceholder);
            this.leftPanelHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.leftPanelHost.Location = new System.Drawing.Point(0, 0);
            this.leftPanelHost.Name = "leftPanelHost";
            this.leftPanelHost.Padding = new System.Windows.Forms.Padding(12);
            this.leftPanelHost.Size = new System.Drawing.Size(384, 564);
            this.leftPanelHost.TabIndex = 0;
            // 
            // lblLeftPlaceholder
            // 
            this.lblLeftPlaceholder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblLeftPlaceholder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblLeftPlaceholder.ForeColor = System.Drawing.Color.DimGray;
            this.lblLeftPlaceholder.Location = new System.Drawing.Point(12, 12);
            this.lblLeftPlaceholder.Name = "lblLeftPlaceholder";
            this.lblLeftPlaceholder.Size = new System.Drawing.Size(360, 24);
            this.lblLeftPlaceholder.TabIndex = 0;
            this.lblLeftPlaceholder.Text = "Left panel (filters / scanner / quick actions)";
            // 
            // rightPanelHost
            // 
            this.rightPanelHost.BackColor = System.Drawing.Color.White;
            this.rightPanelHost.Controls.Add(this.lblRightPlaceholder);
            this.rightPanelHost.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rightPanelHost.Location = new System.Drawing.Point(0, 0);
            this.rightPanelHost.Name = "rightPanelHost";
            this.rightPanelHost.Padding = new System.Windows.Forms.Padding(12);
            this.rightPanelHost.Size = new System.Drawing.Size(892, 564);
            this.rightPanelHost.TabIndex = 0;
            // 
            // lblRightPlaceholder
            // 
            this.lblRightPlaceholder.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblRightPlaceholder.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Italic);
            this.lblRightPlaceholder.ForeColor = System.Drawing.Color.DimGray;
            this.lblRightPlaceholder.Location = new System.Drawing.Point(12, 12);
            this.lblRightPlaceholder.Name = "lblRightPlaceholder";
            this.lblRightPlaceholder.Size = new System.Drawing.Size(868, 24);
            this.lblRightPlaceholder.TabIndex = 0;
            this.lblRightPlaceholder.Text = "Right panel (high-volume product grid and adjustment lines)";
            // 
            // bottomStatus
            // 
            this.bottomStatus.BackColor = System.Drawing.Color.White;
            this.bottomStatus.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.bottomStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslTotals,
            this.tslSpacerLeft,
            this.tslValueChange,
            this.tslSpacerRight,
            this.tslMeta});
            this.bottomStatus.Location = new System.Drawing.Point(0, 694);
            this.bottomStatus.Name = "bottomStatus";
            this.bottomStatus.Size = new System.Drawing.Size(1280, 26);
            this.bottomStatus.SizingGrip = false;
            this.bottomStatus.TabIndex = 3;
            // 
            // tslTotals
            // 
            this.tslTotals.Name = "tslTotals";
            this.tslTotals.Size = new System.Drawing.Size(239, 20);
            this.tslTotals.Text = "Total: 0  |  Adjusted: 0  |  Pending: 0";
            // 
            // tslSpacerLeft
            // 
            this.tslSpacerLeft.Name = "tslSpacerLeft";
            this.tslSpacerLeft.Size = new System.Drawing.Size(33, 20);
            this.tslSpacerLeft.Text = "      ";
            // 
            // tslValueChange
            // 
            this.tslValueChange.Name = "tslValueChange";
            this.tslValueChange.Size = new System.Drawing.Size(143, 20);
            this.tslValueChange.Text = "Value Change: +0.00";
            // 
            // tslSpacerRight
            // 
            this.tslSpacerRight.Name = "tslSpacerRight";
            this.tslSpacerRight.Size = new System.Drawing.Size(33, 20);
            this.tslSpacerRight.Text = "      ";
            // 
            // tslMeta
            // 
            this.tslMeta.Name = "tslMeta";
            this.tslMeta.Size = new System.Drawing.Size(342, 20);
            this.tslMeta.Text = "Last saved: --:--  |  User: -  |  Warehouse/Branch: All";
            // 
            // frm_stock_check_adjustment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(247)))), ((int)(((byte)(250)))));
            this.ClientSize = new System.Drawing.Size(1280, 720);
            this.Controls.Add(this.mainSplit);
            this.Controls.Add(this.bottomStatus);
            this.Controls.Add(this.toolbarPanel);
            this.Controls.Add(this.headerPanel);
            this.MinimumSize = new System.Drawing.Size(1280, 720);
            this.Name = "frm_stock_check_adjustment";
            this.Text = "Stock Check & Adjustment";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_stock_check_adjustment_Load);
            this.headerPanel.ResumeLayout(false);
            this.headerLayout.ResumeLayout(false);
            this.pnlHeaderLeft.ResumeLayout(false);
            this.pnlHeaderLeft.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picWarehouse)).EndInit();
            this.pnlHeaderCenter.ResumeLayout(false);
            this.sessionLayout.ResumeLayout(false);
            this.sessionLayout.PerformLayout();
            this.pnlHeaderRight.ResumeLayout(false);
            this.pnlHeaderRight.PerformLayout();
            this.progressPanel.ResumeLayout(false);
            this.progressPanel.PerformLayout();
            this.toolbarPanel.ResumeLayout(false);
            this.toolbarPanel.PerformLayout();
            this.toolbarStrip.ResumeLayout(false);
            this.toolbarStrip.PerformLayout();
            this.mainSplit.Panel1.ResumeLayout(false);
            this.mainSplit.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.mainSplit)).EndInit();
            this.mainSplit.ResumeLayout(false);
            this.leftPanelHost.ResumeLayout(false);
            this.rightPanelHost.ResumeLayout(false);
            this.bottomStatus.ResumeLayout(false);
            this.bottomStatus.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel headerPanel;
        private System.Windows.Forms.TableLayoutPanel headerLayout;
        private System.Windows.Forms.Panel pnlHeaderLeft;
        private System.Windows.Forms.PictureBox picWarehouse;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlHeaderCenter;
        private System.Windows.Forms.TableLayoutPanel sessionLayout;
        private System.Windows.Forms.Label lblAdjustmentNo;
        private System.Windows.Forms.TextBox txtAdjustmentNo;
        private System.Windows.Forms.Label lblAdjustmentDate;
        private System.Windows.Forms.DateTimePicker dtpAdjustmentDate;
        private System.Windows.Forms.Label lblAdjustmentType;
        private System.Windows.Forms.ComboBox cmbAdjustmentType;
        private System.Windows.Forms.Panel pnlHeaderRight;
        private System.Windows.Forms.Label lblStatusTitle;
        private System.Windows.Forms.Label lblSessionStatus;
        private System.Windows.Forms.Panel progressPanel;
        private System.Windows.Forms.Label lblVerificationProgress;
        private System.Windows.Forms.ProgressBar progressVerification;
        private System.Windows.Forms.Panel toolbarPanel;
        private System.Windows.Forms.ToolStrip toolbarStrip;
        private System.Windows.Forms.ToolStripButton btnNewSession;
        private System.Windows.Forms.ToolStripButton btnOpenSession;
        private System.Windows.Forms.ToolStripButton btnSaveDraft;
        private System.Windows.Forms.ToolStripButton btnPostAdjustment;
        private System.Windows.Forms.ToolStripSeparator sep1;
        private System.Windows.Forms.ToolStripButton btnImportExcel;
        private System.Windows.Forms.ToolStripButton btnExportExcel;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripSeparator sep2;
        private System.Windows.Forms.ToolStripButton btnScanBarcode;
        private System.Windows.Forms.ToolStripButton btnSearchProduct;
        private System.Windows.Forms.ToolStripSeparator sep3;
        private System.Windows.Forms.ToolStripButton btnUndoLast;
        private System.Windows.Forms.ToolStripButton btnSessionLog;
        private System.Windows.Forms.SplitContainer mainSplit;
        private System.Windows.Forms.Panel leftPanelHost;
        private System.Windows.Forms.Label lblLeftPlaceholder;
        private System.Windows.Forms.Panel rightPanelHost;
        private System.Windows.Forms.Label lblRightPlaceholder;
        private System.Windows.Forms.StatusStrip bottomStatus;
        private System.Windows.Forms.ToolStripStatusLabel tslTotals;
        private System.Windows.Forms.ToolStripStatusLabel tslSpacerLeft;
        private System.Windows.Forms.ToolStripStatusLabel tslValueChange;
        private System.Windows.Forms.ToolStripStatusLabel tslSpacerRight;
        private System.Windows.Forms.ToolStripStatusLabel tslMeta;
    }
}
