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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_adjust_qty));
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_deleteProduct = new System.Windows.Forms.Button();
            this.txt_location = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_sale_price = new System.Windows.Forms.TextBox();
            this.lbl_productCode = new System.Windows.Forms.Label();
            this.txt_cost_price = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            resources.ApplyResources(this.lblTitle, "lblTitle");
            this.lblTitle.Name = "lblTitle";
            // 
            // txtQty
            // 
            resources.ApplyResources(this.txtQty, "txtQty");
            this.txtQty.Name = "txtQty";
            this.txtQty.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQty_KeyPress);
            // 
            // btnOk
            // 
            resources.ApplyResources(this.btnOk, "btnOk");
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Name = "btnOk";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnCancel
            // 
            resources.ApplyResources(this.btnCancel, "btnCancel");
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // btn_deleteProduct
            // 
            resources.ApplyResources(this.btn_deleteProduct, "btn_deleteProduct");
            this.btn_deleteProduct.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_deleteProduct.Name = "btn_deleteProduct";
            this.btn_deleteProduct.UseVisualStyleBackColor = true;
            this.btn_deleteProduct.Click += new System.EventHandler(this.btn_deleteProduct_Click);
            // 
            // txt_location
            // 
            resources.ApplyResources(this.txt_location, "txt_location");
            this.txt_location.Name = "txt_location";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // txt_sale_price
            // 
            resources.ApplyResources(this.txt_sale_price, "txt_sale_price");
            this.txt_sale_price.Name = "txt_sale_price";
            this.txt_sale_price.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQty_KeyPress);
            // 
            // lbl_productCode
            // 
            resources.ApplyResources(this.lbl_productCode, "lbl_productCode");
            this.lbl_productCode.Name = "lbl_productCode";
            // 
            // txt_cost_price
            // 
            resources.ApplyResources(this.txt_cost_price, "txt_cost_price");
            this.txt_cost_price.Name = "txt_cost_price";
            this.txt_cost_price.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtQty_KeyPress);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // frm_adjust_qty
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.Controls.Add(this.txt_location);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_productCode);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.txt_cost_price);
            this.Controls.Add(this.txt_sale_price);
            this.Controls.Add(this.txtQty);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.btn_deleteProduct);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frm_adjust_qty";
            this.Load += new System.EventHandler(this.frm_adjust_qty_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_adjust_qty_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_deleteProduct;
        private System.Windows.Forms.TextBox txt_location;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_sale_price;
        private System.Windows.Forms.Label lbl_productCode;
        private System.Windows.Forms.TextBox txt_cost_price;
        private System.Windows.Forms.Label label3;
    }
}