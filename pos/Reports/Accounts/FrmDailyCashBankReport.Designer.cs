namespace pos.Reports.Accounts
{
    partial class FrmDailyCashBankReport
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.rbDateRange = new System.Windows.Forms.RadioButton();
            this.rbSingleDay = new System.Windows.Forms.RadioButton();
            this.gbView = new System.Windows.Forms.GroupBox();
            this.rbByAccount = new System.Windows.Forms.RadioButton();
            this.rbConsolidated = new System.Windows.Forms.RadioButton();
            this.lblSingleDate = new System.Windows.Forms.Label();
            this.dtpSingleDate = new System.Windows.Forms.DateTimePicker();
            this.lblFromDate = new System.Windows.Forms.Label();
            this.dtpFromDate = new System.Windows.Forms.DateTimePicker();
            this.lblToDate = new System.Windows.Forms.Label();
            this.dtpToDate = new System.Windows.Forms.DateTimePicker();
            this.btnLoad = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.pnlVariance = new System.Windows.Forms.Panel();
            this.lblVarianceTitle = new System.Windows.Forms.Label();
            this.lblCashHeader = new System.Windows.Forms.Label();
            this.lblSystemCashLabel = new System.Windows.Forms.Label();
            this.lblSystemCashClosing = new System.Windows.Forms.Label();
            this.lblActualCashLabel = new System.Windows.Forms.Label();
            this.txtActualCash = new System.Windows.Forms.TextBox();
            this.lblCashVarianceLabel = new System.Windows.Forms.Label();
            this.lblCashVariance = new System.Windows.Forms.Label();
            this.lblBankHeader = new System.Windows.Forms.Label();
            this.lblSystemBankLabel = new System.Windows.Forms.Label();
            this.lblSystemBankClosing = new System.Windows.Forms.Label();
            this.lblActualBankLabel = new System.Windows.Forms.Label();
            this.txtActualBank = new System.Windows.Forms.TextBox();
            this.lblBankVarianceLabel = new System.Windows.Forms.Label();
            this.lblBankVariance = new System.Windows.Forms.Label();
            this.lblTotalVarianceLabel = new System.Windows.Forms.Label();
            this.lblTotalVariance = new System.Windows.Forms.Label();
            this.dgvReport = new System.Windows.Forms.DataGridView();
            this.pnlTop.SuspendLayout();
            this.pnlFilters.SuspendLayout();
            this.gbMode.SuspendLayout();
            this.gbView.SuspendLayout();
            this.pnlVariance.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTop.Location = new System.Drawing.Point(0, 0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1200, 60);
            this.pnlTop.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(15, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(463, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Daily Cash && Bank Opening/Closing Report";
            // 
            // pnlFilters
            // 
            this.pnlFilters.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(236)))), ((int)(((byte)(240)))), ((int)(((byte)(241)))));
            this.pnlFilters.Controls.Add(this.gbMode);
            this.pnlFilters.Controls.Add(this.gbView);
            this.pnlFilters.Controls.Add(this.lblSingleDate);
            this.pnlFilters.Controls.Add(this.dtpSingleDate);
            this.pnlFilters.Controls.Add(this.lblFromDate);
            this.pnlFilters.Controls.Add(this.dtpFromDate);
            this.pnlFilters.Controls.Add(this.lblToDate);
            this.pnlFilters.Controls.Add(this.dtpToDate);
            this.pnlFilters.Controls.Add(this.btnLoad);
            this.pnlFilters.Controls.Add(this.btnPrint);
            this.pnlFilters.Controls.Add(this.btnExport);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 60);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Padding = new System.Windows.Forms.Padding(10);
            this.pnlFilters.Size = new System.Drawing.Size(1200, 120);
            this.pnlFilters.TabIndex = 1;
            // 
            // gbMode
            // 
            this.gbMode.Controls.Add(this.rbSingleDay);
            this.gbMode.Controls.Add(this.rbDateRange);
            this.gbMode.Location = new System.Drawing.Point(15, 15);
            this.gbMode.Name = "gbMode";
            this.gbMode.Size = new System.Drawing.Size(150, 90);
            this.gbMode.TabIndex = 0;
            this.gbMode.TabStop = false;
            this.gbMode.Text = "Mode";
            // 
            // rbSingleDay
            // 
            this.rbSingleDay.AutoSize = true;
            this.rbSingleDay.Location = new System.Drawing.Point(15, 25);
            this.rbSingleDay.Name = "rbSingleDay";
            this.rbSingleDay.Size = new System.Drawing.Size(79, 19);
            this.rbSingleDay.TabIndex = 0;
            this.rbSingleDay.TabStop = true;
            this.rbSingleDay.Text = "Single Day";
            this.rbSingleDay.UseVisualStyleBackColor = true;
            // 
            // rbDateRange
            // 
            this.rbDateRange.AutoSize = true;
            this.rbDateRange.Location = new System.Drawing.Point(15, 55);
            this.rbDateRange.Name = "rbDateRange";
            this.rbDateRange.Size = new System.Drawing.Size(86, 19);
            this.rbDateRange.TabIndex = 1;
            this.rbDateRange.TabStop = true;
            this.rbDateRange.Text = "Date Range";
            this.rbDateRange.UseVisualStyleBackColor = true;
            // 
            // gbView
            // 
            this.gbView.Controls.Add(this.rbConsolidated);
            this.gbView.Controls.Add(this.rbByAccount);
            this.gbView.Location = new System.Drawing.Point(180, 15);
            this.gbView.Name = "gbView";
            this.gbView.Size = new System.Drawing.Size(150, 90);
            this.gbView.TabIndex = 1;
            this.gbView.TabStop = false;
            this.gbView.Text = "View";
            // 
            // rbConsolidated
            // 
            this.rbConsolidated.AutoSize = true;
            this.rbConsolidated.Location = new System.Drawing.Point(15, 25);
            this.rbConsolidated.Name = "rbConsolidated";
            this.rbConsolidated.Size = new System.Drawing.Size(96, 19);
            this.rbConsolidated.TabIndex = 0;
            this.rbConsolidated.TabStop = true;
            this.rbConsolidated.Text = "Consolidated";
            this.rbConsolidated.UseVisualStyleBackColor = true;
            // 
            // rbByAccount
            // 
            this.rbByAccount.AutoSize = true;
            this.rbByAccount.Location = new System.Drawing.Point(15, 55);
            this.rbByAccount.Name = "rbByAccount";
            this.rbByAccount.Size = new System.Drawing.Size(85, 19);
            this.rbByAccount.TabIndex = 1;
            this.rbByAccount.TabStop = true;
            this.rbByAccount.Text = "By Account";
            this.rbByAccount.UseVisualStyleBackColor = true;
            // 
            // lblSingleDate
            // 
            this.lblSingleDate.AutoSize = true;
            this.lblSingleDate.Location = new System.Drawing.Point(350, 25);
            this.lblSingleDate.Name = "lblSingleDate";
            this.lblSingleDate.Size = new System.Drawing.Size(34, 15);
            this.lblSingleDate.TabIndex = 2;
            this.lblSingleDate.Text = "Date:";
            // 
            // dtpSingleDate
            // 
            this.dtpSingleDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSingleDate.Location = new System.Drawing.Point(350, 45);
            this.dtpSingleDate.Name = "dtpSingleDate";
            this.dtpSingleDate.Size = new System.Drawing.Size(150, 23);
            this.dtpSingleDate.TabIndex = 3;
            // 
            // lblFromDate
            // 
            this.lblFromDate.AutoSize = true;
            this.lblFromDate.Location = new System.Drawing.Point(350, 25);
            this.lblFromDate.Name = "lblFromDate";
            this.lblFromDate.Size = new System.Drawing.Size(38, 15);
            this.lblFromDate.TabIndex = 4;
            this.lblFromDate.Text = "From:";
            this.lblFromDate.Visible = false;
            // 
            // dtpFromDate
            // 
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Location = new System.Drawing.Point(350, 45);
            this.dtpFromDate.Name = "dtpFromDate";
            this.dtpFromDate.Size = new System.Drawing.Size(150, 23);
            this.dtpFromDate.TabIndex = 5;
            this.dtpFromDate.Visible = false;
            // 
            // lblToDate
            // 
            this.lblToDate.AutoSize = true;
            this.lblToDate.Location = new System.Drawing.Point(520, 25);
            this.lblToDate.Name = "lblToDate";
            this.lblToDate.Size = new System.Drawing.Size(22, 15);
            this.lblToDate.TabIndex = 6;
            this.lblToDate.Text = "To:";
            this.lblToDate.Visible = false;
            // 
            // dtpToDate
            // 
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Location = new System.Drawing.Point(520, 45);
            this.dtpToDate.Name = "dtpToDate";
            this.dtpToDate.Size = new System.Drawing.Size(150, 23);
            this.dtpToDate.TabIndex = 7;
            this.dtpToDate.Visible = false;
            // 
            // btnLoad
            // 
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnLoad.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLoad.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Location = new System.Drawing.Point(700, 40);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(100, 32);
            this.btnLoad.TabIndex = 8;
            this.btnLoad.Text = "Load Report";
            this.btnLoad.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Location = new System.Drawing.Point(820, 40);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(100, 32);
            this.btnPrint.TabIndex = 9;
            this.btnPrint.Text = "Print";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(940, 40);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 32);
            this.btnExport.TabIndex = 10;
            this.btnExport.Text = "Export CSV";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // pnlVariance
            // 
            this.pnlVariance.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(248)))), ((int)(((byte)(255)))));
            this.pnlVariance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlVariance.Controls.Add(this.lblVarianceTitle);
            this.pnlVariance.Controls.Add(this.lblCashHeader);
            this.pnlVariance.Controls.Add(this.lblSystemCashLabel);
            this.pnlVariance.Controls.Add(this.lblSystemCashClosing);
            this.pnlVariance.Controls.Add(this.lblActualCashLabel);
            this.pnlVariance.Controls.Add(this.txtActualCash);
            this.pnlVariance.Controls.Add(this.lblCashVarianceLabel);
            this.pnlVariance.Controls.Add(this.lblCashVariance);
            this.pnlVariance.Controls.Add(this.lblBankHeader);
            this.pnlVariance.Controls.Add(this.lblSystemBankLabel);
            this.pnlVariance.Controls.Add(this.lblSystemBankClosing);
            this.pnlVariance.Controls.Add(this.lblActualBankLabel);
            this.pnlVariance.Controls.Add(this.txtActualBank);
            this.pnlVariance.Controls.Add(this.lblBankVarianceLabel);
            this.pnlVariance.Controls.Add(this.lblBankVariance);
            this.pnlVariance.Controls.Add(this.lblTotalVarianceLabel);
            this.pnlVariance.Controls.Add(this.lblTotalVariance);
            this.pnlVariance.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlVariance.Location = new System.Drawing.Point(0, 600);
            this.pnlVariance.Name = "pnlVariance";
            this.pnlVariance.Padding = new System.Windows.Forms.Padding(10);
            this.pnlVariance.Size = new System.Drawing.Size(1200, 120);
            this.pnlVariance.TabIndex = 3;
            // 
            // lblVarianceTitle
            // 
            this.lblVarianceTitle.AutoSize = true;
            this.lblVarianceTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblVarianceTitle.Location = new System.Drawing.Point(15, 12);
            this.lblVarianceTitle.Name = "lblVarianceTitle";
            this.lblVarianceTitle.Size = new System.Drawing.Size(210, 21);
            this.lblVarianceTitle.TabIndex = 0;
            this.lblVarianceTitle.Text = "Daily Tally (Variance Check)";
            // 
            // lblCashHeader
            // 
            this.lblCashHeader.AutoSize = true;
            this.lblCashHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCashHeader.Location = new System.Drawing.Point(15, 45);
            this.lblCashHeader.Name = "lblCashHeader";
            this.lblCashHeader.Size = new System.Drawing.Size(42, 19);
            this.lblCashHeader.TabIndex = 1;
            this.lblCashHeader.Text = "Cash";
            // 
            // lblSystemCashLabel
            // 
            this.lblSystemCashLabel.AutoSize = true;
            this.lblSystemCashLabel.Location = new System.Drawing.Point(15, 75);
            this.lblSystemCashLabel.Name = "lblSystemCashLabel";
            this.lblSystemCashLabel.Size = new System.Drawing.Size(100, 15);
            this.lblSystemCashLabel.TabIndex = 2;
            this.lblSystemCashLabel.Text = "System Closing:";
            // 
            // lblSystemCashClosing
            // 
            this.lblSystemCashClosing.AutoSize = true;
            this.lblSystemCashClosing.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSystemCashClosing.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblSystemCashClosing.Location = new System.Drawing.Point(120, 73);
            this.lblSystemCashClosing.Name = "lblSystemCashClosing";
            this.lblSystemCashClosing.Size = new System.Drawing.Size(38, 19);
            this.lblSystemCashClosing.TabIndex = 3;
            this.lblSystemCashClosing.Text = "0.00";
            // 
            // lblActualCashLabel
            // 
            this.lblActualCashLabel.AutoSize = true;
            this.lblActualCashLabel.Location = new System.Drawing.Point(200, 75);
            this.lblActualCashLabel.Name = "lblActualCashLabel";
            this.lblActualCashLabel.Size = new System.Drawing.Size(45, 15);
            this.lblActualCashLabel.TabIndex = 4;
            this.lblActualCashLabel.Text = "Actual:";
            // 
            // txtActualCash
            // 
            this.txtActualCash.Location = new System.Drawing.Point(250, 71);
            this.txtActualCash.Name = "txtActualCash";
            this.txtActualCash.Size = new System.Drawing.Size(120, 23);
            this.txtActualCash.TabIndex = 5;
            // 
            // lblCashVarianceLabel
            // 
            this.lblCashVarianceLabel.AutoSize = true;
            this.lblCashVarianceLabel.Location = new System.Drawing.Point(385, 75);
            this.lblCashVarianceLabel.Name = "lblCashVarianceLabel";
            this.lblCashVarianceLabel.Size = new System.Drawing.Size(56, 15);
            this.lblCashVarianceLabel.TabIndex = 6;
            this.lblCashVarianceLabel.Text = "Variance:";
            // 
            // lblCashVariance
            // 
            this.lblCashVariance.AutoSize = true;
            this.lblCashVariance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblCashVariance.Location = new System.Drawing.Point(445, 73);
            this.lblCashVariance.Name = "lblCashVariance";
            this.lblCashVariance.Size = new System.Drawing.Size(38, 19);
            this.lblCashVariance.TabIndex = 7;
            this.lblCashVariance.Text = "0.00";
            // 
            // lblBankHeader
            // 
            this.lblBankHeader.AutoSize = true;
            this.lblBankHeader.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBankHeader.Location = new System.Drawing.Point(550, 45);
            this.lblBankHeader.Name = "lblBankHeader";
            this.lblBankHeader.Size = new System.Drawing.Size(42, 19);
            this.lblBankHeader.TabIndex = 8;
            this.lblBankHeader.Text = "Bank";
            // 
            // lblSystemBankLabel
            // 
            this.lblSystemBankLabel.AutoSize = true;
            this.lblSystemBankLabel.Location = new System.Drawing.Point(550, 75);
            this.lblSystemBankLabel.Name = "lblSystemBankLabel";
            this.lblSystemBankLabel.Size = new System.Drawing.Size(100, 15);
            this.lblSystemBankLabel.TabIndex = 9;
            this.lblSystemBankLabel.Text = "System Closing:";
            // 
            // lblSystemBankClosing
            // 
            this.lblSystemBankClosing.AutoSize = true;
            this.lblSystemBankClosing.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblSystemBankClosing.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblSystemBankClosing.Location = new System.Drawing.Point(655, 73);
            this.lblSystemBankClosing.Name = "lblSystemBankClosing";
            this.lblSystemBankClosing.Size = new System.Drawing.Size(38, 19);
            this.lblSystemBankClosing.TabIndex = 10;
            this.lblSystemBankClosing.Text = "0.00";
            // 
            // lblActualBankLabel
            // 
            this.lblActualBankLabel.AutoSize = true;
            this.lblActualBankLabel.Location = new System.Drawing.Point(720, 75);
            this.lblActualBankLabel.Name = "lblActualBankLabel";
            this.lblActualBankLabel.Size = new System.Drawing.Size(45, 15);
            this.lblActualBankLabel.TabIndex = 11;
            this.lblActualBankLabel.Text = "Actual:";
            // 
            // txtActualBank
            // 
            this.txtActualBank.Location = new System.Drawing.Point(770, 71);
            this.txtActualBank.Name = "txtActualBank";
            this.txtActualBank.Size = new System.Drawing.Size(120, 23);
            this.txtActualBank.TabIndex = 12;
            // 
            // lblBankVarianceLabel
            // 
            this.lblBankVarianceLabel.AutoSize = true;
            this.lblBankVarianceLabel.Location = new System.Drawing.Point(905, 75);
            this.lblBankVarianceLabel.Name = "lblBankVarianceLabel";
            this.lblBankVarianceLabel.Size = new System.Drawing.Size(56, 15);
            this.lblBankVarianceLabel.TabIndex = 13;
            this.lblBankVarianceLabel.Text = "Variance:";
            // 
            // lblBankVariance
            // 
            this.lblBankVariance.AutoSize = true;
            this.lblBankVariance.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblBankVariance.Location = new System.Drawing.Point(965, 73);
            this.lblBankVariance.Name = "lblBankVariance";
            this.lblBankVariance.Size = new System.Drawing.Size(38, 19);
            this.lblBankVariance.TabIndex = 14;
            this.lblBankVariance.Text = "0.00";
            // 
            // lblTotalVarianceLabel
            // 
            this.lblTotalVarianceLabel.AutoSize = true;
            this.lblTotalVarianceLabel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalVarianceLabel.Location = new System.Drawing.Point(1030, 45);
            this.lblTotalVarianceLabel.Name = "lblTotalVarianceLabel";
            this.lblTotalVarianceLabel.Size = new System.Drawing.Size(112, 19);
            this.lblTotalVarianceLabel.TabIndex = 15;
            this.lblTotalVarianceLabel.Text = "Total Variance:";
            // 
            // lblTotalVariance
            // 
            this.lblTotalVariance.AutoSize = true;
            this.lblTotalVariance.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTotalVariance.Location = new System.Drawing.Point(1030, 73);
            this.lblTotalVariance.Name = "lblTotalVariance";
            this.lblTotalVariance.Size = new System.Drawing.Size(44, 21);
            this.lblTotalVariance.TabIndex = 16;
            this.lblTotalVariance.Text = "0.00";
            // 
            // dgvReport
            // 
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReport.Location = new System.Drawing.Point(0, 180);
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowTemplate.Height = 25;
            this.dgvReport.Size = new System.Drawing.Size(1200, 420);
            this.dgvReport.TabIndex = 2;
            // 
            // FrmDailyCashBankReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 720);
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.pnlVariance);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlTop);
            this.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.Name = "FrmDailyCashBankReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Daily Cash & Bank Report";
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            this.gbMode.ResumeLayout(false);
            this.gbMode.PerformLayout();
            this.gbView.ResumeLayout(false);
            this.gbView.PerformLayout();
            this.pnlVariance.ResumeLayout(false);
            this.pnlVariance.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReport)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlTop;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.GroupBox gbMode;
        private System.Windows.Forms.RadioButton rbSingleDay;
        private System.Windows.Forms.RadioButton rbDateRange;
        private System.Windows.Forms.GroupBox gbView;
        private System.Windows.Forms.RadioButton rbConsolidated;
        private System.Windows.Forms.RadioButton rbByAccount;
        private System.Windows.Forms.Label lblSingleDate;
        private System.Windows.Forms.DateTimePicker dtpSingleDate;
        private System.Windows.Forms.Label lblFromDate;
        private System.Windows.Forms.DateTimePicker dtpFromDate;
        private System.Windows.Forms.Label lblToDate;
        private System.Windows.Forms.DateTimePicker dtpToDate;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Panel pnlVariance;
        private System.Windows.Forms.Label lblVarianceTitle;
        private System.Windows.Forms.Label lblCashHeader;
        private System.Windows.Forms.Label lblSystemCashLabel;
        private System.Windows.Forms.Label lblSystemCashClosing;
        private System.Windows.Forms.Label lblActualCashLabel;
        private System.Windows.Forms.TextBox txtActualCash;
        private System.Windows.Forms.Label lblCashVarianceLabel;
        private System.Windows.Forms.Label lblCashVariance;
        private System.Windows.Forms.Label lblBankHeader;
        private System.Windows.Forms.Label lblSystemBankLabel;
        private System.Windows.Forms.Label lblSystemBankClosing;
        private System.Windows.Forms.Label lblActualBankLabel;
        private System.Windows.Forms.TextBox txtActualBank;
        private System.Windows.Forms.Label lblBankVarianceLabel;
        private System.Windows.Forms.Label lblBankVariance;
        private System.Windows.Forms.Label lblTotalVarianceLabel;
        private System.Windows.Forms.Label lblTotalVariance;
        private System.Windows.Forms.DataGridView dgvReport;
    }
}
