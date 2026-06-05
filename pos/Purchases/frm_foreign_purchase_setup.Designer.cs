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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_foreign_purchase_setup));
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
            resources.ApplyResources(this.panel_header, "panel_header");
            this.panel_header.Controls.Add(this.lbl_title);
            this.panel_header.Name = "panel_header";
            // 
            // lbl_title
            // 
            resources.ApplyResources(this.lbl_title, "lbl_title");
            this.lbl_title.ForeColor = System.Drawing.Color.Black;
            this.lbl_title.Name = "lbl_title";
            // 
            // lbl_currency
            // 
            resources.ApplyResources(this.lbl_currency, "lbl_currency");
            this.lbl_currency.Name = "lbl_currency";
            // 
            // cmb_currency
            // 
            resources.ApplyResources(this.cmb_currency, "cmb_currency");
            this.cmb_currency.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_currency.FormattingEnabled = true;
            this.cmb_currency.Name = "cmb_currency";
            this.cmb_currency.SelectedIndexChanged += new System.EventHandler(this.cmb_currency_SelectedIndexChanged);
            // 
            // lbl_exchange_rate
            // 
            resources.ApplyResources(this.lbl_exchange_rate, "lbl_exchange_rate");
            this.lbl_exchange_rate.Name = "lbl_exchange_rate";
            // 
            // txt_exchange_rate
            // 
            resources.ApplyResources(this.txt_exchange_rate, "txt_exchange_rate");
            this.txt_exchange_rate.Name = "txt_exchange_rate";
            this.txt_exchange_rate.TextChanged += new System.EventHandler(this.txt_exchange_rate_TextChanged);
            // 
            // lbl_invoice_no
            // 
            resources.ApplyResources(this.lbl_invoice_no, "lbl_invoice_no");
            this.lbl_invoice_no.Name = "lbl_invoice_no";
            // 
            // txt_invoice_no
            // 
            resources.ApplyResources(this.txt_invoice_no, "txt_invoice_no");
            this.txt_invoice_no.Name = "txt_invoice_no";
            this.txt_invoice_no.ReadOnly = true;
            // 
            // lbl_purchase_date
            // 
            resources.ApplyResources(this.lbl_purchase_date, "lbl_purchase_date");
            this.lbl_purchase_date.Name = "lbl_purchase_date";
            // 
            // dtp_purchase_date
            // 
            resources.ApplyResources(this.dtp_purchase_date, "dtp_purchase_date");
            this.dtp_purchase_date.Name = "dtp_purchase_date";
            this.dtp_purchase_date.ValueChanged += new System.EventHandler(this.dtp_purchase_date_ValueChanged);
            // 
            // lbl_notes
            // 
            resources.ApplyResources(this.lbl_notes, "lbl_notes");
            this.lbl_notes.Name = "lbl_notes";
            // 
            // txt_notes
            // 
            resources.ApplyResources(this.txt_notes, "txt_notes");
            this.txt_notes.Name = "txt_notes";
            // 
            // lbl_currency_hint
            // 
            resources.ApplyResources(this.lbl_currency_hint, "lbl_currency_hint");
            this.lbl_currency_hint.ForeColor = System.Drawing.Color.DimGray;
            this.lbl_currency_hint.Name = "lbl_currency_hint";
            // 
            // btn_refresh_invoice
            // 
            resources.ApplyResources(this.btn_refresh_invoice, "btn_refresh_invoice");
            this.btn_refresh_invoice.Name = "btn_refresh_invoice";
            this.btn_refresh_invoice.UseVisualStyleBackColor = true;
            this.btn_refresh_invoice.Click += new System.EventHandler(this.btn_refresh_invoice_Click);
            // 
            // btn_start_purchase
            // 
            resources.ApplyResources(this.btn_start_purchase, "btn_start_purchase");
            this.btn_start_purchase.Name = "btn_start_purchase";
            this.btn_start_purchase.UseVisualStyleBackColor = true;
            this.btn_start_purchase.Click += new System.EventHandler(this.btn_start_purchase_Click);
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // frm_foreign_purchase_setup
            // 
            this.AcceptButton = this.btn_start_purchase;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
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
