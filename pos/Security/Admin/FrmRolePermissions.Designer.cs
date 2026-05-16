using System.Windows.Forms;

namespace pos.Security.Admin
{
    partial class FrmRolePermissions
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelHeader;
        private Label lblTitle;
        private GroupBox grpRole;
        private Label lblRole;
        private ComboBox cmbRoles;
        private GroupBox grpFilters;
        private Label lblSearch;
        private TextBox txtSearch;
        private Label lblModule;
        private ComboBox cmbModuleFilter;
        private GroupBox grpPermissions;
        private CheckedListBox checkedListBoxPermissions;
        private Button btnCheckAll;
        private Button btnUncheckAll;
        private Panel panelFooter;
        private Label lblSelectedCount;
        private Button btnSave;
        private Button btnClose;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.grpRole = new System.Windows.Forms.GroupBox();
            this.lblRole = new System.Windows.Forms.Label();
            this.cmbRoles = new System.Windows.Forms.ComboBox();
            this.grpFilters = new System.Windows.Forms.GroupBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.lblModule = new System.Windows.Forms.Label();
            this.cmbModuleFilter = new System.Windows.Forms.ComboBox();
            this.grpPermissions = new System.Windows.Forms.GroupBox();
            this.checkedListBoxPermissions = new System.Windows.Forms.CheckedListBox();
            this.btnCheckAll = new System.Windows.Forms.Button();
            this.btnUncheckAll = new System.Windows.Forms.Button();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.lblSelectedCount = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.panelHeader.SuspendLayout();
            this.grpRole.SuspendLayout();
            this.grpFilters.SuspendLayout();
            this.grpPermissions.SuspendLayout();
            this.panelFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(784, 52);
            this.panelHeader.TabIndex = 3;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblTitle.Location = new System.Drawing.Point(12, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(165, 28);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Role Permissions";
            // 
            // grpRole
            // 
            this.grpRole.Controls.Add(this.lblRole);
            this.grpRole.Controls.Add(this.cmbRoles);
            this.grpRole.Location = new System.Drawing.Point(12, 60);
            this.grpRole.Name = "grpRole";
            this.grpRole.Size = new System.Drawing.Size(760, 62);
            this.grpRole.TabIndex = 2;
            this.grpRole.TabStop = false;
            this.grpRole.Text = "Role";
            // 
            // lblRole
            // 
            this.lblRole.AutoSize = true;
            this.lblRole.Location = new System.Drawing.Point(14, 29);
            this.lblRole.Name = "lblRole";
            this.lblRole.Size = new System.Drawing.Size(79, 17);
            this.lblRole.TabIndex = 0;
            this.lblRole.Text = "Select Role:";
            // 
            // cmbRoles
            // 
            this.cmbRoles.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRoles.Location = new System.Drawing.Point(95, 24);
            this.cmbRoles.Name = "cmbRoles";
            this.cmbRoles.Size = new System.Drawing.Size(280, 24);
            this.cmbRoles.TabIndex = 1;
            this.cmbRoles.SelectedIndexChanged += new System.EventHandler(this.cmbRoles_SelectedIndexChanged);
            // 
            // grpFilters
            // 
            this.grpFilters.Controls.Add(this.lblSearch);
            this.grpFilters.Controls.Add(this.txtSearch);
            this.grpFilters.Controls.Add(this.lblModule);
            this.grpFilters.Controls.Add(this.cmbModuleFilter);
            this.grpFilters.Location = new System.Drawing.Point(12, 128);
            this.grpFilters.Name = "grpFilters";
            this.grpFilters.Size = new System.Drawing.Size(760, 72);
            this.grpFilters.TabIndex = 1;
            this.grpFilters.TabStop = false;
            this.grpFilters.Text = "Find Permissions";
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(14, 32);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(55, 17);
            this.lblSearch.TabIndex = 0;
            this.lblSearch.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(70, 27);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(300, 24);
            this.txtSearch.TabIndex = 1;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // lblModule
            // 
            this.lblModule.AutoSize = true;
            this.lblModule.Location = new System.Drawing.Point(390, 32);
            this.lblModule.Name = "lblModule";
            this.lblModule.Size = new System.Drawing.Size(56, 17);
            this.lblModule.TabIndex = 2;
            this.lblModule.Text = "Module:";
            // 
            // cmbModuleFilter
            // 
            this.cmbModuleFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbModuleFilter.Location = new System.Drawing.Point(450, 27);
            this.cmbModuleFilter.Name = "cmbModuleFilter";
            this.cmbModuleFilter.Size = new System.Drawing.Size(280, 24);
            this.cmbModuleFilter.TabIndex = 3;
            this.cmbModuleFilter.SelectedIndexChanged += new System.EventHandler(this.cmbModuleFilter_SelectedIndexChanged);
            // 
            // grpPermissions
            // 
            this.grpPermissions.Controls.Add(this.checkedListBoxPermissions);
            this.grpPermissions.Controls.Add(this.btnCheckAll);
            this.grpPermissions.Controls.Add(this.btnUncheckAll);
            this.grpPermissions.Location = new System.Drawing.Point(12, 206);
            this.grpPermissions.Name = "grpPermissions";
            this.grpPermissions.Size = new System.Drawing.Size(760, 314);
            this.grpPermissions.TabIndex = 0;
            this.grpPermissions.TabStop = false;
            this.grpPermissions.Text = "Permissions";
            // 
            // checkedListBoxPermissions
            // 
            this.checkedListBoxPermissions.CheckOnClick = true;
            this.checkedListBoxPermissions.Location = new System.Drawing.Point(14, 26);
            this.checkedListBoxPermissions.Name = "checkedListBoxPermissions";
            this.checkedListBoxPermissions.Size = new System.Drawing.Size(730, 232);
            this.checkedListBoxPermissions.TabIndex = 0;
            this.checkedListBoxPermissions.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListBoxPermissions_ItemCheck);
            // 
            // btnCheckAll
            // 
            this.btnCheckAll.Location = new System.Drawing.Point(14, 276);
            this.btnCheckAll.Name = "btnCheckAll";
            this.btnCheckAll.Size = new System.Drawing.Size(130, 23);
            this.btnCheckAll.TabIndex = 1;
            this.btnCheckAll.Text = "Check All (Shown)";
            this.btnCheckAll.Click += new System.EventHandler(this.btnCheckAll_Click);
            // 
            // btnUncheckAll
            // 
            this.btnUncheckAll.Location = new System.Drawing.Point(150, 276);
            this.btnUncheckAll.Name = "btnUncheckAll";
            this.btnUncheckAll.Size = new System.Drawing.Size(145, 23);
            this.btnUncheckAll.TabIndex = 2;
            this.btnUncheckAll.Text = "Uncheck All (Shown)";
            this.btnUncheckAll.Click += new System.EventHandler(this.btnUncheckAll_Click);
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.lblSelectedCount);
            this.panelFooter.Controls.Add(this.btnSave);
            this.panelFooter.Controls.Add(this.btnClose);
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 530);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(784, 56);
            this.panelFooter.TabIndex = 4;
            // 
            // lblSelectedCount
            // 
            this.lblSelectedCount.AutoSize = true;
            this.lblSelectedCount.Location = new System.Drawing.Point(14, 19);
            this.lblSelectedCount.Name = "lblSelectedCount";
            this.lblSelectedCount.Size = new System.Drawing.Size(76, 17);
            this.lblSelectedCount.TabIndex = 0;
            this.lblSelectedCount.Text = "Selected: 0";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(586, 12);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 23);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Save";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(682, 12);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(90, 23);
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // FrmRolePermissions
            // 
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(784, 586);
            this.Controls.Add(this.grpPermissions);
            this.Controls.Add(this.grpFilters);
            this.Controls.Add(this.grpRole);
            this.Controls.Add(this.panelHeader);
            this.Controls.Add(this.panelFooter);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(760, 560);
            this.Name = "FrmRolePermissions";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Role Permissions";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            this.grpRole.ResumeLayout(false);
            this.grpRole.PerformLayout();
            this.grpFilters.ResumeLayout(false);
            this.grpFilters.PerformLayout();
            this.grpPermissions.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}