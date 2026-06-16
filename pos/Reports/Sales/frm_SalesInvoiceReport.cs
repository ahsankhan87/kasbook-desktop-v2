using pos.Reports.Common;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.Security.Authorization;
using pos.UI;

namespace pos.Reports.Sales
{
    public partial class frm_SalesInvoiceReport : Form
    {
        public string _product_code = "";
        public string _product_name;
        DataTable sales_invoice_report_dt = new DataTable();
        private DataView _salesInvoiceReportView;

        private DataGridView customersDataGridView;
        private System.Windows.Forms.Timer _customerSearchDebounceTimer;
        private bool _suppressCustomerSearch;
        private int _selectedCustomerId;

        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        ProductBLL productsBLL_obj = new ProductBLL();

        public frm_SalesInvoiceReport()
        {
            InitializeComponent();

            _customerSearchDebounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _customerSearchDebounceTimer.Tick += CustomerSearchDebounceTimer_Tick;

            if (txtCustomerSearch != null)
            {
                txtCustomerSearch.TextChanged += txtCustomerSearch_TextChanged;
                txtCustomerSearch.KeyUp += txtCustomerSearch_KeyUp;
                txtCustomerSearch.Leave += txtCustomerSearch_Leave;
            }

            if (txt_search != null)
            {
                txt_search.TextChanged += txt_search_TextChanged;
            }
        }

        private void frm_SalesInvoiceReport_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            CmbCondition.Items.AddRange(new string[]
            {
                "Custom", "Today", "Yesterday", "This Week", "Last Week",
                "This Month", "Last Month", "This Quarter", "Last Quarter",
                "This Year", "Last Year", "Year to Date (YTD)", "Last 7 Days",
                "Last 30 Days", "Last 90 Days", "Last 6 Months",
                "Previous Fiscal Year", "Next Fiscal Year"
            });
            CmbCondition.SelectedIndex = 0;
            cmb_sale_type.SelectedIndex = 0;
            cmb_sale_account.SelectedIndex = 0;
            get_employees_dropdownlist();

