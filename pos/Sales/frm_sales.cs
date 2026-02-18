using pos.UI.Busy;
using pos.Master.Companies.zatca;
using pos.Security.Authorization;
using pos.UI; // <-- Added Ui namespace
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer; // added
using AppPermissions = pos.Security.Authorization.Permissions;

namespace pos
{
    public partial class frm_sales : Form
    {
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");
        public int cash_account_id = 0;
        public int sales_account_id = 0;
        public int receivable_account_id = 0;
        public int tax_account_id = 0;
        public int sales_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_acc_id = 0;

        public double employee_commission_percent = 0;
        public double user_commission_percent = 0;

        public double total_amount = 0;
        public double total_cost_amount = 0;
        public double total_cost_amount_e_vat = 0;
        public double total_tax = 0;
        public double total_discount = 0;
        public double total_sub_total = 0;
        string invoice_status = "";
        string product_code = "";

        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;

        public double cash_sales_amount_limit = 0;
        //public double cash_purchase_amount_limit= 0;
        public bool allow_credit_sales = false;
        //public bool allow_credit_purchase= false;

        private DataGridView brandsDataGridView = new DataGridView();
        private DataGridView categoriesDataGridView = new DataGridView();
        private DataGridView groupsDataGridView = new DataGridView();
        private DataGridView customersDataGridView;

        // ... inside class frm_sales
        private Timer _customerSearchDebounceTimer;
        private bool _suppressCustomerSearch;

        ProductBLL productsBLL_obj = new ProductBLL();

        public frm_sales()
        {
            InitializeComponent();
            txt_groups.Click += TextBoxOnClick;
            txt_categories.Click += TextBoxOnClick;
            txt_brands.Click += TextBoxOnClick;

            // Debounce for customer search
            SetupCustomersDataGridView(); // build it once here
            _customerSearchDebounceTimer = new Timer { Interval = 300 };
            _customerSearchDebounceTimer.Tick += CustomerSearchDebounceTimer_Tick;
        }


        private void TextBoxOnClick(object sender, EventArgs eventArgs)
        {
            var txt_brands = (TextBox)sender;
            txt_brands.SelectAll();
            txt_brands.Focus();

            var txt_categories = (TextBox)sender;
            txt_categories.SelectAll();
            txt_categories.Focus();

            var txt_groups = (TextBox)sender;
            txt_groups.SelectAll();
            txt_groups.Focus();
        }

        private void frm_sales_Load(object sender, EventArgs e)
        {
            // Apply professional theme
            AppTheme.Apply(this);
            StyleSalesForm();

            this.Text = "Sale Invoice ";
            grid_sales.Rows.Add();
            this.ActiveControl = grid_sales;
            grid_sales.CurrentCell = grid_sales.Rows[0].Cells["code"];
            //grid_sales.Focus();
            //grid_sales.BeginEdit(true);

            //btn_movements.Enabled = false;
            //autoCompleteProductCode();
            load_user_rights(UsersModal.logged_in_userid);
            Get_AccountID_From_Company();
            get_customers_dropdownlist();

            get_employees_dropdownlist();
            get_payment_terms_dropdownlist();
            get_payment_method_dropdownlist();
            get_saletype_dropdownlist();
            get_invoice_subtype_dropdownlist();
            Get_user_total_commission();

            // Apply permissions for ZATCA send checkbox
            ApplyZatcaPermissionsToControls();

            //disable sorting in grid
            foreach (DataGridViewColumn column in grid_sales.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void ApplyZatcaPermissionsToControls()
        {
            // If ZATCA isn't enabled system-wide, keep it off.
            if (!UsersModal.useZatcaEInvoice)
            {
                chk_sendInvoiceToZatca.Checked = false;
                chk_sendInvoiceToZatca.Enabled = false;
                return;
            }

            bool canTransmit = _auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Transmit);
            chk_sendInvoiceToZatca.Checked = _auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Send);

            if (!canTransmit)
            {
                chk_sendInvoiceToZatca.Enabled = false;
                //chk_sendInvoiceToZatca.Visible = false; // hide if denied
            }
            else
            {
                chk_sendInvoiceToZatca.Visible = true;
                chk_sendInvoiceToZatca.Enabled = true;
            }
        }

        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchSaleProducts frm_search_product_obj = new frm_searchSaleProducts();
            frm_search_product_obj.Show();

        }

