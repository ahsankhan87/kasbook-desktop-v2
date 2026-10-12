
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmInventoryReport));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtLocationCode = new System.Windows.Forms.TextBox();
            this.txt_brand_code = new System.Windows.Forms.TextBox();
            this.txt_category_code = new System.Windows.Forms.TextBox();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.txt_categories = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_brands = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.Btn_generate = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.crystalReportViewer1 = new CrystalDecisions.Windows.Forms.CrystalReportViewer();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
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
            this.panel1.Name = "panel1";
            // 
            // txtLocationCode
            // 
            resources.ApplyResources(this.txtLocationCode, "txtLocationCode");
            this.txtLocationCode.Name = "txtLocationCode";
            this.txtLocationCode.ReadOnly = true;
            // 
            // txt_brand_code
            // 
            resources.ApplyResources(this.txt_brand_code, "txt_brand_code");
            this.txt_brand_code.Name = "txt_brand_code";
            this.txt_brand_code.ReadOnly = true;
            // 
            // txt_category_code
            // 
            resources.ApplyResources(this.txt_category_code, "txt_category_code");
            this.txt_category_code.Name = "txt_category_code";
            this.txt_category_code.ReadOnly = true;
            // 
            // txtLocation
            // 
            resources.ApplyResources(this.txtLocation, "txtLocation");
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_brands_KeyUp);
            this.txtLocation.Leave += new System.EventHandler(this.txt_brands_Leave);
            // 
            // txt_categories
            // 
            resources.ApplyResources(this.txt_categories, "txt_categories");
            this.txt_categories.Name = "txt_categories";
            this.txt_categories.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_categories_KeyUp);
            this.txt_categories.Leave += new System.EventHandler(this.txt_categories_Leave);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // txt_brands
            // 
            resources.ApplyResources(this.txt_brands, "txt_brands");
            this.txt_brands.Name = "txt_brands";
            this.txt_brands.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txt_brands_KeyUp);
            this.txt_brands.Leave += new System.EventHandler(this.txt_brands_Leave);
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // Btn_generate
            // 
            resources.ApplyResources(this.Btn_generate, "Btn_generate");
            this.Btn_generate.Name = "Btn_generate";
            this.Btn_generate.UseVisualStyleBackColor = true;
            this.Btn_generate.Click += new System.EventHandler(this.Btn_generate_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.crystalReportViewer1);
            this.panel2.Name = "panel2";
            // 
            // crystalReportViewer1
            // 
            resources.ApplyResources(this.crystalReportViewer1, "crystalReportViewer1");
            this.crystalReportViewer1.ActiveViewIndex = -1;
            this.crystalReportViewer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.crystalReportViewer1.Cursor = System.Windows.Forms.Cursors.Default;
            this.crystalReportViewer1.Name = "crystalReportViewer1";
            // 
            // FrmInventoryReport
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.KeyPreview = true;
            this.Name = "FrmInventoryReport";
            this.ShowIcon = false;
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