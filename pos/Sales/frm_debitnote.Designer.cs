namespace pos.Sales
{
    partial class frm_debitnote
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        // No other changes are required for CS1061.
        private void InitializeComponent()
        {
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtDebitNoteNumber = new System.Windows.Forms.TextBox();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.txtReferenceInvoice = new System.Windows.Forms.TextBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.txtVATAmount = new System.Windows.Forms.TextBox();
            this.txtTotalAmount = new System.Windows.Forms.TextBox();
            this.txtZatcaStatus = new System.Windows.Forms.TextBox();
            this.txtZatcaMessage = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSubmitZatca = new System.Windows.Forms.Button();
            this.btnViewResponse = new System.Windows.Forms.Button();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.gridDebitNotes = new System.Windows.Forms.DataGridView();
            this.lblDebitNoteNumber = new System.Windows.Forms.Label();
            this.lbldate = new System.Windows.Forms.Label();
            this.lblCustomer = new System.Windows.Forms.Label();
            this.lblReferenceInvoice = new System.Windows.Forms.Label();
            this.lblReason = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.lblVATAmount = new System.Windows.Forms.Label();
            this.lblTotalAmount = new System.Windows.Forms.Label();
            this.lblZatcaStatus = new System.Windows.Forms.Label();
            this.lblZatcaMessage = new System.Windows.Forms.Label();
            this.cmbReason = new System.Windows.Forms.ComboBox();
            this.cmb_customers = new System.Windows.Forms.ComboBox();
            this.lbl_subtype_name = new System.Windows.Forms.Label();
            this.lbl_subtype_code = new System.Windows.Forms.Label();
            this.lbl_saletype = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.gridDebitNotes)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(320, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(150, 32);
            this.lblTitle.TabIndex = 10;
            this.lblTitle.Text = "Debit Notes";
            // 
            // txtDebitNoteNumber
            // 
            this.txtDebitNoteNumber.Location = new System.Drawing.Point(30, 70);
            this.txtDebitNoteNumber.Name = "txtDebitNoteNumber";
            this.txtDebitNoteNumber.ReadOnly = true;
            this.txtDebitNoteNumber.Size = new System.Drawing.Size(200, 22);
            this.txtDebitNoteNumber.TabIndex = 11;
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(250, 70);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(200, 22);
            this.dtpDate.TabIndex = 1;
            // 
            // txtReferenceInvoice
            // 
            this.txtReferenceInvoice.Location = new System.Drawing.Point(30, 120);
            this.txtReferenceInvoice.Name = "txtReferenceInvoice";
            this.txtReferenceInvoice.Size = new System.Drawing.Size(200, 22);
            this.txtReferenceInvoice.TabIndex = 3;
            this.txtReferenceInvoice.TextChanged += new System.EventHandler(this.txtReferenceInvoice_TextChanged);
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(30, 169);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(120, 22);
            this.txtAmount.TabIndex = 5;
            this.txtAmount.TextChanged += new System.EventHandler(this.txtAmount_TextChanged);
            // 
            // txtVATAmount
            // 
            this.txtVATAmount.Location = new System.Drawing.Point(170, 169);
            this.txtVATAmount.Name = "txtVATAmount";
            this.txtVATAmount.ReadOnly = true;
            this.txtVATAmount.Size = new System.Drawing.Size(120, 22);
            this.txtVATAmount.TabIndex = 6;
            // 
            // txtTotalAmount
            // 
            this.txtTotalAmount.Location = new System.Drawing.Point(310, 169);
            this.txtTotalAmount.Name = "txtTotalAmount";
            this.txtTotalAmount.ReadOnly = true;
            this.txtTotalAmount.Size = new System.Drawing.Size(120, 22);
            this.txtTotalAmount.TabIndex = 7;
            // 
            // txtZatcaStatus
            // 
            this.txtZatcaStatus.Location = new System.Drawing.Point(450, 169);
            this.txtZatcaStatus.Name = "txtZatcaStatus";
            this.txtZatcaStatus.ReadOnly = true;
            this.txtZatcaStatus.Size = new System.Drawing.Size(120, 22);
            this.txtZatcaStatus.TabIndex = 19;
            // 
            // txtZatcaMessage
            // 
            this.txtZatcaMessage.Location = new System.Drawing.Point(590, 169);
            this.txtZatcaMessage.Name = "txtZatcaMessage";
            this.txtZatcaMessage.ReadOnly = true;
            this.txtZatcaMessage.Size = new System.Drawing.Size(200, 22);
            this.txtZatcaMessage.TabIndex = 20;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(30, 209);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(120, 32);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSubmitZatca
            // 
            this.btnSubmitZatca.Enabled = false;
            this.btnSubmitZatca.Location = new System.Drawing.Point(670, 209);
            this.btnSubmitZatca.Name = "btnSubmitZatca";
            this.btnSubmitZatca.Size = new System.Drawing.Size(120, 32);
            this.btnSubmitZatca.TabIndex = 11;
            this.btnSubmitZatca.Text = "Submit to ZATCA";
            this.btnSubmitZatca.Click += new System.EventHandler(this.btnSubmitZatca_Click);
            // 
            // btnViewResponse
            // 
            this.btnViewResponse.Enabled = false;
            this.btnViewResponse.Location = new System.Drawing.Point(544, 209);
            this.btnViewResponse.Name = "btnViewResponse";
            this.btnViewResponse.Size = new System.Drawing.Size(120, 32);
            this.btnViewResponse.TabIndex = 10;
            this.btnViewResponse.Text = "View Response";
            this.btnViewResponse.Click += new System.EventHandler(this.btnViewResponse_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(418, 209);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(120, 32);
            this.btnRefresh.TabIndex = 9;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // gridDebitNotes
            // 
            this.gridDebitNotes.ColumnHeadersHeight = 29;
            this.gridDebitNotes.Location = new System.Drawing.Point(30, 258);
            this.gridDebitNotes.Name = "gridDebitNotes";
            this.gridDebitNotes.ReadOnly = true;
            this.gridDebitNotes.RowHeadersWidth = 51;
            this.gridDebitNotes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridDebitNotes.Size = new System.Drawing.Size(760, 250);
            this.gridDebitNotes.TabIndex = 25;
            // 
            // lblDebitNoteNumber
            // 
            this.lblDebitNoteNumber.AutoSize = true;
            this.lblDebitNoteNumber.Location = new System.Drawing.Point(30, 50);
            this.lblDebitNoteNumber.Name = "lblDebitNoteNumber";
            this.lblDebitNoteNumber.Size = new System.Drawing.Size(125, 16);
            this.lblDebitNoteNumber.TabIndex = 0;
            this.lblDebitNoteNumber.Text = "Debit Note Number:";
            // 
            // lbldate
            // 
            this.lbldate.AutoSize = true;
            this.lbldate.Location = new System.Drawing.Point(250, 50);
            this.lbldate.Name = "lbldate";
            this.lbldate.Size = new System.Drawing.Size(39, 16);
            this.lbldate.TabIndex = 1;
            this.lbldate.Text = "Date:";
            // 
            // lblCustomer
            // 
            this.lblCustomer.AutoSize = true;
            this.lblCustomer.Location = new System.Drawing.Point(470, 50);
            this.lblCustomer.Name = "lblCustomer";
            this.lblCustomer.Size = new System.Drawing.Size(67, 16);
            this.lblCustomer.TabIndex = 2;
            this.lblCustomer.Text = "Customer:";
            // 
            // lblReferenceInvoice
            // 
            this.lblReferenceInvoice.AutoSize = true;
            this.lblReferenceInvoice.Location = new System.Drawing.Point(30, 100);
            this.lblReferenceInvoice.Name = "lblReferenceInvoice";
            this.lblReferenceInvoice.Size = new System.Drawing.Size(112, 16);
            this.lblReferenceInvoice.TabIndex = 3;
            this.lblReferenceInvoice.Text = "Original Invoice #:";
            // 
            // lblReason
            // 
            this.lblReason.AutoSize = true;
            this.lblReason.Location = new System.Drawing.Point(250, 100);
            this.lblReason.Name = "lblReason";
            this.lblReason.Size = new System.Drawing.Size(58, 16);
            this.lblReason.TabIndex = 4;
            this.lblReason.Text = "Reason:";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(30, 149);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(55, 16);
            this.lblAmount.TabIndex = 5;
            this.lblAmount.Text = "Amount:";
            // 
            // lblVATAmount
            // 
            this.lblVATAmount.AutoSize = true;
            this.lblVATAmount.Location = new System.Drawing.Point(170, 149);
            this.lblVATAmount.Name = "lblVATAmount";
            this.lblVATAmount.Size = new System.Drawing.Size(85, 16);
            this.lblVATAmount.TabIndex = 6;
            this.lblVATAmount.Text = "VAT Amount:";
            // 
            // lblTotalAmount
            // 
            this.lblTotalAmount.AutoSize = true;
            this.lblTotalAmount.Location = new System.Drawing.Point(310, 149);
            this.lblTotalAmount.Name = "lblTotalAmount";
            this.lblTotalAmount.Size = new System.Drawing.Size(89, 16);
            this.lblTotalAmount.TabIndex = 7;
            this.lblTotalAmount.Text = "Total Amount:";
            // 
            // lblZatcaStatus
            // 
            this.lblZatcaStatus.AutoSize = true;
            this.lblZatcaStatus.Location = new System.Drawing.Point(450, 149);
            this.lblZatcaStatus.Name = "lblZatcaStatus";
            this.lblZatcaStatus.Size = new System.Drawing.Size(94, 16);
            this.lblZatcaStatus.TabIndex = 8;
            this.lblZatcaStatus.Text = "ZATCA Status:";
            // 
            // lblZatcaMessage
            // 
            this.lblZatcaMessage.AutoSize = true;
            this.lblZatcaMessage.Location = new System.Drawing.Point(590, 149);
            this.lblZatcaMessage.Name = "lblZatcaMessage";
            this.lblZatcaMessage.Size = new System.Drawing.Size(114, 16);
            this.lblZatcaMessage.TabIndex = 9;
            this.lblZatcaMessage.Text = "ZATCA Message:";
            // 
            // cmbReason
            // 
            this.cmbReason.FormattingEnabled = true;
            this.cmbReason.Location = new System.Drawing.Point(250, 120);
            this.cmbReason.Name = "cmbReason";
            this.cmbReason.Size = new System.Drawing.Size(420, 24);
            this.cmbReason.TabIndex = 4;
            // 
            // cmb_customers
            // 
            this.cmb_customers.FormattingEnabled = true;
            this.cmb_customers.Location = new System.Drawing.Point(473, 70);
            this.cmb_customers.Name = "cmb_customers";
            this.cmb_customers.Size = new System.Drawing.Size(197, 24);
            this.cmb_customers.TabIndex = 2;
            // 
            // lbl_subtype_name
            // 
            this.lbl_subtype_name.AutoSize = true;
            this.lbl_subtype_name.Location = new System.Drawing.Point(145, 15);
            this.lbl_subtype_name.Name = "lbl_subtype_name";
            this.lbl_subtype_name.Size = new System.Drawing.Size(117, 16);
            this.lbl_subtype_name.TabIndex = 0;
            this.lbl_subtype_name.Text = "lbl_subtype_name";
            // 
            // lbl_subtype_code
            // 
            this.lbl_subtype_code.AutoSize = true;
            this.lbl_subtype_code.Location = new System.Drawing.Point(30, 15);
            this.lbl_subtype_code.Name = "lbl_subtype_code";
            this.lbl_subtype_code.Size = new System.Drawing.Size(109, 16);
            this.lbl_subtype_code.TabIndex = 0;
            this.lbl_subtype_code.Text = "lbl sub type code";
            // 
            // lbl_saletype
            // 
            this.lbl_saletype.AutoSize = true;
            this.lbl_saletype.Location = new System.Drawing.Point(728, 9);
            this.lbl_saletype.Name = "lbl_saletype";
            this.lbl_saletype.Size = new System.Drawing.Size(80, 16);
            this.lbl_saletype.TabIndex = 0;
            this.lbl_saletype.Text = "lbl_saletype";
            this.lbl_saletype.Visible = false;
            // 
            // frm_debitnote
            // 
            this.ClientSize = new System.Drawing.Size(820, 520);
            this.Controls.Add(this.cmb_customers);
            this.Controls.Add(this.cmbReason);
            this.Controls.Add(this.lbl_subtype_code);
            this.Controls.Add(this.lbl_saletype);
            this.Controls.Add(this.lbl_subtype_name);
            this.Controls.Add(this.lblDebitNoteNumber);
            this.Controls.Add(this.lbldate);
            this.Controls.Add(this.lblCustomer);
            this.Controls.Add(this.lblReferenceInvoice);
            this.Controls.Add(this.lblReason);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.lblVATAmount);
            this.Controls.Add(this.lblTotalAmount);
            this.Controls.Add(this.lblZatcaStatus);
            this.Controls.Add(this.lblZatcaMessage);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtDebitNoteNumber);
            this.Controls.Add(this.dtpDate);
            this.Controls.Add(this.txtReferenceInvoice);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.txtVATAmount);
            this.Controls.Add(this.txtTotalAmount);
            this.Controls.Add(this.txtZatcaStatus);
            this.Controls.Add(this.txtZatcaMessage);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnSubmitZatca);
            this.Controls.Add(this.btnViewResponse);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.gridDebitNotes);
            this.Name = "frm_debitnote";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Debit Note Management";
            this.Load += new System.EventHandler(this.frm_debitnote_Load);
            ((System.ComponentModel.ISupportInitialize)(this.gridDebitNotes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblDebitNoteNumber;
        private System.Windows.Forms.Label lbldate;
        private System.Windows.Forms.Label lblCustomer;
        private System.Windows.Forms.Label lblReferenceInvoice;
        private System.Windows.Forms.Label lblReason;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label lblVATAmount;
        private System.Windows.Forms.Label lblTotalAmount;
        private System.Windows.Forms.Label lblZatcaStatus;
        private System.Windows.Forms.Label lblZatcaMessage;


        private System.Windows.Forms.TextBox txtDebitNoteNumber, txtReferenceInvoice, txtAmount, txtVATAmount, txtTotalAmount, txtZatcaStatus, txtZatcaMessage;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Button btnSave, btnSubmitZatca, btnViewResponse, btnRefresh;
        private System.Windows.Forms.DataGridView gridDebitNotes;
        private System.Windows.Forms.ComboBox cmbReason;
        private System.Windows.Forms.ComboBox cmb_customers;
        private System.Windows.Forms.Label lbl_subtype_name;
        private System.Windows.Forms.Label lbl_subtype_code;
        private System.Windows.Forms.Label lbl_saletype;
    }
}