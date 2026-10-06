namespace pos.FixedAssets
{
    partial class frm_addFixedAsset
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_addFixedAsset));
            this.pnlMain = new System.Windows.Forms.Panel();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.txtReplacementCost = new System.Windows.Forms.TextBox();
            this.lblReplacementCost = new System.Windows.Forms.Label();
            this.ddlCostCenter = new System.Windows.Forms.ComboBox();
            this.lblCostCenter = new System.Windows.Forms.Label();
            this.ddlLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtSalvageValue = new System.Windows.Forms.TextBox();
            this.lblSalvageValue = new System.Windows.Forms.Label();
            this.txtUsefulLifeMonths = new System.Windows.Forms.TextBox();
            this.lblUsefulLifeMonths = new System.Windows.Forms.Label();
            this.ddlDeprecationMethod = new System.Windows.Forms.ComboBox();
            this.lblDeprecationMethod = new System.Windows.Forms.Label();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.lblCost = new System.Windows.Forms.Label();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.dtPurchaseDate = new System.Windows.Forms.DateTimePicker();
            this.lblPurchaseDate = new System.Windows.Forms.Label();
            this.ddlCategory = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.txtAssetName = new System.Windows.Forms.TextBox();
            this.lblAssetName = new System.Windows.Forms.Label();
            this.txtAssetCode = new System.Windows.Forms.TextBox();
            this.lblAssetCode = new System.Windows.Forms.Label();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlMain.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            resources.ApplyResources(this.pnlMain, "pnlMain");
            this.pnlMain.Controls.Add(this.txtNotes);
            this.pnlMain.Controls.Add(this.lblNotes);
            this.pnlMain.Controls.Add(this.txtReplacementCost);
            this.pnlMain.Controls.Add(this.lblReplacementCost);
            this.pnlMain.Controls.Add(this.ddlCostCenter);
            this.pnlMain.Controls.Add(this.lblCostCenter);
            this.pnlMain.Controls.Add(this.ddlLocation);
            this.pnlMain.Controls.Add(this.lblLocation);
            this.pnlMain.Controls.Add(this.txtSalvageValue);
            this.pnlMain.Controls.Add(this.lblSalvageValue);
            this.pnlMain.Controls.Add(this.txtUsefulLifeMonths);
            this.pnlMain.Controls.Add(this.lblUsefulLifeMonths);
            this.pnlMain.Controls.Add(this.ddlDeprecationMethod);
            this.pnlMain.Controls.Add(this.lblDeprecationMethod);
            this.pnlMain.Controls.Add(this.txtCost);
            this.pnlMain.Controls.Add(this.lblCost);
            this.pnlMain.Controls.Add(this.txtSerialNumber);
            this.pnlMain.Controls.Add(this.lblSerialNumber);
            this.pnlMain.Controls.Add(this.dtPurchaseDate);
            this.pnlMain.Controls.Add(this.lblPurchaseDate);
            this.pnlMain.Controls.Add(this.ddlCategory);
            this.pnlMain.Controls.Add(this.lblCategory);
            this.pnlMain.Controls.Add(this.txtAssetName);
            this.pnlMain.Controls.Add(this.lblAssetName);
            this.pnlMain.Controls.Add(this.txtAssetCode);
            this.pnlMain.Controls.Add(this.lblAssetCode);
            this.pnlMain.Name = "pnlMain";
            // 
            // txtNotes
            // 
            resources.ApplyResources(this.txtNotes, "txtNotes");
            this.txtNotes.Name = "txtNotes";
            // 
            // lblNotes
            // 
            resources.ApplyResources(this.lblNotes, "lblNotes");
            this.lblNotes.Name = "lblNotes";
            // 
            // txtReplacementCost
            // 
            resources.ApplyResources(this.txtReplacementCost, "txtReplacementCost");
            this.txtReplacementCost.Name = "txtReplacementCost";
            this.txtReplacementCost.Leave += new System.EventHandler(this.TxtReplacementCost_Leave);
            // 
            // lblReplacementCost
            // 
            resources.ApplyResources(this.lblReplacementCost, "lblReplacementCost");
            this.lblReplacementCost.Name = "lblReplacementCost";
            // 
            // ddlCostCenter
            // 
            resources.ApplyResources(this.ddlCostCenter, "ddlCostCenter");
            this.ddlCostCenter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCostCenter.Name = "ddlCostCenter";
            // 
            // lblCostCenter
            // 
            resources.ApplyResources(this.lblCostCenter, "lblCostCenter");
            this.lblCostCenter.Name = "lblCostCenter";
            // 
            // ddlLocation
            // 
            resources.ApplyResources(this.ddlLocation, "ddlLocation");
            this.ddlLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLocation.Name = "ddlLocation";
            // 
            // lblLocation
            // 
            resources.ApplyResources(this.lblLocation, "lblLocation");
            this.lblLocation.Name = "lblLocation";
            // 
            // txtSalvageValue
            // 
            resources.ApplyResources(this.txtSalvageValue, "txtSalvageValue");
            this.txtSalvageValue.Name = "txtSalvageValue";
            this.txtSalvageValue.Leave += new System.EventHandler(this.TxtSalvageValue_Leave);
            // 
            // lblSalvageValue
            // 
            resources.ApplyResources(this.lblSalvageValue, "lblSalvageValue");
            this.lblSalvageValue.Name = "lblSalvageValue";
            // 
            // txtUsefulLifeMonths
            // 
            resources.ApplyResources(this.txtUsefulLifeMonths, "txtUsefulLifeMonths");
            this.txtUsefulLifeMonths.Name = "txtUsefulLifeMonths";
            this.txtUsefulLifeMonths.TextChanged += new System.EventHandler(this.txtUsefulLifeMonths_TextChanged);
            // 
            // lblUsefulLifeMonths
            // 
            resources.ApplyResources(this.lblUsefulLifeMonths, "lblUsefulLifeMonths");
            this.lblUsefulLifeMonths.Name = "lblUsefulLifeMonths";
            // 
            // ddlDeprecationMethod
            // 
            resources.ApplyResources(this.ddlDeprecationMethod, "ddlDeprecationMethod");
            this.ddlDeprecationMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDeprecationMethod.Name = "ddlDeprecationMethod";
            // 
            // lblDeprecationMethod
            // 
            resources.ApplyResources(this.lblDeprecationMethod, "lblDeprecationMethod");
            this.lblDeprecationMethod.Name = "lblDeprecationMethod";
            // 
            // txtCost
            // 
            resources.ApplyResources(this.txtCost, "txtCost");
            this.txtCost.Name = "txtCost";
            this.txtCost.Leave += new System.EventHandler(this.TxtCost_Leave);
            // 
            // lblCost
            // 
            resources.ApplyResources(this.lblCost, "lblCost");
            this.lblCost.Name = "lblCost";
            // 
            // txtSerialNumber
            // 
            resources.ApplyResources(this.txtSerialNumber, "txtSerialNumber");
            this.txtSerialNumber.Name = "txtSerialNumber";
            // 
            // lblSerialNumber
            // 
            resources.ApplyResources(this.lblSerialNumber, "lblSerialNumber");
            this.lblSerialNumber.Name = "lblSerialNumber";
            // 
            // dtPurchaseDate
            // 
            resources.ApplyResources(this.dtPurchaseDate, "dtPurchaseDate");
            this.dtPurchaseDate.Name = "dtPurchaseDate";
            // 
            // lblPurchaseDate
            // 
            resources.ApplyResources(this.lblPurchaseDate, "lblPurchaseDate");
            this.lblPurchaseDate.Name = "lblPurchaseDate";
            // 
            // ddlCategory
            // 
            resources.ApplyResources(this.ddlCategory, "ddlCategory");
            this.ddlCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategory.Name = "ddlCategory";
            // 
            // lblCategory
            // 
            resources.ApplyResources(this.lblCategory, "lblCategory");
            this.lblCategory.Name = "lblCategory";
            // 
            // txtAssetName
            // 
            resources.ApplyResources(this.txtAssetName, "txtAssetName");
            this.txtAssetName.Name = "txtAssetName";
            // 
            // lblAssetName
            // 
            resources.ApplyResources(this.lblAssetName, "lblAssetName");
            this.lblAssetName.Name = "lblAssetName";
            // 
            // txtAssetCode
            // 
            resources.ApplyResources(this.txtAssetCode, "txtAssetCode");
            this.txtAssetCode.Name = "txtAssetCode";
            // 
            // lblAssetCode
            // 
            resources.ApplyResources(this.lblAssetCode, "lblAssetCode");
            this.lblAssetCode.Name = "lblAssetCode";
            // 
            // pnlButtons
            // 
            resources.ApplyResources(this.pnlButtons, "pnlButtons");
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Name = "pnlButtons";
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // frm_addFixedAsset
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtons);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_addFixedAsset";
            this.Load += new System.EventHandler(this.frm_addFixedAsset_Load);
            this.pnlMain.ResumeLayout(false);
            this.pnlMain.PerformLayout();
            this.pnlButtons.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        // Controls
        private System.Windows.Forms.Panel pnlMain;
        private System.Windows.Forms.Panel pnlButtons;
        private System.Windows.Forms.Label lblAssetCode;
        private System.Windows.Forms.Label lblAssetName;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblPurchaseDate;
        private System.Windows.Forms.Label lblSerialNumber;
        private System.Windows.Forms.Label lblCost;
        private System.Windows.Forms.Label lblDeprecationMethod;
        private System.Windows.Forms.Label lblUsefulLifeMonths;
        private System.Windows.Forms.Label lblSalvageValue;
        private System.Windows.Forms.Label lblLocation;
        private System.Windows.Forms.Label lblCostCenter;
        private System.Windows.Forms.Label lblReplacementCost;
        private System.Windows.Forms.Label lblNotes;
        public System.Windows.Forms.TextBox txtAssetCode;
        public System.Windows.Forms.TextBox txtAssetName;
        public System.Windows.Forms.ComboBox ddlCategory;
        public System.Windows.Forms.DateTimePicker dtPurchaseDate;
        public System.Windows.Forms.TextBox txtSerialNumber;
        public System.Windows.Forms.TextBox txtCost;
        public System.Windows.Forms.ComboBox ddlDeprecationMethod;
        public System.Windows.Forms.TextBox txtUsefulLifeMonths;
        public System.Windows.Forms.TextBox txtSalvageValue;
        public System.Windows.Forms.ComboBox ddlLocation;
        public System.Windows.Forms.ComboBox ddlCostCenter;
        public System.Windows.Forms.TextBox txtReplacementCost;
        public System.Windows.Forms.TextBox txtNotes;
        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnCancel;
    }
}
