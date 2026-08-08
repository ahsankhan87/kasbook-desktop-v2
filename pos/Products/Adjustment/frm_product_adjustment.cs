using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;
using pos.Reports.Common;

namespace pos
{
    public partial class frm_product_adjustment : Form
    {

        public int inventory_acc_id = 0;
        public int item_variance_acc_id = 0;
        public int SelectedAdjustmentAccountId { get; private set; }
        private readonly Dictionary<string, int> _accountNameToId = new Dictionary<string, int>();

        private readonly List<DeletedProductAdjustment> _deletedProducts = new List<DeletedProductAdjustment>();

        private sealed class ProductAdjustmentImportRow
        {
            public string ProductCode { get; set; }
            public string ProductName { get; set; }
            public decimal? ExactQty { get; set; }
            public decimal? CostPrice { get; set; }
            public string LocationCode { get; set; }
        }

        private sealed class AdjustmentRowMeta
        {
            public decimal OriginalQty { get; set; }
            public decimal OriginalAvgCost { get; set; }
            public int AdjustmentAccountId { get; set; }
        }

        private sealed class DeletedProductAdjustment
        {
            public int ProductId { get; set; }
            public string ProductCode { get; set; }
            public decimal Qty { get; set; }
            public decimal CostPrice { get; set; }
            public int AdjustmentAccountId { get; set; }
        }


        public frm_product_adjustment()
        {
            InitializeComponent();

        }

        private void frm_product_adjustment_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleForm();
            using (BusyScope.Show(this, UiMessages.T("Loading...", "Ã«—Ì «· Õ„Ì·...")))
            {
                txt_ref_no.Text = GetMAXInvoiceNo();
                Get_AccountID_From_Settings();
                LoadInventoryAdjustmentAccounts();
                ConfigureGridEditability();
            }
        }

        private void StyleForm()
        {
            AppTheme.ApplyListFormStyleLightHeader(null, null, panel1, grid_search_products, id);
        }

