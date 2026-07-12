namespace pos.Accounting.CostCenter
{
    partial class frm_cost_center_tree
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
            this.components = new System.ComponentModel.Container();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlTree = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.pnlDetails = new System.Windows.Forms.Panel();
            this.lblDetails = new System.Windows.Forms.Label();
            this.pnlTree.SuspendLayout();
            this.SuspendLayout();

            // lblTitle
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(12, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(250, 25);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Cost Center Hierarchy";

            // pnlTree
            this.pnlTree.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTree.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTree.Controls.Add(this.treeView);
            this.pnlTree.Location = new System.Drawing.Point(12, 45);
            this.pnlTree.Name = "pnlTree";
            this.pnlTree.Size = new System.Drawing.Size(500, 480);
            this.pnlTree.TabIndex = 1;

            // treeView
            this.treeView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeView.BackColor = System.Drawing.Color.White;
            this.treeView.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.treeView.Location = new System.Drawing.Point(5, 5);
            this.treeView.Name = "treeView";
            this.treeView.Size = new System.Drawing.Size(490, 470);
            this.treeView.TabIndex = 0;

            // contextMenu
            this.contextMenu.Name = "contextMenu";
            this.contextMenu.Size = new System.Drawing.Size(200, 150);

            // pnlDetails
            this.pnlDetails.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlDetails.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlDetails.Controls.Add(this.lblDetails);
            this.pnlDetails.Location = new System.Drawing.Point(12, 530);
            this.pnlDetails.Name = "pnlDetails";
            this.pnlDetails.Padding = new System.Windows.Forms.Padding(5);
            this.pnlDetails.Size = new System.Drawing.Size(500, 50);
            this.pnlDetails.TabIndex = 2;

            // lblDetails
            this.lblDetails.AutoSize = true;
            this.lblDetails.Location = new System.Drawing.Point(5, 5);
            this.lblDetails.Name = "lblDetails";
            this.lblDetails.Size = new System.Drawing.Size(100, 13);
            this.lblDetails.TabIndex = 0;
            this.lblDetails.Text = "Select a node...";
            this.lblDetails.Font = new System.Drawing.Font("Segoe UI", 8.25F);

            // frm_cost_center_tree
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(524, 590);
            this.Controls.Add(this.pnlDetails);
            this.Controls.Add(this.pnlTree);
            this.Controls.Add(this.lblTitle);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.Name = "frm_cost_center_tree";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cost Center Hierarchy";
            this.Load += new System.EventHandler(this.FrmCostCenterTree_Load);
            this.pnlTree.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel pnlTree;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.Panel pnlDetails;
        private System.Windows.Forms.Label lblDetails;
    }
}
