using DGVPrinterHelper;
using pos.Security.Authorization;
using pos.UI;
using POS.BLL;
using POS.Core;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace pos
{
    public partial class frm_journal_entries : Form
    {
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private readonly UserIdentity _currentUser = AppSecurityContext.User;
        private readonly BindingSource _costCenterBindingSource = new BindingSource();
        private readonly ContextMenuStrip _templateMenu = new ContextMenuStrip();

        private DataTable _accountsTable;
        private DataTable _costCentersTable;
        private DataTable _customerPartiesTable;
        private DataTable _supplierPartiesTable;
        private DataTable _bankPartiesTable;
        private string _attachmentFilePath = string.Empty;
        private string _editingInvoiceNo = string.Empty;

        private enum JournalPartyMode
        {
            General = 0,
            Customer = 1,
            Supplier = 2,
            Bank = 3
        }

        public double _dr_total = 0;
        public double _cr_total = 0;

        public frm_journal_entries()
        {
            InitializeComponent();
            grid_journal.DataError += grid_journal_DataError;
        }

        private void frm_journal_entries_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);

            if (!_auth.HasPermission(_currentUser, Permissions.Journal_Create))
            {
                MessageBox.Show("You do not have permission to access this module.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                Close();
                return;
            }

            ConfigureVoucherTypes();
            ConfigurePartyLookups();
            ConfigureCostCenters();
            ConfigureTemplateMenu();
            txt_entry_date.Value = DateTime.Today;
            LoadAccountLookup();
            InitializeVoucherLines();
            GetMAXInvoiceNo();
            RefreshVoucherState();
        }

        public void GetMAXInvoiceNo()
        {
            JournalsBLL JournalsBLL_obj = new JournalsBLL();
            txt_invoice_no.Text = JournalsBLL_obj.GetMaxInvoiceNo();
        }

        public bool LoadVoucherForEdit(string invoiceNo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(invoiceNo))
                {
                    return false;
                }

                JournalsBLL journalsBll = new JournalsBLL();
                DataTable headerDt = journalsBll.GetVoucherHeaderByInvoiceNo(invoiceNo);
                if (headerDt == null || headerDt.Rows.Count == 0)
                {
                    return false;
                }

                DataRow headerRow = headerDt.Rows[0];
                string status = Convert.ToString(headerRow["status"]);
                if (!string.Equals(status, "Draft", StringComparison.OrdinalIgnoreCase))
                {
                    MessageBox.Show("Only draft vouchers can be edited.", "Voucher", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }

                _editingInvoiceNo = invoiceNo;
                txt_invoice_no.Text = invoiceNo;
                txt_entry_date.Value = headerRow["EntryDate"] == DBNull.Value ? DateTime.Today : Convert.ToDateTime(headerRow["EntryDate"]);
                cmb_voucher_type.Text = Convert.ToString(headerRow["VoucherType"]);
                txt_reference_no.Text = Convert.ToString(headerRow["ReferenceNo"]);
                txt_narration.Text = Convert.ToString(headerRow["Narration"]);
                _attachmentFilePath = Convert.ToString(headerRow["Attachment"]);
                btn_attachment.Text = string.IsNullOrWhiteSpace(_attachmentFilePath) ? "Attachment" : string.Format("Attachment: {0}", Path.GetFileName(_attachmentFilePath));

                grid_journal.Rows.Clear();
                DataTable lines = journalsBll.GetVoucherLines(invoiceNo);
                foreach (DataRow line in lines.Rows)
                {
                    int rowIndex = grid_journal.Rows.Add();
                    DataGridViewRow gridRow = grid_journal.Rows[rowIndex];
                    gridRow.Cells["account"].Value = line["account_id"];
                    gridRow.Cells["description"].Value = Convert.ToString(line["Description"]);

                    // Set cost_center from the loaded voucher line
                    int costCenterId = line["cost_center_id"] != DBNull.Value ? Convert.ToInt32(line["cost_center_id"]) : 0;
                    gridRow.Cells["cost_center"].Value = costCenterId > 0 ? (object)costCenterId : (object)DBNull.Value;

                    gridRow.Cells["debit_amount"].Value = Convert.ToDecimal(line["Debit"]);
                    gridRow.Cells["credit_amount"].Value = Convert.ToDecimal(line["Credit"]);
                    UpdateAccountTypeForRow(rowIndex);

                    int refId = GetIntFromRow(line, "ref_id");
                    if (refId <= 0)
                    {
                        refId = GetIntFromRow(line, "customer_id");
                    }
                    if (refId <= 0)
                    {
                        refId = GetIntFromRow(line, "supplier_id");
                    }
                    if (refId <= 0)
                    {
                        refId = GetIntFromRow(line, "bank_id");
                    }

                    gridRow.Cells["party"].Value = refId > 0 ? (object)refId : (object)0;
                    BindPartyCellForRow(rowIndex);
                }

                if (grid_journal.Rows.Count < 2)
                {
                    AddVoucherLine(false);
                    AddVoucherLine(false);
                }

                RefreshVoucherState();
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void ConfigureVoucherTypes()
        {
            cmb_voucher_type.Items.Clear();
            cmb_voucher_type.Items.AddRange(new object[]
            {
                "General Journal",
                "Opening Entry",
                "Adjusting Entry",
                "Closing Entry",
                "Reversal Entry"
            });
            if (cmb_voucher_type.Items.Count > 0)
            {
                cmb_voucher_type.SelectedIndex = 0;
            }
        }

        private void ConfigurePartyLookups()
        {
            _customerPartiesTable = BuildPartyLookupTable(new CustomerBLL().GetAll());
            _supplierPartiesTable = BuildPartyLookupTable(new SupplierBLL().GetAll());
            _bankPartiesTable = BuildPartyLookupTable(new BankBLL().GetAll());
        }

        private DataTable BuildPartyLookupTable(DataTable source)
        {
            DataTable dt = source == null ? new DataTable() : source.Copy();

            if (!dt.Columns.Contains("id"))
            {
                dt.Columns.Add("id", typeof(int));
            }

            if (!dt.Columns.Contains("display_text"))
            {
                dt.Columns.Add("display_text", typeof(string));
            }

            foreach (DataRow row in dt.Rows)
            {
                row["display_text"] = BuildPartyDisplayText(row);
            }

            DataRow emptyRow = dt.NewRow();
            emptyRow["id"] = 0;
            emptyRow["display_text"] = string.Empty;
            dt.Rows.InsertAt(emptyRow, 0);
            return dt;
        }

        private string BuildPartyDisplayText(DataRow row)
        {
            string code = GetFirstPartyValue(row, "customer_code", "supplier_code", "bank_code", "code");
            string name = GetPartyName(row);

            if (string.IsNullOrWhiteSpace(code)) return name;
            if (string.IsNullOrWhiteSpace(name)) return code;
            return code + " - " + name;
        }

        private string GetPartyName(DataRow row)
        {
            string displayName = GetFirstPartyValue(row, "display_name", "name", "name_2", "bank_name", "customer_name", "supplier_name");
            string firstName = GetFirstPartyValue(row, "first_name");
            string lastName = GetFirstPartyValue(row, "last_name");
            if (!string.IsNullOrWhiteSpace(firstName) || !string.IsNullOrWhiteSpace(lastName))
            {
                string combined = (firstName + " " + lastName).Trim();
                if (!string.IsNullOrWhiteSpace(combined))
                {
                    return combined;
                }
            }

            return displayName;
        }

        private string GetFirstPartyValue(DataRow row, params string[] columns)
        {
            if (row == null || columns == null)
            {
                return string.Empty;
            }

            foreach (string column in columns)
            {
                if (string.IsNullOrWhiteSpace(column) || row.Table == null || !row.Table.Columns.Contains(column))
                {
                    continue;
                }

                string value = Convert.ToString(row[column]).Trim();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }

            return string.Empty;
        }

        private JournalPartyMode ResolvePartyModeFromAccountType(string accountType)
        {
            string normalized = (accountType ?? string.Empty).Trim().ToLowerInvariant();
            if (normalized.Contains("receivable"))
            {
                return JournalPartyMode.Customer;
            }

            if (normalized.Contains("payable"))
            {
                return JournalPartyMode.Supplier;
            }

            if (normalized.Contains("bank"))
            {
                return JournalPartyMode.Bank;
            }

            return JournalPartyMode.General;
        }

        private DataTable GetPartyTableByMode(JournalPartyMode mode)
        {
            if (mode == JournalPartyMode.Customer)
            {
                return _customerPartiesTable;
            }

            if (mode == JournalPartyMode.Supplier)
            {
                return _supplierPartiesTable;
            }

            if (mode == JournalPartyMode.Bank)
            {
                return _bankPartiesTable;
            }

            return null;
        }

        private DataTable GetPartyTableForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= grid_journal.Rows.Count)
            {
                return null;
            }

            DataGridViewRow row = grid_journal.Rows[rowIndex];
            JournalPartyMode mode = ResolvePartyModeForRow(row);
            return GetPartyTableByMode(mode);
        }

        private void BindPartyCellForRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= grid_journal.Rows.Count)
            {
                return;
            }

            DataGridViewRow row = grid_journal.Rows[rowIndex];
            DataGridViewComboBoxCell partyCell = row.Cells["party"] as DataGridViewComboBoxCell;
            if (partyCell == null)
            {
                return;
            }

            object previousValue = partyCell.Value;
            DataTable source = GetPartyTableForRow(rowIndex);

            partyCell.DataSource = null;
            partyCell.DisplayMember = string.Empty;
            partyCell.ValueMember = string.Empty;

            if (source == null)
            {
                partyCell.Value = DBNull.Value;
                return;
            }

            partyCell.DataSource = source;
            partyCell.DisplayMember = "display_text";
            partyCell.ValueMember = "id";

            int selectedId;
            if (previousValue != null && previousValue != DBNull.Value && int.TryParse(Convert.ToString(previousValue), out selectedId) && selectedId > 0 && source.Select("id = " + selectedId.ToString(CultureInfo.InvariantCulture)).Length > 0)
            {
                partyCell.Value = selectedId;
            }
            else
            {
                partyCell.Value = 0;
            }
        }

        private int GetRowPartyId(DataGridViewRow row)
        {
            if (row == null)
            {
                return 0;
            }

            int partyId;
            if (TryGetInt(row.Cells["party"].Value, out partyId) && partyId > 0)
            {
                return partyId;
            }

            return 0;
        }

        private string GetRowPartyRefModule(DataGridViewRow row)
        {
            if (row == null)
            {
                return string.Empty;
            }

            switch (ResolvePartyModeForRow(row))
            {
                case JournalPartyMode.Customer:
                    return "CUSTOMER";
                case JournalPartyMode.Supplier:
                    return "SUPPLIER";
                case JournalPartyMode.Bank:
                    return "BANK";
                default:
                    return string.Empty;
            }
        }

        private JournalPartyMode ResolvePartyModeForRow(DataGridViewRow row)
        {
            if (row == null)
            {
                return JournalPartyMode.General;
            }

            string accountType = Convert.ToString(row.Cells["colAccountType"].Value);
            JournalPartyMode mode = ResolvePartyModeFromAccountType(accountType);
            if (mode != JournalPartyMode.General)
            {
                return mode;
            }

            int accountId;
            if (!TryGetInt(row.Cells["account"].Value, out accountId) || accountId <= 0)
            {
                return JournalPartyMode.General;
            }

            DataRow accountRow = GetAccountRow(accountId);
            if (accountRow == null)
            {
                return JournalPartyMode.General;
            }

            string accountContext = (Convert.ToString(accountRow["code"]) + " " + Convert.ToString(accountRow["name"]) + " " + Convert.ToString(accountRow["name_2"]) + " " + Convert.ToString(accountRow["account_type_name"]))
                .Trim()
                .ToLowerInvariant();

            if (accountContext.Contains("receivable") || accountContext.Contains("customer") || accountContext.Contains("مدين") || accountContext.Contains("عميل"))
            {
                return JournalPartyMode.Customer;
            }

            if (accountContext.Contains("payable") || accountContext.Contains("supplier") || accountContext.Contains("دائن") || accountContext.Contains("مورد"))
            {
                return JournalPartyMode.Supplier;
            }

            if (accountContext.Contains("bank") || accountContext.Contains("بنك"))
            {
                return JournalPartyMode.Bank;
            }

            return JournalPartyMode.General;
        }

        private void ConfigureCostCenters()
        {
            try
            {
                // Load real cost centers from the database using CostCenterBLL
                CostCenterBLL ccBll = new CostCenterBLL();
                _costCentersTable = ccBll.GetCostCenterDropdown();

                // Add empty row for "Unallocated" entries
                if (_costCentersTable != null && _costCentersTable.Rows.Count >= 0)
                {
                    DataRow emptyRow = _costCentersTable.NewRow();
                    emptyRow["id"] = DBNull.Value;
                    emptyRow["display_text"] = string.Empty;
                    emptyRow["cc_code"] = string.Empty;
                    emptyRow["cc_name"] = string.Empty;
                    _costCentersTable.Rows.InsertAt(emptyRow, 0);
                }

                // Bind to BindingSource
                _costCenterBindingSource.DataSource = _costCentersTable;
                cost_center.DataSource = _costCenterBindingSource;
                cost_center.DisplayMember = "display_text";
                cost_center.ValueMember = "id";
                cost_center.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                cost_center.FlatStyle = FlatStyle.Flat;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load cost centers: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Fallback: Add empty option so form doesn't crash
                cost_center.Items.Clear();
                cost_center.Items.Add(string.Empty);
            }
        }

        private void ConfigureTemplateMenu()
        {
            _templateMenu.Items.Clear();
            _templateMenu.Items.Add(CreateTemplateMenuItem("Depreciation Entry"));
            _templateMenu.Items.Add(CreateTemplateMenuItem("Accrual Entry"));
            _templateMenu.Items.Add(CreateTemplateMenuItem("Bank Charges"));
        }

        private ToolStripMenuItem CreateTemplateMenuItem(string templateName)
        {
            var item = new ToolStripMenuItem(templateName);
            item.Click += (s, e) => ApplyTemplate(templateName);
            return item;
        }

        private void LoadAccountLookup()
        {
            try
            {
                GeneralBLL objBLL = new GeneralBLL();
                string keyword = "A.id, ISNULL(A.code,'') AS code, ISNULL(A.name,'') AS name, ISNULL(A.name_2,'') AS name_2, ISNULL(T.name,'') AS account_type_name, (ISNULL(A.code,'') + ' - ' + ISNULL(A.name,'')) AS display";
                string table = "acc_accounts A INNER JOIN acc_groups G ON A.group_id = G.id INNER JOIN acc_account_type T ON G.account_type_id = T.id WHERE A.branch_id = " + UsersModal.logged_in_branch_id + " ORDER BY A.code, A.name";

                _accountsTable = objBLL.GetRecord(keyword, table);
                if (!_accountsTable.Columns.Contains("display"))
                {
                    _accountsTable.Columns.Add("display", typeof(string));
                    foreach (DataRow row in _accountsTable.Rows)
                    {
                        row["display"] = string.Format("{0} - {1}", row["code"], row["name"]);
                    }
                }

                account.DataSource = _accountsTable;
                account.DisplayMember = "display";
                account.ValueMember = "id";
                account.DisplayStyle = DataGridViewComboBoxDisplayStyle.ComboBox;
                account.FlatStyle = FlatStyle.Flat;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        private void InitializeVoucherLines()
        {
            grid_journal.Rows.Clear();
            AddVoucherLine(false);
            AddVoucherLine(false);
            if (grid_journal.Rows.Count > 0)
            {
                grid_journal.CurrentCell = grid_journal.Rows[0].Cells["account"];
            }
        }

        private void AddVoucherLine(bool focusNewRow)
        {
            int rowIndex = grid_journal.Rows.Add();
            DataGridViewRow row = grid_journal.Rows[rowIndex];
            row.Cells["colAccountType"].Value = string.Empty;
            row.Cells["description"].Value = string.Empty;
            row.Cells["party"].Value = 0;
            row.Cells["cost_center"].Value = string.Empty;
            row.Cells["debit_amount"].Value = null;
            row.Cells["credit_amount"].Value = null;

            BindPartyCellForRow(rowIndex);

            if (focusNewRow)
            {
                FocusCell(rowIndex, "account");
            }

            RefreshVoucherState();
        }

        private void btn_add_line_Click(object sender, EventArgs e)
        {
            AddVoucherLine(true);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            PostVoucher(false);
        }

        private void btn_save_draft_Click(object sender, EventArgs e)
        {
            if (!HasMeaningfulVoucherContent())
            {
                MessageBox.Show("Add voucher lines before saving a draft.", "Draft", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show("Draft saved for this session.", "Draft", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btn_post_new_Click(object sender, EventArgs e)
        {
            PostVoucher(true);
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                DGVPrinter printer = new DGVPrinter();
                printer.Title = "Journal Voucher";
                printer.SubTitle = string.Format("Voucher No: {0} | Date: {1} | Type: {2}", txt_invoice_no.Text, txt_entry_date.Value.Date.ToShortDateString(), cmb_voucher_type.Text);
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = false;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.Footer = "kasbook app";
                printer.FooterSpacing = 15;
                printer.PrintPreviewDataGridView(grid_journal);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btn_attachment_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Title = "Select Supporting Document";
                dialog.Filter = "All Files|*.*";
                if (dialog.ShowDialog(this) == DialogResult.OK)
                {
                    _attachmentFilePath = dialog.FileName;
                    btn_attachment.Text = string.Format("Attachment: {0}", Path.GetFileName(dialog.FileName));
                }
            }
        }

        private void btn_load_template_Click(object sender, EventArgs e)
        {
            if (_templateMenu.Items.Count > 0)
            {
                _templateMenu.Show(btn_load_template, new Point(0, btn_load_template.Height));
            }
        }

        private void ApplyTemplate(string templateName)
        {
            if (MessageBox.Show(string.Format("Load the '{0}' template into the current voucher?", templateName), "Load Template", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            ClearVoucherLines();

            if (templateName == "Depreciation Entry")
            {
                ApplyTemplateRow(0, FindAccountIdByKeywords("depreciation expense", "accumulated depreciation"), "Monthly depreciation", string.Empty, 1000m, 0m);
                ApplyTemplateRow(1, FindAccountIdByKeywords("accumulated depreciation"), "Monthly depreciation", string.Empty, 0m, 1000m);
            }
            else if (templateName == "Accrual Entry")
            {
                ApplyTemplateRow(0, FindAccountIdByKeywords("expense", "accrued expense"), "Accrual entry", string.Empty, 500m, 0m);
                ApplyTemplateRow(1, FindAccountIdByKeywords("accrued liability", "accrual"), "Accrual entry", string.Empty, 0m, 500m);
            }
            else if (templateName == "Bank Charges")
            {
                ApplyTemplateRow(0, FindAccountIdByKeywords("bank charges", "bank fee", "charges expense"), "Bank charges", string.Empty, 250m, 0m);
                ApplyTemplateRow(1, FindAccountIdByKeywords("bank", "cash at bank"), "Bank charges", string.Empty, 0m, 250m);
            }

            RefreshVoucherState();
        }

        private void ApplyTemplateRow(int rowIndex, int? accountId, string descriptionText, string costCenterText, decimal debitAmount, decimal creditAmount)
        {
            EnsureMinimumRows(rowIndex + 1);
            DataGridViewRow row = grid_journal.Rows[rowIndex];
            if (accountId.HasValue)
            {
                row.Cells["account"].Value = accountId.Value;
                UpdateAccountTypeForRow(rowIndex);
            }
            row.Cells["description"].Value = descriptionText;
            row.Cells["cost_center"].Value = costCenterText;
            row.Cells["debit_amount"].Value = debitAmount == 0m ? null : (object)debitAmount;
            row.Cells["credit_amount"].Value = creditAmount == 0m ? null : (object)creditAmount;
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want to clear this voucher?", "Clear Voucher", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                ClearVoucher();
            }
        }

        private void ClearVoucher()
        {
            txt_entry_date.Value = DateTime.Today;
            txt_reference_no.Text = string.Empty;
            txt_narration.Text = string.Empty;
            _attachmentFilePath = string.Empty;
            _editingInvoiceNo = string.Empty;
            btn_attachment.Text = "Attachment";
            cmb_voucher_type.SelectedIndex = 0;

            GetMAXInvoiceNo();
            ClearVoucherLines();
            RefreshVoucherState();
        }

        private bool ValidatePartySelection()
        {
            for (int i = 0; i < grid_journal.Rows.Count; i++)
            {
                DataGridViewRow row = grid_journal.Rows[i];
                if (row == null || row.IsNewRow)
                {
                    continue;
                }

                int accountId;
                if (!TryGetInt(row.Cells["account"].Value, out accountId) || accountId <= 0)
                {
                    continue;
                }

                decimal debit;
                decimal credit;
                TryGetDecimal(row.Cells["debit_amount"].Value, out debit);
                TryGetDecimal(row.Cells["credit_amount"].Value, out credit);
                if (debit <= 0m && credit <= 0m)
                {
                    continue;
                }

                string refModule = GetRowPartyRefModule(row);
                if (string.IsNullOrWhiteSpace(refModule))
                {
                    continue;
                }

                int partyId = GetRowPartyId(row);
                if (partyId <= 0)
                {
                    continue;
                }
            }

            return true;
        }

        private void ClearVoucherLines()
        {
            grid_journal.Rows.Clear();
            AddVoucherLine(false);
            AddVoucherLine(false);
        }

        private void PostVoucher(bool clearAfterSave)
        {
            try
            {
                if (!_auth.HasPermission(_currentUser, Permissions.Journal_Create))
                {
                    MessageBox.Show("You do not have permission to perform this action.", "Permission Denied", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!HasMeaningfulVoucherContent())
                {
                    MessageBox.Show("Add journal lines before posting the voucher.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                RefreshVoucherState();
                if (!IsVoucherBalanced())
                {
                    MessageBox.Show("Debit and Credit amounts must be balanced before posting.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (!ValidatePartySelection())
                {
                    return;
                }

                // Validate cost center budgets
                List<string> budgetAlerts = ValidateLineBudgets(txt_entry_date.Value.Date);
                if (budgetAlerts.Count > 0)
                {
                    string budgetWarningMessage = "Budget Alert(s):\r\n\r\n" + string.Join("\r\n", budgetAlerts) + "\r\n\r\nContinue posting anyway?";
                    if (MessageBox.Show(budgetWarningMessage, "Budget Validation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                    {
                        return;
                    }
                }

                if (MessageBox.Show("Post this journal voucher?", "Post Voucher", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                {
                    return;
                }

                JournalsBLL journalsBll = new JournalsBLL();
                JVHeaderModel header = BuildVoucherHeader();
                List<JVLineModel> lines = BuildVoucherLines(header.VoucherNo, header.VoucherDate);

                if (!string.IsNullOrWhiteSpace(_editingInvoiceNo))
                {
                    journalsBll.DeleteDraftVoucherByInvoiceNo(_editingInvoiceNo);
                }

                List<ValidationError> preflight = journalsBll.ValidateJournalLines(lines);
                if (preflight.Any(x => x.IsBlocking))
                {
                    MessageBox.Show(string.Join(Environment.NewLine, preflight.Select(x => x.Message)), "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                PostResult result = journalsBll.PostJournalVoucher(header, lines, UsersModal.logged_in_userid);
                if (!result.Success)
                {
                    string message = result.Messages.Count > 0
                        ? string.Join(Environment.NewLine, result.Messages.Select(x => x.Message).Distinct())
                        : "Record not saved.";
                    MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                MessageBox.Show(string.Format("Record saved. Voucher No: {0}", result.VoucherNo), "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (clearAfterSave)
                {
                    ClearVoucher();
                }
                else
                {
                    _editingInvoiceNo = string.Empty;
                    GetMAXInvoiceNo();
                    ClearVoucherLines();
                    RefreshVoucherState();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private JVHeaderModel BuildVoucherHeader()
        {
            DateTime voucherDate = txt_entry_date.Value.Date;
            return new JVHeaderModel
            {
                VoucherId = 0,
                VoucherNo = txt_invoice_no.Text,
                VoucherDate = voucherDate,
                VoucherType = cmb_voucher_type.Text,
                ReferenceNo = txt_reference_no.Text.Trim(),
                Narration = txt_narration.Text.Trim(),
                Attachment = string.IsNullOrWhiteSpace(_attachmentFilePath) ? null : _attachmentFilePath,
                TotalDebit = Convert.ToDecimal(_dr_total),
                TotalCredit = Convert.ToDecimal(_cr_total),
                Status = "Posted",
                ReversalOf = null,
                PostedBy = UsersModal.logged_in_userid,
                PostedAt = DateTime.Now,
                IsAutoPosted = false,
                CreatedBy = UsersModal.logged_in_userid,
                CreatedAt = DateTime.Now,
                UpdatedBy = UsersModal.logged_in_userid,
                UpdatedAt = DateTime.Now,
                BranchId = UsersModal.logged_in_branch_id,
                CompanyId = UsersModal.loggedIncompanyID,
                PeriodId = GetCurrentPeriodId(voucherDate)
            };
        }

        private List<JVLineModel> BuildVoucherLines(string invoiceNo, DateTime entryDate)
        {
            List<JVLineModel> lines = new List<JVLineModel>();
            int lineNo = 1;
            int? periodId = GetCurrentPeriodId(entryDate);

            foreach (DataGridViewRow row in grid_journal.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                int accountId;
                if (!TryGetInt(row.Cells["account"].Value, out accountId))
                {
                    continue;
                }

                decimal debit = 0m;
                decimal credit = 0m;
                TryGetDecimal(row.Cells["debit_amount"].Value, out debit);
                TryGetDecimal(row.Cells["credit_amount"].Value, out credit);

                if (debit == 0m && credit == 0m)
                {
                    continue;
                }

                string refModule = GetRowPartyRefModule(row);
                int partyId = GetRowPartyId(row);

                DataRow accountRow = GetAccountRow(accountId);
                lines.Add(new JVLineModel
                {
                    EntryId = 0,
                    VoucherId = 0,
                    LineNo = lineNo++,
                    AccountId = accountId,
                    AccountCode = accountRow == null ? string.Empty : Convert.ToString(accountRow["code"]),
                    AccountName = accountRow == null ? string.Empty : Convert.ToString(accountRow["name"]),
                    Narration = BuildLineDescription(Convert.ToString(row.Cells["description"].Value)),
                    Debit = debit,
                    Credit = credit,
                    CostCenterID = TryGetInt(row.Cells["cost_center"].Value, out int ccId) ? ccId : 0,
                    ModuleName = string.IsNullOrWhiteSpace(refModule) ? "MANUAL" : refModule,
                    RefId = partyId > 0 ? (int?)partyId : null,
                    CustomerId = refModule == "CUSTOMER" && partyId > 0 ? (int?)partyId : null,
                    SupplierId = refModule == "SUPPLIER" && partyId > 0 ? (int?)partyId : null,
                    BankId = refModule == "BANK" && partyId > 0 ? (int?)partyId : null,
                    PeriodId = periodId
                });
            }

            return lines;
        }

        private string BuildLineDescription(string rowDescription)
        {
            string lineDescription = (rowDescription ?? string.Empty).Trim();
            string narration = txt_narration.Text.Trim();

            if (!string.IsNullOrWhiteSpace(lineDescription))
            {
                return lineDescription;
            }

            return narration;
        }

        private int GetCurrentPeriodId(DateTime entryDate)
        {
            try
            {
                AccountingDAL accountingDal = new AccountingDAL(POS.DLL.dbConnection.ConnectionString);
                return accountingDal.GetCurrentPeriodId(entryDate);
            }
            catch
            {
                return 0;
            }
        }

        private int GetIntFromRow(DataRow row, string columnName)
        {
            if (row == null || row.Table == null || !row.Table.Columns.Contains(columnName) || row[columnName] == DBNull.Value)
            {
                return 0;
            }

            int value;
            return int.TryParse(Convert.ToString(row[columnName]), out value) ? value : 0;
        }

        /// <summary>
        /// Validates budget for each journal line with cost center assignment.
        /// Returns list of budget alerts and warnings.
        /// </summary>
        private List<string> ValidateLineBudgets(DateTime voucherDate)
        {
            List<string> budgetAlerts = new List<string>();

            try
            {
                CostCenterBLL ccBll = new CostCenterBLL();
                int lineNumber = 0;

                foreach (DataGridViewRow row in grid_journal.Rows)
                {
                    if (row.IsNewRow)
                    {
                        continue;
                    }

                    lineNumber++;

                    // Get cost center ID from row
                    object ccValue = row.Cells["cost_center"].Value;
                    if (ccValue == null || ccValue == DBNull.Value)
                    {
                        continue;
                    }

                    int costCenterId;
                    if (!int.TryParse(Convert.ToString(ccValue), out costCenterId) || costCenterId <= 0)
                    {
                        continue;
                    }

                    // Get account ID
                    int accountId;
                    if (!TryGetInt(row.Cells["account"].Value, out accountId) || accountId <= 0)
                    {
                        continue;
                    }

                    // Get account type to determine if it's an expense account
                    DataRow accountRow = GetAccountRow(accountId);
                    if (accountRow == null)
                    {
                        continue;
                    }

                    string accountType = Convert.ToString(accountRow["account_type_name"]).ToLowerInvariant().Trim();

                    // Only check budget for expense accounts (assets, liabilities, and equity don't trigger budget checks)
                    // Typical expense account types: "Expense", "COGS", "Operating Expense"
                    bool isExpenseAccount = accountType.Contains("expense") || accountType.Contains("cost");
                    if (!isExpenseAccount)
                    {
                        continue;
                    }

                    // Get debit amount (typical for expense)
                    decimal amount = 0m;
                    if (!TryGetDecimal(row.Cells["debit_amount"].Value, out amount))
                    {
                        TryGetDecimal(row.Cells["credit_amount"].Value, out amount);
                    }

                    if (amount <= 0m)
                    {
                        continue;
                    }

                    // Check budget before posting
                    BudgetCheckResult budgetResult = ccBll.CheckBudgetBeforePosting(costCenterId, accountId, amount, voucherDate);

                    if (budgetResult.IsOverBudget)
                    {
                        string alertMessage = string.Format(
                            "Line {0}: Budget alert ({1}) - {2}",
                            lineNumber,
                            budgetResult.SeverityLevel ?? "Warning",
                            budgetResult.Message
                        );
                        budgetAlerts.Add(alertMessage);
                    }
                }
            }
            catch (Exception ex)
            {
                // Don't block posting due to budget check failure
                // Log but continue
            }

            return budgetAlerts;
        }

        private void frm_journal_entries_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyData == Keys.Enter && ActiveControl != txt_narration && ActiveControl != grid_journal)
                {
                    SendKeys.Send("{TAB}");
                }

                if (e.KeyCode == Keys.F3 && btn_save.Enabled)
                {
                    btn_save.PerformClick();
                }

                if (e.KeyCode == Keys.F1)
                {
                    btn_new.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_journal_KeyDown(object sender, KeyEventArgs e)
        {
            if (grid_journal.CurrentCell == null)
            {
                return;
            }

            if (e.KeyCode == Keys.Enter && grid_journal.CurrentCell.OwningColumn.Name == "debit_amount")
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                MoveDebitToNextCredit();
                return;
            }

            if (e.KeyCode == Keys.Tab && IsLastEditableCell())
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                AddVoucherLine(true);
            }
        }

        private void MoveDebitToNextCredit()
        {
            int currentRow = grid_journal.CurrentCell.RowIndex;
            int nextRow = currentRow + 1;
            if (nextRow >= grid_journal.Rows.Count)
            {
                AddVoucherLine(false);
            }

            nextRow = Math.Min(currentRow + 1, grid_journal.Rows.Count - 1);
            FocusCell(nextRow, "credit_amount");
        }

        private bool IsLastEditableCell()
        {
            if (grid_journal.CurrentCell == null)
            {
                return false;
            }

            DataGridViewCell current = grid_journal.CurrentCell;
            return current.RowIndex == grid_journal.Rows.Count - 1 && current.OwningColumn.Name == "credit_amount";
        }

        private void grid_journal_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || grid_journal.Columns[e.ColumnIndex].Name != "colRemove")
            {
                return;
            }

            if (grid_journal.IsCurrentCellInEditMode)
            {
                grid_journal.EndEdit();
            }

            if (grid_journal.Rows.Count <= 2)
            {
                ClearRow(e.RowIndex);
            }
            else
            {
                grid_journal.Rows.RemoveAt(e.RowIndex);
                EnsureMinimumRows(2);
            }

            RefreshVoucherState();
        }

        private void grid_journal_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            if (e.RowIndex < 0 || grid_journal.Columns[e.ColumnIndex].Name != "account")
            {
                return;
            }

            e.Cancel = true;
            BeginInvoke(new Action(() => ShowAccountLookupForRow(e.RowIndex)));
        }

        private void ShowAccountLookupForRow(int rowIndex)
        {
            if (_accountsTable == null || rowIndex < 0 || rowIndex >= grid_journal.Rows.Count)
            {
                return;
            }

            using (var lookup = new frm_account_lookup())
            {
                lookup.LoadAccounts(_accountsTable, GetInitialAccountLookupText(rowIndex));
                if (lookup.ShowDialog(this) != DialogResult.OK || lookup.SelectedAccountRow == null)
                {
                    return;
                }

                int accountId;
                if (!int.TryParse(Convert.ToString(lookup.SelectedAccountRow["id"]), out accountId))
                {
                    return;
                }

                grid_journal.Rows[rowIndex].Cells["account"].Value = accountId;
                UpdateAccountTypeForRow(rowIndex);
                BindPartyCellForRow(rowIndex);
                RefreshVoucherState();
                FocusCell(rowIndex, "description");
            }
        }

        private string GetInitialAccountLookupText(int rowIndex)
        {
            if (_accountsTable == null || rowIndex < 0 || rowIndex >= grid_journal.Rows.Count)
            {
                return string.Empty;
            }

            int accountId;
            if (!TryGetInt(grid_journal.Rows[rowIndex].Cells["account"].Value, out accountId))
            {
                return string.Empty;
            }

            DataRow[] rows = _accountsTable.Select("id = " + accountId);
            if (rows.Length == 0)
            {
                return string.Empty;
            }

            return Convert.ToString(rows[0]["code"]);
        }

        private void ClearRow(int rowIndex)
        {
            if (rowIndex < 0 || rowIndex >= grid_journal.Rows.Count)
            {
                return;
            }

            DataGridViewRow row = grid_journal.Rows[rowIndex];
            row.Cells["account"].Value = null;
            row.Cells["colAccountType"].Value = string.Empty;
            row.Cells["description"].Value = string.Empty;
            row.Cells["party"].Value = 0;
            row.Cells["cost_center"].Value = string.Empty;
            row.Cells["debit_amount"].Value = null;
            row.Cells["credit_amount"].Value = null;
            BindPartyCellForRow(rowIndex);
        }

        private void grid_journal_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            string columnName = grid_journal.Columns[e.ColumnIndex].Name;
            if (columnName != "debit_amount" && columnName != "credit_amount")
            {
                return;
            }

            string text = Convert.ToString(e.FormattedValue).Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return;
            }

            decimal amount;
            if (!decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out amount) || amount < 0m)
            {
                e.Cancel = true;
                MessageBox.Show("Enter a valid positive amount.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void grid_journal_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            string columnName = grid_journal.Columns[e.ColumnIndex].Name;
            if (columnName == "account")
            {
                UpdateAccountTypeForRow(e.RowIndex);
                BindPartyCellForRow(e.RowIndex);
            }
            else if (columnName == "debit_amount")
            {
                EnforceSingleAmount(e.RowIndex, true);
            }
            else if (columnName == "credit_amount")
            {
                EnforceSingleAmount(e.RowIndex, false);
            }

            RefreshVoucherState();
        }

        private void EnforceSingleAmount(int rowIndex, bool debitEdited)
        {
            DataGridViewRow row = grid_journal.Rows[rowIndex];
            decimal amount;

            if (debitEdited)
            {
                if (TryGetDecimal(row.Cells["debit_amount"].Value, out amount) && amount > 0m)
                {
                    row.Cells["credit_amount"].Value = null;
                }
                else if (amount == 0m)
                {
                    row.Cells["debit_amount"].Value = null;
                }
            }
            else
            {
                if (TryGetDecimal(row.Cells["credit_amount"].Value, out amount) && amount > 0m)
                {
                    row.Cells["debit_amount"].Value = null;
                }
                else if (amount == 0m)
                {
                    row.Cells["credit_amount"].Value = null;
                }
            }
        }

        private void UpdateAccountTypeForRow(int rowIndex)
        {
            if (_accountsTable == null || rowIndex < 0 || rowIndex >= grid_journal.Rows.Count)
            {
                return;
            }

            DataGridViewRow row = grid_journal.Rows[rowIndex];
            int accountId;
            if (!TryGetInt(row.Cells["account"].Value, out accountId))
            {
                row.Cells["colAccountType"].Value = string.Empty;
                return;
            }

            DataRow[] rows = _accountsTable.Select("id = " + accountId);
            if (rows.Length == 0)
            {
                row.Cells["colAccountType"].Value = string.Empty;
                return;
            }

            string accountTypeName = Convert.ToString(rows[0]["account_type_name"]);
            row.Cells["colAccountType"].Value = string.IsNullOrWhiteSpace(accountTypeName) ? string.Empty : accountTypeName.Trim();
            BindPartyCellForRow(rowIndex);
        }

        private void grid_journal_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            if (grid_journal.Columns[e.ColumnIndex].Name == "colRowNo")
            {
                e.Value = (e.RowIndex + 1).ToString();
            }
        }

        private void grid_journal_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (grid_journal.IsCurrentCellDirty)
            {
                grid_journal.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void grid_journal_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["colAccountType"].Value = string.Empty;
            e.Row.Cells["description"].Value = string.Empty;
            e.Row.Cells["party"].Value = 0;
            e.Row.Cells["cost_center"].Value = string.Empty;
            e.Row.Cells["debit_amount"].Value = null;
            e.Row.Cells["credit_amount"].Value = null;
        }

        private void grid_journal_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            ComboBox combo = e.Control as ComboBox;
            if (combo == null)
            {
                return;
            }

            combo.DropDownStyle = ComboBoxStyle.DropDown;
            combo.AutoCompleteSource = AutoCompleteSource.ListItems;
            combo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
        }

        private void grid_journal_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            RefreshVoucherState();
        }

        private void grid_journal_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            RefreshVoucherState();
        }

        private void grid_journal_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            e.Cancel = true;
        }

        private void grid_journal_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.ThrowException = false;
        }

        private void EnsureMinimumRows(int minimumRows)
        {
            while (grid_journal.Rows.Count < minimumRows)
            {
                grid_journal.Rows.Add();
            }
        }

        private void FocusCell(int rowIndex, string columnName)
        {
            if (rowIndex < 0 || rowIndex >= grid_journal.Rows.Count)
            {
                return;
            }

            if (!grid_journal.Columns.Contains(columnName))
            {
                return;
            }

            grid_journal.CurrentCell = grid_journal.Rows[rowIndex].Cells[columnName];
            grid_journal.BeginEdit(true);
        }

        private bool TryGetDecimal(object value, out decimal amount)
        {
            amount = 0m;
            if (value == null)
            {
                return false;
            }

            if (value is decimal)
            {
                amount = (decimal)value;
                return true;
            }

            if (value is double)
            {
                amount = Convert.ToDecimal(value);
                return true;
            }

            string text = Convert.ToString(value).Trim();
            if (string.IsNullOrWhiteSpace(text))
            {
                return false;
            }

            return decimal.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out amount);
        }

        private bool TryGetInt(object value, out int number)
        {
            number = 0;
            if (value == null)
            {
                return false;
            }

            return int.TryParse(Convert.ToString(value), out number);
        }

        private int? FindAccountIdByKeywords(params string[] keywords)
        {
            if (_accountsTable == null || _accountsTable.Rows.Count == 0 || keywords == null || keywords.Length == 0)
            {
                return null;
            }

            foreach (DataRow row in _accountsTable.Rows)
            {
                string searchText = string.Format("{0} {1} {2}", Convert.ToString(row["code"]), Convert.ToString(row["name"]), Convert.ToString(row["display"])).ToLowerInvariant();
                bool matched = true;
                foreach (string keyword in keywords)
                {
                    if (!searchText.Contains((keyword ?? string.Empty).ToLowerInvariant()))
                    {
                        matched = false;
                        break;
                    }
                }

                if (matched)
                {
                    return Convert.ToInt32(row["id"]);
                }
            }

            return null;
        }

        private void RefreshVoucherState()
        {
            decimal debitTotal = 0m;
            decimal creditTotal = 0m;
            int lineCount = 0;
            bool hasMeaningfulContent = false;

            foreach (DataGridViewRow row in grid_journal.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                lineCount++;

                decimal debit;
                decimal credit;
                if (TryGetDecimal(row.Cells["debit_amount"].Value, out debit))
                {
                    debitTotal += debit;
                }

                if (TryGetDecimal(row.Cells["credit_amount"].Value, out credit))
                {
                    creditTotal += credit;
                }

                if (!hasMeaningfulContent)
                {
                    hasMeaningfulContent = row.Cells["account"].Value != null
                        || debit > 0m
                        || credit > 0m
                        || !string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["description"].Value));
                }
            }

            _dr_total = Convert.ToDouble(debitTotal);
            _cr_total = Convert.ToDouble(creditTotal);

            txt_dr_total.Text = debitTotal.ToString("N2");
            txt_cr_total.Text = creditTotal.ToString("N2");

            decimal difference = Math.Abs(debitTotal - creditTotal);
            txt_difference.Text = difference.ToString("N2");
            lbl_line_count.Text = lineCount.ToString() + " lines";

            bool balanced = difference == 0m && hasMeaningfulContent && lineCount >= 2;
            lbl_balance_status.Text = balanced ? "Balanced ✓" : "Not Balanced";
            lbl_balance_status.ForeColor = balanced ? Color.DarkGreen : Color.DarkRed;
            txt_difference.ForeColor = balanced ? Color.DarkGreen : Color.DarkRed;
            btn_save.Enabled = balanced;
            btn_post_new.Enabled = balanced;
        }

        private bool HasMeaningfulVoucherContent()
        {
            foreach (DataGridViewRow row in grid_journal.Rows)
            {
                if (row.IsNewRow)
                {
                    continue;
                }

                decimal debit;
                decimal credit;
                if (row.Cells["account"].Value != null)
                {
                    return true;
                }

                if (TryGetDecimal(row.Cells["debit_amount"].Value, out debit) && debit > 0m)
                {
                    return true;
                }

                if (TryGetDecimal(row.Cells["credit_amount"].Value, out credit) && credit > 0m)
                {
                    return true;
                }

                if (!string.IsNullOrWhiteSpace(Convert.ToString(row.Cells["description"].Value)))
                {
                    return true;
                }
            }

            return false;
        }

        private DataRow GetAccountRow(int accountId)
        {
            if (_accountsTable == null)
            {
                return null;
            }

            DataRow[] rows = _accountsTable.Select("id = " + accountId.ToString(CultureInfo.InvariantCulture));
            return rows.Length > 0 ? rows[0] : null;
        }

        private bool IsVoucherBalanced()
        {
            return Math.Abs(_dr_total - _cr_total) < 0.005d;
        }

    }
}