            SetupCustomersDataGridView();
            ApplyProfitColumnVisibility();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_sales_invoice_report, id);
        }

        public void get_employees_dropdownlist()
        {
            EmployeeBLL employeeBLL = new EmployeeBLL();

            DataTable employees = employeeBLL.GetAll();
            DataRow emptyRow = employees.NewRow();
            emptyRow[0] = 0;
            emptyRow[2] = "All Employee";
            employees.Rows.InsertAt(emptyRow, 0);

            cmb_employees.DisplayMember = "first_name";
            cmb_employees.ValueMember = "id";
            cmb_employees.DataSource = employees;
        }

        private async void btn_search_Click(object sender, EventArgs e)
        {
            using (pos.UI.Busy.BusyScope.Show(this, "Loading sales invoice report..."))
            {
                try
                {
                    DateTime from_date = txt_from_date.Value.Date;
                    DateTime to_date = txt_to_date.Value.Date;
                    int customer_id = _selectedCustomerId;
                    string product_code = _product_code;
                    string sale_type = cmb_sale_type.SelectedItem.ToString();
                    int employee_id = Convert.ToInt16(cmb_employees.SelectedValue);
                    string sale_account = cmb_sale_account.SelectedItem.ToString();
                    int branch_id = UsersModal.logged_in_branch_id;
                    bool showZatcaSkipInvoice = chk_ShowZatcaInvoice.Checked;

                    grid_sales_invoice_report.AutoGenerateColumns = false;

                    sales_invoice_report_dt = await Task.Run(() =>
                    {
                        SalesReportBLL sale_report_obj = new SalesReportBLL();
                        return sale_report_obj.SalesInvoiceReport(from_date, to_date, customer_id, product_code, sale_type, employee_id, sale_account, branch_id, showZatcaSkipInvoice);
                    });

                    bool showProfit = CanViewProfit();

                    double _total_items_sum = 0;
                    double _subtotal_sum = 0;
                    double _discount_value_sum = 0;
                    double _vat_sum = 0;
                    double _total_sum = 0;
                    double _total_with_vat_sum = 0;
                    double _profit_sum = 0;
                    double _cost_total_sum = 0;

                    foreach (DataRow dr in sales_invoice_report_dt.Rows)
                    {
                        _total_items_sum += (dr["total_items"] == DBNull.Value ? 0 : Convert.ToDouble(dr["total_items"]));
                        _subtotal_sum += (dr["subtotal"] == DBNull.Value ? 0 : Convert.ToDouble(dr["subtotal"]));
                        _discount_value_sum += (dr["discount_value"] == DBNull.Value ? 0 : Convert.ToDouble(dr["discount_value"]));
                        _vat_sum += (dr["vat"] == DBNull.Value ? 0 : Convert.ToDouble(dr["vat"]));
                        _total_sum += (dr["total"] == DBNull.Value ? 0 : Convert.ToDouble(dr["total"]));
                        _total_with_vat_sum += (dr["total_with_vat"] == DBNull.Value ? 0 : Convert.ToDouble(dr["total_with_vat"]));

                        if (showProfit)
                        {
                            _profit_sum += (dr["profit"] == DBNull.Value ? 0 : Convert.ToDouble(dr["profit"]));
                            _cost_total_sum += (dr["cost_total"] == DBNull.Value ? 0 : Convert.ToDouble(dr["cost_total"]));
                        }
                        else
                        {
                            dr["profit"] = 0;
                            dr["cost_total"] = 0;
                        }
                    }

                    if (sales_invoice_report_dt.Rows.Count > 0)
                    {
                        DataRow newRow = sales_invoice_report_dt.NewRow();
                        newRow["invoice_no"] = "Total";
                        newRow["total_items"] = _total_items_sum;
                        newRow["subtotal"] = _subtotal_sum;
                        newRow["discount_value"] = _discount_value_sum;
                        newRow["vat"] = _vat_sum;
                        newRow["total"] = _total_sum;
                        newRow["total_with_vat"] = _total_with_vat_sum;

                        if (showProfit)
                        {
                            newRow["profit"] = _profit_sum;
                            newRow["cost_total"] = _cost_total_sum;
                        }

                        sales_invoice_report_dt.Rows.InsertAt(newRow, sales_invoice_report_dt.Rows.Count);
                    }

                    _salesInvoiceReportView = new DataView(sales_invoice_report_dt);
                    grid_sales_invoice_report.DataSource = _salesInvoiceReportView;
                    ApplyGridInvoiceFilter();
                    if (grid_sales_invoice_report.Rows.Count > 0)
                        CustomizeDataGridView();

                    ApplyProfitColumnVisibility();

                    _product_code = "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void CustomizeDataGridView()
        {
            DataGridViewRow lastRow = grid_sales_invoice_report.Rows[grid_sales_invoice_report.Rows.Count - 1];

            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);
                style.Font = new Font(grid_sales_invoice_report.Font, FontStyle.Bold);
                style.BackColor = Color.LightGray;
                cell.Style = style;
            }
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            ApplyGridInvoiceFilter();
        }

        private void ApplyGridInvoiceFilter()
        {
            if (_salesInvoiceReportView == null)
                return;

            string keyword = (txt_search.Text ?? string.Empty).Trim().Replace("'", "''");

            if (string.IsNullOrWhiteSpace(keyword))
            {
                _salesInvoiceReportView.RowFilter = string.Empty;
            }
            else
            {
                _salesInvoiceReportView.RowFilter =
                    $"(invoice_no LIKE '%{keyword}%' OR customer_name LIKE '%{keyword}%')";
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                if (sales_invoice_report_dt == null || sales_invoice_report_dt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to print. Please run a search first.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DGVPrinterHelper.DGVPrinter printer = new DGVPrinterHelper.DGVPrinter();

                // Set page orientation to landscape for better fit
                printer.PageSettings.Landscape = true;

                // Configure title and subtitle
                printer.Title = "Sales Invoice Report";
                printer.SubTitle = string.Format("From {0} To {1}", txt_from_date.Value.ToShortDateString(), txt_to_date.Value.ToShortDateString());
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.Footer = string.Empty;
                printer.FooterSpacing = 15;

                // Use CellWidth mode to respect actual column widths
                printer.ColumnWidth = DGVPrinterHelper.DGVPrinter.ColumnWidthSetting.CellWidth;

                // Set explicit column widths to fit content better
                printer.ColumnWidths.Clear();

                foreach (DataGridViewColumn col in grid_sales_invoice_report.Columns)
                {
                    if (col.Visible)
                    {
                        float width = 0;

                        // Set optimized widths for each column
                        switch (col.Name)
                        {
                            case "id":
                                width = 40;
                                break;
                            case "sale_date":
                                width = 80;
                                break;
                            case "invoice_no":
                                width = 90;
                                break;
                            case "customer_name":
                                width = 150;
                                break;
                            case "total_items":
                                width = 50;
                                break;
                            case "subtotal":
                                width = 70;
                                break;
                            case "discount_value":
                                width = 70;
                                break;
                            case "vat":
                                width = 60;
                                break;
                            case "total":
                                width = 70;
                                break;
                            case "total_with_vat":
                                width = 80;
                                break;
                            case "profit":
                                width = 70;
                                break;
                            case "cost_total":
                                width = 70;
                                break;
                            default:
                                width = 70;
                                break;
                        }

                        printer.ColumnWidths.Add(col.Name, width);
                    }
                }

                // Set header and cell alignment
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.CellAlignment = StringAlignment.Near;

                // Print preview
                printer.PrintPreviewDataGridView(grid_sales_invoice_report);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                if (sales_invoice_report_dt == null || sales_invoice_report_dt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to export. Please run a search first.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Clone the data table and remove profit/cost columns if user doesn't have permission
                DataTable exportDt = sales_invoice_report_dt.Copy();
                bool showProfit = CanViewProfit();

                if (!showProfit)
                {
                    if (exportDt.Columns.Contains("profit"))
                        exportDt.Columns.Remove("profit");
                    if (exportDt.Columns.Contains("cost_total"))
                        exportDt.Columns.Remove("cost_total");
                }

                string range = $"{txt_from_date.Value:yyyyMMdd}-{txt_to_date.Value:yyyyMMdd}";
                string defaultName = $"SalesInvoiceReport_{range}";

                ExcelExportHelper.ExportDataTableToExcel(exportDt, defaultName, this, includeLastRow: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CmbCondition_SelectedIndexChanged(object sender, EventArgs e)
        {
            DateTime today = DateTime.Today;
            DateTime startDate = today;
            DateTime endDate = today;

            switch (CmbCondition.SelectedItem.ToString())
            {
                case "Custom":
                    return;
                case "Today":
                    startDate = endDate = today;
                    break;
                case "Yesterday":
                    startDate = endDate = today.AddDays(-1);
                    break;
                case "This Week":
                    startDate = today.AddDays(-(int)today.DayOfWeek);
                    endDate = startDate.AddDays(6);
                    break;
                case "Last Week":
                    startDate = today.AddDays(-(int)today.DayOfWeek - 7);
                    endDate = startDate.AddDays(6);
                    break;
                case "This Month":
                    startDate = new DateTime(today.Year, today.Month, 1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case "Last Month":
                    startDate = new DateTime(today.Year, today.Month, 1).AddMonths(-1);
                    endDate = startDate.AddMonths(1).AddDays(-1);
                    break;
                case "This Quarter":
                    startDate = new DateTime(today.Year, ((today.Month - 1) / 3) * 3 + 1, 1);
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;
                case "Last Quarter":
                    startDate = new DateTime(today.Year, ((today.Month - 1) / 3) * 3 + 1, 1).AddMonths(-3);
                    endDate = startDate.AddMonths(3).AddDays(-1);
                    break;
                case "Year to Date (YTD)":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = today;
                    break;
                case "Last 7 Days":
                    startDate = today.AddDays(-6);
                    break;
                case "Last 30 Days":
                    startDate = today.AddDays(-29);
                    break;
                case "Last 90 Days":
                    startDate = today.AddDays(-89);
                    break;
                case "Last 6 Months":
                    startDate = today.AddMonths(-6);
                    break;
                case "This Year":
                    startDate = new DateTime(today.Year, 1, 1);
                    endDate = new DateTime(today.Year, 12, 31);
                    break;
                case "Last Year":
                    startDate = new DateTime(today.Year - 1, 1, 1);
                    endDate = new DateTime(today.Year - 1, 12, 31);
                    break;
                case "Previous Fiscal Year":
                    startDate = new DateTime(today.Year - 1, 1, 1);
                    endDate = new DateTime(today.Year - 1, 12, 31);
                    break;
                case "Next Fiscal Year":
                    startDate = new DateTime(today.Year + 1, 1, 1);
                    endDate = new DateTime(today.Year + 1, 12, 31);
                    break;
            }

            txt_from_date.Value = startDate;
            txt_to_date.Value = endDate;
        }

        private bool CanViewProfit()
        {
            return _auth.HasPermission(_currentUser, Permissions.Reports_ProfitLossView);
        }

        private void ApplyProfitColumnVisibility()
        {
            bool show = CanViewProfit();

            if (grid_sales_invoice_report.Columns.Contains("profit"))
                grid_sales_invoice_report.Columns["profit"].Visible = show;

            if (grid_sales_invoice_report.Columns.Contains("cost_total"))
                grid_sales_invoice_report.Columns["cost_total"].Visible = show;
        }

        private void SetupCustomersDataGridView()
        {
            if (customersDataGridView != null) return;

            customersDataGridView = new DataGridView();
            customersDataGridView.ColumnCount = 6;
            bool isRtl = RightToLeft == RightToLeft.Yes;

            customersDataGridView.Size = new Size(520, 240);
            customersDataGridView.BorderStyle = BorderStyle.None;
            customersDataGridView.BackgroundColor = Color.White;
            customersDataGridView.AutoGenerateColumns = false;
            customersDataGridView.ReadOnly = true;
            customersDataGridView.AllowUserToAddRows = false;
            customersDataGridView.AllowUserToDeleteRows = false;
            customersDataGridView.AllowUserToResizeRows = false;
            customersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customersDataGridView.MultiSelect = false;
            customersDataGridView.RowHeadersVisible = false;
            customersDataGridView.RightToLeft = isRtl ? RightToLeft.Yes : RightToLeft.No;
            customersDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            customersDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            customersDataGridView.Columns[0].Name = "Code";
            customersDataGridView.Columns[1].Name = "Name";
            customersDataGridView.Columns[2].Name = "ID";
            customersDataGridView.Columns[3].Name = "Contact";
            customersDataGridView.Columns[4].Name = "VAT No";
            customersDataGridView.Columns[5].Name = "Credit Limit";

            customersDataGridView.Columns[2].Visible = false;
            customersDataGridView.Columns[5].Visible = false;

            customersDataGridView.Columns[0].Width = 90;
            customersDataGridView.Columns[1].Width = 220;
            customersDataGridView.Columns[3].Width = 130;
            customersDataGridView.Columns[4].Width = 120;

            customersDataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Window,
                ForeColor = SystemColors.WindowText,
                Font = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                Padding = new Padding(6, 2, 6, 2)
            };
            customersDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Control,
                ForeColor = SystemColors.ControlText,
                Font = AppTheme.FontGridHeader,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText
            };
            customersDataGridView.EnableHeadersVisualStyles = false;
            customersDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            customersDataGridView.RowTemplate.Height = 28;
            customersDataGridView.ColumnHeadersHeight = 32;
            customersDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            customersDataGridView.CellClick += customersDataGridView_CellClick;
            customersDataGridView.KeyDown += customersDataGridView_KeyDown;

            customersDataGridView.Visible = false;
            this.Controls.Add(customersDataGridView);
            customersDataGridView.BringToFront();
        }

        private void PositionCustomersDropdown()
        {
            Point pt = this.PointToClient(
                txtCustomerSearch.Parent.PointToScreen(txtCustomerSearch.Location));
            int x = Math.Max(0, Math.Min(pt.X, this.ClientSize.Width - customersDataGridView.Width));
            customersDataGridView.Location = new Point(x, pt.Y + txtCustomerSearch.Height + 2);
        }

        private void RefreshCustomersData()
        {
            try
            {
                var customerSearch = txtCustomerSearch.Text ?? string.Empty;
                var normalizedSearch = new CustomerBLL().NormalizeCustomerCodeInput(customerSearch);

                if (_suppressCustomerSearch || !txtCustomerSearch.Focused)
                {
                    customersDataGridView.Visible = false;
                    return;
                }

                if (!string.IsNullOrWhiteSpace(normalizedSearch))
                {
                    DataTable dt = new CustomerBLL().SearchRecord(normalizedSearch) ?? new DataTable();
                    customersDataGridView.Rows.Clear();

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string[] row0 = {
                                dt.Columns.Contains("customer_code") ? dr["customer_code"].ToString() : "",
                                (dr["first_name"].ToString() + " " + dr["last_name"].ToString()).Trim(),
                                dr["id"].ToString(),
                                dt.Columns.Contains("contact_no") ? dr["contact_no"].ToString() : "",
                                dr["vat_no"].ToString(),
                                dt.Columns.Contains("credit_limit") ? dr["credit_limit"].ToString() : ""
                            };
                            customersDataGridView.Rows.Add(row0);
                        }
                        PositionCustomersDropdown();
                        customersDataGridView.Visible = true;
                        customersDataGridView.BringToFront();
                        customersDataGridView.ClearSelection();
                        customersDataGridView.CurrentCell = null;
                        if (customersDataGridView.Rows.Count > 0)
                            customersDataGridView.CurrentCell = customersDataGridView.Rows[0].Cells[0];
                    }
                    else
                    {
                        customersDataGridView.Visible = false;
                    }
                }
                else
                {
                    txtCustomerSearch.Text = "";
                    ClearSelectedCustomer();
                    customersDataGridView.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelectCustomerFromDataRow(DataRow dr)
        {
            _selectedCustomerId = Convert.ToInt32(dr["id"]);
            _suppressCustomerSearch = true;
            txtCustomerSearch.Text = (dr["first_name"].ToString() + " " + dr["last_name"].ToString()).Trim();
            _suppressCustomerSearch = false;
        }

        private void ClearSelectedCustomer()
        {
            _selectedCustomerId = 0;
        }

        private void CustomerSearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            _customerSearchDebounceTimer.Stop();
            if (_suppressCustomerSearch || !txtCustomerSearch.Focused) return;
            RefreshCustomersData();
        }

        private void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            if (_suppressCustomerSearch || !txtCustomerSearch.Focused) return;

            if (string.IsNullOrWhiteSpace(txtCustomerSearch.Text))
            {
                ClearSelectedCustomer();
            }

            _customerSearchDebounceTimer.Stop();
            _customerSearchDebounceTimer.Start();
        }

        private void txtCustomerSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (customersDataGridView.Visible && customersDataGridView.Rows.Count > 0)
                {
                    customersDataGridView.Focus();
                    if (customersDataGridView.CurrentRow == null)
                        customersDataGridView.CurrentCell = customersDataGridView.Rows[0].Cells[0];
                }
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                customersDataGridView.Visible = false;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                var normalizedSearch = new CustomerBLL().NormalizeCustomerCodeInput(txtCustomerSearch.Text);

                if (!string.IsNullOrWhiteSpace(normalizedSearch) && normalizedSearch.StartsWith("C-", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var bll = new CustomerBLL();
                        var dt = bll.SearchRecord(normalizedSearch);
                        if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("customer_code"))
                        {
                            var exact = dt.Select("customer_code = '" + normalizedSearch.Replace("'", "''") + "'");
                            if (exact.Length == 1)
                            {
                                SelectCustomerFromDataRow(exact[0]);
                                customersDataGridView.Visible = false;
                                btn_search.Focus();
                                return;
                            }
                        }
                        ClearSelectedCustomer();
                    }
                    catch { }
                }

                if (customersDataGridView.Visible && customersDataGridView.Rows.Count > 0)
                {
                    if (customersDataGridView.CurrentRow == null)
                        customersDataGridView.CurrentCell = customersDataGridView.Rows[0].Cells[0];

                    _selectedCustomerId = Convert.ToInt32(customersDataGridView.CurrentRow.Cells[2].Value);
                    _suppressCustomerSearch = true;
                    txtCustomerSearch.Text = customersDataGridView.CurrentRow.Cells[1].Value.ToString();
                    _suppressCustomerSearch = false;
                    customersDataGridView.Visible = false;
                    btn_search.Focus();
                }
            }

            _customerSearchDebounceTimer.Stop();
            _customerSearchDebounceTimer.Start();
        }

        private void txtCustomerSearch_Leave(object sender, EventArgs e)
        {
            _customerSearchDebounceTimer.Stop();
            if (!customersDataGridView.Focused)
                customersDataGridView.Visible = false;
        }

        private void customersDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                _selectedCustomerId = Convert.ToInt32(customersDataGridView.CurrentRow.Cells[2].Value);
                _suppressCustomerSearch = true;
                txtCustomerSearch.Text = customersDataGridView.CurrentRow.Cells[1].Value.ToString();
                _suppressCustomerSearch = false;
                customersDataGridView.Visible = false;
                btn_search.Focus();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                customersDataGridView.Visible = false;
                txtCustomerSearch.Focus();
            }
        }

        private void customersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _selectedCustomerId = Convert.ToInt32(customersDataGridView.CurrentRow.Cells[2].Value);
            _suppressCustomerSearch = true;
            txtCustomerSearch.Text = customersDataGridView.CurrentRow.Cells[1].Value.ToString();
            _suppressCustomerSearch = false;
            customersDataGridView.Visible = false;
            btn_search.Focus();
        }

        private void grid_sales_invoice_report_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= grid_sales_invoice_report.Rows.Count)
                return;

            var row = grid_sales_invoice_report.Rows[e.RowIndex];
            if (row.Cells["id"].Value == null || row.Cells["id"].Value == DBNull.Value)
                return;

            int saleId = Convert.ToInt32(row.Cells["id"].Value);
            string invoiceNo = row.Cells["invoice_no"].Value?.ToString() ?? "";

            frm_SaleProductDetail detailForm = new frm_SaleProductDetail(saleId, invoiceNo);
            detailForm.ShowDialog();
        }

        private void frm_SalesInvoiceReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.F3)
            {
                btn_search.PerformClick();
            }
            //print
            if (e.KeyCode == Keys.F4 || (e.Control && e.KeyCode == Keys.P))
            {
                btn_print.PerformClick();
            }
        }
    }
}
