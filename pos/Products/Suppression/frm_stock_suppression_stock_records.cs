using POS.BLL;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Data;
using System.Windows.Forms;

namespace pos.Products.Suppression
{
    public partial class frm_stock_suppression_stock_records : Form
    {
        private readonly ProductBLL _productBLL = new ProductBLL();
        private string _initialSearchTerm = string.Empty;

        public string SelectedItemNumber { get; private set; } = string.Empty;
        public string SelectedDisplayText { get; private set; } = string.Empty;

        /// <summary>
        /// Initialize the stock records dialog with optional initial search term.
        /// This allows the main form to pre-populate the search when a code is typed.
        /// </summary>
        public frm_stock_suppression_stock_records(string initialSearchTerm = "")
        {
            InitializeComponent();
            _initialSearchTerm = initialSearchTerm?.Trim() ?? string.Empty;
        }

        private void frm_stock_suppression_stock_records_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            AppTheme.ApplyListFormStyleLightHeader(null, null, null, gridStockRecords);
            
            // Pre-fill search if initial term was provided
            if (!string.IsNullOrWhiteSpace(_initialSearchTerm))
            {
                txtSearch.Text = _initialSearchTerm;
            }
            
            txtSearch.Focus();
            
            // If we have an initial search term, load products immediately
            if (!string.IsNullOrWhiteSpace(_initialSearchTerm))
            {
                LoadProducts();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void btnPartNumber_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void btnWordSearch_Click(object sender, EventArgs e)
        {
            LoadProducts();
        }

        private void LoadProducts()
        {
            using (BusyScope.Show(this, UiMessages.T("Searching products...", "جاري البحث عن الأصناف...")))
            {
                try
                {
                    string q = (txtSearch.Text ?? string.Empty).Trim();
                    if (q.Length == 0)
                    {
                        gridStockRecords.DataSource = null;
                        lblStatus.Text = "Enter search term";
                        return;
                    }

                    DataTable dt = ProductBLL.SearchProductByBrandAndCategory(q, "", "", "");
                    gridStockRecords.AutoGenerateColumns = false;
                    gridStockRecords.DataSource = dt;
                    lblStatus.Text = dt.Rows.Count + " record(s)";
                    
                    // If only one result, auto-select it
                    if (dt.Rows.Count == 1)
                    {
                        gridStockRecords.CurrentCell = gridStockRecords.Rows[0].Cells[0];
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                    lblStatus.Text = "Error loading results";
                }
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            SelectCurrent();
        }

        private void gridStockRecords_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                SelectCurrent();
            }
        }

        private void SelectCurrent()
        {
            if (gridStockRecords.CurrentRow == null)
            {
                UiMessages.ShowWarning("Please select a part.", "يرجى اختيار قطعة.", captionEn: "Stock Records", captionAr: "سجلات المخزون");
                return;
            }

            SelectedItemNumber = Convert.ToString(gridStockRecords.CurrentRow.Cells["colPartNumber"].Value);
            string desc = Convert.ToString(gridStockRecords.CurrentRow.Cells["colDescription"].Value);
            SelectedDisplayText = SelectedItemNumber + "  " + desc;

            if (string.IsNullOrWhiteSpace(SelectedItemNumber))
            {
                UiMessages.ShowWarning("Invalid selected part.", "القطعة المختارة غير صالحة.", captionEn: "Stock Records", captionAr: "سجلات المخزون");
                return;
            }

            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}
