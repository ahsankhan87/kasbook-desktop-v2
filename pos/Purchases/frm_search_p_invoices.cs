using POS.BLL;
using POS.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using pos.UI;
using pos.UI.Busy;

namespace pos
{
    public partial class frm_search_p_invoices : Form
    {
        frm_purchases mainForm;

        public frm_search_p_invoices(frm_purchases mainForm)
        {
            this.mainForm = mainForm;
            InitializeComponent();
        }
        
        public frm_search_p_invoices()
        {
            InitializeComponent();
            
        }

        private void search_p_invoices_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            StyleSearchForm();

            Listbox_method.SelectedIndex = 3;
            txt_condition.Focus();
        }

        // ── Uniform app styling ───────────────────────────────────────────────
        private void StyleSearchForm()
        {
            // ── Form ─────────────────────────────────────────────────────────
            this.BackColor = SystemColors.Control;
            this.Font      = AppTheme.FontDefault;

            // ── Filter panel (panel1) ─────────────────────────────────────────
            panel1.BackColor = SystemColors.Control;
            panel1.Padding   = new Padding(8, 6, 8, 6);

            // ── Results panel (panel2) ────────────────────────────────────────
            panel2.BackColor = SystemColors.Control;
            panel2.Padding   = new Padding(6, 4, 6, 6);

            // ── GroupBoxes ────────────────────────────────────────────────────
            foreach (GroupBox grp in new[] { groupBox1, groupBox2 })
            {
                grp.BackColor = SystemColors.Control;
                grp.ForeColor = SystemColors.ControlText;
                grp.Font      = AppTheme.FontGroupBox;
                grp.Padding   = new Padding(6, 10, 6, 6);
            }

            // ── Search-method ListBox ─────────────────────────────────────────
            Listbox_method.BackColor   = SystemColors.Window;
            Listbox_method.ForeColor   = AppTheme.TextPrimary;
            Listbox_method.Font        = new Font("Segoe UI", 10F, FontStyle.Regular);
            Listbox_method.BorderStyle = BorderStyle.FixedSingle;

            // DrawMode must be set BEFORE ItemHeight: in Normal mode ItemHeight
            // is read-only (auto-sized from font) so the assignment is silently
            // ignored, causing the subsequent Height calculation to use the wrong value.
            Listbox_method.DrawMode = DrawMode.OwnerDrawFixed;
            Listbox_method.DrawItem -= Listbox_method_DrawItem;
            Listbox_method.DrawItem += Listbox_method_DrawItem;

            Listbox_method.ItemHeight     = 28;
            Listbox_method.IntegralHeight = false;
            Listbox_method.Height         = Listbox_method.ItemHeight * Listbox_method.Items.Count + 4;

            // ── Search condition inputs ───────────────────────────────────────
            txt_condition.Font        = AppTheme.FontDefault;
            txt_condition.BackColor   = SystemColors.Window;
            txt_condition.BorderStyle = BorderStyle.FixedSingle;
            txt_condition.Height      = AppTheme.InputHeight;

            txt_from_date.Font      = AppTheme.FontDefault;
            txt_from_date.CalendarFont = AppTheme.FontDefault;

            txt_to_date.Font        = AppTheme.FontDefault;
            txt_to_date.CalendarFont = AppTheme.FontDefault;

            // ── Labels ────────────────────────────────────────────────────────
            foreach (Label lbl in new[] { label1, label3, label5 })
            {
                lbl.Font      = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular);
                lbl.ForeColor = AppTheme.TextSecondary;
            }

