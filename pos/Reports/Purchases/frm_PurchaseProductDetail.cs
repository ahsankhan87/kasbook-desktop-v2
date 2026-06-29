using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using pos.UI;
using pos.Reports.Common;

namespace pos.Reports.Purchases
{
    public partial class frm_PurchaseProductDetail : Form
    {
        private int _purchaseId;
        private string _invoiceNo;

        public frm_PurchaseProductDetail(int purchaseId, string invoiceNo)
        {
            InitializeComponent();
            _purchaseId = purchaseId;
            _invoiceNo = invoiceNo;
        }

        private void frm_PurchaseProductDetail_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();

            lbl_invoice_no.Text = _invoiceNo;

            LoadProductDetails();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(panel1, null, panel2, grid_products);
        }

        private void LoadProductDetails()
        {
            try
            {
                PurchasesReportBLL purchasesReportBLL = new PurchasesReportBLL();
                DataTable filteredDt = purchasesReportBLL.GetPurchaseLineItems(_purchaseId, UsersModal.logged_in_branch_id);

                if (filteredDt.Rows.Count > 0)
                {
                    double totalQty = 0;
                    double totalCostPrice = 0;
                    double totalDiscount = 0;
                    double totalVat = 0;
                    double totalAmount = 0;
                    double totalWithVat = 0;

                    foreach (DataRow dr in filteredDt.Rows)
                    {
                        totalQty += (dr["quantity"] == DBNull.Value ? 0 : Convert.ToDouble(dr["quantity"]));
                        totalCostPrice += (dr["cost_price"] == DBNull.Value ? 0 : Convert.ToDouble(dr["cost_price"]));
                        totalDiscount += (dr["discount_value"] == DBNull.Value ? 0 : Convert.ToDouble(dr["discount_value"]));
                        totalVat += (dr["vat"] == DBNull.Value ? 0 : Convert.ToDouble(dr["vat"]));
                        totalAmount += (dr["total"] == DBNull.Value ? 0 : Convert.ToDouble(dr["total"]));
                        totalWithVat += (dr["total_with_vat"] == DBNull.Value ? 0 : Convert.ToDouble(dr["total_with_vat"]));
                    }

                    DataRow totalRow = filteredDt.NewRow();
                    totalRow["product_name"] = "Total";
                    totalRow["quantity"] = totalQty;
                    totalRow["cost_price"] = totalCostPrice;
                    totalRow["discount_value"] = totalDiscount;
                    totalRow["vat"] = totalVat;
                    totalRow["total"] = totalAmount;
                    totalRow["total_with_vat"] = totalWithVat;
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

        private void frm_PurchaseProductDetail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
                this.Close();

            if (e.KeyCode == Keys.F4 || (e.Control && e.KeyCode == Keys.P))
                btn_print.PerformClick();
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
                printer.PageSettings.Landscape = true;
                printer.Title = "Purchase Product Details";
                printer.SubTitle = "Invoice No: " + _invoiceNo;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.Footer = string.Empty;
                printer.FooterSpacing = 15;
                printer.ColumnWidth = DGVPrinterHelper.DGVPrinter.ColumnWidthSetting.CellWidth;
                printer.ColumnWidths.Clear();

                foreach (DataGridViewColumn col in grid_products.Columns)
                {
                    if (col.Visible)
                    {
                        float width = 0;
                        switch (col.Name)
                        {
                            case "col_item_code": width = 60; break;
                            case "col_product_name": width = 150; break;
                            case "col_quantity": width = 50; break;
                            case "col_cost_price": width = 70; break;
                            case "col_discount_value": width = 60; break;
                            case "col_vat": width = 50; break;
                            case "col_total": width = 70; break;
                            case "col_total_with_vat": width = 80; break;
                            default: width = 70; break;
                        }
                        printer.ColumnWidths.Add(col.Name, width);
                    }
                }

                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.CellAlignment = StringAlignment.Near;
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
                string defaultName = $"PurchaseDetail_{_invoiceNo}";
                ExcelExportHelper.ExportDataTableToExcel(exportDt, defaultName, this, includeLastRow: true);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
