namespace pos
{
    partial class frm_coa
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frm_coa));
            this._splitContainer = new System.Windows.Forms.SplitContainer();
            this._leftPanel = new System.Windows.Forms.Panel();
            this._tree = new System.Windows.Forms.TreeView();
            this._toolStrip = new System.Windows.Forms.ToolStrip();
            this._btnAddGroup = new System.Windows.Forms.ToolStripButton();
            this._btnAddAccount = new System.Windows.Forms.ToolStripButton();
            this._toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this._searchLabel = new System.Windows.Forms.ToolStripLabel();
            this._searchBox = new System.Windows.Forms.ToolStripTextBox();
            this._toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this._btnExpandAll = new System.Windows.Forms.ToolStripButton();
            this._btnCollapseAll = new System.Windows.Forms.ToolStripButton();
            this._btnPrint = new System.Windows.Forms.ToolStripButton();
            this._btnSetupStandard = new System.Windows.Forms.ToolStripButton();
            this._rightPanel = new System.Windows.Forms.Panel();
            this._detailsTabs = new System.Windows.Forms.TabControl();
            this._groupTab = new System.Windows.Forms.TabPage();
            this._groupLayout = new System.Windows.Forms.TableLayoutPanel();
            this._lblGroupCode = new System.Windows.Forms.Label();
            this._txtGroupCode = new System.Windows.Forms.TextBox();
            this._lblGroupName = new System.Windows.Forms.Label();
            this._txtGroupName = new System.Windows.Forms.TextBox();
            this._lblGroupParent = new System.Windows.Forms.Label();
            this._cmbGroupParent = new System.Windows.Forms.ComboBox();
            this._lblGroupType = new System.Windows.Forms.Label();
            this._cmbGroupType = new System.Windows.Forms.ComboBox();
            this._lblGroupDescription = new System.Windows.Forms.Label();
            this._txtGroupDescription = new System.Windows.Forms.TextBox();
            this._lblGroupStatus = new System.Windows.Forms.Label();
            this._chkGroupActive = new System.Windows.Forms.CheckBox();
            this._accountTab = new System.Windows.Forms.TabPage();
            this._bankGroup = new System.Windows.Forms.GroupBox();
            this._txtIban = new System.Windows.Forms.TextBox();
            this._txtBankAccountNo = new System.Windows.Forms.TextBox();
            this._txtBankBranch = new System.Windows.Forms.TextBox();
            this._txtBankName = new System.Windows.Forms.TextBox();
            this._accountLayout = new System.Windows.Forms.TableLayoutPanel();
            this._lblAccountCode = new System.Windows.Forms.Label();
            this._txtAccountCode = new System.Windows.Forms.TextBox();
            this._lblAccountName = new System.Windows.Forms.Label();
            this._txtAccountName = new System.Windows.Forms.TextBox();
            this._lblAccountParent = new System.Windows.Forms.Label();
            this._cmbAccountParent = new System.Windows.Forms.ComboBox();
            this._lblAccountType = new System.Windows.Forms.Label();
            this._cmbAccountType = new System.Windows.Forms.ComboBox();
            this._lblOpeningDebit = new System.Windows.Forms.Label();
            this._txtOpeningDebit = new System.Windows.Forms.TextBox();
            this._lblOpeningCredit = new System.Windows.Forms.Label();
            this._txtOpeningCredit = new System.Windows.Forms.TextBox();
            this._lblOpeningDate = new System.Windows.Forms.Label();
            this._dtpOpeningBalanceDate = new System.Windows.Forms.DateTimePicker();
            this._lblAccountDescription = new System.Windows.Forms.Label();
            this._txtAccountDescription = new System.Windows.Forms.TextBox();
            this._lblAccountStatus = new System.Windows.Forms.Label();
            this._chkAccountActive = new System.Windows.Forms.CheckBox();
            this._chkIsCashAccount = new System.Windows.Forms.CheckBox();
            this._chkIsBankAccount = new System.Windows.Forms.CheckBox();
            this._footerButtons = new System.Windows.Forms.FlowLayoutPanel();
            this._btnEditSelected = new System.Windows.Forms.Button();
            this._btnRefresh = new System.Windows.Forms.Button();
            this._btnViewLedger = new System.Windows.Forms.Button();
            this._btnViewTransactions = new System.Windows.Forms.Button();
            this._btnNewAccount = new System.Windows.Forms.Button();
            this._btnNewGroup = new System.Windows.Forms.Button();
            this.btn_codes_maintenance = new System.Windows.Forms.Button();
            this._summaryGroup = new System.Windows.Forms.GroupBox();
            this._summaryLayout = new System.Windows.Forms.TableLayoutPanel();
            this._summaryNodeTypeLabel = new System.Windows.Forms.Label();
            this._summaryNodeType = new System.Windows.Forms.TextBox();
            this._summaryCodeLabel = new System.Windows.Forms.Label();
            this._summaryCode = new System.Windows.Forms.TextBox();
            this._summaryNameLabel = new System.Windows.Forms.Label();
            this._summaryName = new System.Windows.Forms.TextBox();
            this._lblSummaryDebit = new System.Windows.Forms.Label();
            this._summaryDebit = new System.Windows.Forms.TextBox();
            this._lblSummaryCredit = new System.Windows.Forms.Label();
            this._summaryCredit = new System.Windows.Forms.TextBox();
            this._lblSummaryBalanceMeta = new System.Windows.Forms.Label();
            this._summaryCompositePanel = new System.Windows.Forms.Panel();
            this._rightHeaderPanel = new System.Windows.Forms.Panel();
            this._subtitleLabel = new System.Windows.Forms.Label();
            this._titleLabel = new System.Windows.Forms.Label();
            this._summaryBalance = new System.Windows.Forms.TextBox();
            this._summaryLastTxn = new System.Windows.Forms.TextBox();
            this._summaryTxnCount = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).BeginInit();
            this._splitContainer.Panel1.SuspendLayout();
            this._splitContainer.Panel2.SuspendLayout();
            this._splitContainer.SuspendLayout();
            this._leftPanel.SuspendLayout();
            this._toolStrip.SuspendLayout();
            this._rightPanel.SuspendLayout();
            this._detailsTabs.SuspendLayout();
            this._groupTab.SuspendLayout();
            this._groupLayout.SuspendLayout();
            this._accountTab.SuspendLayout();
            this._bankGroup.SuspendLayout();
            this._accountLayout.SuspendLayout();
            this._footerButtons.SuspendLayout();
            this._summaryGroup.SuspendLayout();
            this._summaryLayout.SuspendLayout();
            this._rightHeaderPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // _splitContainer
            // 
            resources.ApplyResources(this._splitContainer, "_splitContainer");
            this._splitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this._splitContainer.Name = "_splitContainer";
            // 
            // _splitContainer.Panel1
            // 
            this._splitContainer.Panel1.Controls.Add(this._leftPanel);
            // 
            // _splitContainer.Panel2
            // 
            this._splitContainer.Panel2.Controls.Add(this._rightPanel);
            // 
            // _leftPanel
            // 
            this._leftPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this._leftPanel.Controls.Add(this._tree);
            this._leftPanel.Controls.Add(this._toolStrip);
            resources.ApplyResources(this._leftPanel, "_leftPanel");
            this._leftPanel.Name = "_leftPanel";
            // 
            // _tree
            // 
            this._tree.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(245)))), ((int)(((byte)(245)))));
            this._tree.BorderStyle = System.Windows.Forms.BorderStyle.None;
            resources.ApplyResources(this._tree, "_tree");
            this._tree.HideSelection = false;
            this._tree.Name = "_tree";
            // 
            // _toolStrip
            // 
            this._toolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this._toolStrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this._toolStrip.ImageScalingSize = new System.Drawing.Size(20, 20);
            this._toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this._btnAddGroup,
            this._btnAddAccount,
            this._toolStripSeparator1,
            this._searchLabel,
            this._searchBox,
            this._toolStripSeparator2,
            this._btnExpandAll,
            this._btnCollapseAll,
            this._btnPrint,
            this._btnSetupStandard});
            resources.ApplyResources(this._toolStrip, "_toolStrip");
            this._toolStrip.Name = "_toolStrip";
            this._toolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // _btnAddGroup
            // 
            this._btnAddGroup.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._btnAddGroup.ForeColor = System.Drawing.Color.White;
            this._btnAddGroup.Name = "_btnAddGroup";
            resources.ApplyResources(this._btnAddGroup, "_btnAddGroup");
            // 
            // _btnAddAccount
            // 
            this._btnAddAccount.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._btnAddAccount.ForeColor = System.Drawing.Color.White;
            this._btnAddAccount.Name = "_btnAddAccount";
            resources.ApplyResources(this._btnAddAccount, "_btnAddAccount");
            // 
            // _toolStripSeparator1
            // 
            this._toolStripSeparator1.Name = "_toolStripSeparator1";
            resources.ApplyResources(this._toolStripSeparator1, "_toolStripSeparator1");
            // 
            // _searchLabel
            // 
            this._searchLabel.ForeColor = System.Drawing.Color.White;
            this._searchLabel.Name = "_searchLabel";
            resources.ApplyResources(this._searchLabel, "_searchLabel");
            // 
            // _searchBox
            // 
            resources.ApplyResources(this._searchBox, "_searchBox");
            this._searchBox.BackColor = System.Drawing.Color.White;
            this._searchBox.Name = "_searchBox";
            // 
            // _toolStripSeparator2
            // 
            this._toolStripSeparator2.Name = "_toolStripSeparator2";
            resources.ApplyResources(this._toolStripSeparator2, "_toolStripSeparator2");
            // 
            // _btnExpandAll
            // 
            this._btnExpandAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._btnExpandAll.ForeColor = System.Drawing.Color.White;
            this._btnExpandAll.Name = "_btnExpandAll";
            resources.ApplyResources(this._btnExpandAll, "_btnExpandAll");
            // 
            // _btnCollapseAll
            // 
            this._btnCollapseAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._btnCollapseAll.ForeColor = System.Drawing.Color.White;
            this._btnCollapseAll.Name = "_btnCollapseAll";
            resources.ApplyResources(this._btnCollapseAll, "_btnCollapseAll");
            // 
            // _btnPrint
            // 
            this._btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._btnPrint.ForeColor = System.Drawing.Color.White;
            this._btnPrint.Name = "_btnPrint";
            resources.ApplyResources(this._btnPrint, "_btnPrint");
            // 
            // _btnSetupStandard
            // 
            this._btnSetupStandard.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this._btnSetupStandard.ForeColor = System.Drawing.Color.White;
            this._btnSetupStandard.Name = "_btnSetupStandard";
            resources.ApplyResources(this._btnSetupStandard, "_btnSetupStandard");
            // 
            // _rightPanel
            // 
            this._rightPanel.BackColor = System.Drawing.Color.White;
            this._rightPanel.Controls.Add(this._detailsTabs);
            this._rightPanel.Controls.Add(this._footerButtons);
            this._rightPanel.Controls.Add(this._summaryGroup);
            this._rightPanel.Controls.Add(this._rightHeaderPanel);
            resources.ApplyResources(this._rightPanel, "_rightPanel");
            this._rightPanel.Name = "_rightPanel";
            // 
            // _detailsTabs
            // 
            this._detailsTabs.Controls.Add(this._groupTab);
            this._detailsTabs.Controls.Add(this._accountTab);
            resources.ApplyResources(this._detailsTabs, "_detailsTabs");
            this._detailsTabs.Name = "_detailsTabs";
            this._detailsTabs.SelectedIndex = 0;
            // 
            // _groupTab
            // 
            this._groupTab.BackColor = System.Drawing.Color.White;
            this._groupTab.Controls.Add(this._groupLayout);
            resources.ApplyResources(this._groupTab, "_groupTab");
            this._groupTab.Name = "_groupTab";
            // 
            // _groupLayout
            // 
            resources.ApplyResources(this._groupLayout, "_groupLayout");
            this._groupLayout.Controls.Add(this._lblGroupCode, 0, 0);
            this._groupLayout.Controls.Add(this._txtGroupCode, 1, 0);
            this._groupLayout.Controls.Add(this._lblGroupName, 0, 1);
            this._groupLayout.Controls.Add(this._txtGroupName, 1, 1);
            this._groupLayout.Controls.Add(this._lblGroupParent, 0, 2);
            this._groupLayout.Controls.Add(this._cmbGroupParent, 1, 2);
            this._groupLayout.Controls.Add(this._lblGroupType, 0, 3);
            this._groupLayout.Controls.Add(this._cmbGroupType, 1, 3);
            this._groupLayout.Controls.Add(this._lblGroupDescription, 0, 4);
            this._groupLayout.Controls.Add(this._txtGroupDescription, 1, 4);
            this._groupLayout.Controls.Add(this._lblGroupStatus, 0, 5);
            this._groupLayout.Controls.Add(this._chkGroupActive, 1, 5);
            this._groupLayout.Name = "_groupLayout";
            // 
            // _lblGroupCode
            // 
            resources.ApplyResources(this._lblGroupCode, "_lblGroupCode");
            this._lblGroupCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblGroupCode.Name = "_lblGroupCode";
            // 
            // _txtGroupCode
            // 
            resources.ApplyResources(this._txtGroupCode, "_txtGroupCode");
            this._txtGroupCode.Name = "_txtGroupCode";
            this._txtGroupCode.ReadOnly = true;
            // 
            // _lblGroupName
            // 
            resources.ApplyResources(this._lblGroupName, "_lblGroupName");
            this._lblGroupName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblGroupName.Name = "_lblGroupName";
            // 
            // _txtGroupName
            // 
            resources.ApplyResources(this._txtGroupName, "_txtGroupName");
            this._txtGroupName.Name = "_txtGroupName";
            this._txtGroupName.ReadOnly = true;
            // 
            // _lblGroupParent
            // 
            resources.ApplyResources(this._lblGroupParent, "_lblGroupParent");
            this._lblGroupParent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblGroupParent.Name = "_lblGroupParent";
            // 
            // _cmbGroupParent
            // 
            resources.ApplyResources(this._cmbGroupParent, "_cmbGroupParent");
            this._cmbGroupParent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbGroupParent.FormattingEnabled = true;
            this._cmbGroupParent.Name = "_cmbGroupParent";
            // 
            // _lblGroupType
            // 
            resources.ApplyResources(this._lblGroupType, "_lblGroupType");
            this._lblGroupType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblGroupType.Name = "_lblGroupType";
            // 
            // _cmbGroupType
            // 
            resources.ApplyResources(this._cmbGroupType, "_cmbGroupType");
            this._cmbGroupType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbGroupType.FormattingEnabled = true;
            this._cmbGroupType.Name = "_cmbGroupType";
            // 
            // _lblGroupDescription
            // 
            resources.ApplyResources(this._lblGroupDescription, "_lblGroupDescription");
            this._lblGroupDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblGroupDescription.Name = "_lblGroupDescription";
            // 
            // _txtGroupDescription
            // 
            resources.ApplyResources(this._txtGroupDescription, "_txtGroupDescription");
            this._txtGroupDescription.Name = "_txtGroupDescription";
            this._txtGroupDescription.ReadOnly = true;
            // 
            // _lblGroupStatus
            // 
            resources.ApplyResources(this._lblGroupStatus, "_lblGroupStatus");
            this._lblGroupStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblGroupStatus.Name = "_lblGroupStatus";
            // 
            // _chkGroupActive
            // 
            resources.ApplyResources(this._chkGroupActive, "_chkGroupActive");
            this._chkGroupActive.Name = "_chkGroupActive";
            this._chkGroupActive.UseVisualStyleBackColor = true;
            // 
            // _accountTab
            // 
            this._accountTab.BackColor = System.Drawing.Color.White;
            this._accountTab.Controls.Add(this._bankGroup);
            this._accountTab.Controls.Add(this._accountLayout);
            this._accountTab.Controls.Add(this._chkIsCashAccount);
            this._accountTab.Controls.Add(this._chkIsBankAccount);
            resources.ApplyResources(this._accountTab, "_accountTab");
            this._accountTab.Name = "_accountTab";
            // 
            // _bankGroup
            // 
            this._bankGroup.BackColor = System.Drawing.Color.White;
            this._bankGroup.Controls.Add(this._txtIban);
            this._bankGroup.Controls.Add(this._txtBankAccountNo);
            this._bankGroup.Controls.Add(this._txtBankBranch);
            this._bankGroup.Controls.Add(this._txtBankName);
            resources.ApplyResources(this._bankGroup, "_bankGroup");
            this._bankGroup.Name = "_bankGroup";
            this._bankGroup.TabStop = false;
            // 
            // _txtIban
            // 
            resources.ApplyResources(this._txtIban, "_txtIban");
            this._txtIban.Name = "_txtIban";
            this._txtIban.ReadOnly = true;
            // 
            // _txtBankAccountNo
            // 
            resources.ApplyResources(this._txtBankAccountNo, "_txtBankAccountNo");
            this._txtBankAccountNo.Name = "_txtBankAccountNo";
            this._txtBankAccountNo.ReadOnly = true;
            // 
            // _txtBankBranch
            // 
            resources.ApplyResources(this._txtBankBranch, "_txtBankBranch");
            this._txtBankBranch.Name = "_txtBankBranch";
            this._txtBankBranch.ReadOnly = true;
            // 
            // _txtBankName
            // 
            resources.ApplyResources(this._txtBankName, "_txtBankName");
            this._txtBankName.Name = "_txtBankName";
            this._txtBankName.ReadOnly = true;
            // 
            // _accountLayout
            // 
            resources.ApplyResources(this._accountLayout, "_accountLayout");
            this._accountLayout.Controls.Add(this._lblAccountCode, 0, 0);
            this._accountLayout.Controls.Add(this._txtAccountCode, 1, 0);
            this._accountLayout.Controls.Add(this._lblAccountName, 0, 1);
            this._accountLayout.Controls.Add(this._txtAccountName, 1, 1);
            this._accountLayout.Controls.Add(this._lblAccountParent, 0, 2);
            this._accountLayout.Controls.Add(this._cmbAccountParent, 1, 2);
            this._accountLayout.Controls.Add(this._lblAccountType, 0, 3);
            this._accountLayout.Controls.Add(this._cmbAccountType, 1, 3);
            this._accountLayout.Controls.Add(this._lblOpeningDebit, 0, 4);
            this._accountLayout.Controls.Add(this._txtOpeningDebit, 1, 4);
            this._accountLayout.Controls.Add(this._lblOpeningCredit, 0, 5);
            this._accountLayout.Controls.Add(this._txtOpeningCredit, 1, 5);
            this._accountLayout.Controls.Add(this._lblOpeningDate, 0, 6);
            this._accountLayout.Controls.Add(this._dtpOpeningBalanceDate, 1, 6);
            this._accountLayout.Controls.Add(this._lblAccountDescription, 0, 7);
            this._accountLayout.Controls.Add(this._txtAccountDescription, 1, 7);
            this._accountLayout.Controls.Add(this._lblAccountStatus, 0, 8);
            this._accountLayout.Controls.Add(this._chkAccountActive, 1, 8);
            this._accountLayout.Name = "_accountLayout";
            // 
            // _lblAccountCode
            // 
            resources.ApplyResources(this._lblAccountCode, "_lblAccountCode");
            this._lblAccountCode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblAccountCode.Name = "_lblAccountCode";
            // 
            // _txtAccountCode
            // 
            resources.ApplyResources(this._txtAccountCode, "_txtAccountCode");
            this._txtAccountCode.Name = "_txtAccountCode";
            this._txtAccountCode.ReadOnly = true;
            // 
            // _lblAccountName
            // 
            resources.ApplyResources(this._lblAccountName, "_lblAccountName");
            this._lblAccountName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblAccountName.Name = "_lblAccountName";
            // 
            // _txtAccountName
            // 
            resources.ApplyResources(this._txtAccountName, "_txtAccountName");
            this._txtAccountName.Name = "_txtAccountName";
            this._txtAccountName.ReadOnly = true;
            // 
            // _lblAccountParent
            // 
            resources.ApplyResources(this._lblAccountParent, "_lblAccountParent");
            this._lblAccountParent.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblAccountParent.Name = "_lblAccountParent";
            // 
            // _cmbAccountParent
            // 
            resources.ApplyResources(this._cmbAccountParent, "_cmbAccountParent");
            this._cmbAccountParent.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbAccountParent.FormattingEnabled = true;
            this._cmbAccountParent.Name = "_cmbAccountParent";
            // 
            // _lblAccountType
            // 
            resources.ApplyResources(this._lblAccountType, "_lblAccountType");
            this._lblAccountType.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblAccountType.Name = "_lblAccountType";
            // 
            // _cmbAccountType
            // 
            resources.ApplyResources(this._cmbAccountType, "_cmbAccountType");
            this._cmbAccountType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this._cmbAccountType.FormattingEnabled = true;
            this._cmbAccountType.Name = "_cmbAccountType";
            // 
            // _lblOpeningDebit
            // 
            resources.ApplyResources(this._lblOpeningDebit, "_lblOpeningDebit");
            this._lblOpeningDebit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblOpeningDebit.Name = "_lblOpeningDebit";
            // 
            // _txtOpeningDebit
            // 
            resources.ApplyResources(this._txtOpeningDebit, "_txtOpeningDebit");
            this._txtOpeningDebit.Name = "_txtOpeningDebit";
            this._txtOpeningDebit.ReadOnly = true;
            // 
            // _lblOpeningCredit
            // 
            resources.ApplyResources(this._lblOpeningCredit, "_lblOpeningCredit");
            this._lblOpeningCredit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblOpeningCredit.Name = "_lblOpeningCredit";
            // 
            // _txtOpeningCredit
            // 
            resources.ApplyResources(this._txtOpeningCredit, "_txtOpeningCredit");
            this._txtOpeningCredit.Name = "_txtOpeningCredit";
            this._txtOpeningCredit.ReadOnly = true;
            // 
            // _lblOpeningDate
            // 
            resources.ApplyResources(this._lblOpeningDate, "_lblOpeningDate");
            this._lblOpeningDate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblOpeningDate.Name = "_lblOpeningDate";
            // 
            // _dtpOpeningBalanceDate
            // 
            resources.ApplyResources(this._dtpOpeningBalanceDate, "_dtpOpeningBalanceDate");
            this._dtpOpeningBalanceDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this._dtpOpeningBalanceDate.Name = "_dtpOpeningBalanceDate";
            // 
            // _lblAccountDescription
            // 
            resources.ApplyResources(this._lblAccountDescription, "_lblAccountDescription");
            this._lblAccountDescription.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblAccountDescription.Name = "_lblAccountDescription";
            // 
            // _txtAccountDescription
            // 
            resources.ApplyResources(this._txtAccountDescription, "_txtAccountDescription");
            this._txtAccountDescription.Name = "_txtAccountDescription";
            this._txtAccountDescription.ReadOnly = true;
            // 
            // _lblAccountStatus
            // 
            resources.ApplyResources(this._lblAccountStatus, "_lblAccountStatus");
            this._lblAccountStatus.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblAccountStatus.Name = "_lblAccountStatus";
            // 
            // _chkAccountActive
            // 
            resources.ApplyResources(this._chkAccountActive, "_chkAccountActive");
            this._chkAccountActive.Name = "_chkAccountActive";
            this._chkAccountActive.UseVisualStyleBackColor = true;
            // 
            // _chkIsCashAccount
            // 
            resources.ApplyResources(this._chkIsCashAccount, "_chkIsCashAccount");
            this._chkIsCashAccount.Name = "_chkIsCashAccount";
            this._chkIsCashAccount.UseVisualStyleBackColor = true;
            // 
            // _chkIsBankAccount
            // 
            resources.ApplyResources(this._chkIsBankAccount, "_chkIsBankAccount");
            this._chkIsBankAccount.Name = "_chkIsBankAccount";
            this._chkIsBankAccount.UseVisualStyleBackColor = true;
            // 
            // _footerButtons
            // 
            this._footerButtons.Controls.Add(this._btnEditSelected);
            this._footerButtons.Controls.Add(this._btnRefresh);
            this._footerButtons.Controls.Add(this._btnViewLedger);
            this._footerButtons.Controls.Add(this._btnViewTransactions);
            this._footerButtons.Controls.Add(this._btnNewAccount);
            this._footerButtons.Controls.Add(this._btnNewGroup);
            this._footerButtons.Controls.Add(this.btn_codes_maintenance);
            resources.ApplyResources(this._footerButtons, "_footerButtons");
            this._footerButtons.Name = "_footerButtons";
            // 
            // _btnEditSelected
            // 
            resources.ApplyResources(this._btnEditSelected, "_btnEditSelected");
            this._btnEditSelected.BackColor = System.Drawing.Color.White;
            this._btnEditSelected.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this._btnEditSelected.Name = "_btnEditSelected";
            this._btnEditSelected.UseVisualStyleBackColor = true;
            // 
            // _btnRefresh
            // 
            resources.ApplyResources(this._btnRefresh, "_btnRefresh");
            this._btnRefresh.BackColor = System.Drawing.Color.White;
            this._btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this._btnRefresh.Name = "_btnRefresh";
            this._btnRefresh.UseVisualStyleBackColor = true;
            // 
            // _btnViewLedger
            // 
            resources.ApplyResources(this._btnViewLedger, "_btnViewLedger");
            this._btnViewLedger.BackColor = System.Drawing.Color.White;
            this._btnViewLedger.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this._btnViewLedger.Name = "_btnViewLedger";
            this._btnViewLedger.UseVisualStyleBackColor = true;
            // 
            // _btnViewTransactions
            // 
            resources.ApplyResources(this._btnViewTransactions, "_btnViewTransactions");
            this._btnViewTransactions.BackColor = System.Drawing.Color.White;
            this._btnViewTransactions.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this._btnViewTransactions.Name = "_btnViewTransactions";
            this._btnViewTransactions.UseVisualStyleBackColor = true;
            // 
            // _btnNewAccount
            // 
            resources.ApplyResources(this._btnNewAccount, "_btnNewAccount");
            this._btnNewAccount.BackColor = System.Drawing.Color.White;
            this._btnNewAccount.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this._btnNewAccount.Name = "_btnNewAccount";
            this._btnNewAccount.UseVisualStyleBackColor = true;
            // 
            // _btnNewGroup
            // 
            resources.ApplyResources(this._btnNewGroup, "_btnNewGroup");
            this._btnNewGroup.BackColor = System.Drawing.Color.White;
            this._btnNewGroup.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this._btnNewGroup.Name = "_btnNewGroup";
            this._btnNewGroup.UseVisualStyleBackColor = true;
            // 
            // btn_codes_maintenance
            // 
            resources.ApplyResources(this.btn_codes_maintenance, "btn_codes_maintenance");
            this.btn_codes_maintenance.BackColor = System.Drawing.Color.White;
            this.btn_codes_maintenance.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this.btn_codes_maintenance.Name = "btn_codes_maintenance";
            this.btn_codes_maintenance.UseVisualStyleBackColor = true;
            this.btn_codes_maintenance.Click += new System.EventHandler(this.btn_codes_maintenance_Click);
            // 
            // _summaryGroup
            // 
            this._summaryGroup.BackColor = System.Drawing.Color.White;
            this._summaryGroup.Controls.Add(this._summaryLayout);
            resources.ApplyResources(this._summaryGroup, "_summaryGroup");
            this._summaryGroup.Name = "_summaryGroup";
            this._summaryGroup.TabStop = false;
            // 
            // _summaryLayout
            // 
            resources.ApplyResources(this._summaryLayout, "_summaryLayout");
            this._summaryLayout.Controls.Add(this._summaryNodeTypeLabel, 0, 0);
            this._summaryLayout.Controls.Add(this._summaryNodeType, 1, 0);
            this._summaryLayout.Controls.Add(this._summaryCodeLabel, 2, 0);
            this._summaryLayout.Controls.Add(this._summaryCode, 3, 0);
            this._summaryLayout.Controls.Add(this._summaryNameLabel, 0, 1);
            this._summaryLayout.Controls.Add(this._summaryName, 1, 1);
            this._summaryLayout.Controls.Add(this._lblSummaryDebit, 2, 1);
            this._summaryLayout.Controls.Add(this._summaryDebit, 3, 1);
            this._summaryLayout.Controls.Add(this._lblSummaryCredit, 0, 2);
            this._summaryLayout.Controls.Add(this._summaryCredit, 1, 2);
            this._summaryLayout.Controls.Add(this._lblSummaryBalanceMeta, 2, 2);
            this._summaryLayout.Controls.Add(this._summaryCompositePanel, 3, 2);
            this._summaryLayout.Name = "_summaryLayout";
            // 
            // _summaryNodeTypeLabel
            // 
            resources.ApplyResources(this._summaryNodeTypeLabel, "_summaryNodeTypeLabel");
            this._summaryNodeTypeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._summaryNodeTypeLabel.Name = "_summaryNodeTypeLabel";
            // 
            // _summaryNodeType
            // 
            this._summaryNodeType.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._summaryNodeType, "_summaryNodeType");
            this._summaryNodeType.Name = "_summaryNodeType";
            this._summaryNodeType.ReadOnly = true;
            // 
            // _summaryCodeLabel
            // 
            resources.ApplyResources(this._summaryCodeLabel, "_summaryCodeLabel");
            this._summaryCodeLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._summaryCodeLabel.Name = "_summaryCodeLabel";
            // 
            // _summaryCode
            // 
            this._summaryCode.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._summaryCode, "_summaryCode");
            this._summaryCode.Name = "_summaryCode";
            this._summaryCode.ReadOnly = true;
            // 
            // _summaryNameLabel
            // 
            resources.ApplyResources(this._summaryNameLabel, "_summaryNameLabel");
            this._summaryNameLabel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._summaryNameLabel.Name = "_summaryNameLabel";
            // 
            // _summaryName
            // 
            this._summaryName.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._summaryName, "_summaryName");
            this._summaryName.Name = "_summaryName";
            this._summaryName.ReadOnly = true;
            // 
            // _lblSummaryDebit
            // 
            resources.ApplyResources(this._lblSummaryDebit, "_lblSummaryDebit");
            this._lblSummaryDebit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblSummaryDebit.Name = "_lblSummaryDebit";
            // 
            // _summaryDebit
            // 
            this._summaryDebit.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._summaryDebit, "_summaryDebit");
            this._summaryDebit.Name = "_summaryDebit";
            this._summaryDebit.ReadOnly = true;
            // 
            // _lblSummaryCredit
            // 
            resources.ApplyResources(this._lblSummaryCredit, "_lblSummaryCredit");
            this._lblSummaryCredit.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblSummaryCredit.Name = "_lblSummaryCredit";
            // 
            // _summaryCredit
            // 
            this._summaryCredit.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._summaryCredit, "_summaryCredit");
            this._summaryCredit.Name = "_summaryCredit";
            this._summaryCredit.ReadOnly = true;
            // 
            // _lblSummaryBalanceMeta
            // 
            resources.ApplyResources(this._lblSummaryBalanceMeta, "_lblSummaryBalanceMeta");
            this._lblSummaryBalanceMeta.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(33)))), ((int)(((byte)(37)))), ((int)(((byte)(41)))));
            this._lblSummaryBalanceMeta.Name = "_lblSummaryBalanceMeta";
            // 
            // _summaryCompositePanel
            // 
            resources.ApplyResources(this._summaryCompositePanel, "_summaryCompositePanel");
            this._summaryCompositePanel.Name = "_summaryCompositePanel";
            // 
            // _rightHeaderPanel
            // 
            this._rightHeaderPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(21)))), ((int)(((byte)(101)))), ((int)(((byte)(192)))));
            this._rightHeaderPanel.Controls.Add(this._subtitleLabel);
            this._rightHeaderPanel.Controls.Add(this._titleLabel);
            resources.ApplyResources(this._rightHeaderPanel, "_rightHeaderPanel");
            this._rightHeaderPanel.Name = "_rightHeaderPanel";
            // 
            // _subtitleLabel
            // 
            resources.ApplyResources(this._subtitleLabel, "_subtitleLabel");
            this._subtitleLabel.ForeColor = System.Drawing.Color.WhiteSmoke;
            this._subtitleLabel.Name = "_subtitleLabel";
            // 
            // _titleLabel
            // 
            resources.ApplyResources(this._titleLabel, "_titleLabel");
            this._titleLabel.ForeColor = System.Drawing.Color.White;
            this._titleLabel.Name = "_titleLabel";
            // 
            // _summaryBalance
            // 
            this._summaryBalance.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._summaryBalance, "_summaryBalance");
            this._summaryBalance.Name = "_summaryBalance";
            this._summaryBalance.ReadOnly = true;
            // 
            // _summaryLastTxn
            // 
            this._summaryLastTxn.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._summaryLastTxn, "_summaryLastTxn");
            this._summaryLastTxn.Name = "_summaryLastTxn";
            this._summaryLastTxn.ReadOnly = true;
            // 
            // _summaryTxnCount
            // 
            this._summaryTxnCount.BackColor = System.Drawing.Color.White;
            resources.ApplyResources(this._summaryTxnCount, "_summaryTxnCount");
            this._summaryTxnCount.Name = "_summaryTxnCount";
            this._summaryTxnCount.ReadOnly = true;
            // 
            // frm_coa
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this._splitContainer);
            this.KeyPreview = true;
            this.Name = "frm_coa";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_coa_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frm_coa_KeyDown);
            this._splitContainer.Panel1.ResumeLayout(false);
            this._splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this._splitContainer)).EndInit();
            this._splitContainer.ResumeLayout(false);
            this._leftPanel.ResumeLayout(false);
            this._leftPanel.PerformLayout();
            this._toolStrip.ResumeLayout(false);
            this._toolStrip.PerformLayout();
            this._rightPanel.ResumeLayout(false);
            this._detailsTabs.ResumeLayout(false);
            this._groupTab.ResumeLayout(false);
            this._groupLayout.ResumeLayout(false);
            this._groupLayout.PerformLayout();
            this._accountTab.ResumeLayout(false);
            this._accountTab.PerformLayout();
            this._bankGroup.ResumeLayout(false);
            this._bankGroup.PerformLayout();
            this._accountLayout.ResumeLayout(false);
            this._accountLayout.PerformLayout();
            this._footerButtons.ResumeLayout(false);
            this._footerButtons.PerformLayout();
            this._summaryGroup.ResumeLayout(false);
            this._summaryLayout.ResumeLayout(false);
            this._summaryLayout.PerformLayout();
            this._rightHeaderPanel.ResumeLayout(false);
            this._rightHeaderPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.Panel _leftPanel;
        private System.Windows.Forms.ToolStrip _toolStrip;
        private System.Windows.Forms.ToolStripButton _btnAddGroup;
        private System.Windows.Forms.ToolStripButton _btnAddAccount;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel _searchLabel;
        private System.Windows.Forms.ToolStripTextBox _searchBox;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton _btnExpandAll;
        private System.Windows.Forms.ToolStripButton _btnCollapseAll;
        private System.Windows.Forms.ToolStripButton _btnPrint;
        private System.Windows.Forms.ToolStripButton _btnSetupStandard;
        private System.Windows.Forms.TreeView _tree;
        private System.Windows.Forms.Panel _rightPanel;
        private System.Windows.Forms.TabControl _detailsTabs;
        private System.Windows.Forms.TabPage _groupTab;
        private System.Windows.Forms.TabPage _accountTab;
        private System.Windows.Forms.FlowLayoutPanel _footerButtons;
        private System.Windows.Forms.Button _btnEditSelected;
        private System.Windows.Forms.Button _btnRefresh;
        private System.Windows.Forms.Button _btnViewLedger;
        private System.Windows.Forms.Button _btnViewTransactions;
        private System.Windows.Forms.Button _btnNewAccount;
        private System.Windows.Forms.Button _btnNewGroup;
        private System.Windows.Forms.GroupBox _summaryGroup;
        private System.Windows.Forms.TableLayoutPanel _summaryLayout;
        private System.Windows.Forms.Label _summaryNodeTypeLabel;
        private System.Windows.Forms.TextBox _summaryNodeType;
        private System.Windows.Forms.Label _summaryCodeLabel;
        private System.Windows.Forms.TextBox _summaryCode;
        private System.Windows.Forms.Label _summaryNameLabel;
        private System.Windows.Forms.TextBox _summaryName;
        private System.Windows.Forms.TextBox _summaryDebit;
        private System.Windows.Forms.TextBox _summaryCredit;
        private System.Windows.Forms.TextBox _summaryBalance;
        private System.Windows.Forms.TextBox _summaryLastTxn;
        private System.Windows.Forms.TextBox _summaryTxnCount;
        private System.Windows.Forms.Panel _summaryCompositePanel;
        private System.Windows.Forms.Panel _rightHeaderPanel;
        private System.Windows.Forms.Label _titleLabel;
        private System.Windows.Forms.Label _subtitleLabel;
        private System.Windows.Forms.TableLayoutPanel _groupLayout;
        private System.Windows.Forms.TextBox _txtGroupCode;
        private System.Windows.Forms.TextBox _txtGroupName;
        private System.Windows.Forms.ComboBox _cmbGroupParent;
        private System.Windows.Forms.ComboBox _cmbGroupType;
        private System.Windows.Forms.TextBox _txtGroupDescription;
        private System.Windows.Forms.CheckBox _chkGroupActive;
        private System.Windows.Forms.TableLayoutPanel _accountLayout;
        private System.Windows.Forms.TextBox _txtAccountCode;
        private System.Windows.Forms.TextBox _txtAccountName;
        private System.Windows.Forms.ComboBox _cmbAccountParent;
        private System.Windows.Forms.ComboBox _cmbAccountType;
        private System.Windows.Forms.TextBox _txtOpeningDebit;
        private System.Windows.Forms.TextBox _txtOpeningCredit;
        private System.Windows.Forms.DateTimePicker _dtpOpeningBalanceDate;
        private System.Windows.Forms.TextBox _txtAccountDescription;
        private System.Windows.Forms.CheckBox _chkAccountActive;
        private System.Windows.Forms.GroupBox _bankGroup;
        private System.Windows.Forms.CheckBox _chkIsBankAccount;
        private System.Windows.Forms.CheckBox _chkIsCashAccount;
        private System.Windows.Forms.TextBox _txtBankName;
        private System.Windows.Forms.TextBox _txtBankBranch;
        private System.Windows.Forms.TextBox _txtBankAccountNo;
        private System.Windows.Forms.TextBox _txtIban;
        private System.Windows.Forms.Label _lblSummaryDebit;
        private System.Windows.Forms.Label _lblSummaryCredit;
        private System.Windows.Forms.Label _lblSummaryBalanceMeta;
        private System.Windows.Forms.Label _lblGroupCode;
        private System.Windows.Forms.Label _lblGroupName;
        private System.Windows.Forms.Label _lblGroupParent;
        private System.Windows.Forms.Label _lblGroupType;
        private System.Windows.Forms.Label _lblGroupDescription;
        private System.Windows.Forms.Label _lblGroupStatus;
        private System.Windows.Forms.Label _lblAccountCode;
        private System.Windows.Forms.Label _lblAccountName;
        private System.Windows.Forms.Label _lblAccountParent;
        private System.Windows.Forms.Label _lblAccountType;
        private System.Windows.Forms.Label _lblOpeningDebit;
        private System.Windows.Forms.Label _lblOpeningCredit;
        private System.Windows.Forms.Label _lblOpeningDate;
        private System.Windows.Forms.Label _lblAccountDescription;
        private System.Windows.Forms.Label _lblAccountStatus;
        private System.Windows.Forms.Button btn_codes_maintenance;
    }
}
