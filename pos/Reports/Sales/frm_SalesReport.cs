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
using pos.Security.Authorization;
using pos.UI;

namespace pos
{
    public partial class frm_SalesReport : Form
    {
        public string _product_code = "";
        public string _product_name;
        DataTable sales_report_dt = new DataTable();

        private DataGridView customersDataGridView;
        private System.Windows.Forms.Timer _customerSearchDebounceTimer;
        private bool _suppressCustomerSearch;
        private int _selectedCustomerId;
        
        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        ProductBLL productsBLL_obj = new ProductBLL();
                
        public frm_SalesReport()
        {
            InitializeComponent();
            //get_products_dropdownlist();
            autoCompleteProductCode();

            _customerSearchDebounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _customerSearchDebounceTimer.Tick += CustomerSearchDebounceTimer_Tick;

            if (txtCustomerSearch != null)
            {
                txtCustomerSearch.TextChanged += txtCustomerSearch_TextChanged;
                txtCustomerSearch.KeyUp       += txtCustomerSearch_KeyUp;
                txtCustomerSearch.Leave       += txtCustomerSearch_Leave;
            }
        }

        private void SalesReport_Load(object sender, EventArgs e)
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
            //if (txtCustomerSearch != null)
            //{
            //    txtCustomerSearch.Width = 170;
            //    txtCustomerSearch.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            //    txtCustomerSearch.MaximumSize = new Size(170, 0);
            //}

