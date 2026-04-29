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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_VatDashboard));
            this.splitMain = new System.Windows.Forms.SplitContainer();
            this.grpCompany = new System.Windows.Forms.GroupBox();
            this.gridCompany = new System.Windows.Forms.DataGridView();
            this.grpBranch = new System.Windows.Forms.GroupBox();
            this.gridBranch = new System.Windows.Forms.DataGridView();
            this.grpBranches = new System.Windows.Forms.GroupBox();
            this.gridBranches = new System.Windows.Forms.DataGridView();
            this.topPanel = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblFrom = new System.Windows.Forms.Label();
            this.dtFrom = new System.Windows.Forms.DateTimePicker();
            this.lblNote = new System.Windows.Forms.Label();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtTo = new System.Windows.Forms.DateTimePicker();
            this.btnExecute = new System.Windows.Forms.Button();
            this.grpMode = new System.Windows.Forms.GroupBox();
            this.rbCompany = new System.Windows.Forms.RadioButton();
            this.rbBranches = new System.Windows.Forms.RadioButton();
            this.rbBranch = new System.Windows.Forms.RadioButton();
            this.cmbBranch = new System.Windows.Forms.ComboBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnPreview = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.kpiPanel = new System.Windows.Forms.Panel();
            this.lblVatCollectedTitle = new System.Windows.Forms.Label();
            this.lblVatCollected = new System.Windows.Forms.Label();
            this.lblVatPaidTitle = new System.Windows.Forms.Label();
            this.lblVatPaid = new System.Windows.Forms.Label();
            this.lblVatNetTitle = new System.Windows.Forms.Label();
            this.lblVatNet = new System.Windows.Forms.Label();
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
            this.topPanel.SuspendLayout();
            this.grpMode.SuspendLayout();
            this.kpiPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitMain
            // 
            resources.ApplyResources(this.splitMain, "splitMain");
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
            // 
            // grpCompany
            // 
            this.grpCompany.Controls.Add(this.gridCompany);
            resources.ApplyResources(this.grpCompany, "grpCompany");
            this.grpCompany.Name = "grpCompany";
            this.grpCompany.TabStop = false;
            // 
            // gridCompany
            // 
            this.gridCompany.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.gridCompany, "gridCompany");
            this.gridCompany.Name = "gridCompany";
            // 
            // grpBranch
            // 
            this.grpBranch.Controls.Add(this.gridBranch);
            resources.ApplyResources(this.grpBranch, "grpBranch");
            this.grpBranch.Name = "grpBranch";
            this.grpBranch.TabStop = false;
            // 
            // gridBranch
            // 
            this.gridBranch.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.gridBranch, "gridBranch");
            this.gridBranch.Name = "gridBranch";
            // 
            // grpBranches
            // 
            this.grpBranches.Controls.Add(this.gridBranches);
            resources.ApplyResources(this.grpBranches, "grpBranches");
            this.grpBranches.Name = "grpBranches";
            this.grpBranches.TabStop = false;
            // 
            // gridBranches
            // 
            this.gridBranches.BackgroundColor = System.Drawing.Color.White;
            resources.ApplyResources(this.gridBranches, "gridBranches");
            this.gridBranches.Name = "gridBranches";
            // 
            // topPanel
            // 
            this.topPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(246)))), ((int)(((byte)(250)))));
            this.topPanel.Controls.Add(this.lblTitle);
            this.topPanel.Controls.Add(this.lblFrom);
            this.topPanel.Controls.Add(this.dtFrom);
            this.topPanel.Controls.Add(this.lblNote);
            this.topPanel.Controls.Add(this.lblTo);
            this.topPanel.Controls.Add(this.dtTo);
            this.topPanel.Controls.Add(this.btnExecute);
            this.topPanel.Controls.Add(this.grpMode);
            resources.ApplyResources(this.topPanel, "topPanel");
            this.topPanel.Name = "topPanel";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // lblFrom
            // 
            resources.ApplyResources(this.lblFrom, "lblFrom");
            this.lblFrom.Name = "lblFrom";
            // 
            // dtFrom
            // 
            resources.ApplyResources(this.dtFrom, "dtFrom");
            this.dtFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtFrom.Name = "dtFrom";
            // 
            // lblNote
            // 
            resources.ApplyResources(this.lblNote, "lblNote");
            this.lblNote.ForeColor = System.Drawing.Color.Red;
            this.lblNote.Name = "lblNote";
            // 
            // lblTo
            // 
            resources.ApplyResources(this.lblTo, "lblTo");
            this.lblTo.Name = "lblTo";
            // 
            // dtTo
            // 
            resources.ApplyResources(this.dtTo, "dtTo");
            this.dtTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtTo.Name = "dtTo";
            // 
            // btnExecute
            // 
            resources.ApplyResources(this.btnExecute, "btnExecute");
            this.btnExecute.Name = "btnExecute";
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
            resources.ApplyResources(this.grpMode, "grpMode");
            this.grpMode.Name = "grpMode";
            this.grpMode.TabStop = false;
            // 
            // rbCompany
            // 
            resources.ApplyResources(this.rbCompany, "rbCompany");
            this.rbCompany.Checked = true;
            this.rbCompany.Name = "rbCompany";
            this.rbCompany.TabStop = true;
            this.rbCompany.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // rbBranches
            // 
            resources.ApplyResources(this.rbBranches, "rbBranches");
            this.rbBranches.Name = "rbBranches";
            this.rbBranches.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // rbBranch
            // 
            resources.ApplyResources(this.rbBranch, "rbBranch");
            this.rbBranch.Name = "rbBranch";
            this.rbBranch.CheckedChanged += new System.EventHandler(this.ModeChanged);
            // 
            // cmbBranch
            // 
            this.cmbBranch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.cmbBranch, "cmbBranch");
            this.cmbBranch.Name = "cmbBranch";
            this.cmbBranch.SelectedIndexChanged += new System.EventHandler(this.cmbBranch_SelectedIndexChanged);
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnPreview
            // 
            resources.ApplyResources(this.btnPreview, "btnPreview");
            this.btnPreview.Name = "btnPreview";
            this.btnPreview.UseVisualStyleBackColor = true;
            this.btnPreview.Click += new System.EventHandler(this.btnPreview_Click);
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
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
            resources.ApplyResources(this.kpiPanel, "kpiPanel");
            this.kpiPanel.Name = "kpiPanel";
            // 
            // lblVatCollectedTitle
            // 
            resources.ApplyResources(this.lblVatCollectedTitle, "lblVatCollectedTitle");
            this.lblVatCollectedTitle.Name = "lblVatCollectedTitle";
            // 
            // lblVatCollected
            // 
            resources.ApplyResources(this.lblVatCollected, "lblVatCollected");
            this.lblVatCollected.ForeColor = System.Drawing.Color.DarkGreen;
            this.lblVatCollected.Name = "lblVatCollected";
            // 
            // lblVatPaidTitle
            // 
            resources.ApplyResources(this.lblVatPaidTitle, "lblVatPaidTitle");
            this.lblVatPaidTitle.Name = "lblVatPaidTitle";
            // 
            // lblVatPaid
            // 
            resources.ApplyResources(this.lblVatPaid, "lblVatPaid");
            this.lblVatPaid.ForeColor = System.Drawing.Color.Maroon;
            this.lblVatPaid.Name = "lblVatPaid";
            // 
            // lblVatNetTitle
            // 
            resources.ApplyResources(this.lblVatNetTitle, "lblVatNetTitle");
            this.lblVatNetTitle.Name = "lblVatNetTitle";
            // 
            // lblVatNet
            // 
            resources.ApplyResources(this.lblVatNet, "lblVatNet");
            this.lblVatNet.Name = "lblVatNet";
            // 
            // frm_VatDashboard
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.splitMain);
            this.Controls.Add(this.kpiPanel);
            this.Controls.Add(this.topPanel);
            this.Name = "frm_VatDashboard";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_VatDashboard_Load);
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
            this.topPanel.ResumeLayout(false);
            this.topPanel.PerformLayout();
            this.grpMode.ResumeLayout(false);
            this.grpMode.PerformLayout();
            this.kpiPanel.ResumeLayout(false);
            this.kpiPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Label lblNote;
    }
}
