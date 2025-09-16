using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using POS.BLL;

namespace pos.Controls
{
    public class CustomerSelectedEventArgs : EventArgs
    {
        public int CustomerId { get; }
        public string Name { get; }
        public string Phone { get; }
        public string VatNo { get; }
        public CustomerSelectedEventArgs(int id, string name, string phone, string vat)
        {
            CustomerId = id; Name = name; Phone = phone; VatNo = vat;
        }
    }

    // Borderless popup with a DataGridView anchored to a TextBox (more reliable than ToolStripDropDown hosting)
    public sealed class CustomerGridDropDown : IDisposable
    {
        // Non-activating popup so typing can continue in the textbox without closing the popup
        private class NonActivatingPopup : Form
        {
            protected override bool ShowWithoutActivation => true;
            protected override CreateParams CreateParams
            {
                get
                {
                    var cp = base.CreateParams;
                    cp.ExStyle |= 0x08000000; // WS_EX_NOACTIVATE
                    cp.ExStyle |= 0x00000008; // WS_EX_TOPMOST
                    return cp;
                }
            }
        }

        private readonly Form _popup;
        private readonly DataGridView _grid;
        private readonly BindingSource _bindingSource;
        private readonly Timer _debounceTimer;

        private TextBox _anchorTextBox;
        private DataTable _allCustomers;
        private bool _navigatingWithKeyboard;
        private bool _mouseOverPopup;

        public event EventHandler<Customer1SelectedEventArgs> CustomerSelected;
        public int MaxRows { get; set; } = 50;
        public int DropDownWidth { get; set; } = 520;
        public int DropDownHeight { get; set; } = 240;

        public CustomerGridDropDown()
        {
            _bindingSource = new BindingSource();

            _grid.BorderStyle = BorderStyle.None;
            _grid.BackgroundColor = Color.White;
            _grid.AutoGenerateColumns = true;
            _grid.ReadOnly = true;
            _grid.AllowUserToAddRows = false;
            _grid.AllowUserToDeleteRows = false;
            _grid.AllowUserToResizeRows = false;
            _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            _grid.MultiSelect = false;
            _grid.RowHeadersVisible = false;
            _grid.Size = new Size(520, 240);
            //Dock = DockStyle.Fill

            _grid.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            _grid.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            _grid.Columns[0].Name = "ID";
            _grid.Columns[1].Name = "Name";
            _grid.Columns[2].Name = "Contact";
            _grid.Columns[3].Name = "VAT No";
            _grid.Columns[0].ReadOnly = true;
            _grid.Columns[1].ReadOnly = true;
            _grid.Columns[2].ReadOnly = true;
            _grid.Columns[3].ReadOnly = true;

            _grid.Columns[0].Width = 40;
            _grid.Columns[1].Width = 220;
            _grid.Columns[2].Width = 130;
            _grid.Columns[3].Width = 120;

            _grid.DataSource = _bindingSource;

            _grid.BringToFront();

            _grid.CellDoubleClick += (s, e) => TrySelectCurrent();
            _grid.KeyDown += Grid_KeyDown;

            _popup = new NonActivatingPopup
            {
                FormBorderStyle = FormBorderStyle.None,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.Manual,
                TopMost = true,
                BackColor = Color.White
            };
            _popup.Paint += (s, e) =>
            {
                using (var pen = new Pen(Color.Silver))
                {
                    e.Graphics.DrawRectangle(pen, new Rectangle(0, 0, _popup.Width - 1, _popup.Height - 1));
                }
            };
            _popup.Controls.Add(_grid);
            _popup.BringToFront();

            _debounceTimer = new Timer { Interval = 200 };
            _debounceTimer.Tick += (s, e) =>
            {
                _debounceTimer.Stop();
                RefreshData(_anchorTextBox?.Text ?? string.Empty);
            };

            _popup.MouseEnter += (s, e) => _mouseOverPopup = true;
            _popup.MouseLeave += (s, e) => _mouseOverPopup = false;
        }

