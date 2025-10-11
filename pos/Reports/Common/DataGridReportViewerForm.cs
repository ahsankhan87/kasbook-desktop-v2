using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace pos.Reports.Common
{
    // Temporary lightweight viewer to visualize DataTables until Crystal is wired.
    public class DataGridReportViewerForm : Form
    {
        public DataGridReportViewerForm(string title, IDictionary<string, DataTable> tables)
        {
            Text = string.IsNullOrWhiteSpace(title) ? "Report" : title;
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;

            if (tables == null || tables.Count == 0)
            {
                Controls.Add(new Label
                {
                    Text = "No data",
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleCenter
                });
                return;
            }

            if (tables.Count == 1)
            {
                var grid = CreateGrid(tables.First().Value);
                grid.Dock = DockStyle.Fill;
                Controls.Add(grid);
            }
            else
            {
                var tabs = new TabControl { Dock = DockStyle.Fill };
                foreach (var kv in tables)
                {
                    var page = new TabPage(string.IsNullOrWhiteSpace(kv.Key) ? "Table" : kv.Key);
                    var grid = CreateGrid(kv.Value);
                    grid.Dock = DockStyle.Fill;
                    page.Controls.Add(grid);
                    tabs.TabPages.Add(page);
                }
                Controls.Add(tabs);
            }
        }

        private DataGridView CreateGrid(DataTable dt)
        {
            var grid = new DataGridView
            {
                AutoGenerateColumns = true,
                DataSource = dt,
                ReadOnly = true,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
            };
            return grid;
        }
    }
}
