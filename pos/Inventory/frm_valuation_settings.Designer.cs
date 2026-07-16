namespace pos.Inventory
{
    partial class frm_valuation_settings
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelBody = new System.Windows.Forms.Panel();
            this.grpPosting = new System.Windows.Forms.GroupBox();
            this.chkPostPerProduct = new System.Windows.Forms.CheckBox();
            this.grpAccounts = new System.Windows.Forms.GroupBox();
            this.lblCogsAccount = new System.Windows.Forms.Label();
            this.cmbCogsAccount = new System.Windows.Forms.ComboBox();
            this.lblInventoryAccount = new System.Windows.Forms.Label();
            this.cmbInventoryAccount = new System.Windows.Forms.ComboBox();
            this.grpInclude = new System.Windows.Forms.GroupBox();
            this.rbExcludeZero = new System.Windows.Forms.RadioButton();
            this.rbActiveOnly = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.grpComponents = new System.Windows.Forms.GroupBox();
            this.rbWithLanded = new System.Windows.Forms.RadioButton();
            this.rbPurchaseOnly = new System.Windows.Forms.RadioButton();
            this.grpMethod = new System.Windows.Forms.GroupBox();
            this.rbStandard = new System.Windows.Forms.RadioButton();
            this.rbFifo = new System.Windows.Forms.RadioButton();
            this.rbWac = new System.Windows.Forms.RadioButton();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.panelBody.SuspendLayout();
            this.grpPosting.SuspendLayout();
            this.grpAccounts.SuspendLayout();
            this.grpInclude.SuspendLayout();
            this.grpComponents.SuspendLayout();
            this.grpMethod.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(557, 48);
            this.panelHeader.TabIndex = 2;
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.lblTitle.Size = new System.Drawing.Size(557, 48);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Inventory Valuation Settings";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panelBody
            // 
            this.panelBody.AutoScroll = true;
            this.panelBody.Controls.Add(this.grpPosting);
            this.panelBody.Controls.Add(this.grpAccounts);
            this.panelBody.Controls.Add(this.grpInclude);
            this.panelBody.Controls.Add(this.grpComponents);
            this.panelBody.Controls.Add(this.grpMethod);
            this.panelBody.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelBody.Location = new System.Drawing.Point(0, 48);
            this.panelBody.Name = "panelBody";
            this.panelBody.Padding = new System.Windows.Forms.Padding(16);
            this.panelBody.Size = new System.Drawing.Size(557, 514);
            this.panelBody.TabIndex = 0;
            // 
            // grpPosting
            // 
            this.grpPosting.Controls.Add(this.chkPostPerProduct);
            this.grpPosting.Location = new System.Drawing.Point(16, 440);
            this.grpPosting.Name = "grpPosting";
            this.grpPosting.Size = new System.Drawing.Size(519, 60);
            this.grpPosting.TabIndex = 0;
            this.grpPosting.TabStop = false;
            this.grpPosting.Text = "COGS Posting";
            // 
            // chkPostPerProduct
            // 
            this.chkPostPerProduct.AutoSize = true;
            this.chkPostPerProduct.Location = new System.Drawing.Point(16, 24);
            this.chkPostPerProduct.Name = "chkPostPerProduct";
            this.chkPostPerProduct.Size = new System.Drawing.Size(499, 21);
            this.chkPostPerProduct.TabIndex = 0;
            this.chkPostPerProduct.Text = "Post individual journal lines per product (unchecked = single summary entry)";
            // 
            // grpAccounts
            // 
            this.grpAccounts.Controls.Add(this.lblCogsAccount);
            this.grpAccounts.Controls.Add(this.cmbCogsAccount);
            this.grpAccounts.Controls.Add(this.lblInventoryAccount);
            this.grpAccounts.Controls.Add(this.cmbInventoryAccount);
            this.grpAccounts.Location = new System.Drawing.Point(16, 316);
            this.grpAccounts.Name = "grpAccounts";
            this.grpAccounts.Size = new System.Drawing.Size(519, 110);
            this.grpAccounts.TabIndex = 1;
            this.grpAccounts.TabStop = false;
            this.grpAccounts.Text = "Accounting Integration";
            // 
            // lblCogsAccount
            // 
            this.lblCogsAccount.AutoSize = true;
            this.lblCogsAccount.Location = new System.Drawing.Point(16, 26);
            this.lblCogsAccount.Name = "lblCogsAccount";
            this.lblCogsAccount.Size = new System.Drawing.Size(170, 17);
            this.lblCogsAccount.TabIndex = 0;
            this.lblCogsAccount.Text = "COGS Account (Expense):";
            // 
            // cmbCogsAccount
            // 
            this.cmbCogsAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCogsAccount.Location = new System.Drawing.Point(180, 22);
            this.cmbCogsAccount.Name = "cmbCogsAccount";
            this.cmbCogsAccount.Size = new System.Drawing.Size(262, 24);
            this.cmbCogsAccount.TabIndex = 1;
            // 
            // lblInventoryAccount
            // 
            this.lblInventoryAccount.AutoSize = true;
            this.lblInventoryAccount.Location = new System.Drawing.Point(16, 62);
            this.lblInventoryAccount.Name = "lblInventoryAccount";
            this.lblInventoryAccount.Size = new System.Drawing.Size(165, 17);
            this.lblInventoryAccount.TabIndex = 2;
            this.lblInventoryAccount.Text = "Inventory Asset Account:";
            // 
            // cmbInventoryAccount
            // 
            this.cmbInventoryAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInventoryAccount.Location = new System.Drawing.Point(180, 58);
            this.cmbInventoryAccount.Name = "cmbInventoryAccount";
            this.cmbInventoryAccount.Size = new System.Drawing.Size(262, 24);
            this.cmbInventoryAccount.TabIndex = 3;
            // 
            // grpInclude
            // 
            this.grpInclude.Controls.Add(this.rbExcludeZero);
            this.grpInclude.Controls.Add(this.rbActiveOnly);
            this.grpInclude.Controls.Add(this.rbAll);
            this.grpInclude.Location = new System.Drawing.Point(16, 210);
            this.grpInclude.Name = "grpInclude";
            this.grpInclude.Size = new System.Drawing.Size(519, 90);
            this.grpInclude.TabIndex = 2;
            this.grpInclude.TabStop = false;
            this.grpInclude.Text = "Include in Valuation";
            // 
            // rbExcludeZero
            // 
            this.rbExcludeZero.AutoSize = true;
            this.rbExcludeZero.Location = new System.Drawing.Point(16, 68);
            this.rbExcludeZero.Name = "rbExcludeZero";
            this.rbExcludeZero.Size = new System.Drawing.Size(208, 21);
            this.rbExcludeZero.TabIndex = 0;
            this.rbExcludeZero.Text = "Exclude Zero-Stock Products";
            // 
            // rbActiveOnly
            // 
            this.rbActiveOnly.AutoSize = true;
            this.rbActiveOnly.Location = new System.Drawing.Point(16, 46);
            this.rbActiveOnly.Name = "rbActiveOnly";
            this.rbActiveOnly.Size = new System.Drawing.Size(98, 21);
            this.rbActiveOnly.TabIndex = 1;
            this.rbActiveOnly.Text = "Active Only";
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Location = new System.Drawing.Point(16, 24);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(100, 21);
            this.rbAll.TabIndex = 2;
            this.rbAll.Text = "All Products";
            // 
            // grpComponents
            // 
            this.grpComponents.Controls.Add(this.rbWithLanded);
            this.grpComponents.Controls.Add(this.rbPurchaseOnly);
            this.grpComponents.Location = new System.Drawing.Point(16, 118);
            this.grpComponents.Name = "grpComponents";
            this.grpComponents.Size = new System.Drawing.Size(519, 80);
            this.grpComponents.TabIndex = 3;
            this.grpComponents.TabStop = false;
            this.grpComponents.Text = "Cost Components";
            // 
            // rbWithLanded
            // 
            this.rbWithLanded.AutoSize = true;
            this.rbWithLanded.Location = new System.Drawing.Point(16, 50);
            this.rbWithLanded.Name = "rbWithLanded";
            this.rbWithLanded.Size = new System.Drawing.Size(416, 21);
            this.rbWithLanded.TabIndex = 0;
            this.rbWithLanded.Text = "Purchase Cost + Import Duty + Freight + Other (Landed Cost)";
            // 
            // rbPurchaseOnly
            // 
            this.rbPurchaseOnly.AutoSize = true;
            this.rbPurchaseOnly.Location = new System.Drawing.Point(16, 28);
            this.rbPurchaseOnly.Name = "rbPurchaseOnly";
            this.rbPurchaseOnly.Size = new System.Drawing.Size(149, 21);
            this.rbPurchaseOnly.TabIndex = 1;
            this.rbPurchaseOnly.Text = "Purchase Cost Only";
            // 
            // grpMethod
            // 
            this.grpMethod.Controls.Add(this.rbStandard);
            this.grpMethod.Controls.Add(this.rbFifo);
            this.grpMethod.Controls.Add(this.rbWac);
            this.grpMethod.Location = new System.Drawing.Point(16, 16);
            this.grpMethod.Name = "grpMethod";
            this.grpMethod.Size = new System.Drawing.Size(519, 90);
            this.grpMethod.TabIndex = 4;
            this.grpMethod.TabStop = false;
            this.grpMethod.Text = "Valuation Method";
            // 
            // rbStandard
            // 
            this.rbStandard.AutoSize = true;
            this.rbStandard.Location = new System.Drawing.Point(240, 28);
            this.rbStandard.Name = "rbStandard";
            this.rbStandard.Size = new System.Drawing.Size(117, 21);
            this.rbStandard.TabIndex = 0;
            this.rbStandard.Text = "Standard Cost";
            // 
            // rbFifo
            // 
            this.rbFifo.AutoSize = true;
            this.rbFifo.Location = new System.Drawing.Point(16, 52);
            this.rbFifo.Name = "rbFifo";
            this.rbFifo.Size = new System.Drawing.Size(174, 21);
            this.rbFifo.TabIndex = 1;
            this.rbFifo.Text = "FIFO (First-In, First-Out)";
            // 
            // rbWac
            // 
            this.rbWac.AutoSize = true;
            this.rbWac.Location = new System.Drawing.Point(16, 28);
            this.rbWac.Name = "rbWac";
            this.rbWac.Size = new System.Drawing.Size(219, 21);
            this.rbWac.TabIndex = 2;
            this.rbWac.Text = "Weighted Average Cost (WAC)";
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.panelFooter.Controls.Add(this.btnCancel);
            this.panelFooter.Controls.Add(this.btnSave);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 562);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(557, 52);
            this.panelFooter.TabIndex = 1;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(445, 8);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 32);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(307, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(130, 32);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save Settings";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frm_valuation_settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 614);
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_valuation_settings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Inventory Valuation Settings";
            this.Load += new System.EventHandler(this.frm_valuation_settings_Load);
            this.panelHeader.ResumeLayout(false);
            this.panelBody.ResumeLayout(false);
            this.grpPosting.ResumeLayout(false);
            this.grpPosting.PerformLayout();
            this.grpAccounts.ResumeLayout(false);
            this.grpAccounts.PerformLayout();
            this.grpInclude.ResumeLayout(false);
            this.grpInclude.PerformLayout();
            this.grpComponents.ResumeLayout(false);
            this.grpComponents.PerformLayout();
            this.grpMethod.ResumeLayout(false);
            this.grpMethod.PerformLayout();
            this.panelFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelBody;
        private System.Windows.Forms.GroupBox grpMethod;
        private System.Windows.Forms.RadioButton rbWac;
        private System.Windows.Forms.RadioButton rbFifo;
        private System.Windows.Forms.RadioButton rbStandard;
        private System.Windows.Forms.GroupBox grpComponents;
        private System.Windows.Forms.RadioButton rbPurchaseOnly;
        private System.Windows.Forms.RadioButton rbWithLanded;
        private System.Windows.Forms.GroupBox grpInclude;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbActiveOnly;
        private System.Windows.Forms.RadioButton rbExcludeZero;
        private System.Windows.Forms.GroupBox grpAccounts;
        private System.Windows.Forms.Label lblCogsAccount;
        private System.Windows.Forms.ComboBox cmbCogsAccount;
        private System.Windows.Forms.Label lblInventoryAccount;
        private System.Windows.Forms.ComboBox cmbInventoryAccount;
        private System.Windows.Forms.GroupBox grpPosting;
        private System.Windows.Forms.CheckBox chkPostPerProduct;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
    }
}
