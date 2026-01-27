using CrystalDecisions.CrystalReports.Engine;
using pos.Security.Authorization;
using POS.BLL;
using pos.UI;
using pos.UI.Busy;
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

namespace pos
{
    public partial class frm_all_estimates : Form
    {
        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;
        private readonly Timer _searchDebounce = new Timer();
        private const int DebounceMs = 300;

        public frm_all_estimates()
        {
            InitializeComponent();
        }

        public void frm_all_estimates_Load(object sender, EventArgs e)
        {
            // Debounce for search
            _searchDebounce.Interval = DebounceMs;
            _searchDebounce.Tick += SearchDebounce_Tick;

            load_all_estimates_grid();
        }

        private void SearchDebounce_Tick(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            btn_search.PerformClick();
        }

        public void load_all_estimates_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading estimates...", "جاري تحميل عروض الأسعار...")))
                {
                    grid_all_estimates.DataSource = null;

                    //bind data in data grid view  
                    EstimatesBLL objEstimatesBLL = new EstimatesBLL();
                    grid_all_estimates.AutoGenerateColumns = false;

                    //String keyword = "id,name,date_created";
                   // String table = "pos_all_estimates";
                    grid_all_estimates.DataSource = objEstimatesBLL.GetAllEstimates();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_all_estimates_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    EstimatesBLL objBLL = new EstimatesBLL();

                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_all_estimates.DataSource = objBLL.SearchRecord(condition);
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

        private void txt_search_TextChanged(object sender, EventArgs e)
        {
            // debounce search so DB is not hit for each keypress
            _searchDebounce.Stop();
            _searchDebounce.Start();
        }

        private void grid_all_estimates_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                var sale_id = grid_all_estimates.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
                var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();
                
                load_estimates_items_detail(Convert.ToInt16(sale_id), invoice_no);
            }
        }

        private void grid_all_estimates_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var sale_id = grid_all_estimates.CurrentRow.Cells["id"].Value.ToString(); // retreive the current row
            var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();
            
