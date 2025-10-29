using System.Windows.Forms;

namespace YourApp.Forms
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
            components = new System.ComponentModel.Container();
            panelTop = new Panel();
            lblStart = new Label();
            dtpStart = new DateTimePicker();
            lblEnd = new Label();
            dtpEnd = new DateTimePicker();
            btnRefresh = new Button();
            grid = new DataGridView();
            bindingSales = new BindingSource(components);
            statusStrip = new StatusStrip();
            lblStatus = new ToolStripStatusLabel();
            colPaymentMethod = new DataGridViewTextBoxColumn();
            colTransactionCount = new DataGridViewTextBoxColumn();
            colTotalAmount = new DataGridViewTextBoxColumn();

            // panelTop
            panelTop.Dock = DockStyle.Top;
            panelTop.Height = 48;
            panelTop.Padding = new Padding(8);
            panelTop.Controls.Add(lblStart);
            panelTop.Controls.Add(dtpStart);
            panelTop.Controls.Add(lblEnd);
            panelTop.Controls.Add(dtpEnd);
            panelTop.Controls.Add(btnRefresh);

            // lblStart
            lblStart.AutoSize = true;
            lblStart.Text = "Start:";
            lblStart.Left = 8;
            lblStart.Top = 16;

            // dtpStart
            dtpStart.Left = lblStart.Right + 6;
            dtpStart.Top = 12;
            dtpStart.Width = 160;
            dtpStart.Format = DateTimePickerFormat.Short;
            dtpStart.ShowCheckBox = true;

            // lblEnd
            lblEnd.AutoSize = true;
            lblEnd.Text = "End:";
            lblEnd.Left = dtpStart.Right + 16;
            lblEnd.Top = 16;

            // dtpEnd
            dtpEnd.Left = lblEnd.Right + 6;
            dtpEnd.Top = 12;
            dtpEnd.Width = 160;
            dtpEnd.Format = DateTimePickerFormat.Short;
            dtpEnd.ShowCheckBox = true;

            // btnRefresh
            btnRefresh.Text = "Refresh";
            btnRefresh.Top = 10;
            btnRefresh.Left = dtpEnd.Right + 16;
            btnRefresh.Width = 90;
            btnRefresh.Click += btnRefresh_Click;

            // grid
            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AllowUserToAddRows = false;
            grid.AllowUserToDeleteRows = false;
            grid.AllowUserToOrderColumns = true;
            grid.AutoGenerateColumns = false;
            grid.DataSource = bindingSales;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.MultiSelect = false;
            grid.RowHeadersVisible = false;

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
            bindingSales.DataSource = typeof(YourApp.Models.SalesByPaymentMethodDto);

            // statusStrip
            statusStrip.Items.Add(lblStatus);
            statusStrip.SizingGrip = false;

            // lblStatus
            lblStatus.Text = "Ready";

            // Form
            AutoScaleMode = AutoScaleMode.Font;
            Text = "Sales by Payment Method";
            Width = 800;
            Height = 500;
            Controls.Add(grid);
            Controls.Add(panelTop);
            Controls.Add(statusStrip);
            Load += SalesByPaymentMethodForm_Load;
        }
    }
}