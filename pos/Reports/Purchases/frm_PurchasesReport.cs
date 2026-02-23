using pos.Reports.Purchases.Report_Viewer;
using pos.Reports.Common; // added for Excel export
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.UI;

namespace pos
{
    public partial class frm_PurchasesReport : Form
    {
        ProductBLL productsBLL_obj = new ProductBLL();
        public int _product_id = 0;
        public string _product_name;
        DataTable purchase_report_dt = new DataTable();

        private DataGridView suppliersDataGridView;
        private System.Windows.Forms.Timer _supplierSearchDebounceTimer;
        private bool _suppressSupplierSearch;
        private int _selectedSupplierId;

        public frm_PurchasesReport()
        {
            InitializeComponent();

            SetupSuppliersDataGridView();
            _supplierSearchDebounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _supplierSearchDebounceTimer.Tick += SupplierSearchDebounceTimer_Tick;

            if (txtSupplierSearch != null)
            {
                txtSupplierSearch.TextChanged += txtSupplierSearch_TextChanged;
                txtSupplierSearch.KeyUp += txtSupplierSearch_KeyUp;
                txtSupplierSearch.Leave += txtSupplierSearch_Leave;
            }
        }

        private void PurchasesReport_Load(object sender, EventArgs e)
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
            //get_products_dropdownlist();
            cmb_purchase_type.SelectedIndex = 0;
            autoCompleteProductCode();
            get_employees_dropdownlist();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_Purchases_report, id);
        }

        

        public void autoCompleteProductCode()
        {
            try
            {
                txt_product_code.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt_product_code.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection Products_coll = new AutoCompleteStringCollection();

                DataTable dt = productsBLL_obj.GetAllProductCodes();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        Products_coll.Add(dr["code"].ToString());

                    }

                }

                txt_product_code.AutoCompleteCustomSource = Products_coll;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        
        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                
                DateTime from_date = txt_from_date.Value.Date;
                DateTime to_date = txt_to_date.Value.Date;
                int supplier_id = _selectedSupplierId;
                int product_id = _product_id; //Convert.ToInt16(cmb_products.SelectedValue);
                int employee_id = Convert.ToInt16(cmb_employees.SelectedValue);
                string purchase_type = cmb_purchase_type.SelectedItem.ToString();
                int branch_id = UsersModal.logged_in_branch_id;

                PurchasesReportBLL purchase_report_obj = new PurchasesReportBLL ();
                grid_Purchases_report.AutoGenerateColumns = false;

                double _quantity_total = 0;
                double _cost_price_total = 0;
                double _discount_value_total = 0;
                double _vat_total = 0;
                double _total = 0;
                double _total_with_vat = 0;

                purchase_report_dt = purchase_report_obj.PurchaseReport(from_date, to_date, supplier_id, product_id, purchase_type, employee_id, branch_id);

                foreach (DataRow dr in purchase_report_dt.Rows)
                {
                    _quantity_total += (string.IsNullOrWhiteSpace(dr["quantity"].ToString()) ? 0 : Convert.ToDouble(dr["quantity"].ToString()));
                    _cost_price_total += (string.IsNullOrWhiteSpace(dr["cost_price"].ToString()) ? 0 : Convert.ToDouble(dr["cost_price"].ToString()));
                    _discount_value_total += (string.IsNullOrWhiteSpace(dr["discount_value"].ToString()) ? 0 : Convert.ToDouble(dr["discount_value"].ToString()));
                    _vat_total += (string.IsNullOrWhiteSpace(dr["vat"].ToString()) ? 0 : Convert.ToDouble(dr["vat"].ToString()));
                    _total_with_vat += (dr["total_with_vat"].ToString() == "" ? 0 : Convert.ToDouble(dr["total_with_vat"].ToString()));
                    _total += (dr["total"].ToString() == "" ? 0 : Convert.ToDouble(dr["total"].ToString())); ;
                }

                DataRow newRow = purchase_report_dt.NewRow();
                newRow[6] = "Total";
                newRow[8] = _quantity_total;
                newRow[9] = _cost_price_total;
                newRow[10] = _discount_value_total;
                newRow[12] = _vat_total;
                newRow[13] = _total_with_vat;
                newRow[14] = _total;
                purchase_report_dt.Rows.InsertAt(newRow, purchase_report_dt.Rows.Count);

                grid_Purchases_report.DataSource = purchase_report_dt;
                CustomizeDataGridView();
                
                _product_id = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
                
            }
        }
        private void CustomizeDataGridView()
        {
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_Purchases_report.Rows[grid_Purchases_report.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_Purchases_report.Font, FontStyle.Bold);

                // Set the background color
                style.BackColor = Color.LightGray;

                // Apply the style to the current cell
                cell.Style = style;
            }

        }


        private void txt_product_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_product_code.Text != string.Empty && e.KeyCode == Keys.Enter)
            {
                DataTable product_dt = new DataTable();
                product_dt = productsBLL_obj.SearchRecordByProductCode(txt_product_code.Text);

                if (product_dt.Rows.Count > 0)
                {
                    foreach (DataRow myProductView in product_dt.Rows)
                    {
                        txt_product_name.Text = myProductView["name"].ToString();
                        _product_id = (int)myProductView["id"];

                    }
                }
            }
            else
            {
                _product_id = 0;
                txt_product_name.Text = "";
            }
        }
        public void get_employees_dropdownlist()
        {
            EmployeeBLL employeeBLL = new EmployeeBLL();
            DataTable employees = employeeBLL.GetAll();
            DataRow emptyRow = employees.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "All Employee";              // Set Column Value
            employees.Rows.InsertAt(emptyRow, 0);
            cmb_employees.DisplayMember = "first_name";
            cmb_employees.ValueMember = "id";
            cmb_employees.DataSource = employees;


        }

        private void frm_PurchasesReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (txtSupplierSearch != null && txtSupplierSearch.Focused && e.KeyCode == Keys.Enter)
                    return;

                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

                if (e.KeyCode == Keys.F3)
                {
                    btn_search.PerformClick();
                }

                //Print button
                if (e.Control && e.KeyCode == Keys.P)
                {
                    Btn_print.PerformClick();
                }

                //Export to Excel button
                if (e.Control && e.KeyCode == Keys.E)
                {
                    Btn_export.PerformClick();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void Btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                if (purchase_report_dt == null || purchase_report_dt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to print. Please run a search first.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string date_range = txt_from_date.Value.ToString("dd-MM-yyyy") + " To " + txt_to_date.Value.ToString("dd-MM-yyyy");
                string purchase_type = cmb_purchase_type.Text;
                string employee = cmb_employees.Text;
                frm_purchase_report_viewer frm_Purchase_Report_Viewer = new frm_purchase_report_viewer(purchase_report_dt, date_range, purchase_type, employee, false);
                frm_Purchase_Report_Viewer.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Print Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_export_Click(object sender, EventArgs e)
        {
            try
            {
                if (purchase_report_dt == null || purchase_report_dt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to export. Please run a search first.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Build filename with date range
                string range = $"{txt_from_date.Value:yyyyMMdd}-{txt_to_date.Value:yyyyMMdd}";
                string defaultName = $"PurchasesReport_{range}";

                // Drop the appended "Total" row to avoid duplication (Excel can sum itself)
                ExcelExportHelper.ExportDataTableToExcel(purchase_report_dt, defaultName, this, includeLastRow: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SetupSuppliersDataGridView()
        {
            if (suppliersDataGridView != null) return;

            suppliersDataGridView = new DataGridView();
            suppliersDataGridView.ColumnCount = 6;

            suppliersDataGridView.Size = new Size(520, 240);
            suppliersDataGridView.BorderStyle = BorderStyle.None;
            suppliersDataGridView.BackgroundColor = Color.White;
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

            suppliersDataGridView.Columns[0].Name = "Code";
            suppliersDataGridView.Columns[1].Name = "Name";
            suppliersDataGridView.Columns[2].Name = "ID";
            suppliersDataGridView.Columns[3].Name = "Contact";
            suppliersDataGridView.Columns[4].Name = "VAT No";
            suppliersDataGridView.Columns[5].Name = "VatStatus";

            suppliersDataGridView.Columns[2].Visible = false;
            suppliersDataGridView.Columns[5].Visible = false;

            suppliersDataGridView.Columns[0].Width = 90;
            suppliersDataGridView.Columns[1].Width = 220;
            suppliersDataGridView.Columns[3].Width = 130;
            suppliersDataGridView.Columns[4].Width = 120;

            suppliersDataGridView.CellClick += suppliersDataGridView_CellClick;
            suppliersDataGridView.KeyDown += suppliersDataGridView_KeyDown;

            suppliersDataGridView.Visible = false;
            this.Controls.Add(suppliersDataGridView);
            suppliersDataGridView.BringToFront();
        }

        private void PositionSuppliersDropdown()
        {
            Point pt = this.PointToClient(
                txtSupplierSearch.Parent.PointToScreen(txtSupplierSearch.Location));
            int x = Math.Max(0, Math.Min(pt.X, this.ClientSize.Width - suppliersDataGridView.Width));
            suppliersDataGridView.Location = new Point(x, pt.Y + txtSupplierSearch.Height + 2);
        }

        private void RefreshSuppliersData()
        {
            try
            {
                var supplierSearch = txtSupplierSearch.Text ?? string.Empty;
                var bll = new SupplierBLL();
                var normalizedSearch = bll.NormalizeSupplierCodeInput(supplierSearch);

                if (_suppressSupplierSearch || !txtSupplierSearch.Focused)
                {
                    suppliersDataGridView.Visible = false;
                    return;
                }

                if (!string.IsNullOrWhiteSpace(normalizedSearch))
                {
                    DataTable dt = bll.SearchRecord(normalizedSearch) ?? new DataTable();
                    suppliersDataGridView.Rows.Clear();

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string[] row0 = {
                                dt.Columns.Contains("supplier_code") ? Convert.ToString(dr["supplier_code"]) : "",
                                (Convert.ToString(dr["first_name"]) + " " + Convert.ToString(dr["last_name"])).Trim(),
                                Convert.ToString(dr["id"]),
                                dt.Columns.Contains("contact_no") ? Convert.ToString(dr["contact_no"]) : "",
                                Convert.ToString(dr["vat_no"]),
                                dt.Columns.Contains("vat_status") ? Convert.ToString(dr["vat_status"]) : ""
                            };
                            suppliersDataGridView.Rows.Add(row0);
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

        private void SelectSupplierFromDataRow(DataRow dr)
        {
            _selectedSupplierId = Convert.ToInt32(dr["id"]);
            _suppressSupplierSearch = true;
            txtSupplierSearch.Text = (Convert.ToString(dr["first_name"]) + " " + Convert.ToString(dr["last_name"])).Trim();
            _suppressSupplierSearch = false;
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
            {
                ClearSelectedSupplier();
            }

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
                var bll = new SupplierBLL();
                var normalizedSearch = bll.NormalizeSupplierCodeInput(txtSupplierSearch.Text);

                if (!string.IsNullOrWhiteSpace(normalizedSearch) && normalizedSearch.StartsWith("S-", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var dt = bll.SearchRecord(normalizedSearch);
                        if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("supplier_code"))
                        {
                            var exact = dt.Select("supplier_code = '" + normalizedSearch.Replace("'", "''") + "'");
                            if (exact.Length == 1)
                            {
                                SelectSupplierFromDataRow(exact[0]);
                                suppliersDataGridView.Visible = false;
                                btn_search.Focus();
                                return;
                            }
                        }
                        ClearSelectedSupplier();
                    }
                    catch { }
                }

                if (suppliersDataGridView.Visible && suppliersDataGridView.Rows.Count > 0)
                {
                    var idText = Convert.ToString(suppliersDataGridView.CurrentRow.Cells[2].Value);
                    int supId;
                    if (int.TryParse(idText, out supId) && supId > 0)
                    {
                        var dt = bll.SearchRecordBySupplierID(supId);
                        if (dt != null && dt.Rows.Count > 0)
                            SelectSupplierFromDataRow(dt.Rows[0]);
                        else
                            ClearSelectedSupplier();
                    }
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
                var idText = Convert.ToString(suppliersDataGridView.CurrentRow.Cells[2].Value);
                int supId;
                if (int.TryParse(idText, out supId) && supId > 0)
                {
                    var bll = new SupplierBLL();
                    var dt = bll.SearchRecordBySupplierID(supId);
                    if (dt != null && dt.Rows.Count > 0)
                        SelectSupplierFromDataRow(dt.Rows[0]);
                }
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
            var idText = Convert.ToString(suppliersDataGridView.CurrentRow.Cells[2].Value);
            int supId;
            if (int.TryParse(idText, out supId) && supId > 0)
            {
                var bll = new SupplierBLL();
                var dt = bll.SearchRecordBySupplierID(supId);
                if (dt != null && dt.Rows.Count > 0)
                    SelectSupplierFromDataRow(dt.Rows[0]);
            }
            suppliersDataGridView.Visible = false;
            btn_search.Focus();
        }
    }
}
