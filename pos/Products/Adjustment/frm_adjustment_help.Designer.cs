namespace pos.Products.Adjustment
{
    partial class frm_adjustment_help
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // ── Controls ──────────────────────────────────────────────
            this.pnlHeader      = new System.Windows.Forms.Panel();
            this.lblTitle       = new System.Windows.Forms.Label();
            this.lblSubtitle    = new System.Windows.Forms.Label();
            this.pnlLeft        = new System.Windows.Forms.Panel();
            this.lstSections    = new System.Windows.Forms.ListBox();
            this.lblSectionsHdr = new System.Windows.Forms.Label();
            this.pnlRight       = new System.Windows.Forms.Panel();
            this.lblSectionTitle= new System.Windows.Forms.Label();
            this.gridShortcuts  = new System.Windows.Forms.DataGridView();
            this.colKey         = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAction      = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pnlTip         = new System.Windows.Forms.Panel();
            this.lblTip         = new System.Windows.Forms.Label();
            this.timerTips      = new System.Windows.Forms.Timer(this.components);
            this.pnlFooter      = new System.Windows.Forms.Panel();
            this.btnClose       = new System.Windows.Forms.Button();
            this.lblVersion     = new System.Windows.Forms.Label();
            this.splitter       = new System.Windows.Forms.SplitContainer();

            this.pnlHeader.SuspendLayout();
            this.pnlLeft.SuspendLayout();
            this.pnlRight.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridShortcuts)).BeginInit();
            this.pnlFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitter)).BeginInit();
            this.splitter.Panel1.SuspendLayout();
            this.splitter.Panel2.SuspendLayout();
            this.splitter.SuspendLayout();
            this.SuspendLayout();

            // ── Form ──────────────────────────────────────────────────
            this.Text            = "Stock Adjustment — Keyboard Shortcuts & Workflow";
            this.Size            = new System.Drawing.Size(900, 640);
            this.MinimumSize     = new System.Drawing.Size(720, 500);
            this.StartPosition   = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Font            = new System.Drawing.Font("Segoe UI", 9.5F);
            this.BackColor       = System.Drawing.Color.FromArgb(245, 247, 250);
            this.KeyPreview      = true;
            this.Load           += new System.EventHandler(this.frm_adjustment_help_Load);
            this.KeyDown        += (s, e) => { if (e.KeyCode == System.Windows.Forms.Keys.Escape) Close(); };

            // ── Header panel ──────────────────────────────────────────
            this.pnlHeader.Dock      = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Height    = 70;
            this.pnlHeader.BackColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.pnlHeader.Padding   = new System.Windows.Forms.Padding(16, 8, 16, 8);

            this.lblTitle.AutoSize  = false;
            this.lblTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Height    = 32;
            this.lblTitle.Text      = "⌨  Keyboard Shortcuts & Workflow Guide";
            this.lblTitle.Font      = new System.Drawing.Font("Segoe UI Semibold", 14F);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.lblSubtitle.AutoSize  = false;
            this.lblSubtitle.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.lblSubtitle.Text      = "Stock Check & Adjustment Module  •  Press Escape to close";
            this.lblSubtitle.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(200, 225, 255);
            this.lblSubtitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            this.pnlHeader.Controls.Add(this.lblSubtitle);
            this.pnlHeader.Controls.Add(this.lblTitle);

            // ── Tip panel ─────────────────────────────────────────────
            this.pnlTip.Dock      = System.Windows.Forms.DockStyle.Top;
            this.pnlTip.Height    = 36;
            this.pnlTip.BackColor = System.Drawing.Color.FromArgb(255, 249, 196);
            this.pnlTip.Padding   = new System.Windows.Forms.Padding(12, 0, 12, 0);

            this.lblTip.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.lblTip.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblTip.Font      = new System.Drawing.Font("Segoe UI", 9F);
            this.lblTip.ForeColor = System.Drawing.Color.FromArgb(90, 70, 10);

            this.pnlTip.Controls.Add(this.lblTip);

            this.timerTips.Interval = 5000;
            this.timerTips.Tick    += new System.EventHandler(this.timerTips_Tick);

            // ── Splitter ─────────────────────────────────────────────
            this.splitter.Dock            = System.Windows.Forms.DockStyle.Fill;
            this.splitter.SplitterDistance = 200;
            this.splitter.Panel1MinSize    = 160;
            this.splitter.Panel2MinSize    = 400;
            this.splitter.BorderStyle     = System.Windows.Forms.BorderStyle.None;
            this.splitter.BackColor       = System.Drawing.Color.FromArgb(220, 225, 235);

            // ── Left panel (section list) ─────────────────────────────
            this.pnlLeft.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.pnlLeft.BackColor = System.Drawing.Color.FromArgb(237, 241, 248);
            this.pnlLeft.Padding   = new System.Windows.Forms.Padding(0);

            this.lblSectionsHdr.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblSectionsHdr.Height    = 32;
            this.lblSectionsHdr.Text      = "  SECTIONS";
            this.lblSectionsHdr.Font      = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblSectionsHdr.ForeColor = System.Drawing.Color.FromArgb(90, 100, 130);
            this.lblSectionsHdr.BackColor = System.Drawing.Color.FromArgb(220, 228, 245);
            this.lblSectionsHdr.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblSectionsHdr.Padding   = new System.Windows.Forms.Padding(6, 0, 0, 0);

            this.lstSections.Dock              = System.Windows.Forms.DockStyle.Fill;
            this.lstSections.Font              = new System.Drawing.Font("Segoe UI", 10F);
            this.lstSections.BorderStyle       = System.Windows.Forms.BorderStyle.None;
            this.lstSections.BackColor         = System.Drawing.Color.FromArgb(237, 241, 248);
            this.lstSections.ItemHeight        = 32;
            this.lstSections.DrawMode          = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstSections.DrawItem         += LstSections_DrawItem;
            this.lstSections.SelectedIndexChanged += new System.EventHandler(this.lstSections_SelectedIndexChanged);

            this.pnlLeft.Controls.Add(this.lstSections);
            this.pnlLeft.Controls.Add(this.lblSectionsHdr);

            // ── Right panel (shortcut grid) ───────────────────────────
            this.pnlRight.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.pnlRight.Padding   = new System.Windows.Forms.Padding(12, 8, 12, 4);
            this.pnlRight.BackColor = System.Drawing.Color.FromArgb(245, 247, 250);

            this.lblSectionTitle.Dock      = System.Windows.Forms.DockStyle.Top;
            this.lblSectionTitle.Height    = 38;
            this.lblSectionTitle.Font      = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.lblSectionTitle.ForeColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.lblSectionTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // Grid
            this.gridShortcuts.Dock                  = System.Windows.Forms.DockStyle.Fill;
            this.gridShortcuts.AllowUserToAddRows     = false;
            this.gridShortcuts.AllowUserToDeleteRows  = false;
            this.gridShortcuts.ReadOnly               = true;
            this.gridShortcuts.SelectionMode          = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.gridShortcuts.MultiSelect            = false;
            this.gridShortcuts.RowHeadersVisible      = false;
            this.gridShortcuts.BorderStyle            = System.Windows.Forms.BorderStyle.None;
            this.gridShortcuts.BackgroundColor        = System.Drawing.Color.FromArgb(245, 247, 250);
            this.gridShortcuts.GridColor              = System.Drawing.Color.FromArgb(220, 224, 232);
            this.gridShortcuts.RowTemplate.Height     = 30;
            this.gridShortcuts.AutoSizeRowsMode       = System.Windows.Forms.DataGridViewAutoSizeRowsMode.None;
            this.gridShortcuts.Font                   = new System.Drawing.Font("Segoe UI", 10F);
            this.gridShortcuts.EnableHeadersVisualStyles = false;

            this.gridShortcuts.DefaultCellStyle.Padding         = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.gridShortcuts.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(232, 240, 254);
            this.gridShortcuts.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.gridShortcuts.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(250, 252, 255);

            this.gridShortcuts.ColumnHeadersHeight    = 32;
            this.gridShortcuts.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.gridShortcuts.ColumnHeadersDefaultCellStyle.ForeColor = System.Drawing.Color.White;
            this.gridShortcuts.ColumnHeadersDefaultCellStyle.Font      = new System.Drawing.Font("Segoe UI Semibold", 9.5F);
            this.gridShortcuts.ColumnHeadersDefaultCellStyle.Padding   = new System.Windows.Forms.Padding(8, 0, 0, 0);

            this.colKey.Name            = "colKey";
            this.colKey.HeaderText      = "Shortcut / Feature";
            this.colKey.MinimumWidth    = 140;
            this.colKey.DefaultCellStyle.Font      = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Bold);
            this.colKey.DefaultCellStyle.ForeColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.colKey.DefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(240, 245, 255);
            this.colKey.DefaultCellStyle.SelectionBackColor = System.Drawing.Color.FromArgb(210, 228, 252);
            this.colKey.DefaultCellStyle.SelectionForeColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.colKey.SortMode        = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            this.colAction.Name         = "colAction";
            this.colAction.HeaderText   = "Description";
            this.colAction.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colAction.DefaultCellStyle.Padding = new System.Windows.Forms.Padding(12, 0, 0, 0);
            this.colAction.SortMode     = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;

            this.gridShortcuts.Columns.Add(this.colKey);
            this.gridShortcuts.Columns.Add(this.colAction);

            this.pnlRight.Controls.Add(this.gridShortcuts);
            this.pnlRight.Controls.Add(this.lblSectionTitle);

            this.splitter.Panel1.Controls.Add(this.pnlLeft);
            this.splitter.Panel2.Controls.Add(this.pnlRight);

            // ── Footer ────────────────────────────────────────────────
            this.pnlFooter.Dock      = System.Windows.Forms.DockStyle.Bottom;
            this.pnlFooter.Height    = 44;
            this.pnlFooter.BackColor = System.Drawing.Color.FromArgb(230, 235, 245);
            this.pnlFooter.Padding   = new System.Windows.Forms.Padding(12, 6, 12, 6);

            this.btnClose.Text      = "Close  (Esc)";
            this.btnClose.Size      = new System.Drawing.Size(120, 32);
            this.btnClose.Dock      = System.Windows.Forms.DockStyle.Right;
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(21, 101, 192);
            this.btnClose.ForeColor = System.Drawing.Color.White;
            this.btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClose.FlatAppearance.BorderSize = 0;
            this.btnClose.Font      = new System.Drawing.Font("Segoe UI", 9.5F);
            this.btnClose.Click    += new System.EventHandler(this.btnClose_Click);

            this.lblVersion.Text      = "Stock Check & Adjustment Module  •  Shortcut Reference  •  v2.0";
            this.lblVersion.Dock      = System.Windows.Forms.DockStyle.Fill;
            this.lblVersion.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblVersion.Font      = new System.Drawing.Font("Segoe UI", 8.5F);
            this.lblVersion.ForeColor = System.Drawing.Color.FromArgb(100, 110, 130);

            this.pnlFooter.Controls.Add(this.btnClose);
            this.pnlFooter.Controls.Add(this.lblVersion);

            // ── Assemble form ─────────────────────────────────────────
            this.Controls.Add(this.splitter);
            this.Controls.Add(this.pnlTip);
            this.Controls.Add(this.pnlHeader);
            this.Controls.Add(this.pnlFooter);

            this.pnlHeader.ResumeLayout(false);
            this.pnlLeft.ResumeLayout(false);
            this.pnlRight.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridShortcuts)).EndInit();
            this.pnlFooter.ResumeLayout(false);
            this.splitter.Panel1.ResumeLayout(false);
            this.splitter.Panel2.ResumeLayout(false);
            this.splitter.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        // ──────────────────────────────────────────────────────────────
        // Owner-draw list items
        // ──────────────────────────────────────────────────────────────
        private void LstSections_DrawItem(object sender,
            System.Windows.Forms.DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            e.DrawBackground();

            bool selected = (e.State & System.Windows.Forms.DrawItemState.Selected) != 0;
            var bg = selected
                ? System.Drawing.Color.FromArgb(21, 101, 192)
                : System.Drawing.Color.FromArgb(237, 241, 248);
            var fg = selected
                ? System.Drawing.Color.White
                : System.Drawing.Color.FromArgb(30, 40, 60);

            e.Graphics.FillRectangle(new System.Drawing.SolidBrush(bg), e.Bounds);

            if (selected)
            {
                e.Graphics.FillRectangle(
                    new System.Drawing.SolidBrush(System.Drawing.Color.FromArgb(255, 200, 60)),
                    e.Bounds.Left, e.Bounds.Top, 4, e.Bounds.Height);
            }

            System.Windows.Forms.TextRenderer.DrawText(
                e.Graphics,
                lstSections.Items[e.Index].ToString(),
                new System.Drawing.Font("Segoe UI", 9.5F, selected ? System.Drawing.FontStyle.Bold : System.Drawing.FontStyle.Regular),
                System.Drawing.Rectangle.Inflate(e.Bounds, -10, 0),
                fg,
                System.Windows.Forms.TextFormatFlags.VerticalCenter | System.Windows.Forms.TextFormatFlags.Left);
        }

        // ── Fields ──────────────────────────────────────────────────
        private System.Windows.Forms.Panel              pnlHeader;
        private System.Windows.Forms.Label              lblTitle;
        private System.Windows.Forms.Label              lblSubtitle;
        private System.Windows.Forms.Panel              pnlLeft;
        private System.Windows.Forms.ListBox            lstSections;
        private System.Windows.Forms.Label              lblSectionsHdr;
        private System.Windows.Forms.Panel              pnlRight;
        private System.Windows.Forms.Label              lblSectionTitle;
        private System.Windows.Forms.DataGridView       gridShortcuts;
        private System.Windows.Forms.DataGridViewTextBoxColumn colKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAction;
        private System.Windows.Forms.Panel              pnlTip;
        private System.Windows.Forms.Label              lblTip;
        private System.Windows.Forms.Timer              timerTips;
        private System.Windows.Forms.Panel              pnlFooter;
        private System.Windows.Forms.Button             btnClose;
        private System.Windows.Forms.Label              lblVersion;
        private System.Windows.Forms.SplitContainer     splitter;
    }
}
