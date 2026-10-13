namespace pos
{
    partial class frm_currencies
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_currencies));
            this.panel1 = new System.Windows.Forms.Panel();
            this.txt_search = new System.Windows.Forms.TextBox();
            this.btn_search = new System.Windows.Forms.Button();
            this.btn_refresh = new System.Windows.Forms.Button();
            this.btn_delete = new System.Windows.Forms.Button();
            this.btn_update = new System.Windows.Forms.Button();
            this.grid_currencies = new System.Windows.Forms.DataGridView();
            this.btn_new = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbl_title = new System.Windows.Forms.Label();
            this.id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.symbol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.exchange_rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.is_active = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.date_created = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_currencies)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.txt_search);
            this.panel1.Controls.Add(this.btn_search);
            this.panel1.Controls.Add(this.btn_refresh);
            this.panel1.Controls.Add(this.btn_delete);
            this.panel1.Controls.Add(this.btn_update);
            this.panel1.Controls.Add(this.grid_currencies);
            this.panel1.Controls.Add(this.btn_new);
            this.panel1.Name = "panel1";
            // 
            // txt_search
            // 
            resources.ApplyResources(this.txt_search, "txt_search");
            this.txt_search.Name = "txt_search";
            this.txt_search.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txt_search_KeyPress);
            // 
            // btn_search
            // 
            resources.ApplyResources(this.btn_search, "btn_search");
            this.btn_search.Name = "btn_search";
            this.btn_search.UseVisualStyleBackColor = true;
            this.btn_search.Click += new System.EventHandler(this.btn_search_Click);
            // 
            // btn_refresh
            // 
            resources.ApplyResources(this.btn_refresh, "btn_refresh");
            this.btn_refresh.Name = "btn_refresh";
            this.btn_refresh.UseVisualStyleBackColor = true;
            this.btn_refresh.Click += new System.EventHandler(this.btn_refresh_Click);
            // 
            // btn_delete
            // 
            resources.ApplyResources(this.btn_delete, "btn_delete");
            this.btn_delete.Name = "btn_delete";
            this.btn_delete.UseVisualStyleBackColor = true;
            this.btn_delete.Click += new System.EventHandler(this.btn_delete_Click);
            // 
            // btn_update
            // 
            resources.ApplyResources(this.btn_update, "btn_update");
            this.btn_update.Name = "btn_update";
            this.btn_update.UseVisualStyleBackColor = true;
            this.btn_update.Click += new System.EventHandler(this.btn_update_Click);
            // 
            // grid_currencies
            // 
            resources.ApplyResources(this.grid_currencies, "grid_currencies");
            this.grid_currencies.AllowUserToAddRows = false;
            this.grid_currencies.AllowUserToDeleteRows = false;
            this.grid_currencies.AllowUserToOrderColumns = true;
            this.grid_currencies.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid_currencies.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_currencies.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.id,
            this.code,
            this.name,
            this.symbol,
            this.exchange_rate,
            this.is_active,
            this.date_created});
            this.grid_currencies.Name = "grid_currencies";
            this.grid_currencies.ReadOnly = true;
            this.grid_currencies.RowTemplate.Height = 24;
            this.grid_currencies.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // btn_new
            // 
            resources.ApplyResources(this.btn_new, "btn_new");
            this.btn_new.Name = "btn_new";
            this.btn_new.UseVisualStyleBackColor = true;
            this.btn_new.Click += new System.EventHandler(this.btn_new_Click);
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.lbl_title);
            this.panel2.Name = "panel2";
            // 
            // lbl_title
            // 
            resources.ApplyResources(this.lbl_title, "lbl_title");
            this.lbl_title.ForeColor = System.Drawing.Color.White;
            this.lbl_title.Name = "lbl_title";
            // 
            // id
            // 
            this.id.DataPropertyName = "id";
            resources.ApplyResources(this.id, "id");
            this.id.Name = "id";
            this.id.ReadOnly = true;
            // 
            // code
            // 
            this.code.DataPropertyName = "code";
            resources.ApplyResources(this.code, "code");
            this.code.Name = "code";
            this.code.ReadOnly = true;
            // 
            // name
            // 
            this.name.DataPropertyName = "name";
            resources.ApplyResources(this.name, "name");
            this.name.Name = "name";
            this.name.ReadOnly = true;
            // 
            // symbol
            // 
            this.symbol.DataPropertyName = "symbol";
            resources.ApplyResources(this.symbol, "symbol");
            this.symbol.Name = "symbol";
            this.symbol.ReadOnly = true;
            // 
            // exchange_rate
            // 
            this.exchange_rate.DataPropertyName = "exchange_rate";
            resources.ApplyResources(this.exchange_rate, "exchange_rate");
            this.exchange_rate.Name = "exchange_rate";
            this.exchange_rate.ReadOnly = true;
            // 
            // is_active
            // 
            this.is_active.DataPropertyName = "is_active";
            resources.ApplyResources(this.is_active, "is_active");
            this.is_active.Name = "is_active";
            this.is_active.ReadOnly = true;
            // 
            // date_created
            // 
            this.date_created.DataPropertyName = "date_created";
            resources.ApplyResources(this.date_created, "date_created");
            this.date_created.Name = "date_created";
            this.date_created.ReadOnly = true;
            // 
            // frm_currencies
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.KeyPreview = true;
            this.Name = "frm_currencies";
            this.ShowIcon = false;
            this.Load += new System.EventHandler(this.frm_currencies_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_currencies_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_currencies)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txt_search;
        private System.Windows.Forms.Button btn_search;
        private System.Windows.Forms.Button btn_refresh;
        private System.Windows.Forms.Button btn_delete;
        private System.Windows.Forms.Button btn_update;
        private System.Windows.Forms.DataGridView grid_currencies;
        private System.Windows.Forms.Button btn_new;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbl_title;
        private System.Windows.Forms.DataGridViewTextBoxColumn id;
        private System.Windows.Forms.DataGridViewTextBoxColumn code;
        private System.Windows.Forms.DataGridViewTextBoxColumn name;
        private System.Windows.Forms.DataGridViewTextBoxColumn symbol;
        private System.Windows.Forms.DataGridViewTextBoxColumn exchange_rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn is_active;
        private System.Windows.Forms.DataGridViewTextBoxColumn date_created;
    }
}
