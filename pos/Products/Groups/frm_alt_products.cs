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
using pos.UI.Busy;

namespace pos
{
    public partial class frm_alt_products : Form
    {
        string global_product_code = "";
        int global_alt_id = 0;

        public frm_alt_products()
        {
            InitializeComponent();
        }

        public void frm_alt_products_Load(object sender, EventArgs e)
        {
            txt_source_code.Focus();
        }

        private void txt_source_code_KeyDown(object sender, KeyEventArgs e)
        {
            bool source_product = true;
            if (!string.IsNullOrWhiteSpace(txt_source_code.Text) && e.KeyData == Keys.Enter)
            {
                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    frm_searchProducts search_product_obj = new frm_searchProducts(null, null, this, txt_source_code.Text, "", "", 0, false, source_product);
                    search_product_obj.ShowDialog(this);
                }
            }
        }

        public void fill_product_txtbox(string product_id, string code, string name, int alt_no)
        {
            txt_source_code.Text = name;
            global_product_code = code;
            global_alt_id = alt_no;
            txt_item_code.Text = code;

            if (!string.IsNullOrWhiteSpace(txt_item_code.Text) && global_alt_id != 0)
            {
                using (BusyScope.Show(this, UiMessages.T("Loading alternate products...", "جاري تحميل المنتجات البديلة...")))
                {
                    Load_alternateProductsToGrid(global_alt_id);
                }
            }
        }

