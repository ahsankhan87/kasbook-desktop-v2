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
            this.pnlMain = new System.Windows.Forms.Panel();
            this.pnlButtons = new System.Windows.Forms.Panel();
            this.txtAssetCode = new System.Windows.Forms.TextBox();
            this.lblAssetCode = new System.Windows.Forms.Label();
            this.txtAssetName = new System.Windows.Forms.TextBox();
            this.lblAssetName = new System.Windows.Forms.Label();
            this.ddlCategory = new System.Windows.Forms.ComboBox();
            this.lblCategory = new System.Windows.Forms.Label();
            this.dtPurchaseDate = new System.Windows.Forms.DateTimePicker();
            this.lblPurchaseDate = new System.Windows.Forms.Label();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.lblSerialNumber = new System.Windows.Forms.Label();
            this.txtCost = new System.Windows.Forms.TextBox();
            this.lblCost = new System.Windows.Forms.Label();
            this.ddlDeprecationMethod = new System.Windows.Forms.ComboBox();
            this.lblDeprecationMethod = new System.Windows.Forms.Label();
            this.txtUsefulLifeMonths = new System.Windows.Forms.TextBox();
            this.lblUsefulLifeMonths = new System.Windows.Forms.Label();
            this.txtSalvageValue = new System.Windows.Forms.TextBox();
            this.lblSalvageValue = new System.Windows.Forms.Label();
            this.ddlLocation = new System.Windows.Forms.ComboBox();
            this.lblLocation = new System.Windows.Forms.Label();
            this.txtReplacementCost = new System.Windows.Forms.TextBox();
            this.lblReplacementCost = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnlMain.SuspendLayout();
            this.pnlButtons.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlMain
            // 
            this.pnlMain.AutoScroll = true;
            this.pnlMain.Controls.Add(this.txtNotes);
            this.pnlMain.Controls.Add(this.lblNotes);
            this.pnlMain.Controls.Add(this.txtReplacementCost);
            this.pnlMain.Controls.Add(this.lblReplacementCost);
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
            this.pnlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlMain.Location = new System.Drawing.Point(0, 0);
            this.pnlMain.Name = "pnlMain";
            this.pnlMain.Padding = new System.Windows.Forms.Padding(10);
            this.pnlMain.Size = new System.Drawing.Size(500, 550);
            this.pnlMain.TabIndex = 0;
            // 
            // lblAssetCode
            // 
            this.lblAssetCode.Location = new System.Drawing.Point(10, 15);
            this.lblAssetCode.Name = "lblAssetCode";
            this.lblAssetCode.Size = new System.Drawing.Size(155, 23);
            this.lblAssetCode.TabIndex = 0;
            this.lblAssetCode.Text = "Asset Code:";
            // 
            // txtAssetCode
            // 
            this.txtAssetCode.Location = new System.Drawing.Point(175, 12);
            this.txtAssetCode.Name = "txtAssetCode";
            this.txtAssetCode.Size = new System.Drawing.Size(300, 24);
            this.txtAssetCode.TabIndex = 1;
            // 
            // lblAssetName
            // 
            this.lblAssetName.Location = new System.Drawing.Point(10, 45);
            this.lblAssetName.Name = "lblAssetName";
            this.lblAssetName.Size = new System.Drawing.Size(155, 23);
            this.lblAssetName.TabIndex = 2;
            this.lblAssetName.Text = "Asset Name:";
            // 
            // txtAssetName
            // 
            this.txtAssetName.Location = new System.Drawing.Point(175, 42);
            this.txtAssetName.Name = "txtAssetName";
            this.txtAssetName.Size = new System.Drawing.Size(300, 24);
            this.txtAssetName.TabIndex = 3;
            // 
            // lblCategory
            // 
            this.lblCategory.Location = new System.Drawing.Point(10, 75);
            this.lblCategory.Name = "lblCategory";
            this.lblCategory.Size = new System.Drawing.Size(155, 23);
            this.lblCategory.TabIndex = 4;
            this.lblCategory.Text = "Category:";
            // 
            // ddlCategory
            // 
            this.ddlCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlCategory.Location = new System.Drawing.Point(175, 72);
            this.ddlCategory.Name = "ddlCategory";
            this.ddlCategory.Size = new System.Drawing.Size(300, 24);
            this.ddlCategory.TabIndex = 5;
            // 
            // lblPurchaseDate
            // 
            this.lblPurchaseDate.Location = new System.Drawing.Point(10, 105);
            this.lblPurchaseDate.Name = "lblPurchaseDate";
            this.lblPurchaseDate.Size = new System.Drawing.Size(155, 23);
            this.lblPurchaseDate.TabIndex = 6;
            this.lblPurchaseDate.Text = "Purchase Date:";
            // 
            // dtPurchaseDate
            // 
            this.dtPurchaseDate.Location = new System.Drawing.Point(175, 102);
            this.dtPurchaseDate.Name = "dtPurchaseDate";
            this.dtPurchaseDate.Size = new System.Drawing.Size(300, 24);
            this.dtPurchaseDate.TabIndex = 7;
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.Location = new System.Drawing.Point(10, 135);
            this.lblSerialNumber.Name = "lblSerialNumber";
            this.lblSerialNumber.Size = new System.Drawing.Size(155, 23);
            this.lblSerialNumber.TabIndex = 8;
            this.lblSerialNumber.Text = "Serial Number:";
            // 
            // txtSerialNumber
            // 
            this.txtSerialNumber.Location = new System.Drawing.Point(175, 132);
            this.txtSerialNumber.Name = "txtSerialNumber";
            this.txtSerialNumber.Size = new System.Drawing.Size(300, 24);
            this.txtSerialNumber.TabIndex = 9;
            // 
            // lblCost
            // 
            this.lblCost.Location = new System.Drawing.Point(10, 165);
            this.lblCost.Name = "lblCost";
            this.lblCost.Size = new System.Drawing.Size(155, 23);
            this.lblCost.TabIndex = 10;
            this.lblCost.Text = "Cost (PKR):";
            // 
            // txtCost
            // 
            this.txtCost.Location = new System.Drawing.Point(175, 162);
            this.txtCost.Name = "txtCost";
            this.txtCost.Size = new System.Drawing.Size(300, 24);
            this.txtCost.TabIndex = 11;
            this.txtCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtCost.Leave += new System.EventHandler(this.TxtCost_Leave);
            // 
            // lblDeprecationMethod
            // 
            this.lblDeprecationMethod.Location = new System.Drawing.Point(10, 195);
            this.lblDeprecationMethod.Name = "lblDeprecationMethod";
            this.lblDeprecationMethod.Size = new System.Drawing.Size(155, 23);
            this.lblDeprecationMethod.TabIndex = 12;
            this.lblDeprecationMethod.Text = "Depreciation Method:";
            // 
            // ddlDeprecationMethod
            // 
            this.ddlDeprecationMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlDeprecationMethod.Location = new System.Drawing.Point(175, 192);
            this.ddlDeprecationMethod.Name = "ddlDeprecationMethod";
            this.ddlDeprecationMethod.Size = new System.Drawing.Size(300, 24);
            this.ddlDeprecationMethod.TabIndex = 13;
            // 
            // lblUsefulLifeMonths
            // 
            this.lblUsefulLifeMonths.Location = new System.Drawing.Point(10, 225);
            this.lblUsefulLifeMonths.Name = "lblUsefulLifeMonths";
            this.lblUsefulLifeMonths.Size = new System.Drawing.Size(155, 23);
            this.lblUsefulLifeMonths.TabIndex = 14;
            this.lblUsefulLifeMonths.Text = "Useful Life (Months):";
            // 
            // txtUsefulLifeMonths
            // 
            this.txtUsefulLifeMonths.Location = new System.Drawing.Point(175, 222);
            this.txtUsefulLifeMonths.Name = "txtUsefulLifeMonths";
            this.txtUsefulLifeMonths.Size = new System.Drawing.Size(300, 24);
            this.txtUsefulLifeMonths.TabIndex = 15;
            this.txtUsefulLifeMonths.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // lblSalvageValue
            // 
            this.lblSalvageValue.Location = new System.Drawing.Point(10, 255);
            this.lblSalvageValue.Name = "lblSalvageValue";
            this.lblSalvageValue.Size = new System.Drawing.Size(155, 23);
            this.lblSalvageValue.TabIndex = 16;
            this.lblSalvageValue.Text = "Salvage Value (PKR):";
            // 
            // txtSalvageValue
            // 
            this.txtSalvageValue.Location = new System.Drawing.Point(175, 252);
            this.txtSalvageValue.Name = "txtSalvageValue";
            this.txtSalvageValue.Size = new System.Drawing.Size(300, 24);
            this.txtSalvageValue.TabIndex = 17;
            this.txtSalvageValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtSalvageValue.Leave += new System.EventHandler(this.TxtSalvageValue_Leave);
            // 
            // lblLocation
            // 
            this.lblLocation.Location = new System.Drawing.Point(10, 285);
            this.lblLocation.Name = "lblLocation";
            this.lblLocation.Size = new System.Drawing.Size(155, 23);
            this.lblLocation.TabIndex = 18;
            this.lblLocation.Text = "Location:";
            // 
            // ddlLocation
            // 
            this.ddlLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ddlLocation.Location = new System.Drawing.Point(175, 282);
            this.ddlLocation.Name = "ddlLocation";
            this.ddlLocation.Size = new System.Drawing.Size(300, 24);
            this.ddlLocation.TabIndex = 19;
            // 
            // lblReplacementCost
            // 
            this.lblReplacementCost.Location = new System.Drawing.Point(10, 315);
            this.lblReplacementCost.Name = "lblReplacementCost";
            this.lblReplacementCost.Size = new System.Drawing.Size(155, 23);
            this.lblReplacementCost.TabIndex = 20;
            this.lblReplacementCost.Text = "Replacement Cost (PKR):";
            // 
            // txtReplacementCost
            // 
            this.txtReplacementCost.Location = new System.Drawing.Point(175, 312);
            this.txtReplacementCost.Name = "txtReplacementCost";
            this.txtReplacementCost.Size = new System.Drawing.Size(300, 24);
            this.txtReplacementCost.TabIndex = 21;
            this.txtReplacementCost.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtReplacementCost.Leave += new System.EventHandler(this.TxtReplacementCost_Leave);
            // 
            // lblNotes
            // 
            this.lblNotes.Location = new System.Drawing.Point(10, 345);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(155, 23);
            this.lblNotes.TabIndex = 22;
            this.lblNotes.Text = "Notes:";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(175, 342);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(300, 60);
            this.txtNotes.TabIndex = 23;
            // 
            // pnlButtons
            // 
            this.pnlButtons.Controls.Add(this.btnCancel);
            this.pnlButtons.Controls.Add(this.btnSave);
            this.pnlButtons.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlButtons.Location = new System.Drawing.Point(0, 550);
            this.pnlButtons.Name = "pnlButtons";
            this.pnlButtons.Size = new System.Drawing.Size(500, 50);
            this.pnlButtons.TabIndex = 1;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(195, 10);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(100, 30);
            this.btnSave.TabIndex = 0;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(305, 10);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // frm_addFixedAsset
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 600);
            this.Controls.Add(this.pnlMain);
            this.Controls.Add(this.pnlButtons);
            this.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_addFixedAsset";
            this.Text = "Add Fixed Asset";
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
        public System.Windows.Forms.TextBox txtReplacementCost;
        public System.Windows.Forms.TextBox txtNotes;
        public System.Windows.Forms.Button btnSave;
        public System.Windows.Forms.Button btnCancel;
    }
}
