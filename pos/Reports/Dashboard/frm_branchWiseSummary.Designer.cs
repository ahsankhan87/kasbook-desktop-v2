
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dataGridViewBranchSummary = new System.Windows.Forms.DataGridView();
            this.BranchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalSales = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TotalPurchases = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBranchSummary)).BeginInit();
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
            this.dataGridViewBranchSummary.Location = new System.Drawing.Point(12, 124);
            this.dataGridViewBranchSummary.Name = "dataGridViewBranchSummary";
            this.dataGridViewBranchSummary.ReadOnly = true;
            this.dataGridViewBranchSummary.Size = new System.Drawing.Size(765, 226);
            this.dataGridViewBranchSummary.TabIndex = 10;
            // 
            // BranchName
            // 
            this.BranchName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.BranchName.DataPropertyName = "BranchName";
            this.BranchName.HeaderText = "Branch Name";
            this.BranchName.Name = "BranchName";
            this.BranchName.ReadOnly = true;
            // 
            // TotalSales
            // 
            this.TotalSales.DataPropertyName = "TotalSales";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle7.Format = "N2";
            this.TotalSales.DefaultCellStyle = dataGridViewCellStyle7;
            this.TotalSales.HeaderText = "Total Sales";
            this.TotalSales.Name = "TotalSales";
            this.TotalSales.ReadOnly = true;
            // 
            // TotalPurchases
            // 
            this.TotalPurchases.DataPropertyName = "TotalPurchases";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle8.Format = "N2";
            this.TotalPurchases.DefaultCellStyle = dataGridViewCellStyle8;
            this.TotalPurchases.HeaderText = "TotalPurchases";
            this.TotalPurchases.Name = "TotalPurchases";
            this.TotalPurchases.ReadOnly = true;
            // 
            // frm_branchWiseSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dataGridViewBranchSummary);
            this.Name = "frm_branchWiseSummary";
            this.Text = "Branch Wise Summary";
            this.Load += new System.EventHandler(this.frm_branchWiseSummary_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBranchSummary)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewBranchSummary;
        private System.Windows.Forms.DataGridViewTextBoxColumn BranchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalSales;
        private System.Windows.Forms.DataGridViewTextBoxColumn TotalPurchases;
    }
}