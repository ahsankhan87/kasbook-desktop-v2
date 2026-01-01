namespace pos.Products.Adjustment
{
    partial class frm_adjust_qty
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // frm_adjust_qty
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 130);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_adjust_qty";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Enter Adjustment Qty";
            this.AcceptButton = this.btnOk;
            this.CancelButton = this.btnCancel;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(12, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(102, 15);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Adjustment Qty:";
            // 
            // txtQty
            // 
            this.txtQty.Location = new System.Drawing.Point(130, 17);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(150, 23);
            this.txtQty.TabIndex = 1;
            this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQty_KeyPress);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(130, 70);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(70, 28);
            this.btnOk.TabIndex = 2;
            this.btnOk.Text = "OK";
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(210, 70);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 28);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // Controls
            // 
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btnCancel);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}