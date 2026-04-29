using POS.BLL;
using pos.UI;
using pos.Reports.Common;
using pos.UI.Busy;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace pos.Reports.Taxes
{
    public partial class frm_VatInvoiceDetails : Form
    {
        private readonly VatDashboardBLL _bll = new VatDashboardBLL();
        private readonly DateTime _from;
        private readonly DateTime _to;
        private readonly string _term;

        public frm_VatInvoiceDetails(DateTime from, DateTime to, string term)
        {
            _from = from.Date;
            _to = to.Date;
            _term = term ?? string.Empty;

            InitializeComponent();
        }

        private void frm_VatInvoiceDetails_Load(object sender, EventArgs e)
        {
            AppTheme.Apply(this);
            lblTitle.Text = string.Format("{0}   ({1:d} - {2:d})", _term, _from, _to);
            LoadData();
        }

        private void LoadData()
        {
            using (BusyScope.Show(this, UiMessages.T("Loading invoice details...", "جاري تحميل تفاصيل الفواتير...")))
            {
                var dt = _bll.GetInvoiceDetails(_from, _to, _term);
                AppendGrandTotal(dt);
                gridDetails.DataSource = dt;
                ApplyGridFormatting();
                HighlightGrandTotalRow();
            }
        }

        private static void AppendGrandTotal(DataTable dt)
        {
            if (dt == null || dt.Rows.Count == 0) return;
            if (!dt.Columns.Contains("NetAmount") || !dt.Columns.Contains("VatAmount") || !dt.Columns.Contains("TotalAmount")) return;

            for (int i = dt.Rows.Count - 1; i >= 0; i--)
            {
                if (string.Equals(Convert.ToString(dt.Rows[i]["InvoiceNo"]), "Grand Total", StringComparison.OrdinalIgnoreCase))
                    dt.Rows.RemoveAt(i);
            }

            decimal net = 0m;
            decimal vat = 0m;
            decimal total = 0m;

            foreach (DataRow row in dt.Rows)
            {
                if (row["NetAmount"] != DBNull.Value) net += Convert.ToDecimal(row["NetAmount"]);
                if (row["VatAmount"] != DBNull.Value) vat += Convert.ToDecimal(row["VatAmount"]);
                if (row["TotalAmount"] != DBNull.Value) total += Convert.ToDecimal(row["TotalAmount"]);
            }

            var totalRow = dt.NewRow();
            totalRow["InvoiceNo"] = "Grand Total";
            totalRow["NetAmount"] = net;
            totalRow["VatAmount"] = vat;
            totalRow["TotalAmount"] = total;
            dt.Rows.Add(totalRow);
        }

        private void ApplyGridFormatting()
        {
            foreach (DataGridViewColumn col in gridDetails.Columns)
            {
                if (string.Equals(col.Name, "NetAmount", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(col.Name, "VatAmount", StringComparison.OrdinalIgnoreCase)
                    || string.Equals(col.Name, "TotalAmount", StringComparison.OrdinalIgnoreCase))
                {
                    col.DefaultCellStyle.Format = "#,##0.00;-#,##0.00;0.00";
                    col.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
                }
            }
        }

        private void HighlightGrandTotalRow()
        {
            if (gridDetails.Rows.Count == 0 || !gridDetails.Columns.Contains("InvoiceNo")) return;

            foreach (DataGridViewRow row in gridDetails.Rows)
            {
                if (string.Equals(Convert.ToString(row.Cells["InvoiceNo"].Value), "Grand Total", StringComparison.OrdinalIgnoreCase))
                {
                    row.DefaultCellStyle.BackColor = Color.FromArgb(255, 240, 240);
                    row.DefaultCellStyle.ForeColor = Color.DarkRed;
                    row.DefaultCellStyle.Font = new Font(gridDetails.Font, FontStyle.Bold);
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var dt = gridDetails.DataSource as DataTable;
            ExcelExportHelper.ExportDataTableToExcel(dt, "vat_invoice_details_"+lblTitle.Text.Replace(":", "-").Replace("/", "-"), this, includeLastRow: true);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
