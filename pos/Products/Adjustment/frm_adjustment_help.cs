using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Products.Adjustment
{
    public partial class frm_adjustment_help : Form
    {
        // ──────────────────────────────────────────────────────────────
        // Section data model
        // ──────────────────────────────────────────────────────────────
        private sealed class HelpSection
        {
            public string Title { get; set; }
            public string Icon  { get; set; }
            public List<HelpRow> Rows { get; set; } = new List<HelpRow>();
        }

        private sealed class HelpRow
        {
            public string Key   { get; set; }
            public string Action { get; set; }
            public bool   IsHeader { get; set; }
        }

        // ──────────────────────────────────────────────────────────────
        // Content
        // ──────────────────────────────────────────────────────────────
        private static readonly List<HelpSection> Sections = new List<HelpSection>
        {
            new HelpSection
            {
                Title = "Navigation", Icon = "🧭",
                Rows = new List<HelpRow>
                {
                    new HelpRow { Key = "F2  /  Ctrl+F", Action = "Focus the Search box" },
                    new HelpRow { Key = "F3",             Action = "Toggle Advanced Filter panel" },
                    new HelpRow { Key = "F4",             Action = "Open Product Detail drawer for selected row" },
                    new HelpRow { Key = "F5",             Action = "Refresh product index from database" },
                    new HelpRow { Key = "F6",             Action = "Enter Barcode Scan Mode" },
                    new HelpRow { Key = "F1",             Action = "Open this help window" },
                }
            },
            new HelpSection
            {
                Title = "Session Lifecycle", Icon = "💾",
                Rows = new List<HelpRow>
                {
                    new HelpRow { Key = "F9",  Action = "Save current session as Draft" },
                    new HelpRow { Key = "F10", Action = "Post Adjustment (confirmation dialog)" },
                }
            },
            new HelpSection
            {
                Title = "Editing", Icon = "✏️",
                Rows = new List<HelpRow>
                {
                    new HelpRow { Key = "Ctrl+Z",    Action = "Undo last cell edit (up to 20)" },
                    new HelpRow { Key = "Ctrl+Y",    Action = "Redo last undone edit" },
                    new HelpRow { Key = "Ctrl+A",    Action = "Select all rows in grid" },
                    new HelpRow { Key = "Ctrl+D",    Action = "Copy current row's Location + Reason to all selected" },
                    new HelpRow { Key = "Ctrl+L",    Action = "Apply current row's New Location to all selected" },
                    new HelpRow { Key = "Delete",    Action = "Remove selected rows (confirm if > 5)" },
                    new HelpRow { Key = "Escape",    Action = "Cancel edit / Close drawer / Exit Scan Mode" },
                }
            },
            new HelpSection
            {
                Title = "Grid Navigation", Icon = "⌨️",
                Rows = new List<HelpRow>
                {
                    new HelpRow { Key = "Tab",        Action = "Next editable cell (skips read-only columns)" },
                    new HelpRow { Key = "Shift+Tab",  Action = "Previous editable cell" },
                    new HelpRow { Key = "Enter",      Action = "Add selected search result to grid" },
                }
            },
            new HelpSection
            {
                Title = "Export & Print", Icon = "🖨️",
                Rows = new List<HelpRow>
                {
                    new HelpRow { Key = "Ctrl+E", Action = "Export session to Excel" },
                    new HelpRow { Key = "Ctrl+P", Action = "Print adjustment voucher" },
                }
            },
            new HelpSection
            {
                Title = "Smart Tab Order", Icon = "🔁",
                Rows = new List<HelpRow>
                {
                    new HelpRow { IsHeader = true,  Key = "Tab stops only on editable columns:", Action = "" },
                    new HelpRow { Key = "1st",  Action = "Physical / New Qty" },
                    new HelpRow { Key = "2nd",  Action = "New Sale Price" },
                    new HelpRow { Key = "3rd",  Action = "New Location" },
                    new HelpRow { Key = "4th",  Action = "Reason" },
                    new HelpRow { Key = "5th",  Action = "Notes  →  wraps to next row's Qty" },
                }
            },
            new HelpSection
            {
                Title = "Workflow Features", Icon = "⚡",
                Rows = new List<HelpRow>
                {
                    new HelpRow { Key = "Remember Last Reason", Action = "New rows auto-fill with the last Reason you used" },
                    new HelpRow { Key = "Batch Verify",         Action = "Shift+click two rows → marks all rows between as Verified" },
                    new HelpRow { Key = "Quick Location Copy",  Action = "Edit a Location cell → tooltip reminds you to press Ctrl+L" },
                    new HelpRow { Key = "Auto-Save (5 min)",    Action = "Draft saved automatically; status bar shows last save time" },
                    new HelpRow { Key = "Close Protection",     Action = "Warns about unsaved changes when closing the form" },
                }
            },
            new HelpSection
            {
                Title = "Adjustment Types", Icon = "📋",
                Rows = new List<HelpRow>
                {
                    new HelpRow { Key = "Physical Count",    Action = "Routine stock count reconciliation" },
                    new HelpRow { Key = "Damage Write-Off",  Action = "Remove damaged or expired goods" },
                    new HelpRow { Key = "Found / Excess",    Action = "Record stock found above system level" },
                    new HelpRow { Key = "Price Update",      Action = "Correct sale prices only" },
                    new HelpRow { Key = "Location Transfer", Action = "Move stock between warehouse locations" },
                    new HelpRow { Key = "Opening Stock",     Action = "Initial stock entry for new products" },
                }
            },
        };

        // ──────────────────────────────────────────────────────────────
        // Constructor
        // ──────────────────────────────────────────────────────────────
        public frm_adjustment_help()
        {
            InitializeComponent();
            BuildUi();
        }

        // ──────────────────────────────────────────────────────────────
        // Build UI programmatically
        // ──────────────────────────────────────────────────────────────
        private void BuildUi()
        {
            // Populate section list
            lstSections.Items.Clear();
            foreach (var sec in Sections)
                lstSections.Items.Add(sec.Icon + "  " + sec.Title);

            lstSections.SelectedIndex = 0;
        }

        // ──────────────────────────────────────────────────────────────
        // Section selection → render grid
        // ──────────────────────────────────────────────────────────────
        private void lstSections_SelectedIndexChanged(object sender, EventArgs e)
        {
            int idx = lstSections.SelectedIndex;
            if (idx < 0 || idx >= Sections.Count) return;
            RenderSection(Sections[idx]);
        }

        private void RenderSection(HelpSection section)
        {
            gridShortcuts.Rows.Clear();

            lblSectionTitle.Text = section.Icon + "  " + section.Title;

            foreach (var row in section.Rows)
            {
                if (row.IsHeader)
                {
                    int r = gridShortcuts.Rows.Add(row.Key, "");
                    gridShortcuts.Rows[r].DefaultCellStyle.BackColor    = Color.FromArgb(232, 240, 254);
                    gridShortcuts.Rows[r].DefaultCellStyle.ForeColor    = Color.FromArgb(21, 101, 192);
                    gridShortcuts.Rows[r].DefaultCellStyle.Font         = new Font("Segoe UI", 9F, FontStyle.Bold);
                    gridShortcuts.Rows[r].DefaultCellStyle.SelectionBackColor = Color.FromArgb(210, 228, 252);
                    gridShortcuts.Rows[r].DefaultCellStyle.SelectionForeColor = Color.FromArgb(21, 101, 192);
                }
                else
                {
                    gridShortcuts.Rows.Add(row.Key, row.Action);
                }
            }

            // Resize key column to content
            gridShortcuts.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            gridShortcuts.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        // ──────────────────────────────────────────────────────────────
        // Close button
        // ──────────────────────────────────────────────────────────────
        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        // ──────────────────────────────────────────────────────────────
        // Tips panel content
        // ──────────────────────────────────────────────────────────────
        private static readonly string[] Tips = new[]
        {
            "💡 Use Barcode Scan Mode (F6) for fastest data entry — no mouse needed.",
            "💡 Shift+click to batch-verify an entire shelf's rows at once.",
            "💡 Press F9 mid-session to save a draft — never lose a long count.",
            "💡 Filter by aisle first (F3) to narrow scope before scanning.",
            "💡 Ctrl+D copies Location + Reason from the current row to all selected.",
            "💡 The ✖ column button removes a single row without affecting others.",
            "💡 Blue row number = modified but unsaved.  Green = verified.",
            "💡 The status bar shows auto-save time, current user, and warehouse.",
        };

        private int _tipIndex;

        private void timerTips_Tick(object sender, EventArgs e)
        {
            lblTip.Text = Tips[_tipIndex % Tips.Length];
            _tipIndex++;
        }

        private void frm_adjustment_help_Load(object sender, EventArgs e)
        {
            lblTip.Text = Tips[0];
            timerTips.Start();
        }
    }
}
