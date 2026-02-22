using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace pos.UI
{
    /// <summary>
    /// Microsoft Fluent Design–inspired professional theme for the entire POS application.
    /// Designed for Saudi Arabia market — clean, modern, with full RTL / Arabic support.
    /// 
    /// Detects RTL from the form's <c>RightToLeft</c> property (set by ar-SA .resx files).
    /// Call <see cref="Apply(Control)"/> from any form's Load event.
    ///
    /// Button classification by name convention:
    ///   • "save"  / "payment"            ? Primary  (filled blue)
    ///   • "update"/ "refresh"/ "search"  ? Secondary (outline)
    ///   • "delete"                        ? Danger   (red)
    ///   • "cancel"/ "blank"/ "close"     ? Ghost    (flat neutral)
    ///   • "print" / "ledger"/ "receipt"  ? Info     (teal)
    /// </summary>
    public static class AppTheme
    {
        // ?? Microsoft Fluent colour palette ??????????????????????????
        public static readonly Color Primary          = Color.FromArgb(0, 120, 212);     // #0078D4  MS Blue
        public static readonly Color PrimaryDark      = Color.FromArgb(0, 90, 158);      // #005A9E
        public static readonly Color PrimaryDarker    = Color.FromArgb(0, 69, 120);      // #004578
        public static readonly Color PrimaryLight     = Color.FromArgb(222, 236, 249);   // #DEECF9
        public static readonly Color PrimarySubtle    = Color.FromArgb(239, 246, 252);   // #EFF6FC

        public static readonly Color Accent           = Color.FromArgb(16, 124, 16);     // #107C10  MS Green
        public static readonly Color Danger           = Color.FromArgb(209, 52, 56);     // #D13438  MS Red
        public static readonly Color DangerDark       = Color.FromArgb(168, 0, 0);       // #A80000
        public static readonly Color DangerLight      = Color.FromArgb(253, 231, 233);   // #FDE7E9
        public static readonly Color Warning          = Color.FromArgb(255, 185, 0);     // #FFB900
        public static readonly Color Info             = Color.FromArgb(0, 120, 136);     // #007888  Teal

        public static readonly Color Background       = Color.FromArgb(243, 242, 241);   // #F3F2F1  Fluent gray
        public static readonly Color Surface          = Color.White;
        public static readonly Color SurfaceAlt       = Color.FromArgb(250, 249, 248);   // #FAF9F8

        public static readonly Color TextPrimary      = Color.FromArgb(50, 49, 48);      // #323130
        public static readonly Color TextSecondary    = Color.FromArgb(96, 94, 92);      // #605E5C
        public static readonly Color TextDisabled     = Color.FromArgb(161, 159, 157);   // #A19F9D
        public static readonly Color TextOnPrimary    = Color.White;

        public static readonly Color Border           = Color.FromArgb(237, 235, 233);   // #EDEBE9
        public static readonly Color BorderStrong     = Color.FromArgb(200, 198, 196);   // #C8C6C4
        public static readonly Color BorderFocus      = Primary;
        public static readonly Color InputBackground  = Color.White;

        public static readonly Color GridHeader       = Color.FromArgb(250, 249, 248);   // #FAF9F8
        public static readonly Color GridAltRow       = Color.FromArgb(248, 248, 248);
        public static readonly Color GridSelection    = PrimaryLight;

        // ?? Typography (Segoe UI renders Arabic beautifully) ????????
        public static readonly Font FontDefault       = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font FontSmall         = new Font("Segoe UI", 8.5F, FontStyle.Regular);
        public static readonly Font FontLabel         = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static readonly Font FontSemiBold      = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular);
        public static readonly Font FontHeader        = new Font("Segoe UI Semibold", 13F, FontStyle.Regular);
        public static readonly Font FontSubHeader     = new Font("Segoe UI Semibold", 11F, FontStyle.Regular);
        public static readonly Font FontButton        = new Font("Segoe UI Semibold", 9F, FontStyle.Regular);
        public static readonly Font FontGroupBox      = new Font("Segoe UI Semibold", 9F, FontStyle.Regular);
        public static readonly Font FontTab           = new Font("Segoe UI", 9.5F, FontStyle.Regular);
        public static readonly Font FontTabSelected   = new Font("Segoe UI Semibold", 9.5F, FontStyle.Regular);
        public static readonly Font FontGridHeader    = new Font("Segoe UI Semibold", 8.75F, FontStyle.Regular);
        public static readonly Font FontGrid          = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static readonly Font FontToolStrip     = new Font("Segoe UI", 9F, FontStyle.Regular);
        public static readonly Font FontStatusBar     = new Font("Segoe UI", 8.5F, FontStyle.Regular);

        // ?? Metrics ?????????????????????????????????????????????????
        public const int ButtonRadius   = 4;
        public const int InputRadius    = 2;
        public const int ButtonHeight   = 32;
        public const int InputHeight    = 30;

        // ?? RTL helper ??????????????????????????????????????????????
        private static bool IsRtl(Control ctrl)
        {
            while (ctrl != null)
            {
                if (ctrl.RightToLeft == RightToLeft.Yes) return true;
                if (ctrl.RightToLeft == RightToLeft.No)  return false;
                ctrl = ctrl.Parent;
            }
            return false;
        }

        // ?? Main entry point ????????????????????????????????????????
        public static void Apply(Control root)
        {
            if (root == null) return;

            bool rtl = IsRtl(root);

            if (root is Form form)
            {
                form.BackColor = Background;
                form.Font = FontDefault;
                form.ForeColor = TextPrimary;
            }

            root.SuspendLayout();
            try
            {
                ApplyRecursive(root, rtl);
            }
            finally
            {
                root.ResumeLayout(true);
            }
        }

        public static void ApplyListFormStyleLightHeader(Panel headerPanel, Label headerLabel, Panel bodyPanel, DataGridView grid, params DataGridViewColumn[] hiddenColumns)
        {
            if (headerPanel != null)
            {
                headerPanel.BackColor = SurfaceAlt;
                headerPanel.ForeColor = TextPrimary;
                foreach (Control child in headerPanel.Controls)
                {
                    if (child is Label lbl && lbl != headerLabel)
                        lbl.ForeColor = TextPrimary;
                }
            }

            if (headerLabel != null)
            {
                headerLabel.Font = FontHeader;
                headerLabel.ForeColor = TextPrimary;
            }

            if (bodyPanel != null)
            {
                bodyPanel.BackColor = SystemColors.Control;

                foreach (Control child in bodyPanel.Controls)
                {
                    if (child is Label lbl && lbl != headerLabel)
                    {
                        lbl.ForeColor = TextPrimary;
                    }
                }
            }

            if (grid != null)
            {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.SetProperty,
                    null, grid, new object[] { true });

                grid.BackgroundColor = SystemColors.AppWorkspace;
                grid.RowHeadersVisible = false;
                grid.ColumnHeadersHeight = 36;
                grid.RowTemplate.Height = 30;
                grid.DefaultCellStyle.Font = FontGrid;
                grid.DefaultCellStyle.ForeColor = TextPrimary;
                grid.DefaultCellStyle.BackColor = SystemColors.Window;
                grid.ColumnHeadersDefaultCellStyle.Font = FontGridHeader;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
                grid.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
                grid.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;

                if (hiddenColumns != null)
                {
                    foreach (var column in hiddenColumns)
                    {
                        if (column != null)
                            column.Visible = false;
                    }
                }
            }
        }

        // ?? Recursive walker ????????????????????????????????????????
        private static void ApplyRecursive(Control control, bool rtl)
        {
            control.SuspendLayout();
            try
            {
                StyleControl(control, rtl);

                foreach (Control child in control.Controls)
                {
                    ApplyRecursive(child, rtl);
                }
            }
            finally
            {
                control.ResumeLayout(false);
            }
        }

        private static void StyleControl(Control ctrl, bool rtl)
        {
            if (ctrl is Panel panel)              { StylePanel(panel);            return; }
            if (ctrl is Button btn)               { StyleButton(btn);            return; }
            if (ctrl is TextBox txt)              { StyleTextBox(txt);           return; }
            if (ctrl is ComboBox cmb)             { StyleComboBox(cmb);          return; }
            if (ctrl is NumericUpDown nud)         { StyleNumericUpDown(nud);     return; }
            if (ctrl is DateTimePicker dtp)        { StyleDateTimePicker(dtp);    return; }
            if (ctrl is Label lbl)                { StyleLabel(lbl, rtl);        return; }
            if (ctrl is GroupBox grp)             { StyleGroupBox(grp);          return; }
            if (ctrl is TabControl tab)           { StyleTabControl(tab, rtl);   return; }
            if (ctrl is TabPage tp)               { tp.BackColor = Surface;      return; }
            if (ctrl is DataGridView dgv)         { StyleDataGridView(dgv, rtl); return; }
            if (ctrl is CheckBox chk)             { StyleCheckBox(chk);          return; }
            if (ctrl is RadioButton rb)           { StyleRadioButton(rb);        return; }
            if (ctrl is MenuStrip ms)             { StyleMenuStrip(ms);          return; }
            if (ctrl is StatusStrip ss)           { StyleStatusStrip(ss);        return; }
            if (ctrl is ToolStrip ts)             { StyleToolStrip(ts);          return; }
            if (ctrl is TableLayoutPanel tlp)
            {
                tlp.BackColor = SystemColors.Control;
                tlp.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
                return;
            }
            if (ctrl is FlowLayoutPanel flp)      { flp.BackColor = Color.Transparent; return; }
            if (ctrl is PictureBox pb)            { pb.BackColor = Surface;      return; }

            ctrl.Font = FontDefault;
            ctrl.ForeColor = TextPrimary;
        }

        // ???????????????????????????????????????????????????????????
        //  PANEL
        // ???????????????????????????????????????????????????????????
        private static void StylePanel(Panel panel)
        {
            string name = (panel.Name ?? "").ToLowerInvariant();

            if (name == "panel1" || name.Contains("header"))
            {
                panel.BackColor = Primary;
                panel.ForeColor = TextOnPrimary;
                panel.Padding = new Padding(12, 8, 12, 8);
                return;
            }

            if (name == "panel3" || name.Contains("footer") || name.Contains("action") || name.Contains("bottom"))
            {
                panel.BackColor = Background;
                return;
            }

            if (name.Contains("transactiontop"))
            {
                panel.BackColor = SurfaceAlt;
                panel.Padding = new Padding(8);
                return;
            }

            if (name.Contains("grid"))
            {
                panel.BackColor = Surface;
                return;
            }

            panel.BackColor = Surface;
        }

        // ???????????????????????????????????????????????????????????
        //  BUTTON — Microsoft Fluent style
        // ???????????????????????????????????????????????????????????
        private static void StyleButton(Button btn)
        {
            string name = (btn.Name ?? "").ToLowerInvariant();

            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.Cursor = Cursors.Hand;
            btn.Font = FontButton;
            btn.Height = ButtonHeight;
            btn.UseVisualStyleBackColor = false;

            if (name.Contains("save") || name.Contains("payment"))
                ApplyButtonPrimary(btn);
            else if (name.Contains("delete"))
                ApplyButtonDanger(btn);
            else if (name.Contains("cancel") || name.Contains("blank") || name.Contains("close"))
                ApplyButtonGhost(btn);
            else if (name.Contains("print") || name.Contains("ledger") || name.Contains("receipt"))
                ApplyButtonInfo(btn);
            else if (name.Contains("update") || name.Contains("refresh") || name.Contains("search") || name.Contains("trans_refresh"))
                ApplyButtonSecondary(btn);
            else
                ApplyButtonSecondary(btn);
        }

        private static void ApplyButtonPrimary(Button btn)
        {
            btn.BackColor = Primary;
            btn.ForeColor = TextOnPrimary;
            btn.FlatAppearance.BorderColor = Primary;
            btn.FlatAppearance.MouseOverBackColor = PrimaryDark;
            btn.FlatAppearance.MouseDownBackColor = PrimaryDarker;
        }

        private static void ApplyButtonSecondary(Button btn)
        {
            btn.BackColor = Surface;
            btn.ForeColor = TextPrimary;
            btn.FlatAppearance.BorderColor = BorderStrong;
            btn.FlatAppearance.MouseOverBackColor = Background;
            btn.FlatAppearance.MouseDownBackColor = Border;
        }

        private static void ApplyButtonDanger(Button btn)
        {
            btn.BackColor = Danger;
            btn.ForeColor = TextOnPrimary;
            btn.FlatAppearance.BorderColor = DangerDark;
            btn.FlatAppearance.MouseOverBackColor = DangerDark;
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(140, 0, 0);
        }

        private static void ApplyButtonGhost(Button btn)
        {
            btn.BackColor = Surface;
            btn.ForeColor = TextSecondary;
            btn.FlatAppearance.BorderColor = Border;
            btn.FlatAppearance.MouseOverBackColor = Background;
            btn.FlatAppearance.MouseDownBackColor = Border;
        }

        private static void ApplyButtonInfo(Button btn)
        {
            btn.BackColor = Info;
            btn.ForeColor = TextOnPrimary;
            btn.FlatAppearance.BorderColor = Color.FromArgb(0, 96, 110);
            btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(0, 96, 110);
            btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(0, 78, 90);
        }

        // ???????????????????????????????????????????????????????????
        //  TEXT INPUT CONTROLS
        // ???????????????????????????????????????????????????????????
        private static void StyleTextBox(TextBox txt)
        {
            txt.Font = FontDefault;
            txt.ForeColor = TextPrimary;
            txt.BackColor = txt.ReadOnly ? Background : InputBackground;
            txt.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void StyleComboBox(ComboBox cmb)
        {
            cmb.Font = FontDefault;
            cmb.ForeColor = TextPrimary;
            cmb.BackColor = InputBackground;
            cmb.FlatStyle = FlatStyle.Standard;
        }

        private static void StyleNumericUpDown(NumericUpDown nud)
        {
            nud.Font = FontDefault;
            nud.ForeColor = TextPrimary;
            nud.BackColor = InputBackground;
            nud.BorderStyle = BorderStyle.FixedSingle;
        }

        private static void StyleDateTimePicker(DateTimePicker dtp)
        {
            dtp.Font = FontDefault;
            dtp.CalendarForeColor = TextPrimary;
            dtp.CalendarMonthBackground = Surface;
            dtp.CalendarTitleBackColor = Primary;
            dtp.CalendarTitleForeColor = TextOnPrimary;
            dtp.CalendarTrailingForeColor = TextDisabled;
        }

        // ???????????????????????????????????????????????????????????
        //  LABEL
        // ???????????????????????????????????????????????????????????
        private static void StyleLabel(Label lbl, bool rtl)
        {
            string name = (lbl.Name ?? "").ToLowerInvariant();

            if (lbl.Parent is Panel parentPanel)
            {
                string pName = (parentPanel.Name ?? "").ToLowerInvariant();
                if (pName == "panel1" || pName.Contains("header"))
                {
                    lbl.ForeColor = TextOnPrimary;
                    lbl.Font = name.Contains("customer_name") || name.Contains("title")
                        ? FontHeader : FontSemiBold;
                    return;
                }
            }

            if (name.Contains("customer_name"))
            {
                lbl.ForeColor = Primary;
                lbl.Font = FontHeader;
                return;
            }

            if (name.Contains("title"))
            {
                lbl.ForeColor = PrimaryDark;
                lbl.Font = FontSubHeader;
                return;
            }

            lbl.ForeColor = TextSecondary;
            lbl.Font = FontLabel;

            if (lbl.Parent is TableLayoutPanel)
            {
                lbl.TextAlign = rtl ? ContentAlignment.MiddleRight : ContentAlignment.MiddleLeft;
            }
        }

        // ???????????????????????????????????????????????????????????
        //  CHECKBOX / RADIO
        // ???????????????????????????????????????????????????????????
        private static void StyleCheckBox(CheckBox chk)
        {
            chk.Font = FontDefault;
            chk.ForeColor = TextPrimary;
            chk.FlatStyle = FlatStyle.Standard;
        }

        private static void StyleRadioButton(RadioButton rb)
        {
            rb.Font = FontDefault;
            rb.ForeColor = TextPrimary;
            rb.FlatStyle = FlatStyle.Standard;
        }

        // ???????????????????????????????????????????????????????????
        //  GROUPBOX
        // ???????????????????????????????????????????????????????????
        private static void StyleGroupBox(GroupBox grp)
        {
            grp.Font = FontGroupBox;
            grp.ForeColor = PrimaryDark;
            grp.BackColor = Surface;
            grp.Padding = new Padding(4, 8, 4, 4);
        }

        // ???????????????????????????????????????????????????????????
        //  TAB CONTROL — Fluent pivot with bottom accent
        // ???????????????????????????????????????????????????????????
        private static void StyleTabControl(TabControl tab, bool rtl)
        {
            tab.Font = FontTab;
            tab.DrawMode = TabDrawMode.OwnerDrawFixed;
            tab.SizeMode = TabSizeMode.Fixed;
            tab.ItemSize = new Size(140, 36);
            tab.Padding = new Point(12, 6);

            if (rtl)
                tab.RightToLeftLayout = true;

            tab.DrawItem -= TabControl_DrawItem;
            tab.DrawItem += TabControl_DrawItem;
        }

        private static void TabControl_DrawItem(object sender, DrawItemEventArgs e)
        {
            var tab = (TabControl)sender;
            var page = tab.TabPages[e.Index];
            var bounds = tab.GetTabRect(e.Index);
            bool selected = (tab.SelectedIndex == e.Index);
            bool rtl = IsRtl(tab);

            using (var bgBrush = new SolidBrush(selected ? Surface : Background))
            {
                e.Graphics.FillRectangle(bgBrush, bounds);
            }

            if (selected)
            {
                using (var pen = new Pen(Primary, 3))
                {
                    e.Graphics.DrawLine(pen,
                        bounds.Left + 8, bounds.Bottom - 1,
                        bounds.Right - 8, bounds.Bottom - 1);
                }
            }

            var textColor = selected ? Primary : TextSecondary;
            var textFont  = selected ? FontTabSelected : FontTab;
            var flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter
                      | TextFormatFlags.EndEllipsis;
            if (rtl)
                flags |= TextFormatFlags.RightToLeft | TextFormatFlags.Right;

            TextRenderer.DrawText(e.Graphics, page.Text, textFont, bounds, textColor, flags);
        }

        // ???????????????????????????????????????????????????????????
        //  DATAGRIDVIEW — Excel / Dynamics 365 inspired
        // ???????????????????????????????????????????????????????????
        private static void StyleDataGridView(DataGridView dgv, bool rtl)
        {
            if (rtl)
                dgv.RightToLeft = RightToLeft.Yes;

            if (ShouldApplyGridStyle(dgv))
            {
                dgv.BorderStyle = BorderStyle.None;
                dgv.BackgroundColor = Surface;
                dgv.GridColor = Border;
                dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
                dgv.RowHeadersVisible = false;
                dgv.EnableHeadersVisualStyles = false;

                var headerAlign = rtl
                    ? DataGridViewContentAlignment.MiddleRight
                    : DataGridViewContentAlignment.MiddleLeft;

                dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = GridHeader,
                    ForeColor = TextSecondary,
                    Font = FontGridHeader,
                    Alignment = headerAlign,
                    Padding = new Padding(8, 4, 8, 4),
                    SelectionBackColor = GridHeader,
                    SelectionForeColor = TextSecondary
                };
                dgv.ColumnHeadersHeight = 38;
                dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

                dgv.DefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = Surface,
                    ForeColor = TextPrimary,
                    Font = FontGrid,
                    SelectionBackColor = GridSelection,
                    SelectionForeColor = TextPrimary,
                    Padding = new Padding(6, 3, 6, 3)
                };
                dgv.RowTemplate.Height = 34;

                dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle
                {
                    BackColor = GridAltRow,
                    ForeColor = TextPrimary,
                    Font = FontGrid,
                    SelectionBackColor = GridSelection,
                    SelectionForeColor = TextPrimary
                };
                return;
            }

            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle(dgv.ColumnHeadersDefaultCellStyle)
            {
                Font = FontGridHeader
            };

            dgv.DefaultCellStyle = new DataGridViewCellStyle(dgv.DefaultCellStyle)
            {
                Font = FontGrid
            };

            dgv.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle(dgv.AlternatingRowsDefaultCellStyle)
            {
                Font = FontGrid
            };
        }

        private static bool ShouldApplyGridStyle(Control control)
        {
            var form = control.FindForm();
            if (form == null)
                return false;

            var name = form.Name ?? string.Empty;
            return name.Equals("frm_sales", StringComparison.OrdinalIgnoreCase)
                || name.Equals("frm_purchases", StringComparison.OrdinalIgnoreCase);
        }

        // ???????????????????????????????????????????????????????????
        //  MENUSTRIP
        // ???????????????????????????????????????????????????????????
        private static void StyleMenuStrip(MenuStrip ms)
        {
            ms.BackColor = Surface;
            ms.ForeColor = TextPrimary;
            ms.Font = FontDefault;
            ms.Padding = new Padding(4, 2, 0, 2);
            ms.Renderer = _sharedRenderer;
        }

        // ???????????????????????????????????????????????????????????
        //  TOOLSTRIP
        // ???????????????????????????????????????????????????????????
        private static void StyleToolStrip(ToolStrip ts)
        {
            ts.BackColor = Surface;
            ts.ForeColor = TextPrimary;
            ts.Font = FontToolStrip;
            ts.GripStyle = ToolStripGripStyle.Hidden;
            ts.Padding = new Padding(4, 2, 4, 2);
            ts.Renderer = _sharedRenderer;
        }

        // ???????????????????????????????????????????????????????????
        //  STATUSSTRIP
        // ???????????????????????????????????????????????????????????
        private static void StyleStatusStrip(StatusStrip ss)
        {
            ss.BackColor = PrimaryDark;
            ss.ForeColor = TextOnPrimary;
            ss.Font = FontStatusBar;
            ss.SizingGrip = false;
            ss.Renderer = _sharedRenderer;

            foreach (ToolStripItem item in ss.Items)
            {
                item.ForeColor = TextOnPrimary;
                item.Font = FontStatusBar;
            }
        }

        private static readonly FluentToolStripRenderer _sharedRenderer = new FluentToolStripRenderer();

        // ???????????????????????????????????????????????????????????
        //  PUBLIC HELPERS
        // ???????????????????????????????????????????????????????????

        public static void MakePrimary(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.Cursor = Cursors.Hand;
            btn.Font = FontButton;
            ApplyButtonPrimary(btn);
        }

        public static void MakeDanger(Button btn)
        {
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 1;
            btn.Cursor = Cursors.Hand;
            btn.Font = FontButton;
            ApplyButtonDanger(btn);
        }

        public static void MakeHeader(Panel panel)
        {
            panel.BackColor = Primary;
            panel.ForeColor = TextOnPrimary;
        }

        public static void ApplyListFormStyle(Panel headerPanel, Label headerLabel, Panel bodyPanel, DataGridView grid, params DataGridViewColumn[] hiddenColumns)
        {
            if (headerPanel != null)
            {
                headerPanel.BackColor = PrimaryDark;
                headerPanel.ForeColor = TextOnPrimary;
            }

            if (headerLabel != null)
            {
                headerLabel.Font = FontHeader;
                headerLabel.ForeColor = TextOnPrimary;
            }

            if (bodyPanel != null)
            {
                bodyPanel.BackColor = SystemColors.Control;

                foreach (Control child in bodyPanel.Controls)
                {
                    if (child is Label lbl && lbl != headerLabel)
                    {
                        lbl.ForeColor = TextPrimary;
                    }
                }
            }

            if (grid != null)
            {
                typeof(DataGridView).InvokeMember("DoubleBuffered",
                    System.Reflection.BindingFlags.NonPublic |
                    System.Reflection.BindingFlags.Instance |
                    System.Reflection.BindingFlags.SetProperty,
                    null, grid, new object[] { true });

                grid.BackgroundColor = SystemColors.AppWorkspace;
                grid.RowHeadersVisible = false;
                grid.ColumnHeadersHeight = 36;
                grid.RowTemplate.Height = 30;
                grid.DefaultCellStyle.Font = FontGrid;
                grid.DefaultCellStyle.ForeColor = TextPrimary;
                grid.DefaultCellStyle.BackColor = SystemColors.Window;
                grid.ColumnHeadersDefaultCellStyle.Font = FontGridHeader;
                grid.ColumnHeadersDefaultCellStyle.ForeColor = TextPrimary;
                grid.AlternatingRowsDefaultCellStyle.BackColor = SystemColors.ControlLight;
                grid.AlternatingRowsDefaultCellStyle.ForeColor = TextPrimary;

                if (hiddenColumns != null)
                {
                    foreach (var column in hiddenColumns)
                    {
                        if (column != null)
                            column.Visible = false;
                    }
                }
            }
        }
    }

    // ???????????????????????????????????????????????????????????????
    //  FLUENT TOOLSTRIP RENDERER
    // ???????????????????????????????????????????????????????????????
    internal sealed class FluentToolStripRenderer : ToolStripProfessionalRenderer
    {
        public FluentToolStripRenderer() : base(new FluentColorTable()) { }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            if (e.Item.Owner is StatusStrip)
            {
                e.TextColor = AppTheme.TextOnPrimary;
            }
            else
            {
                e.TextColor = e.Item.Selected
                    ? AppTheme.Primary
                    : (e.Item.Enabled ? AppTheme.TextPrimary : AppTheme.TextDisabled);
            }
            base.OnRenderItemText(e);
        }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected && !e.Item.Pressed)
            {
                base.OnRenderMenuItemBackground(e);
                return;
            }

            var rc = new Rectangle(2, 0, e.Item.Width - 4, e.Item.Height);
            using (var brush = new SolidBrush(e.Item.Pressed ? AppTheme.PrimaryLight : AppTheme.PrimarySubtle))
            {
                e.Graphics.FillRectangle(brush, rc);
            }
        }

        protected override void OnRenderButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected && !e.Item.Pressed)
            {
                base.OnRenderButtonBackground(e);
                return;
            }

            var rc = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2);
            var color = e.Item.Pressed ? AppTheme.PrimaryLight : AppTheme.PrimarySubtle;
            using (var brush = new SolidBrush(color))
            {
                e.Graphics.FillRectangle(brush, rc);
            }
            using (var pen = new Pen(AppTheme.Primary))
            {
                e.Graphics.DrawRectangle(pen, rc);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is StatusStrip)
                return;

            using (var pen = new Pen(AppTheme.Border))
            {
                e.Graphics.DrawLine(pen, 0, e.AffectedBounds.Bottom - 1,
                    e.AffectedBounds.Right, e.AffectedBounds.Bottom - 1);
            }
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            using (var pen = new Pen(AppTheme.Border))
            {
                if (e.Vertical)
                {
                    int x = e.Item.Width / 2;
                    e.Graphics.DrawLine(pen, x, 4, x, e.Item.Height - 4);
                }
                else
                {
                    int y = e.Item.Height / 2;
                    e.Graphics.DrawLine(pen, 0, y, e.Item.Width, y);
                }
            }
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            if (e.ToolStrip is StatusStrip)
            {
                using (var brush = new SolidBrush(AppTheme.PrimaryDark))
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
            else
            {
                using (var brush = new SolidBrush(AppTheme.Surface))
                    e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
        }

        protected override void OnRenderDropDownButtonBackground(ToolStripItemRenderEventArgs e)
        {
            if (!e.Item.Selected && !e.Item.Pressed)
            {
                base.OnRenderDropDownButtonBackground(e);
                return;
            }

            var rc = new Rectangle(1, 1, e.Item.Width - 2, e.Item.Height - 2);
            using (var brush = new SolidBrush(e.Item.Pressed ? AppTheme.PrimaryLight : AppTheme.PrimarySubtle))
            {
                e.Graphics.FillRectangle(brush, rc);
            }
        }
    }

    // ???????????????????????????????????????????????????????????????
    //  FLUENT COLOUR TABLE
    // ???????????????????????????????????????????????????????????????
    internal sealed class FluentColorTable : ProfessionalColorTable
    {
        public override Color MenuStripGradientBegin => AppTheme.Surface;
        public override Color MenuStripGradientEnd   => AppTheme.Surface;

        public override Color MenuItemSelected              => AppTheme.PrimarySubtle;
        public override Color MenuItemSelectedGradientBegin => AppTheme.PrimarySubtle;
        public override Color MenuItemSelectedGradientEnd   => AppTheme.PrimarySubtle;
        public override Color MenuItemPressedGradientBegin  => AppTheme.PrimaryLight;
        public override Color MenuItemPressedGradientEnd    => AppTheme.PrimaryLight;
        public override Color MenuItemBorder                => Color.Transparent;

        public override Color MenuBorder                    => AppTheme.BorderStrong;
        public override Color ImageMarginGradientBegin      => AppTheme.Surface;
        public override Color ImageMarginGradientMiddle     => AppTheme.Surface;
        public override Color ImageMarginGradientEnd        => AppTheme.Surface;

        public override Color ToolStripGradientBegin  => AppTheme.Surface;
        public override Color ToolStripGradientMiddle => AppTheme.Surface;
        public override Color ToolStripGradientEnd    => AppTheme.Surface;
        public override Color ToolStripBorder         => AppTheme.Border;

        public override Color SeparatorDark  => AppTheme.Border;
        public override Color SeparatorLight => AppTheme.Surface;

        public override Color ButtonSelectedHighlight       => AppTheme.PrimarySubtle;
        public override Color ButtonSelectedHighlightBorder => AppTheme.Primary;
        public override Color ButtonSelectedGradientBegin   => AppTheme.PrimarySubtle;
        public override Color ButtonSelectedGradientEnd     => AppTheme.PrimarySubtle;
        public override Color ButtonPressedGradientBegin    => AppTheme.PrimaryLight;
        public override Color ButtonPressedGradientEnd      => AppTheme.PrimaryLight;
        public override Color ButtonSelectedBorder          => AppTheme.Primary;
        public override Color ButtonPressedBorder           => AppTheme.PrimaryDark;
        public override Color ButtonCheckedHighlight        => AppTheme.PrimaryLight;
        public override Color ButtonCheckedHighlightBorder  => AppTheme.Primary;
        public override Color ButtonCheckedGradientBegin    => AppTheme.PrimaryLight;
        public override Color ButtonCheckedGradientEnd      => AppTheme.PrimaryLight;

        public override Color StatusStripGradientBegin => AppTheme.PrimaryDark;
        public override Color StatusStripGradientEnd   => AppTheme.PrimaryDark;

        public override Color OverflowButtonGradientBegin  => AppTheme.Surface;
        public override Color OverflowButtonGradientMiddle => AppTheme.Background;
        public override Color OverflowButtonGradientEnd    => AppTheme.Background;

        public override Color CheckBackground         => AppTheme.PrimaryLight;
        public override Color CheckSelectedBackground => AppTheme.PrimaryLight;
        public override Color CheckPressedBackground  => AppTheme.Primary;
    }
}
