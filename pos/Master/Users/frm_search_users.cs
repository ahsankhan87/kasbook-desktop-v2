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
    public partial class frm_search_users : Form
    {
        private frm_adduser mainForm;
        
        string _search = "";
        
        public bool _returnStatus = false;

        private readonly Timer _searchDebounce = new Timer();
        private const int DebounceMs = 300;

        public frm_search_users(frm_adduser mainForm, string search)
        {
            this.mainForm = mainForm;
            _search = search;

            InitializeComponent();

            _searchDebounce.Interval = DebounceMs;
            _searchDebounce.Tick += SearchDebounce_Tick;
        }

        public frm_search_users()
        {
            InitializeComponent();

            _searchDebounce.Interval = DebounceMs;
            _searchDebounce.Tick += SearchDebounce_Tick;
        }

        private void SearchDebounce_Tick(object sender, EventArgs e)
        {
            _searchDebounce.Stop();
            load_users_grid();
        }

        private void frm_search_users_Load(object sender, EventArgs e)
        {
            txt_search.Text = _search;
            load_users_grid();
            grid_search_users.Focus();
        }

        public void load_users_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading users...", "جاري تحميل المستخدمين...")))
                {
                    grid_search_users.DataSource = null;

                    //bind data in data grid view  
                    UsersBLL objBLL = new UsersBLL();
                    grid_search_users.AutoGenerateColumns = false;

                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_search_users.DataSource = objBLL.SearchRecord(condition);
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

                if (grid_search_users.SelectedCells.Count <= 0 || grid_search_users.CurrentRow == null)
                {
                    UiMessages.ShowInfo(
                        "Please select a user record.",
                        "يرجى اختيار سجل مستخدم.",
                        "Users",
                        "المستخدمون"
                    );
                    return;
                }

                var idObj = grid_search_users.CurrentRow.Cells["id"].Value;
                int userId;
                if (idObj == null || !int.TryParse(idObj.ToString(), out userId) || userId <= 0)
                {
                    UiMessages.ShowInfo(
                        "The selected user record is not valid.",
                        "سجل المستخدم المحدد غير صالح.",
                        "Users",
                        "المستخدمون"
                    );
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Loading user...", "جاري تحميل المستخدم...")))
                {
                    mainForm.load_user_detail(userId);
                    mainForm.load_user_rights(userId);
                    mainForm.load_user_commission_grid(userId);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void grid_search_users_KeyDown(object sender, KeyEventArgs e)
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

        private void grid_search_users_DoubleClick(object sender, EventArgs e)
        {
            btn_ok.PerformClick();
        }


        private void txt_search_KeyUp(object sender, KeyEventArgs e)
        {
            _searchDebounce.Stop();
            _searchDebounce.Start();
        }

        private void frm_search_users_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Down)
            {
                grid_search_users.Focus();
            }
        }
        
    }
}
    