        public void Load_alternateProductsToGrid(int alt_no)
        {
            try
            {
                grid_product_groups.Rows.Clear();
                grid_product_groups.Refresh();

                ProductGroupsBLL objBLL = new ProductGroupsBLL();
                DataTable product_dt = objBLL.SearchAlternateProducts(alt_no);

                if (product_dt != null && product_dt.Rows.Count > 0)
                {
                    foreach (DataRow myProductView in product_dt.Rows)
                    {
                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        string alternate_no = myProductView["alternate_no"].ToString();
                        string item_number = myProductView["item_number"].ToString();

                        string[] row0 = { id.ToString(), code, name, alternate_no, item_number };
                        grid_product_groups.Rows.Add(row0);
                    }
                }
                else
                {
                    // No rows is not an error; just inform if user already selected a source product.
                    if (alt_no > 0)
                    {
                        UiMessages.ShowInfo(
                            "No alternate products found for this item.",
                            "لا توجد منتجات بديلة لهذا الصنف.",
                            captionEn: "Alternate Products",
                            captionAr: "المنتجات البديلة");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(txt_item_code.Text))
                {
                    UiMessages.ShowWarning(
                        "Please select a source product first.",
                        "يرجى اختيار المنتج الأساسي أولاً.",
                        captionEn: "Alternate Products",
                        captionAr: "المنتجات البديلة");
                    txt_source_code.Focus();
                    return;
                }

                if (grid_product_groups.Rows.Count == 0)
                {
                    UiMessages.ShowWarning(
                        "No alternate products to save. Please add products first.",
                        "لا توجد منتجات بديلة للحفظ. يرجى إضافة منتجات أولاً.",
                        captionEn: "Alternate Products",
                        captionAr: "المنتجات البديلة");
                    txt_product_code.Focus();
                    return;
                }

                DialogResult confirm = UiMessages.ConfirmYesNo(
                    "Save alternate products mapping?",
                    "هل تريد حفظ ربط المنتجات البديلة؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد");

                if (confirm != DialogResult.Yes)
                    return;

                using (BusyScope.Show(this, UiMessages.T("Saving...", "جاري الحفظ...")))
                {
                    string result_1 = string.Empty;
                    ProductGroupsModal info = new ProductGroupsModal();
                    ProductGroupsBLL objBLL = new ProductGroupsBLL();

                    int savedCount = 0;

                    for (int i = 0; i < grid_product_groups.Rows.Count; i++)
                    {
                        if (grid_product_groups.Rows[i].IsNewRow) continue;

                        if (grid_product_groups.Rows[i].Cells["code"].Value != null)
                        {
                            info.alt_no = int.Parse(grid_product_groups.Rows[i].Cells["alternate_no"].Value.ToString());
                            info.product_id = grid_product_groups.Rows[i].Cells["code"].Value.ToString();
                            info.item_number = grid_product_groups.Rows[i].Cells["item_number"].Value.ToString();
                            info.code = global_product_code;

                            result_1 = objBLL.InsertProductAlternate(info);
                            if (!string.IsNullOrEmpty(result_1))
                                savedCount++;
                        }
                    }

                    if (savedCount > 0)
                    {
                        UiMessages.ShowInfo(
                            $"Alternate products saved successfully. ({savedCount})",
                            $"تم حفظ المنتجات البديلة بنجاح. ({savedCount})",
                            captionEn: "Success",
                            captionAr: "نجاح");

                        grid_product_groups.DataSource = null;
                        grid_product_groups.Rows.Clear();
                        txt_product_code.Text = "";
                        txt_source_code.Text = "";
                        txt_item_code.Text = "";
                        global_product_code = "";
                        global_alt_id = 0;
                        txt_source_code.Focus();
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "Nothing was saved. Please verify the selected products.",
                            "لم يتم حفظ أي بيانات. يرجى التحقق من المنتجات المختارة.",
                            captionEn: "Warning",
                            captionAr: "تنبيه");
                    }
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
                if (string.IsNullOrWhiteSpace(txt_item_code.Text))
                {
                    UiMessages.ShowWarning(
                        "Please select a source product first.",
                        "يرجى اختيار المنتج الأساسي أولاً.",
                        captionEn: "Alternate Products",
                        captionAr: "المنتجات البديلة");
                    txt_source_code.Focus();
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
                {
                    frm_searchProducts search_product_obj = new frm_searchProducts(null, null, this, txt_product_code.Text, "", "", 0, false);
                    search_product_obj.ShowDialog(this);
                }
            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txt_item_code.Text))
            {
                UiMessages.ShowWarning(
                    "Please select a source product first.",
                    "يرجى اختيار المنتج الأساسي أولاً.",
                    captionEn: "Alternate Products",
                    captionAr: "المنتجات البديلة");
                txt_source_code.Focus();
                return;
            }

            using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
            {
                frm_searchProducts search_product_obj = new frm_searchProducts(null, null, this, txt_product_code.Text, "", "", 0, false);
                search_product_obj.ShowDialog(this);
            }

            txt_source_code.Focus();

        }

        public void load_products(string product_id = "")
        {
            ProductBLL productsBLL_obj = new ProductBLL();
            DataTable product_dt = new DataTable();

            if (product_id != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductNumber(product_id);
            }

            if (product_dt.Rows.Count > 0)
            {
                foreach (DataRow myProductView in product_dt.Rows)
                {
                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();
                    string item_number = myProductView["item_number"].ToString();
                    int alt_no = global_alt_id;

                    if (alt_no == 0)
                    {
                        ProductGroupsBLL objBLL = new ProductGroupsBLL();
                        int maxAltNo = objBLL.GetMaxAlternateNo();
                        alt_no = maxAltNo;
                    }

                    string[] row0 = { id.ToString(), code, name, alt_no.ToString(), item_number };
                    grid_product_groups.Rows.Add(row0);
                }

                txt_product_code.Text = "";
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

        private void frm_alt_products_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                btn_save.PerformClick();
            }
        }

        private void grid_product_groups_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (grid_product_groups.CurrentRow == null)
                    return;

                int product_id = int.Parse(grid_product_groups.CurrentRow.Cells["id"].Value.ToString());

                string colName = grid_product_groups.Columns[e.ColumnIndex].Name;
                if (colName == "btn_delete")
                {
                    var confirm = UiMessages.ConfirmYesNo(
                        "Remove this alternate product?",
                        "هل تريد حذف هذا المنتج البديل؟",
                        captionEn: "Delete",
                        captionAr: "حذف");

                    if (confirm != DialogResult.Yes)
                        return;

                    using (BusyScope.Show(this, UiMessages.T("Deleting...", "جاري الحذف...")))
                    {
                        ProductGroupsBLL objBLL = new ProductGroupsBLL();
                        objBLL.DeleteAltNo(product_id);

                        UiMessages.ShowInfo(
                            "Alternate product removed successfully.",
                            "تم حذف المنتج البديل بنجاح.",
                            captionEn: "Success",
                            captionAr: "نجاح");

                        if (global_alt_id != 0)
                            Load_alternateProductsToGrid(global_alt_id);
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void txt_source_code_KeyUp(object sender, KeyEventArgs e)
        {
            if (txt_source_code.Text == "")
            {
                txt_item_code.Text = "";
            }


        }

    }
}