            ApplyProfitColumnVisibility();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_sales_report, id);
        }

        public void load_products()
        {
            //txt_product_code.Text = _product_name;
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
        

        public void get_products_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "id,name";
            string table = "pos_products";

            DataTable products = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = products.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[1] = "Select Product";              // Set Column Value
            products.Rows.InsertAt(emptyRow, 0);
            
            //cmb_products.DataSource = products;

            //cmb_products.DisplayMember = "name";
            //cmb_products.ValueMember = "id";
        }

        private async void btn_search_Click(object sender, EventArgs e)
        {
            using (pos.UI.Busy.BusyScope.Show(this, "Loading sales report..."))
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

                    grid_sales_report.AutoGenerateColumns = false;

                    // Run DB/report work off the UI thread
                    sales_report_dt = await Task.Run(() =>
                    {
                        SalesReportBLL sale_report_obj = new SalesReportBLL();
                        return sale_report_obj.SaleReport(from_date, to_date, customer_id, product_code, sale_type, employee_id, sale_account, branch_id);
                    });

                    // Ensure profit column exists (you said you created it; this is a safety net)
                    if (!sales_report_dt.Columns.Contains("profit"))
                        sales_report_dt.Columns.Add("profit", typeof(double));

                    // also add cost_total if not present (DAL now provides it, but keep safe)
                    if (!sales_report_dt.Columns.Contains("cost_total"))
                        sales_report_dt.Columns.Add("cost_total", typeof(double));

                    if (!sales_report_dt.Columns.Contains("profit_percent"))
                        sales_report_dt.Columns.Add("profit_percent", typeof(double));

                    bool showProfit = CanViewProfit();

                    double _quantity_sold_total = 0;
                    double _unit_price_total = 0;
                    double _discount_value_total = 0;
                    double _vat_total = 0;
                    double _total = 0;
                    double _cost_price_total = 0;
                    double _total_with_vat = 0;
                    double _profit_total = 0;
                    double _net_revenue_total = 0; // <-- Added revenue accumulator for profit percent totals

                    foreach (DataRow dr in sales_report_dt.Rows)
                    {
                        double qty = (dr["quantity_sold"].ToString() == "" ? 0 : Convert.ToDouble(dr["quantity_sold"]));
                        double total = (dr["total"].ToString() == "" ? 0 : Convert.ToDouble(dr["total"]));

                        _quantity_sold_total += qty;
                        _unit_price_total += (dr["unit_price"].ToString() == "" ? 0 : Convert.ToDouble(dr["unit_price"]));
                        _cost_price_total += (dr["cost_price"].ToString() == "" ? 0 : Convert.ToDouble(dr["cost_price"]));
                        _discount_value_total += (dr["discount_value"].ToString() == "" ? 0 : Convert.ToDouble(dr["discount_value"]));
                        _vat_total += (dr["vat"].ToString() == "" ? 0 : Convert.ToDouble(dr["vat"]));
                        _total_with_vat += (dr["total_with_vat"].ToString() == "" ? 0 : Convert.ToDouble(dr["total_with_vat"]));
                        _total += total;

                        if (showProfit)
                        {
                            double avgCost = (sales_report_dt.Columns.Contains("cost_price") && dr["cost_price"].ToString() != "")
                                ? Convert.ToDouble(dr["cost_price"])
                                : 0;

                            double unitPrice = (dr["unit_price"].ToString() == "" ? 0 : Convert.ToDouble(dr["unit_price"]));
                            double discountValueRaw = (dr["discount_value"].ToString() == "" ? 0 : Convert.ToDouble(dr["discount_value"]));

                            // Normalize discount sign to follow quantity sign
                            double discountValue = (qty < 0) ? -Math.Abs(discountValueRaw) : Math.Abs(discountValueRaw);

                            double lineRevenue = (unitPrice * qty) - discountValue;

                            // Cost follows qty sign
                            double costTotal = avgCost * qty;
                            dr["cost_total"] = costTotal;
                            
                            double profit = lineRevenue - costTotal;

                            // Enforce profit sign to follow qty (returns must be negative impact)
                            if (qty < 0) profit = -Math.Abs(profit);
                            else profit = Math.Abs(profit);

                            dr["profit"] = profit;

                            var pctBase = lineRevenue;
                            var pct = (Math.Abs(pctBase) < 0.0000001) ? 0 : (profit / pctBase) * 100.0;
                            dr["profit_percent"] = pct;

                            _profit_total += profit;
                            _net_revenue_total += lineRevenue;
                        }
                        else
                        {
                            // avoid leaking cost/profit in exported DataTable
                            dr["profit"] = 0;
                            dr["cost_total"] = 0;
                            dr["profit_percent"] = 0;
                        }
                    }

                    if (sales_report_dt.Rows.Count > 0)
                    {
                        DataRow newRow = sales_report_dt.NewRow();
                        newRow[8] = "Total";
                        newRow[9] = _quantity_sold_total;
                        newRow[10] = _unit_price_total;
                        newRow[11] = _discount_value_total;
                        newRow[13] = _vat_total;
                        newRow[14] = _total_with_vat;
                        newRow[15] = _total;
                        newRow[16] = _cost_price_total;

                        if (sales_report_dt.Columns.Contains("profit"))
                            newRow["profit"] = _profit_total;
                        if (sales_report_dt.Columns.Contains("profit_percent"))
                        {
                            var pctTotal = (Math.Abs(_net_revenue_total) < 0.0000001) ? 0 : (_profit_total / _net_revenue_total) * 100.0;
                            newRow["profit_percent"] = pctTotal;
                        }

                        sales_report_dt.Rows.InsertAt(newRow, sales_report_dt.Rows.Count);
                    }

                    grid_sales_report.DataSource = sales_report_dt;
                    if (grid_sales_report.Rows.Count > 0)
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
            // Get the last row in the DataGridView
            DataGridViewRow lastRow = grid_sales_report.Rows[grid_sales_report.Rows.Count - 1];

            // Loop through all cells in the row
            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);

                // Set the font to bold
                style.Font = new Font(grid_sales_report.Font, FontStyle.Bold);

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
                        _product_code = myProductView["code"].ToString();
                       
                    }
                }
            }
             else
             {
                 _product_code = "";
                 txt_product_name.Text = "";
             }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts obj = new frm_searchProducts();
            obj.ShowDialog();
        }

      
        private void frm_SalesReport_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (txtCustomerSearch != null && txtCustomerSearch.Focused && e.KeyCode == Keys.Enter)
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
                
                if (e.Control && e.KeyCode == Keys.P)
                {
                    btn_print.PerformClick();
                }

                //Export to Excel button
                if (e.Control && e.KeyCode == Keys.E)
                {
                    btn_export.PerformClick();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                if (sales_report_dt == null || sales_report_dt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to print. Please run a search first.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string date_range = txt_from_date.Value.ToString("dd-MM-yyyy") + " To " + txt_to_date.Value.ToString("dd-MM-yyyy");
                string sale_type = cmb_sale_type.Text;
                string employee = cmb_employees.Text;
                string sale_account = cmb_sale_account.Text;

                frm_sales_report obj = new frm_sales_report(sales_report_dt, date_range, sale_type, employee, sale_account, false);
                obj.Show();
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
                if (sales_report_dt == null || sales_report_dt.Rows.Count <= 1)
                {
                    MessageBox.Show("No data to export. Please run a search first.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Build filename with date range
                string range = $"{txt_from_date.Value:yyyyMMdd}-{txt_to_date.Value:yyyyMMdd}";
                string defaultName = $"SalesReport_{range}";

                // Drop the appended "Total" row to avoid duplication (Excel can sum itself)
                ExcelExportHelper.ExportDataTableToExcel(sales_report_dt, defaultName, this, includeLastRow: true);
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
            // Owner/Admin short-circuit is already handled by Auth.
            return _auth.HasPermission(_currentUser, Permissions.Reports_ProfitLossView);

            //var user = AppSecurityContext.User;
            //return user != null && AppSecurityContext.Auth.HasPermission(user, "Sales.Profit.View");
        }

        private void ApplyProfitColumnVisibility()
        {
            // If you already created the profit column in designer, set its Name to "profit".
            // Optionally hide cost_price/cost_total as well.
            bool show = CanViewProfit();

            if (grid_sales_report.Columns.Contains("profit"))
                grid_sales_report.Columns["profit"].Visible = show;

            if (grid_sales_report.Columns.Contains("profit_percent"))
                grid_sales_report.Columns["profit_percent"].Visible = show;

            //if (grid_sales_report.Columns.Contains("cost_price"))
            //    grid_sales_report.Columns["cost_price"].Visible = show;

            //if (grid_sales_report.Columns.Contains("cost_total"))
            //    grid_sales_report.Columns["cost_total"].Visible = show;
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void SetupCustomersDataGridView()
        {
            if (customersDataGridView != null) return;

            customersDataGridView = new DataGridView();
            customersDataGridView.ColumnCount = 6;
            bool isRtl = RightToLeft == RightToLeft.Yes;

            customersDataGridView.Size               = new Size(520, 240);
            customersDataGridView.BorderStyle        = BorderStyle.None;
            customersDataGridView.BackgroundColor    = Color.White;
            customersDataGridView.AutoGenerateColumns = false;
            customersDataGridView.ReadOnly           = true;
            customersDataGridView.AllowUserToAddRows    = false;
            customersDataGridView.AllowUserToDeleteRows = false;
            customersDataGridView.AllowUserToResizeRows = false;
            customersDataGridView.SelectionMode      = DataGridViewSelectionMode.FullRowSelect;
            customersDataGridView.MultiSelect        = false;
            customersDataGridView.RowHeadersVisible  = false;
            customersDataGridView.RightToLeft        = isRtl ? RightToLeft.Yes : RightToLeft.No;
            customersDataGridView.AutoSizeRowsMode   = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
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
                BackColor          = SystemColors.Window,
                ForeColor          = SystemColors.WindowText,
                Font               = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                Padding            = new Padding(6, 2, 6, 2)
            };
            customersDataGridView.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Control,
                ForeColor          = SystemColors.ControlText,
                Font               = AppTheme.FontGridHeader,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText
            };
            customersDataGridView.EnableHeadersVisualStyles  = false;
            customersDataGridView.CellBorderStyle            = DataGridViewCellBorderStyle.SingleHorizontal;
            customersDataGridView.RowTemplate.Height         = 28;
            customersDataGridView.ColumnHeadersHeight        = 32;
            customersDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            customersDataGridView.CellClick += customersDataGridView_CellClick;
            customersDataGridView.KeyDown   += customersDataGridView_KeyDown;

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
                var customerSearch   = txtCustomerSearch.Text ?? string.Empty;
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
    }
}