        private void btn_update_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Saving adjustment...", "Ã«—Ì Õ›Ÿ «· ”ÊÌ…...")))
            {
                try
                {
                    SelectedAdjustmentAccountId = ResolveSelectedAdjustmentAccountId();

                    if (grid_search_products.Rows.Count <= 0 && _deletedProducts.Count == 0)
                    {
                        UiMessages.ShowWarning(
                            "No products found. Please add products first.",
                            "·«  ÊÃœ √’‰«›. «·—Ã«¡ ≈÷«›… √’‰«› √Ê·«.",
                            captionEn: "Adjustment",
                            captionAr: " ”ÊÌ…");
                        return;
                    }

                    var confirm = UiMessages.ConfirmYesNo(
                        "Are you sure you want to save this adjustment?",
                        "Â· √‰  „ √ﬂœ √‰ﬂ  —Ìœ Õ›Ÿ Â–Â «· ”ÊÌ…ø",
                        captionEn: "Confirm",
                        captionAr: " √ﬂÌœ");

                    if (confirm != DialogResult.Yes)
                        return;

                    if (inventory_acc_id <= 0)
                    {
                        UiMessages.ShowError(
                            "Default inventory account is not configured. Please check Accounting Settings.",
                            "Õ”«» «·„Œ“Ê‰ «·«› —«÷Ì €Ì— „ÂÌ√. Ì—ÃÏ «· Õﬁﬁ „‰ ≈⁄œ«œ«  «·„Õ«”»….",
                            captionEn: "Adjustment",
                            captionAr: " ”ÊÌ…");
                        return;
                    }

                    var productBLLObj = new ProductBLL();

                    // generate a new invoice number.
                    string invoice_no = GetMAXInvoiceNo();

                    ProductModal info = new ProductModal();

                    bool anyRowUpdated = false;
                    int updatedLines = 0;
                    int parsedLines = 0;
                    var autoJournalLines = new List<JVLineModel>();

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

                        var rowMeta = row.Tag as AdjustmentRowMeta;
                        decimal originalQty = rowMeta != null ? rowMeta.OriginalQty : Convert.ToDecimal(qty);
                        decimal originalAvgCost = rowMeta != null ? rowMeta.OriginalAvgCost : Convert.ToDecimal(avg_cost);
                        int adjustmentAccountId = SelectedAdjustmentAccountId > 0
                            ? SelectedAdjustmentAccountId
                            : item_variance_acc_id;

                        if (avg_cost < 0 || unit_price < 0)
                        {
                            UiMessages.ShowWarning(
                                "Invalid prices detected in grid. Please correct and try again.",
                                " „ «ﬂ ‘«› √”⁄«— €Ì— ’ÕÌÕ… ›Ì «·ÃœÊ·. Ì—ÃÏ «· ’ÕÌÕ Ê«·„Õ«Ê·… „—… √Œ—Ï.",
                                captionEn: "Adjustment",
                                captionAr: " ”ÊÌ…");
                            return;
                        }

                        // Validate adjustment quantity: it should not be negative or zero (unless no change)
                        //string validationMessage;
                        //if (!ValidateAdjustmentQty(row, out validationMessage))
                        //{
                        //    UiMessages.ShowWarning(
                        //        validationMessage,
                        //        validationMessage,
                        //        captionEn: "Adjustment",
                        //        captionAr: " ”ÊÌ…");
                        //    return;
                        //}

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

                        decimal newQty = Convert.ToDecimal(adjustment_qty);
                        decimal newAvgCost = Convert.ToDecimal(avg_cost);
                        decimal oldInventoryValue = Math.Round(originalQty * originalAvgCost, 2);
                        decimal newInventoryValue = Math.Round(newQty * newAvgCost, 2);
                        decimal inventoryImpact = Math.Round(newInventoryValue - oldInventoryValue, 2);

                        if (inventoryImpact != 0m && adjustmentAccountId <= 0)
                        {
                            UiMessages.ShowError(
                                "Inventory adjustment account is not selected/configured for one or more rows.",
                                "Õ”«»  ”ÊÌ… «·„Œ“Ê‰ €Ì— „Õœœ/€Ì— „ÂÌ√ ·Ê«Õœ √Ê √ﬂÀ— „‰ «·√”ÿ—.",
                                captionEn: "Adjustment",
                                captionAr: " ”ÊÌ…");
                            return;
                        }

                        string lineNarration = string.Format(
                            "Product Adjustment: {0} (Qty {1:N2}->{2:N2}, Cost {3:N2}->{4:N2})",
                            info.code,
                            originalQty,
                            newQty,
                            originalAvgCost,
                            newAvgCost);

                        AddAutoJournalLinesForImpact(autoJournalLines, inventoryImpact, adjustmentAccountId, lineNarration);

                    }

                    for (int i = 0; i < _deletedProducts.Count; i++)
                    {
                        var deleted = _deletedProducts[i];
                        if (deleted == null || deleted.ProductId <= 0)
                            continue;

                        decimal oldInventoryValue = Math.Round(deleted.Qty * deleted.CostPrice, 2);
                        decimal inventoryImpact = Math.Round(0m - oldInventoryValue, 2);
                        int adjustmentAccountId = deleted.AdjustmentAccountId > 0
                            ? deleted.AdjustmentAccountId
                            : (SelectedAdjustmentAccountId > 0 ? SelectedAdjustmentAccountId : item_variance_acc_id);

                        if (inventoryImpact != 0m && adjustmentAccountId <= 0)
                        {
                            UiMessages.ShowError(
                                "Inventory adjustment account is missing for a deleted product.",
                                "Õ”«»  ”ÊÌ… «·„Œ“Ê‰ „›ﬁÊœ ·„‰ Ã „Õ–Ê›.",
                                captionEn: "Adjustment",
                                captionAr: " ”ÊÌ…");
                            return;
                        }

                        string deleteNarration = string.Format(
                            "Product Deleted: {0} (Qty {1:N2}, Cost {2:N2})",
                            deleted.ProductCode,
                            deleted.Qty,
                            deleted.CostPrice);

                        AddAutoJournalLinesForImpact(autoJournalLines, inventoryImpact, adjustmentAccountId, deleteNarration);
                    }

                    bool anyDeleted = _deletedProducts.Count > 0;

                    if (!anyRowUpdated && !anyDeleted)
                    {
                        UiMessages.ShowError(
                            "No changes were saved. Please verify the adjustment quantities and try again.",
                            "·„ Ì „ Õ›Ÿ √Ì  €ÌÌ—« . Ì—ÃÏ «· Õﬁﬁ „‰ ﬂ„Ì«  «· ”ÊÌ… Ê«·„Õ«Ê·… „—… √Œ—Ï.",
                            captionEn: "Adjustment",
                            captionAr: " ”ÊÌ…");
                        return;
                    }

                    if (autoJournalLines.Count > 0)
                    {
                        string postError;
                        if (!PostAdjustmentJournal(invoice_no, autoJournalLines, out postError))
                        {
                            UiMessages.ShowError(
                                "Adjustment has been saved, but auto journal posting failed. " + postError,
                                " „ Õ›Ÿ «· ”ÊÌ…° ·ﬂ‰ ›‘· «· —ÕÌ· «·¬·Ì ·ﬁÌÊœ «·ÌÊ„Ì…. " + postError,
                                captionEn: "Adjustment",
                                captionAr: " ”ÊÌ…");
                            return;
                        }
                    }

                    UiMessages.ShowInfo(
                        $"Adjustment saved successfully. Ref: {invoice_no} (Lines: {updatedLines}/{parsedLines}, Deleted: {_deletedProducts.Count})",
                        $" „ Õ›Ÿ «· ”ÊÌ… »‰Ã«Õ. «·„—Ã⁄: {invoice_no} («·√”ÿ—: {updatedLines}/{parsedLines}° «·„Õ–Ê›: {_deletedProducts.Count})",
                        captionEn: "Success",
                        captionAr: "‰Ã«Õ");

                    // After save: reset UI for a new adjustment
                    txt_ref_no.Text = GetMAXInvoiceNo();
                    grid_search_products.DataSource = null;
                    grid_search_products.Rows.Clear();
                    grid_search_products.Refresh();
                    _deletedProducts.Clear();
                    txt_search.Focus();
                    // Select all text inside the control
                    txt_search.SelectAll();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "Œÿ√");
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

