using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using pos.Security.Authorization;
using pos.UI;
using pos.Reports.Common;

namespace pos.Reports.Sales
{
    public partial class frm_SaleProductDetail : Form
    {
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        private int _saleId;
        private string _invoiceNo;

        public frm_SaleProductDetail(int saleId, string invoiceNo)
        {
            InitializeComponent();
            _saleId = saleId;
            _invoiceNo = invoiceNo;
        }

        private void frm_SaleProductDetail_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();

            lbl_invoice_no.Text = _invoiceNo;

            LoadProductDetails();
            ApplyProfitColumnVisibility();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_products);
        }

        private void LoadProductDetails()
        {
            try
            {
                SalesReportBLL salesReportBLL = new SalesReportBLL();

                // Query product details for this sale
                DateTime minDate = new DateTime(2000, 1, 1);
                DateTime maxDate = DateTime.Now.AddYears(10);

                DataTable productsDt = salesReportBLL.SaleReport(minDate, maxDate, 0, "", "All", 0, "All", UsersModal.logged_in_branch_id, true);

                // Filter to only this sale
                DataView dv = new DataView(productsDt);
                dv.RowFilter = $"id = {_saleId}";

                DataTable filteredDt = dv.ToTable();

                bool showProfit = CanViewProfit();

                // Calculate profit if allowed
                if (showProfit && filteredDt.Rows.Count > 0)
                {
                    if (!filteredDt.Columns.Contains("profit"))
                        filteredDt.Columns.Add("profit", typeof(double));

                    foreach (DataRow dr in filteredDt.Rows)
                    {
                        double qty = (dr["quantity_sold"] == DBNull.Value ? 0 : Convert.ToDouble(dr["quantity_sold"]));
                        double unitPrice = (dr["unit_price"] == DBNull.Value ? 0 : Convert.ToDouble(dr["unit_price"]));
                        double discountValue = (dr["discount_value"] == DBNull.Value ? 0 : Convert.ToDouble(dr["discount_value"]));
                        double costPrice = (dr["cost_price"] == DBNull.Value ? 0 : Convert.ToDouble(dr["cost_price"]));

                        double lineRevenue = (unitPrice * qty) - discountValue;
                        double costTotal = costPrice * qty;
                        double profit = lineRevenue - costTotal;

                        dr["profit"] = profit;
                    }
                }
                else if (filteredDt.Rows.Count > 0)
                {
                    if (!filteredDt.Columns.Contains("profit"))
                        filteredDt.Columns.Add("profit", typeof(double));

                    foreach (DataRow dr in filteredDt.Rows)
                    {
                        dr["profit"] = 0;
                    }
                }

                // Calculate totals
                if (filteredDt.Rows.Count > 0)
                {
                    double totalQty = 0;
                    double totalPrice = 0;
                    double totalDiscount = 0;
                    double totalVat = 0;
                    double totalAmount = 0;
                    double totalCost = 0;
                    double totalProfit = 0;

                    foreach (DataRow dr in filteredDt.Rows)
                    {
                        totalQty += (dr["quantity_sold"] == DBNull.Value ? 0 : Convert.ToDouble(dr["quantity_sold"]));
                        totalPrice += (dr["unit_price"] == DBNull.Value ? 0 : Convert.ToDouble(dr["unit_price"]));
                        totalDiscount += (dr["discount_value"] == DBNull.Value ? 0 : Convert.ToDouble(dr["discount_value"]));
                        totalVat += (dr["vat"] == DBNull.Value ? 0 : Convert.ToDouble(dr["vat"]));
                        totalAmount += (dr["total"] == DBNull.Value ? 0 : Convert.ToDouble(dr["total"]));
                        if (showProfit)
                        {
                            totalCost += (dr["cost_price"] == DBNull.Value ? 0 : Convert.ToDouble(dr["cost_price"]));
                            totalProfit += (dr["profit"] == DBNull.Value ? 0 : Convert.ToDouble(dr["profit"]));
                        }
                    }

                    DataRow totalRow = filteredDt.NewRow();
                    totalRow["product_name"] = "Total";
                    totalRow["quantity_sold"] = totalQty;
                    totalRow["unit_price"] = totalPrice;
                    totalRow["discount_value"] = totalDiscount;
                    totalRow["vat"] = totalVat;
                    totalRow["total"] = totalAmount;
                    if (showProfit)
                    {
                        totalRow["cost_price"] = totalCost;
                        totalRow["profit"] = totalProfit;
                    }

                    filteredDt.Rows.Add(totalRow);
                }

                grid_products.AutoGenerateColumns = false;
                grid_products.DataSource = filteredDt;

                if (grid_products.Rows.Count > 0)
                    CustomizeDataGridView();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CustomizeDataGridView()
        {
            DataGridViewRow lastRow = grid_products.Rows[grid_products.Rows.Count - 1];

            foreach (DataGridViewCell cell in lastRow.Cells)
            {
                DataGridViewCellStyle style = new DataGridViewCellStyle(cell.Style);
                style.Font = new Font(grid_products.Font, FontStyle.Bold);
                style.BackColor = Color.LightGray;
                cell.Style = style;
            }
        }

        private bool CanViewProfit()
        {
            return _auth.HasPermission(_currentUser, Permissions.Reports_ProfitLossView);
        }

        private void ApplyProfitColumnVisibility()
        {
            bool show = CanViewProfit();

            if (grid_products.Columns.Contains("profit"))
                grid_products.Columns["profit"].Visible = show;

            if (grid_products.Columns.Contains("cost_price"))
                grid_products.Columns["cost_price"].Visible = show;
        }

        private void frm_SaleProductDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
            if (e.KeyCode == Keys.F4 || (e.Control && e.KeyCode == Keys.P))
            {
                btn_print.PerformClick();
            }
        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_products.Rows.Count == 0)
                {
                    MessageBox.Show("No data to print.", "Print", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DGVPrinterHelper.DGVPrinter printer = new DGVPrinterHelper.DGVPrinter();

                // Set page orientation to landscape for better fit
                printer.PageSettings.Landscape = true;

                // Configure title and subtitle
                printer.Title = "Sale Product Details";
                printer.SubTitle = "Invoice No: " + _invoiceNo;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.Footer = string.Empty;
                printer.FooterSpacing = 15;

                // Use CellWidth mode to respect actual column widths, not proportional
                printer.ColumnWidth = DGVPrinterHelper.DGVPrinter.ColumnWidthSetting.CellWidth;

                // Set explicit column widths to fit content better
                printer.ColumnWidths.Clear();

                foreach (DataGridViewColumn col in grid_products.Columns)
                {
                    if (col.Visible)
                    {
                        float width = 0;

                        // Set optimized widths for each column
                        switch (col.Name)
                        {
                            case "item_code":
                                width = 60;
                                break;
                            case "product_name":
                                width = 150;
                                break;
                            case "quantity_sold":
                                width = 50;
                                break;
                            case "unit_price":
                                width = 60;
                                break;
                            case "discount_value":
                                width = 60;
                                break;
                            case "vat":
                                width = 50;
                                break;
                            case "total":
                                width = 70;
                                break;
                            case "cost_price":
                                width = 60;
                                break;
                            case "profit":
                                width = 60;
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

                // Print preview with landscape orientation
                printer.PrintPreviewDataGridView(grid_products);
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
                if (grid_products.DataSource == null)
                {
                    MessageBox.Show("No data to export.", "Export", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                DataTable exportDt = ((DataTable)grid_products.DataSource).Copy();

                // Remove hidden columns if user doesn't have permission
                bool showProfit = CanViewProfit();
                if (!showProfit)
                {
                    if (exportDt.Columns.Contains("profit"))
                        exportDt.Columns.Remove("profit");
                    if (exportDt.Columns.Contains("cost_price"))
                        exportDt.Columns.Remove("cost_price");
                }

                string defaultName = $"SaleDetail_{_invoiceNo}";
                ExcelExportHelper.ExportDataTableToExcel(exportDt, defaultName, this, includeLastRow: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
