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
            this.grpOldPart.Controls.Add(this.btnSelectOldPart);
            this.grpOldPart.Controls.Add(this.txtOldPartCode);
            this.grpOldPart.Controls.Add(this.lblAlreadySupersededTo);
            this.grpOldPart.Controls.Add(this.lblOldCaption);
            this.grpOldPart.Location = new System.Drawing.Point(14, 15);
            this.grpOldPart.Margin = new System.Windows.Forms.Padding(4);
            this.grpOldPart.Name = "grpOldPart";
            this.grpOldPart.Padding = new System.Windows.Forms.Padding(4);
            this.grpOldPart.Size = new System.Drawing.Size(498, 117);
            this.grpOldPart.TabIndex = 0;
            this.grpOldPart.TabStop = false;
            this.grpOldPart.Text = "Old part number";
            // 
            // btnSelectOldPart
            // 
            this.btnSelectOldPart.Location = new System.Drawing.Point(382, 41);
            this.btnSelectOldPart.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectOldPart.Name = "btnSelectOldPart";
            this.btnSelectOldPart.Size = new System.Drawing.Size(103, 30);
            this.btnSelectOldPart.TabIndex = 1;
            this.btnSelectOldPart.Text = "Search";
            this.btnSelectOldPart.UseVisualStyleBackColor = true;
            this.btnSelectOldPart.Click += new System.EventHandler(this.btnSelectOldPart_Click);
            // 
            // txtOldPartCode
            // 
            this.txtOldPartCode.Location = new System.Drawing.Point(14, 43);
            this.txtOldPartCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtOldPartCode.Name = "txtOldPartCode";
            this.txtOldPartCode.ReadOnly = true;
            this.txtOldPartCode.Size = new System.Drawing.Size(360, 24);
            this.txtOldPartCode.TabIndex = 0;
            // 
            // lblAlreadySupersededTo
            // 
            this.lblAlreadySupersededTo.AutoSize = true;
            this.lblAlreadySupersededTo.Location = new System.Drawing.Point(14, 89);
            this.lblAlreadySupersededTo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAlreadySupersededTo.Name = "lblAlreadySupersededTo";
            this.lblAlreadySupersededTo.Size = new System.Drawing.Size(0, 17);
            this.lblAlreadySupersededTo.TabIndex = 3;
            // 
            // lblOldCaption
            // 
            this.lblOldCaption.AutoSize = true;
            this.lblOldCaption.Location = new System.Drawing.Point(14, 22);
            this.lblOldCaption.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblOldCaption.Name = "lblOldCaption";
            this.lblOldCaption.Size = new System.Drawing.Size(0, 17);
            this.lblOldCaption.TabIndex = 2;
            // 
            // grpOptions
            // 
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
            this.grpOptions.Location = new System.Drawing.Point(14, 139);
            this.grpOptions.Margin = new System.Windows.Forms.Padding(4);
            this.grpOptions.Name = "grpOptions";
            this.grpOptions.Padding = new System.Windows.Forms.Padding(4);
            this.grpOptions.Size = new System.Drawing.Size(498, 289);
            this.grpOptions.TabIndex = 1;
            this.grpOptions.TabStop = false;
            this.grpOptions.Text = "Options";
            // 
            // chkTransferAssemblies
            // 
            this.chkTransferAssemblies.AutoSize = true;
            this.chkTransferAssemblies.Location = new System.Drawing.Point(18, 205);
            this.chkTransferAssemblies.Margin = new System.Windows.Forms.Padding(4);
            this.chkTransferAssemblies.Name = "chkTransferAssemblies";
            this.chkTransferAssemblies.Size = new System.Drawing.Size(172, 21);
            this.chkTransferAssemblies.TabIndex = 7;
            this.chkTransferAssemblies.Text = "Transfer assemblies/kits";
            this.chkTransferAssemblies.UseVisualStyleBackColor = true;
            // 
            // chkTransferPartDescription
            // 
            this.chkTransferPartDescription.AutoSize = true;
            this.chkTransferPartDescription.Location = new System.Drawing.Point(18, 179);
            this.chkTransferPartDescription.Margin = new System.Windows.Forms.Padding(4);
            this.chkTransferPartDescription.Name = "chkTransferPartDescription";
            this.chkTransferPartDescription.Size = new System.Drawing.Size(179, 21);
            this.chkTransferPartDescription.TabIndex = 6;
            this.chkTransferPartDescription.Text = "Transfer part description";
            this.chkTransferPartDescription.UseVisualStyleBackColor = true;
            // 
            // chkResetReorder
            // 
            this.chkResetReorder.AutoSize = true;
            this.chkResetReorder.Location = new System.Drawing.Point(18, 153);
            this.chkResetReorder.Margin = new System.Windows.Forms.Padding(4);
            this.chkResetReorder.Name = "chkResetReorder";
            this.chkResetReorder.Size = new System.Drawing.Size(189, 21);
            this.chkResetReorder.TabIndex = 5;
            this.chkResetReorder.Text = "Reset re-order on old part";
            this.chkResetReorder.UseVisualStyleBackColor = true;
            // 
            // chkZeroDemand
            // 
            this.chkZeroDemand.AutoSize = true;
            this.chkZeroDemand.Location = new System.Drawing.Point(18, 127);
            this.chkZeroDemand.Margin = new System.Windows.Forms.Padding(4);
            this.chkZeroDemand.Name = "chkZeroDemand";
            this.chkZeroDemand.Size = new System.Drawing.Size(218, 21);
            this.chkZeroDemand.TabIndex = 4;
            this.chkZeroDemand.Text = "Zero demand quantity old part";
            this.chkZeroDemand.UseVisualStyleBackColor = true;
            // 
            // chkTransferHistory
            // 
            this.chkTransferHistory.AutoSize = true;
            this.chkTransferHistory.Location = new System.Drawing.Point(18, 101);
            this.chkTransferHistory.Margin = new System.Windows.Forms.Padding(4);
            this.chkTransferHistory.Name = "chkTransferHistory";
            this.chkTransferHistory.Size = new System.Drawing.Size(126, 21);
            this.chkTransferHistory.TabIndex = 3;
            this.chkTransferHistory.Text = "Transfer history";
            this.chkTransferHistory.UseVisualStyleBackColor = true;
            // 
            // chkTransferBackOrders
            // 
            this.chkTransferBackOrders.AutoSize = true;
            this.chkTransferBackOrders.Location = new System.Drawing.Point(18, 75);
            this.chkTransferBackOrders.Margin = new System.Windows.Forms.Padding(4);
            this.chkTransferBackOrders.Name = "chkTransferBackOrders";
            this.chkTransferBackOrders.Size = new System.Drawing.Size(156, 21);
            this.chkTransferBackOrders.TabIndex = 2;
            this.chkTransferBackOrders.Text = "Transfer back orders";
            this.chkTransferBackOrders.UseVisualStyleBackColor = true;
            // 
            // chkTransferPurchaseOrders
            // 
            this.chkTransferPurchaseOrders.AutoSize = true;
            this.chkTransferPurchaseOrders.Location = new System.Drawing.Point(18, 49);
            this.chkTransferPurchaseOrders.Margin = new System.Windows.Forms.Padding(4);
            this.chkTransferPurchaseOrders.Name = "chkTransferPurchaseOrders";
            this.chkTransferPurchaseOrders.Size = new System.Drawing.Size(177, 21);
            this.chkTransferPurchaseOrders.TabIndex = 1;
            this.chkTransferPurchaseOrders.Text = "Transfer purchase order";
            this.chkTransferPurchaseOrders.UseVisualStyleBackColor = true;
            // 
            // chkTransferStock
            // 
            this.chkTransferStock.AutoSize = true;
            this.chkTransferStock.Location = new System.Drawing.Point(18, 23);
            this.chkTransferStock.Margin = new System.Windows.Forms.Padding(4);
            this.chkTransferStock.Name = "chkTransferStock";
            this.chkTransferStock.Size = new System.Drawing.Size(117, 21);
            this.chkTransferStock.TabIndex = 0;
            this.chkTransferStock.Text = "Transfer stock";
            this.chkTransferStock.UseVisualStyleBackColor = true;
            // 
            // btnSelectCompany
            // 
            this.btnSelectCompany.Location = new System.Drawing.Point(350, 241);
            this.btnSelectCompany.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectCompany.Name = "btnSelectCompany";
            this.btnSelectCompany.Size = new System.Drawing.Size(135, 28);
            this.btnSelectCompany.TabIndex = 10;
            this.btnSelectCompany.Text = "Load Branches";
            this.btnSelectCompany.UseVisualStyleBackColor = true;
            this.btnSelectCompany.Click += new System.EventHandler(this.btnSelectCompany_Click);
            // 
            // txtCompany
            // 
            this.txtCompany.Location = new System.Drawing.Point(18, 244);
            this.txtCompany.Margin = new System.Windows.Forms.Padding(4);
            this.txtCompany.Name = "txtCompany";
            this.txtCompany.ReadOnly = true;
            this.txtCompany.Size = new System.Drawing.Size(324, 24);
            this.txtCompany.TabIndex = 9;
            // 
            // lblBranchSummary
            // 
            this.lblBranchSummary.AutoSize = true;
            this.lblBranchSummary.Location = new System.Drawing.Point(14, 272);
            this.lblBranchSummary.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBranchSummary.Name = "lblBranchSummary";
            this.lblBranchSummary.Size = new System.Drawing.Size(116, 17);
            this.lblBranchSummary.TabIndex = 8;
            this.lblBranchSummary.Text = "0 branch selected";
            // 
            // grpNewPart
            // 
            this.grpNewPart.Controls.Add(this.btnSelectNewPart);
            this.grpNewPart.Controls.Add(this.txtNewPartCode);
            this.grpNewPart.Location = new System.Drawing.Point(14, 436);
            this.grpNewPart.Margin = new System.Windows.Forms.Padding(4);
            this.grpNewPart.Name = "grpNewPart";
            this.grpNewPart.Padding = new System.Windows.Forms.Padding(4);
            this.grpNewPart.Size = new System.Drawing.Size(498, 85);
            this.grpNewPart.TabIndex = 2;
            this.grpNewPart.TabStop = false;
            this.grpNewPart.Text = "New part number";
            // 
            // btnSelectNewPart
            // 
            this.btnSelectNewPart.Location = new System.Drawing.Point(382, 32);
            this.btnSelectNewPart.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectNewPart.Name = "btnSelectNewPart";
            this.btnSelectNewPart.Size = new System.Drawing.Size(103, 30);
            this.btnSelectNewPart.TabIndex = 1;
            this.btnSelectNewPart.Text = "Search";
            this.btnSelectNewPart.UseVisualStyleBackColor = true;
            this.btnSelectNewPart.Click += new System.EventHandler(this.btnSelectNewPart_Click);
            // 
            // txtNewPartCode
            // 
            this.txtNewPartCode.Location = new System.Drawing.Point(14, 34);
            this.txtNewPartCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtNewPartCode.Name = "txtNewPartCode";
            this.txtNewPartCode.ReadOnly = true;
            this.txtNewPartCode.Size = new System.Drawing.Size(360, 24);
            this.txtNewPartCode.TabIndex = 0;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(14, 533);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(93, 33);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnHelp
            // 
            this.btnHelp.Location = new System.Drawing.Point(114, 533);
            this.btnHelp.Margin = new System.Windows.Forms.Padding(4);
            this.btnHelp.Name = "btnHelp";
            this.btnHelp.Size = new System.Drawing.Size(93, 33);
            this.btnHelp.TabIndex = 4;
            this.btnHelp.Text = "Help";
            this.btnHelp.UseVisualStyleBackColor = true;
            this.btnHelp.Click += new System.EventHandler(this.btnHelp_Click);
            // 
            // btnSupersede
            // 
            this.btnSupersede.Location = new System.Drawing.Point(419, 533);
            this.btnSupersede.Margin = new System.Windows.Forms.Padding(4);
            this.btnSupersede.Name = "btnSupersede";
            this.btnSupersede.Size = new System.Drawing.Size(93, 33);
            this.btnSupersede.TabIndex = 5;
            this.btnSupersede.Text = "Supersede";
            this.btnSupersede.UseVisualStyleBackColor = true;
            this.btnSupersede.Click += new System.EventHandler(this.btnSupersede_Click);
            // 
            // frm_stock_suppression
            // 
            this.AcceptButton = this.btnSupersede;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(527, 578);
            this.Controls.Add(this.btnSupersede);
            this.Controls.Add(this.btnHelp);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.grpNewPart);
            this.Controls.Add(this.grpOptions);
            this.Controls.Add(this.grpOldPart);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_stock_suppression";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Stock Suppressions";
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
