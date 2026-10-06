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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_valuation_settings));
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
            resources.ApplyResources(this.panelHeader, "panelHeader");
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(212)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Name = "panelHeader";
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Name = "lblTitle";
            // 
            // panelBody
            // 
            resources.ApplyResources(this.panelBody, "panelBody");
            this.panelBody.Controls.Add(this.grpPosting);
            this.panelBody.Controls.Add(this.grpAccounts);
            this.panelBody.Controls.Add(this.grpInclude);
            this.panelBody.Controls.Add(this.grpComponents);
            this.panelBody.Controls.Add(this.grpMethod);
            this.panelBody.Name = "panelBody";
            // 
            // grpPosting
            // 
            resources.ApplyResources(this.grpPosting, "grpPosting");
            this.grpPosting.Controls.Add(this.chkPostPerProduct);
            this.grpPosting.Name = "grpPosting";
            this.grpPosting.TabStop = false;
            // 
            // chkPostPerProduct
            // 
            resources.ApplyResources(this.chkPostPerProduct, "chkPostPerProduct");
            this.chkPostPerProduct.Name = "chkPostPerProduct";
            // 
            // grpAccounts
            // 
            resources.ApplyResources(this.grpAccounts, "grpAccounts");
            this.grpAccounts.Controls.Add(this.lblCogsAccount);
            this.grpAccounts.Controls.Add(this.cmbCogsAccount);
            this.grpAccounts.Controls.Add(this.lblInventoryAccount);
            this.grpAccounts.Controls.Add(this.cmbInventoryAccount);
            this.grpAccounts.Name = "grpAccounts";
            this.grpAccounts.TabStop = false;
            // 
            // lblCogsAccount
            // 
            resources.ApplyResources(this.lblCogsAccount, "lblCogsAccount");
            this.lblCogsAccount.Name = "lblCogsAccount";
            // 
            // cmbCogsAccount
            // 
            resources.ApplyResources(this.cmbCogsAccount, "cmbCogsAccount");
            this.cmbCogsAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCogsAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCogsAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCogsAccount.Name = "cmbCogsAccount";
            // 
            // lblInventoryAccount
            // 
            resources.ApplyResources(this.lblInventoryAccount, "lblInventoryAccount");
            this.lblInventoryAccount.Name = "lblInventoryAccount";
            // 
            // cmbInventoryAccount
            // 
            resources.ApplyResources(this.cmbInventoryAccount, "cmbInventoryAccount");
            this.cmbInventoryAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbInventoryAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbInventoryAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInventoryAccount.Name = "cmbInventoryAccount";
            // 
            // grpInclude
            // 
            resources.ApplyResources(this.grpInclude, "grpInclude");
            this.grpInclude.Controls.Add(this.rbExcludeZero);
            this.grpInclude.Controls.Add(this.rbActiveOnly);
            this.grpInclude.Controls.Add(this.rbAll);
            this.grpInclude.Name = "grpInclude";
            this.grpInclude.TabStop = false;
            // 
            // rbExcludeZero
            // 
            resources.ApplyResources(this.rbExcludeZero, "rbExcludeZero");
            this.rbExcludeZero.Name = "rbExcludeZero";
            // 
            // rbActiveOnly
            // 
            resources.ApplyResources(this.rbActiveOnly, "rbActiveOnly");
            this.rbActiveOnly.Name = "rbActiveOnly";
            // 
            // rbAll
            // 
            resources.ApplyResources(this.rbAll, "rbAll");
            this.rbAll.Name = "rbAll";
            // 
            // grpComponents
            // 
            resources.ApplyResources(this.grpComponents, "grpComponents");
            this.grpComponents.Controls.Add(this.rbWithLanded);
            this.grpComponents.Controls.Add(this.rbPurchaseOnly);
            this.grpComponents.Name = "grpComponents";
            this.grpComponents.TabStop = false;
            // 
            // rbWithLanded
            // 
            resources.ApplyResources(this.rbWithLanded, "rbWithLanded");
            this.rbWithLanded.Name = "rbWithLanded";
            // 
            // rbPurchaseOnly
            // 
            resources.ApplyResources(this.rbPurchaseOnly, "rbPurchaseOnly");
            this.rbPurchaseOnly.Name = "rbPurchaseOnly";
            // 
            // grpMethod
            // 
            resources.ApplyResources(this.grpMethod, "grpMethod");
            this.grpMethod.Controls.Add(this.rbStandard);
            this.grpMethod.Controls.Add(this.rbFifo);
            this.grpMethod.Controls.Add(this.rbWac);
            this.grpMethod.Name = "grpMethod";
            this.grpMethod.TabStop = false;
            // 
            // rbStandard
            // 
            resources.ApplyResources(this.rbStandard, "rbStandard");
            this.rbStandard.Name = "rbStandard";
            // 
            // rbFifo
            // 
            resources.ApplyResources(this.rbFifo, "rbFifo");
            this.rbFifo.Name = "rbFifo";
            // 
            // rbWac
            // 
            resources.ApplyResources(this.rbWac, "rbWac");
            this.rbWac.Name = "rbWac";
            // 
            // panelFooter
            // 
            resources.ApplyResources(this.panelFooter, "panelFooter");
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(243)))), ((int)(((byte)(242)))), ((int)(((byte)(241)))));
            this.panelFooter.Controls.Add(this.btnCancel);
            this.panelFooter.Controls.Add(this.btnSave);
            this.panelFooter.Name = "panelFooter";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // frm_valuation_settings
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelBody);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_valuation_settings";
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
