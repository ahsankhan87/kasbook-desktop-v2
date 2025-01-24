
namespace pos
{
    partial class frm_other_stocks
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
            this.grid_other_stock = new System.Windows.Forms.DataGridView();
            this.company_name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btn_ok = new System.Windows.Forms.Button();
            this.btn_cancel = new System.Windows.Forms.Button();
            this.btn_help = new System.Windows.Forms.Button();
            this.lbl_product_name = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.grid_other_stock)).BeginInit();
            this.SuspendLayout();
            // 
            // grid_other_stock
            // 
            this.grid_other_stock.AllowUserToAddRows = false;
            this.grid_other_stock.AllowUserToDeleteRows = false;
            this.grid_other_stock.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_other_stock.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_other_stock.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.company_name,
            this.qty});
            this.grid_other_stock.Location = new System.Drawing.Point(16, 71);
            this.grid_other_stock.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.grid_other_stock.Name = "grid_other_stock";
            this.grid_other_stock.ReadOnly = true;
            this.grid_other_stock.RowHeadersWidth = 51;
            this.grid_other_stock.Size = new System.Drawing.Size(661, 390);
            this.grid_other_stock.TabIndex = 0;
            // 
            // company_name
            // 
            this.company_name.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.company_name.DataPropertyName = "company_name";
            this.company_name.HeaderText = "Company Name";
            this.company_name.MinimumWidth = 6;
            this.company_name.Name = "company_name";
            this.company_name.ReadOnly = true;
            // 
            // qty
            // 
            this.qty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.qty.DataPropertyName = "qty";
            this.qty.HeaderText = "Quantity";
            this.qty.MinimumWidth = 6;
            this.qty.Name = "qty";
            this.qty.ReadOnly = true;
            this.qty.Width = 90;
            // 
            // btn_ok
            // 
            this.btn_ok.Enabled = false;
            this.btn_ok.Location = new System.Drawing.Point(577, 492);
            this.btn_ok.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(100, 28);
            this.btn_ok.TabIndex = 1;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            // 
            // btn_cancel
            // 
            this.btn_cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btn_cancel.Location = new System.Drawing.Point(16, 492);
            this.btn_cancel.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_cancel.Name = "btn_cancel";
            this.btn_cancel.Size = new System.Drawing.Size(100, 28);
            this.btn_cancel.TabIndex = 1;
            this.btn_cancel.Text = "Cancel";
            this.btn_cancel.UseVisualStyleBackColor = true;
            this.btn_cancel.Click += new System.EventHandler(this.btn_cancel_Click);
            // 
            // btn_help
            // 
            this.btn_help.Location = new System.Drawing.Point(124, 492);
            this.btn_help.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btn_help.Name = "btn_help";
            this.btn_help.Size = new System.Drawing.Size(100, 28);
            this.btn_help.TabIndex = 1;
            this.btn_help.Text = "Help";
            this.btn_help.UseVisualStyleBackColor = true;
            // 
            // lbl_product_name
            // 
            this.lbl_product_name.AutoSize = true;
            this.lbl_product_name.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_product_name.Location = new System.Drawing.Point(145, 29);
            this.lbl_product_name.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbl_product_name.Name = "lbl_product_name";
            this.lbl_product_name.Size = new System.Drawing.Size(79, 25);
            this.lbl_product_name.TabIndex = 2;
            this.lbl_product_name.Text = "Product";
            // 
            // frm_other_stocks
            // 
            this.AcceptButton = this.btn_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btn_cancel;
            this.ClientSize = new System.Drawing.Size(723, 554);
            this.Controls.Add(this.lbl_product_name);
            this.Controls.Add(this.btn_cancel);
            this.Controls.Add(this.btn_help);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.grid_other_stock);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frm_other_stocks";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Other Stock";
            this.Load += new System.EventHandler(this.frm_other_stocks_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid_other_stock)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_other_stock;
        private System.Windows.Forms.Button btn_ok;
        private System.Windows.Forms.Button btn_cancel;
        private System.Windows.Forms.Button btn_help;
        private System.Windows.Forms.Label lbl_product_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn company_name;
        private System.Windows.Forms.DataGridViewTextBoxColumn qty;
    }
}