using pos.Security.Authorization;
using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_purchases : Form
    {
        private static readonly ComponentResourceManager PurchasesResources = new ComponentResourceManager(typeof(frm_purchases));
        // Use centralized, DB-backed authorization and current user
        private readonly IAuthorizationService _auth = AppSecurityContext.Auth;
        private UserIdentity _currentUser = AppSecurityContext.User;
        
        public string lang = (UsersModal.logged_in_lang.Length > 0 ? UsersModal.logged_in_lang : "en-US");
        public int cash_account_id = 0;
        //public int sales_account_id = 0;
        public int payable_account_id = 0;
        public int tax_account_id = 0;
        public int purchases_discount_acc_id = 0;
        public int inventory_acc_id = 0;
        public int purchases_acc_id = 0;

        public static frm_purchases instance;
        public TextBox tb_product_id;
        public TextBox tb_code;
        public TextBox tb_product_name;
        public TextBox tb_qty;
        public TextBox tb_cost_price;

        public decimal total_amount = 0;
        public decimal total_tax = 0;
        public decimal total_discount = 0;
        public decimal total_sub_total = 0;

        public int global_product_id = 0;
        public int global_tax_id = 0;
        public decimal global_tax_rate = 0;
        public decimal global_unit_price = 0;
        public string global_location_code = "";
        public string global_unit = "";
        public string global_item_category = "";

        string invoice_status = "";
        string po_invoice_no = "";

        public double cash_purchase_amount_limit = 0;
        public bool allow_credit_purchase = false;

        private DataGridView brandsDataGridView = new DataGridView();
        private DataGridView categoriesDataGridView = new DataGridView();
        private DataGridView groupsDataGridView = new DataGridView();

        private DataGridView suppliersDataGridView;
        private System.Windows.Forms.Timer _supplierSearchDebounceTimer;
        private bool _suppressSupplierSearch;
        private int _selectedSupplierId;

        public DataTable products_dt = new DataTable();

        //private frm_searchProducts productsMainForm;

        //public frm_purchases(frm_searchProducts productsMainForm) : this()
        //{
        //    this.productsMainForm = productsMainForm;
        //}

        public frm_purchases()
        {
            InitializeComponent();
            //autoCompleteProductCode();

            SetupSuppliersDataGridView();
            _supplierSearchDebounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
            _supplierSearchDebounceTimer.Tick += SupplierSearchDebounceTimer_Tick;

            if (txtSupplierSearch != null)
            {
                txtSupplierSearch.TextChanged += txtSupplierSearch_TextChanged;
                txtSupplierSearch.KeyUp += txtSupplierSearch_KeyUp;
                txtSupplierSearch.Leave += txtSupplierSearch_Leave;
            }

            if (txt_shipping_cost != null)
            {
                txt_shipping_cost.TextChanged -= txt_shipping_cost_TextChanged;
                txt_shipping_cost.TextChanged += txt_shipping_cost_TextChanged;
            }

        }

        private void frm_purchases_Load(object sender, EventArgs e)
        {
            // Apply theme first so everything is styled before data loads
            AppTheme.Apply(this);
            StylePurchasesForm();

            using (BusyScope.Show(this, UiMessages.T("Loading purchases...", "جاري تحميل المشتريات...")))
            {
                grid_purchases.Rows.Add();
                this.ActiveControl = grid_purchases;
                grid_purchases.CurrentCell = grid_purchases.Rows[0].Cells["code"];

                Get_AccountID_From_Company();
                load_user_rights(UsersModal.logged_in_userid);

                get_purchasetype_dropdownlist();
                if (lang == "en-US")
                {
                    cmb_purchase_type.SelectedValue = "Cash";
                }
                else if (lang == "ar-SA")
                {
                    cmb_purchase_type.SelectedIndex = 0;
                }

                // Supplier selection is via txtSupplierSearch + grid
                get_employees_dropdownlist();
                get_payment_method_dropdownlist();

                foreach (DataGridViewColumn column in grid_purchases.Columns)
                {
                    column.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
            }
        }

        /// <summary>
        /// Applies the same classic Windows grey theme used on the sales page,
        /// keeping both forms visually uniform.
        /// </summary>
        private void StylePurchasesForm()
        {
            // ── Title label ───────────────────────────────────────────
            lbl_title.Font = AppTheme.FontHeader;
            lbl_title.ForeColor = SystemColors.ControlText;

            // ── Panels ────────────────────────────────────────────────
            panel_header.BackColor = SystemColors.Control;
            panel_footer.BackColor = SystemColors.Control;
            panel_grid.BackColor   = SystemColors.Control;

            // ── GroupBoxes in header: standard Windows look ───────────
            foreach (Control ctrl in panel_header.Controls)
            {
                if (ctrl is GroupBox grp)
                {
                    grp.BackColor = SystemColors.Control;
                    grp.ForeColor = SystemColors.ControlText;
                    grp.Font      = AppTheme.FontGroupBox;
                    grp.Padding   = new Padding(4, 8, 4, 4);

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

            // ── ToolStrip: classic Windows system renderer ────────────
            PurchaseToolStrip.RenderMode       = ToolStripRenderMode.System;
            PurchaseToolStrip.BackColor        = SystemColors.Control;
            PurchaseToolStrip.ForeColor        = SystemColors.ControlText;
            PurchaseToolStrip.ImageScalingSize = new Size(20, 20);
            PurchaseToolStrip.AutoSize         = true;
            PurchaseToolStrip.GripStyle        = ToolStripGripStyle.Hidden;
            PurchaseToolStrip.Padding          = new Padding(4, 2, 4, 2);
            foreach (ToolStripItem item in PurchaseToolStrip.Items)
            {
                item.ForeColor = SystemColors.ControlText;
                item.Padding   = new Padding(4, 2, 4, 2);
                item.Margin    = new Padding(1, 0, 1, 0);
                if (item is ToolStripButton tsb)
                {
                    tsb.DisplayStyle      = ToolStripItemDisplayStyle.ImageAndText;
                    tsb.TextImageRelation = TextImageRelation.ImageBeforeText;
                }
            }

            // ── Main purchases grid ───────────────────────────────────
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance  |
                System.Reflection.BindingFlags.SetProperty,
                null, grid_purchases, new object[] { true });

            var gridFont   = AppTheme.FontGrid;
            var headerFont = AppTheme.FontGridHeader;

            // Grid-level settings
            grid_purchases.BorderStyle          = BorderStyle.None;
            grid_purchases.CellBorderStyle      = DataGridViewCellBorderStyle.SingleHorizontal;
            grid_purchases.GridColor            = SystemColors.ControlLight;
            grid_purchases.BackgroundColor      = SystemColors.AppWorkspace;
            grid_purchases.RowHeadersVisible    = false;
            grid_purchases.SelectionMode        = DataGridViewSelectionMode.CellSelect;
            grid_purchases.AllowUserToAddRows   = false;

            // Lock header height so it doesn't resize with resx font
            grid_purchases.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid_purchases.ColumnHeadersHeight         = 36;
            grid_purchases.RowTemplate.Height          = 34;
            grid_purchases.EnableHeadersVisualStyles   = false;

            grid_purchases.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Control,
                ForeColor          = SystemColors.ControlText,
                Font               = headerFont,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText,
                Alignment          = DataGridViewContentAlignment.MiddleLeft,
                Padding            = new Padding(6, 4, 6, 4)
            };

            grid_purchases.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Window,
                ForeColor          = SystemColors.WindowText,
                Font               = gridFont,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                Padding            = new Padding(4, 2, 4, 2)
            };

            grid_purchases.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Window,
                ForeColor          = SystemColors.WindowText,
                Font               = gridFont,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText
            };

            // Reset per-column fonts so resx Arial Narrow 12pt is overridden
            foreach (DataGridViewColumn col in grid_purchases.Columns)
                col.DefaultCellStyle.Font = null;

            // sno: grey centered row-number column
            sno.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Control,
                ForeColor          = SystemColors.GrayText,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.GrayText,
                Alignment          = DataGridViewContentAlignment.MiddleCenter,
                Font               = AppTheme.FontSmall
            };

            // code: entry-point column — slight indent highlight
            code.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Window,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText
            };

            // Delete button column
            btn_delete.DefaultCellStyle = new DataGridViewCellStyle
            {
                ForeColor          = AppTheme.Danger,
                SelectionForeColor = AppTheme.Danger,
                Alignment          = DataGridViewContentAlignment.MiddleCenter
            };
            btn_delete.FlatStyle = FlatStyle.Flat;

            // Product-history grid (split panel)
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance  |
                System.Reflection.BindingFlags.SetProperty,
                null, grid_product_history, new object[] { true });

            grid_product_history.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid_product_history.ColumnHeadersHeight         = 32;
            grid_product_history.RowTemplate.Height          = 28;
            grid_product_history.RowHeadersVisible           = false;
            grid_product_history.EnableHeadersVisualStyles   = false;
            grid_product_history.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Control,
                ForeColor          = SystemColors.ControlText,
                Font               = AppTheme.FontGridHeader,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText
            };
            grid_product_history.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Window,
                ForeColor          = SystemColors.WindowText,
                Font               = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText
            };

            // ── Footer ────────────────────────────────────────────────
            tableLayoutPanel4.BackColor = SystemColors.Control;
            tableLayoutPanel5.BackColor = SystemColors.Control;

            // Fix row heights so labels aren't clipped by old resx 15.75pt font
            // tableLayoutPanel4: rows = SubTotal | Discount | Tax(+chkbox) | Total
            tableLayoutPanel4.RowStyles.Clear();
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));  // Sub Total
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));  // Discount
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));  // Tax + VAT chk
            tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));  // Total (larger)

            // tableLayoutPanel5: rows = TotalQty | ShopQty | DiscVal | Shipping
            tableLayoutPanel5.RowStyles.Clear();
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));
            tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Absolute, 34F));

            // groupBox2 (VAT / price-type options)
            groupBox2.BackColor = SystemColors.Control;
            groupBox2.ForeColor = SystemColors.ControlText;
            groupBox2.Font      = AppTheme.FontGroupBox;

            // Labels
            StylePurchaseLabel(label14, false);   // Sub Total
            StylePurchaseLabel(label13, false);   // Discount
            StylePurchaseLabel(label9,  true);    // Total Amount
            StylePurchaseLabel(label10, false);   // Total Qty
            StylePurchaseLabel(label19, false);   // Shop Qty
            StylePurchaseLabel(label20, false);   // Disc Value
            StylePurchaseLabel(label22, false);   // Shipping Cost

            chkbox_is_taxable.Font      = AppTheme.FontSemiBold;
            chkbox_is_taxable.ForeColor = SystemColors.ControlText;

            // Total fields
            StylePurchaseTotalField(txt_total_amount,    true);
            StylePurchaseTotalField(txt_sub_total,       false);
            StylePurchaseTotalField(txt_total_discount,  false);
            StylePurchaseTotalField(txt_total_tax,       false);
            StylePurchaseTotalField(txt_total_qty,       false);
            StylePurchaseTotalField(txt_shop_qty,        false);
            StylePurchaseTotalField(txt_total_disc_value, false);
            StylePurchaseTotalField(txt_shipping_cost,   false);
            txt_shipping_cost.ReadOnly = false;
            txt_total_disc_value.ReadOnly = false;
            // Supplier dropdown grid
            if (suppliersDataGridView != null)
                StylePurchaseDropdownGrid(suppliersDataGridView);
        }

        /// <summary>Style a purchases total/summary TextBox.</summary>
        private static void StylePurchaseTotalField(TextBox txt, bool isPrimary)
        {
            txt.ReadOnly    = true;
            txt.BorderStyle = BorderStyle.Fixed3D;
            txt.TextAlign   = HorizontalAlignment.Right;
            txt.ForeColor   = SystemColors.WindowText;
            txt.BackColor   = SystemColors.Window;
            txt.Font = isPrimary ? AppTheme.FontHeader : AppTheme.FontSubHeader;
        }

        /// <summary>Style a purchases footer label.</summary>
        private static void StylePurchaseLabel(Label lbl, bool isPrimary)
        {
            lbl.ForeColor = SystemColors.ControlText;
            lbl.Font = isPrimary ? AppTheme.FontSemiBold : AppTheme.FontLabel;
        }

        /// <summary>Style a popup DataGridView dropdown on the purchases page.</summary>
        private static void StylePurchaseDropdownGrid(DataGridView dgv)
        {
            dgv.BorderStyle      = BorderStyle.FixedSingle;
            dgv.BackgroundColor  = SystemColors.AppWorkspace;
            dgv.GridColor        = SystemColors.ControlDark;
            dgv.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Window,
                ForeColor          = SystemColors.WindowText,
                Font               = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                Padding            = new Padding(6, 2, 6, 2)
            };
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Control,
                ForeColor          = SystemColors.ControlText,
                Font               = AppTheme.FontGridHeader,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText
            };
            dgv.EnableHeadersVisualStyles  = false;
            dgv.ColumnHeadersBorderStyle   = DataGridViewHeaderBorderStyle.Single;
            dgv.RowHeadersVisible          = false;
            dgv.CellBorderStyle            = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.RowTemplate.Height         = 28;
            dgv.ColumnHeadersHeight        = 32;
        }

        public string GetMAXInvoiceNo()
        {
            PurchasesBLL PurchasesBLL_obj = new PurchasesBLL();
            return PurchasesBLL_obj.GeneratePurchaseInvoiceNo(); //.GetMaxInvoiceNo();
        }

        public string GetMAXInvoiceNo_HOLD()
        {
            PurchasesBLL PurchasesBLL_obj = new PurchasesBLL();
            return PurchasesBLL_obj.GenerateHoldPurchaseInvoiceNo();
        }

        Form purchaseSearchObj;
        private void grid_purchases_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //int iColumn = grid_purchases.CurrentCell.ColumnIndex;
                //int iRow = grid_purchases.CurrentCell.RowIndex;
                string columnName = grid_purchases.Columns[e.ColumnIndex].Name;
                if (columnName == "code")
                {
                    string product_code = (grid_purchases.CurrentRow.Cells["code"].Value != null ? grid_purchases.CurrentRow.Cells["code"].Value.ToString() : "");
                    bool isGrid = true;
                    var brand_code = txt_brand_code.Text; // (cmb_brands.SelectedValue != null ? cmb_brands.SelectedValue.ToString() : "");
                    var category_code = txt_category_code.Text; // (cmb_categories.SelectedValue != null ? cmb_categories.SelectedValue.ToString() : "");
                    var group_code = txt_group_code.Text; // (cmb_groups.SelectedValue != null ? cmb_groups.SelectedValue.ToString() : "");

                    ////////////////////////
                    if (purchaseSearchObj == null || product_code != "")
                    {
                        purchaseSearchObj = new frm_searchPurchaseProducts(this, null, product_code, category_code, brand_code, e.RowIndex, isGrid, group_code);
                        purchaseSearchObj.FormClosed += new FormClosedEventHandler(purchaseSearchObj_FormClosed);
                        //frm_cust.Dock = DockStyle.Fill;
                        purchaseSearchObj.ShowDialog();
                    }
                    else
                    {
                        //frm_searchSaleProducts_obj.ShowDialog();
                        //frm_searchSaleProducts_obj.BringToFront();
                        purchaseSearchObj.Visible = true;
                    }
                    ////////////

                }

                string editedColName = grid_purchases.Columns[e.ColumnIndex].Name;
                if (_numericColumns.Contains(editedColName))
                {
                    var cell = grid_purchases.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    if (cell.Value == null || string.IsNullOrEmpty(cell.Value.ToString()))
                    {
                        cell.Value = 0;
                    }
                }

                // Safely get values from cells
                double GetCellDouble(string colName)
                {
                    var val = grid_purchases.Rows[e.RowIndex].Cells[colName].Value;
                    return val == null || val.ToString() == "" ? 0 : Convert.ToDouble(val);
                }

                double avgCost = GetCellDouble("avg_cost");
                double qty = GetCellDouble("qty");
                double discount = GetCellDouble("discount");
                double taxRate = GetCellDouble("tax_rate");

                double totalValue = avgCost * qty;
                double subTotal = totalValue - discount;
                double tax = (subTotal * taxRate) / 100;

                if (columnName == "Qty") // if qty is changed
                {
                    //double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                    //double tax = (Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * tax_rate / 100);

                   // grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value));
                   // double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                   // grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;

                    grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = tax;
                    grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = subTotal + tax;
                    
                }
                if (columnName == "avg_cost")//if avg_cost is changed
                {
                    double grossTotal = avgCost * qty;
                    double discountValue = GetCellDouble("discount");
                    double netTotal = grossTotal - discountValue;
                    double tax_1 = (netTotal * taxRate) / 100;

                    if (rd_btn_without_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        //double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        //double tax = (Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * tax_rate / 100);
                        //grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value));

                        //double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                        //grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;

                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = tax_1;
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = netTotal + tax_1;
                    }

                    if (rd_btn_without_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double singleCostPrice = avgCost / qty;
                        double taxPerUnit = (singleCostPrice * taxRate) / 100;
                        double totalTax = taxPerUnit * qty;

                        double newTotal = (singleCostPrice * qty) - discountValue;
                        //double discountPercent = grossTotal == 0 ? 0 : (discountValue / grossTotal) * 100;

                        grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = singleCostPrice;
                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (newTotal * taxRate) / 100;
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = newTotal + tax_1;
                       
                        //double total_avg_cost_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value);
                        //double qty_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                        //double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        //string pre_tax = "1." + tax_rate.ToString();
                        //double single_avg_cost = (total_avg_cost_1 / qty_1);
                        //double tax = (single_avg_cost * tax_rate / 100);
                        //double new_tax_value = (tax * qty_1);

                        //grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = single_avg_cost;
                        //grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = new_tax_value;
                        //double sub_total = ((single_avg_cost * qty_1) + new_tax_value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value); 
                        //grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_by_unitprice.Checked)
                    {
                        double preTax = 1 + (taxRate / 100);
                        double netCostPrice = avgCost / preTax;
                        double grossTotalBeforeDiscount = netCostPrice * qty;
                        double netTotal_1 = grossTotalBeforeDiscount - discountValue;
                        double taxAmount = (netTotal_1 * taxRate) / 100;

                        grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = netCostPrice;
                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = taxAmount;
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = netTotal_1 + taxAmount;
                        
                        //double total_avg_cost = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value);
                        //double qty = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                        //double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        //string pre_tax = "1." + tax_rate.ToString();
                        //double single_avg_cost = (total_avg_cost / double.Parse(pre_tax));
                        //double tax = (single_avg_cost * tax_rate / 100);
                        //double new_tax_value = (tax * qty);

                        //grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = single_avg_cost;
                        //grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = new_tax_value;
                        //double sub_total = ((single_avg_cost * qty) + new_tax_value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                        ////grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = (Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) - tax);
                        //grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                    }

                    if (rd_btn_with_vat.Checked && rd_btn_bytotal_price.Checked)
                    {
                        double preTax = 1 + (taxRate / 100);
                        double netTotalWithoutTax = avgCost / preTax;
                        double netCostPrice = netTotalWithoutTax / qty;
                        double grossBeforeDiscount = netCostPrice * qty;
                        double newNetTotal = grossBeforeDiscount - discountValue;
                        double taxAmount = (newNetTotal * taxRate) / 100;

                        grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = netCostPrice;
                        grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = taxAmount;
                        grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = newNetTotal + taxAmount;

                        //double total_avg_cost_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value);
                        //double qty_1 = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) * 1;
                        //double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                        //string pre_tax = "1." + tax_rate.ToString();
                        //double single_avg_cost = ((total_avg_cost_1 / double.Parse(pre_tax)) / qty_1);
                        //double tax = (single_avg_cost * tax_rate / 100);
                        //double new_tax_value = (tax * qty_1);

                        //grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value = single_avg_cost;
                        //grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = new_tax_value;
                        //double sub_total = ((single_avg_cost * qty_1) + new_tax_value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                        //grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total;
                    }
                }

                if (columnName == "discount")//if discount is changed
                {
                    double discountPercent = totalValue == 0 ? 0 : (discount / totalValue) * 100;
                    tax = ((totalValue - discount) * taxRate) / 100;
                    double finalSubtotal = totalValue - discount + tax;

                    grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = finalSubtotal;
                    grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = tax;

                    ////double sub_total = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["tax"].Value) + Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value) - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                    //double tax_rate = (grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[e.RowIndex].Cells["tax_rate"].Value.ToString()));
                    //double total_value = Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["qty"].Value);
                    //double tax_1 = ((total_value - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value)) * tax_rate / 100);
                    //double sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                    ////total_discount += Convert.ToDouble(grid_purchases.Rows[e.RowIndex].Cells["discount"].Value);
                    ////txt_total_discount.Text = total_amount.ToString();
                    //grid_purchases.Rows[e.RowIndex].Cells["tax"].Value = (tax_1);
                    //grid_purchases.Rows[e.RowIndex].Cells["sub_total"].Value = sub_total_1;
                }

                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_amount();
                get_total_qty();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }

        }

        void purchaseSearchObj_FormClosed(object sender, FormClosedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        public void Load_products_to_grid(string item_number, int RowIndex1)
        {
            ProductBLL productsBLL_obj = new ProductBLL();
            products_dt = productsBLL_obj.SearchRecordByProductNumber(item_number);
            //grid_purchases.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            int RowIndex = grid_purchases.CurrentCell.RowIndex;

            if (products_dt.Rows.Count > 0)
            {
                
                for (int i = 0; i < grid_purchases.RowCount; i++)
                {
                    var grid_item_number = (grid_purchases.Rows[i].Cells["item_number"].Value != null ? grid_purchases.Rows[i].Cells["item_number"].Value : "");
                    if (grid_item_number.ToString() == item_number)
                    {
                        MessageBox.Show("Product already added", "Already exist", MessageBoxButtons.OK, MessageBoxIcon.Question);
                        //DataView dvProducts = products_dt.DefaultView;
                        //dvProducts.RowFilter = ""
                        grid_purchases.Focus();
                        grid_purchases.CurrentCell = grid_purchases.Rows[RowIndex].Cells["code"]; //make qty cell active
                        grid_purchases.CurrentCell.Selected = true;
                        grid_purchases.BeginEdit(true);
                        return;
                    }
                    else
                    {
                        grid_purchases.CurrentCell = grid_purchases.Rows[RowIndex].Cells["qty"]; //make qty cell active
                        grid_purchases.CurrentCell.Selected = true;

                    }
                }
               
                foreach (DataRow myProductView in products_dt.Rows)
                {
                    double avg_cost = (myProductView["avg_cost"].ToString() != "" ? double.Parse(myProductView["avg_cost"].ToString()) : 0);
                    double unit_price = (myProductView["unit_price"].ToString() != "" ? double.Parse(myProductView["unit_price"].ToString()) : 0);
                    double qty = Convert.ToDouble(myProductView["purchase_demand_qty"].ToString() == string.Empty || (decimal)myProductView["purchase_demand_qty"] == 0 ? "1" : myProductView["purchase_demand_qty"].ToString());
                    double total = qty * avg_cost;
                    double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                    double tax = (total * tax_rate / 100);
                    double sub_total = tax + total;


                    grid_purchases.Rows[RowIndex].Cells["id"].Value = myProductView["id"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["code"].Value = myProductView["code"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["name"].Value = myProductView["name"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["qty"].Value = qty;
                    grid_purchases.Rows[RowIndex].Cells["avg_cost"].Value = avg_cost;
                    grid_purchases.Rows[RowIndex].Cells["unit_price"].Value = unit_price;
                    grid_purchases.Rows[RowIndex].Cells["discount"].Value = 0.00;
                    grid_purchases.Rows[RowIndex].Cells["tax"].Value = tax;
                    grid_purchases.Rows[RowIndex].Cells["sub_total"].Value = sub_total;
                    grid_purchases.Rows[RowIndex].Cells["location_code"].Value = myProductView["location_code"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["unit"].Value = myProductView["unit"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["category"].Value = myProductView["category"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["btn_delete"].Value = "Del";
                    grid_purchases.Rows[RowIndex].Cells["tax_id"].Value = myProductView["tax_id"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["tax_rate"].Value = myProductView["tax_rate"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["shop_qty"].Value = myProductView["qty"].ToString();
                    grid_purchases.Rows[RowIndex].Cells["item_number"].Value = myProductView["item_number"].ToString();
                    /////
                    //fill_locations_grid_combo(RowIndex);
                    ////
                }
                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_amount();
                get_total_qty();
            }
        }

        private void fill_locations_grid_combo(int RowIndex)
        {
            //var locationComboCell = new DataGridViewComboBoxCell();
            //DataTable dt = new DataTable();

            //GeneralBLL generalBLL_obj = new GeneralBLL();
            //string keyword = "code as location_code,name";
            //string table = "pos_locations";

            //dt = generalBLL_obj.GetRecord(keyword, table);

            //locationComboCell.DataSource = dt;
            //locationComboCell.DisplayMember = "location_code";
            //locationComboCell.ValueMember = "location_code";

            //grid_purchases.Rows[RowIndex].Cells["location_code"] = locationComboCell;
            //grid_purchases.Rows[RowIndex].Cells["location_code"].Value = dt.Rows[0]["location_code"].ToString(); // GET FIRST COLUMN OF DT TO SHOW FIRST VALUE AS SELECTED

        }

        private void btn_save_Click(object sender, EventArgs e)
        {
           
        }
        
        public bool checkNullOrEmpty(string keyword)
        {
            if (keyword == null || keyword == DBNull.Value.ToString() || String.IsNullOrEmpty(keyword)  || String.IsNullOrWhiteSpace(keyword.ToString()))
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        private void hold_purchases()
        {
            try
            {
               
                DialogResult result = UiMessages.ConfirmYesNoCancel("Are you sure you want to hold purchase?", "هل أنت متأكد من رغبتك في تأجيل عملية الشراء؟", "Hold Purchase Transaction " + invoice_status, "عقد معاملة الشراء " + invoice_status);

                if (result == DialogResult.Yes)
                {
                    if (grid_purchases.Rows.Count > 0)
                    {
                        PurchasesModal PurchasesModal_obj = new PurchasesModal();
                        var purchasesObj = new PurchasesBLL();
                        DateTime purchase_date = txt_purchase_date.Value.Date;
                        int employee_id = (cmb_employees.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_employees.SelectedValue.ToString()));
                        int supplier_id = _selectedSupplierId;
                
                        string invoice_no = "";
                        // near start of hold_purchases(), after supplier_id is read
                        var supplierInvoiceNo = (txt_supplier_invoice.Text ?? string.Empty).Trim();
                        
                        if (purchasesObj.IsHoldSupplierInvoiceNoExists(supplier_id, supplierInvoiceNo))
                        {
                            UiMessages.ShowWarning(
                                "This Supplier Invoice No. already exists in Hold purchases for the selected supplier.",
                                "رقم فاتورة المورد موجود بالفعل في المشتريات المحفوظة لنفس المورد.",
                                captionEn: "Duplicate Supplier Invoice",
                                captionAr: "تكرار رقم فاتورة المورد");
                            txt_supplier_invoice.Focus();
                            txt_supplier_invoice.SelectAll();
                            return;
                        }

                        invoice_no = GetMAXInvoiceNo_HOLD();
                        
                        PurchasesModal_obj.employee_id = employee_id;
                        PurchasesModal_obj.supplier_id = supplier_id;
                        PurchasesModal_obj.supplier_invoice_no = txt_supplier_invoice.Text;
                        PurchasesModal_obj.invoice_no = invoice_no;
                        PurchasesModal_obj.total_amount = Math.Round(total_amount, 4);
                        PurchasesModal_obj.purchase_type = cmb_purchase_type.SelectedValue.ToString();
                        PurchasesModal_obj.total_discount = Math.Round(total_discount, 4);
                        PurchasesModal_obj.total_tax = Math.Round(total_tax, 4);
                        PurchasesModal_obj.description = txt_description.Text;
                        PurchasesModal_obj.purchase_date = purchase_date;
                        PurchasesModal_obj.account = "Purchase";

                        //set the date from datetimepicker and set time to te current time
                        DateTime now = DateTime.Now;
                        txt_purchase_date.Value = new DateTime(txt_purchase_date.Value.Year, txt_purchase_date.Value.Month, txt_purchase_date.Value.Day, now.Hour, now.Minute, now.Second);
                        /////////////////////
                        PurchasesModal_obj.purchase_time = txt_purchase_date.Value;

                        Int32 purchase_id = purchasesObj.Insert_hold_purchases(PurchasesModal_obj);
                        int sno = 1;
                        for (int i = 0; i < grid_purchases.Rows.Count; i++)
                        {
                            if (grid_purchases.Rows[i].Cells["id"].Value != null)
                            {
                                PurchasesModal_obj.serialNo = sno++;
                                PurchasesModal_obj.purchase_id = purchase_id;
                                PurchasesModal_obj.code = grid_purchases.Rows[i].Cells["code"].Value.ToString();
                                PurchasesModal_obj.item_number = grid_purchases.Rows[i].Cells["item_number"].Value.ToString();
                                //PurchasesModal_obj.name = grid_purchases.Rows[i].Cells["name"].Value.ToString();
                                PurchasesModal_obj.quantity = decimal.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString());
                                PurchasesModal_obj.cost_price = Math.Round(Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value.ToString()), 4);
                                PurchasesModal_obj.unit_price =Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["unit_price"].Value.ToString()), 4);
                                PurchasesModal_obj.discount = Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["discount"].Value.ToString()), 4);
                                PurchasesModal_obj.tax_id = Convert.ToInt32(grid_purchases.Rows[i].Cells["tax_id"].Value.ToString());
                                decimal tax_rate = (grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : decimal.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));
                                PurchasesModal_obj.tax_rate = (chkbox_is_taxable.Checked ? Math.Round(tax_rate, 4) : 0);
                                if (grid_purchases.Rows[i].Cells["location_code"].Value == null || grid_purchases.Rows[i].Cells["location_code"].Value == DBNull.Value || String.IsNullOrEmpty(grid_purchases.Rows[i].Cells["location_code"].Value as String) || String.IsNullOrWhiteSpace(grid_purchases.Rows[i].Cells["location_code"].Value.ToString()))
                                {
                                    PurchasesModal_obj.location_code = "";
                                }
                                else
                                {
                                    PurchasesModal_obj.location_code = grid_purchases.Rows[i].Cells["location_code"].Value.ToString();
                                }

                                purchasesObj.Insert_hold_purchasesItems(PurchasesModal_obj);
                            }

                        }
                        if (purchase_id > 0)
                        {
                            UiMessages.ShowInfo(invoice_no + " hold purchases transaction saved successfully", invoice_no + "تم حفظ عملية الشراء المعلقة بنجاح", "Success " + invoice_status, "نجاح " + invoice_status);

                            clear_form();
                            //GetMAXInvoiceNo_HOLD();

                        }
                        else
                        {
                            UiMessages.ShowError("Record not saved", "لم يتم حفظ السجل");
                        }
                    }
                    else
                    {
                        UiMessages.ShowWarning("Please add products", "يرجى إضافة منتجات");
                    }
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void get_sub_total_amount()
        {
            total_sub_total = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_sub_total += Convert.ToDecimal(grid_purchases.Rows[i].Cells["qty"].Value) * Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value);
            }

            decimal shippingCost = (string.IsNullOrWhiteSpace(txt_shipping_cost.Text) ? 0 : Convert.ToDecimal(txt_shipping_cost.Text));
            txt_sub_total.Text = Math.Round(total_sub_total + shippingCost, 2).ToString();
        }

        private void get_total_amount()
        {
            total_amount = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_amount += Convert.ToDecimal(grid_purchases.Rows[i].Cells["qty"].Value) * Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value);
            }

            decimal shippingCost = (string.IsNullOrWhiteSpace(txt_shipping_cost.Text) ? 0 : Convert.ToDecimal(txt_shipping_cost.Text));
            decimal net = (total_amount + total_tax - total_discount + shippingCost);
            txt_total_amount.Text = Math.Round(net,2).ToString();
        }

        private void txt_shipping_cost_TextChanged(object sender, EventArgs e)
        {
            try
            {
                decimal shippingCost = (string.IsNullOrWhiteSpace(txt_shipping_cost.Text) ? 0 : Convert.ToDecimal(txt_shipping_cost.Text));

                if (chkbox_is_taxable.Checked && shippingCost != 0)
                {
                    decimal itemsNetTotal = 0;

                    for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
                    {
                        if (grid_purchases.Rows[i].Cells["id"].Value == null || grid_purchases.Rows[i].Cells["code"].Value == null)
                        {
                            continue;
                        }

                        decimal qty = Convert.ToDecimal(grid_purchases.Rows[i].Cells["qty"].Value);
                        decimal avgCost = Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value);
                        decimal discount = Convert.ToDecimal(grid_purchases.Rows[i].Cells["discount"].Value);
                        decimal lineNet = (qty * avgCost) - discount;
                        if (lineNet > 0)
                        {
                            itemsNetTotal += lineNet;
                        }
                    }

                    for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
                    {
                        if (grid_purchases.Rows[i].Cells["id"].Value == null || grid_purchases.Rows[i].Cells["code"].Value == null)
                        {
                            continue;
                        }

                        decimal qty = Convert.ToDecimal(grid_purchases.Rows[i].Cells["qty"].Value);
                        decimal avgCost = Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value);
                        decimal discount = Convert.ToDecimal(grid_purchases.Rows[i].Cells["discount"].Value);
                        decimal taxRate = (grid_purchases.Rows[i].Cells["tax_rate"].Value == null || grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : Convert.ToDecimal(grid_purchases.Rows[i].Cells["tax_rate"].Value));

                        decimal lineNet = (qty * avgCost) - discount;
                        if (lineNet < 0)
                        {
                            lineNet = 0;
                        }

                        decimal shippingShare = (itemsNetTotal > 0 ? (shippingCost * (lineNet / itemsNetTotal)) : 0);
                        decimal taxableBase = lineNet + shippingShare;
                        decimal tax = Math.Round((taxableBase * taxRate) / 100, 4);

                        grid_purchases.Rows[i].Cells["tax"].Value = tax;
                        grid_purchases.Rows[i].Cells["sub_total"].Value = Math.Round(taxableBase + tax, 4);
                    }
                }

                get_total_tax();
                get_total_discount();
                get_sub_total_amount();
                get_total_amount();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void get_total_tax()
        {
            total_tax = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_tax += Convert.ToDecimal(grid_purchases.Rows[i].Cells["tax"].Value);
            }

            txt_total_tax.Text =Math.Round(total_tax,2).ToString();
        }
        private void get_total_discount()
        {
            total_discount = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_discount += Convert.ToDecimal(grid_purchases.Rows[i].Cells["discount"].Value);
            }

            txt_total_discount.Text = Math.Round(total_discount,2).ToString();
        }

        private void get_total_qty()
        {
            double total_qty = 0;

            for (int i = 0; i <= grid_purchases.Rows.Count - 1; i++)
            {
                total_qty += Convert.ToDouble(grid_purchases.Rows[i].Cells["qty"].Value);
            }

            txt_total_qty.Text = (total_qty).ToString();
            
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
                payable_account_id = (int)dr["payable_acc_id"];
                tax_account_id = (int)dr["tax_acc_id"];
                purchases_discount_acc_id = (int)dr["purchases_discount_acc_id"];
                inventory_acc_id = (int)dr["inventory_acc_id"];
                purchases_acc_id = (int)dr["purchases_acc_id"];
            }
        }


        private void txt_barcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txt_barcode.Text != string.Empty && e.KeyCode == Keys.Enter)
            {
                load_products("", "", txt_barcode.Text.Trim());
                txt_barcode.Text = "";

            }
        }
        public void load_products(string product_id = "", string product_name = "", string barcode = "")
        {

            ProductBLL productsBLL_obj = new ProductBLL();
            //DataTable product_dt = productsBLL_obj.SearchRecordByProductCode(txt_product_code.Text);
            DataTable product_dt = new DataTable();

            if (product_id != string.Empty)
            {
                product_dt = productsBLL_obj.SearchRecordByProductID(product_id);
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
                    double qty = Convert.ToDouble(myProductView["purchase_demand_qty"].ToString() == string.Empty || (decimal)myProductView["purchase_demand_qty"] == 0 ? "1" : myProductView["purchase_demand_qty"].ToString());
                    double total = qty * double.Parse(myProductView["avg_cost"].ToString());
                    double tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : double.Parse(myProductView["tax_rate"].ToString()));
                    double tax = (total * tax_rate / 100);
                    double current_sub_total = tax + total;

                    int id = Convert.ToInt32(myProductView["id"]);
                    string code = myProductView["code"].ToString();
                    string name = myProductView["name"].ToString();
                    //string qty= (myProductView["purchase_demand_qty"].ToString() == string.Empty || (decimal)myProductView["purchase_demand_qty"] == 0 ? "1" : myProductView["purchase_demand_qty"].ToString());
                    double avg_cost = Convert.ToDouble(myProductView["avg_cost"]);
                    double unit_price = Convert.ToDouble(myProductView["unit_price"]);
                    double discount = 0.00;
                    string location_code = myProductView["location_code"].ToString();
                    string unit = myProductView["unit"].ToString();
                    string category = myProductView["category"].ToString();
                    string btn_delete = "Del";
                    string tax_id = myProductView["tax_id"].ToString();
                    string shop_qty = myProductView["qty"].ToString();

                    string[] row0 = { "", id.ToString(), code, name, 
                                            qty.ToString(), avg_cost.ToString(),unit_price.ToString(), discount.ToString(), 
                                            tax.ToString(), current_sub_total.ToString(),location_code,unit,category, 
                                            btn_delete, tax_id.ToString(), tax_rate.ToString(), unit_price.ToString(),shop_qty };
                    //Remove the first empty row
                    if (grid_purchases.RowCount > 0 && grid_purchases.Rows[0].Cells["id"].Value == null)
                    {
                        grid_purchases.Rows.RemoveAt(0);
                    }
                    //
                    int RowIndex = grid_purchases.Rows.Add(row0);

                    //GET / SET Location Dropdown list
                    /////
                    //fill_locations_grid_combo(RowIndex);
                    //////////

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();
                    get_total_qty();
                }

                txt_barcode.Focus();

            }
            else
            {

                MessageBox.Show("Record not found", "Products", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                //txt_product_code.Focus();
                txt_barcode.Focus();
            }


        }


        private void frm_purchases_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                // Don't hijack key handling while supplier search box is active
                if (txtSupplierSearch != null && txtSupplierSearch.Focused)
                {
                    return;
                }

                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    //SendKeys.Send("{TAB}");
                }
                if (e.KeyData == Keys.F1)
                {
                    NewToolStripButton.PerformClick();
                }
                if (e.KeyData == Keys.F3)
                {
                    SaveToolStripButton.PerformClick();
                }
                if (e.KeyData == Keys.F4)
                {
                    SearchToolStripButton.PerformClick();
                }
                if (e.Control && e.KeyCode == Keys.H)
                {
                    HistoryToolStripButton.PerformClick();
                }
                if (e.KeyCode == Keys.F9)
                {
                    grid_purchases.Focus();
                }
                if (e.Control && e.KeyCode == Keys.L)
                {
                    frm_search_porder obj = new frm_search_porder(this);
                    obj.ShowDialog();
                }
                
                if (e.Control && e.KeyCode == Keys.O)
                {
                    SearchToolStripButton.PerformClick();
                }

                if (e.KeyCode == Keys.Escape)
                {

                }
                if (e.KeyData == Keys.Down)
                {
                    brandsDataGridView.Focus();
                    categoriesDataGridView.Focus();
                    groupsDataGridView.Focus();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void grid_purchases_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string name = grid_purchases.Columns[e.ColumnIndex].Name;
                if (name == "btn_delete")
                {
                    grid_purchases.Rows.RemoveAt(e.RowIndex);

                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();
                    get_total_qty();
                }

            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");

            }

        }

        private void clear_form()
        {
            //cmb_suppliers.SelectedValue = 0;
            //cmb_suppliers.Refresh();
            //cmb_categories.SelectedValue = 0;
            cmb_employees.SelectedValue = 0;
            cmb_purchase_type.SelectedValue = "Cash";
            //cmb_brands.SelectedValue = 0;
            invoice_status = "";
            //btn_save.Text = "Create (F3)";
            PrinttoolStripButton.Enabled = false;

            txt_brand_code.Text = "";
            txt_brands.Text = "";
            txt_category_code.Text = "";
            txt_categories.Text = "";
            txt_groups.Text = "";
            txt_group_code.Text = "";

            txt_description.Text = "";
            //txt_discount.Text = "";
            //txt_vat.Text = "";

            global_product_id = 0;
            //global_avg_cost = 0;
            global_tax_id = 0;
            global_tax_rate = 0;

            total_amount = 0;
            total_discount = 0;
            total_tax = 0;
            total_sub_total = 0;

            txt_invoice_no.Text = "";
            txt_purchase_date.Refresh();
            txt_purchase_date.Value = DateTime.Now;
            txt_supplier_invoice.Text = "";
            txt_supplier_vat.Text = "";
            txt_cust_balance.Text = "";

            _selectedSupplierId = 0;
            _suppressSupplierSearch = true;
            txtSupplierSearch.Text = "";
            _suppressSupplierSearch = false;

            txt_sub_total.Text = "0.00";
            txt_total_amount.Text = "0.00";
            txt_total_tax.Text = "0.00";
            txt_total_discount.Text = "0.00";
            txt_total_qty.Text = "";
            txt_shop_qty.Text = "";
            
            rd_btn_by_unitprice.Checked = true;
            rd_btn_without_vat.Checked = true;

            grid_purchases.DataSource = null;
            grid_purchases.Rows.Clear();
            grid_purchases.Refresh();
            grid_purchases.Rows.Add();
            //this.ActiveControl = grid_purchases;

            grid_product_history.DataSource = null;
            grid_product_history.Rows.Clear();
            //grid_product_history.Refresh();

        }

        // Supplier combobox list/handler removed; supplier is selected via `txtSupplierSearch`.

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


        private void get_purchasetype_dropdownlist()
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

            if (allow_credit_purchase)
            {
                DataRow _row = dt.NewRow();
                _row["id"] = "Credit";
                if (lang == "en-US")
                {
                    _row["name"] = "Credit";
                }
                else if (lang == "ar-SA")
                {
                    _row["name"] = "اجل";
                }

                dt.Rows.Add(_row);

            }

            DataRow _row_2 = dt.NewRow();
            _row_2["id"] = "Hold";
            if (lang == "en-US")
            {
                _row_2["name"] = "Hold";
            }
            else if (lang == "ar-SA")
            {
                _row_2["name"] = "يحفظ";
            }
            dt.Rows.Add(_row_2);


            //DataRow _row_3 = dt.NewRow();
            //_row_3["id"] = "Return";
            //if (lang == "en-US")
            //{
            //    _row_3["name"] = "Return";
            //}
            //else if (lang == "ar-SA")
            //{
            //    _row_3["name"] = "يعود";
            //}
            //dt.Rows.Add(_row_3);

            cmb_purchase_type.DataSource = dt;

            cmb_purchase_type.DisplayMember = "name";
            cmb_purchase_type.ValueMember = "id";

        }
      
        public void load_user_rights(int user_id)
        {
            UsersBLL userBLL_obj = new UsersBLL();
            DataTable users = userBLL_obj.GetUserRights(user_id);

            foreach (DataRow dr in users.Rows)
            {
                ///USER RIGHTS

                //cash_sales_amount_limit = Convert.ToDouble(dr["cash_sales_amount"]);
                //txt_credit_sales_amt= (double) dr["credit_sales_amount"].ToString();
                cash_purchase_amount_limit = Convert.ToDouble(dr["cash_purchase_amount"]);
                //txt_credit_purchase_amt.Text = (double) dr["credit_purchase_amount"].ToString();
                //chk_allow_cash_sales.Checked = (bool)dr["allow_cash_sales"];
                //allow_credit_sales = (bool)dr["allow_credit_sales"];
                //allow_cash_purchase = (bool)dr["allow_cash_purchase"];
                allow_credit_purchase = (bool)dr["allow_credit_purchase"];

            }
        }

        private void grid_purchases_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Delete)
                {
                    MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                    DialogResult result = MessageBox.Show("Are you sure you want delete", "Delete", buttons, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        grid_purchases.Rows.RemoveAt(grid_purchases.CurrentRow.Index);

                        get_total_tax();
                        get_total_discount();
                        get_sub_total_amount();
                        get_total_amount();
                        get_total_qty();
                    }
                }

                if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
                {
                    e.SuppressKeyPress = true;

                    // Commit the active cell edit before navigating;
                    // changing CurrentCell while in edit mode throws InvalidOperationException.
                    if (grid_purchases.IsCurrentCellInEditMode)
                        grid_purchases.EndEdit();

                    if (grid_purchases.CurrentCell == null) return;

                    int iColumn = grid_purchases.CurrentCell.ColumnIndex;
                    int iRow    = grid_purchases.CurrentCell.RowIndex;

                    int snoIdx  = grid_purchases.Columns["sno"].Index;
                    int idIdx   = grid_purchases.Columns["id"].Index;
                    int codeIdx = grid_purchases.Columns["code"].Index;

                    if (iColumn == snoIdx || iColumn == idIdx)
                    {
                        grid_purchases.CurrentCell = grid_purchases.Rows[iRow].Cells[codeIdx];
                        grid_purchases.Focus();
                        grid_purchases.CurrentCell.Selected = true;
                        grid_purchases.BeginEdit(true);
                    }
                    else if (iColumn <= 9)
                    {
                        if (grid_purchases.Rows[iRow].Cells["code"].Value != null && grid_purchases.Rows[iRow].Cells["avg_cost"].Value != null && grid_purchases.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_purchases.CurrentCell = grid_purchases.Rows[iRow].Cells[iColumn + 1];
                            grid_purchases.Focus();
                            grid_purchases.CurrentCell.Selected = true;
                        }
                    }
                    else if (iColumn > 9)
                    {
                        if (grid_purchases.Rows[iRow].Cells["code"].Value != null && grid_purchases.Rows[iRow].Cells["avg_cost"].Value != null && grid_purchases.Rows[iRow].Cells["qty"].Value != null)
                        {
                            grid_purchases.Rows.Add();
                            this.ActiveControl = grid_purchases;
                            grid_purchases.CurrentCell = grid_purchases.Rows[iRow + 1].Cells["code"];
                            grid_purchases.CurrentCell.Selected = true;
                            grid_purchases.BeginEdit(true);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }

        }

        public void Load_products_to_grid_by_invoiceno(DataTable _dt, string invoice_no)
        {
            try
            {
                // End any pending cell edit before clearing rows; otherwise the grid
                // throws InvalidOperationException ("cannot commit or quit a cell value change").
                if (grid_purchases.IsCurrentCellInEditMode)
                    grid_purchases.EndEdit();
                grid_purchases.CurrentCell = null;

                grid_purchases.Rows.Clear();
                grid_purchases.Refresh();
                txt_invoice_no.Text = invoice_no;

                string invoice_chr = invoice_no.Substring(0, 2);

                //string invoice_chr = invoice_no.Substring(0, 1);

                if (invoice_chr.ToUpper() == "PO")// for invoice edit
                {
                    invoice_status = "PO";
                }
                else if (invoice_no.Substring(0, 1).ToUpper() == "H")// for HOld purchases invoice 
                {
                    invoice_status = "PO";
                }
                else
                {
                    invoice_status = "PO"; // "Update";
                    PrinttoolStripButton.Enabled = true;
                }

                //btn_save.Text = "Save (F3)";
                po_invoice_no = invoice_no;

                if (_dt.Rows.Count > 0)
                {

                    foreach (DataRow myProductView in _dt.Rows)
                    {
                        txt_supplier_invoice.Text = myProductView["supplier_invoice_no"].ToString();
                        _selectedSupplierId = (myProductView["supplier_id"] == null ? 0 : Convert.ToInt32(myProductView["supplier_id"]));
                        try
                        {
                            _suppressSupplierSearch = true;
                            if (_selectedSupplierId > 0)
                            {
                                var bll = new SupplierBLL();
                                var dtSup = bll.SearchRecordBySupplierID(_selectedSupplierId);
                                if (dtSup != null && dtSup.Rows.Count > 0)
                                    SelectSupplierFromDataRow(dtSup.Rows[0]);
                                else
                                    ClearSelectedSupplier();
                            }
                            else
                            {
                                ClearSelectedSupplier();
                                txtSupplierSearch.Text = "";
                            }
                        }
                        finally
                        {
                            _suppressSupplierSearch = false;
                        }
                        cmb_employees.SelectedValue = myProductView["employee_id"];
                        //cmb_purchase_type.SelectedValue = myProductView["purchase_type"];
                        //txt_purchase_date.Value = Convert.ToDateTime(myProductView["purchase_date"].ToString());
                        txt_description.Text = myProductView["description"].ToString();
                        txt_shipping_cost.Text = (string.IsNullOrEmpty(myProductView["shipping_cost"].ToString() as String) ? "" : myProductView["shipping_cost"].ToString());

                        decimal qty = Math.Round(Convert.ToDecimal(myProductView["quantity"].ToString()),2);
                        decimal discount = Math.Round(Convert.ToDecimal(myProductView["discount_value"]), 4);
                        decimal total = qty * decimal.Parse(myProductView["cost_price"].ToString());
                        decimal tax_rate = (myProductView["tax_rate"].ToString() == "" ? 0 : decimal.Parse(myProductView["tax_rate"].ToString()));
                        decimal tax = Math.Round(((total- discount) * tax_rate / 100),4);
                        //decimal tax = Math.Round(Convert.ToDecimal(myProductView["vat"].ToString()), 2);
                        decimal current_sub_total = Math.Round((tax + total-discount),2);

                        int id = Convert.ToInt32(myProductView["id"]);
                        string code = myProductView["code"].ToString();
                        string name = myProductView["name"].ToString();
                        //string qty = myProductView["quantity"].ToString();
                        decimal cost_price = Math.Round(Convert.ToDecimal(myProductView["cost_price"]),4);
                        decimal unit_price = Math.Round( Convert.ToDecimal(myProductView["unit_price"]),4);
                        string location_code = myProductView["location_code"].ToString();
                        string unit = myProductView["unit"].ToString();
                        string category = myProductView["category"].ToString();
                        string item_type = myProductView["item_type"].ToString();
                        string btn_delete = "Del";
                        string tax_id = myProductView["tax_id"].ToString();
                        decimal shop_qty = 0;
                        string item_number = myProductView["item_number"].ToString();

                        string[] row0 = { "", id.ToString(), code, name, 
                                            qty.ToString(), cost_price.ToString(),unit_price.ToString(), discount.ToString(), 
                                            tax.ToString(), current_sub_total.ToString(),location_code,unit,category, 
                                            btn_delete, tax_id.ToString(), tax_rate.ToString(), shop_qty.ToString(),item_number };
                        int RowIndex = grid_purchases.Rows.Add(row0);

                        //GET / SET Location Dropdown list
                        //fill_locations_grid_combo(RowIndex);
                        //////
                    }
                    //grid_purchases.Rows.Add();
                    get_total_tax();
                    get_total_discount();
                    get_sub_total_amount();
                    get_total_amount();
                    get_total_qty();
                    grid_purchases.Focus();

                    this.ActiveControl = grid_purchases;
                    grid_purchases.Focus();
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }


        private void grid_purchases_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            e.Control.KeyPress -= new KeyPressEventHandler(tb_KeyPress);

            if (_numericColumns.Contains(grid_purchases.Columns[grid_purchases.CurrentCell.ColumnIndex].Name)) // Qty, avg_cost, unit_price, discount — numeric only
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

            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
                return;
            }
           
            // Allow only one decimal point
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


        private void productMovementF4ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HistoryToolStripButton.PerformClick();
        }

        private void grid_purchases_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            grid_purchases.Rows[e.RowIndex].Cells["sno"].Value = (e.RowIndex + 1).ToString();
        }

        private void addNewRowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grid_purchases.Rows.Add();
        }
        private void SetupBrandDataGridView()
        {
            brandsDataGridView.ColumnCount = 2;
            brandsDataGridView.Name = "brandsDataGridView";
            brandsDataGridView.Size = new Size(250, 250);

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
            PositionDropdownGrid(brandsDataGridView, txt_brands);
            brandsDataGridView.BringToFront();

        }

        void brandsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(brandsDataGridView);
                grid_purchases.Focus();
            }
        }

        private void brandsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_brand_code.Text = brandsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_brands.Text = brandsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(brandsDataGridView);
            grid_purchases.Focus();

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

                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
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
            categoriesDataGridView.ColumnCount = 2;
            categoriesDataGridView.Name = "categoriesDataGridView";
            categoriesDataGridView.Size = new Size(250, 250);

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
            PositionDropdownGrid(categoriesDataGridView, txt_categories);
            categoriesDataGridView.BringToFront();

        }

        void categoriesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(categoriesDataGridView);
                grid_purchases.Focus();

            }
        }

        private void categoriesDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_category_code.Text = categoriesDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_categories.Text = categoriesDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(categoriesDataGridView);
            grid_purchases.Focus();

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

                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
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
            groupsDataGridView.ColumnCount = 2;
            groupsDataGridView.Name = "groupsDataGridView";
            groupsDataGridView.Size = new Size(250, 250);

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
            PositionDropdownGrid(groupsDataGridView, txt_groups);
            groupsDataGridView.BringToFront();

        }

        void groupsDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
                txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
                this.Controls.Remove(groupsDataGridView);
                grid_purchases.Focus();

            }
        }

        private void groupsDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txt_group_code.Text = groupsDataGridView.CurrentRow.Cells[0].Value.ToString();
            txt_groups.Text = groupsDataGridView.CurrentRow.Cells[1].Value.ToString();
            this.Controls.Remove(groupsDataGridView);
            grid_purchases.Focus();

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

                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void txt_groups_Leave(object sender, EventArgs e)
        {
            if (!groupsDataGridView.Focused)
            {
                this.Controls.Remove(groupsDataGridView);
            }

        }

        private void grid_purchases_SelectionChanged(object sender, EventArgs e)
        {
            if (grid_purchases.Rows.Count > 0 && grid_purchases.Focused)
            {
                string product_code = (grid_purchases.CurrentRow.Cells["code"].Value != null ? grid_purchases.CurrentRow.Cells["code"].Value.ToString() : "");
                string item_number = (grid_purchases.CurrentRow.Cells["item_number"].Value != null ? grid_purchases.CurrentRow.Cells["item_number"].Value.ToString() : "");
                if (item_number != "")
                {
                    if (grid_product_history.Rows.Count > 0) // if history grid is empty then load product history 
                    {
                        //if product history grid has already same product the don not load but 
                        //if history gird has different product the load
                        //it will improve performance
                        if (item_number != grid_product_history.CurrentRow.Cells["history_item_number"].Value.ToString())
                        {
                            load_product_purchase_history(item_number);
                            txt_shop_qty.Text = (grid_purchases.CurrentRow.Cells["shop_qty"].Value != null ? grid_purchases.CurrentRow.Cells["shop_qty"].Value.ToString() : "");

                        }
                    }
                    else
                    {
                        load_product_purchase_history(item_number);
                        txt_shop_qty.Text = (grid_purchases.CurrentRow.Cells["shop_qty"].Value != null ? grid_purchases.CurrentRow.Cells["shop_qty"].Value.ToString() : "");

                    }
                }

            }
            
        }

        public void load_product_purchase_history(string item_number)
        {
            try
            {
                grid_product_history.DataSource = null;

                //bind data in data grid view  
                GeneralBLL objBLL = new GeneralBLL();
                grid_product_history.AutoGenerateColumns = false;

                String keyword = "TOP 100 I.id,P.name AS product_name,I.item_code,I.item_number,I.qty,I.unit_price," +
                    "I.cost_price,I.invoice_no,I.description,I.trans_date, Concat(S.first_name, ' - ',PR.supplier_invoice_no) AS supplier";
                String table = "pos_inventory I LEFT JOIN pos_products P ON P.code=I.item_code " +
                    "LEFT JOIN pos_suppliers S ON S.id=I.supplier_id " +
                    "LEFT JOIN pos_purchases PR on PR.supplier_id = I.supplier_id " +
                    "WHERE I.item_number = '" + item_number + "' AND I.description = 'Purchase' ORDER BY I.id DESC";
                grid_product_history.DataSource = objBLL.GetRecord(keyword, table);


            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }

        }

        private void txt_total_disc_value_TextChanged(object sender, EventArgs e)
        {            
            decimal new_total_discount = (string.IsNullOrEmpty(txt_total_disc_value.Text) ? 0 : Convert.ToDecimal(txt_total_disc_value.Text));

            total_discount_value(new_total_discount);
        }


        public void total_discount_value(decimal total_discount_value)
        {
            int total_rows = grid_purchases.Rows.Count;
            int filled_rows = 0;
            
            for (int i = 0; i <= total_rows - 1; i++)
            {
                Int32 product_id = Convert.ToInt32(grid_purchases.Rows[i].Cells["id"].Value);
                if (product_id > 0)
                {
                    filled_rows++;
                }
            }
            //txt_total_amount.Text = round_total_amount.ToString();
            //double total_diff_amount = old_total_amount - new_amount; // calculate difference amount and add to single item unit price
            //double total_item_share = sub_total * 100 / old_total_amount;
            //double total_tax_share = (old_total_amount - sub_total) * 100 / old_total_amount;
            decimal diff_amount_per_item = (filled_rows > 0 ? (total_discount_value / filled_rows) : 0);

            //double new_amount_total = 0;
            //double new_amount_single = 0;
            //double new_vat_total = 0;
            //double net_total = 0;

            double tax_1 = 0;
            double total_value = 0;
            double tax_rate = 0;
            double sub_total_1 = 0;


                for (int i = 0; i <= filled_rows - 1; i++)
                {
                    int product_id = Convert.ToInt32(grid_purchases.Rows[i].Cells["id"].Value);
                    if(product_id > 0)
                    {
                        grid_purchases.Rows[i].Cells["discount"].Value = Math.Round(diff_amount_per_item,3);

                        //new_amount_single = (double.Parse(grid_purchases.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString()));
                        //new_amount_total = new_amount_single * total_item_share / 100;
                        //new_vat_total = new_amount_single * total_tax_share / 100;
                        //net_total = (grid_purchases.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                        //grid_purchases.Rows[i].Cells["unit_price"].Value = net_total;

                        tax_rate = (grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));

                        ////grid_purchases.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_purchases.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);

                        total_value = Convert.ToDouble(grid_purchases.Rows[i].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[i].Cells["qty"].Value);

                        tax_1 = ((total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                        sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);

                        grid_purchases.Rows[i].Cells["sub_total"].Value = sub_total_1;
                        ////grid_purchases.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                        grid_purchases.Rows[i].Cells["tax"].Value = (tax_1);

                        get_total_tax();
                        get_total_discount();
                        get_sub_total_amount();
                        get_total_amount();
                        get_total_qty(); 
                    }
                    
            }

        }

        private void productDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_purchases.RowCount > 0)
                {
                    if (grid_purchases.CurrentRow.Cells["item_number"].Value != null)
                    {
                        string item_number = grid_purchases.CurrentRow.Cells["item_number"].Value.ToString();

                        frm_product_full_detail obj = new frm_product_full_detail(null,this, item_number);
                        obj.ShowDialog();
                    }
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
                int total_rows = grid_purchases.Rows.Count;
                double tax_1 = 0;
                double total_value = 0;
                double tax_rate = 0;
                double sub_total_1 = 0;

                for (int i = 0; i <= total_rows - 1; i++)
                {
                    string product_code = grid_purchases.Rows[i].Cells["code"].Value.ToString();
                    if (product_code.Length > 0)
                    {

                        //new_amount_single = (double.Parse(grid_purchases.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString()));
                        //new_amount_total = new_amount_single * total_item_share / 100;
                        //new_vat_total = new_amount_single * total_tax_share / 100;
                        //net_total = (grid_purchases.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                        //grid_purchases.Rows[i].Cells["unit_price"].Value = net_total;

                        tax_rate = 0; // (grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : double.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString())));

                        ////grid_purchases.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_purchases.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);

                        total_value = Convert.ToDouble(grid_purchases.Rows[i].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[i].Cells["qty"].Value);

                        tax_1 = ((total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                        sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);
                        grid_purchases.Rows[i].Cells["tax"].Value = tax_1;

                        grid_purchases.Rows[i].Cells["sub_total"].Value = sub_total_1;
                        ////grid_purchases.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                        
                       
                    }
                }
            }
            else
            {
                int total_rows = grid_purchases.Rows.Count;
                double tax_1 = 0;
                double total_value = 0;
                double tax_rate = 0;
                double sub_total_1 = 0;

                for (int i = 0; i <= total_rows - 1; i++)
                {
                    string product_code = grid_purchases.Rows[i].Cells["code"].Value.ToString();
                    if (product_code.Length > 0)
                    {

                        //new_amount_single = (double.Parse(grid_purchases.Rows[i].Cells["sub_total"].Value.ToString()) / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString())) - (diff_amount_per_item / double.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString()));
                        //new_amount_total = new_amount_single * total_item_share / 100;
                        //new_vat_total = new_amount_single * total_tax_share / 100;
                        //net_total = (grid_purchases.Rows[i].Cells["tax"].Value.ToString() == "0" ? (new_amount_total + new_vat_total) : new_amount_total);
                        //grid_purchases.Rows[i].Cells["unit_price"].Value = net_total;

                        tax_rate = (grid_purchases.Rows[i].Cells["tax_rate"].Value == "" ? 0 : double.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));

                        ////grid_purchases.Rows[i].Cells["sub_total"].Value = Convert.ToDouble(grid_purchases.Rows[i].Cells["sub_total"].Value) - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);

                        total_value = Convert.ToDouble(grid_purchases.Rows[i].Cells["avg_cost"].Value) * Convert.ToDouble(grid_purchases.Rows[i].Cells["qty"].Value);

                        tax_1 = ((total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value)) * tax_rate / 100);

                        sub_total_1 = tax_1 + total_value - Convert.ToDouble(grid_purchases.Rows[i].Cells["discount"].Value);
                        grid_purchases.Rows[i].Cells["tax"].Value = tax_1;
                        grid_purchases.Rows[i].Cells["sub_total"].Value = sub_total_1;
                        ////grid_purchases.Rows[i].Cells["total_without_vat"].Value = (sub_total_1 - tax_1);
                        //grid_purchases.Rows[i].Cells["tax"].Value = 0;

                        
                    }
                }
            }
            get_total_tax();
            get_total_discount();
            get_sub_total_amount();
            get_total_amount();
            get_total_qty();
        }

        private void grid_purchases_DataError(object sender, DataGridViewDataErrorEventArgs anError)
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

        private void NewToolStripButton_Click(object sender, EventArgs e)
        {
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show("Are you sure you want new sale transaction", "New Transaction", buttons, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                clear_form();
            }
            this.ActiveControl = grid_purchases;
            grid_purchases.CurrentCell = grid_purchases.Rows[0].Cells["code"];
        }

        private void SaveToolStripButton_Click(object sender, EventArgs e)
        {
            using (BusyScope.Show(this, UiMessages.T("Saving purchase...", "جاري حفظ المشتريات...")))
            {
                try
                {
                    // Permission check
                    if (!_auth.HasPermission(_currentUser, Permissions.Purchases_Create))
                    {
                        UiMessages.ShowWarning(
                            "You do not have permission to create purchase transactions.\nPlease contact your system administrator.",
                            "ليس لديك صلاحية لإنشاء معاملات شراء.\nيرجى التواصل مع مدير النظام.",
                            captionEn: "Permission denied",
                            captionAr: "صلاحية مرفوضة");
                        return;
                    }

                    string purchase_type = (string.IsNullOrEmpty(cmb_purchase_type.SelectedValue.ToString()) ? "Cash" : cmb_purchase_type.SelectedValue.ToString());
                    int supplier_id = _selectedSupplierId;
                    string bankID = "";
                    string bankGLAccountID = "";
                    string paymentMethodText = cmb_payment_method.Text;
                    int payment_method_id = (cmb_payment_method.SelectedValue == null ? 0 : Convert.ToInt32(cmb_payment_method.SelectedValue));


                    if (purchase_type == "Hold")
                    {
                        hold_purchases();
                        return;
                    }

                    if (supplier_id <= 0)
                    {
                        UiMessages.ShowWarning(
                            "Supplier is required.",
                            "المورد مطلوب.",
                            captionEn: "Validation",
                            captionAr: "التحقق");

                        suppliersDataGridView.Visible = false;
                        this.ActiveControl = txtSupplierSearch;
                        txtSupplierSearch.Focus();
                        txtSupplierSearch.SelectAll();

                        BeginInvoke((Action)(() =>
                        {
                            this.ActiveControl = txtSupplierSearch;
                            txtSupplierSearch.Focus();
                            txtSupplierSearch.SelectAll();
                        }));
                        return;
                    }

                    if (string.IsNullOrWhiteSpace(txt_supplier_invoice.Text))
                    {
                        UiMessages.ShowWarning(
                            "Supplier Invoice No. is required.",
                            "رقم فاتورة المورد مطلوب.",
                            captionEn: "Validation",
                            captionAr: "التحقق");

                        this.ActiveControl = txt_supplier_invoice;
                        txt_supplier_invoice.Focus();
                        txt_supplier_invoice.SelectAll();

                        BeginInvoke((Action)(() =>
                        {
                            this.ActiveControl = txt_supplier_invoice;
                            txt_supplier_invoice.Focus();
                            txt_supplier_invoice.SelectAll();
                        }));
                        return;
                    }

                    if (purchase_type == "Cash" && (paymentMethodText.Contains("Bank") || paymentMethodText.Contains("bank") || paymentMethodText.Contains("banks") || paymentMethodText.Contains("Banks")))
                    {
                        Master.Banks.frm_banksPopup bankfrm = new Master.Banks.frm_banksPopup();
                        bankfrm.ShowDialog();
                        string bankIDPlusGLAccountID = bankfrm._bankIDPlusGLAccountID;

                        int condition_index_len = bankIDPlusGLAccountID.IndexOf("+");
                        bankID = bankIDPlusGLAccountID.Substring(0, condition_index_len).Trim();
                        bankGLAccountID = bankIDPlusGLAccountID.Substring(condition_index_len + 1).Trim();

                    }

                    DialogResult result = UiMessages.ConfirmYesNo(
                        "Create this purchase transaction (" + purchase_type + ")?",
                        "هل تريد إنشاء عملية شراء (" + purchase_type + ")؟",
                        captionEn: "Confirm purchase",
                        captionAr: "تأكيد الشراء");

                    if (result == DialogResult.Yes)
                    {
                        if (grid_purchases.Rows.Count > 0)
                        {
                            List<PurchaseModalHeader> purchase_model_header = new List<PurchaseModalHeader> { };
                            List<PurchasesModal> purchase_model_detail = new List<PurchasesModal> { };

                            var supplierInvoiceNo = (txt_supplier_invoice.Text ?? string.Empty).Trim();
                            var purchasesObj = new PurchasesBLL();
                            
                            // For normal purchases, block duplicates in pos_purchases
                            if (purchasesObj.IsSupplierInvoiceNoExists(supplier_id, supplierInvoiceNo))
                            {
                                UiMessages.ShowWarning(
                                    "This Supplier Invoice No. already exists for the selected supplier.",
                                    "رقم فاتورة المورد موجود بالفعل لنفس المورد.",
                                    captionEn: "Duplicate Supplier Invoice",
                                    captionAr: "تكرار رقم فاتورة المورد");
                                txt_supplier_invoice.Focus();
                                txt_supplier_invoice.SelectAll();
                                return;
                            }

                            DateTime purchase_date = txt_purchase_date.Value.Date;

                            int employee_id = (cmb_employees.SelectedValue.ToString() == null ? 0 : int.Parse(cmb_employees.SelectedValue.ToString()));
                            string po_invoice_no_1 = "";
                            bool po_status = false;
                            string invoice_no = "";
                            string location_code = "";

                        //if (invoice_status == "Update" && txt_invoice_no.Text.Substring(0, 1).ToUpper() == "P") //Update sales delete all record first and insert new sales
                        //{
                        //    int qresult = purchasesObj.DeletePurchases(txt_invoice_no.Text); //DELETE ALL TRANSACTIONS
                        //    if (qresult <= 0)
                        //    {
                        //        MessageBox.Show(invoice_no + " has issue while updating, please try again", "Update", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //        return;
                        //    }
                        //    invoice_no = txt_invoice_no.Text;
                        //}
                        //else
                            if (invoice_status == "PO") //if purchase order
                            {
                                po_invoice_no_1 = po_invoice_no;
                                po_status = true;
                                invoice_no = GetMAXInvoiceNo();
                            }
                            else
                            {
                                invoice_no = GetMAXInvoiceNo();
                            }

                            //if purchase return then put minus sign before amount
                            decimal return_minus_value = (purchase_type == "Return" ? -1 : 1);
                            decimal net_total = Math.Round(return_minus_value * total_amount, 6);
                            decimal net_total_discount = Math.Round(return_minus_value * total_discount, 6);
                            decimal net_total_tax = Math.Round(return_minus_value * total_tax, 6);


                            //set the date from datetimepicker and set time to te current time
                            DateTime now = DateTime.Now;
                            txt_purchase_date.Value = new DateTime(txt_purchase_date.Value.Year, txt_purchase_date.Value.Month, txt_purchase_date.Value.Day, now.Hour, now.Minute, now.Second);
                            /////////////////////

                            /////Add sales header into the List
                            purchase_model_header.Add(new PurchaseModalHeader
                            {
                                supplier_id = supplier_id,
                                employee_id = employee_id,
                                invoice_no = invoice_no,
                                supplier_invoice_no = txt_supplier_invoice.Text,
                                total_amount = net_total,
                                total_tax = Math.Round(total_tax, 6),
                                total_discount = total_discount,
                                purchase_type = purchase_type,
                                purchase_date = purchase_date,
                                purchase_time = txt_purchase_date.Value,
                                description = txt_description.Text,
                                shipping_cost = (string.IsNullOrEmpty(txt_shipping_cost.Text) ? 0 : Convert.ToDecimal(txt_shipping_cost.Text)),
                                account = "Purchase",
                                po_invoice_no = po_invoice_no_1,
                                po_status = po_status,

                                payment_method_id = payment_method_id,
                                payment_method_text = paymentMethodText,
                                bankGLAccountID = bankGLAccountID,
                                bank_id = (string.IsNullOrEmpty(bankID) ? 0 : Convert.ToInt32(bankID)),


                                cash_account_id = cash_account_id,
                                payable_account_id = payable_account_id,
                                tax_account_id = tax_account_id,
                                purchases_discount_acc_id = purchases_discount_acc_id,
                                inventory_acc_id = inventory_acc_id,
                                purchases_acc_id = purchases_acc_id,

                            });
                            //////

                            int sno = 1;
                            for (int i = 0; i < grid_purchases.Rows.Count; i++)
                            {
                                if (grid_purchases.Rows[i].Cells["id"].Value != null && grid_purchases.Rows[i].Cells["code"].Value != null)
                                {

                                    if (grid_purchases.Rows[i].Cells["location_code"].Value == null || grid_purchases.Rows[i].Cells["location_code"].Value == DBNull.Value || String.IsNullOrEmpty(grid_purchases.Rows[i].Cells["location_code"].Value as String) || String.IsNullOrWhiteSpace(grid_purchases.Rows[i].Cells["location_code"].Value.ToString()))
                                    {
                                        location_code = "";
                                    }
                                    else
                                    {
                                        location_code = grid_purchases.Rows[i].Cells["location_code"].Value.ToString();
                                    }

                                    ///// Added sales detail in to List
                                    decimal tax_rate = (grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString() == "" ? 0 : decimal.Parse(grid_purchases.Rows[i].Cells["tax_rate"].Value.ToString()));

                                    purchase_model_detail.Add(new PurchasesModal
                                    {
                                        serialNo = sno++,
                                        invoice_no = invoice_no,
                                        item_id = Convert.ToInt32(grid_purchases.Rows[i].Cells["id"].Value.ToString()),
                                        code = grid_purchases.Rows[i].Cells["code"].Value.ToString(),
                                        item_number = grid_purchases.Rows[i].Cells["item_number"].Value.ToString(),
                                        quantity = return_minus_value * (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["qty"].Value.ToString()) ? 0 : decimal.Parse(grid_purchases.Rows[i].Cells["qty"].Value.ToString())),
                                        cost_price = return_minus_value * (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["avg_cost"].Value.ToString()) ? 0 : Math.Round(Convert.ToDecimal(grid_purchases.Rows[i].Cells["avg_cost"].Value.ToString()), 4)),
                                        unit_price = return_minus_value * (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["unit_price"].Value.ToString()) ? 0 : Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["unit_price"].Value.ToString()), 4)),
                                        discount = return_minus_value * (string.IsNullOrEmpty(grid_purchases.Rows[i].Cells["discount"].Value.ToString()) ? 0 : Math.Round(decimal.Parse(grid_purchases.Rows[i].Cells["discount"].Value.ToString()), 4)),
                                        tax_id = Convert.ToInt32(grid_purchases.Rows[i].Cells["tax_id"].Value.ToString()),
                                        tax_rate = tax_rate,
                                        purchase_date = purchase_date,
                                        location_code = location_code,
                                        supplier_id = supplier_id,

                                    });
                                }

                            }

                            var purchase_id = purchasesObj.Insertpurchases(purchase_model_header, purchase_model_detail);

                            if (purchase_id > 0)
                            {
                                UiMessages.ShowInfo(
                                    "Purchase saved successfully. Invoice: " + invoice_no,
                                    "تم حفظ عملية الشراء بنجاح. رقم الفاتورة: " + invoice_no,
                                    captionEn: "Success",
                                    captionAr: "نجاح");

                                clear_form();
                                GetMAXInvoiceNo();
                                grid_purchases.Focus();
                            }
                            else
                            {
                                UiMessages.ShowError(
                                    "Purchase was not saved. Please try again.",
                                    "لم يتم حفظ عملية الشراء. يرجى المحاولة مرة أخرى.",
                                    captionEn: "Error",
                                    captionAr: "خطأ");
                            }
                        }
                        else
                        {
                            UiMessages.ShowWarning(
                                "Please add at least one product.",
                                "يرجى إضافة صنف واحد على الأقل.",
                                captionEn: "Purchases",
                                captionAr: "المشتريات");
                        }
                    }
                }
                catch (Exception ex)
                {
                    UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
                }

            }
        }

        private void HistoryToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                //string item_number = "";

                //if (grid_purchases.Rows.Count > 0)
                //{
                //    item_number = grid_purchases.CurrentRow.Cells["item_number"].Value.ToString();
                //}

                //frm_productsMovements frm_prod_move_obj = new frm_productsMovements(item_number);
                //frm_prod_move_obj.ShowDialog();

                if (grid_purchases.RowCount > 0 && grid_purchases.CurrentRow.Cells["item_number"].Value != null)
                {
                    if (!string.IsNullOrEmpty(grid_purchases.CurrentRow.Cells["item_number"].Value.ToString()))
                    {
                        string item_number = grid_purchases.CurrentRow.Cells["item_number"].Value.ToString();

                        frm_product_full_detail obj = new frm_product_full_detail(null, this, item_number, null, null, "", true);

                        obj.ShowDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void SearchToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                frm_search_p_invoices frm = new frm_search_p_invoices(this);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void LoadPOToolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                frm_search_porder obj = new frm_search_porder(this);
                obj.ShowDialog();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private static readonly HashSet<string> _numericColumns =
            new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Qty", "avg_cost", "unit_price", "discount" };

        private void grid_purchases_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            string colName = grid_purchases.Columns[e.ColumnIndex].Name;
            if (!_numericColumns.Contains(colName)) return;

            if (string.IsNullOrEmpty(e.FormattedValue.ToString()))
            {
                e.Cancel = true;
                grid_purchases.Rows[e.RowIndex].ErrorText = "Value cannot be null or empty";
            }
            else if (!decimal.TryParse(e.FormattedValue.ToString(), out _))
            {
                e.Cancel = true;
                grid_purchases.Rows[e.RowIndex].ErrorText = "Value must be a numeric value";
            }
            else
            {
                grid_purchases.Rows[e.RowIndex].ErrorText = string.Empty;
            }
        }

        private void PrinttoolStripButton_Click(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txt_invoice_no.Text))
            {
                using (frm_purchase_invoice obj = new frm_purchase_invoice(Load_purchase_receipt(txt_invoice_no.Text), true))
                {
                    //obj.load_print(); // send print direct to printer without showing dialog
                    obj.ShowDialog();
                }
            }
        }
        public DataTable Load_purchase_receipt(string invoice_no)
        {
            //bind data in data grid view  
            PurchasesBLL purchasesBLL= new PurchasesBLL();
            DataTable dt = purchasesBLL.PurchaseReceipt(invoice_no);
            return dt;

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

            cmb_employees.SelectedIndex = 0;

        }


        private void SetupSuppliersDataGridView()
        {
            if (suppliersDataGridView != null) return;

            suppliersDataGridView = new DataGridView();
            suppliersDataGridView.ColumnCount = 6;

            bool isRtl = RightToLeft == RightToLeft.Yes || lang == "ar-SA";

            suppliersDataGridView.Size = new Size(520, 240);
            suppliersDataGridView.BorderStyle = BorderStyle.None;
            suppliersDataGridView.BackgroundColor = Color.White;
            suppliersDataGridView.AutoGenerateColumns = false;
            suppliersDataGridView.ReadOnly = true;
            suppliersDataGridView.AllowUserToAddRows = false;
            suppliersDataGridView.AllowUserToDeleteRows = false;
            suppliersDataGridView.AllowUserToResizeRows = false;
            suppliersDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            suppliersDataGridView.MultiSelect = false;
            suppliersDataGridView.RowHeadersVisible = false;
            suppliersDataGridView.RightToLeft = isRtl ? RightToLeft.Yes : RightToLeft.No;

            suppliersDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            suppliersDataGridView.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            suppliersDataGridView.Columns[0].Name = "Code";
            suppliersDataGridView.Columns[1].Name = "Name";
            suppliersDataGridView.Columns[2].Name = "ID";
            suppliersDataGridView.Columns[3].Name = "Contact";
            suppliersDataGridView.Columns[4].Name = "VAT No";
            suppliersDataGridView.Columns[5].Name = "VatStatus";

            suppliersDataGridView.Columns[2].Visible = false;
            suppliersDataGridView.Columns[5].Visible = false;

            suppliersDataGridView.Columns[0].Width = 90;
            suppliersDataGridView.Columns[1].Width = 220;
            suppliersDataGridView.Columns[3].Width = 130;
            suppliersDataGridView.Columns[4].Width = 120;

            suppliersDataGridView.CellClick += suppliersDataGridView_CellClick;
            suppliersDataGridView.KeyDown += suppliersDataGridView_KeyDown;

            suppliersDataGridView.Visible = false;
            // Always add to the form so GroupBox clipping doesn't hide it
            this.Controls.Add(suppliersDataGridView);
            suppliersDataGridView.BringToFront();
        }

        // Positions a popup dropdown grid directly below its anchor control.
        // Converts through PointToScreen → PointToClient so nested panels/groupboxes and
        // RTL mirroring are both handled automatically by WinForms (no manual RTL branch needed).
        private void PositionDropdownGrid(DataGridView dgv, Control anchor)
        {
            Point pt = this.PointToClient(anchor.Parent.PointToScreen(anchor.Location));
            int x = Math.Max(0, Math.Min(pt.X, this.ClientSize.Width - dgv.Width));
            dgv.Location = new Point(x, pt.Y + anchor.Height + 2);
        }

        // Recalculates the supplier dropdown position every time it is shown.
        private void PositionSuppliersDropdown()
        {
            Point pt = this.PointToClient(
                txtSupplierSearch.Parent.PointToScreen(txtSupplierSearch.Location));
            int x = Math.Max(0, Math.Min(pt.X, this.ClientSize.Width - suppliersDataGridView.Width));
            suppliersDataGridView.Location = new Point(x, pt.Y + txtSupplierSearch.Height + 2);
        }

        private void RefreshSuppliersData()
        {
            try
            {
                var supplierSearch = txtSupplierSearch.Text ?? string.Empty;

                var bll = new SupplierBLL();
                var normalizedSearch = bll.NormalizeSupplierCodeInput(supplierSearch);

                if (_suppressSupplierSearch || !txtSupplierSearch.Focused)
                {
                    suppliersDataGridView.Visible = false;
                    return;
                }

                if (!string.IsNullOrWhiteSpace(normalizedSearch))
                {
                    DataTable dt = bll.SearchRecord(normalizedSearch) ?? new DataTable();
                    suppliersDataGridView.Rows.Clear();

                    if (dt.Rows.Count > 0)
                    {
                        foreach (DataRow dr in dt.Rows)
                        {
                            string[] row0 = {
                                dt.Columns.Contains("supplier_code") ? Convert.ToString(dr["supplier_code"]) : "",
                                (Convert.ToString(dr["first_name"]) + " " + Convert.ToString(dr["last_name"]))
                                    .Trim(),
                                Convert.ToString(dr["id"]),
                                dt.Columns.Contains("contact_no") ? Convert.ToString(dr["contact_no"]) : "",
                                Convert.ToString(dr["vat_no"]),
                                dt.Columns.Contains("vat_status") ? Convert.ToString(dr["vat_status"]) : ""
                            };
                            suppliersDataGridView.Rows.Add(row0);
                        }

                        PositionSuppliersDropdown();
                        suppliersDataGridView.Visible = true;
                        suppliersDataGridView.BringToFront();
                        suppliersDataGridView.ClearSelection();
                        suppliersDataGridView.CurrentCell = null;
                        if (suppliersDataGridView.Rows.Count > 0)
                            suppliersDataGridView.CurrentCell = suppliersDataGridView.Rows[0].Cells[0];
                    }
                    else
                    {
                        suppliersDataGridView.Visible = false;
                    }
                }
                else
                {
                    txtSupplierSearch.Text = "";
                    ClearSelectedSupplier();
                    suppliersDataGridView.Visible = false;
                }
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(ex.Message, "خطأ", "Error", "خطأ");
            }
        }

        private void SelectSupplierFromDataRow(DataRow dr)
        {
            _selectedSupplierId = Convert.ToInt32(dr["id"]);
            txtSupplierSearch.Text = (Convert.ToString(dr["first_name"]) + " " + Convert.ToString(dr["last_name"]))
                .Trim();
            txt_supplier_vat.Text = Convert.ToString(dr["vat_no"]);

            bool vat_with_status = bool.TryParse(Convert.ToString(dr["vat_status"]), out var vat) && vat;
            if (vat_with_status)
                rd_btn_with_vat.Checked = true;
            else
                rd_btn_without_vat.Checked = true;

            var bll = new SupplierBLL();
            DataTable supplier_total_balance = bll.GetSupplierAccountBalance(_selectedSupplierId);
            if (supplier_total_balance != null && supplier_total_balance.Rows.Count > 0)
                txt_cust_balance.Text = Convert.ToString(supplier_total_balance.Rows[0]["balance"]);
            else
                txt_cust_balance.Text = "";
        }

        private void ClearSelectedSupplier()
        {
            _selectedSupplierId = 0;
            txt_supplier_vat.Text = "";
            txt_cust_balance.Text = "";
        }

        private void SupplierSearchDebounceTimer_Tick(object sender, EventArgs e)
        {
            _supplierSearchDebounceTimer.Stop();
            if (_suppressSupplierSearch || !txtSupplierSearch.Focused) return;
            RefreshSuppliersData();
        }

        private void txtSupplierSearch_TextChanged(object sender, EventArgs e)
        {
            if (_suppressSupplierSearch || !txtSupplierSearch.Focused) return;
            _supplierSearchDebounceTimer.Stop();
            _supplierSearchDebounceTimer.Start();
        }

        private void txtSupplierSearch_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (suppliersDataGridView.Visible && suppliersDataGridView.Rows.Count > 0)
                {
                    suppliersDataGridView.Focus();
                    if (suppliersDataGridView.CurrentRow == null)
                        suppliersDataGridView.CurrentCell = suppliersDataGridView.Rows[0].Cells[0];
                }
                return;
            }

            if (e.KeyCode == Keys.Escape)
            {
                suppliersDataGridView.Visible = false;
                return;
            }

            if (e.KeyCode == Keys.Enter)
            {
                var bll = new SupplierBLL();
                var normalizedSearch = bll.NormalizeSupplierCodeInput(txtSupplierSearch.Text);

                if (!string.IsNullOrWhiteSpace(normalizedSearch) && normalizedSearch.StartsWith("S-", StringComparison.OrdinalIgnoreCase))
                {
                    try
                    {
                        var dt = bll.SearchRecord(normalizedSearch);
                        if (dt != null && dt.Rows.Count > 0 && dt.Columns.Contains("supplier_code"))
                        {
                            var exact = dt.Select("supplier_code = '" + normalizedSearch.Replace("'", "''") + "'");
                            if (exact.Length == 1)
                            {
                                SelectSupplierFromDataRow(exact[0]);
                                suppliersDataGridView.Visible = false;
                                grid_purchases.Focus();
                                return;
                            }
                        }

                        // No exact match => clear fields
                        ClearSelectedSupplier();
                    }
                    catch
                    {
                    }
                }

                if (suppliersDataGridView.Visible && suppliersDataGridView.Rows.Count > 0)
                {
                    var idText = Convert.ToString(suppliersDataGridView.CurrentRow.Cells[2].Value);
                    int supId;
                    if (int.TryParse(idText, out supId) && supId > 0)
                    {
                        var dt = bll.SearchRecordBySupplierID(supId);
                        if (dt != null && dt.Rows.Count > 0)
                            SelectSupplierFromDataRow(dt.Rows[0]);
                        else
                            ClearSelectedSupplier();
                    }
                    suppliersDataGridView.Visible = false;
                    grid_purchases.Focus();
                }
            }

            _supplierSearchDebounceTimer.Stop();
            _supplierSearchDebounceTimer.Start();
        }

        private void txtSupplierSearch_Leave(object sender, EventArgs e)
        {
            _supplierSearchDebounceTimer.Stop();
            if (!suppliersDataGridView.Focused)
                suppliersDataGridView.Visible = false;
        }

        private void suppliersDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                var idText = Convert.ToString(suppliersDataGridView.CurrentRow.Cells[2].Value);
                int supId;
                if (int.TryParse(idText, out supId) && supId > 0)
                {
                    var bll = new SupplierBLL();
                    var dt = bll.SearchRecordBySupplierID(supId);
                    if (dt != null && dt.Rows.Count > 0)
                        SelectSupplierFromDataRow(dt.Rows[0]);
                }
                suppliersDataGridView.Visible = false;
                grid_purchases.Focus();
            }
            else if (e.KeyCode == Keys.Escape)
            {
                suppliersDataGridView.Visible = false;
                txtSupplierSearch.Focus();
            }
        }

        private void suppliersDataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var idText = Convert.ToString(suppliersDataGridView.CurrentRow.Cells[2].Value);
            int supId;
            if (int.TryParse(idText, out supId) && supId > 0)
            {
                var bll = new SupplierBLL();
                var dt = bll.SearchRecordBySupplierID(supId);
                if (dt != null && dt.Rows.Count > 0)
                    SelectSupplierFromDataRow(dt.Rows[0]);
            }

            suppliersDataGridView.Visible = false;
            grid_purchases.Focus();
        }


        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts frm_search_product_obj = new frm_searchProducts();
            frm_search_product_obj.Show();

        }

        // Kept for designer compatibility. Supplier selection is handled via `txtSupplierSearch`.
        private void cmb_suppliers_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

    }
}
