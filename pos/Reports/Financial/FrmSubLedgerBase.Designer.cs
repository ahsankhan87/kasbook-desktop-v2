namespace pos.Reports.Financial
{
    partial class FrmSubLedgerBase
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

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmSubLedgerBase));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlEntitySelector = new System.Windows.Forms.Panel();
            this.btnLoad = new System.Windows.Forms.Button();
            this.lblToDate = new System.Windows.Forms.Label();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.cmbEntity = new System.Windows.Forms.ComboBox();
            this.lblEntity = new System.Windows.Forms.Label();
            this.pnlInfoCard = new System.Windows.Forms.Panel();
            this.lblEntityInfo = new System.Windows.Forms.Label();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvLedger = new System.Windows.Forms.DataGridView();
            this.pnlAging = new System.Windows.Forms.Panel();
            this.lbl90Plus = new System.Windows.Forms.Label();
            this.lbl61_90 = new System.Windows.Forms.Label();
            this.lbl31_60 = new System.Windows.Forms.Label();
            this.lbl0_30 = new System.Windows.Forms.Label();
            this.lblAging = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnReceivePayment = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.pnlHeader.SuspendLayout();
            this.pnlEntitySelector.SuspendLayout();
            this.pnlInfoCard.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).BeginInit();
            this.pnlAging.SuspendLayout();
            this.pnlFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            resources.ApplyResources(this.pnlHeader, "pnlHeader");
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Name = "pnlHeader";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // pnlEntitySelector
            // 
            resources.ApplyResources(this.pnlEntitySelector, "pnlEntitySelector");
            this.pnlEntitySelector.Controls.Add(this.btnLoad);
            this.pnlEntitySelector.Controls.Add(this.lblToDate);
            this.pnlEntitySelector.Controls.Add(this.lblFromDate);
            this.pnlEntitySelector.Controls.Add(this.dtpToDate);
            this.pnlEntitySelector.Controls.Add(this.dtpFromDate);
            this.pnlEntitySelector.Controls.Add(this.cmbEntity);
            this.pnlEntitySelector.Controls.Add(this.lblEntity);
            this.pnlEntitySelector.Name = "pnlEntitySelector";
            // 
            // btnLoad
            // 
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // lblToDate
            // 
            resources.ApplyResources(this.lblToDate, "lblToDate");
            this.lblToDate.Name = "lblToDate";
            // 
            // lblFromDate
            // 
            resources.ApplyResources(this.lblFromDate, "lblFromDate");
            this.lblFromDate.Name = "lblFromDate";
            // 
            // dtpToDate
            // 
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Name = "dtpToDate";
            // 
            // dtpFromDate
            // 
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Name = "dtpFromDate";
            // 
            // cmbEntity
            // 
            resources.ApplyResources(this.cmbEntity, "cmbEntity");
            this.cmbEntity.FormattingEnabled = true;
            this.cmbEntity.Name = "cmbEntity";
            // 
            // lblEntity
            // 
            resources.ApplyResources(this.lblEntity, "lblEntity");
            this.lblEntity.Name = "lblEntity";
            // 
            // pnlInfoCard
            // 
            resources.ApplyResources(this.pnlInfoCard, "pnlInfoCard");
            this.pnlInfoCard.BackColor = System.Drawing.Color.LightGray;
            this.pnlInfoCard.Controls.Add(this.lblEntityInfo);
            this.pnlInfoCard.Name = "pnlInfoCard";
            // 
            // lblEntityInfo
            // 
            resources.ApplyResources(this.lblEntityInfo, "lblEntityInfo");
            this.lblEntityInfo.Name = "lblEntityInfo";
            // 
            // pnlGrid
            // 
            resources.ApplyResources(this.pnlGrid, "pnlGrid");
            this.pnlGrid.Controls.Add(this.dgvLedger);
            this.pnlGrid.Name = "pnlGrid";
            // 
            // dgvLedger
            // 
            resources.ApplyResources(this.dgvLedger, "dgvLedger");
            this.dgvLedger.AllowUserToAddRows = false;
            this.dgvLedger.AllowUserToDeleteRows = false;
            this.dgvLedger.BackgroundColor = System.Drawing.Color.White;
            this.dgvLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLedger.Name = "dgvLedger";
            this.dgvLedger.ReadOnly = true;
            // 
            // pnlAging
            // 
            resources.ApplyResources(this.pnlAging, "pnlAging");
            this.pnlAging.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlAging.Controls.Add(this.lbl90Plus);
            this.pnlAging.Controls.Add(this.lbl61_90);
            this.pnlAging.Controls.Add(this.lbl31_60);
            this.pnlAging.Controls.Add(this.lbl0_30);
            this.pnlAging.Controls.Add(this.lblAging);
            this.pnlAging.Name = "pnlAging";
            // 
            // lbl90Plus
            // 
            resources.ApplyResources(this.lbl90Plus, "lbl90Plus");
            this.lbl90Plus.Name = "lbl90Plus";
            // 
            // lbl61_90
            // 
            resources.ApplyResources(this.lbl61_90, "lbl61_90");
            this.lbl61_90.Name = "lbl61_90";
            // 
            // lbl31_60
            // 
            resources.ApplyResources(this.lbl31_60, "lbl31_60");
            this.lbl31_60.Name = "lbl31_60";
            // 
            // lbl0_30
            // 
            resources.ApplyResources(this.lbl0_30, "lbl0_30");
            this.lbl0_30.Name = "lbl0_30";
            // 
            // lblAging
            // 
            resources.ApplyResources(this.lblAging, "lblAging");
            this.lblAging.Name = "lblAging";
            // 
            // pnlFooter
            // 
            resources.ApplyResources(this.pnlFooter, "pnlFooter");
            this.pnlFooter.Controls.Add(this.btnReceivePayment);
            this.pnlFooter.Controls.Add(this.btnExport);
            this.pnlFooter.Controls.Add(this.btnPrint);
            this.pnlFooter.Name = "pnlFooter";
            // 
            // btnReceivePayment
            // 
            resources.ApplyResources(this.btnReceivePayment, "btnReceivePayment");
            this.btnReceivePayment.Name = "btnReceivePayment";
            this.btnReceivePayment.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // FrmSubLedgerBase
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlAging);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlInfoCard);
            this.Controls.Add(this.pnlEntitySelector);
            this.Controls.Add(this.pnlHeader);
            this.Name = "FrmSubLedgerBase";
            this.pnlHeader.ResumeLayout(false);
            this.pnlHeader.PerformLayout();
            this.pnlEntitySelector.ResumeLayout(false);
            this.pnlEntitySelector.PerformLayout();
            this.pnlInfoCard.ResumeLayout(false);
            this.pnlInfoCard.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLedger)).EndInit();
            this.pnlAging.ResumeLayout(false);
            this.pnlAging.PerformLayout();
            this.pnlFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        protected System.Windows.Forms.Panel pnlHeader;
        protected System.Windows.Forms.Label lblTitle;
        protected System.Windows.Forms.Panel pnlEntitySelector;
        protected System.Windows.Forms.Label lblEntity;
        protected System.Windows.Forms.ComboBox cmbEntity;
        protected System.Windows.Forms.Label lblFromDate;
        protected System.Windows.Forms.DateTimePicker dtpFromDate;
        protected System.Windows.Forms.Label lblToDate;
        protected System.Windows.Forms.DateTimePicker dtpToDate;
        protected System.Windows.Forms.Button btnLoad;
        protected System.Windows.Forms.Panel pnlInfoCard;
        protected System.Windows.Forms.Label lblEntityInfo;
        protected System.Windows.Forms.Panel pnlGrid;
        protected System.Windows.Forms.DataGridView dgvLedger;
        protected System.Windows.Forms.Panel pnlAging;
        protected System.Windows.Forms.Label lblAging;
        protected System.Windows.Forms.Label lbl0_30;
        protected System.Windows.Forms.Label lbl31_60;
        protected System.Windows.Forms.Label lbl61_90;
        protected System.Windows.Forms.Label lbl90Plus;
        protected System.Windows.Forms.Panel pnlFooter;
        protected System.Windows.Forms.Button btnPrint;
        protected System.Windows.Forms.Button btnExport;
        protected System.Windows.Forms.Button btnReceivePayment;
    }
}