        private string GetMAXInvoiceNo()
        {
            //ProductBLL objBLL = new ProductBLL();
            //txt_ref_no.Text = objBLL.GetMaxAdjustmentInvoiceNo();
            SalesBLL objSales = new SalesBLL();
            return objSales.GenerateAdjustmentInvoiceNo();
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Searching...", "Ã«—Ì «·»ÕÀ...")))
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(txt_search.Text))
                    {
                        UiMessages.ShowWarning(
                            "Please enter item code/name to search.",
                            "Ì—ÃÏ ≈œŒ«· ﬂÊœ/«”„ «·’‰› ··»ÕÀ.",
                            captionEn: "Search",
                            captionAr: "»ÕÀ");
                        txt_search.Focus();
                        return;
                    }

                    bool by_code = rb_by_code.Checked;
                    bool by_name = rb_by_name.Checked;

                    frm_searchProducts search_product_obj = new frm_searchProducts(null, null, null, txt_search.Text, "", "", 0, false, false, null, null, this);
                    search_product_obj.ShowDialog();

                    // 1. Force the application to focus the TextBox
                    txt_search.Focus();

                    // 2. Select all text inside the control
                    txt_search.SelectAll();
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "Œÿ√");
                }
            }
        }

        public void Load_product_to_grid(string product_code = "")
        {
            using (BusyScope.Show(this, UiMessages.T("Loading product...", "Ã«—Ì  Õ„Ì· «·’‰›...")))
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
                            decimal original_avg_cost = Math.Round(Convert.ToDecimal(myProductView["avg_cost"]), 2);
                            decimal avg_cost = original_avg_cost;
                            decimal unit_price = Math.Round(Convert.ToDecimal(myProductView["unit_price"]), 2);
                            string description = Convert.ToString(myProductView["description"]);
                            string item_type = Convert.ToString(myProductView["item_type"]);
                            string btn_delete = "Del";
                            string item_number = Convert.ToString(myProductView["item_number"]);

                            // Show qty dialog per product; default to current qty
                            decimal enteredQty = qty;
                            int productID = id;
                            string productCode = code;
                            int selectedAdjustmentAccountId = ResolveSelectedAdjustmentAccountId();
                            

                            using (var qtyDlg = new pos.Products.Adjustment.frm_adjust_qty(qty, unit_price, location_code, productID, productCode,avg_cost))
                            {
                                if (qtyDlg.ShowDialog(this) == DialogResult.OK)
                                {
                                    enteredQty = qtyDlg.EnteredQty; // this is a decimal
                                    productID = qtyDlg._productID; // in case you need it for something
                                    productCode = qtyDlg._productCode; // in case you need it for something
                                    location_code = qtyDlg.locationCode; // in case location can be changed in dialog
                                    unit_price = qtyDlg.Price; // in case price can be changed in dialog
                                    avg_cost = qtyDlg.CostPrice; // in case price can be changed in dialog
                                }
                                else
                                {
                                    if (qtyDlg.IsProductDeleted)
                                    {
                                        _deletedProducts.Add(new DeletedProductAdjustment
                                        {
                                            ProductId = productID,
                                            ProductCode = productCode,
                                            Qty = qty,
                                            CostPrice = original_avg_cost,
                                            AdjustmentAccountId = selectedAdjustmentAccountId
                                        });
                                        continue;
                                    }

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
                            var addedRow = grid_search_products.Rows[grid_search_products.Rows.Count - 1];
                            addedRow.Tag = new AdjustmentRowMeta
                            {
                                OriginalQty = qty,
                                OriginalAvgCost = original_avg_cost,
                                AdjustmentAccountId = selectedAdjustmentAccountId
                            };
                        }
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "No product found.",
                            "·„ Ì „ «·⁄ÀÊ— ⁄·Ï «·’‰›.",
                            captionEn: "Products",
                            captionAr: "«·√’‰«›");
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "Œÿ√");
                }
            }
        }

        private void Get_AccountID_From_Settings()
        {
            inventory_acc_id = 0;
            item_variance_acc_id = 0;

            var settings = AccountingSettingsService.Instance;
            var inventoryAccount = settings.GetDefaultAccount("INVENTORY");
            if (inventoryAccount != null && inventoryAccount.id > 0)
                inventory_acc_id = inventoryAccount.id;

            string adjustmentAccountCode = settings.GetString("ACC_DEFAULT_STOCK_ADJUSTMENT_ACCOUNT", string.Empty);
            item_variance_acc_id = ResolveAccountIdByCode(adjustmentAccountCode);
        }

        private int ResolveAccountIdByCode(string accountCode)
        {
            if (string.IsNullOrWhiteSpace(accountCode))
                return 0;

            try
            {
                string safeCode = accountCode.Trim().Replace("'", "''");
                GeneralBLL objBLL = new GeneralBLL();
                DataTable accountDt = objBLL.GetRecord("TOP 1 id", "acc_accounts WHERE LTRIM(RTRIM(code))='" + safeCode + "'");
                if (accountDt != null && accountDt.Rows.Count > 0 && accountDt.Rows[0]["id"] != DBNull.Value)
                    return Convert.ToInt32(accountDt.Rows[0]["id"]);
            }
            catch
            {
                return 0;
            }

            return 0;
        }

        private void AddAutoJournalLinesForImpact(List<JVLineModel> lines, decimal inventoryImpact, int adjustmentAccountId, string narration)
        {
            if (lines == null)
                return;

            if (inventory_acc_id <= 0 || adjustmentAccountId <= 0)
                return;

            decimal amount = Math.Round(Math.Abs(inventoryImpact), 2);
            if (amount <= 0m)
                return;

            if (inventoryImpact > 0m)
            {
                // Surplus/Revaluation: Inventory Dr, Adjustment Cr
                lines.Add(new JVLineModel { AccountId = inventory_acc_id, Debit = amount, Credit = 0m, Narration = narration, ModuleName = "PRODUCT_ADJUSTMENT" });
                lines.Add(new JVLineModel { AccountId = adjustmentAccountId, Debit = 0m, Credit = amount, Narration = narration, ModuleName = "PRODUCT_ADJUSTMENT" });
            }
            else
            {
                // Shortage/Devaluation/Delete: Adjustment Dr, Inventory Cr
                lines.Add(new JVLineModel { AccountId = adjustmentAccountId, Debit = amount, Credit = 0m, Narration = narration, ModuleName = "PRODUCT_ADJUSTMENT" });
                lines.Add(new JVLineModel { AccountId = inventory_acc_id, Debit = 0m, Credit = amount, Narration = narration, ModuleName = "PRODUCT_ADJUSTMENT" });
            }
        }

        private bool PostAdjustmentJournal(string referenceNo, List<JVLineModel> lines, out string error)
        {
            error = string.Empty;
            if (lines == null || lines.Count == 0)
                return true;

            var autoModel = new AutoJVModel
            {
                ModuleName = "PRODUCT_ADJUSTMENT",
                RefModule = "pos_products",
                RefId = 0,
                VoucherDate = txt_date.Value.Date,
                ReferenceNo = referenceNo,
                Narration = "Product Adjustment",
                IsAutoPosted = true,
                Lines = lines
            };

            PostResult result = new JournalsBLL().PostAutoJournalEntry(autoModel, UsersModal.logged_in_userid);
            if (result != null && result.Success)
                return true;

            if (result != null && result.Messages != null && result.Messages.Count > 0)
            {
                error = result.Messages[0].Message;
            }

            if (string.IsNullOrWhiteSpace(error))
                error = "Unknown posting error.";

            return false;
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

                // Import Excel Ctrl + I
                if (e.Control && e.KeyCode == Keys.I)
                {
                    StartAdjustmentExcelImport();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "Œÿ√");
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
                UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Products", captionAr: "«·√’‰«›");

            }
        }

        private void txt_ref_no_KeyPress(object sender, KeyPressEventArgs e)
        {
            // When press enter search product adjustment
            if (txt_ref_no.Text != "" && e.KeyChar == (char)Keys.Enter)
            {
                using (BusyScope.Show(this, UiMessages.T("Loading adjustment...", "Ã«—Ì  Õ„Ì· «· ”ÊÌ…...")))
                {
                    try
                    {
                        // validation
                        if (txt_ref_no.Text.Trim().Length == 0)
                        {
                            UiMessages.ShowWarning(
                                "Please enter a valid reference number.",
                                "Ì—ÃÏ ≈œŒ«· —ﬁ„ „—Ã⁄ ’ÕÌÕ.",
                                captionEn: "Adjustment",
                                captionAr: " ”ÊÌ…");
                            return;
                        }
                        // select adjustment from table and fill grid
                        ProductBLL _productBll = new ProductBLL();
                        DataTable dt = _productBll.GetProductAdjustmentsByInvoiceNo(txt_ref_no.Text);

                        if (dt.Rows.Count > 0)
                        {
                            grid_search_products.Rows.Clear();
                            _deletedProducts.Clear();
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
                                var addedRow = grid_search_products.Rows[grid_search_products.Rows.Count - 1];
                                addedRow.Tag = new AdjustmentRowMeta
                                {
                                    OriginalQty = qty,
                                    OriginalAvgCost = avg_cost,
                                    AdjustmentAccountId = item_variance_acc_id
                                };
                            }
                        }
                        else
                        {
                            UiMessages.ShowWarning(
                                "No record found for this reference number.",
                                "·«  ÊÃœ »Ì«‰«  ·Â–« «·—ﬁ„ «·„—Ã⁄Ì.",
                                captionEn: "Adjustment",
                                captionAr: " ”ÊÌ…");
                        }
                    }
                    catch (Exception ex)
                    {
                        UiMessages.ShowError(ex.Message, ex.Message, captionEn: "Error", captionAr: "Œÿ√");
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
            _deletedProducts.Clear();

            UiMessages.ShowInfo(
                "Cleared.",
                " „ «·„”Õ.",
                captionEn: "Adjustment",
                captionAr: " ”ÊÌ…");

        }
        private void LoadInventoryAdjustmentAccounts()
        {
            try
            {
                AccountsBLL accountsBll = new AccountsBLL();

                // Load all accounts
                DataTable allAccounts = accountsBll.GetAccountsWithAccountType();

                ddlAdjustmentAccount.Items.Clear();
                _accountNameToId.Clear();

                // Build name?id map for all accounts so we can look up IDs when saving/posting
                foreach (DataRow row in allAccounts.Rows)
                {
                    string accountName = row["name"]?.ToString() ?? "";
                    int accountId = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                    if (!string.IsNullOrEmpty(accountName) && accountId > 0 && !_accountNameToId.ContainsKey(accountName))
                    {
                        _accountNameToId[accountName] = accountId;
                    }
                }

                foreach (DataRow row in allAccounts.Rows)
                {
                    string accountName = row["name"]?.ToString() ?? "";
                    string accountType = row["account_type"]?.ToString() ?? "";

                    if (string.IsNullOrEmpty(accountName))
                    {
                        continue;
                    }

                    // Adjustment Expense accounts (expense type)
                    if (accountType == "Expense" || accountName.IndexOf("Inventory Adjustment", StringComparison.OrdinalIgnoreCase) >= 0
                        || accountName.IndexOf("Adjustment", StringComparison.OrdinalIgnoreCase) >= 0
                        || accountName.IndexOf("Variance", StringComparison.OrdinalIgnoreCase) >= 0
                    )
                    {
                        ddlAdjustmentAccount.Items.Add(accountName);
                    }

                }

                // Set defaults if available
                if (ddlAdjustmentAccount.Items.Count > 0) ddlAdjustmentAccount.SelectedIndex = 0;

                bool selectedFromSettings = false;
                if (item_variance_acc_id > 0)
                {
                    foreach (DataRow row in allAccounts.Rows)
                    {
                        int accountId = row["id"] != DBNull.Value ? Convert.ToInt32(row["id"]) : 0;
                        if (accountId != item_variance_acc_id)
                            continue;

                        string accountName = row["name"]?.ToString() ?? string.Empty;
                        if (!string.IsNullOrWhiteSpace(accountName) && ddlAdjustmentAccount.Items.Contains(accountName))
                        {
                            ddlAdjustmentAccount.SelectedItem = accountName;
                            selectedFromSettings = true;
                        }
                        break;
                    }
                }

                if (!selectedFromSettings)
                {
                    SelectFirstMatchingAccount(ddlAdjustmentAccount, "Inventory Adjustment", "Inventory", "Variance", "Adjustment");
                }

                SelectedAdjustmentAccountId = ResolveSelectedAdjustmentAccountId();

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    $"Error loading accounts: {ex.Message}",
                    $"Œÿ√ ›Ì  Õ„Ì· «·Õ”«»« : {ex.Message}",
                    "Error", "Œÿ√");
            }
        }

        private void ConfigureGridEditability()
        {
            try
            {
                foreach (DataGridViewColumn col in grid_search_products.Columns)
                {
                    col.ReadOnly = true;
                }

                SetColumnReadOnly("location_code", false);
                SetColumnReadOnly("adjustment_qty", false);
                SetColumnReadOnly("avg_cost", false);

                // allow click delete button
                SetColumnReadOnly("btn_delete", true);
            }
            catch
            {
            }
        }

        private void SetColumnReadOnly(string columnName, bool isReadOnly)
        {
            if (grid_search_products.Columns.Contains(columnName))
            {
                grid_search_products.Columns[columnName].ReadOnly = isReadOnly;
            }
        }

        private void btn_excel_import_Click(object sender, EventArgs e)
        {
            using (var frm = new frm_sales_excel_import(
                StartAdjustmentExcelImport,
                DownloadAdjustmentImportTemplate,
                "Product Adjustment Excel Import",
                "Product Adjustment Excel Import Utility",
                "Import adjustment lines from Excel into product adjustment grid. Required: Product Code or Name. Optional: Location, Exact Qty, Cost Price.",
                "How to use",
                "1. Download the sample template.\r\n2. Fill Product Code or Name, Location, Exact Qty and Cost Price.\r\n3. Click Import Excel and choose your file.\r\n4. Review imported lines before saving adjustment."))
            {
                frm.ShowDialog(this);
            }
        }

        private void DownloadAdjustmentImportTemplate()
        {
            try
            {
                ExcelExportHelper.ExportDataTableToExcel(
                    BuildAdjustmentImportTemplate(),
                    "product_adjustment_import_template",
                    this,
                    includeLastRow: true);
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Import Template", "ﬁ«·» «·«” Ì—«œ");
            }
        }

        private DataTable BuildAdjustmentImportTemplate()
        {
            var dt = new DataTable();
            dt.Columns.Add("Product Code");
            dt.Columns.Add("Name");
            dt.Columns.Add("Location");
            dt.Columns.Add("Exact Qty");
            dt.Columns.Add("Cost Price");

            dt.Rows.Add("PRD-001", "Sample Product 1", "A1", "25", "120.50");
            dt.Rows.Add("PRD-002", "Sample Product 2", "B2", "8", "89.00");

            return dt;
        }

        private void StartAdjustmentExcelImport()
        {
            try
            {
                using (var ofd = new OpenFileDialog())
                {
                    ofd.Title = "Import product adjustments from Excel";
                    ofd.Filter = "Excel Files (*.xlsx;*.xls)|*.xlsx;*.xls";
                    ofd.Multiselect = false;

                    if (ofd.ShowDialog(this) != DialogResult.OK)
                        return;

                    using (BusyScope.Show(this, UiMessages.T("Importing adjustment items...", "Ã«—Ì «” Ì—«œ √’‰«› «· ”ÊÌ…...")))
                    {
                        DataTable excelDt = ProductExcelImportHelper.ReadExcel(ofd.FileName);
                        List<ProductAdjustmentImportRow> rows = ParseAdjustmentImportRows(excelDt);
                        ImportAdjustmentItemsFromExcel(rows);
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, ex.Message, "Import Excel", "«” Ì—«œ ≈ﬂ”·");
            }
        }

        private List<ProductAdjustmentImportRow> ParseAdjustmentImportRows(DataTable source)
        {
            var result = new List<ProductAdjustmentImportRow>();
            if (source == null || source.Rows.Count == 0)
                return result;

            string codeColumn = FindImportColumn(source, "productcode", "product_code", "product code", "code", "itemcode", "item code");
            string nameColumn = FindImportColumn(source, "productname", "product_name", "product name", "name", "itemname", "item name");
            string qtyColumn = FindImportColumn(source, "exactqty", "exact_qty", "exact qty", "adjustmentqty", "adjustment_qty", "adjustment qty", "qty", "quantity");
            string costColumn = FindImportColumn(source, "costprice", "cost_price", "cost price", "avgcost", "avg_cost", "avg cost", "price");
            string locationColumn = FindImportColumn(source, "location", "locationcode", "location_code", "loc", "warehouse", "warehousecode");

            foreach (DataRow row in source.Rows)
            {
                string code = ReadImportText(row, codeColumn);
                string name = ReadImportText(row, nameColumn);
                if (string.IsNullOrWhiteSpace(code) && string.IsNullOrWhiteSpace(name))
                    continue;

                decimal? exactQty = ReadImportDecimal(row, qtyColumn);
                decimal? cost = ReadImportDecimal(row, costColumn);
                string loc = ReadImportText(row, locationColumn);

                if (exactQty.HasValue && exactQty.Value < 0)
                    continue;
                if (cost.HasValue && cost.Value < 0)
                    continue;

                result.Add(new ProductAdjustmentImportRow
                {
                    ProductCode = code,
                    ProductName = name,
                    ExactQty = exactQty,
                    CostPrice = cost,
                    LocationCode = loc
                });
            }

            return result;
        }

        private static string FindImportColumn(DataTable dt, params string[] aliases)
        {
            if (dt == null || aliases == null || aliases.Length == 0)
                return null;

            foreach (DataColumn column in dt.Columns)
            {
                string normalized = NormalizeImportColumn(column.ColumnName);
                for (int i = 0; i < aliases.Length; i++)
                {
                    if (normalized == NormalizeImportColumn(aliases[i]))
                        return column.ColumnName;
                }
            }

            return null;
        }

        private static string NormalizeImportColumn(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return string.Empty;

            return new string(value.Where(char.IsLetterOrDigit).ToArray()).ToLowerInvariant();
        }

        private static string ReadImportText(DataRow row, string columnName)
        {
            if (row == null || string.IsNullOrWhiteSpace(columnName) || !row.Table.Columns.Contains(columnName))
                return string.Empty;

            return Convert.ToString(row[columnName]).Trim();
        }

        private static decimal? ReadImportDecimal(DataRow row, string columnName)
        {
            string text = ReadImportText(row, columnName);
            if (string.IsNullOrWhiteSpace(text))
                return null;

            decimal val;
            return decimal.TryParse(text, out val) ? val : (decimal?)null;
        }

        private void ImportAdjustmentItemsFromExcel(IList<ProductAdjustmentImportRow> items)
        {
            if (items == null || items.Count == 0)
            {
                UiMessages.ShowInfo("The selected Excel file does not contain any valid rows.", "„·› «·≈ﬂ”· «·„Õœœ ·« ÌÕ ÊÌ ⁄·Ï √Ì ’›Ê› ’ÕÌÕ….");
                return;
            }

            int importedCount = 0;
            var skipped = new List<string>();

            for (int i = 0; i < items.Count; i++)
            {
                ProductAdjustmentImportRow item = items[i];
                DataRow productRow = FindProductForAdjustmentImport(item.ProductCode, item.ProductName);
                if (productRow == null)
                {
                    skipped.Add((!string.IsNullOrWhiteSpace(item.ProductCode) ? item.ProductCode : item.ProductName) + " (product not found)");
                    continue;
                }

                ImportProductIntoAdjustmentGrid(productRow, item);
                importedCount++;
            }

            if (importedCount == 0)
            {
                UiMessages.ShowWarning(
                    "No rows were imported. Please verify the Excel columns and product codes.",
                    "·„ Ì „ «” Ì—«œ √Ì ’›Ê›. Ì—ÃÏ «· Õﬁﬁ „‰ √⁄„œ… «·≈ﬂ”· Ê√ﬂÊ«œ «·„‰ Ã« .");
                return;
            }

            string details = skipped.Count > 0 ? "\n\nSkipped: " + string.Join(", ", skipped.Take(10).ToArray()) : string.Empty;
            UiMessages.ShowInfo(
                string.Format("Imported {0} row(s) successfully.{1}", importedCount, details),
                string.Format(" „ «” Ì—«œ {0} ’›/’›Ê› »‰Ã«Õ.{1}", importedCount, skipped.Count > 0 ? "\n\n „  ŒÿÌ »⁄÷ «·’›Ê›." : string.Empty),
                "Import Excel",
                "«” Ì—«œ ≈ﬂ”·");
        }

        private DataRow FindProductForAdjustmentImport(string productCode, string productName)
        {
            var productsBLL = new ProductBLL();
            DataTable dt = null;

            if (!string.IsNullOrWhiteSpace(productCode))
                dt = productsBLL.SearchRecordByProductCode(productCode.Trim());

            if ((dt == null || dt.Rows.Count == 0) && !string.IsNullOrWhiteSpace(productName))
            {
                var searchDt = productsBLL.SearchRecord(productName.Trim(), by_name: true);
                if (searchDt != null && searchDt.Rows.Count > 0)
                {
                    DataRow selectedRow = null;
                    foreach (DataRow searchRow in searchDt.Rows)
                    {
                        if (string.Equals(Convert.ToString(searchRow["name"]), productName.Trim(), StringComparison.OrdinalIgnoreCase))
                        {
                            selectedRow = searchRow;
                            break;
                        }
                    }

                    if (selectedRow == null)
                        selectedRow = searchDt.Rows[0];

                    var itemNumber = Convert.ToString(selectedRow["item_number"]);
                    if (!string.IsNullOrWhiteSpace(itemNumber))
                        dt = productsBLL.SearchRecordByProductNumber(itemNumber);
                }
            }

            return dt != null && dt.Rows.Count > 0 ? dt.Rows[0] : null;
        }

        private void ImportProductIntoAdjustmentGrid(DataRow productRow, ProductAdjustmentImportRow importItem)
        {
            if (productRow == null)
                return;

            string itemNumber = Convert.ToString(productRow["item_number"]);
            int rowIndex = FindAdjustmentGridRowByItemNumber(itemNumber);

            int adjustmentAccountId = ResolveSelectedAdjustmentAccountId();
            decimal qty = Math.Round(Convert.ToDecimal(productRow["qty"]), 2);
            decimal avgCostDb = Math.Round(Convert.ToDecimal(productRow["avg_cost"]), 2);
            decimal unitPrice = Math.Round(Convert.ToDecimal(productRow["unit_price"]), 2);
            decimal exactQty = importItem.ExactQty.HasValue ? Math.Round(importItem.ExactQty.Value, 2) : qty;
            decimal costPrice = importItem.CostPrice.HasValue ? Math.Round(importItem.CostPrice.Value, 2) : avgCostDb;
            string locationCode = !string.IsNullOrWhiteSpace(importItem.LocationCode)
                ? importItem.LocationCode.Trim().ToUpperInvariant()
                : Convert.ToString(productRow["location_code"]);

            if (rowIndex < 0)
            {
                string[] row0 =
                {
                    Convert.ToInt32(productRow["id"]).ToString(),
                    Convert.ToString(productRow["code"]),
                    Convert.ToString(productRow["category"]),
                    Convert.ToString(productRow["name"]),
                    Convert.ToString(productRow["name_ar"]),
                    locationCode,
                    qty.ToString("N2"),
                    exactQty.ToString("N2"),
                    costPrice.ToString("N2"),
                    unitPrice.ToString("N2"),
                    "Del",
                    Convert.ToString(productRow["description"]),
                    Convert.ToString(productRow["item_type"]),
                    itemNumber
                };

                grid_search_products.Rows.Add(row0);
                var addedRow = grid_search_products.Rows[grid_search_products.Rows.Count - 1];
                addedRow.Tag = new AdjustmentRowMeta
                {
                    OriginalQty = qty,
                    OriginalAvgCost = avgCostDb,
                    AdjustmentAccountId = adjustmentAccountId > 0 ? adjustmentAccountId : item_variance_acc_id
                };
                return;
            }

            DataGridViewRow row = grid_search_products.Rows[rowIndex];
            row.Cells["location_code"].Value = locationCode;
            row.Cells["adjustment_qty"].Value = exactQty.ToString("N2");
            row.Cells["avg_cost"].Value = costPrice.ToString("N2");

            var meta = row.Tag as AdjustmentRowMeta;
            if (meta != null)
            {
                meta.AdjustmentAccountId = adjustmentAccountId > 0 ? adjustmentAccountId : item_variance_acc_id;
            }
        }

        private int FindAdjustmentGridRowByItemNumber(string itemNumber)
        {
            if (string.IsNullOrWhiteSpace(itemNumber))
                return -1;

            for (int i = 0; i < grid_search_products.Rows.Count; i++)
            {
                var row = grid_search_products.Rows[i];
                if (row == null || row.IsNewRow)
                    continue;

                string existing = Convert.ToString(row.Cells["item_number"].Value);
                if (string.Equals((existing ?? string.Empty).Trim(), itemNumber.Trim(), StringComparison.OrdinalIgnoreCase))
                    return i;
            }

            return -1;
        }
        private void SelectFirstMatchingAccount(ComboBox ddl, params string[] keywords)
        {
            if (ddl == null || ddl.Items.Count == 0 || keywords == null || keywords.Length == 0)
            {
                return;
            }

            for (int i = 0; i < ddl.Items.Count; i++)
            {
                string accountName = ddl.Items[i]?.ToString() ?? string.Empty;
                foreach (string keyword in keywords)
                {
                    if (!string.IsNullOrWhiteSpace(keyword) && accountName.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        ddl.SelectedIndex = i;
                        return;
                    }
                }
            }
        }

        private int ResolveSelectedAdjustmentAccountId()
        {
            try
            {
                string accountName = ddlAdjustmentAccount.SelectedItem?.ToString() ?? string.Empty;
                if (string.IsNullOrWhiteSpace(accountName))
                    return 0;

                int accountId;
                if (_accountNameToId.TryGetValue(accountName, out accountId) && accountId > 0)
                    return accountId;
            }
            catch
            {
                return 0;
            }

            return 0;
        }

        private static bool ValidateAdjustmentQty(DataGridViewRow row, out string message)
        {
            message = string.Empty;

            double currentQty = ParseDoubleCell(row, "qty");
            double targetQty = ParseDoubleCell(row, "adjustment_qty");

            if (targetQty < 0)
            {
                message = "Adjustment quantity cannot be negative.";
                return false;
            }

            if (Math.Abs(targetQty - currentQty) < 0.0001)
            {
                message = "Adjustment quantity is the same as the current quantity.";
                return false;
            }

            return true;
        }

    }
}
