using DGVPrinterHelper;
using POS.BLL;
using POS.Core;
using pos.Security.Authorization;
using pos.UI;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_coa : Form
    {
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private readonly UserIdentity _currentUser = AppSecurityContext.User;
        private readonly GeneralBLL _generalBll = new GeneralBLL();
        private readonly AccountsBLL _accountsBll = new AccountsBLL();
        private readonly GroupsBLL _groupsBll = new GroupsBLL();
        private readonly FiscalYearBLL _fiscalYearBll = new FiscalYearBLL();

        private readonly Dictionary<int, CoaGroupInfo> _groups = new Dictionary<int, CoaGroupInfo>();
        private readonly Dictionary<int, CoaAccountInfo> _accounts = new Dictionary<int, CoaAccountInfo>();
        private readonly Dictionary<int, CoaAggregate> _groupAggregates = new Dictionary<int, CoaAggregate>();
        private readonly Dictionary<int, CoaAggregate> _accountAggregates = new Dictionary<int, CoaAggregate>();
        private readonly Dictionary<int, TreeNode> _nodeLookup = new Dictionary<int, TreeNode>();

        private ContextMenuStrip _treeMenu;
        private ImageList _treeImages;
        private DataGridView _printGrid;
        private Timer _searchTimer;
        private Font _boldFont;
        private Tuple<DateTime, DateTime> _currentFiscalYearDates; // Cached fiscal-year (from_date, to_date)

        public frm_coa()
        {
            InitializeComponent();
            BuildSupportingObjects();
            WireEvents();
        }

        private void WireEvents()
        {
            Load += frm_coa_Load;
            KeyDown += frm_coa_KeyDown;
            FormClosed += (s, e) => { if (_boldFont != null) _boldFont.Dispose(); };
            _searchBox.TextChanged += SearchBox_TextChanged;
            _searchTimer.Tick += SearchTimer_Tick;
            _tree.AfterSelect += Tree_AfterSelect;
            _tree.NodeMouseClick += Tree_NodeMouseClick;
            _tree.DrawNode += Tree_DrawNode;
            _btnAddGroup.Click += (s, e) => AddGroupForSelectedNode();
            _btnAddAccount.Click += (s, e) => AddAccountForSelectedNode();
            _btnExpandAll.Click += (s, e) => _tree.ExpandAll();
            _btnCollapseAll.Click += (s, e) => _tree.CollapseAll();
            _btnPrint.Click += (s, e) => PrintCoa();
            _btnSetupStandard.Click += (s, e) => SetupStandardChartOfAccounts();
            _btnEditSelected.Click += (s, e) => EditSelectedNode();
            _btnRefresh.Click += (s, e) => ReloadAll();
            _btnNewGroup.Click += (s, e) => AddGroupForSelectedNode();
            _btnNewAccount.Click += (s, e) => AddAccountForSelectedNode();
            _btnViewLedger.Click += (s, e) => ViewLedgerForSelectedNode();
            _btnViewTransactions.Click += (s, e) => ViewTransactionsForSelectedNode();
            _chkIsBankAccount.CheckedChanged += (s, e) => UpdateBankFieldVisibility();
        }

        private void BuildSupportingObjects()
        {
            _printGrid = new DataGridView { Visible = false, AllowUserToAddRows = false, AllowUserToDeleteRows = false };
            Controls.Add(_printGrid);

            _searchTimer = new Timer { Interval = 200 };
            _treeImages = CreateImageList();
            _tree.ImageList = _treeImages;
            _tree.DrawMode = TreeViewDrawMode.OwnerDrawText;
            _tree.HideSelection = false;
            _tree.FullRowSelect = false;

            _treeMenu = new ContextMenuStrip();
            _treeMenu.Items.Add("Add Sub-Group", null, (s, e) => AddGroupForSelectedNode());
            _treeMenu.Items.Add("Add Account", null, (s, e) => AddAccountForSelectedNode());
            _treeMenu.Items.Add(new ToolStripSeparator());
            _treeMenu.Items.Add("Edit", null, (s, e) => EditSelectedNode());
            _treeMenu.Items.Add("Delete", null, (s, e) => DeleteSelectedNode());
            _treeMenu.Items.Add(new ToolStripSeparator());
            _treeMenu.Items.Add("View Ledger", null, (s, e) => ViewLedgerForSelectedNode());
            _treeMenu.Items.Add("View Transactions", null, (s, e) => ViewTransactionsForSelectedNode());
            _tree.ContextMenuStrip = _treeMenu;
        }

        

        private Label CreateSummaryLabel(string text)
        {
            return new Label
            {
                Text = text,
                AutoSize = true,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.FromArgb(33, 37, 41)
            };
        }

        private TextBox CreateReadOnlyTextBox(bool multiline = false)
        {
            return new TextBox
            {
                ReadOnly = true,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White,
                Multiline = multiline,
                Dock = DockStyle.Fill,
                Height = multiline ? 60 : 26,
                ScrollBars = multiline ? ScrollBars.Vertical : ScrollBars.None
            };
        }

        private ComboBox CreateReadOnlyComboBox()
        {
            return new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Enabled = false,
                Dock = DockStyle.Fill
            };
        }

        private ImageList CreateImageList()
        {
            var images = new ImageList { ColorDepth = ColorDepth.Depth32Bit, ImageSize = new Size(18, 18) };
            images.Images.Add("asset", CreateCircleIcon(Color.RoyalBlue));
            images.Images.Add("liability", CreateCircleIcon(Color.Coral));
            images.Images.Add("equity", CreateCircleIcon(Color.Teal));
            images.Images.Add("income", CreateCircleIcon(Color.SeaGreen));
            images.Images.Add("expense", CreateCircleIcon(Color.Goldenrod));
            images.Images.Add("folder", CreateFolderIcon());
            images.Images.Add("account", CreateLeafIcon());
            return images;
        }

        private Bitmap CreateCircleIcon(Color fill)
        {
            var bmp = new Bitmap(18, 18);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (var brush = new SolidBrush(fill))
                {
                    g.FillEllipse(brush, 2, 2, 14, 14);
                }
                using (var pen = new Pen(Color.White, 2))
                {
                    g.DrawEllipse(pen, 2, 2, 14, 14);
                }
            }
            return bmp;
        }

        private Bitmap CreateFolderIcon()
        {
            var bmp = new Bitmap(18, 18);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (var fill = new SolidBrush(Color.Goldenrod))
                using (var outline = new Pen(Color.SaddleBrown, 1))
                {
                    g.FillRectangle(fill, 2, 6, 14, 9);
                    g.FillRectangle(fill, 3, 4, 6, 4);
                    g.DrawRectangle(outline, 2, 6, 14, 9);
                }
            }
            return bmp;
        }

        private Bitmap CreateLeafIcon()
        {
            var bmp = new Bitmap(18, 18);
            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                g.Clear(Color.Transparent);
                using (var brush = new SolidBrush(Color.SeaGreen))
                using (var pen = new Pen(Color.DarkGreen, 1))
                {
                    var points = new[]
                    {
                        new Point(3, 9), new Point(7, 3), new Point(14, 4), new Point(15, 10), new Point(9, 15), new Point(4, 13)
                    };
                    g.FillPolygon(brush, points);
                    g.DrawPolygon(pen, points);
                }
            }
            return bmp;
        }

        private void frm_coa_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            ReloadAll();
        }

        private void ReloadAll()
        {
            LoadLookupData();
            LoadTree(string.Empty);
            if (_tree.Nodes.Count > 0)
            {
                _tree.ExpandAll();
                _tree.SelectedNode = _tree.Nodes[0];
            }
        }

        private void LoadLookupData()
        {
            // Fetch the current active fiscal-year dates (dynamic, not cached at login)
            _currentFiscalYearDates = GetActiveFiscalYearDates();

            _groups.Clear();
            _accounts.Clear();
            _groupAggregates.Clear();
            _accountAggregates.Clear();
            _nodeLookup.Clear();

            var groupTypes = _generalBll.GetRecord("id,name", "acc_account_type");
            var allGroups = _generalBll.GetRecord("*", "acc_groups");
            var allAccounts = _generalBll.GetRecord("*", "acc_accounts WHERE branch_id = " + UsersModal.logged_in_branch_id);

            BindCombo(_cmbGroupType, groupTypes, true);
            BindCombo(_cmbAccountType, groupTypes, true);
            BindGroupCombo(_cmbGroupParent, allGroups);
            BindGroupCombo(_cmbAccountParent, allGroups);

            foreach (DataRow row in allGroups.Rows)
            {
                var id = ToInt(row, "id");
                if (id == 0) continue;
                var group = new CoaGroupInfo
                {
                    Id = id,
                    Code = GetString(row, "code"),
                    Name = GetString(row, "name"),
                    Name2 = GetString(row, "name_2"),
                    ParentId = GetInt(row, "parent_id"),
                    AccountTypeId = GetInt(row, "account_type_id"),
                    Description = GetString(row, "description"),
                    IsActive = !HasColumn(row, "is_active") || GetBool(row, "is_active", true)
                };
                _groups[id] = group;
            }

            foreach (DataRow row in allAccounts.Rows)
            {
                var id = ToInt(row, "id");
                if (id == 0) continue;
                var account = new CoaAccountInfo
                {
                    Id = id,
                    GroupId = GetInt(row, "group_id"),
                    Code = GetString(row, "code"),
                    Name = GetString(row, "name"),
                    Name2 = GetString(row, "name_2"),
                    Description = GetString(row, "description"),
                    OpeningDebit = GetDouble(row, "op_dr_balance"),
                    OpeningCredit = GetDouble(row, "op_cr_balance"),
                    OpeningBalanceDate = GetNullableDate(row, "opening_balance_date"),
                    IsBankAccount = GetBool(row, "is_bank_account", false),
                    IsCashAccount = GetBool(row, "is_cash_account", false),
                    BankName = GetString(row, "bank_name"),
                    BankBranch = GetString(row, "branch"),
                    AccountNo = GetString(row, "account_no"),
                    Iban = GetString(row, "iban"),
                    IsActive = !HasColumn(row, "is_active") || GetBool(row, "is_active", true)
                };
                account.Aggregate = GetAccountAggregate(account.Id, account.OpeningDebit, account.OpeningCredit);
                _accounts[id] = account;
                _accountAggregates[id] = account.Aggregate;
            }

            foreach (var group in _groups.Values)
            {
                group.Aggregate = GetGroupAggregate(group.Id);
                _groupAggregates[group.Id] = group.Aggregate;
            }
        }

        private CoaAggregate GetAccountAggregate(int accountId, double openingDebit, double openingCredit)
        {
            // Use current fiscal-year dates from cache (updated in LoadLookupData)
            DateTime fyFromDate = _currentFiscalYearDates != null ? _currentFiscalYearDates.Item1 : UsersModal.fy_from_date;
            DateTime fyToDate = _currentFiscalYearDates != null ? _currentFiscalYearDates.Item2 : UsersModal.fy_to_date;

            string whereClause = "acc_entries WHERE branch_id = " + UsersModal.logged_in_branch_id + " AND account_id = " + accountId +
                " AND entry_date >= '" + fyFromDate.ToString("yyyy-MM-dd") + "'" +
                " AND entry_date <= '" + fyToDate.ToString("yyyy-MM-dd") + "'";

            DataTable dt = _generalBll.GetRecord("COUNT(*) AS txn_count, MAX(entry_date) AS last_transaction_date, COALESCE(SUM(debit),0) AS debit_total, COALESCE(SUM(credit),0) AS credit_total", whereClause);
            var aggregate = new CoaAggregate();
            if (dt.Rows.Count > 0)
            {
                var row = dt.Rows[0];
                aggregate.Debit = GetDouble(row, "debit_total") + openingDebit;
                aggregate.Credit = GetDouble(row, "credit_total") + openingCredit;
                aggregate.TransactionCount = GetInt(row, "txn_count");
                aggregate.LastTransactionDate = GetNullableDate(row, "last_transaction_date");
            }
            else
            {
                aggregate.Debit = openingDebit;
                aggregate.Credit = openingCredit;
            }
            aggregate.Balance = aggregate.Debit - aggregate.Credit;
            return aggregate;
        }

        private CoaAggregate GetGroupAggregate(int groupId)
        {
            if (_groupAggregates.ContainsKey(groupId))
            {
                return _groupAggregates[groupId];
            }

            if (!_groups.ContainsKey(groupId))
            {
                return new CoaAggregate();
            }

            var aggregate = new CoaAggregate();
            _groupAggregates[groupId] = aggregate;

            foreach (var childGroup in _groups.Values.Where(x => x.ParentId == groupId))
            {
                if (childGroup.Id == groupId)
                {
                    continue;
                }

                var childAggregate = GetGroupAggregate(childGroup.Id);
                aggregate.Add(childAggregate);
            }

            foreach (var account in _accounts.Values.Where(x => x.GroupId == groupId))
            {
                aggregate.Add(account.Aggregate);
            }

            return aggregate;
        }

        private void LoadTree(string filter)
        {
            _tree.BeginUpdate();
            try
            {
                _tree.Nodes.Clear();
                _nodeLookup.Clear();
                var normalized = string.IsNullOrWhiteSpace(filter) ? string.Empty : filter.Trim();
                foreach (var root in _groups.Values.Where(g => g.ParentId == 0).OrderBy(g => g.Code).ThenBy(g => g.Name))
                {
                    AddGroupNode(_tree.Nodes, root, 1, normalized);
                }
                _tree.ExpandAll();
            }
            finally
            {
                _tree.EndUpdate();
            }
        }

        private void AddGroupNode(TreeNodeCollection nodes, CoaGroupInfo group, int level, string filter)
        {
            if (!ShouldIncludeGroup(group.Id, filter)) return;

            var node = new TreeNode(BuildNodeText(group.Code, group.Name))
            {
                ImageKey = GetGroupImageKey(group.Name, level),
                SelectedImageKey = GetGroupImageKey(group.Name, level),
                Tag = new CoaNodeTag { Kind = CoaNodeKind.Group, Id = group.Id, Level = level }
            };
            node.NodeFont = level == 1 ? _boldFont : Font;
            node.ForeColor = level == 1 ? GetLevel1Color(group.Name) : Color.FromArgb(33, 37, 41);
            if (MatchesGroup(group, filter))
            {
                node.BackColor = Color.FromArgb(255, 248, 196);
            }

            nodes.Add(node);
            _nodeLookup[group.Id] = node;

            foreach (var childGroup in _groups.Values.Where(x => x.ParentId == group.Id).OrderBy(x => x.Code).ThenBy(x => x.Name))
            {
                AddGroupNode(node.Nodes, childGroup, level + 1, filter);
            }

            foreach (var account in _accounts.Values.Where(x => x.GroupId == group.Id).OrderBy(x => x.Code).ThenBy(x => x.Name))
            {
                if (!MatchesAccount(account, filter) && !string.IsNullOrWhiteSpace(filter)) continue;
                AddAccountNode(node.Nodes, account, level + 1, filter);
            }
        }

        private void AddAccountNode(TreeNodeCollection nodes, CoaAccountInfo account, int level, string filter)
        {
            if (!MatchesAccount(account, filter) && !string.IsNullOrWhiteSpace(filter)) return;
            var node = new TreeNode(BuildLeafText(account))
            {
                ImageKey = "account",
                SelectedImageKey = "account",
                Tag = new CoaNodeTag { Kind = CoaNodeKind.Account, Id = account.Id, Level = level }
            };
            if (MatchesAccount(account, filter)) node.BackColor = Color.FromArgb(255, 248, 196);
            nodes.Add(node);
            _nodeLookup[account.Id] = node;
        }

        private bool ShouldIncludeGroup(int groupId, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return true;
            if (MatchesGroup(_groups[groupId], filter)) return true;
            return _groups.Values.Where(x => x.ParentId == groupId).Any(x => ShouldIncludeGroup(x.Id, filter)) ||
                   _accounts.Values.Any(x => x.GroupId == groupId && MatchesAccount(x, filter));
        }

        private bool MatchesGroup(CoaGroupInfo group, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return true;
            return Contains(group.Code, filter) || Contains(group.Name, filter) || Contains(group.Name2, filter);
        }

        private bool MatchesAccount(CoaAccountInfo account, string filter)
        {
            if (string.IsNullOrWhiteSpace(filter)) return true;
            return Contains(account.Code, filter) || Contains(account.Name, filter) || Contains(account.Name2, filter);
        }

        private string BuildNodeText(string code, string name)
        {
            if (string.IsNullOrWhiteSpace(code)) return name;
            return code + " — " + name;
        }

        private string BuildLeafText(CoaAccountInfo account)
        {
            return string.Format("{0} — {1} ({2})", account.Code, account.Name, FormatBalanceSide(account.Aggregate.Balance));
        }

        private string FormatBalanceSide(double value)
        {
            var amount = Math.Abs(value).ToString("N0");
            return value < 0 ? amount + " Cr" : amount + " Dr";
        }

        private Color GetLevel1Color(string name)
        {
            string normalized = (name ?? string.Empty).ToLowerInvariant();
            if (normalized.Contains("asset")) return Color.RoyalBlue;
            if (normalized.Contains("liabil")) return Color.Coral;
            if (normalized.Contains("equity")) return Color.Teal;
            if (normalized.Contains("income") || normalized.Contains("revenue")) return Color.SeaGreen;
            if (normalized.Contains("expense")) return Color.DarkGoldenrod;
            return Color.FromArgb(33, 37, 41);
        }

        private string GetGroupImageKey(string name, int level)
        {
            if (level == 1)
            {
                string normalized = (name ?? string.Empty).ToLowerInvariant();
                if (normalized.Contains("asset")) return "asset";
                if (normalized.Contains("liabil")) return "liability";
                if (normalized.Contains("equity")) return "equity";
                if (normalized.Contains("income") || normalized.Contains("revenue")) return "income";
                if (normalized.Contains("expense")) return "expense";
                return "asset";
            }
            return "folder";
        }

        private void SearchBox_TextChanged(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            _searchTimer.Start();
        }

        private void SearchTimer_Tick(object sender, EventArgs e)
        {
            _searchTimer.Stop();
            LoadTree(_searchBox.Text);
        }

        private void Tree_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _tree.SelectedNode = e.Node;
            if (e.Button == MouseButtons.Right)
            {
                e.Node.ContextMenuStrip = _treeMenu;
            }
        }

        private void Tree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            UpdateSelectedNodeDetails();
        }

        private void Tree_DrawNode(object sender, DrawTreeNodeEventArgs e)
        {
            e.DrawDefault = true;
        }

        private void UpdateSelectedNodeDetails()
        {
            if (_tree.SelectedNode == null || !(_tree.SelectedNode.Tag is CoaNodeTag))
            {
                ClearDetails();
                return;
            }

            var tag = (CoaNodeTag)_tree.SelectedNode.Tag;
            if (tag.Kind == CoaNodeKind.Group && _groups.ContainsKey(tag.Id))
            {
                var group = _groups[tag.Id];
                var aggregate = group.Aggregate;
                ShowGroupDetails(group, aggregate, tag.Level);
                _detailsTabs.SelectedTab = _groupTab;
            }
            else if (tag.Kind == CoaNodeKind.Account && _accounts.ContainsKey(tag.Id))
            {
                var account = _accounts[tag.Id];
                var aggregate = account.Aggregate;
                ShowAccountDetails(account, aggregate, tag.Level);
                _detailsTabs.SelectedTab = _accountTab;
            }
        }

        private void ShowGroupDetails(CoaGroupInfo group, CoaAggregate aggregate, int level)
        {
            _summaryNodeType.Text = level == 1 ? "Level 1 Group" : "Level 2 Group";
            _summaryCode.Text = group.Code;
            _summaryName.Text = group.Name;
            _summaryDebit.Text = aggregate.Debit.ToString("N0");
            _summaryCredit.Text = aggregate.Credit.ToString("N0");
            _summaryBalance.Text = FormatBalanceSide(aggregate.Balance);
            _summaryLastTxn.Text = aggregate.LastTransactionDate.HasValue ? aggregate.LastTransactionDate.Value.ToShortDateString() : string.Empty;
            _summaryTxnCount.Text = aggregate.TransactionCount.ToString("N0");

            _txtGroupCode.Text = group.Code;
            _txtGroupName.Text = group.Name;
            _cmbGroupParent.SelectedValue = group.ParentId;
            _cmbGroupType.SelectedValue = group.AccountTypeId;
            _txtGroupDescription.Text = group.Description;
            _chkGroupActive.Checked = group.IsActive;

            ClearAccountFields();
        }

        private void ShowAccountDetails(CoaAccountInfo account, CoaAggregate aggregate, int level)
        {
            _summaryNodeType.Text = "Level 3 Account";
            _summaryCode.Text = account.Code;
            _summaryName.Text = account.Name;
            _summaryDebit.Text = aggregate.Debit.ToString("N0");
            _summaryCredit.Text = aggregate.Credit.ToString("N0");
            _summaryBalance.Text = FormatBalanceSide(aggregate.Balance);
            _summaryLastTxn.Text = aggregate.LastTransactionDate.HasValue ? aggregate.LastTransactionDate.Value.ToShortDateString() : string.Empty;
            _summaryTxnCount.Text = aggregate.TransactionCount.ToString("N0");

            _txtAccountCode.Text = account.Code;
            _txtAccountName.Text = account.Name;
            _cmbAccountParent.SelectedValue = account.GroupId;
            _cmbAccountType.SelectedValue = GetAccountTypeIdForGroup(account.GroupId);
            _txtOpeningDebit.Text = account.OpeningDebit.ToString("N0");
            _txtOpeningCredit.Text = account.OpeningCredit.ToString("N0");
            _dtpOpeningBalanceDate.Value = account.OpeningBalanceDate ?? DateTime.Today;
            _chkIsBankAccount.Checked = account.IsBankAccount;
            _chkIsCashAccount.Checked = account.IsCashAccount;
            _txtBankName.Text = account.BankName;
            _txtBankBranch.Text = account.BankBranch;
            _txtBankAccountNo.Text = account.AccountNo;
            _txtIban.Text = account.Iban;
            _txtAccountDescription.Text = account.Description;
            _chkAccountActive.Checked = account.IsActive;
            UpdateBankFieldVisibility();
        }

        private void ClearDetails()
        {
            _summaryNodeType.Clear();
            _summaryCode.Clear();
            _summaryName.Clear();
            _summaryDebit.Clear();
            _summaryCredit.Clear();
            _summaryBalance.Clear();
            _summaryLastTxn.Clear();
            _summaryTxnCount.Clear();
            ClearGroupFields();
            ClearAccountFields();
        }

        private void ClearGroupFields()
        {
            _txtGroupCode.Clear();
            _txtGroupName.Clear();
            _cmbGroupParent.SelectedIndex = 0;
            _cmbGroupType.SelectedIndex = -1;
            _txtGroupDescription.Clear();
            _chkGroupActive.Checked = false;
        }

        private void ClearAccountFields()
        {
            _txtAccountCode.Clear();
            _txtAccountName.Clear();
            _cmbAccountParent.SelectedIndex = 0;
            _cmbAccountType.SelectedIndex = -1;
            _txtOpeningDebit.Clear();
            _txtOpeningCredit.Clear();
            _dtpOpeningBalanceDate.Value = DateTime.Today;
            _chkIsBankAccount.Checked = false;
            _chkIsCashAccount.Checked = false;
            _txtBankName.Clear();
            _txtBankBranch.Clear();
            _txtBankAccountNo.Clear();
            _txtIban.Clear();
            _txtAccountDescription.Clear();
            _chkAccountActive.Checked = false;
        }

        private void UpdateBankFieldVisibility()
        {
            _bankGroup.Enabled = true;
            _txtBankName.Enabled = _chkIsBankAccount.Checked;
            _txtBankBranch.Enabled = _chkIsBankAccount.Checked;
            _txtBankAccountNo.Enabled = _chkIsBankAccount.Checked;
            _txtIban.Enabled = _chkIsBankAccount.Checked;
        }

        private void AddGroupForSelectedNode()
        {
            if (!_auth.HasPermission(_currentUser, Permissions.Group_Create))
            {
                MessageBox.Show("You do not have permission to create groups.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int parentId = 0;
            if (_tree.SelectedNode != null && _tree.SelectedNode.Tag is CoaNodeTag)
            {
                var tag = (CoaNodeTag)_tree.SelectedNode.Tag;
                parentId = tag.Kind == CoaNodeKind.Group ? tag.Id : _accounts[tag.Id].GroupId;
            }

            var form = new frm_addGroup();
            form.tb_lbl_is_edit.Text = "false";
            form.tb_parent_id.SelectedValue = parentId;
            form.ShowDialog(this);
            ReloadAll();
        }

        private void AddAccountForSelectedNode()
        {
            if (!_auth.HasPermission(_currentUser, Permissions.Account_Create))
            {
                MessageBox.Show("You do not have permission to create accounts.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int groupId = 0;
            if (_tree.SelectedNode != null && _tree.SelectedNode.Tag is CoaNodeTag)
            {
                var tag = (CoaNodeTag)_tree.SelectedNode.Tag;
                groupId = tag.Kind == CoaNodeKind.Group ? tag.Id : _accounts[tag.Id].GroupId;
            }

            var form = new frm_addAccount();
            form.tb_lbl_is_edit.Text = "false";
            form.tb_group_id.SelectedValue = groupId;
            form.ShowDialog(this);
            ReloadAll();
        }

        private void EditSelectedNode()
        {
            if (_tree.SelectedNode == null || !(_tree.SelectedNode.Tag is CoaNodeTag)) return;
            var tag = (CoaNodeTag)_tree.SelectedNode.Tag;
            if (tag.Kind == CoaNodeKind.Group)
            {
                if (!_auth.HasPermission(_currentUser, Permissions.Group_Edit))
                {
                    MessageBox.Show("You do not have permission to edit groups.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var group = _groups[tag.Id];
                var form = new frm_addGroup();
                form.tb_lbl_is_edit.Text = "true";
                form.tb_id.Text = group.Id.ToString();
                form.tb_name.Text = group.Name;
                form.tb_name_2.Text = group.Name2;
                form.tb_parent_id.SelectedValue = group.ParentId;
                form.tb_account_type.SelectedValue = group.AccountTypeId;
                form.ShowDialog(this);
            }
            else if (tag.Kind == CoaNodeKind.Account)
            {
                if (!_auth.HasPermission(_currentUser, Permissions.Account_Edit))
                {
                    MessageBox.Show("You do not have permission to edit accounts.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var account = _accounts[tag.Id];
                var form = new frm_addAccount();
                form.tb_lbl_is_edit.Text = "true";
                form.tb_id.Text = account.Id.ToString();
                form.tb_name.Text = account.Name;
                form.tb_name_2.Text = account.Name2;
                form.tb_code.Text = account.Code;
                form.tb_group_id.SelectedValue = account.GroupId;
                form.tb_op_dr_balance.Text = account.OpeningDebit.ToString();
                form.tb_op_cr_balance.Text = account.OpeningCredit.ToString();
                form.ShowDialog(this);
            }

            ReloadAll();
        }

        private void DeleteSelectedNode()
        {
            if (_tree.SelectedNode == null || !(_tree.SelectedNode.Tag is CoaNodeTag)) return;
            var tag = (CoaNodeTag)_tree.SelectedNode.Tag;

            if (tag.Kind == CoaNodeKind.Group)
            {
                if (!_auth.HasPermission(_currentUser, Permissions.Group_Delete))
                {
                    MessageBox.Show("You do not have permission to delete groups.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (_groups.Values.Any(x => x.ParentId == tag.Id) || _accounts.Values.Any(x => x.GroupId == tag.Id))
                {
                    MessageBox.Show("This group has child groups or accounts. Remove them first.", "Delete Blocked", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Delete this group?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _groupsBll.Delete(tag.Id);
                    ReloadAll();
                }
            }
            else if (tag.Kind == CoaNodeKind.Account)
            {
                if (!_auth.HasPermission(_currentUser, Permissions.Account_Delete))
                {
                    MessageBox.Show("You do not have permission to delete accounts.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (MessageBox.Show("Delete this account?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                {
                    _accountsBll.Delete(tag.Id);
                    ReloadAll();
                }
            }
        }

        private void ViewLedgerForSelectedNode()
        {
            if (_tree.SelectedNode == null || !(_tree.SelectedNode.Tag is CoaNodeTag)) return;
            var tag = (CoaNodeTag)_tree.SelectedNode.Tag;
            if (tag.Kind == CoaNodeKind.Account)
            {
                var form = new frm_account_report();
                form.LoadForAccount(tag.Id);
                form.ShowDialog(this);
            }
            else
            {
                var form = new frm_group_report();
                form.LoadForGroup(tag.Id);
                form.ShowDialog(this);
            }
        }

        private void ViewTransactionsForSelectedNode()
        {
            ViewLedgerForSelectedNode();
        }

        private void PrintCoa()
        {
            BuildPrintGrid();
            // Use current fiscal-year dates from cache for print subtitle
            DateTime fyFromDate = _currentFiscalYearDates != null ? _currentFiscalYearDates.Item1 : UsersModal.fy_from_date;
            DateTime fyToDate = _currentFiscalYearDates != null ? _currentFiscalYearDates.Item2 : UsersModal.fy_to_date;

            var printer = new DGVPrinter
            {
                Title = "Chart of Accounts Manager",
                SubTitle = string.Format("{0} To {1}", fyFromDate.ToShortDateString(), fyToDate.ToShortDateString()),
                SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip,
                PageNumbers = true,
                PageNumberInHeader = false,
                PorportionalColumns = false,
                HeaderCellAlignment = StringAlignment.Near,
                Footer = "khybersoft.com",
                FooterSpacing = 15
            };
            printer.PrintPreviewDataGridView(_printGrid);
        }

        private void BuildPrintGrid()
        {
            var dt = new DataTable();
            dt.Columns.Add("Level");
            dt.Columns.Add("Code");
            dt.Columns.Add("Name");
            dt.Columns.Add("Debit");
            dt.Columns.Add("Credit");
            dt.Columns.Add("Balance");

            foreach (var node in EnumerateVisibleNodes(_tree.Nodes))
            {
                var tag = node.Tag as CoaNodeTag;
                if (tag == null) continue;
                var depthPrefix = new string(' ', Math.Max(0, tag.Level - 1) * 2);
                if (tag.Kind == CoaNodeKind.Group && _groups.ContainsKey(tag.Id))
                {
                    var group = _groups[tag.Id];
                    var agg = group.Aggregate;
                    dt.Rows.Add(depthPrefix + "Group", group.Code, group.Name, agg.Debit.ToString("N0"), agg.Credit.ToString("N0"), FormatBalanceSide(agg.Balance));
                }
                else if (tag.Kind == CoaNodeKind.Account && _accounts.ContainsKey(tag.Id))
                {
                    var account = _accounts[tag.Id];
                    var agg = account.Aggregate;
                    dt.Rows.Add(depthPrefix + "Account", account.Code, account.Name, agg.Debit.ToString("N0"), agg.Credit.ToString("N0"), FormatBalanceSide(agg.Balance));
                }
            }

            _printGrid.DataSource = dt;
        }

        private IEnumerable<TreeNode> EnumerateVisibleNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                yield return node;
                foreach (var child in EnumerateVisibleNodes(node.Nodes))
                {
                    yield return child;
                }
            }
        }

        private void SetupStandardChartOfAccounts()
        {
            if (MessageBox.Show("This will add common Saudi COA entries if they do not already exist. Continue?", "Setup Standard COA", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                return;

            int assetsType = FindAccountTypeId("asset");
            int liabilitiesType = FindAccountTypeId("liabil");
            int equityType = FindAccountTypeId("equity");
            int incomeType = FindAccountTypeId("income");
            int expenseType = FindAccountTypeId("expense");

            int assets = EnsureGroup(0, "1000", "Assets", assetsType, "Top-level asset accounts");
            int liabilities = EnsureGroup(0, "2000", "Liabilities", liabilitiesType, "Top-level liability accounts");
            int equity = EnsureGroup(0, "3000", "Equity", equityType, "Top-level equity accounts");
            int income = EnsureGroup(0, "4000", "Income", incomeType, "Top-level income accounts");
            int expenses = EnsureGroup(0, "5000", "Expenses", expenseType, "Top-level expense accounts");

            int currentAssets = EnsureGroup(assets, "1100", "Current Assets", assetsType, "Current assets");
            int fixedAssets = EnsureGroup(assets, "1200", "Fixed Assets", assetsType, "Fixed assets");
            int currentLiabilities = EnsureGroup(liabilities, "2100", "Current Liabilities", liabilitiesType, "Current liabilities");
            int retainedEarnings = EnsureGroup(equity, "3100", "Capital", equityType, "Owner's equity / capital");
            int sales = EnsureGroup(income, "4100", "Sales", incomeType, "Revenue accounts");
            int operatingExpenses = EnsureGroup(expenses, "5100", "Operating Expenses", expenseType, "Operating expense accounts");

            EnsureAccount(currentAssets, "1110", "Cash in Hand", "", "Cash on hand");
            EnsureAccount(currentAssets, "1120", "Bank Account", "", "Main bank account");
            EnsureAccount(currentLiabilities, "2110", "Accounts Payable", "", "Trade payables");
            EnsureAccount(retainedEarnings, "3110", "Owner Capital", "", "Owner capital");
            EnsureAccount(sales, "4110", "Sales Revenue", "", "Sales revenue");
            EnsureAccount(operatingExpenses, "5110", "Utilities Expense", "", "Utility expenses");
            EnsureAccount(operatingExpenses, "5120", "Rent Expense", "", "Rent expenses");

            ReloadAll();
        }

        private int EnsureGroup(int parentId, string code, string name, int accountTypeId, string description)
        {
            int existing = FindGroupIdByCode(code);
            if (existing > 0) return existing;

            var info = new GroupsModal
            {
                parent_id = parentId,
                account_type_id = accountTypeId,
                code = code,
                name = name,
                name_2 = name,
                description = description
            };
            return _groupsBll.Insert(info);
        }

        private int EnsureAccount(int groupId, string code, string name, string name2, string description)
        {
            int existing = FindAccountIdByCode(code);
            if (existing > 0) return existing;

            var info = new AccountsModal
            {
                group_id = groupId,
                code = code,
                name = name,
                name_2 = string.IsNullOrWhiteSpace(name2) ? name : name2,
                description = description,
                op_dr_balance = 0,
                op_cr_balance = 0
            };
            return _accountsBll.Insert(info);
        }

        private int FindAccountTypeId(string contains)
        {
            var dt = _generalBll.GetRecord("id,name", "acc_account_type");
            foreach (DataRow row in dt.Rows)
            {
                var name = GetString(row, "name");
                if (Contains(name, contains)) return GetInt(row, "id");
            }
            return 0;
        }

        private int FindGroupIdByCode(string code)
        {
            var dt = _generalBll.GetRecord("id", "acc_groups WHERE code = '" + Escape(code) + "'");
            return dt.Rows.Count > 0 ? GetInt(dt.Rows[0], "id") : 0;
        }

        private int FindAccountIdByCode(string code)
        {
            var dt = _generalBll.GetRecord("id", "acc_accounts WHERE branch_id = " + UsersModal.logged_in_branch_id + " AND code = '" + Escape(code) + "'");
            return dt.Rows.Count > 0 ? GetInt(dt.Rows[0], "id") : 0;
        }

        private int GetAccountTypeIdForGroup(int groupId)
        {
            if (_groups.ContainsKey(groupId)) return _groups[groupId].AccountTypeId;
            return 0;
        }

        private void BindCombo(ComboBox combo, DataTable table, bool includeEmptyRoot)
        {
            var dt = table.Copy();
            if (includeEmptyRoot)
            {
                var emptyRow = dt.NewRow();
                emptyRow[0] = 0;
                emptyRow[1] = "Root";
                dt.Rows.InsertAt(emptyRow, 0);
            }
            combo.DataSource = dt;
            combo.DisplayMember = dt.Columns.Contains("name") ? "name" : dt.Columns[1].ColumnName;
            combo.ValueMember = dt.Columns[0].ColumnName;
        }

        private void BindGroupCombo(ComboBox combo, DataTable table)
        {
            var dt = table.Copy();
            var emptyRow = dt.NewRow();
            emptyRow[0] = 0;
            emptyRow[3] = "Root";
            dt.Rows.InsertAt(emptyRow, 0);
            combo.DataSource = dt;
            combo.DisplayMember = dt.Columns.Contains("name") ? "name" : dt.Columns[1].ColumnName;
            combo.ValueMember = dt.Columns[0].ColumnName;
        }

        private string Escape(string value)
        {
            return (value ?? string.Empty).Replace("'", "''");
        }

        private bool Contains(string source, string term)
        {
            return !string.IsNullOrWhiteSpace(source) && !string.IsNullOrWhiteSpace(term) && source.IndexOf(term, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        private string GetString(DataRow row, string column)
        {
            return HasColumn(row, column) && row[column] != DBNull.Value ? Convert.ToString(row[column]) : string.Empty;
        }

        private int GetInt(DataRow row, string column)
        {
            return HasColumn(row, column) && row[column] != DBNull.Value ? Convert.ToInt32(row[column]) : 0;
        }

        private int ToInt(DataRow row, string column)
        {
            return GetInt(row, column);
        }

        private double GetDouble(DataRow row, string column)
        {
            return HasColumn(row, column) && row[column] != DBNull.Value ? Convert.ToDouble(row[column]) : 0d;
        }

        private bool GetBool(DataRow row, string column, bool defaultValue)
        {
            if (!HasColumn(row, column) || row[column] == DBNull.Value) return defaultValue;
            var value = row[column];
            if (value is bool) return (bool)value;
            bool result;
            return bool.TryParse(Convert.ToString(value), out result) ? result : defaultValue;
        }

        private DateTime? GetNullableDate(DataRow row, string column)
        {
            if (!HasColumn(row, column) || row[column] == DBNull.Value) return null;
            DateTime value;
            return DateTime.TryParse(Convert.ToString(row[column]), out value) ? (DateTime?)value : null;
        }

        private bool HasColumn(DataRow row, string column)
        {
            return row.Table.Columns.Contains(column);
        }

        private Tuple<DateTime, DateTime> GetActiveFiscalYearDates()
        {
            try
            {
                DataTable dt = _fiscalYearBll.GetActiveFiscalYear();
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    DateTime fromDate = GetNullableDate(row, "from_date") ?? UsersModal.fy_from_date;
                    DateTime toDate = GetNullableDate(row, "to_date") ?? UsersModal.fy_to_date;
                    return Tuple.Create(fromDate, toDate);
                }
            }
            catch
            {
                // If active fiscal year lookup fails, fall back to UsersModal cached values
            }

            // Fallback to UsersModal cached values if query fails or returns no results
            return Tuple.Create(UsersModal.fy_from_date, UsersModal.fy_to_date);
        }

        private void frm_coa_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }
                if (e.Control && e.KeyCode == Keys.F)
                {
                    _searchBox.Focus();
                    _searchBox.SelectAll();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            PrintCoa();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            LoadTree(_searchBox != null ? _searchBox.Text : string.Empty);
        }

        private sealed class CoaGroupInfo
        {
            public int Id;
            public string Code;
            public string Name;
            public string Name2;
            public int ParentId;
            public int AccountTypeId;
            public string Description;
            public bool IsActive;
            public CoaAggregate Aggregate = new CoaAggregate();
        }

        private sealed class CoaAccountInfo
        {
            public int Id;
            public int GroupId;
            public string Code;
            public string Name;
            public string Name2;
            public string Description;
            public double OpeningDebit;
            public double OpeningCredit;
            public DateTime? OpeningBalanceDate;
            public bool IsBankAccount;
            public bool IsCashAccount;
            public string BankName;
            public string BankBranch;
            public string AccountNo;
            public string Iban;
            public bool IsActive;
            public CoaAggregate Aggregate = new CoaAggregate();
        }

        private sealed class CoaAggregate
        {
            public double Debit;
            public double Credit;
            public double Balance;
            public int TransactionCount;
            public DateTime? LastTransactionDate;

            public void Add(CoaAggregate other)
            {
                if (other == null) return;
                Debit += other.Debit;
                Credit += other.Credit;
                Balance += other.Balance;
                TransactionCount += other.TransactionCount;
                if (other.LastTransactionDate.HasValue)
                {
                    if (!LastTransactionDate.HasValue || other.LastTransactionDate.Value > LastTransactionDate.Value)
                    {
                        LastTransactionDate = other.LastTransactionDate;
                    }
                }
            }
        }

        private enum CoaNodeKind
        {
            Group,
            Account
        }

        private sealed class CoaNodeTag
        {
            public CoaNodeKind Kind;
            public int Id;
            public int Level;
        }

        private void btn_codes_maintenance_Click(object sender, EventArgs e)
        {
            var form = new pos.Accounts.Maintenance.frm_codes_maintenance();
            form.ShowDialog();
        }
    }
}
