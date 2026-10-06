using pos.UI;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmDailyCashBankReport));
            this.pnlTop = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.gbMode = new System.Windows.Forms.GroupBox();
            this.rbSingleDay = new System.Windows.Forms.RadioButton();
            this.rbDateRange = new System.Windows.Forms.RadioButton();
            this.gbView = new System.Windows.Forms.GroupBox();
            this.rbConsolidated = new System.Windows.Forms.RadioButton();
            this.rbByAccount = new System.Windows.Forms.RadioButton();
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
            resources.ApplyResources(this.pnlTop, "pnlTop");
            this.pnlTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.pnlTop.Controls.Add(this.lblTitle);
            this.pnlTop.Name = "pnlTop";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            // 
            // pnlFilters
            // 
            resources.ApplyResources(this.pnlFilters, "pnlFilters");
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
            this.pnlFilters.Name = "pnlFilters";
            // 
            // gbMode
            // 
            resources.ApplyResources(this.gbMode, "gbMode");
            this.gbMode.Controls.Add(this.rbSingleDay);
            this.gbMode.Controls.Add(this.rbDateRange);
            this.gbMode.Name = "gbMode";
            this.gbMode.TabStop = false;
            // 
            // rbSingleDay
            // 
            resources.ApplyResources(this.rbSingleDay, "rbSingleDay");
            this.rbSingleDay.Name = "rbSingleDay";
            this.rbSingleDay.TabStop = true;
            this.rbSingleDay.UseVisualStyleBackColor = true;
            // 
            // rbDateRange
            // 
            resources.ApplyResources(this.rbDateRange, "rbDateRange");
            this.rbDateRange.Name = "rbDateRange";
            this.rbDateRange.TabStop = true;
            this.rbDateRange.UseVisualStyleBackColor = true;
            // 
            // gbView
            // 
            resources.ApplyResources(this.gbView, "gbView");
            this.gbView.Controls.Add(this.rbConsolidated);
            this.gbView.Controls.Add(this.rbByAccount);
            this.gbView.Name = "gbView";
            this.gbView.TabStop = false;
            // 
            // rbConsolidated
            // 
            resources.ApplyResources(this.rbConsolidated, "rbConsolidated");
            this.rbConsolidated.Name = "rbConsolidated";
            this.rbConsolidated.TabStop = true;
            this.rbConsolidated.UseVisualStyleBackColor = true;
            // 
            // rbByAccount
            // 
            resources.ApplyResources(this.rbByAccount, "rbByAccount");
            this.rbByAccount.Name = "rbByAccount";
            this.rbByAccount.TabStop = true;
            this.rbByAccount.UseVisualStyleBackColor = true;
            // 
            // lblSingleDate
            // 
            resources.ApplyResources(this.lblSingleDate, "lblSingleDate");
            this.lblSingleDate.Name = "lblSingleDate";
            // 
            // dtpSingleDate
            // 
            resources.ApplyResources(this.dtpSingleDate, "dtpSingleDate");
            this.dtpSingleDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpSingleDate.Name = "dtpSingleDate";
            // 
            // lblFromDate
            // 
            resources.ApplyResources(this.lblFromDate, "lblFromDate");
            this.lblFromDate.Name = "lblFromDate";
            // 
            // dtpFromDate
            // 
            resources.ApplyResources(this.dtpFromDate, "dtpFromDate");
            this.dtpFromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFromDate.Name = "dtpFromDate";
            // 
            // lblToDate
            // 
            resources.ApplyResources(this.lblToDate, "lblToDate");
            this.lblToDate.Name = "lblToDate";
            // 
            // dtpToDate
            // 
            resources.ApplyResources(this.dtpToDate, "dtpToDate");
            this.dtpToDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpToDate.Name = "dtpToDate";
            // 
            // btnLoad
            // 
            resources.ApplyResources(this.btnLoad, "btnLoad");
            this.btnLoad.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(41)))), ((int)(((byte)(128)))), ((int)(((byte)(185)))));
            this.btnLoad.ForeColor = System.Drawing.Color.White;
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.UseVisualStyleBackColor = false;
            // 
            // btnPrint
            // 
            resources.ApplyResources(this.btnPrint, "btnPrint");
            this.btnPrint.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(39)))), ((int)(((byte)(174)))), ((int)(((byte)(96)))));
            this.btnPrint.ForeColor = System.Drawing.Color.White;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.UseVisualStyleBackColor = false;
            // 
            // btnExport
            // 
            resources.ApplyResources(this.btnExport, "btnExport");
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(156)))), ((int)(((byte)(18)))));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Name = "btnExport";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // pnlVariance
            // 
            resources.ApplyResources(this.pnlVariance, "pnlVariance");
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
            this.pnlVariance.Name = "pnlVariance";
            // 
            // lblVarianceTitle
            // 
            resources.ApplyResources(this.lblVarianceTitle, "lblVarianceTitle");
            this.lblVarianceTitle.Name = "lblVarianceTitle";
            // 
            // lblCashHeader
            // 
            resources.ApplyResources(this.lblCashHeader, "lblCashHeader");
            this.lblCashHeader.Name = "lblCashHeader";
            // 
            // lblSystemCashLabel
            // 
            resources.ApplyResources(this.lblSystemCashLabel, "lblSystemCashLabel");
            this.lblSystemCashLabel.Name = "lblSystemCashLabel";
            // 
            // lblSystemCashClosing
            // 
            resources.ApplyResources(this.lblSystemCashClosing, "lblSystemCashClosing");
            this.lblSystemCashClosing.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblSystemCashClosing.Name = "lblSystemCashClosing";
            // 
            // lblActualCashLabel
            // 
            resources.ApplyResources(this.lblActualCashLabel, "lblActualCashLabel");
            this.lblActualCashLabel.Name = "lblActualCashLabel";
            // 
            // txtActualCash
            // 
            resources.ApplyResources(this.txtActualCash, "txtActualCash");
            this.txtActualCash.Name = "txtActualCash";
            // 
            // lblCashVarianceLabel
            // 
            resources.ApplyResources(this.lblCashVarianceLabel, "lblCashVarianceLabel");
            this.lblCashVarianceLabel.Name = "lblCashVarianceLabel";
            // 
            // lblCashVariance
            // 
            resources.ApplyResources(this.lblCashVariance, "lblCashVariance");
            this.lblCashVariance.Name = "lblCashVariance";
            // 
            // lblBankHeader
            // 
            resources.ApplyResources(this.lblBankHeader, "lblBankHeader");
            this.lblBankHeader.Name = "lblBankHeader";
            // 
            // lblSystemBankLabel
            // 
            resources.ApplyResources(this.lblSystemBankLabel, "lblSystemBankLabel");
            this.lblSystemBankLabel.Name = "lblSystemBankLabel";
            // 
            // lblSystemBankClosing
            // 
            resources.ApplyResources(this.lblSystemBankClosing, "lblSystemBankClosing");
            this.lblSystemBankClosing.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(73)))), ((int)(((byte)(94)))));
            this.lblSystemBankClosing.Name = "lblSystemBankClosing";
            // 
            // lblActualBankLabel
            // 
            resources.ApplyResources(this.lblActualBankLabel, "lblActualBankLabel");
            this.lblActualBankLabel.Name = "lblActualBankLabel";
            // 
            // txtActualBank
            // 
            resources.ApplyResources(this.txtActualBank, "txtActualBank");
            this.txtActualBank.Name = "txtActualBank";
            // 
            // lblBankVarianceLabel
            // 
            resources.ApplyResources(this.lblBankVarianceLabel, "lblBankVarianceLabel");
            this.lblBankVarianceLabel.Name = "lblBankVarianceLabel";
            // 
            // lblBankVariance
            // 
            resources.ApplyResources(this.lblBankVariance, "lblBankVariance");
            this.lblBankVariance.Name = "lblBankVariance";
            // 
            // lblTotalVarianceLabel
            // 
            resources.ApplyResources(this.lblTotalVarianceLabel, "lblTotalVarianceLabel");
            this.lblTotalVarianceLabel.Name = "lblTotalVarianceLabel";
            // 
            // lblTotalVariance
            // 
            resources.ApplyResources(this.lblTotalVariance, "lblTotalVariance");
            this.lblTotalVariance.Name = "lblTotalVariance";
            // 
            // dgvReport
            // 
            resources.ApplyResources(this.dgvReport, "dgvReport");
            this.dgvReport.AllowUserToAddRows = false;
            this.dgvReport.AllowUserToDeleteRows = false;
            this.dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.dgvReport.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvReport.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReport.Name = "dgvReport";
            this.dgvReport.ReadOnly = true;
            this.dgvReport.RowTemplate.Height = 25;
            // 
            // FrmDailyCashBankReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvReport);
            this.Controls.Add(this.pnlVariance);
            this.Controls.Add(this.pnlFilters);
            this.Controls.Add(this.pnlTop);
            this.Name = "FrmDailyCashBankReport";
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
