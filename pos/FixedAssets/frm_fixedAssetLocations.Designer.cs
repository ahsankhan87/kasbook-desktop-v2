namespace pos.FixedAssets
{
    partial class frm_fixedAssetLocations
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
            this.Text = "Fixed Asset Locations";
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

            this.dgvLocations = new System.Windows.Forms.DataGridView();
            this.dgvLocations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvLocations.AllowUserToAddRows = false;
            this.dgvLocations.AllowUserToDeleteRows = false;
            this.dgvLocations.ReadOnly = true;
            this.dgvLocations.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvLocations.MultiSelect = false;
            this.dgvLocations.BackgroundColor = System.Drawing.Color.White;
            this.dgvLocations.GridColor = System.Drawing.Color.FromArgb(220, 220, 220);
            this.dgvLocations.RowHeadersVisible = false;
            this.dgvLocations.DefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10);
            this.dgvLocations.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);

            this.dgvLocations.Columns.Add("LocationId", "ID");
            this.dgvLocations.Columns.Add("LocationCode", "Code");
            this.dgvLocations.Columns.Add("LocationName", "Name");
            this.dgvLocations.Columns.Add("LocationType", "Type");
            this.dgvLocations.Columns.Add("ParentLocation", "Parent");
            this.dgvLocations.Columns.Add("Status", "Status");

            this.dgvLocations.Columns[0].Width = 40;
            this.dgvLocations.Columns[1].Width = 80;
            this.dgvLocations.Columns[2].Width = 200;
            this.dgvLocations.Columns[3].Width = 100;
            this.dgvLocations.Columns[4].Width = 80;
            this.dgvLocations.Columns[5].Width = 80;

            gridPanel.Controls.Add(this.dgvLocations);

            this.Controls.Add(gridPanel);
            this.Controls.Add(toolbarPanel);

            this.ResumeLayout(false);
        }

        public System.Windows.Forms.DataGridView dgvLocations;
        public System.Windows.Forms.Button btnAdd;
        public System.Windows.Forms.Button btnEdit;
        public System.Windows.Forms.Button btnDelete;
        public System.Windows.Forms.Button btnClose;
    }
}