        public void Attach(TextBox textBox)
        {
            if (_anchorTextBox != null)
                Detach();

            _anchorTextBox = textBox;
            _anchorTextBox.TextChanged += TextBox_TextChanged;
            _anchorTextBox.KeyDown += TextBox_KeyDown;
            _anchorTextBox.LostFocus += TextBox_LostFocus;
            _anchorTextBox.Enter += TextBox_EnterOrClick;
            _anchorTextBox.Click += TextBox_EnterOrClick;

            EnsureAllCustomers();
        }

        public void Detach()
        {
            if (_anchorTextBox == null) return;
            _anchorTextBox.TextChanged -= TextBox_TextChanged;
            _anchorTextBox.KeyDown -= TextBox_KeyDown;
            _anchorTextBox.LostFocus -= TextBox_LostFocus;
            _anchorTextBox.Enter -= TextBox_EnterOrClick;
            _anchorTextBox.Click -= TextBox_EnterOrClick;
            _anchorTextBox = null;
            HidePopup();
        }

        private void EnsureAllCustomers()
        {
            if (_allCustomers != null) return;
            var bll = new CustomerBLL();
            _allCustomers = bll.GetAll() ?? new DataTable();

            if (!_allCustomers.Columns.Contains("DisplayName"))
                _allCustomers.Columns.Add("DisplayName", typeof(string));
            if (!_allCustomers.Columns.Contains("first_name"))
                _allCustomers.Columns.Add("first_name", typeof(string));
            if (!_allCustomers.Columns.Contains("last_name"))
                _allCustomers.Columns.Add("last_name", typeof(string));
            if (!_allCustomers.Columns.Contains("contact_no"))
                _allCustomers.Columns.Add("contact_no", typeof(string));
            if (!_allCustomers.Columns.Contains("vat_no"))
                _allCustomers.Columns.Add("vat_no", typeof(string));
            if (!_allCustomers.Columns.Contains("id"))
                _allCustomers.Columns.Add("id", typeof(int));

            foreach (DataRow r in _allCustomers.Rows)
            {
                string first = _allCustomers.Columns.Contains("first_name") ? Convert.ToString(r["first_name"]) : string.Empty;
                string last = _allCustomers.Columns.Contains("last_name") ? Convert.ToString(r["last_name"]) : string.Empty;
                if (string.IsNullOrWhiteSpace(first))
                {
                    if (_allCustomers.Columns.Contains("name")) first = Convert.ToString(r["name"]);
                    if (_allCustomers.Columns.Contains("registrationName")) first = Convert.ToString(r["registrationName"]);
                }
                r["DisplayName"] = (first + " " + last).Trim();
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            var text = _anchorTextBox?.Text ?? string.Empty;
            if (string.IsNullOrWhiteSpace(text))
            {
                HidePopup();
                return;
            }

            // Refresh immediately so results appear as soon as user types again
            RefreshData(text);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                _navigatingWithKeyboard = true;
                if (!_popup.Visible) ShowPopup();
                if (_grid.Rows.Count > 0)
                {
                    _grid.Focus();
                    _grid.CurrentCell = _grid.Rows[0].Cells[0];
                }
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape && _popup.Visible)
            {
                HidePopup();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Enter && _popup.Visible)
            {
                if (_grid.Rows.Count > 0)
                {
                    _grid.CurrentCell = _grid.Rows[0].Cells[0];
                    TrySelectCurrent();
                    HidePopup();
                }
                e.Handled = true;
            }
        }

        private void TextBox_LostFocus(object sender, EventArgs e)
        {
            // If textbox is empty and loses focus, always hide
            if (_anchorTextBox == null || string.IsNullOrWhiteSpace(_anchorTextBox.Text))
            {
                HidePopup();
                return;
            }

            // Otherwise, hide only if the mouse is not over the popup; allows clicking inside the popup
            if (!_mouseOverPopup)
            {
                //HidePopup();
            }
        }

        private void TextBox_EnterOrClick(object sender, EventArgs e)
        {
            var text = _anchorTextBox?.Text ?? string.Empty;
            if (!string.IsNullOrWhiteSpace(text))
            {
                RefreshData(text); // bind latest
                if (!_popup.Visible) ShowPopup();
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                TrySelectCurrent();
                e.Handled = true;
            }
            else if (e.KeyCode == Keys.Escape)
            {
                HidePopup();
                _anchorTextBox?.Focus();
                _navigatingWithKeyboard = false;
                e.Handled = true;
            }
        }

