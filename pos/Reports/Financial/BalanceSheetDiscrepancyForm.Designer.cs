using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Financial
{
    public partial class BalanceSheetDiscrepancyForm
    {
        private Panel panel1;
        private Label lblSummary;
        private DataGridView dgvDiagnostics;
        private Button btnClose;
        private DataGridViewTextBoxColumn colGroup;
        private DataGridViewTextBoxColumn colCurrent;
        private DataGridViewTextBoxColumn colComparison;
        private DataGridViewTextBoxColumn colDifference;
        private DataGridViewTextBoxColumn colNotes;

        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblSummary = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgvDiagnostics = new System.Windows.Forms.DataGridView();
            this.colGroup = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCurrent = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colComparison = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDifference = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colNotes = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiagnostics)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblSummary);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(12);
            this.panel1.Size = new System.Drawing.Size(900, 52);
            this.panel1.TabIndex = 0;
            // 
            // lblSummary
            // 
            this.lblSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSummary.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSummary.Location = new System.Drawing.Point(12, 12);
            this.lblSummary.Name = "lblSummary";
            this.lblSummary.Size = new System.Drawing.Size(751, 28);
            this.lblSummary.TabIndex = 0;
            this.lblSummary.Text = "Difference";
            this.lblSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnClose.Location = new System.Drawing.Point(763, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(125, 28);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgvDiagnostics
            // 
            this.dgvDiagnostics.AllowUserToAddRows = false;
            this.dgvDiagnostics.AllowUserToDeleteRows = false;
            this.dgvDiagnostics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiagnostics.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colGroup,
            this.colCurrent,
            this.colComparison,
            this.colDifference,
            this.colNotes});
            this.dgvDiagnostics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDiagnostics.Location = new System.Drawing.Point(0, 52);
            this.dgvDiagnostics.Name = "dgvDiagnostics";
            this.dgvDiagnostics.ReadOnly = true;
            this.dgvDiagnostics.RowHeadersVisible = false;
            this.dgvDiagnostics.Size = new System.Drawing.Size(900, 428);
            this.dgvDiagnostics.TabIndex = 1;
            // 
            // colGroup
            // 
            this.colGroup.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colGroup.DataPropertyName = "GroupName";
            this.colGroup.HeaderText = "Group";
            this.colGroup.Name = "colGroup";
            this.colGroup.ReadOnly = true;
            // 
            // colCurrent
            // 
            this.colCurrent.DataPropertyName = "CurrentAmount";
            this.colCurrent.HeaderText = "Current";
            this.colCurrent.Name = "colCurrent";
            this.colCurrent.ReadOnly = true;
            this.colCurrent.Width = 120;
            // 
            // colComparison
            // 
            this.colComparison.DataPropertyName = "ComparisonAmount";
            this.colComparison.HeaderText = "Comparison";
            this.colComparison.Name = "colComparison";
            this.colComparison.ReadOnly = true;
            this.colComparison.Width = 120;
            // 
            // colDifference
            // 
            this.colDifference.DataPropertyName = "Difference";
            this.colDifference.HeaderText = "Difference";
            this.colDifference.Name = "colDifference";
            this.colDifference.ReadOnly = true;
            this.colDifference.Width = 120;
            // 
            // colNotes
            // 
            this.colNotes.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colNotes.DataPropertyName = "Notes";
            this.colNotes.HeaderText = "Notes";
            this.colNotes.Name = "colNotes";
            this.colNotes.ReadOnly = true;
            // 
            // BalanceSheetDiscrepancyForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 480);
            this.Controls.Add(this.dgvDiagnostics);
            this.Controls.Add(this.panel1);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Balance Sheet Discrepancy";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiagnostics)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
