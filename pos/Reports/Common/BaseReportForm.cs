using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Common
{
    public class BaseReportForm : Form
    {
        protected readonly DateTimePicker FromPicker = new DateTimePicker { Format = DateTimePickerFormat.Short };
        protected readonly DateTimePicker ToPicker = new DateTimePicker { Format = DateTimePickerFormat.Short };
        protected readonly ComboBox BranchCombo = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList }; // optional
        protected readonly Button BtnLoad = new Button { Text = "Load" };
        protected readonly Button BtnExportCsv = new Button { Text = "Export CSV" };
        protected readonly Button BtnPrint = new Button { Text = "Print" };
        protected readonly DataGridView Grid = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells
        };

        protected FlowLayoutPanel Filters; // exposed to derived classes

        protected bool IsInDesignMode
        {
            get
            {
                return (System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime) || this.DesignMode;
            }
        }

        public BaseReportForm()
        {
            WindowState = FormWindowState.Maximized;
            StartPosition = FormStartPosition.CenterParent;
            DoubleBuffered = true;

            var top = new Panel { Dock = DockStyle.Top, Height = 44, Padding = new Padding(8), BackColor = SystemColors.Control };

            var lblFrom = new Label { Text = "From:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft };
            var lblTo = new Label { Text = "To:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft };
            var lblBranch = new Label { Text = "Branch:", AutoSize = true, TextAlign = ContentAlignment.MiddleLeft };

            Filters = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false, AutoSize = false, FlowDirection = FlowDirection.LeftToRight };
            Filters.Controls.Add(lblFrom); Filters.Controls.Add(FromPicker);
            Filters.Controls.Add(lblTo); Filters.Controls.Add(ToPicker);
            Filters.Controls.Add(lblBranch); Filters.Controls.Add(BranchCombo);
            Filters.Controls.Add(BtnLoad); Filters.Controls.Add(BtnExportCsv); Filters.Controls.Add(BtnPrint);

            top.Controls.Add(Filters);
            Controls.Add(Grid);
            Controls.Add(top);

            FromPicker.Value = DateTime.Today.AddDays(-7);
            ToPicker.Value = DateTime.Today;

            BtnLoad.Click += (s, e) => LoadAndBind();
            BtnExportCsv.Click += (s, e) => ExportCsv();
            BtnPrint.Click += (s, e) => PrintReport();
        }

        protected virtual DataTable GetData(DateTime from, DateTime to, int? branchId)
        {
            // Default implementation to keep designer happy; derived classes should override.
            return new DataTable();
        }

        protected virtual void LoadAndBind()
        {
            try
            {
                int? branchId = null;
                var selected = BranchCombo.SelectedItem as BranchItem;
                if (selected != null) branchId = selected.Id;
                var dt = GetData(FromPicker.Value.Date, ToPicker.Value.Date, branchId);
                if (dt == null) dt = new DataTable();
                Grid.DataSource = dt;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void ExportCsv()
        {
            try
            {
                var dt = Grid.DataSource as DataTable;
                if (dt == null || dt.Rows.Count == 0)
                {
                    MessageBox.Show("No data to export.");
                    return;
                }
                using (var sfd = new SaveFileDialog { Filter = "CSV files (*.csv)|*.csv", FileName = Text.Replace(' ', '_') + ".csv" })
                {
                    if (sfd.ShowDialog(this) != DialogResult.OK) return;
                    using (var sw = new System.IO.StreamWriter(sfd.FileName))
                    {
                        // header
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            sw.Write(dt.Columns[i].ColumnName);
                            if (i < dt.Columns.Count - 1) sw.Write(",");
                        }
                        sw.WriteLine();
                        // rows
                        foreach (DataRow row in dt.Rows)
                        {
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                var val = row[i] == null ? string.Empty : row[i].ToString();
                                if (val == null) val = string.Empty;
                                val = val.Replace("\"", "\"\"");
                                if (val.Contains(",") || val.Contains("\"")) val = "\"" + val + "\"";
                                sw.Write(val);
                                if (i < dt.Columns.Count - 1) sw.Write(",");
                            }
                            sw.WriteLine();
                        }
                    }
                    MessageBox.Show("Exported successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        protected virtual void PrintReport()
        {
            MessageBox.Show("Printing will be wired later.");
        }

        protected class BranchItem
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public override string ToString() => Name;
        }
    }
}
