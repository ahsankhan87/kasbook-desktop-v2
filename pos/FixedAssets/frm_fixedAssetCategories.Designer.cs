namespace pos.FixedAssets
{
    partial class frm_fixedAssetCategories
    {
        private System.ComponentModel.IContainer components = null;

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
            this.SuspendLayout();

            // Form settings
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Text = "Fixed Asset Categories";
            this.Font = new System.Drawing.Font("Segoe UI", 10);
            this.Padding = new System.Windows.Forms.Padding(5);

            // Toolbar panel
            System.Windows.Forms.Panel toolbarPanel = new System.Windows.Forms.Panel();
            toolbarPanel.Dock = System.Windows.Forms.DockStyle.Top;
            toolbarPanel.Height = 35;
            toolbarPanel.Padding = new System.Windows.Forms.Padding(5);

            this.btnAdd = new System.Windows.Forms.Button() { Text = "[Add]", Width = 80, Height = 30, Left = 5, Top = 2 };
            this.btnAdd.Click += BtnAdd_Click;

            this.btnEdit = new System.Windows.Forms.Button() { Text = "[Edit]", Width = 80, Height = 30, Left = 90, Top = 2 };
            this.btnEdit.Click += BtnEdit_Click;

            this.btnDelete = new System.Windows.Forms.Button() { Text = "[Delete]", Width = 80, Height = 30, Left = 175, Top = 2 };
            this.btnDelete.Click += BtnDelete_Click;

            this.btnClose = new System.Windows.Forms.Button() { Text = "[Close]", Width = 80, Height = 30, Left = 260, Top = 2, DialogResult = System.Windows.Forms.DialogResult.Cancel };
            this.btnClose.Click += BtnClose_Click;

            toolbarPanel.Controls.Add(this.btnAdd);
            toolbarPanel.Controls.Add(this.btnEdit);
            toolbarPanel.Controls.Add(this.btnDelete);
            toolbarPanel.Controls.Add(this.btnClose);

            // Grid panel
            System.Windows.Forms.Panel gridPanel = new System.Windows.Forms.Panel();
            gridPanel.Dock = System.Windows.Forms.DockStyle.Fill;

            this.dgvCategories = new System.Windows.Forms.DataGridView();
            this.dgvCategories.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCategories.AllowUserToAddRows = false;
            this.dgvCategories.AllowUserToDeleteRows = false;
            this.dgvCategories.ReadOnly = true;
            this.dgvCategories.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCategories.MultiSelect = false;
            this.dgvCategories.BackgroundColor = System.Drawing.Color.White;
            this.dgvCategories.GridColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.dgvCategories.RowHeadersVisible = false;
            this.dgvCategories.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            this.dgvCategories.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);

            this.dgvCategories.Columns.Add("CategoryId", "ID");
            this.dgvCategories.Columns.Add("CategoryCode", "Code");
            this.dgvCategories.Columns.Add("CategoryName", "Name");
            this.dgvCategories.Columns.Add("DepMethod", "Deprecation Method");
            this.dgvCategories.Columns.Add("UsefulLife", "Useful Life (Months)");
            this.dgvCategories.Columns.Add("DepRate", "Annual Rate");
            this.dgvCategories.Columns.Add("Status", "Status");

            this.dgvCategories.Columns[0].Width = 40;
            this.dgvCategories.Columns[1].Width = 80;
            this.dgvCategories.Columns[2].Width = 150;
            this.dgvCategories.Columns[3].Width = 120;
            this.dgvCategories.Columns[4].Width = 80;
            this.dgvCategories.Columns[5].Width = 100;
            this.dgvCategories.Columns[6].Width = 80;

            gridPanel.Controls.Add(this.dgvCategories);

            this.Controls.Add(gridPanel);
            this.Controls.Add(toolbarPanel);

            this.ResumeLayout(false);
        }

        public System.Windows.Forms.DataGridView dgvCategories;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.Button btnEdit;
        public System.Windows.Forms.Button btnDelete;
        public System.Windows.Forms.Button btnClose;
    }
}
