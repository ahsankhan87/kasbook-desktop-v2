namespace pos.Reports.Financial
{
    partial class frm_CashFlowStatementReport
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
            this.pnlFilters = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.dtpTo = new System.Windows.Forms.DateTimePicker();
            this.lblTo = new System.Windows.Forms.Label();
            this.dtpFrom = new System.Windows.Forms.DateTimePicker();
            this.lblFrom = new System.Windows.Forms.Label();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cmbPeriod = new System.Windows.Forms.ComboBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.dgvCashFlow = new System.Windows.Forms.DataGridView();
            this.colParticular = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlFilters.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCashFlow)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlFilters
            // 
            this.pnlFilters.Controls.Add(this.lblStatus);
            this.pnlFilters.Controls.Add(this.btnGenerate);
            this.pnlFilters.Controls.Add(this.dtpTo);
            this.pnlFilters.Controls.Add(this.lblTo);
            this.pnlFilters.Controls.Add(this.dtpFrom);
            this.pnlFilters.Controls.Add(this.lblFrom);
            this.pnlFilters.Controls.Add(this.cmbMethod);
            this.pnlFilters.Controls.Add(this.lblMethod);
            this.pnlFilters.Controls.Add(this.cmbPeriod);
            this.pnlFilters.Controls.Add(this.lblPeriod);
            this.pnlFilters.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlFilters.Location = new System.Drawing.Point(0, 0);
            this.pnlFilters.Name = "pnlFilters";
            this.pnlFilters.Size = new System.Drawing.Size(984, 96);
            this.pnlFilters.TabIndex = 0;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblStatus.Location = new System.Drawing.Point(15, 65);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 15);
            this.lblStatus.TabIndex = 9;
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(838, 22);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(128, 28);
            this.btnGenerate.TabIndex = 8;
            this.btnGenerate.Text = "Generate Report";
            this.btnGenerate.UseVisualStyleBackColor = true;
            // 
            // dtpTo
            // 
            this.dtpTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpTo.Location = new System.Drawing.Point(688, 25);
            this.dtpTo.Name = "dtpTo";
            this.dtpTo.Size = new System.Drawing.Size(128, 20);
            this.dtpTo.TabIndex = 7;
            // 
            // lblTo
            // 
            this.lblTo.AutoSize = true;
            this.lblTo.Location = new System.Drawing.Point(664, 29);
            this.lblTo.Name = "lblTo";
            this.lblTo.Size = new System.Drawing.Size(20, 13);
            this.lblTo.TabIndex = 6;
            this.lblTo.Text = "To";
            // 
            // dtpFrom
            // 
            this.dtpFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpFrom.Location = new System.Drawing.Point(530, 25);
            this.dtpFrom.Name = "dtpFrom";
            this.dtpFrom.Size = new System.Drawing.Size(128, 20);
            this.dtpFrom.TabIndex = 5;
            // 
            // lblFrom
            // 
            this.lblFrom.AutoSize = true;
            this.lblFrom.Location = new System.Drawing.Point(494, 29);
            this.lblFrom.Name = "lblFrom";
            this.lblFrom.Size = new System.Drawing.Size(30, 13);
            this.lblFrom.TabIndex = 4;
            this.lblFrom.Text = "From";
            // 
            // cmbMethod
            // 
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.FormattingEnabled = true;
            this.cmbMethod.Location = new System.Drawing.Point(296, 24);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(182, 21);
            this.cmbMethod.TabIndex = 3;
            // 
            // lblMethod
            // 
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(243, 28);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(43, 13);
            this.lblMethod.TabIndex = 2;
            this.lblMethod.Text = "Method";
            // 
            // cmbPeriod
            // 
            this.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPeriod.FormattingEnabled = true;
            this.cmbPeriod.Location = new System.Drawing.Point(56, 24);
            this.cmbPeriod.Name = "cmbPeriod";
            this.cmbPeriod.Size = new System.Drawing.Size(172, 21);
            this.cmbPeriod.TabIndex = 1;
            // 
            // lblPeriod
            // 
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(15, 28);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(37, 13);
            this.lblPeriod.TabIndex = 0;
            this.lblPeriod.Text = "Period";
            // 
            // dgvCashFlow
            // 
            this.dgvCashFlow.AllowUserToAddRows = false;
            this.dgvCashFlow.AllowUserToDeleteRows = false;
            this.dgvCashFlow.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvCashFlow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCashFlow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colParticular,
            this.colAmount});
            this.dgvCashFlow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCashFlow.Location = new System.Drawing.Point(0, 96);
            this.dgvCashFlow.Name = "dgvCashFlow";
            this.dgvCashFlow.ReadOnly = true;
            this.dgvCashFlow.RowHeadersVisible = false;
            this.dgvCashFlow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCashFlow.Size = new System.Drawing.Size(984, 465);
            this.dgvCashFlow.TabIndex = 1;
            // 
            // colParticular
            // 
            this.colParticular.DataPropertyName = "Particular";
            this.colParticular.HeaderText = "Particular";
            this.colParticular.Name = "colParticular";
            this.colParticular.ReadOnly = true;
            // 
            // colAmount
            // 
            this.colAmount.DataPropertyName = "Amount";
            this.colAmount.HeaderText = "Amount";
            this.colAmount.Name = "colAmount";
            this.colAmount.ReadOnly = true;
            // 
            // frm_CashFlowStatementReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 561);
            this.Controls.Add(this.dgvCashFlow);
            this.Controls.Add(this.pnlFilters);
            this.Name = "frm_CashFlowStatementReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cash Flow Statement";
            this.pnlFilters.ResumeLayout(false);
            this.pnlFilters.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCashFlow)).EndInit();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel pnlFilters;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.ComboBox cmbPeriod;
        private System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.DateTimePicker dtpFrom;
        private System.Windows.Forms.Label lblFrom;
        private System.Windows.Forms.DateTimePicker dtpTo;
        private System.Windows.Forms.Label lblTo;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.DataGridView dgvCashFlow;
        private System.Windows.Forms.DataGridViewTextBoxColumn colParticular;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAmount;
    }
}
