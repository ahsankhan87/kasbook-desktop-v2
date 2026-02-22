using System;
using System.Data;
using System.Windows.Forms;
using POS.BLL;
using POS.Core;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_products_labels : Form
    {
        int global_product_id = 0;
        int global_alt_id = 0;

        public frm_products_labels()
        {
            InitializeComponent();
        }

        public void frm_products_labels_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            txt_product_code.Focus();

            using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
            {
                autoCompleteInvoice();
            }
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(null, null, panel1, grid_product_groups, id);
        }

        private void btn_save_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Preparing label report...", "جاري تجهيز تقرير الملصقات...")))
            {
                try
                {
                    if (grid_product_groups.Rows.Count <= 0)
                    {
                        UiMessages.ShowWarning(
                            "No products selected. Please add products first.",
                            "لا توجد أصناف مختارة. يرجى إضافة الأصناف أولاً.",
                            captionEn: "Labels",
                            captionAr: "الملصقات");
                        return;
                    }

                    // Convert grid to DataTable and send it to report
                    DataTable dt = new DataTable();
                    foreach (DataGridViewColumn col in grid_product_groups.Columns)
                    {
                        dt.Columns.Add(col.Name);
                    }

                    foreach (DataGridViewRow row in grid_product_groups.Rows)
                    {
                        if (row.IsNewRow) continue;

                        DataRow dRow = dt.NewRow();
                        foreach (DataGridViewCell cell in row.Cells)
                        {
                            dRow[cell.ColumnIndex] = cell.Value;
                        }
                        dt.Rows.Add(dRow);
                    }

                    if (dt.Rows.Count <= 0)
                    {
                        UiMessages.ShowWarning(
                            "No rows to print.",
                            "لا توجد صفوف للطباعة.",
                            captionEn: "Labels",
                            captionAr: "الملصقات");
                        return;
                    }

                    using (frm_ProductLabelReport obj = new frm_ProductLabelReport(dt))
                    {
                        obj.ShowDialog();
                    }

                    grid_product_groups.Rows.Clear();
                    grid_product_groups.Refresh();

                    UiMessages.ShowInfo(
                        "Label report prepared successfully.",
                        "تم تجهيز تقرير الملصقات بنجاح.",
                        captionEn: "Success",
                        captionAr: "نجاح");
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txt_product_code_KeyDown(object sender, KeyEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_product_code.Text) && e.KeyData == Keys.Enter)
            {
                using (BusyScope.Show(this, UiMessages.T("Searching products...", "جاري البحث عن الأصناف...")))
                {
                    frm_searchProducts search_product_obj = new frm_searchProducts(null, null, null, txt_product_code.Text, "", "", 0, false, false, this);
                    search_product_obj.ShowDialog();
                }
            }
        }

        public void load_products(string product_id = "")
        {
            using (BusyScope.Show(this, UiMessages.T("Loading product...", "جاري تحميل الصنف...")))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(product_id))
                    {
                        UiMessages.ShowWarning(
                            "Please select a product.",
                            "يرجى اختيار صنف.",
                            captionEn: "Products",
                            captionAr: "الأصناف");
                        return;
                    }

                    ProductBLL productsBLL_obj = new ProductBLL();
                    DataTable product_dt = productsBLL_obj.SearchRecordByProductID(product_id);

                    if (product_dt.Rows.Count > 0)
                    {
                        foreach (DataRow myProductView in product_dt.Rows)
                        {
                            int id = Convert.ToInt32(myProductView["id"]);
                            string code = myProductView["code"].ToString();
                            string category = myProductView["category"].ToString();
                            string name = myProductView["name"].ToString();
                            string qty = myProductView["qty"].ToString();
                            string unit_price = myProductView["unit_price"].ToString();
                            string barcode = myProductView["barcode"].ToString();
                            string location_code = myProductView["location_code"].ToString();
                            decimal label_qty = 1;
                            
                            // Show qty dialog per product; default to current qty

                            decimal enteredQty = label_qty;
                            using (var qtyDlg = new pos.Products.Adjustment.frm_adjust_qty(label_qty))
                            {
                                if (qtyDlg.ShowDialog(this) == DialogResult.OK)
                                {
                                    enteredQty = qtyDlg.EnteredQty; // this is a decimal
                                }
                                else
                                {
                                    // If cancelled, keep default (current qty)
                                    enteredQty = label_qty;
                                }
                            }

                            string[] row0 = { id.ToString(), code,barcode, category, name, qty, unit_price, location_code,enteredQty.ToString("N2")};
                            grid_product_groups.Rows.Add(row0);
                        }
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "Record not found.",
                            "لم يتم العثور على سجل.",
                            captionEn: "Products",
                            captionAr: "الأصناف");

                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        private void frm_products_labels_KeyDown(object sender, KeyEventArgs e)
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
                string colName = grid_product_groups.Columns[e.ColumnIndex].Name;
                if (colName == "btn_delete")
                {
                    var confirm = UiMessages.ConfirmYesNo(
                        "Are you sure you want to remove this row?",
                        "هل أنت متأكد أنك تريد حذف هذا السطر؟",
                        captionEn: "Delete",
                        captionAr: "حذف");

                    if (confirm == DialogResult.Yes)
                    {
                        if (grid_product_groups.CurrentRow != null && !grid_product_groups.CurrentRow.IsNewRow)
                            grid_product_groups.Rows.Remove(grid_product_groups.CurrentRow);

                        UiMessages.ShowInfo(
                            "Row removed.",
                            "تم حذف السطر.",
                            captionEn: "Delete",
                            captionAr: "حذف");
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txt_product_code.Text))
            {
                using (BusyScope.Show(this, UiMessages.T("Searching products...", "جاري البحث عن الأصناف...")))
                {
                    frm_searchProducts search_product_obj = new frm_searchProducts(null, null, null, txt_product_code.Text, "", "", 0, false, false, this);
                    search_product_obj.ShowDialog();
                }
            }
            else
            {
                UiMessages.ShowWarning(
                    "Please enter product code to search.",
                    "يرجى إدخال كود الصنف للبحث.",
                    captionEn: "Products",
                    captionAr: "الأصناف");
            }

        }

        public void Load_products_to_grid_by_invoiceno(DataTable dt)
        {
            try
            {
                grid_product_groups.Rows.Clear();
                grid_product_groups.Refresh();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow myProductView in dt.Rows)
                    {
                        string id = myProductView["id"].ToString();
                        string code = myProductView["code"].ToString();
                        string category = myProductView["category"].ToString();
                        string name = myProductView["name"].ToString();
                        string qty = myProductView["quantity"].ToString();
                        string unit_price = myProductView["unit_price"].ToString();
                        string barcode = myProductView["barcode"].ToString();
                        string location_code = myProductView["location_code"].ToString();
                        decimal label_qty = 1;
                        
                        string[] row0 = { id, code,barcode, category, name, qty, unit_price, location_code, label_qty.ToString("N2") };
                        grid_product_groups.Rows.Add(row0);
                    }
                }
                else
                {
                    UiMessages.ShowInfo(
                        "No products found for this invoice.",
                        "لا توجد أصناف لهذه الفاتورة.",
                        captionEn: "Invoice",
                        captionAr: "فاتورة");
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }

        private void txt_purchase_inv_no_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_purchase_inv_no.Text != "" && e.KeyData == Keys.Enter)
            {
                btn_invoice_search.PerformClick();
            }
        }

        private void btn_invoice_search_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Loading invoice...", "جاري تحميل الفاتورة...")))
            {
                try
                {
                    string invoice_no = (txt_purchase_inv_no.Text ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(invoice_no))
                    {
                        UiMessages.ShowWarning(
                            "Please enter an invoice number.",
                            "يرجى إدخال رقم الفاتورة.",
                            captionEn: "Invoice",
                            captionAr: "فاتورة");
                        return;
                    }

                    PurchasesBLL purchasesObj = new PurchasesBLL();
                    var dt = purchasesObj.GetAllPurchaseByInvoice(invoice_no);

                    Load_products_to_grid_by_invoiceno(dt);
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        public void autoCompleteInvoice()
        {
            try
            {
                txt_purchase_inv_no.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                txt_purchase_inv_no.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection coll = new AutoCompleteStringCollection();

                GeneralBLL invoicesBLL_obj = new GeneralBLL();
                string keyword = "TOP 500 invoice_no ";
                string table = "pos_purchases WHERE account <> 'return' AND branch_id=" + UsersModal.logged_in_branch_id + " ORDER BY id desc";
                DataTable dt = invoicesBLL_obj.GetRecord(keyword, table);

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        coll.Add(dr["invoice_no"].ToString());

                    }

                }

                txt_purchase_inv_no.AutoCompleteCustomSource = coll;
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }

        }

    }
}
