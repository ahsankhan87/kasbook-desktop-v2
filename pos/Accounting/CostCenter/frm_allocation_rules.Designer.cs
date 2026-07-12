namespace pos.Accounting.CostCenter
{
    partial class frm_allocation_rules
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
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblSourceAccount = new System.Windows.Forms.Label();
            this.cmbSourceAccount = new System.Windows.Forms.ComboBox();
            this.lblTargetCC = new System.Windows.Forms.Label();
            this.cmbTargetCostCenter = new System.Windows.Forms.ComboBox();
            this.lblMethod = new System.Windows.Forms.Label();
            this.cmbMethod = new System.Windows.Forms.ComboBox();
            this.lblPercent = new System.Windows.Forms.Label();
            this.txtPercent = new System.Windows.Forms.TextBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.pnlAllocation = new System.Windows.Forms.GroupBox();
            this.lblPeriod = new System.Windows.Forms.Label();
            this.dtpPeriod = new System.Windows.Forms.DateTimePicker();
            this.btnRunAllocation = new System.Windows.Forms.Button();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnNew = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dgvRules = new System.Windows.Forms.DataGridView();
            this.pnlForm.SuspendLayout();
            this.pnlAllocation.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).BeginInit();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(270, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Allocation Rules & Auto-Allocation";

            // pnlForm
            this.pnlForm.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlForm.Location = new System.Drawing.Point(410, 40);
            this.pnlForm.Name = "pnlForm";
            this.pnlForm.Padding = new System.Windows.Forms.Padding(15);
            this.pnlForm.Size = new System.Drawing.Size(380, 360);
            this.pnlForm.TabIndex = 1;

            // lblName
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(15, 15);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(70, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Rule Name:";

            // txtName
            this.txtName.Location = new System.Drawing.Point(15, 35);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(350, 22);
            this.txtName.TabIndex = 1;

            // lblSourceAccount
            this.lblSourceAccount.AutoSize = true;
            this.lblSourceAccount.Location = new System.Drawing.Point(15, 65);
            this.lblSourceAccount.Name = "lblSourceAccount";
            this.lblSourceAccount.Size = new System.Drawing.Size(101, 13);
            this.lblSourceAccount.TabIndex = 2;
            this.lblSourceAccount.Text = "Source Account:";

            // cmbSourceAccount
            this.cmbSourceAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceAccount.Location = new System.Drawing.Point(15, 85);
            this.cmbSourceAccount.Name = "cmbSourceAccount";
            this.cmbSourceAccount.Size = new System.Drawing.Size(350, 21);
            this.cmbSourceAccount.TabIndex = 3;

            // lblTargetCC
            this.lblTargetCC.AutoSize = true;
            this.lblTargetCC.Location = new System.Drawing.Point(15, 115);
            this.lblTargetCC.Name = "lblTargetCC";
            this.lblTargetCC.Size = new System.Drawing.Size(131, 13);
            this.lblTargetCC.TabIndex = 4;
            this.lblTargetCC.Text = "Target Cost Center:";

            // cmbTargetCostCenter
            this.cmbTargetCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTargetCostCenter.Location = new System.Drawing.Point(15, 135);
            this.cmbTargetCostCenter.Name = "cmbTargetCostCenter";
            this.cmbTargetCostCenter.Size = new System.Drawing.Size(350, 21);
            this.cmbTargetCostCenter.TabIndex = 5;

            // lblMethod
            this.lblMethod.AutoSize = true;
            this.lblMethod.Location = new System.Drawing.Point(15, 165);
            this.lblMethod.Name = "lblMethod";
            this.lblMethod.Size = new System.Drawing.Size(121, 13);
            this.lblMethod.TabIndex = 6;
            this.lblMethod.Text = "Allocation Method:";

            // cmbMethod
            this.cmbMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMethod.Location = new System.Drawing.Point(15, 185);
            this.cmbMethod.Name = "cmbMethod";
            this.cmbMethod.Size = new System.Drawing.Size(350, 21);
            this.cmbMethod.TabIndex = 7;

            // lblPercent
            this.lblPercent.AutoSize = true;
            this.lblPercent.Location = new System.Drawing.Point(15, 215);
            this.lblPercent.Name = "lblPercent";
            this.lblPercent.Size = new System.Drawing.Size(76, 13);
            this.lblPercent.TabIndex = 8;
            this.lblPercent.Text = "Percentage:";

            // txtPercent
            this.txtPercent.Location = new System.Drawing.Point(15, 235);
            this.txtPercent.Name = "txtPercent";
            this.txtPercent.Size = new System.Drawing.Size(175, 22);
            this.txtPercent.TabIndex = 9;

            // chkActive
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(15, 265);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 10;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;

            // pnlAllocation
            this.pnlAllocation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlAllocation.Controls.Add(this.lblPeriod);
            this.pnlAllocation.Controls.Add(this.dtpPeriod);
            this.pnlAllocation.Controls.Add(this.btnRunAllocation);
            this.pnlAllocation.Location = new System.Drawing.Point(410, 410);
            this.pnlAllocation.Name = "pnlAllocation";
            this.pnlAllocation.Padding = new System.Windows.Forms.Padding(15);
            this.pnlAllocation.Size = new System.Drawing.Size(380, 100);
            this.pnlAllocation.TabIndex = 2;
            this.pnlAllocation.TabStop = false;
            this.pnlAllocation.Text = "Run Allocation";

            // lblPeriod
            this.lblPeriod.AutoSize = true;
            this.lblPeriod.Location = new System.Drawing.Point(15, 25);
            this.lblPeriod.Name = "lblPeriod";
            this.lblPeriod.Size = new System.Drawing.Size(48, 13);
            this.lblPeriod.TabIndex = 0;
            this.lblPeriod.Text = "Period:";

            // dtpPeriod
            this.dtpPeriod.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpPeriod.Location = new System.Drawing.Point(15, 45);
            this.dtpPeriod.Name = "dtpPeriod";
            this.dtpPeriod.Size = new System.Drawing.Size(350, 22);
            this.dtpPeriod.TabIndex = 1;

            // btnRunAllocation
            this.btnRunAllocation.Location = new System.Drawing.Point(240, 70);
            this.btnRunAllocation.Name = "btnRunAllocation";
            this.btnRunAllocation.Size = new System.Drawing.Size(125, 25);
            this.btnRunAllocation.TabIndex = 2;
            this.btnRunAllocation.Text = "Run Allocation";
            this.btnRunAllocation.UseVisualStyleBackColor = true;

            // pnlButtons
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.Location = new System.Drawing.Point(410, 515);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(380, 40);
            this.pnlButtons.TabIndex = 3;

            // btnNew
            this.btnNew.Location = new System.Drawing.Point(15, 8);
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(75, 25);
            this.btnNew.TabIndex = 0;
            this.btnNew.Text = "New";
            this.btnNew.UseVisualStyleBackColor = true;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(95, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;

            // btnCancel
            this.btnCancel.Location = new System.Drawing.Point(175, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;

            // btnDelete
            this.btnDelete.Location = new System.Drawing.Point(255, 8);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(110, 25);
            this.btnDelete.TabIndex = 3;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = true;

            // splitter1
            this.splitter1.Location = new System.Drawing.Point(0, 35);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 520);
            this.splitter1.TabIndex = 4;
            this.splitter1.TabStop = false;

            // dgvRules
            this.dgvRules.AllowUserToAddRows = false;
            this.dgvRules.AllowUserToDeleteRows = false;
            this.dgvRules.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvRules.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvRules.BackgroundColor = System.Drawing.Color.White;
            this.dgvRules.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRules.Location = new System.Drawing.Point(10, 40);
            this.dgvRules.Name = "dgvRules";
            this.dgvRules.ReadOnly = true;
            this.dgvRules.RowHeadersVisible = false;
            this.dgvRules.Size = new System.Drawing.Size(395, 515);
            this.dgvRules.TabIndex = 5;

            // frm_allocation_rules
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(802, 567);
            this.Controls.Add(this.dgvRules);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlAllocation);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "frm_allocation_rules";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Allocation Rules & Auto-Allocation";
            this.Load += new System.EventHandler(this.FrmAllocationRules_Load);
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.pnlAllocation.ResumeLayout(false);
            this.pnlAllocation.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRules)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblSourceAccount;
        private System.Windows.Forms.ComboBox cmbSourceAccount;
        private System.Windows.Forms.Label lblTargetCC;
        private System.Windows.Forms.ComboBox cmbTargetCostCenter;
        private System.Windows.Forms.Label lblMethod;
        private System.Windows.Forms.ComboBox cmbMethod;
        private System.Windows.Forms.Label lblPercent;
        private System.Windows.Forms.TextBox txtPercent;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.GroupBox pnlAllocation;
        private System.Windows.Forms.Label lblPeriod;
        private System.Windows.Forms.DateTimePicker dtpPeriod;
        private System.Windows.Forms.Button btnRunAllocation;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGridView dgvRules;
    }
}
