
namespace pos.Reports.Products.Inventory
{
    partial class FrmInventoryReport
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_brand_code = new System.Windows.Forms.TextBox();
            this.txt_category_code = new System.Windows.Forms.TextBox();
            this.txt_categories = new System.Windows.Forms.TextBox();
            this.txt_brands = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Btn_generate = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.label1 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.txtLocationCode = new System.Windows.Forms.TextBox();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtLocationCode);
            this.panel1.Controls.Add(this.txt_brand_code);
            this.panel1.Controls.Add(this.txt_category_code);
            this.panel1.Controls.Add(this.txtLocation);
            this.panel1.Controls.Add(this.txt_categories);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txt_brands);
            this.panel1.Controls.Add(this.label18);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.Btn_generate);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(869, 100);
            this.panel1.TabIndex = 0;
            // 
            // txt_brand_code
            // 
            this.txt_brand_code.Location = new System.Drawing.Point(284, 43);
            this.txt_brand_code.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_brand_code.Name = "txt_brand_code";
            this.txt_brand_code.ReadOnly = true;
            this.txt_brand_code.Size = new System.Drawing.Size(37, 22);
            this.txt_brand_code.TabIndex = 26;
            // 
            // txt_category_code
            // 
            this.txt_category_code.Location = new System.Drawing.Point(284, 17);
            this.txt_category_code.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_category_code.Name = "txt_category_code";
            this.txt_category_code.ReadOnly = true;
            this.txt_category_code.Size = new System.Drawing.Size(37, 22);
            this.txt_category_code.TabIndex = 25;
            // 
            // txt_categories
            // 
            this.txt_categories.Location = new System.Drawing.Point(108, 17);
            this.txt_categories.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_categories.Name = "txt_categories";
            this.txt_categories.Size = new System.Drawing.Size(176, 22);
            this.txt_categories.TabIndex = 21;
            this.txt_categories.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_categories_KeyUp);
            this.txt_categories.Leave += new System.EventHandler(this.txt_categories_Leave);
            // 
            // txt_brands
            // 
            this.txt_brands.Location = new System.Drawing.Point(108, 43);
            this.txt_brands.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt_brands.Name = "txt_brands";
            this.txt_brands.Size = new System.Drawing.Size(176, 22);
            this.txt_brands.TabIndex = 24;
            this.txt_brands.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_brands_KeyUp);
            this.txt_brands.Leave += new System.EventHandler(this.txt_brands_Leave);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(32, 46);
            this.label18.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(57, 17);
            this.label18.TabIndex = 27;
            this.label18.Text = "Brands:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(32, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 17);
            this.label7.TabIndex = 28;
            this.label7.Text = "Category:";
            // 
            // Btn_generate
            // 
            this.Btn_generate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_generate.Location = new System.Drawing.Point(693, 17);
            this.Btn_generate.Name = "Btn_generate";
            this.Btn_generate.Size = new System.Drawing.Size(164, 46);
            this.Btn_generate.TabIndex = 0;
            this.Btn_generate.Text = "Generate Report (F3)";
            this.Btn_generate.UseVisualStyleBackColor = true;
            this.Btn_generate.Click += new System.EventHandler(this.Btn_generate_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.crystalReportViewer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(869, 476);
            this.panel2.TabIndex = 0;
            // 
            // crystalReportViewer1
            // 
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.crystalReportViewer1.Location = new System.Drawing.Point(0, 0);
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            this.crystalReportViewer1.Size = new System.Drawing.Size(869, 476);
            this.crystalReportViewer1.TabIndex = 0;
            this.crystalReportViewer1.ToolPanelView = CrystalDecisions.Windows.Forms.ToolPanelViewType.None;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(32, 72);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 27;
            this.label1.Text = "Locations:";
            // 
            // txtLocation
            // 
            this.txtLocation.Location = new System.Drawing.Point(108, 69);
            this.txtLocation.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.Size = new System.Drawing.Size(176, 22);
            this.txtLocation.TabIndex = 24;
            this.txtLocation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_brands_KeyUp);
            this.txtLocation.Leave += new System.EventHandler(this.txt_brands_Leave);
            // 
            // txtLocationCode
            // 
            this.txtLocationCode.Location = new System.Drawing.Point(284, 69);
            this.txtLocationCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLocationCode.Name = "txtLocationCode";
            this.txtLocationCode.ReadOnly = true;
            this.txtLocationCode.Size = new System.Drawing.Size(37, 22);
            this.txtLocationCode.TabIndex = 26;
            // 
            // FrmInventoryReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(869, 576);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "FrmInventoryReport";
            this.ShowIcon = false;
            this.Text = "Inventory Report (Qty on hand)";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmInventoryReport_1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FrmInventoryReport_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button Btn_generate;
        private System.Windows.Forms.Panel panel2;
        private CrystalDecisions.Windows.Forms.CrystalReportViewer crystalReportViewer1;
        private System.Windows.Forms.TextBox txt_brand_code;
        private System.Windows.Forms.TextBox txt_category_code;
        private System.Windows.Forms.TextBox txt_categories;
        private System.Windows.Forms.TextBox txt_brands;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtLocationCode;
        private System.Windows.Forms.TextBox txtLocation;
        private System.Windows.Forms.Label label1;
    }
}