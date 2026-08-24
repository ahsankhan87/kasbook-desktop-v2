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
    /// Cost Center Allocation Rules Form - Define expense allocation rules
    /// </summary>
    public partial class frm_allocation_rules : Form
    {
        private CostCenterBLL bll = new CostCenterBLL();

        public frm_allocation_rules()
        {
            // Set RTL mode based on user language
            bool isArabic = string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase);
            this.RightToLeft = isArabic ? RightToLeft.Yes : RightToLeft.No;
            this.RightToLeftLayout = isArabic;

            InitializeComponent();
            InitializeUI();
        }

        /// <summary>
        /// Translates text based on current user language (Arabic/English).
        /// </summary>
        private string T(string englishText, string arabicText)
        {
            return string.Equals(UsersModal.logged_in_lang, "ar-SA", StringComparison.OrdinalIgnoreCase) 
                ? arabicText 
                : englishText;
        }

        private void InitializeUI()
        {
            // Translate form title
            this.Text = T("Allocation Rules", "قواعد التوزيع");

            // Translate UI labels and buttons
            lblTitle.Text = T("Allocation Rules & Auto-Allocation", "قواعد التوزيع والتوزيع التلقائي");
            lblName.Text = T("Rule Name:", "اسم القاعدة:");
            lblSourceAccount.Text = T("Source Account:", "حساب المصدر:");
            lblTargetCC.Text = T("Target Cost Center:", "مركز التكلفة المستهدف:");
            lblMethod.Text = T("Allocation Method:", "طريقة التوزيع:");
            lblPercent.Text = T("Allocation Percent:", "نسبة التوزيع:");
            chkActive.Text = T("Active", "نشط");
            lblPeriod.Text = T("Period:", "الفترة:");
            btnNew.Text = T("New", "جديد");
            btnSave.Text = T("Save", "حفظ");
            btnCancel.Text = T("Cancel", "إلغاء");
            btnDelete.Text = T("Delete", "حذف");
            btnRunAllocation.Text = T("Run Allocation", "تشغيل التوزيع");
            pnlAllocation.Text = T("Execute Allocation", "تنفيذ التوزيع");

            // Apply theme
            AppTheme.Apply(this);

            // Setup event handlers
            btnNew.Click += BtnNew_Click;
            btnSave.Click += BtnSave_Click;
            btnCancel.Click += BtnCancel_Click;
            btnDelete.Click += BtnDelete_Click;
            btnRunAllocation.Click += BtnRunAllocation_Click;
            dgvRules.CellDoubleClick += DgvRules_CellDoubleClick;

            // Setup dropdowns
            SetupMethodDropdown();
            SetupAccountDropdown();
            SetupCostCenterDropdown();

            LoadRules();
            EnableEditControls(false);
        }

        private void SetupMethodDropdown()
        {
            cmbMethod.Items.Clear();
            cmbMethod.Items.Add("FIXED_PCT");
            cmbMethod.Items.Add("HEADCOUNT");
            cmbMethod.Items.Add("REVENUE");
            cmbMethod.SelectedIndex = 0;
        }

        private void SetupAccountDropdown()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                {
                    cn.Open();
                    const string sql = @"
                        SELECT id, CONCAT(code, ' — ', name) AS display_text
                        FROM dbo.acc_accounts
                        WHERE group_id IN (SELECT id FROM dbo.acc_groups WHERE account_type_id IN 
                          (SELECT id FROM dbo.acc_account_type WHERE name LIKE '%Expense%'))
                        ORDER BY code;";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                    {
                        using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                cmbSourceAccount.DataSource = dt;
                cmbSourceAccount.DisplayMember = "display_text";
                cmbSourceAccount.ValueMember = "id";
                cmbSourceAccount.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{T("Error loading accounts:", "خطأ في تحميل الحسابات:")} {ex.Message}", T("Error", "خطأ"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupCostCenterDropdown()
        {
            try
            {
                DataTable dt = bll.GetBranchDropdown();
                cmbTargetCostCenter.DataSource = dt;
                cmbTargetCostCenter.DisplayMember = "display_text";
                cmbTargetCostCenter.ValueMember = "id";
                cmbTargetCostCenter.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{T("Error loading cost centers:", "خطأ في تحميل مراكز التكلفة:")} {ex.Message}", T("Error", "خطأ"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadRules()
        {
            try
            {
                DataTable dt = new DataTable();
                using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                {
                    cn.Open();
                    const string sql = @"
SELECT
    a.alloc_id,
    a.alloc_name,
    acc.code AS source_account_code,
    acc.name AS source_account_name,
    c.cc_code AS target_cc_code,
    c.cc_name AS target_cc_name,
    a.allocation_percent,
    a.allocation_method,
    a.is_active
FROM dbo.acc_cost_center_allocations a
INNER JOIN dbo.acc_accounts acc ON acc.id = a.source_acc_id
INNER JOIN dbo.acc_cost_centers c ON c.cc_id = a.cc_id
ORDER BY a.alloc_name;";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                    {
                        using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                dgvRules.DataSource = dt;
                dgvRules.AutoResizeColumns();

                // Translate grid column headers
                if (dgvRules.Columns.Contains("alloc_id"))
                    dgvRules.Columns["alloc_id"].HeaderText = T("ID", "رقم");
                if (dgvRules.Columns.Contains("alloc_name"))
                    dgvRules.Columns["alloc_name"].HeaderText = T("Rule Name", "اسم القاعدة");
                if (dgvRules.Columns.Contains("source_account_code"))
                    dgvRules.Columns["source_account_code"].HeaderText = T("Source Code", "رمز المصدر");
                if (dgvRules.Columns.Contains("source_account_name"))
                    dgvRules.Columns["source_account_name"].HeaderText = T("Source Account", "حساب المصدر");
                if (dgvRules.Columns.Contains("target_cc_code"))
                    dgvRules.Columns["target_cc_code"].HeaderText = T("CC Code", "رمز مركز التكلفة");
                if (dgvRules.Columns.Contains("target_cc_name"))
                    dgvRules.Columns["target_cc_name"].HeaderText = T("Cost Center", "مركز التكلفة");
                if (dgvRules.Columns.Contains("allocation_percent"))
                    dgvRules.Columns["allocation_percent"].HeaderText = T("Percent", "النسبة");
                if (dgvRules.Columns.Contains("allocation_method"))
                    dgvRules.Columns["allocation_method"].HeaderText = T("Method", "الطريقة");
                if (dgvRules.Columns.Contains("is_active"))
                    dgvRules.Columns["is_active"].HeaderText = T("Active", "نشط");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{T("Error loading rules:", "خطأ في تحميل القواعد:")} {ex.Message}", T("Error", "خطأ"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnNew_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableEditControls(true);
            txtName.Focus();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
                return;

            try
            {
                using (BusyScope.Show(this, "Saving allocation rule..."))
                {
                    int sourceAccId = (int)cmbSourceAccount.SelectedValue;
                    int ccId = (int)cmbTargetCostCenter.SelectedValue;
                    decimal allocationPercent = decimal.Parse(txtPercent.Text);
                    string allocationMethod = cmbMethod.SelectedItem?.ToString() ?? "FIXED_PCT";

                    const string sql = @"
IF NOT EXISTS (SELECT 1 FROM dbo.acc_cost_center_allocations 
  WHERE alloc_name = @name)
BEGIN
    INSERT INTO dbo.acc_cost_center_allocations
    (alloc_name, source_acc_id, cc_id, allocation_percent, allocation_method, is_active, created_at)
    VALUES (@name, @sourceAccId, @ccId, @percent, @method, @isActive, GETDATE());
END
ELSE
BEGIN
    UPDATE dbo.acc_cost_center_allocations
    SET source_acc_id = @sourceAccId,
        cc_id = @ccId,
        allocation_percent = @percent,
        allocation_method = @method,
        is_active = @isActive
    WHERE alloc_name = @name;
END";

                    using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                    {
                        cn.Open();
                        using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.Parameters.AddWithValue("@sourceAccId", sourceAccId);
                            cmd.Parameters.AddWithValue("@ccId", ccId);
                            cmd.Parameters.AddWithValue("@percent", allocationPercent);
                            cmd.Parameters.AddWithValue("@method", allocationMethod);
                            cmd.Parameters.AddWithValue("@isActive", chkActive.Checked ? 1 : 0);
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show(T("Allocation rule saved successfully.", "تم حفظ قاعدة التوزيع بنجاح."), T("Success", "نجح"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRules();
                    ClearForm();
                    EnableEditControls(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{T("Error saving rule:", "خطأ في حفظ القاعدة:")} {ex.Message}", T("Error", "خطأ"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            EnableEditControls(false);
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(T("Please select a rule to delete.", "يرجى تحديد القاعدة التي تريد حذفها."), T("Information", "معلومات"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            DialogResult result = MessageBox.Show(T("Are you sure you want to delete this rule?", "هل أنت متأكد من حذف هذه القاعدة؟"), T("Confirm Delete", "تأكيد الحذف"), MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, T("Deleting rule...", "جاري حذف القاعدة...")))
                {
                    const string sql = "DELETE FROM dbo.acc_cost_center_allocations WHERE alloc_name = @name";
                    using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                    {
                        cn.Open();
                        using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                        {
                            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
                            cmd.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show(T("Rule deleted.", "تم حذف القاعدة."), T("Success", "نجح"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadRules();
                    ClearForm();
                    EnableEditControls(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{T("Error deleting rule:", "خطأ في حذف القاعدة:")} {ex.Message}", T("Error", "خطأ"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnRunAllocation_Click(object sender, EventArgs e)
        {
            var period = dtpPeriod.Value.Date;

            DialogResult result = MessageBox.Show(
                T($"Run allocation for period: {period:yyyy-MM}?\n\nThis will create journal entries distributing expenses.",
                  $"تشغيل التوزيع للفترة: {period:yyyy-MM}؟\n\nسيقوم هذا بإنشاء قيود يومية توزع المصاريف."),
                T("Confirm Allocation", "تأكيد التوزيع"),
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, T("Running allocation...", "جاري تشغيل التوزيع...")))
                {
                    var allocationResult = bll.RunExpenseAllocation(period, UsersModal.logged_in_userid);

                    if (allocationResult.Success)
                    {
                        string message = T($"Allocation completed successfully!\n\n" +
                            $"Voucher: {allocationResult.VoucherNo}\n" +
                            $"Total Allocated: {allocationResult.TotalAllocated:N2}\n" +
                            $"Departments: {allocationResult.Allocations.Count}",
                            $"تم إكمال التوزيع بنجاح!\n\n" +
                            $"الكشف: {allocationResult.VoucherNo}\n" +
                            $"الإجمالي الموزع: {allocationResult.TotalAllocated:N2}\n" +
                            $"الأقسام: {allocationResult.Allocations.Count}");
                        MessageBox.Show(message, T("Success", "نجح"), MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show($"{T("Error:", "خطأ:")} {allocationResult.Message}", T("Allocation Failed", "فشل التوزيع"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{T("Error running allocation:", "خطأ في تشغيل التوزيع:")} {ex.Message}", T("Error", "خطأ"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvRules_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dgvRules.Rows[e.RowIndex].Cells[0].Value != null)
            {
                string ruleName = dgvRules.Rows[e.RowIndex].Cells["alloc_name"].Value?.ToString() ?? "";
                if (!string.IsNullOrEmpty(ruleName))
                {
                    LoadRule(ruleName);
                }
            }
        }

        private void LoadRule(string ruleName)
        {
            try
            {
                DataTable dt = new DataTable();
                using (var cn = new System.Data.SqlClient.SqlConnection(POS.DLL.dbConnection.ConnectionString))
                {
                    cn.Open();
                    const string sql = @"
SELECT
    alloc_id, alloc_name, source_acc_id, cc_id, allocation_percent, allocation_method, is_active
FROM dbo.acc_cost_center_allocations
WHERE alloc_name = @name;";
                    using (var cmd = new System.Data.SqlClient.SqlCommand(sql, cn))
                    {
                        cmd.Parameters.AddWithValue("@name", ruleName);
                        using (var da = new System.Data.SqlClient.SqlDataAdapter(cmd))
                        {
                            da.Fill(dt);
                        }
                    }
                }

                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    txtName.Text = row["alloc_name"]?.ToString() ?? "";
                    cmbSourceAccount.SelectedValue = row["source_acc_id"];
                    cmbTargetCostCenter.SelectedValue = row["cc_id"];
                    txtPercent.Text = row["allocation_percent"]?.ToString() ?? "0";
                    cmbMethod.SelectedItem = row["allocation_method"]?.ToString() ?? "FIXED_PCT";
                    chkActive.Checked = (bool)row["is_active"];
                    EnableEditControls(true);
                    txtName.Enabled = false; // Don't allow changing name
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{T("Error loading rule:", "خطأ في تحميل القاعدة:")} {ex.Message}", T("Error", "خطأ"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                MessageBox.Show(T("Rule Name is required.", "اسم القاعدة مطلوب."), T("Validation Error", "خطأ في التحقق"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtName.Focus();
                return false;
            }

            if (cmbSourceAccount.SelectedValue == null)
            {
                MessageBox.Show(T("Source Account is required.", "حساب المصدر مطلوب."), T("Validation Error", "خطأ في التحقق"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbSourceAccount.Focus();
                return false;
            }

            if (cmbTargetCostCenter.SelectedValue == null)
            {
                MessageBox.Show(T("Target Cost Center is required.", "مركز التكلفة الهدف مطلوب."), T("Validation Error", "خطأ في التحقق"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbTargetCostCenter.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPercent.Text, out decimal percent) || percent <= 0 || percent > 100)
            {
                MessageBox.Show(T("Allocation Percent must be between 0.01 and 100.", "يجب أن تكون نسبة التوزيع بين 0.01 و 100."), T("Validation Error", "خطأ في التحقق"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPercent.Focus();
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtName.Clear();
            txtPercent.Clear();
            cmbSourceAccount.SelectedIndex = -1;
            cmbTargetCostCenter.SelectedIndex = -1;
            cmbMethod.SelectedIndex = 0;
            chkActive.Checked = true;
            dtpPeriod.Value = DateTime.Today;
        }

        private void EnableEditControls(bool enabled)
        {
            txtName.Enabled = enabled;
            cmbSourceAccount.Enabled = enabled;
            cmbTargetCostCenter.Enabled = enabled;
            txtPercent.Enabled = enabled;
            cmbMethod.Enabled = enabled;
            chkActive.Enabled = enabled;
            btnSave.Enabled = enabled;
            btnCancel.Enabled = enabled;
            btnDelete.Enabled = enabled && !string.IsNullOrWhiteSpace(txtName.Text);
        }

        private void FrmAllocationRules_Load(object sender, EventArgs e)
        {
            LoadRules();
            dtpPeriod.Value = DateTime.Today;
        }
    }
}