            // ── Hold-purchases checkbox ───────────────────────────────────────
            chk_hold_purchases.Font      = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular);
            chk_hold_purchases.ForeColor = SystemColors.ControlText;

            // ── Buttons ───────────────────────────────────────────────────────
            StyleActionButton(btn_search,  AppTheme.Primary,  AppTheme.PrimaryDark);
            StyleActionButton(btn_ok,      AppTheme.Accent,   AppTheme.PrimaryDark);
            StyleActionButton(btn_close,   AppTheme.Danger,   AppTheme.DangerDark);

            // ── Results grid ─────────────────────────────────────────────────
            typeof(DataGridView).InvokeMember("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic |
                System.Reflection.BindingFlags.Instance  |
                System.Reflection.BindingFlags.SetProperty,
                null, grid_sales_report, new object[] { true });

            grid_sales_report.BackgroundColor     = SystemColors.AppWorkspace;
            grid_sales_report.BorderStyle         = BorderStyle.None;
            grid_sales_report.CellBorderStyle     = DataGridViewCellBorderStyle.SingleHorizontal;
            grid_sales_report.GridColor           = SystemColors.ControlLight;
            grid_sales_report.RowHeadersVisible   = false;
            grid_sales_report.EnableHeadersVisualStyles = false;

            grid_sales_report.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            grid_sales_report.ColumnHeadersHeight         = 36;
            grid_sales_report.RowTemplate.Height          = 32;

            grid_sales_report.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Control,
                ForeColor          = SystemColors.ControlText,
                Font               = AppTheme.FontGridHeader,
                SelectionBackColor = SystemColors.Control,
                SelectionForeColor = SystemColors.ControlText,
                Alignment          = DataGridViewContentAlignment.MiddleLeft,
                Padding            = new Padding(6, 4, 6, 4)
            };

            grid_sales_report.DefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = SystemColors.Window,
                ForeColor          = AppTheme.TextPrimary,
                Font               = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText,
                Padding            = new Padding(6, 2, 6, 2)
            };

            grid_sales_report.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
            {
                BackColor          = AppTheme.GridAltRow,
                ForeColor          = AppTheme.TextPrimary,
                Font               = AppTheme.FontGrid,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText
            };

            // total_amount column: right-aligned, bold accent
            total_amount.DefaultCellStyle = new DataGridViewCellStyle
            {
                Alignment          = DataGridViewContentAlignment.MiddleRight,
                Format             = "N2",
                Font               = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular),
                ForeColor          = AppTheme.Primary,
                SelectionForeColor = SystemColors.HighlightText,
                SelectionBackColor = SystemColors.Highlight
            };

            // invoice_no column: bold
            invoice_no.DefaultCellStyle = new DataGridViewCellStyle
            {
                Font               = new Font("Segoe UI Semibold", 9F, FontStyle.Regular),
                ForeColor          = AppTheme.TextPrimary,
                SelectionBackColor = SystemColors.Highlight,
                SelectionForeColor = SystemColors.HighlightText
            };

            // Reset per-column fonts from resx
            foreach (DataGridViewColumn col in grid_sales_report.Columns)
            {
                if (col.Name != "total_amount" && col.Name != "invoice_no")
                    col.DefaultCellStyle.Font = null;
            }
        }

        // ── Owner-draw ListBox: coloured highlight for selected item ──────────
        private void Listbox_method_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            bool selected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

            Color bg   = selected ? AppTheme.Primary      : SystemColors.Window;
            Color fore = selected ? Color.White            : AppTheme.TextPrimary;

            using (var bgBrush = new SolidBrush(bg))
                e.Graphics.FillRectangle(bgBrush, e.Bounds);

            // Left accent stripe for selected item
            if (selected)
                using (var stripe = new SolidBrush(AppTheme.PrimaryDark))
                    e.Graphics.FillRectangle(stripe, e.Bounds.X, e.Bounds.Y, 4, e.Bounds.Height);

            string text = Listbox_method.Items[e.Index].ToString();
            TextRenderer.DrawText(e.Graphics, text,
                new Font("Segoe UI", 10F, selected ? FontStyle.Bold : FontStyle.Regular),
                new Rectangle(e.Bounds.X + 10, e.Bounds.Y, e.Bounds.Width - 10, e.Bounds.Height),
                fore,
                TextFormatFlags.Left | TextFormatFlags.VerticalCenter);
        }

        // ── Button helper ─────────────────────────────────────────────────────
        private static void StyleActionButton(Button btn, Color baseColor, Color darkColor)
        {
            btn.FlatStyle  = FlatStyle.Flat;
            btn.BackColor  = baseColor;
            btn.ForeColor  = Color.White;
            btn.Font       = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular);
            btn.Cursor     = Cursors.Hand;
            btn.Height     = 34;
            btn.FlatAppearance.BorderSize        = 0;
            btn.FlatAppearance.MouseOverBackColor =
                Color.FromArgb(Math.Min(255, baseColor.R + 30),
                               Math.Min(255, baseColor.G + 30),
                               Math.Min(255, baseColor.B + 30));
            btn.FlatAppearance.MouseDownBackColor = darkColor;
        }

        private void btn_search_Click(object sender, EventArgs e)
        {
            try
            {
                string from_date = "";
                string to_date = "";
                string supplier = "";
                string supplier_inv_no = "";
                string invoice_no = "";
                double total_amount = 0;
                int branch_id = UsersModal.logged_in_branch_id;

                if (Listbox_method.SelectedIndex == 0)
                {
                    invoice_no = txt_condition.Text;
                }

                if (Listbox_method.SelectedIndex == 1)
                {
                    supplier = txt_condition.Text;
                }

                if (Listbox_method.SelectedIndex == 2)
                {
                    supplier_inv_no = txt_condition.Text;
                } 
                
                if (Listbox_method.SelectedIndex == 3)
                {
                    from_date = txt_from_date.Value.Date.ToString("yyyy-MM-dd");
                    to_date = txt_to_date.Value.Date.ToString("yyyy-MM-dd");
                }

                if (Listbox_method.SelectedIndex == 4)
                {
                    total_amount = (string.IsNullOrEmpty(txt_condition.Text) ? 0 : double.Parse(txt_condition.Text));
                }

                grid_sales_report.AutoGenerateColumns = false;

                if (string.IsNullOrEmpty(invoice_no) && string.IsNullOrEmpty(supplier) && string.IsNullOrEmpty(supplier_inv_no)
                    && total_amount == 0 && string.IsNullOrEmpty(from_date) && string.IsNullOrEmpty(to_date))
                {
                    UiMessages.ShowWarning(
                        "Please enter a search value or select a date range.",
                        "يرجى إدخال قيمة للبحث أو تحديد نطاق تاريخ.");
                    txt_condition.Focus();
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Searching purchase invoices...", "جارٍ البحث عن فواتير المشتريات...")))
                {
                    PurchasesReportBLL report_obj = new PurchasesReportBLL();

                    DataTable accounts_dt;
                    if (chk_hold_purchases.Checked)
                    {
                        accounts_dt = report_obj.Hold_PurchaseInvoiceReport(from_date, to_date, supplier, supplier_inv_no, invoice_no, total_amount, branch_id);
                    }
                    else
                    {
                        accounts_dt = report_obj.PurchaseInvoiceReport(from_date, to_date, supplier, supplier_inv_no, invoice_no, total_amount, branch_id);
                    }

                    grid_sales_report.DataSource = accounts_dt;

                    if (accounts_dt == null || accounts_dt.Rows.Count == 0)
                    {
                        UiMessages.ShowInfo(
                            "No invoices matched your search criteria.",
                            "لا توجد فواتير مطابقة لمعايير البحث.");
                    }
                }

                this.ActiveControl = grid_sales_report;
            }
            catch (FormatException)
            {
                UiMessages.ShowWarning(
                    "Please enter a valid amount.",
                    "يرجى إدخال مبلغ صحيح.");
                txt_condition.Focus();
            }
            catch (Exception ex)
            {
                UiMessages.ShowError(
                    "An unexpected error occurred while searching purchase invoices. Please try again.",
                    "حدث خطأ غير متوقع أثناء البحث عن فواتير المشتريات. يرجى المحاولة مرة أخرى.");
            }
        }


        private void btn_search_products_Click(object sender, EventArgs e)
        {
            frm_searchProducts obj = new frm_searchProducts();
            obj.ShowDialog();
        }

        private void frm_search_p_invoices_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //when you enter in textbox it will goto next textbox, work like TAB key
                if (e.KeyData == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

                if (e.KeyCode == Keys.F3)
                {
                    btn_ok.PerformClick();
                }
                if (e.KeyCode == Keys.Escape)
                {
                    btn_close.PerformClick();
                }
                if ((e.KeyCode == Keys.Return) && (e.Modifiers == Keys.Control))
                {
                    btn_ok.PerformClick();
                }
                if (e.KeyCode == Keys.F2 && (e.Modifiers == Keys.Control))
                {
                    txt_condition.Focus();
                }
                if (e.KeyData == Keys.Down)
                {
                    if (this.ActiveControl != grid_sales_report)
                    {
                        this.ActiveControl = grid_sales_report;
                    }
                }
                if(e.KeyData == Keys.F4)
                {
                    chk_hold_purchases.Checked = !chk_hold_purchases.Checked;
                }
                if( e.KeyData == Keys.F5)
                {
                    Listbox_method.Focus();
                }
            }
            catch (Exception)
            {
                UiMessages.ShowWarning(
                    "Unable to process the selected shortcut key.",
                    "تعذر تنفيذ اختصار لوحة المفاتيح المحدد.");
            }
        }

        private void btn_close_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void grid_sales_report_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadPurchaseByInvoiceNo(); 
                this.Close();

            }
            
        }

        private void grid_sales_report_DoubleClick(object sender, EventArgs e)
        {
            LoadPurchaseByInvoiceNo();
            this.Close();
        }

        private void txt_from_date_ValueChanged(object sender, EventArgs e)
        {
            Listbox_method.SelectedIndex = 3;
        }

        private void txt_to_date_ValueChanged(object sender, EventArgs e)
        {
            Listbox_method.SelectedIndex = 3;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            LoadPurchaseByInvoiceNo();
            this.Close();
        }
        private void LoadPurchaseByInvoiceNo()
        {
            try
            {
                if (mainForm == null)
                {
                    UiMessages.ShowWarning(
                        "Unable to load the selected invoice because the purchase form is not available.",
                        "تعذر تحميل الفاتورة المحددة لأن شاشة المشتريات غير متاحة.");
                    return;
                }

                if (grid_sales_report.CurrentRow == null || grid_sales_report.Rows.Count == 0)
                {
                    UiMessages.ShowWarning(
                        "Please select an invoice to continue.",
                        "يرجى تحديد فاتورة للمتابعة.");
                    return;
                }

                var cell = grid_sales_report.CurrentRow.Cells["invoice_no"];
                var selectedInvoiceNo = cell != null && cell.Value != null ? cell.Value.ToString() : string.Empty;

                if (string.IsNullOrWhiteSpace(selectedInvoiceNo))
                {
                    UiMessages.ShowWarning(
                        "The selected row does not contain a valid invoice number.",
                        "الصف المحدد لا يحتوي على رقم فاتورة صالح.");
                    return;
                }

                using (BusyScope.Show(this, UiMessages.T("Loading invoice details...", "جارٍ تحميل تفاصيل الفاتورة...")))
                {
                    PurchasesBLL PurchasesObj = new PurchasesBLL();
                    DataTable _dt;

                    if (chk_hold_purchases.Checked)
                    {
                        _dt = PurchasesObj.GetAll_Hold_PurchaseByInvoice(selectedInvoiceNo);
                    }
                    else
                    {
                        _dt = PurchasesObj.GetAllPurchaseByInvoice(selectedInvoiceNo);
                    }

                    mainForm.Load_products_to_grid_by_invoiceno(_dt, selectedInvoiceNo);
                }
            }
            catch (Exception)
            {
                UiMessages.ShowError(
                    "An error occurred while loading the selected purchase invoice.",
                    "حدث خطأ أثناء تحميل فاتورة المشتريات المحددة.");
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (grid_sales_report.CurrentRow == null)
                {
                    UiMessages.ShowWarning(
                        "Please select an invoice first.",
                        "يرجى تحديد فاتورة أولاً.");
                    return;
                }

                var cell = grid_sales_report.CurrentRow.Cells["invoice_no"];
                var value = cell != null && cell.Value != null ? cell.Value.ToString() : string.Empty;

                if (string.IsNullOrWhiteSpace(value))
                {
                    UiMessages.ShowWarning(
                        "The selected invoice number is empty.",
                        "رقم الفاتورة المحدد فارغ.");
                    return;
                }

                Clipboard.SetText(value);
                UiMessages.ShowInfo(
                    "Invoice number copied to the clipboard.",
                    "تم نسخ رقم الفاتورة إلى الحافظة.");
            }
            catch (Exception)
            {
                UiMessages.ShowError(
                    "Unable to copy the invoice number to the clipboard.",
                    "تعذر نسخ رقم الفاتورة إلى الحافظة.");
            }
        }

    }
}