        private void ShowPopup()
        {
            if (_anchorTextBox == null) return;
            var screenPt = _anchorTextBox.Parent.PointToScreen(new Point(_anchorTextBox.Left, _anchorTextBox.Bottom));
            _popup.Size = new Size(DropDownWidth, DropDownHeight);
            _popup.Location = screenPt;

            if (!_popup.Visible)
            {
                _popup.Show();
                _popup.BringToFront();
            }
            else
            {
                _popup.BringToFront();
            }
        }

        private void HidePopup()
        {
            if (_popup.Visible)
            {
                _popup.Hide();
            }
            _mouseOverPopup = false; // reset hover state so future LostFocus behaves correctly
        }

        private void RefreshData(string term)
        {
            EnsureAllCustomers();
            DataView dv = _allCustomers.DefaultView;
            string safe = (term ?? string.Empty).Replace("'", "''");
            string[] cols = _allCustomers.Columns.Cast<DataColumn>()
                .Select(c => c.ColumnName)
                .Where(n => new[] { "DisplayName", "first_name", "last_name", "contact_no", "vat_no" }.Contains(n))
                .ToArray();
            string filter = string.Join(" OR ", cols.Select(c => $"Convert([{c}], 'System.String') LIKE '%{safe}%'"));
            dv.RowFilter = filter;

            DataTable dt = dv.Count > 0
                ? dv.ToTable().AsEnumerable().Take(MaxRows).CopyToDataTable()
                : _allCustomers.Clone();

            _bindingSource.DataSource = dt;
            _grid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            _grid.Invalidate();
            _grid.Refresh();

            if (dt.Rows.Count > 0)
            {
                if (!_popup.Visible) ShowPopup();
            }
            else
            {
                HidePopup();
            }
        }

        private void TrySelectCurrent()
        {
            if (_grid.CurrentRow == null && _grid.Rows.Count > 0)
            {
                _grid.CurrentCell = _grid.Rows[0].Cells[0];
            }
            if (_grid.CurrentRow == null) return;
            var row = _grid.CurrentRow;

            int id = 0;
            // Try to read id if present
            if (_grid.Columns.Contains("id"))
            {
                int.TryParse(Convert.ToString(row.Cells["id"].Value), out id);
            }

            string name = null;
            if (_grid.Columns.Contains("DisplayName"))
                name = Convert.ToString(row.Cells["DisplayName"].Value);
            if (string.IsNullOrWhiteSpace(name) && _grid.Columns.Contains("first_name"))
            {
                var first = Convert.ToString(row.Cells["first_name"].Value);
                var last = _grid.Columns.Contains("last_name") ? Convert.ToString(row.Cells["last_name"].Value) : string.Empty;
                name = (first + " " + last).Trim();
            }

            string phone = _grid.Columns.Contains("contact_no") ? Convert.ToString(row.Cells["contact_no"].Value) : string.Empty;
            string vat = _grid.Columns.Contains("vat_no") ? Convert.ToString(row.Cells["vat_no"].Value) : string.Empty;

            // Update anchor text silently
            _anchorTextBox.TextChanged -= TextBox_TextChanged;
            _anchorTextBox.Text = name ?? string.Empty;
            _anchorTextBox.SelectionStart = _anchorTextBox.TextLength;
            _anchorTextBox.TextChanged += TextBox_TextChanged;

            CustomerSelected?.Invoke(this, new Customer1SelectedEventArgs(id, name ?? string.Empty, phone, vat));
            HidePopup();
            _navigatingWithKeyboard = false;
            _anchorTextBox?.Focus();
        }

        public void Dispose()
        {
            Detach();
            _debounceTimer?.Dispose();
            _grid?.Dispose();
            _bindingSource?.Dispose();
            _allCustomers?.Dispose();
            _popup?.Dispose();
        }
    }
}
