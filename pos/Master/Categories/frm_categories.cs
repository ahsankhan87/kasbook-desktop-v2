using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using POS.BLL;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_categories : Form
    {

        public frm_categories()
        {
            InitializeComponent();
        }


        public void frm_categories_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            load_categories_grid();
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel2, lbl_taxes_title, panel1, grid_categories, id);
        }

        public void load_categories_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading categories...", "جاري تحميل الأقسام...")))
                {
                    grid_categories.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_categories.AutoGenerateColumns = false;

                    String keyword = "id,code,name,date_created";
                    String table = "pos_categories";
                    grid_categories.DataSource = objBLL.GetRecord(keyword, table);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addCategory frm_addCategory_obj = new frm_addCategory(this);
            frm_addCategory.instance.tb_lbl_is_edit.Text = "false";

            frm_addCategory_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_categories.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a category record to update.",
                    "يرجى اختيار قسم للتحديث.",
                    "Categories",
                    "الأقسام"
                );
                return;
            }

            string id = grid_categories.CurrentRow.Cells["id"].Value.ToString();
            string code = grid_categories.CurrentRow.Cells["code"].Value.ToString();
            string name = grid_categories.CurrentRow.Cells["name"].Value.ToString();
            
            frm_addCategory frm_addCategory_obj = new frm_addCategory(this);
            frm_addCategory.instance.tb_lbl_is_edit.Text = "true";

            frm_addCategory.instance.tb_id.Text = id;
            frm_addCategory.instance.tb_code.Text = code;
            frm_addCategory.instance.tb_name.Text = name;
            
            frm_addCategory.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_categories.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a category record to delete.",
                    "يرجى اختيار قسم للحذف.",
                    "Categories",
                    "الأقسام"
                );
                return;
            }

            string idText = Convert.ToString(grid_categories.CurrentRow.Cells[0].Value);
            int id;
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                UiMessages.ShowInfo(
                    "The selected record is not valid.",
                    "السجل المحدد غير صالح.",
                    "Categories",
                    "الأقسام"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this category?",
                "هل تريد حذف هذا القسم؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    CategoriesBLL objBLL = new CategoriesBLL();
                    objBLL.Delete(id);
                }

                UiMessages.ShowInfo(
                    "Record deleted successfully.",
                    "تم حذف السجل بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );

                load_categories_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_categories_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    CategoriesBLL objBLL = new CategoriesBLL();
                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_categories.DataSource = objBLL.SearchRecord(condition);
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

        private void frm_categories_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control == true && e.KeyCode == Keys.N)
            {
                btn_new.PerformClick();

            }

            if (e.Control == true && e.KeyCode == Keys.U)
            {
                btn_update.PerformClick();
            }

            if (e.Control == true && e.KeyCode == Keys.D)
            {
                btn_delete.PerformClick();

            }
        }
    }
}
