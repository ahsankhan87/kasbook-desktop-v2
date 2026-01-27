using POS.BLL;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Windows.Forms;

namespace pos.Master.Banks
{
    public partial class frm_banks_search : Form
    {
        private frm_banks mainForm;
        string _search = "";

        public bool _returnStatus = false;

        private readonly Timer _searchDebounce = new Timer();
        private const int DebounceMs = 300;

        public frm_banks_search(frm_banks mainForm, string search)
        {
            this.mainForm = mainForm;
            _search = search;

            InitializeComponent();

            _searchDebounce.Interval = DebounceMs;
            _searchDebounce.Tick += SearchDebounce_Tick;
        }

        public frm_banks_search()
        {
            InitializeComponent();

            _searchDebounce.Interval = DebounceMs;
            _searchDebounce.Tick += SearchDebounce_Tick;
        }

        private void frm_banks_search_Load(object sender, EventArgs e)
        {
            txt_search.Text = _search;
            load_customers_grid();
            grid_search_banks.Focus();
        }

        private void SearchDebounce_Tick(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            PerformSearch();
        }

        public void load_customers_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading banks...", "جاري تحميل البنوك...")))
                {
                    grid_search_banks.DataSource = null;

                    //bind data in data grid view  
                    BankBLL objBLL = new BankBLL();
                    grid_search_banks.AutoGenerateColumns = false;

                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_search_banks.DataSource = objBLL.SearchRecord(condition);
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
                    grid_search_banks.DataSource = null;

                    BankBLL objBLL = new BankBLL();
                    grid_search_banks.AutoGenerateColumns = false;

                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_search_banks.DataSource = objBLL.SearchRecord(condition);
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
                if (mainForm == null)
                {
                    UiMessages.ShowError(
                        "Parent form is not available.",
                        "النموذج الرئيسي غير متوفر.",
                        "Error",
                        "خطأ"
                    );
                    return;
                }

                if (grid_search_banks.SelectedCells.Count <= 0 || grid_search_banks.CurrentRow == null)
                {
                    UiMessages.ShowInfo(
                        "Please select a bank record.",
                        "يرجى اختيار سجل بنك.",
                        "Banks",
                        "البنوك"
                    );
                    return;
                }

                var idObj = grid_search_banks.CurrentRow.Cells["id"].Value;
                int bankId;
                if (idObj == null || !int.TryParse(idObj.ToString(), out bankId) || bankId <= 0)
                {
                    UiMessages.ShowInfo(
                        "The selected bank record is not valid.",
                        "سجل البنك المحدد غير صالح.",
                        "Banks",
                        "البنوك"
                    );
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Loading bank...", "جاري تحميل البنك...")))
                {
                    mainForm.load_bank_detail(bankId);
                    mainForm.load_banks_transactions_grid(bankId);
                }

                this.Close();
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

        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            // Debounce DB calls while the user types
            _searchDebounce.Stop();
            _searchDebounce.Start();
        }

        private void grid_search_customers_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }

        private void grid_search_customers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok.PerformClick();
            }
        }

        private void frm_banks_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                grid_search_banks.Focus();
            }
            if (e.KeyData == Keys.Escape)
            {
                this.Close();
            }
        }
    }
}
