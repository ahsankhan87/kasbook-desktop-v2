using pos.UI;
using pos.UI.Busy;
using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_accounting_settings : Form
    {
        private readonly CompaniesBLL _companiesBll = new CompaniesBLL();
        private readonly CurrencyBLL _currencyBll = new CurrencyBLL();
        private readonly AccountsBLL _accountsBll = new AccountsBLL();
        private readonly GeneralBLL _generalBll = new GeneralBLL();
        private readonly AccountingSettingsService _settings = AccountingSettingsService.Instance;

        private readonly Dictionary<ComboBox, string> _accountSettingMap = new Dictionary<ComboBox, string>();
        private DataTable _companyTable;

        public frm_accounting_settings()
        {
            InitializeComponent();
            BuildSettingMaps();
        }

        private void frm_accounting_settings_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);

            if (!EnsureAuthorizedRole())
            {
                UiMessages.ShowError("Access denied. Only Admin/CFO can open Accounting Settings.", "غير مصرح. فقط المدير/CFO يمكنه فتح إعدادات المحاسبة.");
                Close();
                return;
            }

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading accounting settings...", "جاري تحميل إعدادات المحاسبة...")))
                {
                    _settings.LoadAll();
                    LoadMonthDropDowns();
                    LoadCountryDropDown();
                    LoadCompany();
                    LoadCurrencies();
                    LoadAccounts();
                    LoadSettingsToControls();
                    LoadWhtGrid();
                    LoadVoucherGrid();
                    ApplyFinancialYearEndMonth();
                    LockBaseCurrencyIfTransactionsExist();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void BuildSettingMaps()
        {
            _accountSettingMap[cmbSalesAr] = SettingKeys.DefaultArAccount;
            _accountSettingMap[cmbSalesRevenue] = SettingKeys.DefaultSalesAccount;
            _accountSettingMap[cmbSalesTaxOutput] = SettingKeys.DefaultSalesTaxAccount;

            _accountSettingMap[cmbPurchaseAp] = SettingKeys.DefaultApAccount;
            _accountSettingMap[cmbPurchaseCogs] = SettingKeys.DefaultPurchaseAccount;
            _accountSettingMap[cmbPurchaseTaxInput] = "ACC_DEFAULT_TAX_RECEIVABLE";

            _accountSettingMap[cmbDefaultExpense] = "ACC_DEFAULT_EXPENSE_ACCOUNT";
            _accountSettingMap[cmbDefaultCash] = SettingKeys.DefaultCashAccount;
            _accountSettingMap[cmbDefaultBank] = SettingKeys.DefaultBankAccount;

            _accountSettingMap[cmbSalaryExpense] = "ACC_DEFAULT_SALARY_EXPENSE_ACCOUNT";
            _accountSettingMap[cmbSalaryPayable] = "ACC_DEFAULT_SALARY_PAYABLE_ACCOUNT";

            _accountSettingMap[cmbInventoryAsset] = SettingKeys.DefaultInventoryAccount;
            _accountSettingMap[cmbInventoryCogs] = SettingKeys.DefaultCogsAccount;
            _accountSettingMap[cmbInventoryAdjustment] = "ACC_DEFAULT_STOCK_ADJUSTMENT_ACCOUNT";

            _accountSettingMap[cmbFaAsset] = "ACC_DEFAULT_FA_ASSET_ACCOUNT";
            _accountSettingMap[cmbFaAccumDep] = "ACC_DEFAULT_FA_ACCUM_DEP_ACCOUNT";
            _accountSettingMap[cmbFaDepExpense] = "ACC_DEFAULT_FA_DEP_EXPENSE_ACCOUNT";

            _accountSettingMap[cmbInterBranchRec] = "ACC_DEFAULT_INTERBRANCH_RECEIVABLE_ACCOUNT";
            _accountSettingMap[cmbInterBranchPay] = "ACC_DEFAULT_INTERBRANCH_PAYABLE_ACCOUNT";

            _accountSettingMap[cmbOpeningEquity] = SettingKeys.DefaultOpeningEquityAccount;
        }

        private bool EnsureAuthorizedRole()
        {
            var role = (UsersModal.logged_in_user_role ?? string.Empty).Trim().ToLowerInvariant();
            return role == "administrator" || role == "admin" || role == "owner" || role == "cfo";
        }

        private void LoadMonthDropDowns()
        {
            var months = DateTimeFormatInfo.InvariantInfo.MonthNames.Take(12).ToList();
            cmbFyStartMonth.DataSource = new List<string>(months);
            cmbFyEndMonth.DataSource = new List<string>(months);
            cmbFyEndMonth.Enabled = false;
        }

        private void LoadCountryDropDown()
        {
            cmbCountry.Items.Clear();
            cmbCountry.Items.AddRange(new object[]
            {
                "Saudi Arabia",
                "Pakistan",
                "UAE",
                "Qatar",
                "Bahrain",
                "Kuwait",
                "Oman"
            });

            cmbTaxMode.Items.Clear();
            cmbTaxMode.Items.AddRange(new object[] { "Tax Inclusive", "Tax Exclusive" });

            cmbFilingFrequency.Items.Clear();
            cmbFilingFrequency.Items.AddRange(new object[] { "Monthly", "Quarterly", "Annually" });

            cmbAmountFormat.Items.Clear();
            cmbAmountFormat.Items.AddRange(new object[] { "PAKISTANI", "INTERNATIONAL" });

            cmbReportDateFormat.Items.Clear();
            cmbReportDateFormat.Items.AddRange(new object[] { "dd/MM/yyyy", "MM/dd/yyyy", "yyyy-MM-dd" });

            cmbShowAmountsIn.Items.Clear();
            cmbShowAmountsIn.Items.AddRange(new object[] { "Actual", "Thousands", "Lakhs", "Crores" });
        }

        private void LoadCompany()
        {
            _companyTable = _companiesBll.GetCompany();
            if (_companyTable == null || _companyTable.Rows.Count == 0)
            {
                return;
            }

            DataRow row = _companyTable.Rows[0];

            txtCompanyName.Text = GetRowString(row, "name");
            txtAddress.Text = GetRowString(row, "address");
            txtPhone.Text = GetRowString(row, "contact_no");
            txtEmail.Text = GetRowString(row, "email");
            txtLogoPath.Text = GetRowString(row, "image");
            txtWebsite.Text = GetRowString(row, "website");
            txtLegalName.Text = GetRowString(row, "legal_name");
            txtRegistrationNo.Text = GetRowString(row, "registration_no");
            txtNtnVat.Text = string.IsNullOrWhiteSpace(GetRowString(row, "ntn")) ? GetRowString(row, "vat_no") : GetRowString(row, "ntn");
            txtStrn.Text = GetRowString(row, "strn");

            SelectComboByText(cmbCountry, GetRowString(row, "countryName"));

            var logoPath = txtLogoPath.Text.Trim();
            if (!string.IsNullOrWhiteSpace(logoPath))
            {
                TryLoadLogo(logoPath);
            }
        }

        private void LoadCurrencies()
        {
            DataTable dt = _currencyBll.GetAll();
            cmbBaseCurrency.DisplayMember = "name";
            cmbBaseCurrency.ValueMember = "id";
            cmbBaseCurrency.DataSource = dt;
        }

        private void LoadAccounts()
        {
            DataTable accounts = _accountsBll.SearchRecord(string.Empty);
            if (accounts == null)
            {
                return;
            }

            foreach (var kv in _accountSettingMap)
            {
                BindAccountCombo(kv.Key, accounts.Copy());
            }
        }

        private void BindAccountCombo(ComboBox combo, DataTable accounts)
        {
            if (!accounts.Columns.Contains("display"))
                accounts.Columns.Add("display", typeof(string));

            foreach (DataRow row in accounts.Rows)
            {
                row["display"] = string.Format("{0} - {1}", Convert.ToString(row["code"]), Convert.ToString(row["name"]));
            }

            combo.DisplayMember = "display";
            combo.ValueMember = "code";
            combo.DataSource = accounts;
            combo.DropDownStyle = ComboBoxStyle.DropDown;
            combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            combo.AutoCompleteSource = AutoCompleteSource.ListItems;
        }

        private void LoadSettingsToControls()
        {
            int startMonth = _settings.GetInt(SettingKeys.FinancialYearStartMonth, 7);
            if (startMonth >= 1 && startMonth <= 12)
            {
                cmbFyStartMonth.SelectedIndex = startMonth - 1;
            }

            SelectComboByText(cmbAmountFormat, _settings.GetString(SettingKeys.AmountFormat, "PAKISTANI"));
            SelectComboByText(cmbReportDateFormat, _settings.GetString("SYSTEM_DATE_FORMAT", "dd/MM/yyyy"));
            SelectComboByText(cmbShowAmountsIn, _settings.GetString("ACC_SHOW_AMOUNTS_IN", "Actual"));

            txtReportFooter.Text = _settings.GetString(SettingKeys.ReportFooter, string.Empty);
            txtDigitalSignature.Text = _settings.GetString("ACC_DIGITAL_SIGNATURE_PLACEHOLDER", string.Empty);
            txtReportHeader.Text = _settings.GetString("ACC_REPORT_HEADER", txtCompanyName.Text);

            numSalesTaxRate.Value = ClampDecimal(_settings.GetDecimal("ACC_GST_RATE", 15m), 0, 100, 2);
            SelectComboByText(cmbTaxMode, _settings.GetString("ACC_TAX_DEFAULT_MODE", "Tax Exclusive"));
            txtFbrNtn.Text = _settings.GetString("ACC_FBR_NTN", txtNtnVat.Text);
            txtFbrStrn.Text = _settings.GetString("ACC_FBR_STRN", txtStrn.Text);
            SelectComboByText(cmbFilingFrequency, _settings.GetString("ACC_FILING_FREQUENCY", "Monthly"));

            chkAutoPostSales.Checked = _settings.GetBool(SettingKeys.AutoPostSales, true);
            chkAutoPostPurchases.Checked = _settings.GetBool(SettingKeys.AutoPostPurchases, true);
            chkAllowLockedPeriodPosting.Checked = _settings.GetBool("ACC_ALLOW_POSTING_LOCKED_PERIOD", false);
            chkRequireNarration.Checked = _settings.GetBool(SettingKeys.RequireNarration, false);

            numBudgetWarningPct.Value = ClampDecimal(_settings.GetDecimal(SettingKeys.BudgetWarningPct, 85m), 0, 100, 0);
            numBackdatingDays.Value = ClampInt(_settings.GetInt(SettingKeys.BackdatingLimitDays, 90), 0, 3650);
            numApprovalThreshold.Value = ClampDecimal(_settings.GetDecimal(SettingKeys.ApprovalThreshold, 0m), 0, 1000000000m, 2);

            foreach (var kv in _accountSettingMap)
            {
                var code = _settings.GetString(kv.Value, string.Empty);
                SelectComboByValue(kv.Key, code);
            }
        }

        private void LoadWhtGrid()
        {
            DataTable dt = _generalBll.GetRecord("*", "acc_wht_rates ORDER BY wht_type, tax_section");
            gridWhtRates.AutoGenerateColumns = false;
            gridWhtRates.DataSource = dt;
        }

        /// <summary>
        /// Loads the voucher numbering configuration grid.
        /// Voucher format: [Prefix][BranchId]-[DateFormat]-[NumberFormat]
        /// Example: S1-20260713-0001
        /// - S = Prefix (Sales)
        /// - 1 = Branch ID (from logged in user's branch)
        /// - 20260713 = Date format (YYYYMMDD)
        /// - 0001 = Counter (starting number, reset based on configuration)
        /// </summary>
        private void LoadVoucherGrid()
        {
            if (gridVoucher.Rows.Count == 0)
            {
                int branchId = UsersModal.logged_in_branch_id;
                gridVoucher.Rows.Add("JV", _settings.GetString("ACC_VOUCHER_JV_PREFIX", "JV"), branchId.ToString(), _settings.GetString("ACC_VOUCHER_JV_FORMAT", "YYYY-NNNN"), _settings.GetString("ACC_VOUCHER_JV_RESET", "Annually"), _settings.GetString("ACC_VOUCHER_JV_START", "1"), "");
                gridVoucher.Rows.Add("RECEIPT", _settings.GetString("ACC_VOUCHER_RECEIPT_PREFIX", "RV"), branchId.ToString(), _settings.GetString("ACC_VOUCHER_RECEIPT_FORMAT", "YYYY-NNNN"), _settings.GetString("ACC_VOUCHER_RECEIPT_RESET", "Annually"), _settings.GetString("ACC_VOUCHER_RECEIPT_START", "1"), "");
                gridVoucher.Rows.Add("PAYMENT", _settings.GetString("ACC_VOUCHER_PAYMENT_PREFIX", "PV"), branchId.ToString(), _settings.GetString("ACC_VOUCHER_PAYMENT_FORMAT", "YYYY-NNNN"), _settings.GetString("ACC_VOUCHER_PAYMENT_RESET", "Annually"), _settings.GetString("ACC_VOUCHER_PAYMENT_START", "1"), "");
                gridVoucher.Rows.Add("IBT", _settings.GetString("ACC_VOUCHER_IBT_PREFIX", "IBT"), branchId.ToString(), _settings.GetString("ACC_VOUCHER_IBT_FORMAT", "YYYY-NNNN"), _settings.GetString("ACC_VOUCHER_IBT_RESET", "Annually"), _settings.GetString("ACC_VOUCHER_IBT_START", "1"), "");
                gridVoucher.Rows.Add("ADJ", _settings.GetString("ACC_VOUCHER_ADJ_PREFIX", "ADJ"), branchId.ToString(), _settings.GetString("ACC_VOUCHER_ADJ_FORMAT", "YYYY-NNNN"), _settings.GetString("ACC_VOUCHER_ADJ_RESET", "Annually"), _settings.GetString("ACC_VOUCHER_ADJ_START", "1"), "");
            }

            RefreshVoucherPreview();
        }

        private void LockBaseCurrencyIfTransactionsExist()
        {
            DataTable dt = _generalBll.GetRecord("COUNT(1) AS cnt", "acc_entries");
            var hasTx = dt != null && dt.Rows.Count > 0 && Convert.ToInt32(dt.Rows[0]["cnt"]) > 0;
            if (hasTx)
            {
                cmbBaseCurrency.Enabled = false;
                lblCurrencyLockNote.Text = "Base currency is locked after first transaction.";
            }
            else
            {
                lblCurrencyLockNote.Text = string.Empty;
            }
        }

        private void cmbFyStartMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFinancialYearEndMonth();
        }

        private void ApplyFinancialYearEndMonth()
        {
            if (cmbFyStartMonth.SelectedIndex < 0)
            {
                return;
            }

            if (cmbFyEndMonth.Items.Count == 0)
            {
                return;
            }

            int endIndex = (cmbFyStartMonth.SelectedIndex + 11) % 12;
            cmbFyEndMonth.SelectedIndex = endIndex;
        }

        private void btnBrowseLogo_Click(object sender, EventArgs e)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp";
                dlg.Multiselect = false;
                if (dlg.ShowDialog(this) != DialogResult.OK)
                {
                    return;
                }

                txtLogoPath.Text = dlg.FileName;
                TryLoadLogo(dlg.FileName);
            }
        }

        private void TryLoadLogo(string path)
        {
            try
            {
                picLogoPreview.Image = Image.FromFile(path);
            }
            catch
            {
                picLogoPreview.Image = null;
            }
        }

        private void btnTestAutoPostingRules_Click(object sender, EventArgs e)
        {
            var missing = _settings.ValidateAllDefaults();
            if (missing.Count == 0)
            {
                UiMessages.ShowInfo("All critical default accounts are configured.", "جميع الحسابات الافتراضية الأساسية مهيأة.", "Validation", "التحقق");
                return;
            }

            UiMessages.ShowInfo(string.Join(Environment.NewLine, missing), string.Join(Environment.NewLine, missing), "Missing Defaults", "إعدادات ناقصة");
        }

        private void btnSaveSettings_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Saving accounting settings...", "جاري حفظ إعدادات المحاسبة...")))
                {
                    SaveCompany();
                    SaveAccountDefaults();
                    SavePostingRules();
                    SaveTaxSettings();
                    SaveReportSettings();
                    SaveVoucherSettings();
                    SaveWhtRates();
                    _settings.LoadAll();
                }

                UiMessages.ShowInfo("Accounting settings saved successfully.", "تم حفظ إعدادات المحاسبة بنجاح.", "Accounting Settings", "إعدادات المحاسبة");
                Log.LogAction("Save Accounting Settings", "Updated accounting settings form", UsersModal.logged_in_userid, UsersModal.logged_in_branch_id);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void SaveCompany()
        {
            int companyId = GetCompanyId();
            if (companyId <= 0)
            {
                return;
            }

            var row = _companyTable.Rows[0];

            var modal = new CompaniesModal
            {
                id = companyId,
                name = txtCompanyName.Text.Trim(),
                address = txtAddress.Text.Trim(),
                contact_no = txtPhone.Text.Trim(),
                email = txtEmail.Text.Trim(),
                vat_no = txtNtnVat.Text.Trim(),
                image = txtLogoPath.Text.Trim(),
                currency_id = cmbBaseCurrency.SelectedValue == null ? 0 : Convert.ToInt32(cmbBaseCurrency.SelectedValue),
                user_id = UsersModal.logged_in_userid,
                countryName = cmbCountry.Text,
                useZatcaEInvoice = true,
                cash_acc_id = GetIntOrDefault(row, "cash_acc_id"),
                sales_acc_id = GetIntOrDefault(row, "sales_acc_id"),
                inventory_acc_id = GetIntOrDefault(row, "inventory_acc_id"),
                sales_return_acc_id = GetIntOrDefault(row, "sales_return_acc_id"),
                sales_discount_acc_id = GetIntOrDefault(row, "sales_discount_acc_id"),
                tax_acc_id = GetIntOrDefault(row, "tax_acc_id"),
                receivable_acc_id = GetIntOrDefault(row, "receivable_acc_id"),
                payable_acc_id = GetIntOrDefault(row, "payable_acc_id"),
                purchases_acc_id = GetIntOrDefault(row, "purchases_acc_id"),
                purchases_return_acc_id = GetIntOrDefault(row, "purchases_return_acc_id"),
                purchases_discount_acc_id = GetIntOrDefault(row, "purchases_discount_acc_id"),
                item_variance_acc_id = GetIntOrDefault(row, "item_variance_acc_id"),
                commission_acc_id = GetIntOrDefault(row, "commission_acc_id"),
                legal_name = txtLegalName.Text,
                registration_no = txtRegistrationNo.Text,
                strn = txtStrn.Text,
                website = txtWebsite.Text,
                financial_year_start_month = cmbFyStartMonth.SelectedIndex + 1
            };

            _companiesBll.Update(modal);

        }

        private void SaveAccountDefaults()
        {
            foreach (var kv in _accountSettingMap)
            {
                var value = kv.Key.SelectedValue == null ? string.Empty : Convert.ToString(kv.Key.SelectedValue);
                _settings.Set(kv.Value, value, UsersModal.logged_in_userid);
            }
        }

        private void SavePostingRules()
        {
            _settings.Set(SettingKeys.AutoPostSales, chkAutoPostSales.Checked ? "true" : "false", UsersModal.logged_in_userid);
            _settings.Set(SettingKeys.AutoPostPurchases, chkAutoPostPurchases.Checked ? "true" : "false", UsersModal.logged_in_userid);
            _settings.Set("ACC_ALLOW_POSTING_LOCKED_PERIOD", chkAllowLockedPeriodPosting.Checked ? "true" : "false", UsersModal.logged_in_userid);
            _settings.Set(SettingKeys.BudgetWarningPct, numBudgetWarningPct.Value.ToString(CultureInfo.InvariantCulture), UsersModal.logged_in_userid);
            _settings.Set(SettingKeys.RequireNarration, chkRequireNarration.Checked ? "true" : "false", UsersModal.logged_in_userid);
            _settings.Set(SettingKeys.BackdatingLimitDays, Convert.ToInt32(numBackdatingDays.Value).ToString(CultureInfo.InvariantCulture), UsersModal.logged_in_userid);
            _settings.Set(SettingKeys.ApprovalThreshold, numApprovalThreshold.Value.ToString(CultureInfo.InvariantCulture), UsersModal.logged_in_userid);
            _settings.Set(SettingKeys.FinancialYearStartMonth, (cmbFyStartMonth.SelectedIndex + 1).ToString(CultureInfo.InvariantCulture), UsersModal.logged_in_userid);
        }

        private void SaveTaxSettings()
        {
            _settings.Set("ACC_GST_RATE", numSalesTaxRate.Value.ToString(CultureInfo.InvariantCulture), UsersModal.logged_in_userid);
            _settings.Set("ACC_TAX_DEFAULT_MODE", cmbTaxMode.Text, UsersModal.logged_in_userid);
            _settings.Set("ACC_FBR_NTN", txtFbrNtn.Text.Trim(), UsersModal.logged_in_userid);
            _settings.Set("ACC_FBR_STRN", txtFbrStrn.Text.Trim(), UsersModal.logged_in_userid);
            _settings.Set("ACC_FILING_FREQUENCY", cmbFilingFrequency.Text, UsersModal.logged_in_userid);
        }

        private void SaveReportSettings()
        {
            _settings.Set(SettingKeys.AmountFormat, cmbAmountFormat.Text, UsersModal.logged_in_userid);
            _settings.Set("SYSTEM_DATE_FORMAT", cmbReportDateFormat.Text, UsersModal.logged_in_userid);
            _settings.Set("ACC_SHOW_AMOUNTS_IN", cmbShowAmountsIn.Text, UsersModal.logged_in_userid);
            _settings.Set(SettingKeys.ReportFooter, txtReportFooter.Text.Trim(), UsersModal.logged_in_userid);
            _settings.Set("ACC_DIGITAL_SIGNATURE_PLACEHOLDER", txtDigitalSignature.Text.Trim(), UsersModal.logged_in_userid);
            _settings.Set("ACC_REPORT_HEADER", txtReportHeader.Text.Trim(), UsersModal.logged_in_userid);
        }

        private void SaveVoucherSettings()
        {
            foreach (DataGridViewRow row in gridVoucher.Rows)
            {
                if (row.IsNewRow) continue;

                string type = Convert.ToString(row.Cells["colVoucherType"].Value ?? string.Empty).Trim().ToUpperInvariant();
                if (string.IsNullOrWhiteSpace(type)) continue;

                _settings.Set("ACC_VOUCHER_" + type + "_PREFIX", Convert.ToString(row.Cells["colVoucherPrefix"].Value ?? string.Empty), UsersModal.logged_in_userid);
                _settings.Set("ACC_VOUCHER_" + type + "_FORMAT", Convert.ToString(row.Cells["colVoucherFormat"].Value ?? "YYYY-NNNN"), UsersModal.logged_in_userid);
                _settings.Set("ACC_VOUCHER_" + type + "_RESET", Convert.ToString(row.Cells["colVoucherReset"].Value ?? "Annually"), UsersModal.logged_in_userid);
                _settings.Set("ACC_VOUCHER_" + type + "_START", Convert.ToString(row.Cells["colVoucherStart"].Value ?? "1"), UsersModal.logged_in_userid);
            }
        }

        private void SaveWhtRates()
        {
            foreach (DataGridViewRow row in gridWhtRates.Rows)
            {
                if (row.IsNewRow) continue;

                string whtType = Convert.ToString(row.Cells["colWhtType"].Value ?? string.Empty).Trim();
                string section = Convert.ToString(row.Cells["colTaxSection"].Value ?? string.Empty).Trim();
                string desc = Convert.ToString(row.Cells["colWhtDescription"].Value ?? string.Empty).Trim();
                decimal rate = ToDecimal(row.Cells["colWhtRate"].Value, 0m);
                DateTime effectiveFrom = ToDate(row.Cells["colEffectiveFrom"].Value, DateTime.Today);
                bool isActive = ToBool(row.Cells["colIsActive"].Value, true);

                if (string.IsNullOrWhiteSpace(whtType) || string.IsNullOrWhiteSpace(section))
                {
                    continue;
                }

                var idObj = row.Cells["colWhtId"].Value;
                if (idObj != null && idObj != DBNull.Value && Convert.ToInt32(idObj) > 0)
                {
                    int id = Convert.ToInt32(idObj);
                    _generalBll.UpdateOrDeleteRecord("acc_wht_rates",
                        string.Format("wht_type='{0}', tax_section='{1}', description='{2}', rate={3}, effective_from='{4:yyyy-MM-dd}', is_active={5}",
                            SqlSafe(whtType), SqlSafe(section), SqlSafe(desc), rate.ToString(CultureInfo.InvariantCulture), effectiveFrom, isActive ? 1 : 0),
                        "wht_id=" + id);
                }
                else
                {
                    _generalBll.InsertRecord("acc_wht_rates",
                        "wht_type,tax_section,description,rate,effective_from,is_active",
                        string.Format("'{0}','{1}','{2}',{3},'{4:yyyy-MM-dd}',{5}", SqlSafe(whtType), SqlSafe(section), SqlSafe(desc), rate.ToString(CultureInfo.InvariantCulture), effectiveFrom, isActive ? 1 : 0));
                }
            }
        }

        private void btnResetDefaults_Click(object sender, EventArgs e)
        {
            LoadSettingsToControls();
            LoadVoucherGrid();
            UiMessages.ShowInfo("Settings reloaded from saved values.", "تمت إعادة تحميل الإعدادات من القيم المحفوظة.", "Accounting Settings", "إعدادات المحاسبة");
        }

        private void gridVoucher_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            RefreshVoucherPreview();
        }

        private void RefreshVoucherPreview()
        {
            int branchId = UsersModal.logged_in_branch_id;

            foreach (DataGridViewRow row in gridVoucher.Rows)
            {
                if (row.IsNewRow) continue;

                string type = Convert.ToString(row.Cells["colVoucherType"].Value ?? string.Empty).Trim().ToUpperInvariant();
                string prefix = Convert.ToString(row.Cells["colVoucherPrefix"].Value ?? type);
                string format = Convert.ToString(row.Cells["colVoucherFormat"].Value ?? "YYYY-NNNN");
                int start = ToInt(row.Cells["colVoucherStart"].Value, 1);

                row.Cells["colVoucherPreview"].Value = BuildVoucherPreview(prefix, branchId, format, start);
            }
        }

        private static string BuildVoucherPreview(string prefix, int branchId, string format, int number)
        {
            var today = DateTime.Today;
            var yy = today.ToString("yy", CultureInfo.InvariantCulture);
            var yyyy = today.ToString("yyyy", CultureInfo.InvariantCulture);
            var mm = today.ToString("MM", CultureInfo.InvariantCulture);
            var dd = today.ToString("dd", CultureInfo.InvariantCulture);
            var n = number.ToString("D4", CultureInfo.InvariantCulture);

            // Parse the format string which may contain date placeholders and NNNN
            var fmt = (format ?? "YYYY-NNNN").ToUpperInvariant();

            // Replace date placeholders
            string result = fmt.Replace("YYYY", yyyy);
            result = result.Replace("YY", yy);
            result = result.Replace("MM", mm);
            result = result.Replace("DD", dd);
            result = result.Replace("NNNN", n);

            // Build the complete voucher number: Prefix + BranchId + formatted part
            return prefix + branchId + "-" + result;
        }

        private int GetCompanyId()
        {
            if (_companyTable == null || _companyTable.Rows.Count == 0)
            {
                return 0;
            }

            return GetIntOrDefault(_companyTable.Rows[0], "id");
        }

        private static void SelectComboByValue(ComboBox combo, string value)
        {
            if (combo == null || string.IsNullOrWhiteSpace(value) || combo.DataSource == null)
                return;

            combo.SelectedValue = value;
        }

        private static void SelectComboByText(ComboBox combo, string text)
        {
            if (combo == null || string.IsNullOrWhiteSpace(text)) return;

            for (int i = 0; i < combo.Items.Count; i++)
            {
                if (string.Equals(Convert.ToString(combo.Items[i]), text, StringComparison.OrdinalIgnoreCase))
                {
                    combo.SelectedIndex = i;
                    return;
                }
            }

            combo.Text = text;
        }

        private static string GetRowString(DataRow row, string column)
        {
            if (row == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return string.Empty;
            return Convert.ToString(row[column]);
        }

        private static int GetIntOrDefault(DataRow row, string column, int defaultValue = 0)
        {
            if (row == null || !row.Table.Columns.Contains(column) || row[column] == DBNull.Value) return defaultValue;
            int value;
            return int.TryParse(Convert.ToString(row[column]), out value) ? value : defaultValue;
        }

        private static decimal ClampDecimal(decimal value, decimal min, decimal max, int decimals)
        {
            var clamped = Math.Min(max, Math.Max(min, value));
            return Math.Round(clamped, decimals);
        }

        private static int ClampInt(int value, int min, int max)
        {
            return Math.Min(max, Math.Max(min, value));
        }

        private static decimal ToDecimal(object value, decimal defaultValue)
        {
            if (value == null || value == DBNull.Value) return defaultValue;
            decimal parsed;
            return decimal.TryParse(Convert.ToString(value), NumberStyles.Any, CultureInfo.InvariantCulture, out parsed)
                ? parsed
                : defaultValue;
        }

        private static int ToInt(object value, int defaultValue)
        {
            if (value == null || value == DBNull.Value) return defaultValue;
            int parsed;
            return int.TryParse(Convert.ToString(value), out parsed) ? parsed : defaultValue;
        }

        private static DateTime ToDate(object value, DateTime defaultValue)
        {
            if (value == null || value == DBNull.Value) return defaultValue;
            DateTime parsed;
            return DateTime.TryParse(Convert.ToString(value), out parsed) ? parsed : defaultValue;
        }

        private static bool ToBool(object value, bool defaultValue)
        {
            if (value == null || value == DBNull.Value) return defaultValue;
            bool parsed;
            if (bool.TryParse(Convert.ToString(value), out parsed)) return parsed;

            int i;
            if (int.TryParse(Convert.ToString(value), out i)) return i != 0;

            return defaultValue;
        }

        private static string SqlSafe(string value)
        {
            return (value ?? string.Empty).Replace("'", "''");
        }
    }
}
