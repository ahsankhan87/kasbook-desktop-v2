namespace pos.Expenses
{
    partial class frm_expenses
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

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_expenses));
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.grpVoucherInfo = new System.Windows.Forms.GroupBox();
            this.txtReferenceNo = new System.Windows.Forms.TextBox();
            this.lblReferenceNo = new System.Windows.Forms.Label();
            this.txtVoucherNo = new System.Windows.Forms.TextBox();
            this.lblVoucherNo = new System.Windows.Forms.Label();
            this.dtpVoucherDate = new System.Windows.Forms.DateTimePicker();
            this.lblVoucherDate = new System.Windows.Forms.Label();
            this.pnlContent = new System.Windows.Forms.Panel();
            this.grpExpenseDetails = new System.Windows.Forms.GroupBox();
            this.txtNetTotal = new System.Windows.Forms.TextBox();
            this.lblNetTotal = new System.Windows.Forms.Label();
            this.txtTaxAmount = new System.Windows.Forms.TextBox();
            this.lblTaxAmount = new System.Windows.Forms.Label();
            this.nudTaxPercent = new System.Windows.Forms.NumericUpDown();
            this.lblTaxPercent = new System.Windows.Forms.Label();
            this.cmbVatAccount = new System.Windows.Forms.ComboBox();
            this.lblVatAccount = new System.Windows.Forms.Label();
            this.btnAttachment = new System.Windows.Forms.Button();
            this.txtAttachment = new System.Windows.Forms.TextBox();
            this.lblAttachment = new System.Windows.Forms.Label();
            this.txtNarration = new System.Windows.Forms.TextBox();
            this.lblNarration = new System.Windows.Forms.Label();
            this.nudAmount = new System.Windows.Forms.NumericUpDown();
            this.lblAmount = new System.Windows.Forms.Label();
            this.cmbExpenseAccount = new System.Windows.Forms.ComboBox();
            this.lblExpenseAccount = new System.Windows.Forms.Label();
            this.grpPaymentInfo = new System.Windows.Forms.GroupBox();
            this.cmbCreditAccount = new System.Windows.Forms.ComboBox();
            this.lblCreditAccount = new System.Windows.Forms.Label();
            this.cmbPaymentMode = new System.Windows.Forms.ComboBox();
            this.lblPaymentMode = new System.Windows.Forms.Label();
            this.pnlActions = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslLastSaved = new System.Windows.Forms.ToolStripStatusLabel();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.pnlHeader.SuspendLayout();
            this.grpVoucherInfo.SuspendLayout();
            this.pnlContent.SuspendLayout();
            this.grpExpenseDetails.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTaxPercent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).BeginInit();
            this.grpPaymentInfo.SuspendLayout();
            this.pnlActions.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlHeader
            // 
            this.pnlHeader.Controls.Add(this.grpVoucherInfo);
            resources.ApplyResources(this.pnlHeader, "pnlHeader");
            this.pnlHeader.Name = "pnlHeader";
            // 
            // grpVoucherInfo
            // 
            this.grpVoucherInfo.Controls.Add(this.txtReferenceNo);
            this.grpVoucherInfo.Controls.Add(this.lblReferenceNo);
            this.grpVoucherInfo.Controls.Add(this.txtVoucherNo);
            this.grpVoucherInfo.Controls.Add(this.lblVoucherNo);
            this.grpVoucherInfo.Controls.Add(this.dtpVoucherDate);
            this.grpVoucherInfo.Controls.Add(this.lblVoucherDate);
            resources.ApplyResources(this.grpVoucherInfo, "grpVoucherInfo");
            this.grpVoucherInfo.Name = "grpVoucherInfo";
            this.grpVoucherInfo.TabStop = false;
            // 
            // txtReferenceNo
            // 
            resources.ApplyResources(this.txtReferenceNo, "txtReferenceNo");
            this.txtReferenceNo.Name = "txtReferenceNo";
            // 
            // lblReferenceNo
            // 
            resources.ApplyResources(this.lblReferenceNo, "lblReferenceNo");
            this.lblReferenceNo.Name = "lblReferenceNo";
            // 
            // txtVoucherNo
            // 
            resources.ApplyResources(this.txtVoucherNo, "txtVoucherNo");
            this.txtVoucherNo.Name = "txtVoucherNo";
            this.txtVoucherNo.ReadOnly = true;
            // 
            // lblVoucherNo
            // 
            resources.ApplyResources(this.lblVoucherNo, "lblVoucherNo");
            this.lblVoucherNo.Name = "lblVoucherNo";
            // 
            // dtpVoucherDate
            // 
            this.dtpVoucherDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.dtpVoucherDate, "dtpVoucherDate");
            this.dtpVoucherDate.Name = "dtpVoucherDate";
            // 
            // lblVoucherDate
            // 
            resources.ApplyResources(this.lblVoucherDate, "lblVoucherDate");
            this.lblVoucherDate.Name = "lblVoucherDate";
            // 
            // pnlContent
            // 
            this.pnlContent.Controls.Add(this.grpExpenseDetails);
            this.pnlContent.Controls.Add(this.grpPaymentInfo);
            this.pnlContent.Controls.Add(this.pnlActions);
            resources.ApplyResources(this.pnlContent, "pnlContent");
            this.pnlContent.Name = "pnlContent";
            // 
            // grpExpenseDetails
            // 
            this.grpExpenseDetails.Controls.Add(this.txtNetTotal);
            this.grpExpenseDetails.Controls.Add(this.lblNetTotal);
            this.grpExpenseDetails.Controls.Add(this.txtTaxAmount);
            this.grpExpenseDetails.Controls.Add(this.lblTaxAmount);
            this.grpExpenseDetails.Controls.Add(this.nudTaxPercent);
            this.grpExpenseDetails.Controls.Add(this.lblTaxPercent);
            this.grpExpenseDetails.Controls.Add(this.cmbVatAccount);
            this.grpExpenseDetails.Controls.Add(this.lblVatAccount);
            this.grpExpenseDetails.Controls.Add(this.btnAttachment);
            this.grpExpenseDetails.Controls.Add(this.txtAttachment);
            this.grpExpenseDetails.Controls.Add(this.lblAttachment);
            this.grpExpenseDetails.Controls.Add(this.txtNarration);
            this.grpExpenseDetails.Controls.Add(this.lblNarration);
            this.grpExpenseDetails.Controls.Add(this.nudAmount);
            this.grpExpenseDetails.Controls.Add(this.lblAmount);
            this.grpExpenseDetails.Controls.Add(this.cmbExpenseAccount);
            this.grpExpenseDetails.Controls.Add(this.lblExpenseAccount);
            resources.ApplyResources(this.grpExpenseDetails, "grpExpenseDetails");
            this.grpExpenseDetails.Name = "grpExpenseDetails";
            this.grpExpenseDetails.TabStop = false;
            // 
            // txtNetTotal
            // 
            resources.ApplyResources(this.txtNetTotal, "txtNetTotal");
            this.txtNetTotal.Name = "txtNetTotal";
            this.txtNetTotal.ReadOnly = true;
            this.txtNetTotal.TabStop = false;
            // 
            // lblNetTotal
            // 
            resources.ApplyResources(this.lblNetTotal, "lblNetTotal");
            this.lblNetTotal.Name = "lblNetTotal";
            // 
            // txtTaxAmount
            // 
            resources.ApplyResources(this.txtTaxAmount, "txtTaxAmount");
            this.txtTaxAmount.Name = "txtTaxAmount";
            this.txtTaxAmount.ReadOnly = true;
            this.txtTaxAmount.TabStop = false;
            // 
            // lblTaxAmount
            // 
            resources.ApplyResources(this.lblTaxAmount, "lblTaxAmount");
            this.lblTaxAmount.Name = "lblTaxAmount";
            // 
            // nudTaxPercent
            // 
            this.nudTaxPercent.DecimalPlaces = 2;
            resources.ApplyResources(this.nudTaxPercent, "nudTaxPercent");
            this.nudTaxPercent.Name = "nudTaxPercent";
            this.nudTaxPercent.ValueChanged += new System.EventHandler(this.amountOrTax_ValueChanged);
            // 
            // lblTaxPercent
            // 
            resources.ApplyResources(this.lblTaxPercent, "lblTaxPercent");
            this.lblTaxPercent.Name = "lblTaxPercent";
            // 
            // cmbVatAccount
            // 
            this.cmbVatAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbVatAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbVatAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVatAccount.FormattingEnabled = true;
            resources.ApplyResources(this.cmbVatAccount, "cmbVatAccount");
            this.cmbVatAccount.Name = "cmbVatAccount";
            // 
            // lblVatAccount
            // 
            resources.ApplyResources(this.lblVatAccount, "lblVatAccount");
            this.lblVatAccount.Name = "lblVatAccount";
            // 
            // btnAttachment
            // 
            resources.ApplyResources(this.btnAttachment, "btnAttachment");
            this.btnAttachment.Name = "btnAttachment";
            this.btnAttachment.UseVisualStyleBackColor = true;
            this.btnAttachment.Click += new System.EventHandler(this.btnAttachment_Click);
            // 
            // txtAttachment
            // 
            resources.ApplyResources(this.txtAttachment, "txtAttachment");
            this.txtAttachment.Name = "txtAttachment";
            this.txtAttachment.ReadOnly = true;
            this.txtAttachment.TabStop = false;
            // 
            // lblAttachment
            // 
            resources.ApplyResources(this.lblAttachment, "lblAttachment");
            this.lblAttachment.Name = "lblAttachment";
            // 
            // txtNarration
            // 
            resources.ApplyResources(this.txtNarration, "txtNarration");
            this.txtNarration.Name = "txtNarration";
            // 
            // lblNarration
            // 
            resources.ApplyResources(this.lblNarration, "lblNarration");
            this.lblNarration.Name = "lblNarration";
            // 
            // nudAmount
            // 
            this.nudAmount.DecimalPlaces = 2;
            resources.ApplyResources(this.nudAmount, "nudAmount");
            this.nudAmount.Maximum = new decimal(new int[] {
            1316134911,
            2328,
            0,
            0});
            this.nudAmount.Name = "nudAmount";
            this.nudAmount.ValueChanged += new System.EventHandler(this.amountOrTax_ValueChanged);
            // 
            // lblAmount
            // 
            resources.ApplyResources(this.lblAmount, "lblAmount");
            this.lblAmount.Name = "lblAmount";
            // 
            // cmbExpenseAccount
            // 
            this.cmbExpenseAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbExpenseAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbExpenseAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbExpenseAccount.FormattingEnabled = true;
            resources.ApplyResources(this.cmbExpenseAccount, "cmbExpenseAccount");
            this.cmbExpenseAccount.Name = "cmbExpenseAccount";
            // 
            // lblExpenseAccount
            // 
            resources.ApplyResources(this.lblExpenseAccount, "lblExpenseAccount");
            this.lblExpenseAccount.Name = "lblExpenseAccount";
            // 
            // grpPaymentInfo
            // 
            this.grpPaymentInfo.Controls.Add(this.cmbCreditAccount);
            this.grpPaymentInfo.Controls.Add(this.lblCreditAccount);
            this.grpPaymentInfo.Controls.Add(this.cmbPaymentMode);
            this.grpPaymentInfo.Controls.Add(this.lblPaymentMode);
            resources.ApplyResources(this.grpPaymentInfo, "grpPaymentInfo");
            this.grpPaymentInfo.Name = "grpPaymentInfo";
            this.grpPaymentInfo.TabStop = false;
            // 
            // cmbCreditAccount
            // 
            this.cmbCreditAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCreditAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbCreditAccount.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCreditAccount.FormattingEnabled = true;
            resources.ApplyResources(this.cmbCreditAccount, "cmbCreditAccount");
            this.cmbCreditAccount.Name = "cmbCreditAccount";
            // 
            // lblCreditAccount
            // 
            resources.ApplyResources(this.lblCreditAccount, "lblCreditAccount");
            this.lblCreditAccount.Name = "lblCreditAccount";
            // 
            // cmbPaymentMode
            // 
            this.cmbPaymentMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPaymentMode.FormattingEnabled = true;
            resources.ApplyResources(this.cmbPaymentMode, "cmbPaymentMode");
            this.cmbPaymentMode.Name = "cmbPaymentMode";
            this.cmbPaymentMode.SelectedIndexChanged += new System.EventHandler(this.cmbPaymentMode_SelectedIndexChanged);
            // 
            // lblPaymentMode
            // 
            resources.ApplyResources(this.lblPaymentMode, "lblPaymentMode");
            this.lblPaymentMode.Name = "lblPaymentMode";
            // 
            // pnlActions
            // 
            this.pnlActions.Controls.Add(this.btnClose);
            this.pnlActions.Controls.Add(this.btnClear);
            this.pnlActions.Controls.Add(this.btnSave);
            resources.ApplyResources(this.pnlActions, "pnlActions");
            this.pnlActions.Name = "pnlActions";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btn_close_Click);
            // 
            // btnClear
            // 
            resources.ApplyResources(this.btnClear, "btnClear");
            this.btnClear.Name = "btnClear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            resources.ApplyResources(this.btnSave, "btnSave");
            this.btnSave.Name = "btnSave";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslLastSaved});
            resources.ApplyResources(this.statusStrip1, "statusStrip1");
            this.statusStrip1.Name = "statusStrip1";
            // 
            // tslLastSaved
            // 
            this.tslLastSaved.Name = "tslLastSaved";
            resources.ApplyResources(this.tslLastSaved, "tslLastSaved");
            // 
            // openFileDialog1
            // 
            resources.ApplyResources(this.openFileDialog1, "openFileDialog1");
            // 
            // frm_expenses
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnClose;
            this.Controls.Add(this.pnlContent);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.statusStrip1);
            this.KeyPreview = true;
            this.Name = "frm_expenses";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_expenses_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_expenses_KeyDown);
            this.pnlHeader.ResumeLayout(false);
            this.grpVoucherInfo.ResumeLayout(false);
            this.grpVoucherInfo.PerformLayout();
            this.pnlContent.ResumeLayout(false);
            this.grpExpenseDetails.ResumeLayout(false);
            this.grpExpenseDetails.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudTaxPercent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudAmount)).EndInit();
            this.grpPaymentInfo.ResumeLayout(false);
            this.pnlActions.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.GroupBox grpVoucherInfo;
        private System.Windows.Forms.TextBox txtReferenceNo;
        private System.Windows.Forms.Label lblReferenceNo;
        private System.Windows.Forms.TextBox txtVoucherNo;
        private System.Windows.Forms.Label lblVoucherNo;
        private System.Windows.Forms.DateTimePicker dtpVoucherDate;
        private System.Windows.Forms.Label lblVoucherDate;
        private System.Windows.Forms.Panel pnlContent;
        private System.Windows.Forms.GroupBox grpExpenseDetails;
        private System.Windows.Forms.ComboBox cmbVatAccount;
        private System.Windows.Forms.Label lblVatAccount;
        private System.Windows.Forms.TextBox txtNetTotal;
        private System.Windows.Forms.Label lblNetTotal;
        private System.Windows.Forms.TextBox txtTaxAmount;
        private System.Windows.Forms.Label lblTaxAmount;
        private System.Windows.Forms.NumericUpDown nudTaxPercent;
        private System.Windows.Forms.Label lblTaxPercent;
        private System.Windows.Forms.Button btnAttachment;
        private System.Windows.Forms.TextBox txtAttachment;
        private System.Windows.Forms.Label lblAttachment;
        private System.Windows.Forms.TextBox txtNarration;
        private System.Windows.Forms.Label lblNarration;
        private System.Windows.Forms.NumericUpDown nudAmount;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.ComboBox cmbExpenseAccount;
        private System.Windows.Forms.Label lblExpenseAccount;
        private System.Windows.Forms.GroupBox grpPaymentInfo;
        private System.Windows.Forms.ComboBox cmbCreditAccount;
        private System.Windows.Forms.Label lblCreditAccount;
        private System.Windows.Forms.ComboBox cmbPaymentMode;
        private System.Windows.Forms.Label lblPaymentMode;
        private System.Windows.Forms.Panel pnlActions;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslLastSaved;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}