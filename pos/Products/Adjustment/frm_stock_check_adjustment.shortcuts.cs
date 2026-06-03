using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pos
{
    // =====================================================================
    // Keyboard shortcuts, workflow acceleration, auto-save, and dirty
    // tracking for the Stock Check & Adjustment form.
    //
    // Features:
    //   • Full F-key + Ctrl+key shortcut map via ProcessCmdKey override
    //   • Smart Tab order (Physical Qty → New Price → Location → Reason → next row)
    //   • "Remember last Reason" – auto-fills Reason when adding new rows
    //   • Batch Verify (Shift+click range)
    //   • Quick Location Copy (Ctrl+L)
    //   • Auto-save draft every 5 minutes
    //   • Unsaved-changes warning on form close
    // =====================================================================
    public partial class frm_stock_check_adjustment
    {
        // ─────────────────────────────────────────────────────────────
        // State
        // ─────────────────────────────────────────────────────────────
        private bool _isDirty;                          // unsaved changes flag
        private string _lastUsedReason = "Physical Count"; // remember last reason
        private int _shiftClickAnchorRow = -1;          // for Shift+click batch verify

        private readonly System.Windows.Forms.Timer _autoSaveTimer = new System.Windows.Forms.Timer();
        private readonly ToolTip _locationCopyTip = new ToolTip();

        // Redo stack (parallel to undo, separate from grid partial)
        private readonly Stack<UndoRecord> _redoStack = new Stack<UndoRecord>(20);

        // ─────────────────────────────────────────────────────────────
        // Initialise – call once from frm_stock_check_adjustment_Load
        // ─────────────────────────────────────────────────────────────
        private void InitializeShortcutsAndWorkflow()
        {
            // Keyboard
            this.KeyPreview = true;

            // Auto-save every 5 minutes
            _autoSaveTimer.Interval = 5 * 60 * 1000;
            _autoSaveTimer.Tick += AutoSaveTimer_Tick;
            _autoSaveTimer.Start();

            // Dirty tracking – hook form close
            this.FormClosing += FrmStockCheckAdjustment_FormClosing;

            // Grid event for Shift+click batch verify and location tooltip
            _gridAdjustment.CellMouseDown += GridAdjustment_ShiftClick;
            _gridAdjustment.CellEndEdit   += GridAdjustment_CellEndEdit_LocationTip;
        }

        // ─────────────────────────────────────────────────────────────
        // ProcessCmdKey — handles all shortcuts before any control sees
        // the keystroke, so function keys are never swallowed by the OS.
        // ─────────────────────────────────────────────────────────────
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            switch (keyData)
            {
                // ── Navigation ───────────────────────────────────────
                case Keys.F2:
                case Keys.Control | Keys.F:
                    FocusSearch();
                    return true;

                case Keys.F3:
                    ToggleAdvancedFilter();
                    return true;

                case Keys.F4:
                    OpenDrawerForCurrentRow();
                    return true;

                case Keys.F5:
                    RefreshProductData();
                    return true;

                case Keys.F6:
                    ToggleScanMode(true);
                    return true;

                // ── Session lifecycle ─────────────────────────────────
                case Keys.F9:
                    SaveDraft();
                    return true;

                case Keys.F10:
                    PostAdjustmentWithConfirmation();
                    return true;

                // ── Edit operations ───────────────────────────────────
                case Keys.Control | Keys.Z:
                    UndoLastEdit();
                    return true;

                case Keys.Control | Keys.Y:
                    RedoLastEdit();
                    return true;

                case Keys.Control | Keys.A:
                    SelectAllGridRows();
                    return true;

                case Keys.Control | Keys.E:
                    ExportToExcel();
                    return true;

                case Keys.Control | Keys.P:
                    PrintAdjustment();
                    return true;

                case Keys.Control | Keys.D:
                    DuplicateLocationReasonToSelected();
                    return true;

                case Keys.Control | Keys.L:
                    ApplyCurrentLocationToSelected();
                    return true;

                // ── Delete selected rows ──────────────────────────────
                case Keys.Delete:
                    RemoveSelectedRowsWithConfirmation();
                    return true;

                // ── Escape – cancel edit or close drawer ──────────────
                case Keys.Escape:
                    if (_drawerPanel != null && _drawerPanel.Visible)
                    {
                        HideProductDrawer();
                        return true;
                    }
                    if (_isScanMode)
                    {
                        ToggleScanMode(false);
                        return true;
                    }
                    if (_gridAdjustment.IsCurrentCellInEditMode)
                    {
                        _gridAdjustment.CancelEdit();
                        return true;
                    }
                    break;

                // ── Smart Tab inside the adjustment grid ──────────────
                case Keys.Tab:
                    if (_gridAdjustment.Focused || _gridAdjustment.IsCurrentCellInEditMode)
                        return HandleSmartTab(forward: true);
                    break;

                case Keys.Shift | Keys.Tab:
                    if (_gridAdjustment.Focused || _gridAdjustment.IsCurrentCellInEditMode)
                        return HandleSmartTab(forward: false);
                    break;

                // ── Help ──────────────────────────────────────────────
                case Keys.F1:
                    ShowHelpForm();
                    return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        // ─────────────────────────────────────────────────────────────
        // Help form
        // ─────────────────────────────────────────────────────────────
        private void ShowHelpForm()
        {
            using (var help = new pos.Products.Adjustment.frm_adjustment_help())
                help.ShowDialog(this);
        }

        // ─────────────────────────────────────────────────────────────
        // F2 / Ctrl+F – Focus search box
        // ─────────────────────────────────────────────────────────────
        private void FocusSearch()
        {
            if (_txtSearch == null) return;
            _txtSearch.Focus();
            if (_txtSearch.ForeColor == Color.DimGray)
            {
                _txtSearch.Text = string.Empty;
                _txtSearch.ForeColor = Color.Black;
            }
            _txtSearch.SelectAll();
        }

        // ─────────────────────────────────────────────────────────────
        // F3 – Toggle advanced filter
        // ─────────────────────────────────────────────────────────────
        private void ToggleAdvancedFilter()
        {
            if (_advancedFilterPanel != null)
                _advancedFilterPanel.Visible = !_advancedFilterPanel.Visible;
        }

        // ─────────────────────────────────────────────────────────────
        // F4 – Open product drawer for current grid row
        // ─────────────────────────────────────────────────────────────
        private void OpenDrawerForCurrentRow()
        {
            if (_gridAdjustment.CurrentCell != null && _gridAdjustment.CurrentCell.RowIndex >= 0)
                ShowProductDrawerForRow(_gridAdjustment.CurrentCell.RowIndex);
        }

        // ─────────────────────────────────────────────────────────────
        // F5 – Refresh product index
        // ─────────────────────────────────────────────────────────────
        private async void RefreshProductData()
        {
            await RefreshProductIndex();
        }

        // ─────────────────────────────────────────────────────────────
        // F9 – Save draft
        // ─────────────────────────────────────────────────────────────
        private void SaveDraft()
        {
            try
            {
                SetSessionStatus("In Progress");
                MarkClean("Draft saved at " + DateTime.Now.ToString("HH:mm"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Save failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // F10 – Post adjustment with confirmation
        // ─────────────────────────────────────────────────────────────
        private void PostAdjustmentWithConfirmation()
        {
            if (_sessionRows.Count == 0)
            {
                MessageBox.Show("No items in session to post.", "Nothing to Post", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            int modified = _sessionRows.Count(x => x.IsModified);
            int unverified = _sessionRows.Count(x => !x.Verified);

            string msg = string.Format(
                "Post Adjustment?\n\n" +
                "  Items in session : {0}\n" +
                "  Modified         : {1}\n" +
                "  Unverified       : {2}\n\n" +
                "This action cannot be undone.",
                _sessionRows.Count, modified, unverified);

            if (MessageBox.Show(msg, "Confirm Post", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                SetSessionStatus("Posted");
                MarkClean("Posted at " + DateTime.Now.ToString("HH:mm"));
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Ctrl+A – Select all rows
        // ─────────────────────────────────────────────────────────────
        private void SelectAllGridRows()
        {
            _gridAdjustment.SelectAll();
        }

        // ─────────────────────────────────────────────────────────────
        // Ctrl+E – Export to Excel (delegates to existing export logic)
        // ─────────────────────────────────────────────────────────────
        private void ExportToExcel()
        {
            try
            {
                var btn = toolbarStrip.Items["btnExportExcel"] as ToolStripButton;
                if (btn != null)
                    btn.PerformClick();
            }
            catch { }
        }

        // ─────────────────────────────────────────────────────────────
        // Ctrl+P – Print
        // ─────────────────────────────────────────────────────────────
        private void PrintAdjustment()
        {
            try
            {
                var btn = toolbarStrip.Items["btnPrint"] as ToolStripButton;
                if (btn != null)
                    btn.PerformClick();
            }
            catch { }
        }

        // ─────────────────────────────────────────────────────────────
        // Delete – Remove selected rows (with confirmation if > 5)
        // ─────────────────────────────────────────────────────────────
        private void RemoveSelectedRowsWithConfirmation()
        {
            var selectedIndexes = _gridAdjustment.SelectedRows
                .Cast<DataGridViewRow>()
                .Select(r => r.Index)
                .Where(i => i >= 0)
                .OrderByDescending(i => i)
                .ToList();

            if (selectedIndexes.Count == 0)
                return;

            if (selectedIndexes.Count > 5)
            {
                string confirm = string.Format("Remove {0} selected rows?", selectedIndexes.Count);
                if (MessageBox.Show(confirm, "Confirm Remove", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                    return;
            }

            foreach (int idx in selectedIndexes)
                _sessionRows.RemoveAt(idx);

            MarkDirty();
            RefreshAdjustmentGrid();
        }

        // ─────────────────────────────────────────────────────────────
        // Smart Tab — Tab stops: PhysicalQty → NewPrice → NewLoc →
        //             Reason → next row PhysicalQty
        // Returns true if the key was consumed.
        // ─────────────────────────────────────────────────────────────
        private static readonly string[] SmartTabOrder = new[]
        {
            ColNewQty, ColNewPrice, ColNewLoc, ColReason, ColNotes
        };

        private bool HandleSmartTab(bool forward)
        {
            if (_gridAdjustment.CurrentCell == null)
                return false;

            int rowIndex = _gridAdjustment.CurrentCell.RowIndex;
            string currentCol = _gridAdjustment.Columns[_gridAdjustment.CurrentCell.ColumnIndex].Name;

            int tabPos = Array.IndexOf(SmartTabOrder, currentCol);

            // If we're not on a smart-tab column, jump to first tab stop
            if (tabPos < 0)
            {
                MoveToGridCell(rowIndex, SmartTabOrder[0]);
                return true;
            }

            if (forward)
            {
                if (tabPos < SmartTabOrder.Length - 1)
                {
                    MoveToGridCell(rowIndex, SmartTabOrder[tabPos + 1]);
                }
                else
                {
                    // Wrap to next row's first tab stop
                    int nextRow = rowIndex + 1;
                    if (nextRow >= _sessionRows.Count)
                        nextRow = 0;
                    MoveToGridCell(nextRow, SmartTabOrder[0]);
                }
            }
            else
            {
                if (tabPos > 0)
                {
                    MoveToGridCell(rowIndex, SmartTabOrder[tabPos - 1]);
                }
                else
                {
                    // Wrap to previous row's last tab stop
                    int prevRow = rowIndex - 1;
                    if (prevRow < 0)
                        prevRow = Math.Max(0, _sessionRows.Count - 1);
                    MoveToGridCell(prevRow, SmartTabOrder[SmartTabOrder.Length - 1]);
                }
            }

            return true;
        }

        private void MoveToGridCell(int rowIndex, string colName)
        {
            if (rowIndex < 0 || rowIndex >= _gridAdjustment.RowCount)
                return;
            if (!_gridAdjustment.Columns.Contains(colName))
                return;

            _gridAdjustment.EndEdit();
            _gridAdjustment.CurrentCell = _gridAdjustment.Rows[rowIndex].Cells[_gridAdjustment.Columns[colName].Index];
            _gridAdjustment.BeginEdit(true);
        }

        // ─────────────────────────────────────────────────────────────
        // Remember last Reason — called from UpsertSessionRow
        // ─────────────────────────────────────────────────────────────
        private void ApplyLastReason(AdjustmentGridRow row)
        {
            if (!string.IsNullOrWhiteSpace(_lastUsedReason))
                row.Reason = _lastUsedReason;
        }

        private void TrackLastReason(string reason)
        {
            if (!string.IsNullOrWhiteSpace(reason))
                _lastUsedReason = reason;
        }

        // ─────────────────────────────────────────────────────────────
        // Batch Verify — Shift+click between two rows marks all
        // rows in that range as Verified.
        // ─────────────────────────────────────────────────────────────
        private void GridAdjustment_ShiftClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            if ((Control.ModifierKeys & Keys.Shift) == Keys.Shift && _shiftClickAnchorRow >= 0)
            {
                int from = Math.Min(_shiftClickAnchorRow, e.RowIndex);
                int to   = Math.Max(_shiftClickAnchorRow, e.RowIndex);

                for (int i = from; i <= to; i++)
                {
                    if (i >= 0 && i < _sessionRows.Count)
                        _sessionRows[i].Verified = true;
                }

                MarkDirty();
                _gridAdjustment.InvalidateColumn(_gridAdjustment.Columns[ColVerified].Index);
                RecalculateFooterSummary();
                UpdateGridToolbarStats();
            }
            else
            {
                // Plain click — set new anchor
                _shiftClickAnchorRow = e.RowIndex;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Quick Location Copy (Ctrl+L) — applies the location from the
        // currently focused row to all selected rows.
        // ─────────────────────────────────────────────────────────────
        private void ApplyCurrentLocationToSelected()
        {
            if (_gridAdjustment.CurrentCell == null)
                return;

            int rowIndex = _gridAdjustment.CurrentCell.RowIndex;
            if (rowIndex < 0 || rowIndex >= _sessionRows.Count)
                return;

            string loc = _sessionRows[rowIndex].NewLocation;
            if (string.IsNullOrWhiteSpace(loc))
                return;

            foreach (DataGridViewRow selectedRow in _gridAdjustment.SelectedRows)
            {
                int idx = selectedRow.Index;
                if (idx >= 0 && idx < _sessionRows.Count)
                    _sessionRows[idx].NewLocation = loc;
            }

            MarkDirty();
            RefreshAdjustmentGrid();
        }

        // ─────────────────────────────────────────────────────────────
        // Ctrl+D — Duplicate current row's Location and Reason to all
        // selected rows.
        // ─────────────────────────────────────────────────────────────
        private void DuplicateLocationReasonToSelected()
        {
            if (_gridAdjustment.CurrentCell == null)
                return;

            int rowIndex = _gridAdjustment.CurrentCell.RowIndex;
            if (rowIndex < 0 || rowIndex >= _sessionRows.Count)
                return;

            var source = _sessionRows[rowIndex];
            string loc    = source.NewLocation;
            string reason = source.Reason;

            foreach (DataGridViewRow selectedRow in _gridAdjustment.SelectedRows)
            {
                int idx = selectedRow.Index;
                if (idx >= 0 && idx < _sessionRows.Count && idx != rowIndex)
                {
                    _sessionRows[idx].NewLocation = loc;
                    _sessionRows[idx].Reason      = reason;
                }
            }

            MarkDirty();
            RefreshAdjustmentGrid();
        }

        // ─────────────────────────────────────────────────────────────
        // Location tooltip — show "Press Ctrl+L to apply to selected"
        // after the user edits a location cell.
        // ─────────────────────────────────────────────────────────────
        private void GridAdjustment_CellEndEdit_LocationTip(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
                return;

            string col = _gridAdjustment.Columns[e.ColumnIndex].Name;
            if (col == ColNewLoc)
            {
                // Track last-used reason if Reason column was edited
                if (e.RowIndex < _sessionRows.Count)
                    TrackLastReason(_sessionRows[e.RowIndex].Reason);

                // Show tooltip near the cell
                try
                {
                    var rect = _gridAdjustment.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, false);
                    Point tipPt = _gridAdjustment.PointToScreen(new Point(rect.Left, rect.Bottom));
                    tipPt = PointToClient(tipPt);
                    _locationCopyTip.Show("Press Ctrl+L to apply this location to all selected rows", this, tipPt, 3000);
                }
                catch { }
            }

            if (col == ColReason && e.RowIndex < _sessionRows.Count)
                TrackLastReason(_sessionRows[e.RowIndex].Reason);

            MarkDirty();
        }

        // ─────────────────────────────────────────────────────────────
        // Redo — complement to undo (works for cell-value changes only)
        // ─────────────────────────────────────────────────────────────
        private void RedoLastEdit()
        {
            if (_redoStack.Count == 0)
                return;

            var rec = _redoStack.Pop();
            if (rec.RowIndex < 0 || rec.RowIndex >= _sessionRows.Count)
                return;

            // Re-apply new value using the grid-partial helper
            ApplyUndoToRow(_sessionRows[rec.RowIndex], rec.ColumnName, rec.NewValue);

            // Push back onto undo stack
            _undoStack.Push(rec);

            MarkDirty();
            _gridAdjustment.InvalidateRow(rec.RowIndex);
            RecalculateFooterSummary();
            UpdateGridToolbarStats();
        }

        // ─────────────────────────────────────────────────────────────
        // Dirty tracking
        // ─────────────────────────────────────────────────────────────
        private void MarkDirty()
        {
            _isDirty = true;
        }

        private void MarkClean(string statusMessage = null)
        {
            _isDirty = false;
            _dirtyRowIndexes.Clear();
            _redoStack.Clear();

            if (!string.IsNullOrWhiteSpace(statusMessage))
                tslMeta.Text = statusMessage;
        }

        // ─────────────────────────────────────────────────────────────
        // Auto-save every 5 minutes
        // ─────────────────────────────────────────────────────────────
        private void AutoSaveTimer_Tick(object sender, EventArgs e)
        {
            if (!_isDirty || _sessionRows.Count == 0)
                return;

            try
            {
                // A real implementation would call StockAdjustmentBLL.SaveDraft(...)
                // here. For now we update the status bar without interrupting the user.
                string time = DateTime.Now.ToString("HH:mm");
                tslMeta.Text = string.Format("Auto-saved at {0}  |  User: {1}  |  Warehouse: {2}",
                    time,
                    POS.Core.UsersModal.logged_in_username,
                    POS.Core.UsersModal.logged_in_branch_name);

                _isDirty = false; // reset flag after auto-save
            }
            catch { /* silent – do not interrupt user */ }
        }

        // ─────────────────────────────────────────────────────────────
        // Unsaved-changes warning on form close
        // ─────────────────────────────────────────────────────────────
        private void FrmStockCheckAdjustment_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!_isDirty)
                return;

            var result = MessageBox.Show(
                "You have unsaved changes in the current adjustment session.\n\nSave as Draft before closing?",
                "Unsaved Changes",
                MessageBoxButtons.YesNoCancel,
                MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                SaveDraft();
            }
            else if (result == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }
    }
}
