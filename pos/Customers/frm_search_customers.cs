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
    public partial class frm_search_customers : Form
    {
        private frm_addCustomer mainForm;
        string _search = "";

        public bool _returnStatus = false;

        private readonly Timer _searchDebounce = new Timer();

        public frm_search_customers(frm_addCustomer mainForm, string search)
        {
            this.mainForm = mainForm;

            _search = search;

            InitializeComponent();
        }

        public frm_search_customers()
        {
            InitializeComponent();

        }

        private void frm_search_customers_Load(object sender, EventArgs e)
        {
            txt_search.Text = _search;

            // Debounce search
            _searchDebounce.Interval = 300;
            _searchDebounce.Tick += SearchDebounce_Tick;

            load_customers_grid();
            grid_search_customers.Focus();
        }

        private void SearchDebounce_Tick(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    CustomerBLL objBLL = new CustomerBLL();
                    grid_search_customers.AutoGenerateColumns = false;

                    String condition = txt_search.Text.Trim();
                    grid_search_customers.DataSource = objBLL.SearchRecord(condition);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        public void load_customers_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading customers...", "جاري تحميل العملاء...")))
                {
                    grid_search_customers.DataSource = null;

                    //bind data in data grid view  
                    CustomerBLL objBLL = new CustomerBLL();
                    grid_search_customers.AutoGenerateColumns = false;

                    String condition = txt_search.Text.Trim();
                    grid_search_customers.DataSource = objBLL.SearchRecord(condition);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (grid_search_customers.SelectedCells.Count > 0)
            {
                string customer_id = grid_search_customers.CurrentRow.Cells["id"].Value.ToString();

                if (mainForm != null)
                {
                    using (BusyScope.Show(this, UiMessages.T("Loading customer...", "جاري تحميل العميل...")))
                    {
                        mainForm.load_customer_detail(int.Parse(customer_id));
                        mainForm.load_customer_transactions_grid(int.Parse(customer_id));
                    }
                }

                this.Close();
            }
            else
            {
                UiMessages.ShowInfo("Please select record", "يرجى اختيار سجل", "Customers", "العملاء");
            }
        }

        private void grid_search_customers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                btn_ok.PerformClick();
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grid_search_customers_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }


        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            // Debounce DB calls while user types
            _searchDebounce.Stop();
            _searchDebounce.Start();
        }

        private void frm_search_customers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                grid_search_customers.Focus();
            }
        }

    }
}