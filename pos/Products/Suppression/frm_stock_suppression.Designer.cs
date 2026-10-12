namespace pos.Products.Suppression
{
    partial class frm_stock_suppression
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_stock_suppression));
            this.grpOldPart = new System.Windows.Forms.GroupBox();
            this.btnSelectOldPart = new System.Windows.Forms.Button();
            this.txtOldPartCode = new System.Windows.Forms.TextBox();
            this.lblAlreadySupersededTo = new System.Windows.Forms.Label();
            this.lblOldCaption = new System.Windows.Forms.Label();
            this.grpOptions = new System.Windows.Forms.GroupBox();
            this.chkTransferAssemblies = new System.Windows.Forms.CheckBox();
            this.chkTransferPartDescription = new System.Windows.Forms.CheckBox();
            this.chkResetReorder = new System.Windows.Forms.CheckBox();
            this.chkZeroDemand = new System.Windows.Forms.CheckBox();
            this.chkTransferHistory = new System.Windows.Forms.CheckBox();
            this.chkTransferBackOrders = new System.Windows.Forms.CheckBox();
            this.chkTransferPurchaseOrders = new System.Windows.Forms.CheckBox();
            this.chkTransferStock = new System.Windows.Forms.CheckBox();
            this.btnSelectCompany = new System.Windows.Forms.Button();
            this.txtCompany = new System.Windows.Forms.TextBox();
            this.lblBranchSummary = new System.Windows.Forms.Label();
            this.grpNewPart = new System.Windows.Forms.GroupBox();
            this.btnSelectNewPart = new System.Windows.Forms.Button();
            this.txtNewPartCode = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnHelp = new System.Windows.Forms.Button();
            this.btnSupersede = new System.Windows.Forms.Button();
            this.grpOldPart.SuspendLayout();
            this.grpOptions.SuspendLayout();
            this.grpNewPart.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpOldPart
            // 
            resources.ApplyResources(this.grpOldPart, "grpOldPart");
            this.grpOldPart.Controls.Add(this.btnSelectOldPart);
            this.grpOldPart.Controls.Add(this.txtOldPartCode);
            this.grpOldPart.Controls.Add(this.lblAlreadySupersededTo);
            this.grpOldPart.Controls.Add(this.lblOldCaption);
            this.grpOldPart.Name = "grpOldPart";
            this.grpOldPart.TabStop = false;
            // 
            // btnSelectOldPart
            // 
            resources.ApplyResources(this.btnSelectOldPart, "btnSelectOldPart");
            this.btnSelectOldPart.Name = "btnSelectOldPart";
            this.btnSelectOldPart.UseVisualStyleBackColor = true;
            this.btnSelectOldPart.Click += new System.EventHandler(this.btnSelectOldPart_Click);
            // 
            // txtOldPartCode
            // 
            resources.ApplyResources(this.txtOldPartCode, "txtOldPartCode");
            this.txtOldPartCode.Name = "txtOldPartCode";
            this.txtOldPartCode.ReadOnly = true;
            // 
            // lblAlreadySupersededTo
            // 
            resources.ApplyResources(this.lblAlreadySupersededTo, "lblAlreadySupersededTo");
            this.lblAlreadySupersededTo.Name = "lblAlreadySupersededTo";
            // 
            // lblOldCaption
            // 
            resources.ApplyResources(this.lblOldCaption, "lblOldCaption");
            this.lblOldCaption.Name = "lblOldCaption";
            // 
            // grpOptions
            // 
            resources.ApplyResources(this.grpOptions, "grpOptions");
            this.grpOptions.Controls.Add(this.chkTransferAssemblies);
            this.grpOptions.Controls.Add(this.chkTransferPartDescription);
            this.grpOptions.Controls.Add(this.chkResetReorder);
            this.grpOptions.Controls.Add(this.chkZeroDemand);
            this.grpOptions.Controls.Add(this.chkTransferHistory);
            this.grpOptions.Controls.Add(this.chkTransferBackOrders);
            this.grpOptions.Controls.Add(this.chkTransferPurchaseOrders);
            this.grpOptions.Controls.Add(this.chkTransferStock);
            this.grpOptions.Controls.Add(this.btnSelectCompany);
            this.grpOptions.Controls.Add(this.txtCompany);
            this.grpOptions.Controls.Add(this.lblBranchSummary);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.TabStop = false;
            // 
            // chkTransferAssemblies
            // 
            resources.ApplyResources(this.chkTransferAssemblies, "chkTransferAssemblies");
            this.chkTransferAssemblies.Name = "chkTransferAssemblies";
            this.chkTransferAssemblies.UseVisualStyleBackColor = true;
            // 
            // chkTransferPartDescription
            // 
            resources.ApplyResources(this.chkTransferPartDescription, "chkTransferPartDescription");
            this.chkTransferPartDescription.Name = "chkTransferPartDescription";
            this.chkTransferPartDescription.UseVisualStyleBackColor = true;
            // 
            // chkResetReorder
            // 
            resources.ApplyResources(this.chkResetReorder, "chkResetReorder");
            this.chkResetReorder.Name = "chkResetReorder";
            this.chkResetReorder.UseVisualStyleBackColor = true;
            // 
            // chkZeroDemand
            // 
            resources.ApplyResources(this.chkZeroDemand, "chkZeroDemand");
            this.chkZeroDemand.Name = "chkZeroDemand";
            this.chkZeroDemand.UseVisualStyleBackColor = true;
            // 
            // chkTransferHistory
            // 
            resources.ApplyResources(this.chkTransferHistory, "chkTransferHistory");
            this.chkTransferHistory.Name = "chkTransferHistory";
            this.chkTransferHistory.UseVisualStyleBackColor = true;
            // 
            // chkTransferBackOrders
            // 
            resources.ApplyResources(this.chkTransferBackOrders, "chkTransferBackOrders");
            this.chkTransferBackOrders.Name = "chkTransferBackOrders";
            this.chkTransferBackOrders.UseVisualStyleBackColor = true;
            // 
            // chkTransferPurchaseOrders
            // 
            resources.ApplyResources(this.chkTransferPurchaseOrders, "chkTransferPurchaseOrders");
            this.chkTransferPurchaseOrders.Name = "chkTransferPurchaseOrders";
            this.chkTransferPurchaseOrders.UseVisualStyleBackColor = true;
            // 
            // chkTransferStock
            // 
            resources.ApplyResources(this.chkTransferStock, "chkTransferStock");
            this.chkTransferStock.Name = "chkTransferStock";
            this.chkTransferStock.UseVisualStyleBackColor = true;
            // 
            // btnSelectCompany
            // 
            resources.ApplyResources(this.btnSelectCompany, "btnSelectCompany");
            this.btnSelectCompany.Name = "btnSelectCompany";
            this.btnSelectCompany.UseVisualStyleBackColor = true;
            this.btnSelectCompany.Click += new System.EventHandler(this.btnSelectCompany_Click);
            // 
            // txtCompany
            // 
            resources.ApplyResources(this.txtCompany, "txtCompany");
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.ReadOnly = true;
            // 
            // lblBranchSummary
            // 
            resources.ApplyResources(this.lblBranchSummary, "lblBranchSummary");
            this.lblBranchSummary.Name = "lblBranchSummary";
            // 
            // grpNewPart
            // 
            resources.ApplyResources(this.grpNewPart, "grpNewPart");
            this.grpNewPart.Controls.Add(this.btnSelectNewPart);
            this.grpNewPart.Controls.Add(this.txtNewPartCode);
            this.grpNewPart.Name = "grpNewPart";
            this.grpNewPart.TabStop = false;
            // 
            // btnSelectNewPart
            // 
            resources.ApplyResources(this.btnSelectNewPart, "btnSelectNewPart");
            this.btnSelectNewPart.Name = "btnSelectNewPart";
            this.btnSelectNewPart.UseVisualStyleBackColor = true;
            this.btnSelectNewPart.Click += new System.EventHandler(this.btnSelectNewPart_Click);
            // 
            // txtNewPartCode
            // 
            resources.ApplyResources(this.txtNewPartCode, "txtNewPartCode");
            this.txtNewPartCode.Name = "txtNewPartCode";
            this.txtNewPartCode.ReadOnly = true;
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnHelp
            // 
            resources.ApplyResources(this.btnHelp, "btnHelp");
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnSupersede
            // 
            resources.ApplyResources(this.btnSupersede, "btnSupersede");
            this.btnSupersede.Name = "btnSupersede";
            this.btnSupersede.UseVisualStyleBackColor = true;
            this.btnSupersede.Click += new System.EventHandler(this.btnSupersede_Click);
            // 
            // frm_stock_suppression
            // 
            this.AcceptButton = this.btnSupersede;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.btnSupersede);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpNewPart);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpOldPart);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_stock_suppression";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_stock_suppression_Load);
            this.grpOldPart.ResumeLayout(false);
            this.grpOldPart.PerformLayout();
            this.grpOptions.ResumeLayout(false);
            this.grpOptions.PerformLayout();
            this.grpNewPart.ResumeLayout(false);
            this.grpNewPart.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpOldPart;
        private System.Windows.Forms.Button btnSelectOldPart;
        private System.Windows.Forms.TextBox txtOldPartCode;
        private System.Windows.Forms.Label lblAlreadySupersededTo;
        private System.Windows.Forms.Label lblOldCaption;
        private System.Windows.Forms.GroupBox grpOptions;
        private System.Windows.Forms.CheckBox chkTransferAssemblies;
        private System.Windows.Forms.CheckBox chkTransferPartDescription;
        private System.Windows.Forms.CheckBox chkResetReorder;
        private System.Windows.Forms.CheckBox chkZeroDemand;
        private System.Windows.Forms.CheckBox chkTransferHistory;
        private System.Windows.Forms.CheckBox chkTransferBackOrders;
        private System.Windows.Forms.CheckBox chkTransferPurchaseOrders;
        private System.Windows.Forms.CheckBox chkTransferStock;
        private System.Windows.Forms.Button btnSelectCompany;
        private System.Windows.Forms.TextBox txtCompany;
        private System.Windows.Forms.Label lblBranchSummary;
        private System.Windows.Forms.GroupBox grpNewPart;
        private System.Windows.Forms.Button btnSelectNewPart;
        private System.Windows.Forms.TextBox txtNewPartCode;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnHelp;
        private System.Windows.Forms.Button btnSupersede;
    }
}
