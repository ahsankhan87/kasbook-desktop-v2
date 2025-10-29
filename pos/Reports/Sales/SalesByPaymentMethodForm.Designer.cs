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
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblStart = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.lblEnd = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.btnRefresh = new System.Windows.Forms.Button();
            this.grid = new System.Windows.Forms.DataGridView();
            this.bindingSales = new System.Windows.Forms.BindingSource(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.lblStatus = new System.Windows.Forms.ToolStripStatusLabel();
            colPaymentMethod = new DataGridViewTextBoxColumn();
            colTransactionCount = new DataGridViewTextBoxColumn();
            colTotalAmount = new DataGridViewTextBoxColumn();

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
            this.btnRefresh.Size = new System.Drawing.Size(90, 23);
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

            // Columns
            colPaymentMethod.DataPropertyName = "PaymentMethod";
            colPaymentMethod.HeaderText = "Payment Method";
            colPaymentMethod.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            colTransactionCount.DataPropertyName = "TransactionCount";
            colTransactionCount.HeaderText = "Transactions";
            colTransactionCount.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colTransactionCount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            colTotalAmount.DataPropertyName = "TotalAmount";
            colTotalAmount.HeaderText = "Total Amount";
            colTotalAmount.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            colTotalAmount.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            colTotalAmount.DefaultCellStyle.Format = "C2";

            grid.Columns.AddRange(new DataGridViewColumn[]
            {
                colPaymentMethod, colTransactionCount, colTotalAmount
            });

            // bindingSales
            bindingSales.DataSource = typeof(SalesByPaymentMethodDto);

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