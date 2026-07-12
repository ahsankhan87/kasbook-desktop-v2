namespace pos.Accounting.CostCenter
{
    partial class frm_budget_setup
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
            this.pnlSelector = new System.Windows.Forms.GroupBox();
            this.lblCostCenter = new System.Windows.Forms.Label();
            this.cmbCostCenter = new System.Windows.Forms.ComboBox();
            this.lblYear = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.btnLoad = new System.Windows.Forms.Button();
            this.pnlGrid = new System.Windows.Forms.Panel();
            this.dgvBudgets = new System.Windows.Forms.DataGridView();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnFillYear = new System.Windows.Forms.Button();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pnlSelector.SuspendLayout();
            this.pnlGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBudgets)).BeginInit();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(180, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Budget Setup";

            // pnlSelector
            this.pnlSelector.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSelector.Controls.Add(this.lblCostCenter);
            this.pnlSelector.Controls.Add(this.cmbCostCenter);
            this.pnlSelector.Controls.Add(this.lblYear);
            this.pnlSelector.Controls.Add(this.cmbYear);
            this.pnlSelector.Controls.Add(this.btnLoad);
            this.pnlSelector.Location = new System.Drawing.Point(12, 45);
            this.pnlSelector.Name = "pnlSelector";
            this.pnlSelector.Padding = new System.Windows.Forms.Padding(10);
            this.pnlSelector.Size = new System.Drawing.Size(940, 60);
            this.pnlSelector.TabIndex = 1;
            this.pnlSelector.TabStop = false;
            this.pnlSelector.Text = "Selection";

            // lblCostCenter
            this.lblCostCenter.AutoSize = true;
            this.lblCostCenter.Location = new System.Drawing.Point(10, 22);
            this.lblCostCenter.Name = "lblCostCenter";
            this.lblCostCenter.Size = new System.Drawing.Size(78, 13);
            this.lblCostCenter.TabIndex = 0;
            this.lblCostCenter.Text = "Cost Center:";

            // cmbCostCenter
            this.cmbCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCostCenter.Location = new System.Drawing.Point(90, 20);
            this.cmbCostCenter.Name = "cmbCostCenter";
            this.cmbCostCenter.Size = new System.Drawing.Size(250, 21);
            this.cmbCostCenter.TabIndex = 1;

            // lblYear
            this.lblYear.AutoSize = true;
            this.lblYear.Location = new System.Drawing.Point(360, 22);
            this.lblYear.Name = "lblYear";
            this.lblYear.Size = new System.Drawing.Size(74, 13);
            this.lblYear.TabIndex = 2;
            this.lblYear.Text = "Fiscal Year:";

            // cmbYear
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.Location = new System.Drawing.Point(440, 20);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(150, 21);
            this.cmbYear.TabIndex = 3;

            // btnLoad
            this.btnLoad.Location = new System.Drawing.Point(610, 18);
            this.btnLoad.Name = "btnLoad";
            this.btnLoad.Size = new System.Drawing.Size(75, 25);
            this.btnLoad.TabIndex = 4;
            this.btnLoad.Text = "Load";
            this.btnLoad.UseVisualStyleBackColor = true;

            // pnlGrid
            this.pnlGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlGrid.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlGrid.Controls.Add(this.dgvBudgets);
            this.pnlGrid.Location = new System.Drawing.Point(12, 115);
            this.pnlGrid.Name = "pnlGrid";
            this.pnlGrid.Size = new System.Drawing.Size(940, 380);
            this.pnlGrid.TabIndex = 2;

            // dgvBudgets
            this.dgvBudgets.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBudgets.BackgroundColor = System.Drawing.Color.White;
            this.dgvBudgets.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBudgets.Location = new System.Drawing.Point(5, 5);
            this.dgvBudgets.Name = "dgvBudgets";
            this.dgvBudgets.RowHeadersVisible = false;
            this.dgvBudgets.Size = new System.Drawing.Size(930, 370);
            this.dgvBudgets.TabIndex = 0;

            // pnlButtons
            this.pnlButtons.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Controls.Add(this.btnClear);
            this.pnlButtons.Controls.Add(this.btnFillYear);
            this.pnlButtons.Controls.Add(this.lblStatus);
            this.pnlButtons.Location = new System.Drawing.Point(12, 505);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(940, 40);
            this.pnlButtons.TabIndex = 3;

            // btnSave
            this.btnSave.Location = new System.Drawing.Point(10, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 25);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;

            // btnClear
            this.btnClear.Location = new System.Drawing.Point(90, 8);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 25);
            this.btnClear.TabIndex = 1;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;

            // btnFillYear
            this.btnFillYear.Location = new System.Drawing.Point(170, 8);
            this.btnFillYear.Name = "btnFillYear";
            this.btnFillYear.Size = new System.Drawing.Size(100, 25);
            this.btnFillYear.TabIndex = 2;
            this.btnFillYear.Text = "Fill Year...";
            this.btnFillYear.UseVisualStyleBackColor = true;

            // lblStatus
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(280, 12);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(36, 13);
            this.lblStatus.TabIndex = 3;
            this.lblStatus.Text = "Ready";

            // frm_budget_setup
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(964, 555);
            this.Controls.Add(this.pnlButtons);
            this.Controls.Add(this.pnlGrid);
            this.Controls.Add(this.pnlSelector);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "frm_budget_setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Budget Setup";
            this.Load += new System.EventHandler(this.FrmBudgetSetup_Load);
            this.pnlSelector.ResumeLayout(false);
            this.pnlSelector.PerformLayout();
            this.pnlGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBudgets)).EndInit();
            this.pnlButtons.ResumeLayout(false);
            this.pnlButtons.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.GroupBox pnlSelector;
        private System.Windows.Forms.Label lblCostCenter;
        private System.Windows.Forms.ComboBox cmbCostCenter;
        private System.Windows.Forms.Label lblYear;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Button btnLoad;
        private System.Windows.Forms.Panel pnlGrid;
        private System.Windows.Forms.DataGridView dgvBudgets;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnFillYear;
        private System.Windows.Forms.Label lblStatus;
    }
}
