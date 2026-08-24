namespace pos.Accounting.CostCenter
{
    partial class frm_cost_center_setup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_cost_center_setup));
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlForm = new System.Windows.Forms.Panel();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.chkHasEndDate = new System.Windows.Forms.CheckBox();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.lblStartDate = new System.Windows.Forms.Label();
            this.txtBudget = new System.Windows.Forms.TextBox();
            this.lblBudget = new System.Windows.Forms.Label();
            this.cmbManager = new System.Windows.Forms.ComboBox();
            this.lblManager = new System.Windows.Forms.Label();
            this.cmbParent = new System.Windows.Forms.ComboBox();
            this.lblParent = new System.Windows.Forms.Label();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.lblType = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.lblCode = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnNew = new System.Windows.Forms.Button();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dgvCostCenters = new System.Windows.Forms.DataGridView();
            this.pnlForm.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCostCenters)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // pnlForm
            // 
            resources.ApplyResources(this.pnlForm, "pnlForm");
            this.pnlForm.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlForm.Controls.Add(this.txtDescription);
            this.pnlForm.Controls.Add(this.lblDescription);
            this.pnlForm.Controls.Add(this.chkActive);
            this.pnlForm.Controls.Add(this.dtpEndDate);
            this.pnlForm.Controls.Add(this.chkHasEndDate);
            this.pnlForm.Controls.Add(this.dtpStartDate);
            this.pnlForm.Controls.Add(this.lblStartDate);
            this.pnlForm.Controls.Add(this.txtBudget);
            this.pnlForm.Controls.Add(this.lblBudget);
            this.pnlForm.Controls.Add(this.cmbManager);
            this.pnlForm.Controls.Add(this.lblManager);
            this.pnlForm.Controls.Add(this.cmbParent);
            this.pnlForm.Controls.Add(this.lblParent);
            this.pnlForm.Controls.Add(this.cmbType);
            this.pnlForm.Controls.Add(this.lblType);
            this.pnlForm.Controls.Add(this.txtName);
            this.pnlForm.Controls.Add(this.lblName);
            this.pnlForm.Controls.Add(this.txtCode);
            this.pnlForm.Controls.Add(this.lblCode);
            this.pnlForm.Name = "pnlForm";
            // 
            // txtDescription
            // 
            resources.ApplyResources(this.txtDescription, "txtDescription");
            this.txtDescription.Name = "txtDescription";
            // 
            // lblDescription
            // 
            resources.ApplyResources(this.lblDescription, "lblDescription");
            this.lblDescription.Name = "lblDescription";
            // 
            // chkActive
            // 
            resources.ApplyResources(this.chkActive, "chkActive");
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Name = "chkActive";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // dtpEndDate
            // 
            resources.ApplyResources(this.dtpEndDate, "dtpEndDate");
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEndDate.Name = "dtpEndDate";
            // 
            // chkHasEndDate
            // 
            resources.ApplyResources(this.chkHasEndDate, "chkHasEndDate");
            this.chkHasEndDate.Name = "chkHasEndDate";
            this.chkHasEndDate.UseVisualStyleBackColor = true;
            this.chkHasEndDate.CheckedChanged += new System.EventHandler(this.ChkHasEndDate_CheckedChanged);
            // 
            // dtpStartDate
            // 
            resources.ApplyResources(this.dtpStartDate, "dtpStartDate");
            this.dtpStartDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStartDate.Name = "dtpStartDate";
            // 
            // lblStartDate
            // 
            resources.ApplyResources(this.lblStartDate, "lblStartDate");
            this.lblStartDate.Name = "lblStartDate";
            // 
            // txtBudget
            // 
            resources.ApplyResources(this.txtBudget, "txtBudget");
            this.txtBudget.Name = "txtBudget";
            // 
            // lblBudget
            // 
            resources.ApplyResources(this.lblBudget, "lblBudget");
            this.lblBudget.Name = "lblBudget";
            // 
            // cmbManager
            // 
            resources.ApplyResources(this.cmbManager, "cmbManager");
            this.cmbManager.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbManager.Name = "cmbManager";
            // 
            // lblManager
            // 
            resources.ApplyResources(this.lblManager, "lblManager");
            this.lblManager.Name = "lblManager";
            // 
            // cmbParent
            // 
            resources.ApplyResources(this.cmbParent, "cmbParent");
            this.cmbParent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParent.Name = "cmbParent";
            // 
            // lblParent
            // 
            resources.ApplyResources(this.lblParent, "lblParent");
            this.lblParent.Name = "lblParent";
            // 
            // cmbType
            // 
            resources.ApplyResources(this.cmbType, "cmbType");
            this.cmbType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbType.Name = "cmbType";
            // 
            // lblType
            // 
            resources.ApplyResources(this.lblType, "lblType");
            this.lblType.Name = "lblType";
            // 
            // txtName
            // 
            resources.ApplyResources(this.txtName, "txtName");
            this.txtName.Name = "txtName";
            // 
            // lblName
            // 
            resources.ApplyResources(this.lblName, "lblName");
            this.lblName.Name = "lblName";
            // 
            // txtCode
            // 
            resources.ApplyResources(this.txtCode, "txtCode");
            this.txtCode.Name = "txtCode";
            // 
            // lblCode
            // 
            resources.ApplyResources(this.lblCode, "lblCode");
            this.lblCode.Name = "lblCode";
            // 
            // pnlButtons
            // 
            resources.ApplyResources(this.pnlButtons, "pnlButtons");
            this.pnlButtons.Controls.Add(this.btnDelete);
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnEdit);
            this.pnlButtons.Controls.Add(this.btnNew);
            this.pnlButtons.Name = "pnlButtons";
            // 
            // btnDelete
            // 
            resources.ApplyResources(this.btnDelete, "btnDelete");
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnEdit
            // 
            resources.ApplyResources(this.btnEdit, "btnEdit");
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.UseVisualStyleBackColor = true;
            // 
            // btnNew
            // 
            resources.ApplyResources(this.btnNew, "btnNew");
            this.btnNew.Name = "btnNew";
            this.btnNew.UseVisualStyleBackColor = true;
            // 
            // splitter1
            // 
            resources.ApplyResources(this.splitter1, "splitter1");
            this.splitter1.Name = "splitter1";
            this.splitter1.TabStop = false;
            // 
            // dgvCostCenters
            // 
            resources.ApplyResources(this.dgvCostCenters, "dgvCostCenters");
            this.dgvCostCenters.AllowUserToAddRows = false;
            this.dgvCostCenters.AllowUserToDeleteRows = false;
            this.dgvCostCenters.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvCostCenters.BackgroundColor = System.Drawing.Color.White;
            this.dgvCostCenters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCostCenters.Name = "dgvCostCenters";
            this.dgvCostCenters.ReadOnly = true;
            this.dgvCostCenters.RowHeadersVisible = false;
            // 
            // frm_cost_center_setup
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dgvCostCenters);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlForm);
            this.Controls.Add(this.lblTitle);
            this.Name = "frm_cost_center_setup";
            this.Load += new System.EventHandler(this.FrmCostCenterSetup_Load);
            this.pnlForm.ResumeLayout(false);
            this.pnlForm.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCostCenters)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlForm;
        private System.Windows.Forms.Label lblCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblType;
        private System.Windows.Forms.ComboBox cmbType;
        private System.Windows.Forms.Label lblParent;
        private System.Windows.Forms.ComboBox cmbParent;
        private System.Windows.Forms.Label lblManager;
        private System.Windows.Forms.ComboBox cmbManager;
        private System.Windows.Forms.Label lblBudget;
        private System.Windows.Forms.TextBox txtBudget;
        private System.Windows.Forms.Label lblStartDate;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private System.Windows.Forms.CheckBox chkHasEndDate;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnNew;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGridView dgvCostCenters;
    }
}
