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
            this.lblEntity = new System.Windows.Forms.Label();
            this.cmbEntity = new System.Windows.Forms.ComboBox();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.lblToDate = new System.Windows.Forms.Label();
            this.btnLoad = new System.Windows.Forms.Button();
            this.pnlInfoCard = new System.Windows.Forms.Panel();
            this.lblEntityInfo = new System.Windows.Forms.Label();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvLedger = new System.Windows.Forms.DataGridView();
            this.pnlAging = new System.Windows.Forms.Panel();
            this.lbl0_30 = new System.Windows.Forms.Label();
            this.lbl31_60 = new System.Windows.Forms.Label();
            this.lbl61_90 = new System.Windows.Forms.Label();
            this.lbl90Plus = new System.Windows.Forms.Label();
            this.lblAging = new System.Windows.Forms.Label();
            this.pnlFooter = new System.Windows.Forms.Panel();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnReceivePayment = new System.Windows.Forms.Button();
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
            this.pnlHeader.Controls.Add(this.lblTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(1000, 50);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(10, 12);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(100, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Sub Ledger";
            // 
            // pnlEntitySelector
            // 
            this.pnlEntitySelector.Controls.Add(this.btnLoad);
            this.pnlEntitySelector.Controls.Add(this.lblToDate);
            this.pnlEntitySelector.Controls.Add(this.lblFromDate);
            this.pnlEntitySelector.Controls.Add(this.dtpToDate);
            this.pnlEntitySelector.Controls.Add(this.dtpFromDate);
            this.pnlEntitySelector.Controls.Add(this.cmbEntity);
            this.pnlEntitySelector.Controls.Add(this.lblEntity);
            this.pnlEntitySelector.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlEntitySelector.Location = new System.Drawing.Point(0, 50);
            this.pnlEntitySelector.Name = "pnlEntitySelector";
            this.pnlEntitySelector.Size = new System.Drawing.Size(1000, 60);
            this.pnlEntitySelector.TabIndex = 1;
            // 
            // lblEntity
            // 
            this.lblEntity.AutoSize = true;
            this.lblEntity.Location = new System.Drawing.Point(10, 15);
            this.lblEntity.Name = "lblEntity";
            this.lblEntity.Size = new System.Drawing.Size(60, 13);
            this.lblEntity.TabIndex = 0;
            this.lblEntity.Text = "Entity:";
            // 
            // cmbEntity
            // 
            this.cmbEntity.FormattingEnabled = true;
            this.cmbEntity.Location = new System.Drawing.Point(75, 12);
            this.cmbEntity.Name = "cmbEntity";
            this.cmbEntity.Size = new System.Drawing.Size(200, 21);
            this.cmbEntity.TabIndex = 1;
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(290, 15);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(60, 13);
            this.lblFromDate.TabIndex = 2;
            this.lblFromDate.Text = "From Date:";
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Location = new System.Drawing.Point(355, 12);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(120, 20);
            this.dtpFromDate.TabIndex = 3;
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(480, 15);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(54, 13);
            this.lblToDate.TabIndex = 4;
            this.lblToDate.Text = "To Date:";
            // 
            // dtpToDate
            // 
            this.dtpToDate.Location = new System.Drawing.Point(540, 12);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(120, 20);
            this.dtpToDate.TabIndex = 5;
            // 
            // btnLoad
            // 
            this.btnLoad.Location = new System.Drawing.Point(670, 12);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 23);
            this.btnLoad.TabIndex = 6;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;
            // 
            // pnlInfoCard
            // 
            this.pnlInfoCard.BackColor = System.Drawing.Color.LightGray;
            this.pnlInfoCard.Controls.Add(this.lblEntityInfo);
            this.pnlInfoCard.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlInfoCard.Location = new System.Drawing.Point(0, 110);
            this.pnlInfoCard.Name = "pnlInfoCard";
            this.pnlInfoCard.Size = new System.Drawing.Size(1000, 60);
            this.pnlInfoCard.TabIndex = 2;
            // 
            // lblEntityInfo
            // 
            this.lblEntityInfo.AutoSize = true;
            this.lblEntityInfo.Location = new System.Drawing.Point(10, 15);
            this.lblEntityInfo.Name = "lblEntityInfo";
            this.lblEntityInfo.Size = new System.Drawing.Size(100, 13);
            this.lblEntityInfo.TabIndex = 0;
            this.lblEntityInfo.Text = "Entity Information";
            // 
            // pnlGrid
            // 
            this.pnlGrid.Controls.Add(this.dgvLedger);
            this.pnlGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGrid.Location = new System.Drawing.Point(0, 170);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(1000, 270);
            this.pnlGrid.TabIndex = 3;
            // 
            // dgvLedger
            // 
            this.dgvLedger.AllowUserToAddRows = false;
            this.dgvLedger.AllowUserToDeleteRows = false;
            this.dgvLedger.BackgroundColor = System.Drawing.Color.White;
            this.dgvLedger.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLedger.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLedger.Location = new System.Drawing.Point(0, 0);
            this.dgvLedger.Name = "dgvLedger";
            this.dgvLedger.ReadOnly = true;
            this.dgvLedger.Size = new System.Drawing.Size(1000, 270);
            this.dgvLedger.TabIndex = 0;
            // 
            // pnlAging
            // 
            this.pnlAging.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlAging.Controls.Add(this.lbl90Plus);
            this.pnlAging.Controls.Add(this.lbl61_90);
            this.pnlAging.Controls.Add(this.lbl31_60);
            this.pnlAging.Controls.Add(this.lbl0_30);
            this.pnlAging.Controls.Add(this.lblAging);
            this.pnlAging.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlAging.Location = new System.Drawing.Point(0, 400);
            this.pnlAging.Name = "pnlAging";
            this.pnlAging.Size = new System.Drawing.Size(1000, 50);
            this.pnlAging.TabIndex = 4;
            // 
            // lblAging
            // 
            this.lblAging.AutoSize = true;
            this.lblAging.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblAging.Location = new System.Drawing.Point(10, 15);
            this.lblAging.Name = "lblAging";
            this.lblAging.Size = new System.Drawing.Size(94, 19);
            this.lblAging.TabIndex = 0;
            this.lblAging.Text = "Aging Analysis";
            // 
            // lbl0_30
            // 
            this.lbl0_30.AutoSize = true;
            this.lbl0_30.Location = new System.Drawing.Point(120, 15);
            this.lbl0_30.Name = "lbl0_30";
            this.lbl0_30.Size = new System.Drawing.Size(100, 13);
            this.lbl0_30.TabIndex = 1;
            this.lbl0_30.Text = "0-30: 0";
            // 
            // lbl31_60
            // 
            this.lbl31_60.AutoSize = true;
            this.lbl31_60.Location = new System.Drawing.Point(250, 15);
            this.lbl31_60.Name = "lbl31_60";
            this.lbl31_60.Size = new System.Drawing.Size(100, 13);
            this.lbl31_60.TabIndex = 2;
            this.lbl31_60.Text = "31-60: 0";
            // 
            // lbl61_90
            // 
            this.lbl61_90.AutoSize = true;
            this.lbl61_90.Location = new System.Drawing.Point(380, 15);
            this.lbl61_90.Name = "lbl61_90";
            this.lbl61_90.Size = new System.Drawing.Size(100, 13);
            this.lbl61_90.TabIndex = 3;
            this.lbl61_90.Text = "61-90: 0";
            // 
            // lbl90Plus
            // 
            this.lbl90Plus.AutoSize = true;
            this.lbl90Plus.Location = new System.Drawing.Point(510, 15);
            this.lbl90Plus.Name = "lbl90Plus";
            this.lbl90Plus.Size = new System.Drawing.Size(100, 13);
            this.lbl90Plus.TabIndex = 4;
            this.lbl90Plus.Text = "90+: 0";
            // 
            // pnlFooter
            // 
            this.pnlFooter.Controls.Add(this.btnReceivePayment);
            this.pnlFooter.Controls.Add(this.btnExport);
            this.pnlFooter.Controls.Add(this.btnPrint);
            this.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Location = new System.Drawing.Point(0, 450);
            this.pnlFooter.Name = "pnlFooter";
            this.pnlFooter.Size = new System.Drawing.Size(1000, 50);
            this.pnlFooter.TabIndex = 5;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(10, 12);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 0;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(95, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnReceivePayment
            // 
            this.btnReceivePayment.Location = new System.Drawing.Point(180, 12);
            this.btnReceivePayment.Name = "btnReceivePayment";
            this.btnReceivePayment.Size = new System.Drawing.Size(120, 23);
            this.btnReceivePayment.TabIndex = 2;
            this.btnReceivePayment.Text = "Receive Payment";
            this.btnReceivePayment.UseVisualStyleBackColor = true;
            // 
            // FrmSubLedgerBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 500);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlAging);
            this.Controls.Add(this.pnlFooter);
            this.Controls.Add(this.pnlInfoCard);
            this.Controls.Add(this.pnlEntitySelector);
            this.Controls.Add(this.pnlHeader);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "FrmSubLedgerBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sub Ledger";
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
