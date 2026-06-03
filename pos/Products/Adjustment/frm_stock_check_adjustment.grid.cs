using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace pos
{
    // =====================================================================
    // High-performance virtual-mode DataGridView support for the stock
    // check & adjustment form (up to 50 000 product rows).
    //
    // Responsibilities of this partial:
    //   • Double-buffer the grid via reflection (no flicker)
    //   • Freeze the identity columns so they stay visible on scroll
    //   • Fixed row height (28 px) — no AutoSizeRowsMode
    //   • Dirty-row tracking (only modified rows processed on Save/Post)
    //   • Undo stack (last 20 cell edits, Ctrl+Z)
    //   • Look-ahead render buffer (visible rows + 100 ahead)
    //   • Custom cell painting (diff colours, location badge)
    //   • Row-count update helper used by the rest of the form
    // =====================================================================
    public partial class frm_stock_check_adjustment
    {
        // ─────────────────────────────────────────────────────────────
        // Undo operation record
        // ─────────────────────────────────────────────────────────────
        private sealed class UndoRecord
        {
            public int RowIndex { get; set; }
            public string ColumnName { get; set; }
            public object OldValue { get; set; }
            public object NewValue { get; set; }
        }

        // ─────────────────────────────────────────────────────────────
        // State
        // ─────────────────────────────────────────────────────────────
        private readonly HashSet<int> _dirtyRowIndexes = new HashSet<int>();
        private readonly Stack<UndoRecord> _undoStack = new Stack<UndoRecord>(20);
        // _redoStack is declared in frm_stock_check_adjustment.shortcuts.cs
        private const int MaxUndoDepth = 20;

        // Render buffer – indices of rows currently pre-fetched for painting
        private int _renderBufferStart = -1;
        private int _renderBufferEnd   = -1;
        private const int RenderLookAhead = 100;

        // ─────────────────────────────────────────────────────────────
        // One-time setup – call from BuildRightPanelUi() after the
        // DataGridView is created but before it is shown.
        // ─────────────────────────────────────────────────────────────
        private void ConfigureVirtualGrid()
        {
            // ── Double-buffer via reflection (eliminates flicker) ──────
            SetDoubleBuffered(_gridAdjustment, true);

            // ── Fixed row height – AutoSizeRowsMode is O(n) on 50k rows ─
            _gridAdjustment.RowTemplate.Height  = 28;
            _gridAdjustment.AutoSizeRowsMode    = DataGridViewAutoSizeRowsMode.None;
            _gridAdjustment.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;

            // ── Freeze identity columns (always visible while scrolling) ─
            // Columns must be frozen in ascending DisplayIndex order.
            _gridAdjustment.Columns[ColRowNo].Frozen   = true;
            _gridAdjustment.Columns[ColVerified].Frozen = true;
            _gridAdjustment.Columns[ColCode].Frozen     = true;
            _gridAdjustment.Columns[ColName].Frozen     = true;

            // ── Scroll event – pre-fetch render buffer ─────────────────
            _gridAdjustment.Scroll += GridAdjustment_Scroll;

            // ── Custom painting ─────────────────────────────────────────
            _gridAdjustment.CellPainting += GridAdjustment_CellPainting;

            // ── Keyboard shortcuts (Ctrl+Z undo) ────────────────────────
            // Hooked in the existing KeyDown handler via HandleUndoKey()
        }

        // ─────────────────────────────────────────────────────────────
        // Double-buffer helper (reflection – safe on .NET 4.x)
        // ─────────────────────────────────────────────────────────────
        private static void SetDoubleBuffered(Control control, bool value)
        {
            PropertyInfo pi = typeof(Control).GetProperty(
                "DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            if (pi != null)
                pi.SetValue(control, value, null);
        }

        // ─────────────────────────────────────────────────────────────
        // Dirty-row API
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns only the rows that have been modified since the last
        /// Save/Post. Call this instead of iterating all _sessionRows.
        /// </summary>
        private List<AdjustmentGridRow> GetDirtyRows()
        {
            var list = new List<AdjustmentGridRow>(_dirtyRowIndexes.Count);
            foreach (int i in _dirtyRowIndexes)
            {
                if (i >= 0 && i < _sessionRows.Count)
                    list.Add(_sessionRows[i]);
            }
            return list;
        }

        /// <summary>
        /// Called after a successful Save/Post to reset dirty tracking.
        /// </summary>
        public void ClearDirtyState()
        {
            _dirtyRowIndexes.Clear();
            _gridAdjustment.Invalidate();
        }

        /// <summary>
        /// Returns true if any row has been edited since the last save.
        /// </summary>
        public bool HasUnsavedChanges
        {
            get { return _dirtyRowIndexes.Count > 0; }
        }

        // ─────────────────────────────────────────────────────────────
        // Undo support
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Records a cell edit before the value is changed.
        /// Called at the top of CellValuePushed.
        /// </summary>
        private void RecordUndo(int rowIndex, string columnName, object oldValue, object newValue)
        {
            if (_undoStack.Count >= MaxUndoDepth)
            {
                // Stack<T> has no RemoveBottom — rebuild trimmed version
                var tmp = _undoStack.ToArray();          // top … bottom
                _undoStack.Clear();
                for (int i = 0; i < tmp.Length - 1; i++) // drop the oldest (last)
                    _undoStack.Push(tmp[tmp.Length - 2 - i]);
            }

            _undoStack.Push(new UndoRecord
            {
                RowIndex   = rowIndex,
                ColumnName = columnName,
                OldValue   = oldValue,
                NewValue   = newValue
            });
        }

        /// <summary>
        /// Pops the last undo record and restores the cell value.
        /// Wired to Ctrl+Z in HandleUndoKey().
        /// </summary>
        public void UndoLastEdit()
        {
            if (_undoStack.Count == 0)
                return;

            UndoRecord op = _undoStack.Pop();

            if (op.RowIndex < 0 || op.RowIndex >= _sessionRows.Count)
                return;

            // Push to redo stack before reverting
            _redoStack.Push(new UndoRecord
            {
                RowIndex   = op.RowIndex,
                ColumnName = op.ColumnName,
                OldValue   = op.NewValue,
                NewValue   = op.OldValue
            });

            ApplyUndoToRow(_sessionRows[op.RowIndex], op.ColumnName, op.OldValue);

            // Re-evaluate dirty state for that row
            if (!_sessionRows[op.RowIndex].IsModified)
                _dirtyRowIndexes.Remove(op.RowIndex);

            _gridAdjustment.InvalidateRow(op.RowIndex);
            RecalculateFooterSummary();
            UpdateGridToolbarStats();
        }

        private static void ApplyUndoToRow(AdjustmentGridRow row, string columnName, object oldValue)
        {
            switch (columnName)
            {
                case ColVerified:
                    row.Verified = Convert.ToBoolean(oldValue);
                    break;
                case ColNewQty:
                    decimal qty;
                    row.NewQty = decimal.TryParse(Convert.ToString(oldValue), out qty) ? qty : row.CurrentQty;
                    break;
                case ColNewPrice:
                    decimal price;
                    row.NewPrice = decimal.TryParse(Convert.ToString(oldValue), out price) ? price : row.CurrentPrice;
                    break;
                case ColNewLoc:
                    row.NewLocation = Convert.ToString(oldValue);
                    break;
                case ColReason:
                    row.Reason = Convert.ToString(oldValue);
                    break;
                case ColNotes:
                    row.Notes = Convert.ToString(oldValue);
                    break;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // CellValuePushed override – dirty tracking + undo recording
        // This replaces the handler in the main .cs file.
        // ─────────────────────────────────────────────────────────────
        private void GridAdjustment_CellValuePushed_VirtualGrid(object sender, DataGridViewCellValueEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _sessionRows.Count)
                return;

            var row   = _sessionRows[e.RowIndex];
            string col = _gridAdjustment.Columns[e.ColumnIndex].Name;

            // Capture old value BEFORE mutation
            object oldValue = GetCurrentCellValue(row, col);
            object newValue = e.Value;

            // Record undo
            RecordUndo(e.RowIndex, col, oldValue, newValue);

            // Apply change
            if (col == ColVerified)
                row.Verified  = Convert.ToBoolean(newValue);
            else if (col == ColNewQty)
                row.NewQty    = ParseDecimalSafe(newValue, row.NewQty);
            else if (col == ColNewPrice)
                row.NewPrice  = ParseDecimalSafe(newValue, row.NewPrice);
            else if (col == ColNewLoc)
                row.NewLocation = Convert.ToString(newValue);
            else if (col == ColReason)
                row.Reason    = Convert.ToString(newValue);
            else if (col == ColNotes)
                row.Notes     = Convert.ToString(newValue);

            // Dirty tracking
            if (row.IsModified)
                _dirtyRowIndexes.Add(e.RowIndex);
            else
                _dirtyRowIndexes.Remove(e.RowIndex);

            RecalculateFooterSummary();
            UpdateGridToolbarStats();
            _gridAdjustment.InvalidateRow(e.RowIndex);
        }

        private static object GetCurrentCellValue(AdjustmentGridRow row, string col)
        {
            switch (col)
            {
                case ColVerified:      return row.Verified;
                case ColNewQty:        return row.NewQty;
                case ColNewPrice:      return row.NewPrice;
                case ColNewLoc:        return row.NewLocation;
                case ColReason:        return row.Reason;
                case ColNotes:         return row.Notes;
                default:               return null;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Keyboard handler – plug into the grid's existing KeyDown
        // Call HandleUndoKey(e) from GridAdjustment_KeyDown.
        // ─────────────────────────────────────────────────────────────
        private bool HandleUndoKey(KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.Z)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                UndoLastEdit();
                return true;
            }

            return false;
        }

        // ─────────────────────────────────────────────────────────────
        // Scroll pre-fetch – keeps a render buffer of visible + 100 rows
        // ─────────────────────────────────────────────────────────────
        private void GridAdjustment_Scroll(object sender, ScrollEventArgs e)
        {
            if (e.ScrollOrientation != ScrollOrientation.VerticalScroll)
                return;

            UpdateRenderBuffer();
        }

        private void UpdateRenderBuffer()
        {
            if (_sessionRows.Count == 0)
                return;

            // First visible row
            int firstVisible = _gridAdjustment.FirstDisplayedScrollingRowIndex;
            if (firstVisible < 0) firstVisible = 0;

            // Count visible rows from grid height / row height
            int visibleRows = (_gridAdjustment.ClientSize.Height / _gridAdjustment.RowTemplate.Height) + 1;

            int newStart = firstVisible;
            int newEnd   = Math.Min(_sessionRows.Count - 1, firstVisible + visibleRows + RenderLookAhead);

            // Only invalidate if the buffer window actually moved
            if (newStart != _renderBufferStart || newEnd != _renderBufferEnd)
            {
                _renderBufferStart = newStart;
                _renderBufferEnd   = newEnd;
                // Invalidate only the new region so GDI only repaints what changed
                _gridAdjustment.InvalidateRow(newStart);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Custom cell painting
        //   • Qty-diff column   : green (increase) / red (decrease)
        //   • Price-diff column : green / red
        //   • Location column   : badge style
        //   • Row # column      : blue highlight for dirty rows
        //   • Verified column   : custom checkbox tint
        // ─────────────────────────────────────────────────────────────
        private void GridAdjustment_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= _sessionRows.Count)
                return;

            if (e.ColumnIndex < 0 || e.ColumnIndex >= _gridAdjustment.Columns.Count)
                return;

            var row    = _sessionRows[e.RowIndex];
            string col = _gridAdjustment.Columns[e.ColumnIndex].Name;

            bool isDirty = _dirtyRowIndexes.Contains(e.RowIndex);

            // ── Row # column – blue for dirty, teal for verified ────────
            if (col == ColRowNo)
            {
                Color bg = isDirty
                    ? Color.FromArgb(21, 101, 192)
                    : row.Verified
                        ? Color.FromArgb(27, 94, 32)
                        : Color.FromArgb(245, 247, 250);
                Color fg = (isDirty || row.Verified) ? Color.White : Color.DimGray;

                e.Graphics.FillRectangle(new SolidBrush(bg), e.CellBounds);
                TextRenderer.DrawText(e.Graphics,
                    Convert.ToString(e.RowIndex + 1),
                    e.CellStyle.Font ?? _gridAdjustment.DefaultCellStyle.Font,
                    e.CellBounds,
                    fg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
                PaintCellBorder(e);
                e.Handled = true;
                return;
            }

            // ── Qty-diff column ─────────────────────────────────────────
            if (col == ColQtyDiff)
            {
                Color fg = row.QtyDiff > 0
                    ? Color.FromArgb(27, 94, 32)
                    : row.QtyDiff < 0
                        ? Color.FromArgb(183, 28, 28)
                        : Color.Gray;

                Color bg = row.QtyDiff != 0
                    ? (row.QtyDiff > 0
                        ? Color.FromArgb(232, 245, 233)
                        : Color.FromArgb(255, 235, 238))
                    : e.CellStyle.BackColor;

                PaintTextCell(e, row.QtyDiff.ToString("+0.##;-0.##;0"), fg, bg,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                return;
            }

            // ── Price-diff column ────────────────────────────────────────
            if (col == ColPriceDiff)
            {
                Color fg = row.PriceDiff > 0
                    ? Color.FromArgb(27, 94, 32)
                    : row.PriceDiff < 0
                        ? Color.FromArgb(183, 28, 28)
                        : Color.Gray;

                Color bg = row.PriceDiff != 0
                    ? (row.PriceDiff > 0
                        ? Color.FromArgb(232, 245, 233)
                        : Color.FromArgb(255, 235, 238))
                    : e.CellStyle.BackColor;

                PaintTextCell(e, row.PriceDiff.ToString("+0.00;-0.00;0.00"), fg, bg,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                return;
            }

            // ── New-location column – badge style ────────────────────────
            if (col == ColNewLoc && !string.IsNullOrWhiteSpace(row.NewLocation))
            {
                Color bg = row.NewLocation != row.CurrentLocation
                    ? Color.FromArgb(227, 242, 253)    // changed → light blue
                    : Color.FromArgb(232, 240, 254);   // same    → lighter blue

                Color fg = Color.FromArgb(21, 101, 192);
                PaintTextCell(e, row.NewLocation ?? string.Empty, fg, bg,
                    TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.EndEllipsis);
                return;
            }

            // ── Zero-stock row tint on qty columns ───────────────────────
            if ((col == ColCurrentQty || col == ColNewQty) && row.CurrentQty <= 0)
            {
                Color bg = Color.FromArgb(255, 235, 238);
                Color fg = e.State.HasFlag(DataGridViewElementStates.Selected)
                    ? Color.White : Color.FromArgb(183, 28, 28);
                string text = col == ColCurrentQty
                    ? row.CurrentQty.ToString("N2")
                    : row.NewQty.ToString("N2");

                PaintTextCell(e, text, fg, bg,
                    TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
                return;
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Painting helpers
        // ─────────────────────────────────────────────────────────────
        private static void PaintTextCell(DataGridViewCellPaintingEventArgs e,
            string text, Color fg, Color bg, TextFormatFlags flags)
        {
            bool selected = e.State.HasFlag(DataGridViewElementStates.Selected);
            Color actualBg = selected ? SystemColors.Highlight : bg;
            Color actualFg = selected ? SystemColors.HighlightText : fg;

            e.Graphics.FillRectangle(new SolidBrush(actualBg), e.CellBounds);

            Rectangle textRect = Rectangle.Inflate(e.CellBounds, -4, 0);
            TextRenderer.DrawText(e.Graphics, text,
                e.CellStyle.Font ?? SystemFonts.DefaultFont,
                textRect, actualFg, flags);

            PaintCellBorder(e);
            e.Handled = true;
        }

        private static void PaintCellBorder(DataGridViewCellPaintingEventArgs e)
        {
            // Thin bottom and right lines to match default grid appearance
            using (Pen p = new Pen(Color.FromArgb(220, 220, 220)))
            {
                e.Graphics.DrawLine(p,
                    e.CellBounds.Left, e.CellBounds.Bottom - 1,
                    e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                e.Graphics.DrawLine(p,
                    e.CellBounds.Right - 1, e.CellBounds.Top,
                    e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
            }
        }

        // ─────────────────────────────────────────────────────────────
        // Bulk row-count update (used by RefreshAdjustmentGrid)
        // SuspendLayout + RowCount assignment is the correct pattern
        // for updating VirtualMode grids without per-row Rows.Add().
        // ─────────────────────────────────────────────────────────────
        private void SetGridRowCount(int count)
        {
            _gridAdjustment.SuspendLayout();
            try
            {
                _gridAdjustment.RowCount = count;
            }
            finally
            {
                _gridAdjustment.ResumeLayout(false);
            }

            UpdateRenderBuffer();
            _gridAdjustment.Invalidate();
        }

        // ─────────────────────────────────────────────────────────────
        // Selection helpers (used by existing toolbar buttons)
        // ─────────────────────────────────────────────────────────────

        /// <summary>
        /// Returns the row indexes of all selected rows, sorted descending
        /// (safe for removal loops).
        /// </summary>
        public List<int> GetSelectedRowIndexes()
        {
            var list = new List<int>(_gridAdjustment.SelectedRows.Count);
            foreach (DataGridViewRow r in _gridAdjustment.SelectedRows)
            {
                if (r.Index >= 0 && r.Index < _sessionRows.Count)
                    list.Add(r.Index);
            }
            list.Sort((a, b) => b.CompareTo(a)); // descending
            return list;
        }

        // ─────────────────────────────────────────────────────────────
        // Undo toolbar button wiring helper
        // Call this from the form's Load or constructor after the
        // toolstrip button already exists (btnUndoLast is from designer).
        // ─────────────────────────────────────────────────────────────
        private void WireUndoButton()
        {
            if (btnUndoLast != null)
                btnUndoLast.Click += (s, e) => UndoLastEdit();
        }
    }
}
