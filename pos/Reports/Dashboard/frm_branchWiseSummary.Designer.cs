
namespace pos.Reports.Dashboard
{
    partial class frm_branchWiseSummary
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewBranchSummary = new System.Windows.Forms.DataGridView();
            this.BranchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalSales = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPurchases = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.lbl_total_sales = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbl_total_purchases = new System.Windows.Forms.Label();
            this.products_dataset1 = new pos.Reports.Products.Products_dataset();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBranchSummary)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.products_dataset1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewBranchSummary
            // 
            this.dataGridViewBranchSummary.AllowUserToAddRows = false;
            this.dataGridViewBranchSummary.AllowUserToDeleteRows = false;
            this.dataGridViewBranchSummary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBranchSummary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BranchName,
            this.TotalSales,
            this.TotalPurchases});
            this.dataGridViewBranchSummary.Location = new System.Drawing.Point(16, 153);
            this.dataGridViewBranchSummary.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dataGridViewBranchSummary.Name = "dataGridViewBranchSummary";
            this.dataGridViewBranchSummary.ReadOnly = true;
            this.dataGridViewBranchSummary.RowHeadersWidth = 51;
            this.dataGridViewBranchSummary.Size = new System.Drawing.Size(1020, 278);
            this.dataGridViewBranchSummary.TabIndex = 10;
            // 
            // BranchName
            // 
            this.BranchName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BranchName.DataPropertyName = "BranchName";
            this.BranchName.HeaderText = "Branch Name";
            this.BranchName.MinimumWidth = 6;
            this.BranchName.Name = "BranchName";
            this.BranchName.ReadOnly = true;
            // 
            // TotalSales
            // 
            this.TotalSales.DataPropertyName = "TotalSales";
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle9.Format = "N2";
            this.TotalSales.DefaultCellStyle = dataGridViewCellStyle9;
            this.TotalSales.HeaderText = "Total Sales";
            this.TotalSales.MinimumWidth = 6;
            this.TotalSales.Name = "TotalSales";
            this.TotalSales.ReadOnly = true;
            this.TotalSales.Width = 125;
            // 
            // TotalPurchases
            // 
            this.TotalPurchases.DataPropertyName = "TotalPurchases";
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle10.Format = "N2";
            this.TotalPurchases.DefaultCellStyle = dataGridViewCellStyle10;
            this.TotalPurchases.HeaderText = "TotalPurchases";
            this.TotalPurchases.MinimumWidth = 6;
            this.TotalPurchases.Name = "TotalPurchases";
            this.TotalPurchases.ReadOnly = true;
            this.TotalPurchases.Width = 125;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 5;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.Controls.Add(this.button1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel5, 4, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(16, 27);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1020, 100);
            this.tableLayoutPanel1.TabIndex = 11;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lbl_total_sales);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(198, 94);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Moccasin;
            this.panel2.Controls.Add(this.lbl_total_purchases);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Location = new System.Drawing.Point(207, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(198, 94);
            this.panel2.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.SkyBlue;
            this.panel3.Location = new System.Drawing.Point(411, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(198, 94);
            this.panel3.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.Color.DarkOliveGreen;
            this.panel5.Location = new System.Drawing.Point(819, 3);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(198, 94);
            this.panel5.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(3, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 27);
            this.label1.TabIndex = 0;
            this.label1.Text = "Total Sales";
            // 
            // lbl_total_sales
            // 
            this.lbl_total_sales.AutoSize = true;
            this.lbl_total_sales.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_total_sales.Location = new System.Drawing.Point(58, 61);
            this.lbl_total_sales.Name = "lbl_total_sales";
            this.lbl_total_sales.Size = new System.Drawing.Size(79, 27);
            this.lbl_total_sales.TabIndex = 0;
            this.lbl_total_sales.Text = "label1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(3, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(192, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Total Purchases";
            // 
            // lbl_total_purchases
            // 
            this.lbl_total_purchases.AutoSize = true;
            this.lbl_total_purchases.Font = new System.Drawing.Font("Arial", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl_total_purchases.Location = new System.Drawing.Point(48, 67);
            this.lbl_total_purchases.Name = "lbl_total_purchases";
            this.lbl_total_purchases.Size = new System.Drawing.Size(79, 27);
            this.lbl_total_purchases.TabIndex = 0;
            this.lbl_total_purchases.Text = "label1";
            // 
            // products_dataset1
            // 
            this.products_dataset1.DataSetName = "Products_dataset";
            this.products_dataset1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.RosyBrown;
            this.button1.Location = new System.Drawing.Point(615, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(195, 88);
            this.button1.TabIndex = 0;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // frm_branchWiseSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1067, 554);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.dataGridViewBranchSummary);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frm_branchWiseSummary";
            this.Text = "Branch Wise Summary";
            this.Load += new System.EventHandler(this.frm_branchWiseSummary_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBranchSummary)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.products_dataset1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewBranchSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn BranchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSales;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalPurchases;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label lbl_total_sales;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbl_total_purchases;
        private System.Windows.Forms.Label label2;
        private Products.Products_dataset products_dataset1;
        private System.Windows.Forms.Button button1;
    }
}