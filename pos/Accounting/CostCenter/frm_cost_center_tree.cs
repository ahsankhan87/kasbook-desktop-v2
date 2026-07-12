using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;

namespace pos.Accounting.CostCenter
{
    /// <summary>
    /// Cost Center Tree View Form - Hierarchical view with right-click context menu
    /// </summary>
    public partial class frm_cost_center_tree : Form
    {
        private CostCenterBLL bll = new CostCenterBLL();

        public frm_cost_center_tree()
        {
            InitializeComponent();
            InitializeUI();
        }

        private void InitializeUI()
        {
            // Apply theme
            AppTheme.Apply(this);

            // Setup tree view
            treeView.HideSelection = false;
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.NodeMouseClick += TreeView_NodeMouseClick;

            // Setup context menu
            SetupContextMenu();

            // Setup status/detail panel
            LoadTreeData();
        }

        private void SetupContextMenu()
        {
            contextMenu.Items.Clear();

            ToolStripMenuItem itemAddChild = new ToolStripMenuItem("Add Child Cost Center", null, ContextMenu_AddChild);
            ToolStripMenuItem itemEdit = new ToolStripMenuItem("Edit", null, ContextMenu_Edit);
            ToolStripMenuItem itemViewPL = new ToolStripMenuItem("View P&L", null, ContextMenu_ViewPL);
            ToolStripMenuItem itemSetBudget = new ToolStripMenuItem("Set Budget", null, ContextMenu_SetBudget);
            ToolStripMenuItem itemDeactivate = new ToolStripMenuItem("Deactivate", null, ContextMenu_Deactivate);

            contextMenu.Items.Add(itemAddChild);
            contextMenu.Items.Add(itemEdit);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(itemViewPL);
            contextMenu.Items.Add(itemSetBudget);
            contextMenu.Items.Add(new ToolStripSeparator());
            contextMenu.Items.Add(itemDeactivate);
        }

        private void LoadTreeData()
        {
            try
            {
                using (BusyScope.Show(this, "Loading cost center hierarchy..."))
                {
                    DataTable dt = bll.GetCostCenterTree(includeBalances: true);
                    treeView.Nodes.Clear();

                    // Build tree from flat structure with parent references
                    Dictionary<int, TreeNode> nodeDict = new Dictionary<int, TreeNode>();

                    // First pass: create all nodes
                    foreach (DataRow row in dt.Rows)
                    {
                        int ccId = (int)row["cc_id"];
                        string ccCode = row["cc_code"]?.ToString() ?? "";
                        string ccName = row["cc_name"]?.ToString() ?? "";
                        decimal? ytdIncome = row["ytd_income"] == DBNull.Value ? null : (decimal?)row["ytd_income"];
                        decimal? ytdExpense = row["ytd_expense"] == DBNull.Value ? null : (decimal?)row["ytd_expense"];

                        decimal net = (ytdIncome ?? 0) - (ytdExpense ?? 0);
                        string displayText = $"{ccCode} — {ccName}";
                        if (ytdIncome.HasValue || ytdExpense.HasValue)
                        {
                            displayText += $" (Income: {ytdIncome:N2}, Expense: {ytdExpense:N2}, Net: {net:N2})";
                        }

                        TreeNode node = new TreeNode(displayText)
                        {
                            Tag = ccId
                        };

                        // Color based on net result
                        if (net > 0)
                            node.ForeColor = System.Drawing.Color.Green;
                        else if (net < 0)
                            node.ForeColor = System.Drawing.Color.Red;

                        nodeDict[ccId] = node;
                    }

                    // Second pass: link parent-child
                    foreach (DataRow row in dt.Rows)
                    {
                        int ccId = (int)row["cc_id"];
                        int? parentId = row["parent_cc_id"] == DBNull.Value ? null : (int?)row["parent_cc_id"];

                        if (parentId.HasValue && nodeDict.ContainsKey(parentId.Value))
                        {
                            nodeDict[parentId.Value].Nodes.Add(nodeDict[ccId]);
                        }
                        else
                        {
                            // Root node
                            treeView.Nodes.Add(nodeDict[ccId]);
                        }
                    }

                    treeView.ExpandAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tree: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node?.Tag is int ccId)
            {
                LoadCostCenterDetails(ccId);
            }
        }

        private void TreeView_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView.SelectedNode = e.Node;
                contextMenu.Show(treeView, e.Location);
            }
        }

        private void LoadCostCenterDetails(int ccId)
        {
            try
            {
                var model = bll.GetCostCenterById(ccId);
                if (model != null)
                {
                    lblDetails.Text = $"Code: {model.CcCode} | Name: {model.CcName} | Type: {model.CcType} | Active: {(model.IsActive ? "Yes" : "No")}";
                }
            }
            catch (Exception ex)
            {
                lblDetails.Text = $"Error: {ex.Message}";
            }
        }

        private void ContextMenu_AddChild(object sender, EventArgs e)
        {
            if (treeView.SelectedNode?.Tag is int parentCcId)
            {
                // Open cost center form to add child
                var frm = new frm_cost_center_setup(0, parentCcId);
                frm.ShowDialog(this);
                LoadTreeData();
            }
        }

        private void ContextMenu_Edit(object sender, EventArgs e)
        {
            if (treeView.SelectedNode?.Tag is int ccId)
            {
                var frm = new frm_cost_center_setup(ccId);
                frm.ShowDialog(this);
                LoadTreeData();
            }
        }

        private void ContextMenu_ViewPL(object sender, EventArgs e)
        {
            if (treeView.SelectedNode?.Tag is int ccId)
            {
                var frm = new frm_departmental_pl();
                frm.ShowDialog();
            }
        }

        private void ContextMenu_SetBudget(object sender, EventArgs e)
        {
            if (treeView.SelectedNode?.Tag is int ccId)
            {
                var frm = new frm_budget_setup();
                frm.ShowDialog();
            }
        }

        private void ContextMenu_Deactivate(object sender, EventArgs e)
        {
            if (treeView.SelectedNode?.Tag is int ccId)
            {
                DialogResult result = MessageBox.Show("Deactivate this cost center?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (BusyScope.Show(this, "Deactivating..."))
                        {
                            var model = bll.GetCostCenterById(ccId);
                            if (model != null)
                            {
                                model.IsActive = false;
                                bll.SaveCostCenter(model, UsersModal.logged_in_userid);
                                MessageBox.Show("Cost center deactivated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                LoadTreeData();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void FrmCostCenterTree_Load(object sender, EventArgs e)
        {
            LoadTreeData();
        }
    }
}
