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
    public partial class frm_search_suppliers : Form
    {
        private frm_addSupplier mainForm;
        
        string _search = "";
        
        public bool _returnStatus = false;

        private readonly Timer _searchDebounce = new Timer();
        private const int DebounceMs = 300;

        public frm_search_suppliers(frm_addSupplier mainForm, string search)
        {
            this.mainForm = mainForm;
            
            _search = search;
            
            InitializeComponent();
        }

        public frm_search_suppliers()
        {
            InitializeComponent();

        }

        private void frm_search_suppliers_Load(object sender, EventArgs e)
        {
            txt_search.Text = _search;

            // Debounce search
            _searchDebounce.Interval = DebounceMs;
            _searchDebounce.Tick += SearchDebounce_Tick;

            load_suppliers_grid();
            grid_search_suppliers.Focus();
        }

        private void SearchDebounce_Tick(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching suppliers...", "جاري البحث عن الموردين...")))
                {
                    grid_search_suppliers.DataSource = null;

                    SupplierBLL objBLL = new SupplierBLL();
                    grid_search_suppliers.AutoGenerateColumns = false;

                    String condition = txt_search.Text.Trim();
                    grid_search_suppliers.DataSource = objBLL.SearchRecord(condition);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        public void load_suppliers_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading suppliers...", "جاري تحميل الموردين...")))
                {
                    grid_search_suppliers.DataSource = null;

                    //bind data in data grid view  
                    SupplierBLL objBLL = new SupplierBLL();
                    grid_search_suppliers.AutoGenerateColumns = false;

                    String condition = txt_search.Text.Trim();
                    grid_search_suppliers.DataSource = objBLL.SearchRecord(condition);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (grid_search_suppliers.SelectedCells.Count > 0)
            {
                string supplier_id = grid_search_suppliers.CurrentRow.Cells["id"].Value.ToString();

                if (mainForm != null)
                {
                    using (BusyScope.Show(this, UiMessages.T("Loading supplier...", "جاري تحميل المورد...")))
                    {
                        mainForm.load_detail(int.Parse(supplier_id));
                        mainForm.load_transactions_grid(int.Parse(supplier_id));
                    }
                }

                this.Close();
            }
            else
            {
                UiMessages.ShowInfo(
                    "Please select a supplier record.",
                    "يرجى اختيار سجل مورد.",
                    "Suppliers",
                    "الموردون"
                );
            }
        }

        private void grid_search_suppliers_KeyDown(object sender, KeyEventArgs e)
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

        private void grid_search_suppliers_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }


        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            // Debounce DB calls while the user types
            _searchDebounce.Stop();
            _searchDebounce.Start();
        }

        private void frm_search_suppliers_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                grid_search_suppliers.Focus();
            }
        }
        
    }
}
     