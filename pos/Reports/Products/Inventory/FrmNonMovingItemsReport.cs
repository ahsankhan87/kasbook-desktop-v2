using POS.BLL;
using POS.Core;
using POS.DLL;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pos.Reports.Products.Inventory
{
    /// <summary>
    /// Non-Moving Items Report Form
    /// Displays slow-moving and stagnant inventory items (no sales for extended periods).
    /// Helps identify items requiring promotional action, clearance, or obsolescence write-off.
    /// Features: Filter by days dormant, category, brand, location. Export to CSV, Print, and Summary statistics.
    /// </summary>
    public partial class FrmNonMovingItemsReport : Form
    {
        private DataTable _currentData;
        private NonMovingItemsReportBLL _bll;

        public FrmNonMovingItemsReport()
        {
            InitializeComponent();
            _bll = new NonMovingItemsReportBLL();
        }

        private void FrmNonMovingItemsReport_Load(object sender, EventArgs e)
        {
            try
            {
                // Apply theme
                AppTheme.Apply(this);

                // Initialize grid formatting
                ConfigureDataGridView();

                // Load filter options
                LoadFilterOptions();

                // Set default values
                nudDaysThreshold.Value = 90;
                nudMinQty.Value = 1;

                lblStatus.Text = UiMessages.T(
                    "Ready. Click 'Load' to generate report.",
                    "جاهز. انقر على 'تحميل' لإنشاء التقرير.");
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error initializing form: {ex.Message}",
                    $"خطأ في تهيئة النموذج: {ex.Message}");
            }
        }

        private void ConfigureDataGridView()
        {
            dgvReport.DefaultCellStyle.BackColor = AppTheme.Surface;
            dgvReport.DefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            dgvReport.DefaultCellStyle.Font = AppTheme.FontGrid;

            dgvReport.AlternatingRowsDefaultCellStyle.BackColor = AppTheme.GridAltRow;
            dgvReport.AlternatingRowsDefaultCellStyle.ForeColor = AppTheme.TextPrimary;

            dgvReport.ColumnHeadersDefaultCellStyle.BackColor = AppTheme.GridHeader;
            dgvReport.ColumnHeadersDefaultCellStyle.ForeColor = AppTheme.TextPrimary;
            dgvReport.ColumnHeadersDefaultCellStyle.Font = AppTheme.FontGridHeader;
            dgvReport.ColumnHeadersHeight = 28;

            dgvReport.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvReport.AllowUserToResizeRows = false;
            dgvReport.RowHeadersWidth = 40;
        }

        private void LoadFilterOptions()
        {
            try
            {
                string allCategoriesOption = UiMessages.T("-- All Categories --", "-- جميع الفئات --");
                string allBrandsOption = UiMessages.T("-- All Brands --", "-- جميع العلامات --");
                string allLocationsOption = UiMessages.T("-- All Locations --", "-- جميع المواقع --");

                // Load categories
                CategoriesDLL catDLL = new CategoriesDLL();
                DataTable categories = catDLL.GetAll();
                cmbCategory.Items.Add(allCategoriesOption);
                foreach (DataRow row in categories.Rows)
                {
                    cmbCategory.Items.Add(row["code"].ToString());
                }
                cmbCategory.SelectedIndex = 0;

                // Load brands
                BrandsDLL brandDLL = new BrandsDLL();
                DataTable brands = brandDLL.GetAll();
                cmbBrand.Items.Add(allBrandsOption);
                foreach (DataRow row in brands.Rows)
                {
                    cmbBrand.Items.Add(row["code"].ToString());
                }
                cmbBrand.SelectedIndex = 0;

                // Load locations
                LocationsDLL locDLL = new LocationsDLL();
                DataTable locations = locDLL.GetAll();
                cmbLocation.Items.Add(allLocationsOption);
                foreach (DataRow row in locations.Rows)
                {
                    cmbLocation.Items.Add(row["code"].ToString());
                }
                cmbLocation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                UiMessages.ShowWarning(
                    $"Could not load filter options: {ex.Message}",
                    $"لم يتمكن من تحميل خيارات التصفية: {ex.Message}");
            }
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadReport();
        }

        private void LoadReport()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T(
                    "Loading Non-Moving Items Report...",
                    "جاري تحميل تقرير العناصر غير المتحركة...")))
                {
                    int branchId = UsersModal.logged_in_branch_id;
                    int daysThreshold = (int)nudDaysThreshold.Value;
                    decimal minQtyOnHand = (decimal)nudMinQty.Value;

                    string allCategoriesOption = UiMessages.T("-- All Categories --", "-- جميع الفئات --");
                    string allBrandsOption = UiMessages.T("-- All Brands --", "-- جميع العلامات --");
                    string allLocationsOption = UiMessages.T("-- All Locations --", "-- جميع المواقع --");

                    string categoryCode = cmbCategory.SelectedItem?.ToString();
                    if (categoryCode == allCategoriesOption) categoryCode = null;

                    string brandCode = cmbBrand.SelectedItem?.ToString();
                    if (brandCode == allBrandsOption) brandCode = null;

                    string locationCode = cmbLocation.SelectedItem?.ToString();
                    if (locationCode == allLocationsOption) locationCode = null;

                    // Fetch data
                    _currentData = _bll.GetNonMovingItems(branchId, daysThreshold, minQtyOnHand, categoryCode, brandCode, locationCode);

                    // Bind to grid
                    dgvReport.DataSource = _currentData;

                    // Format grid columns
                    FormatGridColumns();

                    // Update summary
                    UpdateSummary();

                    lblStatus.Text = UiMessages.T(
                        $"Report loaded: {_currentData.Rows.Count} items found.",
                        $"تم تحميل التقرير: تم العثور على {_currentData.Rows.Count} عنصراً.");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading report: {ex.Message}",
                    $"خطأ في تحميل التقرير: {ex.Message}");
                lblStatus.Text = UiMessages.T(
                    "Error loading report.",
                    "خطأ في تحميل التقرير.");
            }
        }

        private void FormatGridColumns()
        {
            // Column name to Arabic translation mapping (from sp_GetNonMovingItems)
            var columnTranslations = new Dictionary<string, string>
            {
                { "id", UiMessages.T("ID", "رقم تعريفي") },
                { "ItemCode", UiMessages.T("Item Code", "رمز العنصر") },
                { "ItemName", UiMessages.T("Item Name", "اسم العنصر") },
                { "ItemNameAR", UiMessages.T("Item Name (AR)", "اسم العنصر (عربي)") },
                { "Category", UiMessages.T("Category", "الفئة") },
                { "Brand", UiMessages.T("Brand", "العلامة") },
                { "Location", UiMessages.T("Location", "الموقع") },
                { "QtyOnHand", UiMessages.T("Qty On Hand", "الكمية المتبقية") },
                { "CostPrice", UiMessages.T("Cost Price", "سعر التكلفة") },
                { "SalePrice", UiMessages.T("Sale Price", "سعر البيع") },
                { "TotalValue", UiMessages.T("Total Value", "القيمة الإجمالية") },
                { "LastSaleDate", UiMessages.T("Last Sale Date", "تاريخ آخر مبيعة") },
                { "DaysSinceLastSale", UiMessages.T("Days Dormant", "أيام راكدة") },
                { "TotalTransactions", UiMessages.T("Total Transactions", "إجمالي المعاملات") },
                { "QtyMovedLastPeriod", UiMessages.T("Qty Moved", "الكمية المباعة") },
                { "StockStatus", UiMessages.T("Stock Status", "حالة المخزون") },
                { "QuantityLevel", UiMessages.T("Quantity Level", "مستوى الكمية") }
            };

            // Auto-hide unnecessary columns and format currency/numeric columns
            foreach (DataGridViewColumn col in dgvReport.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                col.MinimumWidth = 80;

                // Translate column header
                if (columnTranslations.ContainsKey(col.Name))
                    col.HeaderText = columnTranslations[col.Name];

                // Hide ID column
                if (col.Name == "id")
                    col.Visible = false;

                if (col.Name.Contains("Value") || col.Name.Contains("Price") || col.Name.Contains("Cost"))
                {
                    col.DefaultCellStyle.Format = "N2";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (col.Name.Contains("Qty") || col.Name.Contains("Quantity") || col.Name.Contains("Transactions") || col.Name.Contains("Moved"))
                {
                    col.DefaultCellStyle.Format = "N0";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
                else if (col.Name.Contains("Date") || col.Name.Contains("LastSale"))
                {
                    col.DefaultCellStyle.Format = "yyyy-MM-dd";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                }
            }
        }

        private void UpdateSummary()
        {
            try
            {
                string totalItemsLabel = UiMessages.T("Total Items:", "إجمالي العناصر:");
                string totalValueLabel = UiMessages.T("Total Value:", "القيمة الإجمالية:");
                string avgDaysLabel = UiMessages.T("Avg Days Dormant:", "متوسط الأيام الراكدة:");

                if (_currentData == null || _currentData.Rows.Count == 0)
                {
                    lblTotalItems.Text = $"{totalItemsLabel} 0";
                    lblTotalValue.Text = $"{totalValueLabel} 0";
                    lblDormantDays.Text = $"{avgDaysLabel} 0";
                    return;
                }

                int totalItems = _currentData.Rows.Count;
                decimal totalValue = 0;
                int totalDays = 0;

                foreach (DataRow row in _currentData.Rows)
                {
                    if (row.Table.Columns.Contains("TotalValue") && decimal.TryParse(row["TotalValue"].ToString(), out decimal value))
                        totalValue += value;

                    if (row.Table.Columns.Contains("DaysSinceLastSale") && int.TryParse(row["DaysSinceLastSale"].ToString(), out int days))
                        totalDays += days;
                }

                int avgDays = totalItems > 0 ? totalDays / totalItems : 0;

                lblTotalItems.Text = $"{totalItemsLabel} {totalItems:N0}";
                lblTotalValue.Text = $"{totalValueLabel} {totalValue:N2}";
                lblDormantDays.Text = $"{avgDaysLabel} {avgDays:N0}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating summary: {ex.Message}");
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            if (_currentData == null || _currentData.Rows.Count == 0)
            {
                UiMessages.ShowWarning(
                    "No data to export. Please load a report first.",
                    "لا توجد بيانات للتصدير. يرجى تحميل التقرير أولاً.");
                return;
            }

            try
            {
                using (SaveFileDialog sfd = new SaveFileDialog
                {
                    Filter = "CSV Files (*.csv)|*.csv|Excel Files (*.xlsx)|*.xlsx",
                    DefaultExt = "csv",
                    FileName = $"NonMovingItems_{DateTime.Now:yyyyMMdd_HHmmss}.csv"
                })
                {
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        using (BusyScope.Show(this, UiMessages.T(
                            "Exporting data...",
                            "جاري تصدير البيانات...")))
                        {
                            string result = _bll.ExportNonMovingItemsToCSV(_currentData, sfd.FileName);
                            UiMessages.ShowInfo(result, UiMessages.T("Export successful.", "تم التصدير بنجاح"));
                            lblStatus.Text = UiMessages.T(
                                $"Exported to: {sfd.FileName}",
                                $"تم التصدير إلى: {sfd.FileName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error exporting data: {ex.Message}",
                    $"خطأ في تصدير البيانات: {ex.Message}");
            }
        }

        private void BtnPrint_Click(object sender, EventArgs e)
        {
            if (_currentData == null || _currentData.Rows.Count == 0)
            {
                UiMessages.ShowWarning(
                    "No data to print. Please load a report first.",
                    "لا توجد بيانات للطباعة. يرجى تحميل التقرير أولاً.");
                return;
            }

            try
            {
                using (PrintDialog pDialog = new PrintDialog())
                {
                    if (pDialog.ShowDialog() == DialogResult.OK)
                    {
                        using (BusyScope.Show(this, UiMessages.T(
                            "Preparing print...",
                            "جاري تحضير الطباعة...")))
                        {
                            PrintDataGridView(dgvReport, "Non-Moving Items Report");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error printing: {ex.Message}",
                    $"خطأ في الطباعة: {ex.Message}");
            }
        }

        private void PrintDataGridView(DataGridView dgv, string reportTitle)
        {
            // Simple print implementation
            // For production, integrate with Crystal Reports or a dedicated reporting library
            UiMessages.ShowInfo(
                "Print functionality requires integration with a reporting library.\nConsider using Crystal Reports or FastReport.",
                "تتطلب وظيفة الطباعة التكامل مع مكتبة إعداد التقارير.\nفكر في استخدام Crystal Reports أو FastReport.");
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFilters();
        }

        private void ClearFilters()
        {
            string totalItemsLabel = UiMessages.T("Total Items:", "إجمالي العناصر:");
            string totalValueLabel = UiMessages.T("Total Value:", "القيمة الإجمالية:");
            string avgDaysLabel = UiMessages.T("Avg Days Dormant:", "متوسط الأيام الراكدة:");

            nudDaysThreshold.Value = 90;
            nudMinQty.Value = 1;
            cmbCategory.SelectedIndex = 0;
            cmbBrand.SelectedIndex = 0;
            cmbLocation.SelectedIndex = 0;
            dgvReport.DataSource = null;
            lblTotalItems.Text = $"{totalItemsLabel} 0";
            lblTotalValue.Text = $"{totalValueLabel} 0";
            lblDormantDays.Text = $"{avgDaysLabel} 0";
            lblStatus.Text = UiMessages.T("Filters cleared. Ready.", "تم مسح المرشحات. جاهز.");
        }
    }
}
