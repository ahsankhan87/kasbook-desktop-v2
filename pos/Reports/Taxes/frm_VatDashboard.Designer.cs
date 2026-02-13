namespace pos.Reports.Taxes
{
    partial class frm_VatDashboard
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

        private System.Windows.Forms.Panel topPanel;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.DateTimePicker dtFrom;
        private System.Windows.Forms.DateTimePicker dtTo;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnExecute;
        private System.Windows.Forms.GroupBox grpMode;
        private System.Windows.Forms.RadioButton rbCompany;
        private System.Windows.Forms.RadioButton rbBranches;
        private System.Windows.Forms.RadioButton rbBranch;
        private System.Windows.Forms.ComboBox cmbBranch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnPreview;

        private System.Windows.Forms.Panel kpiPanel;
        private System.Windows.Forms.Label lblVatCollectedTitle;
        private System.Windows.Forms.Label lblVatPaidTitle;
        private System.Windows.Forms.Label lblVatNetTitle;
        private System.Windows.Forms.Label lblVatCollected;
        private System.Windows.Forms.Label lblVatPaid;
        private System.Windows.Forms.Label lblVatNet;

        private System.Windows.Forms.SplitContainer splitMain;
        private System.Windows.Forms.GroupBox grpCompany;
        private System.Windows.Forms.GroupBox grpBranches;
        private System.Windows.Forms.GroupBox grpBranch;
        private System.Windows.Forms.DataGridView gridCompany;
        private System.Windows.Forms.DataGridView gridBranches;
        private System.Windows.Forms.DataGridView gridBranch;

        private void InitializeComponent()
        {
            this.topPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.btnExecute = new System.Windows.Forms.Button();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.rbCompany = new System.Windows.Forms.RadioButton();
            this.rbBranches = new System.Windows.Forms.RadioButton();
            this.rbBranch = new System.Windows.Forms.RadioButton();
            this.cmbBranch = new System.Windows.Forms.ComboBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.kpiPanel = new System.Windows.Forms.Panel();
            this.lblVatCollectedTitle = new System.Windows.Forms.Label();
            this.lblVatCollected = new System.Windows.Forms.Label();
            this.lblVatPaidTitle = new System.Windows.Forms.Label();
            this.lblVatPaid = new System.Windows.Forms.Label();
            this.lblVatNetTitle = new System.Windows.Forms.Label();
            this.lblVatNet = new System.Windows.Forms.Label();
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.grpCompany = new System.Windows.Forms.GroupBox();
            this.gridCompany = new System.Windows.Forms.DataGridView();
            this.grpBranch = new System.Windows.Forms.GroupBox();
            this.gridBranch = new System.Windows.Forms.DataGridView();
            this.grpBranches = new System.Windows.Forms.GroupBox();
            this.gridBranches = new System.Windows.Forms.DataGridView();
            this.topPanel.SuspendLayout();
            this.grpMode.SuspendLayout();
            this.kpiPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).BeginInit();
            this.splitMain.Panel1.SuspendLayout();
            this.splitMain.Panel2.SuspendLayout();
            this.splitMain.SuspendLayout();
            this.grpCompany.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCompany)).BeginInit();
            this.grpBranch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBranch)).BeginInit();
            this.grpBranches.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridBranches)).BeginInit();
            this.SuspendLayout();
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.topPanel.Controls.Add(this.lblTitle);
            this.topPanel.Controls.Add(this.lblFrom);
            this.topPanel.Controls.Add(this.dtFrom);
            this.topPanel.Controls.Add(this.lblTo);
            this.topPanel.Controls.Add(this.dtTo);
            this.topPanel.Controls.Add(this.btnExecute);
            this.topPanel.Controls.Add(this.grpMode);
            this.topPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.topPanel.Location = new System.Drawing.Point(0, 0);
            this.topPanel.Name = "topPanel";
            this.topPanel.Size = new System.Drawing.Size(1254, 86);
            this.topPanel.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 16);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(157, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "VAT Dashboard";
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblFrom.Location = new System.Drawing.Point(183, 22);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(43, 20);
            this.lblFrom.TabIndex = 1;
            this.lblFrom.Text = "From";
            // 
            // dtFrom
            // 
            this.dtFrom.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Location = new System.Drawing.Point(232, 18);
            this.dtFrom.Name = "dtFrom";
            this.dtFrom.Size = new System.Drawing.Size(110, 27);
            this.dtFrom.TabIndex = 2;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTo.Location = new System.Drawing.Point(352, 22);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(25, 20);
            this.lblTo.TabIndex = 3;
            this.lblTo.Text = "To";
            // 
            // dtTo
            // 
            this.dtTo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Location = new System.Drawing.Point(382, 18);
            this.dtTo.Name = "dtTo";
            this.dtTo.Size = new System.Drawing.Size(110, 27);
            this.dtTo.TabIndex = 4;
            // 
            // btnExecute
            // 
            this.btnExecute.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExecute.Location = new System.Drawing.Point(507, 16);
            this.btnExecute.Name = "btnExecute";
            this.btnExecute.Size = new System.Drawing.Size(100, 28);
            this.btnExecute.TabIndex = 5;
            this.btnExecute.Text = "Execute";
            this.btnExecute.UseVisualStyleBackColor = true;
            this.btnExecute.Click += new System.EventHandler(this.btnExecute_Click);
            // 
            // grpMode
            // 
            this.grpMode.Controls.Add(this.rbCompany);
            this.grpMode.Controls.Add(this.rbBranches);
            this.grpMode.Controls.Add(this.rbBranch);
            this.grpMode.Controls.Add(this.cmbBranch);
            this.grpMode.Controls.Add(this.btnPrint);
            this.grpMode.Controls.Add(this.btnPreview);
            this.grpMode.Controls.Add(this.btnExport);
            this.grpMode.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.grpMode.Location = new System.Drawing.Point(638, 6);
            this.grpMode.Name = "grpMode";
            this.grpMode.Size = new System.Drawing.Size(613, 74);
            this.grpMode.TabIndex = 6;
            this.grpMode.TabStop = false;
            this.grpMode.Text = "Mode";
            // 
            // rbCompany
            // 
            this.rbCompany.AutoSize = true;
            this.rbCompany.Checked = true;
            this.rbCompany.Location = new System.Drawing.Point(8, 18);
            this.rbCompany.Name = "rbCompany";
            this.rbCompany.Size = new System.Drawing.Size(113, 24);
            this.rbCompany.TabIndex = 0;
            this.rbCompany.TabStop = true;
            this.rbCompany.Text = "By Company";
            this.rbCompany.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // rbBranches
            // 
            this.rbBranches.AutoSize = true;
            this.rbBranches.Location = new System.Drawing.Point(121, 20);
            this.rbBranches.Name = "rbBranches";
            this.rbBranches.Size = new System.Drawing.Size(109, 24);
            this.rbBranches.TabIndex = 1;
            this.rbBranches.Text = "By Branches";
            this.rbBranches.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // rbBranch
            // 
            this.rbBranch.AutoSize = true;
            this.rbBranch.Location = new System.Drawing.Point(8, 44);
            this.rbBranch.Name = "rbBranch";
            this.rbBranch.Size = new System.Drawing.Size(95, 24);
            this.rbBranch.TabIndex = 2;
            this.rbBranch.Text = "By Branch";
            this.rbBranch.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // cmbBranch
            // 
            this.cmbBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBranch.Font = new System.Drawing.Font("Segoe UI", 8.5F);
            this.cmbBranch.Location = new System.Drawing.Point(236, 18);
            this.cmbBranch.Name = "cmbBranch";
            this.cmbBranch.Size = new System.Drawing.Size(112, 27);
            this.cmbBranch.TabIndex = 3;
            this.cmbBranch.Visible = false;
            this.cmbBranch.SelectedIndexChanged += new System.EventHandler(this.cmbBranch_SelectedIndexChanged);
            // 
            // btnExport
            // 
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.Location = new System.Drawing.Point(414, 13);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(110, 28);
            this.btnExport.TabIndex = 7;
            this.btnExport.Text = "Export to Excel";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnPreview
            // 
            this.btnPreview.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPreview.Location = new System.Drawing.Point(529, 13);
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.Size = new System.Drawing.Size(75, 28);
            this.btnPreview.TabIndex = 8;
            this.btnPreview.Text = "Preview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrint.Location = new System.Drawing.Point(529, 42);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 28);
            this.btnPrint.TabIndex = 9;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // kpiPanel
            // 
            this.kpiPanel.BackColor = System.Drawing.Color.White;
            this.kpiPanel.Controls.Add(this.lblVatCollectedTitle);
            this.kpiPanel.Controls.Add(this.lblVatCollected);
            this.kpiPanel.Controls.Add(this.lblVatPaidTitle);
            this.kpiPanel.Controls.Add(this.lblVatPaid);
            this.kpiPanel.Controls.Add(this.lblVatNetTitle);
            this.kpiPanel.Controls.Add(this.lblVatNet);
            this.kpiPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.kpiPanel.Location = new System.Drawing.Point(0, 86);
            this.kpiPanel.Name = "kpiPanel";
            this.kpiPanel.Padding = new System.Windows.Forms.Padding(12, 8, 12, 8);
            this.kpiPanel.Size = new System.Drawing.Size(1254, 64);
            this.kpiPanel.TabIndex = 1;
            // 
            // lblVatCollectedTitle
            // 
            this.lblVatCollectedTitle.AutoSize = true;
            this.lblVatCollectedTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblVatCollectedTitle.Location = new System.Drawing.Point(16, 10);
            this.lblVatCollectedTitle.Name = "lblVatCollectedTitle";
            this.lblVatCollectedTitle.Size = new System.Drawing.Size(101, 20);
            this.lblVatCollectedTitle.TabIndex = 0;
            this.lblVatCollectedTitle.Text = "VAT Collected";
            // 
            // lblVatCollected
            // 
            this.lblVatCollected.AutoSize = true;
            this.lblVatCollected.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblVatCollected.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblVatCollected.Location = new System.Drawing.Point(16, 28);
            this.lblVatCollected.Name = "lblVatCollected";
            this.lblVatCollected.Size = new System.Drawing.Size(63, 32);
            this.lblVatCollected.TabIndex = 1;
            this.lblVatCollected.Text = "0.00";
            // 
            // lblVatPaidTitle
            // 
            this.lblVatPaidTitle.AutoSize = true;
            this.lblVatPaidTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblVatPaidTitle.Location = new System.Drawing.Point(260, 10);
            this.lblVatPaidTitle.Name = "lblVatPaidTitle";
            this.lblVatPaidTitle.Size = new System.Drawing.Size(66, 20);
            this.lblVatPaidTitle.TabIndex = 2;
            this.lblVatPaidTitle.Text = "VAT Paid";
            // 
            // lblVatPaid
            // 
            this.lblVatPaid.AutoSize = true;
            this.lblVatPaid.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblVatPaid.ForeColor = System.Drawing.Color.Maroon;
            this.lblVatPaid.Location = new System.Drawing.Point(260, 28);
            this.lblVatPaid.Name = "lblVatPaid";
            this.lblVatPaid.Size = new System.Drawing.Size(63, 32);
            this.lblVatPaid.TabIndex = 3;
            this.lblVatPaid.Text = "0.00";
            // 
            // lblVatNetTitle
            // 
            this.lblVatNetTitle.AutoSize = true;
            this.lblVatNetTitle.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblVatNetTitle.Location = new System.Drawing.Point(480, 10);
            this.lblVatNetTitle.Name = "lblVatNetTitle";
            this.lblVatNetTitle.Size = new System.Drawing.Size(127, 20);
            this.lblVatNetTitle.TabIndex = 4;
            this.lblVatNetTitle.Text = "Net VAT (Payable)";
            // 
            // lblVatNet
            // 
            this.lblVatNet.AutoSize = true;
            this.lblVatNet.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblVatNet.Location = new System.Drawing.Point(480, 28);
            this.lblVatNet.Name = "lblVatNet";
            this.lblVatNet.Size = new System.Drawing.Size(63, 32);
            this.lblVatNet.TabIndex = 5;
            this.lblVatNet.Text = "0.00";
            // 
            // splitMain
            // 
            this.splitMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitMain.Location = new System.Drawing.Point(0, 150);
            this.splitMain.Name = "splitMain";
            // 
            // splitMain.Panel1
            // 
            this.splitMain.Panel1.Controls.Add(this.grpCompany);
            // 
            // splitMain.Panel2
            // 
            this.splitMain.Panel2.Controls.Add(this.grpBranch);
            this.splitMain.Panel2.Controls.Add(this.grpBranches);
            this.splitMain.Size = new System.Drawing.Size(1254, 550);
            this.splitMain.SplitterDistance = 830;
            this.splitMain.Panel1MinSize = 650;
            this.splitMain.Panel2MinSize = 350;
            this.splitMain.TabIndex = 0;
            // 
            // grpCompany
            // 
            this.grpCompany.Controls.Add(this.gridCompany);
            this.grpCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpCompany.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpCompany.Location = new System.Drawing.Point(0, 0);
            this.grpCompany.Name = "grpCompany";
            this.grpCompany.Size = new System.Drawing.Size(1011, 550);
            this.grpCompany.TabIndex = 0;
            this.grpCompany.TabStop = false;
            this.grpCompany.Text = "Whole Company";
            // 
            // gridCompany
            // 
            this.gridCompany.BackgroundColor = System.Drawing.Color.White;
            this.gridCompany.ColumnHeadersHeight = 29;
            this.gridCompany.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridCompany.Location = new System.Drawing.Point(3, 23);
            this.gridCompany.Name = "gridCompany";
            this.gridCompany.RowHeadersWidth = 51;
            this.gridCompany.Size = new System.Drawing.Size(1005, 524);
            this.gridCompany.TabIndex = 0;
            // 
            // grpBranch
            // 
            this.grpBranch.Controls.Add(this.gridBranch);
            this.grpBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpBranch.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpBranch.Location = new System.Drawing.Point(0, 220);
            this.grpBranch.Name = "grpBranch";
            this.grpBranch.Size = new System.Drawing.Size(239, 330);
            this.grpBranch.TabIndex = 0;
            this.grpBranch.TabStop = false;
            this.grpBranch.Text = "Current Branch Summary";
            // 
            // gridBranch
            // 
            this.gridBranch.BackgroundColor = System.Drawing.Color.White;
            this.gridBranch.ColumnHeadersHeight = 29;
            this.gridBranch.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBranch.Location = new System.Drawing.Point(3, 23);
            this.gridBranch.Name = "gridBranch";
            this.gridBranch.RowHeadersWidth = 51;
            this.gridBranch.Size = new System.Drawing.Size(233, 304);
            this.gridBranch.TabIndex = 0;
            // 
            // grpBranches
            // 
            this.grpBranches.Controls.Add(this.gridBranches);
            this.grpBranches.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpBranches.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.grpBranches.Location = new System.Drawing.Point(0, 0);
            this.grpBranches.Name = "grpBranches";
            this.grpBranches.Size = new System.Drawing.Size(239, 220);
            this.grpBranches.TabIndex = 1;
            this.grpBranches.TabStop = false;
            this.grpBranches.Text = "Branches Movement";
            // 
            // gridBranches
            // 
            this.gridBranches.BackgroundColor = System.Drawing.Color.White;
            this.gridBranches.ColumnHeadersHeight = 29;
            this.gridBranches.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gridBranches.Location = new System.Drawing.Point(3, 23);
            this.gridBranches.Name = "gridBranches";
            this.gridBranches.RowHeadersWidth = 51;
            this.gridBranches.Size = new System.Drawing.Size(233, 194);
            this.gridBranches.TabIndex = 0;
            // 
            // frm_VatDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1254, 700);
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.kpiPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "frm_VatDashboard";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "VAT Dashboard";
            this.Load += new System.EventHandler(this.frm_VatDashboard_Load);
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            this.kpiPanel.ResumeLayout(false);
            this.kpiPanel.PerformLayout();
            this.splitMain.Panel1.ResumeLayout(false);
            this.splitMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitMain)).EndInit();
            this.splitMain.ResumeLayout(false);
            this.grpCompany.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCompany)).EndInit();
            this.grpBranch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBranch)).EndInit();
            this.grpBranches.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridBranches)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
