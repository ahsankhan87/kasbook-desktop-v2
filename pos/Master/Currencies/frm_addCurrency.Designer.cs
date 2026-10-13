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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_addCurrency));
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
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.lbl_header_title);
            this.panel1.Name = "panel1";
            // 
            // lbl_header_title
            // 
            resources.ApplyResources(this.lbl_header_title, "lbl_header_title");
            this.lbl_header_title.ForeColor = System.Drawing.Color.White;
            this.lbl_header_title.Name = "lbl_header_title";
            // 
            // lbl_edit_status
            // 
            resources.ApplyResources(this.lbl_edit_status, "lbl_edit_status");
            this.lbl_edit_status.Name = "lbl_edit_status";
            // 
            // btn_cancel
            // 
            resources.ApplyResources(this.btn_cancel, "btn_cancel");
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_save
            // 
            resources.ApplyResources(this.btn_save, "btn_save");
            this.btn_save.Name = "btn_save";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // chk_is_active
            // 
            resources.ApplyResources(this.chk_is_active, "chk_is_active");
            this.chk_is_active.Name = "chk_is_active";
            this.chk_is_active.UseVisualStyleBackColor = true;
            // 
            // txt_exchange_rate
            // 
            resources.ApplyResources(this.txt_exchange_rate, "txt_exchange_rate");
            this.txt_exchange_rate.Name = "txt_exchange_rate";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // txt_symbol
            // 
            resources.ApplyResources(this.txt_symbol, "txt_symbol");
            this.txt_symbol.Name = "txt_symbol";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // txt_name
            // 
            resources.ApplyResources(this.txt_name, "txt_name");
            this.txt_name.Name = "txt_name";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txt_code
            // 
            resources.ApplyResources(this.txt_code, "txt_code");
            this.txt_code.Name = "txt_code";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txt_id
            // 
            resources.ApplyResources(this.txt_id, "txt_id");
            this.txt_id.Name = "txt_id";
            this.txt_id.ReadOnly = true;
            this.txt_id.TabStop = false;
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // frm_addCurrency
            // 
            this.AcceptButton = this.btn_save;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
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
