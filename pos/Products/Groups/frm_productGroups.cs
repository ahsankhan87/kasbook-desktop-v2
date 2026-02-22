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
    public partial class frm_product_groups : Form
    {

        public frm_product_groups()
        {
            InitializeComponent();
        }


        public void frm_product_groups_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            using (BusyScope.Show(this, UiMessages.T("Loading product groups...", "جاري تحميل مجموعات المنتجات...")))
            {
                load_ProductGroups_grid();
            }
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyle(panel2, lbl_taxes_title, panel1, grid_product_groups, id);
        }

        public void load_ProductGroups_grid()
        {
            try
            {
                grid_product_groups.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_product_groups.AutoGenerateColumns = false;

                String keyword = "id,code,name,date_created";
                String table = "pos_product_groups";
                grid_product_groups.DataSource = objBLL.GetRecord(keyword, table);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
                throw;
            }

        }

        private bool TryGetSelectedGroup(out string id, out string code, out string name)
        {
            id = null;
            code = null;
            name = null;

            if (grid_product_groups.CurrentRow == null)
                return false;

            var row = grid_product_groups.CurrentRow;

            id = Convert.ToString(row.Cells["id"].Value);
            code = Convert.ToString(row.Cells["code"].Value);
            name = Convert.ToString(row.Cells["name"].Value);

            return !string.IsNullOrWhiteSpace(id);
        }

        private void btn_new_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Opening...", "جاري الفتح...")))
            {
                frm_addProductGroup frm_addProductGroup_obj = new frm_addProductGroup(this);
                frm_addProductGroup.instance.tb_lbl_is_edit.Text = "false";
                frm_addProductGroup_obj.ShowDialog(this);
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedGroup(out var id, out var code, out var name))
            {
                UiMessages.ShowWarning(
                    "Please select a product group to update.",
                    "يرجى تحديد مجموعة منتجات للتحديث.",
                    captionEn: "Product Groups",
                    captionAr: "مجموعات المنتجات");
                return;
            }

            using (BusyScope.Show(this, UiMessages.T("Opening...", "جاري الفتح...")))
            {
                frm_addProductGroup frm_addProductGroup_obj = new frm_addProductGroup(this);
                frm_addProductGroup.instance.tb_lbl_is_edit.Text = "true";

                frm_addProductGroup.instance.tb_id.Text = id;
                frm_addProductGroup.instance.tb_code.Text = code;
                frm_addProductGroup.instance.tb_name.Text = name;

                frm_addProductGroup_obj.ShowDialog(this);
            }
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (!TryGetSelectedGroup(out var id, out var code, out var name))
            {
                UiMessages.ShowWarning(
                    "Please select a product group to delete.",
                    "يرجى تحديد مجموعة منتجات للحذف.",
                    captionEn: "Product Groups",
                    captionAr: "مجموعات المنتجات");
                return;
            }

            var confirm = UiMessages.ConfirmYesNo(
                $"Delete product group '{name}'?",
                $"هل تريد حذف مجموعة المنتجات '{name}'؟",
                captionEn: "Delete",
                captionAr: "حذف");

            if (confirm != DialogResult.Yes)
                return;

            using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
            {
                try
                {
                    ProductGroupsBLL objBLL = new ProductGroupsBLL();
                    objBLL.Delete(int.Parse(id));

                    UiMessages.ShowInfo(
                        "Product group deleted successfully.",
                        "تم حذف مجموعة المنتجات بنجاح.",
                        captionEn: "Success",
                        captionAr: "نجاح");

                    load_ProductGroups_grid();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
                }
            }
        }

        private void btn_refresh_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Refreshing...", "جاري التحديث...")))
            {
                load_ProductGroups_grid();
            }
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                string condition = (txt_search.Text ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(condition))
                {
                    using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
                    {
                        load_ProductGroups_grid();
                    }
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    ProductGroupsBLL objBLL = new ProductGroupsBLL();
                    grid_product_groups.DataSource = objBLL.SearchRecord(condition);

                    if (grid_product_groups.Rows.Count == 0)
                    {
                        UiMessages.ShowInfo(
                            "No matching groups found.",
                            "لم يتم العثور على مجموعات مطابقة.",
                            captionEn: "Search",
                            captionAr: "بحث");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }

        }

        private void btn_assign_product_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Opening...", "جاري الفتح...")))
            {
                using (frm_assign_products frm = new frm_assign_products())
                {
                    frm.ShowDialog(this);
                }
            }
        }

        // Designer-wired handlers
        private void txt_search_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13) // Enter
            {
                btn_search.PerformClick();
            }
        }

        private void frm_product_groups_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.N)
                btn_new.PerformClick();

            if (e.Control && e.KeyCode == Keys.U)
                btn_update.PerformClick();

            if (e.Control && e.KeyCode == Keys.D)
                btn_delete.PerformClick();
        }
    }
}
