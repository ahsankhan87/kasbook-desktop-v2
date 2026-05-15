using System;
using System.Windows.Forms;
using POS.Core;

namespace pos.Sales.Helpers
{
    /// <summary>
    /// Helper class for sales form styling operations.
    /// Extracts styling logic from frm_sales to improve maintainability.
    /// </summary>
    public static class SalesStylingHelper
    {
        private static readonly Font SalesGridFont = new Font(AppTheme.FontGrid.FontFamily, AppTheme.FontGrid.Size + 1f, AppTheme.FontGrid.Style);
        private static readonly Font SalesGridHeaderFont = new Font(AppTheme.FontGridHeader.FontFamily, AppTheme.FontGridHeader.Size + 1f, AppTheme.FontGridHeader.Style);
        private static readonly Font TaxableCheckFont = new Font(AppTheme.FontSemiBold.FontFamily, AppTheme.FontSemiBold.Size + 1f, AppTheme.FontSemiBold.Style);
        private static readonly Font TotalPrimaryFont = new Font(AppTheme.FontHeader.FontFamily, AppTheme.FontHeader.Size + 2f, AppTheme.FontHeader.Style);
        private static readonly Font TotalSecondaryFont = new Font(AppTheme.FontSubHeader.FontFamily, AppTheme.FontSubHeader.Size + 1f, AppTheme.FontSubHeader.Style);
        private static readonly Font SecondaryFieldFont = new Font(AppTheme.FontSemiBold.FontFamily, AppTheme.FontSemiBold.Size + 1f, AppTheme.FontSemiBold.Style);
        private static readonly Font FooterPrimaryLabelFont = new Font(AppTheme.FontSemiBold.FontFamily, AppTheme.FontSemiBold.Size + 1f, AppTheme.FontSemiBold.Style);
        private static readonly Font FooterSecondaryLabelFont = new Font(AppTheme.FontLabel.FontFamily, AppTheme.FontLabel.Size + 1f, AppTheme.FontLabel.Style);

        /// <summary>
        /// Applies professional theme styling to the sales form.
        /// Gives the sales page a Dynamics 365–like professional look.
        /// </summary>
        public static void StyleSalesForm(Form form, 
            Panel panel_header, Panel panel_footer, Panel panel_grid,
            Label lbl_title, ToolStrip SalesToolStrip, DataGridView grid_sales,
            GroupBox groupBox2, GroupBox groupBox5, GroupBox groupBox6,
            TextBox txt_total_amount, TextBox txt_sub_total, TextBox txt_sub_total_2,
            TextBox txt_total_tax, TextBox txt_total_discount, TextBox txt_total_qty,
            TextBox txt_change_amount, TextBox txt_amount_received,
            TextBox txt_cost_price, TextBox txt_cost_price_with_vat,
            TextBox txt_single_cost_evat, TextBox txt_total_cost,
            TextBox txt_shop_qty, TextBox txt_company_qty, TextBox txt_order_qty,
            CheckBox chkbox_is_taxable,
            TableLayoutPanel tableLayoutPanel5, TableLayoutPanel tableLayoutPanel6,
            TableLayoutPanel tableLayoutPanel7, TableLayoutPanel tableLayoutPanel8,
            DataGridView customersDataGridView = null)
        {
            ApplySalesLabelForeColor(form, Color.Black);

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
                    grp.ForeColor = Color.Black;
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
            lbl_title.ForeColor = Color.Black;

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
            grid_sales.DefaultCellStyle.Font = SalesGridFont;
            grid_sales.ColumnHeadersDefaultCellStyle.Font = SalesGridHeaderFont;

            // Serial number column: muted, centered
            var snoColumn = grid_sales.Columns["sno"];
            if (snoColumn != null)
            {
                snoColumn.DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = SystemColors.Control,
                    ForeColor = SystemColors.GrayText,
                    SelectionBackColor = SystemColors.Control,
                    SelectionForeColor = SystemColors.GrayText,
                    Alignment = DataGridViewContentAlignment.MiddleCenter,
                    Font = AppTheme.FontSmall
                };
            }

            // Code column
            var codeColumn = grid_sales.Columns["code"];
            if (codeColumn != null)
            {
                codeColumn.DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = SystemColors.Window,
                    SelectionBackColor = SystemColors.Highlight,
                    SelectionForeColor = SystemColors.HighlightText
                };
            }

            // Delete button column styling
            var btnDeleteColumn = grid_sales.Columns["btn_delete"];
            if (btnDeleteColumn != null)
            {
                btnDeleteColumn.DefaultCellStyle = new DataGridViewCellStyle
                {
                    ForeColor = AppTheme.Danger,
                    SelectionForeColor = AppTheme.Danger,
                    Alignment = DataGridViewContentAlignment.MiddleCenter
                };
                btnDeleteColumn.FlatStyle = FlatStyle.Flat;
            }

