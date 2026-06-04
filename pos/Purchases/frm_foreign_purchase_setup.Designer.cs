namespace pos
{
    partial class frm_foreign_purchase_setup
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
            this.panel_header = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.lbl_currency = new System.Windows.Forms.Label();
            this.cmb_currency = new System.Windows.Forms.ComboBox();
            this.lbl_exchange_rate = new System.Windows.Forms.Label();
            this.txt_exchange_rate = new System.Windows.Forms.TextBox();
            this.lbl_invoice_no = new System.Windows.Forms.Label();
            this.txt_invoice_no = new System.Windows.Forms.TextBox();
            this.lbl_purchase_date = new System.Windows.Forms.Label();
            this.dtp_purchase_date = new System.Windows.Forms.DateTimePicker();
            this.lbl_notes = new System.Windows.Forms.Label();
            this.txt_notes = new System.Windows.Forms.TextBox();
            this.lbl_currency_hint = new System.Windows.Forms.Label();
            this.btn_refresh_invoice = new System.Windows.Forms.Button();
            this.btn_start_purchase = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.panel_header.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel_header
            // 
            this.panel_header.Controls.Add(this.lbl_title);
            this.panel_header.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel_header.Location = new System.Drawing.Point(0, 0);
            this.panel_header.Name = "panel_header";
            this.panel_header.Size = new System.Drawing.Size(516, 48);
            this.panel_header.TabIndex = 0;
            // 
            // lbl_title
            // 
            this.lbl_title.AutoSize = true;
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Location = new System.Drawing.Point(12, 13);
            this.lbl_title.Name = "lbl_title";
            this.lbl_title.Size = new System.Drawing.Size(150, 17);
            this.lbl_title.TabIndex = 0;
            this.lbl_title.Text = "Foreign Purchase Setup";
            // 
            // lbl_currency
            // 
            this.lbl_currency.AutoSize = true;
            this.lbl_currency.Location = new System.Drawing.Point(29, 72);
            this.lbl_currency.Name = "lbl_currency";
            this.lbl_currency.Size = new System.Drawing.Size(57, 17);
            this.lbl_currency.TabIndex = 1;
            this.lbl_currency.Text = "Currency";
            // 
            // cmb_currency
            // 
            this.cmb_currency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_currency.FormattingEnabled = true;
            this.cmb_currency.Location = new System.Drawing.Point(154, 69);
            this.cmb_currency.Name = "cmb_currency";
            this.cmb_currency.Size = new System.Drawing.Size(242, 24);
            this.cmb_currency.TabIndex = 2;
            this.cmb_currency.SelectedIndexChanged += new System.EventHandler(this.cmb_currency_SelectedIndexChanged);
            // 
            // lbl_exchange_rate
            // 
            this.lbl_exchange_rate.AutoSize = true;
            this.lbl_exchange_rate.Location = new System.Drawing.Point(29, 108);
            this.lbl_exchange_rate.Name = "lbl_exchange_rate";
            this.lbl_exchange_rate.Size = new System.Drawing.Size(95, 17);
            this.lbl_exchange_rate.TabIndex = 3;
            this.lbl_exchange_rate.Text = "Exchange Rate";
            // 
            // txt_exchange_rate
            // 
            this.txt_exchange_rate.Location = new System.Drawing.Point(154, 105);
            this.txt_exchange_rate.Name = "txt_exchange_rate";
            this.txt_exchange_rate.Size = new System.Drawing.Size(141, 22);
            this.txt_exchange_rate.TabIndex = 4;
            this.txt_exchange_rate.Text = "1";
            this.txt_exchange_rate.TextChanged += new System.EventHandler(this.txt_exchange_rate_TextChanged);
            // 
            // lbl_invoice_no
            // 
            this.lbl_invoice_no.AutoSize = true;
            this.lbl_invoice_no.Location = new System.Drawing.Point(29, 144);
            this.lbl_invoice_no.Name = "lbl_invoice_no";
            this.lbl_invoice_no.Size = new System.Drawing.Size(84, 17);
            this.lbl_invoice_no.TabIndex = 5;
            this.lbl_invoice_no.Text = "Invoice No.";
            // 
            // txt_invoice_no
            // 
            this.txt_invoice_no.Location = new System.Drawing.Point(154, 141);
            this.txt_invoice_no.Name = "txt_invoice_no";
            this.txt_invoice_no.ReadOnly = true;
            this.txt_invoice_no.Size = new System.Drawing.Size(242, 22);
            this.txt_invoice_no.TabIndex = 6;
            // 
            // lbl_purchase_date
            // 
            this.lbl_purchase_date.AutoSize = true;
            this.lbl_purchase_date.Location = new System.Drawing.Point(29, 180);
            this.lbl_purchase_date.Name = "lbl_purchase_date";
            this.lbl_purchase_date.Size = new System.Drawing.Size(95, 17);
            this.lbl_purchase_date.TabIndex = 7;
            this.lbl_purchase_date.Text = "Purchase Date";
            // 
            // dtp_purchase_date
            // 
            this.dtp_purchase_date.Location = new System.Drawing.Point(154, 176);
            this.dtp_purchase_date.Name = "dtp_purchase_date";
            this.dtp_purchase_date.Size = new System.Drawing.Size(242, 22);
            this.dtp_purchase_date.TabIndex = 8;
            this.dtp_purchase_date.ValueChanged += new System.EventHandler(this.dtp_purchase_date_ValueChanged);
            // 
            // lbl_notes
            // 
            this.lbl_notes.AutoSize = true;
            this.lbl_notes.Location = new System.Drawing.Point(29, 216);
            this.lbl_notes.Name = "lbl_notes";
            this.lbl_notes.Size = new System.Drawing.Size(48, 17);
            this.lbl_notes.TabIndex = 9;
            this.lbl_notes.Text = "Notes";
            // 
            // txt_notes
            // 
            this.txt_notes.Location = new System.Drawing.Point(154, 213);
            this.txt_notes.Multiline = true;
            this.txt_notes.Name = "txt_notes";
            this.txt_notes.Size = new System.Drawing.Size(330, 70);
            this.txt_notes.TabIndex = 10;
            // 
            // lbl_currency_hint
            // 
            this.lbl_currency_hint.AutoSize = true;
            this.lbl_currency_hint.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_currency_hint.Location = new System.Drawing.Point(151, 295);
            this.lbl_currency_hint.Name = "lbl_currency_hint";
            this.lbl_currency_hint.Size = new System.Drawing.Size(155, 17);
            this.lbl_currency_hint.TabIndex = 11;
            this.lbl_currency_hint.Text = "Foreign purchase mode";
            // 
            // btn_refresh_invoice
            // 
            this.btn_refresh_invoice.Location = new System.Drawing.Point(401, 139);
            this.btn_refresh_invoice.Name = "btn_refresh_invoice";
            this.btn_refresh_invoice.Size = new System.Drawing.Size(83, 27);
            this.btn_refresh_invoice.TabIndex = 12;
            this.btn_refresh_invoice.Text = "Refresh";
            this.btn_refresh_invoice.UseVisualStyleBackColor = true;
            this.btn_refresh_invoice.Click += new System.EventHandler(this.btn_refresh_invoice_Click);
            // 
            // btn_start_purchase
            // 
            this.btn_start_purchase.Location = new System.Drawing.Point(263, 332);
            this.btn_start_purchase.Name = "btn_start_purchase";
            this.btn_start_purchase.Size = new System.Drawing.Size(139, 33);
            this.btn_start_purchase.TabIndex = 13;
            this.btn_start_purchase.Text = "Start Purchase";
            this.btn_start_purchase.UseVisualStyleBackColor = true;
            this.btn_start_purchase.Click += new System.EventHandler(this.btn_start_purchase_Click);
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(408, 332);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(82, 33);
            this.btn_cancel.TabIndex = 14;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // frm_foreign_purchase_setup
            // 
            this.AcceptButton = this.btn_start_purchase;
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(516, 381);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_start_purchase);
            this.Controls.Add(this.btn_refresh_invoice);
            this.Controls.Add(this.lbl_currency_hint);
            this.Controls.Add(this.txt_notes);
            this.Controls.Add(this.lbl_notes);
            this.Controls.Add(this.dtp_purchase_date);
            this.Controls.Add(this.lbl_purchase_date);
            this.Controls.Add(this.txt_invoice_no);
            this.Controls.Add(this.lbl_invoice_no);
            this.Controls.Add(this.txt_exchange_rate);
            this.Controls.Add(this.lbl_exchange_rate);
            this.Controls.Add(this.cmb_currency);
            this.Controls.Add(this.lbl_currency);
            this.Controls.Add(this.panel_header);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_foreign_purchase_setup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Foreign Purchase Setup";
            this.Load += new System.EventHandler(this.frm_foreign_purchase_setup_Load);
            this.panel_header.ResumeLayout(false);
            this.panel_header.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panel_header;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.Label lbl_currency;
        private System.Windows.Forms.ComboBox cmb_currency;
        private System.Windows.Forms.Label lbl_exchange_rate;
        private System.Windows.Forms.TextBox txt_exchange_rate;
        private System.Windows.Forms.Label lbl_invoice_no;
        private System.Windows.Forms.TextBox txt_invoice_no;
        private System.Windows.Forms.Label lbl_purchase_date;
        private System.Windows.Forms.DateTimePicker dtp_purchase_date;
        private System.Windows.Forms.Label lbl_notes;
        private System.Windows.Forms.TextBox txt_notes;
        private System.Windows.Forms.Label lbl_currency_hint;
        private System.Windows.Forms.Button btn_refresh_invoice;
        private System.Windows.Forms.Button btn_start_purchase;
        private System.Windows.Forms.Button btn_cancel;
    }
}
