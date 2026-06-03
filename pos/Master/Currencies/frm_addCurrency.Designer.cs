namespace pos
{
    partial class frm_addCurrency
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbl_header_title = new System.Windows.Forms.Label();
            this.lbl_edit_status = new System.Windows.Forms.Label();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_save = new System.Windows.Forms.Button();
            this.chk_is_active = new System.Windows.Forms.CheckBox();
            this.txt_exchange_rate = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txt_symbol = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt_name = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_code = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_id = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            this.panel1.Controls.Add(this.lbl_header_title);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(474, 50);
            this.panel1.TabIndex = 0;
            this.lbl_header_title.AutoSize = true;
            this.lbl_header_title.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold);
            this.lbl_header_title.ForeColor = System.Drawing.Color.White;
            this.lbl_header_title.Location = new System.Drawing.Point(12, 13);
            this.lbl_header_title.Name = "lbl_header_title";
            this.lbl_header_title.Size = new System.Drawing.Size(122, 25);
            this.lbl_header_title.TabIndex = 0;
            this.lbl_header_title.Text = "Add Currency";
            this.lbl_edit_status.AutoSize = true;
            this.lbl_edit_status.Location = new System.Drawing.Point(385, 67);
            this.lbl_edit_status.Name = "lbl_edit_status";
            this.lbl_edit_status.Size = new System.Drawing.Size(39, 16);
            this.lbl_edit_status.TabIndex = 13;
            this.lbl_edit_status.Text = "false";
            this.lbl_edit_status.Visible = false;
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(251, 264);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(94, 32);
            this.btn_cancel.TabIndex = 7;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            this.btn_save.Location = new System.Drawing.Point(151, 264);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(94, 32);
            this.btn_save.TabIndex = 6;
            this.btn_save.Text = "Save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            this.chk_is_active.AutoSize = true;
            this.chk_is_active.Location = new System.Drawing.Point(151, 219);
            this.chk_is_active.Name = "chk_is_active";
            this.chk_is_active.Size = new System.Drawing.Size(68, 20);
            this.chk_is_active.TabIndex = 5;
            this.chk_is_active.Text = "Active";
            this.chk_is_active.UseVisualStyleBackColor = true;
            this.txt_exchange_rate.Location = new System.Drawing.Point(151, 184);
            this.txt_exchange_rate.Name = "txt_exchange_rate";
            this.txt_exchange_rate.Size = new System.Drawing.Size(194, 22);
            this.txt_exchange_rate.TabIndex = 4;
            this.txt_exchange_rate.Text = "1";
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(39, 187);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "Exchange Rate";
            this.txt_symbol.Location = new System.Drawing.Point(151, 149);
            this.txt_symbol.Name = "txt_symbol";
            this.txt_symbol.Size = new System.Drawing.Size(194, 22);
            this.txt_symbol.TabIndex = 3;
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(84, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "Symbol";
            this.txt_name.Location = new System.Drawing.Point(151, 114);
            this.txt_name.Name = "txt_name";
            this.txt_name.Size = new System.Drawing.Size(194, 22);
            this.txt_name.TabIndex = 2;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(89, 117);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 16);
            this.label2.TabIndex = 7;
            this.label2.Text = "Name";
            this.txt_code.Location = new System.Drawing.Point(151, 79);
            this.txt_code.Name = "txt_code";
            this.txt_code.Size = new System.Drawing.Size(194, 22);
            this.txt_code.TabIndex = 1;
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 82);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "Code";
            this.txt_id.Location = new System.Drawing.Point(151, 56);
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            this.txt_id.Size = new System.Drawing.Size(194, 22);
            this.txt_id.TabIndex = 0;
            this.txt_id.TabStop = false;
            this.txt_id.Visible = false;
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(116, 59);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(17, 16);
            this.label8.TabIndex = 3;
            this.label8.Text = "Id";
            this.label8.Visible = false;
            this.AcceptButton = this.btn_save;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(474, 318);
            this.Controls.Add(this.lbl_edit_status);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.chk_is_active);
            this.Controls.Add(this.txt_exchange_rate);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txt_symbol);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txt_name);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txt_code);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_id);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_addCurrency";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Currency";
            this.Load += new System.EventHandler(this.frm_addCurrency_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_addCurrency_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbl_header_title;
        private System.Windows.Forms.Label lbl_edit_status;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_save;
        private System.Windows.Forms.CheckBox chk_is_active;
        private System.Windows.Forms.TextBox txt_exchange_rate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txt_symbol;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt_name;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_code;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_id;
        private System.Windows.Forms.Label label8;
    }
}
