using POS.BLL;
using POS.DLL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_search_estimates : Form
    {
        private frm_sales salesForm;

        private readonly Timer _searchDebounce = new Timer();
        private const int DebounceMs = 300;

        public frm_search_estimates(frm_sales salesForm)
        {
            InitializeComponent();
            this.salesForm = salesForm;

            _searchDebounce.Interval = DebounceMs;
            _searchDebounce.Tick += SearchDebounce_Tick;
        }

        public frm_search_estimates()
        {
            InitializeComponent();

            _searchDebounce.Interval = DebounceMs;
            _searchDebounce.Tick += SearchDebounce_Tick;
        }

        private void frm_search_estimates_Load(object sender, EventArgs e)
        {
            load_estimates_grid();
            grid_search_estimates.Focus();

        }

        private void SearchDebounce_Tick(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            PerformSearch();
        }

        public void load_estimates_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading estimates...", "جاري تحميل عروض الأسعار...")))
                {
                    grid_search_estimates.DataSource = null;

                    EstimatesBLL objBLL = new EstimatesBLL();
                    grid_search_estimates.AutoGenerateColumns = false;

                    grid_search_estimates.DataSource = objBLL.GetAllActiveEstimates();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void PerformSearch()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    grid_search_estimates.DataSource = null;

                    EstimatesBLL objBLL = new EstimatesBLL();
                    grid_search_estimates.AutoGenerateColumns = false;

                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_search_estimates.DataSource = objBLL.SearchRecord(condition);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                if (salesForm == null)
                {
                    UiMessages.ShowError(
                        "Sales form is not available.",
                        "نموذج المبيعات غير متوفر.",
                        "Error",
                        "خطأ"
                    );
                    return;
                }

                if (grid_search_estimates.SelectedCells.Count <= 0 || grid_search_estimates.CurrentRow == null)
                {
                    UiMessages.ShowInfo(
                        "Please select an estimate.",
                        "يرجى اختيار عرض سعر.",
                        "Estimates",
                        "عروض الأسعار"
                    );
                    return;
                }

                string inv_no = grid_search_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();
                if (string.IsNullOrWhiteSpace(inv_no))
                {
                    UiMessages.ShowInfo(
                        "The selected estimate is not valid.",
                        "عرض السعر المحدد غير صالح.",
                        "Estimates",
                        "عروض الأسعار"
                    );
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Load this estimate into the sales screen?",
                    "هل تريد تحميل عرض السعر إلى شاشة المبيعات؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T("Loading estimate...", "جاري تحميل عرض السعر...")))
                {
                    SalesBLL salesObj = new SalesBLL();
                    DataTable estimates_dt = salesObj.GetEstimatesAndEstimatesItems(inv_no);

                    salesForm.Load_products_to_grid_by_invoiceno(estimates_dt, inv_no);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }


        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grid_search_estimates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok.PerformClick();
            }
        }

        private void grid_search_estimates_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }

        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            // Debounce DB calls while the user types
            _searchDebounce.Stop();
            _searchDebounce.Start();
        }
    }
}