        /// <summary>
        /// POS-specific styling applied after the generic Fluent theme.
        /// Gives the sales page a Dynamics 365–like professional look.
        /// </summary>
        private void StyleSalesForm()
        {
            // ── Classic Windows grey panels ───────────────────────────
            panel_header.BackColor = SystemColors.Control;
            panel_footer.BackColor = SystemColors.Control;
            panel_grid.BackColor = SystemColors.Control;

            // ── GroupBoxes: standard Windows look ─────────────────────
            foreach (Control ctrl in panel_header.Controls)
            {
                if (ctrl is GroupBox grp)
                {
                    grp.BackColor = SystemColors.Control;
                    grp.ForeColor = SystemColors.ControlText;
                    grp.Font = AppTheme.FontGroupBox;
                    grp.Padding = new Padding(4, 8, 4, 4);

                    foreach (Control child in grp.Controls)
                    {
                        if (child is ComboBox cmb)
                        {
                            cmb.BackColor = SystemColors.Window;
                            cmb.FlatStyle = FlatStyle.Standard;
                        }
                    }
                }
            }

            // ── Title label ───────────────────────────────────────────
            lbl_title.Font = AppTheme.FontHeader;
            lbl_title.ForeColor = SystemColors.ControlText;

            // ── ToolStrip: classic Windows system renderer ────────────
            SalesToolStrip.RenderMode = ToolStripRenderMode.System;
            SalesToolStrip.BackColor = SystemColors.Control;
            SalesToolStrip.ForeColor = SystemColors.ControlText;
            SalesToolStrip.ImageScalingSize = new Size(20, 20);
            SalesToolStrip.AutoSize = true;
            SalesToolStrip.GripStyle = ToolStripGripStyle.Hidden;
            SalesToolStrip.Padding = new Padding(4, 2, 4, 2);
            foreach (ToolStripItem item in SalesToolStrip.Items)
            {
                item.ForeColor = SystemColors.ControlText;
                item.Padding = new Padding(4, 2, 4, 2);
                item.Margin = new Padding(1, 0, 1, 0);
                if (item is ToolStripButton tsb)
                {
                    tsb.DisplayStyle = ToolStripItemDisplayStyle.ImageAndText;
                    tsb.TextImageRelation = TextImageRelation.ImageBeforeText;
                }
            }

            // ── Sales grid: clean Dynamics-style ──────────────────────
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.SetProperty,
                null, grid_sales, new object[] { true });
            grid_sales.ColumnHeadersHeight = 38;
            grid_sales.RowTemplate.Height = 34;
            grid_sales.RowHeadersVisible = false;
            grid_sales.BackgroundColor = SystemColors.AppWorkspace;
            grid_sales.DefaultCellStyle.Font = new Font("Segoe UI", 10F, FontStyle.Regular);
            grid_sales.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular);

            // Serial number column: muted, centered
            sno.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Control,
                ForeColor = SystemColors.GrayText,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.GrayText,
                Alignment = DataGridViewContentAlignment.MiddleCenter,
                Font = AppTheme.FontSmall
            };

            // Code column
            code.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Window,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText
            };

            // Delete button column styling
            btn_delete.DefaultCellStyle = new DataGridViewCellStyle
            {
                ForeColor = AppTheme.Danger,
                SelectionForeColor = AppTheme.Danger,
                Alignment = DataGridViewContentAlignment.MiddleCenter
            };
            btn_delete.FlatStyle = FlatStyle.Flat;

            // ── Footer: professional totals area ──────────────────
            // Style TableLayoutPanels as white card sections on gray footer
            StyleFooterCard(tableLayoutPanel5);
            StyleFooterCard(tableLayoutPanel6);
            StyleFooterCard(tableLayoutPanel7);
            StyleFooterCard(tableLayoutPanel8);

            // groupBox2 (product info)
            groupBox2.BackColor = SystemColors.Control;
            groupBox2.ForeColor = SystemColors.ControlText;
            groupBox2.Font = AppTheme.FontGroupBox;

            // ── Main totals labels ──────────────────────────────
            StyleFooterLabel(label14, false);      // Sub Total
            StyleFooterLabel(label13, false);      // Discount
            StyleFooterLabel(label9, true);        // Total Amount
            chkbox_is_taxable.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Regular);
            chkbox_is_taxable.ForeColor = SystemColors.ControlText;

            // ── Secondary labels (tableLayoutPanel7) ──────────────────
            StyleFooterLabel(label22, false);      // Net After Disc
            StyleFooterLabel(label23, false);      // Total Qty

            // ── Received / Change labels (tableLayoutPanel8) ──────────
            StyleFooterLabel(label24, false);      // Received
            StyleFooterLabel(label25, false);      // Change

            // ── Total fields — large and prominent ────────────────────
            StyleTotalField(txt_total_amount, true);       // Grand total: HERO
            StyleTotalField(txt_sub_total, false);
            StyleTotalField(txt_sub_total_2, false);
            StyleTotalField(txt_total_tax, false);
            StyleTotalField(txt_total_discount, false);
            StyleTotalField(txt_total_qty, false);
            StyleSecondaryField(txt_change_amount);
            StyleSecondaryField(txt_amount_received);

            // ── Cost fields: subtle muted look ────────────────────────
            StyleCostField(txt_cost_price);
            StyleCostField(txt_cost_price_with_vat);
            StyleCostField(txt_single_cost_evat);
            StyleCostField(txt_total_cost);
            StyleCostField(txt_shop_qty);
            StyleCostField(txt_company_qty);
            StyleCostField(txt_order_qty);

            // ── Customer search dropdown ──────────────────────────────
            if (customersDataGridView != null)
            {
                StyleDropdownGrid(customersDataGridView);
            }
        }

        /// <summary>Style a summary total field in the footer.</summary>
        private static void StyleTotalField(TextBox txt, bool isPrimary)
        {
            txt.ReadOnly = true;
            txt.BorderStyle = BorderStyle.Fixed3D;
            txt.TextAlign = HorizontalAlignment.Right;
            if (isPrimary)
            {
                txt.Font = new Font("Segoe UI Semibold", 18F, FontStyle.Bold);
                txt.ForeColor = SystemColors.WindowText;
                txt.BackColor = SystemColors.Window;
            }
            else
            {
                txt.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Regular);
                txt.ForeColor = SystemColors.WindowText;
                txt.BackColor = SystemColors.Window;
            }
        }

        /// <summary>Style a secondary footer field (received / change).</summary>
        private static void StyleSecondaryField(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.Fixed3D;
            txt.TextAlign = HorizontalAlignment.Right;
            txt.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Regular);
            txt.ForeColor = SystemColors.WindowText;
            txt.BackColor = SystemColors.Window;
        }

        /// <summary>Style a cost-info read-only field.</summary>
        private static void StyleCostField(TextBox txt)
        {
            txt.ReadOnly = true;
            txt.BorderStyle = BorderStyle.Fixed3D;
            txt.TextAlign = HorizontalAlignment.Right;
            txt.Font = AppTheme.FontDefault;
            txt.ForeColor = SystemColors.GrayText;
            txt.BackColor = SystemColors.Control;
        }

        /// <summary>Style a popup DataGridView dropdown (brands / categories / customers).</summary>
        private static void StyleDropdownGrid(DataGridView dgv)
        {
            dgv.BorderStyle = BorderStyle.FixedSingle;
            dgv.BackgroundColor = SystemColors.AppWorkspace;
            dgv.GridColor = SystemColors.ControlDark;
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Window,
                ForeColor = SystemColors.WindowText,
                Font = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                Padding = new Padding(6, 2, 6, 2)
            };
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor = SystemColors.Control,
                ForeColor = SystemColors.ControlText,
                Font = AppTheme.FontGridHeader,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText
            };
            dgv.EnableHeadersVisualStyles = false;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.RowHeadersVisible = false;
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowTemplate.Height = 28;
            dgv.ColumnHeadersHeight = 32;
        }

        /// <summary>Style a footer TableLayoutPanel.</summary>
        private static void StyleFooterCard(TableLayoutPanel tlp)
        {
            tlp.BackColor = SystemColors.Control;
        }

        /// <summary>Style a footer label.</summary>
        private static void StyleFooterLabel(Label lbl, bool isPrimary)
        {
            if (isPrimary)
            {
                lbl.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold);
                lbl.ForeColor = SystemColors.ControlText;
            }
            else
            {
                lbl.Font = new Font("Segoe UI Semibold", 10F, FontStyle.Regular);
                lbl.ForeColor = SystemColors.ControlText;
            }
        }

        Form frm_searchSaleProducts_obj;

        private void grid_sales_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

            try
            {
                if (e.RowIndex < 0) return;

                int iColumn = grid_sales.CurrentCell.ColumnIndex;
                int iRow = grid_sales.CurrentCell.RowIndex;
                string columnName = grid_sales.Columns[e.ColumnIndex].Name;
                if (columnName == "code")
                {

                    product_code = (grid_sales.CurrentRow.Cells["code"].Value != null ? grid_sales.CurrentRow.Cells["code"].Value.ToString() : "");

                    bool isGrid = true;
                    var brand_code = txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                    var category_code = txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                    var group_code = txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                    ////////////////////////
                    if (frm_searchSaleProducts_obj == null || product_code != "")
                    {
                        frm_searchSaleProducts_obj = new frm_searchSaleProducts(this, product_code, category_code, brand_code, isGrid, group_code);
                        frm_searchSaleProducts_obj.FormClosed += new FormClosedEventHandler(frm_searchSaleProducts_obj_FormClosed);

                        //frm_cust.Dock = DockStyle.Fill;
                        frm_searchSaleProducts_obj.ShowDialog();
                    }
                    else
                    {
                        //frm_searchSaleProducts_obj.ShowDialog();
                        //frm_searchSaleProducts_obj.BringToFront();
                        frm_searchSaleProducts_obj.Visible = true;
                    }
                    ////////////

                }

                // Handle the end of editing for numeric columns (Qty=4, unit_price=5, discount=6, discount_percent=7, total_without_vat=8)
                if (e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 8)
                {
                    var cell = grid_sales.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    // If the cell value is null or empty, set it to 0
                    if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        cell.Value = 0;
                    }
                }

                // Helper to read double safely
                double GetCellDouble(string colName)
                {
                    var val = grid_sales.Rows[e.RowIndex].Cells[colName].Value;
                    return val == null || string.IsNullOrWhiteSpace(Convert.ToString(val)) ? 0 : Convert.ToDouble(val);
                }

                // Prevent unit_price below cost_price
                if (columnName.Equals("unit_price", StringComparison.OrdinalIgnoreCase))
                {
                    var costPriceVal = grid_sales.Rows[e.RowIndex].Cells["cost_price"].Value;
                    var unitPriceVal = grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value;

                    double costPrice = costPriceVal == null || string.IsNullOrWhiteSpace(Convert.ToString(costPriceVal))
                        ? 0
                        : Convert.ToDouble(costPriceVal);

                    double unitPriceInput = unitPriceVal == null || string.IsNullOrWhiteSpace(Convert.ToString(unitPriceVal))
                        ? 0
                        : Convert.ToDouble(unitPriceVal);

                    if (unitPriceInput < costPrice)
                    {
                        UiMessages.ShowWarning(
                            "Warning: Unit price is lower than cost price. The invoice will be saved with this price.",
                            "تنبيه: سعر الوحدة أقل من سعر التكلفة. سيتم حفظ الفاتورة بهذا السعر.");

                        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Style.BackColor = Color.MistyRose;

                        // Continue (do not reset value; do not return)
                    }
                }

                double unitPrice = GetCellDouble("unit_price");
                double qty = GetCellDouble("qty");
                double discount = GetCellDouble("discount");
                double taxRate = GetCellDouble("tax_rate");

                double totalValue = unitPrice * qty;
                double subTotal = totalValue - discount;
                double tax = (subTotal * taxRate) / 100;

                // ----------------------------- QTY Changed -----------------------------
                if (columnName == "Qty")
                {
                    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax;
                    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = subTotal + tax;
                    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = subTotal;
                }

                // --------------------------- UNIT PRICE Changed ------------------------
                if (columnName == "unit_price")
                {
                    double grossTotal = unitPrice * qty;
                    double discountValue = GetCellDouble("discount");
                    double netTotal = grossTotal - discountValue;
                    double tax_1 = (netTotal * taxRate) / 100;

                    if (rd_btn_without_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax_1;
                        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = netTotal + tax_1;
                        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = netTotal;
                    }

                    if (rd_btn_without_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double singleUnitPrice = unitPrice / qty;
                        double taxPerUnit = (singleUnitPrice * taxRate) / 100;
                        double totalTax = taxPerUnit * qty;

                        double newTotal = (singleUnitPrice * qty) - discountValue;
                        double discountPercent = grossTotal == 0 ? 0 : (discountValue / grossTotal) * 100;

                        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value = singleUnitPrice;
                        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = (newTotal * taxRate) / 100;
                        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = newTotal + tax_1;
                        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = newTotal;
                        grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discountPercent;
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        double preTax = 1 + (taxRate / 100);
                        double netUnitPrice = unitPrice / preTax;
                        double grossTotalBeforeDiscount = netUnitPrice * qty;
                        double netTotal_1 = grossTotalBeforeDiscount - discountValue;
                        double taxAmount = (netTotal_1 * taxRate) / 100;

                        double discountPercent = grossTotalBeforeDiscount == 0 ? 0 : (discountValue / grossTotalBeforeDiscount) * 100;

                        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value = netUnitPrice;
                        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = taxAmount;
                        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = netTotal_1 + taxAmount;
                        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = netTotal_1;
                        grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discountPercent;
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double preTax = 1 + (taxRate / 100);
                        double netTotalWithoutTax = unitPrice / preTax;
                        double netUnitPrice = netTotalWithoutTax / qty;
                        double grossBeforeDiscount = netUnitPrice * qty;
                        double newNetTotal = grossBeforeDiscount - discountValue;
                        double taxAmount = (newNetTotal * taxRate) / 100;

                        double discountPercent = grossBeforeDiscount == 0 ? 0 : (discountValue / grossBeforeDiscount) * 100;

                        grid_sales.Rows[e.RowIndex].Cells["unit_price"].Value = netUnitPrice;
                        grid_sales.Rows[e.RowIndex].Cells["tax"].Value = taxAmount;
                        grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = newNetTotal + taxAmount;
                        grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = newNetTotal;
                        grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discountPercent;
                    }
                }

                // --------------------------- DISCOUNT Changed --------------------------
                if (columnName == "discount")
                {
                    double discountPercent = totalValue == 0 ? 0 : (discount / totalValue) * 100;
                    tax = ((totalValue - discount) * taxRate) / 100;
                    double finalSubtotal = totalValue - discount + tax;

                    grid_sales.Rows[e.RowIndex].Cells["discount_percent"].Value = discountPercent;
                    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = finalSubtotal;
                    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = finalSubtotal - tax;
                    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax;
                }

                // -------------------- DISCOUNT PERCENT Changed -------------------------
                if (columnName == "discount_percent")
                {
                    double discountPercent = GetCellDouble("discount_percent");
                    double discountValue = (discountPercent * totalValue) / 100;
                    tax = ((totalValue - discountValue) * taxRate) / 100;
                    double finalSubtotal = totalValue - discountValue + tax;

                    grid_sales.Rows[e.RowIndex].Cells["discount"].Value = discountValue;
                    grid_sales.Rows[e.RowIndex].Cells["sub_total"].Value = finalSubtotal;
                    grid_sales.Rows[e.RowIndex].Cells["total_without_vat"].Value = finalSubtotal - tax;
                    grid_sales.Rows[e.RowIndex].Cells["tax"].Value = tax;
                }

                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_cost_amount();
                get_total_amount();
                get_total_qty();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }

        }

        void frm_searchSaleProducts_obj_FormClosed(object sender, FormClosedEventArgs e)
        {
            //frm_searchSaleProducts_obj = null;
        }

        public void Load_products_to_grid(string item_number)
        {

            try
            {
                DataTable product_dt = new DataTable();

                product_dt = productsBLL_obj.SearchRecordByProductNumber(item_number);

                int RowIndex = grid_sales.CurrentCell.RowIndex;

                if (product_dt.Rows.Count > 0)
                {
                    for (int i = 0; i < grid_sales.RowCount; i++)
                    {
                        var grid_item_number = (grid_sales.Rows[i].Cells["item_number"].Value != null ? grid_sales.Rows[i].Cells["item_number"].Value : "");
                        if (grid_item_number.ToString() == item_number)
                        {
                            UiMessages.ShowWarning("Product already added", "المنتج مضاف مسبقاً", "Already exist", "موجود بالفعل");
                            grid_sales.CurrentCell = grid_sales.Rows[RowIndex].Cells["code"]; //make qty cell active
                                                                                              //grid_sales.CurrentCell.Selected = true;
                            grid_sales.BeginEdit(true);
                            return;
                        }
                        else
                        {
                            grid_sales.CurrentCell = grid_sales.Rows[RowIndex].Cells["qty"]; //make qty cell active
                            grid_sales.CurrentCell.Selected = true;

                        }
                    }

                    foreach (DataRow myProductView in product_dt.Rows)
                    {

                        double qty = Convert.ToDouble(myProductView["sale_demand_qty"].ToString() == string.Empty || (decimal)myProductView["sale_demand_qty"] == 0 ? "1" : myProductView["sale_demand_qty"].ToString());
                        double total = qty * double.Parse(myProductView["unit_price"].ToString());
                        double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                        double tax = (total * tax_rate / 100);
                        double sub_total = tax + total;
                        double sub_total_without_vat = total;

                        grid_sales.Rows[RowIndex].Cells["id"].Value = myProductView["id"].ToString();
                        grid_sales.Rows[RowIndex].Cells["code"].Value = myProductView["code"].ToString();
                        grid_sales.Rows[RowIndex].Cells["name"].Value = myProductView["name"].ToString();
                        grid_sales.Rows[RowIndex].Cells["qty"].Value = qty;
                        grid_sales.Rows[RowIndex].Cells["cost_price"].Value = Math.Round(Decimal.Parse(myProductView["avg_cost"].ToString()), 2);
                        grid_sales.Rows[RowIndex].Cells["unit_price"].Value = Math.Round(Decimal.Parse(myProductView["unit_price"].ToString()), 2);
                        grid_sales.Rows[RowIndex].Cells["discount"].Value = 0.00;
                        grid_sales.Rows[RowIndex].Cells["discount_percent"].Value = 0.00;
                        grid_sales.Rows[RowIndex].Cells["tax"].Value = tax;
                        grid_sales.Rows[RowIndex].Cells["sub_total"].Value = sub_total;
                        grid_sales.Rows[RowIndex].Cells["total_without_vat"].Value = sub_total_without_vat;
                        grid_sales.Rows[RowIndex].Cells["location_code"].Value = myProductView["location_code"].ToString();
                        grid_sales.Rows[RowIndex].Cells["unit"].Value = myProductView["unit"].ToString();
                        grid_sales.Rows[RowIndex].Cells["category"].Value = myProductView["category"].ToString();
                        grid_sales.Rows[RowIndex].Cells["category_code"].Value = myProductView["category_code"].ToString();
                        grid_sales.Rows[RowIndex].Cells["btn_delete"].Value = "Del";

                        grid_sales.Rows[RowIndex].Cells["tax_id"].Value = myProductView["tax_id"].ToString();
                        grid_sales.Rows[RowIndex].Cells["tax_rate"].Value = myProductView["tax_rate"].ToString();
                        grid_sales.Rows[RowIndex].Cells["item_type"].Value = myProductView["item_type"].ToString();

                        grid_sales.Rows[RowIndex].Cells["shop_qty"].Value = myProductView["qty"].ToString();
                        grid_sales.Rows[RowIndex].Cells["item_number"].Value = myProductView["item_number"].ToString();

                        /////
                        //fill_locations_grid_combo(RowIndex, "", myProductView["id"].ToString());
                        ////

                        if (Convert.ToDouble(myProductView["qty"]) <= 0 || myProductView["qty"].ToString() == string.Empty)
                        {
                            grid_sales.Rows[RowIndex].DefaultCellStyle.ForeColor = Color.Red;
                        }

                    }
                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_cost_amount();
                    get_total_amount();
                    get_total_qty();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("An error occurred: " + ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        public void fill_locations_grid_combo(int RowIndex, string SelectedValue = "DEF", string product_id = "")
        {
            DataTable dt = new DataTable();
            var locationComboCell = new DataGridViewComboBoxCell();
            GeneralBLL generalBLL_obj = new GeneralBLL();
            string keyword = "loc_code as location_code";
            string table = "pos_product_stocks WHERE item_id=" + product_id + " AND  qty > 0 GROUP BY loc_code";

            dt = generalBLL_obj.GetRecord(keyword, table);

            //WHEN NO LOCATION ASSIGNED TO PRODUCT THEN ALL LOCATIONS SHALL BE LOADED
            if (dt.Rows.Count <= 0)
            {
                string keyword1 = "L.code as location_code,L.name";
                string table1 = "pos_locations L";

                dt = generalBLL_obj.GetRecord(keyword1, table1);

            }
            ///////////

            locationComboCell.DataSource = dt;
            locationComboCell.DisplayMember = "location_code";
            locationComboCell.ValueMember = "location_code";

            grid_sales.Rows[RowIndex].Cells["location_code"] = locationComboCell;
            //grid_sales.Rows[RowIndex].Cells["location_code"].Value = SelectedValue;
            grid_sales.Rows[RowIndex].Cells["location_code"].Value = dt.Rows[0]["location_code"].ToString(); // GET FIRST COLUMN OF DT TO SHOW FIRST VALUE AS SELECTED


        }

        private void txt_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_barcode.Text != string.Empty && e.KeyCode == Keys.Enter)
            {
                load_products("", "", txt_barcode.Text.Trim());
                txt_barcode.Text = "";

            }
            txt_barcode.Focus();
        }

        public void load_products(string item_number = "", string product_name = "", string barcode = "")
        {

            DataTable product_dt = new DataTable();

            if (item_number != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductNumber(item_number);
            }

            if (product_name != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductName(product_name);
            }

            if (barcode != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByBarcode(barcode);
            }

            if (product_dt.Rows.Count > 0)
            {

                foreach (DataRow myProductView in product_dt.Rows)
                {
                    double qty = Convert.ToDouble(myProductView["sale_demand_qty"].ToString() == string.Empty || (decimal)myProductView["sale_demand_qty"] == 0 ? "1" : myProductView["sale_demand_qty"].ToString());
                    double total = qty * double.Parse(myProductView["unit_price"].ToString());
                    double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                    double tax = (total * tax_rate / 100);
                    double sub_total = tax + total;
                    double sub_total_without_vat = total;

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();
                    double cost_price = Math.Round(Convert.ToDouble(myProductView["avg_cost"]), 2);
                    double unit_price = Math.Round(Convert.ToDouble(myProductView["unit_price"]), 3);
                    double discount = 0.00;
                    double discount_percent = 0.00;
                    string location_code = myProductView["location_code"].ToString();
                    string unit = myProductView["unit"].ToString();
                    string category = myProductView["category"].ToString();
                    string btn_delete = "Del";

                    string shop_qty = myProductView["qty"].ToString();
                    string tax_id = myProductView["tax_id"].ToString();
                    string item_type = myProductView["item_type"].ToString();
                    string category_code = myProductView["category_code"].ToString();
                    string grid_item_number = myProductView["item_number"].ToString();
                    double current_sub_total = Convert.ToDouble(qty) * unit_price + tax - discount;


                    string[] row0 = { "", id.ToString(), code, name, qty.ToString(), unit_price.ToString(), discount.ToString(), discount_percent.ToString(),
                    sub_total_without_vat.ToString(), tax.ToString(), current_sub_total.ToString(), location_code, unit, category,
                    btn_delete, shop_qty, tax_id.ToString(), tax_rate.ToString(), cost_price.ToString(),
                    item_type, category_code, grid_item_number};

                    //Remove the first empty row
                    if (grid_sales.RowCount > 0 && grid_sales.Rows[0].Cells["id"].Value == null)
                    {
                        grid_sales.Rows.RemoveAt(0);
                    }
                    //
                    int RowIndex = grid_sales.Rows.Add(row0);

                    if (Convert.ToDouble(myProductView["qty"]) <= 0 || myProductView["qty"].ToString() == string.Empty)
                    {
                        grid_sales.CurrentRow.DefaultCellStyle.ForeColor = Color.Red;
                    }

                    //GET / SET Location Dropdown list
                    /////
                    //fill_locations_grid_combo(RowIndex,"",myProductView["id"].ToString());
                    //////////

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_cost_amount();
                    get_total_amount();
                    get_total_qty();

                }

                txt_barcode.Focus();

            }
            else
            {
                UiMessages.ShowWarning("Record not found", "لم يتم العثور على سجل", "Products", "المنتجات");
            }
        }

        private void get_sub_total_amount()
        {
            total_sub_total = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_sub_total += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value);
            }

            txt_sub_total.Text = Math.Round(total_sub_total, 2).ToString();
            txt_sub_total_2.Text = Math.Round((total_sub_total - total_discount), 2).ToString();
        }

        private void get_total_cost_amount()
        {
            total_cost_amount = 0;
            total_cost_amount_e_vat = 0;
            double total_cost = 0;
            double tax_rate = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                //if tax is not assigned with product then assign zero
                if (grid_sales.Rows[i].Cells["tax_rate"].Value == null || grid_sales.Rows[i].Cells["tax_rate"].Value == DBNull.Value || String.IsNullOrWhiteSpace(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()))
                {
                    tax_rate = 0;
                }
                else
                {
                    tax_rate = double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString());
                }
                ////

                total_cost = Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["cost_price"].Value);

                total_cost_amount += total_cost;
                total_cost_amount_e_vat += (total_cost + (total_cost * tax_rate / 100));
            }

        }

        private void get_total_amount()
        {
            total_amount = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_amount += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value);
            }
            double net = (total_amount + total_tax - total_discount);
            txt_total_amount.Text = Math.Round(net, 2).ToString();
            double customerBalance = (txt_cust_balance.Text == string.Empty ? 0 : Convert.ToDouble(txt_cust_balance.Text));
            double customer_credit_limit = (txt_cust_credit_limit.Text == "" ? 0 : Convert.ToDouble(txt_cust_credit_limit.Text));
            double netAmount = (txt_total_amount.Text == string.Empty ? 0 : Convert.ToDouble(txt_total_amount.Text));
            double netCreditLimit = customer_credit_limit - customerBalance;
            double limitExceededBy = netAmount - netCreditLimit;

            ///Checking customer credit limit
            if (txt_cust_credit_limit.Text != "")
            {
                if (cmb_sale_type.SelectedValue.ToString() == "Credit" && netAmount > netCreditLimit)
                {
                    UiMessages.ShowWarning("Sales transaction cannot be processed, because customer credit limit has exceeded by " + limitExceededBy.ToString("N2"),
                     "لا يمكن حفظ معاملة البيع، لأنه تم تجاوز حد ائتمان العميل بمقدار " + limitExceededBy.ToString("N2"),
                     "Credit limit",
                     "حد الائتمان");
                    return;
                }
            }
            ///

        }

        private void get_total_tax()
        {
            total_tax = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_tax += Convert.ToDouble(grid_sales.Rows[i].Cells["tax"].Value);
            }

            txt_total_tax.Text = Math.Round(total_tax, 2).ToString();
        }

        private void get_total_discount()
        {
            total_discount = 0;
            //double flatDiscountValue = Convert.ToDouble(txtTotalFlatDiscountValue.Value);
            //double new_total_discount =Convert.ToDouble(txt_total_disc_percent.Value);
            // double flatDiscountPercent = (total_amount * new_total_discount / 100);

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_discount += Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);
            }
            // total_discount += flatDiscountValue;
            //total_discount += flatDiscountPercent;

            txt_total_discount.Text = Math.Round(total_discount, 2).ToString();

            //double total_disc_percent = (string.IsNullOrEmpty(txt_total_disc_percent.Text) ? 0 : Convert.ToDouble(txt_total_disc_percent.Text));
            //if (total_disc_percent == 0)
            //{
            //    txt_total_discount.Text = Math.Round(total_discount,2).ToString();
            //}


        }

        private void get_total_qty()
        {
            double total_qty = 0;

            for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
            {
                total_qty += Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);
            }

            txt_total_qty.Text = Math.Round(total_qty, 2).ToString();
        }

        private int Insert_emp_commission(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description, int employee_id = 0)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            EmployeeBLL emp_Obj = new EmployeeBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;
            JournalsModal_obj.employee_id = employee_id;

            journal_id = emp_Obj.InsertEmpCommission(JournalsModal_obj);
            return journal_id;
        }

        private int Insert_user_commission(string invoice_no, int account_id, double debit, double credit, DateTime date,
            string description)
        {
            int journal_id = 0;
            JournalsModal JournalsModal_obj = new JournalsModal();
            UsersBLL emp_Obj = new UsersBLL();

            JournalsModal_obj.invoice_no = invoice_no;
            JournalsModal_obj.entry_date = date;
            JournalsModal_obj.debit = debit;
            JournalsModal_obj.credit = credit;
            JournalsModal_obj.account_id = account_id;
            JournalsModal_obj.description = description;

            journal_id = emp_Obj.InsertUserCommission(JournalsModal_obj);
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
                cash_account_id = (int)dr["cash_acc_id"];
                sales_account_id = (int)dr["sales_acc_id"];
                receivable_account_id = (int)dr["receivable_acc_id"];
                tax_account_id = (int)dr["tax_acc_id"];
                sales_discount_acc_id = (int)dr["sales_discount_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
                purchases_acc_id = (int)dr["purchases_acc_id"];
            }
        }

        public DataTable load_sales_receipt(string invoice_no)
        {
            //bind data in data grid view  
            SalesBLL objSalesBLL = new SalesBLL();
            DataTable dt = objSalesBLL.SaleReceipt(invoice_no);
            return dt;

        }

        public DataTable load_estiamte_receipt(string invoice_no)
        {
            //bind data in data grid view  
            SalesBLL objSalesBLL = new SalesBLL();
            DataTable dt = objSalesBLL.EstimateReceipt(invoice_no);
            return dt;

        }

        private void frm_sales_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                //if (e.KeyData == Keys.Enter)
                //{
                //    SendKeys.Send("{TAB}");
                //}
                if (e.KeyData == Keys.F1)
                {
                    NewToolStripButton.PerformClick();
                }
                if (e.KeyData == Keys.F3)
                {
                    SaveToolStripButton.PerformClick();
                }
                if (e.Control && e.KeyCode == Keys.H)
                {
                    HistoryToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F4)
                {
                    SearchToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F5)
                {
                    AmountFixToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F6)
                {
                    //chk_print_invoice.Checked = !chk_print_invoice.Checked;
                }
                if (e.KeyCode == Keys.F9)
                {
                    grid_sales.Focus();
                }
                if (e.KeyCode == Keys.Escape)
                {

                }
                if (e.KeyData == Keys.Down)
                {
                    brandsDataGridView.Focus();
                    categoriesDataGridView.Focus();
                    groupsDataGridView.Focus();
                    customersDataGridView.Focus();
                }
                if (e.Control && e.KeyCode == Keys.O)
                {
                    SearchToolStripButton.PerformClick();
                }
                if (e.Control && e.KeyCode == Keys.L)
                {
                    frm_search_estimates obj_estimate = new frm_search_estimates(this);
                    obj_estimate.ShowDialog();
                }
                if (e.Control && e.Alt && e.KeyCode == Keys.B)
                {
                    txt_barcode.Focus();
                }
                if (e.Control && e.Alt && e.KeyCode == Keys.I)
                {
                    txt_invoice_no.Focus();
                }

                if (e.Control && e.Alt && e.KeyCode == Keys.C)
                {
                    txtCustomerSearch.Focus();
                }
                if(e.Control && e.Alt && e.KeyCode == Keys.P)
                {
                    PrinttoolStripButton.PerformClick();
                }
                if (e.Control && e.Alt && e.KeyCode == Keys.R)
                {
                    SaleReturnToolStripButton.PerformClick();
                }
                if(e.Control && e.Alt && e.KeyCode == Keys.N)
                {
                    cmb_employees.Focus();
                }
                if (e.Control && e.Alt && e.KeyCode == Keys.T)
                {
                    cmb_sale_type.Focus();
                }
                if (e.Control && e.Alt && e.KeyCode == Keys.U)
                {
                    chk_show_total_cost.Checked = !chk_show_total_cost.Checked;
                }
                if (e.Control && e.Alt && e.KeyCode == Keys.Z)
                {
                    // Permission check for toggling send-to-zatca
                    if (_auth.HasPermission(_currentUser, AppPermissions.Sales_Zatca_Transmit))
                    {
                        chk_sendInvoiceToZatca.Checked = !chk_sendInvoiceToZatca.Checked;
                    }
                    else
                    {
                        UiMessages.ShowWarning(
                            "You don't have permission to transmit invoices to ZATCA.",
                            "ليس لديك صلاحية لإرسال الفواتير إلى زاتكا.",
                            "Permission Denied",
                            "تم رفض الصلاحية");
                    }
                }

                if (e.Control && e.Alt && e.KeyCode == Keys.S)
                {
                    using (var f = new frm_small_sale_settings())
                        f.ShowDialog(this);
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void grid_sales_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_sales.Columns[e.ColumnIndex].Name;
                if (name == "btn_delete")
                {
                    grid_sales.Rows.RemoveAt(e.RowIndex);

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_cost_amount();
                    get_total_amount();
                    get_total_qty();

                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }

        }

        private void chkbox_is_taxable_CheckedChanged(object sender, EventArgs e)
        {
            if (chkbox_is_taxable.Checked == false)
            {
                txt_total_tax.Text = "0.00";
                double netAmount = (total_amount + total_tax - total_discount);
                netAmount -= total_tax;
                txt_total_amount.Text = netAmount.ToString();
            }
            else
            {
                txt_total_tax.Text = total_tax.ToString();
                double netAmount = (total_amount + total_tax - total_discount);
                //netAmount += total_tax;
                txt_total_amount.Text = netAmount.ToString();
            }
        }

        private void clear_form()
        {
            Get_user_total_commission();
            // grid_sales.DataSource = null;
            grid_sales.Rows.Clear();
            grid_sales.Refresh();
            grid_sales.Rows.Add();

            txt_description.Text = "";
            PrinttoolStripButton.Enabled = false;
            SaleReturnToolStripButton.Enabled = false;
            //cmb_brands.SelectedValue = 0;
            //cmb_customers.Refresh();

            txt_group_code.Text = "";
            txt_groups.Text = "";
            txt_category_code.Text = "";
            txt_categories.Text = "";
            txt_brands.Text = "";
            txt_brand_code.Text = "";

            invoice_status = "";
            //btn_save.Text = "Save (F3)";
            txt_invoice_no.Text = "";

            total_amount = 0;
            total_discount = 0;
            total_tax = 0;
            total_sub_total = 0;
            total_cost_amount = 0;
            total_cost_amount_e_vat = 0;

            txt_sale_date.Refresh();
            txt_sale_date.Value = DateTime.Now;

            txtCustomerSearch.Text = "";
            txt_customerID.Text = "";
            txt_cust_balance.Text = "";
            txt_cust_credit_limit.Text = "";
            txt_customer_vat.Text = "";

            txt_sub_total.Text = "0.00";
            txt_total_amount.Text = "0.00";
            txt_total_tax.Text = "0.00";
            txt_total_discount.Text = "0.00";
            txt_total_amount.Text = "0.00";

            txt_total_disc_percent.Value = 0;
            txtTotalFlatDiscountValue.Value = 0;

            txt_total_qty.Text = "";
            txt_sub_total_2.Text = "";

            txt_order_qty.Text = "";
            txt_total_cost.Text = "";
            txt_cost_price.Text = "";
            txt_cost_price_with_vat.Text = "";
            txt_single_cost_evat.Text = "";
            txt_single_cost_evat.Text = "";

            txt_shop_qty.Text = "";
            txt_company_qty.Text = "";
            txtPONumber.Text = "";
            txt_user_commission_balance.Text = "";

            cmb_employees.SelectedValue = 0;
            cmb_sale_type.SelectedValue = "Cash";
            cmb_customers.SelectedValue = 0;
            //cmb_categories.SelectedValue = 0;

            this.ActiveControl = grid_sales;

        }

        private void btn_new_Click(object sender, EventArgs e)
        {

        }

        private void txt_customer_id_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        public void get_customers_dropdownlist()
        {
            CustomerBLL customerBLL = new CustomerBLL();
            DataTable customers = customerBLL.GetAll();

            DataRow emptyRow = customers.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "";              // Set Column Value
            customers.Rows.InsertAt(emptyRow, 0);

            DataRow emptyRow1 = customers.NewRow();
            emptyRow1[0] = "-1";              // Set Column Value
            emptyRow1[2] = "ADD NEW";              // Set Column Value
            customers.Rows.InsertAt(emptyRow1, 1);

            cmb_customers.DisplayMember = "first_name";
            cmb_customers.ValueMember = "id";
            cmb_customers.DataSource = customers;


        }

        public void get_employees_dropdownlist()
        {
            EmployeeBLL employeeBLL = new EmployeeBLL();
            DataTable employees = employeeBLL.GetAll();

            DataRow emptyRow = employees.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[2] = "Select Employee";              // Set Column Value
            employees.Rows.InsertAt(emptyRow, 0);


            cmb_employees.DisplayMember = "first_name";
            cmb_employees.ValueMember = "id";
            cmb_employees.DataSource = employees;


        }

        public void get_payment_terms_dropdownlist()
        {
            PaymentTermsBLL paymentMethodBLL = new PaymentTermsBLL();

            DataTable payment_terms = paymentMethodBLL.GetAll();
            DataRow emptyRow = payment_terms.NewRow();
            emptyRow[0] = 0;              // Set Column Value
            emptyRow[4] = "";              // Set Column Value
            payment_terms.Rows.InsertAt(emptyRow, 0);


            cmb_payment_terms.DisplayMember = "description";
            cmb_payment_terms.ValueMember = "id";
            cmb_payment_terms.DataSource = payment_terms;


        }

        public void get_payment_method_dropdownlist()
        {
            PaymentMethodBLL paymentMethodBLL = new PaymentMethodBLL();

            DataTable payment_method = paymentMethodBLL.GetAll();
            DataRow emptyRow = payment_method.NewRow();
            //emptyRow[0] = 0;              // Set Column Value
            //emptyRow[1] = "";              // Set Column Value
            //payment_method.Rows.InsertAt(emptyRow, 0);


            cmb_payment_method.DisplayMember = "description";
            cmb_payment_method.ValueMember = "id";
            cmb_payment_method.DataSource = payment_method;


        }
        private void cmb_customers_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //cmb_sale_type.SelectedValue = (allow_credit_sales ? "Credit" : "Cash"); //if user has no right then select cash instead

                if (cmb_customers.SelectedValue != null && cmb_customers.SelectedValue.ToString() != "0")
                {
                    int customer_id = Convert.ToInt32(cmb_customers.SelectedValue.ToString());

                    CustomerBLL customerBLL_obj = new CustomerBLL();
                    DataTable customers = customerBLL_obj.SearchRecordByCustomerID(customer_id);


                    foreach (DataRow dr in customers.Rows)
                    {
                        txt_customer_vat.Text = dr["vat_no"].ToString();

                        txt_cust_credit_limit.Text = dr["credit_limit"].ToString();
                    }

                    ///customer balance
                    decimal customer_total_balance = customerBLL_obj.GetCustomerAccountBalance(customer_id);
                    ///
                    txt_cust_balance.Text = customer_total_balance.ToString();
                }


                if (cmb_customers.SelectedValue != null && cmb_customers.SelectedValue.ToString() == "-1")
                {
                    frm_addCustomer custfrm = new frm_addCustomer();
                    custfrm.ShowDialog();


                    get_customers_dropdownlist();
                }
            }


            catch (Exception ex)
            {
                UiMessages.ShowError("An error occurred: " + ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        public void round_total_amount(double new_amount, double old_total_amount, double sub_total)
        {
            try
            {
                //double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
                //double total_item_share = Math.Round(total_diff_amount / sub_total * 100,2);
                //if (total_diff_amount != 0)
                //{
                //    if(radioDiscValue.Checked)
                //    {
                //        txtTotalFlatDiscountValue.Value += Convert.ToDecimal(total_diff_amount);

                //    }else if(radioDiscPercent.Checked)
                //    {
                //        txt_total_disc_percent.Value += Convert.ToDecimal(total_item_share);
                //    }
                //}

                double total_rows = 0;
                //txt_total_amount.Text = round_total_amount.ToString();
                for (int i = 0; i <= grid_sales.Rows.Count - 1; i++)
                {
                    if (grid_sales.Rows[i].Cells["qty"].Value != null)
                    {
                        total_rows++;
                    }
                }

                double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
                double total_item_share = sub_total * 100 / old_total_amount;
                double total_tax_share = (old_total_amount - sub_total) * 100 / old_total_amount;
                double diff_amount_per_item = total_diff_amount / total_rows;

                double new_amount_total = 0;
                double new_amount_single = 0;
                double new_vat_total = 0;
                double net_total = 0;

                if (total_diff_amount != 0)
                {
                    for (int i = 0; i <= total_rows - 1; i++)
                    {
                        if (grid_sales.Rows[i].Cells["qty"].Value != null)
                        {
                            new_amount_single = (double.Parse(grid_sales.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString()));
                            new_amount_total = new_amount_single * total_item_share / 100;
                            new_vat_total = new_amount_single * total_tax_share / 100;
                            net_total = (grid_sales.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                            grid_sales.Rows[i].Cells["unit_price"].Value = net_total;

                            double tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));

                            //grid_sales.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_sales.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                            double total_value = Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);

                            double tax_1 = ((total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                            double sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                            grid_sales.Rows[i].Cells["sub_total"].Value = sub_total_1;
                            //grid_sales.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                            grid_sales.Rows[i].Cells["tax"].Value = (tax_1);
                        }

                    }

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_cost_amount();
                    get_total_amount();

                }
            }
            catch (Exception ex)
            {

                UiMessages.ShowError("An error occurred: " + ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void get_saletype_dropdownlist()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");

            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "Cash";
            if (lang == "en-US")
            {
                _row_1["name"] = "Cash";
            }
            else if (lang == "ar-SA")
            {
                _row_1["name"] = "نقدي";
            }

            dt.Rows.Add(_row_1);

            if (allow_credit_sales)//user right check
            {
                DataRow _row = dt.NewRow();
                _row["id"] = "Credit";
                if (lang == "ar-SA")
                {
                    _row["name"] = "اجل";
                }
                else { _row["name"] = "Credit"; }

                dt.Rows.Add(_row);

            }

            DataRow _row_2 = dt.NewRow();
            _row_2["id"] = "Quotation";
            if (lang == "ar-SA")
            {
                _row_2["name"] = "عرض سعر";
            }
            else { _row_2["name"] = "Quotation"; }

            dt.Rows.Add(_row_2);

            DataRow _row_3 = dt.NewRow();
            _row_3["id"] = "Gift";

            if (lang == "ar-SA")
            {
                _row_3["name"] = "هدية";
            }
            else { _row_3["name"] = "Gift"; }

            dt.Rows.Add(_row_3);

            DataRow _row_4 = dt.NewRow();
            _row_4["id"] = "ICT";

            if (lang == "ar_SA")
            {
                _row_4["name"] = "نقل قطع الغيار بين الشركات";
            }
            else
            {
                _row_4["name"] = "ICT";
            }
            dt.Rows.Add(_row_4);


            //DataRow _row_4 = dt.NewRow();
            //_row_4["id"] = "Return";
            //if (lang == "en-US")
            //{
            //    _row_4["name"] = "Return";
            //}
            //else if (lang == "ar-SA")
            //{
            //    _row_4["name"] = "يعود";
            //}
            //dt.Rows.Add(_row_4);

            cmb_sale_type.DisplayMember = "name";
            cmb_sale_type.ValueMember = "id";
            cmb_sale_type.DataSource = dt;

            cmb_sale_type.SelectedIndex = 0; //default value

        }
        private void get_invoice_subtype_dropdownlist()
        {
            DataTable dt = new DataTable();
            dt.Clear();
            dt.Columns.Add("id");
            dt.Columns.Add("name");
            DataRow _row_1 = dt.NewRow();
            _row_1["id"] = "02";

            if (lang == "ar-SA")
            {
                _row_1["name"] = "مبسطة";
            }
            else { _row_1["name"] = "Simplified"; }
            dt.Rows.Add(_row_1);

            DataRow _row_2 = dt.NewRow();
            _row_2["id"] = "01";

            if (lang == "ar-SA")
            {
                _row_2["name"] = "ضريبية";
            }
            else { _row_2["name"] = "Standard"; }
            dt.Rows.Add(_row_2);

            cmb_invoice_subtype_code.DisplayMember = "name";
            cmb_invoice_subtype_code.ValueMember = "id";
            cmb_invoice_subtype_code.DataSource = dt;

            cmb_invoice_subtype_code.SelectedIndex = 0; //default value
        }

        public void load_user_rights(int user_id)
        {
            UsersBLL userBLL_obj = new UsersBLL();
            DataTable users = userBLL_obj.GetUserRights(user_id);

            foreach (DataRow dr in users.Rows)
            {
                ///USER RIGHTS

                cash_sales_amount_limit = Convert.ToDouble(dr["cash_sales_amount"]);
                //txt_credit_sales_amt= (double) dr["credit_sales_amount"].ToString();
                //cash_purchase_amount_limit = Convert.ToDouble(dr["cash_purchase_amount"]);
                //txt_credit_purchase_amt.Text = (double) dr["credit_purchase_amount"].ToString();
                //allow_cash_sales.Checked = (bool)dr["allow_cash_sales"];
                allow_credit_sales = (bool)dr["allow_credit_sales"];
                //chk_allow_cash_purchase.Checked = (bool)dr["allow_cash_purchase"];
                //allow_credit_purchase= (bool)dr["allow_credit_purchase"];

            }
        }

        private void grid_sales_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    var confirm = UiMessages.ConfirmYesNoCancel("Are you sure you want delete", "هل أنت متأكد أنك تريد الحذف", "Delete", "حذف");
                    if (confirm == DialogResult.Yes)
                    {
                        grid_sales.Rows.RemoveAt(grid_sales.CurrentRow.Index);

                        get_total_tax();
                        get_total_discount();
                        get_sub_total_amount();
                        get_total_cost_amount();
                        get_total_amount();
                        get_total_qty();
                    }
                }

                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    e.SuppressKeyPress = true;
                    int iColumn = grid_sales.CurrentCell.ColumnIndex;
                    int iRow = grid_sales.CurrentCell.RowIndex;

                    int snoIdx = grid_sales.Columns["sno"].Index;
                    int idIdx = grid_sales.Columns["id"].Index;
                    int codeIdx = grid_sales.Columns["code"].Index;

                    if (iColumn == snoIdx || iColumn == idIdx)
                    {
                        grid_sales.CurrentCell = grid_sales.Rows[iRow].Cells[codeIdx];
                        grid_sales.Focus();
                        grid_sales.CurrentCell.Selected = true;
                        grid_sales.BeginEdit(true);
                    }
                    else if (iColumn <= 9)
                    {
                        if (grid_sales.Rows[iRow].Cells["code"].Value != null && grid_sales.Rows[iRow].Cells["unit_price"].Value != null && grid_sales.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_sales.CurrentCell = grid_sales.Rows[iRow].Cells[iColumn + 1];
                            grid_sales.Focus();
                            grid_sales.CurrentCell.Selected = true;
                            //grid_sales.BeginEdit(true);
                        }
                    }
                    else if (iColumn > 9)
                    {
                        if (grid_sales.Rows[iRow].Cells["code"].Value != null && grid_sales.Rows[iRow].Cells["unit_price"].Value != null && grid_sales.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_sales.Rows.Add();  //adds new row on last cell of row
                            this.ActiveControl = grid_sales;
                            grid_sales.CurrentCell = grid_sales.Rows[iRow + 1].Cells["code"];
                            //grid_sales.Rows[iRow + 1].Cells["code"].Value = product_code;
                            grid_sales.CurrentCell.Selected = true;
                            grid_sales.BeginEdit(true);
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError("An error occurred: " + ex.Message, "خطأ", "Error", "خطأ");
            }

        }

        private void txt_invoice_no_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_invoice_no.Text != "" && e.KeyCode == Keys.Enter)
                {
                    string invoice_no = txt_invoice_no.Text;
                    string invoice_chr = invoice_no.Substring(0, 1);
                    SalesBLL salesObj = new SalesBLL();
                    DataTable estimates_dt = new DataTable();

                    if (invoice_chr.ToUpper() == "S")
                    {
                        estimates_dt = salesObj.GetSaleAndSalesItems(invoice_no);
                        Load_products_to_grid_by_invoiceno(estimates_dt, invoice_no);
                    }
                    else
                    {
                        estimates_dt = salesObj.GetEstimatesAndEstimatesItems(invoice_no);
                        Load_products_to_grid_by_invoiceno(estimates_dt, invoice_no);
                    }
                    txt_invoice_no.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        public void Load_products_to_grid_by_invoiceno(DataTable _dt, string invoice_no)
        {
            try
            {
                grid_sales.Rows.Clear();
                grid_sales.Refresh();
                txt_invoice_no.Text = invoice_no;

                string invoice_chr = invoice_no.Substring(0, 1);

                if (invoice_chr.ToUpper() == "S" || invoice_chr.ToUpper() == "Z")// for invoice edit
                {
                    invoice_status = "Update"; //"Estimate";// 
                    //btn_save.Text = "Update (F3)"; //"Save"; //
                    PrinttoolStripButton.Enabled = true;
                    SaleReturnToolStripButton.Enabled = true;
                }
                else// for estimates 
                {
                    invoice_status = "Estimate";
                    //btn_save.Text = "Sale (F3)";
                    //cmb_invoice_type.Enabled = false;

                }

                if (_dt.Rows.Count > 0)
                {
                    CustomerBLL customerBLL = new CustomerBLL();

                    foreach (DataRow myProductView in _dt.Rows)
                    {
                        // inside Load_products_to_grid_by_invoiceno(...) where the selected lines are
                        _suppressCustomerSearch = true;
                        _customerSearchDebounceTimer.Stop();
                        
                        txtCustomerSearch.Text = myProductView["customer_name"].ToString();
                        txt_customerID.Text = myProductView["customer_id"].ToString();
                        txt_customer_vat.Text = myProductView["vat_no"].ToString();
                        txt_cust_credit_limit.Text = myProductView["credit_limit"].ToString();
                        
                        Decimal customer_total_balance = customerBLL.GetCustomerAccountBalance(Convert.ToInt32(txt_customerID.Text));
                        txt_cust_balance.Text = customer_total_balance.ToString("N2");
                        
                        _suppressCustomerSearch = false;
                        // NEW: Load invoice subtype saved in DB (do NOT override it)
                        string savedSubtype = null;
                        if (_dt.Columns.Contains("invoice_subtype"))
                            savedSubtype = Convert.ToString(myProductView["invoice_subtype"]);
                        else if (_dt.Columns.Contains("invoice_subtype_code"))
                            savedSubtype = Convert.ToString(myProductView["invoice_subtype_code"]);

                        if (!string.IsNullOrWhiteSpace(savedSubtype) && cmb_invoice_subtype_code.DataSource != null)
                        {
                            // Expecting "01" or "02"
                            cmb_invoice_subtype_code.SelectedValue = savedSubtype.Trim();
                        }
                        else
                        {
                            // fallback rule if DB column not present
                            ApplyInvoiceSubtypeForCustomerSelection();
                        }

                        cmb_employees.SelectedValue = myProductView["employee_id"];
                        cmb_sale_type.SelectedValue = myProductView["sale_type"];
                        //txt_sale_date.Value = Convert.ToDateTime(myProductView["sale_date"].ToString());
                        txt_description.Text = myProductView["description"].ToString();
                        total_amount = Convert.ToDouble(myProductView["total_amount"].ToString());
                        txt_total_disc_percent.Value = (myProductView["total_disc_percent"] == null || myProductView["total_disc_percent"].ToString() == "" ? 0 : Math.Round(Convert.ToDecimal(myProductView["total_disc_percent"]), 2));
                        txtTotalFlatDiscountValue.Value = (myProductView["flatDiscountValue"] == null || myProductView["flatDiscountValue"].ToString() == "" ? 0 : Math.Round(Convert.ToDecimal(myProductView["flatDiscountValue"]), 2));

                        double qty = Convert.ToDouble(myProductView["quantity_sold"].ToString());
                        double discount = Math.Round(Convert.ToDouble(myProductView["discount_value"]), 3);
                        double total = qty * double.Parse(myProductView["unit_price"].ToString());
                        double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                        double tax = ((total - discount) * tax_rate / 100);
                        double sub_total = tax + total;
                        double sub_total_without_vat = total - discount;

                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        double cost_price = Math.Round(Convert.ToDouble(myProductView["cost_price"]), 2);
                        double unit_price = Math.Round(Convert.ToDouble(myProductView["unit_price"]), 3);

                        double total_value = Convert.ToDouble(myProductView["unit_price"]) * Convert.ToDouble(myProductView["quantity_sold"].ToString());
                        double discount_percent = Convert.ToDouble(myProductView["discount_percent"]); //Convert.ToDouble(myProductView["discount_value"]) / total_value * 100;

                        string location_code = myProductView["loc_code"].ToString();
                        string unit = myProductView["unit"].ToString();
                        string category = myProductView["category"].ToString();
                        string item_type = myProductView["item_type"].ToString();
                        string btn_delete = "Del";
                        string tax_id = myProductView["tax_id"].ToString();

                        string shop_qty = "0"; // myProductView["qty"].ToString();
                        string category_code = myProductView["category_code"].ToString();
                        string item_number = myProductView["item_number"].ToString();

                        double current_sub_total = Convert.ToDouble(qty) * unit_price + tax - discount;


                        string[] row0 = { "", id.ToString(), code, name, qty.ToString(), unit_price.ToString(), discount.ToString(), discount_percent.ToString(),
                                            sub_total_without_vat.ToString(), tax.ToString(), current_sub_total.ToString(), location_code, unit, category,
                                            btn_delete, shop_qty, tax_id.ToString(), tax_rate.ToString(), cost_price.ToString(),
                                            item_type, category_code, item_number};
                        int rowIndex = grid_sales.Rows.Add(row0);

                        ////////
                        //fill_locations_grid_combo(rowIndex, "", id.ToString());
                        //////

                    }
                    //grid_sales.Rows.Add();

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_cost_amount();
                    get_total_amount();
                    get_total_qty();

                    grid_sales.Focus();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError("An error occurred: " + ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void addToPurchaseOrderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(grid_sales.CurrentRow.Cells["code"].Value.ToString());
                if (grid_sales.CurrentRow.Cells["code"].Value != null && grid_sales.CurrentRow.Cells["id"].Value != null)
                {
                    int id = int.Parse(grid_sales.CurrentRow.Cells["id"].Value.ToString());
                    string item_number = grid_sales.CurrentRow.Cells["item_number"].Value.ToString();
                    string name = grid_sales.CurrentRow.Cells["name"].Value.ToString();
                    string category_code = grid_sales.CurrentRow.Cells["category_code"].Value.ToString();
                    double cost_price = double.Parse(grid_sales.CurrentRow.Cells["cost_price"].Value.ToString());
                    double unit_price = double.Parse(grid_sales.CurrentRow.Cells["unit_price"].Value.ToString());


                    frm_add_porder porder_obj = new frm_add_porder(this, id, item_number, name, category_code, cost_price, unit_price);
                    porder_obj.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError("An error occurred: " + ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void grid_sales_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

            e.Control.KeyPress -= new KeyPressEventHandler(tb_KeyPress);

            if (grid_sales.CurrentCell.ColumnIndex == 4 || grid_sales.CurrentCell.ColumnIndex == 5 || grid_sales.CurrentCell.ColumnIndex == 6
                || grid_sales.CurrentCell.ColumnIndex == 7 || grid_sales.CurrentCell.ColumnIndex == 8) //qty, unit price and discount Column will accept only numeric
            {
                TextBox tb = e.Control as TextBox;
                if (tb != null)
                {
                    tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                }
            }
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            TextBox tb = sender as TextBox;

            // Allow only digits, control keys (Backspace, Delete), and one decimal point
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }

            // Prevent multiple decimal points
            if (e.KeyChar == '.' && tb.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            // Prevent entering "." as the first character
            if (e.KeyChar == '.' && tb.Text.Length == 0)
            {
                e.Handled = true;
                return;
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            HistoryToolStripButton.PerformClick();
        }

        public class Grid : DataGridView
        {
            protected override void OnHandleCreated(EventArgs e)
            {
                // Touching the TopLeftHeaderCell here prevents
                // System.InvalidOperationException:
                // This operation cannot be performed while
                // an auto-filled column is being resized.

                var topLeftHeaderCell = TopLeftHeaderCell;

                base.OnHandleCreated(e);
            }
        }

        private void chk_show_total_cost_CheckedChanged(object sender, EventArgs e)
        {
            if (chk_show_total_cost.Checked)
            {
                txt_cost_price_with_vat.Visible = true;
                lbl_cost_vat.Visible = true;
                lbl_cost_price.Visible = true;
                lbl_cost_price_evat.Visible = true;
                lbl_total_cost.Visible = true;
                txt_total_cost.Visible = true;
                txt_cost_price.Visible = true;
                txt_single_cost_evat.Visible = true;
            }
            else
            {
                //txt_total_cost.Text = "0";
                lbl_cost_price_evat.Visible = false;
                lbl_cost_vat.Visible = false;
                txt_cost_price_with_vat.Visible = false;
                txt_total_cost.Visible = false;
                lbl_cost_price.Visible = false;
                lbl_total_cost.Visible = false;
                txt_cost_price.Visible = false;
                txt_single_cost_evat.Visible = false;
            }
        }

        private void productDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_sales.RowCount > 0)
                {
                    if (grid_sales.CurrentRow.Cells["item_number"].Value != null)
                    {
                        string item_number = grid_sales.CurrentRow.Cells["item_number"].Value.ToString();

                        frm_product_full_detail obj = new frm_product_full_detail(this, null, item_number, null, null, "", true);

                        obj.ShowDialog();
                    }
                }


            }
            catch (Exception ex)
            {
                UiMessages.ShowError("An error occurred: " + ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void grid_sales_SelectionChanged(object sender, EventArgs e)
        {
            if (grid_sales.Rows.Count > 0 && grid_sales.CurrentRow.Cells["shop_qty"].Value != null && grid_sales.CurrentRow.Cells["shop_qty"].Value.ToString() != "")
            {
                string item_number = grid_sales.CurrentRow.Cells["item_number"].Value.ToString();
                double tax_rate = (grid_sales.CurrentRow.Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.CurrentRow.Cells["tax_rate"].Value.ToString()));
                if (item_number != null)
                {
                    string shop_qty = "";
                    string company_qty = "";
                    string avg_cost = "";

                    DataTable dt = productsBLL_obj.GetAllByProductByItemNumber(item_number);
                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow myProductView in dt.Rows)
                        {
                            shop_qty = myProductView["qty"].ToString();
                            company_qty = myProductView["company_qty"].ToString();
                            avg_cost = Math.Round(double.Parse(myProductView["avg_cost"].ToString()), 4).ToString();
                            //total_cost += Math.Round(Convert.ToDouble(myProductView["avg_cost"].ToString()),4);

                            if (myProductView["picture"].ToString() != "" && myProductView["picture"].ToString() != "0x")
                            {
                                byte[] myImage = new byte[0];
                                myImage = (byte[])myProductView["picture"];
                                MemoryStream stream = new MemoryStream(myImage);

                                if (stream.Length > 0)
                                {
                                    product_pic.Image = Image.FromStream(stream);

                                }
                                else
                                {
                                    product_pic.Image = null;
                                }

                            }

                        }

                        txt_shop_qty.Text = shop_qty;
                        txt_company_qty.Text = company_qty;
                        double qty = 0;
                        if (grid_sales.CurrentRow.Cells["qty"].Value != null && !string.IsNullOrWhiteSpace(grid_sales.CurrentRow.Cells["qty"].Value.ToString()))
                        {
                            double.TryParse(grid_sales.CurrentRow.Cells["qty"].Value.ToString(), out qty);
                        }

                        double unitCost = 0;
                        double.TryParse(avg_cost, out unitCost);
                        double unitVat = (unitCost * tax_rate / 100);

                        txt_cost_price.Text = Math.Round(unitCost, 3).ToString();
                        txt_single_cost_evat.Text = Math.Round((unitCost + unitVat), 3).ToString();

                        // Show invoice totals (entire grid), not row totals
                        get_total_cost_amount();
                        txt_total_cost.Text = Math.Round(total_cost_amount, 3).ToString();
                        txt_cost_price_with_vat.Text = Math.Round(total_cost_amount_e_vat, 3).ToString();

                        Purchases_orderBLL poBLL = new Purchases_orderBLL();
                        txt_order_qty.Text = poBLL.GetPOrder_qty(item_number).ToString();
                    }

                }
            }

        }

        private void grid_sales_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // Populate the frozen serial-number column automatically
            grid_sales.Rows[e.RowIndex].Cells["sno"].Value = (e.RowIndex + 1).ToString();
        }

        private void updateProductToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //MessageBox.Show(grid_sales.CurrentRow.Cells["code"].Value.ToString());
                if (grid_sales.CurrentRow.Cells["code"].Value != null)
                {
                    string product_id = grid_sales.CurrentRow.Cells["id"].Value.ToString();
                    frm_addProduct frm_addProduct_obj = new frm_addProduct(null, int.Parse(product_id), "true", null, "", this);
                    frm_addProduct_obj.WindowState = FormWindowState.Maximized;
                    frm_addProduct_obj.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError("An error occurred: " + ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void cmb_employees_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_employees.SelectedValue.ToString() != null && cmb_employees.SelectedValue.ToString() != "0")
            {
                int employee_id = Convert.ToInt32(cmb_employees.SelectedValue.ToString());

                EmployeeBLL obj = new EmployeeBLL();
                DataTable employees = obj.SearchRecordByID(employee_id);

                foreach (DataRow dr in employees.Rows)
                {
                    //txt_customer_vat.Text = dr["commission_percent"].ToString();
                    employee_commission_percent = double.Parse(dr["commission_percent"].ToString());
                }

                ///customer commission balance
                long emp_total_balance = obj.GetEmpCommissionBalance(employee_id);
                txt_cust_balance.Text = emp_total_balance.ToString();
                ///
            }
        }

        private void Get_user_total_commission()
        {
            UsersBLL obj = new UsersBLL();
            DataTable users = obj.GetUser(UsersModal.logged_in_userid);

            foreach (DataRow dr in users.Rows)
            {
                user_commission_percent = double.Parse(dr["commission_percent"].ToString());
            }

            ///customer commission balance
            long user_commission_total_balance = obj.GetUserCommissionBalance(UsersModal.logged_in_userid);
            txt_user_commission_balance.Text = user_commission_total_balance.ToString();
            ///
        }

        private void addNewRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid_sales.Rows.Add();
        }

        private void SetupBrandDataGridView()
        {
            var current_lang_code = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

            brandsDataGridView.ColumnCount = 2;
            int xLocation = groupBox_products.Location.X + txt_brands.Location.X;
            int yLocation = groupBox_products.Location.Y + txt_brands.Location.Y + 22;

            brandsDataGridView.Name = "brandsDataGridView";
            if (current_lang_code == "en-US")
            {
                brandsDataGridView.Location = new Point(xLocation, yLocation);
                brandsDataGridView.Size = new Size(250, 250);
            }
            else if (current_lang_code == "ar-SA")
            {
                brandsDataGridView.Location = new Point(xLocation, yLocation);
                brandsDataGridView.Size = new Size(250, 250);
            }

            brandsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            brandsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            brandsDataGridView.Columns[0].Name = "Code";
            brandsDataGridView.Columns[1].Name = "Name";
            brandsDataGridView.Columns[0].ReadOnly = true;
            brandsDataGridView.Columns[1].ReadOnly = true;
            brandsDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            brandsDataGridView.MultiSelect = false;
            brandsDataGridView.AllowUserToAddRows = false;
            brandsDataGridView.AllowUserToDeleteRows = false;

            brandsDataGridView.RowHeadersVisible = false;
            //brandsDataGridView.ColumnHeadersVisible = false;
            brandsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            brandsDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            brandsDataGridView.AutoResizeColumns();

            brandsDataGridView.CellClick += new DataGridViewCellEventHandler(brandsDataGridView_CellClick);
            this.brandsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(brandsDataGridView_KeyDown);

            this.Controls.Add(brandsDataGridView);
            brandsDataGridView.BringToFront();
            StyleDropdownGrid(brandsDataGridView);

        }

        void brandsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(brandsDataGridView);
                grid_sales.Focus();
            }
        }

        private void brandsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(brandsDataGridView);
            grid_sales.Focus();

        }

        private void txt_brands_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_brands.Text != "")
                {
                    SetupBrandDataGridView();

                    BrandsBLL brandsBLL_obj = new BrandsBLL();
                    string brand_name = txt_brands.Text;

                    DataTable dt = brandsBLL_obj.SearchRecord(brand_name);

                    if (dt.Rows.Count > 0)
                    {
                        brandsDataGridView.Rows.Clear();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string code = dr["code"].ToString();
                            string name = dr["name"].ToString();

                            string[] row0 = { code, name };

                            brandsDataGridView.Rows.Add(row0);
                        }
                        //brandsDataGridView.CurrentCell = brandsDataGridView.Rows[0].Cells[0];
                        brandsDataGridView.ClearSelection();
                        brandsDataGridView.CurrentCell = null;
                    }

                }
                else
                {
                    txt_brand_code.Text = "";
                    this.Controls.Remove(brandsDataGridView);
                }


            }
            catch (Exception ex)
            {

                UiMessages.ShowError(ex.Message, "خطأ", "Error", "Error");
            }
        }

        private void txt_brands_Leave(object sender, EventArgs e)
        {
            if (!brandsDataGridView.Focused)
            {
                this.Controls.Remove(brandsDataGridView);
            }
        }

        private void SetupCategoriesDataGridView()
        {
            var current_lang_code = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;
            categoriesDataGridView.ColumnCount = 2;
            int xLocation = groupBox_products.Location.X + txt_categories.Location.X + 5;
            int yLocation = groupBox_products.Location.Y + txt_categories.Location.Y + 22;

            categoriesDataGridView.Name = "categoriesDataGridView";
            if (current_lang_code == "en-US")
            {
                categoriesDataGridView.Location = new Point(xLocation, yLocation);
                categoriesDataGridView.Size = new Size(250, 250);

            }
            else if (current_lang_code == "ar-SA")
            {
                categoriesDataGridView.Location = new Point(xLocation, yLocation);
                categoriesDataGridView.Size = new Size(250, 250);
            }

            categoriesDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            categoriesDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            categoriesDataGridView.Columns[0].Name = "Code";
            categoriesDataGridView.Columns[1].Name = "Name";
            categoriesDataGridView.Columns[0].ReadOnly = true;
            categoriesDataGridView.Columns[1].ReadOnly = true;
            categoriesDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            categoriesDataGridView.MultiSelect = false;
            categoriesDataGridView.AllowUserToAddRows = false;
            categoriesDataGridView.AllowUserToDeleteRows = false;

            categoriesDataGridView.RowHeadersVisible = false;
            //brandsDataGridView.ColumnHeadersVisible = false;
            categoriesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            categoriesDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            categoriesDataGridView.AutoResizeColumns();

            this.categoriesDataGridView.CellClick += new DataGridViewCellEventHandler(categoriesDataGridView_CellClick);
            this.categoriesDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(categoriesDataGridView_KeyDown);

            this.Controls.Add(categoriesDataGridView);
            categoriesDataGridView.BringToFront();
            StyleDropdownGrid(categoriesDataGridView);

        }

        void categoriesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(categoriesDataGridView);
                grid_sales.Focus();

            }
        }

        private void categoriesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(categoriesDataGridView);
            grid_sales.Focus();

        }

        private void txt_categories_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_categories.Text != "")
                {
                    SetupCategoriesDataGridView();

                    CategoriesBLL brandsBLL_obj = new CategoriesBLL();
                    string category_name = txt_categories.Text;

                    DataTable dt = brandsBLL_obj.SearchRecord(category_name);

                    if (dt.Rows.Count > 0)
                    {
                        categoriesDataGridView.Rows.Clear();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string code = dr["code"].ToString();
                            string name = dr["name"].ToString();

                            string[] row0 = { code, name };

                            categoriesDataGridView.Rows.Add(row0);
                        }
                        //categoriesDataGridView.CurrentCell = categoriesDataGridView.Rows[0].Cells[0];
                        categoriesDataGridView.ClearSelection();
                        categoriesDataGridView.CurrentCell = null;
                    }

                }
                else
                {
                    txt_category_code.Text = "";
                    this.Controls.Remove(categoriesDataGridView);
                }


            }
            catch (Exception ex)
            {

                UiMessages.ShowError(ex.Message, "خطأ", "Error", "Error");
            }
        }

        private void txt_categories_Leave(object sender, EventArgs e)
        {
            if (!categoriesDataGridView.Focused)
            {
                this.Controls.Remove(categoriesDataGridView);
            }

        }

        private void SetupGroupsDataGridView()
        {
            var current_lang_code = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

            groupsDataGridView.ColumnCount = 2;

            int xLocation = groupBox_products.Location.X + txt_groups.Location.X;
            int yLocation = groupBox_products.Location.Y + txt_groups.Location.Y + 22;

            groupsDataGridView.Name = "groupsDataGridView";
            if (current_lang_code == "en-US")
            {
                groupsDataGridView.Location = new Point(xLocation, yLocation);
                groupsDataGridView.Size = new Size(250, 250);
            }
            else if (current_lang_code == "ar-SA")
            {
                groupsDataGridView.Location = new Point(xLocation, yLocation);
                groupsDataGridView.Size = new Size(250, 250);
            }

            groupsDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            groupsDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            groupsDataGridView.Columns[0].Name = "Code";
            groupsDataGridView.Columns[1].Name = "Name";
            groupsDataGridView.Columns[0].ReadOnly = true;
            groupsDataGridView.Columns[1].ReadOnly = true;
            groupsDataGridView.SelectionMode =
                DataGridViewSelectionMode.FullRowSelect;
            groupsDataGridView.MultiSelect = false;
            groupsDataGridView.AllowUserToAddRows = false;
            groupsDataGridView.AllowUserToDeleteRows = false;

            groupsDataGridView.RowHeadersVisible = false;
            //brandsDataGridView.ColumnHeadersVisible = false;
            groupsDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells;
            groupsDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            groupsDataGridView.AutoResizeColumns();

            this.groupsDataGridView.CellClick += new DataGridViewCellEventHandler(groupsDataGridView_CellClick);
            this.groupsDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(groupsDataGridView_KeyDown);

            this.Controls.Add(groupsDataGridView);
            groupsDataGridView.BringToFront();
            StyleDropdownGrid(groupsDataGridView);

        }

        void groupsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(groupsDataGridView);
                grid_sales.Focus();

            }
        }

        private void groupsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(groupsDataGridView);
            grid_sales.Focus();

        }

        private void txt_groups_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (txt_groups.Text != "")
                {
                    SetupGroupsDataGridView();

                    ProductGroupsBLL pg_BLL_obj = new ProductGroupsBLL();
                    string grp_name = txt_groups.Text;

                    DataTable dt = pg_BLL_obj.SearchRecordByName(grp_name);

                    if (dt.Rows.Count > 0)
                    {
                        groupsDataGridView.Rows.Clear();
                        foreach (DataRow dr in dt.Rows)
                        {
                            string code = dr["code"].ToString();
                            string name = dr["name"].ToString();

                            string[] row0 = { code, name };

                            groupsDataGridView.Rows.Add(row0);
                        }
                        //groupsDataGridView.CurrentCell = groupsDataGridView.Rows[0].Cells[0];
                        groupsDataGridView.ClearSelection();
                        groupsDataGridView.CurrentCell = null;
                    }

                }
                else
                {
                    txt_group_code.Text = "";
                    this.Controls.Remove(groupsDataGridView);
                }


            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "Error");
            }
        }

        private void txt_groups_Leave(object sender, EventArgs e)
        {
            if (!groupsDataGridView.Focused)
            {
                this.Controls.Remove(groupsDataGridView);
            }

        }

        private void link_load_estimates_Click(object sender, EventArgs e)
        {
            frm_search_estimates obj_estimate = new frm_search_estimates(this);
            obj_estimate.ShowDialog();
        }

        private void txt_amount_received_KeyUp(object sender, KeyEventArgs e)
        {
            double received_amount = (txt_amount_received.Text == "" ? 0 : double.Parse(txt_amount_received.Text));
            double total_amount = (txt_total_amount.Text == "" ? 0 : double.Parse(txt_total_amount.Text));
            txt_change_amount.Text = (total_amount - received_amount).ToString();
        }


        private void grid_sales_DataError(object sender, DataGridViewDataErrorEventArgs anError)
        {
            MessageBox.Show("Error happened " + anError.Context.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (anError.Context == DataGridViewDataErrorContexts.Commit)
            {
                MessageBox.Show("Commit error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (anError.Context == DataGridViewDataErrorContexts.CurrentCellChange)
            {
                MessageBox.Show("Cell change", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (anError.Context == DataGridViewDataErrorContexts.Parsing)
            {
                MessageBox.Show("parsing error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (anError.Context == DataGridViewDataErrorContexts.LeaveControl)
            {
                MessageBox.Show("leave control error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if ((anError.Exception) is ConstraintException)
            {
                DataGridView view = (DataGridView)sender;
                view.Rows[anError.RowIndex].ErrorText = "an error";
                view.Rows[anError.RowIndex].Cells[anError.ColumnIndex].ErrorText = "an error";

                anError.ThrowException = false;
            }
        }

        private void radioDiscValue_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDiscValue.Checked)
            {
                txtTotalFlatDiscountValue.Enabled = true;
                txt_total_disc_percent.Value = 0;
                total_discount_percent(Convert.ToDouble(txt_total_disc_percent.Value));
                txt_total_disc_percent.Enabled = false;
            }
        }

        private void radioDiscPercent_CheckedChanged(object sender, EventArgs e)
        {
            if (radioDiscPercent.Checked)
            {
                txt_total_disc_percent.Enabled = true;
                txtTotalFlatDiscountValue.Value = 0;
                total_discount_value(Convert.ToDouble(txtTotalFlatDiscountValue.Value));

                txtTotalFlatDiscountValue.Enabled = false;
            }
        }

        private void txtTotalFlatDiscountValue_TextChanged(object sender, EventArgs e)
        {
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_cost_amount();
            get_total_amount();
        }
        private void txt_total_disc_percent_TextChanged(object sender, EventArgs e)
        {
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_cost_amount();
            get_total_amount();
        }

        public void total_discount_value(double total_discount_value)
        {
            int total_rows = grid_sales.Rows.Count;
            int filled_rows = 0;

            for (int i = 0; i <= total_rows - 1; i++)
            {
                int product_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    filled_rows++;
                }
            }
            //txt_total_amount.Text = round_total_amount.ToString();
            //double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
            //double total_item_share = sub_total * 100 / old_total_amount;
            //double total_tax_share = (old_total_amount - sub_total) * 100 / old_total_amount;
            double diff_amount_per_item = total_discount_value / filled_rows;

            //double new_amount_total = 0;
            //double new_amount_single = 0;
            //double new_vat_total = 0;
            //double net_total = 0;

            double discount_percent = 0;
            double tax_1 = 0;
            double total_value = 0;
            double tax_rate = 0;
            double sub_total_1 = 0;

            for (int i = 0; i <= filled_rows - 1; i++)
            {
                int product_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    grid_sales.Rows[i].Cells["discount"].Value = diff_amount_per_item;

                    discount_percent = diff_amount_per_item / total_amount * 100;
                    grid_sales.Rows[i].Cells["discount_percent"].Value = Math.Round(discount_percent, 4).ToString();

                    //new_amount_single = (double.Parse(grid_sales.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString()));
                    //new_amount_total = new_amount_single * total_item_share / 100;
                    //new_vat_total = new_amount_single * total_tax_share / 100;
                    //net_total = (grid_sales.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                    //grid_sales.Rows[i].Cells["unit_price"].Value = net_total;

                    tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));

                    ////grid_sales.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_sales.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                    total_value = Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);

                    tax_1 = ((total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                    sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                    grid_sales.Rows[i].Cells["sub_total"].Value = sub_total_1;
                    ////grid_sales.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                    grid_sales.Rows[i].Cells["tax"].Value = (tax_1);


                }

            }
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_cost_amount();
            get_total_amount();
            get_total_qty();
        }

        private void txtTotalFlatDiscountValue_ValueChanged(object sender, EventArgs e)
        {
            total_discount_value(Convert.ToDouble(txtTotalFlatDiscountValue.Text));

            //get_total_tax();
            //get_total_discount();
            //get_sub_total_amount();
            //get_total_cost_amount();
            //get_total_amount();
        }

        public void total_discount_percent(double total_discount_percent)
        {
            int total_rows = grid_sales.Rows.Count;
            int filled_rows = 0;

            for (int i = 0; i <= total_rows - 1; i++)
            {
                int product_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    filled_rows++;
                }
            }
            //txt_total_amount.Text = round_total_amount.ToString();
            //double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
            //double total_item_share = sub_total * 100 / old_total_amount;
            //double total_tax_share = (old_total_amount - sub_total) * 100 / old_total_amount;
            double diff_percent_per_item = total_discount_percent / filled_rows;

            //double new_amount_total = 0;
            //double new_amount_single = 0;
            //double new_vat_total = 0;
            //double net_total = 0;

            double discount_value = 0;
            double tax_1 = 0;
            double total_value = 0;
            double tax_rate = 0;
            double sub_total_1 = 0;

            for (int i = 0; i <= filled_rows - 1; i++)
            {
                int product_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    grid_sales.Rows[i].Cells["discount_percent"].Value = diff_percent_per_item;

                    discount_value = total_amount * diff_percent_per_item / 100;
                    grid_sales.Rows[i].Cells["discount"].Value = Math.Round(discount_value, 4).ToString();

                    //new_amount_single = (double.Parse(grid_sales.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString()));
                    //new_amount_total = new_amount_single * total_item_share / 100;
                    //new_vat_total = new_amount_single * total_tax_share / 100;
                    //net_total = (grid_sales.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                    //grid_sales.Rows[i].Cells["unit_price"].Value = net_total;

                    tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));

                    ////grid_sales.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_sales.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                    total_value = Convert.ToDouble(grid_sales.Rows[i].Cells["unit_price"].Value) * Convert.ToDouble(grid_sales.Rows[i].Cells["qty"].Value);

                    tax_1 = ((total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                    sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_sales.Rows[i].Cells["discount"].Value);

                    grid_sales.Rows[i].Cells["sub_total"].Value = sub_total_1;
                    ////grid_sales.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                    grid_sales.Rows[i].Cells["tax"].Value = (tax_1);


                }

            }
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_cost_amount();
            get_total_amount();
            get_total_qty();

        }

        private void txt_total_disc_percent_ValueChanged(object sender, EventArgs e)
        {
            total_discount_percent(Convert.ToDouble(txt_total_disc_percent.Text));
        }

        private void frm_sales_FormClosing(object sender, FormClosingEventArgs e)
        {
            _customerSearchDebounceTimer?.Stop();

            if (grid_sales.RowCount > 0 && grid_sales.CurrentRow.Cells["code"].Value != null && grid_sales.CurrentRow.Cells["id"].Value != null)
            {
                if (MessageBox.Show(
                        UiMessages.T("Are you sure you want to close sale?", "هل أنت متأكد أنك تريد إغلاق عملية البيع؟"),
                        UiMessages.T("Close Sale Transaction", "إغلاق معاملة البيع"),
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2) == DialogResult.No)
                {
                    e.Cancel = true;
                }
            }

        }

        private void NewToolStripButton_Click(object sender, EventArgs e)
        {
            var confirm = UiMessages.ConfirmYesNo(
                    "Create new sale transaction?",
                    "هل تريد إنشاء حركة بيع جديدة؟",
                    captionEn: "Confirm",
                    captionAr: "تأكيد"
                );

            if (confirm != DialogResult.Yes)
                return;

            clear_form();
            
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, "Saving..."))
            {
                try
                {
                    // Validate at least one product is added
                    if (grid_sales.Rows.Count <= 1 && grid_sales.CurrentRow.Cells["code"].Value == null)
                    {
                        UiMessages.ShowWarning("Please add products", "يرجى إضافة منتجات", "Sale Transaction", "معاملة البيع");
                        return;
                    }

                    // Existing Standard subtype check
                    // Additional ZATCA postal address validation for Standard subtype
                    // ZATCA validation
                    if (UsersModal.useZatcaEInvoice)
                    {
                        if (cmb_invoice_subtype_code.SelectedValue != null && cmb_invoice_subtype_code.SelectedValue.ToString() == "01")
                        {
                            int customerId = 0;
                            int.TryParse(txt_customerID.Text, out customerId);
                            if (!ValidateStandardInvoiceCustomer(customerId))
                                return;
                        }
                    }

                    // Sale type selection validation
                    if (cmb_sale_type.SelectedValue.ToString() == "0")
                    {
                        UiMessages.ShowWarning("Please select sale type", "يرجى اختيار نوع البيع", "Sale Transaction", "معاملة البيع");
                        return;
                    }

                    string bankID = "";
                    string bankGLAccountID = "";
                    string sale_type;
                    string paymentMethodText = cmb_payment_method.Text;

                    // Get sale type
                    if (cmb_sale_type.SelectedValue.ToString() == null)
                    {
                        UiMessages.ShowWarning("Please select sale type", "يرجى اختيار نوع البيع", "Sale Transaction", "معاملة البيع");
                        return;
                    }
                    else
                    {
                        sale_type = (string.IsNullOrEmpty(cmb_sale_type.SelectedValue.ToString()) ? "Cash" : cmb_sale_type.SelectedValue.ToString());
                    }

                    int destination_branch_id = 0;
                    int source_branch_id = UsersModal.logged_in_branch_id;

                    if (sale_type == "Cash" && (paymentMethodText.Contains("Bank") || paymentMethodText.Contains("bank") || paymentMethodText.Contains("banks") || paymentMethodText.Contains("Banks")))
                    {
                        Master.Banks.frm_banksPopup bankfrm = new Master.Banks.frm_banksPopup();
                        bankfrm.ShowDialog();

                        if (!string.IsNullOrEmpty(bankfrm._bankIDPlusGLAccountID))
                        {
                            string bankIDPlusGLAccountID = bankfrm._bankIDPlusGLAccountID;

                            int condition_index_len = bankIDPlusGLAccountID.IndexOf("+");
                            bankID = bankIDPlusGLAccountID.Substring(0, condition_index_len).Trim();
                            bankGLAccountID = bankIDPlusGLAccountID.Substring(condition_index_len + 1).Trim();
                        }
                        else
                        {
                            return;
                        }
                    }

                    // Print options dialog
                    Frm_print_options formPrintOption = new Frm_print_options();
                    if (formPrintOption.ShowDialog() == DialogResult.OK)
                    {
                        if (sale_type == "ICT")
                        {
                            Products.ICT.frm_destination_branch obj_ict = new Products.ICT.frm_destination_branch();
                            obj_ict.ShowDialog();
                            destination_branch_id = obj_ict._branch_id;
                        }

                        ///Checking customer credit limit
                        double customer_credit_limit = (txt_cust_credit_limit.Text == string.Empty ? 0 : Convert.ToDouble(txt_cust_credit_limit.Text));
                        double customerBalance = (txt_cust_balance.Text == string.Empty ? 0 : Convert.ToDouble(txt_cust_balance.Text));
                        double netAmount = (txt_total_amount.Text == string.Empty ? 0 : Convert.ToDouble(txt_total_amount.Text));
                        double netCreditLimit = customer_credit_limit - customerBalance;

                        if (sale_type == "Credit" && netAmount > netCreditLimit)
                        {
                            UiMessages.ShowError(
                                "Cannot process sale. Customer credit limit would be exceeded!\n\n" +
                                $"Current Balance: {customerBalance:C}\n" +
                                $"Sale Amount: {netAmount:C}\n" +
                                $"Credit Limit: {customer_credit_limit:C}",
                                "Sale Transaction",
                                captionAr: "لا يمكن إتمام البيع لأن حد الائتمان للعميل سيتجاوز الحد المسموح.\n\n" +
                                $"الرصيد الحالي: {customerBalance:C}\n" +
                                $"قيمة الفاتورة: {netAmount:C}\n" +
                                $"حد الائتمان: {customer_credit_limit:C}"
                            );

                            return;
                        }
                        ////

                        if (grid_sales.Rows.Count > 0)
                        {
                            List<SalesModalHeader> sales_model_header = new List<SalesModalHeader> { };
                            List<SalesModal> sales_model_detail = new List<SalesModal> { };

                            string invoice_no = "";
                            double total_tax_var = 0;
                            Int32 tax_id = 0;
                            double tax_rate = 0;
                            string estimate_invoice_no = "";
                            bool estimate_status = false;
                            Int32 sale_id = 0;

                            SalesBLL salesObj = new SalesBLL();

                            if (chkbox_is_taxable.Checked)
                            {
                                total_tax_var = Convert.ToDouble(txt_total_tax.Text);
                            }
                            else
                            {
                                total_tax_var = 0;
                            }

                            if (txt_invoice_no.Text != "" && txt_invoice_no.Text.Substring(0, 1).ToUpper() == "E") //if estimates
                            {
                                estimate_invoice_no = txt_invoice_no.Text;
                                estimate_status = true;

                            }

                            //if purchase return then put minus sign before amount
                            double return_minus_value = (sale_type == "Return" ? -1 : 1);
                            double net_total = Math.Round(return_minus_value * total_amount, 6);
                            double net_total_discount = Math.Round(return_minus_value * total_discount, 6);
                            double net_total_tax = Math.Round(return_minus_value * total_tax, 6);

                            DateTime sale_date = txt_sale_date.Value.Date;
                            int customer_id = (string.IsNullOrWhiteSpace(txt_customerID.Text) ? 0 : Convert.ToInt32(txt_customerID.Text));
                            string customer_name = txtCustomerSearch.Text.Trim();
                            string customer_vat = txt_customer_vat.Text;
                            int employee_id = (cmb_employees.SelectedValue == null ? 0 : Convert.ToInt32(cmb_employees.SelectedValue));
                            int payment_terms_id = (cmb_payment_terms.SelectedValue == null ? 0 : Convert.ToInt32(cmb_payment_terms.SelectedValue));
                            int payment_method_id = (cmb_payment_method.SelectedValue == null ? 0 : Convert.ToInt32(cmb_payment_method.SelectedValue));
                            string invoice_subtype = (cmb_invoice_subtype_code.SelectedValue == null ? "02" : cmb_invoice_subtype_code.SelectedValue.ToString());
                            string PONumber = txtPONumber.Text;

                            double skipSaleThreshold = new SettingsBLL().GetSmallSaleThreshold(0);
                            bool isStandardSubtype = (invoice_subtype == "01");
                            bool isSimplifiedSubtype = (invoice_subtype == "02");

                            // Skip sending to ZATCA only for Simplified invoices when amount exceeds the configured threshold.
                            // Standard invoices must still be sent (clearance) when requested.
                            bool ZatcaSkipSaleInvoice = isSimplifiedSubtype
                                && (sale_type == "Cash" || sale_type == "Credit")
                                && (skipSaleThreshold != 0)
                                && (net_total >= skipSaleThreshold);
                            

                            if (invoice_status == "Update" && txt_invoice_no.Text.Substring(0, 1).ToUpper() == "S") //Update sales delete all record first and insert new sales
                            {
                                UiMessages.ShowWarning("Update are not allowed", "التعديل غير مسموح", "Update", "تعديل");
                                return;

                                //int qresult = salesObj.DeleteSales(txt_invoice_no.Text); //DELETE ALL TRANSACTIONS
                                //if (qresult == 0)
                                //{
                                //    MessageBox.Show(invoice_no + "  has issue while updating, please try again", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                //    return;
                                //}
                                //invoice_no = txt_invoice_no.Text;
                            }
                            else
                            {
                                if (sale_type == "Quotation")
                                {
                                    invoice_no = salesObj.GenerateEstimateInvoiceNo(); //GetMaxEstimateInvoiceNo();
                                }
                                else
                                {
                                    // NEW: pick invoice series based on amount
                                    invoice_no = ZatcaSkipSaleInvoice
                                        ? salesObj.GenerateZatcaSkipSalesInvoiceNo() //GetMaxSmallSaleInvoiceNo()   // ZS1-000001
                                        : salesObj.GenerateSaleInvoiceNo();  //GetMaxSaleInvoiceNo();

                                }

                            }

                            //set the date from datetimepicker and set time to te current time
                            DateTime now = DateTime.Now;
                            txt_sale_date.Value = new DateTime(txt_sale_date.Value.Year, txt_sale_date.Value.Month, txt_sale_date.Value.Day, now.Hour, now.Minute, now.Second);
                            /////////////////////

                            /////Added sales header into the List
                            sales_model_header.Add(new SalesModalHeader
                            {
                                customer_id = customer_id,
                                customer_name = customer_name,
                                customer_vat = customer_vat,
                                employee_id = employee_id,
                                invoice_no = invoice_no,
                                total_amount = net_total,
                                total_tax = total_tax_var,
                                total_discount = net_total_discount,
                                total_discount_percent = Convert.ToDouble(txt_total_disc_percent.Value),
                                flat_discount_value = Convert.ToDouble(txtTotalFlatDiscountValue.Value),
                                sale_type = sale_type,
                                invoice_subtype = invoice_subtype,
                                sale_date = sale_date,
                                sale_time = txt_sale_date.Value,
                                description = txt_description.Text,
                                payment_terms_id = payment_terms_id,
                                payment_method_id = payment_method_id,
                                payment_method_text = paymentMethodText,
                                bankGLAccountID = bankGLAccountID,
                                bank_id = (string.IsNullOrEmpty(bankID) ? 0 : Convert.ToInt32(bankID)),
                                PONumber = PONumber,

                                account = "Sale",
                                is_return = false,
                                estimate_invoice_no = estimate_invoice_no,
                                estimate_status = estimate_status,

                                total_cost_amount = total_cost_amount,
                                cash_account_id = cash_account_id,
                                receivable_account_id = receivable_account_id,
                                tax_account_id = tax_account_id,
                                sales_discount_acc_id = sales_discount_acc_id,
                                inventory_acc_id = inventory_acc_id,
                                purchases_acc_id = purchases_acc_id,
                                sales_account_id = sales_account_id,

                            });
                            //////

                            //if invoice type is sale then insert sales otherwise insert estimates/quotation
                            //Int32 sale_id = (sale_type == "Quotation" ? salesObj.InsertEstimates(salesModal_obj) : salesObj.InsertSales(salesModal_obj));
                            int sno = 0;
                            for (int i = 0; i < grid_sales.Rows.Count; i++)
                            {
                                if (grid_sales.Rows[i].Cells["id"].Value != null)
                                {
                                    if (chkbox_is_taxable.Checked)
                                    {
                                        tax_rate = (grid_sales.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_sales.Rows[i].Cells["tax_rate"].Value.ToString()));
                                        tax_id = Convert.ToInt32(grid_sales.Rows[i].Cells["tax_id"].Value.ToString());
                                    }
                                    else
                                    {
                                        tax_id = 0;
                                        tax_rate = 0;
                                    }

                                    ///// Added sales detail in to List
                                    sales_model_detail.Add(new SalesModal
                                    {
                                        serialNo = sno++,
                                        invoice_no = invoice_no,
                                        item_id = Convert.ToInt32(grid_sales.Rows[i].Cells["id"].Value.ToString()),
                                        item_number = grid_sales.Rows[i].Cells["item_number"].Value.ToString(),
                                        code = grid_sales.Rows[i].Cells["code"].Value.ToString(),
                                        name = grid_sales.Rows[i].Cells["name"].Value.ToString(),
                                        quantity_sold = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["qty"].Value.ToString()) ? 0 : double.Parse(grid_sales.Rows[i].Cells["qty"].Value.ToString())),
                                        unit_price = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["unit_price"].Value.ToString()) ? 0 : Math.Round(double.Parse(grid_sales.Rows[i].Cells["unit_price"].Value.ToString()), 4)),
                                        discount = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["discount"].Value.ToString()) ? 0 : Math.Round(double.Parse(grid_sales.Rows[i].Cells["discount"].Value.ToString()), 4)),
                                        discount_percent = double.Parse(grid_sales.Rows[i].Cells["discount_percent"].Value.ToString()),
                                        cost_price = (string.IsNullOrEmpty(grid_sales.Rows[i].Cells["cost_price"].Value.ToString()) ? 0 : Math.Round(Convert.ToDouble(grid_sales.Rows[i].Cells["cost_price"].Value.ToString()), 4)),// its avg cost actually ,
                                        item_type = grid_sales.Rows[i].Cells["item_type"].Value.ToString(),
                                        location_code = (grid_sales.Rows[i].Cells["location_code"].Value == null ? "" : grid_sales.Rows[i].Cells["location_code"].Value.ToString()),
                                        tax_id = tax_id,
                                        tax_rate = tax_rate,
                                        sale_date = sale_date,
                                        destination_branch_id = destination_branch_id,
                                        source_branch_id = source_branch_id,
                                        customer_id = customer_id,
                                    });
                                    //////////////

                                }
                            }

                            if (sale_type == "Quotation")
                            {
                                sale_id = salesObj.InsertEstimates(sales_model_header, sales_model_detail);
                                if (sale_id > 0)
                                {
                                    UiMessages.ShowInfo(
                                        "Estimate No: " + invoice_no + " " + sale_type + " transaction " + (invoice_status == "Update" ? "updated" : "created") + " successfully",
                                        "تقدير رقم: " + invoice_no + " " + sale_type + " تمت " + (invoice_status == "Update" ? "تحديث" : "إنشاء") + " بنجاح",
                                        captionEn: "Success",
                                        captionAr: "نجاح"
                                    );
                                    txt_invoice_no.Text = invoice_no;
                                    txt_invoice_no.Tag = sale_id;
                                }
                            }
                            else if (sale_type == "ICT")
                            {
                                if (destination_branch_id != 0)
                                {
                                    int ict_result = salesObj.ict_qty_request(sales_model_header, sales_model_detail);
                                    if (ict_result > 0)
                                    {
                                        UiMessages.ShowInfo(
                                            "Request for Inter Company Transfer (ICT) sent successfully",
                                            "تم إرسال طلب التحويل بين الشركات بنجاح",
                                            captionEn: "Success",
                                            captionAr: "نجاح"
                                        );
                                        clear_form();
                                        return;
                                    }
                                }
                            }
                            else
                            {
                                sale_id = salesObj.InsertSales(sales_model_header, sales_model_detail);
                                if (sale_id > 0)
                                {
                                    if (UsersModal.useZatcaEInvoice == true)
                                    {
                                        DataRow activeZatcaCredential = ZatcaInvoiceGenerator.GetActiveZatcaCSID();
                                        if (activeZatcaCredential == null)
                                        {
                                            UiMessages.ShowWarning(
                                                "No active ZATCA CSID/credentials found. Please configure them first to send invoices to zatca.",
                                                "لا توجد بيانات اعتماد Zاتكا نشطة. يرجى تكوينها أولاً."
                                            );

                                        }
                                        else { 
                                            // Sign invoice to ZATCA
                                            // Retrieve PCSID credentials from the database using the credentialId
                                            DataRow PCSID_dataRow = ZatcaInvoiceGenerator.GetZatcaCredentialByParentID(Convert.ToInt32(activeZatcaCredential["id"]));
                                            if (PCSID_dataRow == null)
                                            {
                                                // If PCSID not exist then sign with CSID
                                                //Sign Invoice with CSID instead of Production CSID
                                                ZatcaHelper.SignInvoiceToZatca(invoice_no);

                                                // NEW: skip ZATCA for sales
                                                if (!ZatcaSkipSaleInvoice)
                                                {
                                                    // After signing with CSID, send invoice to ZATCA
                                                    // If invoice subtype is Standard then clear it from ZATCA
                                                    if (cmb_invoice_subtype_code.SelectedValue.ToString() == "01" && chk_sendInvoiceToZatca.Checked == true)
                                                    {
                                                        // Clear invoice from ZATCA
                                                        ZatcaHelper.ZatcaInvoiceClearanceAsync(invoice_no);
                                                    }
                                                    else if (cmb_invoice_subtype_code.SelectedValue.ToString() == "02" && chk_sendInvoiceToZatca.Checked == true)
                                                    //otherwise Report invoice to ZATCA
                                                    {
                                                        // Report invoice to ZATCA
                                                        ZatcaHelper.ZatcaInvoiceReportingAsync(invoice_no);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                //If PCSID exist then sign it 
                                                ZatcaHelper.PCSID_SignInvoiceToZatcaAsync(invoice_no);

                                                // NEW: skip ZATCA for small sales
                                                if (!ZatcaSkipSaleInvoice)
                                                {
                                                    // After signing with PCSID, send invoice to ZATCA
                                                    // If invoice subtype is Standard then clear it from ZATCA
                                                    if (cmb_invoice_subtype_code.SelectedValue.ToString() == "01" && chk_sendInvoiceToZatca.Checked == true)
                                                    {
                                                        // Clear invoice from ZATCA
                                                        ZatcaHelper.ZatcaInvoiceClearanceAsync(invoice_no);
                                                    }
                                                    else if (cmb_invoice_subtype_code.SelectedValue.ToString() == "02" && chk_sendInvoiceToZatca.Checked == true)
                                                    //otherwise Report invoice to ZATCA
                                                    {
                                                        // Report invoice to ZATCA
                                                        ZatcaHelper.ZatcaInvoiceReportingAsync(invoice_no);
                                                    }
                                                }
                                            }
                                        }
                                    }

                                    UiMessages.ShowInfo(
                                        "Invoice No: " + invoice_no + " " + sale_type + " transaction " + (invoice_status == "Update" ? "updated" : "created") + " successfully",
                                        "فاتورة رقم: " + invoice_no + " " + sale_type + " تمت " + (invoice_status == "Update" ? "تحديث" : "إنشاء") + " بنجاح",
                                        captionEn: "Success",
                                        captionAr: "نجاح"
                                    );
                                }
                            }
                            if (sale_type != "Quotation" && sale_type != "Gift" && sale_type != "ICT")
                            {
                                if (employee_commission_percent > 0)
                                {
                                    var emp_commission_amount = employee_commission_percent * net_total / 100;
                                    Insert_emp_commission(invoice_no, 0, 0, emp_commission_amount, sale_date, txt_description.Text, employee_id);
                                }

                                if (user_commission_percent > 0)
                                {
                                    var user_commission_amount = user_commission_percent * net_total / 100;
                                    Insert_user_commission(invoice_no, 0, 0, user_commission_amount, sale_date, txt_description.Text);
                                }
                            }

                            if (sale_id > 0)
                            {
                                if (customer_id == 0 && customer_name.Length > 0)
                                {
                                    get_customers_dropdownlist();
                                }

                                string result1 = formPrintOption.PrintOptions;
                                bool isPrintInvoiceCode = false;
                                bool isPrintPOS80 = false;

                                if (result1 == "0")
                                {
                                    isPrintInvoiceCode = true;
                                    clear_form();
                                }
                                else if (result1 == "1")
                                {
                                    isPrintInvoiceCode = false;
                                    clear_form();
                                }
                                else if (result1 == "2")
                                {
                                    bool isEstimate = (sale_type == "Quotation" ? true : false);
                                    frm_send_whatsapp send_Whatsapp = new frm_send_whatsapp(invoice_no, isEstimate);
                                    send_Whatsapp.ShowDialog();
                                    clear_form();
                                    return;
                                }
                                else if (result1 == "3")
                                {
                                    clear_form();
                                    return;
                                }
                                else if (result1 == "4")
                                {
                                    isPrintPOS80 = true;
                                }

                                if (sale_type == "Cash" || sale_type == "Credit")
                                {
                                    using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(invoice_no), true, isPrintInvoiceCode, isPrintPOS80))
                                    {
                                        obj.load_print();
                                        clear_form();
                                    }
                                }
                                else
                                {
                                    using (frm_sales_invoice obj = new frm_sales_invoice(load_estiamte_receipt(invoice_no), true, isPrintInvoiceCode, isPrintPOS80))
                                    {
                                        obj.load_print();
                                        clear_form();
                                    }
                                }
                            }
                            else
                            {
                                UiMessages.ShowError("Record not saved", "لم يتم حفظ السجل", "Error", "خطأ");
                            }
                        }
                        else
                        {
                            UiMessages.ShowWarning("Please add products", "يرجى إضافة منتجات", "Sale Transaction", "معاملة البيع");
                        }
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, "خطأ", "Error", "Error");
                }
            }
        }

        private void HistoryToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                //this will load the product movement 

                //string global_product_code = "";
                //if (grid_sales.Rows.Count > 0 && grid_sales.CurrentRow.Cells["code"].Value != null)
                //{
                //    global_product_code = grid_sales.CurrentRow.Cells["code"].Value.ToString();
                //    frm_productsMovements frm_prod_move_obj = new frm_productsMovements(global_product_code);
                //    frm_prod_move_obj.ShowDialog();
                //}

                //this will load the product page
                if (grid_sales.RowCount > 0 && grid_sales.CurrentRow.Cells["item_number"].Value != null)
                {
                    if (grid_sales.CurrentRow.Cells["item_number"].Value != null)
                    {
                        string item_number = grid_sales.CurrentRow.Cells["item_number"].Value.ToString();

                        frm_product_full_detail obj = new frm_product_full_detail(this, null, item_number, null, null, "", true);

                        obj.ShowDialog();
                    }
                }


            }
            catch (Exception ex)
            {
                UiMessages.ShowError("An error occurred: " + ex.Message, "خطأ", "Error", "Error");
            }
        }

        private void AmountFixToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                double subtotal = Convert.ToDouble(txt_sub_total.Text);
                double total_amount = Convert.ToDouble(txt_total_amount.Text);
                frm_round_prices obj = new frm_round_prices(this, subtotal, total_amount);
                obj.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "Error");
            }
        }

        private void ICTToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                Products.ICT.frm_ict frm_Ict = new Products.ICT.frm_ict();
                frm_Ict.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "Error");
            }
        }

        private void LoadQuotationToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                frm_search_estimates obj_estimate = new frm_search_estimates(this);
                obj_estimate.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "Error");
            }
        }

        private void SearchToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                frm_search_invoices frm = new frm_search_invoices(this);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "Error");
            }
        }

        private void grid_sales_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            // Validate only the numeric columns (Qty=4, unit_price=5, discount=6, discount_percent=7, total_without_vat=8)
            if (e.ColumnIndex == 4 || e.ColumnIndex == 5 || e.ColumnIndex == 6 || e.ColumnIndex == 7 || e.ColumnIndex == 8)
            {
                // Check if the value is null or empty
                if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
                {
                    e.Cancel = true; // Prevent the cell from losing focus
                    grid_sales.Rows[e.RowIndex].ErrorText = "Value cannot be null or empty";
                }
                // Check if the value is a valid numeric value
                else if (!decimal.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true; // Prevent the cell from losing focus
                    grid_sales.Rows[e.RowIndex].ErrorText = "Value must be a numeric value";
                }
                else
                {
                    // Clear any previous error messages
                    grid_sales.Rows[e.RowIndex].ErrorText = string.Empty;
                }
            }
        }

        private void PrinttoolStripButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_invoice_no.Text))
            {
                using (BusyScope.Show(this, UiMessages.T("Loading...", "تحميل...")))
                {
                    using (frm_sales_invoice obj = new frm_sales_invoice(load_sales_receipt(txt_invoice_no.Text), true))
                    {
                        //obj.load_print(); // send print direct to printer without showing dialog
                        obj.ShowDialog();
                    }
                }
            }

        }

        private void SaleReturnToolStripButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_invoice_no.Text))
            {
                using (BusyScope.Show(this, UiMessages.T("Loading...", "تحميل...")))
                {
                   using (frm_sales_return obj = new frm_sales_return(txt_invoice_no.Text))
                    {
                        obj.ShowDialog();
                        //obj.LoadSalesReturnGrid(); // send print direct to printer without showing dialog
                        obj.Close();
                    }
                }
            }
        }

        private void SetupCustomersDataGridView()
        {
            if (customersDataGridView != null) return; // already created

            var current_lang_code = Thread.CurrentThread.CurrentUICulture.IetfLanguageTag;

            customersDataGridView = new DataGridView();
            customersDataGridView.ColumnCount = 6;

            int xLocation = groupBoxCustomer.Location.X + txtCustomerSearch.Location.X;
            int yLocation = groupBoxCustomer.Location.Y + txtCustomerSearch.Location.Y + 22;

            customersDataGridView.Location = new Point(xLocation, yLocation);
            customersDataGridView.Size = new Size(520, 240);
            customersDataGridView.BorderStyle = BorderStyle.None;
            customersDataGridView.BackgroundColor = Color.White;
            customersDataGridView.AutoGenerateColumns = false;
            customersDataGridView.ReadOnly = true;
            customersDataGridView.AllowUserToAddRows = false;
            customersDataGridView.AllowUserToDeleteRows = false;
            customersDataGridView.AllowUserToResizeRows = false;
            customersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            customersDataGridView.MultiSelect = false;
            customersDataGridView.RowHeadersVisible = false;
            //Dock = DockStyle.Fill

            customersDataGridView.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            customersDataGridView.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;

            customersDataGridView.Columns[0].Name = "Code";
            customersDataGridView.Columns[1].Name = "Name";
            customersDataGridView.Columns[2].Name = "ID";
            customersDataGridView.Columns[3].Name = "Contact";
            customersDataGridView.Columns[4].Name = "VAT No";
            customersDataGridView.Columns[5].Name = "Credit Limit";

            customersDataGridView.Columns[0].ReadOnly = true;
            customersDataGridView.Columns[1].ReadOnly = true;
            customersDataGridView.Columns[2].Visible = false;
            customersDataGridView.Columns[3].ReadOnly = true;
            customersDataGridView.Columns[4].ReadOnly = true;
            customersDataGridView.Columns[5].Visible = false;

            customersDataGridView.Columns[0].Width = 90;
            customersDataGridView.Columns[1].Width = 220;
            customersDataGridView.Columns[3].Width = 130;
            customersDataGridView.Columns[4].Width = 120;

            //customersDataGridView.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            //customersDataGridView.AutoResizeColumns();

            this.customersDataGridView.CellClick += new DataGridViewCellEventHandler(customersDataGridView_CellClick);
            this.customersDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(customersDataGridView_KeyDown);

            customersDataGridView.Visible = false;
            this.Controls.Add(customersDataGridView);
            customersDataGridView.BringToFront();
            StyleDropdownGrid(customersDataGridView);

        }

        void customersDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;

                txt_customerID.Text = customersDataGridView.CurrentRow.Cells[2].Value.ToString();
                txtCustomerSearch.Text = customersDataGridView.CurrentRow.Cells[1].Value.ToString();
                txt_customer_vat.Text = customersDataGridView.CurrentRow.Cells[4].Value.ToString();
                txt_cust_credit_limit.Text = customersDataGridView.CurrentRow.Cells[5].Value.ToString();

                ///customer balance
                CustomerBLL customerBLL_obj = new CustomerBLL();
                Decimal customer_total_balance = customerBLL_obj.GetCustomerAccountBalance(Convert.ToInt32(txt_customerID.Text));
                txt_cust_balance.Text = customer_total_balance.ToString("N2");

                // NEW
                ApplyInvoiceSubtypeForCustomerSelection();

                customersDataGridView.Visible = false;
                grid_sales.Focus();

            }
            else if (e.KeyCode == Keys.Escape)
            {
                customersDataGridView.Visible = false;
                txtCustomerSearch.Focus();
            }

        }

        private void customersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_customerID.Text = customersDataGridView.CurrentRow.Cells[2].Value.ToString();
            txtCustomerSearch.Text = customersDataGridView.CurrentRow.Cells[1].Value.ToString();
            txt_customer_vat.Text = customersDataGridView.CurrentRow.Cells[4].Value.ToString();
            txt_cust_credit_limit.Text = customersDataGridView.CurrentRow.Cells[5].Value.ToString();
            ///customer balance
            CustomerBLL customerBLL_obj = new CustomerBLL();
            Decimal customer_total_balance = customerBLL_obj.GetCustomerAccountBalance(Convert.ToInt32(txt_customerID.Text));
            txt_cust_balance.Text = customer_total_balance.ToString("N2");

            // NEW
            ApplyInvoiceSubtypeForCustomerSelection();

            customersDataGridView.Visible = false;
            grid_sales.Focus();

        }

        private void RefreshData()
        {
            try
            {
                var customerSearch = txtCustomerSearch.Text ?? string.Empty;

                var normalizedSearch = new CustomerBLL().NormalizeCustomerCodeInput(customerSearch);

                // If we are suppressing or the box isn't focused, do not show suggestions
                if (_suppressCustomerSearch || !txtCustomerSearch.Focused)
                {
                    customersDataGridView.Visible = false;
                    return;
                }

                if (!string.IsNullOrWhiteSpace(normalizedSearch))
                {
                    var bll = new CustomerBLL();
                    DataTable dt = bll.SearchRecord(normalizedSearch) ?? new DataTable();

                    customersDataGridView.Rows.Clear();

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string[] row0 = {
                        dt.Columns.Contains("customer_code") ? dr["customer_code"].ToString() : "",
                        dr["first_name"].ToString() + " " + dr["last_name"].ToString(),
                        dr["id"].ToString(),
                        dr["contact_no"].ToString(),
                        dr["vat_no"].ToString(),
                        dr["credit_limit"].ToString()
                    };
                            customersDataGridView.Rows.Add(row0);
                        }

                        customersDataGridView.Visible = true; // textbox is focused here per the guard above
                        customersDataGridView.ClearSelection();
                        customersDataGridView.CurrentCell = null;
                        TrySelectCurrent();
                    }
                    else
                    {
                        customersDataGridView.Visible = false;
                    }
                }
                else
                {
                    txtCustomerSearch.Text = "";
                    txt_customerID.Text = "";
                    txt_customer_vat.Text = "";
                    txt_cust_credit_limit.Text = "";
                    txt_cust_balance.Text = "";
                    customersDataGridView.Visible = false;
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "Error");
            }
        }

        private void SelectCustomerFromDataRow(DataRow dr)
        {
            txt_customerID.Text = Convert.ToString(dr["id"]);
            txtCustomerSearch.Text = (Convert.ToString(dr["first_name"]) + " " + Convert.ToString(dr["last_name"])).Trim();
            txt_customer_vat.Text = Convert.ToString(dr["vat_no"]);
            txt_cust_credit_limit.Text = Convert.ToString(dr["credit_limit"]);

            CustomerBLL customerBLL_obj = new CustomerBLL();
            Decimal customer_total_balance = customerBLL_obj.GetCustomerAccountBalance(Convert.ToInt32(txt_customerID.Text));
            txt_cust_balance.Text = customer_total_balance.ToString("N2");
            ApplyInvoiceSubtypeForCustomerSelection();
        }

        private void CustomerSearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            _customerSearchDebounceTimer.Stop();
            if (_suppressCustomerSearch || !txtCustomerSearch.Focused) return;
            RefreshData();
        }
        private void txtCustomerSearch_TextChanged(object sender, EventArgs e)
        {
            if (_suppressCustomerSearch || !txtCustomerSearch.Focused) return;
            _customerSearchDebounceTimer.Stop();
            _customerSearchDebounceTimer.Start();
        }

        private void txtCustomerSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (customersDataGridView.Visible && customersDataGridView.Rows.Count > 0)
                {
                    customersDataGridView.Focus();
                    if (customersDataGridView.CurrentRow == null)
                    {
                        customersDataGridView.CurrentCell = customersDataGridView.Rows[0].Cells[0];
                    }
                }
                return;
            }
            if (e.KeyCode == Keys.Escape)
            {
                customersDataGridView.Visible = false;
                return;
            }
            if (e.KeyCode == Keys.Enter)
            {
                var normalizedSearch = new CustomerBLL().NormalizeCustomerCodeInput(txtCustomerSearch.Text);

                // If user entered an exact customer code, try to auto-select on Enter
                if (!string.IsNullOrWhiteSpace(normalizedSearch) && normalizedSearch.StartsWith("C-", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var bll = new CustomerBLL();
                        var dt = bll.SearchRecord(normalizedSearch);
                        if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("customer_code"))
                        {
                            var exact = dt.Select("customer_code = '" + normalizedSearch.Replace("'", "''") + "'");
                            if (exact.Length == 1)
                            {
                                SelectCustomerFromDataRow(exact[0]);
                                customersDataGridView.Visible = false;
                                grid_sales.Focus();
                                return;
                            }
                        }

                        // No exact match found for entered code => clear selected customer fields
                        txt_customerID.Text = "";
                        txt_customer_vat.Text = "";
                        txt_cust_credit_limit.Text = "";
                        txt_cust_balance.Text = "";
                        ApplyInvoiceSubtypeForCustomerSelection();
                    }
                    catch
                    {
                        // ignore and fall back to grid selection
                    }
                }

                if (customersDataGridView.Visible && customersDataGridView.Rows.Count > 0)
                {
                    txt_customerID.Text = customersDataGridView.CurrentRow.Cells[2].Value.ToString();
                    txtCustomerSearch.Text = customersDataGridView.CurrentRow.Cells[1].Value.ToString();
                    txt_customer_vat.Text = customersDataGridView.CurrentRow.Cells[4].Value.ToString();
                    txt_cust_credit_limit.Text = customersDataGridView.CurrentRow.Cells[5].Value.ToString();
                    ///customer balance
                    CustomerBLL customerBLL_obj = new CustomerBLL();
                    Decimal customer_total_balance = customerBLL_obj.GetCustomerAccountBalance(Convert.ToInt32(txt_customerID.Text));
                    txt_cust_balance.Text = customer_total_balance.ToString("N2");

                    // NEW
                    ApplyInvoiceSubtypeForCustomerSelection();

                }
            }
            _customerSearchDebounceTimer.Stop();
            _customerSearchDebounceTimer.Start();
        }

        private void txtCustomerSearch_Leave(object sender, EventArgs e)
        {
            _customerSearchDebounceTimer.Stop();
            if (!customersDataGridView.Focused)
            {
                customersDataGridView.Visible = false;
            }
        }

        private void TrySelectCurrent()
        {
            if (customersDataGridView.CurrentRow == null && customersDataGridView.Rows.Count > 0)
            {
                customersDataGridView.CurrentCell = customersDataGridView.Rows[0].Cells[0];
            }
            if (customersDataGridView.CurrentRow == null) return;
            var row = customersDataGridView.CurrentRow;

            int id = 0;
            // Try to read id if present
            if (customersDataGridView.Columns.Contains("id"))
            {
                int.TryParse(Convert.ToString(row.Cells["id"].Value), out id);
            }

            string name = null;
            if (customersDataGridView.Columns.Contains("name"))
                name = Convert.ToString(row.Cells["Name"].Value);
            if (string.IsNullOrWhiteSpace(name) && customersDataGridView.Columns.Contains("first_name"))
            {
                var first = Convert.ToString(row.Cells["first_name"].Value);
                var last = customersDataGridView.Columns.Contains("last_name") ? Convert.ToString(row.Cells["last_name"].Value) : string.Empty;
                name = (first + " " + last).Trim();
            }

            string phone = customersDataGridView.Columns.Contains("contact_no") ? Convert.ToString(row.Cells["contact_no"].Value) : string.Empty;
            string vat = customersDataGridView.Columns.Contains("vat_no") ? Convert.ToString(row.Cells["vat_no"].Value) : string.Empty;

        }

        // Add inside frm_sales class (near other helpers)
        private void ApplyInvoiceSubtypeForCustomerSelection()
        {
            // If no customer selected => Simplified (02)
            // If customer selected => Standard (01)
            bool hasCustomer = !string.IsNullOrWhiteSpace(txt_customerID.Text)
                               && txt_customerID.Text != "0"
                               && txt_customerID.Text != "-1";

            var desired = hasCustomer ? "01" : "02";

            // Guard (combo might not be initialized yet during load)
            if (cmb_invoice_subtype_code.DataSource == null)
                return;

            // Avoid unnecessary SelectedIndexChanged churn
            if (cmb_invoice_subtype_code.SelectedValue == null || cmb_invoice_subtype_code.SelectedValue.ToString() != desired)
                cmb_invoice_subtype_code.SelectedValue = desired;
        }

        // Add this helper inside frm_sales class
        private bool ValidateStandardInvoiceCustomer(int customerId)
        {
            if (customerId <= 0)
            {
                MessageBox.Show(
                    lang == "ar-SA" ? "يرجى اختيار العميل لنوع الفاتورة الضريبية (قياسية)." : "Please select a customer for Standard invoice type.",
                    lang == "ar-SA" ? "معاملة البيع" : "Sale Transaction",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                txtCustomerSearch.Focus();
                return false;
            }

            try
            {
                var customerBLL = new CustomerBLL();
                var dt = customerBLL.SearchRecordByCustomerID(customerId);
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show(
                        lang == "ar-SA" ? "العميل المحدد غير موجود." : "Selected customer not found.",
                        lang == "ar-SA" ? "معاملة البيع" : "Sale Transaction",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                    );
                    txtCustomerSearch.Focus();
                    return false;
                }

                var row = dt.Rows[0];

                string cityName = Convert.ToString(row["CityName"]);
                string countryName = Convert.ToString(row["CountryName"]);
                string streetName = Convert.ToString(row["StreetName"]);
                string postalCode = Convert.ToString(row["PostalCode"]);
                string buildingNumber = Convert.ToString(row["BuildingNumber"]);
                string citySubdivisionName = Convert.ToString(row["CitySubdivisionName"]);
                string registrationName = Convert.ToString(row["RegistrationName"]); // customer legal name

                var missing = new List<string>();
                if (string.IsNullOrWhiteSpace(registrationName)) missing.Add("Registration Name");
                if (string.IsNullOrWhiteSpace(countryName)) missing.Add("Country Name");
                if (string.IsNullOrWhiteSpace(cityName)) missing.Add("City Name");
                if (string.IsNullOrWhiteSpace(citySubdivisionName)) missing.Add("City Subdivision Name");
                if (string.IsNullOrWhiteSpace(streetName)) missing.Add("Street Name");
                if (string.IsNullOrWhiteSpace(buildingNumber)) missing.Add("Building Number");
                if (string.IsNullOrWhiteSpace(postalCode)) missing.Add("Postal Code");
            
                if (missing.Count > 0)
                {
                    var caption = lang == "ar-SA" ? "فاتورة زاتكا القياسية" : "ZATCA Standard Invoice";
                    var head = lang == "ar-SA"
                        ? "تعذر معالجة الفاتورة القياسية. الحقول الإلزامية لعنوان العميل المفقودة:\n\n- "
                        : "Cannot process Standard invoice. Missing mandatory customer address fields:\n\n- ";
                    // Localize field names
                    var localizedMissing = new List<string>();
                    foreach (var m in missing)
                    {
                        switch (m)
                        {
                            case "Registration Name": localizedMissing.Add(lang == "ar-SA" ? "الاسم القانوني (Registration Name)" : m); break;
                            case "Country Name": localizedMissing.Add(lang == "ar-SA" ? "اسم الدولة" : m); break;
                            case "City Name": localizedMissing.Add(lang == "ar-SA" ? "اسم المدينة" : m); break;
                            case "City Subdivision Name": localizedMissing.Add(lang == "ar-SA" ? "اسم تقسيم المدينة" : m); break;
                            case "Street Name": localizedMissing.Add(lang == "ar-SA" ? "اسم الشارع" : m); break;
                            case "Building Number": localizedMissing.Add(lang == "ar-SA" ? "رقم المبنى" : m); break;
                            case "Postal Code": localizedMissing.Add(lang == "ar-SA" ? "الرمز البريدي" : m); break;
                            default: localizedMissing.Add(m); break;
                        }
                    }

                    var proceedQuestion = lang == "ar-SA"
                         ? "\n\nهل ترغب بالمتابعة على أي حال؟"
                         : "\n\nDo you want to proceed anyway?";

                    var result = MessageBox.Show(
                        head + string.Join("\n- ", localizedMissing) + proceedQuestion,
                        caption,
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning,
                        MessageBoxDefaultButton.Button2
                    );

                    if (result == DialogResult.Yes)
                    {
                        // Proceed even with missing fields
                        return true;
                    }

                    txtCustomerSearch.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    (lang == "ar-SA" ? "فشل التحقق من عنوان العميل.\n" : "Failed to validate customer address.\n") + ex.Message,
                    lang == "ar-SA" ? "خطأ" : "Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                ); 
                return false;
            }
        }

        private void panel_footer_Paint(object sender, PaintEventArgs e)
        {

        }
    }

}
