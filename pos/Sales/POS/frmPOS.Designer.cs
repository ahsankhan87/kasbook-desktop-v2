    // Designer/frmPOS.Designer.cs
    partial class frmPOS
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Panel pnlLeft;
        private System.Windows.Forms.Panel pnlRight;
        private System.Windows.Forms.Panel pnlBottom;
        private System.Windows.Forms.DataGridView dgvCart;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Label lblTotal;
        private System.Windows.Forms.Label lblTax;
        private System.Windows.Forms.Label lblSubtotal;
        private System.Windows.Forms.Button btnCheckout;
        private System.Windows.Forms.Button btnRemoveItem;
        private System.Windows.Forms.Button btnClearCart;
        private System.Windows.Forms.Button btnApplyDiscount;
        private System.Windows.Forms.TextBox txtBarcode;
        private System.Windows.Forms.ComboBox cmbPaymentMethod;
        private System.Windows.Forms.TextBox txtCustomer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPOS));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();

            // Main Form
            this.ClientSize = new System.Drawing.Size(1200, 800);
            this.Text = "SuperStore POS System";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = System.Drawing.Color.White;

            // Header Panel
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Size = new System.Drawing.Size(1200, 60);
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);

            // Left Panel (Products)
            this.pnlLeft = new System.Windows.Forms.Panel();
            this.pnlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnlLeft.Size = new System.Drawing.Size(600, 690);
            this.pnlLeft.Location = new System.Drawing.Point(0, 60);

            // Right Panel (Cart)
            this.pnlRight = new System.Windows.Forms.Panel();
            this.pnlRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Size = new System.Drawing.Size(600, 690);
            this.pnlRight.Location = new System.Drawing.Point(600, 60);

            // Bottom Panel
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlBottom.Size = new System.Drawing.Size(1200, 50);
            this.pnlBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));

            // Search TextBox
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.txtSearch.Location = new System.Drawing.Point(20, 20);
            this.txtSearch.Size = new System.Drawing.Size(560, 35);
            this.txtSearch.Font = new System.Drawing.Font("Segoe UI", 14F);
            this.txtSearch.PlaceholderText = "Search products by name, code, or barcode...";
            this.txtSearch.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

            // Products DataGridView
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.dgvProducts.Location = new System.Drawing.Point(20, 70);
            this.dgvProducts.Size = new System.Drawing.Size(560, 600);
            this.dgvProducts.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.BackgroundColor = System.Drawing.Color.White;

            // Style for Products Grid
            this.dgvProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvProducts.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvProducts.EnableHeadersVisualStyles = false;

            // Cart DataGridView
            this.dgvCart = new System.Windows.Forms.DataGridView();
            this.dgvCart.Location = new System.Drawing.Point(20, 100);
            this.dgvCart.Size = new System.Drawing.Size(560, 400);
            this.dgvCart.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            this.dgvCart.ReadOnly = true;
            this.dgvCart.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;

            // Style for Cart Grid
            this.dgvCart.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvCart.DefaultCellStyle = dataGridViewCellStyle4;

            // Labels for totals
            this.lblSubtotal = new System.Windows.Forms.Label();
            this.lblSubtotal.Location = new System.Drawing.Point(400, 520);
            this.lblSubtotal.Size = new System.Drawing.Size(180, 25);
            this.lblSubtotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSubtotal.Font = new System.Drawing.Font("Segoe UI", 12F);

            this.lblTax = new System.Windows.Forms.Label();
            this.lblTax.Location = new System.Drawing.Point(400, 550);
            this.lblTax.Size = new System.Drawing.Size(180, 25);
            this.lblTax.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTax.Font = new System.Drawing.Font("Segoe UI", 12F);

            this.lblTotal = new System.Windows.Forms.Label();
            this.lblTotal.Location = new System.Drawing.Point(400, 580);
            this.lblTotal.Size = new System.Drawing.Size(180, 30);
            this.lblTotal.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblTotal.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTotal.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));

            // Buttons
            this.btnCheckout = new System.Windows.Forms.Button();
            this.btnCheckout.Location = new System.Drawing.Point(400, 620);
            this.btnCheckout.Size = new System.Drawing.Size(180, 50);
            this.btnCheckout.Text = "CHECKOUT";
            this.btnCheckout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnCheckout.ForeColor = System.Drawing.Color.White;
            this.btnCheckout.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.btnCheckout.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnRemoveItem = new System.Windows.Forms.Button();
            this.btnRemoveItem.Location = new System.Drawing.Point(20, 620);
            this.btnRemoveItem.Size = new System.Drawing.Size(120, 50);
            this.btnRemoveItem.Text = "Remove Item";
            this.btnRemoveItem.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(244)))), ((int)(((byte)(67)))), ((int)(((byte)(54)))));
            this.btnRemoveItem.ForeColor = System.Drawing.Color.White;
            this.btnRemoveItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            this.btnClearCart = new System.Windows.Forms.Button();
            this.btnClearCart.Location = new System.Drawing.Point(150, 620);
            this.btnClearCart.Size = new System.Drawing.Size(120, 50);
            this.btnClearCart.Text = "Clear Cart";
            this.btnClearCart.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(0)))));
            this.btnClearCart.ForeColor = System.Drawing.Color.White;
            this.btnClearCart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;

            // Barcode TextBox
            this.txtBarcode = new System.Windows.Forms.TextBox();
            this.txtBarcode.Location = new System.Drawing.Point(20, 60);
            this.txtBarcode.Size = new System.Drawing.Size(300, 35);
            this.txtBarcode.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.txtBarcode.PlaceholderText = "Scan barcode...";

            // Customer TextBox
            this.txtCustomer = new System.Windows.Forms.TextBox();
            this.txtCustomer.Location = new System.Drawing.Point(120, 20);
            this.txtCustomer.Size = new System.Drawing.Size(200, 35);
            this.txtCustomer.Font = new System.Drawing.Font("Segoe UI", 10F);

            // Payment Method ComboBox
            this.cmbPaymentMethod = new System.Windows.Forms.ComboBox();
            this.cmbPaymentMethod.Location = new System.Drawing.Point(120, 60);
            this.cmbPaymentMethod.Size = new System.Drawing.Size(200, 36);
            this.cmbPaymentMethod.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.cmbPaymentMethod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            // Labels
            this.label1 = new System.Windows.Forms.Label();
            this.label1.Text = "Customer:";
            this.label1.Location = new System.Drawing.Point(20, 25);
            this.label1.Size = new System.Drawing.Size(100, 25);
            this.label1.Font = new System.Drawing.Font("Segoe UI", 10F);

            this.label2 = new System.Windows.Forms.Label();
            this.label2.Text = "Payment:";
            this.label2.Location = new System.Drawing.Point(20, 65);
            this.label2.Size = new System.Drawing.Size(100, 25);
            this.label2.Font = new System.Drawing.Font("Segoe UI", 10F);

            // Add controls to panels
            this.pnlLeft.Controls.Add(this.txtSearch);
            this.pnlLeft.Controls.Add(this.dgvProducts);

            this.pnlRight.Controls.Add(this.txtBarcode);
            this.pnlRight.Controls.Add(this.label1);
            this.pnlRight.Controls.Add(this.txtCustomer);
            this.pnlRight.Controls.Add(this.label2);
            this.pnlRight.Controls.Add(this.cmbPaymentMethod);
            this.pnlRight.Controls.Add(this.dgvCart);
            this.pnlRight.Controls.Add(this.lblSubtotal);
            this.pnlRight.Controls.Add(this.lblTax);
            this.pnlRight.Controls.Add(this.lblTotal);
            this.pnlRight.Controls.Add(this.btnRemoveItem);
            this.pnlRight.Controls.Add(this.btnClearCart);
            this.pnlRight.Controls.Add(this.btnCheckout);

            // Add panels to form
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlLeft);
            this.Controls.Add(this.pnlRight);
            this.Controls.Add(this.pnlBottom);

            // Set data grid styles
            InitializeDataGridStyles();
        }

        private void InitializeDataGridStyles()
        {
            // Products grid style
            DataGridViewCellStyle headerStyle = new DataGridViewCellStyle();
            headerStyle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(122)))), ((int)(((byte)(204)))));
            headerStyle.ForeColor = System.Drawing.Color.White;
            headerStyle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            dgvProducts.ColumnHeadersDefaultCellStyle = headerStyle;

            // Cart grid style
            dgvCart.ColumnHeadersDefaultCellStyle = headerStyle;
        }
    }