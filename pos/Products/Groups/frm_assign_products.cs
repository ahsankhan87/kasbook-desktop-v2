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
using POS.Core;
using pos.UI;

namespace pos
{
    public partial class frm_assign_products : Form
    {

        public frm_assign_products()
        {
            InitializeComponent();
        }


        public void frm_assign_products_Load(object sender, EventArgs e)
        {
            get_groups_dropdownlist();
        }

        public void load_ProductGroups_grid(string product_id, int RowIndex)
        {
            try
            {
                ProductBLL productsBLL_obj = new ProductBLL();
                DataTable product_dt = new DataTable();

                product_dt = productsBLL_obj.SearchRecordByProductID(product_id);
                if (product_dt.Rows.Count > 0)
                {
                    foreach (DataRow myProductView in product_dt.Rows)
                    {
                        grid_product_groups.Rows[RowIndex].Cells["id"].Value = myProductView["id"].ToString();
                        grid_product_groups.Rows[RowIndex].Cells["code"].Value = myProductView["code"].ToString();
                        grid_product_groups.Rows[RowIndex].Cells["name"].Value = myProductView["name"].ToString();
                    }
                }
                else
                {
                    UiMessages.ShowWarning(
                        "No matching product was found.",
                        "لم يتم العثور على المنتج المطلوب.",
                        captionEn: "Products",
                        captionAr: "المنتجات");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
                throw;
            }

        }


        public void get_groups_dropdownlist()
        {
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "code,name";
            string table = "pos_product_groups";

            DataTable groups = generalBLL_obj.GetRecord(keyword, table);
            DataRow emptyRow = groups.NewRow();
            emptyRow[0] = "";              // Set Column Value
            emptyRow[1] = "Please Select";              // Set Column Value
            groups.Rows.InsertAt(emptyRow, 0);

            //DataRow emptyRow1 = groups.NewRow();
            //emptyRow1[0] = "-1";              // Set Column Value
            //emptyRow1[1] = "ADD NEW";              // Set Column Value
            //groups.Rows.InsertAt(emptyRow1, 1);

            cmb_product_groups.DisplayMember = "name";
            cmb_product_groups.ValueMember = "code";
            cmb_product_groups.DataSource = groups;

        }

        private void cmb_product_groups_SelectedIndexChanged(object sender, EventArgs e)
        {
            txt_group_code.Text = cmb_product_groups.SelectedValue.ToString();
            
            if(txt_group_code.Text != "")
            {
                Load_grid(txt_group_code.Text);
            }
            
        }

        public void Load_grid(string group_code)
        {
            grid_product_groups.Rows.Clear();
            grid_product_groups.Refresh();

            ProductGroupsBLL objBLL = new ProductGroupsBLL();
            DataTable product_dt = new DataTable();
            product_dt = objBLL.SearchRecord(group_code);
            
            if (product_dt.Rows.Count > 0)
            {
                foreach (DataRow myProductView in product_dt.Rows)
                {

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();

                    string[] row0 = { id.ToString(), code, name };

                    grid_product_groups.Rows.Add(row0);

                }


            }
        }

        private void grid_product_groups_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_group_code.Text))
                {
                    UiMessages.ShowWarning(
                        "Please select a product group first.",
                        "يرجى اختيار مجموعة المنتجات أولاً.",
                        captionEn: "Product Groups",
                        captionAr: "مجموعات المنتجات");
                    cmb_product_groups.Focus();
                    return;
                }

                if (grid_product_groups.Rows.Count == 0)
                {
                    UiMessages.ShowWarning(
                        "No products to assign. Please add products first.",
                        "لا توجد منتجات للإسناد. يرجى إضافة منتجات أولاً.",
                        captionEn: "Product Groups",
                        captionAr: "مجموعات المنتجات");
                    txt_product_code.Focus();
                    return;
                }

                var confirm = UiMessages.ConfirmYesNo(
                    "Assign the listed products to this group?",
                    "هل تريد إسناد المنتجات المدرجة لهذه المجموعة؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد");

                if (confirm != DialogResult.Yes)
                    return;

                string result = string.Empty;
                ProductGroupsModal info = new ProductGroupsModal();
                ProductGroupsBLL objBLL = new ProductGroupsBLL();

                int savedCount = 0;

                for (int i = 0; i < grid_product_groups.Rows.Count; i++)
                {
                    if (grid_product_groups.Rows[i].IsNewRow) continue;

                    var codeCell = grid_product_groups.Rows[i].Cells["code"].Value;
                    if (codeCell == null) continue;

                    info.group_code = txt_group_code.Text;
                    info.product_id = codeCell.ToString();

                    result = objBLL.InsertProductGroupDetail(info);
                    if (!string.IsNullOrEmpty(result))
                        savedCount++;
                }

                if (savedCount > 0)
                {
                    UiMessages.ShowInfo(
                        $"Products assigned successfully. ({savedCount})",
                        $"تم إسناد المنتجات بنجاح. ({savedCount})",
                        captionEn: "Success",
                        captionAr: "نجاح");

                    // Refresh grid from DB
                    Load_grid(txt_group_code.Text);
                }
                else
                {
                    UiMessages.ShowWarning(
                        "Nothing was saved. Please verify selected products.",
                        "لم يتم حفظ أي بيانات. يرجى التحقق من المنتجات المختارة.",
                        captionEn: "Warning",
                        captionAr: "تنبيه");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txt_product_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (string.IsNullOrWhiteSpace(txt_group_code.Text))
                {
                    UiMessages.ShowWarning(
                        "Please select a group before adding products.",
                        "يرجى اختيار المجموعة قبل إضافة المنتجات.",
                        captionEn: "Product Groups",
                        captionAr: "مجموعات المنتجات");
                    cmb_product_groups.Focus();
                    return;
                }

                frm_searchProducts search_product_obj = new frm_searchProducts(null, this, null, txt_product_code.Text, "", "", 0, false);
                search_product_obj.ShowDialog();
            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_group_code.Text))
            {
                UiMessages.ShowWarning(
                    "Please select a group before adding products.",
                    "يرجى اختيار المجموعة قبل إضافة المنتجات.",
                    captionEn: "Product Groups",
                    captionAr: "مجموعات المنتجات");
                cmb_product_groups.Focus();
                return;
            }

            frm_searchProducts search_product_obj = new frm_searchProducts(null, this, null, txt_product_code.Text, "", "", 0, false);
            search_product_obj.ShowDialog();

        }

        public void load_products(string product_id = "")
        {
            ProductBLL productsBLL_obj = new ProductBLL();
            DataTable product_dt = new DataTable();

            if (product_id != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductID(product_id);
            }

            if (product_dt.Rows.Count > 0)
            {
                foreach (DataRow myProductView in product_dt.Rows)
                {
                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();

                    string[] row0 = { id.ToString(), code, name };
                    grid_product_groups.Rows.Add(row0);
                }
            }
            else
            {
                UiMessages.ShowWarning(
                    "No matching product was found.",
                    "لم يتم العثور على المنتج المطلوب.");

            }
        }

        private void frm_assign_products_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyData == Keys.F3) {
                btn_save.PerformClick();
            }
            if(e.KeyData == Keys.Escape) {
                btn_cancel.PerformClick();
            }

        }

        // ...existing code...
    }
}
