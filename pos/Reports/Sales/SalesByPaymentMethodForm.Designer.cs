using POS.BLL;
using POS.DLL;
using System.Windows.Forms;

namespace pos
{
    partial class SalesByPaymentMethodForm
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelTop;
        private Label lblStart;
        private Label lblEnd;
        private DateTimePicker dtpStart;
        private DateTimePicker dtpEnd;
        private Button btnRefresh;
        private DataGridView grid;
        private BindingSource bindingSales;
        private StatusStrip statusStrip;
        private ToolStripStatusLabel lblStatus;
        private DataGridViewTextBoxColumn colPaymentMethod;
        private DataGridViewTextBoxColumn colTransactionCount;
        private DataGridViewTextBoxColumn colTotalAmount;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.DataGridView();
            this.bindingSales = new System.Windows.Forms.BindingSource(this.components);
            this.colPaymentMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTransactionCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTotalAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSales)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblStart);
            this.panelTop.Controls.Add(this.dtpStart);
            this.panelTop.Controls.Add(this.lblEnd);
            this.panelTop.Controls.Add(this.dtpEnd);
            this.panelTop.Controls.Add(this.btnRefresh);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Padding = new System.Windows.Forms.Padding(8);
            this.panelTop.Size = new System.Drawing.Size(782, 48);
            this.panelTop.TabIndex = 1;
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(8, 16);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(37, 16);
            this.lblStart.TabIndex = 0;
            this.lblStart.Text = "Start:";
            // 
            // dtpStart
            // 
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpStart.Location = new System.Drawing.Point(45, 12);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.ShowCheckBox = true;
            this.dtpStart.Size = new System.Drawing.Size(160, 22);
            this.dtpStart.TabIndex = 1;
            // 
            // lblEnd
            // 
            this.lblEnd.AutoSize = true;
            this.lblEnd.Location = new System.Drawing.Point(205, 16);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(34, 16);
            this.lblEnd.TabIndex = 2;
            this.lblEnd.Text = "End:";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpEnd.Location = new System.Drawing.Point(239, 12);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.ShowCheckBox = true;
            this.dtpEnd.Size = new System.Drawing.Size(160, 22);
            this.dtpEnd.TabIndex = 3;
            // 
            // btnRefresh
            // 
            this.btnRefresh.Location = new System.Drawing.Point(399, 10);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(90, 27);
            this.btnRefresh.TabIndex = 4;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AllowUserToOrderColumns = true;
            this.grid.AutoGenerateColumns = false;
            this.grid.ColumnHeadersHeight = 29;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colPaymentMethod,
            this.colTransactionCount,
            this.colTotalAmount});
            this.grid.DataSource = this.bindingSales;
            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grid.Location = new System.Drawing.Point(0, 48);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.RowHeadersWidth = 51;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(782, 379);
            this.grid.TabIndex = 0;
            // 
            // bindingSales
            // 
            this.bindingSales.DataSource = typeof(POS.DLL.SalesByPaymentMethodDto);
            // 
            // colPaymentMethod
            // 
            this.colPaymentMethod.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPaymentMethod.DataPropertyName = "PaymentMethod";
            this.colPaymentMethod.HeaderText = "Payment Method";
            this.colPaymentMethod.MinimumWidth = 6;
            this.colPaymentMethod.Name = "colPaymentMethod";
            this.colPaymentMethod.ReadOnly = true;
            // 
            // colTransactionCount
            // 
            this.colTransactionCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTransactionCount.DataPropertyName = "TransactionCount";
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.colTransactionCount.DefaultCellStyle = dataGridViewCellStyle1;
            this.colTransactionCount.HeaderText = "Transactions";
            this.colTransactionCount.MinimumWidth = 6;
            this.colTransactionCount.Name = "colTransactionCount";
            this.colTransactionCount.ReadOnly = true;
            this.colTransactionCount.Width = 114;
            // 
            // colTotalAmount
            // 
            this.colTotalAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTotalAmount.DataPropertyName = "TotalAmount";
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.Format = "C2";
            this.colTotalAmount.DefaultCellStyle = dataGridViewCellStyle2;
            this.colTotalAmount.HeaderText = "Total Amount";
            this.colTotalAmount.MinimumWidth = 6;
            this.colTotalAmount.Name = "colTotalAmount";
            this.colTotalAmount.ReadOnly = true;
            this.colTotalAmount.Width = 115;
            // 
            // statusStrip
            // 
            this.statusStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblStatus});
            this.statusStrip.Location = new System.Drawing.Point(0, 427);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(782, 26);
            this.statusStrip.SizingGrip = false;
            this.statusStrip.TabIndex = 2;
            // 
            // lblStatus
            // 
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 20);
            this.lblStatus.Text = "Ready";
            // 
            // SalesByPaymentMethodForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(782, 453);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.statusStrip);
            this.Name = "SalesByPaymentMethodForm";
            this.Text = "Sales by Payment Method";
            this.Load += new System.EventHandler(this.SalesByPaymentMethodForm_Load);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSales)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}