            // ── Footer: professional totals area ──────────────────
            // Style TableLayoutPanels as white card sections on gray footer
            StyleFooterCard(tableLayoutPanel5);
            StyleFooterCard(tableLayoutPanel6);
            StyleFooterCard(tableLayoutPanel7);
            StyleFooterCard(tableLayoutPanel8);

            // groupBox2 (product info)
            groupBox2.BackColor = SystemColors.Control;
            groupBox2.ForeColor = Color.Black;
            groupBox2.Font = AppTheme.FontGroupBox;

            groupBox5.BackColor = SystemColors.Control;
            groupBox5.ForeColor = Color.Black;
            groupBox5.Font = AppTheme.FontGroupBox;

            groupBox6.BackColor = SystemColors.Control;
            groupBox6.ForeColor = Color.Black;
            groupBox6.Font = AppTheme.FontGroupBox;

            foreach (RadioButton rb in groupBox5.Controls.OfType<RadioButton>())
            {
                rb.Font = AppTheme.FontDefault;
                rb.AutoSize = true;
                rb.Margin = new Padding(6, 4, 6, 4);
            }

            foreach (RadioButton rb in groupBox6.Controls.OfType<RadioButton>())
            {
                rb.Font = AppTheme.FontDefault;
                rb.AutoSize = true;
                rb.Margin = new Padding(6, 4, 6, 4);
            }

            // ── Main totals labels ──────────────────────────────
            var label14 = FindLabelByName(form, "label14");
            var label13 = FindLabelByName(form, "label13");
            var label9 = FindLabelByName(form, "label9");
            var label22 = FindLabelByName(form, "label22");
            var label23 = FindLabelByName(form, "label23");
            var label24 = FindLabelByName(form, "label24");
            var label25 = FindLabelByName(form, "label25");

            StyleFooterLabel(label14, false);      // Sub Total
            StyleFooterLabel(label13, false);      // Discount
            StyleFooterLabel(label9, true);        // Total Amount
            chkbox_is_taxable.Font = TaxableCheckFont;
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
        public static void StyleTotalField(TextBox txt, bool isPrimary)
        {
            txt.ReadOnly = true;
            txt.BorderStyle = BorderStyle.Fixed3D;
            txt.TextAlign = HorizontalAlignment.Right;
            if (isPrimary)
            {
                txt.Font = TotalPrimaryFont;
                txt.ForeColor = SystemColors.WindowText;
                txt.BackColor = SystemColors.Window;
            }
            else
            {
                txt.Font = TotalSecondaryFont;
                txt.ForeColor = SystemColors.WindowText;
                txt.BackColor = SystemColors.Window;
            }
        }

        /// <summary>Style a secondary footer field (received / change).</summary>
        public static void StyleSecondaryField(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.Fixed3D;
            txt.TextAlign = HorizontalAlignment.Right;
            txt.Font = SecondaryFieldFont;
            txt.ForeColor = SystemColors.WindowText;
            txt.BackColor = SystemColors.Window;
        }

        /// <summary>Style a cost-info read-only field.</summary>
        public static void StyleCostField(TextBox txt)
        {
            txt.ReadOnly = true;
            txt.BorderStyle = BorderStyle.Fixed3D;
            txt.TextAlign = HorizontalAlignment.Right;
            txt.Font = AppTheme.FontDefault;
            txt.ForeColor = SystemColors.GrayText;
            txt.BackColor = SystemColors.Control;
        }

        /// <summary>Style a popup DataGridView dropdown (brands / categories / customers).</summary>
        public static void StyleDropdownGrid(DataGridView dgv)
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
        public static void StyleFooterCard(TableLayoutPanel tlp)
        {
            tlp.BackColor = SystemColors.Control;
        }

        /// <summary>Style a footer label.</summary>
        public static void StyleFooterLabel(Label lbl, bool isPrimary)
        {
            if (lbl == null) return;
            
            if (isPrimary)
            {
                lbl.Font = FooterPrimaryLabelFont;
                lbl.ForeColor = Color.Black;
            }
            else
            {
                lbl.Font = FooterSecondaryLabelFont;
                lbl.ForeColor = Color.Black;
            }
        }

        public static void ApplySalesLabelForeColor(Control parent, Color color)
        {
            if (parent == null)
                return;

            foreach (Control child in parent.Controls)
            {
                var label = child as Label;
                if (label != null)
                    label.ForeColor = color;

                if (child.HasChildren)
                    ApplySalesLabelForeColor(child, color);
            }
        }

        private static Label FindLabelByName(Control parent, string name)
        {
            if (parent == null) return null;
            
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Name == name && ctrl is Label lbl)
                    return lbl;
                    
                if (ctrl.HasChildren)
                {
                    var found = FindLabelByName(ctrl, name);
                    if (found != null) return found;
                }
            }
            return null;
        }
    }
}
