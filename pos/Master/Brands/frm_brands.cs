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
    public partial class frm_brands : Form
    {

        public frm_brands()
        {
            InitializeComponent();
        }


        public void frm_brands_Load(object sender, EventArgs e)
        {
            load_Brands_grid();
           
        }

        public void load_Brands_grid()
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Loading brands...", "جاري تحميل العلامات التجارية...")))
                {
                    grid_brands.DataSource = null;

                    //bind data in data grid view  
                    GeneralBLL objBLL = new GeneralBLL();
                    grid_brands.AutoGenerateColumns = false;

                    String keyword = "*";
                    String table = "pos_brands";
                    grid_brands.DataSource = objBLL.GetRecord(keyword, table);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }

        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            frm_addBrand frm_addBrand_obj = new frm_addBrand(this);
            frm_addBrand.instance.tb_lbl_is_edit.Text = "false";

            frm_addBrand_obj.ShowDialog();
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (grid_brands.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a brand record to update.",
                    "يرجى اختيار علامة تجارية للتحديث.",
                    "Brands",
                    "العلامات التجارية"
                );
                return;
            }

            string id = grid_brands.CurrentRow.Cells["id"].Value.ToString();
            string code = grid_brands.CurrentRow.Cells["code"].Value.ToString();
            string name = grid_brands.CurrentRow.Cells["name"].Value.ToString();
            string category_code = grid_brands.CurrentRow.Cells["category_code"].Value.ToString();
            string group_code = grid_brands.CurrentRow.Cells["group_code"].Value.ToString();
            
            frm_addBrand frm_addBrand_obj = new frm_addBrand(this);
            frm_addBrand.instance.tb_lbl_is_edit.Text = "true";

            frm_addBrand.instance.tb_id.Text = id;
            frm_addBrand.instance.tb_code.Text = code;
            frm_addBrand.instance.tb_name.Text = name;
            frm_addBrand.instance.tb_category.SelectedValue = category_code;
            frm_addBrand.instance.tb_groups.SelectedValue = group_code;
            
            frm_addBrand.instance.Show();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (grid_brands.CurrentRow == null)
            {
                UiMessages.ShowInfo(
                    "Please select a brand record to delete.",
                    "يرجى اختيار علامة تجارية للحذف.",
                    "Brands",
                    "العلامات التجارية"
                );
                return;
            }

            string idText = Convert.ToString(grid_brands.CurrentRow.Cells[0].Value);
            int id;
            if (!int.TryParse(idText, out id) || id <= 0)
            {
                UiMessages.ShowInfo(
                    "The selected record is not valid.",
                    "السجل المحدد غير صالح.",
                    "Brands",
                    "العلامات التجارية"
                );
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                "Delete this brand?",
                "هل تريد حذف هذه العلامة التجارية؟",
                captionEn: "Confirm Delete",
                captionAr: "تأكيد الحذف"
            );

            if (confirm != DialogResult.Yes)
                return;

            try
            {
                using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                {
                    BrandsBLL objBLL = new BrandsBLL();
                    objBLL.Delete(id);
                }

                UiMessages.ShowInfo(
                    "Record deleted successfully.",
                    "تم حذف السجل بنجاح.",
                    "Deleted",
                    "تم الحذف"
                );

                load_Brands_grid();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message);
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            load_Brands_grid();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    BrandsBLL objBLL = new BrandsBLL();
                    String condition = (txt_search.Text ?? string.Empty).Trim();
                    grid_brands.DataSource = objBLL.SearchRecord(condition);
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

        private void frm_brands_KeyDown(object sender, KeyEventArgs e)
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
