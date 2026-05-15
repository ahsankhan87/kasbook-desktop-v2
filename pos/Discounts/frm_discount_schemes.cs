using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Discounts
{
    public partial class frm_discount_schemes : Form
    {
        public frm_discount_schemes()
        {
            InitializeComponent();
        }

        private void frm_discount_schemes_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            LoadGrid();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel_header, lbl_title, panel_body, grid_schemes, col_id);
        }

        private void LoadGrid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading discount schemes...", "جاري تحميل خطط الخصم...")))
                {
                    var bll = new DiscountSchemesBLL();
                    grid_schemes.AutoGenerateColumns = false;
                    grid_schemes.DataSource = bll.GetAll(UsersModal.logged_in_branch_id);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            using (var frm = new frm_add_discount_scheme(this))
            {
                frm.ShowDialog();
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_schemes.CurrentRow == null)
            {
                UiMessages.ShowInfo("Please select a scheme to update.", "يرجى اختيار خطة للتحديث.", "Discount Schemes", "خطط الخصم");
                return;
            }

            int id = Convert.ToInt32(grid_schemes.CurrentRow.Cells["col_id"].Value);
            using (var frm = new frm_add_discount_scheme(this, id))
            {
                frm.ShowDialog();
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_schemes.CurrentRow == null)
            {
                UiMessages.ShowInfo("Please select a scheme to delete.", "يرجى اختيار خطة للحذف.", "Discount Schemes", "خطط الخصم");
                return;
            }

            var confirmResult = MessageBox.Show(
                UiMessages.T("Are you sure you want to delete this scheme?", "هل أنت متأكد من حذف هذه الخطة؟"),
                UiMessages.T("Delete", "حذف"),
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmResult != DialogResult.Yes) return;

            try
            {
                int id = Convert.ToInt32(grid_schemes.CurrentRow.Cells["col_id"].Value);
                new DiscountSchemesBLL().Delete(id);
                LoadGrid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_toggle_Click(object sender, EventArgs e)
        {
            if (grid_schemes.CurrentRow == null)
            {
                UiMessages.ShowInfo("Please select a scheme.", "يرجى اختيار خطة.", "Discount Schemes", "خطط الخصم");
                return;
            }

            try
            {
                int id = Convert.ToInt32(grid_schemes.CurrentRow.Cells["col_id"].Value);
                new DiscountSchemesBLL().ToggleActive(id);
                LoadGrid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            LoadGrid();
        }

        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                FilterGrid(txt_search.Text.Trim());
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            FilterGrid(txt_search.Text.Trim());
        }

        private void FilterGrid(string keyword)
        {
            if (grid_schemes.DataSource is DataTable dt)
            {
                dt.DefaultView.RowFilter = string.IsNullOrWhiteSpace(keyword)
                    ? ""
                    : $"name LIKE '%{keyword}%'";
            }
        }

        public void RefreshGrid() => LoadGrid();
    }
}
