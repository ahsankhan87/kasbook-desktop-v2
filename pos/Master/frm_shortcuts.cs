using System;
using System.Windows.Forms;

namespace pos.Purchases
{
    public partial class frm_shortcuts : Form
    {
        public frm_shortcuts()
        {
            InitializeComponent();
        }

        private void frm_shortcuts_Load(object sender, EventArgs e)
        {
            // Populate shortcut list (read-only) for quick reference.
            // NOTE: Keep these in sync with actual shortcuts implemented in forms.
            if (gridShortcuts == null) return;

            gridShortcuts.Rows.Clear();

            gridShortcuts.Rows.Add("F1", "New Sale / Purchase");
            gridShortcuts.Rows.Add("F2", "—");
            gridShortcuts.Rows.Add("F3", "Save (Sales / Purchases)");
            gridShortcuts.Rows.Add("F4", "Update / Search (depends on screen)");
            gridShortcuts.Rows.Add("F5", "Round / Fix Amount");
            gridShortcuts.Rows.Add("F6", "Enable / Disable Print");
            gridShortcuts.Rows.Add("F9", "Focus Sales / Purchase Grid");

            gridShortcuts.Rows.Add("Ctrl + H", "Load Product History");
            gridShortcuts.Rows.Add("Ctrl + O  (or F4)", "Open Search Form (Sales / Purchases)");
            gridShortcuts.Rows.Add("Ctrl + L", "Load Order / Estimate");

            gridShortcuts.Rows.Add("Ctrl + Alt + C", "Focus Customer (Sales)");
            gridShortcuts.Rows.Add("Ctrl + Alt + B", "Focus Barcode (Sales)");
            gridShortcuts.Rows.Add("Ctrl + Alt + I", "Focus Invoice No (Sales)");

            gridShortcuts.Rows.Add("Ctrl + Alt + P", "Focus Print Invoice (Sales)");
            gridShortcuts.Rows.Add("Ctrl + Alt + R", "Focus Return Invoice (Sales)");
            gridShortcuts.Rows.Add("Ctrl + Alt + N", "Focus Employees (Sales)");

            gridShortcuts.Rows.Add("Ctrl + Alt + T", "Focus Sale Type (Sales)");
            gridShortcuts.Rows.Add("Ctrl + Alt + U", "Focus Show Cost Checkbox (Sales)");
            gridShortcuts.Rows.Add("Ctrl + Alt + Z", "Focus ZATCA Checkbox (Sales)");

            // Make the first row selected for nicer appearance.
            if (gridShortcuts.Rows.Count > 0)
            {
                gridShortcuts.ClearSelection();
                gridShortcuts.Rows[0].Selected = true;
            }
        }

        // wired in Designer
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