            load_estimates_items_detail(Convert.ToInt16(sale_id), invoice_no);
        }

        private void load_estimates_items_detail(int sale_id, string invoice_no)
        {
            using (BusyScope.Show(this, UiMessages.T("Loading estimate details...", "جاري تحميل تفاصيل عرض السعر...")))
            {
                frm_estimates_detail frm_estimates_detail_obj = new frm_estimates_detail(sale_id, invoice_no);
                frm_estimates_detail_obj.load_estimates_detail_grid();
                frm_estimates_detail_obj.ShowDialog();
            }
        }

        private void frm_all_estimates_KeyDown(object sender, KeyEventArgs e)
        {
            // Print estimate receipt on Ctrl+P
            if (e.Control && e.KeyCode == Keys.P)
            {
                btn_print_invoice.PerformClick();
            }
        }

        public DataTable load_estimates_receipt()
        {
            var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();
            //bind data in data grid view  
            EstimatesBLL objEstimatesBLL = new EstimatesBLL();
            DataTable dt = objEstimatesBLL.SaleReceipt(invoice_no);
            return dt;

        }

        private void btn_print_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Preparing receipt...", "جاري تجهيز الإيصال...")))
            {
                using (frm_sales_receipt obj = new frm_sales_receipt(load_estimates_receipt()))
                {
                    obj.ShowDialog();
                }
            }
        }

        private void btn_print_invoice_Click(object sender, EventArgs e)
        {
            if (grid_all_estimates.Rows.Count > 0)
            {
                var confirm = UiMessages.ConfirmYesNo(
                    "Print invoice with product codes?",
                    "هل تريد طباعة الفاتورة مع أكواد المنتجات؟",
                    captionEn: "Print Invoice",
                    captionAr: "طباعة الفاتورة"
                );

                bool isPrintInvoiceCode = (confirm == DialogResult.Yes);

                using (BusyScope.Show(this, UiMessages.T("Opening invoice...", "جاري فتح الفاتورة...")))
                {
                    using (frm_sales_invoice obj = new frm_sales_invoice(load_estimates_receipt(), false, isPrintInvoiceCode))
                    {
                        obj.ShowDialog();
                    }
                }
            }
            else
            {
                UiMessages.ShowInfo(
                    "No estimates available.",
                    "لا توجد عروض أسعار.",
                    "Estimates",
                    "عروض الأسعار"
                );
            }
        }

        private void grid_all_estimates_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string name = grid_all_estimates.Columns[e.ColumnIndex].Name;
            if (name == "detail")
            {
                var sale_id = grid_all_estimates.CurrentRow.Cells["id"].Value.ToString();
                var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();

                load_estimates_items_detail(Convert.ToInt16(sale_id), invoice_no);

            }
            if (name == "btn_delete")
            {
                if (grid_all_estimates.CurrentRow == null)
                {
                    UiMessages.ShowInfo(
                        "Please select an estimate first.",
                        "يرجى اختيار عرض سعر أولاً.",
                        "Estimates",
                        "عروض الأسعار"
                    );
                    return;
                }

                //permission check
                if (!_auth.HasPermission(_currentUser, Permissions.Quotes_Delete))
                {
                    UiMessages.ShowWarning(
                        "You do not have permission to delete estimates.",
                        "ليس لديك صلاحية لحذف عروض الأسعار.",
                        "Permission Denied",
                        "تم رفض الصلاحية"
                    );
                    return;
                }

                var invoice_no = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();

                var confirm = UiMessages.ConfirmYesNo(
                    "Delete this estimate? This action cannot be undone.",
                    "هل تريد حذف عرض السعر؟ لا يمكن التراجع عن هذا الإجراء.",
                    captionEn: "Confirm Delete",
                    captionAr: "تأكيد الحذف"
                );

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T("Deleting estimate...", "جاري حذف عرض السعر...")))
                {
                    EstimatesBLL estimatesBLL = new EstimatesBLL();
                    int qresult = estimatesBLL.DeleteEstimates(invoice_no);
                    if (qresult > 0)
                    {
                        UiMessages.ShowInfo(
                            "Estimate has been deleted successfully.",
                            "تم حذف عرض السعر بنجاح.",
                            "Deleted",
                            "تم الحذف"
                        );
                        load_all_estimates_grid();
                    }
                    else
                    {
                        UiMessages.ShowError(
                            "Estimate could not be deleted. Please try again.",
                            "تعذر حذف عرض السعر. يرجى المحاولة مرة أخرى.",
                            "Error",
                            "خطأ"
                        );
                    }
                }
            }
        }

        private void btnSendWhatsApp_Click(object sender, EventArgs e)
        {
            if (grid_all_estimates.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select an estimate first.",
                    "يرجى اختيار عرض سعر أولاً.",
                    "Estimates",
                    "عروض الأسعار"
                );
                return;
            }

            string invoiceNo = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();
            if (string.IsNullOrWhiteSpace(invoiceNo))
            {
                UiMessages.ShowInfo(
                    "Please select a valid estimate.",
                    "يرجى اختيار عرض سعر صالح.",
                    "Estimates",
                    "عروض الأسعار"
                );
                return;
            }

            using (BusyScope.Show(this, UiMessages.T("Opening WhatsApp...", "جاري فتح واتساب...")))
            {
                frm_send_whatsapp _frm_Send_Whatsapp = new frm_send_whatsapp(invoiceNo, true);
                _frm_Send_Whatsapp.ShowDialog();
            }
        }

        private void Btn_PrintPOS80_Click(object sender, EventArgs e)
        {
            if (grid_all_estimates.Rows.Count > 0)
            {
                string invoiceNo = grid_all_estimates.CurrentRow.Cells["invoice_no"].Value.ToString();
                using (frm_sales_invoice obj = new frm_sales_invoice(load_estimates_receipt(), false, false, true))
                {
                    obj.ShowDialog();
                }
            }
        }
    }
}
