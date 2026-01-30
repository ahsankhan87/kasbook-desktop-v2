using POS.BLL;
using POS.Core;
using System;
using System.Data;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_product_adjustment : Form
    {

        public int inventory_acc_id = 0;
        public int item_variance_acc_id = 0;

        public frm_product_adjustment()
        {
            InitializeComponent();

        }

        private void frm_product_adjustment_Load(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Loading...", "جاري التحميل...")))
            {
                GetMAXInvoiceNo();
                Get_AccountID_From_Company();
            }
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Saving adjustment...", "جاري حفظ التسوية...")))
            {
                try
                {
                    if (grid_search_products.Rows.Count <= 0)
                    {
                        UiMessages.ShowWarning(
                            "No products found. Please add products first.",
                            "لا توجد أصناف. الرجاء إضافة أصناف أولاً.",
                            captionEn: "Adjustment",
                            captionAr: "تسوية");
                        return;
                    }

                    var confirm = UiMessages.ConfirmYesNo(
                        "Are you sure you want to save this adjustment?",
                        "هل أنت متأكد أنك تريد حفظ هذه التسوية؟",
                        captionEn: "Confirm",
                        captionAr: "تأكيد");

                    if (confirm != DialogResult.Yes)
                        return;

                    if (inventory_acc_id <= 0 || item_variance_acc_id <= 0)
                    {
                        UiMessages.ShowError(
                            "Accounts are not configured (Inventory / Item Variance). Please check Company settings.",
                            "الحسابات غير مهيأة (المخزون / فرق الصنف). يرجى التحقق من إعدادات الشركة.",
                            captionEn: "Adjustment",
                            captionAr: "تسوية");
                        return;
                    }

                    var productBLLObj = new ProductBLL();

                    // Use current ref number if present; otherwise generate a new one.
                    string invoice_no = (txt_ref_no.Text ?? string.Empty).Trim();
                    if (string.IsNullOrWhiteSpace(invoice_no))
                        invoice_no = productBLLObj.GetMaxAdjustmentInvoiceNo();

                    ProductModal info = new ProductModal();

                    bool anyRowUpdated = false;
                    int updatedLines = 0;
                    int parsedLines = 0;

                    for (int i = 0; i < grid_search_products.Rows.Count; i++)
                    {
                        var row = grid_search_products.Rows[i];
                        if (row == null || row.Cells["id"] == null || row.Cells["id"].Value == null)
                            continue;

                        parsedLines++;

                        double adjustment_qty = ParseDoubleCell(row, "adjustment_qty");
                        double qty = ParseDoubleCell(row, "qty");
                        double avg_cost = ParseDoubleCell(row, "avg_cost");
                        double unit_price = ParseDoubleCell(row, "unit_price");

                        if (avg_cost < 0 || unit_price < 0)
                        {
                            UiMessages.ShowWarning(
                                "Invalid prices detected in grid. Please correct and try again.",
                                "تم اكتشاف أسعار غير صحيحة في الجدول. يرجى التصحيح والمحاولة مرة أخرى.",
                                captionEn: "Adjustment",
                                captionAr: "تسوية");
                            return;
                        }

                        info.invoice_no = invoice_no;
                        info.item_number = Convert.ToString(row.Cells["item_number"].Value);
                        info.code = Convert.ToString(row.Cells["code"].Value);
                        info.id = Convert.ToInt32(row.Cells["id"].Value);
                        info.cost_price = avg_cost;
                        info.unit_price = unit_price;
                        info.location_code = Convert.ToString(row.Cells["location_code"].Value);
                        info.qty = qty;
                        info.adjustment_qty = adjustment_qty;

                        var qresult = productBLLObj.UpdateQtyAdjustment(info);
                        if (!string.IsNullOrWhiteSpace(qresult))
                        {
                            anyRowUpdated = true;
                            updatedLines++;
                        }

                        // Account Entry
                        double net_qty = (adjustment_qty - qty);
                        double total = avg_cost * net_qty;

                        if (net_qty > 0)
                        {
                            if (total > 0)
                            {
                                // Product Adjustment JOURNAL ENTRY (credit)
                                Insert_Journal_entry(invoice_no, item_variance_acc_id, 0, total, txt_date.Value.Date, "Product Adjustment", 0, 0, 0);
                                // Inventory JOURNAL ENTRY (debit)
                                Insert_Journal_entry(invoice_no, inventory_acc_id, total, 0, txt_date.Value.Date, "Product Adjustment", 0, 0, 0);
                            }

                        }
                        else
                        {
                            if (Math.Abs(total) > 0)
                            {
                                // Product Adjustment JOURNAL ENTRY (debit)
                                Insert_Journal_entry(invoice_no, item_variance_acc_id, Math.Abs(total), 0, txt_date.Value.Date, "Product Adjustment", 0, 0, 0);
                                // Inventory JOURNAL ENTRY (credit)
                                Insert_Journal_entry(invoice_no, inventory_acc_id, 0, Math.Abs(total), txt_date.Value.Date, "Product Adjustment", 0, 0, 0);

                            }
                        }

                    }

                    if (!anyRowUpdated)
                    {
                        UiMessages.ShowError(
                            "No changes were saved. Please verify the adjustment quantities and try again.",
                            "لم يتم حفظ أي تغييرات. يرجى التحقق من كميات التسوية والمحاولة مرة أخرى.",
                            captionEn: "Adjustment",
                            captionAr: "تسوية");
                        return;
                    }

                    UiMessages.ShowInfo(
                        $"Adjustment saved successfully. Ref: {invoice_no} (Lines: {updatedLines}/{parsedLines})",
                        $"تم حفظ التسوية بنجاح. المرجع: {invoice_no} (الأسطر: {updatedLines}/{parsedLines})",
                        captionEn: "Success",
                        captionAr: "نجاح");

                    // After save: reset UI for a new adjustment
                    txt_ref_no.Text = productBLLObj.GetMaxAdjustmentInvoiceNo();
                    grid_search_products.DataSource = null;
                    grid_search_products.Rows.Clear();
                    grid_search_products.Refresh();
                    txt_search.Focus();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        private static double ParseDoubleCell(DataGridViewRow row, string columnName)
        {
            try
            {
                if (row.Cells[columnName] == null || row.Cells[columnName].Value == null)
                    return 0;

                var s = Convert.ToString(row.Cells[columnName].Value);
                if (string.IsNullOrWhiteSpace(s))
                    return 0;

                return Convert.ToDouble(s);
            }
            catch
            {
                return 0;
            }
        }

        private void GetMAXInvoiceNo()
        {
            //ProductBLL objBLL = new ProductBLL();
            //txt_ref_no.Text = objBLL.GetMaxAdjustmentInvoiceNo();
            SalesBLL objSales = new SalesBLL();
            txt_ref_no.Text = objSales.GenerateAdjustmentInvoiceNo();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Searching...", "جاري البحث...")))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txt_search.Text))
                    {
                        UiMessages.ShowWarning(
                            "Please enter item code/name to search.",
                            "يرجى إدخال كود/اسم الصنف للبحث.",
                            captionEn: "Search",
                            captionAr: "بحث");
                        txt_search.Focus();
                        return;
                    }

                    bool by_code = rb_by_code.Checked;
                    bool by_name = rb_by_name.Checked;

                    frm_searchProducts search_product_obj = new frm_searchProducts(null, null, null, txt_search.Text, "", "", 0, false, false, null, null, this);
                    search_product_obj.ShowDialog();

                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                }
            }
        }

        public void Load_product_to_grid(string product_code = "")
        {
            using (BusyScope.Show(this, UiMessages.T("Loading product...", "جاري تحميل الصنف...")))
            {
                try
                {
                    ProductBLL productsBLL_obj = new ProductBLL();
                    DataTable product_dt = new DataTable();

                    if (!string.IsNullOrWhiteSpace(product_code))
                    {
                        product_dt = productsBLL_obj.SearchRecordByProductNumber(product_code);
                    }

                    if (product_dt.Rows.Count > 0)
                    {
                        foreach (DataRow myProductView in product_dt.Rows)
                        {
                            int id = Convert.ToInt32(myProductView["id"]);
                            string code = Convert.ToString(myProductView["code"]);
                            string category = Convert.ToString(myProductView["category"]);
                            string name = Convert.ToString(myProductView["name"]);
                            string name_ar = Convert.ToString(myProductView["name_ar"]);
                            string location_code = Convert.ToString(myProductView["location_code"]);
                            decimal qty = Math.Round(Convert.ToDecimal(myProductView["qty"]), 2);
                            decimal avg_cost = Math.Round(Convert.ToDecimal(myProductView["avg_cost"]), 2);
                            decimal unit_price = Math.Round(Convert.ToDecimal(myProductView["unit_price"]), 2);
                            string description = Convert.ToString(myProductView["description"]);
                            string item_type = Convert.ToString(myProductView["item_type"]);
                            string btn_delete = "Del";
                            string item_number = Convert.ToString(myProductView["item_number"]);

                            // Show qty dialog per product; default to current qty
                            decimal enteredQty = qty;
                            using (var qtyDlg = new pos.Products.Adjustment.frm_adjust_qty(qty))
                            {
                                if (qtyDlg.ShowDialog(this) == DialogResult.OK)
                                {
                                    enteredQty = qtyDlg.EnteredQty; // this is a decimal
                                }
                                else
                                {
                                    // If cancelled, keep default (current qty)
                                    enteredQty = qty;
                                }
                            }

                            string[] row0 =
                            {
                                id.ToString(), code, category, name, name_ar, location_code,
                                qty.ToString("N2"),
                                enteredQty.ToString("N2"), // adjustment_qty (from dialog)
                                avg_cost.ToString("N2"), unit_price.ToString("N2"),
                                btn_delete, description, item_type, item_number
                            };

                            grid_search_products.Rows.Add(row0);
                        }
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "No product found.",
                            "لم يتم العثور على الصنف.",
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

        private int Insert_Journal_entry(string invoice_no, int account_id, double debit, double credit, DateTime date,
           string description, int customer_id, int supplier_id, int entry_id)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            JournalsBLL JournalsObj = new JournalsBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.customer_id = customer_id;
            JournalsModal_obj.supplier_id = supplier_id;
            JournalsModal_obj.entry_id = entry_id;

            journal_id = JournalsObj.Insert(JournalsModal_obj);
            return journal_id;
        }

        private void Get_AccountID_From_Company()
        {
            GeneralBLL objBLL = new GeneralBLL();

            String keyword = "TOP 1 *";
            String table = "pos_companies";
            DataTable companies_dt = objBLL.GetRecord(keyword, table);
            foreach (DataRow dr in companies_dt.Rows)
            {
                item_variance_acc_id = (int)dr["item_variance_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frm_product_adjustment_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F3)
                {
                    btn_update.PerformClick();
                }
                // Print Ctrl + P
                if (e.Control && e.KeyCode == Keys.P)
                {
                    btn_print.PerformClick();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
            }
        }

        private void txt_search_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_search.Text != "" && e.KeyData == Keys.Enter)
            {

                btn_search.PerformClick();
            }
        }

        private void grid_search_products_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_search_products.Columns[e.ColumnIndex].Name;
                if (name == "btn_delete")
                {
                    grid_search_products.Rows.RemoveAt(e.RowIndex);

                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Products", captionAr: "الأصناف");

            }
        }

        private void txt_ref_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            // When press enter search product adjustment
            if (txt_ref_no.Text != "" && e.KeyChar == (char)Keys.Enter)
            {
                using (BusyScope.Show(this, UiMessages.T("Loading adjustment...", "جاري تحميل التسوية...")))
                {
                    try
                    {
                        // validation
                        if (txt_ref_no.Text.Trim().Length == 0)
                        {
                            UiMessages.ShowWarning(
                                "Please enter a valid reference number.",
                                "يرجى إدخال رقم مرجع صحيح.",
                                captionEn: "Adjustment",
                                captionAr: "تسوية");
                            return;
                        }
                        // select adjustment from table and fill grid
                        ProductBLL _productBll = new ProductBLL();
                        DataTable dt = _productBll.GetProductAdjustmentsByInvoiceNo(txt_ref_no.Text);

                        if (dt.Rows.Count > 0)
                        {
                            grid_search_products.Rows.Clear();
                            foreach (DataRow myProductView in dt.Rows)
                            {
                                int id = Convert.ToInt32(myProductView["id"]);
                                string code = Convert.ToString(myProductView["item_code"]);
                                string category = Convert.ToString(myProductView["category_code"]);
                                string name = Convert.ToString(myProductView["name"]);
                                string name_ar = Convert.ToString(myProductView["name_ar"]);
                                string location_code = Convert.ToString(myProductView["location_code"]);
                                decimal qty = Math.Round(Convert.ToDecimal(myProductView["qty"]), 2);
                                decimal adjustment_qty = Math.Round(Convert.ToDecimal(myProductView["adjustment_qty"]), 2);
                                decimal avg_cost = Math.Round(Convert.ToDecimal(myProductView["cost_price"]), 2);
                                decimal unit_price = Math.Round(Convert.ToDecimal(myProductView["unit_price"]), 2);
                                string description = Convert.ToString(myProductView["description"]);
                                string item_type = Convert.ToString(myProductView["item_type"]);
                                string btn_delete = "Del";
                                string item_number = Convert.ToString(myProductView["item_number"]);
                                string[] row0 =
                                {
                                    id.ToString(), code, category, name, name_ar, location_code,
                                    qty.ToString("N2"),
                                    adjustment_qty.ToString("N2"), // adjustment_qty
                                    avg_cost.ToString("N2"), unit_price.ToString("N2"),
                                    btn_delete, description, item_type, item_number
                                };
                                grid_search_products.Rows.Add(row0);
                            }
                        }
                        else
                        {
                            UiMessages.ShowWarning(
                                "No record found for this reference number.",
                                "لا توجد بيانات لهذا الرقم المرجعي.",
                                captionEn: "Adjustment",
                                captionAr: "تسوية");
                        }
                    }
                    catch (Exception ex)
                    {
                        UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "خطأ");
                    }
                }
            }
        }

        private void Btn_clear_Click(object sender, EventArgs e)
        {
            // Clear all fields
            txt_ref_no.Clear();
            grid_search_products.DataSource = null;
            grid_search_products.Rows.Clear();
            grid_search_products.Refresh();

            UiMessages.ShowInfo(
                "Cleared.",
                "تم المسح.",
                captionEn: "Adjustment",
                captionAr: "تسوية");

        }
    }
}
