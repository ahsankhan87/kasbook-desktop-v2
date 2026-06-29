using pos.Reports.Common;
using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.UI;

namespace pos.Reports.Purchases
{
    public partial class frm_PurchaseInvoiceReport : Form
    {
        private DataTable _purchaseInvoiceReportDt = new DataTable();
        private DataView _purchaseInvoiceReportView;

        private DataGridView suppliersDataGridView;
        private System.Windows.Forms.Timer _supplierSearchDebounceTimer;
        private bool _suppressSupplierSearch;
        private int _selectedSupplierId;

        public frm_PurchaseInvoiceReport()
        {
            InitializeComponent();

            _supplierSearchDebounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _supplierSearchDebounceTimer.Tick += SupplierSearchDebounceTimer_Tick;

            if (txtSupplierSearch != null)
            {
                txtSupplierSearch.TextChanged += txtSupplierSearch_TextChanged;
                txtSupplierSearch.KeyUp += txtSupplierSearch_KeyUp;
                txtSupplierSearch.Leave += txtSupplierSearch_Leave;
            }

            if (txt_search != null)
            {
                txt_search.TextChanged += txt_search_TextChanged;
            }
        }

        private void frm_PurchaseInvoiceReport_Load(object sender, EventArgs e)
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
            cmb_purchase_type.SelectedIndex = 0;
            get_employees_dropdownlist();
            SetupSuppliersDataGridView();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_purchase_invoice_report, col_id);
        }

        private void get_employees_dropdownlist()
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
            using (pos.UI.Busy.BusyScope.Show(this, "Loading purchase invoice report..."))
            {
                try
                {
                    DateTime from_date = txt_from_date.Value.Date;
                    DateTime to_date = txt_to_date.Value.Date;
                    int supplier_id = _selectedSupplierId;
                    string purchase_type = cmb_purchase_type.SelectedItem.ToString();
                    int employee_id = Convert.ToInt16(cmb_employees.SelectedValue);
                    int branch_id = UsersModal.logged_in_branch_id;

                    grid_purchase_invoice_report.AutoGenerateColumns = false;

                    _purchaseInvoiceReportDt = await Task.Run(() =>
                    {
                        PurchasesReportBLL purchaseReportBLL = new PurchasesReportBLL();
                        return purchaseReportBLL.PurchaseInvoiceReportNew(from_date, to_date, supplier_id, purchase_type, employee_id, branch_id);
                    });

                    double totalItems = 0;
                    double subtotalSum = 0;
                    double discountSum = 0;
                    double vatSum = 0;
                    double totalSum = 0;
                    double totalWithVatSum = 0;

                    foreach (DataRow dr in _purchaseInvoiceReportDt.Rows)
                    {
                        totalItems += (dr["total_items"] == DBNull.Value ? 0 : Convert.ToDouble(dr["total_items"]));
                        subtotalSum += (dr["subtotal"] == DBNull.Value ? 0 : Convert.ToDouble(dr["subtotal"]));
                        discountSum += (dr["discount_value"] == DBNull.Value ? 0 : Convert.ToDouble(dr["discount_value"]));
                        vatSum += (dr["vat"] == DBNull.Value ? 0 : Convert.ToDouble(dr["vat"]));
                        totalSum += (dr["total"] == DBNull.Value ? 0 : Convert.ToDouble(dr["total"]));
                        totalWithVatSum += (dr["total_with_vat"] == DBNull.Value ? 0 : Convert.ToDouble(dr["total_with_vat"]));
                    }

                    if (_purchaseInvoiceReportDt.Rows.Count > 0)
                    {
                        DataRow totalRow = _purchaseInvoiceReportDt.NewRow();
                        totalRow["invoice_no"] = "Total";
                        totalRow["total_items"] = totalItems;
                        totalRow["subtotal"] = subtotalSum;
                        totalRow["discount_value"] = discountSum;
                        totalRow["vat"] = vatSum;
                        totalRow["total"] = totalSum;
                        totalRow["total_with_vat"] = totalWithVatSum;
                        _purchaseInvoiceReportDt.Rows.InsertAt(totalRow, _purchaseInvoiceReportDt.Rows.Count);
                    }

                    _purchaseInvoiceReportView = new DataView(_purchaseInvoiceReportDt);
                    grid_purchase_invoice_report.DataSource = _purchaseInvoiceReportView;
                    ApplyGridFilter();

                    if (grid_purchase_invoice_report.Rows.Count > 0)
                        CustomizeDataGridView();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error");
                }
            }
        }

        private void CustomizeDataGridView()
        {
            DataGridViewRow lastRow = grid_purchase_invoice_report.Rows[grid_purchase_invoice_report.Rows.Count - 1];

            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);
                style.Font = new Font(grid_purchase_invoice_report.Font, FontStyle.Bold);
                style.BackColor = Color.LightGray;
                cell.Style = style;
            }
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            ApplyGridFilter();
        }

        private void ApplyGridFilter()
        {
            if (_purchaseInvoiceReportView == null)
                return;

            string keyword = (txt_search.Text ?? string.Empty).Trim().Replace("'", "''");

            if (string.IsNullOrWhiteSpace(keyword))
            {
                _purchaseInvoiceReportView.RowFilter = string.Empty;
            }
            else
            {
                _purchaseInvoiceReportView.RowFilter =
                    $"(invoice_no LIKE '%{keyword}%' OR supplier_name LIKE '%{keyword}%' OR supplier_invoice_no LIKE '%{keyword}%')";
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                if (_purchaseInvoiceReportDt == null || _purchaseInvoiceReportDt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to print. Please run a search first.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DGVPrinterHelper.DGVPrinter printer = new DGVPrinterHelper.DGVPrinter();
                printer.PageSettings.Landscape = true;
                printer.Title = "Purchase Invoice Report";
                printer.SubTitle = string.Format("From {0} To {1}", txt_from_date.Value.ToShortDateString(), txt_to_date.Value.ToShortDateString());
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.Footer = string.Empty;
                printer.FooterSpacing = 15;
                printer.ColumnWidth = DGVPrinterHelper.DGVPrinter.ColumnWidthSetting.CellWidth;
                printer.ColumnWidths.Clear();

                foreach (DataGridViewColumn col in grid_purchase_invoice_report.Columns)
                {
                    if (col.Visible)
                    {
                        float width = 0;
                        switch (col.Name)
                        {
                            case "col_id": width = 40; break;
                            case "col_purchase_date": width = 80; break;
                            case "col_invoice_no": width = 90; break;
                            case "col_supplier_invoice_no": width = 90; break;
                            case "col_supplier_name": width = 150; break;
                            case "col_purchase_type": width = 60; break;
                            case "col_total_items": width = 50; break;
                            case "col_subtotal": width = 70; break;
                            case "col_discount_value": width = 70; break;
                            case "col_vat": width = 60; break;
                            case "col_total": width = 70; break;
                            case "col_total_with_vat": width = 80; break;
                            default: width = 70; break;
                        }
                        printer.ColumnWidths.Add(col.Name, width);
                    }
                }

                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.CellAlignment = StringAlignment.Near;
                printer.PrintPreviewDataGridView(grid_purchase_invoice_report);
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
                if (_purchaseInvoiceReportDt == null || _purchaseInvoiceReportDt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to export. Please run a search first.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataTable exportDt = _purchaseInvoiceReportDt.Copy();
                string range = $"{txt_from_date.Value:yyyyMMdd}-{txt_to_date.Value:yyyyMMdd}";
                string defaultName = $"PurchaseInvoiceReport_{range}";
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

        private void SetupSuppliersDataGridView()
        {
            if (suppliersDataGridView != null) return;

            suppliersDataGridView = new DataGridView();
            suppliersDataGridView.ColumnCount = 4;

            suppliersDataGridView.Size = new System.Drawing.Size(420, 200);
            suppliersDataGridView.BorderStyle = BorderStyle.None;
            suppliersDataGridView.BackgroundColor = System.Drawing.Color.White;
            suppliersDataGridView.AutoGenerateColumns = false;
            suppliersDataGridView.ReadOnly = true;
            suppliersDataGridView.AllowUserToAddRows = false;
            suppliersDataGridView.AllowUserToDeleteRows = false;
            suppliersDataGridView.AllowUserToResizeRows = false;
            suppliersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            suppliersDataGridView.MultiSelect = false;
            suppliersDataGridView.RowHeadersVisible = false;
            suppliersDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            suppliersDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            suppliersDataGridView.Columns[0].Name = "Name";
            suppliersDataGridView.Columns[1].Name = "ID";
            suppliersDataGridView.Columns[2].Name = "Contact";
            suppliersDataGridView.Columns[3].Name = "VAT No";

            suppliersDataGridView.Columns[1].Visible = false;

            suppliersDataGridView.Columns[0].Width = 230;
            suppliersDataGridView.Columns[2].Width = 120;
            suppliersDataGridView.Columns[3].Width = 120;

            suppliersDataGridView.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.SystemColors.Window,
                ForeColor = System.Drawing.SystemColors.WindowText,
                Font = AppTheme.FontGrid,
                SelectionBackColor = System.Drawing.SystemColors.Highlight,
                SelectionForeColor = System.Drawing.SystemColors.HighlightText,
                Padding = new Padding(6, 2, 6, 2)
            };
            suppliersDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = System.Drawing.SystemColors.Control,
                ForeColor = System.Drawing.SystemColors.ControlText,
                Font = AppTheme.FontGridHeader,
                SelectionBackColor = System.Drawing.SystemColors.Control,
                SelectionForeColor = System.Drawing.SystemColors.ControlText
            };
            suppliersDataGridView.EnableHeadersVisualStyles = false;
            suppliersDataGridView.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            suppliersDataGridView.RowTemplate.Height = 28;
            suppliersDataGridView.ColumnHeadersHeight = 32;
            suppliersDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            suppliersDataGridView.CellClick += suppliersDataGridView_CellClick;
            suppliersDataGridView.KeyDown += suppliersDataGridView_KeyDown;

            suppliersDataGridView.Visible = false;
            this.Controls.Add(suppliersDataGridView);
            suppliersDataGridView.BringToFront();
        }

        private void PositionSuppliersDropdown()
        {
            System.Drawing.Point pt = this.PointToClient(
                txtSupplierSearch.Parent.PointToScreen(txtSupplierSearch.Location));
            int x = Math.Max(0, Math.Min(pt.X, this.ClientSize.Width - suppliersDataGridView.Width));
            suppliersDataGridView.Location = new System.Drawing.Point(x, pt.Y + txtSupplierSearch.Height + 2);
        }

        private void RefreshSuppliersData()
        {
            try
            {
                var supplierSearch = txtSupplierSearch.Text ?? string.Empty;
                var normalizedSearch = new SupplierBLL().NormalizeSupplierCodeInput(supplierSearch);

                if (_suppressSupplierSearch || !txtSupplierSearch.Focused)
                {
                    suppliersDataGridView.Visible = false;
                    return;
                }

                if (!string.IsNullOrWhiteSpace(normalizedSearch))
                {
                    DataTable dt = new SupplierBLL().SearchRecord(normalizedSearch) ?? new DataTable();
                    suppliersDataGridView.Rows.Clear();

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string[] row = {
                                dr["first_name"].ToString(),
                                dr["id"].ToString(),
                                dt.Columns.Contains("contact_no") ? dr["contact_no"].ToString() : "",
                                dt.Columns.Contains("vat_no") ? dr["vat_no"].ToString() : ""
                            };
                            suppliersDataGridView.Rows.Add(row);
                        }
                        PositionSuppliersDropdown();
                        suppliersDataGridView.Visible = true;
                        suppliersDataGridView.BringToFront();
                        suppliersDataGridView.ClearSelection();
                        suppliersDataGridView.CurrentCell = null;
                        if (suppliersDataGridView.Rows.Count > 0)
                            suppliersDataGridView.CurrentCell = suppliersDataGridView.Rows[0].Cells[0];
                    }
                    else
                    {
                        suppliersDataGridView.Visible = false;
                    }
                }
                else
                {
                    txtSupplierSearch.Text = "";
                    ClearSelectedSupplier();
                    suppliersDataGridView.Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearSelectedSupplier()
        {
            _selectedSupplierId = 0;
        }

        private void SupplierSearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            _supplierSearchDebounceTimer.Stop();
            if (_suppressSupplierSearch || !txtSupplierSearch.Focused) return;
            RefreshSuppliersData();
        }

        private void txtSupplierSearch_TextChanged(object sender, EventArgs e)
        {
            if (_suppressSupplierSearch || !txtSupplierSearch.Focused) return;

            if (string.IsNullOrWhiteSpace(txtSupplierSearch.Text))
                ClearSelectedSupplier();

            _supplierSearchDebounceTimer.Stop();
            _supplierSearchDebounceTimer.Start();
        }

        private void txtSupplierSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (suppliersDataGridView.Visible && suppliersDataGridView.Rows.Count > 0)
                {
                    suppliersDataGridView.Focus();
                    if (suppliersDataGridView.CurrentRow == null)
                        suppliersDataGridView.CurrentCell = suppliersDataGridView.Rows[0].Cells[0];
                }
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                suppliersDataGridView.Visible = false;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                if (suppliersDataGridView.Visible && suppliersDataGridView.Rows.Count > 0)
                {
                    if (suppliersDataGridView.CurrentRow == null)
                        suppliersDataGridView.CurrentCell = suppliersDataGridView.Rows[0].Cells[0];

                    _selectedSupplierId = Convert.ToInt32(suppliersDataGridView.CurrentRow.Cells[1].Value);
                    _suppressSupplierSearch = true;
                    txtSupplierSearch.Text = suppliersDataGridView.CurrentRow.Cells[0].Value.ToString();
                    _suppressSupplierSearch = false;
                    suppliersDataGridView.Visible = false;
                    btn_search.Focus();
                }
            }

            _supplierSearchDebounceTimer.Stop();
            _supplierSearchDebounceTimer.Start();
        }

        private void txtSupplierSearch_Leave(object sender, EventArgs e)
        {
            _supplierSearchDebounceTimer.Stop();
            if (!suppliersDataGridView.Focused)
                suppliersDataGridView.Visible = false;
        }

        private void suppliersDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                _selectedSupplierId = Convert.ToInt32(suppliersDataGridView.CurrentRow.Cells[1].Value);
                _suppressSupplierSearch = true;
                txtSupplierSearch.Text = suppliersDataGridView.CurrentRow.Cells[0].Value.ToString();
                _suppressSupplierSearch = false;
                suppliersDataGridView.Visible = false;
                btn_search.Focus();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                suppliersDataGridView.Visible = false;
                txtSupplierSearch.Focus();
            }
        }

        private void suppliersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            _selectedSupplierId = Convert.ToInt32(suppliersDataGridView.CurrentRow.Cells[1].Value);
            _suppressSupplierSearch = true;
            txtSupplierSearch.Text = suppliersDataGridView.CurrentRow.Cells[0].Value.ToString();
            _suppressSupplierSearch = false;
            suppliersDataGridView.Visible = false;
            btn_search.Focus();
        }

        private void grid_purchase_invoice_report_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= grid_purchase_invoice_report.Rows.Count)
                return;

            var row = grid_purchase_invoice_report.Rows[e.RowIndex];
            if (row.Cells["col_id"].Value == null || row.Cells["col_id"].Value == DBNull.Value)
                return;

            int purchaseId = Convert.ToInt32(row.Cells["col_id"].Value);
            string invoiceNo = row.Cells["col_invoice_no"].Value?.ToString() ?? "";

            frm_PurchaseProductDetail detailForm = new frm_PurchaseProductDetail(purchaseId, invoiceNo);
            detailForm.ShowDialog();
        }

        private void frm_PurchaseInvoiceReport_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.F3)
                btn_search.PerformClick();

            if (e.KeyCode == Keys.F4 || (e.Control && e.KeyCode == Keys.P))
                btn_print.PerformClick();
        }
    }
}
